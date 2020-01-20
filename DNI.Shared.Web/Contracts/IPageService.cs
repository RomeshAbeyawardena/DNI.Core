using DNI.Shared.Web.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.Contracts
{
    public interface IPageService
    {
        Task<Page> GetPage(int pageId, int? parentPageId = null);
        Task<Section> GetPageSection(int pageId, int sectionId, int? parentPageId);
    }
}
