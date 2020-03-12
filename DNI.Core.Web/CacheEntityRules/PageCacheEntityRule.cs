using DNI.Core.Services.Abstraction;
using DNI.Core.Web.Domains;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNI.Core.Web.CacheEntityRules
{
    public class PageCacheEntityRule : CacheEntityRuleBase<Page>
    {
        public PageCacheEntityRule(RequiresRefreshDelegate requiresRefresh, CacheEntityRuleDelegate<Page> next) 
            : base(requiresRefresh, next)
        {
        }

        public override Task<bool> IsEnabled(IServiceProvider serviceProvider)
        {
            return Task.FromResult(true);
        }

        public override Task OnGet(IServiceProvider services, IEnumerable<Page> currentValues)
        {
            var logger = services.GetService<ILogger<PageCacheEntityRule>>();
            var maxCurrentPageId = currentValues.Max(page => page.Id);
            var recentlyModifiedPage = currentValues
                .OrderByDescending(page => page.Modified);

            if(maxCurrentPageId < 5 
                || recentlyModifiedPage.FirstOrDefault()?.Modified < new DateTimeOffset(2020, 03, 08, 15, 0, 0, 0, TimeSpan.Zero))
            {
                logger.LogInformation("Cache is old, requesting new data...");
                RequiresRefresh();
            }
            else
                logger.LogInformation("Cache is fine, carry on!");
            return Next(services, currentValues);
        }
    }
}
