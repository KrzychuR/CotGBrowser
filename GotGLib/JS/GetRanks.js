
function CotGBrowser_GetRanks(aType, aContinent) {
    //wyciągam sobie bazowy adres
    var _baseURL = document.URL.split("World00.php")[0];

    var callback = function (dataReceived) {
        hobbita.rankings(dataReceived, aType, aContinent);
    };

    var failCallback = function (jqXHR, textStatus) {
        hobbita.failed(textStatus, aType, aContinent);
    };

    //a=0 - punktacja graczy na kont
    //a=1 - punktacja sojuszy na kont
    //a=5 - units kills (total)
    //a=4 - def rep
    //a=3 - off rep
    //a=6 - plundered
    //a=7 - lochy
    //a=20 - military, aliance (nie dzialaja kontynenty!)

    var params = "a=" + aType;

    if (aContinent != 'x')
        params = params + "&b=" + aContinent;

    $.ajax({
        url: _baseURL + "includes/gR.php",
        type: "POST",
        data: params,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        //success: callback
    })
    .done(callback)
    .fail(failCallback);
}
