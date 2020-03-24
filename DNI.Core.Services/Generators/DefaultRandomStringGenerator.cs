namespace DNI.Core.Services.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using DNI.Core.Contracts;
    using DNI.Core.Contracts.Enumerations;
    using DNI.Core.Contracts.Generators;
    using DNI.Core.Domains;

    internal sealed class DefaultRandomStringGenerator : IRandomStringGenerator
    {
        private readonly RandomNumberGenerator randomNumberGenerator;
        private readonly ISwitch<CharacterType, Domains.Range> rangeSwitch;

        public DefaultRandomStringGenerator(RandomNumberGenerator randomNumberGenerator, ISwitch<CharacterType, Domains.Range> rangeSwitch)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            this.rangeSwitch = rangeSwitch;
        }

        public string GenerateString(CharacterType characterType, int length)
        {
            return string.Concat(GetCharacters(characterType, length));
        }

        private IEnumerable<byte> GetNumberSequence(int length)
        {
            var buffer = new byte[256 * length];

            randomNumberGenerator.GetBytes(buffer);

            return buffer;
        }

        private IEnumerable<char> GetCharacters(CharacterType characterType, int length)
        {
            var ranges = GetCharacterRanges(characterType).ToArray();
            IEnumerable<byte> rangeSequence = Array.Empty<byte>();

            foreach (Range<byte> range in ranges)
            {
                rangeSequence = rangeSequence.Concat(range.ToSequence());
            }

            return GetNumberSequence(ranges.Length)
                .Where(index => rangeSequence
                .Any(rangeIndex => rangeIndex.Equals(index)))
                .Reverse()
                .Take(length)
                .Select(b => (char)b);
        }

        private IEnumerable<Range<byte>> GetCharacterRanges(CharacterType characterType)
        {
            IEnumerable<Range<byte>> characterRangeList = Array.Empty<Range<byte>>();
            if (characterType.HasFlag(CharacterType.Lowercase))
            {
                characterRangeList = characterRangeList.Append(rangeSwitch.Case(CharacterType.Lowercase));
            }

            if (characterType.HasFlag(CharacterType.Uppercase))
            {
                characterRangeList = characterRangeList.Append(rangeSwitch.Case(CharacterType.Uppercase));
            }

            if (characterType.HasFlag(CharacterType.Numerics))
            {
                characterRangeList = characterRangeList.Append(rangeSwitch.Case(CharacterType.Numerics));
            }

            if (characterType.HasFlag(CharacterType.Symbols))
            {
                characterRangeList = characterRangeList.Append(rangeSwitch.Case(CharacterType.Symbols));
            }

            return characterRangeList;
        }
    }
}
