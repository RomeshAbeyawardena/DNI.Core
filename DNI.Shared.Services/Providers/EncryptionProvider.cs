using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Services.Attributes;
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
        private readonly IMapperProvider _mapperProvider;
        private readonly ISwitch<string, ICryptographicCredentials> _cryptographicCredentialsSwitch;
        private readonly ICryptographyProvider _cryptographyProvider;

        private IEnumerable<PropertyInfo> GetEncryptableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttribute<EncryptAttribute>() != null);
        }

        public TResult Decrypt<T, TResult>(T value)
        {
            var tResultProperties = typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var resultInstance = _mapperProvider.Map<T, TResult>(value);
            foreach(var property in GetEncryptableProperties(typeof(T)))
            {
                var encryptCustomAttribute = property.GetCustomAttribute<EncryptAttribute>();
                var cryptographicCredentials = _cryptographicCredentialsSwitch
                    .Case(encryptCustomAttribute.EncryptionSaltKey);

                var resultProperty = tResultProperties
                    .FirstOrDefault(prop => property.Name == prop.Name); 

                var val = (IEnumerable<byte>)property.GetValue(value);
                
                if(val == null)
                    continue;

                resultProperty.SetValue(resultInstance, _cryptographyProvider.Decrypt(cryptographicCredentials, val));
            }

            return resultInstance;
        }

        public TResult Encrypt<T, TResult>(T value)
        {
            var tResultProperties = typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var resultInstance = _mapperProvider.Map<T, TResult>(value);
            foreach(var property in GetEncryptableProperties(typeof(T)))
            {
                var encryptCustomAttribute = property.GetCustomAttribute<EncryptAttribute>();
                var cryptographicCredentials = _cryptographicCredentialsSwitch
                    .Case(encryptCustomAttribute.EncryptionSaltKey);

                var resultProperty = tResultProperties
                    .FirstOrDefault(prop => property.Name == prop.Name); 

                var val = property.GetValue(value)?.ToString();
                
                if(val == null)
                    continue;

                resultProperty.SetValue(resultInstance, _cryptographyProvider.Encrypt(cryptographicCredentials, val));
            }

            return resultInstance;
        }

        public EncryptionProvider(IMapperProvider mapperProvider,
            ISwitch<string, ICryptographicCredentials> cryptographicCredentialsSwitch, 
            ICryptographyProvider cryptographyProvider)
        {
            _mapperProvider = mapperProvider;
            _cryptographicCredentialsSwitch = cryptographicCredentialsSwitch;
            _cryptographyProvider = cryptographyProvider;
        }
    }
}
