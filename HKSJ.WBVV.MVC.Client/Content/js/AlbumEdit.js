
var wm_album = function() {
    var self = this;
    //专辑创建时间
    self.Title = ko.observable("");
    //简介
    self.Remark = ko.observable("");
    //标签
    self.Tag = ko.observable("");
    //缩略图
    self.Image = "";
    self.InfoTag = null;
    //数据赋值方法
    self.doAssignment = function(data) {
        self.Title(data.Title);
        self.Remark(data.Remark);
        self.Tag(data.Tag);
        if (data.Image && data.Image.length >= 0) {
            $("#imgCover").attr("src", data.Image);
        }
    };
    self.Id = self.load = function() {
        self.Id = GetQueryString("albumId");
        if (self.Id != null) {
            var url = api + 'UserSpecial/GetEditUserAlbum?userId=' + userId + '&albumsId=' + self.Id + '&v=' + Math.random();
            $.ajax({ type: "Get", url: url, async: false, success: function(data) {
                self.doAssignment(data);
            }});
        }
        self.initMethod();
    };
    self.initMethod = function()
    {

        self.InfoTag = new Infotag({
            oldData: self.Tag(),
            id: 'tag-box'
        });
        
        $("#txtTitle").on('keydown keyup keypress paste change', function () {
            changeText($(this), 50);
        });

        if (self.Remark()) $("#remarkLength").text(self.Remark().length + '/150');
        $("#txtAlbumRemark").on('keydown keyup keypress paste change', function () {
            var remarkLength = $(this).val().length;
            $("#remarkLength").text(remarkLength + '/150');
        });

        prtImg.initData(function (res) {
            //self.Image(res.key);
            if (res.key && res.key.length >= 0) {
                self.Image = res.key;
                var imgsrc = HeaderBase.GetImgUrlPath(res.key);
                setTimeout(function () {
                    $("#imgCover").attr("src", imgsrc);
                }, 3000);
            }
        }, 'album', 400, "专辑封面编辑", self.Id);
    };
    self.cancel = function () {
        if (self.Id != null) {
            window.location = "AlbumVideos?albumId=" + self.Id;
        } else {
            window.location = "UserAlbums";
        }
    };
    self.confirm = function () {
        self.Tag = self.InfoTag.returnString();
        var album = {
            userId : userId,
            Id: self.Id,
            Title: self.Title(),
            Remark: self.Remark(),
            Tag: self.Tag,
            Image: self.Image
        };
        if (!self.verfication(album)) {
            return;
        }
        if (self.Id == null) {
            $.post(api + 'UserSpecial/AddUserAlbum', album, function (data) {
                if (!data.Success) {
                    
                } else {
                    window.location = "AlbumVideos?albumId=" + data.Data;
                }
            });
        } else {
            $.post(api + 'UserSpecial/EditUserAlbum', album, function (data) {
                if (!data.Success) {
                    
                } else {
                    window.location = "AlbumVideos?albumId=" + self.Id;
                }
            });
        }
    };
    self.verfication = function (data) {
        var flag = 0;
        if (data.Title.length <= 0) {
            $('#txtTitle').addClass('error_style');
            flag++;
        } else {
            $('#txtTitle').removeClass('error_style');
        }
        if (data.Tag.length <= 0) {
            $('#tag-box .tag-container').addClass('error_style');
            flag++;
        } else {
            $('#tag-box .tag-container').removeClass('error_style');
        }
        if (flag > 0) {
            return false;
        }
        return true;
    },
    self.openDialog = function (obj) {
        var imgSrc = $("#imgCover").attr("src");
        prtImg.OpenPrtWindow(obj, imgSrc);
    };
    self.load();
};

var wm_albumEntity = new wm_album();
$(function () {
    ko.applyBindings(wm_albumEntity, document.getElementById('divAlbumVideo'));
});