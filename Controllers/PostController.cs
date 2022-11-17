﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using server.Models;
using server.Services;
using server.ViewModels;
using System.Runtime.CompilerServices;

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
            return Json(returnPosts);
        }



        //получить все посты группы
        [HttpGet("user/[action]/{groupId}")]
        public IActionResult GetGroupPosts(int groupId)
        {
            List<GroupPost> groupPosts = db.GroupPosts
                .Include(x => x.Group.GroupImage)
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
            return Json(returnPosts);
        }
        [HttpPost("[action]")]

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


        private PostViewModel TransformToPostViewModel(UserPost userPost)
        {
            PostViewModel postViewModel = new PostViewModel(userPost);
            foreach (var i in postViewModel.Comments)
            {
                i.Replies.Add(publicationService.FindCommentReplies(i));
            }
            return postViewModel;
        }
        private PostViewModel TransformToPostViewModel(GroupPost groupPost)
        {
            PostViewModel postViewModel = new PostViewModel(groupPost);
            foreach (var i in postViewModel.Comments)
            {
                i.Replies.Add(publicationService.FindCommentReplies(i));
            }
            return postViewModel;
        }
    }
}