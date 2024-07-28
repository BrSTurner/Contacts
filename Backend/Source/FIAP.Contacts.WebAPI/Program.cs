using FIAP.Contacts.Domain.Contacts.Entities;
using FIAP.Contacts.Domain.Contacts.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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