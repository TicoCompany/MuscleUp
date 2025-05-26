(function () {
    const app = angular.module("app");

    app.controller("ProfessorListController", function ($scope, $http, $mensagem, $rootScope) {
        $scope.iniciar = function (json) {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10
            };
            $scope.listar();

            $scope.academias = json.Academias;
        };

        $scope.listar = function () {
            $rootScope.carregando = true;

            $http.get('/api/Professores?pagina=' + $scope.filtros.pagina + '&porPagina=' + $scope.filtros.porPagina)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $scope.professores = response.data.data.professores;
                        $scope.filtros.totalPaginas = response.data.data.totalPaginas;
                    }
                }, function (error) {
                    $mensagem.error("Erro ao listar os professores");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

        $scope.excluir = function (id) {
            $mensagem.confirm("Deseja realmente excluir o professor?")
                .then(function (resposta) {
                    if (resposta) {
                        $rootScope.carregando = true;
                        $http.delete(`/api/Professores/${id}`)
                            .then(function (response) {
                                if (!response.data.sucesso)
                                    $mensagem.error(`${response.data.mensagem}`);
                                else {
                                    $mensagem.success(response.data.mensagem);
                                    $scope.listar();
                                }
                            }, function (error) {
                                $mensagem.error("Erro ao excluír o professor");
                            }).finally(function () {
                                $rootScope.carregando = false;
                            });
                    }
                });
        };

    });

    app.controller("ProfessorController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function (json) {
            console.log(json);
            if (json.Id) {
                $http.get(`/api/Professores/${json.Id}`)
                    .then(function (response) {
                        if (!response.data.sucesso)
                            $mensagem.error(`${response.data.mensagem}`);
                        else {
                            $scope.Professor = response.data.data;
                        }
                    }, function (error) {
                        $mensagem.error("Erro ao excluír o professor");
                    }).finally(function () {
                        $rootScope.carregando = false;
                    });
            }

            $scope.academias = json.Academias;
        };

        $scope.salvar = function () {
            $rootScope.carregando = true;

            $http.post('/api/Professores', $scope.professor)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        location.href = "/Professor/Index"
                    }
                }, function (error) {
                    $mensagem.error("Erro ao salvar o Professor");
                }).finally(function () {
                    $rootScope.carregando = false;
       s         });
        };
    });

})();