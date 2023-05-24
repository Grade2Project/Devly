const tabsSelector = document.querySelectorAll('.tabs__tab');
const tabsValues = document.querySelectorAll('.tabs__values');

for (let value of tabsValues) {
    value.style.display = 'none';
}
let currentTab = undefined;
showTab(0);
function showTab(n) {
    if (currentTab === n) return;

    if (tabsValues.length !== 0) changeDisplayOn(n);
    changeFocusOn(n);

    currentTab = n;
}

function changeFocusOn(n) {
    tabsSelector[currentTab ?? 0].style.borderBottomColor = '#807D7D';
    tabsSelector[n].style.borderBottomColor = '#FF5A5F';
}

function changeDisplayOn(n) {
    tabsValues[currentTab ?? 0].style.display = 'none';
    tabsValues[n].style.display = 'flex';
}