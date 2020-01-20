using DNI.Shared.Web.Enumerations;

namespace DNI.Shared.Web.ViewModels
{
    public class SectionViewComponentModel
    {
        public string Container { get; set; }
        public string Content { get; set; }
        public SectionType Type { get; }
    }
}
