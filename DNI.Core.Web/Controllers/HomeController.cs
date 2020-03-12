using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Abstraction;
using DNI.Core.Services.Attributes;
using DNI.Core.Web.Domains;
using DNI.Core.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DNI.Core.Web.Controllers
{
    public class HomeController : DefaultControllerBase
    {
        private readonly ICacheProvider _cacheProvider;

        public HomeController(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public async Task<ActionResult> Index([FromQuery] HomeIndexRequestViewModel homeIndexRequestViewModel)
        {
            await Task.FromResult(true);
            return View(new HomeIndexViewModel {
                PageRequest = new PageViewComponentRequestViewModel
                {
                    PageName = "Home"
                }
            });
        }

        [HandleException]
        public async Task<string> GetValue(string value)
        {
            await _cacheProvider.GetOrSet<Page>(Core.Contracts.Enumerations.CacheType.DistributedMemoryCache, "SAS", 
                async(cancellationToken) => await Task.FromResult(new List<Page>(new [] {new Page { Id = 2, Modified = DateTimeOffset.Now } })) );

            await _cacheProvider.GetOrSet<Page>(Core.Contracts.Enumerations.CacheType.DistributedMemoryCache, "MRA", 
                async(cancellationToken) => await Task.FromResult(new List<Page>(new [] {new Page { Id = 2, Modified = DateTimeOffset.Now } })) );
            await _cacheProvider.GetOrSet<Page>(Core.Contracts.Enumerations.CacheType.DistributedMemoryCache, "TMR", 
                async(cancellationToken) => await Task.FromResult(new List<Page>(new [] {new Page { Id = 2, Modified = DateTimeOffset.Now } })) );
            await _cacheProvider.GetOrSet<Page>(Core.Contracts.Enumerations.CacheType.DistributedMemoryCache, "LOL", 
                async(cancellationToken) => await Task.FromResult(new List<Page>(new [] {new Page { Id = 2, Modified = DateTimeOffset.Now } })) );

            return await Task.FromResult(value);
        }

        public async Task<bool> DisplayValue([FromQuery, Required]bool displayValue, [FromQuery, Required] string moo, [FromQuery, Required] string roo)
        {
            return await Task.FromResult(displayValue);
        }

        public async Task<string> GetElseValue(string value)
        {
            Console.WriteLine("GetElseValue");
            return await Task.FromResult(value);
        }

    }
}
