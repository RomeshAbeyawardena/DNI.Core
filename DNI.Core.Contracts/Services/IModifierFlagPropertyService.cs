using DNI.Core.Contracts.Enumerations;

namespace DNI.Core.Contracts.Services
{
    public interface IModifierFlagPropertyService
    {
        void SetModifierFlagValues<TEntity>(TEntity entity, ModifierFlag modifierFlag);
    }
}
