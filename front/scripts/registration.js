function registerClient() {
    let dataRaw = getObjectFromIterable(
        document.querySelectorAll('[id^="reg_user_"]'),
        ti => [ti.id, ti.value]);

    let controller = Controllers.REG.USER;
    let data = {
        login: dataRaw['reg_user_email'],
        password: dataRaw['reg_user_password']
    }

    sendJSON(data, controller, HTTPResponseType.TEXT,
        (statusCode, response) => {
        if (statusCode === 200) {
            localStorage['token'] = response;
            redirectTo('developer.html');
        }
        else {
            let emailField = document.querySelector('input[id=reg_user_email]');
            let passwordField = document.querySelector('input[id=reg_user_password]');
            incorrectInputAnimation(emailField);
            incorrectInputAnimation(passwordField);
        }
    });
}

function registerHR() {
    let dataRaw = getObjectFromIterable(
        document.querySelectorAll('[id^="reg_company_"]'),
        ti => [ti.id, ti.value]
    );

    const controller = Controllers.REG.COMPANY;
    const info = {
        inn: dataRaw['reg_company_inn'],
        tel: dataRaw['reg_company_tel']
    };
    let data = {
        companyName: dataRaw['reg_company_name'],
        companyEmail: dataRaw['reg_company_email'],
        companyInfo: JSON.stringify(info),
        password: dataRaw['reg_company_password'],
        photo: avatarBytes
    }

    sendJSON(data, controller, HTTPResponseType.TEXT,
        (statusCode, response) => {
        if (statusCode === 200) {
            localStorage['token'] = response;
            localStorage['company_name'] = dataRaw['reg_company_name'];
            redirectTo('create_vacancy.html');
        }
        else {
            let emailField = document.querySelector('input[id=reg_company_email]');
            let passwordField = document.querySelector('input[id=reg_company_password]');
            incorrectInputAnimation(emailField);
            incorrectInputAnimation(passwordField);
        }
    });
    }
