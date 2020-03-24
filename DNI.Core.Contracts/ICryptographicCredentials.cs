namespace DNI.Core.Contracts
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    public interface ICryptographicCredentials
    {
        public IEnumerable<byte> Key { get; set; }

        public IEnumerable<byte> InitialVector { get; set; }

        string SymmetricAlgorithm { get; set; }

        KeyDerivationPrf KeyDerivationPrf { get; set; }

        public int Iterations { get; set; }

        public int TotalNumberOfBytes { get; set; }

        public Encoding Encoding { get; set; }
    }
}
