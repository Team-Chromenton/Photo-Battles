﻿namespace PhotoBattles.Models.Strategies
{
    using System.Collections.Generic;
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class TopNWinners : IRewardStrategy
    {
        public ICollection<User> GetWiners(int numberOfWinners, Contest contest)
        {
            return contest.Winners.Take(numberOfWinners).ToList();
        }
    }
}