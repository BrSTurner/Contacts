#pragma warning disable CS8603
#pragma warning disable CS8604
using AutoMapper;
using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.Application.Contacts.Queries;
using FIAP.Contacts.Application.Contacts.Services;
using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.Domain.Contacts.Services;
using FIAP.Contacts.SharedKernel.Exceptions;
using FIAP.Contacts.SharedKernel.UoW;
using Moq;

namespace FIAP.Contacts.UnitTests.Contacts.Domain.Services
{
    public class ContactsServiceTest : IAsyncLifetime
    {
        private Mock<IContactRepository> ContactRepository { get; set; }
        private Mock<IContactService> ContactService { get; set; }
        private Mock<IContactQueries> ContactQueries { get; set; }
        private Mock<IMapper> Mapper { get; set; }
        private Mock<IUnitOfWork> UnitOfWork { get; set; }

        public Task InitializeAsync()
        {
            ContactRepository = new Mock<IContactRepository>();
            UnitOfWork = new Mock<IUnitOfWork>();
            ContactQueries = new Mock<IContactQueries>();
            ContactService = new Mock<IContactService>();
            Mapper = new Mock<IMapper>();
            return Task.CompletedTask;
        }

        public Task DisposeAsync() => Task.CompletedTask;

        #region CREATE CONTACT TESTS

        [Fact(DisplayName = "Should Not Create And Throw Domain Exception")]
        [Trait("Category", "Create")]
        public async Task Should_Not_Create_And_Throw_Domain_Exception()
        {
            //Arrange
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            UnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(false);

            var service = GetContactService();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => service.CreateAsync(contact));
            Assert.Equal("Contact could not be created", exception.Message);
        }

        [Fact(DisplayName = "Should Create Contact")]
        [Trait("Category", "Create")]
        public async Task Should_Create_Contact_And_Return_Guid()
        {
            //Arrange
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            UnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(true);

            var service = GetContactService();

            //Act
            var result =  await service.CreateAsync(contact);           

            //Assert
            Assert.NotEqual(Guid.Empty, result);
            ContactRepository.Verify(rep => rep.Add(contact), Times.Once);
        }

        [Fact(DisplayName = "Should Throw Contact Already Registered")]
        [Trait("Category", "Create")]
        public async Task Should_Throw_Contact_Already_Registered()
        {
            //Arrange
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            ContactRepository
                .Setup(x => x.GetByEmailOrPhoneNumber(contact.Email, contact.PhoneNumber))
                .ReturnsAsync(contact);

            var service = GetContactService();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<ExistingContactException>(() => service.CreateAsync(contact));
            Assert.Equal("A contact with same E-mail or Phone Number already exists", exception.Message);
        }

        #endregion

        #region UPDATE CONTACT TESTS

        [Fact(DisplayName = "Should Update Contact")]
        [Trait("Category", "Update")]
        public async Task Should_Update_Contact()
        {
            //Arrange
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            var modifiedContact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            ContactRepository
                .Setup(x => x.GetByIdAsync(contact.Id))
                .ReturnsAsync(contact);

            UnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(true);

            var service = GetContactService();

            //Act
            var updatedContact = await service.UpdateAsync(contact.Id, modifiedContact);

            //Assert            
            ContactRepository.Verify(rep => rep.GetByIdAsync(contact.Id), Times.Once);
            UnitOfWork.Verify(unit => unit.CommitAsync(), Times.Once);
            Assert.NotNull(updatedContact);
            Assert.Equal(contact.Name, modifiedContact.Name);
            Assert.True(contact.Email.Equals(modifiedContact.Email));
            Assert.True(contact.PhoneNumber.Equals(modifiedContact.PhoneNumber));
        }


        [Fact(DisplayName = "Should Not Find Entity on Update and Throw Exception")]
        [Trait("Category", "Update")]
        public async Task Should_Not_Update_Contact_And_Throw_Not_Found()
        {
            //Arrange
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            var modifiedContact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            ContactRepository
                .Setup(x => x.GetByIdAsync(contact.Id))
                .Returns<Contact>(null);

            var service = GetContactService();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => service.UpdateAsync(contact.Id, modifiedContact));
            Assert.Equal("Contact could not be found", exception.Message);
        }

        [Fact(DisplayName = "Should Not Update and Throw Domain Exception")]
        [Trait("Category", "Update")]
        public async Task Should_Not_Update_Contact_And_Throw_Domain_Exception()
        {
            //Arrange
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            var modifiedContact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            ContactRepository
                .Setup(x => x.GetByIdAsync(contact.Id))
                .ReturnsAsync(contact);

            UnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(false);

            var service = GetContactService();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => service.UpdateAsync(contact.Id, modifiedContact));
            Assert.Equal("Contact could not be updated", exception.Message);
        }

        #endregion

        #region DELETE CONTACT TESTS

        [Fact(DisplayName = "Should Delete")]
        [Trait("Category", "Delete")]
        public async Task Should_Delete_Contact()
        {
            //Arrange
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            UnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(true);

            ContactRepository
                .Setup(x => x.GetByIdAsync(contact.Id))
                .ReturnsAsync(contact);

            var service = GetContactService();

            //Act
            var result = await service.DeleteAsync(contact.Id);

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should Not Delete and Throw Entity Not Found")]
        [Trait("Category", "Delete")]
        public async Task Should_NotDelete_And_Throw_EntityNotFound()
        {
            //Arrange      
            ContactRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns<Contact>(null);

            var service = GetContactService();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAsync(Guid.NewGuid()));
            Assert.Equal("Contact could not be found", exception.Message);
        }

        [Fact(DisplayName = "Should Not Delete and Throw Argument Null")]
        [Trait("Category", "Delete")]
        public async Task Should_NotDelete_And_Throw_ArgumentNull()
        {
            //Arrange      
            var service = GetContactService();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeleteAsync(Guid.Empty));
            Assert.Contains("Contact Identifier must be correctly filled", exception.Message);
        }

        [Fact(DisplayName = "Should Not Delete and Throw Domain Exception")]
        [Trait("Category", "Delete")]
        public async Task Should_NotDelete_And_Throw_DomainException()
        {
            //Arrange      
            var contact = ContactMock
                .ContactFaker
                .Generate(ContactMock.VALID_ENTITY);

            UnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(false);

            ContactRepository
                .Setup(x => x.GetByIdAsync(contact.Id))
                .ReturnsAsync(contact);

            var service = GetContactService();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => service.DeleteAsync(contact.Id));
            Assert.Equal("Contact could not be deleted", exception.Message);
        }
        #endregion

        #region GET CONTACT TESTS
        [Fact(DisplayName = "Should Return All Contacts")]
        [Trait("Category", "GetAll")]
        public async Task Should_Return_All_Contacts()
        {
            // Arrange
            var contacts = ContactMock.ContactFaker.Generate(2, ContactMock.VALID_ENTITY);

            ContactRepository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(contacts);

            var service = GetContactService();

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            foreach (var contact in contacts)
            {
                Assert.Contains(result, c => c.Id == contact.Id);
            }
        }


        [Fact(DisplayName = "Should Return Empty List When No Contacts Exist")]
        [Trait("Category", "GetAll")]
        public async Task Should_Return_Empty_List_When_No_Contacts_Exist()
        {
            // Arrange
            var emptyContacts = new List<Contact>();

            ContactRepository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(emptyContacts);

            var service = GetContactService();

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Fact(DisplayName = "Should Return Empty List When No Contacts Match PhoneCode")]
        [Trait("Category", "GetByPhoneCode")]
        public async Task Should_Return_Empty_List_When_No_Contacts_Match_PhoneCode()
        {
            // Arrange
            var emptyContacts = new List<ContactDTO>();

            ContactQueries
                .Setup(x => x.GetByPhoneCodeAsync(It.IsAny<int>()))
                .ReturnsAsync(emptyContacts);

            var service = GetContactAppService();

            // Act
            var result = await service.GetByPhoneCodeAsync(ContactMock.SPECIFIC_PHONE_CODE);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should Contacts That Match PhoneCode")]
        [Trait("Category", "GetByPhoneCode")]
        public async Task Should_Contacts_That_Match_PhoneCode()
        {
            // Arrange
            var phonecode = ContactMock.SPECIFIC_PHONE_CODE;
            var contacts = ContactMock.ContactDTOFaker
                .Generate(5, ContactMock.VALID_ENTITY);

            var contactWithSpecificPhoneCode = ContactMock.ContactDTOFaker
                .Generate(ContactMock.VALID_SPECIFIC_PHONECODE_ENTITY);

            contacts.Add(contactWithSpecificPhoneCode);

            ContactQueries
                .Setup(x => x.GetByPhoneCodeAsync(phonecode))
                .ReturnsAsync(contacts.Where(c => c.PhoneCode == phonecode).ToList());

            var service = GetContactAppService();

            // Act
            var result = await service.GetByPhoneCodeAsync(phonecode);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, c => Assert.Equal(phonecode, c.PhoneCode));
            Assert.Single(result);
        }
        #endregion

        private ContactService GetContactService() 
            => new ContactService(ContactRepository.Object, UnitOfWork.Object);

        private ContactAppService GetContactAppService()
            => new ContactAppService(ContactService.Object, Mapper.Object, ContactQueries.Object);
    }
}
