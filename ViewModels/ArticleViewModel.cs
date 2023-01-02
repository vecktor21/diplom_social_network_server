using server.Models;

namespace server.ViewModels
{
    public class ArticleViewModel
    {
        //временно. нужно будет переделать
        //ID статьи
        public int ArticleId { get; set; }
        //автор
        public UserViewModel Author { get; set; }
        //название статьи
        public string Title { get; set; }
        //введение в статью
        public string Introduction { get; set; }
        //оценки
        public int Rating { get; set; }
        //ключевые слова статьи
        public List<KeyWordViewModel> ArticleKeyWords { get; set; }
        //лайки
        public List<LikesViewModel> Likes { get; set; }
        //комментарии
        public List<CommentViewModel> Comments { get; set; }
        //страницы статьи
        public List<int> ArticlePages { get; set; }

        //конструктор
        public ArticleViewModel(Article article)
        {
            this.ArticleId= article.ArticleId;
            this.Author = new UserViewModel(article.Author);
            this.Title = article.Title;
            this.Introduction= article.Introduction; 
            this.Rating= article.ArticleLikes.Count();
            this.ArticleKeyWords = article.ArticleKeyWords.Select(x=>new KeyWordViewModel(x.KeyWord)).ToList();

            //лайки
            this.Likes = article
                .ArticleLikes
                .Select(x => new LikesViewModel
                {
                    LikedUserId=x.Like.LikedUserId,
                    LikeId = x.Like.LikeId,
                    ObjectId = x.ArticleId
                })
                .ToList();

            //комментарии
            this.Comments = article.ArticleComments
                .Select(x => new CommentViewModel(x.Comment))
                .ToList();

            //ссылки на страницы
            this.ArticlePages = article.ArticlePages.Select(x=>x.ArticlePageId).ToList();
        }
    }
}
