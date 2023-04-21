const Controllers = {
    AUTH: '/auth/user',
    REG: '/reg'
};

const HTTPMethods = {
    POST: 'POST',
    GET: 'GET',
    PUT: 'PUT',
    PATCH: 'PATCH'
};

const HOME = 'http://localhost:5003';

const regSubmit = document.getElementById('reg_submit');
regSubmit.addEventListener('click', (e) => {
    const userEmail = document.getElementById('reg_user_email');
    const userPass = document.getElementById('reg_user_password');

    if (!userEmail.checkValidity()){
        alert('Invalid');
        return;
    }

    sendData({
        login: userEmail.value,
        password: userPass.value
    }, Controllers.REG);
});

const authSubmit = document.getElementById('auth_submit');
authSubmit.addEventListener('click', (e) => {
    const userEmail = document.getElementById('auth_user_email');
    const userPass = document.getElementById('auth_user_password');

    sendData({
        login: userEmail.value,
        password: userPass.value
    }, Controllers.AUTH);
})

function sendData (data, controller) {
    const XHR = new XMLHttpRequest();
    const json = JSON.stringify(data);

    XHR.addEventListener('error', function(event) {
        alert('Oops! Something went wrong.');
    });

    let url = `${HOME}${controller}`;

    XHR.open(HTTPMethods.POST, url);
    XHR.setRequestHeader('Content-Type', 'application/json');
    // XHR.setRequestHeader('Access-Control-Allow-Origin', 'https://localhost:7172');
    XHR.send(json);
}