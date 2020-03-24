namespace DNI.Core.Services.Generators
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Generators;

    internal sealed class DefaultValueGenerator<TEntity> : IDefaultValueGenerator<TEntity>
    {
        private readonly ISwitch<string, Func<object>> defaultValueGeneratorSwitch;
        private readonly ISwitch<string, Func<IServiceProvider, object>> defaultValueServiceProviderGeneratorSwitch;
        private readonly IServiceProvider serviceProvider;

        private DefaultValueGenerator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            defaultValueGeneratorSwitch = Switch.Create<string, Func<object>>();
            defaultValueServiceProviderGeneratorSwitch = Switch.Create<string, Func<IServiceProvider, object>>();
        }

        public static IDefaultValueGenerator<TEntity> Create(IServiceProvider serviceProvider)
        {
            return new DefaultValueGenerator<TEntity>(serviceProvider);
        }

        public IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<object> createInstance)
        {
            var member = GetMemberInfo(selectProperty);

            if (string.IsNullOrEmpty(member.Name))
            {
                return default;
            }

            defaultValueGeneratorSwitch
                .CaseWhen(member.Name, createInstance);

            return this;
        }

        public IDefaultValueGenerator<TEntity> Add<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty, Func<IServiceProvider, object> createInstance)
        {
            var member = GetMemberInfo(selectProperty);

            if (string.IsNullOrEmpty(member.Name))
            {
                return this;
            }

            defaultValueServiceProviderGeneratorSwitch
                .CaseWhen(member.Name, createInstance);

            return this;
        }

        public TSelector GetDefaultValue<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty)
        {
            var member = GetMemberInfo(selectProperty);
            var memberName = member.Name;
            var memberType = member.DeclaringType;
            return (TSelector)GetDefaultValue(memberName, memberType);
        }

        public object GetDefaultValue(string propertyName, Type propertyType)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return default;
            }

            var defaultValueGenerator = defaultValueGeneratorSwitch
                .Case(propertyName);

            if (defaultValueGenerator == null)
            {
                return defaultValueServiceProviderGeneratorSwitch.Case(propertyName).Invoke(serviceProvider);
            }

            return defaultValueGenerator.Invoke();
        }

        private MemberInfo GetMemberInfo<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty)
        {
            if (selectProperty.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member;
            }

            return null;
        }
    }
}
