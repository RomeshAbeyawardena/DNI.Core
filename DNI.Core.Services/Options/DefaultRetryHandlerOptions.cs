namespace DNI.Core.Services.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DNI.Core.Contracts.Options;

    public class DefaultRetryHandlerOptions : IRetryHandlerOptions
    {
        public int IOExceptionRetryAttempts { get; set; }

        public int Timeout { get; set; }
    }
}
