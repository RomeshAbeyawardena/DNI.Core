using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Domains;
using DNI.Shared.Services.Extensions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
        private readonly IHashingProvider _hashingProvider;

        public async Task<string> Decrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> value)
        {
            return await CreateSymmetricAlgorithm(cryptographicCredentials, 
                async(symmetricAlgorithm) => await Decrypt(value, symmetricAlgorithm));
        }

        public async Task<IEnumerable<byte>> Encrypt(ICryptographicCredentials cryptographicCredentials, string value)
        {
            return await CreateSymmetricAlgorithm(cryptographicCredentials, 
                async(symmetricAlgorithm) => await Encrypt(value, symmetricAlgorithm));
        }

        private async Task<T> CreateSymmetricAlgorithm<T>(ICryptographicCredentials cryptographicCredentials, Func<SymmetricAlgorithm, Task<T>> action)
        {
            return await DisposableHelper.UseAsync(async(symmetricAlgorithm) => await action(symmetricAlgorithm), 
                () => CreateSymmetricAlgorithm(cryptographicCredentials));
        }

        private static SymmetricAlgorithm CreateSymmetricAlgorithm(ICryptographicCredentials cryptographicCredentials)
        {
            var symmetricAlgorithm = SymmetricAlgorithm.Create(cryptographicCredentials.SymmetricAlgorithm);

            symmetricAlgorithm.Key = cryptographicCredentials.Key.ToArray();
            symmetricAlgorithm.IV = cryptographicCredentials.InitialVector.ToArray();

            return symmetricAlgorithm;
        }

        private static async Task<string> Decrypt(IEnumerable<byte> encryptedData, SymmetricAlgorithm symmetricAlgorithm)
        {
            return await DisposableHelper
                .UseAsync(async(decryptor) => await Decrypt(decryptor, encryptedData), 
                    () => symmetricAlgorithm.CreateDecryptor());
        }

        private static async Task<string> Decrypt(ICryptoTransform decryptor, IEnumerable<byte> encryptedData)
        {
            using (var memoryStream = new MemoryStream(encryptedData.ToArray()))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using(var srDecrypt = new StreamReader(cryptoStream))
                        return await srDecrypt.ReadToEndAsync();
        }

        private static async Task<IEnumerable<byte>> Encrypt(string value, SymmetricAlgorithm symmetricAlgorithm)
        {
            return await DisposableHelper.UseAsync(async(encryptor) => await Encrypt(encryptor, value), 
                () => symmetricAlgorithm.CreateEncryptor());
        }

        private static async Task<IEnumerable<byte>> Encrypt(ICryptoTransform decryptor, string plainText)
        {
            var encrypted = Array.Empty<byte>();

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    using(var srDecrypt = new StreamWriter(cryptoStream))
                        await srDecrypt.WriteAsync(plainText);
                encrypted = memoryStream.ToArray();
            }

            return encrypted;
        }

        public TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(string symmetricAlgorithm, IEnumerable<byte> key, IEnumerable<byte> initialVector, int initialVectorSize = 16) where TCryptographicCredentials : ICryptographicCredentials
        {
            var instance = Activator.CreateInstance<TCryptographicCredentials>();

            instance.SymmetricAlgorithm = symmetricAlgorithm;
            instance.Key = key;
            
            if(initialVector == null)
                initialVector = _hashingProvider.GetRandomNumberGeneratorBytes(initialVectorSize);
            
            instance.InitialVector = initialVector;

            return instance;
        }

        public TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(KeyDerivationPrf keyDerivationPrf, string password, IEnumerable<byte> salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector) where TCryptographicCredentials : ICryptographicCredentials
        {
            var key = _hashingProvider.PasswordDerivedBytes(password, salt, keyDerivationPrf, iterations, totalNumberOfBytes);
            return GetCryptographicCredentials<TCryptographicCredentials>(Constants.AES, key, initialVector);
        }

        public CryptographyProvider(IHashingProvider hashingProvider)
        {
            _hashingProvider = hashingProvider;
        }
    }
}
