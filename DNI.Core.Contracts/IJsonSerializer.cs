using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string value);
    }
}
