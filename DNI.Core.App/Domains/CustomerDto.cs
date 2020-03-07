using DNI.Core.Contracts.Enumerations;
using DNI.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DNI.Core.App.Domains
{
    public class CustomerDto
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue]
        public Guid UniqueId { get; set; }

        [Encrypt(Constants.IdentifierDataEncryption, EncryptionMethod.Encryption)]
        public string EmailAddress { get; set; }

        [Encrypt(Constants.PersonalDataEncryption, EncryptionMethod.Encryption)]
        public string FirstName { get; set; }

        [Encrypt(Constants.PersonalDataEncryption, EncryptionMethod.Encryption)]
        public string MiddleName { get; set; }

        [Encrypt(Constants.PersonalDataEncryption, EncryptionMethod.Encryption)]
        public string LastName { get; set; }
        
        [Encrypt(Constants.PersonalDataEncryption, EncryptionMethod.Hashing)]
        public byte[] Password { get; set; }

        [Modifier(ModifierFlag.Created)]
        public DateTime? Created { get; set; }

        [Modifier(ModifierFlag.Created | ModifierFlag.Modified)]
        public DateTimeOffset? Modified { get; set; }
    }
}
