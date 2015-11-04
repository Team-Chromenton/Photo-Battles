namespace PhotoBattles.Models.Strategies.ParticipationStrategies
{
    using System.Collections.Generic;
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class ClosedParticipationStrategy : IParticipationStrategy
    {
        private readonly IContest contest;

        public ClosedParticipationStrategy(IContest contest)
        {
            this.contest = contest;
        }

        public bool CanParticipate(string userName)
        {
            if (this.contest.RegisteredParticipants.Any(rp => rp.UserName == userName)
                && this.contest.Participants.Any(p => p.UserName != userName))
            {
                return true;
            }

            return false;
        }
    }
}