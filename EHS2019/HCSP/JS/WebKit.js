﻿// JScript File

Sys.Browser.WebKit = {}; //Safari 3 is considered WebKit
if( navigator.userAgent.indexOf( 'WebKit/' ) > -1 )
{
  Sys.Browser.agent = Sys.Browser.WebKit;
  Sys.Browser.version = parseFloat( navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
  Sys.Browser.name = 'WebKit';
}