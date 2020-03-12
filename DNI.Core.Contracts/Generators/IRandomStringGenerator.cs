using DNI.Core.Contracts.Enumerations;

namespace DNI.Core.Contracts.Generators
{
    /// <summary>
    /// Represents a secure random string generator
    /// </summary>
    public interface IRandomStringGenerator
    {
        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="characterType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        string GenerateString(CharacterType characterType, int length);
    }
}
