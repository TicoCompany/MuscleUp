(function () {
    const app = angular.module("app");

    app.controller("UsuarioListController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function () {
            console.log("Matheus viadao")
        };
    });

    app.controller("UsuarioController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function () {
        };

        $scope.salvar = function () {

            $http.post('/api/Usuarios', $scope.usuario)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $messages.success(response.data.mensagem);
                        location.href = "Usuario/Index"
                    }
                })
                .catch(function (error) {
                    $mensagem.error("Erro ao salvar o usuário");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

    });

})();