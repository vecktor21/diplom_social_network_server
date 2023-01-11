﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;
using server.ViewModels;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyWordController : Controller
    {
        private ApplicationContext db;
        private AccountService _accountService;
        private PublicationService _publicationService;
        public KeyWordController(ApplicationContext db, AccountService accountService, PublicationService publicationService)
        {
            this.db = db;
            this._accountService = accountService;
            this._publicationService = publicationService;
        }

        //получение всех ключевых слов
        [HttpGet]
        public IActionResult GetKeyWords()
        {
            return Json(db.KeyWords
                .Select(x =>new KeyWordViewModel(x))
                .ToList());
        }

        //создание ключевого слова
        [HttpPost]
        public async Task<IActionResult> AddKeyWord(KeyWordCreateViewModel newKeyWord)
        {
            List<KeyWord> foundKeyWordsRu = _publicationService.FindKeyWords(newKeyWord.KeyWordRu);
            List<KeyWord> foundKeyWordsEn = _publicationService.FindKeyWords(newKeyWord.KeyWordEn);
            if(foundKeyWordsEn.Count + foundKeyWordsRu.Count > 0)
            {
                return BadRequest("такое ключевое слово уже существует");
            }
            db.KeyWords.Add(new KeyWord { KeyWordEn = newKeyWord.KeyWordEn, KeyWordRu = newKeyWord.KeyWordRu });
            await db.SaveChangesAsync();
            return Ok();
        }

        //поиск ключевых слов
        [HttpGet("[action]")]
        public IActionResult Find(string query)
        {
            List<KeyWordViewModel> keyWords = _publicationService.FindKeyWords(query)
                .Select(x => new KeyWordViewModel(x))
                .ToList();
            return Json(keyWords);
        }


        //удаление ключевого слова
        [HttpDelete("[action]/{keywordId}")]
        [Authorize]
        public async Task<IActionResult> DeleteKeyword(int keywordId)
        {
            //проверка прав доступа
            if (!_accountService.IsCurrentUserAdmin())
            {
                return Unauthorized();
            }
            //проверка ключевого слова
            KeyWord keyWord = db.KeyWords.FirstOrDefault(x => x.KeyWordId == keywordId);
            if(keyWord == null) {
                return NotFound();
            }
            db.KeyWords.Remove(keyWord);
            await db.SaveChangesAsync();
            return Ok();
        }


        //получить ключевые слова (интересы) пользователя
        [HttpGet("[action]")]
        public IActionResult GetUserInterests(int userId)
        {
            List<KeyWordViewModel> userKeyWords = db.UserInterests
                .Include(x => x.KeyWord)
                .Where(x => x.UserId == userId)
                .Select(x => new KeyWordViewModel(x.KeyWord))
                .ToList();
            return Json(userKeyWords);
        }

        //добавить ключевое слово в интересы пользователя
        [HttpPost("[action]")]
        public async Task<IActionResult> AddUserInterest(int userId, int keyWordId)
        {
            User user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            KeyWord keyWord= db.KeyWords.FirstOrDefault(x => x.KeyWordId == keyWordId);
            if (keyWord == null)
            {
                return NotFound("ключевое слово не найдено");
            }
            UserInterest userInterest = db.UserInterests.FirstOrDefault(x => x.UserId == userId && x.KeyWordId == keyWordId);
            if(userInterest != null) {
                return BadRequest("это ключевое слово уже добавлено");
            }
            try
            {
                db.UserInterests.Add(new UserInterest
                {
                    User = user,
                    KeyWord = keyWord
                });
                await db.SaveChangesAsync();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest();
            }
        }

        //удалить ключевое слово из интересов пользователя
        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveUserInterest(int userId, int keyWordId)
        {
            User user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            UserInterest userInterest = db.UserInterests.FirstOrDefault(x => x.UserId == userId && x.KeyWordId == keyWordId);
            if (userInterest == null)
            {
                return BadRequest("ключевое слово не найдено");
            }
            try
            {
                db.UserInterests.Remove(userInterest);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
