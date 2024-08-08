using FIAP.Contacts.Application.Contacts.Models;

namespace FIAP.Contacts.Application.Contacts.Queries
{
    public interface IContactQueries
    {
        Task<List<ContactDTO>> GetByPhoneCodeAsync(int phoneCode);
    }
}
