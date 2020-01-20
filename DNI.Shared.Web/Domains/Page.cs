using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.Domains
{
    public class Page
    {
        public int PageId { get; set; }
        public int? ParentPageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaTags { get; set; }
        public string Keywords { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}
