
/// ng-app
var app = angular.module('appMain', ['ngAnimate','ngSanitize','ui.bootstrap','blockUI']);

/// app config
app.config(['$logProvider', 'blockUIConfig', function ($logProvider, blockUIConfig) {
    $logProvider.debugEnabled(true); // turn on $log
    blockUIConfig.autoBlock = false; // default: true
}]);

/// app.constant
/// app value
app.value('appValue', {
    rootUrl: '/webApp_SignalRLab/', // 網站根網址 the root URL of this web application.
    state: 'CTOR',
    lastErrType: 'SUCCESS',
    lastErrMsg: undefined,
    lastErrDtm: undefined,
    lastErrClass: undefined,

    //# properties
    lastErr: function (errType, errMsg, errDtm, errClass) {
        // setter
        if (angular.isDefined(errType)) {
            this.lastErrType = errType;
            this.lastErrMsg = errMsg;
            this.lastErrDtm = (angular.isDefined(errDtm)) ? errDtm : (new Date());
            this.lastErrClass = errClass;
        }

        // getter
        return {
            errType: this.lastErrType,
            errMsg: this.lastErrMsg,
            errDtm: this.lastErrDtm,
            errClass: this.lastErrClass,
        };
    }
});

/// ng-controller
app.controller('appController', ['$scope', '$log', 'appValue', '$uibModal', function ($scope, $log, appValue, $uibModal) {

    //# ctor
    $scope.appValue = appValue;

    // common component : alertDialog
    $scope.callAlertDialog = function (msg, title, size) {
        var instance = $uibModal.open({
            animation: true,
            ariaLabelledBy: 'alert-dialog-title',
            ariaDescribedBy: 'alert-dialog-body',
            templateUrl: appValue.rootUrl.concat('Templates/alertDialogCom.html'),
            backdrop: true,
            size: size || '', // [,lg,sm]
            controller: ['$scope', '$uibModalInstance', function ($scope, $uibModalInstance) {
                $scope.title = title || '';
                $scope.msg = msg || 'no message';
                // event handler
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };

            }],
        });

        instance.result.then(function () {
            //alert('Get triggers when modal is closed');
        }, function () {
            // 事件：點擊到 modal 外面
            //alert('gets triggers when modal is dismissed.');
            // angular.js:14324 Possibly unhandled rejection: backdrop click
        });
    };

    //# event
    $scope.$on('LOADING_EVENT', function () {
        $log.info('on: LOADING_EVENT...');
        appValue.state = 'LOADING';
    });

    //# event
    $scope.$on('LOADED_EVENT', function () {
        $log.info('on: LOADED_EVENT...');
        appValue.state = 'LOADED';
    });

    //# event
    $scope.$on('SUCCESS_EVENT', function () {
        $log.info('on: SUCCESS_EVENT...');
        appValue.lastErr('SUCCESS');
    });

    //# event
    $scope.$on('ERROR_EVENT', function (event, lastErr) {
        $log.info('on: ERROR_EVENT...');
        appValue.lastErr(lastErr.errCode, lastErr.errMsg, lastErr.errDtm, lastErr.errClass);
    });

    //# event
    $scope.$on('EXCEPTION_EVENT', function (event) {
        $log.info('on: EXCEPTION_EVENT...');
        appValue.lastErr('EXCEPTION occured.');
    });

}]);
