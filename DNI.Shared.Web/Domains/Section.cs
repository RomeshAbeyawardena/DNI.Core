namespace DNI.Shared.Web.Domains
{
    public class Section
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int SectionTypeId { get; set; }
        public string Container { get; set; }
        public string Content { get; set; }

        public SectionType SectionType { get; set; }

        public Enumerations.SectionType Type => (Enumerations.SectionType)SectionTypeId;

        public Page Page { get; set; }
    }
}
