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
            var url = rootPath + rootLang + "/Error/PageTimeout";
            location.replace(url);
        }
    }

    window.setInterval(checkTimeout, 10000);
});
