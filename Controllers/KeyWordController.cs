using Microsoft.AspNetCore.Authorization;
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
        private AccountService accountService;
        public KeyWordController(ApplicationContext db, AccountService accountService)
        {
            this.db = db;
            this.accountService = accountService;
        }

        //получение всех ключевых слов
        [HttpGet]
        public IActionResult GetKeyWords()
        {
            return Json(db.KeyWords
                .Select(x =>
                new KeyWordViewModel
                {
                    KeyWordEn = x.KeyWordEn,
                    KeyWordId = x.KeyWordId,
                    KeyWordRu = x.KeyWordRu
                }).ToList());
        }

        //создание ключевого слова
        [HttpPost]
        public async Task<IActionResult> AddKeyWord(KeyWordCreateViewModel newKeyWord)
        {
            List<KeyWord> foundKeyWordsRu = FindKeyWords(newKeyWord.KeyWordRu);
            List<KeyWord> foundKeyWordsEn = FindKeyWords(newKeyWord.KeyWordEn);
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
        public IActionResult Fing(string query)
        {
            List<KeyWord> keyWords = FindKeyWords(query);
            return Json(keyWords);
        }


        //удаление ключевого слова
        [HttpDelete("[action]/{keywordId}")]
        [Authorize]
        public async Task<IActionResult> DeleteKeyword(int keywordId)
        {
            //проверка прав доступа
            if (!accountService.IsCurrentUserAdmin())
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


        //метод для поиска ключевых слов
        private List<KeyWord> FindKeyWords(string query)
        {
            List<KeyWord> keyWords = db.KeyWords
                .Where(x=>
                    EF.Functions.Like(x.KeyWordRu, $"%{query}%") ||
                    EF.Functions.Like(x.KeyWordEn, $"%{query}%")||
                    EF.Functions.Like(x.KeyWordRu+x.KeyWordEn, $"%{query}%") ||
                    EF.Functions.Like(x.KeyWordEn + x.KeyWordRu, $"%{query}%")
                )
                .ToList();
            return keyWords;
        }

        
    }
}
