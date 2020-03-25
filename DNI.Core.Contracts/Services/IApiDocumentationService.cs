namespace DNI.Core.Contracts.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IApiDocumentationService
    {
        ILicense CreateLicense(string licenseName);

        IDocumentInfo GetDocumentInfo(string documentVersion, string documentTitle, ILicense license);

        IApiDocument CreateDocument(string type, string version);
    }
}
