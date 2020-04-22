Public Interface IStatisticsCriteriaUC

    Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
    Sub ValidateCriteriaInput(ByVal strStatisticsID As String, ByRef lstError As List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
    Function ValidateCriteriaInput(ByVal strStatisticsID As String) As List(Of Common.ComObject.SystemMessage)
    Function GetParameterString() As Common.ComFunction.ParameterFunction.ParameterCollection
    Function GetParameterList() As Common.ComFunction.ParameterFunction.ParameterCollection
    Function GetCriteriaInput() As Common.ComFunction.ParameterFunction.StoreProcParamCollection
    Sub SetErrorComponentVisibility(blnVisible As Boolean)
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Function GetReportGenerationDate() As DateTime?
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
End Interface
