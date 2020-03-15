using DNI.Core.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ISystemItem
    {
        bool Exists { get; }
        string FullPath { get; }
        string Path { get; }
        string Name { get; }
        SystemItemType Type { get; }
    }
}
