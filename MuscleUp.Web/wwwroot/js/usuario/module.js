(function () {
    const app = angular.module("app");

    app.controller("UsuarioListController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function () {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10
            };
            $scope.listar();
        };

        $scope.listar = function () {
            $rootScope.carregando = true;

            $http.get('/api/Usuarios?pagina=' + $scope.filtros.pagina + '&porPagina=' + $scope.filtros.porPagina)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $scope.usuarios = response.data.data.usuarios;
                        $scope.filtros.totalPaginas = response.data.data.totalPaginas;
                    }
                }, function (error) {
                    $mensagem.error("Erro ao listar usuários");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

        $scope.excluir = function (id) {
            $mensagem.confirm("Deseja realmente excluir este usuário?")
                .then(function (resposta) {
                    if (resposta) {
                        $rootScope.carregando = true;
                        $http.delete(`/api/Usuarios/${id}`)
                            .then(function (response) {
                                if (!response.data.sucesso)
                                    $mensagem.error(`${response.data.mensagem}`);
                                else {
                                    $mensagem.success(response.data.mensagem);
                                    $scope.listar();
                                }
                            }, function (error) {
                                $mensagem.error("Erro ao excluír o usuário");
                            }).finally(function () {
                                $rootScope.carregando = false;
                            });
                    }
                });
        };

    });

    app.controller("UsuarioController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function (id) {
            if (id) {
                $http.get(`/api/Usuarios/${id}`)
                    .then(function (response) {
                        if (!response.data.sucesso)
                            $mensagem.error(`${response.data.mensagem}`);
                        else {
                            $scope.usuario = response.data.data;
                        }
                    }, function (error) {
                        $mensagem.error("Erro ao excluír o usuário");
                    }).finally(function () {
                        $rootScope.carregando = false;
                    });
            }
        };

        $scope.salvar = function () {
            $rootScope.carregando = true;

            $http.post('/api/Usuarios', $scope.usuario)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        location.href = "/Usuario/Index"
                    }
                }, function (error) {
                    $mensagem.error("Erro ao salvar o usuário");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

    });

})();