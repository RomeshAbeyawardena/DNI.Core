using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Options
{
    public interface IRetryHandlerOptions
    {
        int IOExceptionRetryAttempts { get; set; }
        int Timeout { get; }
    }
}
