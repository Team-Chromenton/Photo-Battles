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
            if (this.contest.Participants.Any(p => p.UserName != userName))
            {
                return true;
            }

            return false;
        }
    }
}