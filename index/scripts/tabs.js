const tabsSelector = document.querySelectorAll('.tabs__tab');
const tabsValues = document.querySelectorAll('.tabs__values');

for (let value of tabsValues) {
    value.style.display = 'none';
}
let currentTab = -1;
showTab(0);
function showTab(n) {
    if (currentTab === n) return;

    if (currentTab !== -1) {
        tabsSelector[currentTab].classList.remove('__selected');
        tabsValues[currentTab].style.display = 'none';
    }

    tabsSelector[n].focus();
    tabsValues[n].style.display = 'flex';

    currentTab = n;
}