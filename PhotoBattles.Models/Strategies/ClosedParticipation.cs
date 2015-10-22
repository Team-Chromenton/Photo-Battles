﻿namespace PhotoBattles.Models.Strategies
{
    using PhotoBattles.Models.Contracts;

    public class ClosedParticipation : IParticipationStrategy
    {
        public bool CanParticipate(User user, Contest contest)
        {
            if (contest.Participants.Contains(user))
            {
                return true;
            }

            return false;
        }
    }
}