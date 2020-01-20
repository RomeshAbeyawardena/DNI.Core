using DNI.Shared.Web.Domains;
using System.Threading.Tasks;

namespace DNI.Shared.Web.Contracts
{
    public interface IPageService
    {
        Task<Page> GetPage(string pageName, int? parentPageId = null);
        Task<Section> GetPageSection(string pageName, int sectionId, int? parentPageId);
    }
}
