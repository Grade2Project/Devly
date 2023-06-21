function fillSelect(selectObj, data) {
    for (let grade of data) {
        let opt = document.createElement('option');
        opt.value = grade;
        opt.innerHTML = grade;

        selectObj.appendChild(opt);
    }
}