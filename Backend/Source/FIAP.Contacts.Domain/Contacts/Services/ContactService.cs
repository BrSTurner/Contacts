using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.SharedKernel.UoW;

namespace FIAP.Contacts.Domain.Contacts.Services
{
    public sealed class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(
            IContactRepository contactRepository, 
            IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Contact contact)
        {
            ValidateContact(contact);

            _contactRepository.Add(contact);

            await _unitOfWork.CommitAsync();
        }

        public async Task Update(Guid contactId, Contact contact)
        {
            ValidateContact(contact);
            ValidateContactId(contactId);
            
            var currentContact = await _contactRepository.GetByIdAsync(contactId);

            if (currentContact == null)
                throw new KeyNotFoundException("Contact could not be found");

            currentContact.Update(
                contact.Name, 
                contact.Email.Address, 
                contact.PhoneNumber.Code, 
                contact.PhoneNumber.Number
            );

            await _unitOfWork.CommitAsync();
        }

        private void ValidateContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException("Contact must be filled");
        }

        private void ValidateContactId(Guid contactId)
        {
            if (contactId.Equals(Guid.Empty))
                throw new ArgumentNullException("Contact Identifier must be correctly filled");
        }
    }
}
