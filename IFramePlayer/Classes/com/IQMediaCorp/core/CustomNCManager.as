package com.IQMediaCorp.core
{
	import fl.video.INCManager;
  import fl.video.NCManager;
  import fl.video.ParseResults;
  import fl.video.SMILManager;
  import fl.video.VideoError;
  import fl.video.flvplayback_internal;

	use namespace flvplayback_internal;
public class CustomNCManager extends NCManager implements INCManager
  {   
  
  	public static var AppName:String;
	public static var StreamName:String;
  
    override public function connectToURL(url : String) : Boolean 
        {
            //ifdef DEBUG
            //debugTrace("connectToURL(" + url + ")");
            //endif

            // init
            initOtherInfo();
            _contentPath = url;
            if (_contentPath == null || _contentPath == "") 
            {
                throw new VideoError(VideoError.INVALID_SOURCE);
            }

            // parse URL to determine what to do with it
            var parseResults : ParseResults = parseURL(_contentPath);
            if (parseResults.streamName == null || parseResults.streamName == "") 
            {
                throw new VideoError(VideoError.INVALID_SOURCE, url);
            }

            // connect to either rtmp or http or download and parse smil
            var canReuse : Boolean;
            if (parseResults.isRTMP) 
            {
				//LogManager.info(this, "\t connectToURL is RTMP");
                canReuse = canReuseOldConnection(parseResults);
                _isRTMP = true;
                _protocol = parseResults.protocol;
                _streamName = /*parseResults.streamName*/StreamName;
                _serverName = parseResults.serverName;
                _wrappedURL = parseResults.wrappedURL;
                _portNumber = parseResults.portNumber;
                _appName = /*parseResults.appName*/AppName;
                
                trace("APPNAME: "+_appName);
				trace("STREAMNAME: "+_streamName);
                
                if ( _appName == null || _appName == "" || _streamName == null || _streamName == "" ) 
                {
                    throw new VideoError(VideoError.INVALID_SOURCE, url);
                }
                _autoSenseBW = (_streamName.indexOf(",") >= 0);
                return (canReuse || connectRTMP());
            } 
            else 
            {
                var name : String = parseResults.streamName;
                
                if (name.indexOf("kewego") > 0 && name.indexOf("flv?key") > 0)
                {
                    
                    canReuse = canReuseOldConnection(parseResults);
                    _isRTMP = false;
                    _streamName = name;
                    return (canReuse || connectHTTP())
                }
                if ( name.indexOf("?") < 0 && (name.length < 4 || name.slice(-4).toLowerCase() != ".txt") && (name.length < 4 || name.slice(-4).toLowerCase() != ".xml") && (name.length < 5 || name.slice(-5).toLowerCase() != ".smil") ) 
                {
                    
                    canReuse = canReuseOldConnection(parseResults);
                    _isRTMP = false;
                    _streamName = name;
                    return (canReuse || connectHTTP());
                }
                
                if (name.indexOf("/fms/fpad") >= 0) 
                {
                    try 
                    {
                        return connectFPAD(name);
                    } catch (err : Error) 
                    {
                    }
                }
                
                _smilMgr = new SMILManager(this);
                return _smilMgr.connectXML(name);
            }
        }
      }
}