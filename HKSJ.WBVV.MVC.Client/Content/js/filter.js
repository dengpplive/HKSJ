
var pageIndex = 1, pageSize = 40, totalPages = 1, curId = -1, filter = "", sortName = "";

$(function () {
    //var pageIndex = (GetQueryString("pageIndex") == "" || GetQueryString("pageIndex") == null) ? 1 : parseInt(GetQueryString("pageIndex"));//当前页码
    curId = GetQueryString("curId");
    var parentId = GetQueryString("parentId");//类型
    var noteId = GetQueryString("noteId");//子项

    filter = GetQueryString("filter");
    sortName = (GetQueryString("sortName") == "" || GetQueryString("sortName") == null) ? "hot" : GetQueryString("sortName");//排序
    if (sortName == "hot")
        $('#CheckboxGroup_hot').attr("checked", true);
    else
        $('#CheckboxGroup_time').attr("checked", true);

    if (filter == null) {
        if (noteId == null) {
            filter = "gc" + curId + "cr";
        } else {
            filter = "gc" + curId + "c" + noteId + "r";
        }
    }

    var urlFilter = api + "Dictionary/GetDictionaryViewList?categoryId=" + curId + "&filter=" + filter + "&v=" + Math.random();

    $.getJSON(urlFilter, function (data) {
        for (var i = 0; i < data.length; i++) {
            var modle = {
                Id: data[i].Id,
                Key: data[i].Key,
                GroupType: data[i].GroupType,
                DictionaryItems: []
            };
            var ischeck = true;
            for (var j = 0; j < data[i].DictionaryItems.length; j++) {
                if (data[i].DictionaryItems[j].IsCheck)
                    ischeck = false;
            }
            modle.DictionaryItems.push({
                Id: "", Name: Translate("web_Content_Js_filter_quanbu"), DictionaryId: data[i].Id, GroupType: data[i].GroupType, IsCheck: ischeck
            });
            for (var j = 0; j < data[i].DictionaryItems.length; j++) {
                var DictionaryItem = data[i].DictionaryItems[j];
                if (i == 0) {
                    if (data[i].Id == parentId && DictionaryItem.Id == noteId) {
                        filter = "g" + data[i].GroupType + data[i].Id + "c" + DictionaryItem.Id + "r";
                        DictionaryItem.IsCheck = true;
                        modle.DictionaryItems[0].IsCheck = false;
                    }
                }
                modle.DictionaryItems.push(DictionaryItem);
            }
            wmfilter.filterData.push(modle);
        }
        wmlist.loadCollectsData(pageIndex);
    });



    //var urlList = api + "Video/GetVideoViewByFilterList?categoryId=" + curId + "&filter=" + filter + "&pageIndex=" + pageIndex + "&sortName=" + sortName + "&v=" + Math.random();
    //$.getJSON(urlList, function (data) {
    //    wmlist.doListData(data.data);
    //    initiPaging(data);
    //});


});


//----------------------------------------------------begin 筛选条件--------------------------------------------------------//
var wm_filter = function () {
    var self = this;

    //列表数组
    self.filterData = ko.observableArray();

    //数据赋值方法
    self.doFilterData = function (topdata) {

        self.filterData(topdata);
    }

    self.doCheck = function (Id, DictionaryId, groupType) {

        for (var i = 0; i < self.filterData().length; i++) {

            for (var j = 0; j < self.filterData()[i].DictionaryItems.length; j++) {
                if (self.filterData()[i].Id == DictionaryId && self.filterData()[i].DictionaryItems[j].Id == Id && self.filterData()[i].DictionaryItems[j].GroupType == groupType) {
                    self.filterData()[i].DictionaryItems[j].IsCheck = true;
                } else if (self.filterData()[i].Id == DictionaryId && self.filterData()[i].DictionaryItems[j].Id != Id && self.filterData()[i].DictionaryItems[j].GroupType == groupType) {
                    self.filterData()[i].DictionaryItems[j].IsCheck = false;
                }
            }

        }
        var filter = "";
        for (var i = 0; i < self.filterData().length; i++) {
            if (i == 0 || i == 1)
                filter += "g" + self.filterData()[i].GroupType;
            for (var j = 0; j < self.filterData()[i].DictionaryItems.length; j++) {
                if (self.filterData()[i].DictionaryItems[j].IsCheck)
                    filter += self.filterData()[i].DictionaryItems[j].DictionaryId + "c" + self.filterData()[i].DictionaryItems[j].Id + "r";
            }
        }
        //filter得到：gc2c3rgd2c4r3c56r4c66r
        var curId = GetQueryString("curId");
        var sortName = (GetQueryString("sortName") == "" || GetQueryString("sortName") == null) ? "hot" : GetQueryString("sortName");//排序
        window.location = rootPath + "/Filter?curId=" + curId + "&filter=" + filter + "&sortName=" + sortName + "&v=" + Math.random();

    }

}

var wmfilter = new wm_filter();
//绑定ko的数据
ko.applyBindings(wmfilter, document.getElementById('divFilter'));

//----------------------------------------------------end 筛选条件--------------------------------------------------------//



//--------------------------------begin 最新、最热-------------------------//
function doOrderList(type) {
    var curId = GetQueryString("curId");
    //var filter = GetQueryString("filter");
    window.location = rootPath + "/Filter?curId=" + curId + "&filter=" + filter + "&sortName=" + type + "&v=" + Math.random();
}
//--------------------------------end 最新、最热-------------------------//




//----------------------------------------------------begin 查询列表--------------------------------------------------------//
var wm_list = function () {
    var self = this;

    //列表数组
    self.listData = ko.observableArray();


    //数据赋值方法
    self.doListData = function (listdata) {

        self.listData(listdata);
    }

    self.goPlayer = function (Id) {
        return rootPath + "/Play?videoId=" + Id + "&v=" + Math.random();
    }
    self.CheckVisible = ko.observable(false);
    //分页数据
    self.loadCollectsData = function (pageIndex) {
        var url = api + "Video/GetFilterVideo?filter=" + filter + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&sortName=" + sortName + "&v=" + Math.random();
        $.getJSON(url, function (data) {
            self.CheckVisible(data.Data && data.Data.length == 0);
            //没有数据隐藏筛选条件--2015.12.2
            if (!self.CheckVisible()) $("#filteropt").show(); else $("#filteropt").hide();

            self.doListData(data.Data);
            totalPages = data.TotalIndex;
            //分页
            //$.jqPaginator('#page', {
            //    totalPages: totalPages,
            //    currentPage: pageIndex,
            //    onPageChange: function (num, type) {
            //        pageIndex = num;
            //        if (type == 'change') {
            //            self.loadCollectsData(num);
            //        }
            //    }
            //});
            $("#page").pager({
                pagenumber: pageIndex, pagecount: totalPages, totalcount: data.TotalCount, buttonClickCallback: function (pageclickednumber) {
                    //单击加载
                    self.loadCollectsData(pageclickednumber);
                }
            });
        });
    }

}

var wmlist = new wm_list();
//绑定ko的数据
ko.applyBindings(wmlist, document.getElementById('divList'));

//----------------------------------------------------end 查询列表--------------------------------------------------------//



