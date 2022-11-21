using Microsoft.Extensions.Hosting;
using server.Models;
using System.Diagnostics;

namespace server.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public string Message { get; set; }
        public int ObjectId { get; set; }
        public string ObjectName { get; set; }
        public bool IsReply { get; set; }
        public AttachmentViewModel Attachment { get; set; }
        public List<LikesViewModel> Likes { get; set; }
        public List<CommentViewModel> Replies { get; set; }

        public CommentViewModel(Comment comment)
        {
            this.CommentId = comment.CommentId;
            this.UserId = comment.UserId;
            this.UserName = comment.User.Nickname;
            this.ProfileImage = comment.User.Image.FileLink;
            this.Message = comment.Message;
            this.Attachment = new AttachmentViewModel
            {
                attachmentId = comment.FileId,
                fileLink = comment.File.FileLink,
                fileType = comment.File.FileType,
                fileName = comment.File.LogicalName
            };
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
