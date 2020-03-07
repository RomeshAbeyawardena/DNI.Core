using AutoMapper;
using DNI.Core.Web.Domains;
using DNI.Core.Web.ViewModels.Partials;

namespace DNI.Core.Web.ViewModels
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Page, HeaderViewComponentModel>();
            CreateMap<Page, PageViewComponentViewModel>();
            CreateMap<Section, SectionViewComponentModel>();
            CreateMap<StyleSheet, StyleSheetViewModel>();
        }
    }
}
