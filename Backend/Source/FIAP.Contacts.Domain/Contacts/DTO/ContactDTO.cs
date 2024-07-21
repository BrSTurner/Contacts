namespace FIAP.Contacts.Domain.Contacts.DTO
{
    public record ContactDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public int PhoneCode { get; init; }
    }
}
