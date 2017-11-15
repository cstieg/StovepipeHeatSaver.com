(function toggleVideo() {
    $('#watch-video-tab').on('click', { toggleTarget: '#watch-video' }, toggleHidden);
})();

function toggleHidden(e) {
    $(e.data.toggleTarget).toggleClass('no-height');
}