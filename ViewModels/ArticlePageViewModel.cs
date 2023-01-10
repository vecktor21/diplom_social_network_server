using server.Models;
using server.Services;

namespace server.ViewModels
{
    public class ArticlePageViewModel
    {
        //id страницы
        public int ArticlePageId { get; set; }
        //текст на странице
        public string Text { get; set; }
        //название статьи, к которой принадлежит страница
        public string ArticleTitle { get; set; }
        public int AuthorId { get; set; }
        //ID статьи
        public int ArticleId { get; set; }
        //лайки
        public List<LikesViewModel> Likes { get; set; }
        //комментарии
        public List<CommentViewModel> Comments { get; set; }
        //все страницы статьи
        public List<int> articlePages { get; set; }

        public ArticlePageViewModel(ArticlePage articlePage)
        {
            this.ArticlePageId = articlePage.ArticlePageId;
            this.Text = articlePage.Text;
            this.ArticleTitle = articlePage.Article.Title;
            this.ArticleId = articlePage.ArticleId;
            this.AuthorId = articlePage.Article.AuthorId;
            this.articlePages = articlePage.Article.ArticlePages.Select(x => x.ArticlePageId).ToList();
            //лайки
            this.Likes = articlePage
                .ArticlePageLikes
                .Select(x => new LikesViewModel
                {
                    LikedUserId = x.Like.LikedUserId,
                    LikeId = x.Like.LikeId,
                    ObjectId = x.ArticlePageId
                })
                .ToList();

            //комментарии
            this.Comments = articlePage.ArticlePageComments
                .Select(x => new CommentViewModel(x.Comment))
                .ToList();

            
        }
    }
}
