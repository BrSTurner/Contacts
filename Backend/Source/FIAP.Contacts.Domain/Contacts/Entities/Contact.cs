using FIAP.Contacts.SharedKernel.DomainObjects;

namespace FIAP.Contacts.Domain.Contacts.Entities
{
    public class Contact : Entity, AggregateRoot
    {
        private string _name;

        public string Name 
        { 
            get
            {
                return _name;
            } 
            private set 
            {
                ValidateName(value);
                _name = value;
            } 
        }

        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }

        public Contact(string name, Email email, PhoneNumber phoneNumber)
        {
            Name = name;         
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public static Contact Create(string name, string email, int phoneCode, string phoneNumber)
        {
            var address = new Email(email);
            var phone = new PhoneNumber(phoneCode, phoneNumber);

            return new Contact(name, address, phone);            
        }

        public void Update(string name, string email, int phoneCode, string phoneNumber)
        {
            UpdateName(name);
            UpdateEmail(email);
            UpdatePhoneNumber(phoneCode, phoneNumber);
        }

        public void UpdateName(string name)
        {
            if (Name.Equals(name))
                return;

            Name = name;                    
        }

        public void UpdateEmail(string email)
        {
            if (Email.Address.Equals(email, StringComparison.OrdinalIgnoreCase))
                return;

            Email = new Email(email);
        }

        public void UpdatePhoneNumber(int phoneCode, string phoneNumber)
        {
            if (PhoneNumber.Code.Equals(phoneCode) && PhoneNumber.Number.Equals(phoneNumber))
                return;

            PhoneNumber = new PhoneNumber(phoneCode, phoneNumber);
        }

        private void ValidateName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(Name), "Name must be filled");
        }


    }
}
