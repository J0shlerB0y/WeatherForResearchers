
//function signOut() {
//    document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
//    window.location.reload();
//}

//function hasCookie(name) {
//    return document.cookie.split(';').some(c => c.trim().startsWith(name + '='));
//}

//document.addEventListener('DOMContentLoaded', function () {
//    var hasLoginCookie = hasCookie("accessToken");

//    if (hasLoginCookie) {
//        var signInLink = document.getElementById('Sign');

//        const authElements = document.querySelectorAll('.auth-required');
//        authElements.forEach(element => {
//            element.classList.remove('auth-required');
//        });

//        if (signInLink) {
//            const signOutLink = document.createElement('a');
//            signOutLink.id = 'Sign';
//            signOutLink.textContent = 'Sign Out';
//            signOutLink.style.cursor = 'pointer';

//            signOutLink.addEventListener('click', signOut);

//            signInLink.parentNode.replaceChild(signOutLink, signInLink);
//        }

//    }
//});

function onAjaxSuccess(data, status, xhr) {
    var $form = $(this);
    var $resultSpan = $form.find('.js-ajax-result');

    if (data.success) {
        $resultSpan.text(data.message).removeClass('text-danger').addClass('text-success');
    } else {
        $resultSpan.text(data.message || 'An unknown error occurred.').removeClass('text-success').addClass('text-danger');
    }
}

function onAjaxFailure(xhr, status, error) {
    var $form = $(this);
    var $resultSpan = $form.find('.js-ajax-result');

    var message = 'Error: ' + error;

    if (xhr.responseJSON && xhr.responseJSON.message) {
        message = xhr.responseJSON.message;
    }

    $resultSpan.text(message).removeClass('text-success').addClass('text-danger');
}