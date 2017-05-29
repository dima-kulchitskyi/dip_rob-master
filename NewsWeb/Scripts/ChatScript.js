var socket,
$txt = document.getElementById('message'),
$user = document.getElementById('user'),
$messages = document.getElementById('messages');
if (typeof (WebSocket) !== 'undefined') {
    socket = new WebSocket("@uri");
} else {
    socket = new MozWebSocket("@uri");
}
socket.onmessage = function (msg) {
    var data = JSON.parse(msg.data);
    if (data.type == "msg") {
        var $el = document.createElement('p');
        $el.innerHTML = data.msg;
        $messages.appendChild($el);
    } else if (data.type == "clear") {
        $messages.innerHTML = "";
    }
    else if (data.type == "msgs") {
        console.log(data);
        for (var i = 0; i < data.msg.length; i++) {
            var $el = document.createElement('p');
            $el.innerHTML = data.msg[i];
            $messages.appendChild($el);
        }
    }
};
socket.onclose = function (event) {
    alert('Мы потеряли её. Пожалуйста, обновите страницу');
};
document.getElementById('send').onclick = function () {
    if ($user.value != "" || $txt.value != "") {
        socket.send($user.value + ' : ' + $txt.value);
    }
    $txt.value = '';
};

function GetAccess() {
    $.ajax({
        type: "POST",
        dataType: "Json",
        url: '/Home/GetAccess',
        async: false,
        success: function (data) {
            console.log(data);
            if (data == null || data.type == null) {
                alert('NEZBS');
            }
            else if (data.type == "access") {
                socket.send(data.id + ' - ' + data.token);
            }
            else if (data.type == "nolog") {
                socket.send('NOLOGIN');
            }
        }
    });
}