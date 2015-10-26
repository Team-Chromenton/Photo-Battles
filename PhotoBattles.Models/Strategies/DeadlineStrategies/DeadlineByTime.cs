namespace PhotoBattles.Models.Strategies.DeadlineStrategies
{
    using System;

    using PhotoBattles.Models.Contracts;

    internal class DeadlineByTime : IDeadlineStrategy
    {
        private readonly Contest contest;

        public DeadlineByTime(Contest contest, DateTime deadline)
        {
            this.contest = contest;
            this.contest.Deadline = deadline;
        }

        public void Expire()
        {
            if (DateTime.Now > this.contest.Deadline)
            {
                this.contest.End();
            }
        }
    }
}