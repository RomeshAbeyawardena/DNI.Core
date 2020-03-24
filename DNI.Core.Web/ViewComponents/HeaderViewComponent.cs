using DNI.Core.Services.Abstraction;
using DNI.Core.Web.Contracts;
using DNI.Core.Web.Domains;
using DNI.Core.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Core.Web.ViewComponents
{
    public class HeaderViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(PageViewComponentRequestViewModel request)
        {
            var getPageTask = _pageService.GetPage(request.PageName, request.ParentPageId);

            var headerViewComponentViewModel = Map<Page, HeaderViewComponentModel>(await getPageTask);

            return View(headerViewComponentViewModel);
        }

        public HeaderViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
