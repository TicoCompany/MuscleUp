(function () {
    const app = angular.module("app");

    app.directive("step1", function ($http, $rootScope, $mensagem) {
        return {
            restrict: "E",
            scope: {
                treino: "=",
                etapaAtual: "=",
                divisoes: "=",
                dificuldades: "="
            },

            link: function (scope) {
                scope.proximaEtapa = function () {
                    if (!scope.treino.id) {
                        $mensagem.confirm(`Tem certeza que deseja salvar o treino? Após salvar a divisão do treino não poderá ser alterada!`)
                            .then(function (resposta) {
                                if (resposta) {
                                    salvarStep1();
                                }
                            });
                    } else {
                        salvarStep1();
                    }
                };

                function salvarStep1() {
                    $rootScope.carregando = true;
                    $http.post('/api/Treinos/Step1', scope.treino)
                        .then(function (response) {
                            if (!response.data.sucesso)
                                $mensagem.error(`${response.data.mensagem}`);
                            else {
                                if (!scope.treino.divisoes || scope.treino.divisoes.length < 1) {
                                    if (!scope.treino.id) {
                                        const url = new URL(window.location.href);
                                        url.searchParams.set('id', response.data.data.id);
                                        window.history.replaceState({}, '', url);
                                        scope.treino.id = response.data.data.id
                                    }
                                    scope.treino.divisoes = response.data.data.divisoes;
                                }

                                scope.etapaAtual++;
                            }
                        }, function (error) {
                            $mensagem.error("Erro ao salvar as informações");
                        }).finally(function () {
                            $rootScope.carregando = false;
                        });
                }

                scope.voltar = function () {
                    location.href = "/Treino/Index";
                };
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
                    console.log(divisao.membroSelecionado);
                    console.log(divisao.membros);
                    if (divisao.membros.some(q => q.grupoMuscular == divisao.membroSelecionado.grupoMuscular))
                        return $mensagem.error("Esse membro já foi inserido!");

                    divisao.membros.push(divisao.membroSelecionado);

                    divisao.membroSelecionado = null;
                };

                scope.voltar = function () {
                    scope.etapaAtual--;
                }

                scope.avancar = function () {
                    if (scope.treino.divisoes.some(q => q.membros.length < 1))
                        return $mensagem.error("Insira ao menos um grupo muscular por dia!");

                    $rootScope.carregando = true;

                    $http.post('/api/Treinos/Step2', scope.treino)
                        .then(function (response) {
                            if (!response.data.sucesso)
                                $mensagem.error(`${response.data.mensagem}`);
                            else {
                                let membrosSalvos = response.data.data.membrosSalvos;

                                scope.treino.divisoes.forEach(divisao => {
                                    divisao.membros.forEach(membro => {
                                        let membroDoBanco = membrosSalvos.find(q => q.grupoMuscular == membro.grupoMuscular);
                                        membro.id = membroDoBanco.idDoMembro;
                                    });
                                });
                                scope.etapaAtual++;
                            }
                        }, function (error) {
                            $mensagem.error("Erro ao salvar as informações");
                        }).finally(function () {
                            $rootScope.carregando = false;
                        });
                };

                scope.removerMembro = function (divisao, membro) {
                    if (!membro.id) {
                        divisao.membros = divisao.membros.filter(q => q != membro);

                    } else {
                        $mensagem.confirm(`Deseja realmente excluir o membro ${membro.nome} deste treino ? Exercícios vinculados a ele serão removidos.`)
                            .then(function (resposta) {
                                if (resposta) {
                                    $rootScope.carregando = true;
                                    $http.delete(`/api/Treinos/ExcluirGrupoMuscular/${membro.id}`)
                                        .then(function (response) {
                                            if (!response.data.sucesso)
                                                $mensagem.error(`${response.data.mensagem}`);
                                            else {
                                                divisao.membros = divisao.membros.filter(q => q != membro);
                                            }
                                        }, function (error) {
                                            $mensagem.error("Erro ao excluír o grupo muscular");
                                        }).finally(function () {
                                            $rootScope.carregando = false;
                                        });
                                }
                            });
                    }

                };


            },
            templateUrl: "/templates/treinos/step2.html"
        };
    });

    app.directive("step3", function ($http, $rootScope, $mensagem, $timeout) {
        return {
            restrict: "E",
            scope: {
                treino: "=",
                etapaAtual: "="
            },

            link: function (scope) {
                const modalExercicio = new bootstrap.Modal('#modalExercicio', {
                    keyboard: false
                });
                scope.filtros = {
                    pagina: 1,
                    porPagina: 8
                };
                scope.abaAtiva = 0;

                scope.trocarTab = function (index) {
                    scope.abaAtiva = index;
                };

                //scope.treino = {
                //    nome: 'd',
                //    divisao: 2,
                //    publico: true,
                //    tempo: 'ddas',
                //    id: 20,
                //    divisoes: [
                //        {
                //            divisaoDeSubTreino: 0,
                //            nomeDaDivisao: 'A',
                //            membroSelecionado: null,
                //            membros: [{ nome: "Peito", grupoMuscular: 0 }, { nome: "Costas", grupoMuscular: 1 }]
                //        },
                //        {
                //            divisaoDeSubTreino: 1,
                //            nomeDaDivisao: 'B',
                //            membroSelecionado: null,
                //            membros: [{ nome: "Peito", grupoMuscular: 0 }, { nome: "Costas", grupoMuscular: 1 }]
                //        },
                //        {
                //            divisaoDeSubTreino: 2,
                //            nomeDaDivisao: 'C',
                //            membroSelecionado: null,
                //            membros: [{ nome: "Peito", grupoMuscular: 0 }, { nome: "Costas", grupoMuscular: 1 }]
                //        }
                //    ]
                //};

                scope.adicionarExercicio = function (membro) {
                    if (!membro.exercicios)
                        membro.exercicios = [];

                    scope.exercicios = [];
                    scope.membroSelecionado = membro;
                    scope.listarExerciciosPorMembro();
                    modalExercicio.show();
                };

                scope.voltar = function () {
                    scope.etapaAtual--;
                }

                scope.selecionarExercicio = function (exercicio) {
                    exercicio.Selecionado = true;
                    scope.membroSelecionado.exercicios.push({ idExercicio: exercicio.id, nomeDoExercicio: exercicio.nome, caminho: exercicio.caminho });
                };

                scope.removerExercicio = function (exercicio) {
                    exercicio.Selecionado = false;
                    scope.membroSelecionado.exercicios = scope.membroSelecionado.exercicios.filter(q => q.idExercicio != exercicio.id)
                };

                scope.aplicarPadrao = function (divisao) {
                    if (!divisao.repeticaoPadrao || !divisao.seriePadrao)
                        return $mensagem.error("Informe os valores padrão para aplicar!");

                    divisao.membros.forEach(membro => {
                        membro.exercicios.forEach(exercicio => {
                            exercicio.serie = divisao.seriePadrao;
                            exercicio.repeticao = divisao.repeticaoPadrao;
                        });
                    });
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
                };

                scope.finalizar = function () {
                    $rootScope.carregando = true;
                    $http.post('/api/Treinos/Step3', scope.treino)
                        .then(function (response) {
                            if (!response.data.sucesso)
                                $mensagem.error(`${response.data.mensagem}`);
                            else {
                                $mensagem.success(response.data.mensagem);
                                location.href = "/Treino/Index";
                            }
                        }, function (error) {
                            $mensagem.error("Erro ao salvar as informações");
                        }).finally(function () {
                            $rootScope.carregando = false;
                        });
                };
            },
            templateUrl: "/templates/treinos/step3.html"
        };
    });

})()   