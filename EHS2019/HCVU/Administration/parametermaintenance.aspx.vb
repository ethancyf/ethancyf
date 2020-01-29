Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Validation
Imports CustomControls
Imports HCVU.ParameteMaintenanceBLL

Partial Public Class parametermaintenance
    Inherits BasePageWithGridView

#Region "Constants"

    Private Class SESS
        Public Const ParameterMaintenanceList As String = "SESS_ParameterMaintenanceList"
        Public Const ParameterMaintenanceDataTable As String = "SESS_ParameterMaintenanceDataTable"
    End Class

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Page set to no cache
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010901

        If Not Page.IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Parameter Maintenance loaded")

            InitControlOnce()

        End If

        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

    End Sub

    Protected Sub InitControlOnce()
        Dim udtParameteMaintenanceBLL As New ParameteMaintenanceBLL((New HCVUUserBLL).GetHCVUUser)

        ' Save the DataTable in Session
        Session(SESS.ParameterMaintenanceDataTable) = udtParameteMaintenanceBLL.GetSystemParametersForExternalUseDT

        ' Get ParameteMaintenanceModel collection by the datatable 
        Session(SESS.ParameterMaintenanceList) = udtParameteMaintenanceBLL.FillParameteMaintenanceModel(Session(SESS.ParameterMaintenanceDataTable))

        ' Bind GridView
        Me.GridViewDataBind(gvParameterMaintenance, Session(SESS.ParameterMaintenanceDataTable), "Category", "ASC", False)

    End Sub

    '

    Private Sub BindGvParameterMaintenance(Optional ByVal blnAllowSorting As Boolean = True)
        gvParameterMaintenance.AllowSorting = blnAllowSorting

        Me.GridViewDataBind(gvParameterMaintenance, Session(SESS.ParameterMaintenanceDataTable))

    End Sub

#End Region

    Protected Sub gvParameterMaintenance_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Key
            Dim hfGKey As HiddenField = e.Row.FindControl("hfGKey")
            hfGKey.Value = String.Format("{0}-{1}", _
                                         DirectCast(e.Row.FindControl("lblParameterID"), Label).Text, _
                                         DirectCast(e.Row.FindControl("lblCategory"), Label).Text)

            ' Parameter Value

            ' Use Bitwise Or to check edit row state, see manual for details
            Select Case e.Row.RowState
                Case DataControlRowState.Normal, DataControlRowState.Alternate
                    ' Nothing here

                Case DataControlRowState.Normal Or DataControlRowState.Edit, DataControlRowState.Alternate Or DataControlRowState.Edit
                    e.Row.FindControl("imgParameterValueError").Visible = False

            End Select

            ' Parameter Boundary
            Dim lblGParameterBoundary As Label = e.Row.FindControl("lblGParameterBoundary")

            Dim udtParameteMaintenance As ParameteMaintenanceModel = Session(SESS.ParameterMaintenanceList)(hfGKey.Value)

            If Not IsNothing(udtParameteMaintenance.ApplyLimit) AndAlso udtParameteMaintenance.ApplyLimit <> String.Empty Then
                ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
                If udtParameteMaintenance.ApplyLimit = Parm_ApplyLimit.Numeric Then
                    lblGParameterBoundary.Text = String.Format("({0} - {1})", udtParameteMaintenance.LowerLimit, udtParameteMaintenance.UpperLimit)
                Else
                    lblGParameterBoundary.Text = "N/A"
                End If
                ' CRE15-022 (Change of parameter maintenance) [End][Winnie]
            Else
                lblGParameterBoundary.Text = "N/A"
            End If

            ' Check Read Only
            ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
            If Not IsNothing(udtParameteMaintenance.ExternalUse) AndAlso udtParameteMaintenance.ExternalUse <> String.Empty Then
                If udtParameteMaintenance.ExternalUse = Parm_ExternalUse.Read_Only Then
                    Dim ibtnGEdit As ImageButton = e.Row.FindControl("ibtnGEdit")
                    ibtnGEdit.Visible = False
                End If
            End If
            ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

            ' Button
            If e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate Then
                Dim ibtnGEdit As ImageButton = e.Row.FindControl("ibtnGEdit")

                If gvParameterMaintenance.EditIndex <> -1 Then
                    ibtnGEdit.Enabled = False
                    ibtnGEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")

                End If

            End If
        End If

    End Sub

    Protected Sub gvParameterMaintenance_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SESS.ParameterMaintenanceDataTable)
    End Sub

    Protected Sub gvParameterMaintenance_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SESS.ParameterMaintenanceDataTable)
    End Sub

    '

    Protected Sub gvParameterMaintenance_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        Dim gvr As GridViewRow = gvParameterMaintenance.Rows(e.NewEditIndex)

        udtAuditLogEntry.AddDescripton("Category", DirectCast(gvr.FindControl("lblCategory"), Label).Text)
        udtAuditLogEntry.AddDescripton("Parameter ID", DirectCast(gvr.FindControl("lblParameterID"), Label).Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Edit Click")

        gvParameterMaintenance.EditIndex = e.NewEditIndex

        BindGvParameterMaintenance(blnAllowSorting:=False)

    End Sub

    Protected Sub gvParameterMaintenance_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        Dim gvr As GridViewRow = gvParameterMaintenance.Rows(e.RowIndex)
        Dim strOriParmaterValue As String = String.Empty

        udtAuditLogEntry.AddDescripton("Category", DirectCast(gvr.FindControl("lblCategory"), Label).Text)
        udtAuditLogEntry.AddDescripton("Parameter ID", DirectCast(gvr.FindControl("lblParameterID"), Label).Text.Trim)

        ' Get current Category and Parameter ID
        Dim strKey As String = DirectCast(gvr.FindControl("hfGKey"), HiddenField).Value
        Dim udtParameterMaintenance As ParameteMaintenanceModel = DirectCast(Session(SESS.ParameterMaintenanceList), ParameteMaintenanceModelCollection)(strKey)

        ' Fill value
        udtParameterMaintenance = udtParameterMaintenance.Clone

        ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
        strOriParmaterValue = udtParameterMaintenance.ParameterValue1
        udtAuditLogEntry.AddDescripton("Old Parameter Value", strOriParmaterValue)
        udtAuditLogEntry.AddDescripton("New Parameter Value", DirectCast(gvr.FindControl("txtGParameterValue"), TextBox).Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Save Click")

        'Format text for BO User
        If Not udtParameterMaintenance.ApplyLimit Is Nothing AndAlso udtParameterMaintenance.ApplyLimit = Parm_ApplyLimit.BOUserID Then
            Dim strParmValueResult As String = String.Empty
            Dim strParmValue As String = DirectCast(gvr.FindControl("txtGParameterValue"), TextBox).Text.Trim().ToUpper

            For Each value As String In strParmValue.Split(",")
                strParmValueResult += "," + value.Trim
            Next

            If Not strParmValueResult.Equals(String.Empty) Then
                strParmValueResult = strParmValueResult.Substring(1)
            End If

            DirectCast(gvr.FindControl("txtGParameterValue"), TextBox).Text = strParmValueResult
        End If
        ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

        udtParameterMaintenance.ParameterValue1 = DirectCast(gvr.FindControl("txtGParameterValue"), TextBox).Text.Trim

        ' --- Validation ---
        ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
        If Not CheckParameterValue(udtParameterMaintenance) Then
            gvr.FindControl("imgParameterValueError").Visible = True
            udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00003, "Save Click failed")
            Return
        End If
        ' --- End of Validation ---

        ' Format value for numberic type parameter
        If Not udtParameterMaintenance.ApplyLimit Is Nothing AndAlso udtParameterMaintenance.ApplyLimit = Parm_ApplyLimit.Numeric Then

            Dim intParameterValue As Integer = Integer.Parse(udtParameterMaintenance.ParameterValue1)
            DirectCast(gvr.FindControl("txtGParameterValue"), TextBox).Text = intParameterValue
            udtParameterMaintenance.ParameterValue1 = intParameterValue
        End If
        ' CRE15-022 (Change of parameter maintenance) [End][Winnie]


        Try
            Dim udtParameteMaintenanceBLL As New ParameteMaintenanceBLL((New HCVUUserBLL).GetHCVUUser)

            udtParameteMaintenanceBLL.SaveSystemParametersForExternalUse(udtParameterMaintenance)

            ' Get DataTable from DB again
            Session(SESS.ParameterMaintenanceDataTable) = udtParameteMaintenanceBLL.GetSystemParametersForExternalUseDT
            Session(SESS.ParameterMaintenanceList) = udtParameteMaintenanceBLL.FillParameteMaintenanceModel(Session(SESS.ParameterMaintenanceDataTable))

        Catch udtParameterSaveSQLException As ParameterSaveSQLException
            If Not IsNothing(udtParameterSaveSQLException.SystemMessage) Then
                udcMessageBox.AddMessage(udtParameterSaveSQLException.SystemMessage)
                udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00003, "Save Click failed")

                Return

            Else
                Throw
            End If

        Catch ex As Exception
            Throw

        End Try

        gvParameterMaintenance.EditIndex = -1

        BindGvParameterMaintenance()

        Dim aryOldChar As String() = {"{Para_ID}", "{Row}"}
        Dim aryNewChar As String() = {DirectCast(gvr.FindControl("lblParameterID"), Label).Text.Trim, _
                                      DirectCast(gvr.FindControl("lblRow"), Label).Text.Trim}

        udcInfoMessageBox.Type = InfoMessageBoxType.Complete
        udcInfoMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, aryOldChar, aryNewChar)
        udcInfoMessageBox.BuildMessageBox()

        ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
        udtAuditLogEntry.AddDescripton("Category", DirectCast(gvr.FindControl("lblCategory"), Label).Text)
        udtAuditLogEntry.AddDescripton("Parameter ID", DirectCast(gvr.FindControl("lblParameterID"), Label).Text.Trim)
        udtAuditLogEntry.AddDescripton("Old Parameter Value", strOriParmaterValue)
        udtAuditLogEntry.AddDescripton("New Parameter Value", udtParameterMaintenance.ParameterValue1)
        ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Save Click successful")

    End Sub

    Protected Sub gvParameterMaintenance_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        Dim gvr As GridViewRow = gvParameterMaintenance.Rows(e.RowIndex)

        udtAuditLogEntry.AddDescripton("Category", DirectCast(gvr.FindControl("lblCategory"), Label).Text)
        udtAuditLogEntry.AddDescripton("Parameter ID", DirectCast(gvr.FindControl("lblParameterID"), Label).Text.Trim)
        udtAuditLogEntry.WriteLog(LogID.LOG00005, "Cancel Click")

        gvParameterMaintenance.EditIndex = -1

        BindGvParameterMaintenance()

    End Sub

    ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
    Private Function CheckParameterValue(ByVal udtParameterMaintenance As ParameteMaintenanceModel) As Boolean
        Dim blnIsValid As Boolean = True
        Dim sm As SystemMessage = Nothing

        Dim udtParameteMaintenanceBLL As New ParameteMaintenanceBLL((New HCVUUserBLL).GetHCVUUser)

        If udtParameterMaintenance.ParameterValue1 = String.Empty Then
            blnIsValid = False
            udcMessageBox.AddMessage(New SystemMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
        End If

        If blnIsValid AndAlso udtParameterMaintenance.ParameterValue1.Length > 255 Then
            blnIsValid = False
            udcMessageBox.AddMessage(New SystemMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004))

        End If

        ' If value is valid
        If blnIsValid AndAlso Not IsNothing(udtParameterMaintenance.ApplyLimit) AndAlso Not udtParameterMaintenance.ApplyLimit.Equals(String.Empty) Then
            sm = udtParameteMaintenanceBLL.chkParameterValueIsvaild(udtParameterMaintenance)

            If sm Is Nothing Then
                If udtParameterMaintenance.ApplyLimit = Parm_ApplyLimit.BOUserID Then
                    ' Check User ID exist
                    Dim aryBOUserID As String() = udtParameterMaintenance.ParameterValue1.Split(",")
                    Dim udtHCVUUSerBLL As New HCVUUserBLL
                    Dim strUserIDNotExist As String = String.Empty

                    For Each UserID As String In aryBOUserID
                        If udtHCVUUSerBLL.GetHCVUUserForLogin(UserID).Rows.Count = 0 Then
                            If strUserIDNotExist.Equals(String.Empty) Then
                                strUserIDNotExist = UserID
                            Else
                                strUserIDNotExist += "," + UserID
                            End If
                        End If
                    Next

                    If Not strUserIDNotExist.Equals(String.Empty) Then
                        blnIsValid = False
                        sm = New SystemMessage(FunctCode.FUNT010901, SeverityCode.SEVE, MsgCode.MSG00007)
                        udcMessageBox.AddMessage(sm, "%s", strUserIDNotExist)
                    End If

                End If
            Else
                blnIsValid = False
                udcMessageBox.AddMessage(sm)
            End If
        End If

        Return blnIsValid

    End Function
    ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

#Region "Implement IWorkingData (CRE11-004)"

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

#End Region

End Class
