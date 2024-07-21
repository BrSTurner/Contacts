using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.Repositories;

namespace FIAP.Contacts.Infrastructure.Contacts.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact>
    {
        public ContactRepository(FIAPContext context) : base(context)
        {
        }

    }
}
