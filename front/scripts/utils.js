const HOME = 'http://localhost:5003';
// const HOME = 'http://localhost:80'; // Для контейнера

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

const XHR = new XMLHttpRequest();
XHR.addEventListener('error', function(event) {
    console.log(event.target);
    alert('Oops! Something went wrong.');
});

function getInputValueById(id) {
    const candidate = document.getElementById(id);

    return candidate.value;
}

async function sendJSON (data, controller, processResponse, authorizationToken) {
    const json = JSON.stringify(data);

    const xhr = new XMLHttpRequest()
    xhr.open(HTTPMethods.POST, controller, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    if (authorizationToken !== undefined) {
        xhr.setRequestHeader('Authorization', `Bearer ${authorizationToken}`);
    }

    xhr.responseType = 'text'; //Сделать разные, пока хуета
    xhr.onload = () => {
        processResponse(xhr.status, xhr.response);
    }

    xhr.send(json);
}

async function fetchJSON (controller, processResponse, authorizationToken) {
    const xhr = new XMLHttpRequest()
    xhr.open(HTTPMethods.GET, controller, true);

    if (authorizationToken !== undefined) {
        xhr.setRequestHeader('Authorization', `Bearer ${authorizationToken}`);
    }

    xhr.responseType = 'json';
    xhr.onload = () => processResponse(xhr.response);

    xhr.send();
}



function getObjectFromIterable(iterable, mappingLambda) {
    return Object.fromEntries(Array.from(iterable, item => item).map(mappingLambda));
}
