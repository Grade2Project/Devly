const HOME = 'http://localhost:5003';

const Controllers = {
    AUTH: {USER: `${HOME}/auth/user`, COMPANY: `${HOME}/auth/company`},
    REG: {USER: `${HOME}/reg`, COMPANY: `${HOME}/company/reg`},
    GRADES: `${HOME}/grades/get`,
    RESUME: {UPDATE: `${HOME}/resume/update`},
    SERVICE: {
        NEXT_USER: `${HOME}/next/user`,
        NEXT_VACANCY: `${HOME}/next/vacancy`
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

const XHR = new XMLHttpRequest();
XHR.addEventListener('error', function (event) {
    console.log(event.target);
    alert('Oops! Something went wrong.');
});

function getInputValueById(id) {
    const candidate = document.getElementById(id);

    return candidate.value;
}

async function sendJSON(data, controller, responseType, processResponse, authorizationToken) {
    const json = JSON.stringify(data);

    const xhr = new XMLHttpRequest()
    xhr.open(HTTPMethods.POST, controller, true);
    xhr.setRequestHeader('Content-Type', 'application/json');

    if (authorizationToken !== undefined) {
        xhr.setRequestHeader('Authorization', `Bearer ${authorizationToken}`);
    }

    xhr.responseType = responseType;
    xhr.onload = () => processResponse(xhr.status, xhr.response);

    xhr.send(json);
}

async function fetchFrom(controller, processResponse, authorizationToken, responseType = 'json') {
    const xhr = new XMLHttpRequest()
    xhr.open(HTTPMethods.GET, controller, true);

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