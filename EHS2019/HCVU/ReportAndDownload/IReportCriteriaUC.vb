Public Interface IReportCriteriaUC

    'CRE13-003 Token Replacement [Start][Karl]
    Sub Build(ByVal dicSetting As Dictionary(Of String, String), ByVal strSetting As String)
    'Sub Build(ByVal dicSetting As Dictionary(Of String, String))
    'CRE13-003 Token Replacement [End][Karl]
    Sub ValidateCriteriaInput(ByVal strReportID As String, ByVal dicSetting As Dictionary(Of String, String), ByRef lstError As List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
    Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of Common.ComObject.SystemMessage)
    Function GetParameterString(ByVal dicSetting As Dictionary(Of String, String)) As String
    Function GetParameterList(ByVal dicSetting As Dictionary(Of String, String)) As Common.ComFunction.ParameterFunction.ParameterCollection
    Function GetCriteriaInput(ByVal dicSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String) As Common.ComFunction.ParameterFunction.StoreProcParamCollection
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Function GetReportGenerationDate(ByVal dicSetting As Dictionary(Of String, String)) As DateTime?
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
End Interface
