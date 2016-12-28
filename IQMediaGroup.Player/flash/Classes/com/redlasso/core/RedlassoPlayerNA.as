/**
	RedlassoPlayer Custom Video/Audio RTMP/FMS streaming player with functionality specifically for Redlasso - requires custom patched FLVPlayback.swc

	Developed by Robert M Hall
	Version:		2.0.8.82D
	Date:		03/09/2010 - 5:30PM

	Copyright (c) 2009-2010 Robert M Hall, II, Inc dba Feasible Impossibilities
   
	http://www.impossibilities.com/ - rhall@impossibilities.com
	
	Licensed in perpetuity to Redlasso Inc via work for hire contract agreement between Feasible Impossibilities and Redlasso
   
	Includes/references other third party libraries - some licenses may differ, refer to each library individually for license requirements
   
	CHANGELOG:
	2.0.8.82	Removed all ADAPTV references to provide a comparison fresh base for ADAPTV staff
	
	2.0.8.81	Fixed resizing of Ads when toggling between fullscreen/normal
   				Added Adaptv.destroy in loadClip method to kill existing ads when loading a new clip without reloading player
				Moved all Actionscript 3 timeline code to this cleaner, more organized document class, easier management, diffing, etc.
				Added more comments to code for benefit of Redlasso, AdapTV and any others who have to work with the application
				
	See Developer notes for previous iterations
	
	Make sure to set proper version number an rev when compiling
	Also make sure to set proper services, development or production D or P
	TODO: Move to AS3 CONFIG switch for service/production

 **/

package com.redlasso.core
{
	import caurina.transitions.Tweener;

	import com.hires.debug.Stats;
	import com.impossibilities.utils.NpContextMenu;
	import com.impossibilities.utils.StringHelper;

	import fl.containers.*;
	import fl.video.*;

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

	import urlshorten.URLShorten;
	import urlshorten.events.URLShortenEvent;

	/**
	 * @author Robert M. Hall, www.impossibilities.com
	 *
	 * @date	March 9, 2010
	 * @version 2.0.8.81
	 */

	public class RedlassoPlayerNA extends MovieClip
	{


		private var stats:Stats = new Stats({bg:0x66000000});

		private var guid:String;
		private var thevidplays:int = 0;

		private var initClip:String;
		private var widgetMode:String;
		private var willAutoPlay:String;
		private var pid:String;
		private var fvars:String;
		private var outStr:String;

		private var outgoing_lc:LocalConnection = new LocalConnection();

		private var embedPage:String = "";
		
		private var jsDEBUG:Boolean = false;
		
		// Production versus test service URL's
		// public static const srvString:String = "http://services.redlasso.com";
		public static const srvString:String = "http://test.redlasso.com/service_test";

		private static const versionBuild:String = "2.0.8.81D RMH";
		private static const menuItemLabel1:String = "Redlasso Video Player";
		private static const menuItemLabel2:String = "Version: " + versionBuild + "";
		
		// private static const debugService1:String = "&debug=failRepond";
		// private static const debugService1:String = "&debug=failGarbage";
		private static const debugService1:String = "";
		// private static const debugService2:String = "&debug=failRepond";
		// private static const debugService2:String = "&debug=failGarbage";
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
		private var _linkUrl:String;
		private var _fileId:String;
		private var clipName:String;
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
		
		private var firstTimeLoadClip:Boolean = true;
		
		private var dataLoaded:Boolean = false;

		private var audioGraphic:String;
		private var hasVideo:String;

		private var getVarsInt:uint;
		private var previewLoaded:Boolean = false;

		private var myTimer:Timer = new Timer(1500);

		//********** Declare Video Variables *****************************
		private var app_source:String;
		private var m_appName:String = "000250/flash";
		private var placeholder:UILoader = new UILoader();

		private var vidSmoothing:Boolean = true;

		private var prevStream:String;

		private var metaState:Boolean = false;

		private var localData:SharedObject = SharedObject.getLocal("redlasso_player");

		private var seekAndStart:Boolean = false;
		private var seekStartPoint:Number;

		private var prevLoader:Loader = new Loader();
		private var loaderContext:LoaderContext = new LoaderContext();

		private var imgPreviewURL:String = srvString + "/svc/clip/previewImage?eid="; // + initClip;
		private var previewFirst:Boolean = true;
		
		private var conMenu:NpContextMenu ;

		public function RedlassoPlayerNA():void
		{
			if (stage)
			{
				oneTimeInit();
			}
			else
			{
				addEventListener(Event.ADDED_TO_STAGE, oneTimeInit);
			}

		}

		/**
		 * Handles reading in flashvars, setting proper stage size, widget mode (in another AS3/Flex container) and other housekeeping neccessary to get the UI and underlying eventListeners, etc. setup to fire things up.
		 *
		 */
		public function oneTimeInit(event:Event = null):void
		{
			trace("RLPLAYER ADDED TO STAGE!");
			removeEventListener(Event.ADDED_TO_STAGE, oneTimeInit);

			stage.align = StageAlign.TOP_LEFT;
			this.opaqueBackground = 0x000000;

			flash.system.Security.allowDomain("*");
			Security.allowDomain("*");

			stats.visible = false;
			addChild(stats);

			// Retrieve any Flashvars passed in
			initClip = String(LoaderInfo(this.root.loaderInfo).parameters.embedId);
			//initClip = "5183fb3d-eb65-464a-988c-f7a60813faa5";
			widgetMode = String(LoaderInfo(this.root.loaderInfo).parameters.widget).toUpperCase();
			willAutoPlay = String(LoaderInfo(this.root.loaderInfo).parameters.autoPlayback).toUpperCase();
			pid = String(LoaderInfo(this.root.loaderInfo).parameters.pid);

			about_scrn.core_mc.versionTxt.text = menuItemLabel2;

			// Adjust scaling/dimensions differently if embedded ina Flex Loader container
			if (widgetMode == "TRUE")
			{
				stage.scaleMode = StageScaleMode.NO_BORDER;
				fullScreen.enabled = false;
				fullScreen.alpha = .35;
				sw = stage.stageWidth;
				sh = stage.stageHeight;
			}
			else
			{
				stage.scaleMode = StageScaleMode.NO_SCALE;
				sw = this.parent.width;
				sh = this.parent.height;
			}

			wchange = sw / 545;
			hchange = sh / 340;

			res_w = sw;
			res_h = sh;

			if (willAutoPlay == "TRUE")
			{
				userClicked = true;
			}

			// External Logging AIR application for troubleshooting
			// Download/install from: http://clients.impossibilities.com/redlasso/2009/logger/
			outgoing_lc.allowDomain('*');
			outgoing_lc.send("_log_output", "startLogging", "Connected to " + menuItemLabel1 + " [ " + menuItemLabel2 + " ]\n" + Capabilities.serverString + "\n" + Capabilities.version + "\n");
			outgoing_lc.addEventListener(StatusEvent.STATUS, logonStatus);

			//Filtering based on INFO, WARN, LOG, ERROR, FATAL to be added in the future for filtering in AIR Logger app
			tracer("INFO", "\n\n\n[______________________________________________________________]");
			tracer("INFO", "Redlasso Video Player :: " + menuItemLabel2);
			tracer("INFO", "Video Component :: " + FLVPlayback.VERSION);

			// setup additional GET params for service requests based in clip ID/GUID
			if (initClip != null)
			{
				guid = initClip;
				if (pid != null)
				{
					fvars = "embedId=" + guid + "&pid=" + pid;
				}
				else
				{
					fvars = "embedId=" + guid;
				}
			}

			//********** Setup Attributes
			mcPlay.alpha = .85;
			mcPlay.visible = false;
			meta_mc.visible = false;
			about_scrn.visible = false;
			popup.visible = false;
			overlay_mc.alpha = 0;

			CC_btn.visible = false;
			CC_btn.enabled = false;
			thumbnailUnavailable.visible = false;
			Scrub.progBar.width = 0;

			myTimer.addEventListener(TimerEvent.TIMER, emailCloseF);
			loader_mc.buttonMode = false;
			loader_mc.mouseEnabled = false;
			capback.visible = false;
			myCap.visible = false;
			dispCap.visible = false;

			myVid.bufferTime = 3;
			myVid.fullScreenTakeOver = false;

			setUpVidListeners();
			moreListeners();

			getVolume();

			setupMenuListeners();
			setupMenuButtonListeners();
			setupExternalInterfaceCalls();
			setupBookMarkListeners();

			stage.addEventListener(Event.RESIZE, resizeListener);
			stage.addEventListener(Event.FULLSCREEN, fireSize);

			if (widgetMode != "TRUE")
			{
				fullScreen.addEventListener(MouseEvent.CLICK, ToggleFullScreen);
			}

			activeBTN(false);
			
			// For handling permissions on media files to allow smoothing to be applied
			loaderContext.checkPolicyFile = true;
			Security.loadPolicyFile("http://media.redlasso.com/crossdomain.xml");

			// ********************* PREVIEW Image Loader/kickoff of app

			// Show clip ID and determine what to do otherwise throw error about missing clip
			// also check all passed in values/paras to determine what/how to load
			tracer("INFO", "INITCLIP: \"" + initClip + "\"");
			if (initClip == null || initClip == "null" || initClip == "NULL" || initClip == "UNDEFINED" || initClip == "undefined" || initClip == "")
			{
				errorDisplay("Missing Video Clip embedId");
				loader_mc.gotoAndStop(1);
				loader_mc.visible = false;
				activeBTN(false);
				lockUI(false);
				if (widgetMode != "TRUE")
				{
					menu_mc.removeEventListener(MouseEvent.CLICK, MenuMove);
				}
			}
			else
			{
				if (initClip != "null" && initClip != "undefined" && willAutoPlay != "TRUE")
				{
					kickPreview();
				}
				else
				{
					if (willAutoPlay == "TRUE")
					{
						activeBTN(true);
						loader_mc.gotoAndPlay(2);
						thumbnailUnavailable.visible = false;
						mcPlay.removeEventListener(MouseEvent.CLICK, startUp);
						GetVarInfo();
						impressionLoadPreviewImage();
						logPlay();
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

			conMenu = new NpContextMenu(this);
			setupContextMenu();

			SizeItems();
			toggleSmoothing(vidSmoothing);

			trace("RLPLAYER READY!");
			// Important event to dispatch for Flex apps/widgets/mediacenter apps so they know when things are ready
			var evt:Event = new Event("RLPlayer_READY", true, false);
			dispatchEvent(evt);

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
		 * Download/install from: http://clients.impossibilities.com/redlasso/2009/logger/		 
		 *
		 */
		private function tracer(typ, arg)
		{
			outStr = "[ " + getTimer() + " : " + typ + " ] :: " + arg;
			if (typ != "WARN")
			{
				trace(outStr);
				outgoing_lc.send("_log_output", "displayMsg", outStr);

				if (jsDEBUG)
				{
					try
					{
						var js = ExternalInterface.call("trace", outStr);
					}
					catch(err:SecurityError)
					{
					}
					catch(err:Error)
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
			conMenu.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, handleMenuSelection);

			conMenu.addMenuItem(menuItemLabel1, false);
			conMenu.addMenuItem(menuItemLabel2, false);
			conMenu.addMenuItem("Enter FullScreen", true);
			conMenu.addMenuItem("Exit FullScreen", false);
			conMenu.addMenuItem("Redlasso Menu", true);
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
			switch(conMenu.selectedMenu)
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
				case "Redlasso Menu":
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

					stats.visible = !stats.visible;
					stats.startStats();
					break;
			}

		}



		/**
		 * Allow an external container app, AS3 or Flex to change the size of the app if necessary
		 * depending on how/if they need to resize things
		 *
		 *@param		wid		Number		Width to set the player
		 *@param		hgt		Number		Height to set the player
		 */
		public function SwfSize(wid:Number, hgt:Number)
		{
			tracer("INFO", wid + "x" + hgt);
			sw = wid;
			sh = hgt; // - 28;
			res_w = sw;
			res_h = sh;
			SizeItems();
		}

		/**
		 * Allow an external container app, AS3 or Flex to load in a new clip for playback
		 * handles resetting certain display objects
		 * and determining how to load the new clip ID passed in
		 *
		 *@param		id				String		GUID from Redlasso database identifying the video clip
		 *@param		playStyle		Number		Autoplay the clip if set to TRUE, if FALSE pause on load
		 */
		public function LoadClip(id:String, playStyle:String)
		{
			tracer("INFO", "LOADCLIP:" + id + " " + playStyle);
			prevLoader.alpha = 0;
			audioGraphic = "";
			hasVideo = "NULL";
			initClip = id;
			previewPersistent = false;

			if (prevID == id)
			{
				tracer("INFO", "Same clip already selected and loaded/playing");
				myVid.seek(globalStart);
			}
			else
			{
				
				tracer("INFO", "\n\n*** NEW CLIP:");
				prevID = id;
				if (playStyle.toUpperCase() == "TRUE")
				{
					willAutoPlay = "TRUE";
					if (widgetMode == "TRUE")
					{
						mc_pp.removeEventListener(MouseEvent.CLICK, startUp);
						mc_pp.gotoAndStop(3);
						activeBTN(true);
						userClicked = true;
						impressionLoadPreviewImage();
					}
				}
				else
				{
					willAutoPlay = "FALSE";
				}
				tracer("INFO", "WILLAUTOPLAY:" + willAutoPlay);
				loggedPlay = false;
				Tweener.addTween(meta_mc, {y:mcStopy, time:.3, transition:"easeInQuint"});
				Tweener.addTween(meta_mc.toggle_mc, {rotation:0, time:.5, transition:"easeInOutBack"});
				clipLoaded = false;
				myVid.visible = false;
				guid = id;
				initClip = id;
				timeDisplayTot.text = "--:--";
				timeDisplayCur.text = "--:--";
				if (getChildByName("thumbnailUnavailable"))
				{
					thumbnailUnavailable.visible = false;
				}
				if (thevidplays == 1)
				{
					tracer("INFO", "The video is playing");
					myVid.stop();
					myVid.getVideoPlayer(0).close();
					thevidplays = 0;
					loader_mc.visible = true;
					loader_mc.gotoAndPlay(2);
				}
				if (willAutoPlay == "TRUE")
				{
					GetVarInfo();
				}
				else
				{
					tracer("INFO", "Kicking PREVIEW!");
					kickPreview();
				}
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
			videoplayer.smoothing = arg;
			vidSmoothing = arg;
		}


		/**
		 * Leverages ExternalInterface to inject anonymous functions into container browser DOM to extract info such as page title, page URL, referer, etc.
		 *
		   @return				String		Returns the info extracted from the browser DOM in format for logging to Redlasso services
		 */
		private function queryDOM():String
		{
			var DOMinfo:String;
			var jsResult:Object;
			if (ExternalInterface.available)
			{
				try
				{
					jsResult = ExternalInterface.call("function() { " + "var pageURL=window.location;" + "pageURL.windowName=window.name;" + "pageURL.documentReferer=document.referer;" + "pageURL.documentTitle=document.title;" + "return pageURL; }");
				}
				catch(err:Error)
				{
					tracer("WARN", "EI Error: " + err.message);
				}
				finally
				{
					DOMinfo == "undefined|undefined";
				}

				try
				{
					DOMinfo = jsResult.href + "|" + jsResult.documentReferer;
					embedPage = jsResult.href;
				}
				catch(err:Error)
				{
					DOMinfo = "undefined|undefined";
					tracer("WARN", "EI Error: " + err.message);
				}
			}
			else
			{
				DOMinfo = "externalInterface NOT AVAILABLE";
			}

			if (DOMinfo == "undefined|undefined")
			{
				DOMinfo = "externalInterface NOT AVAILABLE";
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

			var vidWidth:Number;
			var vidHeight:Number;
			if (widgetMode != "TRUE")
			{
				sw = stage.stageWidth;
				sh = stage.stageHeight;
				tracer("INFO", "STANDARD EMBED SIZE: " + sw + "x" + sh);
			}

			if (widgetMode == "TRUE" && (stage.displayState == StageDisplayState.FULL_SCREEN))
			{
				sw = this.parent.width;
				sh = this.parent.height;
				tracer("INFO", "WIDGET EMBED SIZE: " + sw + "x" + sh);
			}

			var stageBot:Number = sh - 27;
			popmask.width = sw;
			popmask.height = sh;
			popmask.x = 0;
			popmask.y = 0;

			overlay_mc.x = 0;
			overlay_mc.y = 0;
			overlay_mc.width = sw;
			overlay_mc.height = sh;

			if (myVid.metadataLoaded)
			{
				//tracer ("INFO","VIDEO METADATA :: LOADED");
				vidScale = Math.min(sw / myVid.metadata.width, (sh - 27) / myVid.metadata.height);
				newW = vidScale * myVid.metadata.width;
				newH = vidScale * myVid.metadata.height;
				myVid.setSize(newW, newH);
			}
			else
			{
				//tracer ("INFO","VIDEO METADATA :: NOT LOADED");
				vidWidth = myVid.getVideoPlayer(myVid.activeVideoPlayerIndex).width;
				vidHeight = myVid.getVideoPlayer(myVid.activeVideoPlayerIndex).height;
				vidScale = Math.min(sw / vidWidth, (sh - 27) / vidHeight);
				newW = vidScale * vidWidth;
				newH = vidScale * vidHeight;
				myVid.setSize(newW, newH);
			}
			myVid.x = (sw / 2) - (myVid.width / 2);
			myVid.y = (stageBot / 2) - (myVid.height / 2);


			/* For later for fullscreen control in widget - right now we handle fullscreen in widget mode via the widget
				and a special popout HTML page to allow user to scale to any dimension or go fullscreen
				may put support back into this at a later date when Redlasso MediaCenter widget API is finalized
				
			   if (widgetMode == "TRUE" && (stage.displayState == StageDisplayState.FULL_SCREEN))
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
					prevLoader.width = newpW;
					prevLoader.height = newpH;
					prevLoader.x = sw / 2 - newpW / 2;
					prevLoader.y = (sh - 28) / 2 - newpH / 2; //0;
				}
				else
				{
					//tracer ("INFO","PrevLoader vid based Scaling"+myVid.source);
					if (prevLoader && myVid.playing)
					{
						try
						{
							prevLoader.width = newW;
						}
						catch(err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}
						try
						{
							prevLoader.height = newH;
						}
						catch(err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}
						try
						{
							prevLoader.x = myVid.x;
						}
						catch(err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}
						try
						{
							prevLoader.y = myVid.y;
						}
						catch(err:Error)
						{
							tracer("WARN", "ERROR SizeItems prevLoader: " + err);
						}
					}

				}
			}

			try
			{
				thumbnailUnavailable.width = newW;
			}
			catch(err:Error)
			{
				tracer("WARN", "ERROR SizeItems thumbnailUnavailable: " + err);
			}
			try
			{
				thumbnailUnavailable.height = newH;
			}
			catch(err:Error)
			{
				tracer("WARN", "ERROR SizeItems thumbnailUnavailable: " + err);
			}

			meta_mc.y = Math.round((meta_mc.height * .80) * -1);
			mcStopy = meta_mc.y;

			botbar_mc.width = sw;
			botbar_mc.y = sh - botbar_mc.height;

			capback.x = 0;
			capback.width = sw;
			//capback.height = sh*.2220588
			capback.y = sh - (botbar_mc.height + capback.height);

			dispCap.autoSize = "center";
			dispCap.width = sw - 10; //* .4633;
			dispCap.x = (sw - dispCap.width) / 2;
			dispCap.y = capback.y + 10;

			//myCap.height = sh*.18852
			myCap.visible = false;

			meta_mc.metabg_mc.x = 0;
			meta_mc.metabg_mc.width = sw + 200; 
			if (varsLoaded)
			{
				meta_mc.meta1Txt.text = meta1_textString;
				truncMeta(meta_mc.meta1Txt, sw - 85);
			}

			mc_pp.x = 5.5 * wchange;
			mc_pp.y = botbar_mc.y + 4;

			loader_mc.x = mc_pp.x + 10;
			loader_mc.y = botbar_mc.y + 13.5;

			timeDisplayCur.x = mc_pp.x + 12;
			timeDisplayCur.y = botbar_mc.y + 7;

			menu_mc.x = Math.round(botbar_mc.width - 25);
			menu_mc.y = botbar_mc.y + 7;

			CC_btn.x = Math.round(botbar_mc.width - 25);
			CC_btn.Y = 9;

			volbutt_mc.x = menu_mc.x - 23;
			volbutt_mc.y = botbar_mc.y + 7;
			mutebutt_mc.x = menu_mc.x - 23;
			mutebutt_mc.y = botbar_mc.y + 7;

			fullScreen.x = volbutt_mc.x - 28;
			fullScreen.y = botbar_mc.y + 7;

			timeDisplayTot.x = fullScreen.x - 40;
			timeDisplayTot.y = botbar_mc.y + 7;

			placeholder.x = myVid.x;
			placeholder.y = myVid.y;
			placeholder.width = myVid.width;
			placeholder.height = myVid.height;

			Scrub.x = timeDisplayCur.x + 53;
			Scrub.width = Math.round((timeDisplayTot.x - Scrub.x) - 7);
			Scrub.y = botbar_mc.y + 4;

			bKnob.x = Scrub.x;
			bKnob.y = botbar_mc.y + 4;

			tknob_mc.x = Scrub.x;
			tknob_mc.y = botbar_mc.y + 4;

			vol.x = volbutt_mc.x - 3;
			vol.y = Math.round(botbar_mc.y - 34);

			mcPlay.width = sw * .2517;
			mcPlay.height = mcPlay.width;
			mcPlay.x = ((sw - mcPlay.width) / 2) + (mcPlay.width / 2);
			mcPlay.y = ((sh - botbar_mc.height) - (mcPlay.height - (mcPlay.height * .8))) / 2;

			if (widgetMode == "TRUE" && (stage.displayState == StageDisplayState.FULL_SCREEN))
			{
				mcPlay.y = this.parent.height / 2 - mcPlay.height / 2;
			}

			menubg.width = sw;
			menubg.height = menubg.width * .14;
			menubg.x = 0;
			menubg.y = Math.round(botbar_mc.y + (38 * wchange));

			if (menuState == "OPEN")
			{
				showMenu();
			}
			if (metaState)
			{
				metaDisplayCore();
			}

			menuStopx = menubg.x;
			menuStopy = menubg.y;
			menuStartx = menubg.x;
			menuStarty = sh - menubg.height - botbar_mc.height;

			about_scrn.height = sh * .44411;
			about_scrn.width = about_scrn.height * 2.37682;
			about_scrn.x = (sw / 2);
			about_scrn.y = (sh - botbar_mc.height) / 2.5;

			popup.height = sh * .44411;
			popup.width = about_scrn.height * 2.37682;
			popup.x = (sw / 2);
			popup.y = (sh - botbar_mc.height) / 2.5;

			emailShare.height = sh * .7058;
			emailShare.width = emailShare.height * 1.62083;
			emailShare.x = Math.round(((sw - emailShare.width) / 2) + 5);
			emailShare.y = Math.round(botbar_mc.y + (38 * wchange));
			emailStopx = emailShare.x;
			emailStopy = emailShare.y;
			emailStartx = emailShare.x;
			emailStarty = 40 * wchange;

			embedVideo.height = sh * .61970;
			embedVideo.width = embedVideo.height * 1.8462;
			embedVideo.x = (((sw - embedVideo.width) / 2) + 5);
			embedVideo.y = Math.round(botbar_mc.y + (38 * wchange));
			embedStopx = embedVideo.x;
			embedStopy = embedVideo.y;
			embedStartx = embedVideo.x;
			embedStarty = 40 * wchange;

			linkVideo.height = sh * .61764;
			linkVideo.width = linkVideo.height * 1.84622;
			linkVideo.x = (((sw - linkVideo.width) / 2) + 5);
			linkVideo.y = Math.round(botbar_mc.y + (38 * wchange));
			linkStopx = linkVideo.x;
			linkStopy = linkVideo.y;
			linkStartx = linkVideo.x;
			linkStarty = 40 * wchange;

			if (menuSubState != "CLOSED")
			{
				switch(menuSubState)
				{
					case "ABOUT":
						break;
					case "EMBED":
						embedVideo.y = 40 * wchange;
						break;
					case "LINKS":
						linkVideo.y = 40 * wchange;
						break;
					case "EMAIL":
						emailShare.y = 40 * wchange;
						break;
				}
			}

			setChildIndex(stats, numChildren - 1);
			stats.x = 0;
			stats.y = sh - stats.height - 28;
		}


		// ******************* GET XML **********************************

		/**
		 * Handles loading XML data from Redlasso servicesm sets up the listeners, timeouts, and methods to loading
		 *
		 *@param		Uguid		String		user GUID? REvisit this - left over from Armons code - not needed
		 *@param		service		String		The service string to pull the XML data from
		 */
		private function GetXml(Uguid:String, service:String):void
		{
			getVarsInt = setTimeout(ParseVarTimeout, serviceTimeout);

			var xmlURLReq:URLRequest = new URLRequest();
			xmlURLReq.contentType = "text/xml";
			xmlURLReq.method = URLRequestMethod.GET;
			xmlURLReq.url = service;
			//xmlURLReq.data=Uguid;
			var xmlSendLoad:URLLoader = new URLLoader();
			xmlSendLoad.addEventListener(Event.COMPLETE, ParseVarInfo, false, 0, true);
			xmlSendLoad.addEventListener(IOErrorEvent.IO_ERROR, onIOError, false, 0, true);
			//tracer ("INFO","URL1: "+xmlURLReq.url);
			try
			{
				xmlSendLoad.load(xmlURLReq);
			}
			catch(err:Error)
			{
				tracer("WARN", "Error getXml load:" + err.message);
				errorDisplay("Service Failure: " + err.message);
			}
		}



		/**
		 * Handles timeout on loading getVars service timeout - set to a high value for the time being due to JIT issue on the server. When Redlasso fixes their JIT issues, we can set this to a normal value
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
			myVid.playheadUpdateInterval = 33;
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

			placeholder.addEventListener(IOErrorEvent.IO_ERROR, onIOError);
			stage.addEventListener(Event.MOUSE_LEAVE, cursorOFFscreen);
		}

		/**
		 * Handles setting up many of the UI control listeners and set initial state of UI elements
		 *
		 */
		private function moreListeners():void
		{
			//**************** Button Event Listeners
			mcPlay.addEventListener(MouseEvent.CLICK, playClip);
			mc_pp.addEventListener(MouseEvent.CLICK, playBtn);

			volbutt_mc.addEventListener(MouseEvent.CLICK, VolMove);
			mutebutt_mc.addEventListener(MouseEvent.CLICK, VolMove);

			vol.vol_mc.volKnob.addEventListener(MouseEvent.MOUSE_DOWN, MoveVol);
			vol.vol_mc.volKnob.addEventListener(MouseEvent.MOUSE_UP, StopMoveVol);

			meta_mc.toggle_mc.addEventListener(MouseEvent.MOUSE_DOWN, metaDisplay);
			CC_btn.addEventListener(MouseEvent.CLICK, CC_toggle);
			menu_mc.addEventListener(MouseEvent.CLICK, MenuMove);

			meta_mc.toggle_mc.mouseEnabled = true;
			meta_mc.toggle_mc.buttonMode = true;
			Scrub.scrubBar.mouseEnabled = false;
			Scrub.scrubBar.buttonMode = false;
			bKnob.mouseEnabled = true;
			bKnob.buttonMode = true;
			Scrub.progBar.enabled = false;
			Scrub.progBar.mouseEnabled = false;
			loader_mc.gotoAndPlay(2);
			tknob_mc.buttonMode = false;
			tknob_mc.mouseEnabled = false;

			about_scrn.addEventListener(MouseEvent.CLICK, clk_about);
			about_scrn.buttonMode = true;

			emailShare.send_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			emailShare.send_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			emailShare.send_mc.addEventListener(MouseEvent.CLICK, email_send);
			emailShare.cancel_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			emailShare.cancel_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			emailShare.cancel_mc.addEventListener(MouseEvent.CLICK, email_close);
			emailShare.send_mc.buttonMode = true;
			emailShare.cancel_mc.buttonMode = true;
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

			if (widgetMode != "TRUE")
			{
				sw = stage.stageWidth;
				sh = stage.stageHeight;
			}

			thumbnailUnavailable.width = sw;
			thumbnailUnavailable.height = sh - 28;

			thumbnailUnavailable.visible = true;
			activeBTN(false);
			lockUI(false);
			menu_mc.enabled = false;
			mc_pp.gotoAndStop(1);
			mc_pp.static_mc.enabled = false;

			loader_mc.gotoAndStop(1);
			loader_mc.visible = false;
			thumbnailUnavailable.dispText.text = "Video Temporarily Unavailable";
		}

		/**
		 * Handles loading XML data from Redlasso servicesm sets up the listeners, timeouts, and methods to loading
		 *
		   @return		String		Returns state of the flvplacyback component
		 */
		public function queryState():String
		{
			return myVid.state;
		}

		/**
		 * Handles stage change events of the FLVPlayback component/class
		 *
		 *@param		e		VideoEvent		Any event broadcast bt the player
		 */
		private function vidStateChange(e:VideoEvent):void
		{
			dispatchEvent(new Event(e.state, true, false));
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
					try
					{
						myVid.playheadTime = globalStart;
					}
					catch(e:VideoError)
					{
						tracer("WARN", "ERR8 ERROR: " + e.code + " :: " + e);
					}
					mc_pp.gotoAndStop(2);
				}
				if (e.state == "buffering" || e.state == "loading")
				{
					myVid.bufferTime = 3;
					loader_mc.visible = true;
					loader_mc.gotoAndPlay(2);
				}
				else
				{
					myVid.bufferTime = 10;
					loader_mc.gotoAndStop(1);
					loader_mc.visible = false;
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
					prevLoader.visible = false;
				}
				catch(err:Error)
				{
					tracer("WARN", "ERR9 Error prevLoader visible:" + err.message);
				}
			}
			activeBTN(false);
			lockUI(false);
			menu_mc.enabled = false;
			thumbnailUnavailable.visible = true;
			thumbnailUnavailable.dispText.text = arg;
			mc_pp.gotoAndStop(1);
			mc_pp.static_mc.enabled = false;
			mcPlay.enabled = false;
			mcPlay.visible = false;
			thumbnailUnavailable.width = sw;
			thumbnailUnavailable.height = sh - 28;
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
				thevidplays = 1;
				if (hasVideo == "FALSE")
				{
					previewPersistent = true;
					// hardcoded values for testing during dev
					// app_source = "fms3.redlasso.com";
					// m_appName = "redlasso";
					vidname = "mp3:" + vidname.substring(0, vidname.indexOf("."));
					var mvd:String = "rtmpe://" + app_source + "/" + m_appName + "/" + vidname;

					if (willAutoPlay == "TRUE")
					{
						var imgPreviewURL:String = srvString + "/svc/clip/previewImage?eid=" + initClip;
						var loaderContext:LoaderContext = new LoaderContext();
						loaderContext.checkPolicyFile = true;
						Security.loadPolicyFile("http://media.redlasso.com/crossdomain.xml");
						try
						{
							prevLoader.load(new URLRequest(imgPreviewURL), loaderContext);
						}
						catch(err:Error)
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
					// app_source = "fms3.redlasso.com";
					// m_appName = "redlasso";
					if (vidname.substr(vidname.length - 4, 4).toUpperCase() == ".MP4")
					{
						vidname = "mp4:" + vidname.substring(0, vidname.indexOf("."));
					}
					var mvd:String = "rtmpe://" + app_source + "/" + m_appName + "/" + vidname;
				}

				// hardcoded value for development
				//var mvd = "rtmpe://fms3.redlasso.com/redlasso/mp4:106x/media/2009/11/18/21/925_0163";

				tracer("INFO", "MVD: " + mvd);

				mc_pp.gotoAndStop(1);

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
						try
						{
							myVid.source = mvd;
						}
						catch(e:VideoError)
						{
							tracer("WARN", "ERR11 ERROR: " + e.code + " :: " + e);
						}
						myVid.alpha = 0;
						myVid.volume = 0;
					}
				}
				prevStream = mvd;
			}

			mcPlay.visible = false;
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
			myVid.visible = true;
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
			tracer("INFO", "Setup PlayBack");

			if (seekAndStart)
			{
				seekAndStart = false;
				try
				{
					myVid.playheadTime = seekStartPoint;
				}
				catch(e:VideoError)
				{
					tracer("WARN", "ERR12 ERROR: " + e.code + " :: " + e);
				}
			}
			else
			{
				try
				{
					myVid.playheadTime = globalStart;
				}
				catch(e:VideoError)
				{
					tracer("WARN", "ERR13 ERROR: " + e.code + " :: " + e);
				}
			}
			Scrub.addEventListener(MouseEvent.MOUSE_OVER, SetScrub);
			Scrub.addEventListener(MouseEvent.MOUSE_OUT, SetScrubOff);
			Scrub.addEventListener(MouseEvent.CLICK, SetScrubClick);
			bKnob.addEventListener(MouseEvent.MOUSE_OVER, bknob_over);
			bKnob.addEventListener(MouseEvent.MOUSE_OUT, bknob_out);
			bKnob.addEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
			bKnob.addEventListener(MouseEvent.MOUSE_UP, KnobClickUp);

			mc_pp.gotoAndStop(2);

			loader_mc.visible = false;
			if (previewFirst)
			{
				if (!previewPersistent)
				{
					try
					{
						Tweener.addTween(prevLoader, {alpha:0, time:.5, transition:"linear"});
					}
					catch(err:Error)
					{
						tracer("WARN", "ERR14 ERROR: " + err);
					}
				}
				thumbnailUnavailable.visible = false;
			}

			if (willAutoPlay == null || willAutoPlay != "TRUE")
			{
				mcPlay.visible = false;
				mc_pp.gotoAndStop(3);
				if (!previewPersistent)
				{
					if (getChildByName("placeholder"))
					{
						try
						{
							removeChild(placeholder);
						}
						catch(err:Error)
						{
							// Warning ONLY - Not critical DO NOT LOG/DISPLAY
							tracer("WARN", "ERR15 ERROR: " + err);
						}
					}
				}
				try
				{
					myVid.play();
				}
				catch(e:VideoError)
				{
					tracer("WARN", "ERR16 ERROR: " + e.code + " :: " + e);
				}
				myVid.visible = false;
			}
			else
			{
				mcPlay.visible = false;
				mc_pp.gotoAndStop(3);
				try
				{
					myVid.play();

				}
				catch(e:VideoError)
				{
					tracer("WARN", "ERR17 ERROR: " + e.code + " :: " + e);
				}

			}
			if (willAutoPlay == "TRUE")
			{
				//loggedPlay = false;
			}
			logPlay();
		}

		// ******************* Data Functions *****************************

		/**
		 * Handles prep and loading of the XML/REST Redlasso services "getVars"
		 *
		 */
		private function GetVarInfo():void
		{
			if (!varsLoaded || (varsLoaded && widgetMode == "TRUE"))
			{
				// old call svc/clip/getVars
				// var getVarService:String = srvString + "/svc/vars/getVars?eid=" + guid + "&pid=" + guid + debugService1;
				xmlType = "Var";
				var getVarService:String = srvString + "/svc/clip/getVars?fid=" + guid + "&pid=" + guid + debugService1;
				tracer("INFO", "LOAD GETVARS URL: " + getVarService);
				GetXml(guid, getVarService);
			}
			else
			{
				tracer("INFO", "Grabbing app/streamname");
				GetNSConnection(m_appName);
			}

		}


		/**
		 * Handles retrieving closed caption data from custom Redlasso service
		 *
		 *@param		gStart		Number		where to start closed caption data from in seconds
		 *@param		gStop		Number		where to end closed caption data from in seconds
		 */
		private function GetClosedCapInfo(gStart:Number, gStop:Number)
		{
			xmlType = "CC";
			var CCInfo:String = srvString + "/svc/cc/getClosedCaption?fid=" + guid + "&startTime=" + gStart + "&endTime=" + gStop;
			// tracer ("INFO","CCINFO: "+CCInfo);
			captioning.flvPlayback = myVid;
			try
			{
				captioning.source = CCInfo;
			}
			catch(err:Error)
			{
				tracer("WARN", "ERR18 CCDATA LOAD ERROR: " + err);
			}
			captioning.autoLayout = false;
			captioning.flvPlaybackName = myCap.text;
			captioning.addEventListener("captionChange", onCaptionChange);
		}
		
		// helper function for CC button display
		private function CaptionFalse(e:Event)
		{
			captioning.showCaptions = false;
		}
		
		// helper function for CC display to handle upward scrolling CC text
		private function captionClear():void
		{
			myCap.text = "    ";
			dispCap.text = "";
			ccDisplay.splice(0);
			ccDisplayCnt = 3;
		}
		
		// helper function for CC display to handle upward scrolling CC text
		private function onCaptionChange(e:*):void
		{
			if (ccLastText != myCap.text && myCap.text != "")
			{
				ccDisplayCnt++;
				ccDisplay[ccDisplayCnt] = myCap.text;
				dispCap.text = ccDisplay[ccDisplayCnt - 2] + ccDisplay[ccDisplayCnt - 1] + ccDisplay[ccDisplayCnt];
			}
			ccLastText = myCap.text;
		}

		// Helper to load new page when station logo is clicked in metadata dropdown display
		private function clkStation(e:Event)
		{
			tracer("INFO", "GOING TO: " + stURL.url);
			navigateToURL(stURL);
		}

/**
		 * Handler for parsing player/clip config data from custom Redlasso getVars service
		 *
		 *@param		e		Event		data event from XML retreived from getVars consolidated service
		 */
		private function ParseVarInfo(e:Event)
		{
			clearTimeout(getVarsInt);

			var clpXML:XML = new XML(e.target.data);
			hasVideo = String(clpXML.vars.@hasVideo).toUpperCase();
			tracer("INFO", "HAS VIDEO = " + hasVideo);
			if (!previewFirst)
			{
				if (hasVideo == "TRUE")
				{
					try
					{
						LoadThumb(clpXML.vars.@thumbUrl, clpXML.vars.@videoWidth, clpXML.vars.@videoHeight);
					}
					catch(err:Error)
					{
						tracer("WARN", "ERR19 ERROR: " + err);
					}
				}
				else
				{
					previewPersistent = true;
					audioGraphic = clpXML.vars.@audioGraphic;
					try
					{
						LoadThumb(clpXML.vars.@audioGraphic, clpXML.vars.@videoWidth, clpXML.vars.@videoHeight);
					}
					catch(err:Error)
					{
						tracer("WARN", "ERR20 ERROR: " + err);
					}
				}
			}
			var slogoSource:String = clpXML.clipinfo.@stationLogoUrl;
			meta_mc.station_logo.addEventListener(Event.COMPLETE, stationLogocompleteHandler);
			try
			{
				meta_mc.station_logo.source = slogoSource;
			}
			catch(err:Error)
			{
				tracer("WARN", "ERR21 ERROR: " + err);
			}

			stURL = new URLRequest(clpXML.clipinfo.@stationUrl);
			var stime:int = Number(clpXML.clipinfo.@startTime);
			var etime:int = Number(clpXML.clipinfo.@endTime);
			ftime = etime - stime;

			timeDisplayTot.text = timeCode(ftime);
			vidname = clpXML.vars.@fileName;
			m_appName = clpXML.vars.@appNameFMS;
			app_source = clpXML.vars.@streamUrl;
			globalStart = stime;
			globalStop = etime;
			_fileId = clpXML.@rid;
			tracer("INFO", "FILE ID/Embed ID: " + _fileId);
			_linkUrl = clpXML.vars.@linkUrl;
			linkVideo.urlTxt.text = _linkUrl + _fileId;
			embedUrl = clpXML.vars.@embedUrl;
			userId = clpXML.vars.@userId;
			hasCaption = clpXML.vars.@hasCaption;

			// CLIP INFO NODE ITEMS
			meta_mc.visible = true;
			var clpETime:Number = Number(clpXML.clipinfo.@endTime);
			var clpSTime:Number = Number(clpXML.clipinfo.@startTime);
			var clpData:Number = clpETime - clpSTime;
			var clpInfoTime:String = timeCode(clpData);

			// META INFO POPULATION CALL
			clipName = clpXML.clipinfo.@title;
			clipDesc = decodeURI(clpXML.clipinfo);
			meta1_textString = clipName;
			meta_mc.meta1Txt.autoSize = "left";
			meta_mc.meta1Txt.text = clipName;
			meta_mc.meta2Txt.autoSize = "left";
			meta_mc.meta3Txt.autoSize = "left";
			meta_mc.meta2Txt.text = "Duration: " + clpInfoTime + " | Views: " + clpXML.clipinfo.@views + " | Topic: " + clpXML.clipinfo.@categoryName;
			meta_mc.meta3Txt.text = "Clipper: " + clpXML.clipinfo.@author + " | " + "Aired: " + clpXML.clipinfo.@airdate;
			rid = clpXML.@rid;

			

			if (hasCaption)
			{
				if (hasCaption.toUpperCase() == "TRUE")
				{
					GetClosedCapInfo(globalStart, globalStop);
					CC_btn.visible = true;
					CC_btn.enabled = true;
				}
				else
				{
					CC_btn.visible = false;
					CC_btn.enabled = false;
				}
			}

			if (String(clpXML.@msg) == "Invalid ID")
			{
				varsLoaded = true;
				errorDisplay("INVALID SERVICE DATA.\nPLEASE TRY AGAIN LATER.");
			}
			else
			{
				if (clpXML.vars.@emailFrom != null)
				{
					emailShare.fromemailTxt.text = clpXML.vars.@emailFrom;
				}
				if (_fileId == null || _fileId == undefined || _fileId == "")
				{
					errorDisplay("INVALID SERVICE DATA.\nPLEASE TRY AGAIN LATER.");
				}
				else
				{
					dataLoaded = true;
					varsLoaded = true;
					assignEmbed();

					if (userClicked || widgetMode == "TRUE" && userClicked)
					{
						GetNSConnection(m_appName);
					}
					userClicked = true;
					clipLoaded = true;
					truncMeta(meta_mc.meta1Txt, sw - 85);
					// truncMeta (meta_mc.meta2Txt, sw-85);
					// truncMeta (meta_mc.meta3Txt, sw-85);
				}
			}
		}


		
/**
		 * Handler for when stationlogo has been loaded
		 *
			*@param		e		Event		success event data
		 */
		private function stationLogocompleteHandler(event:Event):void
		{
			meta_mc.station_logo.mouseEnabled = true;
			try
			{
				if (meta_mc.station_logo.content is Bitmap)
				{
					Bitmap(meta_mc.station_logo.content).smoothing = true;
				}
			}
			catch(err:Error)
			{
				// Warning ONLY - Not critical DO NOT LOG/DISPLAY
				// tracer ("WARN","ERR22 ERROR: "+err);
			}
			meta_mc.station_logo.buttonMode = true;
			meta_mc.station_logo.addEventListener(MouseEvent.CLICK, clkStation);
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
			while(checkMetricWidth(str) >= widthLimit)
			{
				var a:Array = str.text.split(" ");
				a.length--;
				str.text = a.join(" ") + "…";
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
		 * Loads an image into placeholder displayobject - used in several places
		 *
			*@param		e		Event		success event data
		 */
		private function LoadThumb(img, wid, hgt)
		{
			placeholder.y = 0;
			placeholder.height = sh - (28 * wchange);
			placeholder.width = placeholder.height * 1.33;
			placeholder.x = (sw - placeholder.width) / 2;
			placeholder.scaleContent = true;
			tracer("INFO", "THUMB: " + img);
			if (img != "http://images.redlasso.com/thumbnail-unavailable-160.gif")
			{
				try
				{
					placeholder.load(new URLRequest(img));
				}
				catch(err:Error)
				{
					tracer("WARN", "ERR23 placeholder error: " + err);
				}
				// tracer ("TYPE:"+placeholder);
				if (placeholder.content is Bitmap)
				{
					Bitmap(placeholder.content).smoothing = true;
				}

				addChild(placeholder);
				setChildIndex(placeholder, 2);
			}
			myVid.visible = false;
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
				fulltime = (h < 10 ? "0" + h.toString() : h.toString()) + ":" + (m < 10 ? "0" + m.toString() : m.toString()) + ":" + (s < 10 ? "0" + s.toString() : s.toString());
			}
			else
			{
				fulltime = (m < 10 ? "0" + m.toString() : m.toString()) + ":" + (s < 10 ? "0" + s.toString() : s.toString());
			}
			return fulltime;
		}

		//**************** Interface Functions ****************************************

/**
		 * Handler for cursor leaving screen
		 *
			*@param		e		Event		success event data
		 */
		private function cursorOFFscreen(evt:Event):void
		{
			// bottomBarSlideOff ();
			Tweener.addTween(CC_btn, {alpha:0, time:.3, transition:"linear"});
			Tweener.addTween(meta_mc, {alpha:0, time:.3, transition:"linear"});
			stage.addEventListener(MouseEvent.MOUSE_MOVE, cursorONscreen);
		}
/**
		 * Handler for cursor entering screen
		 *
			*@param		e		Event		success event data
		 */
		private function cursorONscreen(evt:MouseEvent):void
		{
			//tracer("INFO","PHASE: "+evt.eventPhase);
			if (mouseX > 0 && mouseX <= stage.stageWidth && mouseY > 0 && mouseY < stage.stageHeight)
			{
				// bottomBarSlideOn ();
				Tweener.addTween(CC_btn, {alpha:1, time:.3, transition:"linear"});
				Tweener.addTween(meta_mc, {alpha:1, time:.3, transition:"linear"});
				stage.removeEventListener(MouseEvent.MOUSE_MOVE, cursorONscreen);
			}
		}

/**
		 * Function for future use to handle sliding bottom UI controls onscreen
		 *
			*@param		e		Event		success event data
		 */
		private function bottomBarSlideOn():void
		{
			Tweener.addTween(botbar_mc, {alpha:1, time:.3, transition:"linear"});
			Tweener.addTween(timeDisplayCur, {alpha:1, time:.3, transition:"linear"});
			Tweener.addTween(timeDisplayTot, {alpha:1, time:.3, transition:"linear"});
			if (widgetMode == "TRUE")
			{
			}
			else
			{
				Tweener.addTween(fullScreen, {alpha:1, time:.3, transition:"linear"});
			}
			Tweener.addTween(menu_mc, {alpha:1, time:.3, transition:"linear"});
			Tweener.addTween(volbutt_mc, {alpha:1, time:.3, transition:"linear"});
			Tweener.addTween(mutebutt_mc, {alpha:1, time:.3, transition:"linear"});
			Tweener.addTween(mc_pp, {alpha:1, time:.3, transition:"linear"});
			Tweener.addTween(Scrub, {alpha:1, time:.3, transition:"linear"});
		// Tweener.addTween (loader_mc, {alpha:0, time:.3, transition:"linear"});
		//Tweener.addTween (tknob_mc, {alpha:0, time:.3, transition:"linear"});

		//Tweener.addTween (bKnob, {alpha:0, time:.3, transition:"linear"});

		}
/**
		 * Function for future use to handle sliding bottom UI controls OFFscreen
		 *
			*@param		e		Event		success event data
		 */
		private function bottomBarSlideOff():void
		{
			Tweener.addTween(botbar_mc, {alpha:0, time:.3, transition:"linear"});
			Tweener.addTween(timeDisplayCur, {alpha:0, time:.3, transition:"linear"});
			Tweener.addTween(timeDisplayTot, {alpha:0, time:.3, transition:"linear"});
			if (widgetMode == "TRUE")
			{
			}
			else
			{
				Tweener.addTween(fullScreen, {alpha:0, time:.3, transition:"linear"});
			}
			Tweener.addTween(menu_mc, {alpha:0, time:.3, transition:"linear"});
			Tweener.addTween(volbutt_mc, {alpha:0, time:.3, transition:"linear"});
			Tweener.addTween(mutebutt_mc, {alpha:0, time:.3, transition:"linear"});
			Tweener.addTween(mc_pp, {alpha:0, time:.3, transition:"linear"});
			Tweener.addTween(Scrub, {alpha:0, time:.3, transition:"linear"});
		//Tweener.addTween (loader_mc, {alpha:0, time:.3, transition:"linear"});
		//Tweener.addTween (tknob_mc, {alpha:0, time:.3, transition:"linear"});
		//Tweener.addTween (bKnob, {alpha:0, time:.3, transition:"linear"});
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
			bKnobStat = 0;
			clearInterval(progInt);
			clearInterval(overInt);
			clearInterval(volInt);
			tknob_mc.alpha = 0;
			bKnob.alpha = 0;
			Tweener.addTween(vol.vol_mc, {x:0, y:150, time:.3, transition:"linear"});
			stage.removeEventListener(MouseEvent.MOUSE_UP, mouseUp);
		}


/**
		 * Handles display of dropdown metainfo display
		 *
			*@param		e		Event		success event data
		 */
		private function metaDisplayCore():void
		{
			var metaDest:Number = (-1 * meta_mc.height) + 10;

			if (meta_mc.y != 0)
			{
				Tweener.addTween(meta_mc, {y:0, time:.3, transition:"easeOutQuint"});
				Tweener.addTween(meta_mc.toggle_mc, {rotation:-180, time:.5, transition:"easeInOutBack"});
				metaState = true;
			}
			else
			{
				Tweener.addTween(meta_mc, {y:mcStopy, time:.3, transition:"easeInQuint"});
				Tweener.addTween(meta_mc.toggle_mc, {rotation:0, time:.5, transition:"easeInOutBack"});
				metaState = false;
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
			// tracer("INFO",e.target.playheadTime+ " "+Math.floor(e.target.playheadTime)+" - "+globalStop+" "+myVid.totalTime)
			

			var realTime:Number = e.target.playheadTime - globalStart;
			timeDisplayCur.text = timeCode((realTime < 0) ? 0 : realTime);
			var BarWid:Number = Scrub.scrubBar.width;
			var BarDis:Number = BarWid / ftime;
			var BarSet:Number = realTime * BarDis;
			var bdis_S:String = String(BarSet);
			Scrub.progBar.width = BarSet;

			if (e.target.playheadTime < globalStart)
			{
				try
				{
					myVid.playheadTime = globalStart;
				}
				catch(e:VideoError)
				{
					tracer("WARN", "ERR24 ERROR: " + e.code + " :: " + e);
				}
			}

			if (e.target.playheadTime > globalStop || e.target.playheadTime >= myVid.totalTime)
			{
				showMenu();

				try
				{
					myVid.playheadTime = globalStart;
				}
				catch(e:VideoError)
				{
					tracer("WARN", "ERR25 ERROR: " + e.code + " :: " + e);
				}
				myVid.pause();
				try
				{
					myVid.playheadTime = globalStart;
				}
				catch(e:VideoError)
				{
					tracer("WARN", "ERR26 ERROR: " + e.code + " :: " + e);
				}
				mc_pp.gotoAndStop(2);

			}
		}


		

/**
		 * Function for handling initial video startup/play log some events
		 *
			*@param		e		Event		FLVPlayback event
		 */
		private function logInitialPlay(evt:Event):void
		{
			myVid.volume = userVolume;

			if (!previewPersistent)
			{
				Tweener.addTween(myVid, {alpha:1, time:.3, transition:"linear"});
			}

			
			if (!loggedPlay)
			{
				tracer("INFO", "PlayState Entered, logging clip play");
				logPlay();
			}
		}


/**
		 * Function for handling logging data to logClipPlay service on playback by user
		 *
		 */
		private function logPlay():void
		{
			if (!loggedPlay)
			{
				var DOMResults = queryDOM();
				var playingSend:URLLoader = new URLLoader();
				var pUrl:URLRequest = new URLRequest(srvString + "/svc/vars/logClipPlay?play=true&userGUID=" + userId + "&fid=" + guid + "&ref=" + escape(DOMResults));
				tracer("INFO", "LOGCLIPPLAY URL: " + pUrl.url);
				pUrl.method = URLRequestMethod.GET;
				try
				{
					playingSend.load(pUrl);
					tracer("INFO", "Clip Playback Logged");
					loggedPlay = true;
				}
				catch(err:Error)
				{
					tracer("WARN", "ERR27 LOGCLIPPLAY ERROR: " + err);
				}

			}
		}

/**
		 * Function for handling initial video playback via user control of play button
		 *
			*@param		e		Event		FLVPlayback event
		 */
		private function playClip(e:Event)
		{
			tracer("INFO", "LARGE PLAY CLICKED");
			var evt:Event = new Event("RLPlayer_PLAY_CLICKED", true, false);
			dispatchEvent(evt);
			// Log the playback of a clip
			logPlay();
			if (!previewPersistent)
			{
				try
				{
					removeChild(placeholder);
				}
				catch(err:Error)
				{
					tracer("WARN", "ERR28 ERROR: " + err);
				}
			}
			mcPlay.visible = false;
			myVid.visible = false;
			mc_pp.gotoAndStop(3);

			
				try
				{
					tracer("INFO", "WAS PLAY!");
					myVid.play();
				}
				catch(e:VideoError)
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
			switch(mc_pp.currentFrame)
			{
				case 1:
					//tracer("INFO","CASE 1");
					break;
				case 2:
					//tracer("INFO","CASE 2");
					mc_pp.gotoAndStop(3);
					if (mcPlay.visible == true)
					{
						if (!previewPersistent)
						{
							if (getChildByName("placeholder"))
							{
								removeChild(placeholder);
							}
						}
						mcPlay.visible = false;
						myVid.visible = true;
					}

					
					if (myVid.playheadTime > globalStop)
					{
						try
						{
							myVid.play();
						}
						catch(e:VideoError)
						{
							tracer("WARN", "ERR1 ERROR: " + e.code + " :: " + e);
						}
						try
						{
							myVid.playheadTime = globalStart;
						}
						catch(e:VideoError)
						{
							tracer("WARN", "ERR2 ERROR: " + e.code + " :: " + e);
						}
						menuState = "OPEN";
						MenuMove(e);
					}
					else
					{
						menuState = "OPEN";
						MenuMove(e);
						try
						{
							myVid.play();
						}
						catch(e:VideoError)
						{
							tracer("WARN", "ERR3 ERROR: " + e.code + " :: " + e);
						}
					}
					break;
				case 3:
					//tracer("INFO","CASE 3");
					mc_pp.gotoAndStop(2);
					myVid.pause();
					menuState = "CLOSED";
					MenuMove(e);
					break;
				default:
			// tracer ("INFO","Neither 1, 2, 3 was selected");
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
					bKnob.alpha = 0;
					bigScreen = true;
					stage.displayState = StageDisplayState.NORMAL;
				}
				else
				{
					bigScreen = false;
					stage.displayState = StageDisplayState.FULL_SCREEN;
					bKnob.alpha = 0;
				}

				if (widgetMode != "TRUE")
				{
					sw = stage.stageWidth;
					sh = stage.stageHeight;
				}
				else
				{
					sw = this.parent.width;
					sh = this.parent.height;
				}

				
				SizeItems();
			}
		}

		private function checkPreview():Boolean
		{
			if (willAutoPlay != "TRUE")
			{
				myVid.alpha = .75;
			}
			return true;
		}

		private function fadeOverlay(arg:Boolean):void
		{
			if (arg)
			{
				Tweener.addTween(overlay_mc, {alpha:.65, time:.3, transition:"linear"});
			}
			else
			{
				Tweener.addTween(overlay_mc, {alpha:0, time:.3, transition:"linear"});
			}
		}

		private function showMenu():void
		{
			// tracer ("INFO","SHOWING MENU: "+myVid.state);
			if (!userClicked)
			{
				GetVarInfo();
			}
			fadeOverlay(false);

			if (myVid.state == "disconnected")
			{
				playToggle = true;
			}
			else
			{
				playToggle = false;
			}

			if (playToggle)
			{
				mcPlay.visible = true;
				mcPlay.enabled = true;
			}
			else
			{
				mcPlay.visible = false;
				mcPlay.enabled = false;

			}
			dispCap.alpha = .65;

			placeholder.alpha = .3;
			Tweener.addTween(emailShare, {x:emailStopx, y:emailStopy, time:.3, transition:"linear"});
			Tweener.addTween(embedVideo, {x:embedStopx, y:embedStopy, time:.3, transition:"linear"});
			Tweener.addTween(linkVideo, {x:linkStopx, y:linkStopy, time:.3, transition:"linear"});
			Tweener.addTween(menubg, {x:menuStartx, y:menuStarty, time:.5, transition:"easeOutQuint"});
			menuState = "OPEN";
			menuSubState = "CLOSED";
		}

		private function closeMenu():void
		{
			menuState = "CLOSED";

			activeBTN(true);
			fadeOverlay(false);

			if (myVid.state == "disconnected")
			{
				mcPlay.visible = true;
				mcPlay.enabled = true;
			}
			else
			{
				mcPlay.visible = false;
				mcPlay.enabled = false;
			}
			placeholder.alpha = 1;
			about_scrn.visible = false;
			Tweener.addTween(menubg, {x:menuStopx, y:menuStopy, time:.5, transition:"easeInQuint"});
			dispCap.alpha = 1;
		}

		private function closeMenuandSharing():void
		{
			about_scrn.visible = false;
			Tweener.addTween(emailShare, {x:emailStopx, y:emailStopy, time:.3, transition:"linear"});
			Tweener.addTween(embedVideo, {x:embedStopx, y:embedStopy, time:.3, transition:"linear"});
			Tweener.addTween(linkVideo, {x:linkStopx, y:linkStopy, time:.3, transition:"linear"});
			fadeOverlay(false);
			mcPlay.enabled = true;
			mcPlay.visible = true;
			activeBTN(true);
			menuState = "CLOSED";
		}

		private function MenuMove(e:Event)
		{
			// tracer ("INFO","PREVIOUS MENU STATE: "+menuState);
			if (menuState == "CLOSED")
			{
				showMenu();
			}
			else
			{
				if (menuState == "SHARING")
				{
					closeMenuandSharing();
					if (myVid.state == "disconnected")
					{
						mcPlay.visible = true;
						mcPlay.enabled = true;
					}
					else
					{
						mcPlay.visible = false;
						mcPlay.enabled = false;

					}
				}
				else
				{
					closeMenu();
				}
			}
		//tracer ("INFO","CURRENT MENU STATE: "+menuState);
		}

		private function VolMove(e:Event)
		{
			if (vol.vol_mc.y != 20)
			{
				Tweener.addTween(vol.vol_mc, {x:0, y:20, time:.3, transition:"linear"});
			}
			else
			{
				Tweener.addTween(vol.vol_mc, {x:0, y:150, time:.3, transition:"linear"});
			}
		}

		private function MoveVol(e:Event)
		{
			var myRectangle:Rectangle = new Rectangle(8, -100, 0, 100);
			vol.vol_mc.volKnob.startDrag(false, myRectangle);
			stage.addEventListener(MouseEvent.MOUSE_UP, mouseUp);
			volInt = setInterval(updateVol, 100);
		}

		private function StopMoveVol(e:Event)
		{
			vol.vol_mc.volKnob.stopDrag();
			clearInterval(volInt);
			Tweener.addTween(vol.vol_mc, {x:0, y:150, time:.3, transition:"linear"});
		}

		private function saveVolume(vol:Number)
		{
			localData.data.storedVolume = vol;
			localData.flush();
		}


		private function getVolume():void
		{
			if (localData.data.storedVolume != undefined)
			{
				userVolume = localData.data.storedVolume;
				tracer("INFO", "Volume: " + userVolume);
			}
			else
			{
				userVolume = .75;
				tracer("INFO", "DEfault volume: " + userVolume);
			}
			myVid.volume = userVolume;
			vol.vol_mc.volKnob.y = userVolume * -100;
			vol.vol_mc.volTube.height = Math.abs(vol.vol_mc.volKnob.y);
			if (userVolume <= 0)
			{
				mutebutt_mc.visible = true;
				volbutt_mc.visible = false;
			}
			else
			{
				mutebutt_mc.visible = false;
				volbutt_mc.visible = true;
			}
		}

		private function updateVol()
		{
			vol.vol_mc.volTube.height = Math.abs(vol.vol_mc.volKnob.y);
			var userVolume2:Number = Math.abs((vol.vol_mc.volKnob.y));
			userVolume2 = userVolume2 / 100;
			// tracer ("INFO",userVolume2);
			myVid.volume = userVolume2;
			saveVolume(userVolume2);
			if (userVolume2 <= 0)
			{
				mutebutt_mc.visible = true;
				volbutt_mc.visible = false;
			}
			else
			{
				mutebutt_mc.visible = false;
				volbutt_mc.visible = true;
			}

		}

		private function SetScrub(evt:Event)
		{
			scrubInt = setInterval(updateScrub, 10);
		//tracer("INFO",Scrub.scrubBar.mouseX)
		}

		private function SetScrubOff(evt:Event)
		{
			clearInterval(scrubInt);
			tknob_mc.alpha = 0;
			bKnob.alpha = 0;
			if (mouseY < bKnob.y)
			{
				bKnob.alpha = 0;
			}
		}

		private function updateScrub()
		{
			// tracer ("INFO",Scrub.scaleX);
			var scWid:Number = Scrub.width;
			var scSepDis:Number = ftime / scWid;
			var scPlace:Number = Scrub.mouseX * Scrub.scaleX;
			var scVal:Number = scPlace * scSepDis;
			scVal = Math.round(scVal);
			var scTime:String = timeCode(scVal);

			// tracer ("INFO","Bar Width: "+scWid+"  scSepDis:"+scSepDis+"  Mouse Scrub: "+scPlace);
			bKnob.alpha = 1;
			tknob_mc.alpha = 1;
			var scPla:Number = Scrub.x;
			var scTtl:Number = scWid + scPla;
			//tracer("INFO","if mouseX is less than: "+scTtl +"   "+"And mouseX is greater than "+mouseX)
			if (mouseX < scTtl && mouseX > Scrub.x)
			{
				tknob_mc.x = mouseX;
				tknob_mc.newTime.text = scTime;
			}
			if (bKnobStat == 0)
			{
				// tracer ("INFO",Scrub.progBar.width);
				bKnob.x = Scrub.x + (Scrub.progBar.width * Scrub.scaleX);
			}
		}

		private function KnobClickDwn(e:Event)
		{
			bKnobStat = 1;
			var knoby:int = Math.round(botbar_mc.y + 4);
			stage.addEventListener(MouseEvent.MOUSE_UP, mouseUp);
			var myRectangle:Rectangle = new Rectangle(Scrub.x, knoby, Scrub.width, 1);
			bKnob.startDrag(false, myRectangle);
			progInt = setInterval(updateProg, 33);
			overInt = setInterval(updateScrub, 33);
			clearInterval(vidTimeInt);
		}

		private function KnobClickUp(e:Event)
		{
			bKnobStat = 0;
			// tracer ("INFO","KNOB UP");
			bKnob.stopDrag();
			clearInterval(progInt);
			clearInterval(overInt);
			clearInterval(volInt);
			tknob_mc.alpha = 0;
			bKnob.alpha = 0;
			BarLenSeek();
		}

		private function KnobClickOut(e:Event)
		{
			bKnob.stopDrag();
			clearInterval(progInt);
			clearInterval(overInt);
			clearInterval(volInt);
			tknob_mc.alpha = 0;
			bKnob.alpha = 0;
		}

		private function bknob_over(e:Event)
		{
			bKnob.alpha = 1;
		}

		private function bknob_out(e:Event)
		{
			bKnob.alpha = 0;
		}

		private function updateProg()
		{
			var scWidth:Number = Scrub.width;
			var scPla:Number = Scrub.x;
			var scTtl:Number = scWidth + scPla;
			if (mouseX < scTtl)
			{
				Scrub.progBar.width = Scrub.mouseX;
			}
		}

		private function BarLenSeek()
		{
			var ProgWid:Number = Scrub.scrubBar.width;
			var BarRatio:Number = ProgWid / ftime;
			var BarSet:Number = Scrub.progBar.width / BarRatio;
			BarSet = Math.round(BarSet);
			BarSet = BarSet + globalStart;
			try
			{
				myVid.playheadTime = BarSet;
			}
			catch(e:VideoError)
			{
				tracer("WARN", "ERR4 ERROR: " + e.code + " :: " + e);
			}
		}

		private function SetScrubClick(e:Event)
		{
			var scrubWid:Number = Scrub.width;
			var scrubVal:Number = ftime / scrubWid;
			var scrubPlace:Number = Scrub.mouseX * Scrub.scaleX;
			scrubVal = scrubPlace * scrubVal;
			scrubVal = Math.round(scrubVal);

			var realTime:Number = scrubVal;
			var BarDis:Number = Scrub.scrubBar.width / ftime;
			var BarSet:Number = realTime * BarDis;
			var bdis_S:String = String(BarSet);
			Scrub.progBar.width = BarSet;
			BarLenSeek();
		}

		private function CC_toggle(e:Event)
		{
			if (capback.visible == true)
			{
				capback.visible = false;
				myCap.visible = false;
				dispCap.visible = false;
			}
			else
			{
				capback.visible = true;
				myCap.visible = false;
				dispCap.visible = true;
			}
		}


		//*************** About Screen ******************
		private function clk_about(e:Event)
		{
			about_scrn.visible = false;
			if (menuState == "OPEN")
			{
				fadeOverlay(false);
				about_scrn.rotationY = 0;
				menuState = "SHARING";
			}
			menuState = "CLOSED";
			menuSubState = "CLOSED";
			MenuMove(e);
		}


		//*************** Share this video *****************
		private function email_send(e:Event)
		{
			myTimer.reset();
			myTimer.stop();
			var emailCheck:Number = 0;
			var emailValid1:Boolean;

			var strHelper:StringHelper = new StringHelper();

			var email2:Array = emailShare.toemailTxt.text.split(",");
			if (email2.length > 1)
			{
				for(var loop = 0; loop < email2.length; loop++)
				{
					email2[loop] = strHelper.trim(email2[loop], " ");
					//tracer ("INFO",loop+": "+email2[loop]);
					if (!isValidEmail(email2[loop]))
					{
						emailCheck++;
					}
				}
			}
			else
			{
				emailValid1 = isValidEmail(emailShare.toemailTxt.text);
			}
			if (emailCheck > 0)
			{
				emailValid1 = false;
			}
			else
			{
				emailValid1 = true;
			}
			var emailValid2:Boolean = isValidEmail(emailShare.fromemailTxt.text);

			//tracer ("INFO",emailValid1+"   "+emailValid2);
			if (emailValid1 == true && emailValid2 == true)
			{
				var Emailxml:XML =<root rid="" sid="00000000-0000-0000-0000-000000000000">
						<message addresses=" " emailFrom=" "> </message>
					</root>;
				Emailxml.@rid = guid;
				if (emailFrom != null)
				{
					emailShare.fromemailTxt.text = emailFrom;
				}
				Emailxml.message.@emailFrom = emailShare.fromemailTxt.text;
				Emailxml.message.@addresses = emailShare.toemailTxt.text;
				Emailxml.message = emailShare.msgTxt.text;
				var xmlSendLoad5:URLLoader = new URLLoader();
				var xmlURLReq5:URLRequest = new URLRequest(srvString + "/svc/message/sendMail?fid=" + guid);
				xmlURLReq5.data = Emailxml;
				xmlURLReq5.contentType = "application/x-www-form-urlencoded";
				xmlURLReq5.method = URLRequestMethod.POST;
				try
				{
					xmlSendLoad5.load(xmlURLReq5);
				}
				catch(err:Error)
				{
					trace("Error sending email: " + err.message);
				}
				emailShare.statusTxt.text = "Sending...";
				xmlSendLoad5.addEventListener(Event.COMPLETE, onEmailComplete, false, 0, true);
			}
			else
			{
				// indicate error
				emailShare.statusTxt.text = "Invalid email format. Check fields for errors.";
			}
		}

		private function onEmailComplete(evt:Event):void
		{
			try
			{
				emailShare.statusTxt.text = "Email Sent";
				myTimer.start();
			}
			catch(err:TypeError)
			{
				myTimer.reset();
				myTimer.stop();
				emailShare.statusTxt.text = "An error occured";
			}
		}

		private function emailCloseCore():void
		{
			myTimer.stop();
			activeBTN(true);
			menuState = "OPEN";
			menuSubState = "CLOSED";
			Tweener.addTween(emailShare, {x:emailStopx, y:emailStopy, time:.3, transition:"linear"});

			showMenu();

			if (hasVideo == "FALSE")
			{
				if (prevLoader)
				{
					prevLoader.alpha = 1;
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
			var emailExpression:RegExp = /^[a-z][\w.-]+@\w[\w.-]+\.[\w.-]*[a-z][a-z]$/i;
			return emailExpression.test(email);
		}


		//**************Embed Code****************

		private function assignEmbed()
		{
			if (!dataLoaded && embedUrl == null || embedUrl == undefined || embedUrl == "")
			{
				embedUrl = LoaderInfo(this.root.loaderInfo).url;

			}
			if (_fileId == "" || _fileId == undefined || _fileId == null)
			{
				_fileId = initClip;
			}
			embedVideo.embedTxt.text = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" width=\"390\" height=\"320\" id=\"Redlasso\"><param name=\"movie\" value=\"" + embedUrl + "\" /><param name=\"flashvars\" value=\"" + fvars + "\" /><param name=\"allowScriptAccess\" value=\"always\" /><param name=\"allowFullScreen\" value=\"true\" /><embed src=\"" + embedUrl + "\" flashvars=\"" + fvars + "\" width=\"390\" height=\"320\" type=\"application/x-shockwave-flash\" allowScriptAccess=\"always\" allowFullScreen=\"true\" name=\"Redlasso\"></embed></object>";
			embedVideo.WPembedTxt.text = "[redlasso id=\"" + _fileId + "\"" + "]";
		}


		private function link_close(e:Event)
		{
			activeBTN(true);
			menuState = "OPEN";
			menuSubState = "CLOSED";
			Tweener.addTween(embedVideo, {x:embedStopx, y:embedStopy, time:.3, transition:"linear"});
			showMenu();
		}

		private function copy(e:Event)
		{
			menuState = "OPEN";
			activeBTN(true);
			System.setClipboard(embedVideo.embedTxt.text);
			Tweener.addTween(embedVideo, {x:embedStopx, y:embedStopy, time:.3, transition:"linear"});
			showMenu();
			popupDisplay("Copied to Clipboard", 500, 2500);
		}

		private function WPcopy(e:Event)
		{
			menuState = "OPEN";
			activeBTN(true);
			System.setClipboard(embedVideo.WPembedTxt.text);
			Tweener.addTween(embedVideo, {x:embedStopx, y:embedStopy, time:.3, transition:"linear"});
			showMenu();
			popupDisplay("Copied to Clipboard", 500, 2500);
		}

		private function hidePopup(arg:Number):void
		{
			setTimeout(function()
				{
					popup.visible = false;
				}, arg);
		}

		private function popupDisplay(arg, delay, duration):void
		{
			if (widgetMode != "TRUE")
			{
				sw = stage.stageWidth;
				sh = stage.stageHeight;
			}
			popup.x = sw / 2 - popup.width / 2;
			popup.y = (sh - 28) / 2 - popup.height / 2;
			popup.msgTxt.text = arg;
			setTimeout(function()
				{
					popup.visible = true;
				}, delay);
			hidePopup(duration);
		}

		private function Lcopy(e:Event)
		{
			menuState = "OPEN";
			activeBTN(true);
			System.setClipboard(linkVideo.urlTxt.text);
			Tweener.addTween(linkVideo, {x:linkStopx, y:linkStopy, time:.3, transition:"linear"});
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
			embedVideo.cancel_mc.buttonMode = true;
			embedVideo.WPcancel_mc.buttonMode = true;
			embedVideo.WPcopy_mc.buttonMode = true;
			embedVideo.copy_mc.buttonMode = true;
			linkVideo.lcopy_mc.addEventListener(MouseEvent.CLICK, Lcopy);
			linkVideo.lcopy_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			linkVideo.lcopy_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			linkVideo.cancel_mc.addEventListener(MouseEvent.CLICK, linkVideoCancel);
			linkVideo.cancel_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			linkVideo.cancel_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			linkVideo.cancel_mc.buttonMode = true;
			linkVideo.lcopy_mc.buttonMode = true;
		}

		//**************BookMark Code****************
		private function clk_newsvine_mc(e:Event)
		{
			var url:String = "http://www.newsvine.com/_tools/seed?popoff=0&u=" + _linkUrl + _fileId;
			linkVideo.bookmarkTxt.text = "Post to: www.newsvine.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_delicious_mc(e:Event)
		{
			var url:String = "http://del.icio.us/post?v=2&url=" + _linkUrl + _fileId + "&title=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: del.icio.us";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_netvibes_mc(e:Event)
		{
			var url:String = "http://www.netvibes.com/subscribe.php?url=" + _linkUrl + _fileId;
			linkVideo.bookmarkTxt.text = "Post to: www.netvibes.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_reddit_mc(e:Event)
		{
			var url:String = "http://reddit.com/submit?url=" + _linkUrl + _fileId + "&title=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: www.reddit.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_yahoo_mc(e:Event)
		{
			var url:String = "http://bookmarks.yahoo.com/toolbar/savebm?u=" + _linkUrl + _fileId + "&t=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: bookmarks.yahoo.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_google_mc(e:Event)
		{
			var url:String = "http://www.google.com/bookmarks/mark?op=add&bkmk=" + _linkUrl + _fileId + "&title=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: www.google.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_magnolia_mc(e:Event)
		{
			var url:String = "http://ma.gnolia.com/bookmarklet/add?url=" + _linkUrl + _fileId + "&title=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: www.magnolia.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_technorati_mc(e:Event)
		{
			var url:String = "http://technorati.com/faves?sub=favthis&add=" + _linkUrl + _fileId;
			linkVideo.bookmarkTxt.text = "Post to: www.technorati.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_myspace_mc(e:Event)
		{
			var fvars:String = "embedId=" + _fileId;
			var url:String = "http://www.myspace.com/Modules/PostTo/Pages/?t=" + clipName + "&c=" + "<object width=\"390\" height=\"320\" \"><param name=\"movie\" value=\"" + embedUrl + "\" /><param name=\"flashvars\" value=\"" + fvars + "\" /><embed src=\"" + embedUrl + "\" flashvars=\"" + fvars + "\" width=\"390\" height=\"320\" type=\"application/x-shockwave-flash\" \"></embed></object>" + "&u=" + _linkUrl + _fileId + "&l=1";
			linkVideo.bookmarkTxt.text = "Post to: www.myspace.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}


		private function clk_digg_mc(e:Event)
		{
			var url:String = "http://digg.com/submit?phase=2&url=" + _linkUrl + _fileId + "&title=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: www.digg.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_stumble_mc(e:Event)
		{
			var url:String = "http://www.stumbleupon.com/submit?url=" + _linkUrl + _fileId + "&title=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: www.stumbleupon.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_fbook_mc(e:Event)
		{
			var url:String = "http://www.facebook.com/sharer.php?u=" + _linkUrl + _fileId + "&t=" + clipName;
			linkVideo.bookmarkTxt.text = "Post to: www.facebook.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function clk_socialmarker_mc(e:Event)
		{
			var url:String = "http://www.socialmarker.com/?link=" + _linkUrl + _fileId + "&title=" + clipName + "&text=" + clipDesc;
			linkVideo.bookmarkTxt.text = "Post to: www.socialmarker.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}


		private function shortURL_onLoaded(event:URLShortenEvent):void
		{
			tracer("INFO", event.service + ": " + event.url);
			var url:String = "http://twitter.com/home?status=" + escape(clipName) + "%20" + event.url;
			tracer("INFO", "Twitter URL: " + event.url);
			linkVideo.bookmarkTxt.text = "Post to: www.twitter.com";
			var request:URLRequest = new URLRequest(url);
			navigateToURL(request);
		}

		private function urlShrink(arg):void
		{
			var url:String = arg;
			var us:URLShorten = new URLShorten();
			us.addEventListener(URLShortenEvent.ON_URL_SHORTED, shortURL_onLoaded);
			us.bitly(url, "redlasso", "R_093722e292a45716a0228dbfd3fcfbb5", null);
			// Above are Redlassos idKEY and API token for bit.ly
		//us.tinyurl (url);
		}


		private function clk_twitter_mc(e:Event)
		{
			urlShrink(_linkUrl + _fileId);
		}

		private function clearBookMarkText(item):void
		{
			removeGlow(item);
			linkVideo.bookmarkTxt.text = "";
		}

		private function digg_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.digg.com";
		}

		private function digg_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function fbook_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.facebook.com";
		}

		private function fbook_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function stumble_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.stumbleupon.com";
		}

		private function stumble_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function newsvine_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.newsvine.com";
		}

		private function newsvine_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function netvibes_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.netvibes.com";
		}

		private function netvibes_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function reddit_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.reddit.com";
		}

		private function reddit_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function yahoo_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: bookmarks.yahoo.com";
		}

		private function yahoo_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function google_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.google.com/bookmarks/";
		}

		private function google_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}


		private function delicious_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: del.icio.us";
		}

		private function delicious_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function magnolia_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.magnolia.com";
		}

		private function magnolia_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function technorati_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.techorati.com";
		}

		private function technorati_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function myspace_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.myspace.com";
		}

		private function myspace_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function socialmarker_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.socialmarker.com";
		}

		private function socialmarker_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function twitter_mc_roll(e:Event)
		{
			applyGlow(e.target);
			linkVideo.bookmarkTxt.text = "Post to: www.twitter.com";
		}

		private function twitter_mc_out(e:Event)
		{
			clearBookMarkText(e.target);
		}

		private function ovr(e:Event)
		{
			applyGlow(e.target);
			switch(String(e.currentTarget.name))
			{
				case "copy_mc":
					embedVideo.embedTxt.stage.focus = embedVideo.embedTxt;
					embedVideo.embedTxt.setSelection(0, embedVideo.embedTxt.text.length);
					break;
				case "WPcopy_mc":
					embedVideo.WPembedTxt.stage.focus = embedVideo.WPembedTxt;
					embedVideo.WPembedTxt.setSelection(0, embedVideo.WPembedTxt.text.length);
					break;
				case "lcopy_mc":
					linkVideo.urlTxt.stage.focus = linkVideo.urlTxt;
					linkVideo.urlTxt.setSelection(0, linkVideo.urlTxt.text.length);
					break;
				case "cancel_mc":
					linkVideo.urlTxt.stage.focus = null;
					linkVideo.urlTxt.setSelection(0, 0);
					break;
				case "WPcancel_mc":
					linkVideo.urlTxt.stage.focus = null;
					linkVideo.urlTxt.setSelection(0, 0);
					break;
			}
		}

		private function off(e:Event)
		{
			e.target.filters = null;
		}

		private function applyGlow(inst)
		{
			var amt:Number = 15;
			//new GlowFilter(color, alphas, blurx, blury, strength, quality, inner, knockout)                                         
			var filter:GlowFilter = new GlowFilter(0xFFffff, .8, amt, amt, 1.5, 3, false, false);
			var filterArray:Array = new Array();
			filterArray.push(filter);
			inst.filters = filterArray;
		}

		private function removeGlow(inst)
		{
			inst.filters = null;
		}

		private function setupBookMarkListeners():void
		{
			linkVideo.newsvine_mc.addEventListener(MouseEvent.CLICK, clk_newsvine_mc);
			linkVideo.newsvine_mc.addEventListener(MouseEvent.MOUSE_OVER, newsvine_mc_roll);
			linkVideo.newsvine_mc.addEventListener(MouseEvent.MOUSE_OUT, newsvine_mc_out);
			linkVideo.newsvine_mc.buttonMode = true;

			linkVideo.delicious_mc.addEventListener(MouseEvent.CLICK, clk_delicious_mc);
			linkVideo.delicious_mc.addEventListener(MouseEvent.MOUSE_OVER, delicious_mc_roll);
			linkVideo.delicious_mc.addEventListener(MouseEvent.MOUSE_OUT, delicious_mc_out);
			linkVideo.delicious_mc.buttonMode = true;

			linkVideo.netvibes_mc.addEventListener(MouseEvent.CLICK, clk_netvibes_mc);
			linkVideo.netvibes_mc.addEventListener(MouseEvent.MOUSE_OVER, netvibes_mc_roll);
			linkVideo.netvibes_mc.addEventListener(MouseEvent.MOUSE_OUT, netvibes_mc_out);
			linkVideo.netvibes_mc.buttonMode = true;

			linkVideo.reddit_mc.addEventListener(MouseEvent.CLICK, clk_reddit_mc);
			linkVideo.reddit_mc.addEventListener(MouseEvent.MOUSE_OVER, reddit_mc_roll);
			linkVideo.reddit_mc.addEventListener(MouseEvent.MOUSE_OUT, reddit_mc_out);
			linkVideo.reddit_mc.buttonMode = true;

			linkVideo.yahoo_mc.addEventListener(MouseEvent.CLICK, clk_yahoo_mc);
			linkVideo.yahoo_mc.addEventListener(MouseEvent.MOUSE_OVER, yahoo_mc_roll);
			linkVideo.yahoo_mc.addEventListener(MouseEvent.MOUSE_OUT, yahoo_mc_out);
			linkVideo.yahoo_mc.buttonMode = true;

			linkVideo.google_mc.addEventListener(MouseEvent.CLICK, clk_google_mc);
			linkVideo.google_mc.addEventListener(MouseEvent.MOUSE_OVER, google_mc_roll);
			linkVideo.google_mc.addEventListener(MouseEvent.MOUSE_OUT, google_mc_out);
			linkVideo.google_mc.buttonMode = true;

			linkVideo.twitter_mc.addEventListener(MouseEvent.CLICK, clk_twitter_mc);
			linkVideo.twitter_mc.addEventListener(MouseEvent.MOUSE_OVER, twitter_mc_roll);
			linkVideo.twitter_mc.addEventListener(MouseEvent.MOUSE_OUT, twitter_mc_out);
			linkVideo.twitter_mc.buttonMode = true;

			linkVideo.magnolia_mc.addEventListener(MouseEvent.CLICK, clk_magnolia_mc);
			linkVideo.magnolia_mc.addEventListener(MouseEvent.MOUSE_OVER, magnolia_mc_roll);
			linkVideo.magnolia_mc.addEventListener(MouseEvent.MOUSE_OUT, magnolia_mc_out);
			linkVideo.magnolia_mc.buttonMode = true;

			linkVideo.technorati_mc.addEventListener(MouseEvent.CLICK, clk_technorati_mc);
			linkVideo.technorati_mc.addEventListener(MouseEvent.MOUSE_OVER, technorati_mc_roll);
			linkVideo.technorati_mc.addEventListener(MouseEvent.MOUSE_OUT, technorati_mc_out);
			linkVideo.technorati_mc.buttonMode = true;

			linkVideo.myspace_mc.addEventListener(MouseEvent.CLICK, clk_myspace_mc);
			linkVideo.myspace_mc.addEventListener(MouseEvent.MOUSE_OVER, myspace_mc_roll);
			linkVideo.myspace_mc.addEventListener(MouseEvent.MOUSE_OUT, myspace_mc_out);
			linkVideo.myspace_mc.buttonMode = true;

			linkVideo.digg_mc.addEventListener(MouseEvent.CLICK, clk_digg_mc);
			linkVideo.digg_mc.addEventListener(MouseEvent.MOUSE_OVER, digg_mc_roll);
			linkVideo.digg_mc.addEventListener(MouseEvent.MOUSE_OUT, digg_mc_out);
			linkVideo.digg_mc.buttonMode = true;

			linkVideo.stumble_mc.addEventListener(MouseEvent.CLICK, clk_stumble_mc);
			linkVideo.stumble_mc.addEventListener(MouseEvent.MOUSE_OVER, stumble_mc_roll);
			linkVideo.stumble_mc.addEventListener(MouseEvent.MOUSE_OUT, stumble_mc_out);
			linkVideo.stumble_mc.buttonMode = true;

			linkVideo.fbook_mc.addEventListener(MouseEvent.CLICK, clk_fbook_mc);
			linkVideo.fbook_mc.addEventListener(MouseEvent.MOUSE_OVER, fbook_mc_roll);
			linkVideo.fbook_mc.addEventListener(MouseEvent.MOUSE_OUT, fbook_mc_out);
			linkVideo.fbook_mc.buttonMode = true;

			linkVideo.socialmarker_mc.addEventListener(MouseEvent.CLICK, clk_socialmarker_mc);
			linkVideo.socialmarker_mc.addEventListener(MouseEvent.MOUSE_OVER, socialmarker_mc_roll);
			linkVideo.socialmarker_mc.addEventListener(MouseEvent.MOUSE_OUT, socialmarker_mc_out);
			linkVideo.socialmarker_mc.buttonMode = true;
		}


		//********************* MENU *************************************************

		private function linkVideoCancel(e:Event)
		{
			menuState = "OPEN";
			activeBTN(true);
			Tweener.addTween(linkVideo, {x:linkStopx, y:linkStopy, time:.3, transition:"linear"});
			showMenu();
		}


		private function bookmark(e:Event)
		{
			closeAbout();
			menuState = "SHARING";
			menuSubState = "LINKS";
			activeBTN(false);
			fadeOverlay(true);

			mcPlay.visible = false;
			mcPlay.enabled = false;
			Tweener.addTween(menubg, {x:menuStopx, y:menuStopy, time:.3, transition:"linear"});
			Tweener.addTween(linkVideo, {x:linkStartx, y:linkStarty, time:.3, transition:"linear"});
			if (_fileId == null || _fileId == undefined || _fileId == "")
			{
				_fileId = initClip;
				linkVideo.urlTxt.text = "http://www.redlasso.com/player.htm?id=" + _fileId;
			}
			linkVideo.urlTxt.stage.focus = linkVideo.urlTxt;
			linkVideo.urlTxt.setSelection(0, linkVideo.urlTxt.text.length);

		}

		private function closeAbout()
		{
			if (about_scrn.visible == true)
			{
				menuState = "OPEN";
				menuSubState = "CLOSED";
				var evt:Event = new Event(MouseEvent.CLICK, true, false);
				menubg.about_mc.dispatchEvent(evt);
			}
		}

		private function email(e:Event)
		{
			closeAbout();
			menuState = "SHARING";
			menuSubState = "EMAIL";
			activeBTN(false);
			fadeOverlay(true);

			mcPlay.visible = false;
			mcPlay.enabled = false;

			emailShare.toemailTxt.text = " ";
			emailShare.msgTxt.text = " ";
			emailShare.statusTxt.text = " ";

			Tweener.addTween(menubg, {x:menuStopx, y:menuStopy, time:.3, transition:"linear"});
			Tweener.addTween(emailShare, {x:emailStartx, y:emailStarty, time:.3, transition:"linear"});
		}



		private function link(e:Event)
		{
			assignEmbed();
			closeAbout();
			menuState = "SHARING";
			menuSubState = "EMBED";
			activeBTN(false);
			fadeOverlay(true);

			mcPlay.visible = false;
			mcPlay.enabled = false;
			Tweener.addTween(menubg, {x:menuStopx, y:menuStopy, time:.3, transition:"linear"});
			Tweener.addTween(embedVideo, {x:embedStartx, y:embedStarty, time:.3, transition:"linear"});
			try
			{
				embedVideo.embedTxt.stage.focus = embedVideo.embedTxt;
				embedVideo.embedTxt.setSelection(0, embedVideo.embedTxt.text.length);
			}
			catch(err:Error)
			{
			}
		}


		private function about(e:Event)
		{
			if (about_scrn.visible == false)
			{
				menuState = "OPEN";
				menuSubState = "ABOUT";
				fadeOverlay(true);
				mcPlay.visible = false;
				about_scrn.visible = true;
				Tweener.addTween(about_scrn, {rotationY:360, time:1, transition:"easeInOutQuint"});
			}
			else
			{
				fadeOverlay(false);
				if (playToggle)
				{
					mcPlay.visible = true;
					mcPlay.enabled = true;
				}
				else
				{
					mcPlay.visible = false;
					mcPlay.enabled = false;

				}
				about_scrn.visible = false;
				about_scrn.rotationY = 0;
			}
		}

		private function setupMenuButtonListeners():void
		{
			menubg.embed_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			menubg.embed_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			menubg.embed_mc.addEventListener(MouseEvent.CLICK, bookmark);

			menubg.email_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			menubg.email_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			menubg.email_mc.addEventListener(MouseEvent.CLICK, email);

			menubg.link_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			menubg.link_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			menubg.link_mc.addEventListener(MouseEvent.CLICK, link);

			menubg.about_mc.addEventListener(MouseEvent.MOUSE_OVER, ovr);
			menubg.about_mc.addEventListener(MouseEvent.MOUSE_OUT, off);
			menubg.about_mc.addEventListener(MouseEvent.CLICK, about);

			// menubg.closeButt.addEventListener (MouseEvent.MOUSE_OVER, ovr);
			// menubg.closeButt.addEventListener (MouseEvent.MOUSE_OUT, off);
			menubg.closeButt.addEventListener(MouseEvent.CLICK, MenuMove, false, 0, true);

			menubg.embed_mc.buttonMode = true;
			menubg.email_mc.buttonMode = true;
			menubg.link_mc.buttonMode = true;
			menubg.about_mc.buttonMode = true;
			menubg.closeButt.buttonMode = true;
		}

		//*************** Javascript Integration Functions ******************

		public function setSeekPoint(num:Number)
		{
			if (num < globalStart)
			{
				num = globalStart;
			}
			if (num > globalStop)
			{
				num = globalStop;
			}
			// handle enabling playback and then seeking if not already started
			if (!myVid.playing)
			{
				seekAndStart = true;
				userClicked = true;
				seekStartPoint = num;
				activeBTN(true);
				loader_mc.gotoAndPlay(2);
				thumbnailUnavailable.visible = false;
				mcPlay.removeEventListener(MouseEvent.CLICK, startUp);
				GetVarInfo();
			}

			tracer("INFO", "setSeekPoint: " + num);
			if (!previewPersistent)
			{
				placeholder.visible = false;
			}
			dispCap.text = "";
			mcPlay.visible = false;
			mcPlay.enabled = false;

			mc_pp.gotoAndStop(3);
			myVid.pause();
			myVid.seek(num);
			try
			{
				myVid.play();
			}
			catch(e:VideoError)
			{
				tracer("WARN", "ERR5 ERROR: " + e.code + " :: " + e);
			}
		}

		private function setupExternalInterfaceCalls():void
		{
			if (ExternalInterface.available)
			{
				try
				{
					ExternalInterface.addCallback("setSeekPoint", setSeekPoint);
				}
				catch(err:Error)
				{
					tracer("WARN", "ERR6 ERROR: " + err.code + " :: " + err.message);
				}
				try
				{
					ExternalInterface.addCallback("externalPlay", externalPlay);
				}
				catch(err:Error)
				{
				}
			}
		}


		private function externalPlay(arg:String):void
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
			mc_pp.static_mc.enabled = bool;
			mutebutt_mc.enabled = bool;
			volbutt_mc.enabled = bool;
			menu_mc.enabled = bool;
			if (widgetMode == "TRUE")
			{
			}
			else
			{
				fullScreen.enabled = bool;
			}
			CC_btn.enabled = bool;
			meta_mc.toggle_mc.enabled = bool;
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
				mcPlay.removeEventListener(MouseEvent.CLICK, playClip);
				mcPlay.removeEventListener(MouseEvent.CLICK, startUp);
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
				mcPlay.addEventListener(MouseEvent.CLICK, playClip);
				mcPlay.addEventListener(MouseEvent.CLICK, startUp);
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
			switch(stage.displayState)
			{
				case "normal":
					bigScreen = false;
					break;
				case "fullScreen":
					bigScreen = true;
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
			loaderContext.checkPolicyFile = true;
			Security.loadPolicyFile("http://media.redlasso.com/crossdomain.xml");
			try
			{
				imgPreviewURL = srvString + "/svc/clip/previewImage?eid=" + initClip;
				prevLoader.load(new URLRequest(imgPreviewURL), loaderContext);
			}
			catch(err:Error)
			{
				tracer("WARN", "ERR7 ERROR preLoader.load: " + err);
			}
			prevLoader.contentLoaderInfo.addEventListener(Event.COMPLETE, onLoadPreviewComplete);
			prevLoader.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR, onErrorPreview);
			prevLoader.contentLoaderInfo.addEventListener(HTTPStatusEvent.HTTP_STATUS, prevLoader_httpStatusHandler);

			activeBTN(false);
		}


		private function impressionLoadPreviewImage():void
		{
			var imgPreviewURL:String = srvString + "/svc/clip/previewImage?log=true&eid=" + initClip;
			var impressionSend:URLLoader = new URLLoader();
			var impUrl:URLRequest = new URLRequest(imgPreviewURL);

			impressionSend.addEventListener(Event.COMPLETE, onImpressionLoaded);
			impressionSend.addEventListener(HTTPStatusEvent.HTTP_STATUS, onImpressionStatus);
			impressionSend.addEventListener(IOErrorEvent.IO_ERROR, onImpressionNOTLoaded);
			impressionSend.addEventListener(SecurityErrorEvent.SECURITY_ERROR, onImpressionNOTLoaded);

			impUrl.method = URLRequestMethod.GET;
			try
			{
				impressionSend.load(impUrl);
				tracer("INFO", "Clip Impression Loading: " + imgPreviewURL);
			}
			catch(err:Error)
			{
				tracer("WARN", "ERR27 LOGCLIPPLAY ERROR: " + err);
			}

		}

		private function onImpressionStatus(evt:HTTPStatusEvent):void
		{
			tracer("INFO", "Impression HTTPStatus: " + evt.status);
		}

		private function onImpressionLoaded(evt:Event):void
		{
			var impressionSend:URLLoader = URLLoader(evt.target);

			var vars:URLVariables = new URLVariables(impressionSend.data);
			tracer("INFO", "Impression Loaded: " + impressionSend.data);
			tracer("INFO", "Impression Success Result: " + vars.success);
		}

		private function onImpressionNOTLoaded(evt:Event):void
		{
			tracer("INFO", "IMPRESSION LOAD ERROR: " + evt);
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
			mcPlay.visible = true;
			//mcPlay.alpha = .85;
			loader_mc.gotoAndStop(1);
			loader_mc.visible = false;
			thumbnailUnavailable.dispText.text = "Preview Image Temporarily Unavailable";
			thumbnailUnavailable.visible = true;
		}

		private function onErrorPreview(evt:Event):void
		{
			onErrorPreviewSub();
		}

		private function onErrorAudioPreview(evt:Event):void
		{
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
			previewLoaded = true;
			var info:LoaderInfo = LoaderInfo(prevLoader.contentLoaderInfo);

			if (widgetMode != "TRUE")
			{
				sw = stage.stageWidth;
				sh = stage.stageHeight;
			}

			var vidScale = Math.min(sw / prevLoader.width, (sh - 28) / prevLoader.height);
			var newW = vidScale * prevLoader.width;
			var newH = vidScale * prevLoader.height;
			prevLoader.width = newW;
			prevLoader.height = newH;
			thumbnailUnavailable.width = newW;
			thumbnailUnavailable.height = newH;

			if (prevLoader.content is Bitmap)
			{
				Bitmap(prevLoader.content).smoothing = true;
			}

			try
			{
				removeChild(prevLoader);
			}
			catch(err:Error)
			{
				tracer("WARN", "ERR14 ERROR: " + err);
			}

			addChild(prevLoader);

			setChildIndex(prevLoader, 2);
			//tracer("INFO", "AUDIO GRAPHIC SETTING INDEX:" + getChildIndex(prevLoader));
			prevLoader.alpha = 1;

			myVid.alpha = 0;
			previewPersistent = true;

			prevLoader.x = Math.round(sw / 2 - prevLoader.width / 2);

			loader_mc.gotoAndStop(1);
			loader_mc.visible = false;
		}

		private function onLoadPreviewComplete(event:Event):void
		{
			previewLoaded = true;
			var info:LoaderInfo = LoaderInfo(prevLoader.contentLoaderInfo);
			// tracer ("INFO","Image Preview url=" + info.url);
			if (widgetMode != "TRUE")
			{
				sw = stage.stageWidth;
				sh = stage.stageHeight;
			}
			var vidScale = Math.min(sw / prevLoader.width, (sh - 28) / prevLoader.height);
			var newW = vidScale * prevLoader.width;
			var newH = vidScale * prevLoader.height;
			prevLoader.width = newW;
			prevLoader.height = newH;
			thumbnailUnavailable.width = newW;
			thumbnailUnavailable.height = newH;

			try
			{
				if (prevLoader.content is Bitmap)
				{
					Bitmap(prevLoader.content).smoothing = true;
				}
			}
			catch(err:Error)
			{

			}

			try
			{
				removeChild(prevLoader);
			}
			catch(err:Error)
			{
				tracer("WARN", "ERR14 ERROR: " + err);
			}

			addChild(prevLoader);
			prevLoader.alpha = 0;

			prevLoader.x = Math.round(sw / 2 - prevLoader.width / 2);
			prevLoader.y = Math.round((sh - 28) / 2 - prevLoader.height / 2);

			Tweener.addTween(prevLoader, {alpha:1, time:.5, transition:"linear"});

			mcPlay.addEventListener(MouseEvent.CLICK, startUp);
			mc_pp.addEventListener(MouseEvent.CLICK, startUp);
			mc_pp.gotoAndStop(2);
			setChildIndex(mcPlay, numChildren - 1);

			mcPlay.visible = true;
			//mcPlay.alpha = .85;
			loader_mc.gotoAndStop(1);
			loader_mc.visible = false;
			setChildIndex(prevLoader, 0);
		}



		private function startUp(evt:MouseEvent):void
		{
			if (widgetMode == "TRUE" && willAutoPlay == "TRUE")
			{
				// nada
			}
			else
			{
				tracer("INFO", "STARTUP PLAY CLICKED");
				dispatchEvent(new Event("RLPlayer_PLAY_CLICKED", true, false));
				userClicked = true;
				loader_mc.gotoAndPlay(2);
				thumbnailUnavailable.visible = false;
				mcPlay.removeEventListener(MouseEvent.CLICK, startUp);
				mc_pp.removeEventListener(MouseEvent.CLICK, startUp);
				activeBTN(true);
				GetVarInfo();
			}
		}



		

		public function disableControls():void
		{
			tracer("INFO", 'Redlasso.disableControls()');

			mc_pp.enabled = false;
			Scrub.enabled = false;
			fullScreen.enabled = false;
			fullScreen.removeEventListener(MouseEvent.CLICK, ToggleFullScreen);
			menu_mc.enabled = false;

			menu_mc.removeEventListener(MouseEvent.CLICK, MenuMove);
			activeBTN(false);
			lockUI(false);
			bKnob.enabled = false;
			tknob_mc.enabled = false;

			Scrub.removeEventListener(MouseEvent.MOUSE_OVER, SetScrub);
			Scrub.removeEventListener(MouseEvent.MOUSE_OUT, SetScrubOff);
			Scrub.removeEventListener(MouseEvent.CLICK, SetScrubClick);
			bKnob.removeEventListener(MouseEvent.MOUSE_OVER, bknob_over);
			bKnob.removeEventListener(MouseEvent.MOUSE_OUT, bknob_out);
			bKnob.removeEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
			bKnob.removeEventListener(MouseEvent.MOUSE_UP, KnobClickUp);

		}

		public function enableControls():void
		{
			tracer("INFO", 'Redlasso.enableControls()');

			//mcPlay.enabled = true;
			//mcPlay.addEventListener (MouseEvent.CLICK, playClip);
			mc_pp.enabled = true;
			Scrub.enabled = true;
			fullScreen.enabled = true;
			fullScreen.addEventListener(MouseEvent.CLICK, ToggleFullScreen);
			menu_mc.enabled = true;
			menu_mc.addEventListener(MouseEvent.CLICK, MenuMove);
			activeBTN(true);
			lockUI(true);

			bKnob.enabled = true;
			tknob_mc.enabled = true;

			Scrub.addEventListener(MouseEvent.MOUSE_OVER, SetScrub);
			Scrub.addEventListener(MouseEvent.MOUSE_OUT, SetScrubOff);
			Scrub.addEventListener(MouseEvent.CLICK, SetScrubClick);
			bKnob.addEventListener(MouseEvent.MOUSE_OVER, bknob_over);
			bKnob.addEventListener(MouseEvent.MOUSE_OUT, bknob_out);
			bKnob.addEventListener(MouseEvent.MOUSE_DOWN, KnobClickDwn);
			bKnob.addEventListener(MouseEvent.MOUSE_UP, KnobClickUp);
		}

		//Get the status of the video playhead and if the video is complete
		public function getVideoStatus():Number
		{
			
			if (myVid.playheadTime - globalStart == NaN)
			{
				return globalStart;
			}
			else
			{
				return Number(myVid.playheadTime - globalStart);
			}
		}

		public function objectToString(o:Object):String
		{
			var returnString:String = "";
			for(var property in o)
			{
				if (returnString.length > 0)
				{
					returnString += ", ";
				}
				returnString += property;
				returnString += ":";
				returnString += o[property];
			}
			return returnString;
		}



	}
}

/* EXAMPLE of getVars service call return data:

   <root status="1" msg="" sid="26e84020-1977-42e2-bb31-2cf6e96c6ac3"
   rid="1e05f1cb-f1b2-4d84-871a-ed519df8c597">

   <clipinfo
   title="Redlasso Radio2Web Demo Video"
   categoryKey="PR"
   categoryName="Private"
   keywords="Redlasso"
   author="Anonymous"
   startTime="0"
   endTime="104"
   stationName="Redlasso"
   stationLogoUrl="http://media.redlasso.com/logos/stations/small/test.png"
   stationUrl="http://www.redlasso.com"
   airdate="11/18/2009"
   dateCreated="11/18/2009"
   views="188">
   <![CDATA[This is a demo video for the Redlasso Radio2Web Publishing Platform.  Enhanced Podcast distribution, custom media centers, video hosting, content clipping and more. ]]>
   </clipinfo>

   <adaptv ads="FALSE" videoPlayerId="" key="" zid="" adaptag=""
   companionID="" context="" pageUrl="" preRoll="TRUE"
   midRoll="FALSE" overlay="TRUE" postRoll="FALSE" companion="FALSE"
   overlayType="type" />

   <vars
   fileName="0000_radio2web.mp4"
   streamUrl="0250.fms.edgecastcdn.net"
   serviceUrl="http://test.redlasso.com/service_test"
   appNameFMS="000250/flash"
   bitRate="-1"
   videoWidth="-1"
   videoHeight="-1"
   aspectRatio="-1"
   hasVideo="TRUE"
   thumbUrl="http://media.redlasso.com/im18/2009/10/18/1e05f1cb-f1b2-4d84-871a-ed519df8c5971.jpg"
   hasCaption="TRUE"
   userId="094828ea-2fdb-45c3-9ff4-65c64307da98"
   emailFrom="rob2@redlasso.com"
   embedUrl="http://media.redlasso.com/xdrive/web/vidplayer_1b/devplayer/devplayer_rhall/redlasso_player.swf"
   linkUrl="http://test.redlasso.com/blogger/ClipPlayer.aspx?id="
   logPlayURL="http://test.redlasso.com/service_test/svc/vars/logClipPlay" />
   </root>
 */