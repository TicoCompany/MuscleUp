angular.module('app', [])
    .factory('$mensagem', function ($rootScope, $timeout, $q, $templateRequest, $compile) {
        let modalCarregado = false;

        function carregarTemplateModal() {
            return $templateRequest('/templates/confirm-modal.html')
                .then(function (html) {
                    const template = angular.element(html);
                    const compiled = $compile(template)($rootScope);
                    document.body.appendChild(compiled[0]);
                    modalCarregado = true;
                });
        }

        function exibirModalConfirmacao(mensagem) {
            const deferred = $q.defer();

            const modal = new bootstrap.Modal(document.getElementById("confirmModal"));
            document.getElementById("confirmModalMessage").innerText = mensagem;

            const btnSim = document.getElementById("confirmYes");
            const btnNao = document.getElementById("confirmNo");

            const limpar = () => {
                btnSim.removeEventListener("click", onSim);
                btnNao.removeEventListener("click", onNao);
            };

            const onSim = () => {
                limpar();
                modal.hide();
                deferred.resolve(true);
            };

            const onNao = () => {
                limpar();
                modal.hide();
                deferred.resolve(false);
            };

            btnSim.addEventListener("click", onSim);
            btnNao.addEventListener("click", onNao);

            modal.show();

            return deferred.promise;
        }

        return {
            success: function (texto, duracao = 3000) {
                this._mostrar('success', texto, duracao);
            },
            error: function (texto, duracao = 3000) {
                this._mostrar('error', texto, duracao);
            },
            warning: function (texto, duracao = 3000) {
                this._mostrar('warning', texto, duracao);
            },
            info: function (texto, duracao = 3000) {
                this._mostrar('info', texto, duracao);
            },
            confirm: function (mensagem) {
                if (!modalCarregado) {
                    return carregarTemplateModal().then(() => exibirModalConfirmacao(mensagem));
                } else {
                    return exibirModalConfirmacao(mensagem);
                }
            },
            _mostrar: function (tipo, texto, duracao) {
                $rootScope.mensagemGlobal = { tipo, texto };
                this._limpar(duracao);
            },
            _limpar: function (duracao) {
                $timeout(function () {
                    $rootScope.mensagemGlobal = null;
                }, duracao);
            },
        };
    });
