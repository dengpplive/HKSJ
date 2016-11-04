
var pageIndex = 1, pageSize = 24, totalPages = 1, pageType = "new", editAlbumsId = 0;


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
        if (data.SpecialCount <= 0) {
            $("#divNoneCount").show();
        } else {
            $("#divNoneCount").hide();
        }
        self.SpecialCount(data.SpecialCount);
        self.PageCount(data.PageCount);
        self.SpecialVideoList(data.SpecialVideoList);
        $("img[src='null']").attr("src", defaultData.defaultCoverImage);
    }
    //分页数据
    self.loadCollectsData = function (pageIndex) {
        $("#abCount").hide();
        $("#divNoneCount").hide();
        addMask($('div[loading="album"]'), 1);
        var url = api + 'UserSpecial/GetUserAlbumsViews?userId=' + userId + '&pageIndex=' + pageIndex + '&pageSize=' + pageSize + '&v=' + Math.random();
        $.getJSON(url, function (data) {
            try {
                self.doAssignment(data);
                totalPages = data.PageCount <= 0 ? 1 : data.PageCount;
                //分页
                $("#page").pager({
                    pagenumber: pageIndex,
                    pagecount: totalPages,
                    totalcount: data.TotalCount,
                    buttonClickCallback: function (pageclickednumber) {
                        //单击加载
                        self.loadCollectsData(pageclickednumber);
                    }
                });
            } finally {
                $("#abCount").show();
                removeMask($('div[loading="album"]'));
                if (data.TotalCount > 0) {
                    $("#divNoneCount").show();
                }
            }
        });
    };
    self.AlbumDetail = function (albumId) {
        window.location = "AlbumVideos?albumId=" + albumId;
    };
};

var wmalbums = new wm_albums();
wmalbums.loadCollectsData(pageIndex);
//绑定ko的数据
ko.applyBindings(wmalbums, document.getElementById('divUserAlbums'));

