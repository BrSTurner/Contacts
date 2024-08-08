using AutoMapper;
using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.SharedKernel.DomainObjects;

namespace FIAP.Contacts.Application.Contacts.Pofiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<CreateContactInput, Contact>()
                .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("email", opt => opt.MapFrom(src => new Email(src.Email)))
                .ForCtorParam("phoneNumber", opt => opt.MapFrom(src => new PhoneNumber(src.PhoneCode, src.PhoneNumber)));

            CreateMap<UpdateContactInput, Contact>()
                .ForCtorParam("name", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("email", opt => opt.MapFrom(src => new Email(src.Email)))
                .ForCtorParam("phoneNumber", opt => opt.MapFrom(src => new PhoneNumber(src.PhoneCode, src.PhoneNumber)));

            CreateMap<Contact, ContactDTO>()
              .ForMember(e => e.Email, opt => opt.MapFrom(src => src.Email.Address))
              .ForMember(e => e.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Number))
              .ForMember(e => e.PhoneCode, opt => opt.MapFrom(src => src.PhoneNumber.Code));
        }
    }
}
