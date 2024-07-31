using Bogus;
using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.SharedKernel.DomainObjects;
using FIAP.Contacts.SharedKernel.Enumerations;

namespace FIAP.Contacts.UnitTests
{
    public static class ContactMock
    {
        public const string VALID_ENTITY = "Valid";

        public static Faker<Contact> ContactFaker = new Faker<Contact>()
            .RuleSet(VALID_ENTITY, r =>
            {
                r.CustomInstantiator(c => new Contact(
                    c.Name.FullName(),
                    new Email(c.Internet.Email()),
                    new PhoneNumber(
                    c.PickRandom(PhoneCodes.ValidCodes.Values.SelectMany(x => x).ToList()),
                    c.Random.Number(900000000, 999999999).ToString())))
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.CreatedAt, f => f.Date.Recent());
            });           
    }
}
