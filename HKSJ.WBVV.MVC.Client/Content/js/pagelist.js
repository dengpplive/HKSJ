var PageModel = function (text, value, active, viewmodel) {
    var self = this;
    self.text = text;
    self.value = value;
    self.active = ko.observable(active);
    self.goPage = function () {
        viewmodel.pageindex(self.value);
        viewmodel.init();
    };
};
var DataModel = function (index, model) {
    var self = this;
    self.index = index;
    self.model = model;
};
var ViewModel = function () {
    var self = this;
    self.userId = 0;
    self.pageindex = ko.observable(1);
    self.pagesize = ko.observable(10);
    self.dataUrl = "";
    self.totalindex = ko.observable(0);
    self.totalcount = ko.observable(0);
    self.page = ko.observableArray([]);
    self.condtions = [];
    self.ordercondtions = [];
    self.data = ko.observableArray([]);
    self.init = function () {
        self.initData();
    };
    self.initData = function () {
        $.ajax({
            url: self.dataUrl,
            data: {
                loginUserId: self.userId,
                pageindex: self.pageindex(),
                pagesize: self.pagesize(),
                condtions: self.condtions,
                ordercondtions: self.ordercondtions
            },
            type: "post",
            async: true,
            dataType: "json",
            success: function (responseData) {
                if (responseData) {
                    self.pageindex(responseData.PageIndex);
                    self.pagesize(responseData.PageSize);
                    self.totalindex(responseData.TotalIndex);
                    self.totalcount(responseData.TotalCount);
                    var list = responseData.Data;
                    var arr = [];
                    if (list && list.length > 0) {
                        for (var i = 0; i < list.length; i++) {
                            var index = (self.pageindex() - 1) * self.pagesize() + (i + 1);
                            var model = list[i];
                            var dataModel = new DataModel(index, model);
                            arr.push(dataModel);
                        }
                    }
                    self.data(arr);
                    //self.initPage();
                    $("#page").pager({
                        pagenumber: responseData.PageIndex, pagecount: responseData.TotalIndex, totalcount: responseData.TotalCount, buttonClickCallback: function (pageclickednumber) {
                            //单击加载
                            self.goPage(pageclickednumber);
                        }
                    });
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                self.initAlert(Translate("web_Content_Js_pagelist_ViewModel_initData_message_error"), xmlHttpRequest.readyState + xmlHttpRequest.status + xmlHttpRequest.responseText);
            }
        });
    };
    //初始化分页
    self.initPage = function () {
        var arr = [];
        var totalIndex = self.totalindex();
        var pageIndex = self.pageindex();
        var first = new PageModel(1, 1, 1 == pageIndex, self);
        var first1 = new PageModel("..", pageIndex - 3, false, self);
        var last = new PageModel(totalIndex, totalIndex, totalIndex == pageIndex, self);
        var last1 = new PageModel("..", pageIndex + 4, false, self);
        var model;
        if (totalIndex <= 10) {
            for (var i = 1; i <= totalIndex; i++) {
                model = new PageModel(i, i, pageIndex == i, self);
                arr.push(model);
            }
        } else {
            if (pageIndex <= 6) {
                for (var i = 1; i < 9; i++) {
                    model = new PageModel(i, i, pageIndex == i, self);
                    arr.push(model);
                }
                arr.push(last1);
                arr.push(last);
            } else if (pageIndex > 6 && pageIndex <= (totalIndex - 6)) {
                arr.push(first);
                arr.push(first1);
                for (var i = pageIndex - 2; i < pageIndex + 4; i++) {
                    model = new PageModel(i, i, pageIndex == i, self);
                    arr.push(model);
                }
                arr.push(last1);
                arr.push(last);
            } else {
                arr.push(first);
                arr.push(first1);
                for (var i = totalIndex - 7; i < totalIndex + 1; i++) {
                    model = new PageModel(i, i, pageIndex == i, self);
                    arr.push(model);
                }
            }
        }
        self.page(arr);
    };
    //跳到指定页
    self.goPage = function (i) {
        var pageIndex = self.pageindex();
        return new PageModel(i, i, i == pageIndex, self).goPage();
    };
    //上一页
    self.pre = function () {
        var pageIndex = self.pageindex();
        if (pageIndex <= 1) {
            self.initAlert(Translate("web_Content_Js_pagelist_ViewModel_pre_message_title"), Translate("web_Content_Js_pagelist_ViewModel_pre_message_content"));
        } else {
            pageIndex--;
            self.goPage(pageIndex);
        }
    };
    //下一页
    self.next = function () {
        var totalIndex = self.totalindex();
        var pageIndex = self.pageindex();
        if (pageIndex < totalIndex) {
            pageIndex++;
            self.goPage(pageIndex);
        } else {
            self.initAlert(Translate("web_Content_Js_pagelist_ViewModel_next_message_title"), Translate("web_Content_Js_pagelist_ViewModel_next_message_content"));
        }
    };
    self.initAlert = function (title, content) {
        globalPromptBox.showGeneralMassage(1, content, 2000, false);
    };
};