namespace PhotoBattles.Models.Contracts
{
    public interface IVotingStrategy
    {
        bool CanVote(int photoId, string userName);
    }
}