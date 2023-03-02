using server.Models;

namespace server.ViewModels
{
    public class FavoritePostViewModel
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImage { get; set; }
        public string PostType { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Text { get; set; }
        public FavoritePostViewModel(Post p, string authorName, string authorImage, string postType)
        {
            this.PostId = p.PostId;
            this.PostTitle = p.Title;
            this.AuthorName = authorName;
            this.AuthorImage = authorImage;
            PostType = postType;
            Text = p.Text;
            PublicationDate = p.PublicationDate;
        }
    }
}
