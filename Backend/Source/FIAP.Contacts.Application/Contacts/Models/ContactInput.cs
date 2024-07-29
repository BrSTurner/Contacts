using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace FIAP.Contacts.Application.Contacts.Models
{
    public record ContactInput
    {
        public Guid Id { get; set; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public int PhoneCode { get; init; } 
    }
}
