
function sub(item) {
    var cookieArr = document.cookie.split(";");
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        if (item == cookiePair[1]) {
            document.cookie = cookiePair[0] + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
        }
    }
    document.cookie = 'id' + item + '=' + item;
    alert('You are subscribe to a ' + item + ' weather');
}

async function sendDelete(item) {
    const response = await fetch("/api/delete/user", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: item
        })
    });
}

async function sendAdd(item) {
    const response = await fetch("/api/add/user", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: item
        })
    });
}

//LogOut
function signOut(signOutLink, signInLink) {
    var cookieArr = document.cookie.split(";");
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        if (" Login" == cookiePair[0] || " Password" == cookiePair[0]) {
            document.cookie = cookiePair[0] + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
        }
    }
    var signInLink = document.createElement('a');
    signInLink.id = 'Sign';
    signInLink.setAttribute('asp-action', 'Authentication');
    signInLink.setAttribute('asp-controller', 'SignIn');
    signInLink.textContent = 'Sign In';

    var existingSignLink = document.getElementById('Sign');

    // Replace the existing element with the new one
    if (existingSignLink && existingSignLink.parentNode) {
        existingSignLink.parentNode.replaceChild(signInLink, existingSignLink);
    }
}

function hasCookie(name) {
    var cookieArr = document.cookie.split(";");

    var cookieArr = document.cookie.split(";");
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        if (' Login' == cookiePair[0]) {
            return true;
        }
    }
    return false;
}

var hasLoginCookie = hasCookie("Login");
document.addEventListener('DOMContentLoaded', function () {
    if (hasLoginCookie) {
        var signInLink = document.getElementById('Sign');
        var signOutLink = document.createElement('a');
        signOutLink.id = 'Sign';
        signOutLink.textContent = 'Sign Out';

        signOutLink.onclick = signOut;

        signInLink.parentNode.replaceChild(signOutLink, signInLink);

    }
});