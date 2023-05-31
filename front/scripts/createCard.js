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
    async fetchCard() {
        let card = document.createElement('div');
        card.classList.add('tinder__card');

        let centerCropped = document.createElement('div');
        centerCropped.classList.add('center-cropped');

        let img = document.createElement('img');
        img.src = "https://placeimg.com/640/480/tech/grayscale";

        let name = document.createElement('h3');
        let info = document.createElement('ul');

        return await sendJSON(null, Controllers.SERVICE.NEXT_VACANCY, HTTPResponseType.JSON, (status, response) => {
            name.innerText = response['companyName'];
            for (let [key, value] of Object.entries(response)) {
                let item = document.createElement('li');
                item.innerText = `${key} : ${value}`;
                info.appendChild(item);
            }

            centerCropped.appendChild(img);
            card.appendChild(centerCropped);
            card.appendChild(name);
            card.appendChild(info);

            return Promise.resolve(card);

        }, token).then(() => card);
    }
}

class UserCardHandler extends CardHandler {
    async fetchCard() {
        let content = document.getElementById('card_template').content.cloneNode(true);

        let card = document.createElement('div');
        card.classList.add('tinder__card');


        return await sendJSON(null, Controllers.SERVICE.NEXT_USER, HTTPResponseType.JSON, (status, response) => {
            let info = JSON.parse(response['info']);

            try {
                content.getElementById('user_photo').setAttribute(
                    'src',
                    "https://placeimg.com/640/480/tech/grayscale"
                );
                content.getElementById('user_name').innerText = `${response['name']}, ${response['age']}`;
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
            }
            catch (e) {
                console.log(e);
            }

            card.addEventListener('wheel', (we) => {
                if (we.deltaY > 0) {
                    card.classList.add('ofy-scroll');
                    card.scrollBy(0, 430);
                }
            }, {once: true});

            card.append(content);
            return Promise.resolve(card);

        }, token).then(() => card);
    }
}

class CardCreator {
    constructor(handler) {
        this.cardHandler = handler;
    }

    async fetchNCards(n) {
        for (let i = 0; i < n; i++) {
            let a = await this.cardHandler.fetchCard();
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







