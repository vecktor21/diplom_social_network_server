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
    public class ArticlePageController : Controller
    {
        private ApplicationContext db;
        private AccountService _accountService;
        private PublicationService _publicationService;
        public ArticlePageController(ApplicationContext db, AccountService accountService, PublicationService publicationService)
        {
            this.db = db;
            this._accountService = accountService;
            this._publicationService = publicationService;
        }
        //получение страницы 
        [HttpGet("{pageId}")]
        public IActionResult GetArticlePage(int pageId)
        {
            //добавить include для нужных полей
            ArticlePage articlePage = db.ArticlePages
                .Include(x=>x.Article.ArticlePages)
                .Include(x=>x.ArticlePageLikes)
                .ThenInclude(x=>x.Like.LikedUser)
                .Include(x=>x.ArticlePageComments)
                .ThenInclude(x=>x.Comment.User.Image)
                .Include(x=>x.ArticlePageComments)
                .ThenInclude(x=>x.Comment.CommentAttachments)
                .ThenInclude(x=>x.File)
                .Include(x=>x.ArticlePageComments)
                .ThenInclude(x=>x.Comment.CommentLikes)
                .ThenInclude(x=>x.Like.LikedUser)
                .FirstOrDefault(x=>x.ArticlePageId==pageId);
            if (articlePage == null)
            {
                return NotFound("страница не найдена");
            }
            ArticlePageViewModel articlePageViewModel = new ArticlePageViewModel(articlePage);
            foreach (var comment in articlePageViewModel.Comments)
            {
                comment.Replies = _publicationService.FindCommentReplies(comment);
            }

            return Json(articlePageViewModel);
        }

        //получение страницы для изменения 
        [HttpGet("[action]/{pageId}")]
        public IActionResult GetArticlePageForUpdate(int pageId)
        {
            //добавить include для нужных полей
            ArticlePage articlePage = db.ArticlePages
                .FirstOrDefault(x => x.ArticlePageId == pageId);
            if (articlePage == null)
            {
                return NotFound("страница не найдена");
            }
            ArticlePageUpdateViewModel articlePageUpdateViewModel = new ArticlePageUpdateViewModel
            {
                ArticlePageId = articlePage.ArticleId,
                Text = articlePage.Text
            };

            return Json(articlePageUpdateViewModel);
        }

        //создать страницу
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateArticlePage(ArticlePageCreateViewModel newArticlePage)
        {
            //TODO
            //добавить дополнительную проверку на автора статьи

            ArticlePage articlePage = new ArticlePage
            {
                Text = newArticlePage.Text,
                ArticleId = newArticlePage.ArticleId
            };
            db.ArticlePages.Add(articlePage);
            await db.SaveChangesAsync();
            return Ok();
        }

        //удалить страницу
        [HttpDelete("{articlePageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticlePage(int articlePageId)
        {
            ArticlePage articlePage = db.ArticlePages
                .Include(x=>x.Article)
                .FirstOrDefault(x => x.ArticlePageId == articlePageId);
            if(!(_accountService.IsCurrentUserAdmin() && _publicationService.IsCurrentUserArticleAuthor(articlePage.Article)))
            {
                return Forbid();
            }
            try
            {
                db.ArticlePages.Remove(articlePage);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        //изменить страницу
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateArticlePage(ArticlePageUpdateViewModel newArticlePage)
        {
            ArticlePage articlePage = db.ArticlePages
                .Include(x => x.Article)
                .FirstOrDefault(x => x.ArticlePageId == newArticlePage.ArticlePageId);
            if (!(_accountService.IsCurrentUserAdmin() && _publicationService.IsCurrentUserArticleAuthor(articlePage.Article)))
            {
                return Forbid();
            }
            try
            {
                articlePage.Text = newArticlePage.Text;
                db.ArticlePages.Update(articlePage);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
