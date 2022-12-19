﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using server.Models;
using server.ViewModels;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : Controller
    {
        private ApplicationContext db;
        public ArticlesController(ApplicationContext db) 
        {
            this.db = db;
        }


        //создание основы статьи
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateArticle(ArticleCreateViewModel newArticle)
        {
            //проверка на дубликат статьи
            Article articleTemp = db.Articles.FirstOrDefault(x=>x.Title==newArticle.Title);
            if (articleTemp != null) return BadRequest("Такая статья уже существует");
            
            //проверка существования автора
            User author = db.Users.FirstOrDefault(x => x.UserId == newArticle.AuthorId);
            if (author == null) return NotFound("Пользователь не найден");


            //создание статьи
            try
            {
                Article article = new Article
                {
                    Title = newArticle.Title,
                    Author = author,
                    Introduction = newArticle.Introduction ?? ""
                };
                db.Articles.Add(article);
                await db.SaveChangesAsync();

                //поиск ключевых слов
                List<ArticleKeyWord> words = FindKeyWordsByIds(newArticle.KeyWords)
                    .Select(x => new ArticleKeyWord
                    {
                        Article = article,
                        KeyWord = x
                    })
                    .ToList();
                db.ArticleKeyWords.AddRange(words);
                await db.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        //получение статей
        [HttpGet]
        public IActionResult GetArticles()
        {
            try
            {
                List<ArticleViewModel> articles = db.Articles
                    .Include(x => x.Author.Role)
                    .Include(x=>x.Author.Image)
                    .Include(x => x.ArticleLikes)
                    .ThenInclude(x => x.Like.LikedUser)
                    .Include(x => x.ArticleKeyWords)
                    .ThenInclude(x => x.KeyWord)
                    .Include(x => x.ArticleComments)
                    .ThenInclude(x => x.Comment.User)
                    .Include(x => x.ArticlePages)
                    .Select(x => new ArticleViewModel(x))
                    .ToList();
                return Json(articles);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }

        //удаление статьи
        [HttpDelete("[action]/{articleId}")]
        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            try
            {
                Article article = db.Articles.FirstOrDefault(x => x.ArticleId == articleId);
                if(article == null)
                {
                    return NotFound("статья не найдена");
                }
                db.Articles.Remove(article);
                await db.SaveChangesAsync();
                return Ok();
            }catch(Exception e)
            {
                return BadRequest(e);
            }
        }


        //изменение информации о статье
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateArticle(ArticleUpdateViewModel updatedArticle)
        {
            Article article = db.Articles
                .Include(x=>x.ArticleKeyWords)
                .ThenInclude(x=> x.KeyWord)
                .FirstOrDefault(x => x.ArticleId == updatedArticle.ArticleId);

            if (article == null) return NotFound("Сатья не найдена");

            //изменение данных статьи
            article.Title=updatedArticle.Title;
            article.Introduction=updatedArticle.Introduction;

            //проверка - совпадают ли новые ключевые слова со старыми
            bool isKeyWordsNotUpdated = article.ArticleKeyWords.All(x=>updatedArticle.KeyWords.Contains(x.KeyWordId)) && article.ArticleKeyWords.Count == updatedArticle.KeyWords.Count;
            if (!isKeyWordsNotUpdated)
            {
                //удаление старых ключевых слов
                db.ArticleKeyWords.RemoveRange(article.ArticleKeyWords);
                await db.SaveChangesAsync();

                //добавление новых ключевых слов
                List<ArticleKeyWord> words = FindKeyWordsByIds(updatedArticle.KeyWords)
                    .Select(x => new ArticleKeyWord
                    {
                        Article = article,
                        KeyWord = x
                    })
                        .ToList();
                db.ArticleKeyWords.AddRange(words);
            }
            await db.SaveChangesAsync();
            return Ok();
        }


        //найти ключевые слова по массиву айди
        private List<KeyWord> FindKeyWordsByIds(List<int> keyWordIds)
        {
            return db.KeyWords
                .Where(x => keyWordIds.Any(y => y == x.KeyWordId))
                .ToList();
        }
    }
}
