namespace PhotoBattles.Models.Strategies.RewardStrategies
{
    using System.Collections.Generic;
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class SingleWinner : IRewardStrategy
    {
        private readonly IContest contest;

        public SingleWinner(IContest contest)
        {
            this.contest = contest;
        }

        public void SetWinners()
        {
            var winner = this.contest.Photos
                .OrderByDescending(p => p.Votes.Sum(v => v.Score))
                .Select(p => p.Author)
                .FirstOrDefault();

            this.contest.Winners.Add(winner);
        }
    }
}