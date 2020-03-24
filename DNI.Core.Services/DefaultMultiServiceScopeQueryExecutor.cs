namespace DNI.Core.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;

    internal class MultiServiceScopeQuery
    {
        public Type Respository { get; set; }

        public IQueryable Query { get; set; }
    }

    internal class MultiServiceScopeQuery<T> : MultiServiceScopeQuery
    {
        public new IQueryable<T> Query { get => (IQueryable<T>)base.Query; set => base.Query = value; }

        public Type EntityType => typeof(T);
    }

    internal class DefaultMultiServiceScopeQueryExecutor
    {
        private readonly IServiceProvider serviceProvider;

        internal object GetRequiredService(Type serviceType, IServiceScope serviceScope)
        {
            return serviceScope.ServiceProvider.GetRequiredService(serviceType);
        }

        internal IEnumerable<Type> GetGenericTypes(Type serviceType)
        {
            return serviceType.GetGenericArguments();
        }

        public DefaultMultiServiceScopeQueryExecutor(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IDictionary<Type, IEnumerable>> RunAsync(int maximumServiceScopesInUse, params MultiServiceScopeQuery[] queries)
        {
            var serviceScopeCount = 0;
            foreach (var query in queries)
            {
                if (serviceScopeCount >= maximumServiceScopesInUse)
                {
                    throw new IndexOutOfRangeException();
                }

                using var serviceScope = serviceProvider.CreateScope();

                var service = GetRequiredService(query.Respository, serviceScope);
            }

            return new Dictionary<Type, IEnumerable>();
        }
    }
}
