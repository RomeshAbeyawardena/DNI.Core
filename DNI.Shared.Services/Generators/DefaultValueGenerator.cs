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
        private readonly ISwitch<string, Func<object>> _defaultValueGeneratorSwitch;
        private readonly ISwitch<string, Func<IServiceProvider, object>> _defaultValueServiceProviderGeneratorSwitch;
        private readonly IServiceProvider _serviceProvider;

        private DefaultValueGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _defaultValueGeneratorSwitch = Switch.Create<string, Func<object>>();
            _defaultValueServiceProviderGeneratorSwitch = Switch.Create<string, Func<IServiceProvider, object>>();
        }

        public static IDefaultValueGenerator<TEntity> Create(IServiceProvider serviceProvider)
        {
            return new DefaultValueGenerator<TEntity>(serviceProvider);
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

        public IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<IServiceProvider, object> createInstance)
        {
            var member = GetMemberInfo(selectProperty);

            if(string.IsNullOrEmpty(member.Name))
                return this;

            _defaultValueServiceProviderGeneratorSwitch
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

            var defaultValueGenerator = _defaultValueGeneratorSwitch
                .Case(propertyName);
            
            if(defaultValueGenerator == null)
                return _defaultValueServiceProviderGeneratorSwitch.Case(propertyName).Invoke(_serviceProvider);

            return defaultValueGenerator.Invoke();
        }

        private MemberInfo GetMemberInfo<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty)
        {
            if(selectProperty.Body is MemberExpression memberExpression)
                return memberExpression.Member;

            return null;
        }
    }
}
