using FIAP.Contacts.SharedKernel.DomainObjects;

namespace FIAP.Contacts.Domain.Contacts.Entities
{
    public class Contact : Entity, AggregateRoot
    {
        public required string Name { get; set; }
        public required Email Email { get; set; }
        public required PhoneNumber PhoneNumber { get; set; }
    }
}
