namespace DNI.Core.Contracts.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRetryHandlerOptions
    {
        int IOExceptionRetryAttempts { get; set; }

        int Timeout { get; }
    }
}
