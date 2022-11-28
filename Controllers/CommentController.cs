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

                //связь коммента с его вложениями
                db.CommentAttachments.AddRange(
                    db.Files
                    .Where(x => newComment.AttachmentsId.Contains(x.FileId))
                    .Select(x =>
                        new CommentAttachment
                        {
                            File = x,
                            Comment = comment
                        })
                    );

                await db.SaveChangesAsync();
                return Ok();
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
