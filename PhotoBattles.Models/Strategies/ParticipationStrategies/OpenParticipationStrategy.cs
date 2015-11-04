namespace PhotoBattles.Models.Strategies.ParticipationStrategies
{
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class OpenParticipationStrategy : IParticipationStrategy
    {
        private readonly IContest contest;

        public OpenParticipationStrategy(IContest contest)
        {
            this.contest = contest;
        }

        public bool CanParticipate(string userName)
        {
            if (!this.contest.Participants.Any() || this.contest.Participants.All(p => p.UserName != userName))
            {
                if (this.contest.IsActive)
                {
                    return true;
                }
            }

            return false;
        }
    }
}