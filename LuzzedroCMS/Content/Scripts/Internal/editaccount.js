$(document).ready(function () {
    activateCroppie();
});

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
