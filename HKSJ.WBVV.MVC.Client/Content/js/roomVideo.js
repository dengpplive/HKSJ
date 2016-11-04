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
    self.pagenumber = ko.observable(1);
    //查询到的视频总数
    self.totalcount = ko.observable(0);
    //视频数据列表
    self.list = ko.observableArray([]);
    //排序字段名称
    self.orderByFiled = ko.observable('UpdateTime');//PlayCount
    //排序 升序还是降序
    self.IsDesc = ko.observable(true);
    self.setFiledOrder = function (ele) {
        var cur = $(ele);
        $("#secheader li a").removeClass("vid_total_curr").removeAttr("href");
        $("#secheader li a").removeAttr("href");
        cur.addClass("vid_total_curr");
        cur.attr("href", "#");
        self.orderByFiled(cur.attr("orderby"));
        self.IsDesc(cur.attr("isdesc"));
        self.loadPage(1);
    }
    //总共有视频数
    self.showTotalCount = function () {
        var patString = Translate('web_Client_Views_UserRoom_Video_GeVideo');// '共<strong>{0}</strong>个视频';
        return patString.Format(self.totalcount());
    }
    //获取播放页面的url
    self.getPlayUrl = function (id) {
        return rootPath + "/Play/Index?videoId=" + id;
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
            //添加加載中
            $("#vl").hide();
            addMask($('div[loading="main"]'), 1);
            //向服务器发送请求，查询满足条件的记录
            //var url = api + '';        
            var url = api + "UserRoomChoose/GetUserRoomVideoList";
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
            //console.log("发送请求url：" + url);
            //console.log("发送请求数据：");
            //console.log(reqData);
            $.post(url, reqData, function (data) {
                try {
                    //  console.log("返回结果");
                    //  console.log(data);

                    //data 为返回json 对象 并包括(pagecount、totalcount)的key-value值;
                    //设置绑定数据

                    self.list(data.Data);
                    //设置当前页
                    self.pagenumber(pagenumber);
                    //总条数
                    self.totalcount(data.TotalCount);
                    //设置分页
                    $("#" + self.pagerId()).pager({
                        pagenumber: pagenumber, pagecount: data.TotalIndex, totalcount: data.totalcount, buttonClickCallback: function (pageclickednumber) {
                            // console.log("返回结果1");
                            //单击加载
                            self.loadPage(pageclickednumber);
                        }
                    });
                } finally {
                    $("#vl").show();
                    //移除加載中
                    removeMask($('div[loading="main"]'));
                }
            }, "json");

        });
    }
}

var vm_Vedio = new roomVideoList();
vm_Vedio.loadPage(1);
ko.applyBindings(vm_Vedio, document.getElementById("vl"));
