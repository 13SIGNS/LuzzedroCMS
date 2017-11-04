$(document).ready(function () {
    activateChosen();
    activateFileInputs();
    activateDatepickers();
});

function activateChosen() {
    if ($.isFunction($.fn.chosen)) {
        $('select.chosen').chosen({ inherit_select_classes: true });
    }
}

function activateFileInputs() {
    $('.btn-file input[type="file"').on('change', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });
    $(document).ready(function () {
        $('.btn-file input[type="file"').on('fileselect', function (event, numFiles, label) {
            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' files selected' : label;
            if (input.length) {
                input.val(log);
            } else {
                if (log) { console.log(log); }
            }
        });
    });
}

function activateDatepickers() {
    $('.datepicker').datepicker();
}
