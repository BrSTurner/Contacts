﻿using FIAP.Contacts.Infrastructure.Context;
using FIAP.Contacts.SharedKernel.DomainObjects;
using FIAP.Contacts.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FIAP.Contacts.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : 
        IBaseRepository<TEntity>,
        IDisposable
        where TEntity : Entity, IAggregateRoot
    {
        protected readonly FIAPContext _context;
        protected readonly DbSet<TEntity> _entity;

        protected BaseRepository(FIAPContext context)
        {
            _context = context;
            _entity = _context.Set<TEntity>();  
        }

        protected DbConnection DbConnection
        {
            get
            {
                return _context.Database.GetDbConnection();
            }
        }

        public void Add(TEntity entity) => _entity.Add(entity);
        public void Update(TEntity entity) => _entity.Update(entity);
        public void Remove(TEntity entity) => _entity.Remove(entity);
        public ValueTask<TEntity?> GetByIdAsync(Guid id) => _entity.FindAsync(id);
        public Task<List<TEntity>> GetAllAsync() => _entity.ToListAsync();
        public void Dispose() => _context?.Dispose();
    }
}
