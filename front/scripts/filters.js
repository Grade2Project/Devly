let token = localStorage.getItem('token');
let btnApplyFilters = document.getElementById('apply_filters');

btnApplyFilters.onclick = () => {
    cardHandler.applyFilters({
        grades: filters.grades.length !== 0 ? filters.grades : null,
        languages: filters.languages.length !== 0 ? filters.languages : null,
        experienceFrom: 0,
        city: null,
        userName: null
    });
}

function isFiltersEmpty() {
    return filters.languages.length === 0
        && filters.grades.length === 0
        && filters.city.length === 0
        && filters.experienceFrom === 0;
}

let changeHandler = {
    set: function (target, key, value) {
        target[key] = value;

        if (!isFiltersEmpty()) {
            btnApplyFilters.disabled = false;
            return true;
        }

        btnApplyFilters.disabled = true;
        cardHandler.applyFilters(null);

        return true;
    }
}

const filters = new Proxy({
    grades: [],
    languages: [],
    city: [],
    experienceFrom: 0
}, changeHandler);

fetchFrom(Controllers.GRADES, (statusCode, response) => {
    let grades = response.container;
    let gradesFilter = sellect(document.getElementById('grades__filter'), {
        originList: grades,
        onInsert: () => {
            filters.grades = [...gradesFilter.getSelected()];
        }
    });

    gradesFilter.init();
}, localStorage['token']);

fetchFrom(Controllers.LANGS, (statusCode, response) => {
    let langs = response.container;
    let langsFilter = sellect(document.getElementById('languages__filter'), {
        originList: langs,
        onInsert: () => {
            filters.languages = [...langsFilter.getSelected()];
        }
    });

    langsFilter.init();
}, localStorage['token']);

let citiesFilterE = document.getElementById('cities__filter');
let citiesFilter = sellect(citiesFilterE, {
    originList: [],
    onInsert: () => {
        filters.city = [...citiesFilter.getSelected()];
    }
});
citiesFilter.init();

citiesFilterE.oninput = () => {
    fetchFrom(`${Controllers.CITIES}?pattern=${citiesFilterE.value}`, (statusCode, response) => {
        citiesFilter.refresh(response.container);
    });
}