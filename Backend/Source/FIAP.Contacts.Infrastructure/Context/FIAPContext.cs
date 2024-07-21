using FIAP.Contacts.Infrastructure.Contacts.Mapping;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contacts.Infrastructure.Context
{
    public class FIAPContext : DbContext
    {
        public FIAPContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
