using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.SharedKernel.DomainObjects;

namespace FIAP.Contacts.Domain.Contacts.Services
{
    public interface IContactService
    {
        Task<Guid> CreateAsync(Contact contact);
        Task<Contact> UpdateAsync(Guid contactId, Contact contact);
        Task<bool> DeleteAsync(Guid contactId);
    }
}
