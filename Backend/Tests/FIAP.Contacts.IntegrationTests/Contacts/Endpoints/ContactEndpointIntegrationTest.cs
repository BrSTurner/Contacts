using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.IntegrationTests.Base;
using FIAP.Contacts.IntegrationTests.Mock;
using System.Net;
using System.Net.Http.Json;

namespace FIAP.Contacts.IntegrationTests.Contacts.Endpoints
{
    public class ContactEndpointIntegrationTest : IClassFixture<WebClientFixture>
    {
        private readonly WebClientFixture _fixture;

        public ContactEndpointIntegrationTest(WebClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Get All Contacts")]
        [Trait("Integration", "Get")]
        public async Task Get_All_Contacts_Returns_Ok()
        {
            //Arrange
            await _fixture.InsertContactsInDatabase(2);
            var client = _fixture.Client;

            //Act
            var response = await client.GetAsync("/api/contacts");

            //Assert
            response.EnsureSuccessStatusCode();

            var contacts = await response.Content.ReadFromJsonAsync<List<ContactDTO>>();
            var contactsInDbAmount = await _fixture.CountContactsInDatabaseAsync();

            Assert.NotNull(contacts);
            Assert.Equal(contactsInDbAmount, contacts.Count);
        }

        [Fact(DisplayName = "Should Not Get All Contacts")]
        [Trait("Integration", "Get")]
        public async Task Get_No_Contacts_Returns_No_Content()
        {
            //Arrange
            await _fixture.ClearDatabase();
            var client = _fixture.Client;

            //Act
            var response = await client.GetAsync("/api/contacts");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory(DisplayName = "Get Contact By Phone Code")]
        [Trait("Integration", "Get")]        
        [InlineData(11)]
        [InlineData(19)]
        [InlineData(21)]
        [InlineData(44)]
        public async Task Get_Contact_By_PhoneCode_Returns_Ok(int phoneCode)
        {
            //Arrange
            var client = _fixture.Client;
            var contactInDatabase = ContactMock
                    .GenerateContactByPhoneCode(phoneCode)
                    .Generate(ContactMock.CUSTOM_PHONE_VALID_ENTITY);

            await _fixture
                    .InsertContactsInDatabase(contactInDatabase);

            //Act
            var response = await client.GetAsync($"/api/contacts/{phoneCode}");

            //Assert
            response.EnsureSuccessStatusCode();

            var contacts = await response.Content.ReadFromJsonAsync<List<ContactDTO>>();

            Assert.NotNull(contacts);
            Assert.NotNull(contacts.FirstOrDefault(x => x.Id == contactInDatabase.Id));
            Assert.True(contacts.All(x => x.PhoneCode == phoneCode));
        }

        [Fact(DisplayName = "Shoudl Not Get Contact By Phone Code ")]
        [Trait("Integration", "Get")]
        public async Task Get_Contact_By_PhoneCode_Returns_No_Content()
        {
            //Arrange
            var client = _fixture.Client;
            await _fixture.ClearDatabase();

            //Act
            var response = await client.GetAsync($"/api/contacts/11");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Create New Contact")]
        [Trait("Integration", "Create")]
        public async Task Should_Create_New_Contact_Returns_Created()
        {
            //Arrange
            var client = _fixture.Client;
            var newContact = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            var input = new CreateContactInput 
            {
                Email = newContact.Email.Address,
                Name = newContact.Name,
                PhoneCode = newContact.PhoneNumber.Code,
                PhoneNumber = newContact.PhoneNumber.Number
            };

            //Act
            var response = await client.PostAsJsonAsync("/api/contacts", input);

            //Assert
            response.EnsureSuccessStatusCode();
            
            var contactId = response.Headers.Location?.ToString().Split('/').Last();
            var contactFromDatabase = await _fixture.GetByIdInDatabaseAsync(Guid.Parse(contactId));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(contactFromDatabase);
            Assert.Equal(contactFromDatabase.Name, newContact.Name);
            Assert.Equal(contactFromDatabase.Email.Address, newContact.Email.Address);
            Assert.Equal(contactFromDatabase.PhoneNumber.Code, newContact.PhoneNumber.Code);
            Assert.Equal(contactFromDatabase.PhoneNumber.Number, newContact.PhoneNumber.Number);
        }

        [Theory(DisplayName = "Update Contact")]
        [Trait("Integration", "Update")]
        [InlineData("Bruno S1lv4")]
        [InlineData("Gustavo Koz0noe")]
        public async Task Should_Update_Contact_Returns_Ok(string expectedName)
        {
            //Arrange
            var client = _fixture.Client;
            var contactToUpdate = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            await _fixture.InsertContactsInDatabase(contactToUpdate);

            var input = new UpdateContactInput {
                Name = expectedName,
                Email = contactToUpdate.Email.Address,
                Id = contactToUpdate.Id,
                PhoneCode = contactToUpdate.PhoneNumber.Code,
                PhoneNumber = contactToUpdate.PhoneNumber.Number
            };

            //Act
            var response = await client.PutAsJsonAsync($"/api/contacts/{contactToUpdate.Id}", input);

            //Assert
            response.EnsureSuccessStatusCode();
            
            var contact = await response.Content.ReadFromJsonAsync<ContactDTO>();

            Assert.NotNull(contact);
            Assert.Equal(contact.Name, expectedName);
            Assert.Equal(contact.Email, contactToUpdate.Email.Address);
            Assert.Equal(contact.PhoneCode, contactToUpdate.PhoneNumber.Code);
            Assert.Equal(contact.PhoneNumber, contactToUpdate.PhoneNumber.Number);
        }

        [Fact(DisplayName = "Delete Contact")]
        [Trait("Integration", "Delete")]
        public async Task Should_Delete_Contact_Returns_Ok()
        {
            //Arrange
            var client = _fixture.Client;
            var contactToUpdate = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            await _fixture.InsertContactsInDatabase(contactToUpdate);

            //Act
            var response = await client.DeleteAsync($"/api/contacts/{contactToUpdate.Id}");

            //Assert
            response.EnsureSuccessStatusCode();

            var isDeleted = await response.Content.ReadFromJsonAsync<bool>();
            var contactFromDatabase = await _fixture.GetByIdInDatabaseAsync(contactToUpdate.Id);

            Assert.True(isDeleted);
            Assert.Null(contactFromDatabase);
        }
    }
}
