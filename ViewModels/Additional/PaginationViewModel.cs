namespace server.ViewModels.Additional
{
    public class PaginationViewModel<T>
    {
        public List<T> values { get; set; } = null!;
        public PaginationParams paginationParams { get; set; } = null!;
    }
}
