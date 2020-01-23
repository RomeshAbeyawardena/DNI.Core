using DNI.Shared.Domains.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
