using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    /// <summary>
    /// Represents a rule used by the ICacheProvider to run against a specified entity type
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICacheEntityRule<TEntity> : ICacheEntityRule
    {
        /// <summary>
        /// Method consisting of rule logic ICacheProvider should run against the specified entity
        /// </summary>
        /// <param name="services"></param>
        /// <param name="currentValues"></param>
        /// <returns></returns>
        Task OnGet(IServiceProvider services, IEnumerable<TEntity> currentValues);
    }

    /// Represents a base rule
    public interface ICacheEntityRule
    {
        /// <summary>
        /// Determines whether the ICacheProvider should use this rule
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<bool> IsEnabled(IServiceProvider serviceProvider);
    }
}
