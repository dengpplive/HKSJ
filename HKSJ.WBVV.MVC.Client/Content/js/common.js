var companyTitle = Translate('web_Content_Js_Common_companyTitle');

//默认数据
defaultData = {//icon_img/5BVV_move07_03.png
    defaultUserImage: rootPath + "/Content/images/head_img/m_tx_150.jpg",//用户没有上传图像的图片
    defaultSubUserImage: rootPath + "/Content/images/head_img/m_tx_120.jpg",//我的订阅用户没有上传图像的图片
    defaultVideoImage: rootPath + "/Content/images/icon_img/notvideo_01.png",//没有视频图像或者图像不生效显示的图片
    defaultBannerImage: rootPath + "/Content/images/icon_img/Per_bg.jpg",//个人空间的默认背景图片
    defaultCoverImage: rootPath + '/Content/images/icon_img/5bvv_img_mo.png'
};
//全局对话框/弹框/气泡对象
globalPromptBox = {
    showGeneralMassage: function (type, msg, showTime, isRefresh) {
        showTime = parseInt(showTime);
        $('#globalIcon').removeClass();
        if (type == 0)//success
            $('#globalIcon').addClass('ts_01');
        else if (type == 1)//warning
            $('#globalIcon').addClass('ts_02');
        else if (type == 2)//error
            $('#globalIcon').addClass('ts_03');

        $("#globalbBG").show();
        $("#globalMassage").show();
        $('#globalTxtMassage').html(msg);
        setTimeout("$('#globalbBG').hide()", showTime);
        setTimeout("$('#globalMassage').hide()", showTime);
        if (isRefresh)
            setTimeout("location.reload(true)", showTime);
    },
    showPromptMessage: function (title, content, func, parameters, cancel) {
        /*
        参数列表说明:
        title :弹出对话框的标题,标题内容最好在25个字符内,否则会导致显示图片的异常
        text  :弹出对话框的内容,可以使用HTML代码,例如<font color='red'>删除么?</font>,如果直接带入函数,注意转义
        func  :弹出对话框点击确认后执行的函数,需要写全函数的引用,例如add(),如果直接带入函数,注意转义。
        parameters:弹出对话框点击确认后执行的函数的参数，怎么接收怎么传递。
        cancel:弹出对话框是否显示取消按钮,为空或true的话不显示,为false时显示
        */
        $("#globalbPromptBG").show();
        $("#globalbPromptMassage").show();
        $('#globalbPromptTitle').html(title);
        $('#globalbPromptTxtMassage').html(content);

        $('#btnExeFunction').on("click", function () {
            globalPromptBox.hidePromptMessage();
            func(parameters);
            globalPromptBox.clearPromptMessage();
        })


        $("*[name='btnCancelExe']").each(function () {
            $(this).on("click", globalPromptBox.hidePromptMessage);
            if (cancel)
                $(this).hide();
        });

    },
    hidePromptMessage: function () {
        $("#globalbPromptBG").hide();
        $("#globalbPromptMassage").hide();
        $('#btnExeFunction').off("click");
    },
    clearPromptMessage: function () {
        $('#btnExeFunction').off("click");
        $("*[name='btnCancelExe']").each(function () {
            $(this).off("click");
        });
    },
    showBubble: function (id, className, time, text) {
        $.fn.tipBox = function () {
            var $oDiv = $('<div></div');
            var $this = $(this)
            $oDiv.addClass('poptip');
            $oDiv.html('<span class="poptip-arrow poptip-arrow-bottom"><em>◆</em><i>◆</i></span><div class="poptip_fh clearfloat"><a class="t_sk_fuhao_xx" href="javascript:;"></a><span class="t_sk_fuhao"><img src="../Content/images/icon_img/T_sk_fuhao1.png" ></span> <p class="t_sk_tishi">' + text + '</p></div>');
            $this.append($oDiv);
            var $oClose = $this.find('.t_sk_fuhao_xx');
            $oClose.click(function () {
                $oDiv.remove();
            });
            var timer = setTimeout(function () {
                $oDiv.remove();
                clearInterval(timer);
            }, time)
        }
        if (id) {
            $('#' + id).tipBox();
        } else if (className) {
            $('.' + className).tipBox();
        }

    },
    questionMassage: function (msg) {
        $('#globalIcon').removeClass();
        $('#globalIcon').addClass('ts_01');
        $("#globalbBG").show();
        $("#globalMassage").show();
        $('#globalTxtMassage').html(msg);
        setTimeout("$('#globalbBG').hide()", 1000);
        setTimeout("$('#globalMassage').hide()", 1000);
    }
}
//获取url的参数值
function getURLParam(name, url) {
    var strReturn = "";
    var strHref = url.toLowerCase();
    if (strHref.indexOf("?") > -1) {
        var strQueryString = strHref.substr(strHref.indexOf("?") + 1).toLowerCase();
        var aQueryString = strQueryString.split("&");
        for (var iParam = 0; iParam < aQueryString.length; iParam++) {
            if (aQueryString[iParam].indexOf(name.toLowerCase() + "=") > -1) {
                var aParam = aQueryString[iParam].split("=");
                strReturn = aParam[1];
                break;
            }
        }
    }
    return strReturn;
}
//获取浏览空间的用户id
function getRoomBrowserUserId(self) {
    //浏览的用户id
    var browserUserId = $.trim(getURLParam("browserUserId", location.href)).replace("#", "");
    browserUserId = browserUserId == -1 ? self.loginUserId() : browserUserId;
    if (browserUserId == null || browserUserId == "" || isNaN(parseInt(browserUserId, 10)) || (browserUserId <= 0 && browserUserId != -1)) {
        globalPromptBox.showGeneralMassage(1, Translate("web_Content_Js_Common_getRoomBrowserUserId_message"), 2000, false);
        location.href = rootPath + "/Home/Index";
    } else {
        //浏览的用户id
        self.browserUserId(browserUserId);
    }
    return browserUserId;
}
//消息中心定位到用户空间的具体留言
function locatingMessage(messageId, userId, pageSize, commentSize, position) {
    var url = rootPath + "/UserRoom/Message?islocal=1&browserUserId=" + userId + "&linkcss=4&pagesize=" + pageSize + "&commentSize=" + commentSize + "&pageindex=" + position.pageindex + "&cmtInx=" + position.index + "#_" + messageId;
    window.open(url, "newwin");
}
//消息中心定位到播放页面的具体评论
function locatingComment(commentId, videoId, pageSize, commentSize, position) {
    var url = rootPath + "/Play/Index?islocal=1&videoId=" + videoId + "&pagesize=" + pageSize + "&commentSize=" + commentSize + "&pageindex=" + position.pageindex + "&cmtInx=" + position.index + "#_" + commentId
    window.open(url, "newwin");
}
//添加遮罩
function addMask(ele, flag, tip) {
    //需要翻译
    var mtip = tip ? tip : Translate('web_Client_Views_Content_Js_comment_addMask_Loading');// '加载中...';
    var commentOrMessage = Translate("web_Content_Js_player_addMask_message");// '正在提交...';
    var deleteing = Translate('web_Client_Views_Content_Js_comment_addMask_deleting');//'删除中...';

    var loading = '<img src="' + rootPath + '/Content/images/icon_img/loading_16X16.gif" width="16" height="16" style="width:16px;height:16px;border:0;">';
    if (flag == 1) {
        loading = '<img src="' + rootPath + '/Content/images/icon_img/loading_32X32.gif" width="32" height="32" style="width:32px;height:32px;border:0;"><br/><span style="font-size:12px;">' + mtip + '</span>';
    } else if (flag == 2) {
        loading = '<img src="' + rootPath + '/Content/images/icon_img/loading_40X40.gif" width="40" height="40" style="width:40px;height:40px;border:0;">'
    }
    else if (flag == 3) {
        loading = '<img src="' + rootPath + '/Content/images/icon_img/loading_60X60.gif" width="60" height="60" style="width:60px;height:60px;border:0;">'
    } else if (flag == 4) {
        loading = '<img src="' + rootPath + '/Content/images/icon_img/loading_16X16.gif" width="16" height="16" style="width:16px;height:16px;border:0;">' + commentOrMessage;
    } else if (flag == 5) {
        loading = '<img src="' + rootPath + '/Content/images/icon_img/loading_32X32.gif" width="32" height="32" style="width:32px;height:32px;border:0;"><br/><span style="font-size:12px;">' + deleteing + '</span>';
    }
    ele.block({ message: loading, overlayCSS: { opacity: 0, cursor: "pointer" }, css: { border: 'none', cursor: "pointer" } });
    $(ele).find(".blockElement").css("background-color", "");
}
//取消遮罩
function removeMask(ele) {
    ele.unblock();
}
String.prototype.trim = function (char, type) {
    if (char) {
        if (type == 'left') {
            return this.replace(new RegExp('^\\' + char + '+', 'g'), '');
        } else if (type == 'right') {
            return this.replace(new RegExp('\\' + char + '+$', 'g'), '');
        }
        return this.replace(new RegExp('^\\' + char + '+|\\' + char + '+$', 'g'), '');
    }
    return this.replace(/^\s+|\s+$/g, '');
};
//自定义hashtable
function Hashtable() {
    this._hash = new Object();
    this.put = function (key, value) {
        if (typeof (key) != "undefined") {
            if (this.containsKey(key) == false) {
                this._hash[key] = typeof (value) == "undefined" ? null : value;
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
    this.remove = function (key) { delete this._hash[key]; }
    this.size = function () { var i = 0; for (var k in this._hash) { i++; } return i; }
    this.get = function (key) { return this._hash[key]; }
    this.containsKey = function (key) { return typeof (this._hash[key]) != "undefined"; }
    this.clear = function () { for (var k in this._hash) { delete this._hash[k]; } }
}
//发送post请求
function postJson(url, d, succ, err) {
    $.ajax({
        type: "post",
        url: url,
        data: d,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (list) {
            if (succ != null) succ(list);
        },
        error: function (e) {
            if (err != null) err(e);
        }
    });
}
function check(callback) {
    var url = rootPath + "/UserRoom/GetCurrUserId?v=" + Math.random();
    $.getJSON(url, function (data) {
        //console.log("用户数据:");
        //console.log(data);        
        if (callback) callback(data);
    });
}

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
     (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
        RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
};
//深拷贝
function deepcopy(a, b) {
    var _clone = {}, i = 0,
    _arg = arguments, _co = '', len = _arg.length;
    if (!_arg[1]) {
        _clone = this;
    };
    for (; i < len; i++) {
        _co = _arg[i];
        for (var name in _co) {
            //深度拷贝
            if (typeof _co[name] === 'object') {
                _clone[name] = (_co[name].constructor === Array) ? [] : {};
                _clone[name] = deepcopy(_co[name], _clone[name]);
            } else {
                _clone[name] = _co[name];
            }
        }
    }
    return _clone;
};
function partString(str, len, ellipsis) {
    if (str != null) {
        if (str.length > len)
            str = str.substr(0, len) + ellipsis;
    } else
        str = "";
    return str;
}


// PLupload 图片预览
function previewImage(file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数 
    if (!file || !/image\//.test(file.type)) return; //确保文件是图片
    if (file.type == 'image/gif') {//gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
        var fr = new mOxie.FileReader();
        fr.onload = function () {
            callback(fr.result);
            fr.destroy();
            fr = null;
        }
        fr.readAsDataURL(file.getSource());
    } else {
        var preloader = new mOxie.Image();
        preloader.onload = function () {
            preloader.downsize(300, 300);//先压缩一下要预览的图片,宽300，高300
            var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
            callback && callback(imgsrc); //callback传入的参数为预览图片的url
            preloader.destroy();
            preloader = null;
        };
        preloader.load(file.getSource());
    }
}

//播币转换
function numberStr(bb) {
    if (bb) {
        var bbstr = bb.toString();
        if (bbstr.length <= 4) {
            return bb;
        } else {
            // var newBB;
            if (bbstr.length > 4 && bbstr.length <= 8) {
                // newBB = (bb / 10000).toFixed(0);
                // if (newBB.length < 3) {
                return (bb / 10000).toFixed(2) + Translate("web_Content_Js_Common_numberStr_wan");
                // } else if (newBB.length >= 3 & newBB.length < 4) {
                //    return (bb / 10000).toFixed(1) + "万";
                //  }
                // return newBB + "万";
            } else {
                // newBB = (bb / 10000000).toFixed(0);
                // if (newBB.length < 3) {
                return (bb / 10000000).toFixed(2) + Translate("web_Content_Js_Common_numberStr_qianwan");
                // } else if (newBB.length >= 3 & newBB.length < 4) {
                //     return (bb / 10000000).toFixed(1) + "千万";
                //  }
                //   return (bb / 10000000).toFixed(0) + "千万";
            }
        }
    }
    return 0;
}

function doGoPage(Id) {
    var navMenu = [
        {
            Id: 1,
            Name: Translate("web_Content_Js_Common_doGoPage_navMenu_zone"),
            url: rootPath + "/UserRoom/Index?browserUserId=-1"
        },
        {
            Id: 2,
            Name: Translate("web_Content_Js_Common_doGoPage_navMenu_Bobi"),
            url: rootPath + "/UserCenter/UserBoBi"
        }, {
            Id: 3,
            Name: Translate("web_Content_Js_Common_doGoPage_navMenu_video"),
            url: rootPath + "/UserCenter/UserVideo"
        }, {
            Id: 4,
            Name: Translate("web_Content_Js_Common_doGoPage_navMenu_rss"),
            url: rootPath + "/UserCenter/MyFans"
        }, {
            Id: 5,
            Name: Translate("web_Content_Js_Common_doGoPage_navMenu_collect"),
            url: rootPath + "/UserCenter/UserCollect"
        }, {
            Id: 6,
            Name: Translate("web_Content_Js_Common_doGoPage_navMenu_special"),
            url: rootPath + "/UserCenter/UserAlbums"
        }, {
            Id: 7,
            Name: Translate("web_Content_Js_Common_doGoPage_navMenu_account"),
            url: rootPath + "/UserCenter/AccountSet"
        }
    ];

    $(navMenu).each(function (idx, li) {
        if (li != null && li.Id == Id) {
            var target = "_self";
            if (li.url.indexOf(location.pathname) > 0) {
                //当前页打开
                target = "_self";
            } else {
                //新开窗口打开
                target = "_blank";
            }
            window.open(li.url, target);
            return;
        }
    });
}


//加载导航数据
function loadNavData(callback) {
    var url = api + "Category/GetMenuViewList";
    $.getJSON(url, function (data) {
        var arr = [];
        arr.push({
            ParentCategory: { Id: -1, Name: Translate("web_Content_Js_Common_loadNavData_shouye") },
            href: rootPath + "/Home/Index"
        });
        for (var i in data) {
            data[i].href = rootPath + "/Home/Index?curId=" + data[i].ParentCategory.Id;
            arr.push(data[i]);
        }
        arr.push({
            ParentCategory: { Id: -2, Name: Translate("web_Content_Js_Common_loadNavData_zhuanji") },
            href: rootPath + "/Albums/Index?curId=-2"
        });

        var htmlArr = [];
        $(arr).each(function (i, o) {
            htmlArr.push('<li><a href="' + o.href + '" target="_blank">' + o.ParentCategory.Name + '</a></li>');
        });
        htmlArr.push('<div class="clear"></div>');
        $("#ulnav").html(htmlArr.join(""));
        if (callback) callback();
    });
}

/**
 * 取字符串的长度
 * "你好".len() return 4
 * @param {} 
 * @returns {}
 */
String.prototype.len = function () {
    var len = 0;
    for (var i = 0; i < this.length; i++) {
        var val = this.charAt(i);
        if (val.isChinese()) {
            len += 2;
        } else {
            len += 1;
        }
    }
    return len;
}
/**
 * 比较字符串与length的长度
 * "你好".areBigger(10) return -6
 * @param {} 
 * @returns {}
 */
String.prototype.areBigger = function (length) {
    return this.len() - length;
}
/**
 * 验证是否是中文
 * "你".isChinese() return true
 * @param {}  
 * @returns {}
 */
String.prototype.isChinese = function () {
    var reCh = new RegExp("[\\u4E00-\\u9FFF]+", "g");
    return reCh.test(this);
}

function changeText(obj, num) {
    var len = obj.val().len();
    var val = obj.val();

    if (len == num) {
        obj.attr('maxlength', val.length);
    } else if (len < num) {
        obj.removeAttr('maxlength');
    }
    if (len > num) {
        var oldVal = obj.attr('oldVal');
        obj.val(oldVal);
    }
    obj.attr('oldVal', obj.val());

}


//--------------------------------------------------------------------begin 返回顶部、问题反馈----------------------------------------------------------------//
$(function () {
    var bottomTools = $('.bottom_tools');
    $(window).scroll(function () {
        setBottomToolsPos();
    });

    $('#scrollUp').click(function (e) {
        e.preventDefault();
        $('html,body').animate({ scrollTop: 0 });
    });
    intiTop();
    window.onresize = function () {
        intiTop();
        setBottomToolsPos();
    }
});

function setBottomToolsPos() {
    var bottomTools = $('.bottom_tools');
    var scrollHeight = $(document).height() - 100;
    var oClientH = $(window).height();
    var scrollTop = $(window).scrollTop();
    var top = bottomTools.offset().top;
    scrollTop > 50 ? $("#scrollUp").stop().animate({ opacity: 1 }) : $("#scrollUp").stop().animate({ opacity: 0 });
    if (scrollTop + oClientH > scrollHeight) {
        bottomTools.stop().animate({ 'bottom': '170px' });
    } else {
        bottomTools.stop().animate({ 'bottom': '40px' });
    }

}

function intiTop() {
    var left = ($(window).width() - 1200) / 2 + 1250;
    $('.bottom_tools').css({ 'left': left + 'px' });
}
//设置弹框居中 sClass弹出div样式名
function setPosition(sClass) {
    var $obj = $("." + sClass);
    var $videoH = $obj.height() / 2;
    //  var $oScroll = $(document).scrollTop();
    var oClientH = document.documentElement.clientHeight;
    $obj.css('top', oClientH / 2 - $videoH + 'px');

    //设置遮罩层的高度	
    // var setObj = $obj.parent();//获取遮罩层
    // var oPageHeight = $(document).height();
    // setObj.css('height', oPageHeight + "px");
}

//问题反馈
function doFeedback() {
    var user = userId == 0 ? "" : userId;
    var Browser = getBrowserInfo();
    var OperatingSystem = detectOS();
    var QuestionPage = document.title;
    var LinkAddress = document.URL;
    questionPop.addTable(user, Browser, OperatingSystem, QuestionPage, LinkAddress, globalPromptBox.questionMassage);
}

//获取浏览器名字+版本字符串
function getBrowserInfo() {
    var agent = navigator.userAgent.toLowerCase();

    var regStr_ie = /msie [\d.]+;/gi;
    var regStr_ff = /firefox\/[\d.]+/gi
    var regStr_chrome = /chrome\/[\d.]+/gi;
    var regStr_saf = /safari\/[\d.]+/gi;
    //IE
    if (agent.indexOf("msie") > 0) {
        return agent.match(regStr_ie);
    }

    //firefox
    if (agent.indexOf("firefox") > 0) {
        return agent.match(regStr_ff);
    }

    //Chrome
    if (agent.indexOf("chrome") > 0) {
        return agent.match(regStr_chrome);
    }

    //Safari
    if (agent.indexOf("safari") > 0 && agent.indexOf("chrome") < 0) {
        return agent.match(regStr_saf);
    }

}
//获取操作系统
function detectOS() {
    var sUserAgent = navigator.userAgent;
    var isWin = (navigator.platform == "Win32") || (navigator.platform == "Windows");
    var isMac = (navigator.platform == "Mac68K") || (navigator.platform == "MacPPC") || (navigator.platform == "Macintosh") || (navigator.platform == "MacIntel");
    if (isMac) return "Mac";
    var isUnix = (navigator.platform == "X11") && !isWin && !isMac;
    if (isUnix) return "Unix";
    var isLinux = (String(navigator.platform).indexOf("Linux") > -1);
    if (isLinux) return "Linux";
    if (isWin) {
        var isWin2K = sUserAgent.indexOf("Windows NT 5.0") > -1 || sUserAgent.indexOf("Windows 2000") > -1;
        if (isWin2K) return "Win2000";
        var isWinXP = sUserAgent.indexOf("Windows NT 5.1") > -1 || sUserAgent.indexOf("Windows XP") > -1;
        if (isWinXP) return "WinXP";
        var isWin2003 = sUserAgent.indexOf("Windows NT 5.2") > -1 || sUserAgent.indexOf("Windows 2003") > -1;
        if (isWin2003) return "Win2003";
        var isWinVista = sUserAgent.indexOf("Windows NT 6.0") > -1 || sUserAgent.indexOf("Windows Vista") > -1;
        if (isWinVista) return "WinVista";
        var isWin7 = sUserAgent.indexOf("Windows NT 6.1") > -1 || sUserAgent.indexOf("Windows 7") > -1;
        if (isWin7) return "Win7";
    }
    return "other";
}

//--------------------------------------------------------------------end 返回顶部、问题反馈----------------------------------------------------------------//
//格式化字符串
String.prototype.Format = function () {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined && arguments[i] != null) {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}