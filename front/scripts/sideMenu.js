function fillSideMenu(controller, additionalFilling) {
    fetchFrom(controller, (statusCode, json) => {
        if (statusCode === 403)
            location.href = '../../authorization.html'

        console.log(json);
        json['photo'] = `data:image/jpg/png/jpeg;base64,${json['photo']}`
        const info = JSON.parse(json['info'])
        document.getElementById('name').innerText = json['companyName'] || json['name'];
        document.getElementById('image').src = json['photo'];
        additionalFilling(json, info);
    }, localStorage['token']);
}
