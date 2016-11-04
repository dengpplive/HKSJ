
var pageIndex = 1, pageSize = 8, totalPages = 1;

var wm_videos = function () {
    var self = this;
    self.albumId = 0;
    //专辑名
    self.Title = ko.observable();
    //专辑封面
    self.Thumbnail = ko.observable();
    //专辑简介
    self.Remark = ko.observable();
    //专辑下视频数
    self.VideoCount = ko.observable();
    //专辑创建时间
    self.CreateTime = ko.observable();
    //播放总数
    self.PlayCount = ko.observable();
    //评论总数
    self.CommentCount = ko.observable();
    //总页数
    self.PageCount = ko.observable();
    //专辑下视频集合
    self.SpecialVideoList = ko.observableArray();


    //数据赋值方法
    self.doAssignment = function (data) {
        self.Title(data.Title);
        self.Remark(data.Remark);
        self.CreateTime(data.CreateTime);
        self.PageCount(data.PageCount);
        self.VideoCount(data.VideoCount);
        self.SpecialVideoList(data.SpecialVideoList);

        self.Thumbnail(data.Thumbnail == '' ? defaultData.defaultCoverImage : data.Thumbnail);
        self.PlayCount(data.PlayCount);
        self.CommentCount(data.CommentCount);
        if (data.SpecialVideoList.length > 0) {
            $("#divNoneCount").hide();
        } else {
            $("#divNoneCount").show();
        }
    };
    //分页数据
    self.loadCollectsData = function (index) {
        pageIndex = index;
        self.albumId = GetQueryString("albumId");
        var url = api + 'UserSpecial/GetUserAlbumVideoViews?userId=' + userId + '&albumId=' + self.albumId + '&pageIndex=' + pageIndex + '&pageSize=' + pageSize + '&v=' + Math.random();
        $.getJSON(url, function (data) {
            self.doAssignment(data);
            totalPages = data.PageCount <= 0 ? 1 : data.PageCount;
            pageIndex = pageIndex > totalPages ? totalPages : pageIndex;
            $("#page").pager({
                pagenumber: pageIndex,
                pagecount: totalPages,
                totalcount: data.PageCount,
                buttonClickCallback: function (pageclickednumber) {
                    //单击加载
                    self.loadCollectsData(pageclickednumber);
                }
            });
        });
    },
    self.doDeleteAlbum = function () {
        globalPromptBox.showPromptMessage('提示', '您确定删除专辑吗？', function () {
            var url = api + "UserSpecial/DeleteUserAlbum";
            var data1 = { userId: userId, albumId: self.albumId };
            $.post(url, data1, function (data) {
                if (data.Success) {
                    //globalPromptBox.showGeneralMassage(0, "删除视频成功", 1000, false);
                    window.location = "UserAlbums";
                    //UserCenter/UserAlbums
                } else {
                    //globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
                }
            });
        });
    },
    self.editAlbum = function () {
        window.location = "EditAlbum?albumId=" + self.albumId;
    },
    self.doDelete = function (videoId) {
        globalPromptBox.showPromptMessage('提示', '您确定将视频移除该专辑吗？', function () {

            var url = api + "UserSpecial/DeleteAlbumVideos";
            var data = { userId: userId, albumId: self.albumId, videoIds: videoId };
            $.post(url, data, function (data) {
                if (data.Success) {
                    //globalPromptBox.showGeneralMassage(0, "删除视频成功", 1000, false);
                    self.loadCollectsData(pageIndex);
                } else
                    globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
            });

        });
    };
    self.goPlay = function (videoId) {
        window.open(rootPath + "/Play?videoId=" + videoId, "_blank");
    };
}



var wmvideos = new wm_videos();
wmvideos.loadCollectsData(pageIndex);
//绑定ko的数据
ko.applyBindings(wmvideos, document.getElementById('divAlbumVideo'));

