function auth() {
    startWrongPasswordAnimation();

    const dataRaw = getObjectFromIterable(
        document.querySelectorAll('[id^="auth_"]'),
        ti => [ti.id, ti.value]);

    let data = {
        login: dataRaw['auth_email'],
        password: dataRaw['auth_password']
    }
    console.log(data);

    controller = currentTab === 0 ? Controllers.AUTH.USER : Controllers.AUTH.COMPANY;

    if (!data || !controller)
        throw new Error('Internal error');

    sendJSON(data, controller,
        (statusCode) => {
            if (statusCode === 200) {
                localStorage['user_login'] = dataRaw['auth_email'];
                window.location.href = 'index.html'
            } else {
                startWrongPasswordAnimation();
            }
        });
}

function startWrongPasswordAnimation() {
    let passwordField = document.querySelector('input[id=auth_password]');
    passwordField.style.animationName = "";
    passwordField.offsetHeight;
    passwordField.style.animationName = "shake, glow-red";
    passwordField.style.animationDuration = "0.7s, 0.35s";
    passwordField.style.animationIterationCount = "1, 2";
}
