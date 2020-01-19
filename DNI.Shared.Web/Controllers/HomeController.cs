using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.Controllers
{
    public class HomeController : DefaultControllerBase
    {
        public async Task<ActionResult> Index([FromQuery] HomeIndexRequestViewModel homeIndexRequestViewModel)
        {
            await Task.FromResult(true);
            return View(new HomeIndexViewModel {
                DisplayValue = DisplayValue(homeIndexRequestViewModel.DisplayValue),
                Value = GetValue(homeIndexRequestViewModel.Value)
            });
        }

        public async Task<string> GetValue(string value)
        {
            return await Task.FromResult(value);
        }

        public async Task<bool> DisplayValue(bool displayValue)
        {
            await Task.Delay(1000);
            return await Task.FromResult(displayValue);
        }

    }
}
