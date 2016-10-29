$(document).ready(function () {
    activateHtmlEditor();
    activateChosen();
    activateCroppie();
    activateFileInputs();
    addDefinedImageLS();
});

function addDefinedImageLS() {
    $('.addDefinedImageLS').on('click', function () {
        $('.addDefinedImageLS').removeClass('active');
        $(this).addClass('active');
        $('#ExistingImageName').val($(this).attr('data-name'));
    });
}

function activateHtmlEditor() {
    if (undefined != window.tinymce) {
        tinymce.init({
            selector: 'textarea.editor',
            height: 500,
            theme: 'modern',
            plugins: [
              'advlist autolink lists link image charmap print preview hr anchor pagebreak',
              'searchreplace wordcount visualblocks visualchars code fullscreen',
              'insertdatetime media nonbreaking save table contextmenu directionality',
              'emoticons template paste textcolor colorpicker textpattern imagetools'
            ],
            toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
            toolbar2: 'print preview media | forecolor backcolor emoticons',
            image_advtab: true,
            templates: [
              { title: 'Test template 1', content: 'Test 1' },
              { title: 'Test template 2', content: 'Test 2' }
            ],
            content_css: [
              '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
              '//www.tinymce.com/css/codepen.min.css'
            ]
        });
    }
}

function activateChosen() {
    if ($.isFunction($.fn.chosen)) {
        $('select.chosen').chosen({ inherit_select_classes: true });
    }
}

function activateCroppie() {
    if ($.isFunction($.fn.croppie)) {
        $uploadCrop = $('#upload-image').croppie({
            viewport: {
                width: 100,
                height: 100,
                type: 'circle'
            },
            boundary: {
                width: 300,
                height: 300
            },
            enableExif: true
        });
        var $uploadCrop;

        function readFile(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('.upload-image').addClass('ready');
                    $uploadCrop.croppie('bind', {
                        url: e.target.result
                    }).then(function () {
                        console.log('jQuery bind complete');
                    });

                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
                console.log("Sorry - you're browser doesn't support the FileReader API");
            }
        }
        $('#upload').on('change', function () { readFile(this); });
        $('.upload-image-result').on('click', function (ev) {
            $uploadCrop.croppie('result', {
                type: 'canvas',
                size: 'viewport'
            }).then(function (resp) {
                $('#ImageCropped').val(resp);
            });
        });
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
