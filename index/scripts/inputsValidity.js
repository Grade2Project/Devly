const inputs = document.querySelectorAll('input');
console.log(inputs);
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

function alertOnInvalidity(item) {
    let isInvalid = !item.checkValidity() && item.required;
    if (isInvalid) alert(`Invalid input: ${item.value}`);

    return isInvalid;
}