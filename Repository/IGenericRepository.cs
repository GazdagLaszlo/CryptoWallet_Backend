using System.Linq.Expressions;

namespace CryptoWallet.Repository
{
    //Köztes absztrakciós réteg felépítése a végpontok és az adatbázis között
    //Ezt utána a kontrollerben használhatjuk, ezzel elrejtve az adatbázis kapcsolatot

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null, string[]? includeCollections = null);
        //includeReferences - kapcsolódó objektum
        //includeCollections - kapcsolódó gyűjtemény(lista)
        Task<TEntity?> GetByIdAsync(object id, string[]? includeReferences = null, string[]? includeCollections = null);
        Task PostAsync(TEntity entity);

        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
