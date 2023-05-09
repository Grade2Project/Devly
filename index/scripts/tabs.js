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
        tabsSelector[currentTab].style.borderBottomColor = '#807D7D';
        tabsValues[currentTab].style.display = 'none';
    }

    tabsValues[n].style.display = 'flex';
    tabsSelector[n].style.borderBottomColor = '#FF5A5F';

    currentTab = n;
}