namespace Twitter.BlazorApp.Models
{
    public class Paging
    {
        public string LinkText { get; set; }
        public int PageId { get; set; }

        public Paging(int page, string text)
        {
            PageId = page;
            LinkText = text;
        }
    }
}
