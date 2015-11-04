namespace PhotoBattles.Models.Strategies.VotingStrategies
{
    using System.Linq;

    using PhotoBattles.Models.Contracts;

    public class OpenVotingStartegy : IVotingStrategy
    {
        private readonly IContest contest;

        public OpenVotingStartegy(IContest contest)
        {
            this.contest = contest;
        }

        public bool CanVote(int photoId, string userName)
        {
            if (!this.contest.Photos.FirstOrDefault(p => p.Id == photoId).Votes.Any(v => v.Author.UserName == userName))
            {
                return true;
            }

            return false;
        }
    }
}