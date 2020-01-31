using DNI.Shared.Contracts.Enumerations;
using System;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ModifierAttribute : Attribute
    {
        public ModifierAttribute(ModifierFlag modifierFlag)
        {
            ModifierFlag = modifierFlag;
        }

        public ModifierFlag ModifierFlag { get; }
    }
}
