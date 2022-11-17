

namespace server.Models
{
    public class File
    {
        //ID файла
        public int FileId { get; set; }
        //ссылка на файл (при обращении server/file_link - получаем файл) (УНИКАЛЬНО)
        public string FileLink { get; set; }
        //тип файла (видео, картинка или документ)
        public string FileType { get; set; }
        //название файла (псевдоним) (не уникально)
        //отображается только на стороне клиента и т.п.
        public string LogicalName { get; set; }
        //уникальное название файла на сервере. по этому имени образуется ссылки на файл
        public string PhysicalName { get; set; }
        //дата публикации
        public DateTime PublicationDate { get; set; }
        //public int UsersImageId { get; set; }
        //public User UsersImage { get; set; }
        public List<UserFile> UserFiles { get; set; }
        public List<GroupFile> GroupFiles { get; set; }
    }
}
