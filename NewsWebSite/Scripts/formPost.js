$(document).ready(function () {
    $('#postForm').on('submit', function () {
        $('span[data-valmsg-for=FullDescription]').remove();
        $('span[data-valmsg-for=Image]').remove();
        if ($(this).find('.field-validation-error').length == 0) {
            var sub = $(this).find(':submit');
            $(sub).attr('disabled', 'disabled');
            $(sub).val('Loading...');
        }
    });
});