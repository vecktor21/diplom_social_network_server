using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.ViewModels;
using server.Services;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Query;
using System.Runtime.CompilerServices;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly ApplicationContext db;

        public FavoriteController(ApplicationContext context)
        {
            db = context;
        }


        [HttpGet("[action]")]
        public bool IsFavorite(int userId, int itemId, FavoriteTypes type)
        {
            bool isFavorite = false;
            try
            {
                switch (type)
                {
                    case FavoriteTypes.Group:
                        isFavorite = db.FavoriteGroups.Include(x => x.Favorite).Any(x => x.GroupId == itemId && x.Favorite.UserId == userId);
                        break;
                    case FavoriteTypes.Article:
                        isFavorite = db.FavoriteArticles.Include(x => x.Favorite).Any(x => x.ArticleId == itemId && x.Favorite.UserId == userId);
                        break;
                    case FavoriteTypes.Post:
                        isFavorite = db.FavoritePosts.Include(x => x.Favorite).Any(x => x.PostId == itemId && x.Favorite.UserId == userId);
                        break;
                    default: break;
                }
            }
            catch 
            {

            }
            return isFavorite;
        }

        /// <summary>
        /// получить избранное пользователя по критериям
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(int userId, FavoriteTypes type)
        {
            try
            {
                User user = db.Users.First(x=>x.UserId==userId);
                switch (type)
                {
                    case FavoriteTypes.Group:
                            return Ok(db.FavoriteGroups
                           .Include(x => x.Favorite)
                           .Include(x => x.Group.GroupImage)
                           .Where(x => x.Favorite.UserId == userId)
                           .Select(x => new GroupViewModel
                           {
                               GroupId=x.GroupId,
                               GroupImage = x.Group.GroupImage.FileLink,
                               GroupName = x.Group.GroupName,
                               IsPublic = x.Group.IsPublic
                           })
                           .ToList());
                    case FavoriteTypes.Article:

                        List<ArticleViewModel> articles = db.FavoriteArticles
                            .Include(x => x.Article.Author.Role)
                            .Include(x => x.Article.Author.Image)
                            .Include(x => x.Article.ArticleLikes)
                            .ThenInclude(x => x.Like.LikedUser)
                            .Include(x => x.Article.ArticleKeyWords)
                            .ThenInclude(x => x.KeyWord)
                            .Include(x => x.Article.ArticleComments)
                            .ThenInclude(x => x.Comment.User)
                            .Include(x => x.Article.ArticleComments)
                            .ThenInclude(x => x.Comment.CommentLikes)
                            .ThenInclude(x => x.Like)
                            .Include(x => x.Article.ArticleComments)
                            .ThenInclude(x => x.Comment.CommentAttachments)
                            .ThenInclude(x => x.File)
                            .Include(x => x.Article.ArticlePages)
                            .Where(x => x.Favorite.UserId == userId)
                            .Select(x => new ArticleViewModel(x.Article))
                            .ToList();
                        return Ok(articles);

                        /*return Ok(db.FavoriteArticles
                           .Include(x => x.Article.Author.Image)
                           .Where(x => x.Favorite.UserId == userId)
                           .Select(x => new FavoriteArticleViewModel(x.Article))
                           .ToList());*/



                    case FavoriteTypes.Post:
                        List<FavoritePost> favoritePosts= db.FavoritePosts
                            .Include(x=>x.Favorite)
                            .Include(x => x.Post)
                            .Where(x=>x.Favorite.UserId==userId)
                            .ToList();
                        List<UserPost> userPosts = db.UserPosts
                            .Include(x => x.User.Image)
                            .Where(x => favoritePosts.Select(y => y.PostId).Contains(x.PostId)).ToList();
                        List<GroupPost> groupPosts = db.GroupPosts
                            .Include(x => x.Group.GroupImage)
                            .Where(x => favoritePosts.Select(y => y.PostId).Contains(x.PostId)).ToList();
                        List<FavoritePostViewModel> favPosts = new List<FavoritePostViewModel>();
                        userPosts.ForEach(x=>favPosts.Add(new FavoritePostViewModel(x.Post, x.User.Name+" "+ x.User.Surname, x.User.Image.FileLink, "user")));
                        groupPosts.ForEach(x => favPosts.Add(new FavoritePostViewModel(x.Post, x.Group.GroupName, x.Group.GroupImage.FileLink, "group")));
                        return Ok(favPosts);
                    default: throw new InvalidEnumArgumentException();
                }
                    
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// добавление статьи в избранное
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPut("article")]
        public async Task<IActionResult> AddArticleToFavorite(int userId, int articleId)
        {
            try
            {
                User user = db.Users.First(x => x.UserId == userId);
                Article article = db.Articles.First(x => x.ArticleId == articleId);
                db.FavoriteArticles.Add(new FavoriteArticle
                {
                    Article=article,
                    Favorite=new Favorite
                    {
                        User=user
                    }
                });
                await db.SaveChangesAsync();
                return Ok();

            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// добавление группы в избранное
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPut("group")]
        public async Task<IActionResult> AddGroupToFavorite(int userId, int groupId)
        {
            try
            {
                User user = db.Users.First(x => x.UserId == userId);
                Group group = db.Groups.First(x => x.GroupId == groupId);
                db.FavoriteGroups.Add(new FavoriteGroup
                {
                    Group = group,
                    Favorite = new Favorite
                    {
                        User = user
                    }
                });
                await db.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// добавление поста в избранное
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("post")]
        public async Task<IActionResult> AddPostToFavorite(int userId, int postId)
        {
            try
            {
                User user = db.Users.First(x => x.UserId == userId);
                Post post = db.Posts.First(x => x.PostId == postId);
                db.FavoritePosts.Add(new FavoritePost
                {
                    Post = post,
                    Favorite = new Favorite
                    {
                        User = user
                    }
                });
                await db.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// удаление статьи из избранного
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpDelete("article")]
        public async Task<IActionResult> RemoveArticleFavorite(int userId, int articleId)
        {
            try
            {
                User user = db.Users.First(x => x.UserId == userId);
                FavoriteArticle article = db.FavoriteArticles
                    .Include(x=>x.Favorite)
                    .First(x => x.ArticleId == articleId && x.Favorite.UserId==userId);
                db.FavoriteArticles.Remove(article);
                await db.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// удаление поста из избранного
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete("post")]
        public async Task<IActionResult> RemovePostFavorite(int userId, int postId)
        {
            try
            {
                User user = db.Users.First(x => x.UserId == userId);
                FavoritePost post = db.FavoritePosts
                    .Include(x => x.Favorite)
                    .First(x => x.PostId == postId && x.Favorite.UserId == userId);
                db.FavoritePosts.Remove(post);
                await db.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// удаление группы из избранного
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpDelete("group")]
        public async Task<IActionResult> RemoveGroupFavorite(int userId, int groupId)
        {
            try
            {
                User user = db.Users.First(x => x.UserId == userId);
                FavoriteGroup group = db.FavoriteGroups
                    .Include(x => x.Favorite)
                    .First(x => x.GroupId == groupId && x.Favorite.UserId == userId);
                db.FavoriteGroups.Remove(group);
                await db.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
