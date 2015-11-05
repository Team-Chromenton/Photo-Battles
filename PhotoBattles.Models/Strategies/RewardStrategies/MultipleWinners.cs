namespace PhotoBattles.Models.Strategies.RewardStrategies
{
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class MultipleWinners : IRewardStrategy
    {
        private readonly IContest contest;

        public MultipleWinners(IContest contest)
        {
            this.contest = contest;
        }

        public void SetWinners()
        {
            var winners = this.contest.Photos
                .OrderByDescending(p => p.Votes.Sum(v => v.Score))
                .Select(p => p.Author)
                .Take(this.contest.NumberOfWinners)
                .ToList();

            winners.ForEach(this.contest.Winners.Add);
        }
    }
}