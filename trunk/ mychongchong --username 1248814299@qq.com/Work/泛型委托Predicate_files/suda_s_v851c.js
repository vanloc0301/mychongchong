document.write("<div style='position:absolute;top:0;left:0;width:0;height:0;z-index:1'><div style='position:absolute;top:0;left:0;width:1;height:1;'><iframe id='SUDA_FC' src='' width=1 height=1 SCROLLING=NO FRAMEBORDER=0></iframe></div><div style='position:absolute;top:0;left:0;width:0;height:0;visibility:hidden' id='SUDA_CS_DIV'></div></div>");
var SSL={Config:{},Space:function(str){var a=str,o=null;a=a.split('.');o=SSL;for(i=0,len=a.length;i<len;i++){o[a[i]]=o[a[i]]||{};o=o[a[i]]}return o}};
SSL.Space('Global');SSL.Space('Core.Dom');SSL.Space('Core.Event');SSL.Space('App');SSL.Global={win:window||{},doc:document,nav:navigator,loc:location};SSL.Core.Dom={get:function(id){return document.getElementById(id)}};SSL.Core.Event={on:function(){}};
SSL.App={
	_S_gConType:function(){var ct="";try{SSL.Global.doc.body.addBehavior("#default#clientCaps");ct=SSL.Global.doc.body.connectionType}catch(e){ct="unkown"}return ct},
	_S_gKeyV:function(src,k,e,sp){if(src==""){return""}if(sp==""){sp="="}k=k+sp;var ps=src.indexOf(k);if(ps<0){return""}ps=ps+k.length;var pe=src.indexOf(e,ps);if(pe<ps){pe=src.length}return src.substring(ps,pe)},
	_S_gUCk:function(ckName){if((undefined==ckName)||(""==ckName))return"";return SSL.App._S_gKeyV(SSL.Global.doc.cookie,ckName,";","")},
	_S_sUCk:function(ckName,ckValue,ckDays,ckDomain){if(ckValue!=null){if((undefined==ckDomain)||(null==ckDomain)){ckDomain="sina.com.cn";}if((undefined==ckDays)||(null==ckDays)||(''==ckDays)){SSL.Global.doc.cookie=ckName+"="+ckValue+";domain="+ckDomain+";path=/";}else{var now=new Date();var time=now.getTime();time=time+86400000*ckDays;now.setTime(time);time=now.getTime();SSL.Global.doc.cookie=ckName+"="+ckValue+";domain="+ckDomain+";expires="+now.toUTCString()+";path=/";}}},
	_S_gJVer:function(_S_NAV_,_S_NAN_){var p,appsign,appver,jsver=1.0,isN6=0;if('MSIE'==_S_NAN_){appsign='MSIE';p=_S_NAV_.indexOf(appsign);if(p>=0){appver=parseInt(_S_NAV_.substring(p+5));if(3<=appver){jsver=1.1;if(4<=appver){jsver=1.3}}}}else if(("Netscape"==_S_NAN_)||("Opera"==_S_NAN_)||("Mozilla"==_S_NAN_)){jsver=1.3;appsign='Netscape6';p=_S_NAV_.indexOf(appsign);if(p>=0){jsver=1.5}}return jsver},
	_S_gFVer:function(nav){var ua=SSL.Global.nav.userAgent.toLowerCase();var flash_version=0;if(SSL.Global.nav.plugins&&SSL.Global.nav.plugins.length){var p=SSL.Global.nav.plugins['Shockwave Flash'];if(typeof p=='object'){for(var i=10;i>=3;i--){if(p.description&&p.description.indexOf(' '+i+'.')!=-1){flash_version=i;break}}}}else if(ua.indexOf("msie")!=-1&&ua.indexOf("win")!=-1&&parseInt(SSL.Global.nav.appVersion)>=4&&ua.indexOf("16bit")==-1){for(var i=10;i>=2;i--){try{var object=eval("new ActiveXObject('ShockwaveFlash.ShockwaveFlash."+i+"');");if(object){flash_version=i;break}}catch(e){}}}else if(ua.indexOf("webtv/2.5")!=-1){flash_version=3}else if(ua.indexOf("webtv")!=-1){flash_version=2}return flash_version},
	_S_gMeta:function(MName,pidx){var pMeta=SSL.Global.doc.getElementsByName(MName);var idx=0;if(pidx>0){idx=pidx}return(pMeta.length>idx)?pMeta[idx].content:""},
	_S_gHost:function(sUrl){var r=new RegExp('^http(?:s)?\://([^/]+)','im');if(sUrl.match(r)){return sUrl.match(r)[1].toString()}else{return""}},
	_S_gDomain:function(sHost){var p=sHost.indexOf('.sina.');if(p>0){return sHost.substr(0,p)}else{return sHost}},
	_S_gTJMTMeta:function(){return SSL.App._S_gMeta("mediaid")},
	_S_gTJZTMeta:function(){var zt=SSL.App._S_gMeta('subjectid');zt.replace(",",".");zt.replace(";",",");return zt},
	_S_isFreshMeta:function(){var ph=SSL.Global.doc.documentElement.innerHTML.substring(0,1024);var reg=new RegExp("<meta\\s*http-equiv\\s*=((\\s*refresh\\s*)|(\'refresh\')|(\"refresh\"))\s*content\s*=","ig");return reg.test(ph)},
	_S_isIFrameSelf:function(minH,minW){if(SSL.Global.win.top==SSL.Global.win){return false}else{try{if(SSL.Global.doc.body.clientHeight==0){return false}if((SSL.Global.doc.body.clientHeight>=minH)&&(SSL.Global.doc.body.clientWidth>=minW)){return false}else{return true}}catch(e){return true}}},
	_S_isHome:function(curl){var isH="";try{SSL.Global.doc.body.addBehavior("#default#homePage");isH=SSL.Global.doc.body.isHomePage(curl)?"Y":"N"}catch(e){isH="unkown"}return isH;}
}
function SUDA(config,ext1,ext2){
	var SG=SSL.Global,SSD=SSL.Core.Dom,SSE=SSL.Core.Event,SA=SSL.App;var _S_JV_="webbug_meta_ref_mod_noiframe_async_fc_:9.10c",_S_DPID_="-9999-0-0-1";var _S_NAN_=SG.nav.appName.indexOf('Microsoft Internet Explorer')>-1?'MSIE':SG.nav.appName;var _S_NAV_=SG.nav.appVersion;var _S_PURL_=SG.loc.href.toLowerCase();var _S_PREF_=SG.doc.referrer.toLowerCase();var _SP_MPID_="";var _S_PID_="",_S_UNA_="SUP",_S_MI_="",_S_SID_="Apache",_S_GID_="SINAGLOBAL",_S_LV_="ULV",_S_UO_="UOR",_S_UPA_="_s_upa",_S_IFW=320,_S_IFH=240,_S_GIDT=0,_S_EXT1="",_S_EXT2="",_S_SMC=0,_S_SMM=10000,_S_ET=0,_S_ACC_="_s_acc";var _S_HTTP=_S_PURL_.indexOf('https')>-1?'https://':'http://',_S_BCNDOMAIN="beacon.sina.com.cn",_S_CP_RF=_S_HTTP+_S_BCNDOMAIN+"/a.gif",_S_CP_RF_D=_S_HTTP+_S_BCNDOMAIN+"/d.gif",_S_CP_RF_E=_S_HTTP+_S_BCNDOMAIN+"/e.gif",_S_CP_FC=_S_HTTP+_S_BCNDOMAIN+"/fc.html";
	var _S_T1=100,_S_T2=1000;
	var handler={
		_S_sSID:function(){handler._S_p2Bcn("",_S_CP_RF_D)},_S_gsSID:function(){var sid=SA._S_gUCk(_S_SID_);if(""==sid){handler._S_sSID()}return sid},
		_S_sGID:function(gid){if(""!=gid){SA._S_sUCk(_S_GID_,gid,3650)}},_S_gGID:function(){return SA._S_gUCk(_S_GID_)},_S_gsGID:function(){if(""!=_S_GID_){var gid=SA._S_gUCk(_S_GID_);if(""==gid){handler._S_IFC2GID()}return gid}else{return""}},_S_IFC2GID:function(){var _S_ifc=SSD.get("SUDA_FC");if(_S_ifc){_S_ifc.src=_S_CP_FC+"?a=g&n="+_S_GID_+"&r="+Math.random()}},
		_S_gCid:function(){try{var metaTxt=SA._S_gMeta("publishid");if(""!=metaTxt){var pbidList=metaTxt.split(",");if(pbidList.length>0){if(pbidList.length>=3){_S_DPID_="-9999-0-"+pbidList[1]+"-"+pbidList[2]}return pbidList[0]}}else{return"0"}}catch(e){return"0"}},
		_S_gAEC:function(){return SA._S_gUCk(_S_ACC_)},_S_sAEC:function(eid){if(""==eid){return}var acc=handler._S_gAEC();if(acc.indexOf(eid+",")<0){acc=acc+eid+",";}SA._S_sUCk(_S_ACC_,acc,7);},
		_S_p2Bcn:function(q,u){var scd=SSD.get("SUDA_CS_DIV");if(null!=scd){var now=new Date();scd.innerHTML="<img width=0 height=0 src='"+u+"?"+q+"&gUid_"+now.getTime()+"' border='0' alt='' />"}},
		_S_gSUP:function(){if(_S_MI_!=""){return _S_MI_}var sup=unescape(SA._S_gUCk(_S_UNA_));if(sup!=""){var ag=SA._S_gKeyV(sup,"ag","&","");var user=SA._S_gKeyV(sup,"user","&","");var uid=SA._S_gKeyV(sup,"uid","&","");var sex=SA._S_gKeyV(sup,"sex","&","");var bday=SA._S_gKeyV(sup,"dob","&","");_S_MI_=ag+":"+user+":"+uid+":"+sex+":"+bday;return _S_MI_}else{return""}},
		_S_gsLVisit:function(sid){var lvi=SA._S_gUCk(_S_LV_);var lva=lvi.split(":");var lvr="";if(lva.length>=6){if(sid!=lva[4]){var lvn=new Date();var lvd=new Date(parseInt(lva[0]));lva[1]=parseInt(lva[1])+1;if(lvn.getMonth()!=lvd.getMonth()){lva[2]=1}else{lva[2]=parseInt(lva[2])+1}if(((lvn.getTime()-lvd.getTime())/86400000)>=7){lva[3]=1}else{if(lvn.getDay()<lvd.getDay()){lva[3]=1}else{lva[3]=parseInt(lva[3])+1}}lvr=lva[0]+":"+lva[1]+":"+lva[2]+":"+lva[3];lva[5]=lva[0];lva[0]=lvn.getTime();SA._S_sUCk(_S_LV_,lva[0]+":"+lva[1]+":"+lva[2]+":"+lva[3]+":"+sid+":"+lva[5],360)}else{lvr=lva[5]+":"+lva[1]+":"+lva[2]+":"+lva[3]}}else{var lvn=new Date();lvr=":1:1:1";SA._S_sUCk(_S_LV_,lvn.getTime()+lvr+":"+sid+":",360)}return lvr},
		_S_gUOR:function(){var uoc=SA._S_gUCk(_S_UO_);var upa=uoc.split(":");if(upa.length>=2){return upa[0]}else{return""}},
		_S_sUOR:function(){var uoc=SA._S_gUCk(_S_UO_),uor="",uol="",up_t="",up="";var re=/[&|?]c=spr(_[A-Za-z0-9]{1,}){3,}/;var ct=new Date();if(_S_PURL_.match(re)){up_t=_S_PURL_.match(re)[0]}else{if(_S_PREF_.match(re)){up_t=_S_PREF_.match(re)[0]}}if(up_t!=""){up_t=up_t.substr(3)+":"+ct.getTime()}if(uoc==""){if(SA._S_gUCk(_S_LV_)==""&&SA._S_gUCk(_S_LV_)==""){uor=SA._S_gDomain(SA._S_gHost(_S_PREF_));uol=SA._S_gDomain(SA._S_gHost(_S_PURL_))}SA._S_sUCk(_S_UO_,uor+","+uol+","+up_t,365)}else{var ucg=0,uoa=uoc.split(",");if(uoa.length>=1){uor=uoa[0]}if(uoa.length>=2){uol=uoa[1]}if(uoa.length>=3){up=uoa[2]}if(up_t!=""){ucg=1}else{var upa=up.split(":");if(upa.length>=2){var upd=new Date(parseInt(upa[1]));if(upd.getTime()<(ct.getTime()-86400000*30)){ucg=1}}}if(ucg){SA._S_sUCk(_S_UO_,uor+","+uol+","+up_t,365)}}},
		_S_gRef:function(){var re=/^[^\?&#]*.swf([\?#])?/;if((_S_PREF_=="")||(_S_PREF_.match(re))){var ref=SA._S_gKeyV(_S_PURL_,"ref","&","");if(ref!=""){return ref}}return _S_PREF_},
		_S_MEvent:function(){if(_S_SMC==0){_S_SMC++;var c=SA._S_gUCk(_S_UPA_);if(c==""){c=0}c++;if(c<_S_SMM){var re=/[&|?]c=spr(_[A-Za-z0-9]{2,}){3,}/;if(_S_PURL_.match(re)||_S_PREF_.match(re)){c=c+_S_SMM}}SA._S_sUCk(_S_UPA_,c)}},_S_gMET:function(){var c=SA._S_gUCk(_S_UPA_);if(c==""){c=0}return c},
		_S_gCInfo_v2:function(){var now=new Date();return"sz:"+screen.width+"x"+screen.height+"|dp:"+screen.colorDepth+"|ac:"+SG.nav.appCodeName+"|an:"+_S_NAN_+"|cpu:"+SG.nav.cpuClass+"|pf:"+SG.nav.platform+"|jv:"+SA._S_gJVer(_S_NAV_,_S_NAN_)+"|ct:"+SA._S_gConType()+"|lg:"+SG.nav.systemLanguage+"|tz:"+now.getTimezoneOffset()/60+"|fv:"+SA._S_gFVer(SG.nav)},
		_S_gPInfo_v2:function(pid,ref){if((undefined==pid)||(""==pid)){pid=handler._S_gCid()+_S_DPID_}return"pid:"+pid+"|st:"+handler._S_gMET()+"|et:"+_S_ET+"|ref:"+escape(ref)+"|hp:"+SA._S_isHome(_S_PURL_)+"|PGLS:"+SA._S_gMeta("stencil")+"|ZT:"+escape(SA._S_gTJZTMeta())+"|MT:"+escape(SA._S_gTJMTMeta())+"|keys:"},
		_S_gUInfo_v2:function(vid){return"vid:"+vid+"|sid:"+handler._S_gsSID()+"|lv:"+handler._S_gsLVisit(handler._S_gsSID())+"|un:"+handler._S_gSUP()+"|uo:"+handler._S_gUOR()+"|ae:"+handler._S_gAEC()},
		_S_gEXTInfo_v2:function(ext1,ext2){_S_EXT1=(undefined==ext1)?_S_EXT1:ext1;_S_EXT2=(undefined==ext2)?_S_EXT2:ext2;return"ex1:"+_S_EXT1+"|ex2:"+_S_EXT2},
		_S_pBeacon:function(pid,ext1,ext2){try{var gid=handler._S_gsGID();if(""==gid){if(_S_GIDT<1){setTimeout(function(){handler._S_pBeacon(pid,ext1,ext2)},_S_T2);_S_GIDT++;return}else{gid=handler._S_gsSID();handler._S_sGID(gid)}}var sVer="V=2";var sCI=handler._S_gCInfo_v2();var sPI=handler._S_gPInfo_v2(pid,handler._S_gRef());var sUI=handler._S_gUInfo_v2(gid);var sEX=handler._S_gEXTInfo_v2(ext1,ext2);var lbStr=sVer+"&CI="+sCI+"&PI="+sPI+"&UI="+sUI+"&EX="+sEX;handler._S_p2Bcn(lbStr,_S_CP_RF)}catch(e){}},
		_S_acTrack_i:function(eid,p){if((""==eid)||(undefined==eid)){return}handler._S_sAEC(eid);if(0==p){return}var s="AcTrack||"+handler._S_gGID()+"||"+handler._S_gsSID()+"||"+handler._S_gSUP()+"||"+eid+"||";handler._S_p2Bcn(s,_S_CP_RF_E)},
		_S_uaTrack_i:function(acode,aext){var s="UATrack||"+handler._S_gGID()+"||"+handler._S_gsSID()+"||"+handler._S_gSUP()+"||"+acode+"||"+aext+"||";handler._S_p2Bcn(s,_S_CP_RF_E)}
	};
	if(_S_SMC==0){if('MSIE'==_S_NAN_){SSL.Global.doc.attachEvent("onclick",handler._S_MEvent);SSL.Global.doc.attachEvent("onmousemove",handler._S_MEvent);SSL.Global.doc.attachEvent("onscroll",handler._S_MEvent)}else{SSL.Global.doc.addEventListener("click",handler._S_MEvent,false);SSL.Global.doc.addEventListener("mousemove",handler._S_MEvent,false);SSL.Global.doc.addEventListener("scroll",handler._S_MEvent,false)}};handler._S_sUOR();
	return{
		_S_pSt:function(pid,ext1,ext2){try{if((SA._S_isFreshMeta())||(SA._S_isIFrameSelf(_S_IFH,_S_IFW))){return}if(_S_ET>0){return}++_S_ET;setTimeout(function(){handler._S_gsSID()},_S_T1);setTimeout(function(){handler._S_pBeacon(pid,ext1,ext2,0)},_S_T2)}catch(e){}},
		_S_pStM:function(pid,ext1,ext2){++_S_ET;handler._S_pBeacon(pid,((undefined==ext1)?handler._S_upExt1():ext1),ext2)},
		_S_acTrack:function(eid,p){try{if((undefined!=eid)&&(""!=eid)){setTimeout(function(){handler._S_acTrack_i(eid,p)},_S_T1)}}catch(e){}},
		_S_uaTrack:function(acode,aext){try{if(undefined==acode){acode=""}if(undefined==aext){aext=""}if((""!=acode)||(""!=aext)){setTimeout(function(){handler._S_uaTrack_i(acode,aext)},_S_T1)}}catch(e){}}
	}
}
var GB_SUDA;if(GB_SUDA==null){GB_SUDA=new SUDA({})}
var _S_PID_="";
function _S_pSt(pid,ext1,ext2){GB_SUDA._S_pSt(pid,ext1,ext2);}
function _S_pStM(pid,ext1,ext2){GB_SUDA._S_pStM(pid,ext1,ext2);}
function _S_acTrack(eid){GB_SUDA._S_acTrack(eid,1);}
function _S_uaTrack(acode,aext){GB_SUDA._S_uaTrack(acode,aext);}

(function(){
	if(!/\((iPhone|iPad|iPod)/i.test(navigator.userAgent)){return};
	document.addEventListener('mousedown',function(e){
		var ele = e.target;
		do{
			if(ele.tagName == 'A'){
				ele.target = '_self';
				return;
			};
			if(ele.tagName == 'DIV'){return};
		}while(ele = ele.parentNode);
	},false);
})();