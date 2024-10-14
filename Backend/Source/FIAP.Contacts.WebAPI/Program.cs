using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.Application.Contacts.Pofiles;
using FIAP.Contacts.Application.Contacts.Services;
using FIAP.Contacts.Application.Extensions;
using FIAP.Contacts.Domain.Contacts.Services;
using FIAP.Contacts.Infrastructure.Extensions;
using FIAP.Contacts.WebAPI.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ContactProfile));
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpMetrics();

app.UseRouting();

app.UseContactsExceptionFilter();

app.UseEndpoints(endpoints => endpoints.MapMetrics());

var endpointGroup = app
    .MapGroup("api/contacts");

endpointGroup.MapPost(string.Empty, async (CreateContactInput contact, IContactAppService contactService) =>
{
    var contactId = await contactService.CreateAsync(contact);
    return Results.Created($"/{contactId}", contactId);

})
.WithTags("Contacts")
.WithName("Create Contact")
.Produces<Created<Guid>>()
.Produces<BadRequest>();

endpointGroup.MapPut("/{contactId:guid}", async (Guid contactId, UpdateContactInput contact, IContactAppService contactService) =>
{
    var updatedContact = await contactService.UpdateAsync(contactId, contact);
    return Results.Ok(updatedContact);

})
.WithTags("Contacts")
.WithName("Update Contact")
.Produces<Ok>()
.Produces<NotFound>()
.Produces<BadRequest>();

endpointGroup.MapDelete("/{contactId:guid}", async (Guid contactId, IContactAppService contactService) =>
{
    var result = await contactService.DeleteAsync(contactId);
    return Results.Ok(result);

})
.WithTags("Contacts")
.WithName("Delete Contact")
.Produces<Ok>()
.Produces<NotFound>()
.Produces<BadRequest>();


endpointGroup.MapGet(string.Empty, async (IContactAppService contactService) =>
{
    var result = await contactService.GetAllAsync();
    
    if (result == null || !result.Any())
        return Results.NoContent();

    return Results.Ok(result);

})
.WithTags("Contacts")
.WithName("Get All Contacts")
.Produces<Ok>()
.Produces<NoContent>();

endpointGroup.MapGet("/{phoneCode:int}", async (int phoneCode, IContactAppService contactService) =>
{
    var result = await contactService.GetByPhoneCodeAsync(phoneCode);

    if(result == null || !result.Any())
        return Results.NoContent();

    return Results.Ok(result);

})
.WithTags("Contacts")
.WithName("Get Contact By Phone Code")
.Produces<Ok>()
.Produces<NoContent>();




app.Run();

public partial class Program { }