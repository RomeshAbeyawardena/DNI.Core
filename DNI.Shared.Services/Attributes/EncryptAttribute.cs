using DNI.Shared.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EncryptAttribute : Attribute
    {
        public EncryptAttribute(string encryptionSaltKey, EncryptionMethod encryptionMethod, StringCase @case = StringCase.None)
        {
            EncryptionSaltKey = encryptionSaltKey;
            EncryptionMethod = encryptionMethod;
            Case = @case;
        }

        public string EncryptionSaltKey { get; }
        public EncryptionMethod EncryptionMethod { get; }
        public StringCase Case { get; }
    }
}
