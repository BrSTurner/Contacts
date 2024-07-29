using FIAP.Contacts.Application.Extensions;
using FIAP.Contacts.Domain.Contacts.Services;
using FIAP.Contacts.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


app.MapPost("/contacts", async (Contact contact, IContactService contactService) =>
{
    await contactService.Create(contact);

    return Results.Created();
}).WithName("CreateContact");

app.MapPut("/contacts", async (Guid contactId, Contact contact, IContactService contactService) =>
{
    await contactService.Update(contactId, contact);

    return Results.Ok();
}).WithName("UpdateContact");

app.MapGet("/contacts", async (IContactService contactService) =>
    await contactService.GetAll().WithName("GetContacts"));

app.MapGet("/contacts/{id}", async (Guid contactId, IContactService contactService) =>
    await contactService.Get(contactId).WithName("GetContact"));

app.Run();