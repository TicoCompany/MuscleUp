(function () {
    const app = angular.module("app");

    app.controller("AlunoListController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function (json) {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10,
                idAcademia: json.IdAcademia,
                busca: ""
            };
            $scope.idAcademia = json.IdAcademia;
            $scope.listar();
            $scope.academias = json.Academias;
        };

        $scope.listar = function () {
            $rootScope.carregando = true;

            $http.get('/api/Alunos?Pagina=' + $scope.filtros.pagina + '&PorPagina=' + $scope.filtros.porPagina + '&IdAcademia=' + $scope.filtros.idAcademia + '&Busca=' + $scope.filtros.busca)
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
            $mensagem.confirm("Deseja realmente excluir este aluno?")
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
        $scope.iniciar = function (json) {
            if (json.Id) {
                $http.get(`/api/Alunos/${json.Id}`)
                    .then(function (response) {
                        if (!response.data.sucesso)
                            $mensagem.error(`${response.data.mensagem}`);
                        else {
                            $scope.aluno = response.data.data;
                        }
                    }, function (error) {
                        $mensagem.error("Erro ao buscar o aluno");
                    }).finally(function () {
                        $rootScope.carregando = false;
                    });

            }
            $scope.academias = json.Academias;
        };

        $scope.salvar = function () {
            $rootScope.carregando = true;

            $http.post('/api/Alunos', $scope.aluno)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        location.href = "/Aluno/Index";
                    }
                }, function (error) {
                    $mensagem.error("Erro ao salvar o aluno");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };
    });

    app.controller("TreinosDoAlunoController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function (json) {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10,
                idAluno: json.Id,
                busca: ""
            };
            $scope.idAcademia = json.IdAcademia;
            $scope.listar();
            $scope.academias = json.Academias;
        };

        $scope.listar = function () {
            $rootScope.carregando = true;

            $http.get('/api/Alunos/ListarTreinosDestinados?Pagina=' + $scope.filtros.pagina + '&PorPagina=' + $scope.filtros.porPagina + '&IdAcademia=' + $scope.filtros.idAcademia + '&Busca=' + $scope.filtros.busca)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $scope.treinos = response.data.data.treinos;
                        $scope.filtros.totalPaginas = response.data.data.totalPaginas;
                    }
                }, function (error) {
                    $mensagem.error("Erro ao listar os treinos");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

        $scope.desvincularTreino = function (id) {
            $mensagem.confirm("Deseja realmente desvincular o treino do aluno?")
                .then(function (resposta) {
                    if (resposta) {
                        $rootScope.carregando = true;
                        $http.delete(`/api/Alunos/DesvincularTreino/${id}`, )
                            .then(function (response) {
                                if (!response.data.sucesso)
                                    $mensagem.error(`${response.data.mensagem}`);
                                else {
                                    $mensagem.success(response.data.mensagem);
                                    $scope.listar();
                                }
                            }, function (error) {
                                $mensagem.error("Erro ao desvincular o treino!");
                            }).finally(function () {
                                $rootScope.carregando = false;
                            });
                    }
                });


        };

    });

})();