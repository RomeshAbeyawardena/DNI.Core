using DNI.Shared.Domains.Enumerations;
using DNI.Shared.Services.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.App.Domains
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        
        [Modifier(ModifierFlag.Created)]
        public DateTime Created { get; set; }

        [Modifier(ModifierFlag.Modified)]
        public DateTimeOffset Modified { get; set; }
    }
}
