using DNI.Shared.Contracts.Generators;

namespace DNI.Shared.Contracts.Factories
{
    public interface IDefaultValueGeneratorFactory
    {
        IDefaultValueGenerator<TEntity> GetDefaultValueGenerator<TEntity>();
    }
}
