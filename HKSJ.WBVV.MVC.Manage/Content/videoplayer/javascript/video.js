/*
 *该js为视频网站通用方法的存放处 
 */


//获取广告
function getArtInfo()
{
      // return  "http://7xliow.com2.z0.glb.qiniucdn.com/1441172688325_600x480.mp4"+","+"10"+","+"http://1.178pb.com"+"#";  
      ////return "http://7xliow.com2.z0.glb.qiniucdn.com/1441172688325_600x480.mp4"+","+"10"+","+"http://jd.com"+"#" + "http://7xliow.com2.z0.glb.qiniucdn.com/1442044625323_A.mp4"+","+"15"+","+"http://1.178pb.com"+"#";   
	
}

//获取视频文件信息
function getVideoInfo() {
   // return "0"+","+"0"+","+""+","+$("#videoPath").val();
    // return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442574781789a.main.m3u8";      // 标清   少年神探狄仁杰 1.片头(时间秒数) 2.片尾(时间秒数)  3.是否下一集  4.视频URL
    //return  "0"+","+"0"+","+"Y"+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442285938560a.main.m3u8";    // 标清   郑秀晶
    //return  "0"+","+"0"+","+"Y"+","+"http://7xliow.com2.z0.glb.qiniucdn.com/1441159565295a_192k.m3u8";    // 标清    UFO 
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442556294231.main.m3u8";      // 标清    神盾局特工第二季
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442289303332a.main.m3u8";       // 高清   澳门风云
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442301154128a.main.m3u8";      // 高清   父爱如山
    //return  "0"+","+"0"+","+""+","+"http://7xlsse.com2.z0.glb.qiniucdn.com/1442569223654a.main.m3u8";      // 高清    传奇酒馆

}

// 暂停广告的图片
function pausePlayback(){
		//return 	"http://vupload.365sji.com:8082/images/chenjiezong/1434016313668.jpg" + "," + "http://www.baidu.com";
		return 	"http://192.168.33.219/videoplayer/ad.jpg" + "," + "http://www.baidu.com";
}

// 播完广告，送播币
function advertOver(){
	//alert("哈哈，看完广告了，可以赚钱啦 ^_^");
}

//  开始播放视频
function playVideoStart(){
   // alert("播放视频了，计数开始");
   
}

//返回首页
function goToHomePage() {
    var url = window.location.href;
    if (url.indexOf("QA")!=-1) {
        window.location.href = "http://192.168.45.19/QA/Admin/";
    } else {
        window.location.href = "http://192.168.45.19/Admin/";
    }
}