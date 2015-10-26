namespace PhotoBattles.Models.Contracts
{
    using System.Collections.Generic;

    public interface IRewardStrategy
    {
        ICollection<User> GetWiners();
    }
}