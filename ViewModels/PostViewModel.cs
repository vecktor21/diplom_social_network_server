using server.Models;

namespace server.ViewModels
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public AuthorViewModel Author { get; set; }
        public string PostType { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public List<LikesViewModel> Likes { get; set; }
        public List<AttachmentViewModel> PostAttachments { get; set; }

        public PostViewModel(UserPost userPost)
        {
            Post post = userPost.Post;
            User user = userPost.User;
            this.PostId = post.PostId;
            this.PostType = "user";
            this.Title = post.Title;
            this.Text = post.Text;
            this.PublicationDate = post.PublicationDate;
            this.Likes = post.PostLikes.Select(x=>new LikesViewModel
            {
                LikedUserId = x.Like.LikedUserId,
                LikeId = x.LikeId,
                ObjectId = x.PostId
            }).ToList();
            this.PostAttachments = post.PostAttachements.Select(x=>new AttachmentViewModel
            {
                attachmentId = x.FileId,
                fileLink = x.File.FileLink,
                fileType = x.File.FileType,
                fileName = x.File.LogicalName
            }).ToList();

            this.Author = new AuthorViewModel
            {
                AuthorId = user.UserId,
                Img = user.Image.FileLink,
                Name = user.Nickname
            };
            this.Comments = post.PostComments.Select(x =>
            {
                CommentViewModel com = new CommentViewModel(x.Comment);
                com.IsReply = false;
                return com;
            }
            ).ToList();
        }
        public PostViewModel(GroupPost groupPost)
        {
            Post post = groupPost.Post;
            Group group = groupPost.Group;
            this.PostId = post.PostId;
            this.Title = post.Title;
            this.Text = post.Text;
            this.PublicationDate = post.PublicationDate;
            this.PostType = "group";
            this.Likes = post.PostLikes.Select(x => new LikesViewModel
            {
                LikedUserId = x.Like.LikedUserId,
                LikeId = x.LikeId,
                ObjectId = x.PostId
            }).ToList();
            this.PostAttachments = post.PostAttachements.Select(x => new AttachmentViewModel
            {
                attachmentId = x.FileId,
                fileLink = x.File.FileLink,
                fileType = x.File.FileType,
                fileName = x.File.LogicalName
            }).ToList();

            this.Author = new AuthorViewModel
            {
                AuthorId = group.GroupId,
                Img = group.GroupImage.FileLink,
                Name = group.GroupName
            };
            this.Comments = post.PostComments.Select(x =>
            {
                CommentViewModel com = new CommentViewModel(x.Comment);
                com.IsReply = false;
                return com;
            }
            ).ToList();
        }
    }
}
