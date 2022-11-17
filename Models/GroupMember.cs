namespace server.Models
{
    public class GroupMember
    {
        public int GroupMemberId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int GroupMemberRoleId { get; set; }
        public GroupMemberRole GroupMemberRole { get; set; }
    }
}
