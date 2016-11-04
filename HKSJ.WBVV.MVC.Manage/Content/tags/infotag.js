function Infotag(id, url){
	this.allTagContent="";
	this.tag=document.getElementById(id);
	this.tag.innerHTML = 
		'<div class="info-item data-tags" id="info-item">' +
			'<div class="tag-container" id="tag-container" data-style="error">' +
				'<div class="info-tags">' +
				   '<div class="tag-cont" id="tag-cont"></div>' +
					'<div>' +
						'<input class="tag-input" id="tag-input" type="text" placeholder="' +
Translate("admin_Content_tags_infotagjs_tagLengthTips").Farmat(6) + '">' +
					'</div>' +
				'</div>' +
			'</div>'+
		'</div>';
	//获取元素
	this.oInput = this.tag.getElementsByClassName("tag-input")[0];
	this.oInfoItem = this.tag.getElementsByClassName("info-item")[0];
	this.oTagContainer = this.tag.getElementsByClassName("tag-container")[0];
	this.oTagCont = this.tag.getElementsByClassName("tag-cont")[0];
	this.aTagItem = this.oTagCont.children;		
	var This=this;
	//设置标签
	if(url){
		$.ajax({
			type: "GET",
			url: url,
			success: function (json) {
				for(var i=0;i<json.length;i++){
				    This.addTag(json[i].content, json[i].Id);
				}
			}
		});	
	}	
	//input元素获取焦点
	this.oTagContainer.onclick = function () {
		This.oInput.focus();
	}

	//input元素失去焦点
	this.oInput.onblur = function () {
		var str = this.value;
		if (str) {
			This.addTag(str);
		}
	}
	
	this.oInput.onkeyup = function (ev) {
		var oEvent = ev || event;
		//判断是否按了回车且input值是否为空
		if (oEvent.keyCode == 13) {
			var str = this.value;
			if (this.value) {
				This.addTag(str);
			}
		}
	}	
	
	this.oTagCont.onmousedown=function(ev){
		var oEvent = ev || event;
		var oSrc = oEvent.srcElement || oEvent.target;
		//删除标签
		if(oSrc.tagName == "I"){
			This.oTagCont.removeChild(oSrc.parentNode);
			This.oInput.value = "";
			This.oInput.focus();	
			return;			
		}		
		//移动标签			
		if(oSrc.tagName == "DIV"){
			This.tagDrag(oSrc,oEvent);
		}		
		return false;
	}
}
	
Infotag.prototype.tagDrag=function(oSrc,oEvent){
	var This=this;
	var nearObj;
	this.cloneDiv=null;
	this.cloneDiv=oSrc.cloneNode(true);
	this.oTagCont.appendChild(this.cloneDiv);
	oSrc.style.visibility='hidden';
	var disX=oEvent.clientX-oSrc.offsetLeft;
	var disY = oEvent.clientY - oSrc.offsetTop;
	this.cloneDiv.style.left=oEvent.clientX-disX+'px';
	this.cloneDiv.style.top=oEvent.clientY-disY+'px';				
	
	this.cloneDiv.style.position='absolute';							
		
	document.onmousemove=function(ev){
		var oEvent = ev || event;
		This.cloneDiv.style.left=oEvent.clientX-disX+'px';
		This.cloneDiv.style.top=oEvent.clientY-disY+'px';
		
		nearObj=This.findNearest(This.cloneDiv);//找出离自己最近的					
		if(nearObj && nearObj!=This.cloneDiv){	
			if(nearObj==This.aTagItem[This.aTagItem.length-2]){
				This.oTagCont.appendChild(oSrc);
			}else{
				This.oTagCont.insertBefore(oSrc,nearObj);
			}	
		}				
	}
	
	document.onmouseup=function(){
		document.onmousemove=document.onmouseup=null;	
		This.oTagCont.removeChild(This.cloneDiv);
		oSrc.style.visibility = 'visible';
	}	
}	
	
	
//添加标签
Infotag.prototype.addTag=function (str,id) {	
	var This=this;	
	//判断标签是否符合字数
	if (str.trim()=="" || This.stringlen(str) > 12) {
		this.oInput.value = "";
		return;
	}
	//判断有无重复内容
	for (var i = 0; i < this.aTagItem.length; i++) {
		var num=this.aTagItem[i].innerHTML.indexOf('<');
		var tagStr=this.aTagItem[i].innerHTML.substring(0,num);
		if (tagStr==str) {
			this.oInput.value = ""
			return;
		}
	}
	//添加标签
	var oDiv = document.createElement("div");
	oDiv.className = "tag-item";
	if (id) {
	    oDiv.id = id;
    }
	oDiv.innerHTML = str +"<i></i>";
	var oI=oDiv.getElementsByTagName('i')[0];
	this.oTagCont.appendChild(oDiv);
	this.oInput.value='';
};

//碰撞&&找最近
Infotag.prototype.findNearest=function(obj){
	var minDis=99999999;
	var minDisIndex=-1;
	for(var i=0;i<this.aTagItem.length;i++){
		if(obj==this.aTagItem[i]) continue;
		if(this.collTest(obj,this.aTagItem[i])){
			//撞到了,第一关过了
			var dis=this.getDis(obj,this.aTagItem[i]);//求距离
			if(dis<minDis){	//第二关里取最小
				minDis=dis;
				minDisIndex=i;
			}
		}
	}
	if(minDisIndex==-1){
		return null;	
	}else{
		return this.aTagItem[minDisIndex];
	}
}

//求距离
Infotag.prototype.getDis=function(obj1,obj2){
	var a= obj1.offsetLeft-obj2.offsetLeft;
	var b= obj1.offsetTop-obj2.offsetTop;
	return	Math.sqrt(a*a+b*b);
}
//碰撞检测，检测是否碰撞到
Infotag.prototype.collTest=function(obj1, obj2){
	var l1=obj1.offsetLeft;
	var t1=obj1.offsetTop;
	var r1=obj1.offsetLeft+obj1.offsetWidth;
	var b1=obj1.offsetTop+obj1.offsetHeight;
	//a碰撞里找obj2的位置
	var l2=obj2.offsetLeft+obj2.offsetWidth/2;
	var t2=obj2.offsetTop+obj2.offsetHeight/2;
	var r2=obj2.offsetLeft+obj2.offsetWidth/2;
	var b2=obj2.offsetTop+obj2.offsetHeight/2;
	if(l1>r2 || t1>b2 || r1<l2 || b1<t2){
		return false;
	}else{
		return true;
	}
}

Infotag.prototype.getPos=function(obj){
	var json={l:0,t:0};
	while(obj){
		json.l+=obj.offsetLeft;
		json.t+=obj.offsetTop;
		obj=obj.offsetParent;
	}
	return json;
}
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
//得到所有标签内容
Infotag.prototype.returnString=function() {
	this.allTagContent='';
	for(var i=0;i<this.aTagItem.length;i++){
		var num=this.aTagItem[i].innerHTML.indexOf('<');
		var str=this.aTagItem[i].innerHTML.substring(0,num);
		this.allTagContent+=str+'|'
	}
	this.allTagContent = this.allTagContent.substring(0, this.allTagContent.length - 1);
	return this.allTagContent;	
}						
