(function () {
    const app = angular.module("app");

    app.controller("AlunoListController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function () {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10
            };
            $scope.listar();
        };

        $scope.listar = function () {
            $rootScope.carregando = true;

            $http.get('/api/Alunos?pagina=' + $scope.filtros.pagina + '&porPagina=' + $scope.filtros.porPagina)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $scope.alunos = response.data.data.alunos;
                        $scope.filtros.totalPaginas = response.data.data.totalPaginas;
                    }
                }, function (error) {
                    $mensagem.error("Erro ao listar os alunos");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

        $scope.excluir = function (id) {
            $mensagem.confirm("Deseja realmente excluir aluno?")
                .then(function (resposta) {
                    if (resposta) {
                        $rootScope.carregando = true;
                        $http.delete(`/api/Alunos/${id}`)
                            .then(function (response) {
                                if (!response.data.sucesso)
                                    $mensagem.error(`${response.data.mensagem}`);
                                else {
                                    $mensagem.success(response.data.mensagem);
                                    $scope.listar();
                                }
                            }, function (error) {
                                $mensagem.error("Erro ao excluír o aluno");
                            }).finally(function () {
                                $rootScope.carregando = false;
                            });
                    }
                });


        };

    });

    app.controller("AlunoController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function (id) {
            if (id) {
                $http.get(`/api/Alunos/${id}`)
                    .then(function (response) {
                        if (!response.data.sucesso)
                            $mensagem.error(`${response.data.mensagem}`);
                        else {
                            $scope.aluno = response.data.data;
                        }
                    }, function (error) {
                        $mensagem.error("Erro ao excluír o aluno");
                    }).finally(function () {
                        $rootScope.carregando = false;
                    });
            }


        };

        $scope.salvar = function () {
            $rootScope.carregando = true;

            $http.post('/api/Alunos', $scope.aluno)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        location.href = "/Aluno/Index"
                    }
                }, function (error) {
                    $mensagem.error("Erro ao salvar o aluno");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

    });

})();