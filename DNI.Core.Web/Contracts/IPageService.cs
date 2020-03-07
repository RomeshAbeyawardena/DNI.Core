using DNI.Core.Web.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNI.Core.Web.Contracts
{
    public interface IPageService
    {
        Task<Page> GetPage(string pageName, int? parentPageId = null);
        Task<Section> GetPageSection(string pageName, int sectionId, int? parentPageId);
        Task<IEnumerable<Section>> GetPageSections(int sectionId);
        Task<IEnumerable<StyleSheet>> GetStyleSheets(Page page);
    }
}
