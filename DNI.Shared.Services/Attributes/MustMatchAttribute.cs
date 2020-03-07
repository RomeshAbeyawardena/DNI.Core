using System;
using System.ComponentModel.DataAnnotations;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MustMatchAttribute : ValidationAttribute
    {
        public MustMatchAttribute(string matchingMember, string displayMatchingMember = null)
        {
            MatchingMember = matchingMember;
            DisplayMatchingMember = displayMatchingMember ?? MatchingMember;
        }

        public string MatchingMember { get; }
        public string DisplayMatchingMember { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(MatchingMember);

            if(property == null)
                return new ValidationResult($"Property {MatchingMember} does not exist in context");

            var matchingMemberValue = property.GetValue(validationContext.ObjectInstance);

            if((value == null && matchingMemberValue == null) || (value != null && value.Equals(matchingMemberValue)))
                return ValidationResult.Success;

            return new ValidationResult($"'{DisplayMatchingMember}' does not match '{ validationContext.DisplayName }'"); ;
        }
    }
}
