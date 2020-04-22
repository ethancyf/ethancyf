Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration

Partial Public Class reportCriteriaSP
    Inherits System.Web.UI.UserControl
    Implements IReportCriteriaUC

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#End Region

#Region "Implement IReportCriteriaUC"

    'CRE13-003 Token Replacement [Start][Karl]
    'Public Sub Build(ByVal dicSetting As Dictionary(Of String, String)) Implements IReportCriteriaUC.Build
    Public Sub Build(ByVal dicSetting As Dictionary(Of String, String), ByVal strSetting As String) Implements IReportCriteriaUC.Build
        'Sub Build(ByVal dicSetting As Dictionary(Of String, String))
        ' Retrieve Setting:
        ' E.g. <SettingName> Description [DefaultValue]
        ' <LabelText> The text of the first label. Also used for displaying error message. [Label]
        ' <ShowOptionPleaseSelect> Whether to show the option "Please Select". [T]
        ' <SP> The stored procedure to read the items for the dropdown list. []
        ' <DropDownWidth> The width the of the dropdown list. [200]

        Dim strLabelText As String = "Label"
        If dicSetting.ContainsKey("LabelText") Then strLabelText = dicSetting("LabelText")

        Dim blnShowOptionPleaseSelect As Boolean = True
        If dicSetting.ContainsKey("ShowOptionPleaseSelect") Then blnShowOptionPleaseSelect = dicSetting("ShowOptionPleaseSelect") = "T"

        Dim strSP As String = String.Empty
        If dicSetting.ContainsKey("SP") Then strSP = dicSetting("SP")

        Dim strDropDownWidth As String = "200"
        If dicSetting.ContainsKey("DropDownWidth") Then strDropDownWidth = dicSetting("DropDownWidth")

        ' Initialize wording
        lblLabelText.Text = strLabelText

        ' Initialize dropdown
        ddlDropDown.Width = CInt(strDropDownWidth)

        ddlDropDown.Items.Clear()

        ddlDropDown.DataSource = (New FileGenerationBLL).RetrieveDropDownItemBySP(strSP)
        ddlDropDown.DataValueField = "ValueField"
        ddlDropDown.DataTextField = "TextField"
        ddlDropDown.DataBind()

        If blnShowOptionPleaseSelect Then
            ddlDropDown.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), "---"))
        End If

        ddlDropDown.SelectedIndex = 0

        ' Initialize error image
        imgErrorDropDown.Visible = False

    End Sub

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByVal dicSetting As Dictionary(Of String, String), ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput
        ' Retrieve Setting:
        ' E.g. <SettingName> Description [DefaultValue]
        ' <DropDownAllowEmpty> Allow to select "Please select" [F]
        Dim blnDropDownAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("DropDownAllowEmpty") Then blnDropDownAllowEmpty = dicSetting("DropDownAllowEmpty") = "T"

        imgErrorDropDown.Visible = False

        If Not blnDropDownAllowEmpty AndAlso ddlDropDown.SelectedValue = "---" Then
            Dim strLabelText As String = "Label"
            If dicSetting.ContainsKey("LabelText") Then strLabelText = dicSetting("LabelText")

            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00022))
            lstErrorParam1.Add(strLabelText)
            lstErrorParam2.Add(String.Empty)

            imgErrorDropDown.Visible = True

        End If

    End Sub

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput
        Throw New NotImplementedException
    End Function

    Public Function GetParameterString(ByVal dicSetting As Dictionary(Of String, String)) As String Implements IReportCriteriaUC.GetParameterString
        Dim strLabelText As String = "Label"
        If dicSetting.ContainsKey("LabelText") Then strLabelText = dicSetting("LabelText")

        Return String.Format("{0}: {1}", strLabelText, ddlDropDown.SelectedValue)
    End Function

    Public Function GetParameterList(ByVal dicSetting As Dictionary(Of String, String)) As ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Dim udtParameterList As New ParameterCollection

        Dim blnDropDownAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("DropDownAllowEmpty") Then blnDropDownAllowEmpty = dicSetting("DropDownAllowEmpty") = "T"

        If blnDropDownAllowEmpty AndAlso ddlDropDown.SelectedValue = "---" Then Return udtParameterList

        Dim strLabelText As String = "Label"
        If dicSetting.ContainsKey("LabelText") Then strLabelText = dicSetting("LabelText")

        udtParameterList.AddParam(strLabelText, ddlDropDown.SelectedItem.Text)

        Return udtParameterList
    End Function

    Public Function GetCriteriaInput(ByVal dicSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String) As StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Dim udtStoreProcParamList As New StoreProcParamCollection

        udtStoreProcParamList.AddParam(String.Format("@Selected_Value{0}", strParameterSuffix), System.Data.SqlDbType.Char, 10, ddlDropDown.SelectedValue)

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