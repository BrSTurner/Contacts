using Bogus;
using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.SharedKernel.DomainObjects;
using FIAP.Contacts.SharedKernel.Enumerations;

namespace FIAP.Contacts.UnitTests
{
    public static class ContactMock
    {
        public const string VALID_ENTITY = "Valid";
        public const string VALID_SPECIFIC_PHONECODE_ENTITY = "Valid_Specific_PhoneCode";
        public const int SPECIFIC_PHONE_CODE = 11;

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
           
        public static Faker<ContactDTO> ContactDTOFaker = new Faker<ContactDTO>()
            .RuleSet(VALID_ENTITY, r =>
            {
                r.RuleFor(c => c.Id, f => f.Random.Guid())
                    .RuleFor(c => c.Name, f => f.Name.FullName())
                    .RuleFor(c => c.Email, f => f.Internet.Email())
                    .RuleFor(c => c.PhoneNumber, f => f.Random.Number(900000000, 999999999).ToString())
                    .RuleFor(c => c.PhoneCode, f => f.PickRandom(PhoneCodes.ValidCodes.Values.SelectMany(x => x).ToList()));
            }).RuleSet(VALID_SPECIFIC_PHONECODE_ENTITY, r =>
            {
                r.RuleFor(c => c.Id, f => f.Random.Guid())
                    .RuleFor(c => c.Name, f => f.Name.FullName())
                    .RuleFor(c => c.Email, f => f.Internet.Email())
                    .RuleFor(c => c.PhoneNumber, f => f.Random.Number(900000000, 999999999).ToString())
                    .RuleFor(c => c.PhoneCode, f => 11);
            });           
    }
}