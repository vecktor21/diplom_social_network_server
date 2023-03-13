using Microsoft.EntityFrameworkCore;
using server.Models;
using server.ViewModels;

namespace server.Services
{
    public class GroupService
    {
        private ApplicationContext db;
        public GroupService(ApplicationContext ct)
        {
            db = ct;
        }
        public bool CheckUserRole(int userId, int groupId, List<int> requiredRolesId)
        {
            GroupMember groupMember = db.GroupMembers.Include(x=>x.GroupMemberRole).FirstOrDefault(m => m.UserId == userId & m.GroupId == groupId);
            if(groupMember == null)
            {
                return false;
            }

            if (requiredRolesId.Contains(groupMember.GroupMemberRoleId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<GroupViewModel> FindGroup(string search)
        {
            return db.Groups
                .Include(x => x.GroupImage)
                .Where(x =>
                    EF.Functions.Like(x.GroupName, $"%{search}%")
                )
                .Select(x => new GroupViewModel
                {
                    GroupId = x.GroupId,
                    GroupName = x.GroupName,
                    GroupImage = x.GroupImage.FileLink,
                    IsPublic = x.IsPublic
                }).ToList();
        }
        public bool CheckUserRole(int userId, int groupId)
        {
            GroupMember groupMember = db.GroupMembers.Include(x => x.GroupMemberRole).FirstOrDefault(m => m.UserId == userId & m.GroupId == groupId);
            if (groupMember == null)
            {
                return false;
            }
            return true;
        }
    }
}
