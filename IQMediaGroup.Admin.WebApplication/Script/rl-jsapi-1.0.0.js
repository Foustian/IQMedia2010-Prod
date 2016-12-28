/*
RedLasso JSAPI

TODOS:
Authentication
Caching (Possibly Better Implemented?)
Better handling of SvcPrefix & PlayerUrl
*/

//******************************
//******* RedLasso.Util ********
//******************************
if (typeof RedLasso == "undefined") RedLasso = {};
if (typeof RedLasso.Util == "undefined") RedLasso.Util = {};

/////////////////////////////////
//      JS-API Script Vars     //
/////////////////////////////////
if (typeof RedLasso.Util.ScriptVars == "undefined") RedLasso.Util.ScriptVars = {};
RedLasso.Util.ScriptVars.Version = "1.0.0";
RedLasso.Util.ScriptVars.RequestTimeout = 30000; //Timeout after 30 seconds
//RedLasso.Util.ScriptVars.PlayerUrl = "http://media.redlasso.com/xdrive/WEB/vidplayer_1b/redlasso_player_b1b_deploy.swf?swvf=061808";
RedLasso.Util.ScriptVars.PlayerUrl = "http://player.redlasso.com/redlasso_player_b1b_deploy.swf"
RedLasso.Util.ScriptVars.SvcPrefix = "http://services.redlasso.com/";
//RedLasso.Util.ScriptVars.PlayerUrl = "http://media.redlasso.com/xdrive/web/vidplayer_1b/devplayer/redlasso_player_b1b_dev.swf?swfv=061208_1";
//RedLasso.Util.ScriptVars.PlayerUrl = "http://player.redlasso.com/redlasso_player_b1b_dev_102009.swf"
//RedLasso.Util.ScriptVars.SvcPrefix = "http://test.redlasso.com/service_test/";

//////////////////////////////////////
//    JSONP Script Caller Object    //
//////////////////////////////////////
// DynCall - do dynamic script insertion for json call
// TODO: would like to make this a prototype with instances but callback can't call the instance.
RedLasso.Util.JSONP = function() { };
RedLasso.Util.JSONP.scriptCounter = 1;
RedLasso.Util.JSONP.calls = [];
RedLasso.Util.JSONP.listeners = [];

RedLasso.Util.JSONP.Request = function() {
    this.svcSuffix = "";
    this.params = new RedLasso.Util.Hash();

    this.addParam = function(param, value) {
        this.params.set(param, value);
    }
}

RedLasso.Util.JSONP.sendRequest = function(request, obj, callback) {
    if (request instanceof RedLasso.Util.JSONP.Request) {
        // pass state option
        var stateStore = new RedLasso.Util.Hash();
        if (typeof callback == "function") stateStore.set("callback", callback);

        var fullUrl = RedLasso.Util.ScriptVars.SvcPrefix + request.svcSuffix;
        request.params.each(function(pair, i) {
            fullUrl += (i > 0) ? "&" : "?";
            fullUrl += pair.key + "=" + encodeURIComponent(pair.value);
        });

        RedLasso.Util.JSONP.doCall(fullUrl, obj, stateStore, obj.generic_callback);
    }
    else throw ("The 'request' parameter of RedLasso.Util.JSONP.sendRequest() must be of type RedLasso.Util.JSONP.Request");
}

RedLasso.Util.JSONP.doCall = function(fullUrl, callee, state, callback) {
    // Keep IE from caching requests
    var noCacheIE = '&noCacheIE=' + (new Date()).getTime();
    // Get the DOM location to put the script tag
    var headLoc = document.getElementsByTagName("head").item(0);
    // Generate a unique script tag id
    var scriptId = 'RLUTILCID' + this.scriptCounter++;

    var scriptObj = document.createElement("script");
    // Add script object attributes
    scriptObj.setAttribute("type", "text/javascript");
    scriptObj.setAttribute("charset", "utf-8");
    var aorq = (fullUrl.indexOf("?") > -1 ? "&" : "?");
    scriptObj.setAttribute("src", fullUrl + aorq + 'fn=' + scriptId + noCacheIE);
    scriptObj.setAttribute("id", scriptId);

    //Added new timeout handler... (Only do it if a time has been set)
    var timeout = null;
    if (RedLasso.Util.ScriptVars.RequestTimeout > 0) {
        timeout = setTimeout(function() {
            RedLasso.Util.JSONP.callback(null, scriptId);
        }, RedLasso.Util.ScriptVars.RequestTimeout);
    }

    this.calls[scriptId] = [callee, callback, state, scriptObj, timeout];
    headLoc.appendChild(scriptObj);    
}

RedLasso.Util.JSONP.callback = function(res, callback) {
    //We only want to do this if the callback is still relevant
    if (this.calls.hasOwnProperty(callback)) {
        //Clear the timeout (if it exists)
        if (this.calls[callback][4] != null) clearTimeout(this.calls[callback][4]);

        var headLoc = document.getElementsByTagName("head").item(0);
        headLoc.removeChild(this.calls[callback][3]);

        this.calls[callback][1].call(this.calls[callback][0], res, this.calls[callback][2]);

        //Remove the call when we're all done (A little garbage collecting)
        delete this.calls[callback];
    }
}
//TODO: These two functions are added for passive error handling
//AKA, the user is not logged in, or does not have permissions to
//do something. This allows the page to handle the error softly.
RedLasso.Util.JSONP.dispatchEvent = function(event) {
    var listeners = RedLasso.Util.JSONP.listeners[event[0]];
    if (typeof listeners != "undefined") {
        for (var i = 0; i < listeners.length; i++) {
            if (typeof listeners[i] == "function")
                listeners[i](event);
        }
    }
}
RedLasso.Util.JSONP.addEventListener = function(eventType, fn) {
    if (typeof RedLasso.Util.JSONP.listeners[eventType] == "undefined")
        RedLasso.Util.JSONP.listeners[eventType] = new Array();
    RedLasso.Util.JSONP.listeners[eventType].push(fn);
}

/////////////////////////////////////////////////
// Hash - parts removed from Prototype 1.6.0.2 //
/////////////////////////////////////////////////
RedLasso.Util.Hash = function(obj) {
    this._object = obj == null ? {} : obj;
}
RedLasso.Util.Hash.prototype =
{
    set: function(key, value) {
        return this._object[key] = value;
    },
    get: function(key) {
        return this._object[key];
    },
    each: function(iterator) {
        var i = 0;
        for (var key in this._object) {
            var value = this._object[key], pair = [key, value];
            pair.key = key;
            pair.value = value;
            iterator(pair, i);
            i++;
        }
    }
}

/////////////////////////////////
//    ClipSearch Controller    //
/////////////////////////////////
RedLasso.Util.ClipSearchController = function(srchInc, srchTrigPer) {
    this._clipSvc = RedLasso.Service.ServiceFactory.CreateService("Clip");
    this._clipList = new RedLasso.Util.LazyLoadList(RedLasso.Domain.Clip, this._clipSvc);
    this._request = null;
    this._searchComplete = false;
    this.SearchIncrement = srchInc || 50;
    this.SearchTriggerPercent = srchTrigPer || 0.8;

    this.submitSearchRequest = function(request, callback) {
        if (request instanceof RedLasso.Domain.ClipSearchRequest) {
            this._request = request;
            this._request.setMaxResults(this.SearchIncrement);
            this._clipList.clear();
            this._searchComplete = false;
            this._search(callback);
        }
        else throw ("The parameter 'request' of RedLasso.Util.ClipSearchController.submitSearchRequest() " +
            "must be of type RedLasso.Domain.ClipSearchRequest.");
    }
    this.get = function(index, callback) {
        this._clipList.get(index, callback);
    }
    this.getByGuid = function(guid, callback) {
        this._clipList.getByGuid(guid, callback);
    }
    this.getRange = function(startIndex, endIndex, callback) {
        //If the search is complete and the endIndex is out of range, max it out
        if (endIndex > this._clipList.length && this._searchComplete)
            endIndex = this._clipList.length;

        //If the indexes are out of range, try to get more results...
        if (startIndex >= this._clipList.length || endIndex > this._clipList.length) {
            if (!this._searchComplete) {
                var obj = this;
                this._search(function() {
                    obj.getRange(startIndex, endIndex, callback);
                });
            }
            else if (this._clipList.length == 0) callback(new Array());
            else throw ("An index is out of range in RedLasso.Util.ClipSearchController.getRange().");
        }
        else {
            var trigger = this._clipList.length * this.SearchTriggerPercent;
            //If we're inside the search trigger, run another search...
            if ((startIndex >= trigger || endIndex >= trigger) && !this._searchComplete) {
                this._search();
            }
            this._clipList.getRange(startIndex, endIndex, callback);
        }
    }
    this._search = function(callback) {
        //Prep the offset
        this._request.setSearchOffset(this._clipList.length);
        var obj = this;

        if (!this._searchComplete) {
            this._clipSvc.searchClips(this._request, function(data) {
                if (data != null) obj._clipList.addRange(data);
                //if we got back less results than our increment, there is obviously no more results
                if (data == null || data.length < obj.SearchIncrement) obj._searchComplete = true;

                //if a callback has been specified, call it.
                if (typeof callback == "function") callback();
            });
        }
    }
    this.searchFull = function(callback) {
        if (!this._searchComplete) {
            var obj = this;
            this._search(function() {
                obj.searchFull(callback);
            });
        }
    }
    this.isSearchComplete = function() {
        return this._searchComplete;
    }
}

/////////////////////////////////
//     RawSearch Controller    //
/////////////////////////////////
RedLasso.Util.RawSearchController = function() {
    this._ccSvc = RedLasso.Service.ServiceFactory.CreateService("CCSearch");
    this.timeout = 250;
    this.retries = 20;
    this.onSearch = null;
    this.onComplete = null;
    this._request = null;
    this._searchComplete = false;
    this._tries = 0;
    this._results = new Object();
    this._resultsKeys = new Array();

    this.submitSearchRequest = function(request) {
        if (request instanceof RedLasso.Domain.RawSearchRequest) {
            this._request = request;
            this._searchComplete = false;
            this._search();
        }
        else throw ("The parameter 'request' of RedLasso.Util.RawSearchController.submitSearchRequest() " +
            "must be of type RedLasso.Domain.RawSearchRequest.");
    }

    this._search = function() {
        var obj = this;
        this._ccSvc.search(this._request, function(data) {
            if (data == null) {
                obj.onComplete(null);
                return;
            }
            for (var i = 0; i < data.length; i++) {
                obj._results[data[i].CacheKey] = data[i];
                obj._resultsKeys.push(data[i].CacheKey);
            }
            obj.onSearch(obj.getSortedResults());  //First round of data
            obj._get(obj._resultsKeys);

        });
    }

    this._get = function(keys) {
        var obj = this;
        this._ccSvc.get(keys, function(data) {
            if (data == null) {
                obj.onComplete(obj.getSortedResults());
                return;
            }

            //First, if we got any data back, lets update our results list...
            for (var i = 0; i < data.length; i++)
                obj._results[data[i].CacheKey] = data[i];

            //Now to iterate through the list and get any keys that have not been completely searched
            var unprocessedKeys = new Array();
            for (var i = 0; i < obj._resultsKeys.length; i++) {
                if (obj._results[obj._resultsKeys[i]].Complete == false)
                    unprocessedKeys.push(obj._resultsKeys[i]);
            }

            //Call the onSearch function if its defined (only when new data is returned)
            if (data.length > 0 && typeof obj.onSearch == "function")
                obj.onSearch(obj.getSortedResults());

            //If some keys are incomplete...try, try again...(stopping after we've hit our max retries)
            if (unprocessedKeys.length > 0 && obj._tries < obj.retries) {
                obj._tries++;
                var code = function() { obj._get(unprocessedKeys); }
                setTimeout(code, obj.timeout);
            }
            //If our searching is complete or we've reached max tries, call the onComplete function if defined
            else if (typeof obj.onComplete == "function")
                obj.onComplete(obj.getSortedResults());
        });
    }

    this.getSortedResults = function() {
        var results = new Array();
        for (var i = 0; i < this._resultsKeys.length; i++)
            results.push(this._results[this._resultsKeys[i]]);
        results.sort(this._compareRawResults);

        return results;
    }
    this._compareRawResults = function(a, b) {
        //First compare nulls
        if ((typeof a == "object") == false) return -1;
        if ((typeof b == "object") == false) return 1;

        //Then compare hits
        if (a.Hits > b.Hits) return -1;
        if (b.Hits > a.Hits) return 1;

        //Next compare date
        var aD = new Date(a.Hour);
        var bD = new Date(b.Hour);
        if (aD.getTime() > bD.getTime()) return -1;
        if (bD.getTime() > aD.getTime()) return 1;

        return 0;
    }
}

/////////////////////////////////
//     Paginator Control       //
/////////////////////////////////
RedLasso.Util.Paginator = function(srchCtrl, returnfn, clipsPerPage, prefetchPages) {
    this._srchCtrl = srchCtrl;
    this._returnfn = returnfn;
    this._perPage = clipsPerPage || 5;
    this._prefetch = prefetchPages || 1;
    this._position = 0;

    this.prev = function() {
        var top = this._position;
        this._position -= this._perPage;

        //If we're going beyond 0, set it to 0 and get the first page of results
        if (this._position <= 0) {
            this._position = 0;
            top = this._perPage;
        }

        this._srchCtrl.getRange(this._position, top, this._returnfn);
    }
    this.next = function() {
        //Only try it if we know that we can keep going...
        if (this.hasNext()) {
            this._position += this._perPage;
            var obj = this;
            this._srchCtrl.getRange(this._position, this._position + this._perPage, function(clips) {
                obj._returnfn(clips);
                obj.prefetch();
            });
        }
    } 
    this.hasPrev = function() {
        if (this._position - this._perPage >= 0) return true;
        else return false;
    }
    this.hasNext = function() {
        if (!this._srchCtrl._searchComplete) return true;
        else {
            if (this._position + this._perPage < this._srchCtrl._clipList.length) return true;
            else return false;
        }
    }
    this.prefetch = function(numPages) {
        var prefetchPages = numPages || this._prefetch;
        //Prefetch the next page(s) (if possible)
        if (this.hasNext() && prefetchPages > 0) {
            //Ok so what's going on here? I'll tell you...
            //We want to be able to prefetch the next page(s) so that the load appears seemless to the user
            //So after we get the next set of clips, if we can get more, we attempt to with no callback defined
            //Why no callback? Because we're not going to do anything with that data just yet, we just want
            //it to be loaded so when we do ask for it, we're not waiting for the service call...INGENIOUS!
            var start = this._position + this._perPage;
            var end = start + (this._perPage * prefetchPages);
            this._srchCtrl.getRange(start, end);
        }
    }

    //Initialize the Paginator
    var obj = this;
    this._srchCtrl.getRange(this._position, this._perPage, function(clips) {
        obj._returnfn(clips);
        obj.prefetch();
    });
}

/////////////////////////////////
//        LazyLoadList         //
/////////////////////////////////
RedLasso.Util.LazyLoadList = function(objType, svcObj) {
    //Some Error checking.
    //NOTE: I'm not sure how I feel about instantiating the objType just for a test
    //but then again, its only happening once in the constructor, so screw it...
    if (new objType instanceof RedLasso.Domain.BaseObject != true) {
        throw ("The 'objType' parameter of RedLasso.Util.LazyLoadList must implement RedLasso.Domain.BaseObject");
        return;
    }
    if (svcObj instanceof RedLasso.Service.Base != true) {
        throw ("The 'svcObj' parameter of RedLasso.Util.LazyLoadList must implement RedLasso.Service.Base.");
        return;
    }
    this._svcObj = svcObj;
    this._objType = objType;
    this._objList = new Array();
    this._objListGuidMap = new Object();
    this._callbackList = new Object();
    this.length = this._objList.length;

    this.clear = function() {
        //Clear all of our internal arrays...
        this._objList = new Array();
        this._callbackList = new Object();
        this._objListGuidMap = new Object();
        this.length = 0;
    }
    this.add = function(param) {
        //If we're adding the correct object, just do it...
        if (param instanceof this._objType) {
            //If the obj we're trying to add already exists, just update it...
            if (typeof this._objListGuidMap[param.Guid] != "undefined")
                this._objList[this._objListGuidMap[param.Guid]] = param;
            else {
                this._objListGuidMap[param.Guid] = this._objList.push(param) - 1;
                this.length = this._objList.length;
            }
        }
        //If this is a valid guid, then do some more validation
        else if (RedLasso.Util.GuidValidator.test(param)) {
            //If it doesn't already exist, create the clip...otherwise, ignore the add.
            if (typeof this._objListGuidMap[param] == "undefined") {
                var tmpObj = new this._objType;
                tmpObj.Guid = param;
                this._objListGuidMap[tmpObj.Guid] = this._objList.push(tmpObj) - 1;
                this.length = this._objList.length;
            }
        }
        //Must be an exception...
        else throw ("The input parameter of RedLasso.Util.LazyLoadList.add() must be the declared 'objType' or be a Valid GUID.");
    }
    this.addRange = function(param) {
        if (param instanceof Array) {
            for (var i = 0; i < param.length; i++) {
                //If its matches the objType, just add it...
                if (param[i] instanceof this._objType) {
                    //If the obj we're trying to add already exists, just update it...
                    if (typeof this._objListGuidMap[param.Guid] != "undefined")
                        this._objList[this._objListGuidMap[param.Guid]] = param;
                    else
                        this._objListGuidMap[param.Guid] = this._objList.push(param) - 1;
                }
                //If its a valid guid, do some more validation
                else if (RedLasso.Util.GuidValidator.test(param[i])) {
                    //If it doesn't already exist, create the clip...otherwise, ignore the add.
                    if (typeof this._objListGuidMap[param[i]] == "undefined") {
                        var tmpObj = new this._objType;
                        tmpObj.Guid = param[i];
                        this._objListGuidMap[tmpObj.Guid] = this._objList.push(tmpObj) - 1;
                    }
                }
                else throw ("The input array of RedLasso.Util.LazyLoadList.addRange() contains an invalid element.");
            }
            this.length = this._objList.length;
        }
        else throw ("The input parameter of RedLasso.Util.LazyLoadList.addRange() must be an array.");
    }
    this.get = function(index, callback) {
        //Some error handling...
        if (typeof this._objList[index] == "undefined") throw ("Undefined index requested for RedLasso.Util.LazyLoadList.get().");

        //If the obj has not been fully loaded yet, lets do it...
        if (!this._objList[index]._loaded) {
            var callbackUID = (new Date()).getTime();
            this._callbackList[callbackUID] = callback;
            var obj = this;
            this._svcObj.get(new Array(this._objList[index].Guid), function(data) { obj._getComplete(data, callbackUID); });
        }
        else callback(this._objList[index]);
    }
    this.getByGuid = function(guid, callback) {
        //Some error checking...
        if (typeof this._objListGuidMap[guid] == "undefined") throw ("Undefined GUID requested for RedLasso.Util.LazyLoadList.getByGuid().");

        //Call the get() method
        this.get(this._objListGuidMap[guid], callback);
    }
    this.getRange = function(startIndex, endIndex, callback) {
        if (startIndex >= this._objList.length) throw ("RedLasso.Util.LazyLoadList.getRange() 'startIndex' is out of range.");
        //the end index is not required
        if (typeof endIndex == "undefined" || endIndex == null || endIndex == "") endIndex = this._objList.length;

        var lazyObjList = new Array();
        for (var i = startIndex; i < endIndex; i++) {
            if (!this._objList[i]._locked && !this._objList[i]._loaded) {
                lazyObjList.push(this._objList[i].Guid);
                //I'm not sure if I like this method, but if the Guid we're
                //about to request is not actually in the system, it doesn't
                //get returned, so we need a way to know we tried this object already...
                this._objList[i]._locked = true;
            }
        }

        //If its greater than 0, then we have some stragglers that need info!
        if (lazyObjList.length > 0) {
            var callbackUID = (new Date()).getTime();
            this._callbackList[callbackUID] = [startIndex, endIndex, callback];
            var obj = this;
            this._svcObj.get(lazyObjList, function(data) { obj._getRangeComplete(data, callbackUID); });
        }
        //we already have all the clipinfo...
        else if (callback) {
            //Unfortunately we need to clean the results as we do not want
            //to return any clips that are not loaded...
            //TODO: The array should delete failed clips and resize to accomodate...
            var results = new Array();
            for (var i = startIndex; i < endIndex; i++) {
                if (this._objList[i]._loaded) results.push(this._objList[i]);
            }
            callback(results);
        }
    }
    this._getComplete = function(data, callbackUID) {
        var tmpObj = null;
        if (data != null) {
            tmpObj = new this._objType(data[0]);
            this._objList[this._objListGuidMap[tmpObj.Guid]] = tmpObj;
        }
        this._callbackList[callbackUID](tmpObj);
        delete this._callbackList[callbackUID];
    }
    this._getRangeComplete = function(data, callbackUID) {
        //Handle for possible null
        if (data == null) {
            if (typeof this._callbackList[callbackUID] != "undefined" &&
                typeof this._callbackList[callbackUID][2] == "function")
                this._callbackList[callbackUID][2](null);
            return;
        }

        for (var i = 0; i < data.length; i++) {
            var tmpObj = new this._objType(data[i]);
            this._objList[this._objListGuidMap[tmpObj.Guid]] = tmpObj;
        }
        //recursively call the original function again, but this time
        //all elements should be in the array...
        if (typeof this._callbackList[callbackUID] != "undefined") {
            var cbArgs = this._callbackList[callbackUID];
            this.getRange(cbArgs[0], cbArgs[1], cbArgs[2]);
            delete this._callbackList[callbackUID];
        }
    }
}

//////////////////////////////
//       Validators         //
//////////////////////////////
RedLasso.Util.GuidValidator = new RegExp("^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
RedLasso.Util.CategoryValidator = new RegExp("^(ne|sr|pu|fi|fn)$", "i");
RedLasso.Util.ResKeysValidator = new RegExp("^(customerid|userid|category)$", "i");

//**********************************************************************************************

//******************************
//****** RedLasso.Domain *******
//******************************
if (typeof RedLasso == "undefined") RedLasso = {};
if (typeof RedLasso.Domain == "undefined") RedLasso.Domain = {};

/////////////////////////////////
//        Base Object          //
/////////////////////////////////
RedLasso.Domain.BaseObject = function() {
    //Common Parameters
    this.Guid = null;
}

/////////////////////////////////
//     Serializable Object     //
/////////////////////////////////
RedLasso.Domain.SerializableObject = function() {
    this.serialize = function(includeNullValues) {
        var result = "";
        for (var key in this) {
            //gotta handle for arrays...
            if (this[key] instanceof Array) {
                result += "\"" + key + "\":[";
                var innerRes = "";
                for (var i = 0; i < this[key].length; i++) {
                    if (typeof this[key][i] == "object") {
                        var res = "";
                        for (var k in this[key][i])
                            res += "\"" + k + "\":\"" + this[key][i][k] + "\",";
                        innerRes += "{" + res.substr(0, res.length - 1) + "},";
                    }
                    else if (typeof this[key][i] != "function") {
                        innerRes += "\"" + this[key][i] + "\",";
                    }
                }
                result += innerRes.substr(0, innerRes.length - 1) + "],";
            }
            else if (typeof this[key] != "function" && typeof this[key] != "object") {
                if (!includeNullValues && (this[key] != null && this[key] != ""))
                    result += "\"" + key + "\":\"" + this[key] + "\",";
            }
        }
        if (result.length > 0) {
            result = "{" + result.substr(0, result.length - 1) + "}";
        }

        return result;
    }
}
RedLasso.Domain.SerializableObject.prototype = new RedLasso.Domain.BaseObject;

/////////////////////////////////
//            Clip             //
/////////////////////////////////
RedLasso.Domain.Clip = function(splat_data) {
    this.Guid = "";
    this.Title = "";
    this.Description = "";
    this.Category = "";
    this.Author = "";
    this.AirDate = "";
    this.TimeZone = "";
    this.Duration = "";
    this.Views = "";
    this.ThumbUrl = "";
    this.StationLogo = "";
    this.StationUrl = "";
    this.MetaData = new Object();
    this._loaded = false;
    this._locked = false;

    //If we're passing in the R/L Splat Data, lets build this clip...
    if (splat_data && typeof splat_data.cd != "undefined") {
        for (var i = 0; i < splat_data.cd.length; i++) {
            if (splat_data.cd[i].k == "g") this.Guid = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "t") this.Title = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "dtl") this.Description = unescape(decodeURI(splat_data.cd[i].v));
            else if (splat_data.cd[i].k == "category") this.Category = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "author") this.Author = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "aired") this.AirDate = new Date(splat_data.cd[i].v); //GMT...Don't forget!
            else if (splat_data.cd[i].k == "tz") this.TimeZone = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "d") this.Duration = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "views") this.Views = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "thumburl") this.ThumbUrl = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "logo") this.StationLogo = splat_data.cd[i].v;
            else if (splat_data.cd[i].k == "stationurl") this.StationUrl = splat_data.cd[i].v;
            else this.MetaData[splat_data.cd[i].k] = splat_data.cd[i].v;
        }
        this._loaded = true;
    }

    this.getFormattedDuration = function() {
        //Clean up the duration...
        var dur = (this.Duration % 60).toString();
        if (dur.length != 2) dur = "0" + dur;
        return Math.floor(this.Duration / 60) + ":" + dur;
    }
}
RedLasso.Domain.Clip.prototype = new RedLasso.Domain.SerializableObject;

/////////////////////////////////
//            User             //
/////////////////////////////////
RedLasso.Domain.User = function(splat_data) {
    this.Guid = "";
    this.FirstName = "";
    this.LastName = "";
    this.FullName = "";
    this.DisplayName = "";
    this.Password = "";
    this.Email = "";
    this.Comment = "";
    this._loaded = false;
    this._locked = false;

    if (typeof splat_data != "undefined") {
        this.Guid = splat_data.Guid;
        this.FirstName = splat_data.FirstName;
        this.LastName = splat_data.LastName;
        this.FullName = splat_data.FullName;
        this.DisplayName = splat_data.DisplayName;
        this.Password = splat_data.Password;
        this.Email = splat_data.Email;
        this.Comment = splat_data.Comment;
        this._loaded = true;
    }
}
RedLasso.Domain.User.prototype = new RedLasso.Domain.SerializableObject;

/////////////////////////////////
//       ClipSearchRequest     //
/////////////////////////////////
RedLasso.Domain.ClipSearchRequest = function() {
    this._terms = "";
    this._maxResults = 0;
    this._searchOffset = 0;
    this._metaData = new Object();

    //***Category Getter/Setter
    this.getCategory = function() {
        if (typeof this._metaData["category"] == "undefined") return null;
        else return this._metaData["category"];
    }
    this.setCategory = function(cat) {
        if (RedLasso.Util.CategoryValidator.test(cat))
            this._metaData["category"] = cat;
        else throw ("The category: '" + cat + "' is an invalid RedLasso Category");
    }

    //***PartnerId Getter/Setter
    this.getPartnerId = function() {
        if (typeof this._metaData["customerid"] == "undefined") return null;
        else return this._metaData["customerid"];
    }
    this.setPartnerId = function(pid) {
        if (pid == "" || pid == null)
            delete this._metaData["customerid"];
        else if (typeof this._metaData["userid"] != "undefined")
            throw ("A PartnerID may not be set if a UserID has already been set.");
        else if (RedLasso.Util.GuidValidator.test(pid))
            this._metaData["customerid"] = pid;
        else throw ("The PartnerID: '" + pid + "' is not a valid GUID.");
    }

    //***UserId Getter/Setter
    this.getUserId = function() {
        if (typeof this._metaData["userid"] == "undefined") return null;
        else return this._metaData["userid"];
    }
    this.setUserId = function(uid) {
        if (uid == "" || uid == null)
                delete this._metaData["userid"];
        else if (typeof this._metaData["customerid"] != "undefined")
            throw ("A UserID may not be set if a PartnerID has already been set.");        
        else if (RedLasso.Util.GuidValidator.test(uid))
            this._metaData["userid"] = uid;
        else throw ("The UserID: '" + uid + "' is not a valid GUID.");
    }

    //Max Results Getter/Setter
    this.getMaxResults = function() { return this._maxResults; }
    this.setMaxResults = function(mr) { this._maxResults = mr; }

    //Search Offset Getter/Setter
    this.getSearchOffset = function() { return this._searchOffset; }
    this.setSearchOffset = function(so) { this._searchOffset = so; }

    //Search Terms Getter/Setter
    this.getTerms = function() { return this._terms; }
    this.setTerms = function(terms) { this._terms = terms; }

    //MetaData Getter/Setter
    this.getMetaData = function() { return this._metaData; }
    this.addMetaData = function(key, value) {
        if (RedLasso.Util.ResKeysValidator.test(key) != true)
            this._metaData[key] = value;
        else throw ("'" + key + "' is an reserved RedLasso Keyword");
    }

};

/////////////////////////////////
//  FeaturedClipSearchRequest  //
/////////////////////////////////
RedLasso.Domain.FeaturedClipSearchRequest = function() { };
RedLasso.Domain.FeaturedClipSearchRequest.prototype = new RedLasso.Domain.ClipSearchRequest;

//////////////////////
// RawSearchRequest //
//////////////////////
RedLasso.Domain.RawSearchRequest = function() {
    this._terms = null;
    this._dateList = new Array();
    this._stationList = new Array();

    //Search Terms Getter/Setter
    this.getTerms = function() { return this._terms; }
    this.setTerms = function(terms) { this._terms = terms; }

    //DateList Getter/Setter
    this.getDateList = function() { return this._dateList; }
    this.addDate = function(date) {
        var dt = new Date(date);
        if (dt instanceof Date) this._dateList.push(dt);
        else throw ("The parameter 'date' of RedLasso.Domain.RawSearchRequest.addDate() must parse to a valid date.");
    }

    //StationList Getter/Setter
    this.getStationList = function() { return this._stationList; }
    this.addStation = function(stationID) {
        this._stationList.push(stationID);
    }
}

//**********************************************************************************************

//*******************************
//****** RedLasso.Service *******
//*******************************
if (typeof RedLasso == "undefined") RedLasso = {};
if (typeof RedLasso.Service == "undefined") RedLasso.Service = {};

/////////////////////////////////
//         Base Service        //
/////////////////////////////////
RedLasso.Service.Base = function() {
    this.generic_callback = function(data, state) {
        var callback = state.get("callback");
        if (callback)
            callback.call(this, data);
    }
    //This 'get' function is meant to be implemented...
    //(but J/S doesn't really include interfaces)
    this.get = function() { }
};

/////////////////////////////////
//          Category           //
/////////////////////////////////
RedLasso.Service.Category = function() {
    this.get = function(callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/categories/getCategories";

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
}
RedLasso.Service.Category.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//          CC Search          //
/////////////////////////////////
RedLasso.Service.CCSearch = function() {
    this.search = function(rsRequest, callback) {
        if (rsRequest instanceof RedLasso.Domain.RawSearchRequest) {
            var dtl = rsRequest.getDateList();
            for (var i = 0; i < dtl.length; i++)
                dtl[i] = dtl[i].getFullYear() + "-" + (dtl[i].getMonth() + 1) + "-" + dtl[i].getDate() +
                    " " + dtl[i].getHours() + ":" + dtl[i].getMinutes() + ":" + dtl[i].getSeconds();

            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/raw/searchCC";
            request.addParam("t", rsRequest.getTerms());
            request.addParam("sl", "[\"" + rsRequest.getStationList().join("\",\"") + "\"]");
            request.addParam("dtl", "[\"" + rsRequest.getDateList().join("\",\"") + "\"]");

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else throw ("The parameter 'rsRequest' of RedLasso.Service.CCSearch.search() must be of type RedLasso.Domain.RawSearchRequest");
    }
    this.get = function(keys, callback) {
        if (keys instanceof Array) {
            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/raw/searchCC";
            request.addParam("ck", "[\"" + keys.join("\",\"") + "\"]");

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else throw ("The parameter 'keys' of RedLasso.Service.CCSearch.search() must be of type Array");
    }
    //TODO: This whole process is stupid and the player should make getVars calls for raw media just like clips...
    this.getThickMedia = function(media, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/vars/getVars";
        request.addParam("eid", media.RecId);

        RedLasso.Util.JSONP.sendRequest(request, this, function(data) {
            for (key in data) {
                if (typeof data[key] != "function" && typeof data[key] != "object")
                    media[key] = data[key];
            }
            if (callback) callback(media);
        });
    }
}
RedLasso.Service.CCSearch.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//            Clip             //
/////////////////////////////////
RedLasso.Service.Clip = function() {
    this.searchClips = function(csRequest, callback) {
        //TODO: This whole concept of s=0 or s=1 for featured/regular search is just stupid...
        //If it's a featured clips request, set it to 1
        //Else if it's a standard ClipSearchRequest, then set it to 0
        //Featured has to be checked first, because it is technically also a ClipSearchRequest due to inheritance
        var searchType = null;
        if (csRequest instanceof RedLasso.Domain.FeaturedClipSearchRequest) searchType = 1;
        else if (csRequest instanceof RedLasso.Domain.ClipSearchRequest) searchType = 0;

        //Check to make sure the request is of valid type for processing
        if (searchType != null) {
            //Time to deal with the Filter...
            var filters = "";
            var metaData = csRequest.getMetaData();
            for (var key in metaData) {
                if (key != "" && metaData[key] != "" && typeof metaData[key] != "function" && typeof metaData[key] != "object")
                    filters += "{\"k\":\"" + key + "\",\"v\":\"" + metaData[key] + "\"},";
            }
            //Now to add the brackets and remove the trailing comma if any filters exist
            if (filters != "")
                filters = "[" + filters.substr(0, (filters.length - 1)) + "]";

            //Build the JSONP request
            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/clip/search";
            request.addParam("st", csRequest.getTerms());
            request.addParam("kv", filters);
            request.addParam("mr", csRequest.getMaxResults());
            request.addParam("os", csRequest.getSearchOffset());
            request.addParam("s", searchType);

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else
            throw ("The searchClips paramater 'request' must be of type RedLasso.Domain.ClipSearchRequest or RedLasso.Domain.FeaturedClipSearchRequest.");
    }

    this.get = function(guids, callback) {
        if (guids instanceof Array && guids.length > 0) {
            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/clip/get";
            request.addParam("g", "[\"" + guids.join("\",\"") + "\"]");

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else throw ("The 'guids' parameter of RedLasso.Service.Clip.get() must be an Array.");
    }

    this.update = function(clip, callback) {
        if (clip instanceof RedLasso.Domain.Clip) {
            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/clip/update";
            request.addParam("c", clip.serialize());

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else throw ("The 'clip' parameter of RedLasso.Service.Clip.update() must be of type RedLasso.Domain.Clip.");
    }
}
RedLasso.Service.Clip.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//        Closed Caption       //
/////////////////////////////////
RedLasso.Service.ClosedCaption = function() {
    this.getRawCC = function(rfGuid, startTime, endTime, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/cc/getClosedCaption";
        request.addParam("fid", rfGuid);
        request.addParam("startTime", startTime);
        request.addParam("endTime", endTime);

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
    this.getClipCC = function(clipGuid, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/cc/getClosedCaption";
        request.addParam("clid", clipGuid);

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
}
RedLasso.Service.ClosedCaption.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//       Meta Template         //
/////////////////////////////////
RedLasso.Service.MetaTemplate = function() {
    this.get = function(userGuid, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/cust/getMetaTemplate";
        request.addParam("uid", userGuid);

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
}
RedLasso.Service.MetaTemplate.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//      Suggested Search       //
/////////////////////////////////
RedLasso.Service.SuggestedSearch = function() {
    this.get = function(maxResults, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/raw/getSuggestedSearches";
        request.addParam("mr", maxResults);

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
}
RedLasso.Service.SuggestedSearch.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//           Source            //
/////////////////////////////////
RedLasso.Service.Source = function() {
    this.get = function(type, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/sources/get";
        request.addParam("t", type);

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
    this.getClippable = function(callback) {
        this.get("clip", callback);
    }
    this.getViewable = function(callback) {
        this.get("view", callback);
    }
}
RedLasso.Service.Source.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//            User             //
/////////////////////////////////
RedLasso.Service.User = function() {
    this.createUser = function(user, callback) {
        if (user instanceof RedLasso.Domain.User) {
            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/user/create";
            request.addParam("u", user.serialize());

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else
            throw ("The createUser paramater 'user' must be of type RedLasso.Domain.User.");
    }
    this.updateUser = function(user, callback) {
        if (user instanceof RedLasso.Domain.User) {
            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/user/update";
            request.addParam("u", user.serialize());

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else
            throw ("The updateUser paramater 'user' must be of type RedLasso.Domain.User.");
    }
    this.get = function(guids, callback) {
        if (guids instanceof Array && guids.length > 0) {
            var request = new RedLasso.Util.JSONP.Request();
            request.svcSuffix = "svc/user/get";
            request.addParam("g", "[\"" + guids.join("\",\"") + "\"]");

            RedLasso.Util.JSONP.sendRequest(request, this, callback);
        }
        else throw ("The 'guids' parameter of RedLasso.Service.User.get() must be an Array.");
    }
    this.login = function(email, password, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/user/login";
        request.addParam("u", email);
        request.addParam("p", password);
        request.addParam("s", "1");

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
    this.logout = function(callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/user/login";
        request.addParam("s", "2");

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
    this.check = function(callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/user/login";
        request.addParam("s", "0");

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
    this.acceptTerms = function(email, password, termsVer, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/user/login";
        request.addParam("u", email);
        request.addParam("p", password);
        request.addParam("tv", termsVer);
        request.addParam("s", "3");

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
    this.retrievePassword = function(email, callback) {
        var request = new RedLasso.Util.JSONP.Request();
        request.svcSuffix = "svc/user/login";
        request.addParam("u", email);
        request.addParam("s", "4");

        RedLasso.Util.JSONP.sendRequest(request, this, callback);
    }
}
RedLasso.Service.User.prototype = new RedLasso.Service.Base;

/////////////////////////////////
//       Service Factory       //
/////////////////////////////////
RedLasso.Service.ServiceFactory = { };
RedLasso.Service.ServiceFactory._serviceMap = new Array();
RedLasso.Service.ServiceFactory._serviceMap["Category"] = new RedLasso.Service.Category();
RedLasso.Service.ServiceFactory._serviceMap["CCSearch"] = new RedLasso.Service.CCSearch();
RedLasso.Service.ServiceFactory._serviceMap["Clip"] = new RedLasso.Service.Clip();
RedLasso.Service.ServiceFactory._serviceMap["ClosedCaption"] = new RedLasso.Service.ClosedCaption();
RedLasso.Service.ServiceFactory._serviceMap["MetaTemplate"] = new RedLasso.Service.MetaTemplate();
RedLasso.Service.ServiceFactory._serviceMap["Source"] = new RedLasso.Service.Source();
RedLasso.Service.ServiceFactory._serviceMap["SuggestedSearch"] = new RedLasso.Service.SuggestedSearch();
RedLasso.Service.ServiceFactory._serviceMap["User"] = new RedLasso.Service.User();
RedLasso.Service.ServiceFactory.CreateService = function(serviceName) {
    return RedLasso.Service.ServiceFactory._serviceMap[serviceName];
}
