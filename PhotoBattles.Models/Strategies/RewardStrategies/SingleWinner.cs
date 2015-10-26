namespace PhotoBattles.Models.Strategies
{
    using System.Collections.Generic;
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class SingleWinner : IRewardStrategy
    {
        private readonly Contest contest;

        public SingleWinner(Contest contest)
        {
            this.contest = contest;
        }

        public ICollection<User> GetWiners()
        {
            return this.contest.Winners.Take(1).ToList();
        }
    }
}