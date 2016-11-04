function id$(e) {
    return $('#' + e);
}
function span$(e) {
    return $('#span' + e);
}
$(function () {
    LR.ReturnUrlValid();
    $('#per_home').hover(LR.InFunction, LR.OutFunction);
    $('#loginAnchor,#fLoginAnchor').on('click', function () { LR.LoginShow(); });
    $('#registAnchor,#fregistAnchor').on('click', function () { LR.RegistShow(); });
    $('#getPwdAnchor').on('click', function () { LR.GetPwdShow(); });
    $('.lclose').each(function () { $(this).on('click', function () { LR.PubClose(this); }); });
    document.onkeydown = function (event) {
        var e = event || window.event || arguments.callee.caller.arguments[0];
        if (e.keyCode == 13) {
            if ($('#txtSearch').is(':focus')) {
                return;
            }
            if (!$("#per_home").is(':visible')) {
                if ($("#fpwd").is(':visible')) {
                    LR.GetPassword('getPwdBtn', 'getAccount', 'getCode', 'getFirstPwd', 'getSecondPwd');
                    return;
                }
                if (!$("#register").is(':visible') && !$("#popbox").is(':visible')) {
                    LR.LoginShow();
                    return;
                }
                if (!$("#register").is(':visible') && $("#popbox").is(':visible')) {
                    LR.Login('loginBtn', 'loginAccount', 'loginPwd', 'loginRemb');
                    return;
                }
                if (!$("#popbox").is(':visible') && !$("#register").is(':visible')) {
                    LR.RegistShow();
                    return;
                }
                if (!$("#popbox").is(':visible') && $("#register").is(':visible')) {
                    LR.Regist('registBtn', 'registAccount', 'registPwd', 'agree', 'registCode', 'registForm');
                    return;
                }
            }
        }
    };
});
LR = {
    DisableCss: 'unclick',
    ActiveCss: '',
    RootPath: '',
    SmsCodeTimeout: null,
    $CodeBtn: null,
    SmsCodeTime: 120,
    AccountType: 0,//默认手机
    MailArr: ['@qq.com', '@163.com', '@126.com', '@sohu.com', '@sina.com', '@gmail.com', '@21cn.com', '@hotmail.com', '@vip.qq.com', '@yeah.com'],
    //--------------------登录注册功能函数--------------------
    //绑定头部用户中心数据
    BindHeadData: function (d) {
        LR.PubClose();
        $('#lgfirst').hide();
        $('#per_home').show();
        var $accSpan = $('#accSpan');
        if (d.Data.NickName && d.Data.NickName.length > 0) { $accSpan.text(d.Data.NickName); } else { $accSpan.text(d.Data.Account); }
        $('#sbcSpan').text(d.Data.FansCount);
        //        $('#bbSpan').text(d.Data.BB);
        $("#bbSpan").text(numberStr(d.Data.BB));
        $('#picImg,#picImg2').each(function () { $(this).attr('src', d.Data.Picture); });
    },
    //消息推送
    PushMessage: function () {
        //已登陆
        var json = {
            type: "post",
            url: rootPath + "/UserCenter/GetHeaderMessage",
            dataType: "json",
            success: function (responseData) {
                if (responseData != null) {
                    $("#messageUrl").attr("href", rootPath + "/UserCenter/Messager?t=" + responseData.MessageTypeId);
                    //修改消息数量的显示
                    if (responseData.UnreadMessageCount <= 0) {
                        $("#messageUrl #xqtip").removeAttr("class");
                        $("#messageUrl #xqtip").text("");
                    } else {
                        if (responseData.UnreadMessageCount > 99) responseData.UnreadMessageCount = 99;
                        $("#messageUrl #xqtip").attr("class", "xx_num");
                        $("#messageUrl #xqtip").text(responseData.UnreadMessageCount);
                    }
                }
            }
        };
        $.ajax(json);
    },
    //登录 
    Login: function (btnId, accountId, pwdId, rembId) {
        var pvrst = LR.PhoneValid(accountId, null, null, false, false), pdvrst = LR.PasswordValid(pwdId);
        if (!pvrst || !pdvrst) { return; }
        var btn = id$(btnId),
            account = $.trim(id$(accountId).val()),
            pwd = $.trim(id$(pwdId).val()),
            remb = id$(rembId).is(':checked') ? 1 : 0,
            url = LR.RootPath + '/Login/UserLogin',
            data = { account: account, pwd: pwd, remb: remb },
            $spanError = span$(pwdId);
        LR.BtnDisable(btn,Translate('web_Content_Js_load_LR_Login_message_logining'));
        $.post(url, data, function (d) {
            if (d && d.Success && d.Data) {
                LR.BindHeadData(d);
                var rurl = GetQueryString("returnurl");
                if (rurl) { window.location.href = LR.RootPath + rurl; }
                setTimeout(function () { LR.PushMessage(); }, 10);
            } else {
                $spanError.show().text(Translate('web_Content_Js_load_LR_Login_message_error'));
            }
            LR.BtnActive(btn, Translate('web_Content_Js_load_LR_Login_message_login'));
        }, "json");
    },
    //注销
    LogOut: function () {

        //退出第三方 QQ
        if (QC.Login.check()) QC.Login.signOut();
        //退出第三方 sina
        if (WB2.checkLogin()) WB2.logout();

        var url = LR.RootPath + '/Login/LogOut?t=' + Math.random();
        $.getJSON(url, function (d) {
            if (d.Success) {
                $('#lgfirst').show();
                $('#per_home').hide();
                window.location.href = LR.RootPath;
            }
        });
    },
    //注册 
    Regist: function (btnId, accountId, pwdId, agreeId, codeId) {
        var pvrst = LR.PhoneValid(accountId, true, false, false, false), pwvrst = LR.PasswordValid(pwdId), cvrst = LR.CodeValid(codeId, accountId), agvrst = true;
        if (!id$(agreeId).is(':checked')) {
            span$('agree').show().text(Translate('web_Content_Js_load_LR_Regist_message_xieyi'));
            agvrst = false;
        }
        if (!pvrst || !pwvrst || !cvrst || !agvrst) { return; }
        var btn = id$(btnId), url = LR.RootPath + '/Regist/UserRegist',
            data = { raccount: $.trim(id$(accountId).val()), rpwd: $.trim(id$(pwdId).val()), type: LR.AccountType };
        LR.BtnDisable(btn, Translate('web_Content_Js_load_LR_Regist_message_registing'));
        $.post(url, data, function (d) {
            if (d && d.Success) {
                LR.BindHeadData(d);
                LR.RestSmsCodeBtn();
            }
            LR.BtnActive(btn, Translate('web_Content_Js_load_LR_Regist_message_regist'));
        }, "json");
    },
    //找回密码 
    GetPassword: function (btnId, accountId, codeId, firstpwdId, secpwdId) {
        var pvrst = LR.PhoneValid(accountId, true, true), cvrst = LR.CodeValid(codeId, accountId), pvdrst = LR.PasswordValid(firstpwdId), spvrst = LR.SecPasswordValid(firstpwdId, secpwdId);
        if (!pvrst || !cvrst || !pvdrst || !spvrst) { return; }
        var btn = id$(btnId),
            getAccount = $.trim(id$(accountId).val()),
            fpwd = $.trim(id$(firstpwdId).val()),
            url = LR.RootPath + '/Login/UpdatePwdByPhone',
            data = { account: getAccount, pwd: fpwd };
        LR.BtnDisable(btn, Translate('web_Content_Js_load_LR_GetPassword_message_changing'));
        $.post(url, data, function (d) {
            if (d && d.Success && d.Data) {
                LR.PubClose();
                LR.BindHeadData(d);
                LR.RestSmsCodeBtn();
            }
            LR.BtnActive(btn, Translate('web_Content_Js_load_LR_GetPassword_message_sure'));
        }, "json");
    },
    //获取短信验证码
    GetSmsCode: function (btnId, codeId, accountId, isNeedChkExist, isChkExist, isEmail, isPhone, btype) {
        var btn = id$(btnId),
            phone = $.trim(id$(accountId).val()),
            $spanError = span$(codeId),
            pvrst = false,
            url = LR.RootPath + '/SMS/SubmitSMS';
        if (isPhone || LR.AccountType == 0) {
            pvrst = LR.PhoneValid(accountId, isNeedChkExist, isChkExist, isEmail, isPhone);
        }
        if (isEmail || LR.AccountType == 1) {
            pvrst = LR.EmailValid(accountId, isNeedChkExist, isChkExist);
        }
        if (!pvrst) return;
        var data = { PhoneNumber: phone, AccountType: LR.AccountType, ClientBusinessType: btype };
        LR.BtnDisable(btn);
        btn.val(LR.SmsCodeTime + "s");
        LR.SmsCodeTimeout = setInterval(LR.RegSmsPrompt, 1000);
        LR.$CodeBtn = btn;
        $.post(url, data, function (d) {
            if (!d || !d.Success || !d.Data || !d.Data.Code || !d.Data.Code > 0) {
                $spanError.show().text(Translate('web_Content_Js_load_LR_GetSmsCode_message_vercodefail'));
                LR.RestSmsCodeBtn();
            }
        }, "json");
    },
    //短信验证码时间变化
    RegSmsPrompt: function () {
        LR.SmsCodeTime--;
        LR.$CodeBtn.val(LR.SmsCodeTime + "s");
        if (LR.SmsCodeTime <= 0) {
            LR.RestSmsCodeBtn();
        }
    },
    RestSmsCodeBtn: function () {
        clearTimeout(LR.SmsCodeTimeout);
        if (LR.$CodeBtn) {
            LR.BtnActive(LR.$CodeBtn, Translate('web_Content_Js_load_LR_RestSmsCodeBtn_message_resend'));
        }
        LR.SmsCodeTime = 120;
    },
    //--------------------窗口功能函数--------------------
    //弹出登录窗
    LoginShow: function () {
        $('#bg').show();
        $('#popbox').show();
        $('#register').hide();
        $('#loginAccount').val('');
        $('#loginPwd').val('');
        //----------------------------------------------------------------------第三方登录-----------------------------------------------------------------//

        //集成QQ登录按钮
        QC.Login({
            btnId: "qq_login_btn",//插入按钮的html标签id
            size: "A_M",//按钮尺寸
            scope: "get_user_info",//展示授权，全部可用授权可填 all
            display: "pc"//应用场景，可选
        }, function (reqData, opts) {//登录成功
            //处理逻辑
            TP.QQLoginProcess(reqData, opts);

        }, function (opts) {//注销成功
            //QQ登录 注销成功
        });

        //集成sina登录
        WB2.anyWhere(function (W) {
            WB2.checkLogin()
            W.widget.connectButton({
                id: "wb_connect_btn",
                type: '3,5',
                callback: {
                    login: function (o) {
                        //登录后的回调函数
                        TP.SinaLoginProcess(o);
                    },
                    logout: function () {
                        //退出后的回调函数

                    }
                }
            });
        });
        //----------------------------------------------------------------------第三方登录-----------------------------------------------------------------//
    },
    //弹出注册窗
    RegistShow: function () {
        $('#bg').show();
        $('#register').show();
        $('#popbox').hide();
        $('#registAccount').val('');
        $('#registPwd').val('');
        $('#registCode').val('');
        if (LR.$CodeBtn) {
            LR.$CodeBtn.removeAttr('disabled');
            LR.$CodeBtn.val(Translate("web_Content_Js_load_LR_GetSmsCode_message_sendvercode"));
        }

        //----------------------------------------------------------------------第三方登录-----------------------------------------------------------------//

        //集成QQ登录按钮
        QC.Login({
            btnId: "qq_login_btn_regist",//插入按钮的html标签id
            size: "A_M",//按钮尺寸
            scope: "get_user_info",//展示授权，全部可用授权可填 all
            display: "pc"//应用场景，可选
        }, function (reqData, opts) {//登录成功
            //处理逻辑
            TP.QQLoginProcess(reqData, opts);

        }, function (opts) {//注销成功
            //QQ登录 注销成功
        });

        //集成sina登录
        WB2.anyWhere(function (W) {
            WB2.checkLogin()
            W.widget.connectButton({
                id: "wb_connect_btn_regist",
                type: '3,5',
                callback: {
                    login: function (o) {
                        //登录后的回调函数
                        TP.SinaLoginProcess(o);
                    },
                    logout: function () {
                        //退出后的回调函数

                    }
                }
            });
        });
        //----------------------------------------------------------------------第三方登录-----------------------------------------------------------------//

    },
    //弹出找回密码窗
    GetPwdShow: function () {
        $('#popbox').hide();
        $('#fpwd').show();
        $('#getAccount').val('');
        $('#getFirstPwd').val('');
        $('#getSecondPwd').val('');
        $('#getCode').val('');
    },
    //关闭弹出窗
    PubClose: function () {
        $('#bg').hide();
        $('#popbox').hide();
        $('#popbox span').text('');
        $('#register').hide();
        $('#register span').text('');
        $('#fpwd').hide();
        $('#fpwd span').text('');
        $("#popbox,#register,#fpwd").find('.error_style').removeClass('error_style');
    },
    //账户中心窗口显示
    InFunction: function () {
        clearTimeout(LR.timer);
        LR.timer = setTimeout(function () {
            $('#show_home').show();
        }, 350);
    },
    //账户中心窗口隐藏
    OutFunction: function () {
        clearTimeout(LR.timer);
        LR.timer = setTimeout(function () {
            $('#show_home').hide();
        }, 350);
    },
    //--------------------验证函数--------------------
    //禁用按钮
    BtnDisable: function ($btn, text) {
        $btn.attr('disabled', 'disabled').removeClass(LR.ActiveCss).addClass(LR.DisableCss);
        if (text) {
            $btn.val(text);
        }
    },
    //启用按钮
    BtnActive: function ($btn, text) {
        $btn.removeAttr('disabled').removeClass(LR.DisableCss).addClass(LR.ActiveCss);
        if (text) {
            $btn.val(text);
        }
    },
    //检测是否有回调地址,用于检测是否登录
    ReturnUrlValid: function () {
        if (GetQueryString("returnurl")) LR.LoginShow();
    },
    //获取匹配邮箱
    GetEmail: function (e) {
        var $li = $(e), $ul = $li.parent(), nidstr = $li.attr('data-nid'), idstr = $li.attr('data-id'), $id = id$(idstr), ck = parseInt($li.attr('ck'));
        id$(nidstr).focus();
        $id.val($li.attr('data-select'));
        $ul.hide();
        if (ck == 2) LR.EmailValid(idstr, true, false, true);
        else {
            ck = ck > 0;
            LR.EmailValid(idstr, ck, ck, true);
        }
    },
    //自动带出邮箱后缀
    EmailAuto: function (id, nid, isChk) {
        LR.RemoveErrorClass(id);
        var $ul = id$('ul' + id), emailVal = $.trim(id$(id).val());
        if (emailVal.indexOf('@') > -1) {
            if (new RegExp(/^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/).test(emailVal)) {
                $ul.hide();
                return;
            } else if (/@/.test(emailVal)) {
                var prefix = emailVal.replace(/@.*/, ""), sufFix = emailVal.replace(/.*@/, ""), newArrs = [];
                if (prefix.length < 1) return;
                $.map(LR.MailArr, function (n) {
                    var reg = new RegExp(sufFix);
                    if (reg.test(n)) newArrs.push(n);
                });
                if (newArrs.length > 0) {
                    $ul.find('li').remove();
                    newArrs.forEach(function (e) {
                        $ul.append('<li data-select="' + prefix + e + '" data-id="' + id + '" data-nid="' + nid + '" ck="' + isChk + '" data-type="normal" onclick="LR.GetEmail(this)">' + prefix + e + '</li>');
                    });
                    $ul.show();
                    return;
                }
            }
        }
        $ul.hide();
    },
    //验证邮箱是否合法
    EmailValid: function (id, isNeedChkExist, isChkExist, isf) {
        var $email = id$(id), emailVal = $.trim($email.val()), $spanError = span$(id), result = false;
        if (!LR.CheckText(emailVal, id)) { return false; }
        if (emailVal.indexOf('@') > -1) {
            if (!new RegExp(/^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/).test(emailVal)) {
                $spanError.show().text(Translate('web_Content_Js_load_LR_EmailValid_message_email'));
                if (isf) { $spanError.show().text(Translate('web_Content_Js_load_LR_EmailValid_message_phoneoremail')); }
                LR.AddErrorClass(id);
                result = false;
            } else {
                $spanError.hide();
                result = true;
                LR.RemoveErrorClass(id);
            }
        } else {
            $spanError.show().text(Translate('web_Content_Js_load_LR_EmailValid_message_emailtwo'));
            if (isf) { $spanError.show().text(Translate('web_Content_Js_load_LR_EmailValid_message_phoneoremailtwo')); }
            LR.AddErrorClass(id);
            result = false;
        }
        if (result && isNeedChkExist) result = LR.EmailExistValid(id, isChkExist);
        return result;
    },
    //验证手机号码(邮箱)是否合法
    PhoneValid: function (id, isNeedChkExist, isChkExist, isEmail, isPhone) {
        var $phone = id$(id), phoneVal = $.trim($phone.val()), $spanError = span$(id);
        if (!LR.CheckText(phoneVal, id)) return false;
        if (!$phone || phoneVal.length < 1) {
            $spanError.show();
            LR.AddErrorClass(id);
            if (isEmail) $spanError.text(Translate('web_Content_Js_load_LR_PhoneValid_message_ifemail'));
            if (isPhone) $spanError.text(Translate('web_Content_Js_load_LR_PhoneValid_message_ifphone'));
            if (!isEmail && !isPhone) $spanError.text(Translate('web_Content_Js_load_LR_PhoneValid_message_ifphoneoremail'));
            return false;
        }
        if (!LR.CheckText(phoneVal, id)) return false;
        LR.AccountType = 0;
        if (/@/.test(phoneVal) && !isPhone) {
            if (!new RegExp(/^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/).test(phoneVal)) {
                $spanError.show().text(Translate('web_Content_Js_load_LR_PhoneValid_message_iftwophoneoremail'));
                LR.AddErrorClass(id);
                return false;
            }
            LR.AccountType = 1; //邮箱
            if (!LR.EmailValid(id, isNeedChkExist, isChkExist, true)) return false;
        } else {
            if (!new RegExp(/^1[3|4|5|6|7|8|9][0-9]\d{8}$/).test(phoneVal)) {
                $spanError.show().text(Translate('web_Content_Js_load_LR_PhoneValid_message_iftwoelsephoneoremail'));
                if (isPhone) { $spanError.show().text(Translate('web_Content_Js_load_LR_PhoneValid_message_iftwoelsephone')); }
                LR.AddErrorClass(id);
                return false;
            }
            LR.AccountType = 0;//手机
            if (isNeedChkExist) if (!LR.PhoneExistValid(id, isChkExist)) return false;
        }
        $spanError.hide();
        LR.RemoveErrorClass(id);
        return true;
    },
    //验证账户是否已经存在
    PhoneExistValid: function (id, isChkExist) {
        var url = api + 'Register/CheckAccount?account=' + $.trim(id$(id).val()) + '&v=' + Math.random(), $spanError = span$(id), result = false;
        $.ajaxSettings.async = false;
        $.getJSON(url, function (d) {
            if (d && d.Success && !d.Data) {
                //存在手机号
                if (isChkExist) {
                    $spanError.hide();
                    LR.RemoveErrorClass(id);
                    result = true;
                } else {
                    $spanError.show().text(Translate('web_Content_Js_load_LR_PhoneExistValid_message_ifphone'));
                    LR.AddErrorClass(id);
                    result = false;
                }
            } else if (d && d.Success && d.Data) {
                //不存在手机号
                if (isChkExist) {
                    $spanError.show().text(Translate('web_Content_Js_load_LR_PhoneExistValid_message_elsephone'));
                    LR.AddErrorClass(id);
                    result = false;
                } else {
                    $spanError.hide();
                    LR.RemoveErrorClass(id);
                    result = true;
                }
            }
        });
        $.ajaxSettings.async = true;
        return result;
    },
    //验证邮箱是否已经存在
    EmailExistValid: function (id, isChkExist) {
        var $email = id$(id), emailVal = $.trim($email.val()), $spanError = span$(id),
            url = api + 'Register/CheckEmail?email=' + emailVal + '&v=' + Math.random(), result = false;
        $.ajaxSettings.async = false;
        $.getJSON(url, function (d) {
            if (d && d.Success && !d.Data) {
                //存在邮箱
                if (isChkExist) {
                    $spanError.hide();
                    LR.RemoveErrorClass(id);
                    result = true;
                } else {
                    $spanError.show().text(Translate('web_Content_Js_load_LR_EmailExistValid_message_ifemail'));
                    LR.AddErrorClass(id);
                    result = false;
                }
            } else if (d && d.Success && d.Data) {
                //不存在邮箱
                if (isChkExist) {
                    $spanError.show().text(Translate('web_Content_Js_load_LR_EmailExistValid_message_elseemail'));
                    LR.AddErrorClass(id);
                    result = false;
                } else {
                    $spanError.hide();
                    LR.RemoveErrorClass(id);
                    result = true;
                }
            }
        });
        $.ajaxSettings.async = true;
        return result;
    },
    //验证密码是否合法
    PasswordValid: function (id) {
        var $pwd = id$(id), pwdVal = $.trim($pwd.val()), $spanError = span$(id);
        if (!$pwd || !pwdVal || pwdVal < 1) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_PasswordValid_message_ifpasswordnull'));
            LR.AddErrorClass(id);
            return false;
        }
        if (!LR.CheckText(pwdVal, id)) { return false; }
        if (pwdVal.indexOf(" ") > -1) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_PasswordValid_message_ifpdhasspace'));
            LR.AddErrorClass(id);
            return false;
        }
        if (pwdVal.length < 6 || pwdVal.length > 18) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_PasswordValid_message_ifpdlengthout'));
            LR.AddErrorClass(id);
            return false;
        }
        var t1 = /^(?!^(\d+|[a-zA-Z]+|[~`!@#$%^&*()\-_+={[\]}|\\:;"'<,.>?\/]+)$)^[\w~`!@#$%^&*()\-_+={[\]}|\\:;"'<,.>?\/]+$/;
        var t = /.*[\u4e00-\u9fa5]+.*$/;
        if (t.test(pwdVal)) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_PasswordValid_message_ifpdhaschinese'));
            LR.AddErrorClass(id);
            return false;
        }
        if (!t1.test(pwdVal)) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_PasswordValid_message_ifpdvalid'));
            LR.AddErrorClass(id);
            return false;
        }
        $spanError.hide();
        LR.RemoveErrorClass(id);
        return true;
    },
    //验证确认密码和新密码是否一致
    SecPasswordValid: function (fid, sid) {
        var firstPwdVal = $.trim(id$(fid).val()), sencondPwdVal = $.trim(id$(sid).val()), $spanError = span$(sid);
        if (!firstPwdVal || firstPwdVal.length < 1 || !sencondPwdVal || sencondPwdVal.length < 1 || firstPwdVal !== sencondPwdVal) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_SecPasswordValid_message'));
            LR.AddErrorClass(sid);
            return false;
        } else {
            LR.RemoveErrorClass(sid);
            $spanError.hide();
            return true;
        }
    },
    //验证已登录用户密码是否正确
    LoginUserPwdValid: function (id, isf) {
        var $oldpwd = id$(id), oldpwd = $.trim($oldpwd.val()), $spanError = span$(id), result = false;
        if (!$oldpwd || oldpwd.length < 1) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_SecPasswordValid_message_ifoldpwd'));
            if (isf) $spanError.text(Translate('web_Content_Js_load_LR_SecPasswordValid_message_ifisf'));
            id$(id).addClass('error_style');
            return result;
        }
        var url = LR.RootPath + '/Login/LoginUserPwdValid', data = {
            pwd: oldpwd
        };
        $.ajaxSettings.async = false;
        $.post(url, data, function (d) {
            if (d && d.Success) {
                $spanError.hide();
                id$(id).removeClass('error_style');
                result = true;
            } else {
                id$(id).addClass('error_style');
                $spanError.show().text(Translate('web_Content_Js_load_LR_LoginUserPwdValid_message_postelse'));
                if (isf) $spanError.text(Translate('web_Content_Js_load_LR_LoginUserPwdValid_message_postelseif'));
            }
        }, "json");
        $.ajaxSettings.async = true;
        return result;
    },
    //验证已登录用户手机号是否正确
    LoginUserPhoneValid: function (id) {
        var $oldphone = id$(id), oldphone = $.trim($oldphone.val()), $spanError = span$(id), result = false;
        if (!LR.CheckText(oldphone, id)) { return false; }
        if (!$oldphone || oldphone.length < 1) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_LoginUserPhoneValid_message_ifoldlength'));
            LR.AddErrorClass(id);
            return result;
        }
        if (!new RegExp(/^1[3|4|5|6|7|8|9][0-9]\d{8}$/).test(oldphone)) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_LoginUserPhoneValid_message_ifoldphonetest'));
            LR.AddErrorClass(id);
            return false;
        }
        var url = LR.RootPath + '/Login/LoginUserPhoneValid', data = { phone: oldphone };
        $.ajaxSettings.async = false;
        $.post(url, data, function (d) {
            if (d && d.Success) {
                $spanError.hide();
                LR.RemoveErrorClass(id);
                result = true;
            } else {
                $spanError.show().text(Translate('web_Content_Js_load_LR_LoginUserPhoneValid_message_postelse'));
                LR.AddErrorClass(id);
                result = false;
            }
        }, "json");
        $.ajaxSettings.async = true;
        return result;
    },
    //验证验证码是否合法
    CodeValid: function (id, phoneid, isEmail, isPhone) {
        var codeVal = $.trim(id$(id).val()), phone = $.trim(id$(phoneid).val()), $spanError = span$(id), $spanPError = span$(phoneid), phoneRst = false, codeRst = false, result = false;
        if (!phone || phone.length < 1) {
            $spanPError.show();
            LR.AddErrorClass(phoneid);
            if (isEmail)
                $spanPError.text(Translate('web_Content_Js_load_LR_CodeValid_message_ifisEmail'));
            if (isPhone)
                $spanPError.text(Translate('web_Content_Js_load_LR_CodeValid_message_ifisPhone'));
            if (!isEmail && !isPhone)
                $spanPError.text(Translate('web_Content_Js_load_LR_CodeValid_message_ifisPhoneorisEmail'));
        } else phoneRst = true;
        if (!codeVal || codeVal.length < 1) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_CodeValid_message_ifcodevalnull'));
            LR.AddErrorClass(id);
        }
        else if (codeVal.length > 4) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_CodeValid_message_ifcodevallength'));
            LR.AddErrorClass(id);
        } else {
            codeRst = true;
            LR.RemoveErrorClass(id);
        }
        if (!phoneRst || !codeRst) return false;
        var url = LR.RootPath + '/SMS/CheckSmsCode?code=' + codeVal + '&phone=' + phone + '&t=' + Math.random();
        $.ajaxSettings.async = false;
        $.getJSON(url, function (d) {
            if (d && d.Success && d.Data) {
                $spanError.hide();
                LR.RemoveErrorClass(id);
                result = true;
            } else {
                $spanError.show().text(Translate('web_Content_Js_load_LR_CodeValid_message_getelse'));
                LR.AddErrorClass(id);
                result = false;
            }
        });
        $.ajaxSettings.async = true;
        return result;
    },
    //验证图片验证码是否合法
    ImgCodeValid: function (id) {
        var codeVal = $.trim(id$(id).val()), $spanError = span$(id), result = false;
        if (!codeVal || codeVal.length < 1 || codeVal.length > 4) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_ImgCodeValid_message_codeval'));
            LR.AddErrorClass(id);
            return false;
        }
        var url = LR.RootPath + '/Login/CheckValidateCode?vcode=' + codeVal + '&t=' + Math.random();
        $.ajaxSettings.async = false;
        $.getJSON(url, function (d) {
            if (d && d.Success) {
                $spanError.hide();
                LR.RemoveErrorClass(id);
                result = true;
            } else {
                $spanError.show().text(Translate('web_Content_Js_load_LR_ImgCodeValid_message_getelse'));
                LR.AddErrorClass(id);
                result = false;
            }
        });
        $.ajaxSettings.async = true;
        return result;
    },
    //除去错误边框样式
    RemoveErrorClass: function (id) {
        id$(id).removeClass('error_style');
    },
    //添加错误样式
    AddErrorClass: function (id) {
        id$(id).addClass('error_style');
    },
    //去除空格
    RemoveSpace: function (str) {
        return str.replace(/\s/g, "");
    },
    //判断全角半角
    CheckText: function (str, id) {
        var strCode, result = true, $spanError = span$(id);
        for (var i = 0; i < str.length; i++) {
            strCode = str.charCodeAt(i);
            if ((strCode > 65248) || (strCode == 12288)) {
                result = false;
                break;
            }
        }
        if (!result) {
            $spanError.show().text(Translate('web_Content_Js_load_LR_CheckText_message'));
            LR.AddErrorClass(id);
        } else {
            $spanError.hide().text('');
            LR.RemoveErrorClass(id);
        }
        return result;
    }
}

//第三方帐号处理
var TP = {
    ReturnURL: "",
    //QQ登录处理逻辑
    QQLoginProcess: function (reqData, opts) {
        //获取openId和accessToken
        if (QC.Login.check()) {//如果已登录
            QC.Login.getMe(function (openId, accessToken) {

                //根据返回数据，更换按钮显示状态方法，网络卡时用于显示登录的QQ用户
                var dom = document.getElementById(opts['btnId']),
                _logoutTemplate = [
                     //头像
                     //'<span><img src="{figureurl}" class="{size_key}"/></span>',
                     //昵称
                     '<span>{nickname}</span>',
                     //退出
                     '<span><a href="javascript:QC.Login.signOut();">'+Translate('web_Content_Js_load_TP_QQLoginProcess_logoutTemplate')+'</a></span>'
                ].join("");
                dom && (dom.innerHTML = QC.String.format(_logoutTemplate, {
                    nickname: QC.String.escHTML(reqData.nickname)//, //做xss过滤
                    //figureurl: reqData.figureurl
                }));

                //这里可以调用自己的保存接口
                var url = api + "UserBind/IsExistedThirdPartyById?uniquelyId=" + openId + "&typeCode=qq&v=" + Math.random();
                $.getJSON(url, function (data) {
                    if (data.Success) {
                        if (data.Data != null) {
                            //用户已经绑定第三方登录，登录用户即可
                            var url = LR.RootPath + '/Login/UserLoginById', data = { userId: data.Data.UserId };
                            $.post(url, data, function (d) {
                                if (d && d.Success && d.Data) {
                                    LR.BindHeadData(d);
                                    //window.location.reload();
                                } else {
                                    $spanError.show().text(Translate('web_Content_Js_load_TP_QQLoginProcess_message_getpostelse'));
                                }
                            }, "json");

                        } else {
                            //用户未绑定第三方登录，跳转至注册、绑定页处理
                            //因昵称字符范围过于广阔，进行转码处理，以防乱码
                            var url = LR.RootPath + "/SSO?relatedId=" + openId + "&typeCode=qq&nickName=" + base64.Encode(reqData.nickname) + "&figureURL=" + encodeURIComponent(reqData.figureurl_qq_2) + "&returnURL=" + encodeURIComponent(window.location.href)
                            window.location.href = url;
                        }
                    }
                });

            });

        }
        //window.location.reload();


    },
    //Sina登录处理逻辑
    SinaLoginProcess: function (o) {
        //这里可以调用自己的保存接口
        var url = api + "UserBind/IsExistedThirdPartyById?uniquelyId=" + o.id + "&typeCode=sina&v=" + Math.random();
        $.getJSON(url, function (data) {
            if (data.Success) {
                if (data.Data != null) {
                    //用户已经绑定第三方登录，登录用户即可
                    var url = LR.RootPath + '/Login/UserLoginById', data = { userId: data.Data.UserId };
                    $.post(url, data, function (d) {
                        if (d && d.Success && d.Data) {
                            LR.BindHeadData(d);
                            //window.location.reload();
                        } else {
                            $spanError.show().text(Translate('web_Content_Js_load_TP_SinaLoginProcess_message_getpostelse'));
                        }
                    }, "json");

                } else {
                    //用户未绑定第三方登录，跳转至注册、绑定页处理
                    //因昵称字符范围过于广阔，进行转码处理，以防乱码
                    var url = LR.RootPath + "/SSO?relatedId=" + o.id + "&typeCode=sina&nickName=" + base64.Encode(o.screen_name) + "&figureURL=" + encodeURIComponent(o.avatar_large) + "&returnURL=" + encodeURIComponent(window.location.href)
                    window.location.href = url;
                }
            }
        });
    },
    //第三方已有帐号绑定并登录
    ThirdPartyBindingAndLogin: function (btnId, accountId, pwdId) {
        var pvrst = LR.PhoneValid(accountId), pdvrst = LR.PasswordValid(pwdId), typeCode = GetQueryString("typeCode"), relatedId = GetQueryString("relatedId"), nickName = GetQueryString("nickName"), figureURL = GetQueryString("figureURL");
        TP.ReturnURL = GetQueryString("returnURL");
        if (!pvrst || !pdvrst) { return; }
        var btn = id$(btnId),
            account = $.trim(id$(accountId).val()),
            pwd = $.trim(id$(pwdId).val()),
            url = LR.RootPath + '/Login/ThirdPartyBindAndLogin',
            data = { account: account, pwd: pwd, typeCode: typeCode, relatedId: relatedId, nickName: nickName, figureURL: figureURL },
            $spanError = span$(pwdId);
        LR.BtnDisable(btn, Translate('web_Content_Js_load_TP_ThirdPartyBindingAndLogin_message_btn'));
        $.post(url, data, function (d) {
            if (d && d.Success && d.Data) {
                LR.BindHeadData(d);
                if (TP.ReturnURL != "")
                    window.location.href = TP.ReturnURL;
                else
                    window.location.href = LR.RootPath;
            } else {
                $spanError.show().text(Translate("web_Content_Js_load_TP_ThirdPartyBindingAndLogin_message_postelse"));
            }
            LR.BtnActive(btn, Translate('web_Content_Js_load_TP_ThirdPartyBindingAndLogin_message_postbtn'));
        }, "json");
    },
    //第三方注册新帐号并绑定
    ThirdPartyBindingAndRegist: function (btnId, accountId, pwdId, agreeId, codeId, formId) {
        TP.ReturnURL = GetQueryString("returnURL");
        var pvrst = LR.PhoneValid(accountId, true, false), pwvrst = LR.PasswordValid(pwdId), cvrst = LR.CodeValid(codeId, accountId), agvrst = true;
        if (!id$(agreeId).is(':checked')) {
            span$(agreeId).show().text(Translate('web_Content_Js_load_TP_ThirdPartyBindingAndRegist_message_ifagreeId'));
            agvrst = false;
        }
        if (!pvrst || !pwvrst || !cvrst || !agvrst) { return; }
        var btn = id$(btnId), url = LR.RootPath + '/Regist/ThirdPartyBindAndRegister',
            data = { tpaccount: $.trim(id$(accountId).val()), tppwd: $.trim(id$(pwdId).val()), tptypeCode: GetQueryString("typeCode"), tprelatedId: GetQueryString("relatedId"), tpnickName: GetQueryString("nickName"), tpfigureURL: GetQueryString("figureURL"), type: LR.AccountType };

        $spanError = span$(agreeId);
        LR.BtnDisable(btn, Translate('web_Content_Js_load_TP_ThirdPartyBindingAndRegist_message_btn'));
        $.post(url, data, function (d) {
            if (d && d.Success) {
                LR.BindHeadData(d);
                LR.RestSmsCodeBtn();
                if (TP.ReturnURL != "")
                    window.location.href = TP.ReturnURL;
                else
                    window.location.href = LR.RootPath;
            } else {
                $spanError.show().text(d.Message);
            }
            LR.BtnActive(btn, Translate('web_Content_Js_load_TP_ThirdPartyBindingAndRegist_message_postbtn'));
        }, "json");
    },
    //第三方自动注册并绑定（跳过）
    AutoRegisterAndBindThirdParty: function (btnId) {
        TP.ReturnURL = GetQueryString("returnURL");
        var btn = id$(btnId), url = LR.RootPath + '/Regist/AutoRegisterAndBindThirdParty',
            data = { typeCode: GetQueryString("typeCode"), relatedId: GetQueryString("relatedId"), nickName: GetQueryString("nickName"), figureURL: GetQueryString("figureURL") };

        btn.text(Translate('web_Content_Js_load_TP_AutoRegisterAndBindThirdParty_message_btn'));
        btn.attr('href', 'javascript:void(0);');
        $.post(url, data, function (d) {
            if (d && d.Success && d.Data) {
                LR.BindHeadData(d);
                //跳回之前页面
                if (TP.ReturnURL != "")
                    window.location.href = TP.ReturnURL;
                else
                    window.location.href = LR.RootPath;
            } else {
                //$spanError.show().text(d.Message);
            }
            btn.text(Translate('web_Content_Js_load_TP_AutoRegisterAndBindThirdParty_message_postbtn'));
            btn.attr('href', "javascript:TP.AutoRegisterAndBindThirdParty('" + btnId + "');");
        }, "json");
    }
}