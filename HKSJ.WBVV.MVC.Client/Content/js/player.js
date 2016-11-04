$(function () {
    if (videoData == null || videoData.Id <= 0) {
        globalPromptBox.showGeneralMassage(2, Translate('web_Content_Js_player_readyfunction_message'), 5000, false);
        setTimeout("history.go(-1)", 5000);
    }
})
var wm_video = function () {
    var self = this;
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
    //视频编号
    self.videoId = ko.observable(-2);
    //当前登录用户编号
    self.userId = ko.observable(0);
    //请求的url
    self.urls = {
        apiUrls: {
            add: api + "Comment/CreateVideoComment",//添加视频评论
            addReplay: api + "Comment/ReplyVideoComment",//回复视频评论
            getCommentList: api + "Comment/GetVideoComments",//获取视频评论列表
            addPraiseComment: api + "Praise/CreatePraisesComment",//评论点赞 
            cancelPraisesComment: api + "Praise/CancelPraisesComment",//评论取消点赞    
            deleteComment: api + "Comment/DeleteVideoComment"//删除评论
        },
        check: rootPath + "/Comment/Check"
    };
    //显示登录框
    self.showLogin = function () {
        LR.LoginShow();
    }
    //监控的属性
    var obserProperty = [];
    //简介数据
    self.aboutDetail = ko.observableArray([]);
    //添加评论数据
    self.formData = ko.observable('');
    //回复数据
    self.replayData = ko.observable('');

    //分页容器
    self.pagerId = ko.observable('pager');
    //每页大小
    self.pageSize = ko.observable(10);
    //子集每页大小
    self.childPageSize = ko.observable(5);
    //子集页位置 用于定位
    self.childPageNumber = ko.observable(1);
    //当前页
    self.pageNumber = ko.observable(1);
    //查询到的评论总数 不含回复
    self.totalcount = ko.observable(0);
    //总页数
    self.pageCount = ko.observable(1);
    //是否显示分页条 也就是"查看更多"按钮
    self.isShowPagerBar = ko.observable(false);
    //评论列表
    self.list = ko.observableArray([]);
    //检测登录用户
    self.check = function (opCallback) {
        //检查当前有登陆的用户没有
        check(function (d) {
            //设置当前的登录用户的Id 
            self.userId(d.UserId);
            if (opCallback)
                opCallback(d);
        });
    }
    //是否所属留言
    self.isOwner = ko.observable(false);
    //更新分页
    self.refreshPager = function () {
        //如果数据大于每页大小 显示分页条
        if (self.totalcount() > self.pageSize()) self.isShowPagerBar(true); else self.isShowPagerBar(false);
        //重新计算页数
        var pagecnt = self.totalcount() % self.pageSize() == 0 ? parseInt(self.totalcount() / self.pageSize(), 10) : parseInt(self.totalcount() / self.pageSize(), 10) + 1;
        //总页数
        self.pageCount(pagecnt);
        //设置分页
        $("#" + self.pagerId()).pager({
            pagenumber: self.pageNumber(), pagecount: self.pageCount(), totalcount: self.totalcount(), buttonClickCallback: function (pageclickednumber) {
                //单击加载
                self.loadCommentPager(null, pageclickednumber);
            }
        });
        // console.log("totalcount:" + self.totalcount() + " pagenumber:" + self.pageNumber() + " pagecount:" + self.pageCount() + " ");
    }
    //获取评论数据列表 含有分页
    self.loadCommentPager = function (fnBefore, num, fnAfter) {
        var d = {
            videoId: vId,
            loginUserId: self.userId(),
            size: self.childPageSize(),
            index: 1,
            pagesize: self.pageSize(),
            pageindex: num,
            v: Math.random()
        };
        if (fnBefore) d = fnBefore(d);
        var url = self.urls.apiUrls.getCommentList;

        //console.log("获取视频评论url:" + url);
        // console.log(d);
        $.getJSON(url, d, function (data) {
            // console.log("视频评论结果:");
            //console.log(data);
            //评论列表
            var list = [];
            for (var i = 0; i < data.Data.length; i++) {
                var obj = self.setKO(data.Data[i], obserProperty);
                self.initChildData(obj);
                list.push(obj);
            }
            //设置数据
            self.list(list);
            //每页大小
            self.pageSize(d.pagesize);
            //设置当前页
            self.pageNumber(d.pageindex);
            //总条数
            self.totalcount(data.TotalCount);
            //更新分页
            self.refreshPager();
            //回调
            if (fnAfter) fnAfter();
            //处理错误图像
            self.loadError();
        });
    }

    //添加评论
    self.addComment = function (ele) {
        var rqStatus = $(ele).attr("rqStatus");
        if (rqStatus == null || rqStatus == undefined) {
            //正在请求
            $(ele).attr("rqStatus", "requesting");
            $(ele).attr("disabled", "disabled");
            //检测用户登录
            self.check(function (res) {
                //没有登录弹出登录框
                if (self.userId() <= 0) {
                    $(ele).removeAttr("rqStatus");
                    $(ele).removeAttr("disabled");
                    self.showLogin(); return;
                };
                if ($.trim($("#emotion").val()) == "") self.formData("");
                //评论内容 已处理表情格式
                var commentContent = storeEmotion(self.formData());
                if ($.trim(commentContent) == "") {
                    $(ele).removeAttr("rqStatus");
                    $(ele).removeAttr("disabled");
                    return;
                }
                //提交的数据
                var submitData = {
                    videoId: self.videoId(),
                    userId: self.userId(),
                    commentContent: commentContent,
                    v: Math.random()
                };
                //调用接口  
                $.post(self.urls.apiUrls.add, submitData,
                function (data) {
                    if (data.Success) {
                        //成功 更新UI  
                        self.loadCommentPager(null, 1, function () {
                            $(ele).removeAttr("rqStatus");
                            $(ele).removeAttr("disabled");
                            self.formData('');
                        });
                        globalPromptBox.showGeneralMassage(0, Translate("web_Content_Js_player_addcomment_message_postif"), 1000, false);
                    } else {
                        globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
                        $(ele).removeAttr("rqStatus");
                        $(ele).removeAttr("disabled");
                    }
                }, "json");
            });
        }
    }
    //回复评论   
    self.replayComment = function (ele, koData) {
        var commentId = koData.Id;
        var cnt = $(ele).parent("div").parent("div");
        //检测请求状态  
        var rqStatus = $(ele).attr("rqStatus");
        if (rqStatus == null || rqStatus == undefined) {
            $(ele).attr("rqStatus", "requesting");
            $(ele).attr("disabled", "disabled");
            //检测用户登录
            self.check(function (res) {
                //没有登录弹出登录框
                if (self.userId() <= 0) {
                    self.showLogin();
                    $(ele).removeAttr("rqStatus");
                    $(ele).removeAttr("disabled");
                    return;
                };
                if ($.trim($("#emotion_" + commentId).val()) == "")
                    self.replayData("");

                //回复内容 已处理表情格式
                var commentReplay = storeEmotion(self.replayData());
                if ($.trim(commentReplay) == "") {
                    $(ele).removeAttr("rqStatus");
                    $(ele).removeAttr("disabled");
                    return;
                }
                addMask(cnt,4);//添加遮罩
                //请求数据
                var submitData = {
                    videoId: self.videoId(),//视频id
                    userId: self.userId(),//用户id
                    commentId: commentId,//回复的评论id
                    commentContent: commentReplay,//回复的内容
                    v: Math.random()
                };
                //调用接口        
                $.post(self.urls.apiUrls.addReplay, submitData, function (data) {
                    // console.log("回复评论");
                    // console.log(data);
                    if (data.Success) {
                        //重新加载
                        self.loadCommentPager(null, self.pageNumber(), function () {
                            $(ele).removeAttr("rqStatus");
                            $(ele).removeAttr("disabled");
                            removeMask(cnt);//移除遮罩
                        });
                        self.replayData('');
                    }
                    else {
                        globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
                        $(ele).removeAttr("rqStatus");
                        $(ele).removeAttr("disabled");
                        removeMask(cnt);//移除遮罩
                    }
                });
            });
        }
    }
    //删除评论
    self.deleteComment = function (ele, koData) {
        //检测请求状态  
        var rqStatus = $(ele).attr("rqStatus");
        if (rqStatus == null || rqStatus == undefined) {
            $(ele).attr("rqStatus", "requesting");
            $(ele).attr("disabled", "disabled");
            //检测用户登录
            self.check(function (res) {
                //没有登录弹出登录框
                if (self.userId() <= 0) {
                    self.showLogin();
                    $(ele).removeAttr("rqStatus");
                    $(ele).removeAttr("disabled");
                    return;
                };
                //请求数据
                var commentId = koData.Id;
                var submitData = {
                    userId: res.UserId,
                    commentId: commentId,//评论id
                    v: Math.random()
                };
                var li = $('ul li[loading="li_' + commentId + '"]');
                addMask(li, 5);
                //console.log("删除评论url:" + self.urls.apiUrls.deleteComment);
                //console.log(submitData);
                //调用接口        
                $.post(self.urls.apiUrls.deleteComment, submitData, function (data) {
                    // console.log("删除评论结果");
                    // console.log(data);
                    if (data.Success) {
                        //重新加载
                        self.loadCommentPager(null, self.pageNumber(), function () {
                            $(ele).removeAttr("rqStatus");
                            $(ele).removeAttr("disabled");
                            removeMask(li, 1);
                        });
                    }
                    else {
                        globalPromptBox.showGeneralMassage(2, data.ExceptionMessage, 3000, false);
                        $(ele).removeAttr("rqStatus");
                        $(ele).removeAttr("disabled");
                    }
                });
            });
        }
    }
    self.getCommenterUserroomUrl = function (createUserid) {
        return rootPath + '/UserRoom/Index?browserUserId=' + createUserid;
    }
    self.returnCommenterUserroomUrl = function (createUserid) {
        var url = rootPath + '/UserRoom/Index?browserUserId=' + createUserid;
        window.open(url, "_blank");
    }
    //评论赞
    self.commentPraises = function (id, index, data, koData) {
        // console.log("koData:");
        // console.log(koData); return;
        //检测用户登录
        self.check(function (res) {
            //没有登录弹出登录框
            if (self.userId() <= 0) {
                self.showLogin(); return;
            };

            var d = {
                userId: self.userId(),
                commentId: data.Id,//评论id
                v: Math.random()
            };
            //console.log("评论赞url:" + self.urls.apiUrls.addPraiseComment);
            //console.log(d);
            //发送POST请求
            $.post(self.urls.apiUrls.addPraiseComment, d, function (list) {
                //console.log("评论赞结果");
                //console.log(list);
                if (list.Success) {
                    self.loadCommentPager(function (param) {
                        param.index = koData.pagenumber();
                        return param;
                    }, self.pageNumber(), function () {
                        //globalPromptBox.showBubble(id, null, 3000, '赞成功!');
                    });
                } else {
                    //globalPromptBox.showBubble(id, null, 3000, list.ExceptionMessage);
                }
            });
        });
    }
    //评论取消赞
    self.cancelPraisesComment = function (id, index, data, koData) {
        //检测用户登录
        self.check(function (res) {
            //没有登录弹出登录框
            if (self.userId() <= 0) {
                self.showLogin(); return;
            };
            var d = {
                userId: self.userId(),
                commentId: data.Id,//评论id
                v: Math.random()
            };
            //console.log("评论取消赞url:" + self.urls.apiUrls.addPraiseComment);
            //console.log(d);
            //发送POST请求
            $.post(self.urls.apiUrls.cancelPraisesComment, d, function (list) {
                //console.log("评论取消赞结果");
                //console.log(list);
                if (list.Success) {
                    self.loadCommentPager(function (param) {
                        param.index = koData.pagenumber();
                        return param;
                    }, self.pageNumber(), function () {
                        //globalPromptBox.showBubble(id, null, 3000, '取消赞成功!');
                    });
                } else {
                    //globalPromptBox.showBubble(id, null, 3000, list.ExceptionMessage);
                }
            });
        });
    }
    //显示隐藏回复评论
    self.showReplay = function (element, Id) {
        var b = $(element);
        $("div[name^='rpy_'][name!='rpy_" + Id + "']").hide();
        $("a[alink^='a_link_']").text(Translate("web_Content_Js_player_showReplay_message_alink"));
        var container = $("div[name^='rpy_" + Id + "']");
        if (container.is(":hidden")) {
            b.text(Translate("web_Content_Js_player_showReplay_message_ifbtext"));
            container.show();
            container.find("input[type='text']").focus();

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
            b.text(Translate("web_Content_Js_player_showReplay_message_elsebtext"));
        }
        //清空文本
        self.replayData('');
    }
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
        return input.length + "/140";
    }
    //显示部分字符
    self.partString = function (str, len, ellipsis) {
        if (str.length > len) {
            str = str.substr(0, len) + ellipsis;
        }
        return str;
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

        //  console.log("totalcount:" + koData.totalcount() + " pagenumber:" + koData.pagenumber() + " pagecount:" + koData.pageCount() + " ");
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
            var url = self.urls.apiUrls.getCommentList;
            addMask($("#" + replayDivContainer), 1);
            var postData = {
                videoId: self.videoId(),
                loginUserId: self.userId(),
                pId: koData.ParentComment.Id,
                size: koData.pageSize(),
                index: koData.pagenumber(),
                pagesize: self.pageSize(),
                pageindex: self.pageNumber(),
                v: Math.random()
            };
            //console.log("获取子视频评论url:" + url);
            //console.log(postData);
            $.getJSON(url, postData, function (data) {
                try {
                    //console.log("获取子视频评论结果:");
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
    //----------end查看更多----------------------

    //加载视频简介
    self.loadVideoAboutDetail = function (data) {
        //获取到的视频简介详情
        var dicArr = [];
        for (var i in data.DictionaryViews) {
            dicArr.push({
                "key": i, "val": data.DictionaryViews[i]
            });
        }
        var obj = {
        };
        for (var key in data) {
            obj[key] = data[key];
        }
        obj.items = dicArr;
        self.aboutDetail.push(obj);

        //console.log("视频简介数据:");
        //console.log(data);           
    }
    //获取tag数组
    self.getTag = function (tags) {
        if (tags.substr(tags.length - 1, 1) == '|') {
            tags = tags.substring(0, tags.lastIndexOf('|'));
        }
        var tag = tags.split('|');
        return tag;
    }
    self.showAbout = function (about) {
        if (about) {
            return true;
        }
        else {
            return false;
        }
    }
    self.showTags = function (tags) {
        if (tags.length > 0) {
            return true;
        }
        else {
            return false;
        }
    }


    //--------------------------------------------------------------------------------------------------------------------------------           
    //相关推荐视频
    self.recommendationVideo = ko.observableArray([]);
    //加载推荐的视频
    self.loadRecommendationVideo = function (data) {
        addMask($('div[loading="recomment"]'), 1);
        var videoId = vId;
        var url = api + "Video/SearchVideoByRecom";
        //console.log("相关推荐url:" + self.urls.apiUrls.recommendationVideo);
        //读取相关推荐的视频
        $.getJSON(url, {
            searchKey: data.Tags,
            recommendationNum: 6,
            videoId: videoId
        }, function (model) {
            try {
                // console.log("相关推荐结果:");
                // console.log(model);
                if (model.msg == "success") {
                    //数组数据
                    self.recommendationVideo(model.topdata);

                } else {
                    //没有相关推荐的视频
                }
            } finally {
                removeMask($('div[loading="recomment"]'));
            }
        }, "json");
    }
    //获取视频一级分类
    self.ParentCatagoryId = ko.observable('');
    self.ParentCatagoryName = ko.observable('');
    self.GetParentcatagoryOfvideo = function (data) {
        var videoId = vId;
        var url = api + "Category/GetParentInfo";
        $.getJSON(url, {
            cid: data.CategoryId
        }, function (model) {
            if (model.msg == "success" && model.categorydata != null) {
                // return model.categorydata;
                //console.log(model.categorydata);
                self.ParentCatagoryName(model.categorydata.Name);
                self.ParentCatagoryId(model.categorydata.Id);

            } else {
            }
        }, "json");
    }

    self.title = ko.observable();
    self.CategoryName = ko.observable();
    self.CategoryId = ko.observable();
    self.SmallPicturePath = ko.observable();
    self.commentCount = ko.observable();
    self.playCount = ko.observable();
    self.praiseCount = ko.observable();
    self.rewardCount = ko.observable();
    self.collectionCount = ko.observable();
    self.IsCollected = ko.observable();
    self.nickName = ko.observable('');
    self.videoDetailView = videoData;



    //true 已订阅 false 未订阅
    self.IsDingYue = ko.observable(false);
    //视频来源
    self.videoSource = ko.observable(0);
    //上传视频的用户id
    self.videoOwnUserId = ko.observable(0);
    //显示用户订阅那块
    self.isShowSub = ko.observable(false);
    //用户空间
    self.isShowUserCenter = ko.observable(false);
    //上传视频用户的图片
    self.userPicture = ko.observable(rootPath + '/Content/images/head_img/m_tx_48.png');
    //订阅用户
    self.subUser = function () {
        self.check(function (res) {

            //没有登录弹出登录框
            if (self.userId() <= 0) {
                self.showLogin(); return;
            };

            if (res.Success) {
                var d = {
                    createUserId: res.UserId,
                    subscribeUserId: self.videoOwnUserId(),
                    careState: self.IsDingYue()
                };
                var url = api + "UserFans/SaveSubscribe";
                //console.log("订阅和取消订阅url:" + url);
                //console.log("订阅和取消订阅请求数据:");
                //console.log(d);
                $.post(url, d, function (data) {
                    // console.log("订阅和取消订阅结果:");
                    //console.log(data);
                    if (data.Success) {
                        var b = self.IsDingYue();
                        //console.log("IsDingYue:" + b);
                        self.IsDingYue(!self.IsDingYue());
                        //globalPromptBox.showBubble(null, 'play_pl_02', 3000, self.IsDingYue() ? '订阅成功！' : '取消订阅成功！');
                    }
                }, "json");
            } else {
                //没有登录弹出登录框
                if (self.userId() <= 0) {
                    self.showLogin(); return;
                };
            }
        });
    }
    //是否订阅
    self.IsSubscribe = function () {
        self.check(function (res) {
            if (res.Success) {
                var d = {
                    createUserId: res.UserId,
                    subscribeUserId: self.videoOwnUserId()
                };
                var url = api + "UserFans/IsSubscribe";
                $.post(url, d, function (data) {
                    // console.log("订阅状态:");
                    //console.log(data.Data);
                    if (data.Success) {
                        self.IsDingYue(data.Data);
                    }
                }, "json");
            }
        });

    }
    //数据赋值方法
    self.doVideoInfo = function (d) {
        self.videoId(vId);
        if (self.videoDetailView != null) {
            //加载视频和用户信息
            self.loadPlayInfo(self.videoDetailView);
            //加载相关推荐的视频
            self.loadRecommendationVideo(self.videoDetailView);
            //视频详情数据
            self.loadVideoAboutDetail(videoData);
            //加载评论数据
            self.initComment(d);
            if (self.videoDetailView.Id > 0) {
                self.GetParentcatagoryOfvideo(self.videoDetailView);
            }
            //是否订阅
            self.IsSubscribe();
            //播放视频
            setTimeout(function () {
                playVedio(self.videoDetailView);
            }, 10);
        }
    }
    self.loadPlayInfo = function (data) {
        try {
            //console.log("data:" + data);
            self.videoId(data.Id);
            self.title(data.Title);
            self.CategoryName(data.CategoryName);
            self.CategoryId(data.CategoryId);
            self.SmallPicturePath(data.SmallPicturePath);
            self.nickName(data.NickName);
            self.commentCount(data.CommentCount);
            self.playCount(data.PlayCount);
            self.praiseCount(data.PraiseCount);
            self.rewardCount(data.RewardCount);
            self.collectionCount(data.CollectionCount);
            self.IsCollected(data.IsCollected)
            self.videoSource(data.VideoSource);
            //上传视频的用户id
            self.videoOwnUserId(data.UserId);
            self.isOwner(data.UserId == self.userId());
            // console.log("isOwner:" + self.isOwner() + "data.UserId:" + data.UserId + " self.userId:" + self.userId());
            if (data.VideoSource == 0) {
                $("#dashang").parent().remove();//管理员上传的视频则不可打赏
            }
            if (data.Picture != null && data.Picture != '') {
                if (data.Picture.indexOf("http:") > -1) {
                    self.userPicture(data.Picture);
                } else {
                    self.userPicture(rootPath + data.Picture);
                }
            }
            //console.log("VideoSource:" + data.VideoSource + " UserId:" + data.UserId);
            if (data.VideoSource == 1 && data.UserId != self.userId()) self.isShowSub(true);
            if (data.VideoSource == 1) self.isShowUserCenter(true);
        } catch (e) {
            //console.log(e.message);
        } finally {

        }
    }
    //收藏
    self.doFav = function () {

        //检测用户登录
        self.check(function (res) {
            //没有登录弹出登录框
            if (self.userId() <= 0) {
                self.showLogin(); return;
            };
            var url = api + "UserCollect/CollectVideo";
            var d = {
                vid: self.videoId(),
                uid: self.userId()
            };
            $.post(url, d, function (data) {
                if (data.Success) {
                    self.IsCollected(true);
                    self.collectionCount(self.collectionCount() + 1);
                    //globalPromptBox.showBubble(null, 'play_pl_num03', 3000, '收藏成功！');
                } else {
                    //globalPromptBox.showBubble(null, 'play_pl_num03', 3000, data.ExceptionMessage);
                }
            });
        });
    }
    //取消收藏
    self.doUncollect = function () {

        //检测用户登录
        self.check(function (res) {
            //没有登录弹出登录框
            if (self.userId() <= 0) {
                self.showLogin(); return;
            };
            var url = api + "UserCollect/UnCollectVideoWithoutId";
            var d = {
                vid: self.videoId(),
                uid: self.userId()
            };
            $.post(url, d, function (data) {
                if (data.Success && data.Data) {
                    self.IsCollected(false);
                    self.collectionCount(self.collectionCount() - 1);
                    //globalPromptBox.showBubble(null, 'play_pl_num03', 3000, '取消收藏成功！');
                } else {
                    //globalPromptBox.showBubble(null, 'play_pl_num03', 3000, data.ExceptionMessage);
                }
            });
        });
    }
    self.doUserSpace = function () {
        window.open(rootPath + "/UserRoom/Index?browserUserId=" + self.videoOwnUserId());
        //点击昵称跳转用户空间
        //location.href = rootPath + "/UserRoom/Index?browserUserId=" + self.videoOwnUserId();
    }
    self.SecondCategoryUrl = function () {
        return rootPath + "/Home/index?curId=" + self.ParentCatagoryId();
    }
    self.ThirdCategoryUrl = function () {
        if (self.ParentCatagoryId() == '')
        { return rootPath + "/Home/index?curId=" + self.CategoryId(); }
        else {
            return rootPath + "/Filter?curId=" + self.ParentCatagoryId() + "&filter=" + "gc" + self.ParentCatagoryId() + "c" + self.CategoryId() + "r";
        }
    }
    //获取播放页面的url
    self.getPlayUrl = function (id) {
        return rootPath + "/Play/Index?videoId=" + id;
    }
    //加载数据
    self.loadData = function () {
        self.check(function (d) {
            self.doVideoInfo(d);
        });
    }
    //清除HTML标签
    self.getClearHtml = function (title) {
        title = title.replace(new RegExp(title.match("<span[^>]+>"), "g"), "");
        title = title.replace(new RegExp(title.match("</span>"), "g"), "");
        return title;
    }

    //------------评分类型打赏-----------------------------------------------------
    //用户打赏播币
    self.Reward = function (bb) {
        //检测用户登录
        self.check(function (res) {
            //没有登录弹出登录框
            if (self.userId() <= 0) {
                self.showLogin(); return;
            };
            var url = api + "Reward/CreateRewardVedio";
            var d = {
                loginUserId: res.UserId,
                userId: self.videoOwnUserId(),
                vedioId: self.videoId(),
                bb: bb
            };
            //  console.log(d);
            $.post(url, d, function (data) {
                if (data.Success) {
                    self.rewardCount(self.rewardCount() + bb);
                    globalPromptBox.showBubble(null, 'play_pl_num04', 3000, Translate("web_Content_Js_player_Reward_message_postif").Format(bb));
                } else {
                    globalPromptBox.showBubble(null, 'play_pl_num04', 3000, data.ExceptionMessage);
                }
            });
        });
    }
    //-------------------------
    //绑定表情
    self.bindFace = function () {
        //发表评论的表情
        $("#face").faceEmotion({
            id: "emotion", callback: function (imgTitle, txtObj) {
                if (txtObj.attr("id") == "emotion")
                    self.formData(txtObj.val());
            }
        });
    }
    //初始化评论列表
    self.initComment = function (d) {
        addMask($("div[loading='main']"), 1);
        var islocal = parseInt($.trim(getURLParam("islocal", location.href)).split("#")[0], 10);
        if (islocal == 1) {
            var position = self.getLocalPosition();
            self.loadCommentPager(function (param) {
                param.size = position.commentSize;
                param.index = position.index;
                param.pagesize = position.pagesize;
                param.pageindex = position.pageindex;
                return param;
            }, 1,
            function () {
                removeMask($("div[loading='main']"));
                // console.log("pagesize:" + position.pagesize + " pageindex:" + position.pageindex + " index:" + position.index + " commentSize:" + position.commentSize + "  id:" + position.id + " len:" + $(position.id).length);
                //滚动定位
                $.scrollTo(position.id);
            });

        } else {
            self.loadCommentPager(null, 1, function () {
                removeMask($("div[loading='main']"));
            });
        }
    }
}
var wmvideo = new wm_video();
wmvideo.loadData();
//绑定表情
wmvideo.bindFace();
//绑定ko的数据
ko.applyBindings(wmvideo, document.getElementById('topcontainer'));
//---------------------------------------------------end 视频信息-----------------------------------------//


//---------------------------------------------begin 调用播放器--------------------------------//
//页面数据初始化完毕会加载该函数
function playVedio(videoDetailData) {
    // alert(videoDetailData.VideoPath);
    //通过视频路径Key获取播放路径
    var url = api + "QiniuUpload/GetVideoUrl?key=" + videoDetailData.VideoPath + "&v=" + Math.random();
    $.getJSON(url, function (data) {
        //console.log(data);
        //  alert(data);
        var videoInfo = function () {
            self = this;
            self.id = "divFlashPlayer";
            self.videoPathInfo = watchTime + "," + "0" + "," + "" + "," + data;
            //self.advertPathInfo = "http://7xliow.com2.z0.glb.qiniucdn.com/1441172688325_600x480.mp4" + "," + "10" + "," + "http://1.178pb.com" + "#";
            self.advertPathInfo = "http://7xlsse.com2.z0.glb.qiniucdn.com/1443083129132_A.mp4" + "," + "15" + "," + "http://1.178pb.com" + "#";
        }

        doInit(new videoInfo());
    });
}
//---------------------------------------------end 调用播放器--------------------------------//






