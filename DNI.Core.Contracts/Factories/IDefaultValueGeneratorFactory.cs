namespace DNI.Core.Contracts.Factories
{
    using DNI.Core.Contracts.Generators;

    public interface IDefaultValueGeneratorFactory
    {
        IDefaultValueGenerator<TEntity> GetDefaultValueGenerator<TEntity>();
    }
}
