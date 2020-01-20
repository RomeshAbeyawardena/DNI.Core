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
    public class PageViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(int pageId, int? parentPageId)
        {
            var pageTask = _pageService.GetPage(pageId, parentPageId);

            var pageViewComponentViewModel = Map<Page, PageViewComponentViewModel>(await pageTask);

            return View(pageViewComponentViewModel);
        }

        public PageViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
