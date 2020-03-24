namespace DNI.Core.Contracts.Services
{
    using DNI.Core.Contracts.Enumerations;

    public interface IModifierFlagPropertyService
    {
        void SetModifierFlagValues<TEntity>(TEntity entity, ModifierFlag modifierFlag);
    }
}
