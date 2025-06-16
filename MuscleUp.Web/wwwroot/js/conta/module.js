(function () {
    const app = angular.module("app");

    app.controller("LoginController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function () {
        };

        $scope.login = function () {
            $rootScope.carregando = true;

            $http.post('http://localhost:8080/auth/login', $scope.usuario)
                .then(function (response) {
                    $mensagem.success("Login realizado com sucesso!");
                    var usuario = response.data;
                    sessionStorage.setItem("usuarioId", usuario.id);
                    sessionStorage.setItem("usuarioNome", usuario.name);

                    location.href = "/Home";
                })
                .catch(function (error) {
                    $mensagem.error("Erro ao fazer login!");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };
    });

    app.controller("CadastroController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.cadastrar = function () {
            $rootScope.carregando = true;
            $http.post('http://localhost:8080/auth/register', $scope.usuario)
                .then(function (response) {
                    $mensagem.success("Conta cadastrada com sucesso!");
                    location.href = "/Conta/Login";
                })
                .catch(function (error) {
                    $mensagem.error("Erro ao criar a conta!");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

    });

})();