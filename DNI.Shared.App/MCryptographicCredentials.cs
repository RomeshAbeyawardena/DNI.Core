using DNI.Shared.Contracts;
using System.Collections.Generic;

namespace DNI.Shared.App
{
    public class MCryptographicCredentials : ICryptographicCredentials
    {
        public IEnumerable<byte> Key { get; set; }
        public IEnumerable<byte> InitialVector { get; set; }
        public string SymmetricAlgorithm { get; set; }
    }
}
