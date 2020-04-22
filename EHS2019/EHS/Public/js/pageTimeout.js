/* JS Document */

var lastTime = new Date().getTime();
var currentTime = new Date().getTime();
var timeOut = 20 * 60 * 1000;


$(document).ready(function () {
    window.document.onclick = function () {
        lastTime = new Date().getTime();
    }

    function checkTimeout() {
        currentTime = new Date().getTime();

        if (currentTime - lastTime > timeOut) { //timeout
            var rtnValue;
            var currentDate = new Date();

            $.ajax({
                async: false,
                type: 'POST',
                url: "/Public/en/Error/TimeoutLog",
                dataType: 'json',
                data: JSON.stringify({
                    clientTime: currentDate
                }),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    rtnValue = data.Rtn;
                },
                complete: function (XMLHttpRequest, textStatus) {
                    var errorUrl = XMLHttpRequest.getResponseHeader("ErrorUrl");
                    if (errorUrl) {
                        var u = rootPath + rootLang + errorUrl;
                        location.href = u;
                    }
                }
            });

            var url = rootPath + rootLang + "/Error/PageTimeout";
            location.replace(url);
        }
    }

    window.setInterval(checkTimeout, 10000);
});
