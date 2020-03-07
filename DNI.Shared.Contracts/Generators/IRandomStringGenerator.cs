using DNI.Shared.Contracts.Enumerations;

namespace DNI.Shared.Contracts.Generators
{
    public interface IRandomStringGenerator
    {
        string GenerateString(CharacterType characterType, int length);
    }
}
