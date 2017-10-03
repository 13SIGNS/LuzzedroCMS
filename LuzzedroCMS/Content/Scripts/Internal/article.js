function IncrementBookmarkCount() {
    $('.bookmarks-count').text(parseInt($('.bookmarks-count').text()) + 1);
    $('.add-bookmark').fadeOut();
}