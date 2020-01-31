using DNI.Shared.Contracts.Enumerations;

namespace DNI.Shared.Contracts.Services
{
    public interface IModifierFlagPropertyService
    {
        void SetModifierFlagValues<TEntity>(TEntity entity, ModifierFlag modifierFlag);
    }
}
