namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DNI.Core.Contracts.Enumerations;

    public interface ISystemItem
    {
        bool Exists { get; }

        string FullPath { get; }

        string Path { get; }

        string Name { get; }

        SystemItemType Type { get; }
    }
}
