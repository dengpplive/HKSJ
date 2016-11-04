$(function () {
    function setPlayImg() {
        var url = rootPath + "/Content/images/icon_img/play_img.png";
        var l = $(".play_img").length;
        // console.log("play_img length=" + l);
        $(".play_img").each(function () {
            var play_img = $(this);
            var width = play_img.attr("wth");
            var height = play_img.attr("hht");

            var mleft = play_img.attr("mleft"); if (mleft == null) mleft = 0;
            var mtop = play_img.attr("mtop"); if (mtop == null) mtop = 0;

            play_img.css({
                "position": "relative",
                "display": "block",
                "white-space": "normal",
                "width": width + "px",
                "height": height + "px",
                "margin-left": mleft + "px",
                "margin-top": mtop + "px"
            });
            play_img.find("i").css({
                "background-image": url,
                "height": height + "px",
                "width": width + "px",
                "margin-left": mleft + "px",
                "margin-top": mtop + "px"
            });
        });
    }

    function setSlide() {
        $(".right_img li").mouseenter(function () {
            $(this).addClass("right_li_01").siblings().removeClass("right_li_01")
        })
    }
    //板块视频
    var plateContainer = $("#plateContainer");
    //视频分类
    var videoContainer = $("#videoContainer");

    if (videoContainer.length > 0) {
        //设置标题
        var title = $("#navfirst li a[class=curr]").text();
        document.title = title + "-" + companyTitle;
    }
    function DataItem() {
        var self = this;
        //板块数据
        self.plateData = ko.observableArray([]);
        //分类视频
        self.categoryData = ko.observableArray([]);
        //公共数据
        self.commData = {
            adsAlt: "脱贫宝"
        };
        var c = 0;
        function exec() {
            var url = rootPath + "/Home/LoadPlateVideo";
            if (plateData.length > 0) {
                //先按照顺序占位 后补充数据
                for (var i = 0; i < plateData.length; i++) {
                    self.plateData.push({
                        pId: plateData[i].Id,
                        curId: curId,
                        adsAlt: self.commData.adsAlt,
                        data: {
                            model: {},
                            curr: plateData[i].CategoryId,
                            plateView: plateData[i]
                        }
                    });
                }
                for (var key in plateData) {
                    //console.log("板块url:");
                    //console.log(url);
                    $.post(url, plateData[key], function (result) {
                        try {
                            //console.log("板块结果:");
                            //console.log(plateData);
                            var pData = self.plateData();
                            for (var i = 0; i < pData.length; i++) {
                                if (pData[i].pId == result.plateView.Id) {
                                    self.plateData.replace(pData[i], {
                                        pId: pData[i].pId,
                                        curId: curId,
                                        adsAlt: self.commData.adsAlt,
                                        data: result
                                    });
                                }
                            }
                            setPlayImg();
                            setSlide();
                        } finally {
                            c++;
                            if (curId > 0 && c == plateData.length) {
                                if ($('#plateContainer img[video="true"]').length === 0) execCategory();
                            }
                        }
                    }, "json");
                }
            } else {
                if (curId > 0) {
                    execCategory();
                }
            }
        }
        function execCategory() {
            //先按照顺序占位 后补充数据
            for (var i = 0; i < menuViewList.length; i++) {
                self.categoryData.push({
                    menuId: menuViewList[i].ParentCategory.Id,
                    curId: curId,
                    adsAlt: self.commData.adsAlt,
                    data: {
                        model: {}, cId: curId + "_" + menuViewList[i].ParentCategory.Id,
                        category: menuViewList[i].ParentCategory
                    }
                });
            }
            //异步填充数据
            for (var n in menuViewList) {
                var url = rootPath + "/Home/LoadCategoryVideo";
                var postData = menuViewList[n].ParentCategory;
                postData.CurId = curId;
                postData.PageSize = 10;

                // console.log("分类视频url:");
                // console.log(url);
                // console.log(postData);
                $.post(url, postData, function (result) {
                    try {
                        //console.log("分类视频数据");
                        //console.log(result);
                        var menuData = self.categoryData();
                        for (var i = 0; i < menuData.length; i++) {
                            if (menuData[i].menuId == result.category.Id) {
                                self.categoryData.replace(menuData[i], {
                                    menuId: menuData[i].menuId,
                                    curId: curId,
                                    adsAlt: self.commData.adsAlt,
                                    data: result
                                });
                            }
                        }
                    } catch (e) {
                        // console.log("execCategory:" + e.message);
                    } finally {
                        setPlayImg();
                    }
                }, "json");
            }
        }
        //获取播放页面的url
        self.getPlayUrl = function (id) {
            return rootPath + "/Play/Index?videoId=" + id;
        }
        //跳转到过滤页面
        self.getFilterUrl = function (category) {
            var url = '';
            if (category != null)
                url = rootPath + '/Filter/Index?curId=' + (curId < 0 ? category.Id : curId) + '&filter=gc'
                 + (curId < 0 ? category.Id : curId) + "c" + (category.Id) + "r";
            return url;
        }
        //跳转到过滤页面
        self.getChildFilterUrl = function (category, childCategory) {
            var url = '';
            if (category != null && childCategory != null)
                url = rootPath + '/Filter/Index?curId=' + (curId < 0 ? category.Id : curId) + '&parentId=' + category.Id + '&noteId=' + childCategory.ParentCategory.Id
            return url;
        }
        self.getChilds = function (Id) {
            var Childs = [];
            for (var i in menuViewList) {
                if (menuViewList[i].ParentCategory.Id == Id && menuViewList[i].ChildCategorys.length > 0) {
                    Childs = menuViewList[i].ChildCategorys;
                    break;
                }
            }
            return Childs;
        }
        self.showTitle = function (title) {
            return (title != null ? title : '');
        }
        self.showAbout = function (about) {
            return (about != null ? about : '');
        }
        self.initData = function () {
            if (curId > 0) {
                exec();
            } else {
                exec();
                execCategory();
            }
        }
    }
    var index_VM = new DataItem();
    index_VM.initData();
    ko.applyBindings(index_VM, document.getElementById('mainContainer'));
});