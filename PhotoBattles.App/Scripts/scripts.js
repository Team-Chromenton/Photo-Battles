$(function () {
    $.connection.hub.start().done(function() {
        console.log("Voting Hub Started!");
    });

    var votingHub = $.connection.votingHub;

    function updateVotes(id) {
        //TODO: getting votes and replace html
    }

    votingHub.client.updateVotes = updateVotes;
})