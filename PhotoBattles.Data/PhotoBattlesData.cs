namespace PhotoBattles.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;

    public class PhotoBattlesData : IPhotoBattlesData
    {
        private DbContext context;

        private Dictionary<Type, object> repositories;

        public PhotoBattlesData()
            : this(new PhotoBattlesContext())
        {
        }

        public PhotoBattlesData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<Contest> Contests
        {
            get
            {
                return this.GetRepository<Contest>();
            }
        }

        public IRepository<Photo> Photos
        {
            get
            {
                return this.GetRepository<Photo>();
            }
        }

        public IRepository<Vote> Votes
        {
            get
            {
                return this.GetRepository<Vote>();
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof(T);
            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var type = typeof(Repository<T>);

                this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeOfModel];
        }
    }
}