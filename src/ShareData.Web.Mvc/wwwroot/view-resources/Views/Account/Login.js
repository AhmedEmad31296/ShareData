(function () {
    // Set the value of '#ReturnUrlHash' to the current hash
    $('#ReturnUrlHash').val(location.hash);

    // Select the '#LoginForm' element
    var $form = $('#LoginForm');

    // Attach a submit event handler to the form
    $form.submit(function (event) {
        event.preventDefault();  // Prevent the default form submission

        if (!$form.valid()) {
            return;  // Exit if the form is not valid
        }

        // Send an AJAX request
        abp.ui.setBusy(
            $('body'),
            abp.ajax({
                contentType: 'application/x-www-form-urlencoded',
                url: $form.attr('action'),
                data: $form.serialize()
            })
        );
    });
})();
