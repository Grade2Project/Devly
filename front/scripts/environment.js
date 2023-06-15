class Environment {
    constructor(cardHandler, filtersHandler, settingsHandler = null) {
        this.cardHandler = cardHandler;
        this.filtersHandler = filtersHandler;
        this.settingsHandler = settingsHandler;
    }
}

class UserEnvironment extends Environment {
    constructor() {
        super(
            new CardCreator(new VacancyCardHandler()),
            new Filters()
        );
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
}