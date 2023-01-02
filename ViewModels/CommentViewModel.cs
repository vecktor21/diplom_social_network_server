using Microsoft.Extensions.Hosting;
using server.Models;
using System.Diagnostics;

namespace server.ViewModels
{
    public class CommentViewModel
    {
        //ID комента
        public int CommentId { get; set; }
        //ID автора (пользователя)
        public int UserId { get; set; }
        //ник автора (пользователя)
        public string UserName { get; set; }
        //картинка автора (пользователя)
        public string ProfileImage { get; set; }
        //текст коммента
        public string Message { get; set; }
        //ID объекта, к которому пренадлежит (пост, статья или другой коммент)
        public int ObjectId { get; set; }
        //название объекта, к которому пренадлежит (пост, статья или другой коммент)
        public string ObjectName { get; set; }
        //является ли коммент ответом на другой коммент
        public bool IsReply { get; set; }
        public bool IsDeleted { get; set; }
        //список вложений
        public List<AttachmentViewModel> CommentAttachments { get; set; }
        //список лайков
        public List<LikesViewModel> Likes { get; set; }
        //список ответов
        public List<CommentViewModel> Replies { get; set; }

        public CommentViewModel(Comment comment)
        {
            this.IsReply = false;
            this.CommentId = comment.CommentId;
            this.UserId = comment.UserId;
            this.UserName = comment.User.Nickname;
            this.ProfileImage = comment.User.Image.FileLink;
            this.Message = comment.Message;
            this.IsDeleted = comment.IsDeleted;
            this.CommentAttachments = comment.CommentAttachments.Select(x=> new AttachmentViewModel
            {
                attachmentId = x.FileID,
                fileLink = x.File.FileLink,
                fileName = x.File.LogicalName,
                fileType = x.File.FileType
                
            }).ToList();
            this.Likes = comment.CommentLikes.Select(x => new LikesViewModel
            {
                LikedUserId = x.Like.LikedUserId,
                LikeId = x.LikeId,
                ObjectId = x.CommentId
            }).ToList();
            this.Replies = new List<CommentViewModel>();
        }
    }
}
