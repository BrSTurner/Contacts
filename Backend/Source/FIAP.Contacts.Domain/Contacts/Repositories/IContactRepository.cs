using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.SharedKernel.DomainObjects;
using FIAP.Contacts.SharedKernel.Repositories;

namespace FIAP.Contacts.Domain.Contacts.Repositories
{
    public interface IContactRepository : IBaseRepository<Contact>
    {
        Task<Contact?> GetByEmailOrPhoneNumber(Email email, PhoneNumber phoneNumber);
    }
}
