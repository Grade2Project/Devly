function auth() {
    let controller;

    const dataRaw = getObjectFromIterable(
        document.querySelectorAll('[id^="auth_"]'),
        ti => [ti.id, ti.value]);

    let data = {
        login: dataRaw['auth_email'],
        password: dataRaw['auth_password']
    }
    console.log(data);

    controller = Controllers.AUTH.USER;

    if (!data || !controller)
        throw new Error('Internal error');

    sendJSON(data, controller,
        (statusCode, response) => {
            if (statusCode === 200) {
                localStorage['token'] = response;
                // window.location.href = 'index.html' // Показывать на индексе как-то, что я войден
            }
            else {
                console.log('Ошибка входа'); // Сделать чо нибудь прикольное
            }
            console.log(response);
        });
}
