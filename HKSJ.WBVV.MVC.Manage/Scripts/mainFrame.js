function calcPageHeight() {
    var cHeight = Math.max(document.body.clientHeight, document.documentElement.clientHeight);
    var sHeight = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
    var height = Math.max(cHeight, sHeight);
    return height;
}
$(document).ready(function () {
    var height = calcPageHeight();
    var ifarme = parent.document.getElementById('mainFrame');
    if (ifarme) {
        ifarme.style.height = height + 'px';
    }
});