class Environment {
    constructor(cardHandler, filtersHandler, settingsHandler = null) {
        this.cardHandler = cardHandler;
        this.filtersHandler = filtersHandler;
        this.settingsHandler = settingsHandler;
    }

    redirectToSettings() {}
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
        window.location.href = "settings/applicant/profile.html";
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
        window.location.href = "settings/hr/profile.html";
    }
}