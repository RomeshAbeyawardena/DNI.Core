using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DNI.Shared.App.Domains
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue]
        public Guid UniqueId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        
        [Modifier(ModifierFlag.Created)]
        public DateTime? Created { get; set; }

        [Modifier(ModifierFlag.Created | ModifierFlag.Modified)]
        public DateTimeOffset? Modified { get; set; }
    }
}
