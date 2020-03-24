namespace DNI.Core.Services.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Managers;
    using DNI.Core.Contracts.Providers;
    using DNI.Core.Domains;
    using DNI.Core.Services.Extensions;
    using DNI.Core.Shared.Extensions;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    internal sealed class CryptographyProvider : ICryptographyProvider
    {
        private readonly IMemoryStreamManager memoryStreamManager;
        private readonly IHashingProvider hashingProvider;

        public async Task<string> Decrypt(ICryptographicCredentials cryptographicCredentials, IEnumerable<byte> value)
        {
            return await CreateSymmetricAlgorithm(
                cryptographicCredentials,
                async (symmetricAlgorithm) => await Decrypt(value, symmetricAlgorithm)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<byte>> Encrypt(ICryptographicCredentials cryptographicCredentials, string value)
        {
            return await CreateSymmetricAlgorithm(
                cryptographicCredentials,
                async (symmetricAlgorithm) => await Encrypt(value, symmetricAlgorithm)).ConfigureAwait(false);
        }

        private async Task<T> CreateSymmetricAlgorithm<T>(ICryptographicCredentials cryptographicCredentials, Func<SymmetricAlgorithm, Task<T>> action)
        {
            return await DisposableHelper.UseAsync(
                async (symmetricAlgorithm) => await action(symmetricAlgorithm),
                () => CreateSymmetricAlgorithm(cryptographicCredentials)).ConfigureAwait(false); ;
        }

        private static SymmetricAlgorithm CreateSymmetricAlgorithm(ICryptographicCredentials cryptographicCredentials)
        {
            var symmetricAlgorithm = SymmetricAlgorithm.Create(cryptographicCredentials.SymmetricAlgorithm);

            symmetricAlgorithm.Key = cryptographicCredentials.Key.ToArray();
            symmetricAlgorithm.IV = cryptographicCredentials.InitialVector.ToArray();

            return symmetricAlgorithm;
        }

        private async Task<string> Decrypt(IEnumerable<byte> encryptedData, SymmetricAlgorithm symmetricAlgorithm)
        {
            return await DisposableHelper
                .UseAsync(
                    async (decryptor) => await Decrypt(decryptor, encryptedData),
                    () => symmetricAlgorithm.CreateDecryptor()).ConfigureAwait(false);
        }

        private async Task<string> Decrypt(ICryptoTransform decryptor, IEnumerable<byte> encryptedData)
        {
            using var memoryStream = memoryStreamManager.GetStream(buffer: encryptedData.ToArray());
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(cryptoStream);
            return await srDecrypt.ReadToEndAsync().ConfigureAwait(false); ;
        }

        private async Task<IEnumerable<byte>> Encrypt(string value, SymmetricAlgorithm symmetricAlgorithm)
        {
            return await DisposableHelper.UseAsync(
                async (encryptor) => await Encrypt(encryptor, value),
                () => symmetricAlgorithm.CreateEncryptor()).ConfigureAwait(false);
        }

        private async Task<IEnumerable<byte>> Encrypt(ICryptoTransform decryptor, string plainText)
        {
            var encrypted = Array.Empty<byte>();

            using (var memoryStream = memoryStreamManager.GetStream(false))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                using (var srDecrypt = new StreamWriter(cryptoStream))
                {
                    await srDecrypt.WriteAsync(plainText).ConfigureAwait(false);
                }

                encrypted = memoryStream.ToArray();
            }

            return encrypted;
        }

        public TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(string symmetricAlgorithm, IEnumerable<byte> key, IEnumerable<byte> initialVector, int initialVectorSize = 16) where TCryptographicCredentials : ICryptographicCredentials
        {
            var instance = Activator.CreateInstance<TCryptographicCredentials>();

            instance.SymmetricAlgorithm = symmetricAlgorithm;
            instance.Key = key;

            if (initialVector == null)
            {
                initialVector = hashingProvider.GetRandomNumberGeneratorBytes(initialVectorSize);
            }

            instance.InitialVector = initialVector;

            return instance;
        }

        public TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(KeyDerivationPrf keyDerivationPrf, string password, IEnumerable<byte> salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector) where TCryptographicCredentials : ICryptographicCredentials
        {
            var key = hashingProvider.PasswordDerivedBytes(password, salt, keyDerivationPrf, iterations, totalNumberOfBytes);
            var credentials = GetCryptographicCredentials<TCryptographicCredentials>(Constants.AES, key, initialVector);
            credentials.KeyDerivationPrf = keyDerivationPrf;
            credentials.Iterations = iterations;
            credentials.TotalNumberOfBytes = totalNumberOfBytes;
            return credentials;
        }

        public TCryptographicCredentials GetCryptographicCredentials<TCryptographicCredentials>(KeyDerivationPrf keyDerivationPrf, Encoding encoding, string password, string salt, int iterations, int totalNumberOfBytes, IEnumerable<byte> initialVector) where TCryptographicCredentials : ICryptographicCredentials
        {
            var saltByteArray = salt.GetBytes(encoding);
            var credentials = GetCryptographicCredentials<TCryptographicCredentials>(keyDerivationPrf, password, saltByteArray, iterations, totalNumberOfBytes, initialVector);
            credentials.Encoding = encoding;

            return credentials;
        }

        public CryptographyProvider(IMemoryStreamManager memoryStreamManager, IHashingProvider hashingProvider)
        {
            this.memoryStreamManager = memoryStreamManager;
            this.hashingProvider = hashingProvider;
        }
    }
}
