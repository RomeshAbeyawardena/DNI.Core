namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDocumentInfo
    {
        string Version { get; set; }

        string Title { get; set; }

        ILicense License { get; set; }
    }
}
