using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Generators
{
    public class DefaultValueGenerator<TEntity> : IDefaultValueGenerator<TEntity>
    {
        private ISwitch<string, Func<object>> _defaultValueGeneratorSwitch;

        public TSelector GetDefaultValue<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty)
        {
            if(selectProperty.Body is MemberExpression memberExpression)
                return (TSelector)_defaultValueGeneratorSwitch
                    .Case(memberExpression.Member.Name)?.Invoke();

            return default;
        }
    }
}
