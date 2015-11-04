namespace PhotoBattles.Models.Strategies.DeadlineStrategies
{
    using System;

    using PhotoBattles.Models.Contracts;

    public class DeadlineByEndDate : IDeadlineStrategy
    {
        private readonly IContest contest;

        public DeadlineByEndDate(IContest contest)
        {
            this.contest = contest;
        }

        public bool Expire()
        {
            if (this.contest.IsActive && DateTime.Now > this.contest.EndDate)
            {
                this.contest.IsActive = false;
                this.contest.IsOpen = false;
                return true;
            }

            return false;
        }
    }
}