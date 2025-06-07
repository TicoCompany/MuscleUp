(function () {
    const app = angular.module("app");

    app.directive("step1", function ($http, $rootScope, $mensagem) {
        return {
            restrict: "E",
            scope: {
                treino: "=",
                etapaAtual: "=",
                divisoes: "="
            },

            link: function (scope) {
                scope.proximaEtapa = function () {
                    $http.post('/api/Treinos/Step1', scope.treino)
                        .then(function (response) {
                            if (!response.data.sucesso)
                                $mensagem.error(`${response.data.mensagem}`);
                            else {
                                scope.treino.id = response.data.data.id
                                scope.treino.divisoes = response.data.data.divisoes;
                                scope.etapaAtual++;

                            }
                        }, function (error) {
                            $mensagem.error("Erro ao salvar as informações");
                        }).finally(function () {
                            $rootScope.carregando = false;
                        });
                }
            },
            templateUrl: "/templates/treinos/step1.html"
        };
    });

    app.directive("step2", function ($mensagem, $http, $rootScope) {
        return {
            restrict: "E",
            scope: {
                treino: "=",
                etapaAtual: "=",
                gruposMusculares: "="
            },

            link: function (scope) {
                scope.adicionarMembro = function (divisao) {
                    if (!divisao.membroSelecionado)
                        return $mensagem.error("Selecione um membro!");

                    if (!divisao.membros)
                        divisao.membros = [];

                    if (divisao.membros.some(q => q == divisao.membroSelecionado))
                        return $mensagem.error("Esse membro já foi inserido!");

                    divisao.membros.push(divisao.membroSelecionado);

                    divisao.membroSelecionado = null;
                };

                scope.removerMembro = function (dia, membro) {
                    const idx = dia.membros.indexOf(membro);
                    if (idx >= 0) dia.membros.splice(idx, 1);
                };

                scope.avancar = function () {
                    $http.post('/api/Treinos/Step2', scope.treino)
                        .then(function (response) {
                            if (!response.data.sucesso)
                                $mensagem.error(`${response.data.mensagem}`);
                            else {
                                scope.etapaAtual++;
                                console.log(scope.treino);
                            }
                        }, function (error) {
                            $mensagem.error("Erro ao salvar as informações");
                        }).finally(function () {
                            $rootScope.carregando = false;
                        });
                };

            },
            templateUrl: "/templates/treinos/step2.html"
        };
    });

    app.directive("step3", function ($http, $rootScope, $mensagem) {
        return {
            restrict: "E",
            scope: {
                treino: "="
            },

            link: function (scope) {
                const modalExercicio = new bootstrap.Modal('#modalExercicio', {
                    keyboard: false
                });
                scope.filtros = {
                    pagina: 1,
                    porPagina: 8
                };

                scope.treino = {
                    nome: 'd',
                    divisao: 2,
                    publico: true,
                    tempo: 'ddas',
                    id: 20,
                    divisoes: [
                        {
                            divisaoDeSubTreino: 0,
                            nomeDaDivisao: 'A',
                            membroSelecionado: null,
                            membros: [{ nome: "Peito", grupoMuscular: 0 }, { nome: "Costas", grupoMuscular: 1 }]
                        },
                        {
                            divisaoDeSubTreino: 1,
                            nomeDaDivisao: 'B',
                            membroSelecionado: null,
                            membros: [{ nome: "Peito", grupoMuscular: 0 }, { nome: "Costas", grupoMuscular: 1 }]
                        },
                        {
                            divisaoDeSubTreino: 2,
                            nomeDaDivisao: 'C',
                            membroSelecionado: null,
                            membros: [{ nome: "Peito", grupoMuscular: 0 }, { nome: "Costas", grupoMuscular: 1 }]
                        }
                    ]
                };

                scope.adicionarExercicio = function (membro) {
                    if (!membro.exercicios)
                        membro.exercicios = [];
                    scope.membroSelecionado = membro;
                    scope.listarExerciciosPorMembro();
                    modalExercicio.show();
                };

                scope.selecionarExercicio = function (exercicio) {
                    exercicio.Selecionado = true;
                    scope.membroSelecionado.exercicios.push({ idExercicio: exercicio.id, nomeDoExercicio: exercicio.nome, caminho: exercicio.caminho });
                    console.log(scope.treino.divisoes);
                };

                scope.removerExercicio = function (exercicio) {
                    exercicio.Selecionado = false;
                    scope.membroSelecionado.exercicios.filter(q => q.idExercicio != exercicio.id)
                };

                scope.listarExerciciosPorMembro = function () {
                    $rootScope.carregando = true;
                    $http.get('/api/Exercicios/ListarPorMembroMuscular?Pagina=' + scope.filtros.pagina + '&PorPagina=' + scope.filtros.porPagina + '&Busca=' + (scope.filtros.busca || '') + '&GrupoMuscular=' + scope.membroSelecionado.grupoMuscular)
                        .then(function (response) {
                            if (!response.data.sucesso)
                                $mensagem.error(`${response.data.mensagem}`);
                            else {
                                response.data.data.exercicios.forEach(r => {
                                    if (scope.membroSelecionado.exercicios.some(q => q.idExercicio == r.id))
                                        r.Selecionado = true;
                                });
                                scope.exercicios = response.data.data.exercicios;

                                scope.filtros.totalPaginas = response.data.data.totalPaginas;
                            }
                        }, function (error) {
                            $mensagem.error("Erro ao listar exercícios");
                        }).finally(function () {
                            $rootScope.carregando = false;
                        });
                }


                scope.abaAtiva = 0;
                scope.trocarTab = function (index) {
                    scope.abaAtiva = index;
                };
            },
            templateUrl: "/templates/treinos/step3.html"
        };
    });

})()   