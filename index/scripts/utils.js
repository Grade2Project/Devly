const HOME = 'http://localhost:5003';

const Controllers = {
    AUTH: `${HOME}/auth/user`,
    REG: `${HOME}/reg`,
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

async function sendJSON (controller, data, responseFunc) {
    const json = JSON.stringify(data);

    XHR.onload = () => responseFunc(XHR.response);
    XHR.open(HTTPMethods.POST, controller, true);
    XHR.setRequestHeader('Content-Type', 'application/json');

    XHR.send(json);
}

async function fetchJSON (controller, func) {
    XHR.open(HTTPMethods.GET, controller, true);
    XHR.responseType = 'json';

    XHR.onload = () => func(XHR.response);
    XHR.send();
}



