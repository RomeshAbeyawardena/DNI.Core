using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DNI.Shared.App.Domains
{
    public class CustomerDto
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue]
        public Guid UniqueId { get; set; }

        [Encrypt(Constants.IdentifierDataEncryption)]
        public string EmailAddress { get; set; }

        [Encrypt(Constants.PersonalDataEncryption)]
        public string FirstName { get; set; }

        [Encrypt(Constants.PersonalDataEncryption)]
        public string MiddleName { get; set; }

        [Encrypt(Constants.PersonalDataEncryption)]
        public string LastName { get; set; }
        
        [Modifier(ModifierFlag.Created)]
        public DateTime? Created { get; set; }

        [Modifier(ModifierFlag.Created | ModifierFlag.Modified)]
        public DateTimeOffset? Modified { get; set; }
    }
}
