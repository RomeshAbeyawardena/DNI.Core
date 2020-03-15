using DNI.Core.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Attributes
{
    public class OptionalAttribute : ValidationAttribute
    {
        private IEnumerable<PropertyInfo> GetMembers(Type type, IEnumerable<string> optionalMembers)
        {
            return optionalMembers.ForEach(member => { return type.GetProperty(member); });
        }

        public OptionalAttribute(params string[] optionalMembers)
        {
            OptionalMembers = optionalMembers;
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }

        public override bool RequiresValidationContext => true;
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!value.IsDefault())
                return ValidationResult.Success;

            var members = GetMembers(validationContext.ObjectType, OptionalMembers);

            foreach(var member in members)
                if(!member.GetValue(validationContext.ObjectInstance).IsDefault())
                    return ValidationResult.Success;

            return new ValidationResult("A value is required.", 
                OptionalMembers.Append(validationContext.MemberName));
        }

        public IEnumerable<string> OptionalMembers { get; }
    }
}
