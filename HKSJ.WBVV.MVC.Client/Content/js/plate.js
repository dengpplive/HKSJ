var plateId = 0;
//首次加载
$(function () {
    plateId = GetQueryString("plateId");//板块ID
    wmlist.Plate();
    wmlist.loadCollectsData();
});

//----------------------------------------------------begin 查询列表--------------------------------------------------------//
var wm_list = function () {
    var self = this;

    //列表数组
    self.listData = ko.observableArray();
    self.plateName = ko.observableArray();

    //数据赋值方法
    self.doListData = function (listdata) {
        self.listData(listdata);
    }
    self.goPlayer = function (Id) {
        window.location = rootPath + "/Play?videoId=" +Id + "&v=" +Math.random();
}
    //分页数据
    self.loadCollectsData = function () {
        var url = api + "Video/GetPlateVideoList?plateId=" + plateId + "&v=" +Math.random();
        $.getJSON(url, function (o) {
            self.doListData(o);
    });
}
    //获取板块名称
    self.Plate = function () {
        var url = api + "Plate/GetPlate?id=" + plateId + "&v=" +Math.random();
        $.post(url, function (o) {
            self.plateName(o.Name);
    });
    }
    }

var wmlist = new wm_list();
//绑定ko的数据
ko.applyBindings(wmlist, document.getElementById('divList'));

//----------------------------------------------------end 查询列表--------------------------------------------------------//
