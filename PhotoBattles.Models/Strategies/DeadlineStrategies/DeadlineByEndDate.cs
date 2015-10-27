namespace PhotoBattles.Models.Strategies.DeadlineStrategies
{
    using System;

    using PhotoBattles.Models.Contracts;

    internal class DeadlineByEndDate : IDeadlineStrategy
    {
        private readonly Contest contest;

        public DeadlineByEndDate(Contest contest, DateTime deadline)
        {
            this.contest = contest;
            this.contest.EndDate = deadline;
        }

        public void Expire()
        {
            if (DateTime.Now > this.contest.EndDate)
            {
                this.contest.End();
            }
        }
    }
}