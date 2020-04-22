Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component

Partial Public Class reportCriteriaSchemeProf
    Inherits System.Web.UI.UserControl
    Implements IReportCriteriaUC

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        

    End Sub

#Region "Implements IReportCriteriaUC"
    Public Sub Build(ByVal dicSetting As Dictionary(Of String, String), ByVal strSetting As String) Implements IReportCriteriaUC.Build
        ucStatisticsCriteriaBase.Build(strSetting)
        '   ucStatisticsCriteriaBase.Build("<ControlList><Control><CID>Scheme</CID><ClassName>udcSchemeProf.ascx</ClassName><Setting><Field><FID>Profession</FID><DescResource>HealthProf</DescResource><Visible>Y</Visible><DefaultValue></DefaultValue><SPParamName>@ProfessionList</SPParamName></Field><Field><FID>Scheme</FID><DescResource>Scheme</DescResource><Visible>N</Visible><DefaultValue></DefaultValue><SPParamName></SPParamName></Field></Setting><DisplaySeq>4</DisplaySeq></Control><RemarkResource></RemarkResource></ControlList>")
    End Sub

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByVal dicSetting As Dictionary(Of String, String), ByRef lstError As List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput
        Call ucStatisticsCriteriaBase.ValidateCriteriaInput(strReportID, lstError, lstErrorParam1, lstErrorParam2)
    End Sub

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of Common.ComObject.SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput
        Throw New NotImplementedException
    End Function

    Public Function GetParameterString(ByVal dicSetting As Dictionary(Of String, String)) As String Implements IReportCriteriaUC.GetParameterString
        ' CRE17-014 (Enhance eHSU0002) [Start][Winnie]
        'Return ucStatisticsCriteriaBase.GetParameterString(0).ParamValue.ToString

        Dim lstParameterString As New List(Of String)

        For Each udtParam As ParameterObject In ucStatisticsCriteriaBase.GetParameterString
            lstParameterString.Add(String.Format("{0}: {1}", udtParam.ParamName, udtParam.ParamValue))
        Next

        If lstParameterString.Count > 0 Then
            Return String.Join(",", lstParameterString.ToArray)
        Else
            Return String.Empty
        End If
        ' CRE17-014 (Enhance eHSU0002) [End][Winnie]
    End Function

    Public Function GetParameterList(ByVal dicSetting As Dictionary(Of String, String)) As Common.ComFunction.ParameterFunction.ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Return ucStatisticsCriteriaBase.GetParameterList
    End Function

    Public Function GetCriteriaInput(ByVal dicSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String) As Common.ComFunction.ParameterFunction.StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Return ucStatisticsCriteriaBase.GetCriteriaInput
    End Function
#End Region

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Function GetReportGenerationDate(ByVal dicSetting As Dictionary(Of String, String)) As DateTime? Implements IReportCriteriaUC.GetReportGenerationDate
        Return ucStatisticsCriteriaBase.GetReportGenerationDate
    End Function
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
End Class