const bEmployee = document.getElementById('bEmployee');
const bCompany = document.getElementById('bCompany');

bEmployee.style.background = '#FF5A5F';
bCompany.style.background = '#087E8B';

const inputs = document.querySelectorAll('.text-input');
for (let i of inputs) {
    i.addEventListener('blur', function () {
        if (!(i.checkValidity())) {
            i.classList.add('invalid');
        }
        else {
            i.classList.remove('invalid');
        }
    })
}