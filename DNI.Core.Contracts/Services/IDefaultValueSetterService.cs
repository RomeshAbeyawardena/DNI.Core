namespace DNI.Core.Contracts.Services
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IDefaultValueSetterService
    {
        void SetDefaultValues<TEntity>(TEntity entity);

        void SetDefaultValues<TEntity>(IEnumerable<PropertyInfo> properties, TEntity value);
    }
}
