//我的粉丝列表和我的订阅
function roomFansList(pagerContainer, pagesize) {
    var self = this;
    //浏览的用户空间的Id
    self.browserUserId = ko.observable(browserUserId);
    //当前登录的用户Id
    self.loginUserId = ko.observable(-2);
    //是否可以显示编辑
    self.isShowEdit = ko.observable(false);
    //当前浏览的空间是否为当前登录人的用户空间
    self.isMyRoom = ko.observable(false);
    //当前的选择项 即选项卡
    self.currSelected = ko.observable('');
    //每页大小
    self.pageSize = ko.observable(pagesize);
    //当前第几页
    self.pagenumber = ko.observable(1);
    //查询到的视频总数
    self.totalcount = ko.observable(0);
    //是否显示分页条
    self.isShowPagerToolbar = ko.observable(false);
    //数据列表 
    self.list = ko.observableArray([]);
    //容器id
    self.pagerId = ko.observable(pagerContainer);
    //默认用户图像
    self.defaultUserImage = ko.observable(defaultData.defaultUserImage);
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
    //显示登录框
    self.showLogin = function () {
        LR.LoginShow();
    }
    //加载错误处理
    self.loadError = function () {
        //用户图像无法显示时 显示默认的图像
        $("img[userimg]").error(function () {
            $(this).attr("src", self.defaultUserImage());
        });
    }
    //检测用户
    self.check = function (opCallback) {
        //检查当前有登陆的用户没有
        check(function (d) {
            //设置当前的登录用户的Id 
            self.loginUserId(d.UserId);
            //浏览的用户id
            browserUserId = getRoomBrowserUserId(self);
            self.browserUserId(browserUserId);
            if (self.browserUserId() == self.loginUserId()) {
                self.isMyRoom(true);
            }
            else {
                self.isMyRoom(false);
            }
            // console.log("登录id:" + d.UserId + " 浏览id:" + self.browserUserId() + " isMyRoom:" + self.isMyRoom());
            if (opCallback)
                opCallback(d);
        });
    }

    //初始化数据
    self.loadPage = function (pagenumber) {
        self.check(function (d) {
            var reqData =
            {
                userId: self.browserUserId(),
                loginUserId: d.UserId,
                pagesize: self.pageSize(),
                pageindex: pagenumber,
                condtions: [],
                ordercondtions: []
            };
            //设置当前页
            self.pagenumber(pagenumber);
            var url = api + "UserFans/GetUserFunsList";
            if (self.pagerId() == "pager1") {
                url = api + "UserFans/GetUserSubscribeList";
                $("div[loading='main2']").show();
                addMask($("div[loading='main2']"), 1);
            } else {
                $("div[loading='main1']").show();
                addMask($("div[loading='main1']"), 1);
            }

            self.list.removeAll();
            self.isShowPagerToolbar(false);
            //console.log("url:" + url);
            //console.log("请求数据:");
            //console.log(reqData);

            $.post(url, reqData, function (data) {
                try {
                    //console.log(url + "返回结果");
                    //console.log(data);
                    var list = data.Data;
                    for (var i in list) {
                        for (var key in list[i]) {
                            //处理状态
                            if (key == "State" || key == "LoginSubState") {
                                list[i][key] = ko.observable(list[i][key]);
                            }
                        }
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
                            self.loadPage(pageclickednumber);
                        }
                    });
                } finally {
                    //用户图片无效时显示默认图片
                    self.loadError();
                    if (self.pagerId() == "pager1") {
                        removeMask($("div[loading='main2']"));
                        $("div[loading='main2']").hide();
                    } else {
                        removeMask($("div[loading='main1']"));
                        $("div[loading='main1']").hide();
                    }
                }
            }, "json");
        });
    }
    //跳转到播放页面
    self.goPlayer = function (Id) {
        return rootPath + "/Play/Index?videoId=" + Id + "&v=" + Math.random();
    }
    //跳转到用户
    self.more = function (uid) {
        return rootPath + "/UserRoom/Index?browserUserId=" + uid + "&v=" + Math.random();
    }
    //跳转到留言界面
    self.gotoMessage = function (uid) {
        location.href = rootPath + "/UserRoom/Message?browserUserId=" + uid + "&linkcss=4";
    }
    //是否为当前用户
    self.isCurrUser = function (uid) {
        return self.loginUserId() == uid;
    }
    //确认框
    self.confirmSub = function (type, kData, flag) {
        globalPromptBox.showPromptMessage(Translate("web_Content_Js_roomFans_confirmSum_message_title"), Translate("web_Content_Js_roomFans_confirmSum_message_content"), function () {
            self.subUser(type, kData, flag);
        })
    }
    //我的粉丝中的订阅按钮事件type 0浏览自己的空间 1浏览其他人的空间
    self.subUser = function (type, fan, flag) {
        self.check(function (res) {
            if (res.Success) {
                //没有登录弹出登录框
                if (res.UserId <= 0) {
                    self.showLogin(); return;
                };
                var careState = (flag == 1 ? true : fan.State());
                if (type == 1)
                    careState = (flag == 1 ? true : fan.LoginSubState());

                var d = {
                    createUserId: res.UserId,
                    subscribeUserId: fan.UserView.Id,
                    careState: careState
                };
                var url = api + "UserFans/SaveSubscribe";
                // console.log("订阅和取消订阅url:" + url);
                // console.log("订阅和取消订阅请求数据:");
                // console.log(d);
                $.post(url, d, function (data) {
                    // console.log("订阅和取消订阅结果:");
                    // console.log(data);
                    if (data.Success) {
                        if (type == 0) {
                            //更改状态数据
                            if (flag == 1) {
                                fan.State(true);
                            } else {
                                fan.State(!fan.State());
                            }
                        } else {
                            //更改状态数据
                            if (flag == 1) {
                                fan.LoginSubState(true);
                            } else {
                                fan.LoginSubState(!fan.LoginSubState());
                            }
                        }

                        vm_Subscribe.loadPage(1);
                        vm_FS.loadPage(1);
                        //给出提示
                        var msg = !d.careState ? Translate("web_Content_Js_roomFans_subUser_postiftrue") : Translate("web_Content_Js_roomFans_subUser_postiffalse");
                        globalPromptBox.showGeneralMassage(0, msg, 1000, false);
                    } else {
                        var msg = d.careState ? Translate("web_Content_Js_roomFans_subUser_postelsetrue") : Translate("web_Content_Js_roomFans_subUser_postelsefalse");
                        globalPromptBox.showGeneralMassage(2, msg, 1000, false);
                    }
                }, "json");
            } else {
                //没有登录弹出登录框
                self.showLogin();
            }
        });
    }
}

var vm_FS = new roomFansList('pager', 8);
vm_FS.loadPage(1);
ko.applyBindings(vm_FS, document.getElementById("con_news_1"));

var vm_Subscribe = new roomFansList('pager1', 6);
ko.applyBindings(vm_Subscribe, document.getElementById("con_news_2"));

var isInit = true;
$("#menuTab a[itemid]").click(function (e) {
    var itemid = $(this).attr("itemid");
    //设置和显示
    $(this).attr("class", "Per_fans_nav");
    $("div[id=con_news_" + itemid + "]").show();

    //清空样式和隐藏
    $("div[id^=con_news_]").not("[id='con_news_" + itemid + "']").hide();
    $("#menuTab a[itemid]").not("[itemid='" + itemid + "']").removeAttr("class");

    if (isInit) {
        isInit = false;
        vm_Subscribe.loadPage(1);
    }
    return false;
});