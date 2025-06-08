(function () {
    const app = angular.module("app");

    app.controller("TreinoListController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function (idAcademia) {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10,
                idAcademia: idAcademia,
            };
            $scope.listar();
        };

        $scope.listar = function () {
            $rootScope.carregando = true;

            $http.get('/api/Treinos?Pagina=' + $scope.filtros.pagina + '&PorPagina=' + $scope.filtros.porPagina + '&IdAcademia=' + $scope.filtros.idAcademia)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $scope.treinos = response.data.data.treinos;
                        $scope.filtros.totalPaginas = response.data.data.totalPaginas;
                    }
                }, function (error) {
                    $mensagem.error("Erro ao listar treinos");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

        $scope.excluir = function (id) {
            $mensagem.confirm("Deseja realmente excluir este treino?")
                .then(function (resposta) {
                    if (resposta) {
                        $rootScope.carregando = true;
                        $http.delete(`/api/Treinos/${id}`)
                            .then(function (response) {
                                if (!response.data.sucesso)
                                    $mensagem.error(`${response.data.mensagem}`);
                                else {
                                    $mensagem.success(response.data.mensagem);
                                    $scope.listar();
                                }
                            }, function (error) {
                                $mensagem.error("Erro ao excluír o treino");
                            }).finally(function () {
                                $rootScope.carregando = false;
                            });
                    }
                });
        };

    });

    app.controller("TreinoController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function (json) {
            if (json.Id) {
                $rootScope.carregando = true;
                $http.get(`/api/Treinos/${json.Id}`)
                    .then(function (response) {
                        if (!response.data.sucesso)
                            $mensagem.error(`${response.data.mensagem}`);
                        else {
                            $scope.treino = response.data.data.treino;
                            console.log($scope.treino);
                        }
                    }, function (error) {
                        $mensagem.error("Erro ao buscar o treino");
                    }).finally(function () {
                        $rootScope.carregando = false;
                    });
            } else {
                $scope.treino = {
                    idAcademia: json.IdAcademia,
                }
            }
            $scope.divisoes = json.Divisoes;
            $scope.gruposMusculares = json.GruposMusculares;
            $scope.etapaAtual = 1;
            console.log(json);

        };

        $scope.etapaAtual = 1;

        $scope.treino = {
            nome: '',
            divisao: null,
            publico: null,
            tempo: ''
        };

        $scope.irParaEtapa = function (etapa) {
            $scope.etapaAtual = etapa;
        };

        $scope.proximaEtapa = function () {
            if ($scope.treino.nome && $scope.treino.divisao && $scope.treino.tempo !== '') {
                $scope.etapaAtual = 2;
            }
        };

        $scope.salvar = function () {
            $rootScope.carregando = true;

            $http.post('/api/treinos', $scope.treino)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        location.href = "/Treino/Index"
                    }
                }, function (error) {
                    $mensagem.error("Erro ao salvar o treino");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

    });

})();