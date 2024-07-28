using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.Repositories;
using FIAP.Contacts.SharedKernel.DomainObjects;
using Microsoft.Data.SqlClient;

namespace FIAP.Contacts.Infrastructure.Contacts.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact>
    {
        public ContactRepository(FIAPContext context) : base(context)
        {
            //public Task<Contact> FilterByPhoneCode(PhoneNumber phoneNumber)
            //{
            //    using (var connection = new SqlConnection())
            //    {
            //        string sql = "SELECT * FROM Contacts WHERE phoneNumber = @PhoneNumber";

            //        var contacts = connection.QueryAsync<Contact>(sql, new { PhoneNumber = phoneNumber }).ToList();
            //        return contacts;
            //    }
            //}
        }

    }
}
