namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IJsonSerializer
    {
        string Serialize<T>(T value);

        T Deserialize<T>(string value);
    }
}
