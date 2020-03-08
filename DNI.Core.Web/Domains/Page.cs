using System;
using System.Collections.Generic;

namespace DNI.Core.Web.Domains
{
    [MessagePack.MessagePackObject(true)]
    public class Page
    {
        public int Id { get; set; }
        public int? ParentPageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaTags { get; set; }
        public string Keywords { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public Page ParentPage { get; set; }
        public DateTimeOffset Modified { get; set; }
    }
}
