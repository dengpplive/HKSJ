browserUserId = -1;//当前浏览的个人空间的用户Id
function roomHeader() {
    var self = this;
    //浏览的用户空间的Id
    self.browserUserId = ko.observable(browserUserId);
    //当前登录的用户Id
    self.loginUserId = ko.observable(-2);
    //个性签名
    self.Bardian = ko.observable('');
    //个人空间的背景图片
    self.backgroundImage = ko.observable();
    //个人空间的背景图片 副本
    self.backgroundImageBak = ko.observable();
    //浏览的用户空间信息
    self.userRoomInfo = ko.observableArray([]);
    //浏览的空间是否为登录的用户空间 默认隐藏
    self.isMyRoom = ko.observable(false);
    //是否正在编辑 否则编辑结束
    self.isEditIng = ko.observable(false);
    //是否进入编辑管理页面
    self.isEditManage = ko.observable(false);
    //是否被订阅 true已订阅 false未订阅
    self.IsSubed = ko.observable(false);

    //默认用户图像
    self.defaultUserImage = ko.observable(defaultData.defaultUserImage);
    //默认的背景图片
    self.defaultBannerImage = ko.observable(defaultData.defaultBannerImage);
    //获取“浏览我的空间”和空间设置链接
    self.getHref = function (flag) {
        var url = rootPath + "/UserRoom/Index?browserUserId=" + self.loginUserId();
        if (flag == 1)
            url = rootPath + "/UserRoom/RoomManage?browserUserId=" + self.loginUserId() + "&manage=1&edit=1";
        return url;
    }
    //显示登录框
    self.showLogin = function () {
        LR.LoginShow();
    }
    self.gotoIndex = function () {
        location.href = rootPath + "/Home/Index";
    }
    var imgArr = [rootPath + "/Content/images/icon_img/Per_sx.png", rootPath + "/Content/images/icon_img/Per_sx_Save.png"];
    //显示编辑呢称
    self.getBardian = function (kData) {
        if (!self.isEditIng()) {
            //正在编辑
            self.isEditIng(true);
            if (self.isEditManage()) {
                $("#editimg").attr("src", imgArr[1]);
            }
        } else {
            //编辑结束           
            self.saveBardian(kData);
        }
    }
    //保存个性签名
    self.saveBardian = function (kData) {
        // alert(11);
        var lid = self.loginUserId();
        if (lid > 0) {

            var newBardian = $("#bd").val();
            var url = api + 'User/UpdateUserBardian';
            var d = {
                userId: lid,
                bardian: newBardian
            };
            //console.log("个性签名url:" + url);
            $.post(url, d, function (data) {
                //console.log("保存个性签名返回数据:");
                // console.log(data);
                var msg = "";
                if (data.Success) {
                    msg = "保存成功";
                } else {
                    //$("#bd").val(self.Bardian());                    
                    msg = "输入文字最多30个," + data.ExceptionMessage
                }
                //self.alert(msg, function () { });
                globalPromptBox.showGeneralMassage(0, msg, 1000, false);
                if (data.Success) {

                    self.Bardian(newBardian);
                    $("#editimg").attr("src", imgArr[0]);
                    self.isEditIng(false);

                    //self.loadUserRoom(function () {

                    //});
                }

            }, "json");
        }
    }
    //加载浏览的用户空间数据
    self.loadUserRoom = function (fn) {
        addMask($("div[loadMask='header']"), 1);
        $("div[loadMask='header'] .blockElement").css("background-color", "");
        $("div[loadMask='header'] .blockElement span").css("color", "white");
        var url = rootPath + '/UserRoom/GetUserRoomInfo?browserUserId=' + self.browserUserId() + "&loginUserId=" + self.loginUserId() + "&v=" + Math.random();
        // console.log("该用户空间url:" + url);
        $.getJSON(url, function (data) {
            try {
                //  console.log("空间数据");
                // console.log(data);
                if (data == null || !data.Success) {
                    globalPromptBox.showGeneralMassage(1, Translate("web_Content_Js_roomFans_roomHeader_loadUserRoom_message_data"), 2000, false);
                    self.gotoIndex(); return;
                }
                else {
                    data = data.Data;
                }
                //默认背景大图片
                if (data.BannerImage == null || data.BannerImage == "")
                    data.BannerImage = self.defaultBannerImage();

                //默认头像
                if (data.Picture == null || data.Picture == "")
                    data.Picture = self.defaultUserImage();

                //设置背景图片
                self.backgroundImage(data.BannerImage);
                //临时保存
                self.backgroundImageBak(data.BannerImage);
                //个性签名
                self.Bardian(data.Bardian);
                //订阅状态
                self.IsSubed(data.IsSubed);
                //浏览用户的皮肤id
                self.SkinId(data.SkinId);
                //设置标题
                document.title = data.NickName + Translate("web_Content_Js_roomFans_roomHeader_loadUserRoom_message_title") + companyTitle;

                //加入集合
                self.userRoomInfo.push(data);

                var manage = $.trim(getURLParam("manage", location.href)).replace("#", "");
                var edit = $.trim(getURLParam("edit", location.href)).replace("#", "");
                var _isEdit = (manage == "1" && edit == "1") ? true : false;
                var _isMyRoom = (self.loginUserId() != data.Id ? false : true);

                //是否编辑
                self.isEditManage(_isEdit)
                //是否是我的空间
                self.isMyRoom(_isMyRoom);


                //加载皮肤
                self.initSkinData((_isMyRoom && _isEdit));

            } finally {
                if (fn) fn();
                removeMask($("div[loadMask='header']"));
            }
        });
    }
    self.SubscribeUser = function () {
        check(function (res) {
            //没有登录弹出登录框
            if (res.UserId <= 0) { self.showLogin(); return; };
            var d = {
                createUserId: res.UserId,
                subscribeUserId: self.browserUserId(),
                careState: self.IsSubed()
            };
            var url = api + "UserFans/SaveSubscribe";
            //console.log("订阅和取消订阅url:" + url);
            //console.log("订阅和取消订阅请求数据:");
            //console.log(d);
            $.post(url, d, function (data) {
                //console.log("订阅和取消订阅结果:");
                //console.log(data);
                if (data.Success) {
                    /*
                    //更改状态数据                      
                    self.IsSubed(!self.IsSubed());
                    try {
                        if (vm_Index != null)
                            vm_Index.loadForYouRecomment();
                    } catch (e) {
                    } finally { 
                        self.loadUserRoom();
                    }*/
                    location.reload(true);
                }
                if (data.Success) {
                    var msg = !d.careState ? Translate("web_Content_Js_roomHeader_SubscribeUser_postiftrue") : Translate("web_Content_Js_roomHeader_SubscribeUser_postiffalse");
                    globalPromptBox.showGeneralMassage(0, msg, 1000, false);
                } else {
                    var msg = !d.careState ? Translate("web_Content_Js_roomHeader_SubscribeUser_postelsetrue") : Translate("web_Content_Js_roomHeader_SubscribeUser_postelsefalse");
                    globalPromptBox.showGeneralMassage(1, msg, 1000, false);
                }
            }, "json");

        });
    }

    //设置用户的等级显示
    self.showLevel = function () {

    }
    //提示    
    self.showMsg = ko.computed(function () {
        var strBardian = self.Bardian();
        var remsg = '0/30';
        if (strBardian != null) {
            if (strBardian.length > 30)
                remsg = Translate('web_Content_Js_roomHeader_showMsg_remsg');
            else if (strBardian.length == 30)
                remsg = '';
            else
                remsg = strBardian.length + '/30';
        }
        if (!self.isEditIng()) { remsg = ''; }
        return remsg;
    }, self);
    //用于显示
    self.showBardian = ko.computed(function () {
        var strBardian = self.Bardian();
        if (strBardian != null) {
            if (strBardian.length >= 30)
                strBardian = strBardian.substr(0, 30);
        }
        self.Bardian(strBardian);

        return strBardian;
    }, self);
    //添加和更新访问者记录
    self.addUserVisitLog = function () {
        var loginId = self.loginUserId(), bUserId = self.browserUserId();
        if (loginId == bUserId) {
            return;
        }
        var reqData = {
            loginUserId: loginId,
            browserUserId: bUserId
        };
        var url = api + "UserVisitLog/CreateUserVisitLog";
        //console.log("访客记录url:" + url);
        $.post(url, reqData, function (data) {
            // console.log("历史记录:");
            // console.log(data);

        }, "json");
    }
    //------------------begin 换肤-------------------------------------------------------
    //浏览的用户空间的皮肤id
    self.SkinId = ko.observable(0);
    self.defaultSkin = ko.observable(0);
    self.SkinData = [];
    self.curLink = {};
    self.initSkinData = function (isShow) {
        //console.log(skinData);
        $(skinData).each(function (idx, o) {
            var html = "<div class='pifu_tp'><div class='pifu_t_img'><img src='" + rootPath + o.SmallImage + "' width='58' height='58' class='themes' ></div>" + o.SkinName + "</div>";
            var linkPath = rootPath + o.CssPath;
            var linkId = self.browserUserId() + '_' + o.Id;
            self.SkinData.push([html, linkPath, linkId, o.Id]);
            if (o.IsDefaultSkin)
                self.defaultSkin(o.Id);
        });
        //设置生效的css
        self.writeCSS();
        //添加link
        self.writeCSSLinks();
        // console.log("result:" + _isMyRoom);
        if (isShow && self.loginUserId() > 0) {
            var timer = null;
            //隐藏的皮肤                                                             
            $("#corediv").bind("click", function (e) {
                $("#Sright").show("slow");
                return false;
            });
            $("#savebtn").bind("click", function (e) {
                self.saveSkin();
                $("#Sright").hide("slow");
            });
            $(document).bind("click", function (e) {
                var ckEle = $(e.target ? e.target : srcElement);
                if (ckEle.find("#Sright").length > 0)
                    $("#Sright").hide("slow");
            });
            $("#corediv").show();
        } else {
            $("#corediv").hide();
        }
    }
    self.writeCSS = function () {
        var cssLink = [];
        $(self.SkinData).each(function (idx, o) {
            cssLink.push('<link title="css_' + o[2] + '" href="' + o[1] + '" skin="1" rel="stylesheet" disabled="true" type="text/css" />');
        });
        $("#csslink").html(cssLink.join(''));
        var strCss = "css_" + self.browserUserId() + "_" + self.SkinId();
        if (self.SkinId() == 0)
            strCss = "css_" + self.browserUserId() + "_" + self.defaultSkin();
        //console.log("strCss:" + strCss);
        // $("#page_options").css("background", "url(../images/pifu_img/banner1.png) no-repeat center center;");
        self.setStyleSheet(strCss);
    }

    self.writeCSSLinks = function () {
        var linkArr = [];
        $(self.SkinData).each(function (idx, o) {
            if (idx > 0) linkArr.push('  ');
            linkArr.push('<a href="javascript:v()" onclick="vm_roomHeader.setStyleSheet(\'css_' + o[2] + '\')">' + o[0] + '</a>');
        });
        var html = linkArr.join('');
        // console.log(html);
        $("#ulink").html(html);
    }

    self.setStyleSheet = function (strCSS) {
        var intFound = 0;
        $("link[skin=1]").each(function (i, link) {
            if ($(link)[0].type.indexOf("css") > -1 && $(link)[0].title) {
                $(link)[0].disabled = true;
                if ($(link)[0].title == strCSS) intFound = i;
            }
        });
        self.curLink = $("link[skin=1]")[intFound];
        self.curLink.disabled = false;
    }
    //保存皮肤
    self.saveSkin = function () {
        var arr = self.curLink != null ? self.curLink.title.split('_') : [];
        if (arr.length == 3) {
            var _userId = arr[1], _skinId = arr[2];
            var url = api + "User/SaveSkinByUserId?userId=" + _userId + "&skinId=" + _skinId + "&v=" + Math.random();
            // console.log(url);
            $.getJSON(url, function (data) {
                if (data.Success && data.Data) {
                    globalPromptBox.showGeneralMassage(0, Translate('web_Content_Js_roomHeader_saveSkin_getif'), 1000, false);
                } else {
                    globalPromptBox.showGeneralMassage(2, Translate('web_Content_Js_roomHeader_saveSkin_getelse'), 3000, false);
                }
            });
        }
    }
    //------------------end--------------------------------------------------------------
    //初始化数据
    self.init = function () {
        //检查当前有登陆的用户没有
        check(function (d) {
            //没有登录弹出登录框
            // if (d.UserId <= 0) { self.showLogin(); return; };
            //设置当前的登录用户的Id 
            self.loginUserId(d.UserId);
            // console.log("d:");
            //console.log(d);
            // console.log("browserUserId" + browserUserId + " 加载空间:" + d.UserId);

            //浏览的用户id
            browserUserId = getRoomBrowserUserId(self);
            self.browserUserId(browserUserId);
            if (self.browserUserId() > 0) {
                //加载浏览的用户空间数据
                self.loadUserRoom();
                //添加纪录
                self.addUserVisitLog();
            }
        });
    }
}
vm_roomHeader = new roomHeader();
vm_roomHeader.init();
ko.applyBindings(vm_roomHeader, document.getElementById("roomHeader"));
