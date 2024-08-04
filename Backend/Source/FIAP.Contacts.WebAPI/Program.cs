using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.Application.Contacts.Pofiles;
using FIAP.Contacts.Application.Contacts.Services;
using FIAP.Contacts.Application.Extensions;
using FIAP.Contacts.Domain.Contacts.Services;
using FIAP.Contacts.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ContactProfile));
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var endpointGroup = app.MapGroup("Contacts");

endpointGroup.MapPost("/contacts", async (CreateContactInput contact, IContactAppService contactService) =>
{
    var contactId = await contactService.CreateAsync(contact);
    return Results.Created($"/{contactId}", contactId);

})
.WithTags("Contacts")
.WithName("Create Contact");

endpointGroup.MapPut("/contacts/{contactId:guid}", async (Guid contactId, UpdateContactInput contact, IContactAppService contactService) =>
{
    var updatedContact = await contactService.UpdateAsync(contactId, contact);
    return Results.Ok(updatedContact);

})
.WithTags("Contacts")
.WithName("Update Contact");

endpointGroup.MapDelete("/contacts/{contactId:guid}", async (Guid contactId, IContactAppService contactService) =>
{
    var result = await contactService.DeleteAsync(contactId);
    return Results.Ok(result);

})
.WithTags("Contacts")
.WithName("Delete Contact");


endpointGroup.MapGet("/contacts", async (IContactAppService contactService) =>
{
    var result = await contactService.GetAllAsync();
    return Results.Ok(result);

}).WithTags("Contacts").WithName("GetAllContacts");

endpointGroup.MapGet("/contacts/{id:guid}", async (Guid contactId, IContactAppService contactService) =>
{
    var result = await contactService.GetByIdAsync(contactId);
    return Results.Ok(result);

}).WithTags("Contacts").WithName("GetContactById");

endpointGroup.MapGet("/contacts/Filter/{phoneCode:int}", (int phoneCode, IContactAppService contactService) =>
{
    var result = contactService.FilterByPhoneCodeAsync(phoneCode);
    return Results.Ok(result);

}).WithTags("Contacts").WithName("FilterContactByPhoneCode");

app.Run();