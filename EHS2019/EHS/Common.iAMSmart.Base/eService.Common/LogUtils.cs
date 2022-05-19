using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using log4net;
using ServiceStack.Redis;
using Common.ComObject;

namespace eService.Common
{
    public class LogUtils  
    {
        //private static log4net.ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ILog _log;
       

        public LogUtils(Type type)
        {
            if (_log == null)
            {
                _log = LogManager.GetLogger(type);
            }
        }
        public void Info(object message)
        {
            _log.Info(message);
      
            //AuditLogEntry udtAuditLogEntry = new AuditLogEntry(FUNT020001, "DBFlag");
            //udtAuditLogEntry.WriteLog("iAMSmartLogin: " + message.ToString());
        }

        public void Debug(object message)
        {
            _log.Debug(message);
          
            //AuditLogEntry udtAuditLogEntry = new AuditLogEntry(FUNT020001, "DBFlag");
            //udtAuditLogEntry.WriteLog("iAMSmartLogin: " + message.ToString());
        }

        public void Warn(object message)
        {
            _log.Warn(message);
           
            //AuditLogEntry udtAuditLogEntry = new AuditLogEntry(FUNT020001, "DBFlag");
            //udtAuditLogEntry.WriteLog("iAMSmartLogin: " + message.ToString());
        }

        public void Error(object message)
        {
            _log.Error(message);
           
            //AuditLogEntry udtAuditLogEntry = new AuditLogEntry(FUNT020001, "DBFlag");
            //udtAuditLogEntry.WriteLog("00000","iAMSmartLogin: " + message.ToString());

        }
    }
}