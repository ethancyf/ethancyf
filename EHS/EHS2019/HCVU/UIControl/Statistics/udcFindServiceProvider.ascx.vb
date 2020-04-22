Imports System.Data.SqlTypes
Imports System.Globalization
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.BaseBLL
Imports Common.Component.FileGeneration
Imports Common.Component.ServiceProvider
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation

Partial Public Class udcFindServiceProvider
    Inherits StatisticsCriteriaUC

#Region "Variables"

#End Region

#Region "Session and Const"

    Public Class Field
        Public Const ServiceProvider As String = "ServiceProvider"
    End Class

    Public Class AdditionalFieldSetting
        Public Const FunctionCode As String = "FunctionCode"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub ibtnSPIDSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSPIDSearch.Click
        ' Init
        'udcInfoMessageBox.Visible = False
        'udcErrorMessage.Visible = False
        imgSPIDError.Visible = False

        'Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        'udtAuditLogEntry.AddDescripton("ServiceProviderID", txtP2SPID.Text)
        'udtAuditLogEntry.WriteStartLog(LogID.LOG00015, "Selection of Service Provider - Search click")

        Dim strSPID As String = txtSPID.Text.Trim

        ' --- Validation ---

        Dim blnError As Boolean = False

        ' Empty
        If strSPID = String.Empty Then
            'udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
            imgSPIDError.Visible = True
            blnError = True
        End If

        ' Valid format
        If blnError = False Then
            Dim udtSystemMessage As SystemMessage = (New Validator).chkSPID(strSPID)

            If Not IsNothing(udtSystemMessage) Then
                'udcErrorMessage.AddMessage(udtSystemMessage)
                imgSPIDError.Visible = True
                blnError = True
            End If

        End If

        ' Exist
        If blnError = False Then
            Dim udtSearchResult As BLLSearchResult = (New ServiceProviderBLL).GetServiceProviderMaintenanceSearch(String.Empty, _
                                                        String.Empty, strSPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                                                        New Database, False, False)

            Dim dt As DataTable = udtSearchResult.Data

            If dt.Rows.Count = 0 Then
                'udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
                imgSPIDError.Visible = True
                blnError = True

            Else
                Dim strEngName As String = CStr(dt.Rows(0)("SP_Eng_Name")).Trim
                Dim strChiName As String = CStr(dt.Rows(0)("SP_Chi_Name")).Trim

                If strChiName <> String.Empty Then
                    lblSPName.Text = String.Format("{0} ( {1} )", strEngName, strChiName)
                Else
                    lblSPName.Text = String.Format("{0}", strEngName)
                End If

            End If


        End If

        ' --- End of Validation ---

        If blnError Then
            Exit Sub
        End If

        txtSPID.Enabled = False
        ibtnSPIDSearch.Enabled = False
        ibtnSPIDSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchDisableSBtn")
        ibtnSPIDClear.Enabled = True
        ibtnSPIDClear.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearSBtn")


        'udtAuditLogEntry.AddDescripton("PracticeAvailableToAdd", dtPractice.Rows.Count)
        'udtAuditLogEntry.WriteEndLog(LogID.LOG00016, "Selection of Service Provider - Search click successful")

    End Sub

    Protected Sub ibtnSPIDClear_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSPIDClear.Click
        'udcInfoMessageBox.Visible = False
        'udcErrorMessage.Visible = False

        'Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        'udtAuditLogEntry.WriteStartLog(LogID.LOG00018, "Selection of Service Provider - Clear click")

        txtSPID.Text = String.Empty
        txtSPID.Enabled = True
        ibtnSPIDSearch.Enabled = True
        ibtnSPIDSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        ibtnSPIDClear.Enabled = False
        ibtnSPIDClear.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        imgSPIDError.Visible = False

        lblSPName.Text = String.Empty

        txtSPID.Focus()

        'udtAuditLogEntry.WriteEndLog(LogID.LOG00019, "Selection of Service Provider - Clear click successful")

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting

        MyBase.Build(dicSetting)

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' SPID
        If IsExistValue(Field.ServiceProvider, FieldSetting.Visible) Then
            If GetSetting(Field.ServiceProvider, FieldSetting.Visible) = Condition.YES Then
                ' Check SPID in textbox
                If Not CheckServiceProviderID(txtSPID.Text.Trim) Or lblSPName.Text = String.Empty Then
                    Dim strSPIDText As String = String.Empty

                    If IsExistValue(Field.ServiceProvider, FieldSetting.DescResource) Then
                        strSPIDText = Me.GetGlobalResourceObject("Text", GetSetting(Field.ServiceProvider, FieldSetting.DescResource))
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00316))
                    lstErrorParam1.Add(strSPIDText)
                    lstErrorParam2.Add(String.Empty)
                    imgSPIDError.Visible = True
                End If
            End If
        End If
    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' SPID
        If IsExistValue(Field.ServiceProvider, FieldSetting.Visible) Then
            If GetSetting(Field.ServiceProvider, FieldSetting.Visible) = Condition.YES Then
                udtParameterList.AddParam("SPID", txtSPID.Text.Trim)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' SPID
        If IsExistValue(Field.ServiceProvider, FieldSetting.Visible) Then
            If GetSetting(Field.ServiceProvider, FieldSetting.Visible) = Condition.YES Then
                udtParameterList.AddParam("SPID", txtSPID.Text.Trim)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' SPID
        If IsExistValue(Field.ServiceProvider, FieldSetting.Visible) Then
            If GetSetting(Field.ServiceProvider, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.ServiceProvider, FieldSetting.SPParamName) Then
                    Dim strParam As String = GetSetting(Field.ServiceProvider, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strParam, System.Data.SqlDbType.Char, 8, txtSPID.Text.Trim)
                End If

            ElseIf GetSetting(Field.ServiceProvider, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.ServiceProvider, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.ServiceProvider, FieldSetting.DefaultValue) Is String.Empty Then
                        If IsExistValue(Field.ServiceProvider, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As DateTime
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.ServiceProvider, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.ServiceProvider, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 8, strDefaultValue)
                        End If
                    End If
                End If
            End If

        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgSPIDError.Visible = blnVisible

    End Sub

#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - Service Provider Input
        SetServiceProviderInput()

    End Sub

    Private Sub InitializeDateValue()
        txtSPID.Text = String.Empty

        imgSPIDError.Visible = False

        ibtnSPIDSearch.Enabled = True
        ibtnSPIDSearch.ImageUrl = GetGlobalResourceObject("ImageUrl", "SearchSBtn")

        ibtnSPIDClear.Enabled = False
        ibtnSPIDClear.ImageUrl = GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

    End Sub

    Private Function CheckServiceProviderID(ByVal strSPID As String) As Boolean
        Dim blnValid As Boolean = True

        ' Empty
        If strSPID = String.Empty Then
            blnValid = False
        End If

        ' Valid format
        If blnValid Then
            Dim udtSystemMessage As SystemMessage = (New Validator).chkSPID(strSPID)

            If Not IsNothing(udtSystemMessage) Then
                blnValid = False
            End If
        End If

        ' If not valid, arise alert.
        If Not blnValid Then
            imgSPIDError.Visible = True
        End If

        Return blnValid

    End Function

#End Region

#Region "Fields Setting"

    ' Set item - Service Provider Input
    Private Sub SetServiceProviderInput()
        ' Set field description
        If IsExistValue(Field.ServiceProvider, FieldSetting.DescResource) Then
            lblSPIDText.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.ServiceProvider, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.ServiceProvider, FieldSetting.Visible) Then
            Select Case GetSetting(Field.ServiceProvider, FieldSetting.Visible)
                Case Condition.YES
                    panInput.Visible = True
                Case Condition.NO
                    panInput.Visible = False
                Case Else
                    panInput.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.ServiceProvider, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.ServiceProvider, FieldSetting.DefaultValue) = String.Empty Then
                txtSPID.Text = GetSetting(Field.ServiceProvider, FieldSetting.DefaultValue)
            End If
        End If

        If panInput.Visible Then
            InitializeDateValue()
        End If

    End Sub

#End Region

End Class