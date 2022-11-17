using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NuGet.DependencyResolver;
using server.Models;
using server.Services;
using server.ViewModels;
using System.Data.Common;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private ApplicationContext db;
        private GroupService groupService;
        public GroupController(ApplicationContext ct, GroupService groupService)
        {
            db = ct;
            this.groupService = groupService;
        }
        //получить все группы
        [HttpGet]
        public IActionResult GetGroups()
        {
            return Json(db.Groups.Include(x => x.GroupImage).ToList());
        }


        //получить группу по ID
        [HttpGet("{groupId}")]
        public IActionResult GetGroup(int groupId)
        {
            Group g = db.Groups.Include(x=>x.GroupImage).FirstOrDefault(x => x.GroupId == groupId);

            if (g == null)
            {
                return NotFound();
            }
            return Json(new GroupViewModel
            {
                GroupId = g.GroupId,
                GroupName = g.GroupName,
                IsPublic = g.IsPublic,
                GroupImage = g.GroupImage.FileLink
            });
        }


        //получить всех членов групп (GroupMembers)
        [HttpGet("[action]")]
        public IActionResult GetGroupMembers()
        {
            return Json(db.GroupMembers.ToList());
        }


        //получить члена группы по userId (GroupMembers)
        //то есть узнать в каких группах состоит пользователь
        [HttpGet("[action]/{memberId}")]
        public IActionResult GetUserGroups(int memberId)
        {
            /*var d = db.GroupMembers
                .Include(x => x.Group)
                .FirstOrDefault(x => x.UserId == memberId)
                */
            var d = db.Groups
                .Include(x=>x.GroupMembers)
                .Include(x=>x.GroupImage)
                .Where(x=>
                    x.GroupMembers
                        .Where(x=>x.UserId == memberId)
                        .Any()
                    )
                .Select(x=>new GroupViewModel
                {
                    GroupId=x.GroupId,
                    GroupImage = x.GroupImage.FileLink,
                    GroupName = x.GroupName,
                    IsPublic = x.IsPublic
                });
            return Json(d);
        }


        //получить члена группы по groupId(GroupMembers)
        //то есть узнать кто состоит в конкретной группе
        [HttpGet("[action]/{groupId}")]
        public IActionResult GetGroupMembersByGroupId(int groupId)
        {
            return Json(db.GroupMembers.FirstOrDefault(x => x.GroupId == groupId));
        }


        //создание группы
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateGroup(GroupCreateViewModel newGroup)
        {
            Group g = db.Groups.FirstOrDefault(x => x.GroupName == newGroup.GroupName);
            if(g != null)
            {
                return BadRequest("группа с таким именем уже существует");
            }
            Group group = new Group
            {
                GroupName = newGroup.GroupName,
                IsPublic = newGroup.IsPublic,
                GroupImage = new Models.File
                {
                    LogicalName = "default_group_image.png",
                    PhysicalName = "default_group_image.png",
                    FileType = "IMAGE",
                    PublicationDate = DateTime.Now,
                    FileLink = "/files/images/default_group_image.png"
                }
            };
            db.Groups.Add(group);
            await db.SaveChangesAsync();
            group = db.Groups.Single(x => x.GroupName == newGroup.GroupName);

            GroupMember groupMember = new GroupMember
            {
                UserId = newGroup.AdminId,
                GroupId = group.GroupId,
                GroupMemberRole = db.GroupMemberRoles.Single(x => x.GroupMemberRoleId == 1)
            };
            db.GroupMembers.Add(groupMember);
            await db.SaveChangesAsync();
            return Ok();
        }



        //поиск групп по ключу
        [HttpGet("[action]")]
        public IActionResult FindGroups(string search)
        {
            if(string.IsNullOrEmpty(search))
            {
                return Json(Array.Empty<string>());
            }
            var groups = db.Groups
                .Include(x => x.GroupImage)
                .Where(x =>
                    EF.Functions.Like(x.GroupName, $"%{search}%")
                )
                .Select(x=>new GroupViewModel { 
                    GroupId=x.GroupId,
                    GroupName=x.GroupName,
                    GroupImage=x.GroupImage.FileLink,
                    IsPublic=x.IsPublic
                });
            return Json(groups);
        }


        //подписаться на группу
        [HttpGet("[action]")]
        public async Task<IActionResult> SubscribeToGroup(int userId, int groupId)
        {
            //поиск нового пользователя и группы
            User user = db.Users.Include(x => x.Image).FirstOrDefault(x => x.UserId == userId);
            Group group = db.Groups.Include(x => x.GroupImage).FirstOrDefault(x => x.GroupId == groupId);
            if (user == null || group == null)
            {
                return NotFound("указанная группа или пользователь не найден");
            }
            GroupMember existingMember = db.GroupMembers.FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
            if (existingMember != null)
            {
                return BadRequest("вы уше являетесь участником этой группы");
            }
            if (group.IsPublic)
            {
                //добавление пользователя в публичную группу
                GroupMemberRole role = db.GroupMemberRoles.Single(x => x.GroupMemberRoleId == 3);
                GroupMember member = new GroupMember
                {
                    Group = group,
                    User = user,
                    GroupMemberRole = role
                };
                try
                {
                    db.GroupMembers.Add(member);
                    await db.SaveChangesAsync();
                }catch(Exception e)
                {
                    return BadRequest(e);
                }
                //рассылка уведомления всем руководящим группой
                List<User> moderators = db.GroupMembers
                    .Include(x => x.GroupMemberRole)
                    .Include(x => x.User)
                    .Where(x => x.GroupMemberRole.GroupMemberRoleId == 1 || x.GroupMemberRole.GroupMemberRoleId == 2)
                    .Select(x => x.User)
                    .ToList();
                foreach(var u in moderators)
                {
                    Notification newUserNotification = new Notification
                    {
                        IsViewed = false,
                        Message = $"В вашей группе {group.GroupName} новый пользователь",
                        UserId = u.UserId
                    };
                    db.Notifications.Add(newUserNotification);
                    await db.SaveChangesAsync();
                }
                return Ok();

            }
            else
            {
                //отправка запроса на вступление в приватную группу
                RequestToGroup req = new RequestToGroup
                {
                    Group = group,
                    User = user
                };
                try
                {
                    db.RequestToGroup.Add(req);
                    await db.SaveChangesAsync();
                }catch(Exception e)
                {
                    return BadRequest(e);
                }
                return Ok();
            }
        }

        //ответь запрос на вступление в приватную группу
        /*применяетя 
         * 1) пользователям для отмены запроса на вступление
         * 2) лидерам для принятия\отказа на вступление в группу
         */
        [HttpGet("[action]")]
        public async Task<IActionResult> ReactToRequest(int userId, int groupId, bool isAccepted)
        {
            RequestToGroup req = db.RequestToGroup.FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
            if (req == null)
            {
                return NotFound();
            }
            if (isAccepted)
            {
                GroupMember member = new GroupMember
                {
                    GroupId = groupId,
                    GroupMemberRoleId = 3,
                    UserId = userId
                };
                db.GroupMembers.Add(member);
            }
            db.RequestToGroup.Remove(req);
            await db.SaveChangesAsync();
            return Ok();
        }

        //отписаться
        [HttpGet("[action]")]
        public async Task<IActionResult> Unsubscribe(int userId, int groupId)
        {
            GroupMember member = db.GroupMembers.FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
            if(member == null)
            {
                return NotFound();
            }
            db.GroupMembers.Remove(member);
            await db.SaveChangesAsync();
            return Ok();
        }


        //получить запросу на добавление в приватную группу
        [HttpGet("[action]")]
        public IActionResult GetRequestToGroup(int groupId)
        {
            return Json(db.RequestToGroup.Where(x=>x.GroupId==groupId));
        }

        //получить все запросы на вступление в приватную группу, лидером которой является пользователь
        [HttpGet("[action]")]
        public IActionResult GetRequestsToGroupByGroupLeader(int userId)
        {
            List<Group> groupsLeader = db.GroupMembers
                .Include(x=>x.Group)
                .Where(x=>x.UserId==userId && (x.GroupMemberRoleId == 1 || x.GroupMemberRoleId == 2))
                .Select(x=>x.Group)
                .ToList();
            List<RequestToGroup> requests = new List<RequestToGroup>();
            foreach(Group group in groupsLeader)
            {
                requests.AddRange(
                        db.RequestToGroup
                        .Include(x=>x.User)
                        .Include(x=>x.Group)
                        .Where(x=>x.GroupId == group.GroupId)
                    );
            }
            return Json(requests.Select(x=>new RequestToGroupViewModel(x)));
        }

        //получить руководство группы (админа и модераторов)
        [HttpGet("[action]/{groupId}")]
        public IActionResult GetGroupLeaders (int groupId)
        {
            var groupLeader = db.GroupMembers
                .Include(x=>x.User)
                .Include(x=>x.Group)
                .Where(x=>x.GroupId== groupId);
            
            return Json(groupLeader.First().User.Name);
        }

        //проверка на членство и различные роли пользователя в группе
        [HttpGet("[action]")]
        public IActionResult CheckBelonging(int userId, int groupId)
        {
            GroupBelongingViewModel belonging = new GroupBelongingViewModel();
            var groupMember = db.GroupMembers
                .Include(x => x.GroupMemberRole)
                .FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);
            //проверяем, подписан ли пользователь на группу
            if (groupMember == null)
            {
                //если не подписан - проверяем отправлял ли он запрос 
                belonging.IsMember = false;
                RequestToGroup req = db.RequestToGroup.FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
                if (req != null)
                {
                    belonging.IsRequestSent = true;
                }
                return Json(belonging);
            }
            GroupMemberRole role = groupMember.GroupMemberRole;
            //если подписан - проверяем его роль
            switch (role.GroupMemberRoleId)
            {
                case 1:
                    belonging.IsMember = true;
                    belonging.IsLeader = true;
                    belonging.IsAdmin = true;
                    break;
                case 2:
                    belonging.IsMember = true;
                    belonging.IsLeader = true;
                    belonging.IsModerator = true;
                    break;
                case 3:
                    belonging.IsMember = true;
                    break;
                default:
                    belonging.IsMember = false;
                    break;
            }
            return Json(belonging);
        }
    }
}
