
var userId;
var videoData;
var albumsData;
var videoPageIndex = 1;
var albumsPageIndex = 1;
var videoPageCount = 0;
var albumsPageCount = 0;

//加载执行分页选择视频绑定
$(function () {
    userId = GetQueryString("browserUserId") == null ? "" : parseInt(GetQueryString("browserUserId"));
    //获取可选视频
    var videoUrl = api + "UserRoomChoose/GetUserVideoViews?userId=" + userId + "&v=" + Math.random();
    $.getJSON(videoUrl, function (data) {
        videoPageCount = data.Data.PageCount;
        videoData = data.Data.MyVideoViews;
        wmvideo.doVideosData(videoData);
        videoPageIndex++;
    }
    );

    //获取可选专辑
    var albumUrl = api + "UserRoomChoose/GetUserAlbumsViews?userId=" + userId + "&v=" + Math.random();
    $.getJSON(albumUrl, function (data) {
        albumsPageCount = data.Data.PageCount;
        albumsData = data.Data.SpecialVideoList;
        wmalbum.doAlbumsData(albumsData);
        albumsPageIndex++;
    }
    );
});

//设置选中的切换
function ckTolgge() {
    var ck = $(this).prev();
    if (ck.is(":checked"))
        ck.removeAttr("checked");
    else
        ck.attr("checked", "checked");
}

//点击查看更多-视频
function doLoadMoreVideo() {
    var url = api + "UserRoomChoose/GetUserVideoViews?userId=" + userId + "&pageIndex=" + videoPageIndex + "&v=" + Math.random();
    $.getJSON(url, function (data) {
        videoData = videoData.concat(data.Data.MyVideoViews);
        wmvideo.doVideosData(videoData);
        videoPageIndex++;
        if (videoPageIndex > videoPageCount) {
            $('#btnLoadMoreVideo').text(Translate("web_Content_Js_roomManage_doLoadMoreVideo_text"));
            $('#btnLoadMoreVideo').attr("href", "javascript:void(0);");
        }
    }
    );
}

//点击查看更多-专辑
function doLoadMoreAlbums() {
    var url = api + "UserRoomChoose/GetUserAlbumsViews?userId=" + userId + "&pageIndex=" + albumsPageIndex + "&v=" + Math.random();
    $.getJSON(url, function (data) {
        albumsData = albumsData.concat(data.Data.SpecialVideoList);
        wmalbum.doAlbumsData(albumsData);
        albumsPageIndex++;
        if (albumsPageIndex > albumsPageCount) {
            $('#btnLoadMoreAlbums').text(Translate("web_Content_Js_roomManage_doLoadMoreAlbums_text"));
            $('#btnLoadMoreAlbums').attr("href", "javascript:void(0);");
        }
    }
    );
}





//根据name获取被选中的checkbox，以‘,’隔开
function GetCheckedValue(name) {
    var values = '';
    $(":checkbox[name='" + name + "'][checked]").each(function () {
        values += this.value + ',';
    })
    values = values.substring(0, values.length - 1);
    return values;
}

//确认添加视频
function doAddVideo() {
    var videoIds = GetCheckedValue("chkVideo");
    if (videoIds == "")
        globalPromptBox.showGeneralMassage(1, Translate("web_Content_js_roomManage_doAddVideo_videoIds"), 2000, false);
    var videoId = videoIds.split(',');
    if (videoId.length > 12 - videoNum) {
        globalPromptBox.showGeneralMassage(1, Translate("web_Content_Js_roomManage_doAddVideo_message_videoNum").Format(12 - videoNum), 2000, false);
        return;
    }

    $.ajax({
        type: "Post",
        url: api + "UserRoomChoose/AddVideoToUserRoom",
        data: { userId: userId, videoIds: videoIds },
        dataType: "json",
        success: function (data) {
            globalPromptBox.showGeneralMassage(0, Translate("web_Content_Js_roomManage_doAddVideo_message_postsuccess"), 1000, true);
        },
        error: function (error) {
            globalPromptBox.showGeneralMassage(2, error.statusText, 3000, true);
        }
    });
}


//确认添加专辑
function doAddAlbums() {
    var albumIds = GetCheckedValue("chkAlbums");
    if (albumIds == "")
        globalPromptBox.showGeneralMassage(1, Translate("web_Content_js_roomManage_doAddAlbums_albumIds"), 2000, false);
    var albumId = albumIds.split(',');
    if (albumId.length > 3 - albumNum) {
        globalPromptBox.showGeneralMassage(1, Translate("web_Content_Js_roomManage_doAddAlbums_message_albumNum").Format(3 - albumNum), 2000, false);
        return;
    }

    $.ajax({
        type: "Post",
        url: api + "UserRoomChoose/AddAlbumToUserRoom",
        data: { userId: userId, albumIds: albumIds },
        dataType: "json",
        success: function (data) {
            globalPromptBox.showGeneralMassage(0, Translate("web_Content_Js_roomManage_doAddAlbums_message_postsuccess"), 1000, true);
        },
        error: function (error) {
            globalPromptBox.showGeneralMassage(2, error.statusText, 3000, true);
        }
    });
}


//视频
var wm_video = function () {
    var self = this;
    //视频集合
    self.videos = ko.observableArray();
    //数据赋值方法
    self.doVideosData = function (data) {
        self.videos(data);
    }
    //设置选中的切换
    self.ckTolgge = function (ele) {
        var ck = $(ele).prev();
        if (ck.is(":checked"))
            ck.removeAttr("checked");
        else
            ck.attr("checked", "checked");
    }
}

var wmvideo = new wm_video();
//绑定ko的数据
ko.applyBindings(wmvideo, document.getElementById('divVideoAddBOX'));


//专辑
var wm_Album = function () {
    var self = this;
    //专辑集合
    self.Albums = ko.observableArray();
    //数据赋值方法
    self.doAlbumsData = function (data) {
        self.Albums(data);
    }
    //设置选中的切换
    self.ckTolgge = function (ele) {
        var ck = $(ele).prev();
        if (ck.is(":checked"))
            ck.removeAttr("checked");
        else
            ck.attr("checked", "checked");
    }
}

var wmalbum = new wm_Album();
//绑定ko的数据
ko.applyBindings(wmalbum, document.getElementById('divAlbumsAddBOX'));
