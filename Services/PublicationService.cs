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

        public CommentViewModel FindCommentReplies(CommentViewModel rootComment)
        {
            if (db.ReplyComments.Any(x => x.MajorCommentId == rootComment.CommentId))
            {
                foreach(Comment i in db.ReplyComments.Where(x => x.MajorCommentId == rootComment.CommentId).Select(x=>x.RepliedComment).ToList())
                {
                    CommentViewModel repliedComment = new CommentViewModel(i);
                    repliedComment.IsReply = true;
                    rootComment.Replies.Add(FindCommentReplies(repliedComment));
                }
                return rootComment;
            }
            return rootComment;
        }
    }
}
