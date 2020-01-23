﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Generators
{
    public interface IDefaultValueGenerator<TEntity>
    {
        TSelector GetDefaultValue<TSelector>(Expression<Func<TEntity, TSelector>> selectProperty);
    }
}
