using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.IntegrationTests.Base;
using System.Net.Http.Json;
using System.Net;
using FIAP.Contacts.IntegrationTests.Mock;
using FIAP.Contacts.IntegrationTests.Base.Models;

namespace FIAP.Contacts.IntegrationTests.Contacts.ErrorHandling
{
    public class ErrorHandlingIntegrationTest : IClassFixture<WebClientFixture>
    {
        private readonly WebClientFixture _fixture;

        public ErrorHandlingIntegrationTest(WebClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Shout NOT Create Invalid Contact")]
        [Trait("Integration", "Create")]
        public async Task Should_Not_Create_Invalid_Contact_Returns_BadRequest()
        {
            //Arrange
            var client = _fixture.Client;
            var newContact = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            var input = new CreateContactInput
            {
                Email = string.Empty,
                Name = string.Empty,
                PhoneCode = 0,
                PhoneNumber = string.Empty
            };

            //Act
            var response = await client.PostAsJsonAsync("/api/contacts", input);

            var result = await response.Content.ReadFromJsonAsync<ErrorResponseDTO>();

            //Assert           
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            Assert.False(result.Success);
        }

        [Theory(DisplayName = "Shout NOT Update with Invalid Contact")]
        [Trait("Integration", "Update")]
        [InlineData("")]
        [InlineData(null)]
        public async Task Should_Not_Update_Contact_Returns_BadRequest(string expectedName)
        {
            //Arrange
            var client = _fixture.Client;
            var contactToUpdate = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            await _fixture.InsertContactsInDatabase(contactToUpdate);

            var input = new UpdateContactInput
            {
                Name = expectedName,
                Email = contactToUpdate.Email.Address,
                Id = contactToUpdate.Id,
                PhoneCode = contactToUpdate.PhoneNumber.Code,
                PhoneNumber = contactToUpdate.PhoneNumber.Number
            };

            //Act
            var response = await client.PutAsJsonAsync($"/api/contacts/{contactToUpdate.Id}", input);

            //Assert
            var result = await response.Content.ReadFromJsonAsync<ErrorResponseDTO>();

            //Assert           
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Should NOT Update with No Existent Contact")]
        [Trait("Integration", "Update")]
        public async Task Should_Not_Update_Inexistent_Contact_Returns_NotFound()
        {
            //Arrange
            var client = _fixture.Client;
            var contactToUpdate = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            var input = new UpdateContactInput
            {
                Name = "Bruno",
                Email = contactToUpdate.Email.Address,
                Id = contactToUpdate.Id,
                PhoneCode = contactToUpdate.PhoneNumber.Code,
                PhoneNumber = contactToUpdate.PhoneNumber.Number
            };

            //Act
            var response = await client.PutAsJsonAsync($"/api/contacts/{contactToUpdate.Id}", input);

            //Assert
            var result = await response.Content.ReadFromJsonAsync<ErrorResponseDTO>();

            //Assert           
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Should NOT Delete Empty Contact")]
        [Trait("Integration", "Delete")]
        public async Task Should_Not_Delete_Contact_Returns_BadRequest()
        {
            //Arrange
            var client = _fixture.Client;

            //Act
            var response = await client.DeleteAsync($"/api/contacts/{Guid.Empty}");

            //Assert
            var result = await response.Content.ReadFromJsonAsync<ErrorResponseDTO>();
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Should NOT Delete Inexistent Contact")]
        [Trait("Integration", "Delete")]
        public async Task Should_Not_Delete_Inexistent_Contact_Returns_NotFound()
        {
            //Arrange
            var client = _fixture.Client;

            //Act
            var response = await client.DeleteAsync($"/api/contacts/{Guid.NewGuid()}");

            //Assert
            var result = await response.Content.ReadFromJsonAsync<ErrorResponseDTO>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(result.Message);
            Assert.False(result.Success);
        }
    }
}
