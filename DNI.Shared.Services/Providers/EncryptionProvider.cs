using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Services.Attributes;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Providers
{
    public class EncryptionProvider : IEncryptionProvider
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
            ICryptographicCredentials cryptographicCredentials, string value)
        {
            
            switch (encryptionMethod)
            {
                case EncryptionMethod.Encryption:
                    return await _cryptographyProvider.Encrypt(cryptographicCredentials, value);
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
                    cryptographicCredentials, val.ToString());

                resultProperty.SetValue(resultInstance, encryptedValue);
            }

            return resultInstance;
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
