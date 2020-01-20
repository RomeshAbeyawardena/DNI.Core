using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewComponents
{
    public class HeroViewComponent : DefaultViewComponentBase
    {
        public async Task<IViewComponentResult> InvokeAsync(HeroViewComponentViewModel heroViewComponentViewModel)
        {
            return View(heroViewComponentViewModel);
        }
    }
}
