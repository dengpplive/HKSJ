
function setPageNum(id,currentNum,allPageNum,showNumTotal,fn){
	var oParent=document.getElementById(id);//页数的祖父
	if(allPageNum<=1){
		oParent.style.display='none';
		return;
	}
	oParent.innerHTML='<a class="" href="javascript:;">下一页</a><p></p><a class="" href="javascript:;">上一页</a>';
	var $oP=$(oParent).find('p');//页数的父级
	var oNext=oParent.children[0];//下一页
	var oPrve=oParent.children[oParent.children.length-1];//上一页
	var now=currentNum;//当前页数
	var fragment=document.createDocumentFragment();
	for(var i=0;i<allPageNum;i++){
		var oA=document.createElement("a");
		oA.innerHTML=i+1;
		oA.href='javascript:;';
		fragment.appendChild(oA);
	}
	$oP.append(fragment);
	var aPage=$oP.children();//所有页数

//初始化
	showPage(now);

	//给页数的父级添加事件委托
	$oP.on('click','a',function(){
		var index=$(this).html();
		//阻止用户点击...
		if(index=='...'){return}
		now=index-1;
		showPage(now);
		fn(now);
	});

	//点击上一页
	oPrve.onclick=function(){
		now--;
		if(now==-1){
			now=aPage.length-1;
		}
		showPage(now);
		fn(now);
	}
	//点击下一页
	oNext.onclick=function(){
		now++;
		if(now==aPage.length){
			now=0;
		}		
		showPage(now);
		fn(now);
	}	

	//控制页数的显示
	function showPage(index){
		//替换页码选中状态
		for(var j=0;j<aPage.length;j++){
			aPage[j].className='';
		}
		aPage[index].className='my_m_n_f_curr';
		if(allPageNum<=5){
			return;
		}
		//点击的页码少于3时
		if(index<2){
			//隐藏不需要的页码
			for(var i=1;i<aPage.length-1;i++){
				if(i<=2){
					aPage[i].style.display='block';
					aPage[i].innerHTML=i+1;	
				}else{
					aPage[i].style.display='none';
				}
			}
			//显示选中页码的上一页和下一页
			if(index==1){
				aPage[index+1].style.display='block';
				aPage[index+1].innerHTML=index+2;
			}	
			//不需要的页码统一成...
			aPage[3].style.display='block';	
			aPage[3].innerHTML='...';
		}else if(index>=aPage.length-2){//点击的页码大于最后三页时
			//隐藏不需要的页码
			for(var i=1;i<aPage.length-1;i++){
				if(i>=aPage.length-3){
					aPage[i].style.display='block';
					aPage[i].innerHTML=i+1;	
				}else{
					aPage[i].style.display='none';
				}
			}		
			//显示选中页码的上一页
			if(index==aPage.length-2){
				aPage[index-1].innerHTML=index;
				aPage[index-1].style.display='block';
			}
			//不需要的页码统一成...
			aPage[aPage.length-4].style.display='block';
			aPage[aPage.length-4].innerHTML='...';
		}else{
			//隐藏不需要的页码
			aPage[index].innerHTML=index+1;
			for(var i=1;i<aPage.length-1;i++){
				if(i<index-showNumTotal || i>index+showNumTotal){
					aPage[i].style.display='none';
				}
			}
			//防止第一页被显示成...
			if(index-showNumTotal-1>0){
				aPage[index-showNumTotal-1].style.display='block';
				aPage[index-showNumTotal-1].innerHTML='...';
				//只有一页隐藏的不变...
				if(index-showNumTotal-1==1){
					aPage[index-showNumTotal-1].innerHTML=index-showNumTotal;
					aPage[index-showNumTotal-1].style.display='block';
				}
			}
			//防止最后一页被显示成...
			if(index+showNumTotal+1 <aPage.length-1){
				aPage[index+showNumTotal+1].style.display='block';
				aPage[index+showNumTotal+1].innerHTML='...';
				//只有一页隐藏的不变...
				if(index+showNumTotal+1==aPage.length-2){
					aPage[index+showNumTotal+1].innerHTML=index+showNumTotal+2;
					aPage[index+showNumTotal+1].style.display='block';
				}
			}
			//显示选中页码的上一页和下一页
			for(var i=1;i<=showNumTotal;i++){
				aPage[index-i].innerHTML=index-i+1;
				aPage[index-i].style.display='block';
				aPage[index+i].innerHTML=index+i+1;
				aPage[index+i].style.display='block';
			}
		}
	}
}