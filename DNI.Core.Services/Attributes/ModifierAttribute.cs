namespace DNI.Core.Services.Attributes
{
    using System;
    using DNI.Core.Contracts.Enumerations;

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
