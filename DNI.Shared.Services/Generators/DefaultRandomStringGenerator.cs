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
                .CaseWhen(CharacterType.Lowercase, new Range());
        }

        public Task<string> GenerateString(CharacterType characterType)
        {
            
        }

        private Range GetCharacter(CharacterType characterType)
        {
            return _rangeSwitch.Case(characterType);
        }
    }
}
