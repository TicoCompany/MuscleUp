(function () {
    const app = angular.module("app");

    app.controller("LoginController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function () {
        };

        $scope.login = function () {
            $rootScope.carregando = true; 

            $http.post('/api/Contas/Login', $scope.usuario)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        location.href = "/Home"
                    }
                })
                .catch(function (error) {
                    $mensagem.error("Erro ao fazer login");
                }).finally(function () {
                    $rootScope.carregando = false; 
                });
        };

    });

})();