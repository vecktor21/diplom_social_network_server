using Microsoft.EntityFrameworkCore;
using server.Models;
using server.ViewModels;

namespace server.Services
{
    public class PublicationService
    {
        private ApplicationContext db;
        private IHttpContextAccessor _httpContextAccessor;
        public PublicationService(ApplicationContext db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        //рекурсивно получает все ответы на исходный коммент
        public List<CommentViewModel> FindCommentReplies(CommentViewModel rootComment)
        {
            if (db.ReplyComments.Any(x => x.MajorCommentId == rootComment.CommentId))
            {
                List<CommentViewModel> commentReplies = new List<CommentViewModel>();
                foreach (Comment i in db.ReplyComments
                    .Include(x=>x.RepliedComment.CommentAttachments)
                    .ThenInclude(x=>x.File)
                    .Include(x=>x.RepliedComment.User.Image)
                    .Include(x => x.RepliedComment.CommentLikes)
                    .ThenInclude(x=>x.Like)
                    .Where(x => x.MajorCommentId == rootComment.CommentId)
                    .Select(x=>x.RepliedComment)
                    .ToList())
                {
                    CommentViewModel repliedComment = new CommentViewModel(i);
                    repliedComment.IsReply = true;
                    repliedComment.ObjectId = rootComment.UserId;
                    repliedComment.ObjectName = rootComment.UserName;
                    repliedComment.Replies = FindCommentReplies(repliedComment);
                    commentReplies.Add(repliedComment);
                }
                return commentReplies;
            }
            return new List<CommentViewModel>();
        }
        //проверяет, является ли текущий пользователь - автором статьи
        public bool IsCurrentUserArticleAuthor(Article article)
        {
            User user = db.Users.FirstOrDefault(x => x.Login == _httpContextAccessor.HttpContext.User.Identity.Name);

            if (article.AuthorId == user.UserId)
            {
                return true;
            }
            return false;
        }

        //метод для поиска ключевых слов
        public List<KeyWord> FindKeyWords(string query)
        {
            List<KeyWord> keyWords = db.KeyWords
                .Include(x=>x.ArticleKeyWords)
                .Where(x =>
                    EF.Functions.Like(x.KeyWordRu, $"%{query}%") ||
                    EF.Functions.Like(x.KeyWordEn, $"%{query}%") ||
                    EF.Functions.Like(x.KeyWordRu + x.KeyWordEn, $"%{query}%") ||
                    EF.Functions.Like(x.KeyWordEn + x.KeyWordRu, $"%{query}%")
                )
                .ToList();
            return keyWords;
        }
    }
}
