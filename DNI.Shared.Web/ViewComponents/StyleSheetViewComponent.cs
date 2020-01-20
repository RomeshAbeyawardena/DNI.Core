using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewComponents
{
    public class StyleSheetViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(string pageName, int? parentPageId)
        {
            var pageTask =_pageService.GetPage(pageName, parentPageId);
            var styleSheets = _pageService.GetStyleSheets(await pageTask);
            return View();
        }

        public StyleSheetViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
