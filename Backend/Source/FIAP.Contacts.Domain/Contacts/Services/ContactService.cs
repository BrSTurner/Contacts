﻿using FIAP.Contacts.Domain.Contacts.Entities;
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

            if(await IsContactAlreadyRegistered(contact))
                throw new ExistingContactException();

            _contactRepository.Add(contact);

            if (await _unitOfWork.CommitAsync())
                return contact.Id;

            throw new DomainException("Contact could not be created");
        }

        public async Task<List<Contact>> GetAllAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

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

        private async Task<bool> IsContactAlreadyRegistered(Contact contact)
        {
            var existentContact = await _contactRepository.GetByEmailOrPhoneNumber(contact.Email, contact.PhoneNumber);
            return existentContact != null;
        }

        private void ValidateContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact), "Contact must be filled");
        }

        private void ValidateContactId(Guid contactId)
        {
            if (contactId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(contactId), "Contact Identifier must be correctly filled");
        }
    }
}
