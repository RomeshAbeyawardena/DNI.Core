using DNI.Core.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Text;

namespace DNI.Core.App
{
    public class MCryptographicCredentials : ICryptographicCredentials
    {
        public IEnumerable<byte> Key { get; set; }
        public IEnumerable<byte> InitialVector { get; set; }
        public string SymmetricAlgorithm { get; set; }
        public KeyDerivationPrf KeyDerivationPrf { get; set; }
        public int Iterations { get; set; }
        public int TotalNumberOfBytes { get; set; }
        public Encoding Encoding { get; set; }
    }
}
