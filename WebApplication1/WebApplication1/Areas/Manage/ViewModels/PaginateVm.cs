namespace WebApplication1.Areas.Manage.ViewModels
{
    public class PaginateVm<T> where T : class
    {
        public int Take { get; set; }
        public decimal Totalpage { get; set; }
        public int Currentpage { get; set; }
        public List<T> items { get; set; }



    }
}
