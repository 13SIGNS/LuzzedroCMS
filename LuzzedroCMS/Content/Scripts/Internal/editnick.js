function OnEditNickSuccess(data) {
    if (data.IsNickAdded) {
        $("#edit-comment-form").removeClass("hidden");
        $(".edit-nick-form").hide();
    } else {
        $("#nick-validation").text(data.Error);
    }
    console.log(data.IsNickAdded);
}