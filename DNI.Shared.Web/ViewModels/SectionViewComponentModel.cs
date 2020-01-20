using DNI.Shared.Web.Enumerations;
using System.Collections.Generic;

namespace DNI.Shared.Web.ViewModels
{
    public class SectionViewComponentModel
    {
        public string Name { get; set; }
        public string Container { get; set; }
        public string Content { get; set; }
        public SectionType Type { get; set; }
        public IEnumerable<SectionViewComponentModel> Sections { get; set; }
    }
}
