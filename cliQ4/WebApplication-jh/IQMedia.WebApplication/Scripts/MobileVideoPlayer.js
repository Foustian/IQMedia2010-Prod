function handleUpdate(player) {
    //console.log("update percent: " + percent);
    var seconds = player.currentTime;
    if (seconds >= player.dataset.nextLog) {
        //log call
        console.log("Next percent: " + player.dataset.nextLog);
        var url = sprintf(player.dataset.logUrlPattern, seconds);
        jQuery.getJSON(url, function (data) {
            console.log(data);
        });
        console.log("Updating seconds played: " + seconds);
        player.dataset.nextLog += player.dataset.timeInterval;
    }
}

var videoPlayer = document.getElementById('myvideo');

videoPlayer.dataset.nextLog = 0;
videoPlayer.dataset.timeInterval = 0;

videoPlayer.addEventListener('durationchange', function () {
    this.dataset.timeInterval = Math.round(self.duration * (parseFloat(this.dataset.updateInterval) * 0.01));
    if (this.dataset.playOffset > 0) {
        this.currentTime = this.dataset.playOffset;
        this.dataset.playOffset = 0;
    }
});

videoPlayer.addEventListener('timeupdate', function () {
    handleUpdate(this);
});

videoPlayer.addEventListener('ended', function () {
    handleUpdate(this);
});