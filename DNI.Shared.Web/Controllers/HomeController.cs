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
                HeroViewComponentViewModel = new HeroViewComponentViewModel
                {
                    Title = "Home",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus quis eros non lectus pulvinar vehicula. Cras ut molestie ex. Curabitur vulputate dignissim lorem, quis malesuada metus convallis at. Morbi eu mi ante. Nunc aliquet tempor erat ac posuere. Nulla facilisi. Duis nec quam purus. Morbi non ipsum nibh.",
                    ImageUrl = "https://www.lipsum.com/images/banners/white_970x90.gif"
                },
                AlternateHeroViewComponentViewModel = new HeroViewComponentViewModel
                {
                    Title = "Alternate Home",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus quis eros non lectus pulvinar vehicula. Cras ut molestie ex. Curabitur vulputate dignissim lorem, quis malesuada metus convallis at. Morbi eu mi ante. Nunc aliquet tempor erat ac posuere. Nulla facilisi. Duis nec quam purus. Morbi non ipsum nibh.",
                    ImageUrl = "https://www.lipsum.com/images/banners/black_970x90.gif"
                },
                DisplayValue = DisplayValue(homeIndexRequestViewModel.DisplayValue),
                Value = GetValue(homeIndexRequestViewModel.Value),
                ElseValue = GetElseValue("Else")
            });
        }

        public async Task<string> GetValue(string value)
        {
            Console.WriteLine("GetValue");
            return await Task.FromResult(value);
        }

        public async Task<bool> DisplayValue(bool displayValue)
        {
            return await Task.FromResult(displayValue);
        }

        public async Task<string> GetElseValue(string value)
        {
            Console.WriteLine("GetElseValue");
            return await Task.FromResult(value);
        }

    }
}
