
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