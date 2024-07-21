using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.SharedKernel.UoW;

namespace FIAP.Contacts.Infrastructure.UoW
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FIAPContext _context;

        public UnitOfWork(FIAPContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync() => await _context.SaveChangesAsync() > 0;

        public void Dispose() => _context?.Dispose();        
    }
}
