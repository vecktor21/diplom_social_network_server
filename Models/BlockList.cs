using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class BlockList
    {
        [Key]
        public int BlockListId { get; set; }
        public int ObjectId { get; set; }
        public string Reason { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        [ForeignKey("ObjectId")]
        public User UserObject { get; set; }
        [ForeignKey("ObjectId")]
        public Group GroupObject { get; set; }
    }
}
