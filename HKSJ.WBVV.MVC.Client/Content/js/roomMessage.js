function msgView() {
    var self = this;
    //登录用户Id
    self.loginUserId = ko.observable(-2);
    //浏览的用户
    self.browserUserId = ko.observable();
    //分页容器
    self.pagerId = ko.observable('pager');
    //每页大小
    self.pageSize = ko.observable(10);
    //子集每页大小
    self.childPageSize = ko.observable(5);
    //当前第几页
    self.pagenumber = ko.observable(1);
    //查询到的留言总数
    self.totalcount = ko.observable(0);
    //总页数
    self.pageCount = ko.observable(1);
    //留言数据列表
    self.list = ko.observableArray();
    //是否显示分页条
    self.isShowPagerBar = ko.observable(false);
    //url列表
    self.urls = {
        apiUrls: {
            addMessageUrl: api + "Comment/CreateSpaceComment",//用户空间发表留言
            replayMessageUrl: api + "Comment/ReplySpaceComment",//用户空间回复留言
            getMessagesUrl: api + "Comment/GetSpaceComments",//获取用户空间留言  
            deleteMessage: api + "Comment/DeleteSpaceComment"//删除用户留言
        }
    };
    //是否所属留言
    self.isOwner = function (userId) {
        return self.loginUserId() == userId;
    }

    //监控的属性
    var obserProperty = ["ReplyNum"];

    //留言文本数据
    self.formData = ko.observable('');
    //留言回复数据    
    self.replayData = ko.observable('');

    //是否查看的是自己的留言
    self.isOwnerMess = function (Id) {
        return self.loginUserId() == Id;
    }
    //是否浏览的自己的留言
    self.isBrowserOwner = function () {
        return self.loginUserId() == self.browserUserId();
    }
    //显示登录框
    self.showLogin = function () {
        LR.LoginShow();
    }
    //默认用户图像
    self.defaultUserImage = ko.observable(defaultData.defaultUserImage);
    //处理用户的图像
    self.handUserPicture = function (pic) {
        if (pic == null || pic == '')
            pic = self.defaultUserImage();
        else {
            if (pic.indexOf("http:") > -1) {
                return pic;
            } else {
                return rootPath + pic;
            }
        }
        return pic;
    }
    //加载错误处理
    self.loadError = function () {
        //用户图像无法显示时 显示默认的图像
        $("img[userimg=1]").error(function () {
            $(this).attr("src", self.defaultUserImage());
        });
    }
    //将对象中的数组转为ko对象监视 
    self.setKO = function (data, observableArray) {
        var obj = {};
        for (var key in data) {
            if (data[key] != null
                && data[key].constructor === Array) {
                for (var i = 0; i < data[key].length; i++) {
                    if (data[key][i] != null && typeof data[key][i] === 'object') {
                        data[key][i] = self.setKO(data[key][i], observableArray);
                    }
                }
                obj[key] = ko.observableArray(data[key]);

            } else {
                if (data[key] != null && typeof data[key] === 'object') {
                    obj[key] = self.setKO(data[key], observableArray);
                } else {

                    obj[key] = data[key];
                    if (observableArray != null && observableArray.length > 0) {
                        for (var i = 0; i < observableArray.length; i++) {
                            if (observableArray[i] == key) {
                                obj[key] = ko.observable(data[key]);
                            }
                        }
                    }
                }
            }
        }
        return obj;
    }

    //加载留言列表
    self.loadMessagePager = function (fnBefore, num, fn) {
        self.check(function (res) {
            var url = self.urls.apiUrls.getMessagesUrl;
            var d = {
                userId: self.browserUserId(),
                loginUserId: res.UserId,
                pagesize: self.pageSize(),
                pageindex: num,
                size: self.childPageSize(),
                index: 1,
                v: Math.random()
            };
            if (fnBefore) d = fnBefore(d);

            //console.log("获取留言列表url:" + url);
            //console.log(d);
            $.getJSON(url, d, function (data) {
                //console.log("留言列表结果:");
                //console.log(data);

                //转换为KO对象
                var list = [];
                for (var i = 0; i < data.Data.length; i++) {
                    var obj = self.setKO(data.Data[i], obserProperty);
                    //初始化
                    obj = self.initChildData(obj);
                    //添加
                    list.push(obj);
                }
                // console.log("处理后的留言列表结果:");
                // console.log(list);
                //设置数据
                self.list(list);
                //设置当前页
                self.pagenumber(d.pageindex);
                self.pageSize(d.pagesize);
                //总条数
                self.totalcount(data.TotalCount);

                //更新分页
                self.refreshPager();
                //图片错误处理
                self.loadError();
                if (fn) fn();
            });
        });
    }
    //更新分页
    self.refreshPager = function () {
        //console.log("totalcount:" + self.totalcount() + " pagenumber:" + self.pagenumber() + " pagecount:" + self.pageCount() + " ");
        //如果数据大于每页大小 显示分页条
        if (self.totalcount() > self.pageSize()) self.isShowPagerBar(true); else self.isShowPagerBar(false);
        //重新计算页数
        var pagecnt = self.totalcount() % self.pageSize() == 0 ? parseInt(self.totalcount() / self.pageSize(), 10) : parseInt(self.totalcount() / self.pageSize(), 10) + 1;
        self.pageCount(pagecnt);
        //设置分页
        $("#" + self.pagerId()).pager({
            pagenumber: self.pagenumber(), pagecount: self.pageCount(), totalcount: self.totalcount(), buttonClickCallback: function (pageclickednumber) {
                //单击加载
                self.loadMessagePager(null, pageclickednumber);
            }
        });
    }
    //添加留言
    self.addMessage = function (ele) {
        var rqStatus = $(ele).attr("rqStatus");
        if (rqStatus == null || rqStatus == undefined) {
            //正在请求
            $(ele).attr("rqStatus", "requesting");
            self.check(function (res) {
                //没有登录弹出登录框
                if (res.UserId <= 0) {
                    self.showLogin();
                    $(ele).removeAttr("rqStatus");
                    return;
                };
                if ($.trim($("#emotion").val()) == "") self.formData("");
                //留言内容 已处理表情数据格式
                var messCon = storeEmotion(self.formData());
                if ($.trim(messCon) == "") {
                    $(ele).removeAttr("rqStatus");
                    return;
                }
                var url = self.urls.apiUrls.addMessageUrl;
                var d = {
                    userId: res.UserId,
                    toUserId: self.browserUserId(),
                    commentContent: messCon,
                    v: Math.random()
                };
                //console.log("留言url:" + url);
                //console.log("留言请求数据:");
                //console.log(d);              
                //发送请求
                $.post(url, d, function (data) {
                    //console.log("留言返回数据:");
                    //console.log(data);
                    if (data.Success) {
                        //刷新界面
                        self.loadMessagePager(null, self.pagenumber(), function () {
                            $(ele).removeAttr("rqStatus");
                        });
                        //清空评论
                        self.formData("");
                        //alert("留言成功");
                    } else {
                        globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
                        $(ele).removeAttr("rqStatus");
                    }
                });
            });
        }
    }
    //回复留言
    self.replayMessage = function (ele, idx, kdata) {
        var cnt = $(ele).parent("div").parent("div");
        var rqStatus = $(ele).attr("rqStatus");
        if (rqStatus == null || rqStatus == undefined) {
            //正在请求
            $(ele).attr("rqStatus", "requesting");
            $(ele).attr("disabled", "disabled");
            self.check(function (res) {
                //没有登录弹出登录框
                if (res.UserId <= 0) {
                    self.showLogin();
                    $(ele).removeAttr("disabled");
                    $(ele).removeAttr("rqStatus");
                    return;
                };
                var messageId = kdata.Id;
                if ($.trim($("#emotion_" + messageId).val()) == "")
                    self.replayData("");
                //留言内容 已处理表情数据格式
                var replayCon = storeEmotion(self.replayData());
                if ($.trim(replayCon) == "") {
                    $(ele).removeAttr("disabled");
                    $(ele).removeAttr("rqStatus");
                    return;
                }
                addMask(cnt, 4); //添加遮罩
                var createUserId = kdata.FromUser.Id;
                var url = self.urls.apiUrls.replayMessageUrl;
                var d = {
                    userId: res.UserId,
                    toUserId: self.browserUserId(),
                    commentId: messageId,
                    commentContent: replayCon,
                    v: Math.random()
                };
                //console.log("回复留言url:" + url);
                // console.log("回复留言请求数据:");
                // console.log(d);
                //console.log(kdata);
                $.post(url, d, function (data) {
                    // console.log("回复留言返回数据:");
                    // console.log(data);
                    if (data.Success) {
                        //刷新界面
                        self.loadMessagePager(null, self.pagenumber(), function () {
                            $(ele).removeAttr("disabled");
                            $(ele).removeAttr("rqStatus");
                            //解除遮罩
                            removeMask(cnt);
                        });
                        //清空回复数据
                        self.replayData("");
                    } else {
                        globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
                        $(ele).removeAttr("disabled");
                        $(ele).removeAttr("rqStatus");
                        //解除遮罩
                        removeMask(cnt);
                    }
                });
            });
        }
    }

    //删除留言
    self.deleteMessage = function (ele, parentComment) {
        var rqStatus = $(ele).attr("rqStatus");
        if (rqStatus == null || rqStatus == undefined) {
            //正在请求
            $(ele).attr("rqStatus", "requesting");
            $(ele).attr("disabled", "disabled");
            self.check(function (res) {
                //没有登录弹出登录框
                if (res.UserId <= 0) {
                    self.showLogin();
                    $(ele).removeAttr("disabled");
                    $(ele).removeAttr("rqStatus");
                    return;
                };
                var url = self.urls.apiUrls.deleteMessage;
                var messageId = parentComment.Id;//留言的Id
                var d = {
                    userId: res.UserId,
                    commentId: messageId,
                    v: Math.random()
                };
                //console.log("删除留言url:" + url);
                //console.log(d);
                //console.log(kdata);
                var li = $('ul li[loading="li_' + messageId + '"]');
                addMask(li, 5);
                $.post(url, d, function (data) {
                    //console.log("删除留言返回数据:");
                    //console.log(data);
                    if (data.Success) {
                        //刷新界面
                        self.loadMessagePager(null, self.pagenumber(), function () {
                            $(ele).removeAttr("disabled");
                            $(ele).removeAttr("rqStatus");
                            removeMask(li, 1);
                        });
                    } else {
                        globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
                        $(ele).removeAttr("disabled");
                        $(ele).removeAttr("rqStatus");
                    }
                });
            });
        }
    }


    //检测登录用户
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
    //---------------------------UI处理 begin-------------------------------------
    //回复显示输入字数
    self.showReplayInputText = ko.computed(function () {
        var input = self.replayData();
        return getMessage(input, 0);
    }, self);
    //显示输入评论字数
    self.showInputText = ko.computed(function () {
        var input = self.formData();
        return getMessage(input, 1);
    }, self);
    //内部使用
    function getMessage(input, flag) {
        if (input.length > 140) {
            input = input.substr(0, 140);
            if (flag == 0)
                self.replayData(input);
            else
                self.formData(input);
        }
        //console.log(self.formData());
        var patString = Translate("{0}/140字");
        return patString.Format(input.length);
    }
    //显示隐藏回复
    self.toggleReplay = function (ele, Id) {
        var b = $(ele);
        $("div.play_detail_huifu[name!='rpy_" + Id + "']").hide();
        $("div[id='msgCon'] a[uid^='link_']").text(Translate("web_Content_Js_roomMessage_toggleReplay_linktext"));
        var container = $("div.play_detail_huifu[name^='rpy_" + Id + "']");
        if (container.is(":hidden")) {
            container.show();
            container.find("input[type='text']").focus();
            b.text(Translate("web_Content_Js_roomMessage_toggleReplay_ifbtext"));
            //注册回复表情              
            var isReg = $("#face_" + Id).attr("reg");
            if (isReg == null) {
                $("#face_" + Id).attr("reg", "1");
                $("#face_" + Id).faceEmotion({
                    id: "emotion_" + Id,
                    callback: function (imgTitle, txtObj) {
                        self.replayData(txtObj.val());
                        $("#emotion").trigger("change");
                    }
                });
            }
        } else {
            container.hide();
            b.text(Translate("web_Content_Js_roomMessage_toggleReplay_elsebtext"));
        }
        //清空文本
        self.replayData('');
    }


    //-----------显示更多回复 子集分页--------------------------
    self.getLocalPosition = function () {
        var pageindex = parseInt($.trim(getURLParam("pageindex", location.href)).split("#")[0], 10);
        var pagesize = parseInt($.trim(getURLParam("pagesize", location.href)).split("#")[0], 10);
        var commentSize = parseInt($.trim(getURLParam("commentSize", location.href)).split("#")[0], 10);
        var index = parseInt($.trim(getURLParam("cmtInx", location.href)).split("#")[0], 10);
        if (!isNaN(pageindex) && !isNaN(pagesize) && !isNaN(commentSize) && !isNaN(index)) {
            if (pageindex <= 0) pageindex = 1;
            if (pagesize <= 0) pagesize = self.pageSize();
            if (commentSize <= 0) commentSize = self.childPageSize();
            if (index <= 0) index = 1;
            return {
                id: location.hash, //用于定位
                pageindex: pageindex,
                pagesize: pagesize,
                commentSize: commentSize,
                index: index
            };
        } else {
            return {
                id: -1,
                pageindex: self.pageNumber(),
                pagesize: self.pageSize(),
                commentSize: self.childPageSize(),
                index: index
            };
        }
    }
    self.initChildData = function (koData) {
        var replayDivContainer = 'replayList_' + koData.ParentComment.Id;
        koData.pagenumber = ko.observable(1);
        koData.dataLength = ko.observable(self.childPageSize());
        var islocal = parseInt($.trim(getURLParam("islocal", location.href)).split("#")[0], 10);
        if (islocal == 1) {
            var position = self.getLocalPosition();
            var isExist = false;
            if ("#_" + koData.ParentComment.Id == position.id) isExist = true;
            else {
                var childArr = koData.ChildComments.Data();
                for (var key in childArr) {
                    if ("#_" + childArr[key].Id == position.id) {
                        isExist = true;
                        break;
                    }
                }
            }
            if (isExist) koData.pagenumber(position.index);
            else koData.pagenumber(1);
        } else {
            //当前第几页
            koData.pagenumber = ko.observable(1);
        }
        //每页大小
        koData.pageSize = ko.observable(self.childPageSize());
        //查询到的子留言总数
        koData.totalcount = ko.observable(koData.ChildComments.TotalCount);
        //总页数
        var pagecnt = koData.totalcount() % koData.pageSize() == 0 ? parseInt(koData.totalcount() / koData.pageSize(), 10) : parseInt(koData.totalcount() / koData.pageSize(), 10) + 1;
        koData.pageCount = ko.observable(pagecnt);
        //是否显示分页
        koData.isShowChildPagerBar = ko.observable(koData.totalcount() > koData.pageSize() ? true : false);

        //console.log("totalcount:" + koData.totalcount() + " pagenumber:" + koData.pagenumber() + " pagecount:" + koData.pageCount() + " ");
        //显示上一页
        koData.showPrevPage = ko.observable(false);
        //显示下一页
        koData.showNextPage = ko.observable(true);
        koData.pagerSwitch = function () {
            if (koData.pagenumber() <= 1) {
                koData.pagenumber(1);
                koData.showPrevPage(false);
                koData.showNextPage(true);
            }
            else if (koData.pagenumber() >= koData.pageCount()) {
                koData.pagenumber(koData.pageCount());
                koData.showPrevPage(true);
                koData.showNextPage(false);
            }
            else {
                koData.showPrevPage(true);
                koData.showNextPage(true);
            }
        }
        //上一页
        koData.prevPage = function () {
            //自减
            koData.pagenumber(koData.pagenumber() - 1);
            //分页显示切换
            koData.pagerSwitch();
            //加载分页
            koData.loadPager();
        }
        //下一页
        koData.nextPage = function () {
            //自增
            koData.pagenumber(koData.pagenumber() + 1);
            //分页显示切换
            koData.pagerSwitch();
            //加载分页
            koData.loadPager();
        }
        //加载分页数据
        koData.loadPager = function () {
            addMask($("#" + replayDivContainer), 1);
            var url = self.urls.apiUrls.getMessagesUrl;
            var postData = {
                userId: self.browserUserId(),
                loginUserId: self.loginUserId(),
                pId: koData.ParentComment.Id,
                size: koData.pageSize(),
                index: koData.pagenumber(),
                pagesize: self.pageSize(),
                pageindex: self.pagenumber(),
                v: Math.random()
            };
            //console.log("获取子集留言url:" + url);
            //console.log(postData);
            $.getJSON(url, postData, function (data) {
                try {
                    //console.log("获取子集留言结果:");
                    //console.log(data);
                    //更新UI
                    koData.ChildComments.Data.removeAll();
                    koData.ChildComments.Data(data.Data);
                    koData.dataLength(data.Data.length);
                    //图片错误处理
                    self.loadError();
                } finally {
                    removeMask($("#" + replayDivContainer));
                }
            });
        }
        //分页显示切换
        koData.pagerSwitch();
        return koData;
    }
    //-------------------------------------
    //跳转到用户空间
    self.goRoom = function (uid) {
        location.href = rootPath + "/UserRoom/Index?browserUserId=" + uid;
    }
    //---------------------------UI处理 end-------------------------------------
    self.init = function () {
        self.check(function (res) {
            //初始化留言
            self.initMessage();
            //绑定表情
            self.bindFace();
        });
    }
    //-------------------------
    //绑定表情
    self.bindFace = function () {
        //发表留言的表情
        $("#face").faceEmotion({
            id: "emotion", callback: function (imgTitle, txtObj) {
                self.formData(txtObj.val());
            }
        });
    }
    //初始化留言
    self.initMessage = function () {
        $("#mcount").hide();
        addMask($("div[loading='main']"), 1);
        var islocal = parseInt($.trim(getURLParam("islocal", location.href)).split("#")[0], 10);
        if (islocal == 1) {
            var position = self.getLocalPosition();
            //console.log("position:");
            //console.log(position);
            self.loadMessagePager(function (param) {
                param.pagesize = position.pagesize;
                param.pageindex = position.pageindex;
                param.size = position.commentSize;
                param.index = position.index;
                return param;
            }, 1, function () {
                //console.log("pagesize:" + position.pagesize + " pageindex:" + position.pageindex + " index:" + position.index + " commentSize:" + position.commentSize + "  id:" + position.id + " len:" + $(position.id).length);
                removeMask($("div[loading='main']"));
                //$("div[loading='main']").remove();
                $("#mcount").show();
                //滚动定位
                $.scrollTo(position.id);
            });
        }
        else {
            self.loadMessagePager(null, 1, function () {
                $("#mcount").show();
                removeMask($("div[loading='main']"));
            });
        }
    }
}

var vm_message = new msgView();
vm_message.init();
ko.applyBindings(vm_message, document.getElementById("msgCon"));