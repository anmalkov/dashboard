(function () {

    $("#getDataButton").click(function () {
        getToken().done(function (data) {
            console.log('access token: ' + data.token);
            getWeather(data.token);
        }).fail(function (jqXHR, textStatus) {
            console.log('jqXHR', jqXHR);
            console.log('ERROR:' + textStatus);
            if (jqXHR.status == 401) {
                location.href = '/';
                //window.Location = location;
            }
        });
    });

    function getToken() {
        return $.ajax({
            type: "GET",
            url: "https://localhost:5001/api/token",
        });
    }

    function getWeather(accessToken) {
        $.ajax({
            type: "GET",
            url: "https://localhost:44371/WeatherForecast",
            headers: {
                'Authorization': 'Bearer ' + accessToken
            }
        }).done(function (data) {
            console.log(data);
        }).fail(function (jqXHR, textStatus) {
            console.log('ERROR:' + textStatus);
            if (jqXHR.status == 401) {
                // get new access token
            }
        });
    }
}());




