using DNI.Core.Web.Enumerations;
using Microsoft.AspNetCore.Html;

namespace DNI.Core.Web.ViewModels.Partials
{
    public class SectionViewModel
    {
        public IHtmlContent Content { get; set; }
        public string Container { get; set; }
        public SectionType Type { get; set; }
    }
}
