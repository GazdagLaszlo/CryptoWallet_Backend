using CryptoWallet.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CryptoWallet.Repository
{
    //Köztes absztrakciós réteg felépítése a végpontok és az adatbázis között
    //Ezt utána a kontrollerben használhatjuk, ezzel elrejtve az adatbázis kapcsolatot
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private AppDbContext _context;
        private DbSet<TEntity> _dbset;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }


        public async virtual Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null, string[]? includeCollections = null)
        {
            IQueryable<TEntity> query = _dbset;
            if (includeProperties != null)
            {
                foreach (string property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            if (includeCollections != null)
            {
                foreach (var collection in includeCollections)
                {
                    query = query.Include(collection);
                }
            }
            
            return await query.ToListAsync();
        }
        /*
        public async Task<TEntity?> GetByIdAsync(object id, string[]? includeReferences = null, string[]? includeCollections = null)
        {
            TEntity? entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            List<Task> loadTasks = new List<Task>();

            if (includeReferences != null)
            {
                foreach (var reference in includeReferences)
                {
                    await _context.Entry(entity).Reference(reference).LoadAsync();
                }
            }
            if (includeCollections != null)
            {
                foreach (var collection in includeCollections)
                {
                    await _context.Entry(entity).Collection(collection).LoadAsync();
                }
            }
            //await Task.WhenAll(loadTasks);
            return entity;
        }
        */

        /*
        public virtual TEntity? GetById(object id, string[]? includeReferences = null, string[]? includeCollections = null)
        {
            TEntity? entity = _dbset.Find(id);
            if (entity == null)
            {
                return null;
            }
            if (includeReferences != null)
            {
                foreach (var reference in includeReferences)
                {
                    _context.Entry(entity).Reference(reference).Load();
                }
            }
            if (includeCollections != null)
            {
                foreach (var collection in includeCollections)
                {
                    _context.Entry(entity).Collection(collection).Load();
                }
            }
            return entity;
        }
        */

        public async Task<TEntity?> GetByIdAsync(object id, string[]? includeReferences = null, string[]? includeCollections = null)
        {
            IQueryable<TEntity> query = _dbset;

            if (includeReferences != null)
            {
                foreach (var reference in includeReferences)
                {
                    query = query.Include(reference);
                }
            }

            if (includeCollections != null)
            {
                foreach (var collection in includeCollections)
                {
                    query = query.Include(collection);
                }
            }

            var keyProperty = _context.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties.FirstOrDefault();
            if (keyProperty == null)
                throw new InvalidOperationException("No key property found.");

            return await query.FirstOrDefaultAsync(e =>
                EF.Property<object>(e, keyProperty.Name).Equals(id));
        }

        public async Task PostAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dbset.Update(entity);
        }
    }
}
