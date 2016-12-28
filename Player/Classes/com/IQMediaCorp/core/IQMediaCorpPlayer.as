/**
   IQMediaPlayer Custom Video/Audio RTMP/FMS streaming player with functionality specifically for IQMedia - requires custom patched FLVPlayback.swc
**/
   
package com.IQMediaCorp.core
{
	import caurina.transitions.Tweener;

	import com.hires.debug.Stats;
	import com.impossibilities.utils.NpContextMenu;
	import com.impossibilities.utils.StringHelper;
	import flash.geom.ColorTransform;
	import flash.geom.Transform;
	import fl.video.*;
	
	import flash.display.DisplayObject;
	import flash.display.Bitmap;
	import flash.display.Loader;
	import flash.display.LoaderInfo;
	import flash.display.MovieClip;
	import flash.display.StageAlign;
	import flash.display.StageDisplayState;
	import flash.display.StageScaleMode;
	import flash.events.AsyncErrorEvent;
	import flash.events.ContextMenuEvent;
	import flash.events.Event;
	import flash.events.FullScreenEvent;
	import flash.events.HTTPStatusEvent;
	import flash.events.IOErrorEvent;
	import flash.events.MouseEvent;
	import flash.events.SecurityErrorEvent;
	import flash.events.StatusEvent;
	import flash.events.TimerEvent;
	import flash.external.*;
	import flash.filters.GlowFilter;
	import flash.geom.Rectangle;
	import flash.net.*;
	import flash.system.Capabilities;
	import flash.system.LoaderContext;
	import flash.system.Security;
	import flash.system.System;	
	import flash.text.TextField;
	import flash.text.TextLineMetrics;
	import flash.utils.*;
	import flash.accessibility.AccessibilityProperties;
	import fl.controls.ComboBox;
	import fl.controls.Slider;
	import fl.events.SliderEvent;
	
	import flash.geom.*;
	import urlshorten.URLShorten;
	import urlshorten.events.URLShortenEvent;
	import flash.xml.XMLDocument;
    import flash.xml.XMLNode;
    import flash.xml.XMLNodeType;
	import flash.net.LocalConnection;
	import flash.display.InteractiveObject;
	import flash.events.KeyboardEvent;
	import flash.ui.Keyboard;
	import fl.managers.FocusManager;
	import flash.geom.*;		
	import caurina.transitions.Tweener;
	import caurina.transitions.properties.ColorShortcuts;
	import caurina.transitions.*;
	import flash.events.*;
	import flash.text.*;
	//import com.hybrid.ui.ToolTip;
	import flash.display.DisplayObject;
	import fl.motion.Color;
	import flash.external.ExternalInterface;
	import flash.system.JPEGLoaderContext;	
	
	//import com.curiousmedia.accessibility.*;
	import com.IQMediaCorp.core.JSON.JSON;
	import flash.globalization.DateTimeFormatter;	
	import flash.media.Video;
	import flashx.textLayout.formats.Float;
	
	public class IQMediaCorpPlayer extends MovieClip
	{
		private var myVid:FLVPlayback = new FLVPlayback();

		private var stats:Stats = new Stats({bg: 0x66000000});
		
		private var TraceStr:String;
			
		var enable:Boolean = true;
			
		private var guid:String;

		private var thevidplays:int = 0;

		private var initClip:String;
		
		private var LogPlayURLDynamic:String;
		
		private var IsRawMedia:String;
		
		private var IsUGC:String;
		
		private var IsDipalyLogoInEmbed:String;
		
		private var PlayerLogo:String;
		
		private var image:Bitmap;
		
		private var PageName:String;
		
		private var ToEmail:String;
		
		private var clientGUID:String;
		
		private var IsAutoDownload:String;
		
		private var customerGUID:String;
		
		private var RL_User_GUID:String;
		
		private var categoryCode:String;
		
		private var widgetMode:String;

		private var willAutoPlay:String;
		
		private var IsLogPlay:String;
		
		private var pid:String;

		private var fvars:String;

		private var outStr:String;

		private var outgoing_lc:LocalConnection = new LocalConnection();
	
		private var userIddynamic:String;
		
		private var embedPage:String = "";

		private var jsDEBUG:Boolean = false;
		
		private var IsClipperEnable:Boolean=false;
		
		private var offsetTime:Number=0;
		
		private var isCustomSeek:Boolean=false;
		
		private var customSeekPoint:Number;
		
		private var customSeekPointRight:Number;
		
		private var keyShift:Boolean=false;
		

		private var keyLeft:Boolean=false;
		


		private var keyRight:Boolean=false;
		
		private var keySpace:Boolean=false;
		
		private var keyAlt:Boolean=false;
		
		private var keyCtl:Boolean=false;
		
		private var isLeftKnob:Boolean;
		
		private var keyvalue:String;
		
		private var versionBuild:String = "2.0.1";
		
		private static const menuItemLabel1:String = "IQMedia Video Player";
		
		private var menuItemLabel2:String = "Version: " + versionBuild + "";
		
		private static const debugService1:String = "";

		private static const debugService2:String = "";

		private static const serviceTimeout:Number = 45000;

		//********** Declare XML Variables
		private var xmlResponse:XML;

		private var filename:String;

		private var xmlType:String;

		private var startTime:Number = 0;

		private var endTime:Number = 10;

		private var vidname:String;

		private var hasCaption:String;

		private var globalStart:Number;

		private var globalStop:Number;

		private var _fileId:String;
		
		private var _imagePath:String;
		
		private var clipName:String;

		private var srvString:String;
	
		private var verPP:String;
		
		private var ServicesBaseURL:String;
		
		private var PlayerFromLocal:String;
		
		private var ClipLength:Number;
		
		private var embedUrl:String;

		private var rid:String;

		private var userId:String;

		private var emailFrom:String = null;

		private var clipDesc:String;

		private var stURL:URLRequest;

		//********** Interface Variables
		private var meta1_textString:String;

		private var vidTimeInt:uint;

		private var scrubInt:uint;

		private var progInt:uint;

		private var overInt:uint;

		private var volInt:uint;

		private var ftime:int;

		private var sw:Number;

		private var sh:Number;

		private var wchange:Number;

		private var hchange:Number;

		private var res_w:Number;

		private var res_h:Number;

		// menu vars
		private var menuStopx:Number;

		private var menuStartx:Number;

		private var menuStopy:Number;

		private var menuStarty:Number;

		//
		private var emailStopx:Number;

		private var emailStopy:Number;

		private var emailStartx:Number;

		private var emailStarty:Number;

		private var embedStopx:Number;

		private var embedStopy:Number;

		private var embedStartx:Number;

		private var embedStarty:Number;

		private var mcStopy:Number;

		private var linkStopx:Number;

		private var linkStopy:Number;

		private var linkStartx:Number;

		private var linkStarty:Number;

		private var menuState:String = "CLOSED";

		private var menuSubState:String = "CLOSED";

		private var bKnobStat:int = 0;

		private var playToggle:Boolean = false;

		private var userVolume:Number = 100;

		// for captioning display
		private var ccDisplay:Array = new Array("", "", "", "");

		private var ccDisplayCnt:Number = 3;

		private var ccLastText:String = "";

		private var domReferSite:String;

		private var userClicked:Boolean = false;

		private var varsLoaded:Boolean = false;

		private var clipLoaded:Boolean = false;

		private var loggedPlay:Boolean = false;

		private var prevID:String = "";

		private var previewPersistent:Boolean = false;

		private var dataLoaded:Boolean = false;

		private var audioGraphic:String;

		private var hasVideo:String;

		private var getVarsInt:uint;
		
		private var getClosedCaptionInt:uint;
		
		private var getPlayerInfoInt:uint;

		private var previewLoaded:Boolean = false;

		private var myTimer:Timer = new Timer(1500);

		//********** Declare Video Variables *****************************
		private var app_source:String;

		private var m_appName:String = "000250/flash";

		private var vidSmoothing:Boolean = true;

		private var prevStream:String;

		private var metaState:Boolean = false;

		private var localData:SharedObject = SharedObject.getLocal("iqmedia_player");
		
		private var seekAndStart:Boolean = false;

		private var seekStartPoint:Number;

		private var thumbLdr:Loader = new Loader();

		private var prevLoader:Loader = new Loader();

		private var loaderContext:LoaderContext = new LoaderContext();

		private var imgPreviewURL:String = srvString + "/svc/clip/previewImage?eid="; 

		private var previewFirst:Boolean = true;

		private var conMenu:NpContextMenu;

		private var initialOffset:int;
		
		private var firstTimeLoadRawMedia:Boolean=true;
		
		private var firstTimeLoadClip:Boolean=true;

		private var EB:String;	
		
		private var IsFirstLK:Boolean=false;
		
		private var CCUnavailable:String="Closed caption temporarily unavailable";
		
		private var listCCBegin:Vector.<int>;
		
		private var listCCEnd:Vector.<int>;
		
		private var listCCText:Vector.<String>;
		
		private var CurrentCCPosition:int=2;
		
		private var PreviousSecond:int=-1;
		
		private var MediaType:String;
		
		private var DefaultClipLength:Number;
		
		private var PlayerDataKeyWord:String ="";
		private var PlayerDataDescription:String = "";
		
		private var FlagPopup:Boolean= false;
		
		private var ForwardFlag:Boolean = false;
		private var RewindFlag:Boolean = false;
		private var SeekSpeedFor:int=0;
		private var SeekSpeedBack:int=0;
		private var SeekPointFR:int=0;
		private var RTimer;
		private var FTimer;
		
		private var IsInitEnabled:Boolean=false;
		
		private var StartCustomSeek:Boolean = false;
		
		private var NielSenResponseObject:Object=new Object();
		
		private var NeilSenResponseRcvd:Boolean = false;
		
		private var Title120:String;
		
		private var ListTitle120:XMLList;

		private var ListIQStartPoint:XMLList;

		private var ListIQStartMinute:XMLList;
		
		private var EleProccessed:int =-1;
		
		private var IsLoadMetaDisplayed = 0;
		
		private var TimerMetaDisplay:Number;
		
		//private var CurrentStartPoint:Number;
		

		public function IQMediaCorpPlayer():void
		{
			/*flash.system.Security.allowDomain("*");			
			Security.allowDomain("*");
			Security.allowInsecureDomain("*");*/
			if (stage)
			{				
				oneTimeInit();
			}
			else
			{
				this.addEventListener(SecurityErrorEvent.SECURITY_ERROR, ohNO_handler);
				addEventListener(Event.ADDED_TO_STAGE, oneTimeInit);
			}

		}

		private function ohNO_handler(evt:SecurityErrorEvent):void
		{
			tracer("INFO", "FUDGE! googleIMA ffing it up for us...");
		}
		
		function roundNumber(numb:Number, decimal:Number):Number
		{
			var precision:Number = Math.pow(10,decimal);
			return Math.round(numb*precision )/precision;
		}

		
		public function oneTimeInit(event:Event=null):void
		{									
			var swfnamearray:Array = stage.loaderInfo.url.split("/");
			
			if(swfnamearray[swfnamearray.length - 1].toString().lastIndexOf("_v") != -1)
			{		
				var swfname:String = swfnamearray[swfnamearray.length - 1];
				versionBuild = swfname.substring(swfname.lastIndexOf("_v") + 2,swfname.indexOf(".swf"));
				menuItemLabel2 = "Version: " + versionBuild + "";			
			}
			
			//trace("Doamin :"+ExternalInterface.call("window.location.href.toString"));
			trace("Domain :"+this.parent.parent);
			
			clipperUI.shortcuts.hitArea = clipperUI.clearButt;
									
			trace("RLPLAYER ADDED TO STAGE!");
			removeEventListener(Event.ADDED_TO_STAGE, oneTimeInit);

			stage.align=StageAlign.TOP_LEFT;
			this.opaqueBackground=0x000000;
			
			
			botbar2_mc.visible=false;
			botbar2_mc.enabled = false;
			botbar2_mc.useHandCursor = false;

			/*flash.system.Security.allowDomain("*");*/
			Security.allowDomain("*");
			Security.allowInsecureDomain("*");
			

			stats.visible=false;
			addChild(stats);

			addChildAt(myVid, 0);
			myVid.x=0;
			myVid.y=0;
			myVid.width=545;
			myVid.height=340;

			// Retrieve any Flashvars passed in
			initClip=String(LoaderInfo(this.root.loaderInfo).parameters.embedId);
			//initClip="7af51677-a217-4a3b-90dc-be90fbdced3b"; /* Clip */
			//initClip = "9bd9a061-a509-48e7-821d-f78646bb78f0"; /* RawMedia*/
			//initClip ="7082867b-8ff9-4e8b-95bb-dd40ccdb7f14"; /* Radio */
			//initClip="5959f8be-b311-456c-853e-231b3c05452c"; /* UGC-Upload */
			
			IsRawMedia=String(LoaderInfo(this.root.loaderInfo).parameters.IsRawMedia);			
			//IsRawMedia ="TRUE";
			//IsRawMedia = "FALSE";
			
			IsUGC = LoaderInfo(this.root.loaderInfo).parameters.IsUGC == undefined ? "FALSE" : String(LoaderInfo(this.root.loaderInfo).parameters.IsUGC);
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				if(IsUGC.toLocaleUpperCase()=="TRUE")
				{
					MediaType="ugc";
				}
				else
				{
					MediaType="rawmedia";
				}
				ForwardBtn.visible =true;
				RewindBtn.visible = true;
				trace(".........FF/Rew Disabled OneTimeInit........");
				EnableForwardRewind(false);
				ForwardTxt.text ="";
				RewindTxt.text ="";
			}
			else
			{
				MediaType="clip";
				ForwardBtn.visible =false;
				RewindBtn.visible = false;
			}
			
			IsDipalyLogoInEmbed = LoaderInfo(this.root.loaderInfo).parameters.IsDipalyLogoInEmbed == undefined ? "FALSE" : String(LoaderInfo(this.root.loaderInfo).parameters.IsDipalyLogoInEmbed);
			if(IsDipalyLogoInEmbed.toLocaleUpperCase() == "FALSE")
			{
				//mcPlay.logobg_mc.visible = false;
			}
			
			userIddynamic=String(LoaderInfo(this.root.loaderInfo).parameters.userId);
			//userIddynamic="b7f1bbe8-1f44-4f1c-9b11-ac771aa4aa55";
			
			PageName=String(LoaderInfo(this.root.loaderInfo).parameters.PageName);
			
			ToEmail = String(LoaderInfo(this.root.loaderInfo).parameters.ToEmail);
			//ToEmail = "maulik@amultek.com";
			
			clientGUID = String(LoaderInfo(this.root.loaderInfo).parameters.clientGUID);
			//clientGUID = "7722A116-C3BC-40AE-8070-8C59EE9E3D2A";
			
			IsAutoDownload = String(LoaderInfo(this.root.loaderInfo).parameters.IsAutoDownload);
			
			customerGUID = String(LoaderInfo(this.root.loaderInfo).parameters.customerGUID);
			//customerGUID = "B92B5C68-FA30-478F-9A20-B6F754C1F89C";
			
			RL_User_GUID = String(LoaderInfo(this.root.loaderInfo).parameters.RL_User_GUID);
			//RL_User_GUID = "07175c0e-2b70-4325-be6d-611910730968";
			
			categoryCode = String(LoaderInfo(this.root.loaderInfo).parameters.categoryCode);
			//categoryCode = "PR";			
			
			ServicesBaseURL = String(LoaderInfo(this.root.loaderInfo).parameters.ServicesBaseURL);
			//ServicesBaseURL = "1";
			
			PlayerFromLocal = String(LoaderInfo(this.root.loaderInfo).parameters.PlayerFromLocal);
			//PlayerFromLocal = "true";
			
			ClipLength = Number(LoaderInfo(this.root.loaderInfo).parameters.ClipLength);
			//ClipLength = 1800;
			
			initialOffset = int(LoaderInfo(this.root.loaderInfo).parameters.Offset);
			
			DefaultClipLength=Number(LoaderInfo(this.root.loaderInfo).parameters.DefaultClipLength);
			
			EB = String(LoaderInfo(this.root.loaderInfo).parameters.EB);
			
			keyvalue=String(LoaderInfo(this.root.loaderInfo).parameters.KeyValues);
			//keyvalue="{\"keyvalue\":[{\"name\":\"vishal\"},{\"surname\":\"parekh\"}]}";
			//keyvalue="{\"name\":\"vishal\",\"surname\":\"parekh\"}";			
			
			
			if(ServicesBaseURL == "1")
			{
				srvString = "http://qaservices.iqmediacorp.com";
				
				verPP = "D";
			}
			else if(ServicesBaseURL == "2")
			{
				srvString = "http://services.iqmediacorp.com";
				verPP = "P";
			}
			else if(ServicesBaseURL == "3")
			{
				srvString = "http://qaservices.mycliqmedia.com";
				verPP = "D";
			}
			else if(ServicesBaseURL == "4")
			{
				srvString = "http://services.mycliqmedia.com";
				verPP = "P";
			}
				
			IsLogPlay = "false";
			
			/*var myDate:Date = new Date();
			trace(myDate.toTimeString());*/
			
			widgetMode=String(LoaderInfo(this.root.loaderInfo).parameters.widget).toUpperCase();
			
			willAutoPlay=String(LoaderInfo(this.root.loaderInfo).parameters.autoPlayback).toUpperCase();
			//willAutoPlay = "TRUE";
			
			
			PlayerLogo = LoaderInfo(this.root.loaderInfo).parameters.PlayerLogo == undefined ? "" : String(LoaderInfo(this.root.loaderInfo).parameters.PlayerLogo);
			//PlayerLogo = "http://qa.iqmediacorp.com/Images/PlayerLogo/iQ Media2_27_2012 6_24_07 AM_PlayerLogo.png";
			//PlayerLogo = "http://qa.iqmediacorp.com/Images/client_logo.jpg";
			
			if(PageName == "ClipPlayer")
			{
				CallClientWaterMarkService();
			}
			else if(PlayerLogo !=  "" && PlayerLogo != undefined)
			{
				SetClientWaterMark();
			}
			
			pid=String(LoaderInfo(this.root.loaderInfo).parameters.pid);

			about_scrn.core_mc.versionTxt.text=menuItemLabel2;
			
			// Adjust scaling/dimensions differently if embedded ina Flex Loader container
			if (widgetMode == "TRUE")
			{
				trace('...in if ...');
				
				stage.scaleMode=StageScaleMode.NO_BORDER;
				fullScreen.enabled=false;
				fullScreen.alpha=.35;
				sw=stage.stageWidth;
				sh=stage.stageHeight;
			}
			else
			{
				trace('old player ...in else ...');
				trace(this.parent.toString());
				
				stage.scaleMode=StageScaleMode.NO_SCALE;
				sw=this.parent.width;
				sh=this.parent.height;
				
				//trace("sw: "+sw);
				//trace("sh: "+sh);
				
			}
			
			trace("sw :"+ sw);
			
			wchange=sw / 545;
			hchange=sh / 340;
			
			trace("wchange :"+ wchange);
			//trace("hchange: "+hchange);

			res_w=sw;
			res_h=sh;

			if (willAutoPlay == "TRUE")
			{
				userClicked=true;
			}			

			// External Logging AIR application for troubleshooting
			// Download/install from: http://clients.impossibilities.com/IQMedia/2009/logger/
			
			trace("..........LocalConnection.isSupported......"+LocalConnection.isSupported);
			outgoing_lc.allowDomain('*');
			OutLog("startLogging","Connected to " + menuItemLabel1 + " [ " + menuItemLabel2 + " ]\n" + Capabilities.serverString + "\n" + Capabilities.version + "\n")
			outgoing_lc.addEventListener(StatusEvent.STATUS, logonStatus);

			//Filtering based on INFO, WARN, LOG, ERROR, FATAL to be added in the future for filtering in AIR Logger app
			tracer("INFO", "\n\n\n[______________________________________________________________]");
			tracer("INFO", "IQMedia Video Player :: " + menuItemLabel2);
			tracer("INFO", "Video Component :: " + FLVPlayback.VERSION);

			// setup additional GET params for service requests based in clip ID/GUID
			if (initClip != null)
			{
				guid=initClip;

				if (pid != null)
				{
					fvars="embedId=" + guid + "&pid=" + pid;
				}
				else
				{
					fvars="embedId=" + guid;
				}
			}

			//********** Setup Attributes
			
			mcPlay.alpha=.85;
			mcPlay.visible=false;
			meta_mc.visible=false;
			about_scrn.visible=false;
			popup.visible=false;
			overlay_mc.alpha=0;
			menu_mc.mouseEnabled = false;

			CC_btn.visible=false;
			CC_btn.enabled=false;
			thumbnailUnavailable.visible=false;
			Scrub.progBar.width=0;
			tknob_mc.useHandCursor = true;

			myTimer.addEventListener(TimerEvent.TIMER, emailCloseF);
			loader_mc.buttonMode=false;
			loader_mc.mouseEnabled=false;
			capback.visible=false;
			myCap.visible=false;
			dispCap.visible=false;			

			myVid.bufferTime=3;
			
			myVid.fullScreenTakeOver=false;
			
			setUpVidListeners();
			moreListeners();

			getVolume();

			setupMenuListeners();
			setupMenuButtonListeners();
			setupExternalInterfaceCalls();
			setupClippingListeners();
			setupBookMarkListeners();

			stage.addEventListener(Event.RESIZE, resizeListener);
			stage.addEventListener(Event.FULLSCREEN, fireSize);
			

			if (widgetMode != "TRUE")
			{
				fullScreen.addEventListener(MouseEvent.CLICK, ToggleFullScreen);
			}

			activeBTN(false);

			// For handling permissions on media files to allow smoothing to be applied
			loaderContext.checkPolicyFile=true;
			
			// ********************* PREVIEW Image Loader/kickoff of app

			// Show clip ID and determine what to do otherwise throw error about missing clip
			// also check all passed in values/paras to determine what/how to load
			tracer("INFO", "INITCLIP: \"" + initClip + "\"");

			if (initClip == null || initClip == "null" || initClip == "NULL" || initClip == "UNDEFINED" || initClip == "undefined" || initClip == "")
			{
				errorDisplay("Missing Video Clip embedId");
				loader_mc.gotoAndStop(1);
				loader_mc.visible=false;
				activeBTN(false);
				lockUI(false);
				
				if (widgetMode != "TRUE")
				{
					menu_mc.removeEventListener(MouseEvent.CLICK, MenuMove);
				}
			}
			else
			{
				
				//TempparseClientWaterMark();
				if (initClip != "null" && initClip != "undefined" && willAutoPlay != "TRUE")
				{
					kickPreview();
				}
				else
				{
					if (willAutoPlay == "TRUE")
					{
						loader_mc.gotoAndPlay(2);
						thumbnailUnavailable.visible=false;
						mcPlay.removeEventListener(MouseEvent.CLICK, startUp);
						GetVarInfo();
						//impressionLoadPreviewImage();
						//logPlay();
						dispatchEvent(new Event("RLPlayer_AUTOPLAY_STARTED", true, false));
					}
					else
					{
						onErrorPreviewSub();
						tracer("INFO", "No Preview Image Available");
					}
				}

			}

			prevLoader.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR, onErrorAudioPreview);

			conMenu=new NpContextMenu(this);
			setupContextMenu();

			SizeItems();
			toggleSmoothing(vidSmoothing);

			trace("RLPLAYER READY!");
			// Important event to dispatch for Flex apps/widgets/mediacenter apps so they know when things are ready
			var evt:Event = new Event("RLPlayer_READY", true, false);
			dispatchEvent(evt);
			try {
				//Notify the j/s that the player is ready. If the function doesn't exist... its all good...
				ExternalInterface.call("RLPlayer_READY");
			}
			catch(err:Error) { /* I don't care about the error... */ }			
			
			scaleUpitem(clipSave);
			menubg.closeButt.useHandCursor=true;
			menubg.closeButt.buttonMode=true;
			
			clipperUI.pp_mc.visible=false;
			expand_mc.visible = false;
			
			stage.addEventListener(Event.FULLSCREEN, fullScreenHandler1);
			
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				FillCategories();
			}
			
			
			clipperUI.visible = false;
			clipSave.visible = false;
			//menubg.visible = false;
			linkVideo.visible = false;
			emailShare.visible = false;
			embedVideo.visible = false;
			//about_scrn.visible = false;
			
		}	
		
		private function StartForward(evt:MouseEvent)
		{
			//reset rewind flags
			RewindTxt.text ="";
			RewindFlag = false
			SeekSpeedBack =0;
			clearTimeout(RTimer);
			
			// set forward speed
			SeekSpeedFor = SeekSpeedFor +3;
			ForwardFlag= true;
			
			if(SeekSpeedFor <= 6)
			{
				bKnob.removeEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
				bKnob.removeEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
				Scrub.removeEventListener(MouseEvent.CLICK, SetScrubClick);
				myVid.ncMgr.videoPlayer.netStream.inBufferSeek = true;
				myVid.ncMgr.videoPlayer.netStream.maxPauseBufferTime = 3600;
				myVid.ncMgr.videoPlayer.netStream.bufferTimeMax = 3600;
				myVid.ncMgr.videoPlayer.netStream.addEventListener(NetStatusEvent.NET_STATUS,OnNetStatus);
				if(SeekSpeedFor ==3)
				{
					ForwardTxt.text ="1X";
				}
				else
				{
					ForwardTxt.text ="2X";
				}
				trace("SeekSpeed: " + SeekSpeedFor);
				SeekPointFR = myVid.playheadTime;
				mc_pp.gotoAndStop(2);
				myVid.pause();
				SeekPointFR = SeekPointFR + SeekSpeedFor;
				myVid.seek(SeekPointFR);
			}
			else
			{
				mc_pp.gotoAndStop(3);
				StopForwardRewind();
				myVid.play();
			}
			
			
			
		}
		
		private function StartRewind(evt:MouseEvent)
		{
			SeekSpeedBack = SeekSpeedBack + 3;
			ForwardTxt.text ="";
			SeekSpeedFor =0;
			ForwardFlag =false
			RewindFlag= true;
			clearTimeout(FTimer);
			if(SeekSpeedBack <= 6)
			{
				bKnob.removeEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
				bKnob.removeEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
				Scrub.removeEventListener(MouseEvent.CLICK, SetScrubClick);
				
				myVid.ncMgr.videoPlayer.netStream.inBufferSeek = true;
				myVid.ncMgr.videoPlayer.netStream.maxPauseBufferTime = 3600;
				myVid.ncMgr.videoPlayer.netStream.bufferTimeMax = 3600;
				myVid.ncMgr.videoPlayer.netStream.addEventListener(NetStatusEvent.NET_STATUS,OnNetStatus);
				if(SeekSpeedBack == 3)
				{
					RewindTxt.text ="1X";
				}
				else
				{
					RewindTxt.text ="2X";
				}
				trace("SeekSpeed: " + SeekSpeedBack);
				SeekPointFR = myVid.playheadTime;
				mc_pp.gotoAndStop(2);
				myVid.pause();
				SeekPointFR = SeekPointFR - SeekSpeedBack;
				myVid.seek(SeekPointFR);
			}
			else
			{
				mc_pp.gotoAndStop(3);
				StopForwardRewind();
				myVid.play();
			}
			
			
		}
		
		function ForwardTimer()
		{
			if(ForwardFlag)
			{
				SeekPointFR = SeekPointFR + SeekSpeedFor;
				if(SeekPointFR > ftime)
				{
					myVid.seek(ftime);
					StopForwardRewind();
					
				}
				else				
				{
					myVid.seek(SeekPointFR);
				}
				
			}
		}
		
		function RewindTimer()
		{
			if(RewindFlag)
			{			
				SeekPointFR = SeekPointFR - SeekSpeedBack;
				if(SeekPointFR>0)
					myVid.seek(SeekPointFR);
				else
				{
					myVid.seek(0);
					StopForwardRewind();
				}
					
			}
		}
		
		private function StopForwardRewind()
		{
				myVid.ncMgr.videoPlayer.netStream.inBufferSeek = false;
				bKnob.addEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
				bKnob.addEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
				Scrub.addEventListener(MouseEvent.CLICK, SetScrubClick);
				myVid.ncMgr.videoPlayer.netStream.removeEventListener(NetStatusEvent.NET_STATUS,OnNetStatus);
				if(ForwardFlag)
				{
					ForwardFlag= false;
					clearTimeout(FTimer);
					ForwardTxt.text ="";
					SeekSpeedFor =0;
				}
				
				if(RewindFlag)
				{
					RewindFlag= false;
					clearTimeout(RTimer);
					RewindTxt.text ="";
					SeekSpeedBack =0;
				}
		}
		
		private function EnableForwardRewind(bool:Boolean)
		{
			if(bool)
			{
				ForwardBtn.addEventListener(MouseEvent.CLICK,StartForward);
				RewindBtn.addEventListener(MouseEvent.CLICK,StartRewind);
				ForwardBtn.gotoAndStop(2);
				RewindBtn.gotoAndStop(2);
			}
			else
			{
				ForwardBtn.removeEventListener(MouseEvent.CLICK,StartForward);
				RewindBtn.removeEventListener(MouseEvent.CLICK,StartRewind);
				ForwardBtn.gotoAndStop(1);
				ForwardBtn.static_mc_ff.enabled = false;
				RewindBtn.gotoAndStop(1);
				RewindBtn.static_mc_rew.enabled = false;
			}
			
		}
											  
		
		function CallClientWaterMarkService()
		{
			 
			 var RequestURL:String;
			
			RequestURL= srvString + '/iqsvc/GetWaterMark?ClipID='+ initClip;

			var JSONLoader:URLLoader = new URLLoader();

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, getClientWaterMarkError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseClientWaterMark, false, 0, true);
			
			var request:URLRequest = new URLRequest(RequestURL);

			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
		}
		
		private function getClientWaterMarkError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred GetWaterMark"+evt.text);
		}
		
		function parseClientWaterMark(e:Event):void
		{
			
			var jsonobj:Object=new Object();
				
			jsonobj=JSON.decode(e.target.data,false);	
			if(jsonobj["Status"] == "0" || jsonobj["Status"] == 0)
			{
				var waterMarkLoader:Loader = new Loader(); 
   				var url :URLRequest = new URLRequest(jsonobj["Path"]);
   				waterMarkLoader.contentLoaderInfo.addEventListener(Event.COMPLETE, onImageLoaded);
   				waterMarkLoader.load(url);
			}
		}
		
		
		function SetClientWaterMark()
		{
			
			loaderContext.checkPolicyFile=true;
			
			var waterMarkLoader:Loader = new Loader(); 
			var url :URLRequest = new URLRequest(PlayerLogo);
			waterMarkLoader.contentLoaderInfo.addEventListener(Event.COMPLETE, onImageLoaded);
			waterMarkLoader.load(url,loaderContext);
		}
		
		function onImageLoaded(e:Event):void 
		{
      		
			
			
			try
			{
				image = new Bitmap(e.target.content.bitmapData);
				
				client_logo.x = 20;
				client_logo.y = stage.stageHeight - 90;
				
				//client_logo.graphics.lineStyle(1, 0xFFFFFF);
				//client_logo.graphics.drawRect(0, 0, 53, 53);
				//client_logo.graphics.endFill();
				
				image.x = (client_logo.width - image.width) / 2;
				image.y = (client_logo.height -image.height) / 2;
				
				client_logo.addChild(image);
			}
			catch(err:Error)
			{
					  
			}
			
   		}


		/**
		 * Handles swapping/toggling of the context menu between normal and fullscreen mode
		 *
		 */
		private function fullScreenHandler(event:FullScreenEvent):void
		{
			if (event.fullScreen)
			{
				conMenu.hideMenuItem("Enter FullScreen");
				conMenu.showMenuItem("Exit FullScreen");
			}
			else
			{
				conMenu.hideMenuItem("Exit FullScreen");
				conMenu.showMenuItem("Enter FullScreen");
				
				
			}
		}
	
		private function fullScreenHandler1(event:FullScreenEvent):void
		{
			if (event.fullScreen)
			{
				/*conMenu.hideMenuItem("Enter FullScreen");
				conMenu.showMenuItem("Exit FullScreen");*/
			}
			else
			{
				categories_mc.alpha = 0;
				categories_mc.x = 0 - categories_mc.width - 10;
				clipperUI.keyStatus = false;
				Tweener.addTween(clipperUI, {x: 0, y: stage.stageHeight + 50, time: 0, transition: "linear"});
				mc_pp.alpha=100;
				clipperUI.pp_mc.alpha=0;
				IsClipperEnable=false;
				Tweener.addTween(clipSave, {x: 0, y: stage.stageHeight + 50, time: 0, transition: "linear"});
				clipperUI.visible = false;
				clipSave.visible = false;
				//FlagPopup = false;
				
			}
		}


		/**
		 * Dummy function for handling events from localConnection based logging function "TRACER"
		 *
		 */
		private function logonStatus(event:StatusEvent):void
		{
			// catching the events to avoid errors - nothing needed here for time being - reserved for enhancements to AIR logger app
		}


		/**
		 * Handles output of logging via trace and localConnection to custom AIR app written to capture logs in realtime, as well as to JavaScript output if enabled.
		 * TODO: Add in additional verbosit for levels, LOG, DEBUG, INFO, WARN, ERROR, FATAL log levels with filtering support in AIR app with color coding
		 * External Logging AIR application for troubleshooting
		 * Download/install from: http://clients.impossibilities.com/IQMedia/2009/logger/
		 *
		 */
		private function tracer(typ, arg)
		{
			outStr="[ " + getTimer() + " : " + typ + " ] :: " + arg;
			//txtMsg.text += outStr+"\n";
			OutLog("displayMsg",outStr);
			if (typ != "WARN")
			{
				trace(outStr);
				if (jsDEBUG)
				{
					try
					{
						var js = ExternalInterface.call("trace", outStr);
					}
					catch (err:SecurityError)
					{
					}
					catch (err:Error)
					{
					}
				}
			}
		}
		
		
		private function OutLog(MethodName:String,outStr:String):void
		{
				var byte_max:Number = 50;
				if(outStr.length > byte_max)
				{
					var parts = Math.ceil(outStr.length/byte_max);
					for (var i = 0; i<parts; i++) {
						outgoing_lc.send("_log_output", MethodName, outStr.substr(byte_max*i, byte_max));
					}
				}
				else
				{
					outgoing_lc.send("_log_output", MethodName, outStr);
				}
		}


		/**
		 * Prepare the custom right-click contextual menus, set up listeners
		 *
		 */
		private function setupContextMenu():void
		{
			//add the event listener that will respond to a user selecting a menu item
			conMenu.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, handleMenuSelection);

			conMenu.addMenuItem(menuItemLabel1, false);
			conMenu.addMenuItem(menuItemLabel2, false);
			conMenu.addMenuItem("Enter FullScreen", true);
			conMenu.addMenuItem("Exit FullScreen", false);
			conMenu.addMenuItem("IQMedia Menu", true);
			conMenu.addMenuItem("Video Smoothing: ON", true);
			conMenu.addMenuItem("Video Smoothing: OFF", false);
			conMenu.addMenuItem("Toggle Memory Info", true);

			//disable the Exit FullScreen menu item on load
			conMenu.hideMenuItem("Exit FullScreen");
			
			if (widgetMode == "TRUE")
			{
				conMenu.hideMenuItem("Enter FullScreen");
			}

			//add event listener for stage to handle toggling of menus item display
			stage.addEventListener(Event.FULLSCREEN, fullScreenHandler);
		}


		/**
		 * Handler for custom contextual menus - dispatch the right events/call proper functions
		 *
		 */
		private function handleMenuSelection(event:ContextMenuEvent):void
		{
			switch (conMenu.selectedMenu)
			{

				case menuItemLabel1:
					var evt0:Event = new Event(MouseEvent.CLICK, true, false);
					menubg.about_mc.dispatchEvent(evt0);
					break;
				case menuItemLabel2:
					var evt01:Event = new Event(MouseEvent.CLICK, true, false);
					menubg.about_mc.dispatchEvent(evt01);
					break;
				case "Exit FullScreen":
					var evt:Event = new Event(MouseEvent.CLICK, true, false);
					fullScreen.dispatchEvent(evt);
					break;
				case "Enter FullScreen":
					var evt:Event = new Event(MouseEvent.CLICK, true, false);
					fullScreen.dispatchEvent(evt);
					break;
				case "IQMedia Menu":
					var evt2:Event = new Event(MouseEvent.CLICK, true, false);
					menu_mc.dispatchEvent(evt2);
					break;
				case "Video Smoothing: ON":
					toggleSmoothing(true);
					break;
				case "Video Smoothing: OFF":
					toggleSmoothing(false);
					break;
				case "Toggle Memory Info":

					stats.visible=!stats.visible;
					stats.startStats();
					break;
			}

		}
		

		/**
		 * Handle toggling of the smoothing property of the FLVplayback instance
		 *
		 *@param		arg				Boolean		True or False / on or off
		 */
		public function toggleSmoothing(arg:Boolean):void
		{
			if (arg == true)
			{
				conMenu.hideMenuItem("Video Smoothing: ON");
				conMenu.showMenuItem("Video Smoothing: OFF");
			}
			else
			{
				conMenu.showMenuItem("Video Smoothing: ON");
				conMenu.hideMenuItem("Video Smoothing: OFF");
			}
			var videoplayer:VideoPlayer = myVid.getVideoPlayer(0);
			videoplayer.smoothing=arg;
			vidSmoothing=arg;
		}


		/**
		 * Leverages ExternalInterface to inject anonymous functions into container browser DOM to extract info such as page title, page URL, referer, etc.
		 *
		   @return				String		Returns the info extracted from the browser DOM in format for logging to IQMedia services
		 */
		private function queryDOM():String
		{
			var DOMinfo:String;
			var jsResult:Object;

			if (ExternalInterface.available)
			{
				try
				{
					jsResult=ExternalInterface.call("function() { " + "var pageURL=window.location;" + "pageURL.windowName=window.name;" + "pageURL.documentReferer=document.referer;" + "pageURL.documentTitle=document.title;" + "return pageURL; }");
				}
				catch (err:Error)
				{
					tracer("WARN", "EI Error: " + err.message);
				}
				finally
				{
					DOMinfo == "undefined|undefined";
				}

				try
				{
					DOMinfo=jsResult.href; //+ "|" + jsResult.documentReferer;
					embedPage=jsResult.href;
				}
				catch (err:Error)
				{
					DOMinfo="undefined|undefined";
					tracer("WARN", "EI Error: " + err.message);
				}
			}
			else
			{
				DOMinfo="externalInterface NOT AVAILABLE";
			}

			if (DOMinfo == "undefined|undefined")
			{
				DOMinfo="externalInterface NOT AVAILABLE";
			}

			return DOMinfo;

		}


		/**
		 * Handles resizing of stage/UI elements on resize events, swapping between full/normal screen
		 * lots of little edge cases - handles sizing things properly wether embedded standalone
		 * or as a widget in AS3 container, Flex loader, etc. also if the video size has actually been determined yet
		 * either from metadata event firing, or values passed in from container app, or if not, based on the size of the embed
		 * keeps everything as a proportional scale in relation to the embedded dimensions so we never get skewed/stretched video/previews
		 *
		 */
		public function SizeItems():void
		{

			var vidScale:Number;
			var newW:Number;
			var newH:Number;
			var Message:String;
			var vidWidth:Number;
			var vidHeight:Number;

			if (widgetMode != "TRUE")
			{
				sw=stage.stageWidth;
				sh=stage.stageHeight;
				tracer("INFO", "STANDARD EMBED SIZE: " + sw + "x" + sh);
				
			}

			if (widgetMode == "TRUE" && (stage.displayState == StageDisplayState.FULL_SCREEN))
			{
				sw=this.parent.width;
				sh=this.parent.height;
				tracer("INFO", "WIDGET EMBED SIZE: " + sw + "x" + sh);
			}

			var stageBot:Number = sh - 27;
			popmask.width=sw;
			popmask.height=sh;
			popmask.x=0;
			popmask.y=0;

			overlay_mc.x=0;
			overlay_mc.y=0;
			overlay_mc.width=sw;
			overlay_mc.height=sh;

			if (myVid.metadataLoaded)
			{
				//tracer ("INFO","VIDEO METADATA :: LOADED");
				vidScale=Math.min(sw / myVid.metadata.width, (sh - 27) / myVid.metadata.height);
				newW=vidScale * myVid.metadata.width;
				newH=vidScale * myVid.metadata.height;
				myVid.setSize(newW, newH);
			}
			else
			{
				//tracer ("INFO","VIDEO METADATA :: NOT LOADED");
				vidWidth=myVid.getVideoPlayer(myVid.activeVideoPlayerIndex).width;
				vidHeight=myVid.getVideoPlayer(myVid.activeVideoPlayerIndex).height;
				vidScale=Math.min(sw / vidWidth, (sh - 27) / vidHeight);
				newW=vidScale * vidWidth;
				newH=vidScale * vidHeight;
				myVid.setSize(newW, newH);
			}
			myVid.x=(sw / 2) - (myVid.width / 2);
			myVid.y=(stageBot / 2) - (myVid.height / 2);
			
			client_logo.x = 20;
			client_logo.y = stage.stageHeight - 90;
			
			//client_logo.graphics.lineStyle(1, 0xFFFFFF);
			//client_logo.graphics.drawRect(0, 0, 53, 53);
			//client_logo.graphics.endFill();
			if(image)
			{
				image.x = (client_logo.width - image.width) / 2;
				image.y = (client_logo.height -image.height) / 2;
			}

			/* For later for fullscreen control in widget - right now we handle fullscreen in widget mode via the widget
			   and a special popout HTML page to allow user to scale to any dimension or go fullscreen
			   may put support back into this at a later date when IQMedia MediaCenter widget API is finalized

			   if (widgetMode == "TRUE" && (stage.displayState == StageDisplayState.FULL_SCREEN_INTERACTIVE))
			   {
			   myVid.x = 0;
			   myVid.y = 0;
			   sh = myVid.height - botbar_mc.height - 6;
			   }
			   if (widgetMode == "TRUE" && (stage.displayState == StageDisplayState.NORMAL))
			   {
			   myVid.x = 0;
			   myVid.y = 0;
			   }
			 */

			/***************/ /* Values for width/height of preview Images not for video */
			if (previewLoaded)
			{
				if (String(hasVideo).toUpperCase() == "FALSE" || String(hasVideo).toUpperCase() == "NULL" || myVid.source == "")
				{
					var prevScale = Math.min(sw / prevLoader.content.width, (sh - 28) / prevLoader.content.height);
					var newpW = prevScale * prevLoader.content.width;
					var newpH = prevScale * prevLoader.content.height;
					prevLoader.width=newpW;
					prevLoader.height=newpH;
					prevLoader.x=sw / 2 - newpW / 2;
					prevLoader.y=(sh - 28) / 2 - newpH / 2; //0;
				}
				else
				{
					//tracer ("INFO","PrevLoader vid based Scaling"+myVid.source);
					if (prevLoader && myVid.playing)
					{
						try
						{
							prevLoader.width=newW;
						}
						catch (err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}

						try
						{
							prevLoader.height=newH;
						}
						catch (err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}

						try
						{
							prevLoader.x=myVid.x;
						}
						catch (err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}

						try
						{
							prevLoader.y=myVid.y;
						}
						catch (err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}
					}

				}
			}

			try
			{
				thumbnailUnavailable.width=newW;
			}
			catch (err:Error)
			{
				tracer("WARN", "ERROR SizeItems thumbnailUnavailable: " + err);
			}

			try
			{
				thumbnailUnavailable.height=newH;
			}
			catch (err:Error)
			{
				tracer("WARN", "ERROR SizeItems thumbnailUnavailable: " + err);
			}

			meta_mc.y=Math.round((meta_mc.height * .80) * -1);
			mcStopy=meta_mc.y;

			botbar_mc.width=sw;
			botbar_mc.y=sh - botbar_mc.height;
			
			clipperUI.botbar_mc.width=sw;
			botbar2_mc.width=sw;
			botbar2_mc.y=sh - botbar_mc.height;
			
			capback.x=0;
			capback.width=sw;
			//capback.height = sh*.2220588
			capback.y=sh - (botbar_mc.height + capback.height);

			dispCap.autoSize="center";
			dispCap.width=sw - 10; //* .4633;
			dispCap.x=(sw - dispCap.width) / 2;
			dispCap.y=capback.y + 10;

			//myCap.height = sh*.18852
			myCap.visible=false;

			meta_mc.metabg_mc.x=0;
			meta_mc.metabg_mc.width=sw + 200;
			if (stage.displayState == StageDisplayState.FULL_SCREEN)
			{
				trace('..................i am at full screen........................');
				trace('............ Resultant Ad X ....................'+(sw - meta_mc.txtNeilSenAd.width+75));
				meta_mc.txtNeilSenAd.x =  sw - meta_mc.txtNeilSenAd.width+75;
				meta_mc.txtNeilSenAud.x =  sw - meta_mc.txtNeilSenAud.width+75;
				meta_mc.txtDmaRank.x =  sw - meta_mc.txtDmaRank.width +75;
			}
			else
			{
				meta_mc.txtNeilSenAd.x =  sw - meta_mc.txtNeilSenAd.width+10;
				meta_mc.txtNeilSenAud.x =  sw - meta_mc.txtNeilSenAud.width+10;
				meta_mc.txtDmaRank.x =  sw - meta_mc.txtDmaRank.width +10;
			}
			

			if (varsLoaded)
			{
				/*meta_mc.meta1Txt.text=meta1_textString;*/
				truncMeta(meta_mc.meta1Txt, sw - 85);
			}

			mc_pp.x=2.5 * wchange;
			mc_pp.y=botbar_mc.y + 4;
			
			trace("mc_pp.x: "+mc_pp.x);
			trace("mc_pp.y: "+mc_pp.y);
			

			/*loader_mc.x=Math.ceil(mc_pp.x + 10);*/
			loader_mc.x=mc_pp.x + 10;
			loader_mc.y=botbar_mc.y + 13.5;
			
			

			

			menu_mc.x=Math.round(botbar_mc.width - 25);
			menu_mc.y=botbar_mc.y + 7;
			
			trace("menu_mc.x: "+menu_mc.x);
			trace("menu_mc.y: "+menu_mc.y);

			CC_btn.x=Math.round(botbar_mc.width - 25);
			CC_btn.Y=0;

			volbutt_mc.x=menu_mc.x - 21;
			volbutt_mc.y=botbar_mc.y + 7;
			mutebutt_mc.x=menu_mc.x - 21;
			mutebutt_mc.y=botbar_mc.y + 7;

			fullScreen.x=volbutt_mc.x - 26;
			fullScreen.y=botbar_mc.y + 7;
						
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				ForwardBtn.visible =true;
				RewindBtn.visible = true;
				
				RewindTxt.visible=true;
				ForwardTxt.visible=true;
				
				/*ForwardBtn.x = mc_pp.x + 36;*/
				ForwardBtn.x = mc_pp.x + 26;
				ForwardBtn.y = mc_pp.y
				
				ForwardTxt.x = ForwardBtn.x + 12;
				ForwardTxt.y = mc_pp.y + 11;
				
				/*timeDisplayCur.x=ForwardBtn.x + 8;*/
				
				
				
				
				timeDisplayCur.x=ForwardBtn.x + 24;
				timeDisplayCur.y=botbar_mc.y + 7;
				
				
				//RewindBtn.x=fullScreen.x - 38;
				RewindBtn.x=fullScreen.x - 29;
				RewindBtn.y=mc_pp.y;
				
				RewindTxt.x = RewindBtn.x +12;
				RewindTxt.y = mc_pp.y + 11;
				
				/*timeDisplayTot.x=RewindBtn.x - 38;*/
				
				timeDisplayTot.x=RewindBtn.x - 50;
				timeDisplayTot.y=botbar_mc.y + 7;
				
			}
			else
			{
				ForwardBtn.visible =false;
				RewindBtn.visible = false;
				RewindTxt.visible=false;
				ForwardTxt.visible=false;
				timeDisplayCur.x=mc_pp.x + 12;
				timeDisplayCur.y=botbar_mc.y + 7;
				timeDisplayTot.x=fullScreen.x - 40;
				timeDisplayTot.y=botbar_mc.y + 7;
				
			}

			

			Scrub.x=timeDisplayCur.x + 53;
			Scrub.width=Math.round((timeDisplayTot.x - Scrub.x) - 7);
			Scrub.y=botbar_mc.y + 4;

			bKnob.x=Scrub.x;
			bKnob.y=botbar_mc.y + 4;

			tknob_mc.x=Scrub.x;
			tknob_mc.y=botbar_mc.y + 4;

			vol.x=volbutt_mc.x - 3;
			vol.y=Math.round(botbar_mc.y - 34);

			mcPlay.width=sw * .2517;
			mcPlay.height=mcPlay.width;
			mcPlay.x=((sw - mcPlay.width) / 2) + (mcPlay.width / 2);
			mcPlay.y=((sh - botbar_mc.height) - (mcPlay.height - (mcPlay.height * .8))) / 2;

			if (widgetMode == "TRUE" && (stage.displayState == StageDisplayState.FULL_SCREEN))
			{
				mcPlay.y=this.parent.height / 2 - mcPlay.height / 2;
			}

			menubg.width=sw;
			menubg.height=menubg.width * .14;
			menubg.x=0;
			menubg.y=Math.round(botbar_mc.y + (38 * wchange));

			menuStopx=menubg.x;
			menuStopy=menubg.y;
			menuStartx=menubg.x;
			
			
			
			if (stage.displayState == "fullScreen")
			{
				menuStarty=sh - (menubg.height + botbar_mc.height) + 15;
			}
			else
			{
				menuStarty=sh - (menubg.height + botbar_mc.height) + 5;
			}
			
			
			
			if (menuState == "OPEN")
			{
				showMenu();
			}

			if (metaState)
			{
				metaDisplayCore(); 
			}

			about_scrn.height=sh * .44411;
			about_scrn.width=about_scrn.height * 2.37682;
			about_scrn.x=(sw / 2);
			about_scrn.y=(sh - botbar_mc.height) / 2.5;

			popup.height=sh * .44411;
			popup.width=about_scrn.height * 2.37682;
			popup.x=(sw / 2);
			popup.y=(sh - botbar_mc.height) / 2.5;
			
			emailShare.x =0;
			clipSave.x = 0;
			linkVideo.x =0;
			embedVideo.x =0;

			//emailShare.height=sh * .7058;
			//emailShare.width=emailShare.height * 1.62083;
			//emailShare.x=Math.round(((sw - emailShare.width) / 2) + 5);
			//emailShare.y=Math.round(botbar_mc.y + (38 * wchange));
			//emailStopx=emailShare.x;
			//emailStopy=emailShare.y;
			//emailStartx=emailShare.x;
			//emailStarty=40 * wchange;

			//embedVideo.height=sh * .61970;
			//embedVideo.width=embedVideo.height * 1.8462;
			//embedVideo.x=(((sw - embedVideo.width) / 2) + 5);
			//embedVideo.y=Math.round(botbar_mc.y + (38 * wchange));
			//embedStopx=embedVideo.x;
			//embedStopy=embedVideo.y;
			//embedStartx=embedVideo.x;
			//embedStarty=40 * wchange;

			//linkVideo.height=sh * .61764;
			//linkVideo.width=linkVideo.height * 1.84622;
			//linkVideo.x=(((sw - linkVideo.width) / 2) + 5);
			//linkVideo.y=Math.round(botbar_mc.y + (38 * wchange));
			//linkStopx=linkVideo.x;
			//linkStopy=linkVideo.y;
			//linkStartx=linkVideo.x;
			//linkStarty=40 * wchange;

			if (menuSubState != "CLOSED")
			{
				switch (menuSubState)
				{
					case "ABOUT":
						break;
					case "EMBED":
						//embedVideo.y=40 * wchange;
						break;
					case "LINKS":
						//linkVideo.y=40 * wchange;
						break;
					case "EMAIL":
						//emailShare.y=40 * wchange;
						break;
				}
			}

			setChildIndex(stats, numChildren - 1);
			stats.x=0;
			stats.y=sh - stats.height - 28;
		}		
		
		
		// ******************* GET XML **********************************

		/**
		 * Handles loading XML data from IQMedia servicesm sets up the listeners, timeouts, and methods to loading
		 *
		 *@param		Uguid		String		user GUID? REvisit this - left over from Armons code - not needed
		 *@param		service		String		The service string to pull the XML data from
		 */
		private function GetXml(Uguid:String, service:String):void
		{
			getVarsInt=setTimeout(ParseVarTimeout, serviceTimeout);

			var xmlURLReq:URLRequest ;
			
			var xmlSendLoad:URLLoader = new URLLoader();			
			xmlSendLoad.addEventListener(IOErrorEvent.IO_ERROR, onIOError, false, 0, true);
			
			xmlURLReq= new URLRequest();
			xmlURLReq.url=service;
				
			xmlSendLoad.addEventListener(Event.COMPLETE, ParseVarInfo, false, 0, true);
			
			xmlURLReq.contentType="text/xml";
			xmlURLReq.method=URLRequestMethod.GET;			
			try
			{
				xmlSendLoad.load(xmlURLReq);
			}
			catch (err:Error)
			{
				tracer("WARN", "Error getXml load:" + err.message);
				errorDisplay("Service Failure: " + err.message);
			}
		}		
		
		
		
		
		/**
		 * Handler for parsing player/clip config data from custom IQMedia getVars service
		 *
		 *@param		e		Event		data event from XML retreived from getVars consolidated service
		 */
		private function ParseVarInfo(e:Event)
		{
			clearTimeout(getVarsInt);

			var clpXML:XML;

			clpXML= new XML(e.target.data);
			//clpXML = new XML('<root status="1" msg="" rid="7af51677-a217-4a3b-90dc-be90fbdced3b"><mediaInfo title="My Player 100 Correct" startTime="856" endTime="1553" sourceName="KSWB Fox5 San Diego" sourceLogo="http://media.iqmediacorp.com/logos/stations/small/kswbtv.gif" sourceUrl="http://www.fox5sandiego.com/pages/main"><![CDATA[My Player 100% Correct]]></mediaInfo><vars fileName="fs01i/media/2010/12/01/06/KSWBTV_20101201_06.flv" streamUrl="fms.iqmediacorp.com" appNameFMS="iqmedia" hasVideo="TRUE" hasCaption="TRUE" logPlayURL="http://qaservices.iqmediacorp.com/svc/logs/logClipPlay" thumbUrl="http://media.iqmediacorp.com/im41/2010/12/1/7af51677-a217-4a3b-90dc-be90fbdced3b.jpg" userId="07175c0e-2b70-4325-be6d-611910730968" emailFrom="lv@probusys.com" embedUrl="http://media.iqmediacorp.com/xdrive/web/vidplayer_1b/devplayer/devsandbox/redlasso_player_b1b_dev.swf" /></root>');
			
			hasVideo=String(clpXML.vars.@hasVideo).toUpperCase();
			
			try
			{
				audioGraphic=clpXML.vars.@audioGraphic;
			}
			catch (err:Error)
			{
				
			}
			
			var slogoSource:String = clpXML.mediaInfo.@sourceLogo;
			
			/* change for new call over */
			
			try
			{
				loaderContext.checkPolicyFile=true;		

				var pictURLReq:URLRequest = new URLRequest(slogoSource);
				thumbLdr.load(pictURLReq, loaderContext);
				thumbLdr.contentLoaderInfo.addEventListener(Event.COMPLETE, thumbInit)
				meta_mc.station_logo.addChild(thumbLdr);
			
				thumbLdr.width=57;
				thumbLdr.height=57;
			
			}
			catch(err:Error)
			{
				
			}

			
			 if(widgetMode.toUpperCase() == "TRUE")
			{
				if(clpXML.@status == 1 && IsRawMedia.toLocaleUpperCase()=="TRUE")
				{
					/* STatic Text */
					if(IsRawMedia.toLocaleUpperCase()=="TRUE")
					{
						expand_mc.enabled = true;
						
						menubg.clip_mc.enabled = false;
						menubg.clip_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
						
						
						menubg.embed_mc.removeEventListener (MouseEvent.CLICK,bookmark);
						menubg.embed_mc.addEventListener(MouseEvent.MOUSE_UP, embedmcClick);						
						
						
						menubg.email_mc.removeEventListener (MouseEvent.CLICK,email);
						menubg.email_mc.addEventListener(MouseEvent.MOUSE_UP, emailmcClick);
						
					
						menubg.link_mc.removeEventListener (MouseEvent.CLICK,link);
						menubg.link_mc.addEventListener(MouseEvent.MOUSE_UP, linkmcClick);
					
						var color:ColorTransform = new ColorTransform();
						color.color  = menubg.embed_mc.bri - 10;
						var colorObj:Color = new Color (); 
						colorObj.brightness = -.50;
						menubg.clip_mc.transform.colorTransform = colorObj;
						menubg.embed_mc.transform.colorTransform = colorObj;
						menubg.email_mc.transform.colorTransform = colorObj;
						menubg.link_mc.transform.colorTransform = colorObj;
					}
					else
					{
						expand_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
						expand_mc.visible = false;
						
						menubg.clip_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
						menubg.clip_mc.visible = false;
						
						
						if(EB == "false")
						{
							menubg.email_mc.visible = false;
							menubg.embed_mc.x = 175.80 - 30;
							menubg.link_mc.x = 270.25 - 5;
							menubg.about_mc.x = 364.70 + 25;
						}
						else
						{
							menubg.embed_mc.x = 175.80 - 75;
							menubg.email_mc.x = 270.25 - 75;
							menubg.link_mc.x = 364.70 - 75;
							menubg.about_mc.x = 459.15 - 75;
						}
					}
				}
				else
				{
					var color:ColorTransform = new ColorTransform();
					color.color  = menubg.embed_mc.bri - 10;
					var colorObj:Color = new Color (); 
					colorObj.brightness = -.50;
					
					menubg.email_mc.removeEventListener (MouseEvent.CLICK,email);
					menubg.email_mc.addEventListener(MouseEvent.MOUSE_UP, emailmcClick);
					menubg.email_mc.transform.colorTransform = colorObj;
					
					expand_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
					expand_mc.visible = false;
					
					menubg.clip_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
					menubg.clip_mc.visible = false;
					
					
					
					if(EB == "false")
					{
						menubg.email_mc.visible = false;
						menubg.embed_mc.x = 175.80 - 30;
						menubg.link_mc.x = 270.25 - 5;
						menubg.about_mc.x = 364.70 + 25;
					}
					else
					{
						menubg.embed_mc.x = 175.80 - 75;
						menubg.email_mc.x = 270.25 - 75;
						menubg.link_mc.x = 364.70 - 75;
						menubg.about_mc.x = 459.15 - 75;
					}
				}
			}
			else
			{
				if(clpXML.@status == 1 && IsRawMedia.toLocaleUpperCase()=="TRUE")
				{
					/* STatic Text */
					if(IsRawMedia.toLocaleUpperCase()=="TRUE")
					{
						expand_mc.enabled = true;
						menubg.clip_mc.enabled = true;
						
						menubg.embed_mc.removeEventListener (MouseEvent.CLICK,bookmark);
						menubg.embed_mc.addEventListener(MouseEvent.MOUSE_UP, embedmcClick);
						
						menubg.email_mc.removeEventListener (MouseEvent.CLICK,email);
						menubg.email_mc.addEventListener(MouseEvent.MOUSE_UP, emailmcClick);
						
					
						menubg.link_mc.removeEventListener (MouseEvent.CLICK,link);
						menubg.link_mc.addEventListener(MouseEvent.MOUSE_UP, linkmcClick);
					
						var color:ColorTransform = new ColorTransform();
						color.color  = menubg.embed_mc.bri - 10;
						var colorObj:Color = new Color (); 
						colorObj.brightness = -.50;
						menubg.embed_mc.transform.colorTransform = colorObj;
						menubg.email_mc.transform.colorTransform = colorObj;
						menubg.link_mc.transform.colorTransform = colorObj;
						
						/*var color:ColorTransform = new ColorTransform();
						color.blueOffset  = 127;
						menubg.embed_mc.transform.colorTransform = color;*/
					}
					else
					{
						expand_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
						expand_mc.visible = false;
						
						menubg.clip_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
						menubg.clip_mc.visible = false;
						
						
						if(EB == "false")
						{
							menubg.email_mc.visible = false;
							menubg.embed_mc.x = 175.80 - 30;
							menubg.link_mc.x = 270.25 -5;
							menubg.about_mc.x = 364.70 + 25;
						}
						else
						{
							menubg.embed_mc.x = 175.80 - 75;
							menubg.email_mc.x = 270.25 - 75;
							menubg.link_mc.x = 364.70 - 75;
							menubg.about_mc.x = 459.15 - 75;
						}
					}
				}
				else
				{
					expand_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
					expand_mc.visible = false;
					
					menubg.clip_mc.removeEventListener(MouseEvent.CLICK, ClipMCClick);
					menubg.clip_mc.visible = false;
					
					
					if(EB == "false")
					{
						menubg.email_mc.visible = false;
						menubg.embed_mc.x = 175.80 - 30;
						menubg.link_mc.x = 270.25 -5;
						menubg.about_mc.x = 364.70 + 25;
					}
					else
					{
						menubg.embed_mc.x = 175.80 - 75;
						menubg.email_mc.x = 270.25 - 75;
						menubg.link_mc.x = 364.70 - 75;
						menubg.about_mc.x = 459.15 - 75;
					}
					
				}
				
			}


			stURL=new URLRequest(clpXML.mediaInfo.@sourceUrl);
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				var stime:int = Number(clpXML.mediaInfo.@startTime);				
				var etime:int = Number(clpXML.mediaInfo.@endTime);
				firstTimeLoadClip = false;
				if(etime==-1)
				{
					etime=3599;
				}
			}
			else
			{
				var stime:int = Number(clpXML.mediaInfo.@startTime);				
				var etime:int = Number(clpXML.mediaInfo.@endTime);		
				firstTimeLoadClip = true;
				//seekAndStart = true;
				
				//seekStartPoint = stime;
			}
			
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				// changed by meghana on 26-June-2012 
				// commetned if condition code as now our etime can be any
				/*if(etime-stime>3599)
				{*/
					etime=etime-stime-1;
				/*}*/
			}
			
			ftime=etime - stime;
			//ftime = -1;
			
			// changed by meghana on 26-June-2012 removed 
			// initialOffset<=3599 Condition as now our etime can be any
			if(IsRawMedia.toLocaleUpperCase()=="TRUE" && firstTimeLoadRawMedia==true && initialOffset>=0)
			{
				
			}
			else
			{
				timeDisplayTot.text=timeCode(ftime);
			}

			//trace("stime:"+stime);
			
			vidname=clpXML.vars.@fileName;
			
			//trace("vidname" + vidname);
			
			m_appName=clpXML.vars.@appNameFMS;
			
			LogPlayURLDynamic =  clpXML.vars.@logPlayURL;
			
			app_source=clpXML.vars.@streamUrl;
			var myPattern:RegExp = /media2/gi;  
			var str:String = clpXML.vars.@streamUrl;
			app_source=str.replace(myPattern, "fms3");
			//trace("app_source:"+app_source);
			globalStart=stime;
			
			
			/*if(IsClipperEnable==true)
			{
				closeMenu();
				if(isCustomSeek==true)
				{					
					globalStop=customSeekPointRight;
					isCustomSeek=false;
				}
				globalStop=customSeekPointRight;
			}*/
			
			globalStop=etime;
			
			
			_fileId=clpXML.@rid;
			tracer("INFO", "FILE ID/Embed ID: " + _fileId);
			
			var lc:LocalConnection = new LocalConnection();
			var domain:String = lc.domain;
			
			embedUrl=clpXML.vars.@embedUrl;
			
			//trace("embedURL :"+embedUrl);
		
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				userId = userIddynamic;
			}
			else
			{
				userId=clpXML.vars.@userId;
			}
			
			_imagePath = clpXML.vars.@thumbUrl;
			
			//trace("_imagePath :"+_imagePath);
			
			
			hasCaption=clpXML.vars.@hasCaption;

			// CLIP INFO NODE ITEMS
			
				meta_mc.visible=true;
				var clpETime:Number = Number(clpXML.mediaInfo.@endTime);
				var clpSTime:Number = Number(clpXML.mediaInfo.@startTime);
				var clpData:Number = clpETime - clpSTime;
				var clpInfoTime:String = timeCode(clpData);
			
			// META INFO POPULATION CALL
			
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				/*clipName=clpXML.mediaInfo.@clipName;*/
				/* Static */
				clipName=clpXML.mediaInfo.@sourceTitle;
				/* Static */
				
				// commented to remove %issue while playing clip
				/* clipDesc=clpXML.mediaInfo;*/
				/* static */
				//clipDesc="Test Description";
				/* static */
				
				meta1_textString=clipName;
				meta_mc.meta1Txt.autoSize="left";
				//meta_mc.meta1Txt.text=clipName;
				meta_mc.meta2Txt.autoSize="left";
				meta_mc.meta3Txt.autoSize="left";
				//meta_mc.meta2Txt.text="Duration: " + clpInfoTime + " | Views: " + clpXML.vars.@views + " | Topic: RawMedia"
				//working
				//meta_mc.meta2Txt.text="Duration: " + clpInfoTime + " | Views: " + clpXML.mediaInfo.@plays + " | Topic: " + /*clpXML.mediaInfo.@category*/ /* Static Text */"Test Category" /* Static Text*/;
				//meta_mc.meta3Txt.text="Clipper: RawMedia" + clpXML.clipinfo.@author + " | " + "Aired: " + clpXML.vars.@airdate;
				//working
				//meta_mc.meta3Txt.text="Clipper: " + /*clpXML.mediaInfo.@user*/ /* Static Text*/ "Test User"/*Static Text*/ + " | " + "Aired: " + clpXML.mediaInfo.@airdate;
				rid=clpXML.@rid;
			}
			else
			{
				if(firstTimeLoadClip)
				{
					myVid.addEventListener(VideoEvent.SEEKED,OnSeeked);
				}
				clipName=clpXML.mediaInfo.@title;
				//trace("clipName" +clipName);
				
				// commented to remove %issue while playing clip
				/*clipDesc=clpXML.mediaInfo;*/
				meta1_textString=clipName;
				meta_mc.meta1Txt.autoSize="left";
				
				
				
				/*if(clipName.length > 30)
				{
					meta_mc.meta1Txt.text= clipName.substr(0,28) +'...';
				}
				else
				{*/
					meta_mc.meta1Txt.text=clipName;
				/*}*/
				
				
				meta_mc.meta2Txt.autoSize="left";
				meta_mc.meta3Txt.autoSize="left";
				//meta_mc.meta2Txt.text="Duration: " + clpInfoTime + " | Views: " + clpXML.mediaInfo.@plays + " | Topic: " + clpXML.mediaInfo.@categoryName;
				//meta_mc.meta3Txt.text="Clipper: " + clpXML.mediaInfo.@author + " | " + "Aired: " + clpXML.mediaInfo.@airdate;
				rid=clpXML.@rid;
			}
			
			//trace("going to call GetPlayerInfo");
			
			GetPlayerInfo();			

			if (hasCaption)
			{
				//trace("hasCaption.toUpperCase" +hasCaption.toUpperCase())
				
				if (hasCaption.toUpperCase() == "TRUE")
				{
					/* updated by vishal......hide closedcaption call */
					/*GetClosedCapInfo(globalStart, globalStop);*/
					
					GetClosedCaption(globalStart,globalStop);
					
					/* over updated by vishal......hide closedcaption call */
					CC_btn.visible=true;
					CC_btn.enabled=true;
				}
				else
				{
					CC_btn.visible=false;
					CC_btn.enabled=false;
				}
			}
			if (String(clpXML.@msg) == "0")
			{

				varsLoaded=true;
				errorDisplay("INVALID SERVICE DATA.\nPLEASE TRY AGAIN LATER.");
			}
			else
			{
				if(ToEmail != null && ToEmail != "")
				{
					emailShare.fromemailTxt.text = ToEmail;
				}
				
				// commented by meghana
				// to allow email only from flashvars 
				// and will only be shown on website.
				/*else if(clpXML.vars.@emailFrom != null)
				{
					emailShare.fromemailTxt.text=clpXML.vars.@emailFrom;
				}*/
				
				if (_fileId == null || _fileId == undefined || _fileId == "")
				{
					errorDisplay("INVALID SERVICE DATA.\nPLEASE TRY AGAIN LATER.");
				}
				else
				{
					dataLoaded=true;
					varsLoaded=true;
					assignEmbed();

					if ((userClicked) || (widgetMode == "TRUE" && userClicked))
					{
						 /*tracer("INFO", "fireNSLoad Started");
						fireNSLoad(m_appName);*/
						
						GetNSConnection(m_appName);
					}
					userClicked = true;
					clipLoaded = true;
					truncMeta(meta_mc.meta1Txt, sw - 85);
				}
			}
			if(IsRawMedia.toLocaleUpperCase() == "FALSE")
			{
				BookMarkURL();
			}
			
			if(IsRawMedia.toLocaleUpperCase()=="FALSE")
			{
				GetStationSharingInfo();
			}
			else	
			{
				menubg.clip_mc.x = 80.5 + 140;					
				menubg.about_mc.x = 459.15 - 140;		
			}
		}
		
		private function ParseClosedCaptionTimeout():void
		{
			clearTimeout(getClosedCaptionInt);
			tracer("WARN", "GETClosedCaption SERVICE TIMEOUT!");
			dispCap.text=CCUnavailable;
		}
	

		/**
		 * Handles timeout on loading getVars service timeout - set to a high value for the time being due to JIT issue on the server. When IQMedia fixes their JIT issues, we can set this to a normal value
		 *
		 */
		private function ParseVarTimeout():void
		{
			clearTimeout(getVarsInt);
			tracer("WARN", "GETVARS SERVICE TIMEOUT!");
			errorDisplay("DATA SERVICE TIMEOUT.\nPLEASE TRY AGAIN LATER.");
		}


		/**
		 * Handler for IO Errors in XML services
		 *
		 *@param		evt		IOErrorEvent		event fired from xmlSendLoad request
		 */
		private function onIOError(evt:IOErrorEvent):void
		{
			clearTimeout(getVarsInt);
			tracer("WARN", "An error occurred when attempting to load external" + evt.text);
			errorDisplay("DATA SERVICE TIMEOUT.\nPLEASE TRY AGAIN LATER.");
		}
		
		private function ClipAutoDownloadError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred in ClipAutoDownload"+evt.text);
		}
		
		private function UGCClipExportError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred UGC Clip Export"+evt.text);
		}


		/**
		 * Setup all the FLVPlayback class listeners for states, errors, metadata, custom errors, and a few other eventlisteners for the app
		 *
		 */
		private function setUpVidListeners():void
		{
			myVid.playheadUpdateInterval=33;
			myVid.addEventListener(VideoEvent.STATE_CHANGE, vidStateChange);
			myVid.addEventListener(VideoEvent.PLAYHEAD_UPDATE, UpdateCurrentTime); //
			myVid.addEventListener(VideoEvent.READY, SetupPlay);
			myVid.addEventListener(VideoEvent.PLAYING_STATE_ENTERED, logInitialPlay);
			myVid.addEventListener(IOErrorEvent.IO_ERROR, onIOError);
			myVid.addEventListener(SecurityErrorEvent.SECURITY_ERROR, netSecurityError);
			myVid.addEventListener(AsyncErrorEvent.ASYNC_ERROR, asyncErrorHandler);
			myVid.addEventListener(MetadataEvent.METADATA_RECEIVED, handleMetadata);
			

			// Add custom stream errors here overriding VideoError class - see custom patches
			// to the FLVPlayback component/class by RHALL SWC required to be in local path
			myVid.addEventListener("NetStream.Play.StreamNotFound", noStream);

			stage.addEventListener(Event.MOUSE_LEAVE, cursorOFFscreen);
		}


		/**
		 * Handles setting up many of the UI control listeners and set initial state of UI elements
		 *
		 */
		private function moreListeners():void
		{
			//**************** Button Event Listeners
			mcPlay.addEventListener(MouseEvent.MOUSE_DOWN, playClip);
			mc_pp.addEventListener(MouseEvent.CLICK, playBtn);
			
			//clipperUI.pp_mc.addEventListener(MouseEvent.CLICK, playBtn);

			volbutt_mc.addEventListener(MouseEvent.CLICK, VolMove);
			mutebutt_mc.addEventListener(MouseEvent.CLICK, VolMove);

			vol.vol_mc.volKnob.addEventListener(MouseEvent.MOUSE_DOWN, MoveVol);
			vol.vol_mc.volKnob.addEventListener(MouseEvent.MOUSE_UP, StopMoveVol);

			meta_mc.toggle_mc.addEventListener(MouseEvent.MOUSE_DOWN, metaDisplay);
			CC_btn.addEventListener(MouseEvent.CLICK, CC_toggle);
			menu_mc.addEventListener(MouseEvent.CLICK, MenuMove);

			meta_mc.toggle_mc.mouseEnabled=true;
			meta_mc.toggle_mc.buttonMode=true;
			Scrub.scrubBar.mouseEnabled=false;
			Scrub.scrubBar.buttonMode=false;
			
			Scrub.progBar.enabled=false;
			Scrub.progBar.mouseEnabled=false;
			loader_mc.gotoAndPlay(2);
			bKnob.mouseEnabled=true;
			bKnob.buttonMode=true;
			tknob_mc.buttonMode=false;
			tknob_mc.mouseEnabled=false;

			about_scrn.addEventListener(MouseEvent.CLICK, clk_about);
			about_scrn.buttonMode=true;

			emailShare.send_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			emailShare.send_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			emailShare.send_mc.addEventListener(MouseEvent.CLICK, email_send);
			emailShare.cancel_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			emailShare.cancel_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			emailShare.cancel_mc.addEventListener(MouseEvent.CLICK, email_close);
			emailShare.send_mc.buttonMode=true;
			emailShare.cancel_mc.buttonMode=true;
		}


		/**
		 * Handles displaying errors related to netstream connections
		 *
		 *@param		evt		Event		An error event
		 */
		public function noStream(evt:Event):void
		{
			trace("No Stream");
			tracer("WARN", ">> NETSTREAM ERROR: " + evt.type);
			setChildIndex(thumbnailUnavailable, numChildren - 1);

			if (widgetMode != "TRUE")
			{
				sw=stage.stageWidth;
				sh=stage.stageHeight;
			}

			thumbnailUnavailable.width=sw;
			thumbnailUnavailable.height=sh - 28;

			thumbnailUnavailable.visible=true;
			activeBTN(false);
			lockUI(false);
			menu_mc.mouseEnabled=false;
			mc_pp.gotoAndStop(1);
			mc_pp.static_mc.enabled=false;
			
			if(IsRawMedia.toLocaleUpperCase() == "TRUE")
			{
				trace(".........FF/Rew Disabled noStream........");
				EnableForwardRewind(false);
			}
			
			loader_mc.gotoAndStop(1);
			loader_mc.visible=false;
			thumbnailUnavailable.dispText.text="Video Temporarily Unavailable";
		}		


		/**
		 * Handles stage change events of the FLVPlayback component/class
		 *
		 *@param		e		VideoEvent		Any event broadcast bt the player
		 */
		 
		private function OnSeeked(e:VideoEvent):void
		{
			if(firstTimeLoadClip)//if(myVid.playheadTime >= globalStart)
			{
				firstTimeLoadClip = false;
				lockUI(true);
				myVid.play();
				mc_pp.gotoAndStop(3);
				myVid.removeEventListener(VideoEvent.SEEKED,OnSeeked);
			}
			
			if(IsRawMedia.toLocaleUpperCase() == "TRUE" && StartCustomSeek)
			{
				StartCustomSeek = false;
				myVid.removeEventListener(VideoEvent.SEEKED,OnSeeked);
				if(myVid.playheadTime >5)
				{
					EnableForwardRewind(true);
				}
				
			}
		}
		
		private function OnNetStatus(e:NetStatusEvent):void
		{
			if(e.info.code=="NetStream.Seek.Notify")
			{
				if(ForwardFlag)
				{
					clearTimeout(FTimer);
					FTimer = setTimeout(ForwardTimer, 500);
				}
				
				if(RewindFlag)
				{
					clearTimeout(RTimer);
					RTimer = setTimeout(RewindTimer, 500);
				}	
			}
		}
		 
		private function vidStateChange(e:VideoEvent):void
		{
			//dispatchEvent(new Event(e.state, true, false));

			if (e.state == VideoState.CONNECTION_ERROR)
			{
				tracer("INFO", ">> STATE_CHANGE - ERROR: " + e.state);
			}
			else
			{
				tracer("INFO", "VIDSTATE: " + e.state);

				if (e.state == "playing")
				{
					myVid.visible = true;
					//trace("globalStart: "+globalStart);
					//trace("myVid.playheadTime: "+myVid.playheadTime);
					//myVid.visible = myVid.playheadTime > globalStart;
				}
				
				if (e.state == "stopped" && myVid.playheadTime >= (globalStop))
				{
					showMenu();

					try
					{
						myVid.playheadTime=globalStart;
					}
					catch (e:VideoError)
					{
						tracer("WARN", "ERR8 ERROR: " + e.code + " :: " + e);
					}
					mc_pp.gotoAndStop(2);
				}

				if (e.state == "buffering" || e.state == "loading")
				{
					myVid.bufferTime=3;
					
					loader_mc.visible=true;
					loader_mc.gotoAndPlay(2);
				}
				else
				{
					myVid.bufferTime=10;
					
					loader_mc.gotoAndStop(1);
					loader_mc.visible=false;
				}
				
				
			}

		}


		/**
		 * Displays errors from various operations, tpyically fatal conditions
		 *
		 *@param		arg		String		Error message/indicator text to display in the error dialog/window
		 */
		private function errorDisplay(arg:String):void
		{
			tracer("WARN", "Error Occured :" + arg);
			SizeItems();
			clearTimeout(getVarsInt);

			if (getChildByName("prevLoader"))
			{
				try
				{
					prevLoader.visible=false;
				}
				catch (err:Error)
				{
					tracer("WARN", "ERR9 Error prevLoader visible:" + err.message);
				}
			}
			activeBTN(false);
			lockUI(false);
			menu_mc.enabled=false;
			thumbnailUnavailable.visible=true;
			thumbnailUnavailable.dispText.text=arg;
			mc_pp.gotoAndStop(1);
			mc_pp.static_mc.enabled=false;
			mcPlay.enabled=false;
			mcPlay.visible=false;
			if(IsRawMedia.toLocaleUpperCase() == "TRUE")
			{
				trace(".........FF/Rew Disabled errorDisplay........");
				EnableForwardRewind(false);
				
			}
			thumbnailUnavailable.width=sw;
			thumbnailUnavailable.height=sh - 28;
		}


		/**
		 * Handles parsing of file name, FMS app name, location, RTMP URL's for both audio and video, including Mp4 and H.264/mov FMS url handling
		 *
		 *@param		m_appName		String		App name of the FMS instance, can be different depending on CDN
		 */
		private function GetNSConnection(m_appName:String):void
		{
			if (vidname == "temp_unavailable.flv")
			{
				tracer("INFO", "Video Temporarily Unavailable");
				errorDisplay("Video Temporarily Unavailable");
			}
			else
			{
				thevidplays=1;
				
				var TempVidName:String;
				TempVidName=vidname;

				if (hasVideo == "FALSE")
				{
					previewPersistent = true;
					//vidname=vidname.substring(0, vidname.indexOf("."));
					vidname="mp3:" + vidname.substring(0, vidname.indexOf("."));
					TempVidName="mp3:"+TempVidName;
					
					//vidname=vidname.substring(0, vidname.indexOf("."));
					var mvd:String = "rtmpe://" + app_source + "/" + m_appName + "/" + vidname;
					
					if (willAutoPlay == "TRUE")
					{
						var imgPreviewURL:String = srvString + "/svc/clip/previewImage?eid=" + initClip;
						var loaderContext:LoaderContext = new LoaderContext();
						loaderContext.checkPolicyFile=true;
						try
						{
							prevLoader.load(new URLRequest(imgPreviewURL), loaderContext);
						}
						catch (err:Error)
						{
							tracer("WARN", "ERR10 ERROR: " + err);
						}						
						
						prevLoader.contentLoaderInfo.addEventListener(Event.COMPLETE, onLoadAudioPreviewComplete);
						prevLoader.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR, onErrorAudioPreview);
						prevLoader.contentLoaderInfo.addEventListener(HTTPStatusEvent.HTTP_STATUS, prevLoaderAudio_httpStatusHandler);
					}
				}
				else
				{
					if (vidname.substr(vidname.length - 4, 4).toUpperCase() == ".MP4")
					{
						vidname="mp4:" + vidname.substring(0, vidname.indexOf("."));
						TempVidName="mp4:"+TempVidName;
					}
					
					if(IsRawMedia.toLocaleUpperCase()=="TRUE")
					{
						//var mvd = "rtmp://" + app_source + "/" + m_appName + "/" + "105t/media/2010/07/23/03/WOFL_20100723_03.flv";
						var mvd:String = "rtmpe://" + app_source + "/" + m_appName + "/" + vidname;
					}
					else
					{
						var mvd:String = "rtmpe://" + app_source + "/" + m_appName + "/" + vidname;
					}					
				}
				tracer("INFO", "Video:" + mvd);

				mc_pp.gotoAndStop(1);
				if(IsRawMedia.toLocaleUpperCase() =="TRUE")
				{
					trace(".........FF/Rew Disabled GetNSConnection........");
					EnableForwardRewind(false);
				}
				
				myVid.autoPlay = false;

				if (vidname == "" || vidname == " " || vidname == null || vidname == undefined)
				{
					tracer("WARN", "Bad RTMP STREAM NAME");
					errorDisplay("Invalid Stream Name - Temporarily Unavailable");					
				}
				else
				{
					// handle back to back streams with same name but different start points					
					
					if (prevStream == mvd)
					{
						myVid.seek(globalStart);
						myVid.play();
						mc_pp.gotoAndStop(3);
					}
					else
					{
						VideoPlayer.iNCManagerClass=CustomNCManager;
						CustomNCManager.AppName=m_appName;
						CustomNCManager.StreamName=TempVidName.substr(0,TempVidName.indexOf("."));
						/*CustomNCManager.StreamName=vidname.substr(0,vidname.indexOf("."));*/
						
						try
						{
							myVid.source=mvd;
							//tracer("INFO", "myVid.source="+mvd+"");
							
						}
						catch (e:VideoError)
						{
							tracer("WARN", "ERR11 ERROR: " + e.code + " :: " + e);
						}
						myVid.alpha=0;
						myVid.volume=0;
					}
					//myVid.ncMgr.videoPlayer.netConnection.connect("rtmpe://fms.iqmediacorp.com:1935/iqmedia");
				}
				prevStream=mvd;
			}

			mcPlay.visible=false;
		}


		/**
		 * Handles meta-data event fired by FMS - not always loaded when expected due to various video formats, and FMS behavious
		 *
		 *@param		evt		MetadataEvent		Object from FMS containing metadata about the current video
		 */
		private function handleMetadata(evt:MetadataEvent):void
		{
			tracer("INFO", "VIDEO METADATA EVENT :: LOADED");
			SizeItems();
			myVid.visible=true;
		}


		/**
		 * Handle security errors - wrong context, etc.
		 *
		 *@param		evt		SecurityErrorEvent		Object from FMS containing metadata about the current video
		 */
		private function netSecurityError(evt:SecurityErrorEvent):void
		{
			tracer("WARN", "NETSECURITY ERROR: " + evt.text);
		}


		/**
		 * Handles meta-data event fired by FMS - not always loaded when expected due to various video formats, and FMS behavious
		 *
		 *@param		evt		asyncErrorHandler		Object from FMS containingasync error message
		 */
		private function asyncErrorHandler(evt:AsyncErrorEvent):void
		{
			tracer("WARN", "ASYNC ERROR: " + evt.text);
		}


		/**
		 * Handles setting up the initial playback of the video info from UI elements
		 *
		 *@param		e		Event		Either a click o some other user generated event or app dispatched event
		 */
		public function SetupPlay(e:Event):void
		{
			tracer("INFO", "READY: Setup PlayBack");
			mcPlay.visible = false;	
			
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				/*myVid.ncMgr.videoPlayer.netStream.bufferTime = 3600;*/
				myVid.ncMgr.videoPlayer.netStream.backBufferTime = 120;
			}
			
			activeBTN(true);
			if (seekAndStart)
			{
				seekAndStart=false;

				try
				{
					if(IsRawMedia.toLocaleUpperCase()=="TRUE" && firstTimeLoadRawMedia==true)
					{
						// changed by meghana on 26-June-2012 chnaged
						// from initialOffset<=3599 to  initialOffset <= globalStop Condition 
						// as now our etime can be any
						if(initialOffset>=0 && initialOffset <= globalStop)
						{
							setSeekPoint(Number(initialOffset));
							
							myVid.playheadTime=initialOffset;							
						}
						else
						{
							myVid.playheadTime=seekStartPoint;
						}
						
						firstTimeLoadRawMedia=false;
					}
					else
					{
						myVid.playheadTime=seekStartPoint;
					}
				}
				catch (e:VideoError)
				{
					tracer("WARN", "ERR12 ERROR: " + e.code + " :: " + e);
				}
			}
			else
			{
				try
				{
					if(IsRawMedia.toLocaleUpperCase()=="TRUE" && firstTimeLoadRawMedia==true)
					{
						// changed by meghana on 26-June-2012 chnaged
						// from initialOffset<=3599 to  initialOffset <= globalStop Condition 
						// as now our etime can be any
						if(initialOffset>=0 && initialOffset <= globalStop)
						{
							setSeekPoint(Number(initialOffset));
							
							myVid.playheadTime=initialOffset;
						}
						else
						{
							myVid.playheadTime=globalStart;
						}
						
						firstTimeLoadRawMedia=false;
					}				
					else
					{
						myVid.playheadTime=globalStart;
					}					
				}
				catch (e:VideoError)
				{
					tracer("WARN", "ERR13 ERROR: " + e.code + " :: " + e);
				}
			}
			
			

			if (previewFirst)
			{
				if (!previewPersistent)
				{
					try
					{
						Tweener.addTween(prevLoader, {alpha: 0, time: .5, transition: "linear"});
					}
					catch (err:Error)
					{
						tracer("WARN", "ERR14 ERROR: " + err);
					}
				}
				thumbnailUnavailable.visible=false;
			}
			
			Scrub.addEventListener(MouseEvent.MOUSE_OVER, SetScrub);
			Scrub.addEventListener(MouseEvent.MOUSE_OUT, SetScrubOff);
			Scrub.addEventListener(MouseEvent.CLICK, SetScrubClick);
			bKnob.addEventListener(MouseEvent.MOUSE_OVER, bknob_over);
			bKnob.addEventListener(MouseEvent.MOUSE_OUT, bknob_out);
			bKnob.addEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
			bKnob.addEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
			mc_pp.gotoAndStop(2);
			//loader_mc.visible=false;

			if (willAutoPlay == null || willAutoPlay != "TRUE")
			{
										
						if(hasVideo == "FALSE") 
						{
							 myVid.play();
							 mcPlay.visible = false; 
						  	 mc_pp.gotoAndStop(3);
						} 
						else 
						{
						
							try
							{
								//myVid.pause();
								myVid.play();
								mcPlay.visible = false;
								mc_pp.gotoAndStop(3);
							}
							catch (e:VideoError)
							{
								tracer("WARN", "ERR16 ERROR: " + e.code + " :: " + e);
							}			
						
							/*if(videoStatus.preRollSKIP) 
							{
								videoStatus.preRollSKIP=false;
								 myVid.play();
								 mcPlay.visible = false;
								 mc_pp.gotoAndStop(3);
							}	*/					
						}						
					
				
				myVid.visible=false;
			}
			else
			{

				try
				{
					if(firstTimeLoadClip == false || IsRawMedia.toLocaleUpperCase()=="TRUE")
					{
						myVid.play();
						mc_pp.gotoAndStop(3);
					}
					else if(firstTimeLoadClip && IsRawMedia.toLocaleUpperCase()=="FALSE")
					{
						//trace("manually lock on SetupPlay");
						lockUI(false);
						loader_mc.visible=true;
						loader_mc.gotoAndPlay(1);
					}
				}
				catch (e:VideoError)
				{
					tracer("WARN", "ERR17 ERROR: " + e.code + " :: " + e);
				}
			}
			
			/*if(videoStatus.adsEnabled == false) {
				myVid.play();
				mc_pp.gotoAndStop(3);
			}*/
			
			if(IsLoadMetaDisplayed == 0)
			{
				meta_mc.y = mcStopy;
				metaDisplayCore();
				IsLoadMetaDisplayed = 1; 				
				TimerMetaDisplay = setTimeout(metaDisplayCore, 10000); // 1 second
			}

			if (willAutoPlay == "TRUE")
			{
				//loggedPlay = false;
			}
			
			//logPlay();
		}


		// ******************* Data Functions *****************************

		/**
		 * Handles prep and loading of the XML/REST IQMedia services "getVars"
		 *
		 */
		private function GetVarInfo():void
		{
			if (!varsLoaded || (varsLoaded && widgetMode == "TRUE"))
			{
				xmlType="Var";
				if (pid == null || pid == undefined) {
					pid = "";
				}
				
				
				if(IsRawMedia.toLocaleUpperCase()=="TRUE")
				{
					var getVarService:String =  srvString + "/svc/media/getVars?fid=" + guid;
				}
				else
				{
					var getVarService:String = srvString + "/svc/media/getVars?local=" + PlayerFromLocal + "&fid=" + guid;
				}
				
				GetXml(guid, getVarService);
			}
			else
			{
				tracer("INFO", "Grabbing app/streamname: "+m_appName);				
				
				GetNSConnection(m_appName);				
			}

		}		
		
		private function GetClosedCaption(gStart:Number,gStop:Number)
		{
			try
			{
				getClosedCaptionInt=setTimeout(ParseClosedCaptionTimeout, serviceTimeout);
				
				var CCInfo:String;
				
				if(IsRawMedia.toLocaleUpperCase()=="TRUE")
				{
					 CCInfo= srvString + "/svc/cc/getClosedCaption?fid=" + guid + "&startTime=" + gStart + "&endTime=" + gStop;
					/*var CCInfo ="http://qaservices.iqmediacorp.com/Isvc/Statskedprog/getData/";*/
				}
				else
				{			
					CCInfo = srvString + "/svc/cc/getClosedCaption?fid=" + guid;
				}
				
				var xmlURLReq:URLRequest ;
			
				var xmlSendLoad:URLLoader = new URLLoader();			
				xmlSendLoad.addEventListener(IOErrorEvent.IO_ERROR, onCaptionIOError, false, 0, true);
				
				xmlURLReq= new URLRequest();
				xmlURLReq.url=CCInfo;
					
				xmlSendLoad.addEventListener(Event.COMPLETE, ParseClosedCaption, false, 0, true);
				
				xmlURLReq.contentType="text/xml";
				xmlURLReq.method=URLRequestMethod.GET;
				
				try
				{
					xmlSendLoad.load(xmlURLReq);
				}
				catch (err:Error)
				{
					tracer("WARN", "Error getClosedCaption load");					
					dispCap.text=CCUnavailable;
				}				
			}
			catch(err:Error)
			{
				tracer("WARN", "Error getClosedCaption load");
				dispCap.text=CCUnavailable;
			}
		}

		private function ParseClosedCaption(e:Event):void
		{
			try
			{
				clearTimeout(getClosedCaptionInt);
				
				listCCBegin=new Vector.<int>();
				listCCEnd=new Vector.<int>();
				listCCText=new Vector.<String>();
				
				var _xml:XML = new XML(e.target.data);
				
				var _xmlns:Namespace=_xml.namespace();
				
				var _xmlList:XMLList=_xml._xmlns::body._xmlns::div._xmlns::p;
				
				var index:int=2;
				
				var beginSec:String;
				var endSec:String;
				
				listCCBegin[0]=-2;
				listCCBegin[1]=-1;
				
				listCCEnd[0]=-2;
				listCCEnd[1]=-1;
				
				listCCText[0]="";
				listCCText[1]="";
				
				if(IsRawMedia.toLocaleUpperCase()=="TRUE")
				{				
					for each (var CC:XML in _xmlList) 
					{
						beginSec=CC.@begin.toString();
						endSec=CC.@end.toString();
						
						listCCBegin[index]= parseInt(beginSec.substr(0,beginSec.length-1));
						listCCEnd[index]=parseInt(endSec.substr(0,endSec.length-1));
						listCCText[index]=CC.text();
						
						index=index+1;
					}
				}
				else
				{
					var intSec:int = -1;					
					
					for each (var CC:XML in _xmlList) 
					{
						beginSec=CC.@begin.toString();
						endSec=CC.@end.toString();
						
						listCCBegin[index]= parseInt(beginSec.substr(0,beginSec.length-1));
						
						if(intSec<0)
						{
							intSec= globalStart;
							listCCBegin[index]=0;
						}
						else
						{
							listCCBegin[index]=listCCBegin[index]-intSec;
						}
						
						listCCEnd[index]=parseInt(endSec.substr(0,endSec.length-1))-intSec;
						listCCText[index]=CC.text();
						
						index=index+1;
					}
				}
			}
			catch(err:Error)
			{
				tracer("WARN", "Error while parsing ClosedCaption");
				dispCap.text=CCUnavailable;
			}
		}				
		
		private function onCaptionIOError(e:*):void
		{			
			clearTimeout(getClosedCaptionInt);
			dispCap.text="Closedcaption tempory unavailable";
		}

		// Helper to load new page when station logo is clicked in metadata dropdown display
		private function clkStation(e:Event)
		{
			tracer("INFO", "GOING TO: " + stURL.url);
			navigateToURL(stURL);
		}




		private function thumbInit(evt:Event):void
		{
			try
			{
				//meta_mc.station_logo.addChild(evt.target);

				if (thumbLdr.content is Bitmap)
				{
					Bitmap(thumbLdr.content).smoothing=true;
				}
	
				thumbLdr.width=57;
				thumbLdr.height=57;
	
				meta_mc.station_logo.mouseEnabled=true;
	
				meta_mc.station_logo.buttonMode=true;
				meta_mc.station_logo.addEventListener(MouseEvent.CLICK, clkStation);
			}
			catch (err:Error)
			{
				tracer("WARN", "Error thumbInit load:" + err.message);					
			}
		}
		
		private function GetPlayerInfo():void
		{
			try
			{				
				getPlayerInfoInt=setTimeout(ParsePlayerInfoTimeout, serviceTimeout);

				var xmlURLReq:URLRequest ;
				
				var xmlSendLoad:URLLoader = new URLLoader();			
				xmlSendLoad.addEventListener(IOErrorEvent.IO_ERROR, onIOError, false, 0, true);
				
				xmlURLReq= new URLRequest();
				xmlURLReq.url=srvString+"/iqsvc/Statskedprog/GetPlayerData?ID="+guid+"&Type="+MediaType;
					
				xmlSendLoad.addEventListener(Event.COMPLETE, ParsePlayerInfo, false, 0, true);
				
				xmlURLReq.contentType="text/xml";
				xmlURLReq.method=URLRequestMethod.GET;			
				try
				{
					xmlSendLoad.load(xmlURLReq);
				}
				catch (err:Error)
				{
					clearTimeout(getPlayerInfoInt);
					tracer("WARN", "Error getPlayerInfo load:" + err.message);					
				}
			}
			catch(err:Error)
			{
				clearTimeout(getPlayerInfoInt);
				tracer("WARN", "Error getPlayerInfo load:" + err.message);		
			}
		}
		
		/*public function mouseOverMeta1Txt(event:MouseEvent):void 
		{
			meta_mc.meta1Txt.text= meta1_textString;
			var myFormatBlackSelect:TextFormat = new TextFormat();
			myFormatBlackSelect.font = "Verdana";
			myFormatBlackSelect.size = 12;
			myFormatBlackSelect.color = 0x393939;
			meta_mc.txtDmaMarket.setTextFormat(myFormatBlackSelect);
			meta_mc.txtDmaRank.setTextFormat(myFormatBlackSelect);
		}
		
		public function mouseOutMeta1Txt(event:MouseEvent):void 
		{
			if(meta1_textString.length > 30)
			{
				meta_mc.meta1Txt.text= meta1_textString.substr(0,28) +'...';
			}
			else
			{
				meta_mc.meta1Txt.text= meta1_textString;
			}
			var myFormatBlackOut:TextFormat = new TextFormat();
			myFormatBlackOut.font = "Verdana";
			myFormatBlackOut.size = 12;
			myFormatBlackOut.color = 0xFFFFFF;
			meta_mc.txtDmaMarket.setTextFormat(myFormatBlackOut);
			meta_mc.txtDmaRank.setTextFormat(myFormatBlackOut);
		}*/
		
		public function mouseOverMeta3Txt(event:MouseEvent):void 
		{
			var strHelper:StringHelper = new StringHelper();
			if(strHelper.trim(Title120," ") == "")
			{
				meta_mc.meta3Txt.text = "Title: NA";
			}
			else
			{
				meta_mc.meta3Txt.text =  "Title: " + Title120;
			}
			var myFormatBlackSelect:TextFormat = new TextFormat();
			myFormatBlackSelect.font = "Verdana";
			myFormatBlackSelect.size = 12;
			myFormatBlackSelect.color = 0x393939;
			meta_mc.txtNeilSenAd.setTextFormat(myFormatBlackSelect);
			
		}
		
		public function mouseOutMeta3Txt(event:MouseEvent):void 
		{
			var strHelper:StringHelper = new StringHelper();
			if(strHelper.trim(Title120," ") == "")
			{
				meta_mc.meta3Txt.text = "Title: NA";
			}
			else
			{
				if(Title120.length > 42)
				{
					meta_mc.meta3Txt.text =  "Title: " + Title120.substr(0,40)+'...';
				}
				else
				{
					meta_mc.meta3Txt.text =  "Title: " + Title120;
				}
			}
			var myFormatBlackOut:TextFormat = new TextFormat();
			myFormatBlackOut.font = "Verdana";
			myFormatBlackOut.size = 12;
			myFormatBlackOut.color = 0xFFFFFF;
			meta_mc.txtNeilSenAd.setTextFormat(myFormatBlackOut);
		}
		
		private function ParsePlayerInfo(e:Event):void
		{
			try
			{
				clearTimeout(getPlayerInfoInt);
			
				var ResultXml:XML=new XML(e.target.data);
				//ResultXml = new XML('<PlayerInfo><IQ_Dma_Name>New York</IQ_Dma_Name><IQ_Local_Air_Date>2012-06-20T00:00:00</IQ_Local_Air_Date><Title120s><Title120>Nightline</Title120><Title120>Jimmy Kimmel Live</Title120></Title120s><IQ_Start_Points><IQ_Start_Point>2</IQ_Start_Point><IQ_Start_Point>4</IQ_Start_Point></IQ_Start_Points><IQ_Start_Minutes><IQ_Start_Minute>21</IQ_Start_Minute><IQ_Start_Minute>46</IQ_Start_Minute></IQ_Start_Minutes><IQ_Dma_Num>001</IQ_Dma_Num></PlayerInfo>');
				
				var dateTimeFormatter:DateTimeFormatter =new DateTimeFormatter("en-US","short","short");
				dateTimeFormatter.setDateTimePattern("yyyy-MM-dd hh:mm a");
				var strHelper:StringHelper = new StringHelper();
				if(MediaType.toLocaleLowerCase()=="rawmedia")
				{
					//meta1_textString ="THIS IS TEST FOR LONG TEXT WRAPPING OR NOT";
					/*if(meta1_textString.length > 30)
					{
						meta_mc.meta1Txt.text= meta1_textString.substr(0,28) +'...';
					}
					else
					{*/
						meta_mc.meta1Txt.text= meta1_textString;
					/*}*/
					
					

					ListTitle120 = ResultXml.Title120s.Title120;
					ListIQStartPoint = ResultXml.IQ_Start_Points.IQ_Start_Point;
					ListIQStartMinute = ResultXml.IQ_Start_Minutes.IQ_Start_Minute;
					
					if(strHelper.trim(ResultXml.IQ_Dma_Name.text()," ") != "")
					{
						var IQ_Dma_Name:String = ResultXml.IQ_Dma_Name.text();
						/*if(IQ_Dma_Name.length > 12)
						{
							meta_mc.txtDmaMarket.text = "DMA Market | "+IQ_Dma_Name.substr(0,10)+'...';
						}
						else
						{*/
							meta_mc.txtDmaMarket.text = "DMA Market : "+IQ_Dma_Name;
						/*}*/
					}
					else
					{
						meta_mc.txtDmaMarket.text = "DMA Market : NA";
					}
					
					if(strHelper.trim(ResultXml.IQ_Dma_Num.text()," ") != "")
					{
						meta_mc.txtDmaRank.text = "Rank : "+ResultXml.IQ_Dma_Num.text();
					}
					else
					{
						meta_mc.txtDmaRank.text = "Rank : NA";
					}
					
					
					var LocalAirDate:String=ResultXml.IQ_Local_Air_Date.text();				
					if(strHelper.trim(LocalAirDate," ") != "")
					{
						LocalAirDate=LocalAirDate.replace("T"," ");
						var tempDate:Date=new Date(Number(LocalAirDate.substr(0,4)),Number(LocalAirDate.substr(5,2))-1,Number(LocalAirDate.substr(8,2)),Number(LocalAirDate.substr(11,2)),Number(LocalAirDate.substr(14,2)));				
						meta_mc.meta2Txt.text="Local Air Date & Time: "+dateTimeFormatter.format(tempDate);
					}
					else
					{
						meta_mc.meta2Txt.text="Local Air Date & Time: NA";
					}
					
					//Title120 =ResultXml.Title120.text();
					//if(strHelper.trim(Title120," ") == "")
					//{
					//	meta_mc.meta3Txt.text = "Title: NA";
					//}
					//else
					//{
					//	if(Title120.length > 42)
					//	{
					//		meta_mc.meta3Txt.text =  "Title: " + Title120.substr(0,40)+'...';
					//	}
					//	else
					//	{
					//		meta_mc.meta3Txt.text =  "Title: " + Title120;
					//	}
					//}
					
				}
				else if(MediaType.toLocaleLowerCase()=="ugc")
				{
					var LocalAirDate:String=ResultXml.AirDate.text();				
					
					meta_mc.meta1Txt.text=meta1_textString;
					if(strHelper.trim(LocalAirDate," ") != "")
					{
						LocalAirDate=LocalAirDate.replace("T"," ");
						var tempDate:Date=new Date(Number(LocalAirDate.substr(0,4)),Number(LocalAirDate.substr(5,2))-1,Number(LocalAirDate.substr(8,2)),Number(LocalAirDate.substr(11,2)),Number(LocalAirDate.substr(14,2)));				
						meta_mc.meta2Txt.text="UGC | "+dateTimeFormatter.format(tempDate);
					}
					
					meta_mc.meta3Txt.text = ResultXml.Title.text();
					PlayerDataKeyWord = ResultXml.Keywords.text() == undefined  ? "" : ResultXml.Keywords.text();
					PlayerDataDescription = ResultXml.Description.text() == undefined  ? "" : ResultXml.Description.text();
					
				}
				else if(MediaType.toLocaleLowerCase()=="clip")
				{
					if(strHelper.trim(ResultXml.IQ_Dma_Name.text()," ") != "")
					{
						var IQ_Dma_Name:String = ResultXml.IQ_Dma_Name.text();
						/*if(IQ_Dma_Name.length > 12)
						{
							meta_mc.txtDmaMarket.text = "DMA Market | "+IQ_Dma_Name.substr(0,10)+'...';
						}
						else
						{*/
							meta_mc.txtDmaMarket.text = "DMA Market : "+IQ_Dma_Name;
						/*}*/
					}
					else
					{
						meta_mc.txtDmaMarket.text = "DMA Market : NA";
					}
					
					if(strHelper.trim(ResultXml.IQ_Dma_Num.text()," ") != "")
					{
						meta_mc.txtDmaRank.text = "Rank : "+ResultXml.IQ_Dma_Num.text();
					}
					else
					{
						meta_mc.txtDmaRank.text = "Rank : NA";
					}
					
					var LocalAirDate:String=ResultXml.IQ_Local_Air_Date.text();				
					meta_mc.meta2Txt.text=ResultXml.StationID.text();
					if(strHelper.trim(LocalAirDate," ") != "")
					{
						if(strHelper.trim(meta_mc.meta2Txt.text," ") != "")
						{
							meta_mc.meta2Txt.text+= " | ";
						}
						LocalAirDate=LocalAirDate.replace("T"," ");
						var tempDate:Date=new Date(Number(LocalAirDate.substr(0,4)),Number(LocalAirDate.substr(5,2))-1,Number(LocalAirDate.substr(8,2)),Number(LocalAirDate.substr(11,2)),Number(LocalAirDate.substr(14,2)));				
						meta_mc.meta2Txt.text+= dateTimeFormatter.format(tempDate);
					}
					
					Title120 =ResultXml.Title120.text();
					if(strHelper.trim(Title120," ") == "")
					{
						meta_mc.meta3Txt.text = "Title: NA";
					}
					else
					{
						if(Title120.length > 42)
						{
							meta_mc.meta3Txt.text =  "Title: " + Title120.substr(0,40)+'...';
						}
						else
						{
							meta_mc.meta3Txt.text =  "Title: " + Title120;
						}
					}
					
				}
				
				if(MediaType.toLocaleLowerCase()=="rawmedia" || MediaType.toLocaleLowerCase()=="clip")
				{
					CallNielSenDataService(ResultXml.IQ_Dma_Num.text());
					//meta_mc.meta1Txt.addEventListener(MouseEvent.MOUSE_OVER, mouseOverMeta1Txt);
					//meta_mc.meta1Txt.addEventListener(MouseEvent.MOUSE_OUT, mouseOutMeta1Txt);
					meta_mc.meta3Txt.addEventListener(MouseEvent.MOUSE_OVER, mouseOverMeta3Txt);
                    meta_mc.meta3Txt.addEventListener(MouseEvent.MOUSE_OUT, mouseOutMeta3Txt);
				}
			}
			catch (err:Error)
			{
				clearTimeout(getPlayerInfoInt);
				tracer("WARN", "Error ParsePlayerInfo" + err.message);
			}
		}
		
		private function ParsePlayerInfoTimeout():void
		{
			clearTimeout(getPlayerInfoInt);
			tracer("WARN", "Error getPlayerInfo timeout");			
		}
		
		private function embedmcClick(e:Event)
		{
			menubg.embed_mc.mouseEnabled = false;
		}	
		
		private function linkmcClick(e:Event)
		{
			menubg.link_mc.mouseEnabled = false;
		}	
		
		private function emailmcClick(e:MouseEvent)
		{
			menubg.email_mc.mouseEnabled = false;
		}	

		/**
		 * Helper function to truncate text based on a given textField and area to fit it into
		 * will try to break on spaces and add an ellipsis
		 *
		 *@param		str			TextField		textField to truncate
		 *@param		widthLimit	Number			Width in pixels to try and fit inside/truncate to
		 */
		public function truncMeta(str:TextField, widthLimit:Number):String
		{
			while (checkMetricWidth(str) >= widthLimit)
			{
				var a:Array = str.text.split(" ");
				a.length--;
				str.text=a.join(" ") + "…";
			}
			return str.text;
		}


		// helper function for truncMeta to provide textfiled line metrics
		public function checkMetricWidth(str:TextField):Number
		{
			var metrics:TextLineMetrics = str.getLineMetrics(0);
			return metrics.width;
		}



		/**
		 * Function for returning formatted time based on minutes
		 *
		 *@param		sec		Number		minutes in
		 */
		private function timeCode(sec:Number):String
		{
			var h:Number = Math.floor(sec / 3600);
			var m:Number = Math.floor((sec % 3600) / 60);
			var s:Number = Math.floor((sec % 3600) % 60);
			var fulltime:String;

			if (h > 0 || globalStop >= 3600)
			{
				fulltime=(h < 10 ? "0" + h.toString() : h.toString()) + ":" + (m < 10 ? "0" + m.toString() : m.toString()) + ":" + (s < 10 ? "0" + s.toString() : s.toString());
			}
			else
			{
				fulltime=(m < 10 ? "0" + m.toString() : m.toString()) + ":" + (s < 10 ? "0" + s.toString() : s.toString());
			}
			
			return fulltime;
		}
		
		private function timeCodeWithMS(sec:Number):String
		{
			var h:Number = Math.floor(sec / 3600);
			var m:Number = Math.floor((sec % 3600) / 60);
			var s:Number = Math.floor((sec % 3600) % 60);
			var ms:Number=Math.floor((((sec % 3600) % 60)%1)*1000);
			
			/*if(keyLeft==true)
			{	*/		
				if(ms>=500)
				{
					ms=500;
					
					//s=s+1;
				}
				else
				{
					ms=0;
					//s=s-1;
				}
			/*}*/
			/*else if(keyRight==true)
			{
				if(ms>=500)
				{
					ms=0;
					
					//s=s+1;
				}
				else
				{
					ms=500;
					//s=s-1;
				}
			}*/
			
			var fulltime:String;

			if (h > 0)
			{
				fulltime=(h < 10 ? "0" + h.toString() : h.toString()) + ":" + (m < 10 ? "0" + m.toString() : m.toString()) + ":" + (s < 10 ? "0" + s.toString() : s.toString())+ ":" + (ms < 100 ? "00" + ms.toString() : ms.toString());
			}
			else
			{
				fulltime=(m < 10 ? "0" + m.toString() : m.toString()) + ":" + (s < 10 ? "0" + s.toString() : s.toString())+ ":" + (ms < 100 ? "00" + ms.toString() : ms.toString());
			}
			
			return fulltime;
		}


		//**************** Interface Functions ****************************************

		/**
		 * Handler for cursor leaving screen
		 *cmb
		 *@param		e		Event		success event data
		 */
		private function cursorOFFscreen(evt:Event):void
		{
			// bottomBarSlideOff ();
			Tweener.addTween(CC_btn, {alpha: 0, time: .3, transition: "linear"});
			Tweener.addTween(meta_mc, {alpha: 0, time: .3, transition: "linear"});
			stage.addEventListener(MouseEvent.MOUSE_MOVE, cursorONscreen);
		}


		/**
		 * Handler for cursor entering screen
		 *
		 *@param		e		Event		success event data
		 */
		private function cursorONscreen(evt:MouseEvent):void
		{
			if (mouseX > 0 && mouseX <= stage.stageWidth && mouseY > 0 && mouseY < stage.stageHeight)
			{
				// bottomBarSlideOn ();
				Tweener.addTween(CC_btn, {alpha: 1, time: .3, transition: "linear"});
				Tweener.addTween(meta_mc, {alpha: 1, time: .3, transition: "linear"});
				stage.removeEventListener(MouseEvent.MOUSE_MOVE, cursorONscreen);
			}
		}		


		/**
		 * Mouse event handler for use with volume slider
		 *
		 *@param		event		MouseEvent		mouse event
		 */
		private function mouseUp(event:MouseEvent):void
		{
			vol.vol_mc.volKnob.stopDrag();
			bKnob.stopDrag();
			bKnobStat=0;
			clearInterval(progInt);
			clearInterval(overInt);
			clearInterval(volInt);
			tknob_mc.alpha=0;
			bKnob.alpha=0;
			Tweener.addTween(vol.vol_mc, {x: 0, y: 150, time: .3, transition: "linear"});
			stage.removeEventListener(MouseEvent.MOUSE_UP, mouseUp);
		}


		/**
		 * Handles display of dropdown metainfo display
		 *
		 *@param		e		Event		success event data
		 */
		private function metaDisplayCore():void
		{
			if(IsLoadMetaDisplayed == 1)
			{
				clearInterval(TimerMetaDisplay);
			}
			
			var metaDest:Number = (-1 * meta_mc.height) + 10;

			if (meta_mc.y != 0)
			{
				Tweener.addTween(meta_mc, {y: 0, time: .3, transition: "easeOutQuint"});
				Tweener.addTween(meta_mc.toggle_mc, {rotation: -180, time: .5, transition: "easeInOutBack"});
				metaState=true;
			}
			else
			{
				Tweener.addTween(meta_mc, {y: mcStopy, time: .3, transition: "easeInQuint"});
				Tweener.addTween(meta_mc.toggle_mc, {rotation: 0, time: .5, transition: "easeInOutBack"});
				metaState=false;
			}
		}


		// Helper function to metaDisplayCore to accept event when called via an event instead of directly
		private function metaDisplay(e:Event)
		{
			metaDisplayCore();
		}


		/**
		 * Function for updating the onscreen time for video
		 *
		 *@param		e		Event		timer event
		 */
		private function UpdateCurrentTime(e:Event)
		{
			
			if(IsLogPlay == "false")
			{
				logPlay();
				IsLogPlay = "true";
				menu_mc.mouseEnabled=true;
			}
			
			

			var actualTime:Number=e.target.playheadTime - globalStart;
			actualTime=actualTime<0?0:actualTime;
			var realTime:Number;
			
			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{			
				realTime= e.target.playheadTime /*- globalStart*/;
			}
			else
			{
				realTime= e.target.playheadTime - globalStart;
			}
			timeDisplayCur.text=timeCode((realTime < 0) ? 0 : realTime);			
			
			var CurrentSec:int =int(realTime);
			
			CurrentSec=(CurrentSec<0)?0:CurrentSec;
			
			ShowNielSenUpdate(realTime);
			ShowTitle120Update(realTime);
			
			if(IsRawMedia.toLocaleUpperCase() =="TRUE" && realTime > 5 && !IsInitEnabled)
			{
				IsInitEnabled = true;
				EnableForwardRewind(true);
			}
			if(listCCBegin!=null && listCCBegin.length>2)
			{
				DisplayCC(CurrentSec);			
			}
			else
			{			
				dispCap.text=CCUnavailable;
			}
			
			var BarWid:Number = Scrub.scrubBar.width;
			var BarDis:Number = BarWid / ftime;
			var BarSet:Number = actualTime * BarDis;
			var bdis_S:String = String(BarSet);
			Scrub.progBar.width=BarSet;

			if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				if (e.target.playheadTime < globalStart && e.target.playheadTime<0)
				{
					try
					{
						myVid.playheadTime=globalStart;
					}
					catch (e:VideoError)
					{
						tracer("WARN", "ERR24 ERROR: " + e.code + " :: " + e);
					}
				}
			}
			else
			{			
				if (e.target.playheadTime < globalStart)
				{
						try
						{
							myVid.playheadTime=globalStart;
						}
						catch (e:VideoError)
						{
							tracer("WARN", "ERR24 ERROR: " + e.code + " :: " + e);
						}
				}
			}
			
			if(IsClipperEnable==true)
			{
				if(e.target.playheadTime > customSeekPointRight)
				{
					//myVid.seek(customSeekPoint);
					mc_pp.gotoAndStop(2);
					myVid.pause();
					menuState="CLOSED";
					activeBTN(true);
					fadeOverlay(false);
	
					/*if (myVid.state == "disconnected" && videoStatus.reloading != true)
					{
						mcPlay.visible=true;
						mcPlay.enabled=true;
					}
					else
					{
						mcPlay.visible=false;
						mcPlay.enabled=false;
					}*/
		
					about_scrn.visible=false;
					dispCap.alpha=1;
					isCustomSeek=true;
					
				}
			}
			

			if (e.target.playheadTime > globalStop || e.target.playheadTime >= myVid.totalTime)
			{
				showMenu();
				try
				{
					myVid.playheadTime=globalStart;
				}
				catch (e:VideoError)
				{
					tracer("WARN", "ERR25 ERROR: " + e.code + " :: " + e);
				}
				myVid.pause();

				try
				{
					myVid.playheadTime=globalStart;
				}
				catch (e:VideoError)
				{
					tracer("WARN", "ERR26 ERROR: " + e.code + " :: " + e);
				}
				mc_pp.gotoAndStop(2);				
			}
		}

		function DisplayCC(CurrentSec:int):void
		{
			try
			{
				var CC:String="";
				var isSameSec:Boolean=true;
				var isMatch:Boolean=false;
				var tempCurrentPosition:int=CurrentCCPosition;
				var tempCurrentSec:int=CurrentSec;
				
				if(CurrentSec==0 && PreviousSecond>0)
				{
					CurrentCCPosition=2;
				}					
				do
				{
					if(CurrentSec==(PreviousSecond+1) || CurrentSec==PreviousSecond)
					{
						if(listCCBegin.length>CurrentCCPosition)
						{
							if(listCCBegin[CurrentCCPosition]==CurrentSec)
							{
								isMatch=true;
									
								CC=CC+" "+listCCText[CurrentCCPosition];
									
								CurrentCCPosition=CurrentCCPosition+1;
							}									
							else
							{
								isSameSec=false;
							}
						}
					}
					else
					{
						var indexOfSec:int=-1;
							
						indexOfSec=listCCBegin.indexOf(CurrentSec,0);
							
						if(indexOfSec>=0)
						{
							isMatch=true;
								
							CurrentCCPosition=indexOfSec;
							PreviousSecond=CurrentSec-1;
							tempCurrentPosition=indexOfSec;
						}
						else
						{
							var tempSec:int=CurrentSec;
							do
							{
								tempSec=tempSec-1;
								indexOfSec=listCCBegin.indexOf(tempSec,0);		
									
							}while(tempSec>=0 && indexOfSec<0);
								
							if(indexOfSec>=0)
							{
								isMatch=true;
								
								CurrentCCPosition=indexOfSec;								
								tempCurrentPosition=indexOfSec;
								CurrentSec=tempSec;
								PreviousSecond=CurrentSec-1;
							}
							else
							{
								isSameSec=false;
							}
						}						
					}
				}while(isSameSec && listCCBegin.length>CurrentCCPosition)
				
				
				if(isMatch)
				{
					CC=listCCText[tempCurrentPosition-2]+"\n"+listCCText[tempCurrentPosition-1]+"\n"+CC;
					
					dispCap.text=CC;
				}
				
				PreviousSecond=tempCurrentSec;
			}
			catch(err:Error)
			{
				trace("err :"+err.message+err.getStackTrace());
				dispCap.text=CCUnavailable;
			}
		}
		
		/**
		 * Function for handling initial video startup/play log some events, and trigger some adapTV stuff
		 *
		 *@param		e		Event		FLVPlayback event
		 */
		private function logInitialPlay(evt:Event):void
		{
			tracer("INFO", "PLAYING_STATE_ENTERED");
			mcPlay.visible=false;

			/*if (videoStatus.adsEnabled && videoStatus.firstPass)
			{
				adaptvPlayer3.contentStarted();
				if (videoStatus.preRollBreak == "BREAK_START") 
				{
					disableControls();
				}
			}*/

			/*if (videoStatus.adsEnabled && videoStatus.firstPass && videoStatus.preRollBreak == "BREAK_START") 
			{
				videoStatus.firstPass = false;
				myVid.pause();
			} else {
				// rhall here
				videoStatus.firstPass = false;
			}*/
			
			
			//myVid.volume = userVolume;
			if (!previewPersistent)
			{
				if (myVid.alpha != 1)
				{
					// getVolume was here - adjust for radio clips
					getVolume();
					Tweener.addTween(myVid, {alpha: 1, time: .3, transition: "linear"});
				}
			}
			
			if (hasVideo == "FALSE") {
				getVolume();
			}

			if (!loggedPlay)
			{
				tracer("INFO", "PlayState Entered, logging clip play");
				//logPlay();
			}
		}


		/**
		 * Function for handling logging data to logClipPlay service on playback by user
		 *
		 */
		private function logPlay():void
		{
			
			
			
			//if(IsRawMedia.toLocaleUpperCase()!="TRUE")
			//{
				if (!loggedPlay)
				{
					var DOMResults = queryDOM();
					var playingSend:URLLoader = new URLLoader();
					var pUrl:URLRequest = new URLRequest(LogPlayURLDynamic + "?fid=" + guid + "&ref=" + escape(DOMResults));
					tracer("INFO", "LOGCLIPPLAY URL: " + pUrl.url);
					pUrl.method=URLRequestMethod.GET;
	
					try
					{
						playingSend.load(pUrl);
						tracer("INFO", "Clip Playback Logged");
						loggedPlay=true;
					}
					catch (err:Error)
					{
						tracer("WARN", "ERR27 LOGCLIPPLAY ERROR: " + err);
					}
					
				}
			//}
		}


		/**
		 * Function for handling initial video playback via user control of play button
		 *
		 *@param		e		Event		FLVPlayback event
		 */
		private function playClip(e:Event)
		{
			// stop forward and rewind if it is running
			if(IsRawMedia.toLocaleUpperCase() =="TRUE" && (ForwardFlag || RewindFlag))
			{
				StopForwardRewind();
			}
			
			
			
			tracer("INFO", "LARGE PLAY CLICKED");
			var evt:Event = new Event("RLPlayer_PLAY_CLICKED", true, false);
			dispatchEvent(evt);
			
			// we only need it one time , then we'll make it unvisible,
			mcPlay.enabled=false;
			mcPlay.visible=false;
			myVid.visible=false;
			
			// video starts playing , show pause button
			mc_pp.gotoAndStop(3);
			
				try
				{
					// play video 
					tracer("INFO", "WAS PLAY!");
					myVid.play();
				}
				catch (e:VideoError)
				{
					tracer("WARN", "ERR29 ERROR: " + e.code + " :: " + e);
				}
		}


		/**
		 * Function for handling initial video playback via user control of play button
		 *
		 *@param		e		Event		MouseEvent
		 */
		private function playBtn(e:Event)
		{
			tracer("INFO", "PLAYPAUSE CLICKED");
			var evt:Event = new Event("RLPlayer_PLAY_CLICKED", true, false);
			dispatchEvent(evt);

			switch (mc_pp.currentFrame)
			{
				case 1:
					//tracer("INFO","CASE 1");
					break;
				case 2:
					//tracer("INFO","CASE 2");
					if(IsRawMedia.toLocaleUpperCase() =="TRUE" && (ForwardFlag || RewindFlag))
					{
						StopForwardRewind();
					}
					mc_pp.gotoAndStop(3);
					if (mcPlay.visible == true)
					{
						mcPlay.visible=false;
						myVid.visible=true;
					}

					/* START ADAPTV */
					/*if (videoStatus.adsEnabled)
					{
						tracer("INFO", "CASE 2: videoStatus_done:" + videoStatus_done);

						if (videoStatus_done)
						{
							myVid.playheadTime=globalStart;
							videoStatus_done=false;
							closeMenu();
							adaptvConfig.preload=false;
							adaptvConfig.autostartads=true;
							adaptvPlayer3.setMetadata(adaptvConfig);
							adaptvPlayer3.contentPlayheadChanged(this.getVideoStatus());
						}
						
					}*/

					/* END ADAPTV */
					if (myVid.playheadTime > globalStop)
					{
						try
						{
							//trace("On PlayBtn Event");
							myVid.play();
						}
						catch (e:VideoError)
						{
							tracer("WARN", "ERR1 ERROR: " + e.code + " :: " + e);
						}

						try
						{
							myVid.playheadTime=globalStart;
						}
						catch (e:VideoError)
						{
							tracer("WARN", "ERR2 ERROR: " + e.code + " :: " + e);
						}
						if(!FlagPopup)
						{
							menuState="OPEN";
							MenuMove(e);
						}
						
					}
					else
					{
						if(!FlagPopup)
						{
							menuState="OPEN";
							MenuMove(e);
						}

						try
						{
							//trace("On PlayBtn Event");
							myVid.play();
						}
						catch (e:VideoError)
						{
							tracer("WARN", "ERR3 ERROR: " + e.code + " :: " + e);
						}
					}
					if(IsRawMedia.toLocaleUpperCase() == "TRUE" && myVid.playheadTime>5 && !FlagPopup)
					{
						EnableForwardRewind(true);
					}
					break;
				case 3:
					//tracer("INFO","CASE 3");
					mc_pp.gotoAndStop(2);
					myVid.pause();
					if(!FlagPopup)
					{
						menuState="CLOSED";
						MenuMove(e);
					}
					
					if(IsRawMedia.toLocaleUpperCase() == "TRUE")
					{
						trace(".........FF/Rew Disabled PlayPause Case 3........");
						EnableForwardRewind(false);
					}
					
					break;
				default:
				
			}
			
			if(IsClipperEnable==true)
			{
				closeMenu();
				if(isCustomSeek==true)
				{					
					myVid.seek(customSeekPoint);
					isCustomSeek=false;
				}
			}			
		}


		/**
		 * Function for handling toggling of normal/fullscreen modes
		 *
		 *@param		e		Event		stage change/resize event
		 */
		public function ToggleFullScreen(e:Event)
		{
			
			var bigScreen:Boolean;

			if (widgetMode != "TRUE")
			{
				
				
				if (stage.displayState == StageDisplayState.FULL_SCREEN)
				{
					bKnob.alpha=0;
					bigScreen=true;					
					stage.displayState=StageDisplayState.NORMAL;
				}
				else
				{
					bigScreen=false;
					stage.displayState=StageDisplayState.FULL_SCREEN;
					bKnob.alpha=0;
					
				}

				if (widgetMode != "TRUE")
				{
					sw=stage.stageWidth;
					sh=stage.stageHeight;
				}
				else
				{
					sw=this.parent.width;
					sh=this.parent.height;
				}
				
				closeClipSaveUI();
				categories_mc.alpha = 0;
				categories_mc.x = 0 - categories_mc.width - 10;
				clipperUI.keyStatus = true;
				Tweener.addTween(clipperUI, {x: 0, y: stage.stageHeight + 50, time: 0, transition: "linear"});
				mc_pp.alpha=100;
				clipperUI.pp_mc.alpha=0;
				IsClipperEnable=false;
				Tweener.addTween(clipSave, {x: 0, y: stage.stageHeight + 50, time: 0, transition: "linear"});
				SizeItems();
				
				
			}
		}

		private function fadeOverlay(arg:Boolean):void
		{
			if (arg)
			{
				Tweener.addTween(overlay_mc, {alpha: .65, time: .3, transition: "linear"});
			}
			else
			{
				Tweener.addTween(overlay_mc, {alpha: 0, time: .3, transition: "linear"});
			}
		}


		private function showMenu():void
		{
			if (!userClicked)
			{				
				GetVarInfo();
			}
			
			fadeOverlay(false);

			if (myVid.state == "disconnected")
			{
				playToggle=true;
			}
			else
			{
				playToggle=false;
			}

			if (playToggle)
			{
				mcPlay.visible=true;
				mcPlay.enabled=true;
			}
			else
			{
				mcPlay.visible=false;
				mcPlay.enabled=false;

			}
			dispCap.alpha=.65;

			Tweener.addTween(emailShare, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			//Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			Tweener.addTween(embedVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			//Tweener.addTween(linkVideo, {x: linkStopx, y: linkStopy, time: .3, transition: "linear"});
			Tweener.addTween(linkVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			Tweener.addTween(menubg, {x: menuStartx, y: menuStarty, time: .5, transition: "easeOutQuint"});
			menuState="OPEN";
			menuSubState="CLOSED";
		}
		
		private function showRAWMediaMenu():void
		{
			if(IsRawMedia.toLocaleUpperCase() == "TRUE" && (ForwardFlag || RewindFlag))
			{
				popupDisplay("Action can't be performed \nas fast forward / rewind is running", 500, 2500);
			}
			else
			{
				if (!userClicked)
				{				
					GetVarInfo();
				}
				
				fadeOverlay(false);
	
				if (myVid.state == "disconnected")
				{
					playToggle=true;
				}
				else
				{
					playToggle=false;
				}
	
				if (playToggle)
				{
					mcPlay.visible=true;
					mcPlay.enabled=true;
				}
				else
				{
					mcPlay.visible=false;
					mcPlay.enabled=false;
	
				}
				dispCap.alpha=.65;
				
				if(IsRawMedia.toLocaleUpperCase() == "TRUE")
				{
					trace(".........FF/Rew Disabled ShowRawMedia Menu........");
					EnableForwardRewind(false);
				}
				
				menubg.link_mc.enabled = false;
				menubg.email_mc.enabled = false;
				menubg.embed_mc.enabled = false;
				Tweener.addTween(menubg, {x: menuStartx, y: menuStarty, time: .5, transition: "easeOutQuint"});
				menuState="OPEN";
				menuSubState="CLOSED";
			}
		}

		private function closeMenu():void
		{
			if(IsRawMedia.toLocaleUpperCase() == "TRUE" && myVid.playheadTime >5 && myVid.playing)
			{
				EnableForwardRewind(true);
			}
			menuState="CLOSED";
			activeBTN(true);
			fadeOverlay(false);
			if (myVid.state == "disconnected")
			{
				mcPlay.visible=true;
				mcPlay.enabled=true;
			}
			else
			{
				mcPlay.visible=false;
				mcPlay.enabled=false;
			}
			about_scrn.visible=false;
			Tweener.addTween(menubg, {x: menuStopx, y: menuStopy, time: .5, transition: "easeInQuint"});
			dispCap.alpha=1;
		}


		private function closeMenuandSharing():void
		{
			about_scrn.visible=false;
			Tweener.addTween(emailShare, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			//Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			Tweener.addTween(embedVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			//Tweener.addTween(linkVideo, {x: linkStopx, y: linkStopy, time: .3, transition: "linear"});
			Tweener.addTween(linkVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			fadeOverlay(false);
			mcPlay.enabled=true;
			mcPlay.visible=true;
			activeBTN(true);
			menuState="CLOSED";
		}


		private function MenuMove(e:Event)
		{
			if (menuState == "CLOSED")
			{
				showRAWMediaMenu();
			}
			else
			{
				if (menuState == "SHARING")
				{
					closeMenuandSharing();

					if (myVid.state == "disconnected")
					{
						mcPlay.visible=true;
						mcPlay.enabled=true;
					}
					else
					{
						mcPlay.visible=false;
						mcPlay.enabled=false;

					}
				}
				else
				{
					closeMenu();
				}
			}
				
				//BookMarkURL();
		}


		private function VolMove(e:Event)
		{
			if (vol.vol_mc.y != 20)
			{
				Tweener.addTween(vol.vol_mc, {x: 0, y: 20, time: .3, transition: "linear"});
			}
			else
			{
				Tweener.addTween(vol.vol_mc, {x: 0, y: 150, time: .3, transition: "linear"});
			}
		}


		private function MoveVol(e:Event)
		{
			var myRectangle:Rectangle = new Rectangle(8, -100, 0, 100);
			vol.vol_mc.volKnob.startDrag(false, myRectangle);
			stage.addEventListener(MouseEvent.MOUSE_UP, mouseUp);
			volInt=setInterval(updateVol, 100);
		}


		private function StopMoveVol(e:Event)
		{
			vol.vol_mc.volKnob.stopDrag();
			clearInterval(volInt);
			Tweener.addTween(vol.vol_mc, {x: 0, y: 150, time: .3, transition: "linear"});
		}


		private function saveVolume(vol:Number)
		{
			localData.data.storedVolume=vol;
			localData.flush();
		}


		private function getVolume():void
		{
			if (localData.data.storedVolume != undefined)
			{
				userVolume=localData.data.storedVolume;
			}
			else
			{
				userVolume=.75;
			}
			myVid.volume=userVolume;
			vol.vol_mc.volKnob.y=userVolume * -100;
			vol.vol_mc.volTube.height=Math.abs(vol.vol_mc.volKnob.y);

			if (userVolume <= 0)
			{
				mutebutt_mc.visible=true;
				volbutt_mc.visible=false;
			}
			else
			{
				mutebutt_mc.visible=false;
				volbutt_mc.visible=true;
			}
		}


		private function updateVol()
		{
			vol.vol_mc.volTube.height=Math.abs(vol.vol_mc.volKnob.y);
			var userVolume2:Number = Math.abs((vol.vol_mc.volKnob.y));
			userVolume2=userVolume2 / 100;
			myVid.volume=userVolume2;
			saveVolume(userVolume2);

			if (userVolume2 <= 0)
			{
				mutebutt_mc.visible=true;
				volbutt_mc.visible=false;
			}
			else
			{
				mutebutt_mc.visible=false;
				volbutt_mc.visible=true;
			}

		}


		private function SetScrub(evt:Event)
		{
			scrubInt=setInterval(updateScrub, 10);
		}


		private function SetScrubOff(evt:Event)
		{
			clearInterval(scrubInt);
			tknob_mc.alpha=0;
			bKnob.alpha=0;

			if (mouseY < bKnob.y)
			{
				bKnob.alpha=0;
			}
		}

		private function updateScrub()
		{
			var scWid:Number = Scrub.width;
			var scSepDis:Number = ftime / scWid;
			var scPlace:Number = Scrub.mouseX * Scrub.scaleX;
			var scVal:Number = scPlace * scSepDis;
			scVal=Math.round(scVal);
			scVal=scVal+offsetTime;
			var scTime:String = timeCode(scVal);

			bKnob.alpha=1;
			tknob_mc.alpha=1;
			var scPla:Number = Scrub.x;
			var scTtl:Number = scWid + scPla;

			if (mouseX < scTtl && mouseX > Scrub.x)
			{
				tknob_mc.x=mouseX;
				tknob_mc.newTime.text=scTime;
			}

			if (bKnobStat == 0)
			{
				bKnob.x=Scrub.x + (Scrub.progBar.width * Scrub.scaleX);
			}
		}


		private function KnobClickDwn(e:Event)
		{
			bKnobStat=1;
			var knoby:int = Math.round(botbar_mc.y + 4);
			stage.addEventListener(MouseEvent.MOUSE_UP, mouseUp);
			var myRectangle:Rectangle = new Rectangle(Scrub.x, knoby, Scrub.width, 1);
			bKnob.startDrag(false, myRectangle);
			progInt=setInterval(updateProg, 33);
			overInt=setInterval(updateScrub, 33);
			clearInterval(vidTimeInt);
		}


		private function KnobClickUp(e:Event)
		{
			bKnobStat=0;
			bKnob.stopDrag();
			clearInterval(progInt);
			clearInterval(overInt);
			clearInterval(volInt);
			tknob_mc.alpha=0;
			bKnob.alpha=0;
			BarLenSeek();
		}


		private function bknob_over(e:Event)
		{
			bKnob.alpha=1;
		}


		private function bknob_out(e:Event)
		{
			bKnob.alpha=0;
		}


		private function updateProg()
		{
			var scWidth:Number = Scrub.width;
			var scPla:Number = Scrub.x;
			var scTtl:Number = scWidth + scPla;

			if (mouseX < scTtl)
			{
				Scrub.progBar.width=Scrub.mouseX;
			}
		}


		private function BarLenSeek()
		{
			var ProgWid:Number = Scrub.scrubBar.width;
			var BarRatio:Number = ProgWid / ftime;
			var BarSet:Number = Scrub.progBar.width / BarRatio;
			BarSet=Math.round(BarSet);
			BarSet=BarSet + globalStart;

			try
			{
				myVid.playheadTime=BarSet;
				if(IsRawMedia.toLocaleUpperCase() == "TRUE")
				{
					StartCustomSeek = true;
					myVid.addEventListener(VideoEvent.SEEKED,OnSeeked);
					trace(".........FF/Rew Disabled BarLen Seek........");
					EnableForwardRewind(false);
				}
			}
			catch (e:VideoError)
			{
				tracer("WARN", "ERR4 ERROR: " + e.code + " :: " + e);
			}
		}


		private function SetScrubClick(e:Event)
		{
			var scrubWid:Number = Scrub.width;
			var scrubVal:Number = ftime / scrubWid;
			var scrubPlace:Number = Scrub.mouseX * Scrub.scaleX;
			scrubVal=scrubPlace * scrubVal;
			scrubVal=Math.round(scrubVal);

			var realTime:Number = scrubVal;
			var BarDis:Number = Scrub.scrubBar.width / ftime;
			var BarSet:Number = realTime * BarDis;
			var bdis_S:String = String(BarSet);
			Scrub.progBar.width=BarSet;
			BarLenSeek();
		}


		private function CC_toggle(e:Event)
		{
			if (capback.visible == true)
			{
				capback.visible=false;
				myCap.visible=false;
				dispCap.visible=false;
			}
			else
			{
				capback.visible=true;
				myCap.visible=false;
				dispCap.visible=true;
			}
		}


		//*************** About Screen ******************
		private function clk_about(e:Event)
		{
			about_scrn.visible=false;

			if (menuState == "OPEN")
			{
				fadeOverlay(false);
				about_scrn.rotationY=0;
				menuState="SHARING";
			}
			menuState="CLOSED";
			menuSubState="CLOSED";
			MenuMove(e);
		}


		//*************** Share this video *****************
		private function email_send(e:Event)
		{
			myTimer.reset();
			myTimer.stop();
			//var emailCheck:Number = 0;
			var emailCheck:Boolean;
			var emailValid1:Boolean;

			var strHelper:StringHelper = new StringHelper();

			var email2:Array = emailShare.toemailTxt.text.split(";");
			
			emailCheck = true;
			//trace(emailCheck);
			if (email2.length > 1)
			{
				for (var loop = 0; loop < email2.length; loop++)
				{
					email2[loop]=strHelper.trim(email2[loop], " ");
					if (!isValidEmail(email2[loop]))
					{
						emailCheck = false;
						break;
					}
				}
				
				if (emailCheck == true)
				{
					emailValid1=true;
				}
				else
				{
					emailValid1=false;
				}
			}
			else
			{
				emailValid1=isValidEmail(emailShare.toemailTxt.text);
				//trace(emailValid1);
			}
			
			var emailValid2:Boolean = isValidEmail(emailShare.fromemailTxt.text);
			//trace(emailValid1);
			//tracer ("INFO",emailValid1+"   "+emailValid2);
			if (emailValid1 == true && emailValid2 == true)
			{
				
				try
				{
					CallEmailService();
					emailShare.statusTxt.text="Sending...";
				}
				catch (err:Error)
				{
					trace("Error sending email: " + err.message);
				}
				emailShare.statusTxt.text="Sending...";
			}
			else
			{
				// indicate error
				emailShare.statusTxt.text="Invalid email format. Check fields for errors.";
			}
		}
		
		function CallEmailService()
		{
			
			var FileName:String = clipName;
			
			if(emailShare.msgTxt.text == null || emailShare.msgTxt.text == "")
			{
				emailShare.msgTxt.text = "  ";
			}
			
			var messages:Array = new Array ();
			
			var imagePath:String;
			
			
			imagePath =_imagePath;
			var _Subject:String;
			if(emailShare.subemailTxt.text != null && emailShare.subemailTxt.text!= "")
			{
				_Subject = emailShare.subemailTxt.text;
			}
			else
			{
				_Subject = "IQMedia Clip Sharing";
			}
			
			var RequestURL:String;
			RequestURL= srvString + '/iqsvc/SendEmail';
			
			var JSONLoader:URLLoader = new URLLoader();
			JSONLoader.dataFormat=URLLoaderDataFormat.TEXT;

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, EmailServiceURLError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseEmailServiceResult, false, 0, true);
			
			
			var hdr:URLRequestHeader = new URLRequestHeader("Content-type", "application/json");
			
			var request:URLRequest = new URLRequest(RequestURL);
			request.requestHeaders.push(hdr);
			request.data="{\"From\":\""+emailShare.fromemailTxt.text+"\",\"To\":\""+emailShare.toemailTxt.text+"\",\"Subject\":\""+_Subject+"\",\"Body\":\""+emailShare.msgTxt.text+"\",\"_imagePath\":\""+imagePath+"\",\"FileName\":\""+FileName+"\",\"FileID\":\""+_fileId+"\",\"PageName\":\""+PageName+"\"}";
			request.method = URLRequestMethod.POST;
			
			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
		}
		
		
		
		function parseEmailServiceResult(e:Event):void
		{
			try
			{
				if(JSON.decode(e.target.data) == "0")
				{
					emailShare.statusTxt.text="Email Sent";
					myTimer.start();
				}
				else
				{
					myTimer.reset();
					myTimer.stop();
					emailShare.statusTxt.text="An error occurred";
				}
			}
			catch (err:TypeError)
			{
				myTimer.reset();
				myTimer.stop();
				emailShare.statusTxt.text="An error occurred";
			}
		}
		
		private function EmailServiceURLError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred in EmailService"+evt.text);
		}
		
		private function onEmailComplete(evt:Event):void
		{
			try
			{
				emailShare.statusTxt.text="Email Sent";
				myTimer.start();
			}
			catch (err:TypeError)
			{
				myTimer.reset();
				myTimer.stop();
				emailShare.statusTxt.text="An error occured";
			}
		}


		private function emailCloseCore():void
		{
			myTimer.stop();
			activeBTN(true);
			menuState="OPEN";
			menuSubState="CLOSED";
			//Tweener.addTween(emailShare, {x: emailStopx, y: emailStopy, time: .3, transition: "linear"});
			Tweener.addTween(emailShare, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			emailShare.visible = false;
			showMenu();
			FlagPopup = false;

			if (hasVideo == "FALSE")
			{
				if (prevLoader)
				{
					prevLoader.alpha=1;
				}
			}
		}


		private function email_close(e:Event)
		{
			emailCloseCore();
		}


		private function emailCloseF(e:TimerEvent)
		{
			emailCloseCore();
		}


		private function isValidEmail(email:String):Boolean
		{
			var emailExpression:RegExp = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
			return emailExpression.test(email);
		}


		//**************Embed Code****************

		private function assignEmbed()
		{
			if (!dataLoaded && embedUrl == null || embedUrl == undefined || embedUrl == "")
			{
				embedUrl=LoaderInfo(this.root.loaderInfo).url;

			}

			if (_fileId == "" || _fileId == undefined || _fileId == null)
			{
				_fileId=initClip;
			}
			
			var lcdomain:LocalConnection = new LocalConnection();
			var vardomain:String = lcdomain.domain;
			
			/*var swfnamearray:Array = stage.loaderInfo.url.split("/");
			
			if(swfnamearray[swfnamearray.length - 1].toString().lastIndexOf("_v") != -1)
			{		
				var swfname:String = swfnamearray[swfnamearray.length - 1];
				versionBuild = swfname.substring(swfname.lastIndexOf("_v") + 2,swfname.indexOf(".swf"));
				menuItemLabel2 = "Version: " + versionBuild + "";			
			}*/
			
			var swfUrl:String=stage.loaderInfo.url;
			
			/*if(ServicesBaseURL=="1")
			{
				swfUrl = "http://" + vardomain + "/QA_iqmedia_player.swf"
			}
			else
			{
				swfUrl = "http://" + vardomain + "/IQMedia_Player.swf"
			}*/
			
			var PlayerEmbedObject:String = (PlayerLogo !=  "" && PlayerLogo != undefined) ? "&amp;PlayerLogo="+ PlayerLogo : "";
			var PlayerEmbedGigya:String =  (PlayerLogo !=  "" && PlayerLogo != undefined) ? "&PlayerLogo="+ PlayerLogo+"" : "";
			
			
			embedVideo.embedTxt.text="<object height=\"340\" width=\"545\" " +
									"data=\"" + swfUrl + "\" " + 
									"type=\"application/x-shockwave-flash\" "  +
									"name=\"HYETA\" id=\"HUY\"> " +
									"<param value=\""+ swfUrl + "\" name=\"movie\">" +
									"<param value=\"true\" name=\"allowfullscreen\">" +
									"<param value=\"always\" name=\"allowscriptaccess\">" +
									"<param value=\"high\" name=\"quality\">" +
									"<param value=\"transparent\" name=\"wmode\">" +
									"<param value=\"userId=" + userIddynamic +"&amp;" + 
										"IsRawMedia="+ IsRawMedia +"&amp;"+ 
										"embedId=" + initClip + 
										"&amp;PageName=" + PageName + 
										"&amp;EB=false" +  
										"&amp;ServicesBaseURL=" + ServicesBaseURL + 
										"&amp;PlayerFromLocal=" + PlayerFromLocal  +  PlayerEmbedObject +
										"&amp;"+"autoPlayback=false\"" +  
										" name=\"flashvars\">" +
									"</object>";

			
			embedVideo.WPembedTxt.text ="[gigya height=\"340\" width=\"500\" src=\"" + swfUrl + "\" " +
										"wmode=\"transparent\" " +
										"allowscriptaccess=\"always\"  " +
										"allowfullscreen=\"true\" " +
										"flashvars=\"userId=" + userIddynamic +"&" + 
										"IsRawMedia="+ IsRawMedia +"&"+ 
										"embedId=" + initClip + 
										"&PageName=" + PageName + 
										"&EB=false" +  
										"&ServicesBaseURL=" + ServicesBaseURL + 
										"&PlayerFromLocal=" + PlayerFromLocal  + PlayerEmbedGigya + 
										"&"+"autoPlayback=false\" ]";
			
			
			
		}


		private function link_close(e:Event)
		{
			activeBTN(true);
			menuState="OPEN";
			menuSubState="CLOSED";
			//Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			Tweener.addTween(embedVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			embedVideo.visible = false;
			FlagPopup = false;
			showMenu();
		}


		private function copy(e:Event)
		{
			menuState="OPEN";
			activeBTN(true);
			System.setClipboard(embedVideo.embedTxt.text);
			//Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			Tweener.addTween(embedVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			showMenu();
			popupDisplay("Copied to Clipboard", 500, 2500);
		}


		private function WPcopy(e:Event)
		{
			menuState="OPEN";
			activeBTN(true);
			System.setClipboard(embedVideo.WPembedTxt.text);
			//Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			Tweener.addTween(embedVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			showMenu();
			popupDisplay("Copied to Clipboard", 500, 2500);
		}


		private function hidePopup(arg:Number):void
		{
			setTimeout(function()
				{
					popup.visible=false;
				}, arg);
		}


		private function popupDisplay(arg, delay, duration):void
		{
			if (widgetMode != "TRUE")
			{
				sw=stage.stageWidth;
				sh=stage.stageHeight;
			}
			popup.x=sw / 2 - popup.width / 2;
			popup.y=(sh - 28) / 2 - popup.height / 2;
			popup.msgTxt.text=arg;
			setTimeout(function()
				{
					popup.visible=true;
				}, delay);
			hidePopup(duration);
		}


		private function Lcopy(e:Event)
		{
			menuState="OPEN";
			activeBTN(true);
			System.setClipboard(linkVideo.urlTxt.text);
			//Tweener.addTween(linkVideo, {x: linkStopx, y: linkStopy, time: .3, transition: "linear"});
			Tweener.addTween(linkVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			fadeOverlay(false);
			showMenu();
			popupDisplay("Copied to Clipboard", 500, 2500);
		}


		private function setupMenuListeners():void
		{
			embedVideo.WPcancel_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			embedVideo.WPcancel_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			embedVideo.WPcancel_mc.addEventListener(MouseEvent.CLICK, link_close);
			embedVideo.cancel_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			embedVideo.cancel_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			embedVideo.cancel_mc.addEventListener(MouseEvent.CLICK, link_close);
			embedVideo.WPcopy_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			embedVideo.WPcopy_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			embedVideo.WPcopy_mc.addEventListener(MouseEvent.CLICK, WPcopy);
			embedVideo.copy_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			embedVideo.copy_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			embedVideo.copy_mc.addEventListener(MouseEvent.CLICK, copy);
			embedVideo.cancel_mc.buttonMode=true;
			embedVideo.WPcancel_mc.buttonMode=true;
			embedVideo.WPcopy_mc.buttonMode=true;
			embedVideo.copy_mc.buttonMode=true;
			linkVideo.lcopy_mc.addEventListener(MouseEvent.CLICK, Lcopy);
			linkVideo.lcopy_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			linkVideo.lcopy_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			linkVideo.cancel_mc.addEventListener(MouseEvent.CLICK, linkVideoCancel);
			linkVideo.cancel_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			linkVideo.cancel_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			linkVideo.cancel_mc.buttonMode=true;
			linkVideo.lcopy_mc.buttonMode=true;
		}
		
		private function setupClippingListeners():void
		{
			expand_mc.addEventListener(MouseEvent.CLICK, ClipMCClick);
			//clipperUI.leftKnob.addEventListener(MouseEvent., ClipMCLeftKnobClick);			
			clipperUI.leftKnob.addEventListener(MouseEvent.MOUSE_DOWN,ClipMCLeftKnobClick);					
			
			clipperUI.rightKnob.addEventListener(MouseEvent.MOUSE_DOWN,ClipMCRightKnobClick);
			clipperUI.save_mc.addEventListener(MouseEvent.CLICK, ClipMCSave);
			
			clipSave.inStep.stepUp.addEventListener(MouseEvent.MOUSE_DOWN, ClipSaveInStepUpClick);
			clipSave.inStep.stepUp.addEventListener(MouseEvent.MOUSE_UP, ClipSaveInStepUpUp);
			
			clipSave.inStep.stepDown.addEventListener(MouseEvent.MOUSE_DOWN, ClipSaveInStepDownClick);
			clipSave.inStep.stepDown.addEventListener(MouseEvent.MOUSE_UP, ClipSaveInStepDownUp);
			
			clipSave.outStep.stepUp.addEventListener(MouseEvent.MOUSE_DOWN, ClipSaveOutStepUpClick);
			clipSave.outStep.stepUp.addEventListener(MouseEvent.MOUSE_UP, ClipSaveOutStepUpUp);
			
			clipSave.outStep.stepDown.addEventListener(MouseEvent.MOUSE_DOWN, ClipSaveOutStepDownClick);
			clipSave.outStep.stepDown.addEventListener(MouseEvent.MOUSE_UP, ClipSaveOutStepDownUp);
			
			clipperUI.cancel_mc.addEventListener(MouseEvent.CLICK, ClipSaveCancelClick);
			
			menubg.clip_mc.addEventListener(MouseEvent.CLICK, ClipMCClick);
			menubg.closeButt.addEventListener(MouseEvent.CLICK, MenuMove);
		}		
		private function ROLLOVERshortcuts(e:Event) 
		{
			clipperUI.shortcuts.useHandCursor = true;
			Tweener.addTween(clipperUI.shortcuts, {x: clipperUI.shortcuts.x, y: clipperUI.botbar_mc.y - 87, time: .45});
		};	
	
		private function ROLLOUTshortcuts(e:Event) 
		{
			clipperUI.shortcuts.useHandCursor = true;
			//clipperUI.shortcuts.slideTo (null,this._parent.botbar_mc._y - 13,.35);
			Tweener.addTween(clipperUI.shortcuts, {x:clipperUI.shortcuts.x, y: clipperUI.botbar_mc.y - 13, time: .35});
		};
	
		private function ClipMCLeftKnobLocationChange(e:Event)
		{
			if(clipperUI.leftKnob.x<clipperUI.scrubBar.x)
			{
				clipperUI.leftKnob.x=clipperUI.scrubBar.x;
			}
		}
		
		private function ClipMCRightKnobLocationChange(e:Event)
		{
			var position:Number;
			
			position=clipperUI.scrubBar.x+clipperUI.scrubBar.width;
			
			if(clipperUI.rightKnob.x>position)
			{
				clipperUI.rightKnob.x=position;
			}
		}
		
		private function ClipSaveCancelClick(e:Event)
		{
			categories_mc.alpha = 0;
			categories_mc.x = 0 - categories_mc.width - 10;
			clipperUI.keyStatus = false;
			Tweener.addTween(clipperUI, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			
			stage.removeEventListener(KeyboardEvent.KEY_DOWN,ClipperKeyDown);
			stage.removeEventListener(KeyboardEvent.KEY_UP,ClipperKeyUP);
			
			mc_pp.alpha=100;
			clipperUI.pp_mc.alpha=0;
			IsClipperEnable=false;
			isCustomSeek=false;
			/*uimask_mc.height = stage.stageHeight - 28;*/
			/*clipperUI.slideTo (0,stage.stageHeight + 50,.35);
			coverup_mc.x = 0;
			coverup_mc.y = stage.stageHeight + 10;
			coverup_mc.alpha = 100;*/
			Tweener.addTween(clipSave, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			clipperUI.visible = false;
			clipSave.visible = false;
			FlagPopup = false;
			/*showHideMenu (!botmenu);
			clipUI = false;*/
		}
		
		private function ClipSaveOutStepDownUp(e:Event)
		{
			clearInterval(clipSave.outStepDownINT);
			clipSave.outStepDownINT=null;
		}
		
		private function ClipSaveOutStepDownClick(e:Event)
		{
			if (clipSave.outStepDownINT==null)
			{
				clipSave.outStepDownINT = setInterval (OutStepDown, 66);
			}
		}
		
		private function OutStepDown()
		{
				var seekPointRight:Number;
				
				clipperUI.rightKnob.x--;
				
				if (clipperUI.rightKnob.x <= clipperUI.leftKnob.x || clipperUI.rightKnob.x < clipperUI.scrubBar.x)
				{
					clipperUI.rightKnob.x++;
				}
				
				updateSaveTime ();						
		}
		
		private function ClipSaveOutStepUpClick(e:Event)
		{
			if (clipSave.outStepUpINT==null)
			{
				clipSave.outStepUpINT = setInterval (OutStepUp, 66);
			}
		}
		
		private function OutStepUp()
		{
				var seekPointRight:Number;
				var tempRightKnobX:Number;
				
				tempRightKnobX=clipperUI.rightKnob.x;
				
				clipperUI.rightKnob.x++;
				if (clipperUI.rightKnob.x <= clipperUI.leftKnob.x || clipperUI.rightKnob.x > clipperUI.scrubBar.x + clipperUI.scrubBar.width)
				{
					clipperUI.rightKnob.x--;
				}
				
				if(tempRightKnobX==clipperUI.rightKnob.x)
				{
					if(clipperUI.rightKnob.x<(clipperUI.scrubBar.x + clipperUI.scrubBar.width))
					{
						clipperUI.rightKnob.x=(clipperUI.scrubBar.x + clipperUI.scrubBar.width);
					}
				}
				
				updateSaveTime ();				
		}
		
		private function ClipSaveOutStepUpUp(e:Event)
		{
			clearInterval(clipSave.outStepUpINT);
			clipSave.outStepUpINT=null;
		}
		
		private function ClipSaveInStepUpClick(e:Event)
		{			
			if (clipSave.inStepUpINT==null)
			{
				clipSave.inStepUpINT = setInterval (InStepUp, 66);
			}
		}
		
		private function InStepUp()
		{
				var seekPointLeft:Number;			
				
				clipperUI.leftKnob.x++;
				if (clipperUI.leftKnob.x >= clipperUI.rightKnob.x || clipperUI.leftKnob.x > clipperUI.scrubBar.x + clipperUI.scrubBar.width)
				{
					clipperUI.leftKnob.x--;
				}
				
				updateSaveTime ();						
		}
		
		private function ClipSaveInStepUpUp(e:Event)
		{			
			clearInterval (clipSave.inStepUpINT);			
			clipSave.inStepUpINT=null;
		}
		
		private function ClipSaveInStepDownClick(e:Event)
		{
			if (clipSave.inStepDownINT==null)
			{
				clipSave.inStepDownINT = setInterval (InStepDown, 66);
			}
		}
		
		private function InStepDown()
		{			
				var seekPointLeft:Number;
				
				clipperUI.leftKnob.x--;
				if (clipperUI.leftKnob.x >= clipperUI.rightKnob.x || clipperUI.leftKnob.x < clipperUI.scrubBar.x)
				{
					clipperUI.leftKnob.x++;
				}
				
				updateSaveTime ();			
		}
		
		private function ClipSaveInStepDownUp(e:Event)
		{
			clearInterval (clipSave.inStepDownINT);			
			clipSave.inStepDownINT=null;
		}
		
		private function ClipMCSave(e:Event)
		{
			clipperUISave ();
		}
		
		private function clipperUISave () 
		{
			/*clipSave.y=0;
			clipSave.x=0;*/
			
			clipSave.visible = true;
			clipSave.statusTxt.text = "";
			var seekPointLeft:Number;
			var seekPointRight:Number;
			var seekOffSet:Number;
			clipperUI.keyStatus = false;
			//scaleUpitem(clipSave);
			//Tweener.addTween(clipSave, {x: 0, y: 0, time: .35, transition: "linear"});
			
			
			if (clipperUI.save_mc.txtLabel.text == "FINISH")
			{
				try 
				{ 		
					clipSave.clipTitleTxt.text = ""; 
					clipSave.keywordsTxt.text = PlayerDataKeyWord; 
					clipSave.descriptionTxt.text = PlayerDataDescription;
					clipSave.statusTxt.text = "";
					clipSave.statusTxt.text = "";
					
					clipSave.scrollKeywords.update();
					clipSave.scrollDescription.update();
					
					emailShare.fromemailTxt.tabIndex = undefined;
					emailShare.toemailTxt.tabIndex = undefined;
					emailShare.subemailTxt.tabIndex = undefined;
					emailShare.msgTxt.tabIndex = undefined;
					emailShare.cancel_mc.tabIndex = undefined;
					emailShare.send_mc.tabIndex = undefined;
					
					mc_pp.tabIndex = undefined;
					loader_mc.tabIndex = undefined;
					timeDisplayCur.tabIndex = undefined;
					timeDisplayTot.tabIndex = undefined;
					menu_mc.tabIndex = undefined;
					volbutt_mc.tabIndex = undefined;
					fullScreen.tabIndex = undefined;
					menubg.clip_mc.tabIndex = undefined;
					menubg.embed_mc.tabIndex = undefined;
					menubg.email_mc.tabIndex = undefined;
					menubg.link_mc.tabIndex = undefined;
					menubg.about_mc.tabIndex = undefined;
					menubg.closeButt.tabIndex = undefined;
					meta_mc.toggle_mc.tabIndex = undefined;
					meta_mc.station_logo.tabIndex = undefined;
					meta_mc.metabg_mc.tabIndex = undefined;
					meta_mc.meta3Txt.tabIndex = undefined;
					meta_mc.meta2Txt.tabIndex = undefined;
					meta_mc.meta1Txt.tabIndex = undefined;
					CC_btn.tabIndex = undefined;
					tknob_mc.tabIndex= undefined;
					bKnob.tabIndex = undefined;
					
					clipperUI.cKnob.tabIndex = undefined;
					clipperUI.rightKnob.tabIndex = undefined;
					clipperUI.leftKnob.tabIndex = undefined;
					clipperUI.timeDisplayCur.tabIndex = undefined;
					clipperUI.timeDisplayScrub.tabIndex = undefined;
					clipperUI.timeDisplayTot.tabIndex = undefined;
					clipperUI.hitMarker_mc.tabIndex = undefined;
					clipperUI.progBar.tabIndex = undefined;
					
					clipperUI.pp_mc.tabIndex = undefined;
					clipperUI.loader_mc.tabIndex = undefined;
					clipperUI.botbar_mc.tabIndex = undefined;
					clipperUI.clearButt.tabIndex = undefined;
					clipperUI.shortcuts.tabIndex = undefined;
					
					clipSave.clipTitleTxt.tabIndex = 1;
					clipSave.keywordsTxt.tabIndex = 2;
					clipSave.cmbCategory.tabIndex = 3;
					
					var myFormatBlack:TextFormat = new TextFormat();
					myFormatBlack.font = "Verdana";
					myFormatBlack.size = 10;
					myFormatBlack.color = 0x000000;
					
					
					
					
					var myFormatBlackSelect:TextFormat = new TextFormat();
					myFormatBlackSelect.font = "Verdana";
					myFormatBlackSelect.size = 8;
					myFormatBlackSelect.color = 0x000000;
					
					
					//clipSave.cmbCategory.textField.setStyle("embedFonts", true);
					clipSave.cmbCategory.textField.setStyle("textFormat", myFormatBlack);
					//clipSave.cmbCategory.dropdown.setRendererStyle("embedFonts", true);
					clipSave.cmbCategory.dropdown.setRendererStyle("textFormat", myFormatBlack);
					clipSave.cmbCategory.setStyle("embedFonts", true);
					clipSave.cmbCategory.setStyle("textFormat", myFormatBlackSelect);
					
					
					clipSave.timeStartEndTxt.tabIndex = 4;
					clipSave.descriptionTxt.tabIndex = 5;
					clipperUI.cancel_mc.tabEnabled = true;
					clipperUI.cancel_mc.tabIndex = 6;
					clipperUI.cancel_mc.buttonMode = true;
					clipperUI.save_mc.tabEnabled = true;
					clipperUI.save_mc.tabIndex = 7;
					clipperUI.save_mc.buttonMode = true;
										
					emailShare.fromemailTxt.focusRect = undefined;
					emailShare.toemailTxt.focusRect = undefined;
					emailShare.subemailTxt.focusRect = undefined;
					emailShare.msgTxt.focusRect = undefined;
					emailShare.cancel_mc.focusRect = undefined;
					emailShare.send_mc.focusRect = undefined;
					
					mc_pp.focusRect = false;
					timeDisplayCur.focusRect = false;
					timeDisplayTot.focusRect = false;
					menu_mc.focusRect = false;
					volbutt_mc.focusRect = false;
					fullScreen.focusRect = false;
					menubg.clip_mc.focusRect = false;
					menubg.embed_mc.focusRect = false;
					menubg.email_mc.focusRect = false;
					menubg.link_mc.focusRect = false;
					menubg.about_mc.focusRect = false;
					menubg.closeButt.focusRect = false;
					meta_mc.toggle_mc.focusRect = false;
					meta_mc.station_logo.focusRect = false;
					meta_mc.metabg_mc.focusRect = false;
					meta_mc.meta3Txt.focusRect = false;
					meta_mc.meta2Txt.focusRect = false;
					meta_mc.meta1Txt.focusRect = false;
					CC_btn.focusRect = false;
					
					clipperUI.cKnob.focusRect = false;
					clipperUI.rightKnob.focusRect = false;
					clipperUI.leftKnob.focusRect = false;
					clipperUI.timeDisplayCur.focusRect = false;
					clipperUI.timeDisplayScrub.focusRect = false;
					clipperUI.timeDisplayTot.focusRect = false;
					clipperUI.hitMarker_mc.focusRect = false;
					clipperUI.progBar.focusRect = false;
					clipperUI.pp_mc.focusRect = false;
					clipperUI.botbar_mc.focusRect = false;
					clipperUI.clearButt.focusRect = false;
					clipperUI.shortcuts.focusRect = false;
		
					clipSave.clipTitleTxt.focusRect = true;
					clipSave.keywordsTxt.focusRect = true;
					clipSave.cmbCategory.focusRect = true;
					clipSave.timeStartEndTxt.focusRect = true;
					clipSave.descriptionTxt.focusRect = false;
					clipperUI.cancel_mc.focusRect = true;
					clipperUI.save_mc.focusRect = true;	
	
					clipperUI.save_mc.txtLabel.text = "SAVE";
					scaleUpitem(clipSave);
					SizeItems();
					clipSave.visible = true;
					
					Tweener.addTween(clipSave, {x: 0, y: 0, time: .35, transition: "linear"});
					
					//clipperUI.save_mc.txtLabel2.text = "SAVE";
					//uimask_mc._height = Stage.height;
					clipperUI.coverup_mc.enabled = false;
	
					clipperUI.coverup_mc.useHandCursor = false;
					//coverup_mc._alpha = 100;
					//coverup_mc._x = 0;
					//coverup_mc._y = 0;
					//scaleUpitem (clipSave);
					
					//clipSave.slideTo (0,0,.35,null,0,setFocusClipsave);
					
					//clipSave.catHold.categoryTxt.text = "";
					
					clipSave.timeStartEndTxt.text = clipperUI.timeDisplayCur.text + " / " + clipperUI.timeDisplayTot.text;
					
				} 
				catch (error:Error) 
				{ 
					trace("An ArgumentError has occurred."); 
				} 
				
			}
			else
			{
				if (clipSave.clipTitleTxt.text != "" && clipSave.keywordsTxt.text != "" && clipSave.descriptionTxt.text != "" && clipSave.cmbCategory.text != "")
				{
					//categories_mc.slideTo (null,Stage.height + 36);
					
					//app_source=clpXML.vars.@streamUrl;
					
					/*var theContent:String = clipSave.clipTitleTxt.text;
					theContent = theContent.split("\/:*?<>|").join("-");*/
					
					var theContent:String = clipSave.clipTitleTxt.text;
					var _ClipTitle:String="";
					
					for(var i:int = 0; i<theContent.length ; i++)
					{
						_ClipTitle=_ClipTitle+ReplaceString(theContent.charAt(i));
					}
					/*theContent = theContent.split("\\").join("_"); 
					theContent = theContent.replace("/","_");
					theContent = theContent.replace(":","_");
					theContent = theContent.replace("*","_");
					theContent = theContent.replace("?","_");
					theContent = theContent.replace("<","_");
					theContent = theContent.replace(">","_");
					theContent = theContent.replace("|","_");*/
			
					//trace("SPLIT" + _ClipTitle);
					
					seekPointLeft = (clipperUI.leftKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
					seekPointRight = (clipperUI.rightKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
					// seekOffSet = vidComm["startTime"];
					
					//seekOffSet = sTime;
					seekOffSet = globalStart;
					clipperUI.save_mc.txtLabel.text = "SAVE";
					//clipperUI.save_mc.txtLabel2.text = "SAVE";
					clipSave.statusTxt.text = "Saving...";
					//clipSave.loader_mc.alphaTo (100,.25);
					clipSave.loader_mc.alpha = 0;
					clipSave.loader_mc.onEnterFrame = function () {
						this._rotation += 15;
					};
					
					// call sendCLIP with data and wait
										
					//CreateClip(clipSave.clipTitleTxt.text,clipSave.cmbCategory.value,clipSave.keywordsTxt.text,clipSave.descriptionTxt.text,Math.floor (seekPointLeft) + seekOffSet,Math.floor (seekPointRight) + seekOffSet);
					//CreateClip(clipSave.clipTitleTxt.text,categoryCode,clipSave.keywordsTxt.text,clipSave.descriptionTxt.text,Math.floor (seekPointLeft) + seekOffSet,Math.floor (seekPointRight) + seekOffSet);
					CreateClip(_ClipTitle,categoryCode,clipSave.keywordsTxt.text,clipSave.descriptionTxt.text,Math.floor (seekPointLeft) + seekOffSet,Math.floor (seekPointRight) + seekOffSet);
					//CreateClip(clipSave.clipTitleTxt.text,categoryCode,clipSave.keywordsTxt.text,clipSave.descriptionTxt.text,(seekPointLeft + seekOffSet),(seekPointRight + seekOffSet));
				}
				else
				{
					clipSave.loader_mc.alpha=0;
					clipSave.loader_mc.onEnterFrame = null;
					clipSave.statusTxt.text = "Error - Empty Required Fields";
					//trace("clipTitleTxt" + clipSave.clipTitleTxt.text);
				}
			}
			FlagPopup = true;
		}
		
		private function ReplaceString(Char:String)
		{
			var myArray:Array = new Array("\\", "/", ":", "*","?","<",">","|","\"");
			if(myArray.indexOf(Char)<0)
			{
				return Char;
			}
			else
			{
				return "-";
			}
		}
		
		// added by meghana for Change to fill Category from ISVC and removed Webservice call.
		function FillCategories()
		{
			/* new code */		
			
			var RequestURL:String;
			RequestURL= srvString + '/iqsvc/GetCategoryList?ClientGUID='+clientGUID;
			
			var JSONLoader:URLLoader = new URLLoader();

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, FillCategoriesError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseFillCategoriesResult, false, 0, true);
			
			var request:URLRequest = new URLRequest(RequestURL);

			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
			
		}
		
		function parseFillCategoriesResult(e:Event):void
		{
			var CatList:Object=new Object();
				
			CatList=JSON.decode(e.target.data,false);	
			
			for (var i:Number=0;i<CatList.length;i++)
			{
				clipSave.cmbCategory.addItem( { label: CatList[i].CategoryName, data:CatList[i].CategoryGUID } );		
			}
			
		}
		
		private function FillCategoriesError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred in FillCategories"+evt.text);
		}
		
		
		
		function scaleUpitem (target:MovieClip) 
		{
			var imageAspectRatio = target.width / target.height;
			var stageAspectRatio = stage.stageWidth / (stage.stageHeight - 28);
			var stageXCenter = stage.stageWidth / 2;
			var stageYCenter = (stage.stageHeight / 2) - 28;
			if (target.width > target.height)
			{
				// long skinny stage - match image width and adjust height to fit 
				target.width = stage.stageWidth;
				target.height = stage.stageWidth / imageAspectRatio;
			}
			else
			{
				target.width = (stage.stageHeight - 28) * imageAspectRatio;
				target.height = stage.stageHeight - 28;
			}
			
			trace("width:"+target.width);
			trace("height:"+target.height);
		}
		
		private function CreateClip(paramtitle:String, paramcategory:String, paramkeywords:String, paramdescription:String, paramstartTime:Number, paramendTime:Number)
		{
			
			var _MetaData:String;		
			
			
			if(keyvalue=="undefined" || keyvalue==null || keyvalue=="" || keyvalue.length<=0)
			{
				
			}
			else
			{
				var jsonobj:Object=new Object();
				
				jsonobj=JSON.decode(keyvalue,false);		
				
				//var typeDef:XML = describeType(jsonobj);
						
				var MetaData:String="";		
				
				for (var key:String in jsonobj)
				{
					if(_MetaData==null || _MetaData=="")
					{
						_MetaData="<meta key='"+key+"' value='"+jsonobj[key]+"' />";
					}
					else
					{
						_MetaData=_MetaData+"<meta key='"+key+"' value='"+jsonobj[key]+"' />";
					}
				}
			}
			
			/* json */
			
			var RequestURL:String;
			
			var CateGoryGUID:String=clipSave.cmbCategory.value;
			
			RequestURL= srvString + '/svc/clip/createClip' + '?sid=' + userId + '&fid=' + _fileId + "&pid=" + _fileId;
			
			var request:URLRequest = new URLRequest(RequestURL); 

			var xmlString:String;
			xmlString='<root rid='+'"'+_fileId+'"'+' sid='+'"'+RL_User_GUID+'"'+'><clipinfo startTime='+'"'+paramstartTime+'"'+' endTime='+'"'+paramendTime+'"'+' keywords='+'"'+ htmlEncode(paramkeywords)+'"'+' category='+'"'+paramcategory+'"'+' title='+'"'+htmlEncode(paramtitle)+'"'+' userId='+'"'+RL_User_GUID+'"'+' fileId='+'"'+_fileId+'"'+'>'+htmlEncode(paramdescription)+'</clipinfo><clipmeta><meta key="iqClientid" value='+'"'+clientGUID+'"/><meta key="iQUser" value='+'"'+customerGUID+'"/><meta key="iQCategory" value='+'"'+CateGoryGUID+'"/>';
			
			if(_MetaData!=null && _MetaData!="")
			{
				xmlString=xmlString+_MetaData+"</clipmeta></root>";
			}
			else
			{
				xmlString=xmlString+"</clipmeta></root>";
			}
			
			var dataXML:XML = new XML(xmlString);
						
			request.data = dataXML.toXMLString(); 
			request.method = URLRequestMethod.POST; 
			var loader:URLLoader = new URLLoader();
			loader.addEventListener(Event.COMPLETE,parseResultXML);
			try 
			{ 
				loader.load(request); 
						/*sendToURL(request);*/
			} 
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}		
					
		}
		
		
		function parseResultXML (e:Event):void
		{			
			var rootNode:XMLNode;
			var rid:String;
			var service:String;
			var error:String;
			var msg:String;
			var status:Boolean;
			var resultxml:XML = new XML(e.target.data);
			if(resultxml.@status==1)
			{
				clipSave.statusTxt.text="Clip Created and Saved!";
				clipSave.loader_mc.onEnterFrame = null;
				//clipSave.loader_mc.alpha (0,.75,null,0,closeClipSave);
				clipSave.loader_mc.alpha = 0;
				
				
				if(IsAutoDownload.toLocaleUpperCase() == "TRUE")
				{
					DownloadClip(resultxml.text());
				}
				
				if(IsUGC.toLocaleUpperCase()=="TRUE")
				{
					DownloadClipCall(resultxml.text());
				}
				
				ThumbGenCall(resultxml.text());
				ExportClipCall(resultxml.text());
				IOSExportClipCall(resultxml.text());
				
				
				
				
			
				closeClipSave();
				IsClipperEnable=false;
				isCustomSeek=false;			
				
			}
			else
			{
				clipSave.statusTxt.text = "Error in Saving Clip.";
			}
			
		}
		
		function ThumbGenCall(clipID:String):void
		{
			/* new code */		
			
			var RequestURL:String;
			
			RequestURL= srvString + '/svc//clip/generateThumbnail?fid='+ clipID+"";
			
			var JSONLoader:URLLoader = new URLLoader();

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, ThumbGenError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseThumbGenResult, false, 0, true);
			
			var request:URLRequest = new URLRequest(RequestURL);

			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
			
		}
		
		private function ThumbGenError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred at ThumbGen"+evt.text);
		}
		
		function parseThumbGenResult(e:Event):void
		{
			tracer("INFO","Thumb GenResult Message :"+e.target.data);
		}
		
		
		// added by meghana to call IOS Clip Export Service (htpp Handler)
		function IOSExportClipCall(clipID:String):void
		{
			/* new code */		
			
			var RequestURL:String;
			
			RequestURL= srvString + '/iossvc/ClipExport?clipGUID='+ clipID+"";
			
			var JSONLoader:URLLoader = new URLLoader();

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, IOSClipExportError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseIOSClipExportResult, false, 0, true);
			
			var request:URLRequest = new URLRequest(RequestURL);

			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
			
		}
		
		private function IOSClipExportError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred at Clip Export"+evt.text);
		}
		
		function parseIOSClipExportResult(e:Event):void
		{
			tracer("INFO","Message :"+e.target.data);
		}
		
		
		// added by meghana to call Clip Export Service 
		function ExportClipCall(clipID:String):void
		{
			/* new code */		
			
			var RequestURL:String;
			
			RequestURL= srvString + '/svc/clip/export?fid='+ clipID+"";
			
			var JSONLoader:URLLoader = new URLLoader();

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, ClipExportError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseClipExportResult, false, 0, true);
			
			var request:URLRequest = new URLRequest(RequestURL);

			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
			
		}
		
		private function ClipExportError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred at Clip Export"+evt.text);
		}
		
		function parseClipExportResult(e:Event):void
		{
			tracer("INFO","Message :"+e.target.data);
		}
		
		// added by meghana to call UGC Clip Export Service if Creted Clip is UGC Clip
		function DownloadClipCall(clipID:String):void
		{
			/* new code */		
			
			var RequestURL:String;
			
			//RequestURL= srvString + '/iqsvc/UGCRawClipExport?ClipID='+ clipID;
			RequestURL= srvString + '/iqsvc/ExportClipForMicrosite?ClipGUID='+ clipID+"&ClientGUID="+clientGUID+"&IsUGC="+IsUGC.toLocaleLowerCase();
			

			var JSONLoader:URLLoader = new URLLoader();

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, UGCClipExportError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseUGCClipExportResult, false, 0, true);
			
			var request:URLRequest = new URLRequest(RequestURL);

			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
			
		}
		
		
		function DownloadClip(clipID:String):void
		{
			/* new code */		
			
			
			var cateGoryGUID:String=clipSave.cmbCategory.value;
			
			var RequestURL:String;
			
			RequestURL= srvString + '/iqsvc/Clipautodownload?ClipID='+ clipID + '&ClientGUID=' + clientGUID + "&CategoryGUID=" + cateGoryGUID+"&CustomerGUID="+customerGUID;
			
			var JSONLoader:URLLoader = new URLLoader();

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, ClipAutoDownloadError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseDownloadClipResult, false, 0, true);
			
			var request:URLRequest = new URLRequest(RequestURL);
			
			//request.method =URLRequestMethod.GET;		
			
			//request.contentType="application/json";

			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
		}		
		
		function parseDownloadClipResult(e:Event):void
		{
			trace ("Message :"+e.target.data);
		}
		
		function parseUGCClipExportResult(e:Event):void
		{
			trace ("Message :"+e.target.data);
		}
		
		function ClipMetaCall_Make(ClipGUID:String,CategoryGUID:String):void
		{
			var RequestURL:String;
			
			RequestURL = srvString + "/svc/clip/update?c={\"Guid\":\""+ ClipGUID +"\",\"MetaData\":[{\"k\":\"iqClientid\",\"v\":\""+clientGUID+"\"},{\"k\":\"iQUser\",\"v\":\""+customerGUID+"\"},{\"k\":\"iQCategory\",\"v\":\""+CategoryGUID+"\"}]}";
			var request:URLRequest = new URLRequest(RequestURL); 
			request.method = URLRequestMethod.GET; 
			var loader:URLLoader = new URLLoader();
			loader.addEventListener(Event.COMPLETE,ClipMetaCall_Returned);
			try 
			{ 
				var myDate:Date = new Date();
				TraceStr = TraceStr +  "Clip Meta Request Made :" + myDate.toTimeString();
				loader.load(request); 
			} 
			catch (err:Error)
			{
				trace("Error Occured"); 
			} 
		}
		
		function ClipMetaCall_Returned(e:Event):void
		{	
			var myDate:Date = new Date();
			TraceStr = TraceStr +  "Clip Meta Returned :" + myDate.toTimeString();
		
			var r:URLRequest = new URLRequest("javascript:showtrace('"+TraceStr+"')");
			navigateToURL(r,'_self');
			
			//trace(e.target.data);
			clipSave.statusTxt.text="Clip Created and Saved!";
			clipSave.loader_mc.onEnterFrame = null;
			//clipSave.loader_mc.alpha (0,.75,null,0,closeClipSave);
			clipSave.loader_mc.alpha = 0;
			closeClipSave();
			IsClipperEnable=false;
			isCustomSeek=false;
		}
		
		function closeClipSave () 
		{
			setTimeout (closeClipSaveUI,2000);
		}

		function closeClipSaveUI () 
		{
			clipperUI.keyStatus = false;
			Tweener.addTween(clipperUI, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			Tweener.addTween(clipSave, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			clipperUI.visible = false;
			clipSave.visible = false;
			FlagPopup = false;
		}

		//**************BookMark Code****************
		private function clk_newsvine_mc(e:Event)
		{
			var url:String = "http://www.newsvine.com/_tools/seed?popoff=0&u=" + escape(linkVideo.urlTxt.text);
			linkVideo.bookmarkTxt.text="Post to: www.newsvine.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_delicious_mc(e:Event)
		{
			var url:String = "http://del.icio.us/post?v=2&url=" + escape(linkVideo.urlTxt.text) + "&title=" + clipName;
			linkVideo.bookmarkTxt.text="Post to: del.icio.us";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_netvibes_mc(e:Event)
		{
			var url:String = "http://www.netvibes.com/subscribe.php?url=" + escape(linkVideo.urlTxt.text);
			linkVideo.bookmarkTxt.text="Post to: www.netvibes.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_reddit_mc(e:Event)
		{
			var url:String = "http://reddit.com/submit?url=" + escape(linkVideo.urlTxt.text) + "&title=" + clipName;
			linkVideo.bookmarkTxt.text="Post to: www.reddit.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_yahoo_mc(e:Event)
		{
			var url:String = "http://bookmarks.yahoo.com/toolbar/SaveBM?u=" + escape(linkVideo.urlTxt.text) + "&t=" + clipName;
			linkVideo.bookmarkTxt.text="Post to: bookmarks.yahoo.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_google_mc(e:Event)
		{
			var url:String = "http://www.google.com/bookmarks/mark?op=add&bkmk=" + escape(linkVideo.urlTxt.text) + "&title=" + clipName;
			trace(linkVideo.urlTxt.text);
			linkVideo.bookmarkTxt.text="Post to: www.google.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_magnolia_mc(e:Event)
		{
			var url:String = "http://ma.gnolia.com/bookmarklet/add?url=" + escape(linkVideo.urlTxt.text) + "&title=" + clipName;
			linkVideo.bookmarkTxt.text="Post to: www.magnolia.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_technorati_mc(e:Event)
		{
			var url:String = "http://technorati.com/faves?sub=favthis&add=" + escape(linkVideo.urlTxt.text);
			linkVideo.bookmarkTxt.text="Post to: www.technorati.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_myspace_mc(e:Event)
		{
			var fvars:String = "embedId=" + _fileId;
			var url:String = "http://www.myspace.com/Modules/PostTo/Pages/?t=" + clipName + "&c=" + "<object width=\"390\" height=\"320\" \"><param name=\"movie\" value=\"" + embedUrl + "\" /><param name=\"flashvars\" value=\"" + fvars + "\" /><embed src=\"" + embedUrl + "\" flashvars=\"" + fvars + "\" width=\"390\" height=\"320\" type=\"application/x-shockwave-flash\" \"></embed></object>" + "&u=" + escape(linkVideo.urlTxt.text) + "&l=1";
			
			linkVideo.bookmarkTxt.text="Post to: www.myspace.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_digg_mc(e:Event)
		{
			var url:String = "http://digg.com/submit?phase=2&url=" + escape(linkVideo.urlTxt.text) + "&title=" + clipName;
			linkVideo.bookmarkTxt.text="Post to: www.digg.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_stumble_mc(e:Event)
		{
			var url:String = "http://www.stumbleupon.com/submit?url=" + escape(linkVideo.urlTxt.text) + "&title=" + clipName;
			linkVideo.bookmarkTxt.text="Post to: www.stumbleupon.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_fbook_mc(e:Event)
		{
			var tempURL:String = linkVideo.urlTxt.text;
			
			tempURL = tempURL + "&source=facebook";
			
			var tampTitle:String = clipName;
			var url:String = "http://www.facebook.com/sharer.php?u=" + escape(tempURL) + "&t=" + clipName;
			linkVideo.bookmarkTxt.text="Post to: www.facebook.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_socialmarker_mc(e:Event)
		{
			var url:String = "http://www.socialmarker.com/?link=" + escape(linkVideo.urlTxt.text)+ "&title=" + clipName + "&text=" + clipDesc;
			linkVideo.bookmarkTxt.text="Post to: www.socialmarker.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function shortURL_onLoaded(event:URLShortenEvent):void
		{
			//tracer("INFO", event.service + ": " + event.url);
			var url:String = "http://twitter.com/home?status=" + escape(clipName) + "%20" + event.url;
			//tracer("INFO", "Twitter URL: " + event.url);
			linkVideo.bookmarkTxt.text="Post to: www.twitter.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function urlShrink(arg):void
		{
			var url:String = arg;
			var us:URLShorten = new URLShorten();
			us.addEventListener(URLShortenEvent.ON_URL_SHORTED, shortURL_onLoaded);
			us.bitly(url, "iqmediacorp", "R_e52607797ee611c982cb464fba458524", null);
			//us.bitly(url, "vrparekh", "R_a5b19c917b4f53c43859e5a53a8c8bac", null);
			//us.bitly(url, "IQMedia", "R_093722e292a45716a0228dbfd3fcfbb5", null);
			// Above are IQMedia idKEY and API token for bit.ly
			//us.tinyurl (url);
		}

		private function clk_twitter_mc(e:Event)
		{
			urlShrink(escape(linkVideo.urlTxt.text));
		}

		private function clearBookMarkText(item):void
		{
			removeGlow(item);
			linkVideo.bookmarkTxt.text="";
		}

		private function digg_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.digg.com";
		}

		private function digg_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function fbook_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.facebook.com";
		}

		private function fbook_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function stumble_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.stumbleupon.com";
		}

		private function stumble_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function newsvine_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.newsvine.com";
		}

		private function newsvine_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function netvibes_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.netvibes.com";
		}

		private function netvibes_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function reddit_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.reddit.com";
		}

		private function reddit_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function yahoo_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: bookmarks.yahoo.com";
		}

		private function yahoo_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function google_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.google.com/bookmarks/";
		}

		private function google_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function delicious_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: del.icio.us";
		}

		private function delicious_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function magnolia_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.magnolia.com";
		}

		private function magnolia_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function technorati_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.techorati.com";
		}

		private function technorati_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function myspace_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.myspace.com";
		}

		private function myspace_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function socialmarker_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.socialmarker.com";
		}

		private function socialmarker_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function twitter_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text="Post to: www.twitter.com";
		}

		private function twitter_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function ovr(e:Event)
		{
			applyGlow(e.target);

			switch (String(e.currentTarget.name))
			{
				case "copy_mc":
					embedVideo.embedTxt.stage.focus=embedVideo.embedTxt;
					embedVideo.embedTxt.setSelection(0, embedVideo.embedTxt.text.length);
					break;
				case "WPcopy_mc":
					embedVideo.WPembedTxt.stage.focus=embedVideo.WPembedTxt;
					embedVideo.WPembedTxt.setSelection(0, embedVideo.WPembedTxt.text.length);
					break;
				case "lcopy_mc":
					linkVideo.urlTxt.stage.focus=linkVideo.urlTxt;
					linkVideo.urlTxt.setSelection(0, linkVideo.urlTxt.text.length);
					break;
				case "cancel_mc":
					linkVideo.urlTxt.stage.focus=null;
					linkVideo.urlTxt.setSelection(0, 0);
					break;
				case "WPcancel_mc":
					linkVideo.urlTxt.stage.focus=null;
					linkVideo.urlTxt.setSelection(0, 0);
					break;
			}
		}
		
		private function ClipperUIPlayAction(e:Event)
		{
				clipperUI.pp_mc.gotoAndStop (3);
				myVid.play();
				
				clipperUI.pp_mc.gotoAndStop (3);
				closeMenu();
				clipperUI.pp_mc.removeEventListener(MouseEvent.MOUSE_DOWN,ClipperUIPlayAction);
				clipperUI.pp_mc.addEventListener(MouseEvent.MOUSE_DOWN,ClipperUIPauseAction);
				
		}
		
		private function ClipperUIPauseAction(e:Event)
		{
		
			myVid.pause();
			clipperUI.pp_mc.gotoAndStop (2);						
			/*setVideoStatus (false);
			clipperUI.pp_mc.gotoAndStop (2);*/
			closeMenu();
			clipperUI.pp_mc.removeEventListener(MouseEvent.MOUSE_DOWN,ClipperUIPauseAction);
			clipperUI.pp_mc.addEventListener(MouseEvent.MOUSE_DOWN,ClipperUIPlayAction);
			
		}
		
		private function ClipMCClick(e:Event)
		{
			clipSave.statusTxt.text = "";		
			
			clipperUI.visible = true;
			
			
			
			IsClipperEnable=true;
			
			if(mc_pp.currentFrame==3)
			{
				var evt:Event = new Event(MouseEvent.CLICK, true, false);
				mc_pp.dispatchEvent(evt);				
			}
			
			closeMenu();
			if(IsRawMedia.toLocaleUpperCase() == "TRUE")
			{
				trace(".........FF/Rew Disabled ClipMCClick........");
				EnableForwardRewind(false);
			}
			
			SizeItems();		
			
			popmask.height = stage.stageHeight;
			
			Tweener.addTween(clipperUI, {x: 0, y: stage.stageHeight - 28, time: .35, transition: "linear"});
			
			clipperUIAdjust ();
			
			var startPoint:Number;
			var markerPoint:Number;		
			
			clipperUI.loader_mc.onEnterFrame = null;
			
			clipperUI.loader_mc.alpha=0;
			clipperUI.loader_mc.seconds=0.35;			
			clipperUI.pp_mc.alpha=100;
			
			stage.addEventListener(KeyboardEvent.KEY_DOWN,ClipperKeyDown);
			stage.addEventListener(KeyboardEvent.KEY_UP,ClipperKeyUP);
			
			FlagPopup = true;
			
			clipperUI.clearButt.addEventListener(MouseEvent.ROLL_OVER, ROLLOVERshortcuts, false, 0, true);
			clipperUI.clearButt.addEventListener(MouseEvent.ROLL_OUT, ROLLOUTshortcuts,false, 0, true);
		}	
		
		private function ClipperKeyDown(event:KeyboardEvent)
		{
			if (event.keyCode == Keyboard.SHIFT)
			{
				keyShift=true;
			}
			else if(event.keyCode==Keyboard.LEFT)
			{
				keyLeft=true;
			}
			else if(event.keyCode==Keyboard.RIGHT)
			{
				keyRight=true;
			}
			else if(event.keyCode==Keyboard.SPACE)
			{
				keySpace=true;
			}
			else if(event.keyCode==Keyboard.CONTROL)
			{							
				keyCtl=true;
			}						
			
			/*var changePositionOffset:Number;
			changePositionOffset=0.1;*/
			
			if((keyLeft==true || keyRight==true) && ((keySpace==true && keyShift==false && keyCtl==false) || (keySpace==false && keyShift==true && keyCtl==false) || (keySpace==false && keyShift==false && keyCtl==false) || (keySpace==false && keyShift==true && keyCtl==true) || (keySpace==false && keyShift==false && keyCtl==true)))
			{			
				
				ChangeKnob();
			}			
		}
		
		private function ClipperKeyUP(event:KeyboardEvent)
		{
			var IsMS:Boolean=false;
			var IsNavigate:Boolean=false;
			
			if (event.keyCode == Keyboard.SHIFT)
			{
				keyShift=false;
			}
			else if(event.keyCode==Keyboard.LEFT)
			{
				IsNavigate=true;
				keyLeft=false;				
			}
			else if(event.keyCode==Keyboard.RIGHT)
			{
				IsNavigate=true;
				keyRight=false;
			}
			else if(event.keyCode==Keyboard.SPACE)
			{
				keySpace=false;
			}			
			else if(event.keyCode==Keyboard.CONTROL)
			{							
				IsMS=true;
			}					
			
			var seekPointLeft:Number;
			var seekPointRight:Number;
			
			seekPointLeft = (clipperUI.leftKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			seekPointRight = (clipperUI.rightKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			
			if(keyLeft==true || keyRight==true || IsNavigate==true)
			{
				ChangeKnob();
			}
			
			if(IsMS==true)
			{
				updateScrubTime();
				keyCtl=false;
			}
			
			if(isLeftKnob==true)
			{
				myVid.seek(seekPointLeft+offsetTime);
			}
			else if(isLeftKnob==false)
			{
				myVid.seek(seekPointRight+offsetTime);
			}
		}
		
		private function ChangeKnob():Boolean
		{
			var seekPointLeft:Number;
			var seekPointRight:Number;
			var IsLeft:Boolean;
			
			mc_pp.gotoAndStop(2);
			myVid.pause();
			
			if(keyLeft==true)
			{
				if(keyShift==true && keyCtl==false)
				{
					clipperUI.rightKnob.x =  clipperUI.rightKnob.x -0.1;
					
					if (clipperUI.rightKnob.x <= clipperUI.leftKnob.x)
					{
						clipperUI.rightKnob.x =  clipperUI.rightKnob.x +0.1;
					}
					
					clipperUI.cKnob.x=clipperUI.rightKnob.x;
					clipperUI.progBar.x =clipperUI.progBar.x-0.1;
					
					IsLeft=false;
				}
				else if(keySpace==true)
				{
					clipperUI.leftKnob.x =  clipperUI.leftKnob.x -0.1;
					clipperUI.rightKnob.x =  clipperUI.rightKnob.x -0.1;					
					clipperUI.progBar.x =clipperUI.progBar.x-0.1;
					
					if(clipperUI.leftKnob.x<clipperUI.scrubBar.x)
					{
						clipperUI.leftKnob.x=clipperUI.scrubBar.x;
						clipperUI.rightKnob.x=clipperUI.rightKnob.x+0.1;
					}
					
					clipperUI.cKnob.x=clipperUI.leftKnob.x;
					
					IsLeft=true;
				}
				else if(keyCtl==true && keyShift==false)
				{
					var tempLeftKnobx:Number=clipperUI.leftKnob.x;					
					
					tempLeftKnobx=tempLeftKnobx-0.0001;
					clipperUI.leftKnob.x =  tempLeftKnobx;						
					
					if (clipperUI.leftKnob.x <= clipperUI.scrubBar.x)
					{
						clipperUI.leftKnob.x = clipperUI.scrubBar.x + 0.0001;
					}				
					
					clipperUI.cKnob.x=clipperUI.leftKnob.x;
					clipperUI.progBar.x =clipperUI.progBar.x-0.0001;
					
					IsLeft=true;
				}
				else if(keyCtl==true && keyShift==true)
				{
					var tempRightKnobx:Number=clipperUI.rightKnob.x;
					tempRightKnobx=tempRightKnobx-0.0001;
					
					clipperUI.rightKnob.x =  tempRightKnobx;
					
					if (clipperUI.rightKnob.x <= clipperUI.leftKnob.x)
					{
						clipperUI.rightKnob.x =  clipperUI.rightKnob.x +0.05;
					}
					
					clipperUI.cKnob.x=clipperUI.rightKnob.x;
					clipperUI.progBar.x =clipperUI.progBar.x-0.05;
					
					IsLeft=false;
				}
				else
				{
					clipperUI.leftKnob.x =  clipperUI.leftKnob.x -0.1;
					
					
					if (clipperUI.leftKnob.x <= clipperUI.scrubBar.x)
					{
						clipperUI.leftKnob.x = clipperUI.scrubBar.x + 0.1;
					}				
					
					clipperUI.cKnob.x=clipperUI.leftKnob.x;
					clipperUI.progBar.x =clipperUI.progBar.x-0.1;				
					
					IsLeft=true;
				}
			}
			else if(keyRight==true)
			{
				if(keyShift==true && keyCtl==false)
				{
					clipperUI.rightKnob.x =  clipperUI.rightKnob.x +0.1;
					clipperUI.progBar.x =clipperUI.progBar.x+0.1;	
					
					if (clipperUI.rightKnob.x > clipperUI.scrubBar.x + clipperUI.scrubBar.width)
					{
						clipperUI.rightKnob.x = clipperUI.scrubBar.x + clipperUI.scrubBar.width;
					}
					
					clipperUI.cKnob.x=clipperUI.rightKnob.x;
					
					IsLeft=false;
				}
				else if(keySpace==true)
				{
					clipperUI.rightKnob.x =  clipperUI.rightKnob.x +0.1;
					clipperUI.leftKnob.x =  clipperUI.leftKnob.x +0.1;
					clipperUI.progBar.x =clipperUI.progBar.x+0.1;
					
					if (clipperUI.rightKnob.x > clipperUI.scrubBar.x + clipperUI.scrubBar.width)
					{
						clipperUI.rightKnob.x = clipperUI.scrubBar.x + clipperUI.scrubBar.width;
						clipperUI.leftKnob.x =  clipperUI.leftKnob.x -0.1;
					}
					
					clipperUI.cKnob.x=clipperUI.leftKnob.x;
					
					IsLeft=true;
				}
				else if(keyCtl==true && keyShift==false)
				{
					var beforeIn:Number=clipperUI.leftKnob.x;										
					
					clipperUI.leftKnob.x =  clipperUI.leftKnob.x +0.05;					
					clipperUI.progBar.x =clipperUI.progBar.x+0.05;
					
					if(beforeIn==clipperUI.leftKnob.x)
					{
						clipperUI.leftKnob.x =  clipperUI.leftKnob.x +0.1;					
						clipperUI.progBar.x =clipperUI.progBar.x+0.1;
					}
					
					if (clipperUI.leftKnob.x >= clipperUI.rightKnob.x)
					{
						clipperUI.leftKnob.x = clipperUI.leftKnob.x - 0.05;
						//trace(clipperUI.leftKnob.x+"...leftknob inside");
					}
					
					clipperUI.cKnob.x=clipperUI.leftKnob.x;
					
					IsLeft=true;
				}
				else if(keyCtl==true && keyShift==true)
				{
					var tempRightKnobx:Number=clipperUI.rightKnob.x;	
					
					tempRightKnobx=tempRightKnobx+0.05;
					clipperUI.rightKnob.x =  tempRightKnobx;						
					
					if (clipperUI.rightKnob.x > clipperUI.scrubBar.x + clipperUI.scrubBar.width)
					{
						clipperUI.rightKnob.x = clipperUI.scrubBar.x + clipperUI.scrubBar.width;						
					}			
					
					clipperUI.cKnob.x=clipperUI.rightKnob.x;
					clipperUI.progBar.x =clipperUI.progBar.x+0.05;
					
					IsLeft=false;
				}
				else
				{
					clipperUI.leftKnob.x =  clipperUI.leftKnob.x +0.1;
					clipperUI.progBar.x =clipperUI.progBar.x+0.1;
					
					if (clipperUI.leftKnob.x >= clipperUI.rightKnob.x)
					{
						clipperUI.leftKnob.x = clipperUI.leftKnob.x - 0.1;
					}
					
					clipperUI.cKnob.x=clipperUI.leftKnob.x;
					
					IsLeft=true;
				}
			}
			
			updateProgBar();
			
			if(IsLeft==true && (keyLeft==true || keyRight==true))
			{
				seekPointLeft = (clipperUI.leftKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
				
				clipperUI.timeDisplayScrub.text=timeCode(seekPointLeft+offsetTime);
				
				isLeftKnob=true;
			}
			else if(IsLeft==false && (keyLeft==true || keyRight==true))
			{
				seekPointRight = (clipperUI.rightKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
				
				
				clipperUI.timeDisplayScrub.text=timeCode(seekPointRight+offsetTime);
				
				isLeftKnob=false;
			}
			
			return IsLeft;
		}
		
		private function updateSaveTime()
		{
			if (clipperUI.leftKnob.x <= clipperUI.scrubBar.x)
			{
				clipperUI.leftKnob.x = clipperUI.scrubBar.x + 1;
			}
			if (clipperUI.rightKnob.x > clipperUI.scrubBar.x + clipperUI.scrubBar.width)
			{
				clipperUI.rightKnob.x = clipperUI.scrubBar.x + clipperUI.scrubBar.width;
			}
			
			var seekPointLeft:Number;
			var seekPointRight:Number;
			
			seekPointLeft = (clipperUI.leftKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			seekPointRight = (clipperUI.rightKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width)
			
			clipperUI.timeDisplayCur.text = timeCode (seekPointLeft);
			clipperUI.timeDisplayTot.text=timeCode (seekPointRight);
			
			clipperUI.timeDisplayCur.x=clipperUI.leftKnob.x-49;
			clipperUI.timeDisplayTot.x=clipperUI.rightKnob.x;
			
			clipperUI.clipDuration.text=timeCode(seekPointRight-seekPointLeft);
			
			clipperUI.progBar.x = clipperUI.leftKnob.x;
			clipperUI.progBar.width = clipperUI.rightKnob.x - clipperUI.leftKnob.x;
			clipSave.timeStartEndTxt.text = clipperUI.timeDisplayCur.text + " / " + clipperUI.timeDisplayTot.text;
		}
		
		private function ClipMCLeftKnobClick(e:Event)
		{			
			
			clipperUI.leftKnob.addEventListener(Event.ENTER_FRAME ,ClipMCLeftKnobLocationChange);
			
			var checkLeft:Number;
			var checkRight:Number;
			var leftMost:Number;
			var refWidth:Number;
			var checkLeftSeek:Number;
			var checkRightSeek:Number;
			var leftPosCheck:Number;
			var leftKnobConstraint:Number;
			
			checkLeft = clipperUI.leftKnob.x;
			checkRight = clipperUI.rightKnob.x;
			leftMost = clipperUI.scrubBar.x;
	
			refWidth = clipperUI.scrubBar.width;
	
			checkLeftSeek = (checkLeft - leftMost) * (ftime / refWidth);
			checkRightSeek = (checkRight - leftMost) * (ftime / refWidth);
	
			leftPosCheck = checkRightSeek - ClipLength;
	
			if (leftPosCheck < 0)
			{
				leftPosCheck = 0;
			}
			leftKnobConstraint = leftMost + (leftPosCheck * refWidth / ftime);			
			
			var rectangle:Rectangle = new Rectangle(clipperUI.scrubBar.x, clipperUI.leftKnob.y, (clipperUI.rightKnob.x - 1-clipperUI.scrubBar.x), 0);
			 
			clipperUI.leftKnob.startDrag(true,rectangle);						
			clipperUI.progBar.x = clipperUI.leftKnob.x;
			clipperUI.progBar.width = clipperUI.rightKnob.x - clipperUI.leftKnob.x;
			clipperUI.updateProgBarInt = setInterval (updateProgBar, 99);
			clipperUI.timeDisplayCur.x = clipperUI.leftKnob.x - 49;
			/*ns.pause (true);*/
			clipperUI.pp_mc.gotoAndStop (2);
			this.addEventListener(MouseEvent.MOUSE_UP,ClipMCLeftKnobUP);
		}
		
		private function ClipMCLeftKnobUP(e:Event)
		{			
			var evt:Event = new Event(Event.ENTER_FRAME, true, false);
			clipperUI.leftKnob.dispatchEvent(evt);
			
			clipperUI.leftKnob.removeEventListener(Event.ENTER_FRAME ,ClipMCLeftKnobLocationChange);
				
			clipperUI.leftKnob.stopDrag();
			clearInterval (clipperUI.updateProgBarInt);
			clipperUI.progBar.x = clipperUI.leftKnob.x;
			clipperUI.cKnob.x=clipperUI.leftKnob.x;
			clipperUI.progBar.width = clipperUI.rightKnob.x - clipperUI.leftKnob.x;
			clipperUI.timeDisplayCur.x = clipperUI.leftKnob.x - 49;
			this.removeEventListener(MouseEvent.MOUSE_UP,ClipMCLeftKnobUP);			
			
			var seekPointLeft:Number;			
			
			seekPointLeft = (clipperUI.leftKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			myVid.seek((offsetTime+seekPointLeft));
			
			updateScrubTime ();		
			
			clipperUI.timeDisplayScrub.text = timeCode (seekPointLeft+offsetTime);			
		}
		
		private function updateProgBar()
		{
			var checkLeft:Number;
			var checkRight:Number;
			var leftMost:Number;
			var refWidth:Number;
			var checkLeftSeek:Number;
			var checkRightSeek:Number;
			var leftPosCheck:Number;
			var rightPosCheck:Number;
			var leftKnobConstraint:Number;
			var rightKnobConstraint:Number;
			
			checkLeft = clipperUI.leftKnob.x;
			checkRight = clipperUI.rightKnob.x;
			leftMost = clipperUI.scrubBar.x;
			refWidth = clipperUI.scrubBar.width;
			checkLeftSeek = (checkLeft - leftMost) * (ftime / refWidth);
			checkRightSeek = (checkRight - leftMost) * (ftime / refWidth);
			if (checkRightSeek - checkLeftSeek > ClipLength)
			{
				rightPosCheck = checkLeftSeek + ClipLength;
				rightKnobConstraint = leftMost + (rightPosCheck * refWidth / ftime);
				clipperUI.rightKnob.x = rightKnobConstraint;
				clipperUI.progBar.width = clipperUI.rightKnob.x - clipperUI.leftKnob.x;
			}
			clipperUI.timeDisplayCur.x = clipperUI.leftKnob.x - 49;
			clipperUI.timeDisplayTot.x = clipperUI.rightKnob.x;
			//was 12
			clipperUI.timeDisplayScrub.x = clipperUI.leftKnob.x;
			clipperUI.progBar.x = clipperUI.leftKnob.x;
			clipperUI.progBar.width = clipperUI.rightKnob.x - clipperUI.leftKnob.x;
			updateScrubTime ();
		};
		
		private function updateScrubTime()
		{
			var seekPointLeft:Number;
			var seekPointRight:Number;
			
			seekPointLeft = (clipperUI.leftKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			seekPointRight = (clipperUI.rightKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			registerStopPoint (seekPointRight);			
			
			if((seekPointRight-seekPointLeft)>ClipLength)
			{
				var TempDif:Number;
				TempDif=(seekPointRight-seekPointLeft)-ClipLength;
				seekPointRight=seekPointRight-TempDif;
			}
			
			clipperUI.timeDisplayScrub.x = clipperUI.cKnob.x;
			
			if(keyCtl==true)
			{
				clipperUI.timeDisplayCur.text = timeCodeWithMS (seekPointLeft+offsetTime);
				clipperUI.timeDisplayTot.text = timeCodeWithMS (seekPointRight+offsetTime);
			}
			else
			{
				clipperUI.timeDisplayCur.text = timeCode (seekPointLeft+offsetTime);
				clipperUI.timeDisplayTot.text = timeCode (seekPointRight+offsetTime);
			}
			
			clipperUI.clipDuration.text=timeCode((seekPointRight+offsetTime)-(seekPointLeft+offsetTime));
			
			clipSave.timeStartEndTxt.text = clipperUI.timeDisplayCur.text + " / " + clipperUI.timeDisplayTot.text;
			
			isCustomSeek=true;
			customSeekPoint=(seekPointLeft+offsetTime);
			customSeekPointRight = (seekPointRight+offsetTime);
			
		};
		
		private function ClipMCRightKnobClick(e:Event)
		{
			clipperUI.rightKnob.addEventListener(Event.ENTER_FRAME ,ClipMCRightKnobLocationChange);
			
			var checkLeft:Number;
			var checkRight:Number;
			var leftMost:Number;
			var refWidth:Number;
			var checkLeftSeek:Number;
			var checkRightSeek:Number;
			var rightPosCheck:Number;
			var rightKnobConstraint:Number;
			
			checkLeft = clipperUI.leftKnob.x;
			checkRight = clipperUI.rightKnob.x;
			leftMost = clipperUI.scrubBar.x;
	
			refWidth = clipperUI.scrubBar.width;
	
			checkLeftSeek = (checkLeft - leftMost) * (ftime / refWidth);
			checkRightSeek = (checkRight - leftMost) * (ftime / refWidth);
	
			rightPosCheck = checkLeftSeek + ClipLength;
	
			if (rightPosCheck > ftime)
			{
				rightPosCheck = ftime;
			}
			rightKnobConstraint = (leftMost + (rightPosCheck * refWidth / ftime))-clipperUI.leftKnob.x;
			
			var msg:String;
			var rectangle:Rectangle = new Rectangle(clipperUI.leftKnob.x+1, clipperUI.rightKnob.y,rightKnobConstraint, 0);
								
			clipperUI.rightKnob.startDrag(true,rectangle);						
			clipperUI.progBar.width = clipperUI.rightKnob.x - clipperUI.leftKnob.x;
			clipperUI.updateProgBarInt = setInterval (updateProgBar, 99);
			clipperUI.timeDisplayTot.x = clipperUI.rightKnob.x;
			clipperUI.pp_mc.gotoAndStop (2);
			this.addEventListener(MouseEvent.MOUSE_UP,ClipMCRightKnobUP);
		}
		
		private function ClipMCRightKnobUP(e:Event)
		{
			var evt:Event = new Event(Event.ENTER_FRAME, true, false);
			clipperUI.rightKnob.dispatchEvent(evt);
			
			clipperUI.rightKnob.removeEventListener(Event.ENTER_FRAME ,ClipMCRightKnobLocationChange);
			
			clipperUI.rightKnob.stopDrag ();
			clearInterval (clipperUI.updateProgBarInt);
			clipperUI.progBar.x = clipperUI.leftKnob.x;
			clipperUI.progBar.width = clipperUI.rightKnob.x - clipperUI.leftKnob.x;
			clipperUI.timeDisplayTot.x = clipperUI.rightKnob.x;
			clipperUI.cKnob.x=clipperUI.rightKnob.x;
			this.removeEventListener(MouseEvent.MOUSE_UP,ClipMCRightKnobUP);					
			
			var seekPointRight:Number;			
			
			seekPointRight = (clipperUI.rightKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			
			updateScrubTime ();	
			clipperUI.timeDisplayScrub.text = timeCode (seekPointRight+offsetTime);
			myVid.playheadTime=(seekPointRight+offsetTime);
		}

		private function clipperUIAdjust () 
		{
			clipperUI.leftKnob.buttonMode=true;
			clipperUI.rightKnob.buttonMode=true;
			clipperUI.botbar_mc.width = stage.stageWidth;			
			clipperUI.save_mc.x = stage.stageWidth - 56;
			clipperUI.cancel_mc.x = clipperUI.save_mc.x - 58;
			clipperUI.scrubBar.x = (Scrub.x+Scrub.scrubBar.x);
			clipperUI.progBar.x = (Scrub.x+Scrub.scrubBar.x);			
			clipperUI.scrubBar.width = Scrub.width-Scrub.scrubBar.x;
			
			var centerSeek:Number;
			var leftSeek:Number;
			var rightSeek:Number;
			var leftPos:Number;
			var rightPos:Number;
			var seekPointLeft:Number;
			var seekPointRight:Number;		
			
			//centerSeek = ui.vid_mc.sTime;
			centerSeek = myVid.playheadTime;

			leftSeek = centerSeek;
			if (leftSeek < 0)
			{
				leftSeek = 0;
			}
			rightSeek = leftSeek + DefaultClipLength;
			if (rightSeek > globalStop)
			{
				rightSeek = globalStop;
			}
			
			leftPos = clipperUI.scrubBar.x + ((ftime-(globalStop-leftSeek)) * clipperUI.scrubBar.width / ftime);
			rightPos = clipperUI.scrubBar.x + ((ftime-(globalStop-rightSeek)) * clipperUI.scrubBar.width / ftime);
			clipperUI.leftKnob.x = leftPos;
			clipperUI.rightKnob.x = rightPos;
			
			clipperUI.hitMarker_mc.x=leftPos;
			clipperUI.cKnob.x = leftPos;
			
			// determine 10 minutes past left knob
		
			clipperUI.progBar.x = leftPos;
			clipperUI.progBar.width = rightPos - leftPos;
			clipperUI.rightKnob.x = clipperUI.progBar.width+clipperUI.progBar.x;
			clipperUI.timeDisplayCur.x = clipperUI.leftKnob.x - 49;//was 52
			clipperUI.timeDisplayTot.x = clipperUI.rightKnob.x;

			clipperUI.timeDisplayScrub.x = clipperUI.cKnob.x;// - clipperUI.timeDisplayScrub._width/2;
			seekPointLeft = (clipperUI.leftKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			seekPointRight = (clipperUI.rightKnob.x - clipperUI.scrubBar.x) * ftime / (clipperUI.scrubBar.width);
			
			clipperUI.timeDisplayCur.text = timeCode (seekPointLeft+offsetTime);
			clipperUI.timeDisplayTot.text = timeCode (seekPointRight+offsetTime);
			clipperUI.timeDisplayScrub.text=timeCode (seekPointLeft+offsetTime);

			
			clipperUI.clipDuration.text=timeCode ((seekPointRight+offsetTime)-(seekPointLeft+offsetTime));
			
			clipSave.timeStartEndTxt.text = clipperUI.timeDisplayCur.text + " / " + clipperUI.timeDisplayTot.text;
			
			customSeekPointRight = (seekPointRight+offsetTime);
			
			clipperUI.save_mc.txtLabel.text = "FINISH";
			//trace("FINISH===" +clipperUI.save_mc.txtLabel.text);						
		}
		
		private function registerStopPoint (endPoint:Number) 
		{
			clipperUI.endPoint = endPoint;			
		}
		
		private function off(e:Event)
		{
			e.target.filters=null;
		}

		private function applyGlow(inst)
		{
			var amt:Number = 15;			
			var filter:GlowFilter = new GlowFilter(0xFFffff, .8, amt, amt, 1.5, 3, false, false);
			var filterArray:Array = new Array();
			filterArray.push(filter);
			inst.filters=filterArray;
		}

		private function removeGlow(inst)
		{
			inst.filters=null;
		}


		private function setupBookMarkListeners():void
		{
			/*linkVideo.newsvine_mc.addEventListener(MouseEvent.CLICK, clk_newsvine_mc);
			linkVideo.newsvine_mc.addEventListener(MouseEvent.MOUSE_OVER, newsvine_mc_roll);
			linkVideo.newsvine_mc.addEventListener(MouseEvent.MOUSE_OUT, newsvine_mc_out);
			linkVideo.newsvine_mc.buttonMode=true;

			linkVideo.delicious_mc.addEventListener(MouseEvent.CLICK, clk_delicious_mc);
			linkVideo.delicious_mc.addEventListener(MouseEvent.MOUSE_OVER, delicious_mc_roll);
			linkVideo.delicious_mc.addEventListener(MouseEvent.MOUSE_OUT, delicious_mc_out);
			linkVideo.delicious_mc.buttonMode=true;

			linkVideo.netvibes_mc.addEventListener(MouseEvent.CLICK, clk_netvibes_mc);
			linkVideo.netvibes_mc.addEventListener(MouseEvent.MOUSE_OVER, netvibes_mc_roll);
			linkVideo.netvibes_mc.addEventListener(MouseEvent.MOUSE_OUT, netvibes_mc_out);
			linkVideo.netvibes_mc.buttonMode=true;

			linkVideo.reddit_mc.addEventListener(MouseEvent.CLICK, clk_reddit_mc);
			linkVideo.reddit_mc.addEventListener(MouseEvent.MOUSE_OVER, reddit_mc_roll);
			linkVideo.reddit_mc.addEventListener(MouseEvent.MOUSE_OUT, reddit_mc_out);
			linkVideo.reddit_mc.buttonMode=true;
*/
			linkVideo.yahoo_mc.addEventListener(MouseEvent.CLICK, clk_yahoo_mc);
			linkVideo.yahoo_mc.addEventListener(MouseEvent.MOUSE_OVER, yahoo_mc_roll);
			linkVideo.yahoo_mc.addEventListener(MouseEvent.MOUSE_OUT, yahoo_mc_out);
			linkVideo.yahoo_mc.buttonMode=true;

			linkVideo.google_mc.addEventListener(MouseEvent.CLICK, clk_google_mc);
			linkVideo.google_mc.addEventListener(MouseEvent.MOUSE_OVER, google_mc_roll);
			linkVideo.google_mc.addEventListener(MouseEvent.MOUSE_OUT, google_mc_out);
			linkVideo.google_mc.buttonMode=true;

			linkVideo.twitter_mc.addEventListener(MouseEvent.CLICK, clk_twitter_mc);
			linkVideo.twitter_mc.addEventListener(MouseEvent.MOUSE_OVER, twitter_mc_roll);
			linkVideo.twitter_mc.addEventListener(MouseEvent.MOUSE_OUT, twitter_mc_out);
			linkVideo.twitter_mc.buttonMode=true;

			/*linkVideo.magnolia_mc.addEventListener(MouseEvent.CLICK, clk_magnolia_mc);
			linkVideo.magnolia_mc.addEventListener(MouseEvent.MOUSE_OVER, magnolia_mc_roll);
			linkVideo.magnolia_mc.addEventListener(MouseEvent.MOUSE_OUT, magnolia_mc_out);
			linkVideo.magnolia_mc.buttonMode=true;

			linkVideo.technorati_mc.addEventListener(MouseEvent.CLICK, clk_technorati_mc);
			linkVideo.technorati_mc.addEventListener(MouseEvent.MOUSE_OVER, technorati_mc_roll);
			linkVideo.technorati_mc.addEventListener(MouseEvent.MOUSE_OUT, technorati_mc_out);
			linkVideo.technorati_mc.buttonMode=true;
*/
			//linkVideo.myspace_mc.addEventListener(MouseEvent.CLICK, clk_myspace_mc);
			//linkVideo.myspace_mc.addEventListener(MouseEvent.MOUSE_OVER, myspace_mc_roll);
			//linkVideo.myspace_mc.addEventListener(MouseEvent.MOUSE_OUT, myspace_mc_out);
			//linkVideo.myspace_mc.buttonMode=true;

			/*linkVideo.digg_mc.addEventListener(MouseEvent.CLICK, clk_digg_mc);
			linkVideo.digg_mc.addEventListener(MouseEvent.MOUSE_OVER, digg_mc_roll);
			linkVideo.digg_mc.addEventListener(MouseEvent.MOUSE_OUT, digg_mc_out);
			linkVideo.digg_mc.buttonMode=true;

			linkVideo.stumble_mc.addEventListener(MouseEvent.CLICK, clk_stumble_mc);
			linkVideo.stumble_mc.addEventListener(MouseEvent.MOUSE_OVER, stumble_mc_roll);
			linkVideo.stumble_mc.addEventListener(MouseEvent.MOUSE_OUT, stumble_mc_out);
			linkVideo.stumble_mc.buttonMode=true;*/

			linkVideo.fbook_mc.addEventListener(MouseEvent.CLICK, clk_fbook_mc);
			linkVideo.fbook_mc.addEventListener(MouseEvent.MOUSE_OVER, fbook_mc_roll);
			linkVideo.fbook_mc.addEventListener(MouseEvent.MOUSE_OUT, fbook_mc_out);
			linkVideo.fbook_mc.buttonMode=true;

			/*linkVideo.socialmarker_mc.addEventListener(MouseEvent.CLICK, clk_socialmarker_mc);
			linkVideo.socialmarker_mc.addEventListener(MouseEvent.MOUSE_OVER, socialmarker_mc_roll);
			linkVideo.socialmarker_mc.addEventListener(MouseEvent.MOUSE_OUT, socialmarker_mc_out);
			linkVideo.socialmarker_mc.buttonMode=true;*/
		}


		//********************* MENU *************************************************

		private function linkVideoCancel(e:Event)
		{
			menuState="OPEN";
			activeBTN(true);
			//Tweener.addTween(linkVideo, {x: linkStopx, y: linkStopy, time: .3, transition: "linear"});
			Tweener.addTween(linkVideo, {x: 0, y: stage.stageHeight + 50, time: .35, transition: "linear"});
			showMenu();
			FlagPopup = false;
			linkVideo.visible = false;
		}		

		private function bookmark(e:Event)
		{
			scaleUpitem(linkVideo);
			linkVideo.visible=true;
			closeAbout();
			menuState="SHARING";
			menuSubState="LINKS";
			//activeBTN(false);
			fadeOverlay(true);
			FlagPopup = true;
			
			mcPlay.visible=false;
			mcPlay.enabled=false;
			Tweener.addTween(menubg, {x: menuStopx, y: menuStopy, time: .3, transition: "linear"});
			//Tweener.addTween(linkVideo, {x: linkStartx, y: linkStarty, time: .3, transition: "linear"});
			Tweener.addTween(linkVideo, {x: 0, y: 0, time: .35, transition: "linear"});

			if (_fileId == null || _fileId == undefined || _fileId == "")
			{
				_fileId=initClip;
			}
			linkVideo.urlTxt.stage.focus=linkVideo.urlTxt;
			linkVideo.urlTxt.setSelection(0, linkVideo.urlTxt.text.length);

		}
		
		private function BookMarkURL()
		{
			
			var FileName:String = clipName;
			
			var messages:Array = new Array ();
			
			var imagePath:String;
			if(PageName == "iqBasic")
			{
				imagePath =_imagePath
			}
			else if(PageName == "ClipPlayer")
			{
				imagePath =_imagePath
			}
			else
			{
				imagePath ="";
			}
			messages.push({"FileID": _fileId,"PageName": PageName});
			//messages.push({"From":"hello","FileID": "hello","PageName": "hello"});
			
			
			var JsonObj:String = "{\"FileID\":\""+_fileId+"\",\"PageName\":\""+PageName+"\"}";
			//var JsonObj:String = JSON.encode(messages);
			
			trace(JsonObj);
			
			//var variables:URLVariables=new URLVariables(JsonObj);
			var RequestURL:String;
			RequestURL= srvString + '/iqsvc/GetBookmarkURL';
			
			var JSONLoader:URLLoader = new URLLoader();
			JSONLoader.dataFormat=URLLoaderDataFormat.TEXT;

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, GetBookmarkURLError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseBookmarkURLResult, false, 0, true);
			
			
			var hdr:URLRequestHeader = new URLRequestHeader("Content-type", "application/json");
			
			var request:URLRequest = new URLRequest(RequestURL);
			request.requestHeaders.push(hdr);
			request.data=JsonObj;
			request.method = URLRequestMethod.POST;
			
			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
		}
		
		function parseBookmarkURLResult(e:Event):void
		{
			try
			{
				linkVideo.urlTxt.text =  JSON.decode(e.target.data);
			}
			catch (err:TypeError)
			{
				
			}
		}
		
		private function GetBookmarkURLError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred in GetBookmarkURLError"+evt.text);
		}
		
		function ShowTitle120Update(CurrentTime:Number)
		{
			try
			{
				var CuurentMin:Number = CurrentTime / 60;
				//var NewStartPoint:int =  int(CuurentMin / 15) + 1;
				//if(CurrentStartPoint !=  NewStartPoint)
				//{
				//	trace('----------CurrentStartPoint------------'+CurrentStartPoint);
				//	trace('----------NewStartPoint------------'+NewStartPoint);
				//	CurrentStartPoint = NewStartPoint;
				//}
				
				if(IsRawMedia.toLocaleUpperCase() == "TRUE" && ListIQStartPoint != null && ListIQStartPoint.length() > 0)
				{
					for (var i:int = EleProccessed+1; i < ListIQStartPoint.length(); i++)
					{
						var EleStartPoint:Number = Number(ListIQStartPoint[i].text());
						var EleStartMinute:Number = Number(ListIQStartMinute[i].text());
						if(CuurentMin < 15 && EleStartPoint == 1 && CuurentMin >= EleStartMinute)
						{
							//CurrentStartPoint  = EleStartPoint;
							Title120= ListTitle120[i].text();
							trace('-----------CuurentMin------------'+CuurentMin);
							trace('-----------EleStartMinute------------'+EleStartMinute);
							trace('-----------Title120------------'+Title120);
							EleProccessed = i;
							break;
						}
						else if(CuurentMin >= 15 && CuurentMin < 30 && EleStartPoint == 2 && CuurentMin >= EleStartMinute)
						{
							//CurrentStartPoint  = EleStartPoint;
							Title120= ListTitle120[i].text();
							trace('-----------CuurentMin------------'+CuurentMin);
							trace('-----------EleStartMinute------------'+EleStartMinute);
							trace('-----------Title120------------'+Title120);
							EleProccessed = i;
							break;
						}
						else if(CuurentMin >= 30 && CuurentMin < 45 && EleStartPoint == 3 && CuurentMin >= EleStartMinute)
						{
							//CurrentStartPoint  = EleStartPoint;
							Title120= ListTitle120[i].text();
							trace('-----------CuurentMin------------'+CuurentMin);
							trace('-----------EleStartMinute------------'+EleStartMinute);
							trace('-----------Title120------------'+Title120);
							EleProccessed = i;
							break;
						}
						else if(CuurentMin >= 45 && CuurentMin < 60 && EleStartPoint == 4 && CuurentMin >= EleStartMinute)
						{
							//CurrentStartPoint  = EleStartPoint;
							Title120= ListTitle120[i].text();
							trace('-----------CuurentMin------------'+CuurentMin);
							trace('-----------EleStartMinute------------'+EleStartMinute);
							trace('-----------Title120------------'+Title120);
							EleProccessed = i;
							break;
						}
                    }

					var strHelper:StringHelper = new StringHelper();
					if(strHelper.trim(Title120," ") == "")
					{
						meta_mc.meta3Txt.text = "Title: NA";
					}
					else
					{
						if(Title120.length > 42)
						{
							meta_mc.meta3Txt.text =  "Title: " + Title120.substr(0,40)+'...';
						}
						else
						{
							meta_mc.meta3Txt.text =  "Title: " + Title120;
						}
					}						
				}
			}
			catch(err:Error)
			{
			}
		}
		
		function CallNielSenDataService(Iq_Dma_Num:String)
		{
			var Start_Point_Min:Number = (globalStart) / 60;
			var IQ_Start_Point:Number=1;
			if(Start_Point_Min < 15)
			{
				IQ_Start_Point = 1;
			}
			else if(Start_Point_Min >= 15 && Start_Point_Min < 30)
			{
				IQ_Start_Point = 2;
			}
			else if(Start_Point_Min >= 30 && Start_Point_Min < 45)
			{
				IQ_Start_Point = 3;
			}
			else if(Start_Point_Min >= 45 && Start_Point_Min < 60)
			{
				IQ_Start_Point = 4;
			}
			
			var JsonObj:String = "{\"Guid\":\""+initClip+"\",\"ClientGuid\":\""+clientGUID+"\",\"IsRawMedia\":\""+IsRawMedia+"\",\"IQ_Start_Point\":\""+IQ_Start_Point+"\",\"IQ_Dma_Num\":\""+Iq_Dma_Num +"\"}";
			trace("GetNielSenData Requesat :"+JsonObj);
			
			//var variables:URLVariables=new URLVariables(JsonObj);
			
			
			var RequestURL:String;
			RequestURL= srvString + '/iqsvc/GetNielSenData';
			
			var JSONLoader:URLLoader = new URLLoader();
			JSONLoader.dataFormat=URLLoaderDataFormat.TEXT;

			JSONLoader.addEventListener(IOErrorEvent.IO_ERROR, NielSenDataServiceError, false, 0, true);
			JSONLoader.addEventListener(Event.COMPLETE, parseNielSenData, false, 0, true);
			
			
			var hdr:URLRequestHeader = new URLRequestHeader("Content-type", "application/json");
			
			var request:URLRequest = new URLRequest(RequestURL);
			request.requestHeaders.push(hdr);
			request.data=JsonObj;
			request.method = URLRequestMethod.POST;
			
			try 
			{
				JSONLoader.load(request);
			}
			catch (error:ArgumentError) 
			{ 
				trace("An ArgumentError has occurred."+error.errorID.toString()); 
			} 
			catch (error:SecurityError) 
			{ 
				trace("A SecurityError has occurred."); 
			}
			catch (error:Error) 
			{
				trace("Unable to load requested document.");
			}
		}
		
		function ShowNielSenUpdate(CurrentTime:Number)
		{
			try
			{
				var CuurentMin:Number = CurrentTime / 60;
				var NielSenADShare:String;
				var NielSenAudiance:String;
				if(NeilSenResponseRcvd == true && NielSenResponseObject.Status ==0)
				{
					if(NielSenResponseObject.NielSenData != null && NielSenResponseObject.NielSenData.length >0)
					{
						if(CuurentMin < 15)
						{
							NielSenADShare = NielSenResponseObject.NielSenData[0].SQAD_SHAREVALUE;
							NielSenAudiance = NielSenResponseObject.NielSenData[0].AUDIENCE;
						}
						else if(CuurentMin >= 15 && CuurentMin < 30 && NielSenResponseObject.NielSenData.length > 1)
						{
							NielSenADShare = NielSenResponseObject.NielSenData[1].SQAD_SHAREVALUE;
							NielSenAudiance = NielSenResponseObject.NielSenData[1].AUDIENCE;
						}
						else if(CuurentMin >= 30 && CuurentMin < 45 && NielSenResponseObject.NielSenData.length > 2)
						{
							NielSenADShare = NielSenResponseObject.NielSenData[2].SQAD_SHAREVALUE;
							NielSenAudiance = NielSenResponseObject.NielSenData[2].AUDIENCE;
						}
						else if(CuurentMin >= 45 && CuurentMin < 60 && NielSenResponseObject.NielSenData.length > 3)
						{
							NielSenADShare = NielSenResponseObject.NielSenData[3].SQAD_SHAREVALUE;
							NielSenAudiance = NielSenResponseObject.NielSenData[3].AUDIENCE;
						}
						if(NielSenADShare != null && NielSenADShare != undefined)
						{
							meta_mc.txtNeilSenAd.text ="iQ Adshare Value : "+ NielSenADShare;
						}
						else
						{
							meta_mc.txtNeilSenAd.text ="iQ Adshare Value : NA";
						}
						
						if(NielSenAudiance != null && NielSenAudiance != undefined)
						{
							meta_mc.txtNeilSenAud.text ="Nielsen Audience : "+ NielSenAudiance +"";
						}
						else
						{
							meta_mc.txtNeilSenAud.text ="Nielsen Audience : NA";
						}
					}
					else
					{
						meta_mc.txtNeilSenAd.text ="iQ Adshare Value : NA";
						meta_mc.txtNeilSenAud.text ="Nielsen Audience : NA";
					}
				}
			}
			catch(err:Error)
			{
			}
		}
		
		function parseNielSenData(e:Event):void
		{
			try
			{
				var Data = e.target.data;
				//var Data = "{\"NielSenData\":[{\"IQ_Start_Point\":1,\"Ratings_PT\":0.003432},{\"IQ_Start_Point\":2,\"Ratings_PT\":0.104324},{\"IQ_Start_Point\":3,\"Ratings_PT\":0.074553},{\"IQ_Start_Point\":4,\"Ratings_PT\":0.063567}],\"Status\":0}";
				NeilSenResponseRcvd = true;
				NielSenResponseObject = JSON.decode(Data,false);	
				var strHelper:StringHelper = new StringHelper();
				
				if(NielSenResponseObject.Status ==1)
				{
					meta_mc.txtNeilSenAd.text ="iQ Adshare Value : NA";
					meta_mc.txtNeilSenAud.text ="Nielsen Audience : NA";
				}
				if(IsRawMedia == false && NielSenResponseObject.Status ==0)
				{
					if(NielSenResponseObject.NielSenData != null && NielSenResponseObject.NielSenData.length > 0)
					{
						if(NielSenResponseObject.NielSenData[0].SQAD_SHAREVALUE != null)
						{
							meta_mc.txtNeilSenAd.text ="iQ Adshare Value : "+NielSenResponseObject.NielSenData[0].SQAD_SHAREVALUE;
						}
						else
						{
							meta_mc.txtNeilSenAd.text ="iQ Adshare Value : NA";
						}
						
						if(NielSenResponseObject.NielSenData[0].AUDIENCE != null)
						{
							meta_mc.txtNeilSenAud.text ="Nielsen Audience : "+ NielSenResponseObject.NielSenData[0].AUDIENCE + "";
						}
						else
						{
							meta_mc.txtNeilSenAud.text ="Nielsen Audience : NA";
						}
						
					}
					else
					{
						meta_mc.txtNeilSenAd.text ="iQ Adshare Value : NA";
						meta_mc.txtNeilSenAud.text ="Nielsen Audience : NA";
					}
				}
			}
			catch (err:Error)
			{
				
			}
		}
		
		private function NielSenDataServiceError(evt:IOErrorEvent):void
		{
			tracer("WARN","An error occurred in NielSenDataServiceError"+evt.text);
		}
		
		private function closeAbout()
		{
			if (about_scrn.visible == true)
			{
				FlagPopup = false;
				menuState="OPEN";
				menuSubState="CLOSED";
				var evt:Event = new Event(MouseEvent.CLICK, true, false);
				menubg.about_mc.dispatchEvent(evt);
				
			}
		}

		private function email(e:Event)
		{
			scaleUpitem(emailShare);
			emailShare.visible = true;
			closeAbout();
			FlagPopup = true;
			menuState="SHARING";
			menuSubState="EMAIL";
			//activeBTN(false);
			fadeOverlay(true);

			mcPlay.visible=false;
			mcPlay.enabled=false;

			emailShare.toemailTxt.text="";
			emailShare.msgTxt.text="";
			emailShare.statusTxt.text="";
			

			Tweener.addTween(menubg, {x: menuStopx, y: menuStopy, time: .3, transition: "linear"});
			//Tweener.addTween(emailShare, {x: emailStartx, y: emailStarty, time: .3, transition: "linear"});
			Tweener.addTween(emailShare, {x: 0, y: 0, time: .35, transition: "linear"});
		}

		private function link(e:Event)
		{
			scaleUpitem(embedVideo);
			embedVideo.visible = true;
			assignEmbed();
			closeAbout();
			menuState="SHARING";
			menuSubState="EMBED";
			//activeBTN(false);
			fadeOverlay(true);
			FlagPopup = true;
			mcPlay.visible=false;
			mcPlay.enabled=false;
			Tweener.addTween(menubg, {x: menuStopx, y: menuStopy, time: .3, transition: "linear"});
			//Tweener.addTween(embedVideo, {x: embedStartx, y: embedStarty, time: .3, transition: "linear"});
			Tweener.addTween(embedVideo, {x: 0, y: 0, time: .3, transition: "linear"});

			try
			{
				embedVideo.embedTxt.stage.focus=embedVideo.embedTxt;
				embedVideo.embedTxt.setSelection(0, embedVideo.embedTxt.text.length);
			}
			catch (err:Error)
			{
			}
		}		
		
		private function off1 (e:Event)
		{
			e.target.filters = null;
			menubg.about_mc.functionTxt.text="";
			menubg.embed_mc.functionTxt.text="";
			menubg.email_mc.functionTxt.text="";
			menubg.link_mc.functionTxt.text="";
			menubg.clip_mc.functionTxt.text="";
			
		}
		
		function ovr1 (e:Event)
		{
			applyGlow (e.target);
			menubg.about_mc.functionTxt.text="About";
		}
		function ovr2 (e:Event)
		{
			applyGlow (e.target);
			menubg.embed_mc.functionTxt.text="Link";
		}
		function ovr3 (e:Event)
		{
			applyGlow (e.target);
			menubg.email_mc.functionTxt.text="Email";
		}
		function ovr4 (e:Event)
		{
			applyGlow (e.target);
			menubg.link_mc.functionTxt.text="Embed Clip";
		}
		function ovr5 (e:Event)
		{
			applyGlow (e.target);
			menubg.clip_mc.functionTxt.text="Make a Clip";
		}

		private function setupMenuButtonListeners():void
		{
			
			menubg.clip_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr5);
			menubg.clip_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
			
			// Add event listner removed on 28/05/2013 
			// this event listner will be added in ParseStationSharingInfo					
			
			
			menubg.embed_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr2);
			menubg.embed_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
			menubg.embed_mc.addEventListener (MouseEvent.CLICK,bookmark);
			
			
			if(EB == "false")
			{
				menubg.email_mc.removeEventListener (MouseEvent.CLICK,email);
				menubg.email_mc.removeEventListener (MouseEvent.MOUSE_OVER,ovr3);
				menubg.email_mc.removeEventListener (MouseEvent.MOUSE_OUT,off1);
			}
			else
			{
				menubg.email_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr3);
				menubg.email_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
				menubg.email_mc.addEventListener (MouseEvent.CLICK,email);
				menubg.email_mc.buttonMode=true;
			}			
			
			menubg.link_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr4);
			menubg.link_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
			menubg.link_mc.addEventListener (MouseEvent.CLICK,link);
			
			menubg.about_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr1);
			menubg.about_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
			menubg.about_mc.addEventListener (MouseEvent.CLICK,about);
			menubg.embed_mc.buttonMode=true;
			menubg.embed_mc.useHandCursor = true;
			menubg.link_mc.buttonMode=true;
			menubg.about_mc.buttonMode=true;
			
			//this will be visible false by default , these 3 buttons will be visible in ParseStationSharingInfo()
			menubg.embed_mc.visible = false;
			menubg.email_mc.visible = false;
			menubg.link_mc.visible = false;
		}

		//*************** Javascript Integration Functions ******************
		public function setSeekPoint(num:Number)
		{
			if(ForwardFlag || RewindFlag)
			{
				popupDisplay("Action can't be performed \nas fast forward/ rewind is running", 500, 2500);
			}
			else
			{
				if(IsRawMedia.toLocaleUpperCase()=="TRUE")
				{
				
					if (num < globalStart && num<0)
					{
						num=globalStart;
					}
					
					// changed by meghana on 26-June-2012 removed 
					// num>3599 Condition as now our etime can be any
					if (num > globalStop)
					{
						num=globalStop;
					}
				}
				else
				{
						if (num < globalStart)
						{
							num=globalStart;
						}
			
						if (num > globalStop)
						{
							num=globalStop;
						}
				}
	
				// handle enabling playback and then seeking if not already started
				if(IsRawMedia.toLocaleUpperCase()!="TRUE")
				{
					if (!myVid.playing)
					{
						seekAndStart=true;
						userClicked=true;
						seekStartPoint=num;
						activeBTN(true);
						loader_mc.gotoAndPlay(2);
						thumbnailUnavailable.visible=false;
						mcPlay.removeEventListener(MouseEvent.CLICK, startUp);
						GetVarInfo();
						//txt_Msg.text=txt_Msg.text+"this";
					}
				}
				
				//tracer("INFO", "setSeekPoint: " + num);
				
				//HACK FOR OFFSETS		
				if(IsRawMedia.toLocaleUpperCase()=="TRUE")
				{
					var seekPoint:int;
					seekPoint=num;
					
					// changed by meghana on 26-June-2012 removed 
					// num>3599 Condition as now our etime can be any
					if(num >= globalStop) seekPoint = globalStop-300;
					
					if(seekPoint < globalStart && seekPoint<0) seekPoint = globalStart;
					
					globalStart=num;
					var st = 0;
					
					var et=(globalStart+globalStop);
					/* chage for custom upload */
					var sd = 0;
					var durr = null;
					
					if (st < globalStart && st<0)
					{
						st = globalStart;
						//HACK FOR CLIPS				
						sd = Math.abs (seekPoint - 600 - globalStart);
					}
					
					// changed by meghana on 26-June-2012 
					// commented below if condition as now our etime can be any
					/*if (st >= 3599)
					{
						durr = null;
						et = null;
					}
					else*/
					{
						/* working code et = st + 3599;*/
						et = st + globalStop;
						
						//txt_Msg.text=txt_Msg.text+"Inside else";
						
						// changed by meghana on 26-June-2012 removed 
						// et>3599 Condition as now our etime can be any
						if (et > globalStop)
						{
							et = globalStop;
							
							//txt_Msg.text=txt_Msg.text+"Inside else if";
							/*working code et=3599;*/
						}
						durr = et - st;
					}
				
					//HACK FOR CLIPS
					var time = sd;
					
					timeDisplayTot.text=timeCode(et);
					timeDisplayCur.text=timeCode(st);
					
					time = seekPoint-globalStart;			
					ftime=et-st;
					globalStart=st;
					globalStop=et;
				}
				
				offsetTime=globalStart;
				dispCap.text="";
				mcPlay.visible=false;
				mcPlay.enabled=false;
				
				mc_pp.gotoAndStop(3);
				myVid.pause();
				myVid.seek(num);						
	
				try
				{
					myVid.play();
				}
				catch (e:VideoError)
				{
					tracer("WARN", "ERR5 ERROR: " + e.code + " :: " + e);
				}
				
				closeMenu();
				
				if(IsRawMedia.toLocaleUpperCase() == "TRUE")
				{
					StartCustomSeek = true;
					myVid.addEventListener(VideoEvent.SEEKED,OnSeeked);
					trace(".........FF/Rew Disabled setSeekPoint........");
					EnableForwardRewind(false);
				}
				
				var evt:Event = new Event(MouseEvent.CLICK, true, false);
				clipperUI.cancel_mc.dispatchEvent(evt);
			}
			
			
			
		}

		private function setupExternalInterfaceCalls():void {
			if (ExternalInterface.available) {
				try {
					//JS Function to move the playhead...
					ExternalInterface.addCallback("setSeekPoint", setSeekPoint);
					
					//JS Function to Play/Pause Playback
					ExternalInterface.addCallback("externalPlay", externalPlay);
				}
				catch (err:Error) {
					tracer("WARN", "ERR6 ERROR: " + err.code + " :: " + err.message);
				}
			}
		}

		public function externalPlay(arg:String):void
		{
			var evt:Event = new Event(MouseEvent.CLICK, true, false);

			if (arg == "PLAY" && myVid.paused)
			{
				playBtn(evt);
			}

			if (arg == "PAUSE" && myVid.playing)
			{
				playBtn(evt);
			}
		}

		private function lockUI(bool:Boolean):void
		{
			mc_pp.gotoAndStop(1);
			
			if(IsRawMedia.toLocaleUpperCase() == "TRUE")
			{
				trace(".........FF/Rew Disabled lockUI........");
				EnableForwardRewind(false);
			}
			
			mc_pp.static_mc.enabled=bool;
			mutebutt_mc.enabled=bool;
			volbutt_mc.enabled=bool;
			menu_mc.enabled=bool;

			if (widgetMode == "TRUE")
			{
			}
			else
			{
				fullScreen.enabled=bool;
			}
			CC_btn.enabled=bool;
			meta_mc.toggle_mc.enabled=bool;
		}


		private function activeBTN(att:Boolean)
		{
			if (att == false)
			{
				mc_pp.removeEventListener(MouseEvent.CLICK, playBtn);
				
				//fullScreen.removeEventListener (MouseEvent.CLICK, ToggleFullScreen);
				
				
				volbutt_mc.removeEventListener(MouseEvent.CLICK, VolMove);
				mutebutt_mc.removeEventListener(MouseEvent.CLICK, VolMove);
				bKnob.removeEventListener(MouseEvent.MOUSE_OVER, bknob_over);
				bKnob.removeEventListener(MouseEvent.MOUSE_OUT, bknob_out);
				bKnob.removeEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
				bKnob.removeEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
				mcPlay.removeEventListener(MouseEvent.MOUSE_DOWN, playClip);
				mcPlay.removeEventListener(MouseEvent.MOUSE_DOWN, startUp);
					//menu_mc.removeEventListener(MouseEvent.CLICK)
			}

			if (att == true)
			{
				mc_pp.addEventListener(MouseEvent.CLICK, playBtn);
				
				//fullScreen.addEventListener (MouseEvent.CLICK, ToggleFullScreen);
				volbutt_mc.addEventListener(MouseEvent.CLICK, VolMove);
				mutebutt_mc.addEventListener(MouseEvent.CLICK, VolMove);
				bKnob.addEventListener(MouseEvent.MOUSE_OVER, bknob_over);
				bKnob.addEventListener(MouseEvent.MOUSE_OUT, bknob_out);
				bKnob.addEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
				bKnob.addEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
				mcPlay.addEventListener(MouseEvent.MOUSE_DOWN, playClip);
				mcPlay.addEventListener(MouseEvent.MOUSE_DOWN, startUp);
			}
		}

		//listen for fullscreen events
		private function fireSize(e:Event):void
		{
			if (stage.displayState == StageDisplayState.FULL_SCREEN)
			{
				SizeItems();
			}
		}

		private function resizeListener(e:Event):void
		{
			// tracer ("INFO","resizeListener: stageWidth: " + stage.stageWidth + " stageHeight: " + stage.stageHeight);
			// Code to handle sizing differences when embedded as a widget in Flex/AS3 as oppoed to straight

			var bigScreen:Boolean;

			switch (stage.displayState)
			{
				case "normal":
					bigScreen=false;
					break;
				case "fullScreen":
					bigScreen=true;
					break;
			}

			if (stage.displayState == StageDisplayState.NORMAL)
			{
				SizeItems();
			}			
		}


		public function kickPreview():void
		{
			// tracer ("INFO","IMAGE PREVIEW URL: "+imgPreviewURL);
			loaderContext.checkPolicyFile=true;
			
			try
			{
				imgPreviewURL=srvString + "/svc/clip/previewImage?eid=" + initClip;
				prevLoader.load(new URLRequest(imgPreviewURL), loaderContext);
			}
			catch (err:Error)
			{
				tracer("WARN", "ERR7 ERROR preLoader.load: " + err);
			}
			
			prevLoader.contentLoaderInfo.addEventListener(Event.COMPLETE, onLoadPreviewComplete);
			prevLoader.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR, onErrorPreview);
			prevLoader.contentLoaderInfo.addEventListener(HTTPStatusEvent.HTTP_STATUS, prevLoader_httpStatusHandler);

			activeBTN(false);
		}
		
		private function onLoadPreviewComplete(event:Event):void
		{
			previewLoaded=true;
			var info:LoaderInfo = LoaderInfo(prevLoader.contentLoaderInfo);

			// tracer ("INFO","Image Preview url=" + info.url);
			if (widgetMode != "TRUE")
			{
				sw=stage.stageWidth;
				sh=stage.stageHeight;
			}
			var vidScale = Math.min(sw / prevLoader.width, (sh - 28) / prevLoader.height);
			var newW = vidScale * prevLoader.width;
			var newH = vidScale * prevLoader.height;
			prevLoader.width=newW;
			prevLoader.height=newH;
			thumbnailUnavailable.width=newW;
			thumbnailUnavailable.height=newH;

			try
			{
				if (prevLoader.content is Bitmap)
				{
					Bitmap(prevLoader.content).smoothing=true;
				}
			}
			catch (err:Error)
			{

			}

			try
			{
				removeChild(prevLoader);
			}
			catch (err:Error)
			{
				tracer("WARN", "ERR14 ERROR: " + err);
			}

			addChild(prevLoader);
			prevLoader.alpha=0;

			prevLoader.x=Math.round(sw / 2 - prevLoader.width / 2);
			prevLoader.y=Math.round((sh - 28) / 2 - prevLoader.height / 2);

			Tweener.addTween(prevLoader, {alpha: 1, time: .5, transition: "linear"});

			mcPlay.addEventListener(MouseEvent.CLICK, startUp);
			mc_pp.addEventListener(MouseEvent.CLICK, startUp);
			mc_pp.gotoAndStop(2);
			setChildIndex(mcPlay, numChildren - 1);

			mcPlay.visible=true;
			//mcPlay.alpha = .85;
			loader_mc.gotoAndStop(1);
			loader_mc.visible=false;
			setChildIndex(prevLoader, 0);
		}

		private function prevLoader_httpStatusHandler(evt:HTTPStatusEvent):void
		{
			tracer("INFO", "Std Preview image HTTP status: " + evt.status);

			if (evt.status != 200)
			{
				onErrorPreviewSub();
			}
		}

		private function onErrorPreviewSub():void
		{
			tracer("INFO", "Error Loading Preview Image");

			mcPlay.addEventListener(MouseEvent.CLICK, startUp);
			mc_pp.gotoAndStop(2);
			mc_pp.addEventListener(MouseEvent.CLICK, startUp);
			setChildIndex(mcPlay, numChildren - 1);
			mcPlay.visible=true;
			//mcPlay.alpha = .85;
			loader_mc.gotoAndStop(1);
			loader_mc.visible=false;
			thumbnailUnavailable.dispText.text="Preview Image Temporarily Unavailable";
			thumbnailUnavailable.visible=true;
		}

		private function onErrorPreview(evt:Event):void
		{
			onErrorPreviewSub();
		}

		private function onErrorAudioPreview(evt:Event):void
		{
			
			tracer("INFO", "onErrorAudioPreview");
			// revisit and handle error with more user feedback at later date
			// onErrorPreviewSub ();
		}

		private function prevLoaderAudio_httpStatusHandler(evt:HTTPStatusEvent):void
		{
			tracer("INFO", "Audio Preview image HTTP status: " + evt.status);

			if (evt.status != 200)
			{
				// revisit and handle error with more user feedback at later date
				// onErrorPreviewSub ();
			}
		}


		private function onLoadAudioPreviewComplete(event:Event):void
		{
			previewLoaded=true;
			var info:LoaderInfo = LoaderInfo(prevLoader.contentLoaderInfo);

			if (widgetMode != "TRUE")
			{
				sw=stage.stageWidth;
				sh=stage.stageHeight;
			}

			var vidScale = Math.min(sw / prevLoader.width, (sh - 28) / prevLoader.height);
			var newW = vidScale * prevLoader.width;
			var newH = vidScale * prevLoader.height;
			prevLoader.width=newW;
			prevLoader.height=newH;
			thumbnailUnavailable.width=newW;
			thumbnailUnavailable.height=newH;

			if (prevLoader.content is Bitmap)
			{
				Bitmap(prevLoader.content).smoothing=true;
			}

			try
			{
				removeChild(prevLoader);
			}
			catch (err:Error)
			{
				tracer("WARN", "ERR14 ERROR: " + err);
			}

			addChild(prevLoader);

			setChildIndex(prevLoader, 1); // was 2
			
			prevLoader.alpha=1;

			myVid.alpha=0;
			previewPersistent=true;

			prevLoader.x=Math.round(sw / 2 - prevLoader.width / 2);

			loader_mc.gotoAndStop(1);
			loader_mc.visible=false;
		}

		private function startUp(evt:MouseEvent):void
		{						
			tracer("INFO", "STARTUP");
			if(IsRawMedia.toLocaleUpperCase() =="TRUE" && (ForwardFlag || RewindFlag))
			{
				StopForwardRewind();
			}
			if (widgetMode == "TRUE" && willAutoPlay == "TRUE")
			{
				// nada
			}
			else
			{
				// rhall turned this off for onbreak
				// willAutoPlay = "TRUE";
				tracer("INFO", "STARTUP PLAY CLICKED");
				dispatchEvent(new Event("RLPlayer_PLAY_CLICKED", true, false));
				userClicked=true;
				loader_mc.visible=true;
					loader_mc.gotoAndPlay(2);
				thumbnailUnavailable.visible=false;
				mcPlay.visible=false;
				mcPlay.removeEventListener(MouseEvent.CLICK, startUp);
				mc_pp.removeEventListener(MouseEvent.CLICK, startUp);
				activeBTN(true);
				GetVarInfo();
			}
		}	
		
		private function about(e:Event)
		{
			if (about_scrn.visible == false)
			{
				//about_scrn.visible = true;
				
				menuState="OPEN";
				menuSubState="ABOUT";
				fadeOverlay(true);
				mcPlay.visible=false;
				about_scrn.visible=true;
				FlagPopup = true;
				Tweener.addTween(about_scrn, {rotationY: 360, time: 1, transition: "easeInOutQuint"});
			}
			else
			{
				fadeOverlay(false);

				FlagPopup = false;
				if (playToggle)
				{
					mcPlay.visible=true;
					mcPlay.enabled=true;
				}
				else
				{
					mcPlay.visible=false;
					mcPlay.enabled=false;

				}
				about_scrn.visible=false;
				about_scrn.rotationY=0;
			}
		}
		
		public static function htmlEncode($str:String):String {
			var regExp:RegExp;
			
			// ampersand (Entity Number: &#38;)
			regExp = /&/g;
			$str = $str.replace(regExp, "&amp;");
			
			// double quotation mark (Entity Number: &#34;)
			regExp = /"/g;
			$str = $str.replace(regExp, "&quot;");
			
			// apostrophe (Entity Number: &#39;)
			regExp = /'/g;
			$str = $str.replace(regExp, "&apos;");
			
			// less-than sign (Entity Number: &#60;)
			regExp = /</g;
			$str = $str.replace(regExp, "&lt;");
			
			// greater-than sign (Entity Number: &#62;)
			regExp = />/g;
			$str = $str.replace(regExp, "&gt;");
			
			
			// cent sign (Entity Number: &#162;)
			regExp = /¢/g;
			$str = $str.replace(regExp, "&cent;");
			
			// pound sterling (Entity Number: &#163;)
			regExp = /£/g;
			$str = $str.replace(regExp, "&pound;");
			
			// general currency sign (Entity Number: &#164;)
			regExp = /¤/g;
			$str = $str.replace(regExp, "&curren;");
			
			// yen sign (Entity Number: &#165;)
			regExp = /¥/g;
			$str = $str.replace(regExp, "&yen;");
			
			// broken vertical bar (Entity Number: &#166;)
			regExp = /¦/g;
			$str = $str.replace(regExp, "&brvbar;");
			
			// section sign (Entity Number: &#167;)
			regExp = /§/g;
			$str = $str.replace(regExp, "&sect;");
			
			// umlaut (Entity Number: &#168;)
			regExp = /¨/g;
			$str = $str.replace(regExp, "&uml;");
			
			// copyright (Entity Number: &#169;)
			regExp = /©/g;
			$str = $str.replace(regExp, "&copy;");
			
			// feminine ordinal (Entity Number: &#170;)
			regExp = /ª/g;
			$str = $str.replace(regExp, "&ordf;");
			
			// left angle quote (Entity Number: &#171;)
			regExp = /«/g;
			$str = $str.replace(regExp, "&laquo;");
			
			// not sign (Entity Number: &#172;)
			regExp = /¬/g;
			$str = $str.replace(regExp, "&not;");
			
			// soft hyphen (Entity Number: &#173;)
			//regExp = //g;
			//$str = $str.replace(regExp, "&shy;");
			
			// registered trademark (Entity Number: &#174;)
			regExp = /®/g;
			$str = $str.replace(regExp, "&reg;");
			
			// macron accent (Entity Number: &#175;)
			regExp = /¯/g;
			$str = $str.replace(regExp, "&macr;");
			
			// degree sign (Entity Number: &#176;)
			regExp = /°/g;
			$str = $str.replace(regExp, "&deg;");
			
			// plus or minus (Entity Number: &#177;)
			regExp = /±/g;
			$str = $str.replace(regExp, "&plusmn;");
			
			// superscript two (Entity Number: &#178;)
			regExp = /²/g;
			$str = $str.replace(regExp, "&sup2;");
			
			// superscript three (Entity Number: &#179;)
			regExp = /³/g;
			$str = $str.replace(regExp, "&sup3;");
			
			// acute accent (Entity Number: &#180;)
			regExp = /´/g;
			$str = $str.replace(regExp, "&acute;");
			
			// micro sign (Entity Number: &#181;)
			regExp = /µ/g;
			$str = $str.replace(regExp, "&micro;");
			
			// paragraph sign (Entity Number: &#182;)
			regExp = /¶/g;
			$str = $str.replace(regExp, "&para;");
			
			// cedilla (Entity Number: &#184;)
			regExp = /¸/g;
			$str = $str.replace(regExp, "&cedil;");
			
			// superscript one (Entity Number: &#185;)
			regExp = /¹/g;
			$str = $str.replace(regExp, "&sup1;");
			
			// masculine ordinal (Entity Number: &#186;)
			regExp = /º/g;
			$str = $str.replace(regExp, "&ordm;");
			
			// right angle quote (Entity Number: &#187;)
			regExp = /»/g;
			$str = $str.replace(regExp, "&raquo;");
			
			// one-fourth (Entity Number: &#188;)
			regExp = /¼/g;
			$str = $str.replace(regExp, "&frac14;");
			
			// one-half (Entity Number: &#189;)
			regExp = /½/g;
			$str = $str.replace(regExp, "&frac12;");
			
			// three-fourths (Entity Number: &#190;)
			regExp = /¾/g;
			$str = $str.replace(regExp, "&frac34;");
			
			// inverted question mark (Entity Number: &#191;)
			regExp = /¿/g;
			$str = $str.replace(regExp, "&iquest;");
			
			// multiplication sign (Entity Number: &#215;)
			regExp = /×/g;
			$str = $str.replace(regExp, "&times;");
			
			// division sign (Entity Number: &#247;)
			regExp = /÷/g;
			$str = $str.replace(regExp, "&divide;");
			
			
			// for all (Entity Number: &#8704;)
			regExp = /∀/g;
			$str = $str.replace(regExp, "&forall;");
			
			2202
			// part (Entity Number: &#8706;)
			regExp = /∂/g;
			$str = $str.replace(regExp, "&part;");
			
			// exists (Entity Number: &#8707;)
			regExp = /∃/g;
			$str = $str.replace(regExp, "&exist;");
			
			// empty (Entity Number: &#8709;)
			regExp = /∅/g;
			$str = $str.replace(regExp, "&empty;");
			
			// nabla (Entity Number: &#8711;)
			regExp = /∇/g;
			$str = $str.replace(regExp, "&nabla;");
			
			// isin (Entity Number: &#8712;)
			regExp = /∈/g;
			$str = $str.replace(regExp, "&isin;");
			
			// notin (Entity Number: &#8713;)
			regExp = /∉/g;
			$str = $str.replace(regExp, "&notin;");
			
			// ni (Entity Number: &#8715;)
			regExp = /∋/g;
			$str = $str.replace(regExp, "&ni;");
			
			// prod (Entity Number: &#8719;)
			regExp = /∏/g;
			$str = $str.replace(regExp, "&prod;");
			
			// sum (Entity Number: &#8721;)
			regExp = /∑/g;
			$str = $str.replace(regExp, "&sum;");
			
			// minus (Entity Number: &#8722;)
			regExp = /−/g;
			$str = $str.replace(regExp, "&minus;");
			
			// lowast (Entity Number: &#8727;)
			regExp = /∗/g;
			$str = $str.replace(regExp, "&lowast;");
			
			// square root (Entity Number: &#8730;)
			regExp = /√/g;
			$str = $str.replace(regExp, "&radic;");
			
			// proportional to (Entity Number: &#8733;)
			regExp = /∝/g;
			$str = $str.replace(regExp, "&prop;");
			
			// infinity (Entity Number: &#8734;)
			regExp = /∞/g;
			$str = $str.replace(regExp, "&infin;");
			
			// angle (Entity Number: &#8736;)
			regExp = /∠/g;
			$str = $str.replace(regExp, "&ang;");
			
			// and (Entity Number: &#8743;)
			regExp = /∧/g;
			$str = $str.replace(regExp, "&and;");
			
			// or (Entity Number: &#8744;)
			regExp = /∨/g;
			$str = $str.replace(regExp, "&or;");
			
			// cap (Entity Number: &#8745;)
			regExp = /∩/g;
			$str = $str.replace(regExp, "&cap;");
			
			// cup (Entity Number: &#8746;)
			regExp = /∪/g;
			$str = $str.replace(regExp, "&cup;");
			
			// integral (Entity Number: &#8747;)
			regExp = /∫/g;
			$str = $str.replace(regExp, "&int;");
			
			// therefore (Entity Number: &#8756;)
			regExp = /∴/g;
			$str = $str.replace(regExp, "&there4;");
			
			// similar to (Entity Number: &#8764;)
			regExp = /∼/g;
			$str = $str.replace(regExp, "&sim;");
			
			// congruent to (Entity Number: &#8773;)
			regExp = /≅/g;
			$str = $str.replace(regExp, "&cong;");
			
			// almost equal (Entity Number: &#8776;)
			regExp = /≈/g;
			$str = $str.replace(regExp, "&asymp;");
			
			// not equal (Entity Number: &#8800;)
			regExp = /≠/g;
			$str = $str.replace(regExp, "&ne;");
			
			// equivalent (Entity Number: &#8801;)
			regExp = /≡/g;
			$str = $str.replace(regExp, "&equiv;");
			
			// less or equal (Entity Number: &#8804;)
			regExp = /≤/g;
			$str = $str.replace(regExp, "&le;");
			
			// greater or equal (Entity Number: &#8805;)
			regExp = /≥/g;
			$str = $str.replace(regExp, "&ge;");
			
			// subset of (Entity Number: &#8834;)
			regExp = /⊂/g;
			$str = $str.replace(regExp, "&sub;");
			
			// superset of (Entity Number: &#8835;)
			regExp = /⊃/g;
			$str = $str.replace(regExp, "&sup;");
			
			// not subset of (Entity Number: &#8836;)
			regExp = /⊄/g;
			$str = $str.replace(regExp, "&nsub;");
			
			// subset or equal (Entity Number: &#8838;)
			regExp = /⊆/g;
			$str = $str.replace(regExp, "&sube;");
			
			// superset or equal (Entity Number: &#8839;)
			regExp = /⊇/g;
			$str = $str.replace(regExp, "&supe;");
			
			// circled plus (Entity Number: &#8853;)
			regExp = /⊕/g;
			$str = $str.replace(regExp, "&oplus;");
			
			// cirled times (Entity Number: &#8855;)
			regExp = /⊗/g;
			$str = $str.replace(regExp, "&otimes;");
			
			// perpendicular (Entity Number: &#8869;)
			regExp = /⊥/g;
			$str = $str.replace(regExp, "&perp;");
			
			// dot operator (Entity Number: &#8901;)
			regExp = /⋅/g;
			$str = $str.replace(regExp, "&sdot;");

			// modifier letter circumflex accent (Entity Number: &#710;)
			regExp = /ˆ/g;
			$str = $str.replace(regExp, "&circ;");
			
			// small tilde (Entity Number: &#732;)
			regExp = /˜/g;
			$str = $str.replace(regExp, "&tilde;");
			
			// en space (Entity Number: &#8194;)
			regExp = / /g;
			$str = $str.replace(regExp, "&ensp;");
			
			// em space (Entity Number: &#8195;)
			regExp = / /g;
			$str = $str.replace(regExp, "&emsp;");
			
			// thin space (Entity Number: &#8201;)
			regExp = / /g;
			$str = $str.replace(regExp, "&thinsp;");
			
			// left-to-right mark (Entity Number: &#8206;)
			regExp = /	‎/g;
			$str = $str.replace(regExp, "&lrm;");
			
			// right-to-left mark (Entity Number: &#8207;)
			regExp = /‏‏	‏/g;
			$str = $str.replace(regExp, "&rlm;");
			
			// en dash (Entity Number: &#8211;)
			regExp = /–/g;
			$str = $str.replace(regExp, "&ndash;");
			
			// em dash (Entity Number: &#8212;)
			regExp = /—/g;
			$str = $str.replace(regExp, "&mdash;");

			// left single quote (Entity Number: &#8216;)
			regExp = /‘/g;
			$str = $str.replace(regExp, "&lsquo;");
			
			// right single quote (Entity Number: &#8217;)
			regExp = /’/g;
			$str = $str.replace(regExp, "&rsquo;");
			
			// single low-9 quote (Entity Number: &#8218;)
			regExp = /‚/g;
			$str = $str.replace(regExp, "&sbquo;");
			
			// left double quote (Entity Number: &#8220;)
			regExp = /“/g;
			$str = $str.replace(regExp, "&ldquo;");
			
			// right double quote (Entity Number: &#8221;)
			regExp = /”/g;
			$str = $str.replace(regExp, "&rdquo;");
			
			// double low-9 quote (Entity Number: &#8222;)
			regExp = /„/g;
			$str = $str.replace(regExp, "&bdquo;");
			
			// minutes (Entity Number: &#8242;)
			regExp = /′/g;
			$str = $str.replace(regExp, "&prime;");

			// seconds (Entity Number: &#8243;)
			regExp = /″/g;
			$str = $str.replace(regExp, "&Prime;");

			
			///////////////////////////////////////////////////////////////////////
			//  Other Named Entities
			///////////////////////////////////////////////////////////////////////
			
			// slash (Entity Number: &#47;)
			regExp = /\//g;
			$str = $str.replace(regExp, "&frasl;");
			
			///////////////////////////////////////////////////////////////////////

			return $str;
		}
		
		//Get station Sharing Info
		private function GetStationSharingInfo():void
		{
			try
			{				
				//getPlayerInfoInt=setTimeout(ParsePlayerInfoTimeout, serviceTimeout);
				tracer("DEBUG", "GetStationSharingInfo CALLED");
				var xmlURLReq:URLRequest ;
				
				var xmlSendLoad:URLLoader = new URLLoader();			
				xmlSendLoad.addEventListener(IOErrorEvent.IO_ERROR, onIOError, false, 0, true);
				
				xmlURLReq= new URLRequest();
				
				tracer("DEBUG", "GetStationSharingInfo IQSVC URL " + srvString+ "/iqsvc/GetStationSharing?clipID=" +  guid + "&Format=xml");
				
				xmlURLReq.url=srvString+ "/iqsvc/GetStationSharing?clipID=" +  guid + "&Format=xml";
				//xmlURLReq.url=srvString+"/iqsvc/Statskedprog/GetPlayerData?ID="+guid+"&Type="+MediaType;
					
				xmlSendLoad.addEventListener(Event.COMPLETE, ParseStationSharingInfo, false, 0, true);
				
				xmlURLReq.contentType="text/xml";
				xmlURLReq.method=URLRequestMethod.GET;			
				try
				{
					xmlSendLoad.load(xmlURLReq);
				}
				catch (err:Error)
				{
					//clearTimeout(getPlayerInfoInt);
					tracer("WARN", "Error GetStationSharingInfo load:" + err.message);
					GetEmailSharingInfo();
				}
			}
			catch(err:Error)
			{
				//clearTimeout(getPlayerInfoInt);
				tracer("WARN", "Error GetStationSharingInfo load:" + err.message);		
				GetEmailSharingInfo();
			}
		}
		
		private function ParseStationSharingInfo(e:Event):void
		{
			try
			{
				GetEmailSharingInfo();
				//clearTimeout(getPlayerInfoInt);
				tracer("DEBUG", "ParseStationSharingInfo CALLED");
			
				var ResultXml:XML=new XML(e.target.data);
				tracer("DEBUG", "ParseStationSharingInfo Status :" + ResultXml.Status);
				tracer("DEBUG", "ParseStationSharingInfo isSharing :" + ResultXml.IsSharing);
				if(ResultXml.Status == 0)
				{			
				if(ResultXml.IsSharing.toUpperCase() == "TRUE" )				
					{		
					menubg.embed_mc.visible = true;
					menubg.email_mc.visible = true;
					menubg.link_mc.visible = true;
					
					/*menubg.link_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr4);
					menubg.link_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
					menubg.link_mc.addEventListener (MouseEvent.CLICK,link);


					menubg.email_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr3);
					menubg.email_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
					menubg.email_mc.addEventListener (MouseEvent.CLICK,email);
					menubg.email_mc.buttonMode=true;


					menubg.embed_mc.addEventListener (MouseEvent.MOUSE_OVER,ovr2);
					menubg.embed_mc.addEventListener (MouseEvent.MOUSE_OUT,off1);
					menubg.embed_mc.addEventListener (MouseEvent.CLICK,bookmark);

					menubg.link_mc.transform.colorTransform = new ColorTransform();
					menubg.email_mc.transform.colorTransform = new ColorTransform();
					menubg.embed_mc.transform.colorTransform = new ColorTransform();
					
					menubg.link_mc.buttonMode = true;*/
					}
					else
					{
						menubg.about_mc.x = 407.15 - 140;	
					}
					/*if(ResultXml.IsSharing.toUpperCase() == "FALSE" )				
					{		
						tracer("DEBUG", "ParseStationSharingInfo into IsSharing False");
						
						menubg.embed_mc.removeEventListener (MouseEvent.CLICK,bookmark);
						menubg.embed_mc.addEventListener(MouseEvent.MOUSE_UP, embedmcClick);						
						
						
						menubg.email_mc.removeEventListener (MouseEvent.CLICK,email);
						menubg.email_mc.addEventListener(MouseEvent.MOUSE_UP, emailmcClick);
						
					
						menubg.link_mc.removeEventListener (MouseEvent.CLICK,link);
						menubg.link_mc.addEventListener(MouseEvent.MOUSE_UP, linkmcClick);
						
						var color:ColorTransform = new ColorTransform();
						color.color  = menubg.embed_mc.bri - 10;
						var colorObj:Color = new Color (); 
						colorObj.brightness = -.50;
						menubg.embed_mc.transform.colorTransform = colorObj;
						menubg.email_mc.transform.colorTransform = colorObj;
						menubg.link_mc.transform.colorTransform = colorObj;
						
					}*/
				}
				else
				{
					menubg.about_mc.x = 407.15 - 140;	
				}
			}
			catch(err:Error)
			{
				//clearTimeout(getPlayerInfoInt);
				tracer("WARN", "Error GetStationSharingInfo load:" + err.message);		
			}			
		}
		
		//Get station Sharing Info
		private function GetEmailSharingInfo():void
		{
			try
			{				
				//getPlayerInfoInt=setTimeout(ParsePlayerInfoTimeout, serviceTimeout);
				tracer("DEBUG", "GetEmailSharingInfo CALLED");
				var xmlURLReq:URLRequest ;
				
				var xmlSendLoad:URLLoader = new URLLoader();			
				xmlSendLoad.addEventListener(IOErrorEvent.IO_ERROR, onIOError, false, 0, true);
				
				xmlURLReq= new URLRequest();
				
				tracer("DEBUG", "GetEmailSharingInfo IQSVC URL " + srvString+ "/iqsvc/GetEmailSharing?Format=xml");
				
				xmlURLReq.url=srvString+ "/iqsvc/GetEmailSharing?Format=xml";
					
				xmlSendLoad.addEventListener(Event.COMPLETE, ParseEmailSharingInfo, false, 0, true);
				
				xmlURLReq.contentType="text/xml";
				xmlURLReq.method=URLRequestMethod.GET;			
				try
				{
					xmlSendLoad.load(xmlURLReq);
				}
				catch (err:Error)
				{
					//clearTimeout(getPlayerInfoInt);
					tracer("WARN", "Error GetEmailSharingInfo load:" + err.message);
				}
			}
			catch(err:Error)
			{
				//clearTimeout(getPlayerInfoInt);
				tracer("WARN", "Error GetEmailSharingInfo load:" + err.message);		
			}
		}
		private function ParseEmailSharingInfo(e:Event):void
		{
			try
			{
				tracer("DEBUG", "ParseEmailSharingInfo CALLED");
			
				var ResultXml:XML=new XML(e.target.data);
				tracer("DEBUG", "ParseEmailSharingInfo Status :" + ResultXml.Status);
				tracer("DEBUG", "ParseEmailSharingInfo isSharing :" + ResultXml.IsEmailSharing);
				
				if(ResultXml.Status == 0)
				{			
					if(ResultXml.IsEmailSharing.toUpperCase() == "TRUE" )				
					{	
						menubg.email_mc.visible = true;
						if(menubg.embed_mc.visible == false && menubg.link_mc.visible == false)
						{
							menubg.email_mc.x = 80.5 + 140;					
							menubg.about_mc.x = 459.15 - 140;	
						}
					}
				}
			}
			catch(err:Error)
			{
				//clearTimeout(getPlayerInfoInt);
				tracer("WARN", "Error GetStationSharingInfo load:" + err.message);		
			}
		}
	}
}