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
        public IActionResult Fing(string query)
        {
            List<KeyWord> keyWords = _publicationService.FindKeyWords(query);
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


        

        
    }
}
