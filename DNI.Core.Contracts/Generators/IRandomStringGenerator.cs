using DNI.Core.Contracts.Enumerations;

namespace DNI.Core.Contracts.Generators
{
    public interface IRandomStringGenerator
    {
        string GenerateString(CharacterType characterType, int length);
    }
}
