namespace server.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsPublic { get; set; }
        public int GroupImageId { get; set; }
        public File GroupImage { get; set; }
        public List<GroupBlockList> BlockedUsers { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<GroupFile> GroupFiles { get; set; }
        public List<GroupMember> GroupMembers { get; set; }
    }
}