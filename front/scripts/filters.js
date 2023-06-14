//TODO: сделать единый environment для всех хендлеров, фильтров и прочего.

class Filter {
    __container = document.querySelector('.content__container');
    __filer__template = document.getElementById('filter__base');

    values = [];

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

    __initialize() {

    }

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
    __changeHandler = {
        set: function (target, key, value) {
            target[key] = value;

            //TODO: Сломалось кхуям из-за this
            if (false) {
                btnApplyFilters.disabled = false;
                return true;
            }

            btnApplyFilters.disabled = true;
            cardHandler.applyFilters(null);

            return true;
        }
    }

    constructor() {
        let __filters = {};
        for (let filter of arguments) {
            __filters[filter.name] = filter.values;
        }

        this.__proxy = new Proxy(__filters, this.__changeHandler);
    }

    isEmpty() {
        console.log(this.__proxy);
        return false;
    }

    applyFilters() {}
}

class CompanyFilters extends Filters {
    applyFilters() {
        cardHandler.applyFilters({
            grades: this.__proxy.grades.length !== 0 ? this.__proxy.grades : null,
            languages: this.__proxy.languages.length !== 0 ? this.__proxy.languages : null,
            cities: this.__proxy.cities.length !== 0 ? this.__proxy.cities : null,
            experienceFrom: 0,
            userName: null
        })
    }
}

let token = localStorage.getItem('token');
let btnApplyFilters = document.getElementById('apply_filters');

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