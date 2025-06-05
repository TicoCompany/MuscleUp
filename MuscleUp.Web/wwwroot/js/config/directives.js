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

app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

app.directive('paginador', function ($timeout) {
    return {
        restrict: 'E',
        scope: {
            aoPaginar: '&',
            totalPaginas: '=',
            paginaAtual: '='
        },
        template: `
        <nav>
            <ul class="pagination justify-content-center">
                <li class="page-item" ng-class="{ disabled: paginaAtual === 1 }">
                    <button class="page-link" ng-click="irParaPagina(paginaAtual - 1)">Anterior</button>
                </li>
                <li class="page-item"
                    ng-repeat="n in getPaginas()"
                    ng-class="{ active: n === paginaAtual }">
                    <button class="page-link" ng-click="irParaPagina(n)">{{ n }}</button>
                </li>
                <li class="page-item" ng-class="{ disabled: paginaAtual === totalPaginas }">
                    <button class="page-link" ng-click="irParaPagina(paginaAtual + 1)">Próxima</button>
                </li>
            </ul>
        </nav>
        `,
        link: function (scope) {
            scope.getPaginas = function () {
                var paginas = [];
                for (var i = 1; i <= scope.totalPaginas; i++) {
                    paginas.push(i);
                }
                return paginas;
            };

            scope.irParaPagina = function (pagina) {
                if (pagina < 1 || pagina > scope.totalPaginas) return;
                scope.paginaAtual = pagina;

                paginar();
            };

            function paginar() {
                $timeout(function () {
                    scope.aoPaginar();
                }, 0)
            }
        }
    };

});