class CardsStorage {
    static fetchedCards = [];
    static fetchedCardsMax = 4;
    static cardsHolder = document.querySelector('.tinder__cards');
}

class CardHandler {
    async fetchCard() {};
}

class VacancyCardHandler extends CardHandler{
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
        let card = document.createElement('div');
        card.classList.add('tinder__card');

        let centerCropped = document.createElement('div');
        centerCropped.classList.add('center-cropped');

        let img = document.createElement('img');
        img.src = "https://placeimg.com/640/480/tech/grayscale";
        // img.src = "../../index/gK0xAc7fDOY.jpg";

        let informationHolder = document.createElement('div');
        informationHolder.classList.add('information__holder');

        let nameAndCity = document.createElement('div');
        nameAndCity.classList.add('inner__holder');

        let gradeAndLangs = document.createElement('div');
        gradeAndLangs.classList.add('inner__holder');

        return await sendJSON(null, Controllers.SERVICE.NEXT_USER, HTTPResponseType.JSON, (status, response) => {
            let nameHolder = document.createElement('span');
            nameHolder.innerText = `${response['name']}, ${response['age']}`;
            nameHolder.classList.add('label__28B_600');
            nameAndCity.appendChild(nameHolder);

            let cityLabel = document.createElement('span');
            cityLabel.classList.add('label__18B_500_GREY');
            cityLabel.innerText = response['city'];
            nameAndCity.appendChild(cityLabel);

            let favLangsLabel = document.createElement('span');
            favLangsLabel.classList.add('label__18B_500_GREY');
            favLangsLabel.innerText = `${response['favoriteLanguages'].join(', ')}`;

            let gradeLabel = document.createElement('span');
            gradeLabel.classList.add('label__18B_500_GREY');
            gradeLabel.innerText = response['grade'];

            gradeAndLangs.appendChild(favLangsLabel);
            gradeAndLangs.appendChild(gradeLabel);

            informationHolder.appendChild(nameAndCity);
            informationHolder.appendChild(gradeAndLangs);

            centerCropped.appendChild(img);
            card.appendChild(centerCropped);
            card.appendChild(informationHolder);

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
        CardsStorage.cardsHolder.appendChild(card);

        setHammerOnSingleCard(card);
        refreshCards();
    }
}







