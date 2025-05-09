angular.module('app', [])
    .factory('$mensagem', function ($rootScope, $timeout) {
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
            _mostrar: function (tipo, texto, duracao) {
                $rootScope.mensagemGlobal = { tipo, texto };
            },
            _limpar: function () {
                $rootScope.mensagemGlobal = null;
            }
        };
    });

    
