namespace server.ViewModels.Additional
{
    public class PaginationParams
    {
        //page - номер страницы
        //take - количество элементов на странице
        //skip = take * page
        public int? page { get; set; }
        public int? total { get; set; }
        public int? skip { get; set; }
        public int? take { get; set; }
        public int? totalPages { get; set; }
    }
}
