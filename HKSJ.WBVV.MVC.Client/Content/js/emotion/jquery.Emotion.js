/*
author:djy
date:2015.11.23
注册即可：
$("#face").faceEmotion({
        id: "emotion", callback: function (imgTitle) {
            //单击表情的回调处理
        }
});
*/
var emotions = new Array();//表情数组 key分类名称 值为分类的数组 数组中每个对象 
var categorys = new Array();// 分组
var emotionsHt = new Hashtable();//建为[dbkey] 值为icon
var emotionsNameHt = new Hashtable();//建为[name] 值为dbkey
function initFace() {
    //分类数据
    var cateData = [{
        cateId: "d",//分类标识
        cateName: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_cateDate_cateName"),//分类名称
        cateDir: rootPath + "/Content/images/face/default/",//分类的文件夹路径
        folder: "bigimg"//表情文件夹        
    }];
    //表情数据
    var data = [{
        cateId: "d",//分类标识
        //表情数据
        data: [
            //dbkey:表情代码 name:表情名称        exname:表情扩展名
            { dbkey: "ts", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_tanshou"), exname: ".png" },
            { dbkey: "sx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_shangxin"), exname: ".png" },
            { dbkey: "sj", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_shuijiao"), exname: ".png" },
            { dbkey: "sh", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_shihua"), exname: ".png" },
            { dbkey: "s", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_se"), exname: ".png" },
            { dbkey: "qx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_qiexi"), exname: ".png" },
            { dbkey: "qqlx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_qiqiaoliuxie"), exname: ".png" },
            { dbkey: "qq", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_qinqin"), exname: ".png" },
            { dbkey: "pz", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_piezui"), exname: ".png" },
            { dbkey: "ng", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_nanguo"), exname: ".png" },
            { dbkey: "myx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_miyanjing"), exname: ".png" },
            { dbkey: "ms", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_miaoshi"), exname: ".png" },
            { dbkey: "lbx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_liubixie"), exname: ".png" },
            { dbkey: "kq", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_kuqi"), exname: ".png" },
            { dbkey: "kl", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_keling"), exname: ".png" },
            { dbkey: "ka", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_keai"), exname: ".png" },
            { dbkey: "k", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_ku"), exname: ".png" },
            { dbkey: "jy", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_jingya"), exname: ".png" },
            { dbkey: "jm", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_jianmeng"), exname: ".png" },
            { dbkey: "jk", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_jingkong"), exname: ".png" },
            { dbkey: "g", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_jiong"), exname: ".png" },//
            { dbkey: "jd", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_jingdai"), exname: ".png" },
            { dbkey: "j", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_jing"), exname: ".png" },
            { dbkey: "hx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_haixiu"), exname: ".png" },
            { dbkey: "hd", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_huoda"), exname: ".png" },
            { dbkey: "h", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_han"), exname: ".png" },
            { dbkey: "dy", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_deyi"), exname: ".png" },
            { dbkey: "dh", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_dahan"), exname: ".png" },
            { dbkey: "bz", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_bizui"), exname: ".png" },
            { dbkey: "by", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_biyan"), exname: ".png" },
            { dbkey: "bs", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_bishi"), exname: ".png" },
            { dbkey: "zy", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_ziya"), exname: ".png" },
            { dbkey: "zx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_jianxiao"), exname: ".png" },
            { dbkey: "y", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_yun"), exname: ".png" },
            { dbkey: "x", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_xiao"), exname: ".png" },
            { dbkey: "wa", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_wuyan"), exname: ".png" },//
            { dbkey: "wy", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_wuyu"), exname: ".png" },
            { dbkey: "wx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_weixiao"), exname: ".png" },
            { dbkey: "wq", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_weiqu"), exname: ".png" },
            { dbkey: "wn", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_wulai"), exname: ".png" },
            { dbkey: "tu", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_tuxie"), exname: ".png" },//
            { dbkey: "tx", name: Translate("web_Content_Js_emotion_jqueryEmotion_initFace_data_touxiao"), exname: ".png" }
        ]
    }];
    //组织数据
    for (var i in cateData) {
        categorys.push(cateData[i].cateName);
        for (var m = 0; m < data.length; m++) {
            if (data[m].cateId == cateData[i].cateId) {
                for (var j = 0; j < data[m].data.length; j++) {
                    var url = cateData[i].cateDir + cateData[i].folder + "/" + data[m].data[j].dbkey.replace("[", "").replace("]", "") + data[m].data[j].exname;
                    //var dbkey = "[" + cateData[i].cateId + data[m].data[j].dbkey + "]";
                    var dbkey = "[" + data[m].data[j].dbkey + "]";
                    emotionsHt.put(dbkey, url);
                    emotionsNameHt.put(data[m].data[j].name, dbkey);

                    data[m].data[j].url = url;
                    data[m].data[j].dbkey = dbkey;
                }
                emotions[cateData[i].cateName] = data[m].data;
            }
        }
    }
}
//初始化数据
initFace();

//数据库读出来显示
function analyticEmotion(s, w, h) {
    if (s != null && s != undefined) {
        //标签转义
        s = s.replace(/\</g, '&lt;');
        s = s.replace(/\>/g, '&gt;');
        if (typeof (s) != "undefined") {
            var sArr = s.match(/\[.*?\]/g);
            if (sArr != null && sArr.length > 0) {
                for (var i = 0; i < sArr.length; i++) {
                    if (emotionsHt.containsKey(sArr[i])) {
                        var reStr = "<img src=\"" + emotionsHt.get(sArr[i]) + "\" " + (w ? "width='" + w + "'" : "") + " " + (h ? "height='" + h + "'" : "") + "/>";
                        s = s.replace(sArr[i], reStr);
                    }
                }
            }
        }
        //简单的格式控制
        s = s.replace(/\n/g, '<br/>');
    }
    return s;
}
//存储数据库
function storeEmotion(s) {
    if (s != null && s != undefined) {
        if (typeof (s) != "undefined") {
            var sArr = s.match(/\[.*?\]/g);
            if (sArr != null && sArr.length > 0) {
                for (var i = 0; i < sArr.length; i++) {
                    if (emotionsNameHt.containsKey(sArr[i])) {
                        s = s.replace(sArr[i], emotionsNameHt.get(sArr[i]));
                    }
                }
            }
        }
    }
    return s;
}
(function ($) {
    //定义组件的默认值
    var defaults = {
        id: 'emotion',                            //赋值对象id 如input textarea
        preview: true,                            //是否预览表情,默认为true  
        showCategory: true,                       //是否显示表情分类
        isPager: true,                            //表情是否显示分页
        callback: null                            //单击图标后的回掉函数
    }
    //插件的一个实例
    $.fn.faceEmotion = function (options) {
        var self = this;
        var cat_current;//当前分类名称
        var cat_page;//当前分类页索引
        var rowCount = 12;//每行表情个数
        var pageTotal = 72;//每屏最多表情数       
        var option = $.extend(defaults, options);
        //赋值对象
        var target = $("#" + option.id).length == 0 ? $("." + option.id) : $("#" + option.id);
        if (target.length <= 0) {
            //alert('缺少表情赋值对象。');
            return false;
        }
        //单击表情图标显示表情框
        $(this).click(function (event) {
            event.stopPropagation();
            var eTop = target.offset().top + target.height() + 15 + 33;
            var eLeft = target.offset().left - 1;
            if ($('#emotions .categorys')[0]) {
                $('#emotions').css({ top: eTop, left: eLeft });
                $('#emotions').toggle();//显示和隐藏切换
                return;
            }
            $('body').append('<div id="emotions"></div>');
            $('#emotions').css({ top: eTop, left: eLeft });
            $('#emotions').html(Translate('web_Content_Js_emotion_jqueryEmotion_fnfaceEmotion_zhengzaijz'));
            $('#emotions').click(function (event) {
                event.stopPropagation();
            });
            //初始化布局
            var html = '';
            if (option.showCategory) {
                html += '<div style="float:right" id="cateContainerId"><a href="javascript:void(0);" id="prev">&laquo;</a><a href="javascript:void(0);" id="next">&raquo;</a></div>';
                html += '<div class="categorys"></div>';
            }
            if (option.preview)
                html += '<div class="preview"><img width="102" height="96" /><br/><span></span></div>';
            html += '<div class="container"></div>';

            if (option.isPager)
                html += '<div class="page"></div>';
            $('#emotions').html(html);

            if (option.showCategory) {
                //上一个分类
                $('#emotions #prev').click(function () {
                    showCategorys(cat_page - 1);
                });
                //下一个分类
                $('#emotions #next').click(function () {
                    showCategorys(cat_page + 1);
                });
                //显示分类
                showCategorys();
            }
            //显示表情
            showEmotions();
        });
        //单击文档隐藏
        $(document).click(function (e) {
            $('#emotions').hide();
            $('#emotions').remove();
        });
        //插入表情
        $.fn.insertText = function (text) {
            this.each(function () {
                if (this.tagName !== 'INPUT' && this.tagName !== 'TEXTAREA') { return; }
                if (document.selection) {
                    this.focus();
                    var cr = document.selection.createRange();
                    cr.text = text;
                    cr.collapse();
                    cr.select();
                } else if (this.selectionStart || this.selectionStart == '0') {
                    try {
                    var
					start = this.selectionStart,
					end = this.selectionEnd;
                    this.value = this.value.substring(0, start) + text + this.value.substring(end, this.value.length);
                    this.selectionStart = this.selectionEnd = start + text.length;
                        //处理IE11光标问题12-21
                    var mvlength = (this.value.substring(0, start) + text).length;
                    var textRange = this.createTextRange();
                    textRange.moveStart('character', mvlength);
                    textRange.collapse();
                    textRange.select();
                    } catch (e) {
                        //console.log(e.message);
                    }
                } else {
                    this.value += text;
                }
                //聚焦
                this.focus();
            });
            return this;
        }
        function showCategorys() {
            var page = arguments[0] ? arguments[0] : 0;
            if (page < 0 || page >= categorys.length / 5) {
                return;
            }
            var cateLen = categorys.length;
            if (cateLen > 1)
                $("#cateContainerId").show();
            else
                $("#cateContainerId").hide();

            $('#emotions .categorys').html('');
            cat_page = page;
            for (var i = page * 5; i < (page + 1) * 5 && i < categorys.length; ++i) {
                $('#emotions .categorys').append($('<a href="javascript:void(0);">' + categorys[i] + '</a>'));
            }
            $('#emotions .categorys a').click(function () {
                showEmotions($(this).text());
            });
            $('#emotions .categorys a').each(function () {
                if ($(this).text() == cat_current) {
                    $(this).addClass('current');
                }
            });
        }
        function showEmotions() {
            var category = arguments[0] ? arguments[0] : Translate('web_Content_Js_emotion_jqueryEmotion_showEmotions_moren');
            var page = arguments[1] ? arguments[1] - 1 : 0;
            $('#emotions .preview').hide();
            $('#emotions .container').html('');
            $('#emotions .page').html('');
            cat_current = category;

            //填充表情
            var row = 0, col = 0, pagesize = pageTotal;
            for (var i = page * pagesize; i < (page + 1) * pagesize && i < emotions[category].length; ++i) {
                row = (i + 1) % rowCount == 0 ? parseInt((i + 1) / rowCount, 10) : parseInt((i + 1) / rowCount, 10) + 1;
                col = ((i + rowCount) % rowCount) + 1;
                var addName = emotions[category][i].name;
                $('#emotions .container').append($('<a href="javascript:void(0);" title="' + addName.replace("[", "").replace("]", "") + '" ttl="' + addName + '"><img row="' + row + '" col="' + col + '"  src="' + emotions[category][i].url + '" alt="' + emotions[category][i].name.replace("[", "").replace("]", "") + '" /></a>'));//height=\"22\" width=\"22\"
            }
            //单击插入表情到目标框
            $('#emotions .container a').click(function () {
                try {
                    var insText = $(this).attr('ttl');
                    var maxLength = $(target).attr("maxlength");
                    if (maxLength && ($.trim($(target).val()).length + insText.length) <= maxLength) {
                    target.insertText(insText);
                    }
                    target.trigger("change");//触发事件
                    $('#emotions').remove();
                    if (option.callback) option.callback(insText, target);
                } catch (e) {
                    //console.log(e.message);
                }
            });
            //添加分页
            var pageCount = parseInt(emotions[category].length / pageTotal + 1, 10);
            if (pageCount > 1) {
                for (var i = 1; i <= pageCount; ++i) {
                    $('#emotions .page').append($('<a href="javascript:void(0);"' + (i == page + 1 ? ' class="current"' : '') + '>' + i + '</a>'));
                }
            }
            //单击分页
            $('#emotions .page a').click(function () {
                showEmotions(category, $(this).text());
            });
            //分类
            $('#emotions .categorys a.current').removeClass('current');
            $('#emotions .categorys a').each(function () {
                if ($(this).text() == category) {
                    $(this).addClass('current');
                }
            });
            //显示预览框
            if (option.preview)
                showPreview();
        }
        //显示预览表情
        function showPreview() {
            $('#emotions .container a img').mouseover(function (e) {
                var container = $('#emotions .container');
                var preview = $('#emotions .preview');
                var row = $(this).attr("row");
                var col = $(this).attr("col");
                // console.log("row:" + row + " col:" + col);               
                if (col > parseInt(rowCount / 2, 10))
                    preview.attr("style", "left: 12px;"); //左边显示                            
                else
                    preview.attr("style", "right:14px;");//右边显示
                //设置数据
                preview.find("img").attr("src", $(this).attr("src")).attr("alt", $(this).attr("alt"));
                preview.find("span").text($(this).attr("alt"));
                //显示
                if (preview.is(":hidden")) {
                    preview.show();
                }
            });
        }
    }
})(jQuery);
