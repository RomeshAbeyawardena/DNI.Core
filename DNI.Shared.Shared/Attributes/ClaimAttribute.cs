using System;

namespace DNI.Shared.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ClaimAttribute : Attribute
    {
        public ClaimAttribute(string claimType = null)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }
}
