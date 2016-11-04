var UserInfo = function (userId) {
    var self = this;
    var loading = '<img src="' + rootPath + '/Content/images/icon_img/loading_16X16.gif" width="16" height="16" style="width:16px;height:16px;border:0;">';
    self.user = ko.observable({});
    self.playCount = ko.observable("-1");
    self.fansCount = ko.observable("-1");
    self.nickName = ko.observable('');
    self.playCountText = ko.computed(function () {
        if (self.playCount() == "-1")
            return loading;
        else
            return self.playCount();
    }, self);
    self.fansCountText = ko.computed(function () {
        if (self.fansCount() == "-1")
            return loading;
        else
            return self.fansCount();
    }, self);

    self.nickNameText = ko.computed(function () {
        if (self.nickName() == "")
            return loading;
        else
            return self.nickName();
    }, self);
    self.initUserData = function () {
        var url = api + "User/GetUser?id=" + userId + "&v=" + Math.random();
        $.getJSON(url, function (data) {
            try {
                self.user(data);
                self.playCount(window.numberStr(data.PlayCount));
                self.fansCount(window.numberStr(data.FansCount));
                self.nickName(data.NickName);
            } finally {
            }
        });
    };
};
