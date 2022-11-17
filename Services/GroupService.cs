using Microsoft.EntityFrameworkCore;
using server.Models;

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
