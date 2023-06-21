function authorize() {
    const dataRaw = getObjectFromIterable(
        document.querySelectorAll('[id^="auth_"]'),
        ti => [ti.id, ti.value]);

    let data = {
        login: dataRaw['auth_email'],
        password: dataRaw['auth_password']
    }
    console.log(data);

    let controller = currentTab === 0 ? Controllers.AUTH.USER : Controllers.AUTH.COMPANY;

    if (!data || !controller)
        throw new Error('Internal error');

    sendJSON(data, controller,
        HTTPResponseType.TEXT, (statusCode, response) => {
            if (statusCode === 200) {
                localStorage['token'] = response;
                localStorage['way'] = currentTab === 0 ? 'user' : 'company'; // ToDo: нах way, давай из jwt
                redirectTo('../html/swipe.html');
            } else {
                startWrongPasswordAnimation();
            }
        });
}

function startWrongPasswordAnimation() {
    let passwordField = document.querySelector('input[id=auth_password]');
    incorrectInputAnimation(passwordField);
}
