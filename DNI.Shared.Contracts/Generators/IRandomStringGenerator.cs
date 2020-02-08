﻿using DNI.Shared.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Generators
{
    public interface IRandomStringGenerator
    {
        string GenerateString(CharacterType characterType, int length);
    }
}
