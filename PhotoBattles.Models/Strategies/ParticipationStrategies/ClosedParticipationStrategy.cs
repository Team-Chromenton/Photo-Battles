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
            if (this.contest.IsActive && this.contest.RegisteredParticipants.Any(rp => rp.UserName == userName))
            {
                if (!this.contest.Participants.Any() || this.contest.Participants.All(p => p.UserName != userName))
                {
                    if (this.contest.IsActive)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}