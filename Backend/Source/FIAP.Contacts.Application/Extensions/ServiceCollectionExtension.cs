using FIAP.Contacts.Application.Contacts.Pofiles;
using FIAP.Contacts.Application.Contacts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.Contacts.Application.Extensions
{
    public static class ServiceCollectionExtension 
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            services.AddScoped<IContactAppService, ContactAppService>();
            services.AddAutoMapper(typeof(ContactProfile));
            return services;
        }
    }
}
