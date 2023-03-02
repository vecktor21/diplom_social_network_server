using server.Models;

namespace server.ViewModels
{
    public class FavoriteArticleViewModel
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string AuthorName { get; set; }
        public FavoriteArticleViewModel(Article a)
        {
            ArticleId = a.ArticleId;
            ArticleTitle = a.Title;
            AuthorName = a.Author.Name + " " + a.Author.Surname;
        }
    }
}
