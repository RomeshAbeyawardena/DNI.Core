using DNI.Core.Web.Enumerations;
using System.Collections.Generic;

namespace DNI.Core.Web.ViewModels
{
    public class SectionViewComponentModel
    {
        public string Name { get; set; }
        public string Container { get; set; }
        public string Content { get; set; }
        public SectionType Type { get; set; }
        public bool IsHero { get; set; }
        public IEnumerable<SectionViewComponentModel> Sections { get; set; }
    }
}
