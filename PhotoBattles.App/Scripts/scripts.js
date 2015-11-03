$(function () {
    $.connection.hub.start().done(function() {
        console.log("Voting Hub Started!");
    });

    var votingHub = $.connection.votingHub;

    function increaseScore(photoId) {
        var value = parseInt($('#photo-' + photoId + '-score').text(), 10) + 1;
        $('#photo-' + photoId + '-score').text(value);
    }

    function decreaseScore(photoId) {
        var value = parseInt($('#photo-' + photoId + '-score').text(), 10) - 1;
        $('#photo-' + photoId + '-score').text(value);
    }

    votingHub.client.increaseScore = increaseScore;
    votingHub.client.decreaseScore = decreaseScore;
})