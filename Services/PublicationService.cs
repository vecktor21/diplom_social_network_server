using Microsoft.EntityFrameworkCore;
using server.Models;
using server.ViewModels;

namespace server.Services
{
    public class PublicationService
    {
        private ApplicationContext db;
        public PublicationService(ApplicationContext db)
        {
            this.db = db;
        }

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
    }
}
