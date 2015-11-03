namespace PhotoBattles.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    public class EditContestViewModel : IMapFrom<Contest>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public VotingStrategy VotingStrategy { get; set; }

        public ParticipationStrategy ParticipationStrategy { get; set; }

        public int? NumberOfWinners { get; set; }

        public RewardStrategy RewardStrategy { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ParticipantsLimit { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}