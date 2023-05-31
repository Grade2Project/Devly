function redirectIfNotAuthorized() {
    if (localStorage['token'] === undefined) {
        location.href = '../html/authorization.html';
    }
}