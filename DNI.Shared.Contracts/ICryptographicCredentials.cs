using System.Collections.Generic;

namespace DNI.Shared.Contracts
{
    public interface ICryptographicCredentials
    {
        public IEnumerable<byte> Key { get; set; }
        public IEnumerable<byte> InitialVector { get; set; }
        string SymmetricAlgorithm { get; set; }
    }
}
