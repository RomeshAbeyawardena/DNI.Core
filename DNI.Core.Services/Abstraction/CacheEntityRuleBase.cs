using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstraction
{
    public delegate Task CacheEntityRuleDelegate<TEntity>(IServiceProvider serviceProvider, IEnumerable<TEntity> currentValues);
    public delegate Task RequiresRefreshDelegate();
    #pragma warning disable CA1012
    public abstract class CacheEntityRuleBase<TEntity> : ICacheEntityRule<TEntity>
    {
        public CacheEntityRuleBase(RequiresRefreshDelegate requiresRefresh, CacheEntityRuleDelegate<TEntity> next)
        {
            RequiresRefresh = requiresRefresh;
            Next = next;
        }

        
        protected readonly CacheEntityRuleDelegate<TEntity> Next;
        protected readonly RequiresRefreshDelegate RequiresRefresh;
        public virtual Task OnGet(IServiceProvider services, IEnumerable<TEntity> currentValues)
        {
            if(currentValues == null || !currentValues.Any())
                return RequiresRefresh();

            return Next(services, currentValues);
        }

        public abstract Task<bool> IsEnabled(IServiceProvider serviceProvider);
    }
    #pragma warning restore CA1012
}
