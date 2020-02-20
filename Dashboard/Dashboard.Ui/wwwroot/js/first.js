// the AAD application
var msalInstance;

(function () {

    // Enter Global Config Values & Instantiate MSAL Client application
    window.config = {
        //clientID: 'ff6adcea3-7043-4b27-a5df-5e5f77c4c112',
        //tenantID: '72f988bf-86f1-41af-91ab-2d7cd011db47'
        clientID: '9bb00cc0-2ff9-4786-8bf4-b2598a066e0b',
        tenantID: 'ae8ad9d2-e715-4c56-bc0b-e743cd2b2588'
    };

    if (!msalInstance) {
        const config = {
            auth: {
                clientId: window.config.clientID,
                authority: 'https://login.microsoftonline.com/' + window.config.tenantID
            }
        };
        msalInstance = new Msal.UserAgentApplication(config);

        msalInstance.handleRedirectCallback((error, response) => {
            console.log('handle redirect');
            onSignin(response.idToken);
        });
    }    
    
    
    var loginRequest = {
        scopes: ["user.read"]
    };

    $("#login").click(function () {
        msalInstance.loginRedirect(loginRequest);
    });
    //msalInstance.loginRedirect(loginRequest);

    if (msalInstance.getAccount() && !msalInstance.isCallback(window.location.hash)) {
        onSignin("blah-blah");
    } else {
        //force the user to sign in
        //msalInstance.loginRedirect(loginRequest);
    }
    
    //msalInstance.loginPopup(loginRequest)
    //    .then(response => {
    //        onSignin(response.idToken);
    //    })
    //    .catch(err => {
    //        console.log('ERROR: ' + err);
    //    });


    function onSignin(idToken) {
        console.log('ID token: ', idToken);
        var account = msalInstance.getAccount();
        if (account) {
            console.log('Logged user: ' + account.name);
            var tokenRequest = {
                scopes: ["api://9bb00cc0-2ff9-4786-8bf4-b2598a066e0b/User.Login"]
            };
            msalInstance.acquireTokenSilent(tokenRequest)
                .then(response => {
                    redirect(idToken, response.accessToken);
                })
                .catch(err => {
                    if (err.name === "InteractionRequiredAuthError") {
                        //return msalInstance.acquireTokenPopup(tokenRequest)
                        return msalInstance.acquireTokenRedirect(tokenRequest)
                            .then(response => {
                                redirect(idToken, response.accessToken);
                            })
                            .catch(err => {
                                console.log('ERROR: ' + err);
                            });
                    }
                });
        } else {
            var loginRequest = {
                scopes: ["user.read"]
            };

            msalInstance.loginPopup(loginRequest)
                .then(response => {
                    onSignin(response.idToken);
                })
                .catch(err => {
                    console.log('ERROR: ' + err);
                });
        }
    }

    function redirect(idToken, accessToken) {
        document.location.href = "https://localhost:44396/signin-oidc?access_token=" + accessToken + "&id_token=" + idToken;
        //console.log('access token: ' + accessToken);
        //$.ajax({
        //    type: "GET",
        //    url: "https://localhost:44371/WeatherForecast",
        //    headers: {
        //        'Authorization': 'Bearer ' + accessToken,
        //    }
        //}).done(function (data) {
        //    console.log(data);
        //}).fail(function (jqXHR, textStatus) {
        //    console.log('ERROR:' + textStatus);
        //});
    }
}());




