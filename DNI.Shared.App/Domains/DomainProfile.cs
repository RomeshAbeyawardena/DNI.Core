using AutoMapper;

namespace DNI.Shared.App.Domains
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<CustomerDto, Customer>()
                .ForMember(member => member.EmailAddress, options => options.Ignore())
                .ForMember(member => member.FirstName, options => options.Ignore())
                .ForMember(member => member.MiddleName, options => options.Ignore())
                .ForMember(member => member.LastName, options => options.Ignore());

            CreateMap<Customer, CustomerDto>()
                .ForMember(member => member.EmailAddress, options => options.Ignore())
                .ForMember(member => member.FirstName, options => options.Ignore())
                .ForMember(member => member.MiddleName, options => options.Ignore())
                .ForMember(member => member.LastName, options => options.Ignore());
        }
    }
}
