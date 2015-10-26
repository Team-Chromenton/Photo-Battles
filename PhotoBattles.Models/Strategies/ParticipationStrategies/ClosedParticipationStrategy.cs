namespace PhotoBattles.Models.Strategies.ParticipationStrategies
{
    using System.Collections.Generic;

    using PhotoBattles.Models.Contracts;

    public class ClosedParticipationStrategy : IParticipationStrategy
    {
        private IList<string> participants = null;

        public ClosedParticipationStrategy(string[] participants)
        {
            this.participants = participants;
        }

        public bool CanParticipate(string user)
        {
            if (this.participants.Contains(user))
            {
                return true;
            }

            return false;
        }
    }
}