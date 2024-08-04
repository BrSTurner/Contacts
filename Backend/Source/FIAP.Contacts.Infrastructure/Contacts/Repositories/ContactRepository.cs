using Dapper;
using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.Repositories;
using FIAP.Contacts.SharedKernel.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contacts.Infrastructure.Contacts.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(FIAPContext context) : base(context)
        {

        }

        public async Task<List<Contact>> GetByPhoneCode(int phoneCode)
        {
            var sql = "SELECT * FROM Contacts WHERE phoneCode = @PhoneCode";

            var result = await DbConnection.QueryAsync<Contact>(sql, new { PhoneCode = phoneCode });

            //var entity = _entity.Where(x => x.PhoneNumber.Code.Equals(phoneCode)).ToList();

            return result.ToList();
        }

        public Task<Contact?> GetByEmailOrPhoneNumber(Email email, PhoneNumber phoneNumber)
        {
            return _entity.FirstOrDefaultAsync(x => 
                x.Email.Address.Equals(email.Address) || (
                x.PhoneNumber.Code.Equals(phoneNumber.Code) && 
                x.PhoneNumber.Number.Equals(phoneNumber.Number)));
        }
    }
}
