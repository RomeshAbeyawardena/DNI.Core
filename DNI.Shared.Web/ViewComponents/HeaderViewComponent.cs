using DNI.Shared.Services.Abstraction;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.Domains;
using DNI.Shared.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Shared.Web.ViewComponents
{
    public class HeaderViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(PageViewComponentRequestViewModel request)
        {
            var getPageTask = _pageService.GetPage(request.PageName, request.ParentPageId);

            var headerViewComponentViewModel = Map<Page,HeaderViewComponentModel>(await getPageTask);

            return View(headerViewComponentViewModel);
        }

        public HeaderViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
