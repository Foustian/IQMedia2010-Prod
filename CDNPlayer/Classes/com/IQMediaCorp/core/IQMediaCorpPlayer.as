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
	import alducente.services.WebService;
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
	
	public class IQMediaCorpPlayer extends MovieClip
	{
		private var myVid:FLVPlayback = new FLVPlayback();

		private var stats:Stats = new Stats({bg: 0x66000000});
		private var customSeekPointRight:Number;
		
		private var guid:String;
		private var offsetTime:Number=0;
		private var thevidplays:int = 0;
		private var customSeekPoint:Number;
		private var initClip:String;
		
		private var LogPlayURLDynamic:String;
		
		private var IsRawMedia:String;

		private var willAutoPlay:String;
		
		private var IsLogPlay:String;

		private var outgoing_lc:LocalConnection = new LocalConnection();
		
		private var outStr:String;
	
		private var jsDEBUG:Boolean = false;
		
		private var isCustomSeek:Boolean=false;
		
		private var ws:WebService = new WebService();
		
		private static const versionBuild:String = "2.0";
		
		private static const menuItemLabel1:String = "IQMedia Video Player";
		
		private static const menuItemLabel2:String = "Version: " + versionBuild + "";
		
		private static const serviceTimeout:Number = 45000;

		//********** Declare XML Variables
		

		private var vidname:String;

		private var hasCaption:String;

		private var globalStart:Number;

		private var globalStop:Number;

		private var _fileId:String;
		
		private var embedStopx:Number;

		private var embedStopy:Number;

		private var embedStartx:Number;

		private var embedStarty:Number;
		
		private var clipName:String;

		private var srvString:String;
	
		private var verPP:String;
		
		private var ServicesBaseURL:String;
		
		private var PlayerFromLocal:String;
		
		private var ClipLength:Number;
		
		private var embedUrl:String;

		private var rid:String;

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

		private var mcStopy:Number;
		
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

		private var firstTimeLoadClip:Boolean = true;

		private var dataLoaded:Boolean = false;

		private var audioGraphic:String;

		private var hasVideo:String;

		private var getVarsInt:uint;
		
		
		
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
		
		private var IsFirstLK:Boolean=false;
		
		private var CCUnavailable:String="Closedcaption temporary unavailable";
		
		private var listCCBegin:Vector.<int>;
		
		private var listCCEnd:Vector.<int>;
		
		private var listCCText:Vector.<String>;
		
		private var CurrentCCPosition:int=2;
		
		private var PreviousSecond:int=-1;
		
		private var PlayerDataKeyWord:String ="";
		private var PlayerDataDescription:String = "";

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
		
		
		public function oneTimeInit(event:Event=null):void
		{						
			//trace("Doamin :"+ExternalInterface.call("window.location.href.toString"));
			trace("Domain :"+this.parent.parent);
			
			trace("RLPLAYER ADDED TO STAGE!");
			removeEventListener(Event.ADDED_TO_STAGE, oneTimeInit);

			stage.align=StageAlign.TOP_LEFT;
			this.opaqueBackground=0x000000;
			

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
			//initClip="e5e4c732-3f26-4b13-9809-eb10369df15d"; /* Clip */
			//initClip = "9bd9a061-a509-48e7-821d-f78646bb78f0"; /* RawMedia*/
			//initClip ="7082867b-8ff9-4e8b-95bb-dd40ccdb7f14"; /* Radio */
			//initClip="5959f8be-b311-456c-853e-231b3c05452c"; /* UGC-Upload */
			
			
			//commented on 9 Feb 2012 by meghana 
			//IsRawMedia=String(LoaderInfo(this.root.loaderInfo).parameters.IsRawMedia);
			IsRawMedia = "FALSE";
			
			//commented on 9 Feb 2012 by meghana 
			//ServicesBaseURL = String(LoaderInfo(this.root.loaderInfo).parameters.ServicesBaseURL);
			ServicesBaseURL = "2";
			
			//commented on 9 Feb 2012 by meghana 
			//PlayerFromLocal = String(LoaderInfo(this.root.loaderInfo).parameters.PlayerFromLocal);
			PlayerFromLocal = "false";
			
			//commented on 9 Feb 2012 by meghana 
			//ClipLength = Number(LoaderInfo(this.root.loaderInfo).parameters.ClipLength);
			//ClipLength = 1800;
			
			//commented on 9 Feb 2012 by meghana 
			//initialOffset = int(LoaderInfo(this.root.loaderInfo).parameters.Offset);
			
			if(ServicesBaseURL == "1")
			{
				srvString = "http://qaservices.iqmediacorp.com";
				verPP = "D";
			}
			else
			{
				srvString = "http://services.iqmediacorp.com";
				verPP = "P";
			}
				
			IsLogPlay = "false";
			
			/*var myDate:Date = new Date();
			trace(myDate.toTimeString());*/
			
			
			//commented on 9 Feb 2012 by meghana 
			willAutoPlay = LoaderInfo(this.root.loaderInfo).parameters.autoPlayback == undefined ? "FALSE" : String(LoaderInfo(this.root.loaderInfo).parameters.autoPlayback).toUpperCase();
			//willAutoPlay = "FALSE";

             
			if (willAutoPlay == "TRUE")
			{
				userClicked=true;
			}	
			/*
			outgoing_lc.allowDomain('*');
			outgoing_lc.send("_log_output", "startLogging", "Connected to " + menuItemLabel1 + " [ " + menuItemLabel2 + " ]\n" + Capabilities.serverString + "\n" + Capabilities.version + "\n");
			outgoing_lc.addEventListener(StatusEvent.STATUS, logonStatus);
			*/
			//trace('new player...in else ...');
			//trace(this.parent.toString());
				
			//stage.scaleMode=StageScaleMode.NO_SCALE;
			
			
			//trace("sw:"+sw);
			//trace("sh:"+sh);
				
			
				
			
			// External Logging AIR application for troubleshooting
			// Download/install from: http://clients.impossibilities.com/IQMedia/2009/logger/
			
			//Filtering based on INFO, WARN, LOG, ERROR, FATAL to be added in the future for filtering in AIR Logger app
			tracer("INFO", "\n\n\n[______________________________________________________________]");
			tracer("INFO", "IQMedia Video Player :: " + menuItemLabel2);
			tracer("INFO", "Video Component :: " + FLVPlayback.VERSION);
			
			

			// setup additional GET params for service requests based in clip ID/GUID
			if (initClip != null)
			{
				guid=initClip;
			}

			//********** Setup Attributes
			mcPlay.alpha=.85;
			mcPlay.visible=false;
			overlay_mc.alpha=0;
			thumbnailUnavailable.visible=false;
			Scrub.progBar.width=0;

			loader_mc.buttonMode=false;
			loader_mc.mouseEnabled=false;

			myVid.bufferTime=3;
			myVid.fullScreenTakeOver=false;

			setUpVidListeners();
			moreListeners();
			setupMenuListeners();

			getVolume();

			setupExternalInterfaceCalls();

			stage.addEventListener(Event.RESIZE, resizeListener);
			stage.addEventListener(Event.FULLSCREEN, fireSize);

			

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
				
				
			}
			else
			{
				SetClientWaterMark();
				if (initClip != "null" && initClip != "undefined" && willAutoPlay == "FALSE")
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
			hidePopup(3);
			toggleSmoothing(vidSmoothing);
			
			embedVideo.visible = false;

			trace("RLPLAYER READY!");
			// Important event to dispatch for Flex apps/widgets/mediacenter apps so they know when things are ready
			var evt:Event = new Event("RLPlayer_READY", true, false);
			dispatchEvent(evt);
			try {
				//Notify the j/s that the player is ready. If the function doesn't exist... its all good...
				ExternalInterface.call("RLPlayer_READY");
			}
			catch(err:Error) { /* I don't care about the error... */ }			
			
			
			
			
			
			
			//menubg.visible = false;
			//about_scrn.visible = false;
			
		}	
		

		/**
		 * Handles output of logging via trace and localConnection to custom AIR app written to capture logs in realtime, as well as to JavaScript output if enabled.
		 * TODO: Add in additional verbosit for levels, LOG, DEBUG, INFO, WARN, ERROR, FATAL log levels with filtering support in AIR app with color coding
		 * External Logging AIR application for troubleshooting
		 * Download/install from: http://clients.impossibilities.com/IQMedia/2009/logger/
		 *
		 */
		 
		 function SetClientWaterMark()
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
				loaderContext.checkPolicyFile=true;
				var waterMarkLoader:Loader = new Loader(); 
   				var url :URLRequest = new URLRequest(jsonobj["Path"]);
   				waterMarkLoader.contentLoaderInfo.addEventListener(Event.COMPLETE, onImageLoaded);
   				waterMarkLoader.load(url,loaderContext);
			}
		}
		
		function onImageLoaded(e:Event):void 
		{
      		var image:Bitmap;
			image = new Bitmap(e.target.content.bitmapData);
      		
			client_logo.x = 20;
			client_logo.y = stage.stageHeight - 90;
			
			image.x = (client_logo.width - image.width) / 2;
			image.y = (client_logo.height -image.height) / 2;
			
			
      		client_logo.addChild(image);
   		}
		 
		 
		 /**
		 * Dummy function for handling events from localConnection based logging function "TRACER"
		 *
		 */
		private function logonStatus(event:StatusEvent):void
		{
			// catching the events to avoid errors - nothing needed here for time being - reserved for enhancements to AIR logger app
		}
		 
		private function tracer(typ, arg)
		{
			outStr="[ " + getTimer() + " : " + typ + " ] :: " + arg;
			//txtMsg.text += outStr+"\n";
			
			if (typ != "WARN")
			{
				trace(outStr);
				/*outgoing_lc.send("_log_output", "displayMsg", outStr);*/
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


		/**
		 * Prepare the custom right-click contextual menus, set up listeners
		 *
		 */
		private function setupContextMenu():void
		{
			//add the event listener that will respond to a user selecting a menu item

			conMenu.addMenuItem(menuItemLabel1, false);
			conMenu.addMenuItem(menuItemLabel2, false);
			conMenu.addMenuItem("Enter FullScreen", true);
			conMenu.addMenuItem("Exit FullScreen", false);
			//conMenu.addMenuItem("IQMedia Menu", true);
			conMenu.addMenuItem("MediaID:"+initClip, true); 

			conMenu.addMenuItem("Video Smoothing: ON", true);
			conMenu.addMenuItem("Video Smoothing: OFF", false);
			conMenu.addMenuItem("Toggle Memory Info", true);

			//disable the Exit FullScreen menu item on load
			conMenu.hideMenuItem("Exit FullScreen");
			
			//if (widgetMode == "TRUE")
			//{
				//conMenu.hideMenuItem("Enter FullScreen");
			//}

			//add event listener for stage to handle toggling of menus item display
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
					DOMinfo=jsResult.href + "|" + jsResult.documentReferer;
					
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
		

			sw=stage.stageWidth;
			sh=stage.stageHeight;
			
			wchange=sw / 545;
			hchange=sh / 340;
			
			trace("sw:"+sw);
			trace("sh:"+sh);
			
			tracer("INFO", "STANDARD EMBED SIZE: " + sw + "x" + sh);

			var stageBot:Number = sh - 27;


			if (myVid.metadataLoaded)
			{
				tracer ("INFO","VIDEO METADATA :: LOADED");
				vidScale=Math.min(sw / myVid.metadata.width, (sh - 27) / myVid.metadata.height);
				newW=vidScale * myVid.metadata.width;
				newH=vidScale * myVid.metadata.height;
				myVid.setSize(newW, newH);
			}
			else
			{
				tracer ("INFO","VIDEO METADATA :: NOT LOADED");
				vidWidth=myVid.getVideoPlayer(myVid.activeVideoPlayerIndex).width;
				vidHeight=myVid.getVideoPlayer(myVid.activeVideoPlayerIndex).height;
				vidScale=Math.min(sw / vidWidth, (sh - 27) / vidHeight);
				newW=vidScale * vidWidth;
				newH=vidScale * vidHeight;
				myVid.setSize(newW, newH);
			}
			myVid.x=(sw / 2) - (myVid.width / 2);
			myVid.y=(stageBot / 2) - (myVid.height / 2);
			
			popmask.width=sw;
			popmask.height=sh;
			popmask.x=0;
			popmask.y=0;
			
			overlay_mc.x=0;
			overlay_mc.y=0;
			overlay_mc.width=sw;
			overlay_mc.height=sh;


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

			botbar_mc.width=sw;
			botbar_mc.y=sh - botbar_mc.height;
			//myCap.height = sh*.18852


			if (varsLoaded)
			{
				/*meta_mc.meta1Txt.text=meta1_textString;*/
			}

			//mc_pp.x=5.5 * wchange;
			//mc_pp.y=botbar_mc.y + 4;
			
			//trace("mc_pp.x: "+mc_pp.x);
			//trace("mc_pp.y: "+mc_pp.y);
			

			/*loader_mc.x=Math.ceil(mc_pp.x + 10);*/
			loader_mc.x=mc_pp.x + 10;
			loader_mc.y=botbar_mc.y + 13.5;

			timeDisplayCur.x=mc_pp.x + 12;
			timeDisplayCur.y=botbar_mc.y + 7;



			//volbutt_mc.x=menu_mc.x - 23;
			volbutt_mc.y=botbar_mc.y + 7;
			embed_mc.y =botbar_mc.y + 4;
			
			//mutebutt_mc.x=menu_mc.x - 23;
			mutebutt_mc.y=botbar_mc.y + 7;

			//fullScreen.x=volbutt_mc.x - 28;
			//fullScreen.y=botbar_mc.y + 7;

			//timeDisplayTot.x=fullScreen.x - 40;
			timeDisplayTot.x=volbutt_mc.x - 40;
			timeDisplayTot.y=botbar_mc.y + 7;

			Scrub.x=timeDisplayCur.x + 53;
			Scrub.width=Math.round((timeDisplayTot.x - Scrub.x) - 7);
			Scrub.y=botbar_mc.y + 4;
			
			trace("Scrub.x: "+Scrub.x);
			trace("Scrub.width: "+Scrub.width);
			
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

			if (stage.displayState == StageDisplayState.FULL_SCREEN_INTERACTIVE)
			{
				mcPlay.y=this.parent.height / 2 - mcPlay.height / 2;
			}

			if (menuState == "OPEN")
			{
				showMenu();
			}

			

			embedVideo.height=sh * .61970;
			embedVideo.width=embedVideo.height * 1.8462;
			embedVideo.x=(((sw - embedVideo.width) / 2) + 5);
			embedVideo.y=Math.round(botbar_mc.y +(38 * wchange));
			
			embedStopx=embedVideo.x;
			embedStopy=embedVideo.y;
			embedStartx=embedVideo.x;
			embedStarty=40 * wchange;
			
			
			linkStarty=40 * wchange;

			

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
		}


		/**
		 * Handles setting up many of the UI control listeners and set initial state of UI elements
		 *
		 */
		private function moreListeners():void
		{
			//**************** Button Event Listeners
			
			embed_mc.addEventListener(MouseEvent.CLICK,bookmark);
			
			
			mcPlay.addEventListener(MouseEvent.MOUSE_DOWN, playClip);
			mc_pp.addEventListener(MouseEvent.CLICK, playBtn);
			//clipperUI.pp_mc.addEventListener(MouseEvent.CLICK, playBtn);

			volbutt_mc.addEventListener(MouseEvent.CLICK, VolMove);
			mutebutt_mc.addEventListener(MouseEvent.CLICK, VolMove);

			vol.vol_mc.volKnob.addEventListener(MouseEvent.MOUSE_DOWN, MoveVol);
			vol.vol_mc.volKnob.addEventListener(MouseEvent.MOUSE_UP, StopMoveVol);


			Scrub.scrubBar.mouseEnabled=false;
			Scrub.scrubBar.buttonMode=false;
			Scrub.progBar.enabled=false;
			Scrub.progBar.mouseEnabled=false;
			loader_mc.gotoAndPlay(2);
			bKnob.mouseEnabled=true;
			bKnob.buttonMode=true;
			tknob_mc.buttonMode=false;
			tknob_mc.mouseEnabled=false;

		}
		
		private function bookmark(e:Event)
		{
			if(!embedVideo.visible)
			{
				embedVideo.visible=true;
				assignEmbed();
				//closeAbout();
				trace("hello");
				//activeBTN(false);
				fadeOverlay(true);
	
				mcPlay.visible=false;
				mcPlay.enabled=false;
				Tweener.addTween(embedVideo, {x: embedStartx, y: embedStarty, time: .3, transition: "linear"});
				//Tweener.addTween(linkVideo, {x: linkStartx, y: linkStarty, time: .3, transition: "linear"});
	
				if (_fileId == null || _fileId == undefined || _fileId == "")
				{
					_fileId=initClip;
				}
				embedVideo.embedTxt.stage.focus=embedVideo.embedTxt;
				embedVideo.embedTxt.setSelection(0, embedVideo.embedTxt.length);
			}
			else
			{
				link_close(e);
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
			
			var swfUrl:String;
			var swfUrl:String=stage.loaderInfo.url;
			
			
			/*if(ServicesBaseURL=="1")
			{
				swfUrl = "http://" + vardomain + "/QA_iqmedia_player_resize_cdn.swf"
			}
			else
			{
				swfUrl = "http://" + vardomain + "/iqmedia_player_resize_cdn.swf"
			}*/
			
			//embedVideo.embedTxt.text="<object height=\"340\" width=\"545\" data=\"" + swfUrl + "\" type=\"application/x-shockwave-flash\"  name=\"HYETA\" id=\"HUY\"><param value=\""+ swfUrl + "\" name=\"movie\"><param value=\"true\" name=\"allowfullscreen\"><param value=\"always\" name=\"allowscriptaccess\"><param value=\"high\" name=\"quality\"><param value=\"transparent\" name=\"wmode\"><param value=\"userId=" + userIddynamic +"&amp;" + "IsRawMedia="+ IsRawMedia +"&amp;"+ "embedId=" + initClip + "&amp;PageName=" + PageName + "&amp;ToEmail=" + ToEmail + "&amp;EB=false" +  "&amp;ServicesBaseURL=" + ServicesBaseURL + "&amp;PlayerFromLocal=" + PlayerFromLocal  + "&amp;"+"autoPlayback=true\" name=\"flashvars\"><embed height=\"340\" width=\"545\" name=\"IQMedia\" allowfullscreen=\"true\" allowscriptaccess=\"always\" type=\"application/x-shockwave-flash\"	src=\"" + swfUrl +"\"></object>";
			embedVideo.embedTxt.text="<object height=\"340\" width=\"545\" data=\"" + swfUrl + "\" type=\"application/x-shockwave-flash\"  name=\"HYETA\" id=\"HUY\"><param value=\""+ swfUrl + "\" name=\"movie\"><param value=\"true\" name=\"allowfullscreen\"><param value=\"always\" name=\"allowscriptaccess\"><param value=\"high\" name=\"quality\"><param value=\"transparent\" name=\"wmode\"><param value=\"embedId=" + initClip + "&amp;autoPlayback="+willAutoPlay +"\" name=\"flashvars\"></object>";

			//embedVideo.WPembedTxt.text="[IQMedia id=\"" + _fileId + "\"" + "]";
			embedVideo.WPembedTxt.text ="[gigya height=\"340\" width=\"500\" src=\"" + swfUrl + "\" wmode=\"transparent\" allowscriptaccess=\"always\"  allowfullscreen=\"true\" flashvars=\"embedId=" + initClip + "&amp;autoPlayback="+willAutoPlay +"\" ]";
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
			}
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
		
		private function off(e:Event)
		{
			e.target.filters=null;
		}

		private function copy(e:Event)
		{
			//menuState="OPEN";
			activeBTN(true);
			System.setClipboard(embedVideo.embedTxt.text);
			Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			embedVideo.visible = false;
			showMenu();
			popupDisplay("Copied to Clipboard", 500, 2500);
			
		}
		
		private function WPcopy(e:Event)
		{
			//menuState="OPEN";
			activeBTN(true);
			System.setClipboard(embedVideo.WPembedTxt.text);
			Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			embedVideo.visible = false;
			showMenu();
			popupDisplay("Copied to Clipboard", 500, 2500);
			
		}
		
		private function link_close(e:Event)
		{
			activeBTN(true);
			Tweener.addTween(embedVideo, {x: embedStopx, y: embedStopy, time: .3, transition: "linear"});
			embedVideo.visible = false;
			showMenu();
		}
		
		private function popupDisplay(arg, delay, duration):void
		{
			
			popup.x=sw / 2 - popup.width / 2;
			popup.y=(sh - 28) / 2 - popup.height / 2;
			popup.msgTxt.text=arg;
			setTimeout(function()
				{
					popup.visible=true;
				}, delay);
			hidePopup(duration);
		}
		
		private function hidePopup(arg:Number):void
		{
			setTimeout(function()
				{
					popup.visible=false;
				}, arg);
		}


		/**
		 * Handles displaying errors related to netstream connections
		 *
		 *@param		evt		Event		An error event
		 */
		public function noStream(evt:Event):void
		{
			tracer("WARN", ">> NETSTREAM ERROR: " + evt.type);
			setChildIndex(thumbnailUnavailable, numChildren - 1);

			sw=stage.stageWidth;
			sh=stage.stageHeight;
			
			thumbnailUnavailable.width=sw;
			thumbnailUnavailable.height=sh - 28;

			thumbnailUnavailable.visible=true;
			activeBTN(false);
			lockUI(false);
			mc_pp.gotoAndStop(1);
			mc_pp.static_mc.enabled=false;

			loader_mc.gotoAndStop(1);
			loader_mc.visible=false;
			thumbnailUnavailable.dispText.text="Video Temporarily Unavailable";
		}		
		
		private function OnSeeked(e:VideoEvent):void
		{
			//trace("video onSeeked Event Called");
			
			if(firstTimeLoadClip)//if(myVid.playheadTime >= globalStart)
			{
				firstTimeLoadClip = false;
				myVid.play();
				mc_pp.gotoAndStop(3);
				//trace("video playe event fired");
				myVid.removeEventListener(VideoEvent.SEEKED,OnSeeked);
				
			}
		}


		/**
		 * Handles stage change events of the FLVPlayback component/class
		 *
		 *@param		e		VideoEvent		Any event broadcast bt the player
		 */
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
				}
				
				if (e.state == "stopped" && myVid.playheadTime >= (globalStop))
				{
					showMenu();
					//bookmark(e);

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
			thumbnailUnavailable.visible=true;
			thumbnailUnavailable.dispText.text=arg;
			mc_pp.gotoAndStop(1);
			mc_pp.static_mc.enabled=false;
			mcPlay.enabled=false;
			mcPlay.visible=false;
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
					var mvd:String = "rtmpe://" + app_source + "/" + m_appName + "/" + vidname;
					
				}

				tracer("INFO", "MVD: " + mvd);

				mc_pp.gotoAndStop(1);
				
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
						
						tracer("INFO", ""+prevStream+"="+mvd+"");
						
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
							tracer("INFO", "myVid.source="+mvd+"");
							
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
			
			if (seekAndStart)
			{
				seekAndStart=false;

				try
				{
					myVid.playheadTime=seekStartPoint;
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
					myVid.playheadTime=globalStart;
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
						
							
						}						
					
				
				myVid.visible=false;
			}
			else
			{
				try
				{
					if(firstTimeLoadClip == false)
					{
						myVid.play();
						mc_pp.gotoAndStop(3);
					}
					else
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

			//commented on 9 Feb 2012 by meghana
			/*if (willAutoPlay == "TRUE")
			{
				//loggedPlay = false;
			}*/
			
			//logPlay();
		}


		// ******************* Data Functions *****************************

		/**
		 * Handles prep and loading of the XML/REST IQMedia services "getVars"
		 *
		 */
		private function GetVarInfo():void
		{
			if (!varsLoaded)
			{
				
				var getVarService:String = srvString + "/svc/media/getVars?local=" + PlayerFromLocal + "&fid=" + guid;
				GetXml(guid, getVarService);
			}
			else
			{
				tracer("INFO", "Grabbing app/streamname: "+m_appName);				
				
				GetNSConnection(m_appName);				
			}

		}		
		

		private function thumbInit(evt:Event):void
		{
			//meta_mc.station_logo.addChild(evt.target);

			if (thumbLdr.content is Bitmap)
			{
				Bitmap(thumbLdr.content).smoothing=true;
			}

			thumbLdr.width=57;
			thumbLdr.height=57;




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

			loaderContext.checkPolicyFile=true;		
			

			var pictURLReq:URLRequest = new URLRequest(slogoSource);
			thumbLdr.load(pictURLReq, loaderContext);
			thumbLdr.contentLoaderInfo.addEventListener(Event.COMPLETE, thumbInit)
			
			thumbLdr.width=57;
			thumbLdr.height=57;

			stURL=new URLRequest(clpXML.mediaInfo.@sourceUrl);
			var stime:int = Number(clpXML.mediaInfo.@startTime);				
			var etime:int = Number(clpXML.mediaInfo.@endTime);			
			firstTimeLoadClip = true;
			
			ftime=etime - stime;
			//ftime = -1;
			
			//Commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()=="TRUE" && firstTimeLoadRawMedia==true && initialOffset>=0 && initialOffset<=3599)
			{
				
			}
			else
			{*/
				timeDisplayTot.text=timeCode(ftime);
			/*}*/

			
			vidname=clpXML.vars.@fileName;
			
			trace("vidname" + vidname);
			
			m_appName=clpXML.vars.@appNameFMS;
			
			LogPlayURLDynamic =  clpXML.vars.@logPlayURL;
			
			app_source=clpXML.vars.@streamUrl;
			var myPattern:RegExp = /media2/gi;  
			var str:String = clpXML.vars.@streamUrl;
			app_source=str.replace(myPattern, "fms3");
			trace("app_source:"+app_source);
			globalStart=stime;
			
			if(firstTimeLoadClip)
			{
				myVid.addEventListener(VideoEvent.SEEKED,OnSeeked);
			}
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
			
			//_linkUrl = llnkUrldynamic;
			
			
			embedUrl=clpXML.vars.@embedUrl;
			
			
			hasCaption=clpXML.vars.@hasCaption;

			// CLIP INFO NODE ITEMS
			
				var clpETime:Number = Number(clpXML.mediaInfo.@endTime);
				var clpSTime:Number = Number(clpXML.mediaInfo.@startTime);
				var clpData:Number = clpETime - clpSTime;
				var clpInfoTime:String = timeCode(clpData);
			
			// META INFO POPULATION CALL
			
			// commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				
				clipName=clpXML.mediaInfo.@sourceTitle;
				
				clipDesc="Test Description";
				
				
				meta1_textString=clipName;
				//meta_mc.meta1Txt.text=clipName;
				//meta_mc.meta2Txt.text="Duration: " + clpInfoTime + " | Views: " + clpXML.vars.@views + " | Topic: RawMedia"
				//working
				//meta_mc.meta2Txt.text="Duration: " + clpInfoTime + " | Views: " + clpXML.mediaInfo.@plays + " | Topic: ";
				//meta_mc.meta3Txt.text="Clipper: RawMedia" + clpXML.clipinfo.@author + " | " + "Aired: " + clpXML.vars.@airdate;
				//working
				//meta_mc.meta3Txt.text="Clipper: " + " | " + "Aired: " + clpXML.mediaInfo.@airdate;
				rid=clpXML.@rid;
			}
			else
			{*/
				clipName=clpXML.mediaInfo.@title;
				trace("clipName" +clipName);
				
				clipDesc=decodeURI(clpXML.mediaInfo);
				meta1_textString=clipName;
				//meta_mc.meta2Txt.text="Duration: " + clpInfoTime + " | Views: " + clpXML.mediaInfo.@plays + " | Topic: " + clpXML.mediaInfo.@categoryName;
				//meta_mc.meta3Txt.text="Clipper: " + clpXML.mediaInfo.@author + " | " + "Aired: " + clpXML.mediaInfo.@airdate;
				rid=clpXML.@rid;
			/*}*/
			

			if (hasCaption)
			{
				trace("hasCaption.toUpperCase" +hasCaption.toUpperCase())
				
				if (hasCaption.toUpperCase() == "TRUE")
				{
					/* updated by vishal......hide closedcaption call */
					/*GetClosedCapInfo(globalStart, globalStop);*/
					
					
					
					/* over updated by vishal......hide closedcaption call */
				}
				else
				{
				}
			}

			if (String(clpXML.@msg) == "0")
			{

				varsLoaded=true;
				errorDisplay("INVALID SERVICE DATA.\nPLEASE TRY AGAIN LATER.");
			}
			else
			{
				
				if(clpXML.vars.@emailFrom != null)
				{
				}
				if (_fileId == null || _fileId == undefined || _fileId == "")
				{
					errorDisplay("INVALID SERVICE DATA.\nPLEASE TRY AGAIN LATER.");
				}
				else
				{
					dataLoaded=true;
					varsLoaded=true;
					assignEmbed();

					if (userClicked)
					{
						 /*tracer("INFO", "fireNSLoad Started");
						fireNSLoad(m_appName);*/
						
						GetNSConnection(m_appName);
					}
					userClicked = true;
					clipLoaded = true;
				}
			}
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

			if (h > 0)
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
		


		// Helper function to metaDisplayCore to accept event when called via an event instead of directly
		


		/**
		 * Function for updating the onscreen time for video
		 *
		 *@param		e		Event		timer event
		 */
		private function UpdateCurrentTime(e:Event)
		{
			
			trace("UpdateCurrentTime called")
			if(IsLogPlay == "false")
			{
				
				logPlay();
				IsLogPlay = "true";
			}

			var actualTime:Number=e.target.playheadTime - globalStart;
			actualTime=actualTime<0?0:actualTime;
			var realTime:Number;
			
			// commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{			
				realTime= e.target.playheadTime ;
			}
			else
			{*/
				realTime= e.target.playheadTime - globalStart;
			/*}*/
			
			timeDisplayCur.text=timeCode((realTime < 0) ? 0 : realTime);			
			
			var CurrentSec:int =int(realTime);
			
			CurrentSec=(CurrentSec<0)?0:CurrentSec;
			
			
			var BarWid:Number = Scrub.scrubBar.width;
			var BarDis:Number = BarWid / ftime;
			var BarSet:Number = actualTime * BarDis;
			var bdis_S:String = String(BarSet);
			Scrub.progBar.width=BarSet;

			// commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()=="TRUE")
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
			{	*/		
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
			/*}*/
			
			// commeted on 9 Feb 2012 by meghana
			/*if(IsClipperEnable==true)
			{
				if(e.target.playheadTime > customSeekPointRight)
				{
					//myVid.seek(customSeekPoint);
					mc_pp.gotoAndStop(2);
					myVid.pause();
					menuState="CLOSED";
					activeBTN(true);
	
					isCustomSeek=true;
					
				}
			}*/
			

			if (e.target.playheadTime > globalStop || e.target.playheadTime >= myVid.totalTime)
			{
				showMenu();
				//bookmark(e);
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

		
		
		/**
		 * Function for handling initial video startup/play log some events, and trigger some adapTV stuff
		 *
		 *@param		e		Event		FLVPlayback event
		 */
		private function logInitialPlay(evt:Event):void
		{
			tracer("INFO", "PLAYING_STATE_ENTERED");
			activeBTN(true);
			mutebutt_mc.enabled=true;
			volbutt_mc.enabled=true;
			embed_mc.enabled=true;
			mcPlay.visible=false;

			
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
			trace("logPlay called")
			// commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()!="TRUE")
			{*/
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
			/*}*/
		}


		/**
		 * Function for handling initial video playback via user control of play button
		 *
		 *@param		e		Event		FLVPlayback event
		 */
		private function playClip(e:Event)
		{
			mcPlay.enabled=false;
			tracer("INFO", "LARGE PLAY CLICKED");
			var evt:Event = new Event("RLPlayer_PLAY_CLICKED", true, false);
			dispatchEvent(evt);
			
			
			mcPlay.visible=false;
			myVid.visible=false;
			mc_pp.gotoAndStop(3);
			
				try
				{
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
						menuState="OPEN";
						MenuMove(e);
					}
					else
					{
						menuState="OPEN";
						MenuMove(e);

						try
						{
							myVid.play();
						}
						catch (e:VideoError)
						{
							tracer("WARN", "ERR3 ERROR: " + e.code + " :: " + e);
						}
					}
					break;
				case 3:
					//tracer("INFO","CASE 3");
					mc_pp.gotoAndStop(2);
					myVid.pause();
					menuState="CLOSED";
					MenuMove(e);
					break;
				default:
				
			}
			// commented on 9 Feb 2012 by meghana
			/*if(IsClipperEnable==true)
			{
				closeMenu();
				if(isCustomSeek==true)
				{					
					myVid.seek(customSeekPoint);
					isCustomSeek=false;
				}
			}	*/		
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

			menuState="OPEN";
			menuSubState="CLOSED";
		}
		
		private function showRAWMediaMenu():void
		{
			if (!userClicked)
			{				
				GetVarInfo();
			}
			

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
			
			menuState="OPEN";
			menuSubState="CLOSED";
		}

		private function closeMenu():void
		{
			menuState="CLOSED";

			activeBTN(true);

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


		private function closeMenuandSharing():void
		{
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
			updateScrubTime()
		}

		private function updateScrubTime()
		{
			var seekPointLeft:Number;
			var seekPointRight:Number;
			
			
			
			if((seekPointRight-seekPointLeft)>ClipLength)
			{
				var TempDif:Number;
				TempDif=(seekPointRight-seekPointLeft)-ClipLength;
				seekPointRight=seekPointRight-TempDif;
			}
			
			
			isCustomSeek=true;
			customSeekPoint=(seekPointLeft+offsetTime);
			customSeekPointRight = (seekPointRight+offsetTime);
			
		};

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



		//*************** Javascript Integration Functions ******************
		public function setSeekPoint(num:Number)
		{
			//txt_Msg.text=txt_Msg.text+"seekOn:"+num;			
			
			// commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
			
				if (num < globalStart && num<0)
				{
					num=globalStart;
				}
	
				if (num > globalStop && num>3599)
				{
					num=globalStop;
				}
			}
			else
			{*/
					if (num < globalStart)
					{
						num=globalStart;
					}
		
					if (num > globalStop)
					{
						num=globalStop;
					}
			/*}*/

			// handle enabling playback and then seeking if not already started
			
			// commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()!="TRUE")
			{*/
				if (!myVid.playing)
				{
					seekAndStart=true;
					userClicked=true;
					seekStartPoint=num;
					activeBTN(true);
					loader_mc.gotoAndPlay(2);
					thumbnailUnavailable.visible=false;
					GetVarInfo();
					//txt_Msg.text=txt_Msg.text+"this";
				}
			/*}*/
			
			tracer("INFO", "setSeekPoint: " + num);
			
			//HACK FOR OFFSETS		
			// commented on 9 Feb 2012 by meghana
			/*if(IsRawMedia.toLocaleUpperCase()=="TRUE")
			{
				var seekPoint:int;
				seekPoint=num;
				
				if(num >= globalStop && num>3599) seekPoint = globalStop-300;
				if(seekPoint < globalStart && seekPoint<0) seekPoint = globalStart;
				
				globalStart=num;
				var st = 0;
				
				var et=(globalStart+globalStop);
				
				var sd = 0;
				var durr = null;
				
				if (st < globalStart && st<0)
				{
					st = globalStart;
					//HACK FOR CLIPS				
					sd = Math.abs (seekPoint - 600 - globalStart);
				}
				if (st >= 3599)
				{
					durr = null;
					et = null;
				}
				else
				{
					
					et = st + globalStop;
					
					//txt_Msg.text=txt_Msg.text+"Inside else";
					if (et > globalStop && et>3599)
					{
						et = globalStop;
						
						//txt_Msg.text=txt_Msg.text+"Inside else if";
						
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
			}*/
			
			offsetTime=globalStart;
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
			
			var evt:Event = new Event(MouseEvent.CLICK, true, false);
			
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
			mc_pp.static_mc.enabled=bool;
			mutebutt_mc.enabled=bool;
			volbutt_mc.enabled=bool;
			embed_mc.enabled=bool;
			

			
		}


		private function activeBTN(att:Boolean)
		{
			if (att == false)
			{
				mc_pp.removeEventListener(MouseEvent.CLICK, playBtn);
				//embed_mc.removeEventListener(MouseEvent.CLICK,bookmark);
				
				//fullScreen.removeEventListener (MouseEvent.CLICK, ToggleFullScreen);
				volbutt_mc.removeEventListener(MouseEvent.CLICK, VolMove);
				mutebutt_mc.removeEventListener(MouseEvent.CLICK, VolMove);
				mcPlay.removeEventListener(MouseEvent.MOUSE_DOWN, playClip);
				bKnob.removeEventListener(MouseEvent.MOUSE_OVER, bknob_over);
				bKnob.removeEventListener(MouseEvent.MOUSE_OUT, bknob_out);
				bKnob.removeEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
				bKnob.removeEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
				
					//menu_mc.removeEventListener(MouseEvent.CLICK)
			}

			if (att == true)
			{
				mc_pp.addEventListener(MouseEvent.CLICK, playBtn);
				//embed_mc.addEventListener(MouseEvent.CLICK,bookmark);
				//fullScreen.addEventListener (MouseEvent.CLICK, ToggleFullScreen);
				volbutt_mc.addEventListener(MouseEvent.CLICK, VolMove);
				mutebutt_mc.addEventListener(MouseEvent.CLICK, VolMove);
				mcPlay.addEventListener(MouseEvent.MOUSE_DOWN, playClip);
				bKnob.addEventListener(MouseEvent.MOUSE_OVER, bknob_over);
				bKnob.addEventListener(MouseEvent.MOUSE_OUT, bknob_out);
				bKnob.addEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
				bKnob.addEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
			}
		}

		//listen for fullscreen events
		private function fireSize(e:Event):void
		{
			if (stage.displayState == StageDisplayState.FULL_SCREEN_INTERACTIVE)
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
			embed_mc.addEventListener(MouseEvent.CLICK,bookmark);
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

			sw=stage.stageWidth;
			sh=stage.stageHeight;
			

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
			
			//commented on 9 Feb 2012 by meghana
			/*if (willAutoPlay == "TRUE")
			{
				// nada
			}
			else
			{*/
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
			/*}*/
		}	


		private function onLoadPreviewComplete(event:Event):void
		{
			previewLoaded=true;
			var info:LoaderInfo = LoaderInfo(prevLoader.contentLoaderInfo);

			// tracer ("INFO","Image Preview url=" + info.url);
			sw=stage.stageWidth;
			sh=stage.stageHeight;
			
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
			embed_mc.addEventListener(MouseEvent.CLICK,bookmark);
			mc_pp.gotoAndStop(2);
			setChildIndex(mcPlay, numChildren - 1);

			mcPlay.visible=true;
			//mcPlay.alpha = .85;
			loader_mc.gotoAndStop(1);
			loader_mc.visible=false;
			setChildIndex(prevLoader, 0);
		}
		
	}
}