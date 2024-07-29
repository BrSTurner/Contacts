using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.SharedKernel.Exceptions;
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

        public async Task<Guid> CreateAsync(Contact contact)
        {
            ValidateContact(contact);

            if(IsContactAlreadyRegistered(contact))
                throw new ExistingContactException();

            _contactRepository.Add(contact);

            if (await _unitOfWork.CommitAsync())
                return contact.Id;

            throw new Exception("Contact could not be created");
        }

        //public async Task<List<Contact>> FilterByPhoneCode(int phoneCode)
        //{
        //    _contactRepository
        //}

        //public async Task<Contact> Get(Guid contactId)
        //{
        //    return _contactRepository.GetByIdAsync(contactId);
        //}

        //public async Task<List<Contact>> GetAll()
        //{
        //    return _contactRepository.GetAllAsync<Contact>();
        //}

        public async Task<Contact> UpdateAsync(Guid contactId, Contact contact)
        {
            ValidateContact(contact);
            ValidateContactId(contactId);
            
            var currentContact = await _contactRepository.GetByIdAsync(contactId);

            if (currentContact == null)
                throw new EntityNotFoundException("Contact could not be found");

            currentContact.Update(contact);

            if (await _unitOfWork.CommitAsync())
                return currentContact;

            throw new DomainException("Contact could not be updated");
        }

        public async Task<bool> DeleteAsync(Guid contactId)
        {
            ValidateContactId(contactId);

            var currentContact = await _contactRepository.GetByIdAsync(contactId);

            if (currentContact == null)
                throw new EntityNotFoundException("Contact could not be found");

            _contactRepository.Remove(currentContact);

            if (await _unitOfWork.CommitAsync())
                return true;

            throw new DomainException("Contact could not be deleted");
        }

        private bool IsContactAlreadyRegistered(Contact contact)
        {
            var existentContact = _contactRepository.GetByEmailOrPhoneNumber(contact.Email, contact.PhoneNumber);
            return existentContact != null;
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
