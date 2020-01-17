using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Services.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Providers
{
    public class CryptographyProvider : ICryptographyProvider
    {
        public string Decrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> key, IEnumerable<byte> value)
        {
            return DisposableHelper.Use((symmetricAlgorithm) => Decrypt(value, symmetricAlgorithm, cryptographicCredentials), 
                () => CreateSymmetricAlgorithm(cryptographicCredentials));
        }

        public IEnumerable<byte> Encrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> key, string value)
        {
            return DisposableHelper.Use((symmetricAlgorithm) => Encrypt(symmetricAlgorithm, cryptographicCredentials), 
                () => CreateSymmetricAlgorithm(cryptographicCredentials));
        }

        private static SymmetricAlgorithm CreateSymmetricAlgorithm(ICryptographicCredentials cryptographicCredentials)
        {
            var symmetricAlgorithm = SymmetricAlgorithm.Create(cryptographicCredentials.SymmetricAlgorithm);

            symmetricAlgorithm.Key = cryptographicCredentials.Key.ToArray();
            symmetricAlgorithm.IV = cryptographicCredentials.InitialVector.ToArray();

            return symmetricAlgorithm;
        }

        private static string Decrypt(IEnumerable<byte> encryptedData, SymmetricAlgorithm symmetricAlgorithm, ICryptographicCredentials cryptographicCredentials)
        {
            return DisposableHelper.Use((decryptor) => Decrypt(decryptor, encryptedData), () => symmetricAlgorithm.CreateDecryptor());
        }

        private static string Decrypt(ICryptoTransform decryptor, IEnumerable<byte> encryptedData)
        {
            return DisposableHelper.Use<string, MemoryStream>(memoryStream => )
        }

        private static IEnumerable<byte> Encrypt(SymmetricAlgorithm symmetricAlgorithm, ICryptographicCredentials cryptographicCredentials)
        {
            var decryptor = symmetricAlgorithm.CreateEncryptor();
            return Array.Empty<byte>();
        }
    }
}
