using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstraction
{
    public abstract class CacheEntityRuleBase<TEntity> : ICacheEntityRule<TEntity>
    {
        public delegate Task CacheEntityRuleDelegate(IEnumerable<TEntity> currentValues);
        public delegate Task RequiresRefreshDelegate();
        
        
        protected readonly CacheEntityRuleDelegate _next;
        protected readonly RequiresRefreshDelegate _requiresRefresh;
        public Task OnGet(IEnumerable<TEntity> currentValues)
        {
            if(currentValues == null || !currentValues.Any())
                return _requiresRefresh();

            return _next(currentValues);
        }
    }
}
