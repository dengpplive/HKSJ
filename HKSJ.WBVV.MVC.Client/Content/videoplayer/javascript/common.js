/*
 * HTML加载渲染SWF文件
 * @param flashDivId 渲染显示SWF的div id
 * @param swfPath    SWF文件路径
 * @param param      传入swf的参数
 */
function renderFlash(flashDivId, swfPath, param) {	
	// For version detection, set to min. required Flash Player version, or 0 (or 0.0.0), for no version detection. 
	var swfVersionStr = "11.1.0";
	// To use express install, set to playerProductInstall.swf, otherwise the empty string. 
	var xiSwfUrlStr = "playerProductInstall.swf";
	var flashvars = {divid:flashDivId,  controlbar:param.controlbar,  httpUrl:param.httpUrl,  httpShotUrl:param.httpShotUrl,  crossDomainPath:param.crossDomainPath, videoInfo:param.videoInfo,  advertInfo:param.advertInfo};
	var params = {};
	params.quality = "high";
	params.bgcolor = "#ffffff";
	params.allowscriptaccess = "always";
	params.allowfullscreen = "true";
	
	if (param.wmode) {
		params.wmode = param.wmode;
	}
	
	var attributes = {};
	attributes.id = flashDivId + "_huixin";
	attributes.name = flashDivId + "_huixin";
	attributes.align = "middle";
	
	swfobject.embedSWF(
	    swfPath, flashDivId, 
	    param.flashWidth, param.flashHeight, 
	    swfVersionStr, xiSwfUrlStr, 
	    flashvars, params, attributes);
	// JavaScript enabled so display the flashContent div in case it is not replaced with a swf object.
	//swfobject.createCSS("#flashContent", "display:block;text-align:left;");
}

/**
 * 获取SWF对象
 * @param flashDivId
 * @returns
 */
function getSWF(flashDivId) {
	var swfid = flashDivId + "_huixin";
	
	var browserName = navigator.appName.toLowerCase();
	
	if (browserName.indexOf("microsoft") >= 0)
	{
		return window[swfid];
	} 
	else
	{
	    return document[swfid];
	}
}

function getWindowSize() {
	var sw =  window.screen.width; 
	var sh =  window.screen.height; 
	
	return sw + "|" + sh;
}

/*
 * 检查浏览器是否有flashplayer
 */
function chkFlashPlayer(){
	try {
		var hasFlash = false; 
		var flashVersion = 0; 

		if(document.all)
		{
			var swf = new ActiveXObject('ShockwaveFlash.ShockwaveFlash');
			
			if(swf) {
				hasFlash = true;
				var vSwf = swf.GetVariable("$version");
				flashVersion = parseInt(vSwf.split(" ")[1].split(",")[0]);
			}
		}
		else {
			if (navigator.plugins && navigator.plugins.length > 0)
			{
				var swf = navigator.plugins["Shockwave Flash"];
				
				if (swf)
			    {
					hasFlash = true;
			        var words = swf.description.split(" ");
			        
			        for (var i = 0; i < words.length; ++i)
					{
			        	if (isNaN(parseInt(words[i]))) continue;
			        	flashVersion = parseInt(words[i]);
					}
			    }
			}
		}
	}
	catch (e)
	{
	}

	return hasFlash;
}

