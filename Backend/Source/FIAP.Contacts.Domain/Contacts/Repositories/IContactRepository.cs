using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.SharedKernel.Repositories;

namespace FIAP.Contacts.Domain.Contacts.Repositories
{
    public interface IContactRepository : IBaseRepository<Contact>
    {
    }
}
