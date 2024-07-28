using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.SharedKernel.DomainObjects;
using FIAP.Contacts.SharedKernel.Enumerations;

namespace FIAP.Contacts.Domain.Contacts.Services
{
    public interface IContactService
    {
        Task Create(Contact contact);
        //Task FilterByPhoneCode(int phoneCode);
        //Task<Contact> Get(Guid contactId);
        //Task<List<Contact>> GetAll();
        Task Update(Guid contactId, Contact contact);
    }
}
