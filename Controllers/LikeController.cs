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
    }
}
