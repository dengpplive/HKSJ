var videoNum = 0;
var albumNum = 0;

function IndexModel() {
    var self = this;
    self.browserUserId = ko.observable(-1);
    //当前的登录用户id
    self.loginUserId = ko.observable(-2);
    //是否进入编辑管理页面
    self.isEditManage = ko.observable(false);
    //首页需要的数据
    self.OwnerData = {
        HotVideos: ko.observableArray([]),
        AlbumVideos: ko.observableArray([]),
        AlbumVideosLength: ko.observable(0),
        Subscribes: ko.observableArray([]),
        Fans: ko.observableArray([]),
        HistoryVisitors: ko.observableArray([]),
        ForYouRecomments: ko.observableArray([])
    };
    //显示子专辑名称
    self.showSonAlbumName = function (name) {
        var albumTitle = '【{0}】';
        return albumTitle.Format(name);
    }
    //默认用户图像
    self.defaultUserImage = ko.observable(defaultData.defaultUserImage);
    //加载错误处理
    self.loadError = function () {
        //用户图像无法显示时 显示默认的图像
        $("img[userimg]").error(function () {
            $(this).attr("src", self.defaultUserImage());
        });
    }
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
    self.goSonAlbum = function (id) {
        return rootPath + "/UserRoom/SonAlbum?browserUserId=" + self.browserUserId() + "&spId=" + id + "&linkcss=2";
    }
    self.goAlbum = function () {
        return rootPath + "/UserRoom/Album?browserUserId=" + self.browserUserId() + "&linkcss=2";
    }
    self.goVideo = function () {
        return rootPath + "/UserRoom/Video?browserUserId=" + self.browserUserId() + "&linkcss=1";
    }
    //默认的视频图片
    //....暂无

    //显示 时和分
    self.showPlayTime = function (second) {
        return new Date(second * 1000 - 8 * 60 * 60 * 1000).format("hh:mm");
    }

    //加载数据检测用户
    self.loadCheck = function (opCallback) {
        //检查当前有登陆的用户没有
        check(function (d) {
            //设置当前的登录用户的Id 
            self.loginUserId(d.UserId);
            //浏览的用户id
            browserUserId = getRoomBrowserUserId(self);
            self.browserUserId(browserUserId);
            //console.log("browserUserId" + browserUserId + " 加载空间:" + d.UserId);
            if (opCallback)
                opCallback(d);
        });
    }
    //显示登录框
    self.showLogin = function () {
        LR.LoginShow();
    }
    //操作数据检测用户
    self.operCheck = function (opCallback) {
        //检查当前有登陆的用户没有
        check(function (d) {
            if (d.Success) {
                //设置当前的登录用户的Id 
                self.loginUserId(d.UserId);
                //浏览的用户id
                browserUserId = getRoomBrowserUserId(self);
                self.browserUserId(browserUserId);
                if (opCallback)
                    opCallback(d);
            } else {
                //显示登陆框
                LR.LoginShow();
            }
        });
    }
    //加载热门视频
    self.loadHotVideo = function () {
        //发送ajax请求获取数据      
        var url = api + "UserRoomChoose/GetUserRoomVideoData?userId=" + self.browserUserId() + "&v=" + Math.random();
        //console.log("空间热门视频url:" + url);
        $.getJSON(url, function (data) {
            try {
                //    console.log("数据");
                //   console.log(data);
                for (var i in data) {
                    data[i].PlayTime = data[i].TimeLength == null ? "00:00" : self.showPlayTime(data[i].TimeLength);
                }
                self.OwnerData.HotVideos(data);
                videoNum = data.length;
                if (data.length >= 12)
                    $('#btnAddVideo').hide();
            } finally {
                $("#vdCount").show();
                removeMask($("div[loading='vedio']"));
            }
        });
    }
    //加载专辑视频
    self.loadAlbumVideo = function () {
        var url = api + "UserRoomChoose/GetUserRoomSpecialData?userId=" + self.browserUserId() + "&videoNum=0&v=" + Math.random();
        //console.log("专辑视频url:" + url);
        $.getJSON(url, function (data) {
            try {
                //console.log("专辑视频数据:");
                //console.log(data);

                for (var i in data.SpecialVideoList) {
                    var obj = {
                        Category: {
                            Id: data.SpecialVideoList[i].Id, Name: data.SpecialVideoList[i].Title
                        },
                        AlbumVideo: ko.observableArray([])
                    };
                    var list = data.SpecialVideoList[i].SpecialVideoList;
                    for (var j in list) {
                        list[j].PlayTime = list[j].TimeLength == null ? "00:00" : self.showPlayTime(list[j].TimeLength);
                    }
                    obj.AlbumVideo(list);
                    self.OwnerData.AlbumVideos.push(obj);
                }
                self.OwnerData.AlbumVideosLength(data.SpecialCount);

                albumNum = data.SpecialCount;
                if (data.SpecialCount >= 3)
                    $('#btnAddAlbums').hide();
            } finally {
                removeMask($("div[loading='album']"));
                $("#bmCount").show();
            }
        });

    }

    //加载订阅
    self.loadSubscribe = function () {

        //var obj = {
        //    Id: 12,
        //    NickName: "三德子",//用户呢称
        //    Picture: rootPath + "/Content/images/icon_img/per_x_ttx.png",//用户头像         
        //};
        //for (var i = 0; i < 15; i++) {
        //    self.OwnerData.Subscribes.push(obj);
        //}
    }
    //加载粉丝
    self.loadFans = function () {
        var pageSize = 8;
        var pageindex = 1;
        var reqData =
        {
            userId: self.browserUserId(),
            pagesize: pageSize,
            pageindex: pageindex,
            condtions: [],
            ordercondtions: [{
                FiledName: "UpdateTime",
                IsDesc: true
            }]
        };
        var url = api + "UserFans/GetUserFunsList";
        // console.log("首页用户粉丝url:" + url)
        ///console.log("首页用户粉丝请求数据:");
        //console.log(reqData);
        //发送请求
        $.post(url, reqData, function (data) {
            try {
                // console.log("首页用户粉丝结果:");
                // console.log(data);
                if (data != null)
                    self.OwnerData.Fans(data.Data);
                else {
                    //console.log("无数据");
                }
                self.loadError();
            } finally {
                removeMask($("div[loading='fensi']"));
            }
        }, "json");
    }
    //加载最近访客
    self.loadHistoryVisitor = function () {
        var reqData = {
            pagesize: 8,
            browserUserId: self.browserUserId()
        };
        var url = api + "UserVisitLog/GetUserVisitLogList";
        //console.log("最近访客url:" + url)
        $.post(url, reqData, function (data) {
            try {
                //console.log("最近访客数据:");
                //console.log(data);
                self.OwnerData.HistoryVisitors(data);
                self.loadError();
            } finally {
                removeMask($("div[loading='fangke']"));
            }
        }, "json");
    }

    //我的推荐中的订阅按钮事件
    self.subUser = function (fan) {
        self.operCheck(function (res) {
            if (res.Success) {
                //没有登录弹出登录框
                if (res.UserId <= 0) {
                    self.showLogin(); return;
                };

                var d = {
                    createUserId: res.UserId,
                    subscribeUserId: fan.UserView.Id,
                    careState: fan.State()
                };
                var url = api + "UserFans/SaveSubscribe";
                // console.log("订阅和取消订阅url:" + url);
                // console.log("订阅和取消订阅请求数据:");
                // console.log(d);
                $.post(url, d, function (data) {
                    // console.log("订阅和取消订阅结果:");
                    //console.log(data);
                    var msg = data.Success ? Translate("web_Content_Js_roomIndex_subUser_msgtrue") : Translate("web_Content_Js_roomIndex_subUser_msgfalse");
                    globalPromptBox.showGeneralMassage(data.Success ? 0 : 2, msg, 1000, data.Success);
                }, "json");
            }
        });
    }
    //加载为你推荐
    self.loadForYouRecomment = function () {
        var url = api + "UserRecommend/GetUserRecommendList?v=" + Math.random();
        var d = {
            pagesize: 5,
            userId: self.browserUserId(),
            loginUserId: self.loginUserId()
        };
        // console.log("推荐用户url:" + url)
        $.post(url, d, function (data) {
            try {
                // console.log("推荐用户数据:");
                //console.log(data);
                var list = data;
                for (var i in list) {
                    for (var key in list[i]) {
                        //处理状态
                        if (key == "State") {
                            list[i][key] = ko.observable(list[i][key]);
                        }
                    }
                }
                self.OwnerData.ForYouRecomments.length = 0;
                self.OwnerData.ForYouRecomments(data);
            } finally {
                self.loadError();
                removeMask($("div[loading='foryoutj']"));
            }
        }, "json");
    }
    //是否为当前登录用户
    self.isCurrLogin = function (userId) {
        return self.loginUserId() == userId;
    }
    //从访问记录进入另一用户空间
    self.getVisitLogUserRoomUrl = function (data) {
        return rootPath + '/UserRoom/Index?browserUserId=' + data.UserView.Id;
    }
    //跳转到播放页面
    self.goPlayer = function (Id, anchor) {
        var url = rootPath + "/Play/Index?videoId=" + Id + "&v=" + Math.random();
        if (anchor)
            url = url + "#" + anchor;
        return url;
    }
    //显示部分字符
    self.partString = function (str, len, ellipsis) {
        if (str.length > len) {
            str = str.substr(0, len) + ellipsis;
        }
        return str;
    }
    //初始化数据
    self.init = function () {
        self.loadCheck(function (d) {
            if (self.browserUserId() > 0) {
                var manage = $.trim(GetQueryString("manage"));
                if (manage == "1") {
                    self.isEditManage(true);
                } else {
                    self.isEditManage(false);
                }
                self.setInitLoad();
                self.loadHotVideo();
                self.loadAlbumVideo();
                self.loadSubscribe();
                self.loadFans();
                self.loadHistoryVisitor();
                self.loadForYouRecomment();
            }
        });
    }
    //初始化添加加載中的效果
    self.setInitLoad = function () {
        $("#vdCount").hide();
        $("#bmCount").hide();
        addMask($("div[loading]"));
    }


    //移除视频
    self.doRemoveVideo = function (videoId, ele) {
        var deleteing = $(ele).attr("deleteing");
        if (deleteing == null || deleteing == undefined) {
            //标志正在删除中 防止单击多次
            $(ele).attr("deleteing", true);
            $.ajax({
                type: "Post",
                url: api + "UserRoomChoose/RemoveVideoToUserRoom",
                data: {
                    userId: userId, videoId: videoId
                },
                dataType: "json",
                success: function (data) {
                    $(ele).removeAttr("deleteing");
                    globalPromptBox.showGeneralMassage(data.Success ? 0 : 2, data.Success ? Translate("web_Content_Js_roomIndex_doRemoveVideo_success_true") : Translate("web_Content_Js_roomIndex_doRemoveVideo_success_false"), 1000, data.Success);
                },
                error: function (error) {
                    $(ele).removeAttr("deleteing");
                    globalPromptBox.showGeneralMassage(2, error.ExceptionMessage, 3000, false);
                }
            });
        }
    }


    self.doRemoveAlbum = function (albumId, ele) {
        var deleteing = $(ele).attr("deleteing");
        if (deleteing == null || deleteing == undefined) {
            $(ele).attr("deleteing", true);
            $.ajax({
                type: "Post",
                url: api + "UserRoomChoose/RemoveAlbumtToUserRoom",
                data: {
                    userId: userId, albumId: albumId, t: Math.random()
                },
                dataType: "json",
                success: function (data) {
                    $(ele).removeAttr("deleteing");
                    globalPromptBox.showGeneralMassage(data.Success ? 0 : 2, data.Success ? Translate("web_Content_Js_roomIndex_doRemoveAlbum_success_true") : Translate("web_Content_Js_roomIndex_doRemoveAlbum_success_false"), 1000, data.Success);
                },
                error: function (error) {
                    $(ele).removeAttr("deleteing");
                    globalPromptBox.showGeneralMassage(2, error.ExceptionMessage, 3000, false);
                }
            });
        }
    }




}


vm_Index = new IndexModel();
vm_Index.init();
ko.applyBindings(vm_Index, document.getElementById("main_index"));

