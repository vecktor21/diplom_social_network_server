namespace server.ViewModels
{
    //абстрактный класс, который контроллеры принимают
    public abstract class LikeViewModel
    {
        public int UserId { get; set; }
        public bool IsLiked { get; set;}
    }
}
