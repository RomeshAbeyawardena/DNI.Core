using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public static class Try
    {
        public static ITry Create()
        {
            return DefaultTry.Create();
        }

        public static ITry<TResult> Create<TResult>()
        {
            return DefaultTry<TResult>.Create();
        }
    }
}
