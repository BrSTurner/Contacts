using FIAP.Contacts.SharedKernel.DomainObjects;
using System.Data.Common;

namespace FIAP.Contacts.SharedKernel.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        ValueTask<TEntity?> GetByIdAsync(Guid id);
        Task<List<TEntity>> GetAllAsync();
    }
}
