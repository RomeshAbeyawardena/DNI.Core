using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DNI.Shared.Web.ViewComponents
{
    public class SectionViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        //public async Task<IViewComponentResult> InvokeAsync(SectionViewComponentRequestViewModel request)
        //{
        //    var pageSectionTask = _pageService.GetPageSection(request.PageName, request.SectionId, request.ParentPageId);

        //    var sectionViewComponentViewModel = Map<Section, SectionViewComponentViewModel>(await pageSectionTask);
        //    return Invoke(sectionViewComponentViewModel);
        //}

        public IViewComponentResult Invoke(SectionViewComponentViewModel sectionViewComponentViewModel)
        {
            return View(sectionViewComponentViewModel);
        }

        public SectionViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
