curCaterogyId = -1;//特殊分类 -1首页 -2专辑
ko.bindingHandlers.initHeader = {
    //初始化数据
    init: function (element, valueAccess, allBingdingsAccess, viewModel) {
        var value = valueAccess();
        var allBinding = allBingdingsAccess();
        //导航菜单
        navMenu(viewModel);
    }
};
//导航菜单
function navMenu(viewModel) {
    curCaterogyId = getURLParam("curId", location.href);
    if (curCaterogyId == "") curCaterogyId = -1;
    var url = api + "Category/GetMenuViewList?v=" + Math.random();
   // console.log("导航菜单url:" + url);
    $.getJSON(url, function (data) {
        viewModel.firstCaterogy(data);
       // console.log("导航菜单数据:");
        // console.log(data);
        viewModel.applyCss(curCaterogyId);
        for (var i in data) {
            if (data[i].ParentCategory.Id == curCaterogyId) {
                viewModel.applyCss(data[i].ParentCategory.Id);
               // console.log("id:" + data[i].ParentCategory.Id);
            }
        }
    });
}
var vm_Header = function () {
    var self = this;
    //首页菜单一级分类
    self.firstCaterogy = ko.observableArray([]);
    //当前选中的分类样式
    self.applyCss = ko.observable(1);
    //跳转页面
    self.goPage = function (id) {
        url = rootPath + "/Home/Index?curId=" + id;
        //console.log("url：" + url);
        return url;
    }
}
ko.applyBindings(new vm_Header(), document.getElementById("header"));




