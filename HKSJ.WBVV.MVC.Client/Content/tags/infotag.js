
function Infotag(data){
	data.oldData=data.oldData || "";//已保存的标签
	data.adviceData=data.adviceData || "";//推荐标签
	data.minChar=data.minChar || 2;//最小字数
	data.maxChar=data.maxChar || 6;//最大字数
	data.AllTag=data.AllTag || 10;//总标签数
	data.url=data.url || "";
	data.isHasTitle=data.isHasTitle || false;//是否添加标题
	
	var This=this;
	this.allTagContent="";
	this.oData=[];
	this.infoBox=document.getElementById(data.id);
	this.infoBox.innerHTML = 
		'<div class="info-item data-tags" id="info-item">' +
			'<div class="tag-container">' +
				//'<h3>标签</h3>' +
			   '<div class="info-tags">' +
				   '<div class="tag-cont"></div>' +
					'<div>' +
						'<input class="tag-input" type="text" placeholder="' + Translate('web_Content_Js_infotag_innerhtml_taginput').Format(data.maxChar) + '">' +
					'</div>' +
				'</div>' +
			'</div>' +
			'<div class="message tag-message"><span class="message_span" id="message_span">' + Translate('web_Content_Js_infotag_innerhtml_messagespan') + '</span><span>' + Translate('web_Content_Js_infotag_innerhtml_spantag').Format(10) + '</span></div>' +
			//'<i class="smark">*</i>' +
		'</div>' +
		'<div style="display: block;" class="info-recom-tag">' +
		'<h3>'+Translate('web_Content_Js_infotag_innerhtml_h3')+':</h3>' +
			'<div class="tag-list">' +
			'</div>' +
		'</div>';
	
	this.oInput = this.infoBox.getElementsByClassName("tag-input")[0],
	this.oInfoItem=this.infoBox.getElementsByClassName("info-item")[0],
	this.oTagContainer = this.infoBox.getElementsByClassName("tag-container")[0],
	this.oMessage = this.infoBox.getElementsByClassName("tag-message")[0],
	this.oMessageSpanLeft=this.oMessage.children[0];
	this.oMessageSpanRight=this.oMessage.children[1];
    this.oTagCont = this.infoBox.getElementsByClassName("tag-cont")[0],
    this.oInfoRecomTag = this.infoBox.getElementsByClassName("info-recom-tag")[0],
	this.oTagList = this.infoBox.getElementsByClassName("tag-list")[0],
	this.oInfoTags = this.infoBox.getElementsByClassName("info-tags")[0],
	this.aSpan = this.oTagList.children,
	this.aTagItem = this.oTagCont.children,
	this.oMessNum = data.AllTag;	
	this.minChar=data.minChar*2;//最小字数
	this.maxChar=data.maxChar*2;//最大字数

	//是否添加标题
	if(data.isHasTitle){
		var oH3=document.createElement("h3");
		var oI=document.createElement("i");
		oI.className="smark";
		oH3.innerHTML=Translate("web_Content_Js_infotag_ishastitle");
		oI.innerHTML="*";
		this.oTagContainer.insertBefore(oH3,this.oTagContainer.children[0]);
		this.oInfoItem.appendChild(oI);
	}		
	
	//设置推荐标签
    if (data.url) {
        $.ajax({
            type: "GET",
            url: data.url,
            success: function(json) {
                if (json.length == 0) {
                    This.oInfoRecomTag.style.display = 'none';
                    return;
                }
                This.oInfoRecomTag.style.display = 'block';
                This.oData = json;
                for (var i = 0; i < json.length; i++) {
                    This.oData[i].index = i;
                }
                //判断是否有保存的标签
				if (data.oldData) {
					var tagArr = data.oldData.split("|");
					for (var i = 0; i < tagArr.length; i++) {
						This.addTag(tagArr[i]);
					}
				}	
            },
            error: function() {
                This.oInfoRecomTag.style.display = 'none';
            }
        });
    } else {
        This.oInfoRecomTag.style.display = 'none';
        //判断是否有保存的标签
        if (data.oldData) {
            var tagArr = data.oldData.split("|");
            for (var i = 0; i < tagArr.length; i++) {
                This.addTag(tagArr[i]);
            }
        }
    }


	//input元素获取焦点
	this.oTagContainer.onclick = function () {
		This.oInput.focus();
	};

	this.oInput.onfocus=function(){
		This.oMessageSpanLeft.style.display="none"
	};	
	
	//input元素失去焦点
	this.oInput.onblur = function () {
		var str = This.oInput.value;
		//判断当时input的值
		if (This.oInput.value) {
			if (This.oMessNum <= 0) {
				This.oInput.value = "";
				return;
			}
			This.addTag(str);
		}
		This.oInput.value = ""
	};
	
	this.oInput.onkeyup = function (ev) {
		var oEvent = ev || event;
		//判断是否按了回车且input值是否为空
		if (oEvent.keyCode == 13) {
			var str = This.oInput.value;
			if (This.oInput.value && This.oMessNum > 0) {
			    This.addTag(str);
			    This.oInput.value = '';
			} else {
			    This.oInput.value = '';
			}
		}
	};
	
	this.oTagList.onclick = function (ev) {
		This.oInput.focus();
		var oEvent = ev || event;
		var oSrc = oEvent.srcElement || oEvent.target;
		//判断是否点击了推荐标签且标签未超出
		if (oSrc.tagName == "SPAN") {
			var str = oSrc.innerHTML.substring(2);
			for(var i=0;i<This.aTagItem.length;i++){
				var num=This.aTagItem[i].innerHTML.indexOf('<');
				var ostr=This.aTagItem[i].innerHTML.substring(0,num);
				if(str == ostr){
					This.oTagList.removeChild(oSrc);
					return;
				}
			}
			if(This.oMessNum > 0){
			This.addTag(str,oSrc);
		}
			
		}
		oEvent.cancelBubble = true;
	};
	
	this.oInfoTags.onclick = function (ev) {
		var oEvent = ev || event;
		var oSrc = oEvent.srcElement || oEvent.target;	
		if (oSrc.tagName == "I") {
		    var oParent = oSrc.parentNode;
			var num=oParent.innerHTML.indexOf('<');
			var str=oParent.innerHTML.substring(0,num);			
			This.oTagCont.removeChild(oParent);
			This.oMessageSpanRight.innerHTML = Translate("web_Content_Js_infotag_oInfoTags_innerhtml").Format(This.oMessNum += 1);
			This.oInput.style.display="block";
			This.oInput.value = "";
			This.allTagContent = This.allTagContent.replace(str + "|", "");
			//把删除的标签放回推荐标签原来的位置
			if (oParent.index || oParent.index == 0) {
			    if (This.oData[0]) {
			        if (This.oData[0].index > oParent.index) {
			            This.oData.unshift({ content: str, index: oParent.index });
			        } else {
			            if (This.oData[This.oData.length - 1].index < oParent.index) {
			                This.oData.push({ content: str, index: oParent.index });
			            }
			            for (var i = 0; i < This.oData.length; i++) {
			                if (This.oData[i].index > oParent.index) {
			                    This.oData.splice(i, 0, { content: str, index: oParent.index });
			                    break;
			                }
			            }
			        }
			    } else {
			        This.oData.unshift({ content: str, index: oParent.index });
			    }
				
			}	
			This.setTagContent(This.oData);
			This.oInput.focus();
			oEvent.cancelBubble=true;
		}
	}	
}
	//添加标签
Infotag.prototype.addTag=function (str,obj) {
		var This=this;
		var index=0;
		var isHasTag=false;
		obj&&(isHasTag=true);
		obj&&(index=obj.index);
		//判断标签是否符合字数  
		if (str.trim() == "" || this.stringlen(str) > this.maxChar) {
			this.oInput.value = "";
			return;
		}
		//判断是否点击了推荐标签
		obj&&this.oTagList.removeChild(obj);
		
		//判断有无重复内容
		for (var i = 0; i < this.aTagItem.length; i++) {
			var num=this.aTagItem[i].innerHTML.indexOf('<');
			var tagStr=this.aTagItem[i].innerHTML.substring(0,num);	
			if (tagStr==str) {
				return;
			}
		}
		
    //清除推荐标签里相同的数据
		for(var i=0;i<this.oData.length;i++){
			if(this.oData[i].content == str){
			    index = this.oData[i].index;
				this.oData.splice(i,1);
				isHasTag = true;
				break;
			}
		}
		this.setTagContent(this.oData);
		
		//添加标签
		var oDiv = document.createElement("div");
		oDiv.className = "tag-item";
		oDiv.innerHTML = str + "<i></i>";
		isHasTag&&(oDiv.index=index);
		this.oTagCont.appendChild(oDiv);
		this.oMessageSpanRight.innerHTML = Translate("web_Content_Js_infotagprototype_addTag_innerHtml").Format(this.oMessNum -= 1);
		if (this.oMessNum <= 0) {
		    this.oInput.style.display = 'none';
		}
    this.allTagContent += str + "|";
};
	//设置推荐标签函数
Infotag.prototype.setTagContent=function(str) {
		this.oTagList.innerHTML="";
		var fragment=document.createDocumentFragment();
		for(var i=0;i<str.length;i++){
			if(i==10){break}
			var oSpan=document.createElement("span");
			oSpan.innerHTML="+ "+str[i].content;
			oSpan.index = i;
			fragment.appendChild(oSpan);
		}
		//console.log(this.oData);
		this.oTagList.appendChild(fragment);
};	

//计算字符数量
Infotag.prototype.stringlen=function(str) {
	var len = 0;
	for (var i = 0; i < str.length; i++) {
		var val = str.charAt(i);
		if (this.isChinese(val)) {
			len += 2;
		} else {
			len += 1;
		}
	}
	return len;
}		
//检测是否是中文
Infotag.prototype.isChinese=function(str) {
	var reCh = new RegExp("[\\u4E00-\\u9FFF]+", "g");
	return reCh.test(str);
}

Infotag.prototype.juidePrompt=function () {//判断提示是否出现
	this.aTagItem.length?this.oMessageSpanLeft.style.display="none":this.oMessageSpanLeft.style.display="block";
}

Infotag.prototype.returnString=function () {//输出最终标签内容函数
	if (this.allTagContent.charAt(this.allTagContent.length - 1) == "|"){
		this.allTagContent = this.allTagContent.substring(0, this.allTagContent.length - 1);
	}
	return this.allTagContent;
}

Infotag.prototype.changeTag=function(url){
	var This=this;
	$.ajax({
		 type: "GET",
		 url:url,
		 success: function (json) {
		     if (json.length == 0) {
		         This.oInfoRecomTag.style.display = 'none';
		         return;
		     }
		     This.oInfoRecomTag.style.display = 'block';
		     This.oData = json;
			 for (var i = 0; i < json.length; i++) {
				This.oData[i].index = i
			 }
			This.setTagContent(This.oData);
		},
		 error: function () {
		     This.oInfoRecomTag.style.display = 'none';
		}
	});
}

