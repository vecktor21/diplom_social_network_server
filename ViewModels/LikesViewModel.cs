namespace server.ViewModels
{
    //этот класс контроллеры возвращают
    public class LikesViewModel
    {
        public int LikeId { get; set; }
        public int LikedUserId { get; set; }
        public int ObjectId { get; set; }
    }
}
