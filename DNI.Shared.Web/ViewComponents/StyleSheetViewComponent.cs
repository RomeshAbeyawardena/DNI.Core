using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.Domains;
using DNI.Shared.Web.ViewModels;
using DNI.Shared.Web.ViewModels.Partials;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewComponents
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
