namespace PhotoBattles.Models.Strategies
{
    using System.Collections.Generic;
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class TopNWinners : IRewardStrategy
    {
        private readonly Contest contest;

        private readonly int numberOfWinners;

        public TopNWinners(Contest contest, int numberOfWinners)
        {
            this.contest = contest;
            this.numberOfWinners = numberOfWinners;
        }

        public ICollection<User> GetWiners()
        {
            return this.contest.Winners.Take(this.numberOfWinners).ToList();
        }
    }
}