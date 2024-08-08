using FIAP.Contacts.Application.Contacts.Queries;
using FIAP.Contacts.Domain.Contacts.Repositories;
using FIAP.Contacts.Infrastructure.Contacts.Queries;
using FIAP.Contacts.Infrastructure.Contacts.Repositories;
using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.Infrastructure.UoW;
using FIAP.Contacts.SharedKernel.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.Contacts.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactQueries, ContactQueries>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<FIAPContext>(c => c.UseSqlServer(configuration.GetConnectionString("Default")));

            return services;
        }
    }
}
