(function () {
    const app = angular.module("app");

    app.controller("ExercicioListController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function () {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10
            };
            $scope.listar();
        };

        $scope.listar = function () {
            $rootScope.carregando = true;

            $http.get('/api/Exercicios?pagina=' + $scope.filtros.pagina + '&porPagina=' + $scope.filtros.porPagina)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $scope.exercicios = response.data.data.exercicios;
                        $scope.filtros.totalPaginas = response.data.data.totalPaginas;
                    }
                }, function (error) {
                    $mensagem.error("Erro ao listar exercícios");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

        $scope.excluir = function (id) {
            $mensagem.confirm("Deseja realmente excluir este exercício?")
                .then(function (resposta) {
                    if (resposta) {
                        $rootScope.carregando = true;
                        $http.delete(`/api/Exercicios/${id}`)
                            .then(function (response) {
                                if (!response.data.sucesso)
                                    $mensagem.error(`${response.data.mensagem}`);
                                else {
                                    $mensagem.success(response.data.mensagem);
                                    $scope.listar();
                                }
                            }, function (error) {
                                $mensagem.error("Erro ao excluír o exercício");
                            }).finally(function () {
                                $rootScope.carregando = false;
                            });
                    }
                });
        };

    });

    app.controller("ExercicioController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function (id) {
            if (id) {
                $http.get(`/api/Exercicios/${id}`)
                    .then(function (response) {
                        if (!response.data.sucesso)
                            $mensagem.error(`${response.data.mensagem}`);
                        else {
                            $scope.exercicio = response.data.data;
                        }
                    }, function (error) {
                        $mensagem.error("Erro ao excluír o exercício");
                    }).finally(function () {
                        $rootScope.carregando = false;
                    });
            }
        };

        $scope.salvar = function () {
            $rootScope.carregando = true;

            $http.post('/api/Exercicios', $scope.exercicio)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        location.href = "/Exercicio/Index"
                    }
                }, function (error) {
                    $mensagem.error("Erro ao salvar o exercício");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

    });

})();