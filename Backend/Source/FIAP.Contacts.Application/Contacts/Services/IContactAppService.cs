using FIAP.Contacts.Application.Contacts.Models;

namespace FIAP.Contacts.Application.Contacts.Services
{
    public interface IContactAppService
    {
        Task<Guid> CreateAsync(ContactInput contact);
        Task<ContactDTO> UpdateAsync(Guid contactId, ContactInput contact);
        Task<bool> DeleteAsync(Guid contactId);
    }
}
