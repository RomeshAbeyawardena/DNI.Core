﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewModels
{
    public class PageViewComponentViewModel
    {
        public IEnumerable<SectionViewComponentViewModel> Sections { get; set; }
    }
}
