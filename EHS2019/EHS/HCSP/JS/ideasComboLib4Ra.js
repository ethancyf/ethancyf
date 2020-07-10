/**
 * Javascript library for connecting to IdeasComboClient
 */
var IC4RA_LIB_VERSION = "1.0.0f";

// Customizable values
// Timeout value for connecting to IDEAS Combo Client (referenced from IDEAS2)
var IC4RA_TIMEOUTVALUE = 5000;

// eHs
var IC4RA_URL_CHECKIDEASCOMBOCLIENT_HTTP	= "http://127.0.0.1:44927/GetInfo";
var IC4RA_URL_CHECKIDEASCOMBOCLIENT_HTTPS	= "https://127.0.0.1:44928/GetInfo";

// The following codes are used by RA
var IC4RA_ERRORCODE_SUCCESS		= "E0000";
var IC4RA_ERRORCODE_NOCLIENT	= "E0009";

//var IC4RA_HTTP_READYSTATE_UNINITIALIZED	= 0;
//var IC4RA_HTTP_READYSTATE_OPEN			= 1;
//var IC4RA_HTTP_READYSTATE_SENT			= 2;
//var IC4RA_HTTP_READYSTATE_RECEIVING		= 3;
var IC4RA_HTTP_READYSTATE_LOADED		= 4;

//var IC4RA_HTTP_STATUS_NOTREADY	= 0;
var IC4RA_HTTP_STATUS_OK		= 200;


var url4IcRa = IC4RA_URL_CHECKIDEASCOMBOCLIENT_HTTPS;

function log4IcRa(msg) {
	if (typeof console === "undefined" || typeof console.log === "undefined") {
	} else if (console.log) {
		console.log(msg);
	}
}

var checkIdeasComboClientSuccessCallback;
var checkIdeasComboClientFailureCallback;
function checkIdeasComboClientSuccess(_param) {
	var param = {result:_param, ideasVer:"", artifactId:""};
	checkIdeasComboClientSuccessCallback(param);
}

function checkIdeasComboClientFailure(_param) {
	var param = {result:_param, ideasVer:"", artifactId:""};
	checkIdeasComboClientFailureCallback(param);
}

function checkIdeasComboClientCore(_onSuccess, _onFailure) {
	checkIdeasComboClientSuccessCallback = _onSuccess;
	checkIdeasComboClientFailureCallback = _onFailure;
	log4IcRa("checkIdeasComboClientCore(): url4IcRa: '" + url4IcRa + "'");

	if ('withCredentials' in new XMLHttpRequest()) {
		log4IcRa("checkIdeasComboClientCore(): Using XMLHttpRequest");
		xmlHttpRequest4IcRa = new XMLHttpRequest();
		xmlHttpRequest4IcRa.open("POST", url4IcRa, true);
		xmlHttpRequest4IcRa.setRequestHeader("Content-type","application/x-www-form-urlencoded");
		xmlHttpRequest4IcRa.timeout = IC4RA_TIMEOUTVALUE;
		xmlHttpRequest4IcRa.onreadystatechange = function() {
			if (xmlHttpRequest4IcRa != null) {
				if (xmlHttpRequest4IcRa.readyState == IC4RA_HTTP_READYSTATE_LOADED) {
					var status = xmlHttpRequest4IcRa.status;
					xmlHttpRequest4IcRa = null;
					if (status == IC4RA_HTTP_STATUS_OK) {
						checkIdeasComboClientSuccess(IC4RA_ERRORCODE_SUCCESS);
				    } else {
				    	checkIdeasComboClientFailure(IC4RA_ERRORCODE_NOCLIENT);
				    }
				} else {
				}
			}
		};
		xmlHttpRequest4IcRa.ontimeout = function() {
			if (xmlHttpRequest4IcRa != null) {
				xmlHttpRequest4IcRa = null;
		    	checkIdeasComboClientFailure(IC4RA_ERRORCODE_NOCLIENT);
			}
		};
		xmlHttpRequest4IcRa.send(null);
	} else if (typeof XDomainRequest !== "undefined") {
		log4IcRa("checkIdeasComboClientCore(): Using XDomainRequest");
		var xdr = new XDomainRequest();
		xdr.open("POST", url4IcRa);
		xdr.timeout = IC4RA_TIMEOUTVALUE;
		xdr.onload = function() {
			if (xdr != null) {
				xdr = null;
				checkIdeasComboClientSuccess(IC4RA_ERRORCODE_SUCCESS);
			}
		};
		xdr.onerror = function() {
			// XDomainRequest does not provide interface to get status
			// No way to get status code like 200, 404 etc
			// therefore always return IC4RA_ERRORCODE_NOCLIENT if error occurs
			if (xdr != null) {
				xdr = null;
				checkIdeasComboClientFailure(IC4RA_ERRORCODE_NOCLIENT);
			}
		};
		xdr.ontimeout = function () {
			if (xdr != null) {
				xdr = null;
				checkIdeasComboClientFailure(IC4RA_ERRORCODE_NOCLIENT);
			}
		};
		
		setTimeout(function() {xdr.send(null);}, 0);
	} else {
		checkIdeasComboClientFailure(IC4RA_ERRORCODE_NOCLIENT);
	}
}

function isFirefox4IcRa() {
	if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
		return true;
	}
	
	return false;
}

function requireHttp4IcRa() {
	// List out all the conditions that require HTTP to communicate with IDEAS Combo Client

//	// Firefox requires to install certificate manually for HTTPS
//	if (isFirefox4IcRa()) {
//		return true;
//	}
	
	// XDomainRequest object requires to use same protocol as the current page
	if (!('withCredentials' in new XMLHttpRequest())
			&& (typeof XDomainRequest !== "undefined")) {
		if (location.protocol == 'http:') {
			return true;
		}
	}
	
	return false;
}

function getRequiredUrl4IcRa() {
	if (requireHttp4IcRa()) {
		return IC4RA_URL_CHECKIDEASCOMBOCLIENT_HTTP;
	} else {
		return IC4RA_URL_CHECKIDEASCOMBOCLIENT_HTTPS;
	}
}

function checkIdeasComboClient(_onSuccess, _onFailure) {
	url4IcRa = getRequiredUrl4IcRa();
	
	checkIdeasComboClientCore(_onSuccess, _onFailure);
}

function getIdeasComboClientLib4RaVersion() {
	log4IcRa("getIdeasComboClientLib4RaVersion(): Entering: " + IC4RA_LIB_VERSION);
    return IC4RA_LIB_VERSION;
}
