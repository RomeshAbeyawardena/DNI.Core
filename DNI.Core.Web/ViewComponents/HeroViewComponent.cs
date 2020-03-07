using DNI.Core.Services.Abstraction;
using DNI.Core.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Core.Web.ViewComponents
{
    public class HeroViewComponent : DefaultViewComponentBase
    {
        public async Task<IViewComponentResult> InvokeAsync(HeroViewComponentViewModel heroViewComponentViewModel)
        {
            return View(heroViewComponentViewModel);
        }
    }
}
