using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Contracts.Services
{
    public interface IDefaultValueSetterService
    {
        void SetDefaultValues<TEntity>(TEntity entity);
        void SetDefaultValues<TEntity>(IEnumerable<PropertyInfo> properties, TEntity value);
    }
}
