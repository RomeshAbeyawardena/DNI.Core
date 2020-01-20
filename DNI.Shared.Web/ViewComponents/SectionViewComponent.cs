using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DNI.Shared.Web.ViewComponents
{
    public class SectionViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public IViewComponentResult Invoke(SectionViewComponentModel sectionViewComponentViewModel)
        {
            return View(sectionViewComponentViewModel);
        }

        public SectionViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
