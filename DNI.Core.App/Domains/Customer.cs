using DNI.Core.Contracts.Enumerations;
using DNI.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DNI.Core.App.Domains
{
#pragma warning disable CA1819
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue]
        public Guid UniqueId { get; set; }

        [Encrypt(Constants.IdentifierDataEncryption,
            EncryptionMethod.Encryption)]
        public byte[] EmailAddress { get; set; }

        [Encrypt(Constants.PersonalDataEncryption,
            EncryptionMethod.Encryption)]
        public byte[] FirstName { get; set; }

        [Encrypt(Constants.PersonalDataEncryption,
            EncryptionMethod.Encryption)]
        public byte[] MiddleName { get; set; }

        [Encrypt(Constants.PersonalDataEncryption,
            EncryptionMethod.Encryption)]
        public byte[] LastName { get; set; }

        [Encrypt(Constants.PersonalDataEncryption,
            EncryptionMethod.Hashing)]
        public byte[] Password { get; set; }


        [Modifier(ModifierFlag.Created)]
        public DateTime? Created { get; set; }

        [Modifier(ModifierFlag.Created | ModifierFlag.Modified)]
        public DateTimeOffset? Modified { get; set; }
    }
}
