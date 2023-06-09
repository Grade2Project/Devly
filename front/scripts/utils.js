// const HOME = 'http://localhost:8080/back';
// const HOME = 'http://localhost:5003';
const HOME = 'https://signup-application.8kerlk9kt0tio.eu-north-1.cs.amazonlightsail.com/back';

const Controllers = {
    AUTH: {USER: `${HOME}/auth/user`, COMPANY: `${HOME}/auth/company`},
    REG: {USER: `${HOME}/reg`, COMPANY: `${HOME}/company/reg`},
    GRADES: `${HOME}/grades/get`,
    LANGS: `${HOME}/lang/get`,
    CITIES: {SIMILAR: `${HOME}/cities/similar`, ALL: `${HOME}/cities/all`},
    RESUME: {UPDATE: `${HOME}/resume/update`},
    VACANCY: {UPDATE: `${HOME}/vacancy/update`, DELETE: `${HOME}/vacancy/delete`},
    SERVICE: {
        NEXT_USER: `${HOME}/next/user`,
        NEXT_VACANCY: `${HOME}/next/vacancy`
    },
    ABOUTME: {
        USER: `${HOME}/aboutMe/user`,
        COMPANY: `${HOME}/aboutMe/company`
    },
    LIKE: {
        USER: `${HOME}/like/user`,
        VACANCY: `${HOME}/like/vacancy`
    }
};

const HTTPMethods = {
    POST: 'POST',
    GET: 'GET',
    PUT: 'PUT',
    PATCH: 'PATCH'
};

const HTTPResponseType = {
    TEXT: 'text',
    JSON: 'json'
}

function forceAuthorize(status) {
    if (location.href.endsWith('authorization.html') || location.href.endsWith('register.html'))
        return;
    if (Math.floor(status / 100) !== 2) {
        redirectTo('authorization.html');
    }
}

function logout() {
    localStorage.removeItem('token');
    redirectTo('index.html');
}

const XHR = new XMLHttpRequest();
XHR.addEventListener('error', function (event) {
    console.log(event.target);
    alert('Oops! Something went wrong.');
});

async function sendJSON(data, controller, responseType, processResponse, authorizationToken) {
    const json = JSON.stringify(data);

    const xhr = new XMLHttpRequest()
    xhr.open(HTTPMethods.POST, controller, true);
    xhr.setRequestHeader('Content-Type', 'application/json');

    if (authorizationToken !== undefined) {
        xhr.setRequestHeader('Authorization', `Bearer ${authorizationToken}`);
    }

    xhr.responseType = responseType;
    xhr.onload = () => {
        forceAuthorize(xhr.status);
        processResponse(xhr.status, xhr.response);
    };

    xhr.send(json);
}

async function fetchFrom(controller, processResponse, authorizationToken, responseType = 'json', async= true) {
    const xhr = new XMLHttpRequest();

    xhr.open(HTTPMethods.GET, controller, async);

    if (authorizationToken !== undefined) {
        xhr.setRequestHeader('Authorization', `Bearer ${authorizationToken}`);
    }

    xhr.responseType = responseType;
    xhr.onload = () => processResponse(xhr.status, xhr.response);

    xhr.send();
}


function getObjectFromIterable(iterable, mappingLambda) {
    return Object.fromEntries(Array.from(iterable, item => item).map(mappingLambda));
}

function plural(n) {
    [choice1, choice2, choice3] = ['год', 'года', 'лет']
    return n + ' ' + ((((n % 10) === 1) && ((n % 100) !== 11)) ? (choice1) : (((((n % 10) >= 2) && ((n % 10) <= 4)) && (((n % 100) < 10) || ((n % 100) >= 20))) ? (choice2) : (choice3)))
}

function encodeImage(file) {
    reader.onloadend = function () {
        avatarBytes = reader.result.split(',')[1];
        console.log(avatarBytes);
    }
    reader.readAsDataURL(file);
}

function redirectTo(location) {
    window.location.href = location;
}
