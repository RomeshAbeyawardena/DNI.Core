namespace DNI.Core.Web.Domains
{
    [MessagePack.MessagePackObject(true)]
    public class SectionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Description { get; set; }
    }
}
