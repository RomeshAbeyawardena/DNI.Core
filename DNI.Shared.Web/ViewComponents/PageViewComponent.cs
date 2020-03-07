using DNI.Core.Services.Abstraction;
using DNI.Core.Web.Contracts;
using DNI.Core.Web.Domains;
using DNI.Core.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DNI.Core.Web.ViewComponents
{
    public class PageViewComponent : DefaultViewComponentBase
    {
        private readonly IPageService _pageService;

        public async Task<IViewComponentResult> InvokeAsync(PageViewComponentRequestViewModel request)
        {
            var pageTask = _pageService.GetPage(request.PageName, request.ParentPageId);
            var pages = await pageTask;
            var pageViewComponentViewModel = Map<Page, PageViewComponentViewModel>(pages);

            return View(pageViewComponentViewModel);
        }

        public PageViewComponent(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
