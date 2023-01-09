namespace Twitter.BlazorApp.Models
{
    public class Pagination
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount(int recCount, int recPerPage)
        {
            return Convert.ToInt32(Math.Ceiling(recCount / (double)recPerPage));
        }
    }
}
