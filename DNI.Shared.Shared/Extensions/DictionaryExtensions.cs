using DNI.Shared.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static TClaim ToClaimObject<TClaim>(this IDictionary<string, object> dictionary, params object[] constructorArguments)
        {
            return ToObject<TClaim>(dictionary, typeof(ClaimAttribute), nameof(ClaimAttribute.ClaimType), constructorArguments);
        }

        public static TClaim ToClaimObject<TClaim>(this IDictionary<string, string> dictionary, params object[] constructorArguments)
        {
            return ToObject<TClaim>(dictionary, typeof(ClaimAttribute), nameof(ClaimAttribute.ClaimType), constructorArguments);
        }

        public static T ToObject<T>(this IDictionary<string, object> dictionary, params object[] constructorArguments)
        {
            return ToObject<T>(dictionary, null, null, constructorArguments);
        }

        public static T ToObject<T>(this IDictionary<string, string> dictionary, params object[] constructorArguments)
        {
            return ToObject<T>(dictionary, null, null, constructorArguments);
        }

        public static T ToObject<T>(this IDictionary<string, object> dictionary, Type customAttributeType = null, 
            string attributePropertyOrField = null, params object[] constructorArguments)
        {
            var objectType = typeof(T);
            var properties = objectType.GetProperties();
            var instance = Activator.CreateInstance(objectType, constructorArguments);
            var interfaceTypes = objectType.GetInterfaces();

            foreach (var (key, value) in dictionary)
            {
                var property = properties.FirstOrDefault(prop => prop.Name == key 
                || MatchesCustomAttributeField(prop, customAttributeType, attributePropertyOrField, key)
                || MatchesInheritedInterfacesCustomAttributeField(interfaceTypes, objectType, customAttributeType, attributePropertyOrField, key));

                if (property == null)
                    continue;

                property.SetValue(instance, Convert.ChangeType(value, property.PropertyType));
            }

            return (T)instance;
        }

        public static T ToObject<T>(this IDictionary<string, string> dictionary, Type customAttributeType = null,
            string attributePropertyOrField = null, params object[] constructorArguments)
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

            return ToObject<T>(objectDictionary, customAttributeType, attributePropertyOrField, constructorArguments);
        }

        public static bool MatchesInheritedInterfacesCustomAttributeField(IEnumerable<Type> interfaceTypes, Type type, Type customAttributeType, string propertyOrFieldName, string value)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                var properties = interfaceType.GetProperties();

                if(properties.Any(property => MatchesCustomAttributeField(property, customAttributeType, propertyOrFieldName, value)))
                    return true;
            }

            return false;
        }

        public static bool MatchesCustomAttributeField(PropertyInfo property, Type customAttributeType, string propertyOrFieldName, string value)
        {
            var propertyCustomAttribute = (customAttributeType == null
                    ? default
                    : property.GetCustomAttributes(customAttributeType, true))?.SingleOrDefault();


            if (propertyCustomAttribute == null)
                return false;

            var attributeProperty = customAttributeType.GetProperty(propertyOrFieldName);
            

            if(attributeProperty == null)
                return false;

            var propertyName = attributeProperty.GetValue(propertyCustomAttribute);

            return propertyName.Equals(value);
        }
    }
}
