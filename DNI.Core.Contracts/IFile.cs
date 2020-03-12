using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IFile : IDisposable
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public FileStream GetFileStream();
    }
}
