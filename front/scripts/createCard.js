class CardsStorage {
    static fetchedCards = [];
    static fetchedCardsMax = 4;
    static cardsHolder = document.querySelector('.tinder__cards');
}

class CardHandler {
    async fetchCard() {
    };
}

class VacancyCardHandler extends CardHandler {
    async fetchCard(filter) {
        let content = document.getElementById('template__vacancy_card').content.cloneNode(true);

        let card = document.createElement('div');
        card.classList.add('tinder__card');

        return await sendJSON(filter, Controllers.SERVICE.NEXT_VACANCY, HTTPResponseType.JSON, (status, response) => {
            let info = JSON.parse(response['info']);
            console.log(info);

            content.getElementById('company_photo').setAttribute(
                'src',
                `data:image/jpg/png/jpeg;base64,${response['photo']}`
            );

            card.setAttribute('data-id', response['id']);
            content.getElementById('company_name').innerText = response['companyName'];
            content.getElementById('vacancy_city').innerText = 'г. Екатеринбург';
            content.getElementById('vacancy_language').innerText = response['programmingLanguage'];
            content.getElementById('vacancy_grade').innerText = response['grade'];
            content.getElementById('vacancy_position').innerText = `Должность: ${info['position']}`;
            content.getElementById('vacancy_salary').innerText = `Зарплата: ${response['salary']}`;
            content.getElementById('vacancy_ext_info').innerText = info['desc'];

            card.addEventListener('wheel', (we) => {
                if (we.deltaY > 0) {
                    card.classList.add('ofy-scroll');
                    card.scrollBy(0, 430);
                }
            }, {once: true});

            card.appendChild(content);
            return Promise.resolve(card);

        }, token).then(() => card);
    }
}

class UserCardHandler extends CardHandler {
    async fetchCard(filter) {
        let content = document.getElementById('template__user_card').content.cloneNode(true);

        let card = document.createElement('div');
        card.classList.add('tinder__card');


        return await sendJSON(filter, Controllers.SERVICE.NEXT_USER, HTTPResponseType.JSON, (status, response) => {
            let info = JSON.parse(response['info']);

            content.getElementById('user_photo').setAttribute(
                'src',
                `data:image/jpg/png/jpeg;base64,${response['photo']}`
            );

            card.setAttribute('data-id', response['login']);
            content.getElementById('user_name').innerText = `${response['name'].split(' ').splice(0, 2).join(' ')}, ${response['age']}`;
            content.getElementById('user_city').innerText = response['city'];
            content.getElementById('user_languages').innerText = response['favoriteLanguages'].join(', ');
            content.getElementById('user_grade').innerText = response['grade'];
            content.getElementById('user_position').innerText = `Должность: ${info['position']}`;
            content.getElementById('user_salary').innerText = `Зарплата: ${info['salary']}₽`;
            content.getElementById('user_schedule').innerText = `График: ${info['schedule']}`;
            content.getElementById('user_experience').innerText = `Стаж: ${plural(response['experience'])}`;
            content.getElementById('user_edu_name').innerText = `Университет: ${info['edu_name']}`;
            content.getElementById('user_edu_grad').innerText = `Уровень образования: ${info['edu_grad']}`;
            content.getElementById('user_edu_program').innerText = `Направление: ${info['edu_program']}`;
            content.getElementById('user_edu_release').innerText = `Год окончания: ${new Date(info['edu_release']).toLocaleDateString()}`;
            content.getElementById('user_ext_info').innerText = info['ext_info'];

            card.addEventListener('wheel', (we) => {
                if (we.deltaY > 0) {
                    card.classList.add('ofy-scroll');
                }
            });

            card.append(content);

            return Promise.resolve(card);

        }, token).then(() => card);
    }
}

class CardCreator {
    constructor(handler) {
        this.cardHandler = handler;
        this.filter = null;
    }

    async applyFilters(newFilter) {
        this.filter = newFilter;
        console.log(this.filter);
        CardsStorage.fetchedCards = [];

        removeCardsFromDoc();
        await this.appendCardToDoc();
    }

    async fetchNCards(n) {
        for (let i = 0; i < n; i++) {
            let a = await this.cardHandler.fetchCard(this.filter);
            CardsStorage.fetchedCards.push(a);
        }
    }

    async appendCardToDoc() {
        if (CardsStorage.fetchedCards.length <= CardsStorage.fetchedCardsMax / 2) await this.fetchNCards(CardsStorage.fetchedCardsMax);
        let card = CardsStorage.fetchedCards.shift();
        await CardsStorage.cardsHolder.appendChild(card);

        setHammerOnSingleCard(card);
        refreshCards();
    }
}







