using DNI.Shared.Web.Enumerations;
using Microsoft.AspNetCore.Html;

namespace DNI.Shared.Web.ViewModels.Partials
{
    public class SectionViewModel
    {
        public IHtmlContent Content { get; set; }
        public string Container { get; set; }
        public SectionType Type { get; set; }
    }
}
