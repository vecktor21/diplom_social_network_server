
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public bool IsReply { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
        public List<CommentLike> CommentLikes { get; set; }
        public List<ReplyComment> ReplyComments { get; set; }


    }
}
