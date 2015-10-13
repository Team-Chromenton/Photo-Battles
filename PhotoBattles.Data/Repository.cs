namespace PhotoBattles.Data
{
    using System.Data.Entity;
    using System.Linq;

    using PhotoBattles.Data.Contracts;

    public class Repository<T> : IRepository<T>
        where T : class
    {
        private DbContext context;

        private IDbSet<T> set;

        public Repository()
            : this(new PhotoBattlesContext())
        {
        }

        public Repository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public T Find(object id)
        {
            return this.set.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return this.set;
        }

        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        private void ChangeEntityState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}