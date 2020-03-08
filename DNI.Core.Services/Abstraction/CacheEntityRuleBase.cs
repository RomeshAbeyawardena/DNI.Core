using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstraction
{
    public delegate Task CacheEntityRuleDelegate<TEntity>(IServiceProvider serviceProvider, IEnumerable<TEntity> currentValues);
    public delegate Task RequiresRefreshDelegate();

    public abstract class CacheEntityRuleBase<TEntity> : ICacheEntityRule<TEntity>
    {
        public CacheEntityRuleBase(RequiresRefreshDelegate requiresRefresh, CacheEntityRuleDelegate<TEntity> next)
        {
            _requiresRefresh = requiresRefresh;
            _next = next;
        }

        
        protected readonly CacheEntityRuleDelegate<TEntity> _next;
        protected readonly RequiresRefreshDelegate _requiresRefresh;
        public virtual Task OnGet(IServiceProvider services, IEnumerable<TEntity> currentValues)
        {
            if(currentValues == null || !currentValues.Any())
                return _requiresRefresh();

            return _next(services, currentValues);
        }
    }
}
