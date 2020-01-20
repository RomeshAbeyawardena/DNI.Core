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
    public class HeaderViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(int pageId, int? parentPageId)
        {
            var getPageTask = _pageService.GetPage(pageId, parentPageId);

            var headerViewComponentViewModel = Map<Page,HeaderViewComponentViewModel>(await getPageTask);

            return View(headerViewComponentViewModel);
        }

        public HeaderViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
