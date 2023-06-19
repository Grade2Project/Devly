let rangeSlider = function () {
    let slider = document.querySelector('.range-slider'),
        range = document.querySelector('.range-slider__range'),
        value = document.querySelector('.range-slider__value');

    value.classList.add('value');

    range.oninput = function () {
        value.innerHTML = this.value;
    };
};

rangeSlider();