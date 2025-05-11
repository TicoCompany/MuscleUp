(function () {
    const app = angular.module("app");
    app.constant("$appInfo", {
        version: "?v=1.0.0"
    });
    app.constant("$configRequest", {
        headers: {
            'Content-Type': 'application/json'
        },
        withCredentials: true
    });
})();