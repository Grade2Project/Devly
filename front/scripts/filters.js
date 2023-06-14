//TODO: сделать единый environment для всех хендлеров, фильтров и прочего.

class Filter {
    __container = document.querySelector('.content__container');
    __filer__template = document.getElementById('filter__base');
    __values = [];

    set values(values) {
        //TODO: тут должна была быть проверка прокси, но пока не работает.
        this.__values = values;
    }

    get values() {
        return this.__values;
    }

    constructor(name, description) {
        this.name = name;
        let filterObj = this.__filer__template.content.cloneNode(true);
        filterObj.querySelector('input').setAttribute('id', `${this.name}__filter`);
        filterObj.querySelector('label').setAttribute('for', `${this.name}__filter`);
        filterObj.querySelector('label').innerText = description;

        this.pID = `${this.name}__filter`;
        this.__container.appendChild(filterObj);

        this.__initialize();
    }

    __initialize() {}

    isEmpty() {
        return this.values.length === 0;
    }
}

class GradesFilter extends Filter {
    constructor() {
        super('grades', 'Грейд');
    }

    __initialize() {
        fetchFrom(Controllers.GRADES, (statusCode, response) => {
            let grades = response.container;
            let gradesFilter = sellect(document.getElementById(this.pID), {
                originList: grades,
                onInsert: () => {
                    this.values= [...gradesFilter.getSelected()];
                }
            });

            console.log(this.pID);
            gradesFilter.init();
        }, localStorage['token']);
    }
}

class LanguagesFilter extends Filter {
    constructor() {
        super('languages', 'Язык программирования');
    }

    __initialize() {
        fetchFrom(Controllers.LANGS, (statusCode, response) => {
            let langs = response.container;
            let langsFilter = sellect(document.getElementById(this.pID), {
                originList: langs,
                onInsert: () => {
                    this.values = [...langsFilter.getSelected()];
                }
            });

            langsFilter.init();
        }, localStorage['token']);
    }
}

class CitiesFilter extends Filter {
    constructor() {
        super('cities', 'Город');
    }

    __initialize() {
        let citiesFilterE = document.getElementById(this.pID);
        let citiesFilter = sellect(citiesFilterE, {
            originList: [],
            onInsert: () => {
                this.values = [...citiesFilter.getSelected()];
            }
        });
        citiesFilter.init();

        citiesFilterE.oninput = () => {
            fetchFrom(`${Controllers.CITIES}?pattern=${citiesFilterE.value}`, (statusCode, response) => {
                citiesFilter.refresh(response.container);
            });
        }
    }
}

class Filters {
    __container = document.querySelector('.content__container');
    __filters = {};
    constructor() {
        for (let filter of arguments) {
            this.__filters[filter.name] = filter;
        }

        this.__container.appendChild(
            document
                .getElementById('filter__apply__button')
                .content
                .cloneNode(true)
        );
    }

    isEmpty() {
        for (let filter of Object.values(this.__filters)) {
            if (!filter.isEmpty()) return false;
        }

        return true;
    }

    applyFilters() {}
}

class CompanyFilters extends Filters {
    applyFilters() {
        let filter = this.isEmpty()
            ? null
            : {
            grades: this.__filters.grades.values.length !== 0 ? this.__filters.grades.values : null,
            languages: this.__filters.languages.values.length !== 0 ? this.__filters.languages.values : null,
            cities: this.__filters.cities.values.length !== 0 ? this.__filters.cities.values : null,
            experienceFrom: 0,
            userName: null
        }

        cardHandler.applyFilters(filter);
    }
}

let token = localStorage.getItem('token');

// btnApplyFilters.onclick = () => {
//     cardHandler.applyFilters({
//         grades: filters.grades.length !== 0 ? filters.grades : null,
//         languages: filters.languages.length !== 0 ? filters.languages : null,
//         experienceFrom: 0,
//         city: null,
//         userName: null
//     });
// }
//
// function isFiltersEmpty() {
//     return filters.languages.length === 0
//         && filters.grades.length === 0
//         && filters.city.length === 0
//         && filters.experienceFrom === 0;
// }
//
// let changeHandler = {
//     set: function (target, key, value) {
//         target[key] = value;
//
//         if (!isFiltersEmpty()) {
//             btnApplyFilters.disabled = false;
//             return true;
//         }
//
//         btnApplyFilters.disabled = true;
//         cardHandler.applyFilters(null);
//
//         return true;
//     }
// }
//
// const filters = new Proxy({
//     grades: [],
//     languages: [],
//     city: [],
//     experienceFrom: 0
// }, changeHandler);
//
// fetchFrom(Controllers.GRADES, (statusCode, response) => {
//     let grades = response.container;
//     let gradesFilter = sellect(document.getElementById('grades__filter'), {
//         originList: grades,
//         onInsert: () => {
//             filters.grades = [...gradesFilter.getSelected()];
//         }
//     });
//
//     gradesFilter.init();
// }, localStorage['token']);
//
// fetchFrom(Controllers.LANGS, (statusCode, response) => {
//     let langs = response.container;
//     let langsFilter = sellect(document.getElementById('languages__filter'), {
//         originList: langs,
//         onInsert: () => {
//             filters.languages = [...langsFilter.getSelected()];
//         }
//     });
//
//     langsFilter.init();
// }, localStorage['token']);
//
// let citiesFilterE = document.getElementById('cities__filter');
// let citiesFilter = sellect(citiesFilterE, {
//     originList: [],
//     onInsert: () => {
//         filters.city = [...citiesFilter.getSelected()];
//     }
// });
// citiesFilter.init();
//
// citiesFilterE.oninput = () => {
//     fetchFrom(`${Controllers.CITIES}?pattern=${citiesFilterE.value}`, (statusCode, response) => {
//         citiesFilter.refresh(response.container);
//     });
// }