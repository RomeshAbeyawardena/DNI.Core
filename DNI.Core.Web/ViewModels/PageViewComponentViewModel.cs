using System.Collections.Generic;

namespace DNI.Core.Web.ViewModels
{
    public class PageViewComponentViewModel
    {
        public IEnumerable<SectionViewComponentModel> Sections { get; set; }
        public IEnumerable<StyleSheetViewComponentModel> StyleSheets { get; set; }
    }
}
