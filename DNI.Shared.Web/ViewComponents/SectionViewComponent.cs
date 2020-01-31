using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.Domains;
using DNI.Shared.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewComponents
{
    public class SectionViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(SectionViewComponentModel sectionViewComponentViewModel)
        {
            sectionViewComponentViewModel.Sections = Map<Section, SectionViewComponentModel>(await _pageService.GetPageSections(1));
            return View(sectionViewComponentViewModel);
        }

        public SectionViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
