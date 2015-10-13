namespace PhotoBattles.Data.Contracts
{
    using PhotoBattles.Models;

    public interface IPhotoBattlesData
    {
        IRepository<User> Users { get; }

        IRepository<Contest> Contests { get; }

        IRepository<Photo> Photos { get; }

        IRepository<Vote> Votes { get; }

        void SaveChanges();
    }
}