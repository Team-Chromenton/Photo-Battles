namespace PhotoBattles.Models.Strategies.DeadlineStrategies
{
    using PhotoBattles.Models.Contracts;

    public class DeadlineByParticipantsLimit : IDeadlineStrategy
    {
        private readonly Contest contest;

        private readonly int numberOfParticipants;

        public DeadlineByParticipantsLimit(Contest contest, int numberOfParticipants)
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