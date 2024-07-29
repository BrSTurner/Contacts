namespace FIAP.Contacts.Application.Contacts.Models
{
    public record UpdateContactInput : ContactInput
    {
        public required Guid Id { get; set; }
    }
}
