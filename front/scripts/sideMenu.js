function fillSideMenu(controller, additionalFilling) {
    fetchFrom(controller, (statusCode, json) => {
        // if (statusCode === 403)
        //     location.href = '../../authorization.html'

        json['photo'] = `data:image/jpg/png/jpeg;base64,${json['photo']}`
        const info = JSON.parse(json['info'])
        additionalFilling(json, info);
    }, localStorage['token']);
}
