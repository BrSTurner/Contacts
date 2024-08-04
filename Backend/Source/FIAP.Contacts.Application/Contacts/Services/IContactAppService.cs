using FIAP.Contacts.Application.Contacts.Models;

namespace FIAP.Contacts.Application.Contacts.Services
{
    public interface IContactAppService
    {
        Task<Guid> CreateAsync(CreateContactInput contact);
        Task<ContactDTO> UpdateAsync(Guid contactId, UpdateContactInput contact);
        Task<bool> DeleteAsync(Guid contactId);
        Task<List<ContactDTO>> GetAllAsync();
        Task<ContactDTO> GetByIdAsync(Guid id);
        List<ContactDTO> FilterByPhoneCodeAsync(int phoneCode);

    }
}
