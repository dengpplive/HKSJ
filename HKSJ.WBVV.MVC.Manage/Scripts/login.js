var Login = function () {

    return {
        //main function to initiate the module
        init: function () {

            $('.login-form').validate({
                errorElement: 'label', //default input error message container
                errorClass: 'help-inline', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                rules: {
                    loginName: {
                        required: true
                    },
                    password: {
                        required: true
                    },
                    remember: {
                        required: false
                    }
                },

                messages: {
                    loginName: {
                        required: "loginName is required."
                    },
                    password: {
                        required: "Password is required."
                    }
                },
                highlight: function (element) { // hightlight error inputs
                    $(element)
	                    .closest('.control-group').addClass('error'); // set error class to the control group
                },

                success: function (label) {
                    label.closest('.control-group').removeClass('error');
                    label.remove();
                },
                errorPlacement: function (error, element) {
                    $('.alert-error', $('.login-form')).hide();
                    error.addClass('help-small no-left-padding').insertAfter(element.closest('.input-icon'));
                }
            });
            $('.login-form input').keypress(function (e) {
                if (e.which === 13) {
                    if ($('.login-form').validate().form()) {
                        $('.login-form').submit();
                    }
                    return false;
                }
            });
        }
    };

}();
