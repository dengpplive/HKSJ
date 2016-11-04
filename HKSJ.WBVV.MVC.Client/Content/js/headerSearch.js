function enterSeach(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : ""); //兼容IE和Firefox获得keyBoardEvent对象
    var key = evt.keyCode ? evt.keyCode : evt.which; //兼容IE和Firefox获得keyBoardEvent对象的键值
    if (key == 13) {
        doSearch();
    } else {
        var searchKey = $('#txtSearch').val().trim();
        if (searchKey != null && searchKey != undefined && searchKey != "") {
            //checktitle();
            if (searchKey.length > 15)
                searchKey = searchKey.substr(0, 15);
            loadSearchKeyWords(searchKey);
        }
        else {
            loadhotKeyWord();
        }
    }
};

function checktitle() {
    var title = $("#txtSearch").val().trim();
    if (title.length > 15) {
        $("#txtSearch").val(title.substr(0, 15));
        globalPromptBox.showGeneralMassage(1, Translate("web_Content_Js_headerSearch_checktitle_biaoti"), 2000, false);
    }
}

function doSearch() {
    var searchKey = $('#txtSearch').val().trim();
    //if (searchKey == "" || searchKey == undefined) {
    //    globalPromptBox.showGeneralMassage(1, "请输入查询字符串", 2000, false);
    //    return;
    //}
    if (searchKey.length > 15) {
        var searchKeysub = searchKey.substr(0, 15);
        AddOrUpdateAKeyWord(searchKeysub);
    } else {
        AddOrUpdateAKeyWord(searchKey);
    }
    //window.open('@ServerHelper.RootPath/Search?searchKey=' + escape(searchKey) + "&v=" + Math.random(), "_blank");
    location.href = LR.RootPath + '/Search?searchKey=' + escape(searchKey) + "&v=" + Math.random();
};

//top搜索下拉
$("#seach").on("click",
    function (e) {
        var searchKey = $('#txtSearch').val().trim();
        // $("#showSeach").show();
        if (searchKey == "") {
            loadhotKeyWord();
        }
        $(document).one("click", function () {
            $("#showSeach").hide();
        });
        e.stopPropagation();
    });
$("#showSeach").on("click", function (e) {
    e.stopPropagation();
});

var self = window;
// self.hotKeyWord = ko.observableArray([]);
self.loadhotKeyWord = function () {
    var url = api + "KeyWord/GetHotKeyWords";
    $.ajax(url, {
        type: "get",
        dataType: "json",
        success: function (data) {
            //if (data.Success)
            // self.hotKeyWord([]);
            // self.hotKeyWord(data.Data);

            $("#hotlist").empty();
            var hothtml = createhtml(data.Data);
            $("#hotlist").html(hothtml);

            $("#showSeach").show();
        }
    });
};

function createhtml(data) {
    var hothtml = '';
    $.each(data, function (i, item) {
        i++;
        if (i > 3) {
            hothtml += '<li><div class="seach_01 seach_02"><span>' + i + ' </span></div><a href="#" onclick="javascript:doSearchkeyword(\'' + item.Keyword + '\');">' + item.Keyword + '</a><div class="clear"></div></li>';
        } else {
            hothtml += '<li><div class="seach_01 "><span>' + i + ' </span></div><a href="#" onclick="javascript:doSearchkeyword(\'' + item.Keyword + '\');">' + item.Keyword + '</a><div class="clear"></div></li>';
        }
    });
    return hothtml;
}

self.loadSearchKeyWords = function (searchTxt) {
    var url = api + "KeyWord/GetFilteredKeyword?keyword=" + searchTxt;
    $.ajax(url, {
        type: "get",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                //self.hotKeyWord([]);
                if (data.Data.length == 0) {
                    $("#showSeach").hide();
                } else {
                    $("#hotlist").empty();
                    var hothtml = createhtml(data.Data);
                    $("#hotlist").html(hothtml);
                    $("#showSeach").show();
                    //self.hotKeyWord(data.Data);
                }
            }
        }
    });
};
self.AddOrUpdateAKeyWord = function (searchTxt) {
    var url = api + "KeyWord/AddOrUpdateAKeyWord?keyword=" + searchTxt;
    $.ajax(url, {
        type: "post",
        dataType: "json",
        success: function (data) {
        }
    });
};
self.doSearchkeyword = function (keyword) {
    self.AddOrUpdateAKeyWord(keyword);
    //window.open('@ServerHelper.RootPath/Search?searchKey=' + escape(keyword) + "&v=" + Math.random(), "_blank");
    location.href = LR.RootPath + '/Search?searchKey=' + escape(keyword) + "&v=" + Math.random();
};



//var vmhotKeyWord = new vm_hotKeyWord();
//ko.applyBindings(vmhotKeyWord, document.getElementById("showSeach"));


function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
};

