using FIAP.Contacts.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace FIAP.Contacts.WebAPI.Filters
{
    public static class ContactsExceptionFilter
    {
        public static WebApplication UseContactsExceptionFilter(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var statusCode = HttpStatusCode.BadRequest;       
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    if (exception is EntityNotFoundException)
                    {
                        statusCode = HttpStatusCode.NotFound;
                    }

                    context.Response.StatusCode = (int)statusCode;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        Message = exception?.Message ?? "An unknow error occured",
                        Success = false
                    };

                    await context.Response.WriteAsJsonAsync(response);
                });
            });

            return app;
        }
    }
}
