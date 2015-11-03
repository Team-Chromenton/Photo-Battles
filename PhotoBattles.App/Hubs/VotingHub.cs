namespace PhotoBattles.App.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("votingHub")]
    public class VotingHub : Hub
    {
        public void IncreaseScore(int photoId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<VotingHub>();
            hubContext.Clients.All.increaseScore(photoId);
        }

        public void DecreaseScore(int photoId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<VotingHub>();
            hubContext.Clients.All.decreaseScore(photoId);
        }
    }
}