namespace DNI.Shared.Web.Domains
{
    public class Section
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string Container { get; set; }
        public string Content { get; set; }

        public Page Page { get; set; }
    }
}
