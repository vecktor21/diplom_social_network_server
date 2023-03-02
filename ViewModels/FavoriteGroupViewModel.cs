using server.Models;

namespace server.ViewModels
{
    public class FavoriteGroupViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupImage { get; set; }
        public FavoriteGroupViewModel(Group g)
        {
            this.GroupName = g.GroupName;
            this.GroupId = g.GroupId;
            this.GroupImage = g.GroupImage.FileLink;
        }
    }
}
