Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component

Partial Public Class reportCriteriaPeriodType
    Inherits System.Web.UI.UserControl
    Implements IReportCriteriaUC

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Implements IReportCriteriaUC"
    'CRE13-003 Token Replacement [Start][Karl]
    'Public Sub Build(ByVal dicSetting As Dictionary(Of String, String)) Implements IReportCriteriaUC.Build
    Public Sub Build(ByVal dicSetting As Dictionary(Of String, String), ByVal strSetting As String) Implements IReportCriteriaUC.Build
        'Sub Build(ByVal dicSetting As Dictionary(Of String, String))

        ' Retrieve Setting:
        ' E.g. <SettingName> Description [DefaultValue]
        ' <PeriodTypeText> The text of the first label. Also used for displaying error message. [Period Type]
        ' <OptionText01> The text of the first option of Radio Button. [OptionText01]
        ' <OptionValue01> The value of the first option of Radio Button. [OptionValue01]
        ' <OptionText02> The text of the second option of Radio Button. [OptionText02]
        ' <OptionValue02> The value of the second option of Radio Button. [OptionValue02]
        ' <OptionText03> The text of the third option of Radio Button. [OptionText03]
        ' <OptionValue03> The value of the third option of Radio Button. [OptionValue03]

        Dim strPeriodTypeText As String = "Period Type"
        If dicSetting.ContainsKey("PeriodTypeText") Then strPeriodTypeText = dicSetting("PeriodTypeText")

        Dim strOptionText01 As String = "OptionText01"
        If dicSetting.ContainsKey("OptionText01") Then strOptionText01 = dicSetting("OptionText01")

        Dim strOptionValue01 As String = "OptionValue01"
        If dicSetting.ContainsKey("OptionValue01") Then strOptionValue01 = dicSetting("OptionValue01")

        Dim strOptionText02 As String = "OptionText02"
        If dicSetting.ContainsKey("OptionText02") Then strOptionText02 = dicSetting("OptionText02")

        Dim strOptionValue02 As String = "OptionValue02"
        If dicSetting.ContainsKey("OptionValue02") Then strOptionValue02 = dicSetting("OptionValue02")
		

        ' Initialize wording
        lblPeriodTypeText.Text = strPeriodTypeText

        rdolPeriodType.Items.Clear()
        rdolPeriodType.Items.Add(New ListItem(strOptionText01, strOptionValue01))
        rdolPeriodType.Items.Add(New ListItem(strOptionText02, strOptionValue02))

        ' CRE16-013 Voucher aberrant and new monitoring [Start][Dickson Law]
        If dicSetting.ContainsKey("OptionText03") AndAlso dicSetting.ContainsKey("OptionValue03") Then
            Dim strOptionText03 As String = "OptionText03"
            strOptionText03 = dicSetting("OptionText03")
            Dim strOptionValue03 As String = "OptionValue03"
            strOptionValue03 = dicSetting("OptionValue03")
            rdolPeriodType.Items.Add(New ListItem(strOptionText03, strOptionValue03))
        End If
        ' CRE16-013 Voucher aberrant and new monitoring [End][Dickson Law]

        rdolPeriodType.SelectedIndex = -1

        ' Initialize error image
        imgErrorPeriodType.Visible = False
    End Sub

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByVal dicSetting As Dictionary(Of String, String), ByRef lstError As List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput
        imgErrorPeriodType.Visible = False

        If rdolPeriodType.SelectedIndex = -1 Then
            Dim strPeriodTypeText As String = "Period Type"
            If dicSetting.ContainsKey("PeriodTypeText") Then strPeriodTypeText = dicSetting("PeriodTypeText")

            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00023))
            lstErrorParam1.Add(strPeriodTypeText)
            lstErrorParam2.Add(String.Empty)

            imgErrorPeriodType.Visible = True
        End If
    End Sub

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of Common.ComObject.SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput
        Throw New NotImplementedException
    End Function

    Public Function GetParameterString(ByVal dicSetting As Dictionary(Of String, String)) As String Implements IReportCriteriaUC.GetParameterString
        Dim strPeriodTypeText As String = "Period Type"
        If dicSetting.ContainsKey("PeriodTypeText") Then strPeriodTypeText = dicSetting("PeriodTypeText")

        Return String.Format("{0}: {1}", strPeriodTypeText, rdolPeriodType.SelectedValue)
    End Function

    Public Function GetParameterList(ByVal dicSetting As Dictionary(Of String, String)) As Common.ComFunction.ParameterFunction.ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Dim udtParameterList As New ParameterCollection

        Dim strPeriodTypeText As String = "Period Type"
        If dicSetting.ContainsKey("PeriodTypeText") Then strPeriodTypeText = dicSetting("PeriodTypeText")

        udtParameterList.AddParam(strPeriodTypeText, rdolPeriodType.SelectedItem.Text)

        Return udtParameterList
    End Function

    Public Function GetCriteriaInput(ByVal dicSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String) As Common.ComFunction.ParameterFunction.StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Dim udtStoreProcParamList As New StoreProcParamCollection

        udtStoreProcParamList.AddParam(String.Format("@period_type{0}", strParameterSuffix), System.Data.SqlDbType.Char, 1, rdolPeriodType.SelectedValue)

        Return udtStoreProcParamList
    End Function

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Function GetReportGenerationDate(ByVal dicSetting As Dictionary(Of String, String)) As DateTime? Implements IReportCriteriaUC.GetReportGenerationDate
        Return Nothing
    End Function
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
#End Region

End Class
