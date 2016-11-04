
var ConvertPlayTimeUtil = {
    //显示 时:分:秒
    ConvertPlayTime: function (second) {
        if (second >= 3600)
            return new Date(second * 1000 - 8 * 60 * 60 * 1000).format("hh:mm:ss");
        else
            return new Date(second * 1000 - 8 * 60 * 60 * 1000).format("mm:ss");
    }

}