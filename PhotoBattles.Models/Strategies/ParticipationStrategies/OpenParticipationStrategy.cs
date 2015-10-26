namespace PhotoBattles.Models.Strategies.ParticipationStrategies
{
    using PhotoBattles.Models.Contracts;

    internal class OpenParticipationStrategy : IParticipationStrategy
    {
        public bool CanParticipate(string user)
        {
            return true;
        }
    }
}