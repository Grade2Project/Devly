const HOME = 'http://localhost:5003';

const Controllers = {
    AUTH: {USER: `${HOME}/auth/user`, COMPANY: `${HOME}/auth/company`},
    REG: {USER: `${HOME}/reg`, COMPANY: `${HOME}/company/reg`},
    GRADES: `${HOME}/grades/get`
};

const HTTPMethods = {
    POST: 'POST',
    GET: 'GET',
    PUT: 'PUT',
    PATCH: 'PATCH'
};

const XHR = new XMLHttpRequest();
XHR.addEventListener('error', function(event) {
    console.log(event.target);
    alert('Oops! Something went wrong.');
});

function getInputValueById(id) {
    const candidate = document.getElementById(id);

    return candidate.value;
}

async function sendJSON (data, controller, processResponse) {
    const json = JSON.stringify(data);

    XHR.open(HTTPMethods.POST, controller, true);
    XHR.setRequestHeader('Content-Type', 'application/json');

    XHR.responseType = 'json';
    XHR.onload = () => processResponse(XHR.status, XHR.response);

    XHR.send(json);
}

async function fetchJSON (controller, processResponse) {
    XHR.open(HTTPMethods.GET, controller, true);

    XHR.responseType = 'json';
    XHR.onload = () => processResponse(XHR.response);

    XHR.send();
}



function getObjectFromIterable(iterable, mappingLambda) {
    return Object.fromEntries(Array.from(iterable, item => item).map(mappingLambda));
}



