using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EncryptAttribute : Attribute
    {
        public EncryptAttribute(string encryptionSaltKey)
        {
            EncryptionSaltKey = encryptionSaltKey;
        }

        public string EncryptionSaltKey { get; }
    }
}
