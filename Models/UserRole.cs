namespace server.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public string RoleName{ get; set; }
        public List<User> Users { get; set; }
    }
}
