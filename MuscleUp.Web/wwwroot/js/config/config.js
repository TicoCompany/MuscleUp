(function () {
    angular.module("servicesApp", []);
    angular.module("app", ["ngSanitize", "ngCookies", "servicesApp", "ngMask"]);
})();
