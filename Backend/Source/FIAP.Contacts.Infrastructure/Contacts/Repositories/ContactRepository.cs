using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.Repositories;

namespace FIAP.Contacts.Infrastructure.Contacts.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact>
    {
        public ContactRepository(FIAPContext context) : base(context)
        {
            public Task<Contact> FilterByPhoneCode(PhoneNumber phoneNumber)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = "SELECT * FROM Contacts WHERE phoneNumber = @PhoneNumber";

                    var contacts = connection.Query<Contact>(sql, new { PhoneNumber = phoneNumber }).ToList();
                    return contacts;    
                }
            }
        }

    }
}
