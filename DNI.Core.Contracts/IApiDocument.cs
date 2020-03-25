namespace DNI.Core.Contracts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IApiDocument
    {
        IDocumentInfo DocumentInfo { get; set; }

        IEnumerable<IServerInfo> Servers { get; set; }

        IEnumerable<IEndPoint> Paths { get; set; }

        string Type { get; set; }

        string Version { get; set; }
    }
}
