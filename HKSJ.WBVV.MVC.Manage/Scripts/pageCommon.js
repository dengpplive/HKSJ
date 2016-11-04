//分页
var PageModel = function (text, value, active, viewModel) {
    var self = this;
    self.text = text;
    self.value = value;
    self.active = ko.observable(active);
    self.goPage = function () {
        viewModel.pageindex(self.value);
        viewModel.initData();
    };
};
//数据
var DataModel = function (index, ischeck, model, editUrl, deleteUrl) {
    var self = this;
    self.ischeck = ko.observable(ischeck);
    self.index = index;
    self.model = model;
    self.editUrl = editUrl;
    self.deleteUrl = deleteUrl;
    //修改
    self.updateModel = function () {
        viewModel.initIframe(Translate("admin_Scripts_pageCommon_DataModel_updateModel_editInfo"), self.editUrl);
    };
    //删除
    self.deleteModel = function () {
        self.initConfirm(Translate("admin_Scripts_pageCommon_DataModel_deleteModel_OperationTips"), Translate("admin_Scripts_pageCommon_DataModel_deleteModel_delete"), function () {
            viewModel.operAjax(deleteUrl, null);
        });
    };
};
var ViewModel = function () {
    var self = this;
    self.listUrl = "";
    self.editUrl = "";
    self.deleteUrl = "";
    self.deletesUrl = "";
    self.addUrl = "";
    self.updateUrl = "";
    self.userId = "";
    self.pageindex = ko.observable(1);
    self.pagesize = ko.observable(10);
    self.totalindex = ko.observable(0);
    self.totalcount = ko.observable(0);
    self.data = ko.observableArray([]);
    self.page = ko.observableArray([]);
    self.condtions = ko.observableArray([]);
    self.ordercondtions = ko.observableArray([]);
    self.dataTip == ko.observableArray([]);
    //获取选中的编号
    self.getCheckIds = function () {
        var data = self.data();
        var arr = [];
        for (var i = 0; i < data.length; i++) {
            if (data[i].ischeck()) {
                arr.push(data[i].model.Id);
            }
        }
        return arr;
    };
    //获取数据模型
    self.getModel = function (url) {
        var model = {};
        self.woboajax(url, "GET", null, function (responseModel) {
            if (responseModel) {
                model = responseModel;
            }
        });
        return model;
    };
    //删除选中
    self.deleteModels = function () {
        var ids = self.getCheckIds();
        console.log(ids);
        if (ids && ids.length > 0) {
            self.initConfirm(Translate("admin_Scripts_pageCommon_ViewModel_deleteModels_OperationTips"), Translate("admin_Scripts_pageCommon_ViewModel_deleteModels_delete"), function () {
                //console.log("删除选中" + self.deletesUrl);
                self.operAjax(self.deletesUrl, { ids: ids });
            });
        } else {
            self.initAlert(Translate("admin_Scripts_pageCommon_ViewModel_deleteModels_else_OperationTips"), Translate("admin_Scripts_pageCommon_ViewModel_deleteModels_else_selectDelete"));
        }
    };
    //添加
    self.addModel = function () {
        self.initIframe(Translate("admin_Scripts_pageCommon_ViewModel_addModel_addInfo"), self.editUrl + "?id=0");
    };
    self.woboajax = function (url, type, data, success) {
        $.ajax({
            url: url,
            type: type,
            data: data,
            dataType: "json",
            success: success,
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                self.initAlert(Translate("admin_Scripts_pageCommon_ViewModel_woboajax_error"), xmlHttpRequest.readyState + xmlHttpRequest.status + xmlHttpRequest.responseText);
            }
        });
    };
    self.operAjax = function (url, data) {
        self.woboajax(url, "POST", data, function (responseData) {
            if (responseData.Success) {
                window.location.reload();
            } else {
                self.initAlert(Translate("admin_Scripts_pageCommon_ViewModel_operAjax_woboajax_else_OperationTips"), responseData.ExceptionMessage);
            }
        });
    };
    //跳到指定页
    self.goPage = function (i) {
        var pageIndex = self.pageindex();
        return new PageModel(i, i, i == pageIndex, viewModel).goPage();
    };
    //上一页
    self.pre = function () {
        var pageIndex = self.pageindex();
        if (pageIndex <= 1) {
            self.initAlert(Translate("admin_Scripts_pageCommon_ViewModel_pre_if_OperationTips"), Translate("admin_Scripts_pageCommon_ViewModel_pre_if_firstPage"));
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
            self.initAlert(Translate("admin_Scripts_pageCommon_ViewModel_next_else_OperationTips"), Translate("admin_Scripts_pageCommon_ViewModel_pre_else_lastPage"));
        }
    };
    //初始化数据
    self.initData = function () {
        self.blockUI(".responsive");
        self.woboajax(self.listUrl, "POST", {
            pageindex: self.pageindex(),
            pagesize: self.pagesize(),
            condtions: self.condtions(),
            ordercondtions: self.ordercondtions()
        }, function (responseData) {
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
                        var editUrl = self.editUrl + "?id=" + model.Id;
                        var deleteUrl = self.editUrl + "?id=" + model.Id;
                        var dataModel = new DataModel(index, false, model, editUrl, deleteUrl);
                        arr.push(dataModel);
                    }
                }
                self.dataTip(arr);
                self.data(arr);
                self.initPage();
            }
        });
        self.unblockUI(".responsive");
    };
    //初始化分页
    self.initPage = function () {
        var arr = [];
        var totalIndex = self.totalindex();
        var pageIndex = self.pageindex();
        var first = new PageModel(1, 1, 1 == pageIndex, viewModel);
        var first1 = new PageModel("..", pageIndex - 3, false, viewModel);
        var last = new PageModel(totalIndex, totalIndex, totalIndex == pageIndex, viewModel);
        var last1 = new PageModel("..", pageIndex + 4, false, viewModel);
        var model;
        if (totalIndex <= 10) {
            for (var i = 1; i <= totalIndex; i++) {
                model = new PageModel(i, i, pageIndex == i, viewModel);
                arr.push(model);
            }
        } else {
            if (pageIndex <= 6) {
                for (var i = 1; i < 9; i++) {
                    model = new PageModel(i, i, pageIndex == i, viewModel);
                    arr.push(model);
                }
                arr.push(last1);
                arr.push(last);
            } else if (pageIndex > 6 && pageIndex <= (totalIndex - 6)) {
                arr.push(first);
                arr.push(first1);
                for (var i = pageIndex - 2; i < pageIndex + 4; i++) {
                    model = new PageModel(i, i, pageIndex == i, viewModel);
                    arr.push(model);
                }
                arr.push(last1);
                arr.push(last);
            } else {
                arr.push(first);
                arr.push(first1);
                for (var i = totalIndex - 7; i < totalIndex + 1; i++) {
                    model = new PageModel(i, i, pageIndex == i, viewModel);
                    arr.push(model);
                }
            }
        }
        self.page(arr);
    };
    //初始化提示框
    self.initAlert = function (title, content) {
        $("#alert").modal({backdrop:'static'}).css({ top: 0 });
        ko.applyBindings({
            title: title,
            content: content
        }, document.getElementById("alert"));
    };
    //初始化选择框
    self.initConfirm = function (title, content, confirm) {
        $("#confirm").modal({backdrop:'static'});
        ko.applyBindings({
            title: title,
            content: content,
            confirm: confirm
        }, document.getElementById("confirm"));
    };
    //初始化编辑
    self.initIframe = function (title, editUrl) {
        $("#iframe").modal({ backdrop: 'static' });
        self.blockUI("#form1");
        self.woboajax(editUrl, "GET", null, function (responseData) {
            if (responseData) {
                ko.applyBindings({
                    title: title,
                    model: responseData,
                    save: function (obj) {
                        var model = obj.model;
                        var url = model.Id <= 0 ? viewModel.addUrl : viewModel.updateUrl;
                        if (model.Id <= 0) {
                            model.CreateManageId = self.userId;
                        } else {
                            model.UpdateManageId = self.userId;
                        }
                        self.blockUI("#form1");
                        console.log(model);
                        self.operAjax(url, model);
                        self.unblockUI("#form1");
                    }
                }, document.getElementById("iframe"));
            }
        });
        self.unblockUI("#form1");
    };
    self.blockUI = function (el, centerY) {
        var el = jQuery(el);
        var imageUrl = rootPath + "/Content/bootstrap/image/select2-spinner.gif";
        el.block({
            message: "<img src='" + imageUrl + "'>",
            centerY: centerY != undefined ? centerY : true,
            css: {
                top: '10%',
                border: 'none',
                padding: '2px',
                backgroundColor: 'none'
            },
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.05,
                cursor: 'wait'
            }
        });
    };
    self.unblockUI = function (el) {
        jQuery(el).unblock({
            onUnblock: function () {
                jQuery(el).removeAttr("style");
            }
        });
    };
    self.dataTip = function (o) {
        if (o.length == 0) {
            $("#divTip").attr("style", "display:block;text-align:center;");
        } else {
            $("#divTip").attr("style", "display:none;");
        }
    }
};
var viewModel = new ViewModel();
ko.applyBindings(viewModel, document.getElementById("content"));
//全选
function checkAll(element) {
    for (var i = 0; i < viewModel.data().length; i++) {
        viewModel.data()[i].ischeck($(element).is(':checked'));
    }
}
//排序
function checkSort(element) {
    var obj = $(element);
    var filedName = obj.attr("data-sort");
    var isDesc = obj.attr("data-desc");
    if (isDesc == "true") {
        obj.removeClass("sorting_asc").addClass("sorting_desc");
        obj.attr("data-desc", false);
    } else {
        obj.removeClass("sorting_desc").addClass("sorting_asc");
        obj.attr("data-desc", true);
    }
    var orderModel = {
        FiledName: filedName,
        IsDesc: isDesc
    };
    var ordercondtions = viewModel.ordercondtions();
    if (ordercondtions.length <= 0) {
        viewModel.ordercondtions.push(orderModel);
    }
    for (var i = 0; i < ordercondtions.length; i++) {
        if (ordercondtions[i].FiledName == filedName) {
            viewModel.ordercondtions.remove(ordercondtions[i]);
        }
        viewModel.ordercondtions.push(orderModel);
    }
    viewModel.initData();
}
//扩展方法
Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};
Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};
