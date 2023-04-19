const bEmployee = document.getElementById('bEmployee');
const bCompany = document.getElementById('bCompany');

bEmployee.style.background = '#FF5A5F';
bCompany.style.background = '#087E8B';

const inputs = document.querySelectorAll('input');
for (let i of inputs) {
    i.addEventListener('blur', function () {
        if (!(i.checkValidity()) && i.required) {
            i.classList.add('invalid');
        }
        else {
            i.classList.remove('invalid');
        }
    });
}

//
// const ppReg = document.getElementById('reg-pop');
// const ppAuth = document.getElementById('auth-pop');
const ppBlur = document.getElementById('pp-bg');
function ppShow (id) {
    const pp = document.getElementById(id);
    pp.style.visibility = 'visible';
    pp.style.opacity = '1';

    ppBlur.style.visibility = 'visible';
}

function ppHide(id) {
    const pp = document.getElementById(id);
    pp.style.visibility = 'hidden';
    pp.style.opacity = '0';

    ppBlur.style.visibility = 'hidden';
    for (let child of pp.children) {
        if (child.className === 'input-line') {
            for (let c of child.children) {
                if (c.tagName === 'INPUT')
                    c.value = '';
            }
        }
    }
}