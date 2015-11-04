namespace PhotoBattles.Models.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IContest
    {
        ICollection<Photo> Photos { get; }

        ICollection<User> RegisteredVoters { get; }

        ICollection<User> RegisteredParticipants { get; }

        ICollection<User> Participants { get; }

        ICollection<User> Winners { get; }

        int NumberOfWinners { get; }

        DateTime? EndDate { get; }

        int? ParticipantsLimit { get; }

        bool IsActive { get; set; }

        bool IsOpen { get; set; }
    }
}