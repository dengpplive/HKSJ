
var pageIndex = 1, recommendPageIndex = 1, recommendPageSize = 3, hotPageSize = 20, totalPages = 1, albumsData = [];


$(function () {

    var urlRecommend = api + "UserSpecial/GetRecommendAlbumsViews?pageIndex=" + recommendPageIndex + "&pageSize=" + recommendPageSize + "&v=" + Math.random();
    $.getJSON(urlRecommend, function (data) {
        wmRecommendAlbums.doAssignment(data);
    });


});





var wm_albums = function () {
    var self = this;
    //专辑总数
    self.SpecialCount = ko.observable();
    //总页数
    self.PageCount = ko.observable();
    //专辑集合
    self.SpecialVideoList = ko.observableArray();


    //数据赋值方法
    self.doAssignment = function (data) {
        self.SpecialCount(data.SpecialCount);
        self.PageCount(data.PageCount);
        self.SpecialVideoList(data.SpecialVideoList);
    }
    self.doListData = function (data) {
        self.SpecialVideoList(data);
    }

    self.GoAlbumDetail = function (albumId) {
        var curId = GetQueryString("curId");
        return rootPath + "/Albums/AlbumVideos?curId=" + curId + "&albumId=" + albumId;
    }
    self.GoPlay = function (videoId) {
        return rootPath + "/Play?videoId=" + videoId;
    }
    //分页数据
    self.loadCollectsData = function () {
        var url = api + "UserSpecial/GetAllAlbumsViews?pageIndex=" + pageIndex + "&pageSize=" + hotPageSize + "&v=" + Math.random();
        $.getJSON(url, function (data) {
            if (data.SpecialVideoList != null)
                albumsData = albumsData.concat(data.SpecialVideoList);
            self.doListData(albumsData);
            //totalPages = data.PageCount;
            if (data.PageCount == 0)
                window.onscroll = function () { }
            else
                pageIndex++;
            //分页
            setBottomToolsPos();
        });
    }

}


var wmRecommendAlbums = new wm_albums();
//绑定ko的数据
ko.applyBindings(wmRecommendAlbums, document.getElementById('divRecommendAlbums'));

var wmHotAlbums = new wm_albums();
wmHotAlbums.loadCollectsData();
//绑定ko的数据
ko.applyBindings(wmHotAlbums, document.getElementById('divHotAlbums'));


var page = {
    //createAlbums: function () {
    //    window.open(rootPath + "/UserCenter/UserAlbums", "_blank");
    //}

}

var clientHeight = document.documentElement.clientHeight;
window.onscroll = function () {
    var pageHeight = document.documentElement.scrollHeight;
    var scrTop = document.documentElement.scrollTop || document.body.scrollTop;
    if (scrTop + clientHeight >= pageHeight) {
        wmHotAlbums.loadCollectsData();
    }
}



























