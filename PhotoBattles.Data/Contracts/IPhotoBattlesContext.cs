namespace PhotoBattles.Data.Contracts
{
    using System.Data.Entity;

    using PhotoBattles.Models;

    public interface IPhotoBattlesContext
    {
        IDbSet<Contest> Contests { get; }

        IDbSet<Photo> Photos { get; }

        IDbSet<Vote> Votes { get; }
    }
}