namespace PhotoBattles.Models.Strategies.DeadlineStrategies
{
    using PhotoBattles.Models.Contracts;

    public class DeadlineByParticipants : IDeadlineStrategy
    {
        private readonly Contest contest;

        private readonly int numberOfParticipants;

        public DeadlineByParticipants(Contest contest, int numberOfParticipants)
        {
            this.contest = contest;
            this.numberOfParticipants = numberOfParticipants;
        }

        public void Expire()
        {
            if (this.contest.Participants.Count > this.numberOfParticipants)
            {
                this.contest.IsOpen = false;
            }
        }
    }
}