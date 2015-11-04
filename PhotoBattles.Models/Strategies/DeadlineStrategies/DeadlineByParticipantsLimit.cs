namespace PhotoBattles.Models.Strategies.DeadlineStrategies
{
    using PhotoBattles.Models.Contracts;

    public class DeadlineByParticipantsLimit : IDeadlineStrategy
    {
        private readonly IContest contest;

        public DeadlineByParticipantsLimit(IContest contest)
        {
            this.contest = contest;
        }

        public bool Expire()
        {
            if (this.contest.IsActive && this.contest.RegisteredParticipants.Count > this.contest.ParticipantsLimit)
            {   
                this.contest.IsActive = false;
                this.contest.IsOpen = false;
                return true;
            }

            return false;
        }
    }
}