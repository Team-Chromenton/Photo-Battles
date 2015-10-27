namespace PhotoBattles.Data.Contracts
{
    using PhotoBattles.Models;

    public interface IPhotoBattlesData
    {
        IRepository<User> Users { get; }

        IRepository<Contest> Contests { get; }

        IRepository<Photo> Photos { get; }

        IRepository<Vote> Votes { get; }

        IRepository<Commitee> Commitees { get; }

        void SaveChanges();
    }
}