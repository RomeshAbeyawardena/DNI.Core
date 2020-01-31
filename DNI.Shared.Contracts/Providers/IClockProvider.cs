using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Providers
{
    public interface IClockProvider
    {
        DateTimeOffset DateTimeOffset { get; }
        DateTime UtcDateTime { get; }
        DateTime DateTime { get; }
    }
}
