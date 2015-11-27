/*
* Validation Javascript
*/

var FormValidations = function () {
    // Init Login Form Validation, for more examples you can check out https://github.com/jzaefferer/jquery-validation
    var initValidationLogin = function () {
        jQuery('.js-validation-login').validate({
            errorClass: 'help-block text-right animated fadeInDown',
            errorElement: 'div',
            errorPlacement: function (error, e) {
                jQuery(e).parents('.form-group .form-material').append(error);
            },
            highlight: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error').addClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            success: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            rules: {
                'Username': {
                    required: true,
                    minlength: 3
                },
                'Password': {
                    required: true,
                    minlength: 5
                }
            },
            messages: {
                'Username': {
                    required: 'Please enter a username',
                    minlength: 'Your username must consist of at least 3 characters',

                },
                'Password': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long'
                }
            }
        });
    };
    // Init Register Form Validation
    var initValidationRegister = function () {
        jQuery('.js-validation-register').validate({
            errorClass: 'help-block text-right animated fadeInDown',
            errorElement: 'div',
            errorPlacement: function (error, e) {
                jQuery(e).parents('.form-group .form-material').append(error);
            },
            highlight: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error').addClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            success: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            rules: {
                'Username': {
                    required: true,
                    minlength: 3,
                    remote: { url: '/doesUsernameExists', type: 'POST'}
                },
                'Password': {
                    required: true,
                    minlength: 5
                },
                'Email': {
                    required: true,
                    email: true
                },
                'Password2': {
                    required: true,
                    equalTo: '#Password'
                },
                'AgreeWithTerms': {
                    required: true
                }
            },
            messages: {
                'Username': {
                    required: 'Please enter a username',
                    minlength: 'Your username must consist of at least 3 characters'
                },
                'Password': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long'
                },
                'Email': 'Please enter a valid email address',
                'Password2': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long',
                    equalTo: 'Please enter the same password as above'
                },
                'AgreeWithTerms': {
                    required: 'You must agree with the terms of service'
                }
            }
        });
    };
    // Init Profile Form Validation (form that updates the account's password)
    var initValidationProfile = function () {
        jQuery('.js-validation-profile').validate({
            errorClass: 'help-block text-right animated fadeInDown',
            errorElement: 'div',
            errorPlacement: function (error, e) {
                jQuery(e).parents('.form-group .form-material').append(error);
            },
            highlight: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error').addClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            success: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            rules: {
                'OldPassword': {
                    required: true,
                    minlength: 5/*,
                    remote: { url: '/isSamePassword', type: 'POST'}*/
                },
                'NewPassword': {
                    required: true,
                    minlength: 5
                },
                'NewPassword2': {
                    required: true,
                    minlength: 5,
                    equalTo: "#NewPassword"
                }
            },
            messages: {
                'OldPassword': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long'
                },
                'NewPassword': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long'
                },
                'NewPassword2': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long',
                    equalTo: 'Please enter the same password as above'
                }
            }
        });
    };
    // Init Profile2 Form Validation (form that updates the account's Gmail information)
    var initValidationProfile2 = function () {
        jQuery('.js-validation-profile2').validate({
            errorClass: 'help-block text-right animated fadeInDown',
            errorElement: 'div',
            errorPlacement: function (error, e) {
                jQuery(e).parents('.form-group .col-md-12').append(error);
            },
            highlight: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error').addClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            success: function (e) {
                jQuery(e).closest('.form-group').removeClass('has-error');
                jQuery(e).closest('.help-block').remove();
            },
            rules: {
                'GmailAddress': {
                    required: true,
                    email: true
                },
                'GmailPassword': {
                    required: true,
                    minlength: 5
                }
            },
            messages: {
                'GmailAddress': 'Please enter a valid email address',
                'GmailPassword': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 5 characters long'
                }
            }
        });
    };

    return {
        init: function () {
            // Init Validation
            initValidationLogin();
            initValidationRegister();
            initValidationProfile();
            initValidationProfile2();
        }
    };
} ();

// Initialize when page loads
jQuery(function () { FormValidations.init(); });