using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;
using server.ViewModels;
using System.ComponentModel.Design;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private IWebHostEnvironment env { get; set; }
        private ApplicationContext db { get; set; }
        private FileService fileService { get; set; }
        public CommentController(IWebHostEnvironment env, ApplicationContext db, FileService fileService)
        {
            this.db = db;
            this.env = env;
            this.fileService = fileService;
        }



        //создать комментарии поста
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePostComment(CommentCreateViewModel newComment)
        {
            try
            {
                //проверки
                Post post = db.Posts.FirstOrDefault(x => x.PostId == newComment.PostId);
                if (post == null)
                {
                    return NotFound("пост не найден");
                }
                User user = db.Users.FirstOrDefault(x => x.UserId == newComment.UserId);
                if (user == null)
                {
                    return NotFound("пользователь не найден");
                }


                //создание коммента
                Comment comment = new Comment
                {
                    Message = newComment.Message,
                    UserId = newComment.UserId,
                    IsReply = false
                };

                //сохранение коммента в бд
                db.Comments.Add(comment);
                await db.SaveChangesAsync();

                //связь коммента с постом (создание PostComment)
                db.PostComments.Add(new PostComment
                {
                    Post = post,
                    Comment = comment
                });


                await AddAttachmentsToComment(newComment.AttachmentsId, comment);


                return Ok();
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }




        //ответить на коммент
        [HttpPost("[action]/{commentId}")]
        public async Task<IActionResult> ReplyToComment(int commentId, [FromBody]CommentCreateViewModel newComment)
        {
            try
            {
                //проверки
                Post post = db.Posts.FirstOrDefault(x => x.PostId == newComment.PostId);
                if (post == null)
                {
                    return NotFound("пост не найден");
                }
                User user = db.Users.FirstOrDefault(x => x.UserId == newComment.UserId);
                if (user == null)
                {
                    return NotFound("пользователь не найден");
                }
                Comment majorComment = db.Comments.FirstOrDefault(x => x.CommentId == commentId);
                if (majorComment == null)
                {
                    return NotFound( value: "коммент не найден");
                }

                //создание коммента
                Comment comment = new Comment
                {
                    Message = newComment.Message,
                    UserId = newComment.UserId,
                    IsReply = true
                };

                //сохранение коммента в бд
                db.Comments.Add(comment);
                await db.SaveChangesAsync();

                //создание ответа 
                db.ReplyComments.Add(new ReplyComment
                {
                    RepliedComment = comment,
                    MajorComment = majorComment
                });


                await AddAttachmentsToComment(newComment.AttachmentsId, comment);


                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //удаление коммента
        //не удаляет коммент полностью, а только помечает его как удаленный (поле IsDeleted)
        [HttpDelete()]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            Comment comment = db.Comments
                .Include(x=>x.CommentAttachments)
                .ThenInclude(x=>x.File)
                .FirstOrDefault(x => x.CommentId == commentId);
            if(comment == null)
            {
                return NotFound("комментарий не найден");
            }
            User user = db.Users.Include(x => x.Role).FirstOrDefault(x => x.Login == HttpContext.User.Identity.Name);
            if (user.UserId != comment.UserId && user.RoleId != 1)
            {
                return Forbid("вы не можете удалить этот комментарий");
            }
            
            try
            {
                comment.IsDeleted = true;
                db.Comments.Update(comment);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            await db.SaveChangesAsync();
            return Ok();
        }


        //изменение коммента
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(CommentUpdateViewModel newComment)
        {
            Comment comment = db.Comments
                .FirstOrDefault(x => x.CommentId == newComment.CommentId);
            if (comment == null)
            {
                return NotFound("комментарий не найден");
            }
            User user = db.Users.FirstOrDefault(x => x.Login == HttpContext.User.Identity.Name);
            if (user.UserId != comment.UserId)
            {
                return Forbid("вы не можете изменить этот комментарий");
            }
            try
            {
                comment.Message = newComment.Message;
                db.Comments.Update(comment);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        //связь коммента с его вложениями
        private async Task AddAttachmentsToComment(List<int> attachments, Comment comment)
        {
            db.CommentAttachments.AddRange(
                db.Files
                .Where(x => attachments.Contains(x.FileId))
                .Select(x =>
                    new CommentAttachment
                    {
                        File = x,
                        Comment = comment
                    })
                );
            await db.SaveChangesAsync();
        }
    }
}
