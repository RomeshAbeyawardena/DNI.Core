using DNI.Shared.App.Contracts;
using DNI.Shared.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.App.Domains
{
    public class Data : ICustomerReference
    {
        public Guid UniqueReference { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public double Height { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string CustomerReference { get; set; }
    }
}
