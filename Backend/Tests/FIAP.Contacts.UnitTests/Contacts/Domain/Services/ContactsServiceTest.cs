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
        private Mock<IUnitOfWork> UnitOfWork { get; set; }

        public Task InitializeAsync()
        {
            ContactRepository = new Mock<IContactRepository>();
            UnitOfWork = new Mock<IUnitOfWork>();
            return Task.CompletedTask;
        }

        public Task DisposeAsync() => Task.CompletedTask;
        

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


        private ContactService GetContactService() 
            => new ContactService(ContactRepository.Object, UnitOfWork.Object);
    }
}
