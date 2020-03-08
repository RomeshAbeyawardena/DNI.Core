using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ICacheEntityRule<TEntity> : ICacheEntityRule
    {
        Task OnGet(IServiceProvider services, IEnumerable<TEntity> currentValues);
    }

    public interface ICacheEntityRule
    {

    }
}
