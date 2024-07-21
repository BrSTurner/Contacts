using FIAP.Contacts.Domain.Contacts.Entities;

namespace FIAP.Contacts.Domain.Contacts.Services
{
    public interface IContactService
    {
        Task Create(Contact contact);
        Task Update(Guid contactId, Contact contact);
    }
}
