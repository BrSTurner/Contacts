namespace FIAP.Contacts.Application.Contacts.Models
{
    public record ContactDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneCode { get; set; }
    }
}
