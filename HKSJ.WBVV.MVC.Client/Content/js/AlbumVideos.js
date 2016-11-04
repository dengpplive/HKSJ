
var pageIndex = 1, pageSize = 40, totalPages = 1, albumId = 0, isHot = "Y";

$(function () {
    
    if (isHot == "Y") {
        $('#cbxHot').attr("checked", "checked");
        $('#cbxTime').removeAttr("checked");
    }
    else {
        $('#cbxTime').attr("checked", "checked");
        $('#cbxHot').removeAttr("checked");
    }
});

var wm_videos = function () {
    var self = this;
    //专辑名
    self.Title = ko.observable();
    //专辑简介
    self.Remark = ko.observable();
    //专辑下视频数
    self.VideoCount = ko.observable();
    //总页数
    self.PageCount = ko.observable();
    //专辑下视频集合
    self.SpecialVideoList = ko.observableArray();


    //数据赋值方法
    self.doAssignment = function (data) {
        self.Title(data.Title);
        self.Remark(data.Remark);
        self.PageCount(data.PageCount);
        self.VideoCount(data.VideoCount);
        self.SpecialVideoList(data.SpecialVideoList);
    }
    //分页数据
    self.loadCollectsData = function (pageIndex) {
        albumId = GetQueryString("albumId");
        isHot = (GetQueryString("isHot") == "" || GetQueryString("isHot") == null) ? "Y" : GetQueryString("isHot");
        var url = api + 'UserSpecial/GetAlbumVideoViews?albumId=' + albumId + '&pageIndex=' + pageIndex + '&pageSize=' + pageSize + '&isHot=' + isHot + '&v=' + Math.random();
        $.getJSON(url, function (data) {
            self.doAssignment(data);
            totalPages = data.PageCount <= 0 ? 1 : data.PageCount;
            $("#page").pager({
                pagenumber: pageIndex, pagecount: totalPages, totalcount: data.PageCount, buttonClickCallback: function (pageclickednumber) {
                    //单击加载
                    self.loadCollectsData(pageclickednumber);
                }
            });
        });
    }
    self.doCheck = function (isHot) {
        var curId = GetQueryString("curId");
        var albumId = GetQueryString("albumId");
        window.location = "AlbumVideos?curId=" + curId + "&albumId=" + albumId + "&isHot=" + isHot
    }


    self.goPlay = function (videoId) {
        window.open(rootPath + "/Play?videoId=" + videoId, "_blank");
    }
}

var wmvideos = new wm_videos();
wmvideos.loadCollectsData(pageIndex);
//绑定ko的数据
ko.applyBindings(wmvideos, document.getElementById('divAlbumVideos'));

