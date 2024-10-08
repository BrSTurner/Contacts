﻿using AutoMapper;
using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.Application.Contacts.Queries;
using FIAP.Contacts.Application.Contacts.Validations;
using FIAP.Contacts.Application.Services;
using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Services;

namespace FIAP.Contacts.Application.Contacts.Services
{
    public sealed class ContactAppService : BaseAppService, IContactAppService
    {
        private IContactService _contactService;
        private IContactQueries _contactQueries;
        private IMapper _mapper;

        public ContactAppService(
            IContactService contactService,
            IMapper mapper,
            IContactQueries contactQueries) 
        {
            _contactService = contactService;   
            _contactQueries = contactQueries;
            _mapper = mapper;  
        }

        public async Task<Guid> CreateAsync(CreateContactInput contact)
        {
            EnsureContactInputIsValid(contact);

            await EnsureValidationAsync<CreateContactInput, CreateContactValidation>(contact);

            return await _contactService.CreateAsync(_mapper.Map<Contact>(contact));
        }

        public async Task<List<ContactDTO>> GetAllAsync()
        {
            var contacts = await _contactService.GetAllAsync();
            return _mapper.Map<List<ContactDTO>>(contacts);
        }

        public async Task<List<ContactDTO>> GetByPhoneCodeAsync(int phoneCode)
        {
            return await _contactQueries.GetByPhoneCodeAsync(phoneCode);
        }

        public async Task<ContactDTO> UpdateAsync(Guid contactId, UpdateContactInput contact)
        {
            EnsureContactInputIsValid(contact);

            await EnsureValidationAsync<UpdateContactInput, UpdateContactValidation>(contact);

            var updatedContact = await _contactService.UpdateAsync(contactId, _mapper.Map<Contact>(contact));

            return _mapper.Map<ContactDTO>(updatedContact);
        }

        public async Task<bool> DeleteAsync(Guid contactId)
        {
            if (contactId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(contactId), "Contact Id must be correctly filled");

            return await _contactService.DeleteAsync(contactId);
        }

        private void EnsureContactInputIsValid(ContactInput contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact), "Contact must be correctly filled");
        }
    }
}
