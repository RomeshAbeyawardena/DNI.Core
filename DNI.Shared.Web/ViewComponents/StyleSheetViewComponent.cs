using DNI.Core.Services.Abstraction;
using DNI.Core.Web.Contracts;
using DNI.Core.Web.Domains;
using DNI.Core.Web.ViewModels;
using DNI.Core.Web.ViewModels.Partials;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Core.Web.ViewComponents
{
    public class StyleSheetViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(PageViewComponentRequestViewModel request)
        {
            var pageTask =_pageService.GetPage(request.PageName, request.ParentPageId);
            var styleSheets = _pageService.GetStyleSheets(await pageTask);

            var styleSheetViewModels = Map<StyleSheet, StyleSheetViewModel>(await styleSheets);

            return View(new StyleSheetViewComponentModel {
                StyleSheetViewModel = styleSheetViewModels
            });
        }

        public StyleSheetViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
