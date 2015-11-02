namespace PhotoBattles.App.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("votingHub")]
    public class VotingHub : Hub
    {
        public void UpdateVote(int id)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<VotingHub>();
            hubContext.Clients.All.updateVote(id);
        }
    }
}