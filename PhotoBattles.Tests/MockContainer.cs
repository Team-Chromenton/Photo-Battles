namespace PhotoBattles.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    public class MockContainer
    {
        public Mock<IRepository<User>> FakeUserRepository { get; set; }

        public Mock<IRepository<Contest>> FakeContestRepository { get; set; }

        public ICollection<User> FakeUsers { get; set; }

        public ICollection<Contest> FakeContests { get; set; }

        public void PrepareMock()
        {
            this.SetupFakeUsers();
            this.SetupFakeContests();
        }

        private void SetupFakeContests()
        {
            var fakeContets = new List<Contest>()
                {
                    new Contest()
                        {
                            Id = 1,
                            Title = "Contest one",
                            Description = "Contests one description",
                            CreatedOn = DateTime.Now.AddDays(-10),
                            IsActive = true,
                            IsOpen = true,
                            OrganizerId = "fakeuseroneid",
                            VotingStrategy = VotingStrategy.Open,
                            ParticipationStrategy = ParticipationStrategy.Open,
                            RewardStrategy = RewardStrategy.SingleWinner,
                            DeadlineStrategy = DeadlineStrategy.ParticipantsLimit,
                            ParticipantsLimit = 10
                        },
                    new Contest()
                        {
                            Id = 2,
                            Title = "Contest two",
                            Description = "Contests two description",
                            CreatedOn = DateTime.Now.AddDays(-5),
                            IsActive = true,
                            IsOpen = true,
                            OrganizerId = "fakeusertwoid",
                            VotingStrategy = VotingStrategy.Open,
                            ParticipationStrategy = ParticipationStrategy.Open,
                            RewardStrategy = RewardStrategy.SingleWinner,
                            DeadlineStrategy = DeadlineStrategy.ParticipantsLimit,
                            ParticipantsLimit = 5
                        }
                };

            this.FakeContestRepository = new Mock<IRepository<Contest>>();

            this.FakeContestRepository
                .Setup(c => c.GetAll())
                .Returns(fakeContets.AsQueryable());

            this.FakeContestRepository
                .Setup(c => c.Find(It.IsAny<int>()))
                .Returns((int id) => fakeContets[id]);
        }

        private void SetupFakeUsers()
        {
            var fakeUsers = new List<User>()
                {
                    new User()
                        {
                            Id = "fakeuseroneid",
                            UserName = "FakeUserOne",
                            Email = "fakeuserone@example.com",
                            FirstName = "Fake User",
                            LastName = "One"
                        },
                    new User()
                        {
                            Id = "fakeusertwoid",
                            UserName = "FakeUserTwo",
                            Email = "fakeusertwo@example.com",
                            FirstName = "Fake User",
                            LastName = "Two"
                        }
                };

            this.FakeUserRepository = new Mock<IRepository<User>>();

            this.FakeUserRepository
                .Setup(r => r.GetAll())
                .Returns(fakeUsers.AsQueryable());

            this.FakeUserRepository
                .Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) => fakeUsers[id]);
        }
    }
}