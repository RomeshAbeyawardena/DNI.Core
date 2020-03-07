using DNI.Core.Services.Abstraction;
using DNI.Core.Web.Contracts;
using DNI.Core.Web.Domains;
using DNI.Core.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Core.Web.ViewComponents
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
