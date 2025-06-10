(function () {
    const app = angular.module("app");

    app.controller("TreinoListController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        const modalAluno = new bootstrap.Modal('#modalAluno', {
            keyboard: false
        });

        $scope.iniciar = function (idAcademia) {
            $scope.filtros = {
                pagina: 1,
                porPagina: 10,
                idAcademia: idAcademia,
            };

            $scope.filtrosAluno = {
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

        $scope.abrirModalDeAlunos = function (treino) {
            $scope.request = {
                idTreino: treino.id,
                alunosSelecionados: []
            }
            $scope.listarAlunos();
            $scope.treinoSelecionado = treino;
            modalAluno.show();
        };

        $scope.selecionarAluno = function (aluno) {
            aluno.selecionado = true;
            $scope.request.alunosSelecionados.push(aluno);
        };

        $scope.removerAluno = function (aluno) {
            aluno.selecionado = false;
            $scope.request.alunosSelecionados = $scope.request.alunosSelecionados.filter(q => q.id != aluno.id);
        };

        $scope.listarAlunos = function () {
            $rootScope.carregando = true;

            $http.get('/api/Alunos?Pagina=' + $scope.filtrosAluno.pagina + '&PorPagina=' + $scope.filtrosAluno.porPagina + '&IdAcademia=' + $scope.filtrosAluno.idAcademia + '&Busca=' + ($scope.filtrosAluno.busca || ''))
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        let alunos = response.data.data.alunos;
                        alunos.forEach(r => {
                            if ($scope.request.alunosSelecionados.some(q => q.id == r.id))
                                r.selecionado = true;
                        });
                        $scope.alunos = alunos;
                        $scope.filtros.totalPaginas = response.data.data.totalPaginas;
                    }
                }, function (error) {
                    $mensagem.error("Erro ao listar os alunos");
                }).finally(function () {
                    $rootScope.carregando = false;
                });
        };

        $scope.enviarTreinoAosAlunos = function () {
            if ($scope.request.alunosSelecionados.length < 1)
                return $mensagem.error("Selecione ao menos um aluno para enviar o treino");

            $scope.request.idsDosAlunos = $scope.request.alunosSelecionados.map(q => q.id);
            $rootScope.carregando = true;
            $http.post('/api/Treinos/EnviarParaAlunos', $scope.request)
                .then(function (response) {
                    if (!response.data.sucesso)
                        $mensagem.error(`${response.data.mensagem}`);
                    else {
                        $mensagem.success(response.data.mensagem);
                        modalAluno.hide();
                    }
                }, function (error) {
                    $mensagem.error("Erro ao enviar treino");
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
            $scope.dificuldades = json.Dificuldades;
            $scope.gruposMusculares = json.GruposMusculares;
            $scope.etapaAtual = 1;
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