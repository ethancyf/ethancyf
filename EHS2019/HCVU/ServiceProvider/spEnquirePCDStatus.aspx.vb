
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports Common.PCD
Imports Common.PCD.Component.PCDStatus
Imports Common.PCD.WebService.Interface
Imports Common.Validation
Imports HCVU.BLL


Partial Public Class spEnquirePCDStatus
    Inherits BasePageWithGridView

#Region "Fields"

    Private udtSPProfileBLL As New SPProfileBLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator

    Private udtAuditLogEntry As AuditLogEntry

    Private Const ViewIndexSearchCriteria As Integer = 0

#End Region

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const PageLoad As String = "Service Provider Enquire PCD Status loaded"
        Public Const PageLoad_ID As String = LogID.LOG00000

        Public Const SearchEnquireButtonClick As String = "Search - Enquire button click"
        Public Const SearchEnquireButtonClick_ID As String = LogID.LOG00001

        Public Const SearchSPIDButtonClick As String = "Search - SPID Radio button click"
        Public Const SearchSPIDButtonClick_ID As String = LogID.LOG00002

        Public Const SearchSPHKIDButtonClick As String = "Search - SP HKID Radio button click"
        Public Const SearchSPHKIDButtonClick_ID As String = LogID.LOG00003

        Public Const SearchInvokePCDStart As String = "Search - Invoke PCD Start"
        Public Const SearchInvokePCDStart_ID As String = LogID.LOG00004

        Public Const SearchSuccess As String = "Search - Invoke PCD End [Success]"
        Public Const SearchSuccess_ID As String = LogID.LOG00005

        Public Const SearchFail As String = "Search - Invoke PCD End [Fail]"
        Public Const SearchFail_ID As String = LogID.LOG00006

        Public Const SearchValidationFail As String = "Search - Validation Fail"
        Public Const SearchValidationFail_ID As String = LogID.LOG00007

    End Class
#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010206

        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        pnlEnquiry.DefaultButton = ibtnEnquire.ID

        If Not Page.IsPostBack Then

            If rdoSPID.Checked Then
                Me.ScriptManager1.SetFocus(Me.txtSPID)
            End If

            ' Write Audit Log
            udtAuditLogEntry.WriteLog(AuditLogDesc.PageLoad_ID, AuditLogDesc.PageLoad)
        End If

    End Sub

#End Region

#Region "Events"
    Private Sub ibtnEnquire_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnEnquire.Click
        Dim strHKID As String = String.Empty
        Dim strSPID As String = String.Empty

        udtAuditLogEntry.AddDescripton("SPID", Me.txtSPID.Text)
        udtAuditLogEntry.AddDescripton("HKID", Me.txtSPHKID.Text)
        udtAuditLogEntry.WriteLog(AuditLogDesc.SearchEnquireButtonClick_ID, AuditLogDesc.SearchEnquireButtonClick)

        CompleteMsgBox.Clear()
        Me.msgBox.Clear()

        If Not ValidateInput() Then Exit Sub

        ' CRE17-016 (Checking of PCD status during VSS enrolment) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        If Me.rdoSPID.Checked Then
            ' -------------------------
            ' Resolve SPID to HKID
            ' -------------------------
            strSPID = txtSPID.Text.Trim

            'Get HKID by Service Provider ID
            udtSPProfileBLL.GetServiceProviderHKID(String.Empty, txtSPID.Text.Trim, strHKID, String.Empty)
            If strHKID = String.Empty Then
                udtAuditLogEntry.AddDescripton("SPID", Me.txtSPID.Text)
                udtAuditLogEntry.AddDescripton("Validation", "Invalid")
                udtAuditLogEntry.WriteLog(AuditLogDesc.SearchValidationFail_ID, AuditLogDesc.SearchValidationFail)
                Me.msgBox.AddMessage(New Common.ComObject.SystemMessage("990000", "E", "00271"))
            End If
        Else
            ' --------------------------------
            ' Resolve HKID to SPID (If exists)
            ' --------------------------------
            strHKID = Replace(Replace(txtSPHKID.Text.Trim, "(", ""), ")", "")

            'Get Service Provider ID by HKID
            Dim dtRes As DataTable = udtSPProfileBLL.GetServiceProviderParticularsByERN(strHKID)

            If Not dtRes Is Nothing Then
                Dim drRes() As DataRow = dtRes.Select("Record_Status <> 'D'")

                If drRes.Length = 1 Then
                    strSPID = drRes(0).Item("SP_ID").ToString.Trim
                End If
            End If

        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            ' Enquire PCD
            Dim objWS As New Common.PCD.PCDWebService(Me.FunctionCode)
            Dim objResult As WebService.Interface.PCDCheckAccountStatusResult = Nothing

            udtAuditLogEntry.WriteStartLog(AuditLogDesc.SearchInvokePCDStart_ID, AuditLogDesc.SearchInvokePCDStart)

            objResult = objWS.PCDCheckAccountStatus(strHKID)

            ' Handle Enquire Result
            udtAuditLogEntry.AddDescripton("WebMethod", "PCDCheckAccountStatus")
            udtAuditLogEntry.AddDescripton("ReturnCode", objResult.ReturnCode)
            udtAuditLogEntry.AddDescripton("MessageID", objResult.MessageID)
            udtAuditLogEntry.AddDescripton("AccountStatus", objResult.AccountStatus)
            udtAuditLogEntry.AddDescripton("EnrolmentStatus", objResult.EnrolmentStatus)

            Select Case objResult.ReturnCode

                Case WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.Success
                    udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchSuccess_ID, AuditLogDesc.SearchSuccess)

                    If strSPID <> String.Empty Then
                        Dim udtHCVUUser As HCVUUserModel
                        Dim udtHCVUUserBLL As New HCVUUserBLL
                        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                        Dim strMessage As String = String.Empty

                        If Not objResult.UpdateJoinPCDStatus(strSPID, udtHCVUUser.UserID, strMessage) Then
                            Throw New Exception(strMessage)
                        End If
                    End If

                    lblRSPID.Text = IIf(strSPID = String.Empty, "-", strSPID)
                    lblRHKID.Text = (New Formatter).formatHKID(strHKID, True)
                    lblRPCDStatus.Text = objResult.GetPCDStatusDesc

                    lblRPCDProfessional.Text = objResult.GetPCDProfessionalDesc

                Case Else
                    udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchFail_ID, AuditLogDesc.SearchFail)
                    ' PCD service is temporary not available. Please try again later!
                    Me.msgBox.AddMessage(objResult.SystemMessage)
                    Me.msgBox.BuildMessageBox("ValidationFail")

                    lblRSPID.Text = "-"
                    lblRHKID.Text = "-"
                    lblRPCDStatus.Text = "-"
                    lblRPCDProfessional.Text = HttpContext.GetGlobalResourceObject("Text", "PCDProfessionalEmpty", New System.Globalization.CultureInfo(CultureLanguage.English))
            End Select
            ' CRE17-016 (Checking of PCD status during VSS enrolment) [End][Chris YIM]

        Else
            Me.msgBox.BuildMessageBox("ValidationFail")
        End If
    End Sub

    Private Sub rdoSPID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoSPID.CheckedChanged
        udtAuditLogEntry.WriteLog(AuditLogDesc.SearchSPIDButtonClick_ID, AuditLogDesc.SearchSPIDButtonClick)

        If rdoSPID.Checked Then
            txtSPID.Enabled = True
            Me.ScriptManager1.SetFocus(Me.txtSPID)
            rdoSPHKID.Checked = False
            txtSPHKID.Enabled = False
            txtSPHKID.Text = String.Empty
        End If
    End Sub

    Private Sub rdoSPHKID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoSPHKID.CheckedChanged
        udtAuditLogEntry.WriteLog(AuditLogDesc.SearchSPHKIDButtonClick_ID, AuditLogDesc.SearchSPHKIDButtonClick)

        If rdoSPHKID.Checked Then
            txtSPHKID.Enabled = True
            Me.ScriptManager1.SetFocus(Me.txtSPHKID)
            rdoSPID.Checked = False
            txtSPID.Enabled = False
            txtSPID.Text = String.Empty
        End If
    End Sub

#End Region

#Region "Supported Functions"
    ''' <summary>
    ''' Validate user input
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateInput() As Boolean
        Dim objMsg As SystemMessage = Nothing
        Dim blnResult As Boolean = True
        If Me.rdoSPID.Checked Then
            ' Validate SPID
            If Me.txtSPID.Text.Trim = String.Empty Then
                ' Empty SPID
                udtAuditLogEntry.AddDescripton("SPID", Me.txtSPID.Text)
                udtAuditLogEntry.AddDescripton("Validation", "Empty")
                udtAuditLogEntry.WriteLog(AuditLogDesc.SearchValidationFail_ID, AuditLogDesc.SearchValidationFail)
                msgBox.AddMessage(New Common.ComObject.SystemMessage("990000", "E", "00316"))
                blnResult = False
            End If
        Else
            ' Validate SP HKID
            If Me.txtSPHKID.Text.Trim = String.Empty Then
                ' Empty SP HKID
                udtAuditLogEntry.AddDescripton("SP HKID", Me.txtSPHKID.Text)
                udtAuditLogEntry.AddDescripton("Validation", "Empty")
                udtAuditLogEntry.WriteLog(AuditLogDesc.SearchValidationFail_ID, AuditLogDesc.SearchValidationFail)
                msgBox.AddMessage(New Common.ComObject.SystemMessage("990000", "E", "00317"))
                blnResult = False
            Else
                ' Invalid format SP HKID
                objMsg = udtValidator.chkHKID(Me.txtSPHKID.Text.Trim)
                If objMsg IsNot Nothing Then
                    udtAuditLogEntry.AddDescripton("SP HKID", Me.txtSPHKID.Text)
                    udtAuditLogEntry.AddDescripton("Validation", "Invalid")
                    udtAuditLogEntry.WriteLog(AuditLogDesc.SearchValidationFail_ID, AuditLogDesc.SearchValidationFail)
                    msgBox.AddMessage(New Common.ComObject.SystemMessage("990000", "E", "00319"))
                    blnResult = False
                End If
            End If
        End If

        If blnResult = False Then
            Me.msgBox.BuildMessageBox("ValidationFail")
        End If
        Return blnResult
    End Function

#End Region

#Region "Implement IWorkingData"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

End Class