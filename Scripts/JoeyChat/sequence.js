
$(function () {

    // 建立對應server端Hub class的proxy物件，請注意第一個字母要改成小寫
    var chat = $.connection.chatHub; // the SignalR Hub proxy

    //# 定義 client side method 
    // 定義client端的javascript function，供server端hub，透過dynamic的方式，呼叫所有Clients的javascript function
    chat.client.addMessage = function (message) {
        //當server端調用addMessage時，將server push的message資料，呈現在wholeMessage中
        $('#wholeMessages').append('<li>' + message + '</li>');
    };

    $("#send").click(function () {
        // 查看連線狀態
        if ($.connection.hub.state == 0)
            alert('on : connecting');
        else if ($.connection.hub.state == 2)
            alert('on : reconnecting');
        else if ($.connection.hub.state == 4)
            alert('on : disconnected');
        //else if ($.connection.hub.state == 1)
        //    alert('on : connected');

        //呼叫server端的Hub物件，將#message資料傳給server
        chat.server.sendMessage($('#message').val());
        $('#message').val("");
    });

    $.connection.hub.connectionSlow(function () {
        alert('on : connectionSlow');
    });

    $.connection.hub.reconnecting(function () {
        alert('on : reconnecting');
    });

    $.connection.hub.reconnected(function () {
        alert('on : reconnected');
    });

    $.connection.hub.disconnected(function () {
        alert('on : disconnected');
    });

    //把connection打開
    $.connection.hub.logging = true;
    $.connection.hub.start()
        .done(function () {
            var msg = 'Now connected, connection.hub.id =' + $.connection.hub.id;
            console.log(msg);
            alert(msg);
        })
        .fail(function () {
            var msg = 'To connect chatHub fail!';
            console.log(msg);
            alert(msg);
        });
});

