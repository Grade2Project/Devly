function redirectIfNotAuthorized() {
    if (localStorage['user_login'] === undefined) {
        location.href = '../html/authorization.html';
    }
}