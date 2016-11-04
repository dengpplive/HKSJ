//异步或同步加载
function getAjax(obj) {
    var data = undefined;
    try {
        $.ajax({
            url: obj.url,
            type: obj.type,
            data: obj.data,
            async: obj.async,
            success: function (o) {
                data = o;
            },
            dataType: obj.dataType
        });
    } catch (e) {

    }
    return data;
}

//设置分页栏
function getSetPager(divId, page, rows, total, showTotal) {
    var pagerTotal = total % rows == 0 ? parseInt(total / rows) : parseInt(total / rows) + 1;
    var div = "";
    div += "<div class=\"span6\">";
    div += "      <div class=\"dataTables_info\" >第" + page + "页/共" + pagerTotal + "页 共" + total + "条数据</div>";
    div += "</div>";
    div += "<div class=\"span6\">";
    div += "      <div class=\"dataTables_paginate paging_bootstrap pagination\">";
    div += "      <ul>";
    if (page > 1) {
        div += "<li><a style=\"cursor:pointer;\" onclick=\"getPager(" + (page - 1) + "," + rows + "," + showTotal + ")\"><span class=\"hidden-480\">上一页</span></a></li>";
    }
    if (pagerTotal>1) {
        var obj = getPagerObj(page, pagerTotal);
        if (obj[obj.length - 1] == page & page < pagerTotal) {
            obj = getPagerObj((page + 1), pagerTotal);
        }
        $(obj).each(function (i, v) {
            if (i == 0 && (parseInt(v) - 1) > 0) {
                div += "<li><a style=\"cursor:pointer;\" onclick=\"getPager(" + (v - 2) + "," + rows + "," + showTotal + ")\">...</a></li>";
                if ((parseInt(v) - 1)==page) {
                    div += "<li><a style=\"cursor:pointer;color:red;\" onclick=\"getPager(" + page + "," + rows + "," + showTotal + ")\">" + page + "</a></li>";
                } else {
                    div += "<li><a style=\"cursor:pointer;\" onclick=\"getPager(" +( v-1) + "," + rows + "," + showTotal + ")\">" + (v-1) + "</a></li>";
                }
            }
            if (v == page) {
                div += "<li class=\"active\" ><a style=\"color:red;\">" + v + "</a></li>";
            }
            else if (i == (obj.length - 1) && (parseInt(v) + 1) < pagerTotal) {
                div += "<li><a style=\"cursor:pointer;style=\"color:red;\" onclick=\"getPager(" + v + "," + rows + "," + showTotal + ")\">"+v+"</a></li>";
                div += "<li><a style=\"cursor:pointer;style=\"color:red;\" onclick=\"getPager(" + (parseInt(v) + 1) + "," + rows + "," + showTotal + ")\">...</a></li>";
            }
            else {
                div += "<li><a style=\"cursor:pointer\" onclick=\"getPager(" + v + "," + rows + "," + showTotal + ")\">" + v + "</a></li>";
            }
        });
    }
    if (page != pagerTotal && pagerTotal>1) {
        div += "<li class=\"next\"><a style=\"cursor:pointer\" onclick=\"getPager(" + (page + 1) + "," + rows + "," + showTotal + ")\"><span class=\"hidden-480\">下一页</span></a></li>";
    }
    div += "      </ul>";
    div += "      </div>";
    div += "</div>";
    $("#" + divId).html(div);
}
function getPagerObj(page, pagerTotal) {
    var obj = getArray(pagerTotal);
    var array;
    var istrue = false;
    $(obj).each(function (i, o) {
        array = o.n.split(',');
        for (var i = 0; i < array.length; i++) {
            if (page == array[i]) {
                istrue = true;
                break;
            }
        }
        if (istrue) {
            return false;
        }
    });
    return array;
}

function getArray(pagerTotal) {
    var pagerArray = [];
    var num = "";
    var n = 1;
    for (var i = 1; i <= pagerTotal; i++) {
        if (showTotal == n) {
            num += i;
            pagerArray.push({ n: num });
            num = "";
            n = 0;
        }
        else if (i == pagerTotal && n < showTotal) {
            num += i;
            pagerArray.push({ n: num });
        }
        else {
            num += i + ",";
        }
        n++;
    }
    return pagerArray;
}
//加载图片
function getLoadImg(api, key, imgId) {
    $.get(api + "Video/ImageURL?key=" + key, function (src) {
        $("#"+imgId).attr("src", src);
    });
}

//输入数字类型
function num(obj) {
    var reg = new RegExp("^[0-9]*$");
    if ($(obj).val() != "") {
        if (!reg.test($(obj).val())) {
            alert("请输入数字!");
            $(obj).val("").focus();
        }
        else if ($(obj).val().length>9) {
            alert("您输入数字太大了!");
            var v = $(obj).val().substring(0, 9);
            $(obj).val(v);
            $(obj).focus();
        }
    }
}

//验证输入最大字符串个数
function getMaxStr(obj, num) {
    var str = $(obj).val();
    if (str.length > num) {
        var v = str.substring(0, num);
        $(obj).val(v);
    }
}

function getStr(str, num) {
    if (str.length > num) {
        return "<span title='" + str + "'>" + str.substring(0, num) + "...</span>";
    }
    return str;
}

    function textOver(obj) {
        $(obj).attr("style", "width:50px; background-color:#ccc;");
        $(obj).select();
    }

    function textOut(obj) {
        $(obj).attr("style", "width:50px;");
    }