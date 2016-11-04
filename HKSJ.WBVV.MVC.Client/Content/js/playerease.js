/*视频添加鼠标移入移出效果*/
/*
使用方法:在视频的图片添加
<div class="play_img" wth="170" hht="120">
<i></i>
视频图片
<div>
*/
(function () {
    var url = rootPath + "/Content/images/icon_img/play_img.png";
    var l = $(".play_img").length;
    // console.log("play_img length=" + l);
    $(".play_img").each(function () {
        var play_img = $(this);
        var width = play_img.attr("wth");
        var height = play_img.attr("hht");

        var mleft = play_img.attr("mleft"); if (mleft == null) mleft = 0;
        var mtop = play_img.attr("mtop"); if (mtop == null) mtop = 0;
        var mr = play_img.attr("mr"); if (mr == null) mr = 0;
        var mb = play_img.attr("mb"); if (mb == null) mb = 0;
        var mtype = play_img.attr("mtype"); if (mtype == null) mtype = 0;

        if (mtype == 0) {
            play_img.css({
                "position": "relative",
                "display": "block",
                "white-space": "normal",
                "width": width + "px",
                "height": height + "px",
                "margin-left": mleft + "px",
                "margin-top": mtop + "px"
            });
            play_img.find("i").css({
                "background-image": url,
                "height": height + "px",
                "width": width + "px",
                "margin-left": mleft + "px",
                "margin-top": mtop + "px"
            });
        }
        else {
            play_img.css({
                "position": "relative",
                "display": "block",
                "white-space": "normal",
                "width": width + "px",
                "height": height + "px",
                "margin-left": mleft + "px",
                "margin-top": mtop + "px",
                "right": mr + "px",
                "bottom": mb + "px"
            });
            play_img.find("i").css({
                "background-image": url,
                "height": height + "px",
                "width": width + "px",
                "right": mr + "px",
                "bottom": mb + "px"
            });
        }
    });
})();

