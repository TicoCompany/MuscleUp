const app = angular.module("app");

app.directive("loaderGlobal", function () {
    return {
        restrict: "E",
        template: `
        <div class="anterior-loader" ng-show="carregando">
            <div class="loader-overlay">
                <div class="spinner-border" role="status">
                    <span class="visually-hidden">Carregando...</span>
                </div>
            </div>
        </div>
        `,
        link: function (scope, element, attrs) {
            scope.$watch('carregando', function (newVal) {
                if (newVal) {
                    element.find(".loader-overlay").fadeIn();
                } else {
                    element.find(".loader-overlay").fadeOut();
                }
            });
        }
    };
});