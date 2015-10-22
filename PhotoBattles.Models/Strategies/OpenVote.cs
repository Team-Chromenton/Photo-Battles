namespace PhotoBattles.Models.Strategies
{
    using PhotoBattles.Models.Contracts;

    public class OpenVote : IVotingStrategy
    {
        public bool CanVote(User user, Contest contest)
        {
            return true;
        }
    }
}