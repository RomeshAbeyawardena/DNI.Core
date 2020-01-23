using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Generators
{
    internal sealed class DefaultValueGenerator<TEntity> : IDefaultValueGenerator<TEntity>
    {
        private ISwitch<string, Func<object>> _defaultValueGeneratorSwitch;

        public DefaultValueGenerator()
        {
            _defaultValueGeneratorSwitch = Switch.Create<string, Func<object>>();
        }

        public IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<object> createInstance)
        {
            var member = GetMemberInfo(selectProperty);

            if(string.IsNullOrEmpty(member.Name))
                return default;

            _defaultValueGeneratorSwitch
                .CaseWhen(member.Name, createInstance);

            return this;
        }

        public TSelector GetDefaultValue<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty)
        {
            var member  = GetMemberInfo(selectProperty);
            var memberName = member.Name;
            var memberType = member.DeclaringType;
            return (TSelector)GetDefaultValue(memberName, memberType);
        }

        public object GetDefaultValue(string propertyName, Type propertyType)
        {
            if(string.IsNullOrEmpty(propertyName))
                return default;

            return _defaultValueGeneratorSwitch
                .Case(propertyName)?
                .Invoke();
        }

        private MemberInfo GetMemberInfo<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty)
        {
            if(selectProperty.Body is MemberExpression memberExpression)
                return memberExpression.Member;

            return null;
        }
    }
}
