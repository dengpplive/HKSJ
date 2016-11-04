//我的粉丝列表和我的订阅
function subList(pagerContainer, pagesize) {
    var self = this;
    //当前登录的用户Id
    self.loginUserId = ko.observable(-2);
    //是否可以显示编辑
    self.isShowEdit = ko.observable(false);
    //当前的选择项 即选项卡
    self.currSelected = ko.observable('');
    //每页大小
    self.pageSize = ko.observable(pagesize);
    //当前第几页
    self.pagenumber = ko.observable(1);
    //查询到的视频总数
    self.totalcount = ko.observable(0);
    //数据列表 
    self.list = ko.observableArray([]);
    //容器id
    self.pagerId = ko.observable(pagerContainer);
    //是否显示分页条
    self.isShowPagerToolbar = ko.observable(false);
    //false未加载或者加载完成  true正在加载
    self.isLoading = ko.observable(false);
    //显示登录框
    self.showLogin = function () {
        LR.LoginShow();
    }
    //默认用户图像
    self.defaultUserImage = ko.observable(defaultData.defaultSubUserImage);
    //处理用户的图像
    self.handUserPicture = function (pic) {
        if (pic == null || pic == '')
            pic = self.defaultUserImage();
        else if (pic.indexOf("http:") > -1) {
            return pic;
        } else {
            return rootPath + pic;
        }
        return pic;
    }
    //检测用户
    self.check = function (opCallback) {
        //检查当前有登陆的用户没有
        check(function (d) {
            if (d.Success) {
                //设置当前的登录用户的Id 
                self.loginUserId(d.UserId);
                if (opCallback)
                    opCallback(d);
            } else {
                self.showLogin();
            }
        });
    }
    //跳转到用户
    self.gotoRoom = function (uid) {
        return rootPath + "/UserRoom/Index?browserUserId=" + uid + "&v=" + Math.random();
    }
    //显示 时和分
    self.showPlayTime = function (second) {
        return new Date(second * 1000 - 8 * 60 * 60 * 1000).format("hh:mm");
    }
    //界面显示
    self.tipMsg = ko.computed(function () {
        var msg = "";
        if (self.isLoading()) {
            msg = "";
        }
        else
            if (self.list().length == 0) {
                msg = "未查到数据";
            }
        return msg;
    }, self);
    //初始化数据
    self.loadPage = function (pagenumber, ckpage) {
        self.check(function (d) {
            var reqData =
            {
                userId: self.loginUserId(),
                loginUserId: d.UserId,
                pagesize: self.pageSize(),
                pageindex: pagenumber,
                condtions: [],
                ordercondtions: []
            };
            self.isLoading(true);
            self.list.removeAll();
            if (ckpage) {
                $("#" + self.pagerId()).hide();
            }

            //设置当前页
            self.pagenumber(pagenumber);
            var url = '';
            if (self.pagerId() == "pager1") {
                url = api + "UserFans/GetUserSubscribeVideoList";//视频  
                addMask($("div[loading='main1']"), 1);
            }
            else if (self.pagerId() == "pager2") {
                url = api + "UserFans/GetUserSubscribeUserList";//用户
                addMask($("div[loading='main2']"), 1);
            }
            // console.log("url:" + url);
            $.post(url, reqData, function (data) {
                var list = [];
                try {
                    // console.log("返回结果");
                    // console.log(data);
                    list = data.Data;
                    for (var i in list) {
                        if (self.pagerId() == "pager1")
                            list[i].PlayTime = list[i].TimeLength == null ? "00:00" : self.showPlayTime(list[i].TimeLength);
                    }
                    //设置绑定数据
                    self.list(list);
                    //总条数
                    self.totalcount(data.TotalCount);
                    //是否显示分页条
                    if (self.totalcount() > self.pageSize()) self.isShowPagerToolbar(true); else self.isShowPagerToolbar(false);
                    //设置分页
                    $("#" + self.pagerId()).pager({
                        pagenumber: pagenumber, pagecount: data.TotalIndex, totalcount: data.TotalCount, buttonClickCallback: function (pageclickednumber) {
                            //单击加载
                            self.loadPage(pageclickednumber, "1");
                        }
                    });
                } finally {
                    if (self.pagerId() == "pager1") {
                        removeMask($("div[loading='main1']"));
                    }
                    else if (self.pagerId() == "pager2") {
                        removeMask($("div[loading='main2']"));
                    }
                    if (ckpage && self.isShowPagerToolbar()) {
                        $("#" + self.pagerId()).show();
                    }
                    self.isLoading(false);
                }
            }, "json");
        });
    }
    //跳转到播放页面   
    self.goPlayer = function (Id, anchor) {
        var url = rootPath + "/Play/Index?videoId=" + Id + "&v=" + Math.random();
        if (anchor)
            url = url + "#" + anchor;
        return url;
    }
    //确认
    self.confirmSub = function (kData) {
        globalPromptBox.showPromptMessage(Translate("web_Content_Js_userFans_confirmSub_message_title"), Translate("web_Content_Js_userFans_confirmSub_message_content"), function () {
            self.subUser(kData);
        })
    }

    //我的粉丝中的订阅按钮事件
    self.subUser = function (kData) {
        self.check(function (res) {
            if (res.Success) {
                var d = {
                    createUserId: res.UserId,
                    subscribeUserId: kData.Id,
                    careState: true
                };
                var url = api + "UserFans/SaveSubscribe";
                // console.log("订阅url:" + url);
                //console.log("订阅请求数据:");
                //console.log(d);
                $.post(url, d, function (data) {
                    //  console.log("订阅和取消订阅结果:");
                    // console.log(data);
                    if (data.Success) {
                        // self.loadPage(self.pagenumber());
                        vm_dyvideo.loadPage(1);
                        vm_dyuser.loadPage(1);
                        globalPromptBox.showGeneralMassage(0, Translate("web_Content_Js_userFans_subUser_postif_message"), 1000, false);
                    } else {
                        globalPromptBox.showGeneralMassage(2, Translate("web_Content_Js_userFans_subUser_postelse_message"), 1000, false);
                    }
                });
            }
        });
    }
}

var vm_dyvideo = new subList('pager1', 8);
vm_dyvideo.loadPage(1);
ko.applyBindings(vm_dyvideo, document.getElementById("showSubDiv1"));


var vm_dyuser = new subList('pager2', 10);
ko.applyBindings(vm_dyuser, document.getElementById("showSubDiv2"));

//我的订阅 切换
$("#menuTab a[itemid]").click(function (e) {
    var itemid = $(this).attr("itemid");
    if (itemid == "1" && !vm_dyvideo.isLoading() && !vm_dyuser.isLoading()) {
        //设置和显示
        $(this).attr("class", "v_r_ti_cur");
        $("#subContainer div[id=showSubDiv" + itemid + "]").show();

        //清空样式和隐藏
        $("#subContainer div[id^=showSubDiv]").not("[id='showSubDiv" + itemid + "']").hide();
        $("#menuTab a[itemid]").not("[itemid='" + itemid + "']").removeAttr("class");

        $("div[id^=pager]").hide();
        vm_dyvideo.loadPage(1, "1");
    }
    else if (itemid == "2" && !vm_dyvideo.isLoading() && !vm_dyuser.isLoading()) {
        //设置和显示
        $(this).attr("class", "v_r_ti_cur");
        $("#subContainer div[id=showSubDiv" + itemid + "]").show();

        //清空样式和隐藏
        $("#subContainer div[id^=showSubDiv]").not("[id='showSubDiv" + itemid + "']").hide();
        $("#menuTab a[itemid]").not("[itemid='" + itemid + "']").removeAttr("class");

        $("div[id^=pager]").hide();
        vm_dyuser.loadPage(1, "1");
    }
    return false;
});