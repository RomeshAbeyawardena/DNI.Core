using DNI.Shared.Web.Enumerations;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewModels.Partials
{
    public class SectionViewModel
    {
        public IHtmlContent Content { get; set; }
        public string Container { get; set; }
        public SectionType Type { get; set; }
    }
}
