
var id;

function doInit(obj) {
    var hasFlash = chkFlashPlayer();//检查flash插件
    // var videoInfo = getVideoInfo();//播放路径
    var videoInfo = obj.videoPathInfo;//播放路径
    var advertInfo = "";//getArtInfo();//广告路径

    if (hasFlash) {
        var flashWidth = obj.fWidth == undefined ? 900 : obj.fWidth;//900;
        var flashHeight = obj.fHeight == undefined ? 510 : obj.fHeight;//510;

        var params = {
            crossDomainPath: getDomain(),
            httpUrl: "",
            httpShotUrl: "",
            videoInfo: videoInfo,
            advertInfo: advertInfo,
            controlbar: "true",
            wmode: "opaque",
            flashWidth: flashWidth,
            flashHeight: flashHeight
        }; 
        id = obj.id;
        renderFlash(id, rootPath + "/Content/videoplayer/flash/Main.swf?v=1.02", params);
    }
}

//七牛跨域策略文件地址
function getDomain() {
    var path = "";
    $.ajax({
        type: "get",
        url: api + "QiniuUpload/GetVideoUrl?key=crossdomain.xml",
        async: false,
        success: function (data) {
            path = data;
        }
    });
    return path;
}

document.onmouseup = function (event) {
    var swfObj = getSWF(id);

    if (swfObj) {
        try {
            if (swfObj.mouseupHandle && typeof (swfObj.mouseupHandle) == "function") {
                swfObj.mouseupHandle();
            }
        } catch (e) {
        }
    }
};