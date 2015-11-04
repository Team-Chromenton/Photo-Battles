$(function () {
    $.connection.hub.start().done(function() {
        console.log("Hub component initiated ...");
    });

    // Voting Hub
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

    // Contest Information Hub
    var contestInfoHub = $.connection.contestInfoHub;

    function infoExpiredContest(contestTitle) {
        $("#info-messages").append("<li>Contest " + contestTitle + " has expired.</li>");
    }

    contestInfoHub.client.infoExpiredContest = infoExpiredContest;
})