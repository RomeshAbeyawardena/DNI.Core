namespace DNI.Core.Services.Attributes
{
    using System;
    using DNI.Core.Contracts.Enumerations;

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
