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