class CardsStorage {
    static fetchedCards = [];
    static fetchedCardsMax = 4;
    static cardsHolder = document.querySelector('.tinder__cards');
}

class CardHandler {

    async fetchNCards(n) {
        for (let i = 0; i < n; i++) {
            let a = await this.fetchCard();
            CardsStorage.fetchedCards.push(a);
        }
    }
    async fetchCard() {
        let card = document.createElement('div');
        card.classList.add('tinder__card');

        let img = document.createElement('img');
        img.src = "https://placeimg.com/600/300/people";

        let name = document.createElement('h3');
        let info = document.createElement('ul');

        return await fetchJSON(Controllers.SERVICE.NEXT_VACANCY, (response) => {
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
    async appendCardToDoc() {
        if (CardsStorage.fetchedCards.length <= CardsStorage.fetchedCardsMax / 2) await this.fetchNCards(CardsStorage.fetchedCardsMax);
        let card = CardsStorage.fetchedCards.shift();
        CardsStorage.cardsHolder.appendChild(card);

        setHammerOnSingleCard(card);
        refreshCards();
    }
}







