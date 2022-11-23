using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;
using server.ViewModels;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private ApplicationContext db;
        private PublicationService publicationService;
        public PostController(ApplicationContext ct, PublicationService publicationService)
        {
            this.db = ct;
            this.publicationService = publicationService;
        }
        //получить конкретный пост пользователя
        [HttpGet("user/{postId}")]
        public IActionResult GetUserPost(int postId)
        {
            UserPost post = db.UserPosts
                .Include(x=>x.User.Image)
                .Include(x=>x.Post.PostAttachements)
                .ThenInclude(x=>x.File)
                .Include(x=>x.Post.PostComments)
                .ThenInclude(x=>x.Comment.User.Image)
                .Include(x=>x.Post.PostLikes)
                .ThenInclude(x=>x.Like.LikedUser)
                .FirstOrDefault(x => x.PostId == postId);
            if(post == null)
            {
                return NotFound();
            }

            return Json(TransformToPostViewModel(post));
        }



        //получить конкретный пост группы
        [HttpGet("group/{postId}")]
        public IActionResult GetGroupPost(int postId)
        {
            GroupPost post = db.GroupPosts
                .Include(x => x.Group.GroupImage)
                .Include(x => x.Post.PostAttachements)
                .ThenInclude(x => x.File)
                .Include(x => x.Post.PostComments)
                .ThenInclude(x => x.Comment.User.Image)
                .Include(x => x.Post.PostLikes)
                .ThenInclude(x => x.Like.LikedUser)
                .FirstOrDefault(x => x.PostId == postId);
            if (post == null)
            {
                return NotFound();
            }

            return Json(TransformToPostViewModel(post));
        }



        //получить все посты пользователя
        [HttpGet("user/[action]/{userId}")]
        public IActionResult GetUserPosts(int userId)
        {
            List<UserPost> userPosts = db.UserPosts
                .Include(x => x.User.Image)
                .Include(x => x.Post.PostAttachements)
                .ThenInclude(x => x.File)
                .Include(x => x.Post.PostComments)
                .ThenInclude(x => x.Comment.User.Image)
                .Include(x => x.Post.PostLikes)
                .ThenInclude(x => x.Like.LikedUser)
                .Where(x => x.UserId == userId)
                .ToList();

            List<PostViewModel> returnPosts = new List<PostViewModel>();

            foreach(var i in userPosts)
            {
                returnPosts.Add(TransformToPostViewModel(i));
            }
            return Json(returnPosts.OrderByDescending(x => x.PublicationDate));
        }



        //получить все посты группы
        [HttpGet("group/[action]/{groupId}")]
        public IActionResult GetGroupPosts(int groupId)
        {
            List<GroupPost> groupPosts = db.GroupPosts
                .Include(x => x.Group.GroupImage)
                .Include(x => x.Post.PostAttachements)
                .ThenInclude(x => x.File)
                .Include(x => x.Post.PostComments)
                .ThenInclude(x => x.Comment.User.Image)
                .Include(x => x.Post.PostLikes)
                .ThenInclude(x => x.Like.LikedUser)
                .Where(x => x.GroupId == groupId)
                .ToList();

            List<PostViewModel> returnPosts = new List<PostViewModel>();

            foreach (var i in groupPosts)
            {
                returnPosts.Add(TransformToPostViewModel(i));
            }
            return Json(returnPosts.OrderByDescending(x => x.PublicationDate));
        }



        //получить посты друзей пользователя и посты групп, на которые он подписан
        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserLinkedPosts(int userId)
        {
            User user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if(user == null)
            {
                return NotFound("пользователь не найден");
            }
            List<PostViewModel> posts = new List<PostViewModel>();
            List<GroupMember> userGroups = db.GroupMembers.Where(x => x.UserId == userId).ToList();

            List<GroupPost> groupPosts = new List<GroupPost>();
            
            foreach(var userGroup in userGroups)
            {
                groupPosts.AddRange(
                        db.GroupPosts
                        .Include(x => x.Group.GroupImage)
                        .Include(x => x.Post.PostAttachements)
                        .ThenInclude(x => x.File)
                        .Include(x => x.Post.PostComments)
                        .ThenInclude(x => x.Comment.User.Image)
                        .Include(x => x.Post.PostLikes)
                        .ThenInclude(x => x.Like.LikedUser)
                        .Where(x=>x.GroupId==userGroup.GroupId)
                        .ToList()
                    );
            }
            foreach (var i in groupPosts)
            {
                posts.Add(TransformToPostViewModel(i));
            }

            List<Friend> friends = db.Friends.Where(x => (x.User1Id == userId & x.User2Id != userId) || (x.User1Id != userId & x.User2Id == userId)).ToList();

            List<UserPost> userPosts = new List<UserPost>();
            foreach (var friend in friends)
            {
                userPosts.AddRange(
                        db.UserPosts
                        .Include(x => x.User.Image)
                        .Include(x => x.Post.PostAttachements)
                        .ThenInclude(x => x.File)
                        .Include(x => x.Post.PostComments)
                        .ThenInclude(x => x.Comment.User.Image)
                        .Include(x => x.Post.PostLikes)
                        .ThenInclude(x => x.Like.LikedUser)
                        .Where(x => friend.User1Id == userId ? (x.UserId == friend.User2Id) : (x.UserId == friend.User1Id))
                        .ToList()
                    );
            }
            foreach (var i in userPosts)
            {
                posts.Add(TransformToPostViewModel(i));
            }
            return Json(posts.OrderByDescending(x=>x.PublicationDate));
        }



        //создание поста пользователя
        //при этом все вложения в посте загружаются отдельным запросом на сервер, сюда идут только ID этих вложений
        [HttpPost("user/[action]")]
        public async Task<IActionResult> CreateUserPost(PostCreateViewModel newPost)
        {
            try
            {
                User user = db.Users.FirstOrDefault(x => x.UserId == newPost.AuthorId);
                if (user == null)
                {
                    return NotFound("не верный ID пользователя");
                }

                Post post = new Post
                {
                    Text = newPost.Text,
                    Title = newPost.Title
                };
                db.UserPosts.Add(new UserPost
                {
                    Post = post,
                    UserId = user.UserId
                });
                await db.SaveChangesAsync();

                foreach (int i in newPost.Attachments)
                {
                    db.PostAttachements.Add(
                        new PostAttachement
                        {
                            Post = post,
                            File = db.Files.Single(x => x.FileId == i)
                        }
                    );
                }
                await db.SaveChangesAsync();
                return Ok();
            }
            catch(Exception E) {
                return BadRequest();
            }
        }



        //создание поста группы
        //при этом все вложения в посте загружаются отдельным запросом на сервер, сюда идут только ID этих вложений
        [HttpPost("group/[action]")]
        public async Task<IActionResult> CreateGroupPost(PostCreateViewModel newPost)
        {
            try
            {
                Group group = db.Groups.FirstOrDefault(x => x.GroupId == newPost.AuthorId);
                if (group == null)
                {
                    return NotFound("не верный ID группы");
                }

                Post post = new Post
                {
                    Text = newPost.Text,
                    Title = newPost.Title
                };
                db.GroupPosts.Add(new GroupPost
                {
                    Post = post,
                    GroupId = group.GroupId
                });
                await db.SaveChangesAsync();

                foreach (int i in newPost.Attachments)
                {
                    db.PostAttachements.Add(
                        new PostAttachement
                        {
                            Post = post,
                            File = db.Files.Single(x => x.FileId == i)
                        }
                    );
                }
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception E)
            {
                return BadRequest();
            }
        }


        //методы для преобразования поста группы\пользователя в модель представления
        private PostViewModel TransformToPostViewModel(UserPost userPost)
        {
            PostViewModel postViewModel = new PostViewModel(userPost);
            foreach (var i in postViewModel.Comments)
            {
                i.Replies.Add(publicationService.FindCommentReplies(i));
            }
            postViewModel.PostType = "user";
            return postViewModel;
        }
        private PostViewModel TransformToPostViewModel(GroupPost groupPost)
        {
            PostViewModel postViewModel = new PostViewModel(groupPost);
            foreach (var i in postViewModel.Comments)
            {
                i.Replies.Add(publicationService.FindCommentReplies(i));
            }
            postViewModel.PostType = "group";
            return postViewModel;
        }
    }
}
