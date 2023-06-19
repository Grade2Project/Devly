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
            window.location.href = '../html/developer.html';
        }
        else {
            startWrongEmailAnimation();
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
            location.href = '../html/settings/hr/create_vacancy.html'
        }
        else {
            startWrongEmailAnimation()
        }
    });
    }

function startWrongEmailAnimation() {
    let emailField = document.querySelector('input[id=reg_user_email]');
    incorrectInputAnimation(emailField);
}
