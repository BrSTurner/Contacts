namespace FIAP.Contacts.IntegrationTests.Base.Models
{
    public record ErrorResponseDTO
    {
        public string Message { get; set; }
        public bool Success { get; set; }   
    }
}
