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
            var winner = this.contest.Participants
                .Where(p => p.Votes.Count > 0)
                .OrderByDescending(p => p.Votes.Count)
                .FirstOrDefault();

            this.contest.Winners.Add(winner);
        }
    }
}