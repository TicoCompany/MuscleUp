(function () {
    const app = angular.module("app");

    app.controller("LoginController", function ($scope, $http, $mensagem) {
        $scope.iniciar = function () {
            $mensagem.error("erro");
        };

        $scope.login = function () {
            $http.post('/api/Contas/Login', $scope.usuario)
                .then(function (response) {
                    console.log('Subtreino criado com sucesso:', response.data);
                })
                .catch(function (error) {
                    console.error('Erro ao criar subtreino:', error);
                });
        };

    });

})();