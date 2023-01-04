using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.ViewModels;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private ApplicationContext db { get; set; }
        public LikeController(ApplicationContext db)
        {
            this.db = db;
        }

        //добавить лайк\убрать лайк с поста
        [HttpPost("post")]
        public async Task<IActionResult> LikePost(PostLikeViewModel like)
        {
            //проверка на существование пользователя и поста
            Post post = db.Posts.FirstOrDefault(x =>x.PostId==like.PostId);
            User user = db.Users.FirstOrDefault(x => x.UserId == like.UserId);
            if (post == null)
            {
                return NotFound("пост не найден");
            }
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            //если пост не лайкнут - лайкаем
            if (!like.IsLiked)
            {
                //дополнительная проверка на то, лайкнут пост или нет:   
                if(db.PostLikes.Include(x => x.Like).Any(x=>(x.Like.LikedUserId == like.UserId)&&(x.PostId == like.PostId)))
                {
                    return BadRequest("этот пост уже был лайкнут");
                }
                db.PostLikes.Add(new PostLike
                {
                    Post = post,
                    Like = new Like
                    {
                        LikedUser = user
                    }
                });
            }
            //если лайкнут - убираем лайк
            else
            {
                PostLike postLike = db.PostLikes.Include(x=>x.Like).FirstOrDefault(x => x.PostId == like.PostId && x.Like.LikedUserId == like.UserId);
                if (postLike == null)
                {
                    return NotFound("лайк не найден");
                }
                db.PostLikes.Remove(postLike);
            }
            await db.SaveChangesAsync();
            return Ok();
        }




        //добавить лайк\убрать лайк с комментария
        [HttpPost("comment")]
        public async Task<IActionResult> LikeComment(CommentLikeViewModel like)
        {
            //проверка на существование пользователя и коммента
            Comment comment = db.Comments.FirstOrDefault(x => x.CommentId == like.CommentId);
            User user = db.Users.FirstOrDefault(x => x.UserId == like.UserId);
            if (comment == null)
            {
                return NotFound("комментарий не найден");
            }
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            //если комментарий не лайкнут - лайкаем
            if (!like.IsLiked)
            {
                //дополнительная проверка на то, лайкнут коммент или нет:   
                if (db.CommentLikes.Include(x => x.Like).Any(x => (x.Like.LikedUserId == like.UserId) && (x.CommentId == like.CommentId)))
                {
                    return BadRequest("этот комментарий уже был лайкнут");
                }
                db.CommentLikes.Add(new CommentLike
                {
                    Comment = comment,
                    Like = new Like
                    {
                        LikedUser = user
                    }
                });
            }
            //если лайкнут - убираем лайк
            else
            {
                CommentLike commentLike = db.CommentLikes.Include(x => x.Like).FirstOrDefault(x => x.CommentId == like.CommentId && x.Like.LikedUserId == like.UserId);
                if (commentLike == null)
                {
                    return NotFound("лайк не найден");
                }
                db.CommentLikes.Remove(commentLike);
            }
            await db.SaveChangesAsync();
            return Ok();
        }



        //добавить лайк\убрать лайк к статье
        [HttpPost("article")]
        public async Task<IActionResult> LikeArticle(ArticleLikeViewModel like)
        {
            //проверка на существование пользователя и статьи
            Article article = db.Articles.FirstOrDefault(x => x.ArticleId == like.ArticleId);
            User user = db.Users.FirstOrDefault(x => x.UserId == like.UserId);
            if (article == null)
            {
                return NotFound("статья не найдена");
            }
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            //если статья не лайкнута - лайкаем
            if (!like.IsLiked)
            {
                //дополнительная проверка на то, лайкнута статья или нет:   
                if (db.ArticleLikes.Include(x => x.Like).Any(x => (x.Like.LikedUserId == like.UserId) && (x.ArticleId == like.ArticleId)))
                {
                    return BadRequest("эта статья уже была лайкнута");
                }
                db.ArticleLikes.Add(new ArticleLike
                {
                    Article = article,
                    Like = new Like
                    {
                        LikedUser = user
                    }
                });
            }
            //если лайкнут - убираем лайк
            else
            {
                ArticleLike articleLike = db.ArticleLikes.Include(x => x.Like).FirstOrDefault(x => x.ArticleId == like.ArticleId && x.Like.LikedUserId == like.UserId);
                if (articleLike == null)
                {
                    return NotFound("лайк не найден");
                }
                db.ArticleLikes.Remove(articleLike);
            }
            await db.SaveChangesAsync();
            return Ok();
        }

        //добавить лайк\убрать лайк к странице статьи
        [HttpPost("articlePage")]
        public async Task<IActionResult> LikeArticlePage(ArticlePageLikeViewModel like)
        {
            //проверка на существование пользователя и статьи
            ArticlePage articlePage = db.ArticlePages.FirstOrDefault(x => x.ArticlePageId == like.ArticlePageId);
            User user = db.Users.FirstOrDefault(x => x.UserId == like.UserId);
            if (articlePage == null)
            {
                return NotFound("страница не найдена");
            }
            if (user == null)
            {
                return NotFound("пользователь не найден");
            }
            //если страница не лайкнута - лайкаем
            if (!like.IsLiked)
            {
                //дополнительная проверка на то, лайкнута статья или нет:   
                if (db.ArticlePageLikes.Include(x => x.Like).Any(x => (x.Like.LikedUserId == like.UserId) && (x.ArticlePageId == like.ArticlePageId)))
                {
                    return BadRequest("эта страница уже была лайкнута");
                }
                db.ArticlePageLikes.Add(new ArticlePageLike
                {
                    ArticlePage = articlePage,
                    Like = new Like
                    {
                        LikedUser = user
                    }
                });
            }
            //если лайкнут - убираем лайк
            else
            {
                ArticlePageLike articlePageLike = db.ArticlePageLikes.Include(x => x.Like).FirstOrDefault(x => x.ArticlePageId == like.ArticlePageId && x.Like.LikedUserId == like.UserId);
                if (articlePageLike == null)
                {
                    return NotFound("лайк не найден");
                }
                db.ArticlePageLikes.Remove(articlePageLike);
            }
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
