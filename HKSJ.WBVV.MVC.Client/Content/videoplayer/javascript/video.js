/*
 *该js为视频网站通用方法的存放处 
 */


//获取广告
function getArtInfo() {
    //return "http://7xlsse.com2.z0.glb.qiniucdn.com/1443493166487_A.mp4" + "," + "31" + "," + "http://www.baidu.com";

    return "http://7xliow.com2.z0.glb.qiniucdn.com/1441172688325_600x480.mp4" + "," + "10" + "," + "http://1.178pb.com" + "#";
    //return "http://7xliow.com2.z0.glb.qiniucdn.com/1441172688325_600x480.mp4"+","+"10"+","+"http://jd.com"+"#" + "http://7xliow.com2.z0.glb.qiniucdn.com/1442044625323_A.mp4"+","+"15"+","+"http://1.178pb.com"+"#";   

}

//获取视频文件信息
function getVideoInfo() {
    return "0" + "," + "0" + "," + "" + "," + "http://7xlsse.com2.z0.glb.qiniucdn.com/1442574781789a.main.m3u8";      // 标清   少年神探狄仁杰 1.片头(时间秒数) 2.片尾(时间秒数)  3.是否下一集  4.视频URL
    //return  "0"+","+"0"+","+"Y"+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442285938560a.main.m3u8";    // 标清   郑秀晶
    //return  "0"+","+"0"+","+"Y"+","+"http://7xliow.com2.z0.glb.qiniucdn.com/1441159565295a_192k.m3u8";    // 标清    UFO 
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442556294231.main.m3u8";      // 标清    神盾局特工第二季
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442289303332a.main.m3u8";       // 高清   澳门风云
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442301154128a.main.m3u8";      // 高清   父爱如山
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442569223654a.main.m3u8";      // 高清    传奇酒馆

}

// 暂停广告的图片
function pausePlayback() {
    //return 	"http://vupload.365sji.com:8082/images/chenjiezong/1434016313668.jpg" + "," + "http://www.baidu.com";
    return "http://192.168.33.219/videoplayer/ad.jpg" + "," + "http://www.baidu.com";
}

// 播完广告，送播币
function advertOver() {
    //alert("哈哈，看完广告了，可以赚钱啦 ^_^");
    followWindow();
}

//  开始播放视频
function playVideoStart() {
    //balert("播放视频了，计数开始");
    var uid = $("#uid").val();
//    var windowlocationStr = (window.location || document.location).toString();
//    var beginlocation = windowlocationStr;
//    if (windowlocationStr.indexOf('&') > 0) {
//      beginlocation = windowlocationStr.substring(0, windowlocationStr.indexOf('&'));
//}
//    alert(beginlocation + getURLParam("type", location.href));
    // 处理分享的事件
    var shareUserId = getURLParam("uid", location.href);
    var shareUserIp = getURLParam("ip", location.href);
    if (shareUserId != "")
    {
        //var surl = api + 'User/AddBBByShare';
        var surl = api + 'User/IncomeShare';
        var sdata = { videoId: vId, DemandUserId: uid, IpAddress: ipAddress, ShareUserId: shareUserId, ShareIpAddress: shareUserIp };

        $.ajax({
            type: "post",
            url: surl,
            data: sdata,
            dataType: "json",
            async: false,
            success: function (data) {
               
            },
            error: function (data) {

            }
        });
        //$.post(surl, sdata, function (data) {

        //});
    }

    
    //播放明细记录，无论是否登录，都有记录，详情请见api add个人观看记录
    //var url = api + 'VideoPlayRecord/AddVideoPlayRecord';
    //if (uid == 0 || uid == undefined) uid = 0;
    //var data = { userId: uid, videoId: vId, IpAddress: ipAddress };
    //$.post(url, data, function (data) {

    //});

    
    //个人观看记录，可在个人空间删除
    //if (uid > 0 && uid != undefined) {
    //    url = api + 'User/AddHistoryVideo?videoId=' + vId + '&userId=' + uid;
    //    $.ajax({
    //        type: "post",
    //        url: url,
    //        data: { IpAddress: ipAddress },
    //        dataType: "json",
    //        success: function (data) {

    //        },
    //        error: function (data) {

    //        }
    //    });
    //}
    //首次加载视频播放次数+1
    loadPlayRecord(uid);

    //如果有用户登陆，刷新或关闭浏览器时，记录当前观看的历史时间
    window.onbeforeunload = function() {
        addPalyRecord(uid);
    };
}

//首次加载视频播放次数+1
function loadPlayRecord(uid) {
    var url = api + 'VideoPlayRecord/AddVideoPlayRecord';
    if (uid == 0 || uid == undefined) { uid = 0; }
    var data = { userId: uid, videoId: vId, IpAddress: ipAddress, watchTime: watchTime };
    $.ajax({ type: "post", url: url, data: data, async: false, success: function (data) { } });
}

//记录该视频播放次数  如果有登陆用户则记录观看历史时间
function addPalyRecord(uid) {
    sendToFlash();
    var url = api + 'VideoPlayRecord/AddVideoPlayRecord';
    if (uid == 0 || uid == undefined) { uid = 0; }
    var data = { userId: uid, videoId: vId, IpAddress: ipAddress, watchTime: wTime };
    $.ajax({type: "post",url: url,data: data,async: false,success: function (data) {}});
}

//--------------获取播话器当前播放时间 start----------------------------
var wTime = 1;

function sendToFlash() {
    getSWF("divFlashPlayer").sendToFlash();
}

function vCurrentTime(value) {
    wTime = value;
}
//--------------获取播话器当前播放时间 end----------------------------

//打开安装flash插件页面
function adevertJumpUrl(url) {
    window.open(url);
}

//返回首页
function goToHomePage() {
    window.location.href = rootPath;
}