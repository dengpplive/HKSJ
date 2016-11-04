//视频分页数据模型
function roomVideoList() {
    var self = this;
    //浏览的用户空间的Id
    self.browserUserId = ko.observable(browserUserId);
    //当前登录的用户Id
    self.loginUserId = ko.observable(-2);

    //专辑标题
    self.specialTitle = ko.observable('');
    self.specialId = ko.observable(0);

    self.pagerId = ko.observable('pager');
    //每页大小
    self.pageSize = ko.observable(30);
    //当前第几页
    //self.pagenumber = ko.observable(1);
    //查询到的视频总数
    self.totalcount = ko.observable(0);
    //视频数据列表
    self.list = ko.observableArray([]);
    //获取播放页面的url
    self.getPlayUrl = function (id) {
        return rootPath + "/Play/Index?videoId=" + id;
    }
    self.check = function (opCallback) {
        //检查当前有登陆的用户没有
        check(function (d) {
            //设置当前的登录用户的Id 
            self.loginUserId(d.UserId);
            var spId = parseInt($.trim(getURLParam("spId", location.href)).replace("#", ""), 10);
            if (spId <= 0 || isNaN(spId)) {
                globalPromptBox.showGeneralMassage(1, Translate("web_Content_Js_roomSonAlbum_roomVideoList_check_spId"), 2000, false);
                location.href = rootPath + "/Home/Index";
            } else {
                self.specialId(spId);
                //浏览的用户id
                browserUserId = getRoomBrowserUserId(self);
                self.browserUserId(browserUserId);
            }
            if (opCallback)
                opCallback(d);
        });
    }

    //初始化数据
    self.loadPage = function (pagenumber) {
        self.check(function (d) {
            $("#h_title").hide();
            addMask($("#vl"),1);
            //向服务器发送请求，查询满足条件的记录
            var url = api + '';
            var url = api + "UserSpecial/GetUserAlbumVideosById";
            //console.log("发送请求url：" + url);
            var reqData =
            {
                userId: self.browserUserId(),
                userSpecialId: self.specialId(),
                pagesize: self.pageSize(),
                pageindex: pagenumber,
                condtions: [],
                ordercondtions: []
            };
            //console.log("请求数据:");
            //console.log(reqData);
            $.post(url, reqData, function (data) {
                try {
                    //console.log("返回结果");
                    //console.log(data);

                    //data 为返回json 对象 并包括(pagecount、totalcount)的key-value值;
                    //设置绑定数据
                    self.specialTitle(data.Data.SpecialTitle);
                    self.list(data.Data.VideoViewList);
                    //总条数
                    self.totalcount(data.Data.VideoCount);
                    //设置分页
                    $("#" + self.pagerId()).pager({
                        pagenumber: pagenumber, pagecount: data.TotalIndex, totalcount: data.totalcount, buttonClickCallback: function (pageclickednumber) {
                            //console.log("返回结果：" + pageclickednumber);
                            //单击加载
                            self.loadPage(pageclickednumber);
                        }
                    });
                } finally {
                    removeMask($("#vl"));
                    $("#h_title").show();
                }
            }, "json");

        });
    }
}

var vm_SonAlbum = new roomVideoList();
vm_SonAlbum.loadPage(1);
ko.applyBindings(vm_SonAlbum, document.getElementById("vl"));
