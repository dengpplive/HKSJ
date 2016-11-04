var UserMenu = function () {
    var self = this;
    self.menus = ko.observableArray([
        {
            Id: 2,
            Name: Translate("web_Content_Js_userMenu_menus_video"),
            IsCheck: ko.observable(false),
            Iocn: ko.observable("i_index lis_2"),
            Url: rootPath + "/UserCenter/UserVideo"
        },
        {
            Id: 3,
            Name: Translate("web_Content_Js_userMenu_menus_album"),
            IsCheck: ko.observable(false),
            Iocn: ko.observable("i_index lis_3"),
            Url: rootPath + "/UserCenter/UserAlbums"
        },
        {
            Id: 4,
            Name: Translate("web_Content_Js_userMenu_menus_rss"),
            IsCheck: ko.observable(false),
            Iocn: ko.observable("i_index lis_4"),
            Url: rootPath + "/UserCenter/MyFans"
        },
        {
            Id: 5,
            Name: Translate("web_Content_Js_userMenu_menus_record"),
            IsCheck: ko.observable(false),
            Iocn: ko.observable("i_index lis_5"),
            Url: rootPath + "/UserCenter/UserHistory"
        },
        {
            Id: 6,
            Name: Translate("web_Content_Js_userMenu_menus_newscenter"),
            IsCheck: ko.observable(false),
            Iocn: ko.observable("i_index lis_6"),
            Url: rootPath + "/UserCenter/Messager"
        },
        {
            Id: 7,
            Name: Translate("web_Content_Js_userMenu_menus_collect"),
            IsCheck: ko.observable(false),
            Iocn: ko.observable("i_index lis_7"),
            Url: rootPath + "/UserCenter/UserCollect"
        },
        {
            Id: 8,
            Name: Translate("web_Content_Js_userMenu_menus_account"),
            IsCheck: ko.observable(false),
            Iocn: ko.observable("i_index lis_8"),
            Url: rootPath + "/UserCenter/AccountSet"
        }
    ]);
    self.getlistId = function (id) {
        return "lis_" + id;
    }
    self.initMenus = function () {

    };
    self.initCheckMenus = function (id) {
        var menuId = 1;
        if (!isNaN(id)) {
            menuId = id;
        }
        var meuns = self.menus();
        for (var i = 0; i < meuns.length; i++) {
            var menu = meuns[i];
            menu.IsCheck(menu.Id === menuId);
            if (menu.Id === menuId) {
                menu.Iocn("i_index act" + menu.Id);
            }
        }
    };
};
