﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Stores
{
    public interface IJsonFileCacheTrackerStore : ICacheTrackerStore
    {
        public IJsonFileCacheTrackerStoreOptions Options { get; }
    }
}
