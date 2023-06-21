const __holder = document.querySelector('.vacancies__picker');
const __template__card = document.getElementById('vacancy__element__template');
const __template__empty = document.getElementById('vacancy__template__empty');
class VacancyElement {
    constructor(vacancyObj) {
        let vcard = __template__card.content.cloneNode(true);
        let vinfo = JSON.parse(vacancyObj['info']);

        vcard.getElementById('vacancy__position').innerText = vinfo['position'];
        vcard.getElementById('vacancy__experience').innerText = vinfo['experience'];
        vcard.getElementById('vacancy__grade').innerText = vacancyObj['grade'];
        vcard.getElementById('vacancy__language').innerText = vacancyObj['programmingLanguage'];
        vcard.getElementById('vacancy__salary').innerText = vacancyObj['salary'];

        vcard.querySelector('.remove__button').onclick = () => {
            console.log(vacancyObj['id']);
        }

        __holder.appendChild(vcard);
    }
}