/// ng-controller
// , blockUI, apiSvc, $filter, $timeout, $window, $location
app.controller('mainCtrl', ['$scope', '$log', 'appValue', 'apiSvc', function ($scope, $log, appValue, apiSvc) {

    //# ctor.
    $scope.appValue = appValue; // ref    
    $scope.dataList = []; // array element: {notifyType, notifyMsg}
    $scope.formData = {
        //type: undefined,
        //userId: undefined,
        //workDate: undefined,
        //checkTime: undefined,
    };
    $scope.idName = undefined;

    //# SignlaR hub proxy
    // 建立對應server端Hub class的proxy物件，請注意第一個字母要改成小寫
    var hubProxy = $.connection.inboxHub;

    // 定義client端的javascript function，供server端hub，透過dynamic的方式，呼叫所有Clients的javascript function
    hubProxy.client.notifyMessage = function (msg) {
        $log.log('on : notifyMessage');
        $log.log(msg);

        // do something
        $scope.dataList.push(msg); // enqueue
        if ($scope.dataList.length > 6) {
            $scope.dataList.shift(); // dequeue
        }

        $scope.$apply(); // 此處效果同refresh UI，但不可爛用 $apply 會死人的。
    };

    //# method
    $scope.startMonitorHub = function (enable) {
        $.connection.hub.logging = true;
        $.connection.hub.start()
            .done(function () {
                var msg = 'Now connected, connection.hub.id =' + $.connection.hub.id;
                $log.log(msg);
                //$scope.$apply(); // refresh UI
                alert(msg);
            })
            .fail(function () {
                var msg = 'To connect InboxHub fail!';
                $log.log(msg);
                //$scope.$apply(); // refresh UI
                alert(msg);
            });
    }

    //# method
    $scope.stopMonitorHub = function () {
        $.connection.hub.stop()
            .done(function () {
                var msg = 'The InboxHub connection has stopped.';
                $log.log(msg);
                //$scope.$apply(); // refresh UI
                alert(msg);
            })
            .fail(function () {
                var msg = 'To stop InboxHub fail!';
                $log.log(msg);
                //$scope.$apply(); // refresh UI
                alert(msg);
            });
    }

    //# 連線訊息
    $.connection.hub.connectionSlow(function () {
        $log.log('on : connectionSlow');
        alert('on : connectionSlow');
    });

    $.connection.hub.reconnecting(function () {
        $log.log('on : reconnecting');
        alert('on : reconnecting');
    });

    $.connection.hub.reconnected(function () {
        $log.log('on : reconnected');
        alert('on : reconnected');
    });

    $.connection.hub.disconnected(function () {
        $log.log('on : disconnected');
        alert('on : disconnected');
    });

    //# properties
    Object.defineProperty($scope, 'hobConnId', {
        get: function () {
            // 連線ID
            return $.connection.hub.id;
        }
    });

    Object.defineProperty($scope, 'hobConnState', {
        get: function () {
            // 查看連線狀態
            if ($.connection.hub.state == 0)
                return '0.connecting';
            else if ($.connection.hub.state == 1)
                return '1.connected';
            else if ($.connection.hub.state == 2)
                return '2.reconnecting';
            else if ($.connection.hub.state == 4)
                return '4.disconnected';
            // otherwise
            return $.connection.hub.state;
        }
    });

    ////# properties
    //// mode := [ ONWORK | OFFWORK | WORKLOG ]
    //var _mode = 'WORKLOG';
    //let _modes = ['ONWORK', 'OFFWORK', 'WORKLOG'];
    //Object.defineProperty($scope, "mode", {
    //    get: function () {
    //        return _mode;
    //    },
    //    set: function (v) {
    //        if (_modes.indexOf(v) > -1)
    //            _mode = v;
    //        else
    //            _mode = 'WORKLOG';
    //
    //        if (_mode == 'WORKLOG')
    //            $scope.qryDataList();
    //    }
    //});

    ////# behavior method - query
    //$scope.qryDataList = function () {
    //    // 載入資料
    //    apiSvc.qryDataList({}, // param
    //        function (data) {  // success handler
    //            $scope.dataList = data;
    //        },
    //        function (lastErr) { // error handler
    //            $scope.callAlertDialog(lastErr.errMsg);
    //        });
    //};

    ////# behavior method - db access
    //$scope.checkInWork = function () {
    //
    //    // 日期格式處理
    //    $scope.formData.checkTimeF = $filter('date')($scope.formData.checkTime, 'yyyy/MM/dd HH:mm:ss');
    //
    //    // 打卡
    //    apiSvc.checkInWork($scope.formData, // param
    //        function (data) {  // success handler
    //            var msg = $scope.formData.type == 'ONWORK' ? '簽到成功。'
    //                    : $scope.formData.type == 'OFFWORK' ? '簽退成功。'
    //                    : '打卡成功。';
    //            $scope.callAlertDialog(msg, '訊息');
    //            $scope.formData.type = 'WORKLOG'; // change mode
    //        },
    //        function (lastErr) { // error handler
    //            $scope.callAlertDialog(lastErr.errMsg); // 
    //        });
    //};

    ////# watch
    //$scope.$watch('formData.type', function (newValue, oldValue) {
    //    // ignore when newValue == oldValue.
    //    if (newValue == oldValue) return;
    //    $log.log('$watch(formData.type): '.concat(oldValue).concat(' → ').concat(newValue));
    //    // to set mode
    //    $scope.formData.checkTime = getCheckDate();
    //    $scope.mode = newValue;
    //}, false);

    //# method
    $scope.addNotifyMessage = function (notifyType, notifyMsg) {
        $scope.dataList.push({
            notifyType: notifyType,
            notifyMsg: notifyMsg
        });
    };

    //# method
    $scope.resetDataList = function () {
        $scope.dataList = [];
    };

    ////# utilities
    //function getCheckDate() {
    //    var now = new Date();
    //    return new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes());
    //}

    //# event handler
    $scope.registerUser = function()
    {
        $log.log('on: registerUser');
        let idName = $('#idName').val();
        hubProxy.server.registerUser(idName)
            .done(function () {
                $scope.idName = idName;
                var msg = 'registerUser done.';
                $log.log(msg);
                $scope.$apply(); // refresh UI
                alert(msg);
            });
    }

    ////# event handler
    //$scope.doRefresh = function () {
    //    $log.log('on: doRefresh...');
    //    $scope.qryDataListFirstPage();
    //};

    //# init.
    $scope.init = function () {
        $log.log('on: init...');

        $scope.startMonitorHub(true);

        //$scope.addNotifyMessage('success', '白日依山盡，XXX');
        //$scope.addNotifyMessage('info', '黃河入海流。YYY');
        //$scope.addNotifyMessage('warning', '欲窮千里目，AAA');
        //$scope.addNotifyMessage('danger', '更上一層樓。BBB');
    }

}]);