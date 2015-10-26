namespace PhotoBattles.Models.Strategies.VotingStrategies
{
    using PhotoBattles.Models.Contracts;

    public class OpenVotingStartegy : IVotingStrategy
    {
        public bool CanVote(string user)
        {
            return true;
        }
    }
}