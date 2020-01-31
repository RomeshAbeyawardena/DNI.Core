using System;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ClaimAttribute : Attribute
    {
        public ClaimAttribute(string claimType = null)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }
}
