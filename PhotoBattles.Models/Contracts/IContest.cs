namespace PhotoBattles.Models.Contracts
{
    using System;
    using System.Collections.Generic;

    using PhotoBattles.Models.Enumerations;

    public interface IContest
    {
        string Title { get; }

        string Description { get; }

        DateTime CreatedOn { get; }

        bool IsActive { get; set; }

        bool IsOpen { get; set; }

        User Organizer { get; }

        //// Strategies - Enum
        VotingStrategyEnum VotingStrategyEnum { get;  }

        ParticipationStrategyEnum ParticipationStrategyEnum { get; }

        RewardStrategyEnum RewardStrategyEnum { get; }

        DeadlineStrategyEnum DeadlineStrategyEnum { get; }
        //// Strategies - Enum

        int NumberOfWinners { get; }

        DateTime? EndDate { get; }

        int? ParticipantsLimit { get; }

        ICollection<Photo> Photos { get; }

        ICollection<User> RegisteredParticipants { get; }

        ICollection<User> Participants { get; }

        ICollection<User> RegisteredVoters { get; }

        ICollection<User> Winners { get; }
    }
}