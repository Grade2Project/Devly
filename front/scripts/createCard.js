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
            card.appendChild(img);
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

        let img = document.createElement('img');
        img.src = "https://placeimg.com/640/480/tech/grayscale";

        let name = document.createElement('h3');
        let info = document.createElement('ul');

        return await sendJSON(null, Controllers.SERVICE.NEXT_USER, HTTPResponseType.JSON, (status, response) => {
            name.innerText = response['name'];
            for (let [key, value] of Object.entries(response)) {
                let item = document.createElement('li');
                item.innerText = `${key} : ${value}`;
                info.appendChild(item);
            }

            card.appendChild(img);
            card.appendChild(name);
            card.appendChild(info);

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







