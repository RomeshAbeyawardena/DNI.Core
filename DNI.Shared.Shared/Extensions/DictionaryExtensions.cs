using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> dictionary, params object[] constructorArguments)
        {
            var objectType = typeof(T);
            var properties = objectType.GetProperties();
            var instance = Activator.CreateInstance(objectType, constructorArguments);

            foreach (var (key, value) in dictionary)
            {
                var property = properties.FirstOrDefault(property => property.Name == key);

                if (property == null)
                    continue;

                property.SetValue(instance, Convert.ChangeType(value, property.PropertyType));
            }

            return (T)instance;
        }

        public static T ToObject<T>(this IDictionary<string, string> dictionary, params object[] constructorArguments)
        {
            var objectDictionary = new Dictionary<string, object>();
            
            foreach (var (key, value) in dictionary)
            {
                object val = value;

                if(long.TryParse(value, out var integerValue))
                    val = integerValue;

                if(decimal.TryParse(value, out var decimalValue))
                    val = decimalValue;

                if(bool.TryParse(value, out var booleanValue))
                    val = booleanValue;

                if(Guid.TryParse(value, out var guidValue))
                    val = guidValue;

                objectDictionary.Add(key, val);
            }

            return ToObject<T>(objectDictionary, constructorArguments);
        }
    }
}
