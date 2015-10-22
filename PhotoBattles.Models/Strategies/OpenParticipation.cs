namespace PhotoBattles.Models.Strategies
{
    using PhotoBattles.Models.Contracts;

    internal class OpenParticipation : IParticipationStrategy
    {
        public bool CanParticipate(User user, Contest contest)
        {
            return true;
        }
    }
}