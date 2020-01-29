Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.UserRole

Partial Public Class reportCriteriaScheme
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
        ' <SchemeText> The text of the first label. Also used for displaying error message. [Scheme]
        ' <ShowOptionAll> Whether to show the option "All". [T]
        ' <ShowOptionPleaseSelect> Whether to show the option "Please Select". [T]
        ' <TurnOnAccessibleSchemeFilter> Only show the accessible schemes to the user. [T]

        Dim strSchemeText As String = "Scheme"
        If dicSetting.ContainsKey("SchemeText") Then strSchemeText = dicSetting("SchemeText")

        Dim blnShowOptionAll As Boolean = True
        If dicSetting.ContainsKey("ShowOptionAll") Then blnShowOptionAll = dicSetting("ShowOptionAll") = "T"

        Dim blnShowOptionPleaseSelect As Boolean = True
        If dicSetting.ContainsKey("ShowOptionPleaseSelect") Then blnShowOptionPleaseSelect = dicSetting("ShowOptionPleaseSelect") = "T"

        Dim blnTurnOnAccessibleSchemeFilter As Boolean = True
        If dicSetting.ContainsKey("TurnOnAccessibleSchemeFilter") Then blnTurnOnAccessibleSchemeFilter = dicSetting("TurnOnAccessibleSchemeFilter") = "T"

        ' Initialize wording
        lblSchemeText.Text = strSchemeText

        ' Initialize Scheme dropdown
        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection

        Dim udtSchemeCList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllDistinctSchemeClaim()

        If blnTurnOnAccessibleSchemeFilter Then
            Dim udtUserRoleCollection As UserRoleModelCollection = (New UserRoleBLL).GetUserRoleCollection((New HCVUUserBLL).GetHCVUUser.UserID)

            For Each udtSchemeC As SchemeClaimModel In udtSchemeCList
                For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                    If udtUserRoleModel.SchemeCode.Trim = udtSchemeC.SchemeCode Then
                        If Not udtSchemeClaimModelListFilter.Contains(udtSchemeC) Then udtSchemeClaimModelListFilter.Add(udtSchemeC)
                    End If
                Next
            Next

        Else
            ' Show all schemes
            udtSchemeClaimModelListFilter = udtSchemeCList

        End If

        ddlScheme.Items.Clear()

        ddlScheme.DataSource = udtSchemeClaimModelListFilter
        ddlScheme.DataValueField = "SchemeCode"
        ddlScheme.DataTextField = "DisplayCode"
        ddlScheme.DataBind()

        If blnShowOptionAll Then
            ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "All"), String.Empty))
        End If

        If blnShowOptionPleaseSelect Then
            ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), "---"))
        End If

        ddlScheme.SelectedIndex = 0

        ' Initialize error image
        imgErrorScheme.Visible = False

    End Sub

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByVal dicSetting As Dictionary(Of String, String), ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput
        ' Retrieve Setting:
        ' E.g. <SettingName> Description [DefaultValue]
        ' <SchemeAllowEmpty> Allow to select "Please select" [F]
        Dim blnSchemeAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("SchemeAllowEmpty") Then blnSchemeAllowEmpty = dicSetting("SchemeAllowEmpty") = "T"

        imgErrorScheme.Visible = False

        If Not blnSchemeAllowEmpty AndAlso ddlScheme.SelectedValue = "---" Then
            Dim strSchemeText As String = "Scheme"
            If dicSetting.ContainsKey("SchemeText") Then strSchemeText = dicSetting("SchemeText")

            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00022))
            lstErrorParam1.Add(strSchemeText)
            lstErrorParam2.Add(String.Empty)

            imgErrorScheme.Visible = True

        End If

    End Sub

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput
        Throw New NotImplementedException
    End Function

    Public Function GetParameterString(ByVal dicSetting As Dictionary(Of String, String)) As String Implements IReportCriteriaUC.GetParameterString
        Dim strSchemeText As String = "Scheme"
        If dicSetting.ContainsKey("SchemeText") Then strSchemeText = dicSetting("SchemeText")

        Return String.Format("{0}: {1}", strSchemeText, ddlScheme.SelectedValue)
    End Function

    Public Function GetParameterList(ByVal dicSetting As Dictionary(Of String, String)) As ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Dim udtParameterList As New ParameterCollection

        Dim blnSchemeAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("SchemeAllowEmpty") Then blnSchemeAllowEmpty = dicSetting("SchemeAllowEmpty") = "T"

        If blnSchemeAllowEmpty AndAlso ddlScheme.SelectedValue = "---" Then Return udtParameterList

        Dim strSchemeText As String = "Scheme"
        If dicSetting.ContainsKey("SchemeText") Then strSchemeText = dicSetting("SchemeText")

        udtParameterList.AddParam(strSchemeText, ddlScheme.SelectedItem.Text)

        Return udtParameterList
    End Function

    Public Function GetCriteriaInput(ByVal dicSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String) As StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Dim udtStoreProcParamList As New StoreProcParamCollection

        udtStoreProcParamList.AddParam(String.Format("@Scheme_Code{0}", strParameterSuffix), System.Data.SqlDbType.Char, 10, ddlScheme.SelectedValue)
        udtStoreProcParamList.AddParam(String.Format("@User_ID{0}", strParameterSuffix), System.Data.SqlDbType.Char, 20, (New HCVUUserBLL).GetHCVUUser.UserID)

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