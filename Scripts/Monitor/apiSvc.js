/// ng-service to call WEBAPI
app.service('apiSvc', ['appValue', '$log', function (appValue, $log) {

    //// $promise
    //var checkInWork_Promise = function (param) {
    //    return $http({
    //        method: 'POST',
    //        url: appValue.rootUrl.concat('CheckIn/CheckInWork'),
    //        data: $.param(param),
    //        headers: {
    //            'Content-Type': 'application/x-www-form-urlencoded',
    //            __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val()
    //        }
    //    });
    //};

    //# private methods : stratagy pattern --- 非同步 --- without block UI, without $emit event
    var handleAjaxPromise = function (apiName, httpPromise, successHandler, errorHandler, finallyHandler) {
        $log.log('on: ' + apiName + ' ...');
        httpPromise.then(function (response) { // success
            //$log.log('handleHttpPromise → may success');
            // response.status === 200
            var data = response.data;
            if (angular.isDefined(data.errType)) // fail! when the data is "lastErr"
            {
                // success maybe
                if (data.errType == 'SUCCESS') {
                    // success
                    $log.info(apiName + ' → SUCCESS');
                    if (typeof successHandler === "function")
                        successHandler(data); // invoke errorHandler
                }
                else {
                    // error
                    $log.info(apiName + ' → ERROR');
                    if (typeof errorHandler === "function")
                        errorHandler(data); // invoke errorHandler
                }
            }
            else {
                // success
                $log.info(apiName + ' → SUCCESS');
                if (typeof successHandler === "function")
                    successHandler(data); // invoke successHandler
            }
        }, function (response) { // error
            $log.warn(apiName + ' → error');
            // 邏輯上應該不會發生
        }).catch(function (exception) {
            $log.error(apiName + ' → catch');
            // 應該永遠都不會發生
        }).finally(function () {
            $log.log(apiName + ' → finally');
            if (typeof finallyHandler === "function")
                finallyHandler(); // invoke finallyHandler
        });
    };

    ////# public methods
    //this.qryDataList = function (param, successHandler, errorHandler, finallyHandler) {
    //    handleHttpPromise(
    //        'apiSvc.qryDataList', // api name
    //        qryDataList_Promise(param),
    //        successHandler,
    //        errorHandler,
    //        finallyHandler);
    //};


}]);

