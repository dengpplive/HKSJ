		// 对Date的扩展，将 Date 转化为指定格式的String
        // 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
        // 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
        // 例子： 
        // (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
        // (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
        Date.prototype.Format = function (fmt) { //author: meizz 
            var o = {
                "M+": this.getMonth() + 1, //月份 
                "d+": this.getDate(), //日 
                "h+": this.getHours(), //小时 
                "m+": this.getMinutes(), //分 
                "s+": this.getSeconds(), //秒 
                "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
                "S": this.getMilliseconds() //毫秒 
            };
            if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        }
var messageData = [];
var MsgClassArr=["所有用户","指定用户类别","指定用户"];
function Message(id,msgClass,title,keywords,content,sendTime,
 expiredTime) {
    var self = this;
    self.Id = ko.observable(id);
	self.MsgClass = ko.observable(msgClass);
	self.MsgClassText=MsgClassArr[msgClass];
    self.Title = ko.observable(title);
	self.KeyWords=ko.observable(keywords);
    self.Content = ko.observable(content);
    self.SendTime = ko.observable(sendTime.Format("yyyy-MM-dd"));
    self.ExpiredTime = ko.observable(expiredTime.Format("yyyy-MM-dd"));
}

function InitMessageList() {
    if (messageData == null || messageData.length <= 0) {
        messageData.push(new Message(1, 0, '消息标题 1', "key1,key2","内容 1" ,new Date('2014-05-14 19:44:33'), new Date()));
        messageData.push(new Message(2, 1, '消息标题 2', "key1,key2","内容 2" , new Date('2014-06-24 19:59:22'), new Date()));
        messageData.push(new Message(3, 2, '消息标题 3', "key1,key2","内容 3" ,new Date('2024-05-14 19:23:33'), new Date()));
        messageData.push(new Message(4, 0, '消息标题 4', "key1,key2","内容 4" , new Date('2013-05-14 19:54:33'), new Date()));
    }
}

InitMessageList();

function AppListViewModel() {
    var self = this;
    self.MessageList = ko.observableArray(messageData);
    self.RemoveNews = function (message) {
        $("#delMessageId").val(message.Id());
        $("#confirmMessage").text("确定删除消息：" + message.Title() + "？");
        $("#deletemessage").modal();
    }
    self.EditNews = function (message) {
        window.location.href = 'messagedetail.html?id=' + message.Id();
    }
    self.Detail = ko.observable(null);
}

function DeleteMessage() {
    var id = $("#delMessageId").val();
    //alert(id);
    var delresult = false;
    for (i = 0; i < view.MessageList().length; i++) {
        if (view.MessageList()[i].Id() == Number(id)) {
            $("#deletemessage").modal("hide");
            view.MessageList.remove(view.MessageList()[i]);
            delresult = true;
            break;
        }
    }
    if (delresult) {
        $("#resultmessage").text("删除成功！");
    }
    else {
        $("#resultmessage").text("删除失败！");
    }
    $("#delresult").modal({ backdrop: "static" });
}

function GetMessageById(id) {
    if (messageData != null && messageData.length > 0) {
        for (i = 0; i < messageData.length; i++) {
            if (messageData[i].Id() == Number(id)) {
                return messageData[i];
            }
        }
    }
}

function GetUrlParam(url, name) {
    if (url == null || url.length <= 0) {
        return "";
    }
    var index = url.indexOf('?');
    if (index < 0) {
        return "";
    }

    var keyvalues = url.substr(index + 1).split('&');
    if (keyvalues != null && keyvalues.length > 0) {
        for (i = 0; i < keyvalues.length; i++) {
            var parms = keyvalues[i].split('=');
            if (parms[0] == name) {
                return parms[1];
            }
        }
    }
}