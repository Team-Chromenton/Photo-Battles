namespace PhotoBattles.Data.Contracts
{
    using System.Linq;

    public interface IRepository<T>
        where T : class
    {
        T Find(object id);

        IQueryable<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}