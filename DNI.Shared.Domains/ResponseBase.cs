using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Domains
{
    public class ResponseBase
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<ValidationFailure> Errors { get; set; }
    }
}
