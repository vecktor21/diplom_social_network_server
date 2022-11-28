using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<PostAttachment> PostAttachements { get; set; }
        public List<PostComment> PostComments { get; set; }
        public List<PostLike> PostLikes { get; set; }
    }
}
