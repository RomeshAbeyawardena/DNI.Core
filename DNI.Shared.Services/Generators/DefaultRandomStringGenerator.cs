using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DNI.Shared.Contracts.Generators;
using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts;

namespace DNI.Shared.Services.Generators
{
    internal sealed class DefaultRandomStringGenerator : IRandomStringGenerator
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly ISwitch<CharacterType, Range> _rangeSwitch;
        public DefaultRandomStringGenerator(RandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _rangeSwitch = Switch.Create<CharacterType, Range>()
                .CaseWhen(CharacterType.Lowercase, new Range(97, 122))
                .CaseWhen(CharacterType.Uppercase, new Range(65, 90))
                .CaseWhen(CharacterType.Numerics, new Range(48, 57))
                .CaseWhen(CharacterType.Symbols, new Range(33, 47));
        }

        public async Task<string> GenerateString(CharacterType characterType, int length)
        {
            
            return string.Concat(await GetCharacters(characterType, length));
        }

        private async Task<IEnumerable<char>> GetCharacters(CharacterType characterType, int length)
        {
            var range = GetCharacterRange(characterType);
            var rangeSequence = GetSequence(ref range);
            var buffer = new byte[1024];

            _randomNumberGenerator.GetBytes(buffer);

            return await Task.FromResult(buffer
                .Where(b => rangeSequence.Any(r => r.Equals(b)))
                .Take(length).Select(b => (char)b));
        }

        private IEnumerable<int> GetSequence(ref Range range)
        {
            var rangeList = new List<int>();
            for(var index = range.Start.Value; index <= range.End.Value; index++)
                rangeList.Add(index);

            return rangeList.ToArray();
        }

        private Range GetCharacterRange(CharacterType characterType)
        {
            return _rangeSwitch.Case(characterType);
        }
    }
}
