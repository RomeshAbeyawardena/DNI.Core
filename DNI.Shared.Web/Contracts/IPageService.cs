using DNI.Shared.Web.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNI.Shared.Web.Contracts
{
    public interface IPageService
    {
        Task<Page> GetPage(string pageName, int? parentPageId = null);
        Task<Section> GetPageSection(string pageName, int sectionId, int? parentPageId);
        Task<IEnumerable<Section>> GetPageSections(int sectionId);
        Task<IEnumerable<StyleSheet>> GetStyleSheets(Page page);
    }
}
