(function () {
    const app = angular.module("app");

    app.controller("ExercicioListController", function ($scope, $http, $mensagem, $rootScope, $timeout) {
        $scope.iniciar = function (json) {
            $scope.filtros = {
                pagina: 1,
                porPagina: 8
            };
            $scope.listar();
            $scope.academias = json.Academias;
            $scope.idAcademia = json.IdAcademia;
            $scope.gruposMusculares = json.GruposMusculares;
            $scope.dificuldades = json.Dificuldades;
        };

        $scope.listar = function () {
            $rootScope.carregando = true;
            let url = '/api/Exercicios' +
                '?Pagina=' + ($scope.filtros.pagina) +
                '&PorPagina=' + ($scope.filtros.porPagina) +
                '&Busca=' + ($scope.filtros.busca || '') +
                '&GrupoMuscular=' + ($scope.filtros.grupoMuscular || '') +
                '&Dificuldade=' + ($scope.filtros.dificuldade || '');
            $http.get(url)
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
        $scope.iniciar = function (json) {
            if (json.Id) {
                $http.get(`/api/Exercicios/${json.Id}`)
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
            } else {
                $scope.exercicio = {
                    idAcademia: json.IdAcademia,
                    id: 0,
                }
            }
            $scope.idAcademia = json.IdAcademia;
            $scope.gruposMusculares = json.GruposMusculares;
            $scope.dificuldades = json.Dificuldades;
        };

        $scope.$watch('exercicio.arquivo', function () {
            $timeout(function () {
                var arquivo = $scope.exercicio.arquivo;

                if (!arquivo) {
                    return;
                }

                var tiposPermitidos = ['image/png', 'image/jpeg', 'image/webp'];
                if (tiposPermitidos.indexOf(arquivo.type) === -1) {
                    $mensagem.error('Tipo de arquivo não suportado. Envie uma imagem PNG ou JPEG.');
                    return;
                }

                var tamanhoMaximo = 1 * 1024 * 1024; // 1 MB
                if (arquivo.size > tamanhoMaximo) {
                    alert('Imagem muito grande. Tamanho máximo permitido: 1MB.');
                    return;
                }

                var reader = new FileReader();
                reader.onload = function (e) {
                    $scope.$apply(function () {
                        $scope.exercicio.imagemBase64 = e.target.result;
                    });
                };

                reader.readAsDataURL(arquivo);
                console.log($scope.exercicio);
            }, 100);


        });

        $scope.salvar = function () {
            if ($scope.exercicio.id == 0 && !$scope.exercicio.arquivo)
                return $mensagem.error("Coloque uma imagem para salvar o exercício")

            $rootScope.carregando = true;
            console.log($scope.exercicio);
            var formData = new FormData();
            formData.append('Id', $scope.exercicio.id);
            formData.append('IdAcademia', $scope.exercicio.idAcademia ?? 0);
            formData.append('Nome', $scope.exercicio.nome);
            formData.append('Descricao', $scope.exercicio.descricao);
            formData.append('Dificuldade', $scope.exercicio.dificuldade);
            formData.append('GrupoMuscular', $scope.exercicio.grupoMuscular);
            formData.append('arquivo', $scope.exercicio.arquivo);
            formData.append('PublicId', $scope.exercicio.publicId ?? "");

            $http.post('/api/Exercicios', formData, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
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