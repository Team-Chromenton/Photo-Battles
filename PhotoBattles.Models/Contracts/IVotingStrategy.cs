namespace PhotoBattles.Models.Contracts
{
    public interface IVotingStrategy
    {
        bool CanVote(User user, Contest contest);
    }
}