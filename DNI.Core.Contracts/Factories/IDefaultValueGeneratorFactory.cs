using DNI.Core.Contracts.Generators;

namespace DNI.Core.Contracts.Factories
{
    public interface IDefaultValueGeneratorFactory
    {
        IDefaultValueGenerator<TEntity> GetDefaultValueGenerator<TEntity>();
    }
}
