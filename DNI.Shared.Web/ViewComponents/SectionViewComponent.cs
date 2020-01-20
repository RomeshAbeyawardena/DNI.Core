using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.Domains;
using DNI.Shared.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewComponents
{
    public class SectionViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(int pageId, int sectionId, int? parentPageId)
        {
            var pageSectionTask = _pageService.GetPageSection(pageId, sectionId, parentPageId);

            var sectionViewComponentViewModel = Map<Section, SectionViewComponentViewModel>(await pageSectionTask);
            return View(sectionViewComponentViewModel);
        }

        public SectionViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
