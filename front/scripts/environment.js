const modal = document.getElementById('__match__dialog');

function setModal(tel) {
    modal.showModal();
    document.getElementById('__match__telephone').innerText = tel;
}

class Environment {
    constructor(cardHandler, filtersHandler, settingsHandler = null) {
        this.cardHandler = cardHandler;
        this.filtersHandler = filtersHandler;
        this.settingsHandler = settingsHandler;
    }

    redirectToSettings() {}
    like(card) {}
    dislike(card) {
        removeCardFromDocWithDelay(card);
    }
}

class UserEnvironment extends Environment {
    constructor() {
        super(
            new CardCreator(new VacancyCardHandler()),
            new UserFilters(
                new GradesFilter(),
                new LanguagesFilter()
            )
        );
    }

    redirectToSettings() {
        redirectTo("developer_profile.html");
    }

    like(card) {
        sendJSON(+card.dataset['id'],
            Controllers.LIKE.VACANCY,
            HTTPResponseType.JSON, (status, json) => {
                if (json['isMutual']) {
                    setModal(JSON.parse(json['data']['info'])['tel']);
                }
                removeCardFromDocWithDelay(card);
            }, localStorage['token']);
    }
}

class CompanyEnvironment extends Environment {
    constructor() {
        super(
            new CardCreator(new UserCardHandler()),
            new CompanyFilters(
                new GradesFilter(),
                new LanguagesFilter(),
                new CitiesFilter()
            )
        );
    }

    redirectToSettings() {
        redirectTo("hr_profile.html");
    }

    like(card) {
        sendJSON(card.dataset['id'],
            Controllers.LIKE.USER,
            HTTPResponseType.JSON, (status, json) => {
            console.log(json);
                if (json['isMutual']) {
                    setModal(json['data']['phone']);
                }
                removeCardFromDocWithDelay(card);
            }, localStorage['token']);
    }
}