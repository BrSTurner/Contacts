using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.Repositories;
using FIAP.Contacts.SharedKernel.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace FIAP.Contacts.Infrastructure.Contacts.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact>, IContactRepository
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

        public Task<Contact?> GetByEmailOrPhoneNumber(Email email, PhoneNumber phoneNumber)
        {
            return _entity.FirstOrDefaultAsync(x => x.Email == email || x.PhoneNumber == phoneNumber);
        }
    }
}
