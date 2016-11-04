//视频分页数据模型
function roomVideoList() {
    var self = this;
    //浏览的用户空间的Id
    self.browserUserId = ko.observable(browserUserId);
    //当前登录的用户Id
    self.loginUserId = ko.observable(-2);

    self.pagerId = ko.observable('pager');
    //每页大小
    self.pageSize = ko.observable(30);
    //当前第几页
    //self.pagenumber = ko.observable(1);
    //查询到的视频总数
    self.totalcount = ko.observable(0);
    //视频数据列表
    self.list = ko.observableArray([]);
    //排序字段名称
    self.orderByFiled = ko.observable('OrderCreateTime');//PlayCount
    //排序 升序还是降序
    self.IsDesc = ko.observable(true);
    self.setFiledOrder = function (el) {
        var cur = $(el);
        $("#secheader li a").removeClass("vid_total_curr").removeAttr("href");
        $("#secheader li a").removeAttr("href");
        cur.addClass("vid_total_curr");
        cur.attr("href", "#");
        self.orderByFiled($(el).attr("orderby"));
        self.loadPage(1);
    }
    //显示专辑数
    self.showAlbumTotalHtml = ko.computed(function () {
        var patString = Translate('web_Client_Views_UserRoom_Album_GeAlbum');//'共<strong>{0}</strong>个专辑';
        var total = patString.Format(self.totalcount());
        return total;
    }, self);
    //播放次数
    self.showPlayCountHtml = function (playCount) {
        var patString = Translate('web_Client_Views_UserRoom_Album_PlayNum');//'播放：{0}次';
        var total = patString.Format(playCount);
        return total;
    };
    self.showVideoCount = function (videoCount) {
        var patString = Translate('web_Client_Views_UserRoom_Album_GeVideoCount');//'{0}个';
        var total = patString.Format(videoCount);
        return total;
    }
    self.goSonAlbum = function (id) {
        return rootPath + "/UserRoom/SonAlbum?browserUserId=" + self.browserUserId() + "&spId=" + id + "&linkcss=2";
    }
    self.check = function (opCallback) {
        //检查当前有登陆的用户没有
        check(function (d) {
            //设置当前的登录用户的Id 
            self.loginUserId(d.UserId);
            //浏览的用户id
            browserUserId = getRoomBrowserUserId(self);
            self.browserUserId(browserUserId);
            if (opCallback)
                opCallback(d);
        });
    }

    //初始化数据
    self.loadPage = function (pagenumber) {
        self.check(function (d) {
            $("#vl").hide();
            addMask($("div[loading='main']"), 1);
            //向服务器发送请求，查询满足条件的记录
            var url = api + "UserSpecial/GetUserAlbumsViewsByOrder";
            // console.log("发送请求url：" + url);
            var reqData =
            {
                userId: self.browserUserId(),
                pagesize: self.pageSize(),
                pageindex: pagenumber,
                condtions: [],
                ordercondtions: [{
                    FiledName: self.orderByFiled(),
                    IsDesc: self.IsDesc()
                }]
            };
            //console.log("请求数据:");
            // console.log(reqData);
            $.post(url, reqData, function (data) {
                try {
                    // console.log("返回结果");
                    // console.log(data);
                    //data 为返回json 对象 并包括(pagecount、totalcount)的key-value值;
                    //设置绑定数据
                    self.list(data.Data.SpecialVideoList);
                    //设置当前页
                    //self.pagenumber(pagenumber);
                    //总条数
                    self.totalcount(data.TotalCount);
                    //设置分页
                    $("#" + self.pagerId()).pager({
                        pagenumber: pagenumber, pagecount: data.TotalIndex, totalcount: data.totalcount, buttonClickCallback: function (pageclickednumber) {
                            //console.log("返回结果：" + pageclickednumber);
                            //单击加载
                            self.loadPage(pageclickednumber);
                        }
                    });
                } finally {
                    //移除加載中
                    $("#vl").show();
                    removeMask($("div[loading='main']"));
                }
            }, "json");

        });
    }
}
var vm_Album = new roomVideoList();
vm_Album.loadPage(1);
ko.applyBindings(vm_Album, document.getElementById("vl"));
