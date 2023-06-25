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

function SendResume() {
    const grade = document.querySelector('select[id="user_prog_grade"]').value;
    const dataRaw = getObjectFromIterable(
        document.querySelectorAll('input[id^="user_"]'),
        ti => [ti.id, ti.value]);
    console.log(dataRaw);
    dataRaw['user_birthdate'] += 'T00:00:00Z'
    const info = {
        'edu_grad': dataRaw['user_grad_level'],
        'edu_name': dataRaw['user_institution'],
        'edu_program': dataRaw['user_faculty'],
        'edu_release': dataRaw['user_year_of_grad'],
        'position': dataRaw['user_position'],
        'salary': dataRaw['user_salary'],
        'schedule': dataRaw['user_word_schedule'],
        'ext_info': dataRaw['user_ext_info'],
    }

    const data = {
        login: 'kek',
        name: dataRaw['user_initials'],
        birthDate: dataRaw['user_birthdate'],
        city: document.getElementById('user_city').value,
        info: JSON.stringify(info),
        grade: grade,
        experience: dataRaw['user_work_exp'],
        photo: avatarBytes,
        email: 'lol',
        phone: dataRaw['user_tel'],
        favoriteLanguages: filtersContainer
    }

    sendJSON(data, Controllers.RESUME.UPDATE, HTTPResponseType.TEXT,
        (statusCode) => {
            if (statusCode === 200) {
                redirectTo("swipe.html");
            }
            if (statusCode === 400) {
                alert('BadKekus');
            }
        }, localStorage['token']);
}

function SendVacancy() {
    const dataRaw = getObjectFromIterable(
        document.querySelectorAll('[id^="user_"]'),
        ti => [ti.id, ti.value]);
    console.log(dataRaw);

    const info = {
        'edu_grad': dataRaw['user_grad_level'],
        'experience': +dataRaw['user_work_exp'],
        'position': dataRaw['user_position'],
        'schedule': dataRaw['user_word_schedule'],
        'desc': dataRaw['user_ext_info']
    }

    const data = {
        companyName: 'abcabc',
        city: dataRaw['user_city'],
        info: JSON.stringify(info),
        salary: +dataRaw['user_salary'],
        grade: dataRaw['user_prog_grade'],
        photo: null,
        programmingLanguage: dataRaw['user_language']
    }

    sendJSON(data, Controllers.VACANCY.UPDATE, HTTPResponseType.TEXT,
        (statusCode) => {
            if (statusCode === 200) {
                redirectTo("vacancies.html");
            }
        }, localStorage['token']);
}

