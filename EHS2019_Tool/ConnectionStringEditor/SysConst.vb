Public Class SysConst
    Public Shared ReadOnly MATCHPARAMPATERN = "<add[ ]+key=""{0}""[ ]+value=""(?<paramValue>.+)""[ ]*/>"
    Public Shared ReadOnly CONNSTREPLACE = "<add key=""{0}"" value=""{1}"" />"

    Public Shared ReadOnly DB_CONNSTRFORMAT = "data source={0}; initial catalog={1}; persist security info=False; user id={2}; password={3}; packet size=4096; max pool size={4}"
    Public Shared ReadOnly DB_CONNSTR_DATASOURCE = "data source"
    Public Shared ReadOnly DB_CONNSTR_INITCATEGORY = "initial catalog"
    Public Shared ReadOnly DB_CONNSTR_USERID = "user id"
    Public Shared ReadOnly DB_CONNSTR_PWD = "password"
    Public Shared ReadOnly DB_CONNSTR_MAXPOOLSIZE = "max pool size"

    Public Enum CONFIGFILE_PARAMSTATUS
        NORMAL = 0
        EXTRA
        MISSING
    End Enum

    Public Shared ReadOnly PARAMFILTER = "ConnectionString"
    Public Shared ReadOnly NORMALPARAMCOLOR = Color.Green
    Public Shared ReadOnly MISSINGPARAMCOLOR = Color.Red
    Public Shared ReadOnly EXTRAPARAMCOLOR = Color.Blue

    Public Shared ReadOnly MYAPPSETTINGPATH = Environment.CurrentDirectory + "/AppMySetting.config"

    Public Shared ReadOnly READCONFIGFILENAME = "web.config"
    Public Shared ReadOnly READCONFIGFILEEXT = ".exe.config"
End Class
