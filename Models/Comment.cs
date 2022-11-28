
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        //ID автора (пользователя)
        public int UserId { get; set; }
        //ссылка на автора (пользователя)
        public User User { get; set; }
        public string Message { get; set; }
        public bool IsReply { get; set; }
        public List<CommentAttachment> CommentAttachments { get; set; }
        public List<CommentLike> CommentLikes { get; set; }
        public List<ReplyComment> ReplyComments { get; set; }


    }
}
