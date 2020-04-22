Public Class ParameterListModel
    Public Property strParamName As String
    Public Property strParamValue As String
    Public Property eParamStatus As SysConst.CONFIGFILE_PARAMSTATUS
End Class
Public Class ConfigFileViewModel
    Public Property pConfigFileRawDetail As Dictionary(Of String, List(Of ParameterListModel))
End Class
