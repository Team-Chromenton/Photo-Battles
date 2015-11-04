namespace PhotoBattles.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Contracts;
    using PhotoBattles.Models.Enumerations;
    using PhotoBattles.Models.Strategies.DeadlineStrategies;
    using PhotoBattles.Models.Strategies.ParticipationStrategies;
    using PhotoBattles.Models.Strategies.RewardStrategies;
    using PhotoBattles.Models.Strategies.VotingStrategies;

    public class ContestViewModel : IMapFrom<Contest>, ICustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserViewModel Organizer { get; set; }

        //// Strategies - Enum
        public VotingStrategyEnum VotingStrategyEnum { get; set; }

        public ParticipationStrategyEnum ParticipationStrategyEnum { get; set; }

        public RewardStrategyEnum RewardStrategyEnum { get; set; }

        public DeadlineStrategyEnum DeadlineStrategyEnum { get; set; }
        //// Strategies - Enum

        public int? ParticipantsLimit { get; set; }

        public DateTime? EndDate { get; set; }

        public int? NumberOfWinners { get; set; }

        public bool IsActive { get; set; }

        public bool IsOpen { get; set; }

        public ICollection<UserViewModel> InvitedUsers { get; set; }

        public ICollection<UserViewModel> Participants { get; set; }

        public ICollection<UserViewModel> AvailableParticipants { get; set; }

        public ICollection<UserViewModel> AvailableVoters { get; set; }

        //// Strategies
        public IVotingStrategy GetVotingStrategy(IContest contest)
        {
            if (this.VotingStrategyEnum == VotingStrategyEnum.Open)
            {
                return new OpenVotingStartegy(contest);
            }

            return new ClosedVotingStartegy(contest);
        }

        public IParticipationStrategy GetParticipationStrategy(IContest contest)
        {
            if (this.ParticipationStrategyEnum == ParticipationStrategyEnum.Open)
            {
                return new OpenParticipationStrategy(contest);
            }

            return new ClosedParticipationStrategy(contest);
        }

        public IRewardStrategy GetRewardStrategy(IContest contest)
        {
            if (this.RewardStrategyEnum == RewardStrategyEnum.SingleWinner)
            {
                return new SingleWinner(contest);
            }
            else
            {
                return new MultipleWinners(contest);
            }
        }

        public IDeadlineStrategy GetDeadlineStrategy(IContest contest)
        {

            if (this.DeadlineStrategyEnum == DeadlineStrategyEnum.EndDate)
            {
                return new DeadlineByEndDate(contest);
            }
            else
            {
                return new DeadlineByParticipantsLimit(contest);
            }
        }
        //// Strategies

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModel>()
                         .ForMember(c => c.InvitedUsers, opt => opt.MapFrom(c => c.RegisteredParticipants))
                         .ForMember(c => c.Participants, opt => opt.MapFrom(c => c.Participants));
        }
    }
}