using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.Repositories;
using FIAP.Contacts.SharedKernel.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;

namespace FIAP.Contacts.Infrastructure.Contacts.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(FIAPContext context) : base(context)
        {

        }

        //TODO 
        //Change the query to filter by the PhoneNumber Code field
        //public Task<IEnumerable<Contact>> FilterByPhoneCode(PhoneNumber phoneNumber)
        //{
        //    var sql = "SELECT * FROM Contacts WHERE phoneNumber = @PhoneNumber";

        //    return DbConnection.QueryAsync<Contact>(sql, new { PhoneNumber = phoneNumber }));    
        //}

        public Task<Contact?> GetByEmailOrPhoneNumber(Email email, PhoneNumber phoneNumber)
        {
            return _entity.FirstOrDefaultAsync(x => 
                x.Email.Address.Equals(email.Address) || (
                x.PhoneNumber.Code.Equals(phoneNumber.Code) && 
                x.PhoneNumber.Number.Equals(phoneNumber.Number)));
        }
    }
}
