class Environment {
    constructor(cardHandler, filtersHandler, settingsHandler = null) {
        this.cardHandler = cardHandler;
        this.filtersHandler = filtersHandler;
        this.settingsHandler = settingsHandler;
    }

    redirectToSettings() {}
    like(card) {}
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
        let id = card.getElementById('__vid').getAttribute('data-id');
        sendJSON({},
            `${Controllers.LIKE.VACANCY}?vacancyId=${id}`,
            HTTPResponseType.JSON, (status, json) => {
                console.log(json);
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
        let ul = card.getElementById('__uid').getAttribute('data-id');
        sendJSON({},
            `${Controllers.LIKE.USER}?userLogin=${ul}`,
            HTTPResponseType.JSON, (status, json) => {
                console.log(json);
            }, localStorage['token']);
    }
}