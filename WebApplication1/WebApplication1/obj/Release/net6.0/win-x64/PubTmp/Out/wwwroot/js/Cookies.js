function getCookie(name) {
    var cookieArr = document.cookie.split(";");
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        if (name == cookiePair[0].trim()) {
            return decodeURIComponent(cookiePair[1]);
        }
    }
    return null;
}

function sub(i) {
    var utf8Encode = new TextEncoder();
    var value = getCookie("id");
    if (value != "" || document.cookie.indexOf('id=') != -1) {
        value = (Number(value) + 1).toString();
        document.cookie = 'country' + Number(value) + '=' + utf8Encode.encode(document.getElementById('Country-' + i).innerText).toString().replace(/,/g, '-');
        document.cookie = 'city' + Number(value) + '=' + utf8Encode.encode(document.getElementById('City-' + i).innerText).toString().replace(/,/g, '-');
        document.cookie = 'id=' + value;
        alert('You are subscribe to an ' + document.getElementById('City-' + i).innerText + ' weather')
    } else {
        value = '1';
        document.cookie = 'country' + Number(value) + '=' + utf8Encode.encode(document.getElementById('Country-' + i).innerText).toString().replace(/,/g, '-');
        document.cookie = 'city' + Number(value) + '=' + utf8Encode.encode(document.getElementById('City-' + i).innerText).toString().replace(/,/g, '-');
        document.cookie = 'id=' + value;
        alert('You are subscribe to an ' + document.getElementById('City-' + i).innerText + ' weather')
    }
}

function deleteAllCookies() {
    var cookies = document.cookie.split(";");

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }
}

function deleteCookie(i) {
    var decoder = new TextDecoder('utf-8'), decodedMessage;
    var country = document.getElementById('Country-' + i).innerText;
    var city = document.getElementById('City-' + i).innerText;
    var cookieArr = document.cookie.split(";");
    var cityOrCountryCookie;
    var countryCookieName;
    var countryCookieVal;
    var value = getCookie("id");
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");
        cityOrCountryCookie = decoder.decode(new Uint8Array(cookiePair[1].split('-'))).toString();
        console.log(city + "   " + cityOrCountryCookie + "   " + country);
        if (city == cityOrCountryCookie) {
            countryCookieName = "country" + cookiePair[0].split("city")[1];
            countryCookieVal = decoder.decode(new Uint8Array(getCookie(countryCookieName).split('-'))).toString();
            if (countryCookieVal == country) {
                console.log("deleting");
                console.log(cookiePair[0]);
                document.cookie = cookiePair[0] + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
                document.cookie = countryCookieName + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
                value = (Number(value) - 1).toString();
                alert('deleted');
            }
        }
    }
    document.cookie = 'id=' + value + ';path=/';
}