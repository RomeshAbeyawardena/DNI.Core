using DNI.Core.Contracts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Options
{
    public class DefaultRetryHandlerOptions : IRetryHandlerOptions
    {
        public int IOExceptionRetryAttempts { get; set; }

        public int Timeout { get; set; }
    }
}
