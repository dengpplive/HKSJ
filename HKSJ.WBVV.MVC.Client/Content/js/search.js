
var pageIndex = 1, pageSize = 40, totalPages = 1, sortName = 0;

$(function () {
    var searchKey = GetQueryString("searchKey");

    document.title = Translate('web_Content_Js_search_documentitle') + searchKey + " " + companyTitle;

    sortName = (GetQueryString("sortName") == "" || GetQueryString("sortName") == null) ? 0 : parseInt(GetQueryString("sortName"));
    if (sortName == 1)
        $('#btnTime').attr("checked", true);
    else if(sortName==2)
        $('#btnHot').attr("checked", true);



    var urlTitle = api + "Video/SearchVideoByTop?searchKey=" + escape(searchKey) + "&v=" + Math.random();
    $.getJSON(urlTitle, function (data) {
        wmtitle.doProfile(data.topdata);
    }
    );


});


//--------------------------------begin 最新、最热-------------------------//
function doOrderList(type) {
    var searchKey = $('#txtSearch').val();
    window.location = 'Search?searchKey=' + escape(searchKey) + "&pageSize=" + pageSize + "&sortName=" + type + "&v=" + Math.random();
}
//--------------------------------end 最新、最热-------------------------//


//--------------------------------------begin 头部简介--------------------------------------//
var wm_title = function () {
    var self = this;

    //列表数组
    self.listData = ko.observableArray();


    //数据赋值方法
    self.doProfile = function (topdata) {

        self.listData(topdata);
    }

    self.goPlayer = function (Id) {
        return "Play/Index?videoId=" + Id + "&v=" + Math.random();
    }

}

var wmtitle = new wm_title();
//绑定ko的数据
ko.applyBindings(wmtitle, document.getElementById('divTitle'));

//--------------------------------------end 头部简介--------------------------------------//



//--------------------------------------begin 查询索引--------------------------------------//

var wm_index = function () {
    var self = this;
    //图片
    self.searchText = ko.observable();
    //标题
    self.quantity = ko.observable();


    //数据赋值方法
    self.doQuantity = function (page) {

        self.searchText($('#txtSearch').val());
        self.quantity(page.totalNum);
    }


}

var wmindex = new wm_index();
//绑定ko的数据
ko.applyBindings(wmindex, document.getElementById('divQuantity'));


//--------------------------------------end 查询索引--------------------------------------//


//--------------------------------------begin 查询列表--------------------------------------//

var wm_list = function () {
    var self = this;
    //列表数组
    self.listData = ko.observableArray();


    //数据赋值方法
    self.doListData = function (listdata) {

        self.listData(listdata);
    };
    self.CheckVisible = ko.observable(false);
    self.SearchKey = ko.observable();
    //分页数据
    self.loadCollectsData = function(pageIndex) {
        var searchKey = GetQueryString("searchKey");
        self.SearchKey(searchKey);
        sortName = (GetQueryString("sortName") == "" || GetQueryString("sortName") == null) ? 0 : parseInt(GetQueryString("sortName"));
        var url = api + "Video/GetSearchListByPage?searchKey=" + escape(searchKey) + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&sortName=" + sortName + "&v=" + Math.random();
        $.getJSON(url, function (data) {
            if (data.msg != "success") {
                self.loadYesRecVideos();
                self.CheckVisible(true);
                wmindex.quantity(0);
                return;
            }
            wmindex.doQuantity(data.page);
            self.CheckVisible(data.listdata && data.listdata.length == 0);
            self.doListData(data.listdata);
            totalPages = data.page.totalPage <= 0 ? 1 : data.page.totalPage;
            if (data.listdata && data.listdata.length == 0) {
                self.loadYesRecVideos();
            }
           
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
                pagenumber: pageIndex,
                pagecount: totalPages,
                totalcount: data.page.totalNum,
                buttonClickCallback: function(pageclickednumber) {
                    //单击加载
                    self.loadCollectsData(pageclickednumber);
                }
            });
        });
    };
    //昨日推荐--播放量最多的12个视频
    self.yesterdayRecVideos = ko.observableArray();

    self.loadYesRecVideos = function () {
        var num = 12;
        var url = api + "Video/GetYesRecVideos?num="+num+"&v="+Math.random();
        $.getJSON(url, function (data) {
            
            self.yesterdayRecVideos(data.Data);
        });
    };


    self.goPlayer = function(Id) {
        return "Play/Index?videoId=" + Id + "&v=" + Math.random();
    };

    self.getTitle = function (title) {
        //<span style="font-style:normal;color:#cc0000;">杀破狼</span>217
        //美少<span style="font-style:normal;color:#cc0000;">女</span>
        while (true) {
            if (title.indexOf("<span style=\"font-style:normal;color:#cc0000;\">") < 0 && title.indexOf("</span>") < 0)
                break;
            if (title.indexOf("<span style=\"font-style:normal;color:#cc0000;\">") >= 0)
                title = title.replace("<span style=\"font-style:normal;color:#cc0000;\">", "");
            if (title.indexOf("</span>") >= 0)
                title = title.replace("</span>", "");
        }

        return title;
    }
}

var wmlist = new wm_list();
wmlist.loadCollectsData(pageIndex);
//绑定ko的数据
ko.applyBindings(wmlist, document.getElementById('divList'));


//--------------------------------------end 查询列表--------------------------------------//


//---------------------------------------执行---------------------------------------//
//$(function () {
//    wmtitle.doProfile();
//    wmindex.doQuantity();
//    wmlist.doListData();

//});
//---------------------------------------执行---------------------------------------//











