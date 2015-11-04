$(function() {
    $.connection.hub.start().done(function() {
        console.log("Hub component initiated ...");
    });

    // Voting Hub
    var votingHub = $.connection.votingHub;
    var contestInfoHub = $.connection.contestInfoHub;

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

    function infoExpiredContest(contestTitle, contestId) {
        $("#info-messages-container").attr('style', 'display: block');

        $("#info-messages").append("<li>Contest " + contestTitle + " has expired.</li>");
        $("#contest-" + contestId).attr('class', 'panel panel-default');

    }

    contestInfoHub.client.infoExpiredContest = infoExpiredContest;
});