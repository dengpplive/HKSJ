
$("#btnloadImg").hover(function () {
    $("#load1").hide();
    $("#load2").show();
}, function () {
    $("#load1").show();
    $("#load2").hide();
});

$("#per_home").hover(inFunction, outFunction);
var timer;
function inFunction() {
    clearTimeout(timer);
    timer = setTimeout(function () {
        $("#show_home").show();
    }, 350); //延迟显示框
}
function outFunction() {
    clearTimeout(timer);
    timer = setTimeout(function () {
        $("#show_home").hide();
    }, 350); //延迟隐藏框
};


//隐藏的导航框
$("#daoh").hover(showDiv, hideDiv);
function showDiv() {
    clearTimeout(timer);
    timer = setTimeout(function () {
        $("#show_dao").show();
    }, 350); //延迟显示框
}
function hideDiv() {
    clearTimeout(timer);
    timer = setTimeout(function () {
        $("#show_dao").hide();
    }, 350); //延迟隐藏框
}

//将表单数据转成 Object
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


function addSelectOption($selector, obj, parentValue) {
    $selector.append("<option value = \'" + obj.id + "\' >" + obj.name + "</option>");
}

var DataConvert =
   {
       parseTime: function (data) {
           if (data < 60) {
               return Translate("web_Content_Js_load_DataConvert_miao").Format(parseInt(data));
           }
           var timeText = "";

           var second = parseInt(data % 60);
           var minute = parseInt(data / 60);

           if (minute > 60) {
               var hour = parseInt(minute / 60);
               minute = parseInt(minute % 60);
               timeText += Translate("web_Content_Js_load_DataConvert_xiaoshi").Format(hour);
           }
           timeText += Translate("web_Content_Js_load_DataConvert_fen").Format(minute);
           timeText += second > 0 ? Translate("web_Content_Js_load_DataConvert_miaotwo").Format(second) : "";
           return timeText;
       }
   };


var uploadMain = {
    selectFileId:'',
    uid : null,
    uploader: null,
    videoList: [],
    publicDomain: '',
    privateDomain: '',
    videoToken: '',
    uploadFileId:'',
    videoExtension: ['wmv', 'asf', 'rm', 'rmvb', 'mpg', 'mpeg', 'mpe', 'mov', 'mp4', 'avi', 'mkv', 'flv', 'vob', 'f4v'],
    openWindow: function (obj,fid) {
        uploadMain.selectFileId = fid;
        var imgSrc = $("#" + fid + " .coverImg").attr("src");
        prtImg.OpenPrtWindow(obj,imgSrc);
    },
    categoryList: null,
    IsAllCompleted: function () {
        var rtn = true;
        for (var item in this.videoList) {
            if (!this.videoList[item].uploadCompleted) {
                rtn = false;
                break;
            }
        }
        return rtn;
    },
    bindRefresh: function () {
        window.onbeforeunload = function () {
            return Translate("web_Content_Js_load_uploadMain_bindRefresh_message");
        };
        window.onunload = function () {
            for (var item in uploadMain.videoList) {
                if (!uploadMain.videoList[item].uploadCompleted) {
                    uploadMain.videoList[item].removeVideo();
                }
            }
        };
    },
    unBindRefresh: function () {
        window.onbeforeunload = null;
        window.onunload = null;
    },
    initData: function() {
        $.get(api + "Category/GetOneCategoryViewList", function (data, status) {
            uploadMain.categoryList = data;
        });
        uploadMain.uploader = new QiniuJsSDK().uploader({
            runtimes: 'html5,flash,silverlight,html4',
            browse_button: 'btnload',
            container: 'loadContent',
            multipart: true,
            chunk_size: '4mb',
            rename: true,
            uptoken: uploadMain.videoToken,
            domain: Ads.PrivateDomain,
            flash_swf_url: LR.RootPath + '/Scripts/Moxie.swf',
            silverlight_xap_url: LR.RootPath + '/Scripts/Moxie.xap',
            auto_start: true,
            unique_names: true,
            multi_selection: true,
            max_file_size: '2GB',
            filters: {
                max_file_size: '2GB',
                mime_types: [
                    { title: "Video files", extensions: "*" }
                ]
            },
            init: {
                PostInit: function (up) {
                },
                FilesAdded: function (up, files) {
                    plupload.each(files, function (file) {
                        uploadMain.AddVideo(file);
                    });
                },

                UploadProgress: function (up, file) {
                    uploadMain.uploadFileId = file.id;
                    var speed = plupload.formatSize(file.speed);
                    var loaded = plupload.formatSize(file.loaded);
                    var surplusTime = (file.size - file.loaded) / file.speed;
                    var szSurplusTime = DataConvert.parseTime(surplusTime);
                    $("#" + file.id + " .progressTime").text(szSurplusTime);
                    var percent = file.percent + "%";
                    $("#" + file.id + " .progressbar").width(percent);
                    $("#" + file.id + " .progressbarText").text(percent);
                    var ss = parseInt(speed);
                    if (speed && !isNaN(ss)) {
                        //if (console && console.log) console.log(ss);
                        $("#" + file.id + " .loadSpeed").text(speed.toUpperCase() + "/s");
                    }
                    $("#" + file.id + " .loadSpeedSize").text(loaded.toUpperCase());
                    $("#" + file.id + " .uploadTitle").text(Translate('web_Content_Js_load_initData_UploadProgress_message'));
                },
                FileUploaded: function (up, file, info) {
                    var res = $.parseJSON(info);
                    uploadMain.videoList[file.id].VideUploadCompled(res);
                },
                Error: function (up, err, errTip) {
                    //Catch the Http error in Firefox when use left the page during file uploading.
                    if (errTip.toLocaleUpperCase() == 'HTTP ERROR.') {
                        return;
                    }

                    if (err.code == -600) {
                        //var dotIdx = errTip.lastIndexOf('。(');
                        //var message = errTip.substr(0, dotIdx + 1);
                        //if (message.length <= 0) {
                        //    message = errTip;
                        //}
                        globalPromptBox.showGeneralMassage(1, Translate('web_Content_Js_load_initData_Error_message_daxiao'), 3000, false);
                        return;
                    }
                    var obj = uploadMain.videoList[uploadMain.uploadFileId];
                    
                    obj.$main.find('.am-active').removeClass('am-active');
                    obj.$main.find('.uploadTitle').addClass('m_r_scwc');
                    obj.$main.find('.uploadTitle').text(Translate("web_Content_Js_load_initData_Error_message_schshibai"));
                    obj.$main.find('.uploadTitle').css("color", 'red');
                }
            }
        });
    },
    AddVideo: function(file) {
        var ret = false;
        var dotIndex = file.name.lastIndexOf('.');
        var extension = file.name.substr(dotIndex + 1).toLocaleLowerCase();
        for (var idx in this.videoExtension) {
            if (this.videoExtension[idx] == extension) {
                ret = true;
                break;
            }
        }
        if (!ret) {
            uploadMain.uploader.removeFile(file.id);
            globalPromptBox.showGeneralMassage(1, Translate('web_Content_Js_load_initData_AddVideo_message_schgeshi'), 2000, false);
            return;
        }

        $('#loadContent').hide();
        $('#loadContent3').hide();
        $('#loadContent2').append("<div class=\"filelist\"></div>");

        var video = new VideoModel(file);
        this.videoList[file.id] = video;
        $(".filelist:last").load(LR.RootPath + "/Upload/load2", { fileId: file.id, percent: file.percent }, function (data1) {
            $("#continueAddMore").show();
        });

        this.bindRefresh();
    },
    hasContent: function () {
        var flag = false;
        for (var item in this.videoList) {
            flag = true;
            break;
        }
        return flag;
    }
};

function VideoModel(file) {
    var self = this;
    this.file = file;
    this.InfoTag = null;
    this.propertyData = null;
    this.$main = null;
    this.titleLen = 0;
    this.videoPara = {};
    //添加视频后对数据事件进行绑定
    this.initData = function () {
        this.$main = $('#' + file.id);
        self.DataKey = self.file.name + self.file.size;
        self.$main.find('.l_m_r_title').hide();
        self.$main.find('.load_more_bcsp').hide();
        self.$main.find('.uploadTitle').text(Translate("web_Content_Js_load_VideoModel_initData_message_dengdai"));

        var fileSize = plupload.formatSize(this.file.size);
        this.$main.find('.videoSize').text(fileSize.toUpperCase());
        $("#" + file.id + " .uploadTitle").text(Translate('web_Content_Js_load_VideoModel_initData_message_zhengzai'));


        if (!uploadMain.categoryList) {
            $.get(api + "Category/GetOneCategoryViewList", function (data, status) {
                uploadMain.categoryList = data;
            });
        }
        //视频属性数据绑定
        for (var i = 0; i < uploadMain.categoryList.length; i++) {
            addSelectOption(this.$main.find("[name='category']"), uploadMain.categoryList[i], '');
        }
        //绑定按钮事件
        this.$main.find('.removeUpload').bind('click', function () {
            globalPromptBox.showPromptMessage(Translate('web_Content_Js_load_VideoModel_initData_message_quxiao'),Translate( 'web_Content_Js_load_VideoModel_initData_message_quxiaosure'), self.removeVideo);
        });
        this.$main.find('.btnOpenEdit').bind('click', this.openEdit);

        var filename = '';
        var oldTags = '';
        var localFileInfo = localStorage.getItem(self.DataKey);
        if (localFileInfo) {
            localFileInfo = JSON.parse(localFileInfo);
            filename = localFileInfo.title;
            oldTags = localFileInfo.tags;
            self.$main.find('[name="about"]').val(localFileInfo.about);
            self.$main.find('[name="category"]').val(localFileInfo.category);
            self.$main.find('[name="copyright"]').val(localFileInfo.copyright);
            self.$main.find('[name="isPublic"]').val(localFileInfo.isPublic);

        } else {
            var dotIndex = this.file.name.lastIndexOf('.');
            filename = this.file.name.substr(0, dotIndex).substr(0, 25);
            
        }
        self.$main.find('[name="title"]').val(filename);
        
        //Tag 数据绑定
        var tagId = 'tag_' + this.file.id;
        this.$main.find('.info-box').attr('id', tagId);
        self.InfoTag = new Infotag({
            oldData: null,
            id: tagId
        });
        //判断标题输入不能超过25个字

        changeText(self.$main.find('[name="title"]'), 50);
        
        self.$main.find('[name="category"]').on('change', function () {
            var cid = $(this).val();
            var url = api + 'Tags/GetTagsOfWebByCategoryId?id=' + cid;
            self.InfoTag.changeTag(url);
        });
        self.$main.find('[name="title"]').on('keydown keyup keypress paste change', function () {
            changeText($(this), 50);
        });
        self.$main.find('.btnSaveVideo').click(self.ClickSave);
    };
    this.removeVideo = function () {
        if (self.videoPara.BigPicturePath && self.videoPara.BigPicturePath.length > 0
            && self.videoPara.SmallPicturePath && self.videoPara.SmallPicturePath.length > 0) {
            var delUrl = api + 'QiniuUpload/DelQiniuImage?keys=' + self.videoPara.BigPicturePath + ',' + self.videoPara.SmallPicturePath;
            $.ajax({type: "post",url: delUrl,async: false,success: function (data) {}});
        }
        if (!isNaN(self.videoPara.Id) && self.videoPara.Id > 0) {
            $.ajax({
                type: "post",
                url: api + 'Video/DeleteAVideo',
                data: {vid : self.videoPara.Id },
                async: false,
                success: function (data) {
                }
            });
        }

        uploadMain.uploader.removeFile(self.file.id);

        delete uploadMain.videoList[self.file.id];

        if (!uploadMain.hasContent()) {

            //所有电影取消上传
            $('#loadContent2').html('');
            $('#loadContent3').hide();
            $('#loadContent3').hide();
            $('#loadContent').show();
            $('#continueAddMore').hide();
            uploadMain.unBindRefresh();
        } else {
            if (uploadMain.IsAllCompleted()) {
                //所有电影上传保存完成
                $('#loadContent2').html('');
                $('#loadContent3').show();
                $('#continueAddMore').hide();
                uploadMain.unBindRefresh();
                uploadMain.videoList = [];
            }
        }
        self.$main.parent().remove();
    };
    this.openEdit = function () {
        self.Completed = false;

        self.$main.find('.l_m_r_title').hide();
        self.$main.find('.load_more_bcsp').hide();
        self.$main.find('.load_more_floot').show();
    };
    //点击保存 数据验证
    this.ClickSave = function () {
        self.GetVideoPara();
        //checkinput();
        var flag = 0;
        var titleLen = self.videoPara.Title.len();
        if (titleLen < 4 || titleLen > 50) {
            // globalPromptBox.showGeneralMassage(1, '标题不能为空', 2000, false);
            flag++;
            self.$main.find('[name="title"]').addClass('error_style');
        } else {
            self.$main.find('[name="title"]').removeClass('error_style');
        }
        if (!self.videoPara.Tags || self.videoPara.Tags.length <= 0) {
            self.$main.find('.tag-container ').addClass('error_style');
            flag++;
        } else {
            self.$main.find('.tag-container ').removeClass('error_style');
        }
        if (self.videoPara.CategoryId == '-1') {
            self.$main.find('[name="category"]').addClass('error_style');
            flag++;
        } else {
            self.$main.find('[name="category"]').removeClass('error_style');
        }
        if (self.videoPara.Copyright == -1) {
            self.$main.find('[name="copyright"]').parent().addClass('error_style');
            flag++;
        } else {
            self.$main.find('[name="copyright"]').parent().removeClass('error_style');
        }
        if (self.videoPara.IsPublic == -1) {
            self.$main.find('[name="isPublic"]').parent().addClass('error_style');
            flag++;
        } else {
            self.$main.find('[name="isPublic"]').parent().removeClass('error_style');
        }
        if (flag>0) {
            return false;
        }
        self.Completed = true;

        var data = self.$main.serializeObject();
        data.tags = self.InfoTag.returnString();
        
        localStorage.setItem(self.DataKey, JSON.stringify(data));

        self.SaveToData();
        
        self.$main.find('.l_m_r_title').text(self.videoPara.Title);
        self.$main.find('.l_m_r_title').show();
        self.$main.find('.load_more_bcsp').show();
        self.$main.find('.load_more_floot').hide();
        
        return true;
    };
    //数据保存
    this.SaveToData = function () {
        if (!self.Completed || !(self.videoPara.Id && self.videoPara.Id != '')) {
            return false;
        }
        if (!self.videoPara.Tags || self.videoPara.Tags.length <= 0) {
            return false;
        }
        var url = api + 'Video/UpdateAVideo';
        $.post(url,
            {
                dataModel: JSON.stringify(self.videoPara)
            },
            function (data) {
                if (!data.Success) {
                    self.$main.find('.uploadTitle').text(Translate('web_Content_Js_load_VideoModel_SaveToData_message_savefail'));
                } else {
                    self.$main.find('.uploadTitle').text(Translate('web_Content_Js_load_VideoModel_SaveToData_message_uploadsuccess'));
                    localStorage.removeItem(self.DataKey);
                    self.uploadCompleted = true;
                }
                if (uploadMain.IsAllCompleted()) {
                    //所有电影上传保存完成
                    $('#loadContent2').html('');
                    $('#loadContent3').show();
                    $('#continueAddMore').hide();
                    uploadMain.videoList = [];
                    uploadMain.unBindRefresh();
                }
            }
        );
        return true;
    };
    this.GetVideoPara = function () {
        if (!this.videoPara) {
            this.videoPara = new Object();
        }
        var data = self.$main.serializeObject();
        data.about = $.trim(data.about);
        data.title = $.trim(data.title);
        
        self.$main.find('[name="title"]').val(data.title);
        self.$main.find('[name="about"]').val(data.about);
        
        var categoryId = data.category.substr(data.category.lastIndexOf('|') + 1);


        self.videoPara.Title = data.title;
        self.videoPara.About = data.about;
        self.videoPara.Copyright = data.copyright;
        self.videoPara.IsPublic = data.isPublic;
        self.videoPara.CategoryId = categoryId;
        self.videoPara.Tags = self.InfoTag.returnString();

        return self.videoPara;
    };
    this.VideUploadCompled = function (res) {
        self.$main.find('.am-active').removeClass('am-active');
        self.$main.find('.uploadTitle').addClass('m_r_scwc');
        if (!res.videoId) {
            self.$main.find('.uploadTitle').text(Translate("web_Content_Js_load_VideoModel_VideUploadCompled_message_fail"));
            self.$main.find('.uploadTitle').css("color", 'red');
        } else {
            self.$main.find('.uploadTitle').text(Translate("web_Content_Js_load_VideoModel_VideUploadCompled_message_success"));
        }
        //
        self.$main.find('[name="videoId"]').val(res.videoId);
        self.videoPara.Id = res.videoId;

        if (!self.videoPara.BigPicturePath || self.videoPara.BigPicturePath == '') {
            self.videoPara.BigPicturePath = res.bigKey;
            self.videoPara.SmallPicturePath = res.smallKey;
            self.ShowCover();
        }

        $("#" + file.id + " .removeUpload").hide();
        self.SaveToData();
    };
    this.CoverUploadCompled = function (res) {

        if (self.videoPara.BigPicturePath && self.videoPara.BigPicturePath.length > 0 &&
            self.videoPara.SmallPicturePath && self.videoPara.SmallPicturePath.length > 0) {
            
            var delUrl = api + 'QiniuUpload/DelQiniuImage?keys=' + self.videoPara.BigPicturePath + ',' + self.videoPara.SmallPicturePath;
            $.post(delUrl, function () {
            });
        }

        self.videoPara.BigPicturePath = res.bigKey;
        self.videoPara.SmallPicturePath = res.smallKey;
        self.ShowCover();
    };
    this.ShowCover = function() {
        var imgKey = HeaderBase.GetImgUrlPath(self.videoPara.SmallPicturePath);
        setTimeout(function () {
            self.$main.find('.coverImg').attr("src", imgKey);
        }, 3000);
    };
}
