﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface ICryptographicCredentials
    {
        public IEnumerable<byte> Key { get; set; }
        public IEnumerable<byte> InitialVector { get; set; }
        string SymmetricAlgorithm { get; set; }
    }
}
