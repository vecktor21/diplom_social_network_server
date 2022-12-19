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
        //TODO
        //добавить остальные поля (комментарии и т.п.)

        //конструктор
        public ArticleViewModel(Article article)
        {
            this.ArticleId= article.ArticleId;
            this.Author = new UserViewModel(article.Author);
            this.Title = article.Title;
            this.Introduction= article.Introduction; 
            this.Rating= article.Rating;
            this.ArticleKeyWords = article.ArticleKeyWords.Select(x=>new KeyWordViewModel(x.KeyWord)).ToList();
        }
    }
}
