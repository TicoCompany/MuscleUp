(function () {
    const app = angular.module("app");

    app.directive("step1", function () {
        return {
            restrict: "E",
            scope: {
                treino: "=",
                etapaAtual: "=",
                divisoes: "="
            },

            link: function (scope) {
                scope.proximaEtapa = function () {
                    $http.post('/api/Treinos/Step1', $scope.treino)
                        .then(function (response) {
                            if (!response.data.sucesso)
                                $mensagem.error(`${response.data.mensagem}`);
                            else {
                                $mensagem.success(response.data.mensagem);
                                location.href = "/treino/Index"
                            }
                        }, function (error) {
                            $mensagem.error("Erro ao salvar o treino");
                        }).finally(function () {
                            $rootScope.carregando = false;
                        });
                    scope.etapaAtual++;
                }
            },
            templateUrl: "/templates/treinos/step1.html"
        };
    });

    app.directive("step2", function () {
        return {
            restrict: "E",
            scope: {
                treino: "=",
                gruposMusculares: "="
            },

            link: function (scope) {
                scope.treino = {
                    divisao: 3, // ABC
                    divisoes: [
                        { nome: 'A', membros: [] },
                        { nome: 'B', membros: [] },
                        { nome: 'C', membros: [] }
                    ]
                };

                scope.novoMembroSelecionado = {};

                scope.adicionarMembro = function (dia) {
                    const membro = scope.novoMembroSelecionado[dia.nome];
                    if (!membro) return;

                    if (!dia.membros.some(m => m.id === membro.id)) {
                        dia.membros.push(membro);
                    }

                    scope.novoMembroSelecionado[dia.nome] = null;
                };

                scope.removerMembro = function (dia, membro) {
                    const idx = dia.membros.indexOf(membro);
                    if (idx >= 0) dia.membros.splice(idx, 1);
                };

                scope.membrosDisponiveis = [
                    { id: 1, nome: 'Peito' },
                    { id: 2, nome: 'Costas' },
                    { id: 3, nome: 'Bíceps' },
                    { id: 4, nome: 'Tríceps' },
                    { id: 5, nome: 'Pernas' },
                    { id: 6, nome: 'Ombros' }
                ];
            },
            templateUrl: "/templates/treinos/step2.html"
        };
    });

    app.directive("step3", function () {
        return {
            restrict: "E",
            scope: {
                treino: "="
            },

            link: function (scope) {
                console.log(scope.treino);
            },
            templateUrl: "/templates/treinos/step3.html"
        };
    });

})()   