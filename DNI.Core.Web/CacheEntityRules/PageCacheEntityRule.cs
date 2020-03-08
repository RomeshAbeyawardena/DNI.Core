using DNI.Core.Services.Abstraction;
using DNI.Core.Web.Domains;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Web.CacheEntityRules
{
    public class PageCacheEntityRule : CacheEntityRuleBase<Page>
    {
        public PageCacheEntityRule(RequiresRefreshDelegate requiresRefresh, CacheEntityRuleDelegate<Page> next) : base(requiresRefresh, next)
        {
        }

        public override Task OnGet(IServiceProvider services, IEnumerable<Page> currentValues)
        {
            var logger = services.GetService<ILogger<PageCacheEntityRule>>();

            logger.LogInformation("I was requested");
            return _next(services, currentValues);
        }
    }
}
