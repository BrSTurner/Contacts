using System.ComponentModel.DataAnnotations;

namespace FIAP.Contacts.SharedKernel.DomainObjects
{
    public record class Email
    {
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Address { get; init; }

        public Email(string address)
        {
            if (!IsValidEmail(address))
            {
                throw new ArgumentException("Invalid email address format.", nameof(address));
            }

            Address = address;
        }

        private static bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        public override string ToString() => Address;
    }
}
