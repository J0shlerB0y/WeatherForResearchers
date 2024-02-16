
function sub(item) {
    var cookieArr = document.cookie.split(";");
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        if (item == cookiePair[1]) {
            document.cookie = cookiePair[0] + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
        }
    }
    document.cookie = 'id'+ item +'=' + item;
    alert('You are subscribe to a ' + item + ' weather');
}

function deleteCookie(item) {
    var cookieArr = document.cookie.split(";");
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        if (cookiePair[0] == "Login") {
            sendDelete(item);
            return;
        }
    }
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        if (item == cookiePair[1]) {
            document.cookie = cookiePair[0] + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
            alert('You are deleted a ' + item + ' weather');
            location.reload();
        }
    }
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