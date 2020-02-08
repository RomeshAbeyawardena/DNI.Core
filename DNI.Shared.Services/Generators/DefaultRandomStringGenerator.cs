using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNI.Shared.Domains;
using System.Security.Cryptography;
using DNI.Shared.Contracts.Generators;
using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts;

namespace DNI.Shared.Services.Generators
{
    internal sealed class DefaultRandomStringGenerator : IRandomStringGenerator
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly ISwitch<CharacterType, Domains.Range> _rangeSwitch;
        public DefaultRandomStringGenerator(RandomNumberGenerator randomNumberGenerator, ISwitch<CharacterType, Domains.Range> rangeSwitch)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _rangeSwitch = rangeSwitch;
        }

        public string GenerateString(CharacterType characterType, int length)
        {
            return string.Concat(GetCharacters(characterType, length));
        }

        private IEnumerable<byte> GetNumberSequence(int length)
        {
            var buffer = new byte[256 * length];

            _randomNumberGenerator.GetBytes(buffer); 

            return buffer;
        }

        private IEnumerable<char> GetCharacters(CharacterType characterType, int length)
        {
            var ranges = GetCharacterRanges(characterType).ToArray();
            IEnumerable<byte> rangeSequence = Array.Empty<byte>();

            foreach(Range<byte> range in ranges)
                rangeSequence = rangeSequence.Concat(range.ToSequence());
            
            return GetNumberSequence(ranges.Length)
                .Where(index => rangeSequence
                .Any(rangeIndex => rangeIndex.Equals(index)))
                .Take(length)
                .Select(b => (char)b);
        }

        private IEnumerable<Range<byte>> GetCharacterRanges(CharacterType characterType)
        {
            IEnumerable<Range<byte>> characterRangeList = Array.Empty<Range<byte>>();
            if (characterType.HasFlag(CharacterType.Lowercase))
                characterRangeList = characterRangeList.Append(_rangeSwitch.Case(CharacterType.Lowercase));

            if (characterType.HasFlag(CharacterType.Uppercase))
                characterRangeList = characterRangeList.Append(_rangeSwitch.Case(CharacterType.Uppercase));

            if (characterType.HasFlag(CharacterType.Numerics))
                characterRangeList = characterRangeList.Append(_rangeSwitch.Case(CharacterType.Numerics));

            if (characterType.HasFlag(CharacterType.Symbols))
                characterRangeList= characterRangeList.Append(_rangeSwitch.Case(CharacterType.Symbols));

            return characterRangeList;
        }
    }
}
