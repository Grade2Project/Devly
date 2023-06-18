const tabsSelector = document.querySelectorAll('.tabs__tab');
const tabsValues = document.querySelectorAll('.tabs__values');

for (let value of tabsValues) {
    value.style.display = 'none';
}

let currentTab = undefined;
showTab(0);
function showTab(n) {
    if (currentTab === n)
        return;

    if (tabsValues.length !== 0)
        changeDisplayOn(n % tabsValues.length);
    changeFocusOn(n);
    disableFormFields(n % tabsValues.length);

    currentTab = n;
}

function changeFocusOn(n) {
    tabsSelector[currentTab ?? 0].style.borderBottomColor = '#807D7D';
    tabsSelector[n].style.borderBottomColor = '#FF5A5F';
}

function changeDisplayOn(n) {
    tabsValues[(currentTab ?? 0) % tabsValues.length].style.display = 'none';
    tabsValues[n].style.display = 'flex';
}

function disableFormFields(n) {
    for (let tabsValue of tabsValues) {
        for (let child of tabsValue.children) {
            child.getElementsByTagName('input')[0].setAttribute('disabled', '');
        }
    }
    for (let child of tabsValues[n].children) {
        child.getElementsByTagName('input')[0].removeAttribute('disabled');
    }
}
