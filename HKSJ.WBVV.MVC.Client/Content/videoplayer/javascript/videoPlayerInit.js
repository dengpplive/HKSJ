
var id;

function doInit(obj) {
    var hasFlash = chkFlashPlayer();//检查flash插件
    var videoInfo = obj.videoPathInfo;//getVideoInfo();//播放路径
    var advertInfo = obj.advertPathInfo;//getArtInfo();//广告路径
    if (hasFlash) {
        var flashWidth = typeof(obj.fWidth) == 'undefined' ? 900 : obj.fWidth;
        var flashHeight = typeof(obj.fHeight) == 'undefined' ? 510 : obj.fHeight;

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
    } else {
        $("#setupFlash").attr("style", "display:block;");//提示安装flash插件
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

function toDownLoadFlash() {
    window.open("https://get.adobe.com/cn/flashplayer/");
}

//播放器关灯
function turnOff() {
    $("body").append("<div id='light' style='width:100%;height:100%;z-index:2;background:#333;position:fixed;top:0;'></div>");
    $("#divFlashPlayer_huixin").attr("style", "z-index:3;position:relative;");
}

//播放器开灯
function turnOn() {
    $("#light").remove();
    $("body").removeAttr("style");
}

//隐藏进度条和禁用双击事件
function miniSwf() {
    getSWF("divFlashPlayer").mini();
}

//显示进度条和启用双击事件
function normalSwf() {
    getSWF("divFlashPlayer").normal();
}