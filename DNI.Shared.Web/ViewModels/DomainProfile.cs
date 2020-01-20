using AutoMapper;
using DNI.Shared.Web.Domains;

namespace DNI.Shared.Web.ViewModels
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Page, HeaderViewComponentModel>();
            CreateMap<Page, PageViewComponentViewModel>();
            CreateMap<Section, SectionViewComponentModel>();
        }
    }
}
