using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.Infrastructure.Contacts.Repositories;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.UoW;
using FIAP.Contacts.SharedKernel.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.Contacts.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<FIAPContext>(c => c.UseInMemoryDatabase("FiapContacts"));


            return services;
        }
    }
}
