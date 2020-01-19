using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public Task<bool> DisplayValue { get; set; }
        public Task<string> Value { get; set; }
    }
}
