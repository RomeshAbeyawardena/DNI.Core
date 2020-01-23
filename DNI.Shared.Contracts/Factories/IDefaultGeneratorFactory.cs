using DNI.Shared.Contracts.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Factories
{
    public interface IDefaultGeneratorFactory
    {
        IDefaultValueGenerator<TEntity> GetDefaultValueGenerator<TEntity>();
    }
}
