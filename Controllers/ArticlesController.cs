using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using server.Models;
using server.Services;
using server.ViewModels;
using System.Collections.Generic;
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
        private PublicationService _publicationService;
        private AccountService _accountService;
        public ArticlesController(ApplicationContext db, PublicationService publicationService, AccountService accountService) 
        {
            this.db = db;
            _publicationService = publicationService;
            _accountService = accountService;
        }

        //получение рекомендаций пользователя
        [HttpGet("[action]")]
        public IActionResult GetRecomendedArticles(int userId)
        {

            User user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            List<KeyWord> userInterests = db.UserInterests
                .Include(x=>x.KeyWord)
                .Where(x => x.UserId == userId)
                .Select(x=>x.KeyWord)
                .ToList();

            List<ArticleViewModel> articles = IncludeArticleData()
                .Where(x => x.ArticleKeyWords.Any(y=>userInterests.Contains(y.KeyWord)))
                .Select(x => new ArticleViewModel(x))
                .OrderByDescending(x=>x.PublicationDate)
                .Take(10)
                .ToList();

            return Json(articles);
        }

        //поиск статей
        [HttpGet("[action]")]
        public List <ArticleViewModel> SearchArticles(string query)
        {
            List<ArticleViewModel> searchResult = db.Articles
                .Include(x => x.Author.Role)
                .Include(x => x.Author.Image)
                .Include(x => x.ArticleLikes)
                .ThenInclude(x => x.Like.LikedUser)
                .Include(x => x.ArticleKeyWords)
                .ThenInclude(x => x.KeyWord)
                .Include(x => x.ArticleComments)
                .ThenInclude(x => x.Comment.User)
                .Include(x => x.ArticleComments)
                .ThenInclude(x => x.Comment.CommentLikes)
                .ThenInclude(x => x.Like)
                .Include(x => x.ArticleComments)
                .ThenInclude(x => x.Comment.CommentAttachments)
                .ThenInclude(x => x.File)
                .Include(x => x.ArticlePages)
                .Where(x =>
                    EF.Functions.Like(x.Title, $"%{query}%") ||
                    EF.Functions.Like(x.Introduction, $"%{query}%")
                )
                .Select(x => new ArticleViewModel(x))
                .ToList();

            List<KeyWord> keyWords = _publicationService.FindKeyWords(query);

            foreach (var keyWord in keyWords)
            {
                foreach (var articleKeyWord in keyWord.ArticleKeyWords)
                {
                    searchResult.AddRange(
                        IncludeArticleData()
                        .Where(x=>x.ArticleId==articleKeyWord.ArticleId)
                        .Select(x => new ArticleViewModel(x)));
                }
            }


            return searchResult.DistinctBy(x => x.ArticleId).ToList();
                
        }

        //получение всех статей автора
        [HttpGet("[action]")]
        public async Task<IActionResult> GetArticlesByAuthor(int authorId)
        {
            List<ArticleViewModel> articles = IncludeArticleData()
                .Where(x=>x.AuthorId==authorId)
                .Select(x => new ArticleViewModel(x))
                .ToList();
            return Json(articles);
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
                List<ArticleViewModel> articles = IncludeArticleData()
                    .Select(x => new ArticleViewModel(x))
                    .ToList();
                return Json(articles);
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        //получение одной статьи
        [HttpGet("{articleId}")]
        public IActionResult GetArticle(int articleId)
        {
            try
            {
                Article article = IncludeArticleData()
                    .FirstOrDefault(x => x.ArticleId == articleId);
                if(article == null)
                {
                    return NotFound("статья не найдена");
                }
                ArticleViewModel articleViewModel = new ArticleViewModel(article);
                foreach (var comment in articleViewModel.Comments)
                {
                    comment.Replies = _publicationService.FindCommentReplies(comment);
                }
                return Json(articleViewModel);
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }



        //получение одной статьи
        [HttpGet("[action]/{articleId}")]
        public IActionResult GetArticleForUpdate(int articleId)
        {
            try
            {
                Article article = IncludeArticleData()
                    .FirstOrDefault(x => x.ArticleId == articleId);
                if (article == null)
                {
                    return NotFound("статья не найдена");
                }
                ArticleUpdateViewModel articleUpdateViewModel = new ArticleUpdateViewModel
                {
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    Introduction = article.Introduction,
                    KeyWords = article.ArticleKeyWords.Select(x=>x.KeyWordId).ToList()
                };
                
                return Json(articleUpdateViewModel);
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }


        //удаление статьи
        [HttpDelete("[action]/{articleId}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            try
            {
                Article article = db.Articles.FirstOrDefault(x => x.ArticleId == articleId);
                if (!(_accountService.IsCurrentUserAdmin() && _publicationService.IsCurrentUserArticleAuthor(article)))
                {
                    return Forbid();
                }
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
        [Authorize]
        public async Task<IActionResult> UpdateArticle(ArticleUpdateViewModel updatedArticle)
        {
            Article article = db.Articles
                .Include(x=>x.ArticleKeyWords)
                .ThenInclude(x=> x.KeyWord)
                .FirstOrDefault(x => x.ArticleId == updatedArticle.ArticleId);
            if (!(_accountService.IsCurrentUserAdmin() && _publicationService.IsCurrentUserArticleAuthor(article)))
            {
                return Forbid();
            }
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
        
        //подгрузить данные статей
        private List<Article> IncludeArticleData()
        {
            return db.Articles
                    .Include(x => x.Author.Role)
                    .Include(x => x.Author.Image)
                    .Include(x => x.ArticleLikes)
                    .ThenInclude(x => x.Like.LikedUser)
                    .Include(x => x.ArticleKeyWords)
                    .ThenInclude(x => x.KeyWord)
                    .Include(x => x.ArticleComments)
                    .ThenInclude(x => x.Comment.User)
                    .Include(x => x.ArticleComments)
                    .ThenInclude(x => x.Comment.CommentLikes)
                    .ThenInclude(x => x.Like)
                    .Include(x => x.ArticleComments)
                    .ThenInclude(x => x.Comment.CommentAttachments)
                    .ThenInclude(x => x.File)
                    .Include(x => x.ArticlePages)
                    .ToList();
        }
    }
}
