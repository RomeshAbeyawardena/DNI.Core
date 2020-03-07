using DNI.Core.Contracts;
using DNI.Core.Contracts.Enumerations;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Attributes;
using DNI.Core.Services.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DNI.Core.Services.Providers
{
    internal sealed class EncryptionProvider : IEncryptionProvider
    {
        private readonly ILogger<EncryptionProvider> _logger;
        private readonly IMapperProvider _mapperProvider;
        private readonly ISwitch<string, ICryptographicCredentials> _cryptographicCredentialsSwitch;
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly IHashingProvider _hashingProvider;

        private IEnumerable<PropertyInfo> GetEncryptableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttribute<EncryptAttribute>() != null);
        }

        
        private async Task<IEnumerable<byte>> Encrypt(EncryptionMethod encryptionMethod,
            ICryptographicCredentials cryptographicCredentials, string value, StringCase @case)
        {
            
            switch (encryptionMethod)
            {
                case EncryptionMethod.Encryption:
                    return await _cryptographyProvider.Encrypt(cryptographicCredentials, value.Case(@case));
                case EncryptionMethod.Hashing:
                    return _hashingProvider.PasswordDerivedBytes(value, cryptographicCredentials.Key, 
                        cryptographicCredentials.KeyDerivationPrf, cryptographicCredentials.Iterations, 
                        cryptographicCredentials.TotalNumberOfBytes);
                default: throw new NotSupportedException();
            };
        }

        public async Task<TResult> Decrypt<T, TResult>(T value)
        {
            var tResultProperties = typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var resultInstance = _mapperProvider.Map<T, TResult>(value);
            foreach (var property in GetEncryptableProperties(typeof(T)))
            {
                var encryptCustomAttribute = property.GetCustomAttribute<EncryptAttribute>();

                if (encryptCustomAttribute.EncryptionMethod == EncryptionMethod.Hashing)
                    continue;

                var cryptographicCredentials = _cryptographicCredentialsSwitch
                    .Case(encryptCustomAttribute.EncryptionSaltKey);

                var resultProperty = tResultProperties
                    .FirstOrDefault(prop => property.Name == prop.Name);

                var val = (IEnumerable<byte>)property.GetValue(value);

                if (val == null)
                    continue;

                resultProperty.SetValue(resultInstance, await _cryptographyProvider.Decrypt(cryptographicCredentials, val));
            }

            return resultInstance;
        }

        public async Task<TResult> Encrypt<T, TResult>(T value)
        {
            var encryptableProperties = GetEncryptableProperties(typeof(T));
            var tResultProperties = typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var resultInstance = _mapperProvider.Map<T, TResult>(value);
            foreach (var property in encryptableProperties)
            {
                var encryptCustomAttribute = property.GetCustomAttribute<EncryptAttribute>();
                var cryptographicCredentials = _cryptographicCredentialsSwitch
                    .Case(encryptCustomAttribute.EncryptionSaltKey);

                var resultProperty = tResultProperties
                    .FirstOrDefault(prop => property.Name == prop.Name);

                var val = property.GetValue(value);

                if(property.PropertyType.IsArray)
                    val = cryptographicCredentials.Encoding.GetString((byte[])val);

                if (val is IEnumerable<byte> enumerableValue)
                    val = cryptographicCredentials.Encoding.GetString(enumerableValue.ToArray());


                if (val == null)
                    continue;

                var encryptedValue = await Encrypt(encryptCustomAttribute.EncryptionMethod,
                    cryptographicCredentials, val.ToString(), encryptCustomAttribute.Case);

                resultProperty.SetValue(resultInstance, encryptedValue);
            }

            return resultInstance;
        }

        public async Task<IEnumerable<TResult>> Encrypt<T, TResult>(IEnumerable<T> value)
        {
            var encryptedList = new List<TResult>();
            foreach (var item in value)
            {
                encryptedList.Add(await Encrypt<T, TResult>(item));
            }

            return encryptedList.ToArray();
        }

        public async Task<IEnumerable<TResult>> Decrypt<T, TResult>(IEnumerable<T> value)
        {
            var decryptedList = new List<TResult>();
            foreach (var item in value)
            {
                decryptedList.Add(await Decrypt<T, TResult>(item));
            }

            return decryptedList.ToArray();
        }

        public EncryptionProvider(ILogger<EncryptionProvider> logger, IMapperProvider mapperProvider,
            ISwitch<string, ICryptographicCredentials> cryptographicCredentialsSwitch,
            ICryptographyProvider cryptographyProvider, IHashingProvider hashingProvider)
        {
            _logger = logger;
            _mapperProvider = mapperProvider;
            _cryptographicCredentialsSwitch = cryptographicCredentialsSwitch;
            _cryptographyProvider = cryptographyProvider;
            _hashingProvider = hashingProvider;
        }
    }
}
