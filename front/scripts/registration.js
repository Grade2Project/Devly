function registerClient() {
    let dataRaw = getObjectFromIterable(
        document.querySelectorAll('[id^="reg_user_"]'),
        ti => [ti.id, ti.value]);

    let controller = Controllers.REG.USER;
    let data = {
        login: dataRaw['reg_user_email'],
        password: dataRaw['reg_user_password']
    }

    sendJSON(data, controller,
        (statusCode) => {
        if (statusCode === 200) {
            localStorage['user_login'] = dataRaw['reg_user_email'];
            console.log('Успешная регистрация');
        }
        else {
            console.log('Ошибка регистрации');
        }
    });
}

function registerHR() {
        let dataRaw = getObjectFromIterable(
            document.querySelectorAll('[id^="reg_company_"]'),
            ti => [ti.id, ti.value]);

        let controller = Controllers.REG.COMPANY;
        let data = {
            companyName: dataRaw['reg_company_name'],
            companyEmail: dataRaw['reg_company_email'],
            companyInfo: {
                companyInn: dataRaw['reg_company_inn'],
                companyTel: dataRaw['reg_company_tel']
            },
            password: dataRaw['reg_company_password']
        }

        sendJSON(data, controller,
            (statusCode) => {
            if (statusCode === 200) {
                localStorage['user_login'] = dataRaw['reg_company_email'];
                console.log('Успешная регистрация');
            }
            else {
                console.log('Ошибка регистрации');
            }
        });
    }