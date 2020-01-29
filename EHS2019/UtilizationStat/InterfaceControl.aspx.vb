Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text.RegularExpressions
Imports System.Web.Security.AntiXss
Imports System.Xml

Imports Common
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.DataAccess
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports Common.Encryption
Imports Common.Format
Imports Common.WebService.Interface
Imports Common.ComFunction.GeneralFunction
Imports Common.ComFunction.AccountSecurity
Imports Common.Component.EHSTransaction
Imports Common.OCSSS

Imports com.rsa.admin
Imports com.rsa.admin.data
Imports com.rsa.authmgr.admin.agentmgt
Imports com.rsa.authmgr.admin.agentmgt.data
Imports com.rsa.authmgr.admin.hostmgt.data
Imports com.rsa.authmgr.admin.principalmgt
Imports com.rsa.authmgr.admin.principalmgt.data
Imports com.rsa.authmgr.admin.tokenmgt
Imports com.rsa.authmgr.admin.tokenmgt.data
Imports com.rsa.authmgr.common
Imports com.rsa.authn
Imports com.rsa.authn.data
Imports com.rsa.command
Imports com.rsa.command.exception
Imports com.rsa.common
Imports com.rsa.common.search

Imports Microsoft.Web.Services3.Design
Imports System.Data.SqlClient



Partial Public Class InterfaceControl
    Inherits System.Web.UI.Page

#Region "Private Class"

    Private Class ViewIndexEOM
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexEOLCurrentStandy
        Public Const NotInUse As Integer = 0
        Public Const ONE As Integer = 1
        Public Const MULTIPLE As Integer = 2
    End Class

    Private Class ViewIndexEOL
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexEOV
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexPPIePRSite
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexTokenServer
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexEHRSite
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexRCT
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexDOM
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexDOLCurrentStandy
        Public Const NotInUse As Integer = 0
        Public Const ONE As Integer = 1
        Public Const MULTIPLE As Integer = 2
    End Class

    Private Class ViewIndexDOL
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexDOV
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexOCL
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class ViewIndexOCT
        Public Const Button As Integer = 0
        Public Const Confirm As Integer = 1
    End Class

    Private Class SESS
        Public Const StaffID As String = "ICW_StaffID"
        Public Const EOVAction As String = "ICW_EOVAction"
        Public Const DOVAction As String = "ICW_DOVAction"
        Public Const RCDC1Action As String = "ICW_RCDC1Action"
        Public Const RCDC2Action As String = "ICW_RCDC2Action"
        Public Const RCTAction As String = "ICW_RCTAction"
        Public Const OCTAction As String = "ICW_OCTAction"
    End Class

    Private Class AuditLogDescription
        Public Const SwitchCMSMode_ID As String = "00003"
        Public Const SwitchCMSMode As String = "Switch CMS Mode"

        Public Const SwitchCMSLink_ID As String = "00001"
        Public Const SwitchCMSLink As String = "Switch CMS Link"

        Public Const TurnOnSuspendEVaccinationRecord_ID As String = "00002"
        Public Const TurnOnSuspendEVaccinationRecord As String = "Turn On / Suspend CMS eVaccination Record Sharing"

        Public Const SwitchCIMSMode_ID As String = "00004"
        Public Const SwitchCIMSMode As String = "Switch CIMS Mode"

        Public Const SwitchCIMSLink_ID As String = "00005"
        Public Const SwitchCIMSLink As String = "Switch CIMS Link"

        Public Const TurnOnSuspendCIMSEVaccinationRecord_ID As String = "00006"
        Public Const TurnOnSuspendCIMSEVaccinationRecord As String = "Turn On / Suspend CIMS eVaccination Record Sharing"

        Public Const SwitchPPIePRSite_ID As String = "00001"
        Public Const SwitchPPIePRSite As String = "Switch PPI-ePR Site"

        Public Const SwitchTokenServer_ID As String = "00001"
        Public Const SwitchTokenServer As String = "Switch Token Server"

        Public Const SwitchWebService_ID As String = "00001"
        Public Const SwitchWebService As String = "Switch Web Service Link"

        Public Const SwitchVerifySystem_ID As String = "00002"
        Public Const SwitchVerifySystem As String = "Switch Verify System Link"

        Public Const ResumeSuspendEHRServer_ID As String = "00003"
        Public Const ResumeSuspendEHRServer As String = "Resume / Suspend eHR Server"

        Public Const TurnOnSuspendEHRFunctionHCSP_ID As String = "00004"
        Public Const TurnOnSuspendEHRFunctionHCSP As String = "Turn On / Suspend Token Sharing Function in HCSP Platform"

        Public Const SwitchOCSSSLink_ID As String = "00001"
        Public Const SwitchOCSSSLink As String = "Switch OCSSS Link"

        Public Const TurnOnSuspendOCSSSFunctionHCSP_ID As String = "00002"
        Public Const TurnOnSuspendOCSSSFunctionHCSP As String = "Turn On / Suspend OCSSS Function in HCSP Platform"
    End Class

    Private Class EHRRCAction
        Public Const WebService As String = "WS"
        Public Const VerifySystem As String = "VS"
        Public Const ResumeServer As String = "Y"
        Public Const SuspendServer As String = "N"
    End Class

#End Region

#Region "Private Variables"

    Private Const FUNCT_CODE_EVACC_CHECK As String = Common.Component.FunctCode.FUNT090101
    Private Const FUNCT_CODE_EVACC_CONTROL As String = Common.Component.FunctCode.FUNT090102
    Private Const FUNCT_CODE_SCHEDULEJOB_SUSPEND As String = Common.Component.FunctCode.FUNT090103
    Private Const FUNCT_CODE_PPIEPR_CONTROL As String = Common.Component.FunctCode.FUNT090104
    Private Const FUNCT_CODE_TOKENSERVER_CONTROL As String = Common.Component.FunctCode.FUNT090105
    Private Const FUNCT_CODE_DOWNLOAD_REPORT As String = Common.Component.FunctCode.FUNT090106
    Private Const FUNCT_CODE_EHR_CONTROLSITE As String = Common.Component.FunctCode.FUNT090107
    Private Const FUNCT_CODE_EHR_ENQUIREEHR As String = Common.Component.FunctCode.FUNT090108
    Private Const FUNCT_CODE_EHR_TRACEOUTSYNCRECORD As String = Common.Component.FunctCode.FUNT090109
    Private Const FUNCT_CODE_CONNSTR_TESTER As String = Common.Component.FunctCode.FUNT090110
    Private Const FUNCT_CODE_OCSSS_CONTROLSITE As String = Common.Component.FunctCode.FUNT090111
    Private Const FUNCT_CODE_OCSSS_ENQUIREOCSSS As String = Common.Component.FunctCode.FUNT090112

    Private Const CONST_SYS_PARAM_TurnOnOCSSS As String = "TurnOnOCSSS"
    Private Const CONST_SYS_PARAM_OCSSS_WS_Link As String = "OCSSS_WS_Link"
    Private Const CONST_SYS_PARAM_OCSSS_WS_PassPhrase As String = "OCSSS_WS_PassPhrase"

    Private Const CONST_SYS_PARAM_TurnOnCMS As String = "TurnOnVaccinationRecord_CMS"
    Private Const CONST_SYS_PARAM_TurnOnCIMS As String = "TurnOnVaccinationRecord_CIMS"
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' CRE11-006
            ' Handle redirect back from ScheduleJobSuspend.aspx
            ' ---------------------------------------------------
            If Session(SESS.StaffID) Is Nothing Then
                MultiViewCore.SetActiveView(ViewLogin)
            Else
                MultiViewCore.SetActiveView(ViewMenu)
            End If
            ' ---------------------------------------------------
        Else

            If MultiViewCore.GetActiveView.ID = ViewEVaccCheck.ID Then
                Call GenerateSummaryCMSURL()
            End If

            If MultiViewCore.GetActiveView.ID = ViewDHCIMSEVaccCheck.ID Then
                Call GenerateSummaryCIMSURL()
            End If

        End If

    End Sub

    Protected Sub MultiViewCore_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case MultiViewCore.GetActiveView.ID
            Case ViewLogin.ID
                SetupLogin()

            Case ViewLoginChangePassword.ID
                SetupChangePassword()

            Case ViewMenu.ID
                SetupMenu()

            Case ViewEVaccCheck.ID
                SetupEVaccinationCheck()

            Case ViewDHCIMSEVaccCheck.ID
                SetupCIMSEVaccinationCheck()

            Case ViewEVaccControl.ID
                SetupEVaccinationControl()

            Case ViewDHCIMSEvaccControl.ID
                SetupCIMSEVaccinationControl()

            Case ViewPPIePRControl.ID
                SetupPPIePRControl()

            Case ViewTokenServerControl.ID
                SetupTokenServerControl()

            Case ViewEHRControlSite.ID
                SetupEHRControlSite()

            Case ViewEHREnquireEHR.ID
                SetupEHREnquireEHR()

            Case ViewEHRTraceOutsyncRecord.ID
                SetupEHRTraceOutsyncRecord()

            Case ViewOCSSSControlSite.ID
                SetupOCSSSControlSite()

            Case ViewOCSSSEnquireOCSSS.ID
                SetupOCSSSEnquireOCSSS()
        End Select

    End Sub

#Region "View 0: Login"

    Private Sub SetupLogin()
        txtStaffIDL.Text = String.Empty

    End Sub

    '

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblErrorL.Visible = False

        Dim strStaffID As String = txtStaffIDL.Text.Trim
        Dim strPassword As String = txtPasswordL.Text.Trim

        ' Validation #1: Staff ID and Password both cannot be empty
        If strStaffID = String.Empty OrElse strPassword = String.Empty Then
            lblErrorL.Text = "Staff ID and Password cannot be empty"
            lblErrorL.Visible = True
            Return
        End If

        ' Validation #2: Staff ID not exist
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim dtICWStaffAC As DataTable = udtInterfaceControlBLL.GetICWStaffAccount(strStaffID)

        If dtICWStaffAC.Rows.Count = 0 Then
            lblErrorL.Text = "Staff ID or Password is incorrect"
            lblErrorL.Visible = True
            Return
        End If

        '  Validation #3: Staff is suspended
        Dim dr As DataRow = dtICWStaffAC.Rows(0)

        If dr("Record_Status").ToString.Trim = "S" Then
            lblErrorL.Text = String.Format("Staff ID {0} is suspended", strStaffID)
            lblErrorL.Visible = True
            Return
        End If

        '  Validation #4: If Staff_Password is NULL need to set new password
        'If IsDBNull(dr("Staff_Password")) OrElse dr("Record_Status").ToString.Trim = "P" Then
        If IsDBNull(dr("Staff_Password")) Then
            ' For first login, Staff ID is the same as password
            If strPassword <> strStaffID Then
                lblErrorL.Text = "Staff ID or Password is incorrect"
                lblErrorL.Visible = True
                Return
            End If

            Session(SESS.StaffID) = strStaffID
            MultiViewCore.SetActiveView(ViewLoginChangePassword)
            Return
        End If

        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
        '  Validation #5: Check password
        Dim IResult As EnumVerifyPasswordResult = VerifyPassword(EnumPlatformType.ICW, dtICWStaffAC, strPassword).VerifyResult
        If IResult = EnumVerifyPasswordResult.Incorrect Then
            lblErrorL.Text = "Staff ID or Password is incorrect"
            lblErrorL.Visible = True
            Return
        End If

        If IResult = EnumVerifyPasswordResult.RequireUpdate Then
            lblErrorL.Text = "Your password is expired!"
            lblErrorL.Visible = True
            Return
        End If

        ' If Record_Status is P and the pw is correct, need to set new pw
        If dr("Record_Status").ToString.Trim = "P" Then
            Session(SESS.StaffID) = strStaffID
            MultiViewCore.SetActiveView(ViewLoginChangePassword)
            Return
        End If
        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

        ' Update Last_Login_Dtm in ICWStaffAccount
        udtInterfaceControlBLL.UpdateICWStaffAccount(strStaffID)

        Session(SESS.StaffID) = strStaffID
        MultiViewCore.SetActiveView(ViewMenu)

    End Sub

#End Region

#Region "View 1: Change Password"

    Private Sub SetupChangePassword()
        lblStaffIDCP.Text = Session(SESS.StaffID)
    End Sub

    '

    Protected Sub btnSetPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        lblErrorCP.Visible = False

        Dim strPasswordA As String = txtPasswordACP.Text
        Dim strPasswordB As String = txtPasswordBCP.Text

        ' Validation #1: Staff ID and Password both cannot be empty
        If strPasswordA = String.Empty OrElse strPasswordB = String.Empty Then
            lblErrorCP.Text = "Passwords cannot be empty"
            lblErrorCP.Visible = True
            Return
        End If

        ' Validation #2: Password A = Password B
        If strPasswordA <> strPasswordB Then
            lblErrorCP.Text = "Passwords do not match"
            lblErrorCP.Visible = True
            Return
        End If


        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
        udtInterfaceControlBLL.UpdateICWStaffAccount(strStaffID, Hash(strPasswordA))
        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

        MultiViewCore.SetActiveView(ViewMenu)

    End Sub

#End Region

#Region "View 2: Menu"

    Private Sub SetupMenu()
        ' Check access right
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strStaffRole As String = udtInterfaceControlBLL.GetICWStaffAccount(strStaffID).Rows(0)("Staff_Role").ToString.Trim

        Dim dt As DataTable = udtInterfaceControlBLL.GetICWStaffAccessRight(strStaffRole)

        lbtnCMSEVaccCheck.Enabled = False
        lbtnDHCIMSEvaccCheck.Enabled = False
        lbtnCMSEVaccControl.Enabled = False
        lbtnDHCIMSEVaccControl.Enabled = False
        lbtnPPIePRControl.Enabled = False
        lbtnTokenServerControl.Enabled = False
        lbtnEHRControlSite.Enabled = False
        lbtnEHREnquireEHR.Enabled = False
        lbtnOCSSSControlSite.Enabled = False
        lbtnOCSSSEnquireOCSSS.Enabled = False
        lbtnEHRTraceOutSyncRecord.Enabled = False
        lbtnScheduleJobSuspend.Enabled = False
        lbtnDownloadReport.Enabled = False
        lbtnConnectionStringTester.Enabled = False

        For Each dr As DataRow In dt.Rows
            Select Case dr("Function_Code").ToString.Trim
                Case FUNCT_CODE_EVACC_CHECK
                    lbtnCMSEVaccCheck.Enabled = True
                    lbtnDHCIMSEvaccCheck.Enabled = True
                Case FUNCT_CODE_EVACC_CONTROL
                    lbtnCMSEVaccControl.Enabled = True
                    lbtnDHCIMSEVaccControl.Enabled = True
                Case FUNCT_CODE_SCHEDULEJOB_SUSPEND
                    lbtnScheduleJobSuspend.Enabled = True
                Case FUNCT_CODE_PPIEPR_CONTROL
                    lbtnPPIePRControl.Enabled = True
                Case FUNCT_CODE_TOKENSERVER_CONTROL
                    lbtnTokenServerControl.Enabled = True
                Case FUNCT_CODE_EHR_CONTROLSITE
                    lbtnEHRControlSite.Enabled = True
                Case FUNCT_CODE_EHR_ENQUIREEHR
                    lbtnEHREnquireEHR.Enabled = True
                Case FUNCT_CODE_EHR_TRACEOUTSYNCRECORD
                    lbtnEHRTraceOutSyncRecord.Enabled = True
                Case FUNCT_CODE_OCSSS_CONTROLSITE
                    lbtnOCSSSControlSite.Enabled = True
                Case FUNCT_CODE_OCSSS_ENQUIREOCSSS
                    lbtnOCSSSEnquireOCSSS.Enabled = True
                Case FUNCT_CODE_DOWNLOAD_REPORT
                    lbtnDownloadReport.Enabled = True
                Case FUNCT_CODE_CONNSTR_TESTER
                    lbtnConnectionStringTester.Enabled = True
            End Select
        Next

    End Sub

    Protected Sub lbtnCMSEVaccCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewEVaccCheck)
    End Sub

    Protected Sub lbtnDHCIMSEVaccCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewDHCIMSEVaccCheck)
    End Sub

    Protected Sub lbtnCMSEVaccControl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewEVaccControl)
    End Sub

    Protected Sub lbtnDHCIMSEVaccControl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewDHCIMSEvaccControl)
    End Sub

    Protected Sub lbtnPPIePRControl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewPPIePRControl)
    End Sub

    Protected Sub lbtnTokenServerControl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewTokenServerControl)
    End Sub

    Protected Sub lbtnEHRControlSite_Click(sender As Object, e As EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewEHRControlSite)
    End Sub

    Protected Sub lbtnEHREnquireEHR_Click(sender As Object, e As EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewEHREnquireEHR)
    End Sub

    Protected Sub lbtnEHRTraceOutSyncRecord_Click(sender As Object, e As EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewEHRTraceOutsyncRecord)
    End Sub

    Protected Sub lbtnOCSSSControlSite_Click(sender As Object, e As EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewOCSSSControlSite)
    End Sub

    Protected Sub lbtnOCSSSEnquireOCSSS_Click(sender As Object, e As EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(ViewOCSSSEnquireOCSSS)
    End Sub

    Protected Sub lbtnScheduleJobSuspend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/ScheduleJob.aspx")
    End Sub

    Protected Sub lbtnDownloadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(vDownloadReport)

        txtDRGenerationID.Text = String.Empty
        txtDRExcelPassword.Text = String.Empty
        lblDRError.Text = String.Empty

    End Sub

    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    Protected Sub lbtnConnectionStringTester_Click(sender As Object, e As EventArgs)
        Dim blnInit = True
        GetStaffIDFromSession()
        MultiViewCore.SetActiveView(vConnectStringTester)
        gvConnectionString.DataSource = GetAllConnStr(blnInit)
        gvConnectionString.DataBind()
    End Sub

    Protected Sub btnCallAllConnstr_Click(sender As Object, e As EventArgs) Handles btnCallAllConnstr.Click
        Dim blnInit = False
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strDescription As String
        strDescription = String.Format("Call All Connection String")
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_CONNSTR_TESTER, "00001", strDescription)
        gvConnectionString.DataSource = GetAllConnStr(blnInit)
        gvConnectionString.DataBind()
        For i As Integer = 0 To gvConnectionString.Rows.Count - 1
            Dim ConnStr As Label
            Dim ConnStrResponse As Label
            ConnStr = gvConnectionString.Rows(i).FindControl("ConnStr")
            ConnStrResponse = gvConnectionString.Rows(i).FindControl("ConnStrResponse")
            If Not ConnStrResponse.Text.Contains("Success") Then
                ConnStrResponse.ForeColor = Drawing.Color.Red
                ConnStrResponse.Width = 300
                strDescription = String.Format("Unsuccess Call Connection String {0}: <{1}>", ConnStr.Text, ConnStrResponse.Text)
            Else
                strDescription = String.Format("Success Call Connection String {0}: <{1}>", ConnStr.Text, ConnStrResponse.Text)
            End If

            udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_CONNSTR_TESTER, "00001", strDescription)
        Next





    End Sub

    Protected Sub btnClearAllConnstr_Click(sender As Object, e As EventArgs) Handles btnClearAllConnstr.Click
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strDescription As String
        Dim blnInit = True
        gvConnectionString.DataSource = GetAllConnStr(blnInit)
        gvConnectionString.DataBind()
        strDescription = String.Format("Clear All Connection String Response")
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_CONNSTR_TESTER, "00002", strDescription)
    End Sub

    Private Sub gvdataEntryAcc_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvConnectionString.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Dim udtInterfaceControlBLL As New InterfaceControlBLL
            Dim strDescription As String
            Dim strCommandArgument As String
            Dim indexOf As Integer
            Dim lastIndexOf As Integer
            Dim row As Integer
            Dim strConn As String
            Dim ConnStr As Label
            Dim ConnStrResponse As Label



            strCommandArgument = e.CommandArgument.ToString.Trim
            indexOf = strCommandArgument.IndexOf("&")
            row = strCommandArgument.Substring(0, indexOf)

            If strCommandArgument.Contains("call") Then
                lastIndexOf = strCommandArgument.LastIndexOf("&")
                strConn = strCommandArgument.Remove(0, lastIndexOf + 1)
                ConnStr = gvConnectionString.Rows(row).FindControl("ConnStr")
                ConnStrResponse = gvConnectionString.Rows(row).FindControl("ConnStrResponse")

                ConnStrResponse.Text = GetConnectionResopne(strConn)

                strDescription = String.Format("Call Connection String <{0}>", ConnStr.Text)
                udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_CONNSTR_TESTER, "00003", strDescription)

                If Not ConnStrResponse.Text.Contains("Success") Then
                    ConnStrResponse.ForeColor = Drawing.Color.Red
                    strDescription = String.Format("Unsuccess Call Connection String <{0}>: <{1}>", ConnStr.Text, ConnStrResponse.Text)
                Else
                    strDescription = String.Format("Success Call Connection String <{0}>:<{1}>", ConnStr.Text, ConnStrResponse.Text)
                End If
                udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_CONNSTR_TESTER, "00003", strDescription)
            ElseIf strCommandArgument.Contains("clear") Then
                ConnStr = gvConnectionString.Rows(row).FindControl("ConnStr")
                ConnStrResponse = gvConnectionString.Rows(row).FindControl("ConnStrResponse")
                ConnStrResponse.Text = ""
                strDescription = String.Format("Clear Connection String Response <{0}>", ConnStr.Text)
                udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_CONNSTR_TESTER, "00004", strDescription)
            End If
        End If

    End Sub

    Protected Sub btnConnStrTestBack_Click(sender As Object, e As EventArgs) Handles btnConnStrTestBack.Click
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strDescription As String
        MultiViewCore.SetActiveView(ViewMenu)
        strDescription = String.Format("Back to Menu")
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_CONNSTR_TESTER, "00005", strDescription)
    End Sub

    Private Function GetAllConnStr(ByRef blnInit As Boolean) As DataTable
        Dim i As Integer = 0
        Dim table As New DataTable
        table.Columns.Add("ConnStr", GetType(String))
        table.Columns.Add("Call", GetType(String))
        table.Columns.Add("Clear", GetType(String))
        table.Columns.Add("ResponseTime", GetType(String))
        Try
            Dim appSettings = ConfigurationManager.AppSettings
            If appSettings.Count = 0 Then
                table.Rows.Add("AppSettings is empty.")
            Else
                For Each key As String In appSettings.AllKeys
                    If key.Contains("ConnectionString") Then
                        Dim strConn As String = key
                        Dim ResponseTime As String = String.Empty
                        If blnInit = False Then
                            ResponseTime = GetConnectionResopne(strConn)
                        End If
                        table.Rows.Add(key, i & "&call&" & strConn, i & "&clear&" & strConn, ResponseTime)
                        i = i + 1
                    End If
                Next
            End If
        Catch e As ConfigurationErrorsException
            table.Rows.Add("Error reading app settings")
        End Try
        Return table
    End Function

    Private Function GetConnectionResopne(ByVal strConn As String) As String
        Dim dtConnStrResponseTime As DataTable = Nothing
        Dim ResponseTime As String
        Try
            dtConnStrResponseTime = TestConnectionString(strConn)
            Dim dtmDateTime As Date = CDate(dtConnStrResponseTime.Rows(0).Item("DateTime").ToString)
            ResponseTime = "Success at " & String.Format("{0} {1}", dtmDateTime.ToString("yyyy-MM-dd"), dtmDateTime.ToString("HH:mm:ss"))
        Catch ex As Exception
            ResponseTime = ex.ToString
        End Try
        Return ResponseTime
    End Function

    Private Function TestConnectionString(ByVal strConn As String) As DataTable
        Dim strConnValue As String = ConfigurationManager.AppSettings(strConn)
        Dim udtDB As New Database(strConn, strConnValue)
        Dim dt As New DataTable
        Try
            udtDB.BeginTransaction()
            udtDB.RunProc("proc_SystemDateTime_get", dt)
            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        Finally
            If Not udtDB Is Nothing Then udtDB.Dispose()
        End Try
        Return dt

    End Function
    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]


    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Session.Clear()
        MultiViewCore.SetActiveView(ViewLogin)
    End Sub

#End Region

#Region "View 3: [e]Vaccination [C]heck"

    Private Sub SetupEVaccinationCheck()
        lblErrorECS.Visible = False
        lblErrorECC.Visible = False
        lblErrorECT.Visible = False
        lblErrorECA.Visible = False
        lblErrorECE.Visible = False

        lblECSLastUpdate.Text = String.Empty
        lblECCLastUpdate.Text = String.Empty
        lblECTLastUpdate.Text = String.Empty
        lblECALastUpdate.Text = String.Empty
        lblECELastUpdate.Text = String.Empty
        'CRE13-015 Add URL for new EAI server [Start][Karl]
        '' CRE11-002
        'lblECSSecondarySite2.Visible = False
        'lblECSSecondarySiteURL2.Visible = False

        '' CRE11-002
        'lblECSPrimarySiteURL.Text = String.Empty
        'lblECSSecondarySiteURL1.Text = String.Empty
        'lblECSSecondarySiteURL2.Text = String.Empty
        'CRE13-015 Add URL for new EAI server [End][Karl]
        lblECSResult1.Text = String.Empty
        lblECSResult2.Text = String.Empty
        lblECSResult3.Text = String.Empty
        lblECSResult4.Text = String.Empty

        lblECSResult1.BackColor = Nothing
        lblECSResult2.BackColor = Nothing
        lblECSResult3.BackColor = Nothing
        lblECSResult4.BackColor = Nothing

        gvECC1.Visible = False
        gvECC2.Visible = False
        gvECT.Visible = False
        gvECA.Visible = False
        gvECE.Visible = False

        lblECCSite1.Visible = False
        lblECCSite2.Visible = False

        ' Summary: From = Empty
        txtECSFrom.Text = String.Empty

        ' Summary: To = Empty
        txtECSTo.Text = String.Empty

        ' CMS Health Check History: Top Row = 10
        txtECCTopRow.Text = 10

        ' CMS Health Check History: From = Today 00:00
        txtECC2From.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' CMS Health Check History: To = Empty
        txtECC2To.Text = String.Empty

        ' Transaction External Reference Status History: From = Today 00:00
        txtECTFrom.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' Transaction External Reference Status History: To = Empty
        txtECTTo.Text = String.Empty

        ' HCSP Audit Log History: From = Today 00:00
        txtECAFrom.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' HCSP Audit Log History: To = Empty
        txtECATo.Text = String.Empty

        ' eHS eVaccination Record Service History: From = Today 00:00
        txtECEFrom.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' eHS eVaccination Record Service History: To = Empty
        txtECETo.Text = String.Empty

        ' CRE11-002
        ' Refresh Primary/Secondary site URL for user reference

        'CRE13-015 Add URL for new EAI server [Start][Karl]             
        Call GenerateSummaryCMSURL()

        'If cllnURL(1) <> String.Empty Then lblECSPrimarySiteURL.Text = cllnURL(1)
        'If cllnURL(2) <> String.Empty Then lblECSSecondarySiteURL1.Text = cllnURL(2)
        'If cllnURL.Count >= 3 AndAlso cllnURL(3) <> String.Empty Then
        '    lblECSSecondarySiteURL2.Text = cllnURL(3)
        '    lblECSSecondarySite2.Visible = True
        '    lblECSSecondarySiteURL2.Visible = True
        'End If

        'CRE13-015 Add URL for new EAI server [End][Karl]

    End Sub
    'CRE13-015 Add URL for new EAI server [Start][Karl]          
    Private Sub GenerateSummaryCMSURL()
        Dim intCount As Integer
        Dim cllnURL As Collection = GetUsingEndpointURLListIgnoreServerCache()

        For intCount = 1 To cllnURL.Count
            Dim lblSite As New Label
            Dim lblSiteURL As New Label

            lblSite.Width = 150
            lblSiteURL.Width = 600

            If intCount = 1 Then
                lblSite.ID = "lblECSPrimarySite"
                lblSiteURL.ID = "lblECSPrimarySiteURL"

                lblSite.Text = "Primary Site:"
                lblSiteURL.Text = cllnURL(intCount)
            Else
                lblSite.ID = "lblECSSecondarySite" & (intCount - 1).ToString
                lblSiteURL.ID = "lblECSSecondarySiteURL" & (intCount - 1).ToString

                lblSite.Text = "Secondary Site " & (intCount - 1).ToString & ":"
                lblSiteURL.Text = cllnURL(intCount)
            End If

            Me.pnlCMSSites.Controls.Add(lblSite)
            Me.pnlCMSSites.Controls.Add(lblSiteURL)
            Me.pnlCMSSites.Controls.Add(New LiteralControl("<br />"))
            Me.pnlCMSSites.Controls.Add(New LiteralControl("<br />"))
        Next
    End Sub
    'CRE13-015 Add URL for new EAI server [End][Karl]          

    ' [S]ummary

    Protected Sub btnECSRefreshAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshSummary()
    End Sub

    ' (1) [C]MS Health Check History

    Protected Sub btnECCRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCMSHealthCheckHistory("R")
    End Sub

    Protected Sub btnECCRefresh2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCMSHealthCheckHistory("T")
    End Sub

    Protected Sub gvECC1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblLogID As Label = e.Row.FindControl("lblLogID")
            If Regex.IsMatch(lblLogID.Text.Trim, ConfigurationManager.AppSettings("CriterionECCRegEx")) Then
                lblLogID.ForeColor = Drawing.Color.Red
            End If

        End If

    End Sub

    Protected Sub gvECC2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblLogID As Label = e.Row.FindControl("lblLogID")
            If Regex.IsMatch(lblLogID.Text.Trim, ConfigurationManager.AppSettings("CriterionECCRegEx")) Then
                lblLogID.ForeColor = Drawing.Color.Red
            End If

        End If

    End Sub

    Protected Sub btnECCClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvECC1.Visible = False
        gvECC2.Visible = False
        lblECCSite1.Visible = False
        lblECCSite2.Visible = False
    End Sub

    Protected Sub lbtnECC2GetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtECC2From.Text = AntiXssEncoder.HtmlEncode(txtECSFrom.Text, True)
        txtECC2To.Text = AntiXssEncoder.HtmlEncode(txtECSTo.Text, True)
    End Sub

    ' (2) [T]ransaction External Reference Status History 

    Protected Sub btnECTRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshTransactionExternalReferenceStatusHistory()
    End Sub

    Protected Sub gvECT_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblExtRefStatus As Label = e.Row.FindControl("lblExtRefStatus")
            If Regex.IsMatch(lblExtRefStatus.Text.Trim, ConfigurationManager.AppSettings("CriterionECTRegEx")) Then
                lblExtRefStatus.ForeColor = Drawing.Color.Red
            End If

            Dim lblTransactionTime As Label = e.Row.FindControl("lblTransactionTime")
            lblTransactionTime.Text = FormatDateTime(lblTransactionTime.Text.Trim)

        End If

    End Sub

    Protected Sub btnECTClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvECT.Visible = False
    End Sub

    Protected Sub lbtnECTGetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtECTFrom.Text = AntiXssEncoder.HtmlEncode(txtECSFrom.Text, True)
        txtECTTo.Text = AntiXssEncoder.HtmlEncode(txtECSTo.Text, True)
    End Sub

    ' (3) HCSP [A]udit Log History

    Protected Sub btnECARefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshHCSPAuditLogHistory()
    End Sub

    Protected Sub gvECA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblLogID As Label = e.Row.FindControl("lblLogID")
            If Regex.IsMatch(lblLogID.Text.Trim, ConfigurationManager.AppSettings("CriterionECARegEx")) Then
                lblLogID.ForeColor = Drawing.Color.Red
            End If

            Dim lblDescription As Label = e.Row.FindControl("lblDescription")
            lblDescription.Text = lblDescription.Text.Substring(0, lblDescription.Text.IndexOf("<") - 2)

        End If

    End Sub

    Protected Sub btnECAClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvECA.Visible = False
    End Sub

    Protected Sub lbtnECAGetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtECAFrom.Text = AntiXssEncoder.HtmlEncode(txtECSFrom.Text, True)
        txtECATo.Text = AntiXssEncoder.HtmlEncode(txtECSTo.Text, True)
    End Sub

    ' (4) eHS [e]Vaccination Record Service History  

    Protected Sub btnECERefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshEHSEVaccinationRecordServiceHistory()
    End Sub

    Protected Sub gvECE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblTimeDiff As Label = e.Row.FindControl("lblTimeDiff")
            If CInt(lblTimeDiff.Text.Trim) >= CInt(ConfigurationManager.AppSettings("CriterionECE")) Then
                lblTimeDiff.ForeColor = Drawing.Color.Red
            End If

        End If

    End Sub

    Protected Sub btnECEClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvECE.Visible = False
    End Sub

    Protected Sub lbtnECEGetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtECEFrom.Text = AntiXssEncoder.HtmlEncode(txtECSFrom.Text, True)
        txtECETo.Text = AntiXssEncoder.HtmlEncode(txtECSTo.Text, True)
    End Sub

    '

    Protected Sub btnECBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

    '

    Private Sub RefreshSummary()
        Dim dtmFrom As Date = Date.MinValue

        If txtECSFrom.Text = String.Empty Then
            dtmFrom = CDate(Date.Now.AddHours(CInt(ConfigurationManager.AppSettings("SummaryHourBefore"))).ToString("yyyy-MM-dd HH:mm:ss"))
            txtECSFrom.Text = dtmFrom.ToString("yyyy-MM-dd HH:mm:ss")
        Else
            Try
                dtmFrom = CDate(txtECSFrom.Text)
            Catch ex As Exception
                lblErrorECS.Text = "From is invalid"
                lblErrorECS.Visible = True
                Return
            End Try
        End If

        Dim dtmTo As Date = Date.MinValue

        If txtECSTo.Text = String.Empty Then
            dtmTo = Date.Now()
            txtECSTo.Text = dtmTo.ToString("yyyy-MM-dd HH:mm:ss")
        Else
            Try
                dtmTo = CDate(txtECSTo.Text)

            Catch ex As Exception
                lblErrorECS.Text = "To is invalid"
                lblErrorECS.Visible = True
                Return
            End Try
        End If

        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        ' (1) CMS Health Check History
        Dim intError As Integer = 0
        Dim strRegExECC As String = ConfigurationManager.AppSettings("CriterionECCRegEx")

        For Each dr As DataRow In udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC", Nothing, Nothing, dtmFrom, dtmTo).Rows
            If Regex.IsMatch(dr("Log_ID").ToString.Trim, strRegExECC) Then
                intError += 1
            End If
        Next

        If intError = 0 Then
            lblECSResult1.Text = "Normal"
            lblECSResult1.BackColor = Drawing.Color.PaleGreen
        Else
            lblECSResult1.Text = String.Format("{0} Error Cases", intError)
            lblECSResult1.BackColor = Drawing.Color.Pink
        End If

        ' (2) Transaction External Reference Status History
        intError = 0
        Dim strExtRefStatus As String = String.Empty
        Dim strRegExECT As String = ConfigurationManager.AppSettings("CriterionECTRegEx")

        For Each dr As DataRow In udtInterfaceControlBLL.GetVoucherTransactionByTransactionDtm(dtmFrom, dtmTo).Rows
            strExtRefStatus = String.Empty

            If Not IsDBNull(dr("Ext_Ref_Status")) Then strExtRefStatus = dr("Ext_Ref_Status").ToString.Trim

            If strExtRefStatus <> String.Empty AndAlso Regex.IsMatch(strExtRefStatus, strRegExECT) Then
                intError += 1
            End If

        Next

        If intError = 0 Then
            lblECSResult2.Text = "Normal"
            lblECSResult2.BackColor = Drawing.Color.PaleGreen
        Else
            lblECSResult2.Text = String.Format("{0} Error Cases", intError)
            lblECSResult2.BackColor = Drawing.Color.Pink
        End If

        ' (3) HCSP Audit Log History
        intError = 0
        Dim strRegExECA As String = ConfigurationManager.AppSettings("CriterionECARegEx")

        For Each dr As DataRow In udtInterfaceControlBLL.GetHCSPAuditLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CMS.ToString).Rows
            If Regex.IsMatch(dr("Log_ID").ToString.Trim, strRegExECA) Then
                intError += 1
            End If

        Next

        If intError = 0 Then
            lblECSResult3.Text = "Normal"
            lblECSResult3.BackColor = Drawing.Color.PaleGreen
        Else
            lblECSResult3.Text = String.Format("{0} Error Cases", intError)
            lblECSResult3.BackColor = Drawing.Color.Pink
        End If

        ' (4) eHS eVaccination Record Service History  
        intError = 0
        Dim intTimeDiffLimit As Integer = CInt(ConfigurationManager.AppSettings("CriterionECE"))

        For Each dr As DataRow In udtInterfaceControlBLL.GetInterfaceLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CMS.ToString).Rows
            If CInt(dr("Time_Diff").ToString.Trim) >= intTimeDiffLimit Then
                intError += 1
            End If

        Next

        If intError = 0 Then
            lblECSResult4.Text = "Normal"
            lblECSResult4.BackColor = Drawing.Color.PaleGreen
        Else
            lblECSResult4.Text = String.Format("{0} Cases >= {1} ms", intError, intTimeDiffLimit)
            lblECSResult4.BackColor = Drawing.Color.Pink
        End If

        ' Update Last update
        lblECSLastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshCMSHealthCheckHistory(ByVal strMode As String)
        Dim dt1 As DataTable = Nothing
        Dim dt2 As DataTable = Nothing

        If strMode = "R" Then
            lblErrorECC.Visible = False

            ' Retrieve input-ed Top Row
            Dim intTopRow As Integer = -1

            Try
                intTopRow = CInt(txtECCTopRow.Text.Trim)
            Catch ex As Exception
                lblErrorECC.Text = "Top Row is invalid"
                lblErrorECC.Visible = True
                Return
            End Try

            If intTopRow <= 0 Then
                lblErrorECC.Text = "Top Row is invalid"
                lblErrorECC.Visible = True
                Return
            End If

            ' Retrieve data
            Dim udtInterfaceControlBLL As New InterfaceControlBLL
            dt1 = udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC", "HEALTH1", intTopRow)
            dt2 = udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC", "HEALTH2", intTopRow)

        ElseIf strMode = "T" Then
            lblErrorECC2.Visible = False

            ' Retrieve input-ed From
            Dim dtmFrom As Date = DateTime.MinValue
            Dim dtmTo As Date = DateTime.MinValue

            Try
                dtmFrom = CDate(txtECC2From.Text.Trim)
            Catch ex As Exception
                lblErrorECC2.Text = "From is invalid"
                lblErrorECC2.Visible = True
                Return
            End Try

            ' Retrieve input-ed To
            If txtECC2To.Text.Trim = String.Empty Then
                dtmTo = DateTime.Now

            Else
                Try
                    dtmTo = CDate(txtECC2To.Text.Trim)
                Catch ex As Exception
                    lblErrorECC2.Text = "To is invalid"
                    lblErrorECC2.Visible = True
                    Return
                End Try

            End If

            ' Retrieve data
            Dim udtInterfaceControlBLL As New InterfaceControlBLL
            dt1 = udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC", "HEALTH1", Nothing, dtmFrom, dtmTo)
            dt2 = udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC", "HEALTH2", Nothing, dtmFrom, dtmTo)

        End If

        gvECC1.DataSource = dt1
        gvECC1.DataBind()
        gvECC1.Visible = True

        gvECC2.DataSource = dt2
        gvECC2.DataBind()
        gvECC2.Visible = True

        lblECCSite1.Visible = True
        lblECCSite2.Visible = True

        ' Update Last update
        lblECCLastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshTransactionExternalReferenceStatusHistory()
        lblErrorECT.Visible = False

        Dim dtmFrom As Date = DateTime.MinValue
        Dim dtmTo As Date = DateTime.MinValue

        ' Retrieve input-ed From
        Try
            dtmFrom = CDate(txtECTFrom.Text.Trim)
        Catch ex As Exception
            lblErrorECT.Text = "From is invalid"
            lblErrorECT.Visible = True
            Return
        End Try

        ' Retrieve input-ed To
        If txtECTTo.Text.Trim = String.Empty Then
            dtmTo = DateTime.Now

        Else
            Try
                dtmTo = CDate(txtECTTo.Text.Trim)
            Catch ex As Exception
                lblErrorECT.Text = "To is invalid"
                lblErrorECT.Visible = True
                Return
            End Try

        End If

        Dim dt As DataTable = (New InterfaceControlBLL).GetVoucherTransactionByTransactionDtm(dtmFrom, dtmTo)

        gvECT.DataSource = dt
        gvECT.DataBind()
        gvECT.Visible = True

        ' Update Last update
        lblECTLastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshHCSPAuditLogHistory()
        lblErrorECA.Visible = False

        Dim dtmFrom As Date = DateTime.MinValue
        Dim dtmTo As Date = DateTime.MinValue

        ' Retrieve input-ed From
        Try
            dtmFrom = CDate(txtECAFrom.Text.Trim)
        Catch ex As Exception
            lblErrorECA.Text = "From is invalid"
            lblErrorECA.Visible = True
            Return
        End Try

        ' Retrieve input-ed To
        If txtECATo.Text.Trim = String.Empty Then
            dtmTo = DateTime.Now

        Else
            Try
                dtmTo = CDate(txtECATo.Text.Trim)
            Catch ex As Exception
                lblErrorECA.Text = "To is invalid"
                lblErrorECA.Visible = True
                Return
            End Try

        End If

        Dim dt As DataTable = (New InterfaceControlBLL).GetHCSPAuditLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CMS.ToString)

        gvECA.DataSource = dt
        gvECA.DataBind()
        gvECA.Visible = True

        ' Update Last update
        lblECALastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshEHSEVaccinationRecordServiceHistory()
        lblErrorECE.Visible = False

        Dim dtmFrom As Date = DateTime.MinValue
        Dim dtmTo As Date = DateTime.MinValue

        ' Retrieve input-ed From
        Try
            dtmFrom = CDate(txtECEFrom.Text.Trim)
        Catch ex As Exception
            lblErrorECE.Text = "From is invalid"
            lblErrorECE.Visible = True
            Return
        End Try

        ' Retrieve input-ed To
        If txtECETo.Text.Trim = String.Empty Then
            dtmTo = DateTime.Now

        Else
            Try
                dtmTo = CDate(txtECETo.Text.Trim)
            Catch ex As Exception
                lblErrorECE.Text = "To is invalid"
                lblErrorECE.Visible = True
                Return
            End Try

        End If

        Dim dt As DataTable = (New InterfaceControlBLL).GetInterfaceLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CMS.ToString)

        gvECE.DataSource = dt
        gvECE.DataBind()
        gvECE.Visible = True

        ' Update Last update
        lblECELastUpdate.Text = GetLastUpdate()

    End Sub

#End Region

#Region "View 4: [e]Vaccination C[o]ntrol"

    Private Sub SetupEVaccinationControl()
        RefreshCMSMode()
        RefreshCMSLink()
        RefreshEVaccinationRecordStatus()
        ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [Start][Tommy L]
        ' --------------------------------------------------------------------------------------------------
        'RefreshClearCacheRequest()
        ucClearCacheRequestView_EVaccControl.RefreshClearCacheRequest()
        ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [End][Tommy L]

        MultiViewEOM.ActiveViewIndex = ViewIndexEOM.Button
        MultiViewEOL.ActiveViewIndex = ViewIndexEOL.Button
        MultiViewEOV.ActiveViewIndex = ViewIndexEOV.Button

    End Sub


    ' Switch CMS [M]ode

    Protected Sub btnEOMRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshCMSMode()
    End Sub

    Protected Sub btnEOMSwitch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewEOM.ActiveViewIndex = ViewIndexEOM.Confirm
    End Sub

    Protected Sub btnEOMYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.SwitchCMSMode(VaccinationBLL.VaccineRecordSystem.CMS.ToString, strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <New Using: {1}>", AuditLogDescription.SwitchCMSMode, GetUsingEndpointValue)
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EVACC_CONTROL, AuditLogDescription.SwitchCMSMode_ID, strDescription)

        ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [Start][Tommy L]
        ' --------------------------------------------------------------------------------------------------
        'RefreshClearCacheRequest()
        ucClearCacheRequestView_EVaccControl.RefreshClearCacheRequest()
        ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [End][Tommy L]

        RefreshCMSMode()
        MultiViewEOM.ActiveViewIndex = ViewIndexEOM.Button

    End Sub

    Protected Sub btnEOMNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewEOM.ActiveViewIndex = ViewIndexEOM.Button
    End Sub

    ' Switch CMS [L]ink

    Protected Sub btnEOLRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshCMSLink()
    End Sub

    Protected Sub btnEOLPoll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PollCMSLink()
    End Sub

    Protected Sub btnEOLSwitch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewEOL.ActiveViewIndex = ViewIndexEOL.Confirm
    End Sub

    Protected Sub btnEOLYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------

        ' CRE11-002
        'Dim enumUsingEndPoint As EndpointEnum = GetUsingEndpoint()
        'If enumUsingEndPoint = EndpointEnum.EMULATE Then
        '    udtInterfaceControlBLL.SwitchEmulateLink(VaccinationBLL.VaccineRecordSystem.CMS.ToString, strStaffID)
        'Else
        'If rdoEOLCurrentStandbyWEBLOGIC.SelectedIndex < 0 _
        '    OrElse String.IsNullOrEmpty(rdoEOLCurrentStandbyWEBLOGIC.SelectedItem.Value) Then
        '    Return
        'End If

        '' Switch CMS Link            
        'udtInterfaceControlBLL.SwitchCMSLink(strStaffID, rdoEOLCurrentStandbyWEBLOGIC.SelectedItem.Value)
        'End If

        If MultiViewEOLCurrentStandby.ActiveViewIndex = ViewIndexEOLCurrentStandy.MULTIPLE Then
            If rdoEOLCurrentStandbyMULTIPLE.SelectedIndex < 0 _
                OrElse String.IsNullOrEmpty(rdoEOLCurrentStandbyMULTIPLE.SelectedItem.Value) Then
                Return
            End If

            udtInterfaceControlBLL.SwitchCMSLink(strStaffID, rdoEOLCurrentStandbyMULTIPLE.SelectedItem.Value)

        ElseIf MultiViewEOLCurrentStandby.ActiveViewIndex = ViewIndexEOLCurrentStandy.ONE Then
            udtInterfaceControlBLL.SwitchCMSLink(strStaffID, lblEOLCurrentStandby.Text)
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strUsingLink As String = String.Empty
        Dim strUsingMode As String = GetUsingEndpointValue()

        udtInterfaceControlBLL.GetSystemParameter(String.Format("CMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, Nothing)

        Dim strDescription As String = String.Format("{0}: <New Using: {1}>", AuditLogDescription.SwitchCMSLink, strUsingLink)

        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EVACC_CONTROL, AuditLogDescription.SwitchCMSLink_ID, strDescription)

        ucClearCacheRequestView_EVaccControl.RefreshClearCacheRequest()


        RefreshCMSLink()
        MultiViewEOL.ActiveViewIndex = ViewIndexEOL.Button

    End Sub

    Protected Sub btnEOLNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewEOL.ActiveViewIndex = ViewIndexEOL.Button
    End Sub

    ' Turn On / Suspend e[V]accination Record Sharing  

    Protected Sub btnEOVRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshEVaccinationRecordStatus()
    End Sub

    Protected Sub btnEOVTurnOn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.EOVAction) = "Y"
        MultiViewEOV.ActiveViewIndex = ViewIndexEOV.Confirm
    End Sub

    Protected Sub btnEOVSuspend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.EOVAction) = "S"
        MultiViewEOV.ActiveViewIndex = ViewIndexEOV.Confirm
    End Sub

    Protected Sub btnEOVYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.UpdateEVaccinationRecordStatus(VaccinationBLL.VaccineRecordSystem.CMS.ToString, Session(SESS.EOVAction), strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <New Status: {1}>", AuditLogDescription.TurnOnSuspendEVaccinationRecord, Session(SESS.EOVAction))
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EVACC_CONTROL, AuditLogDescription.TurnOnSuspendEVaccinationRecord_ID, strDescription)

        RefreshEVaccinationRecordStatus()

        ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [Start][Tommy L]
        ' --------------------------------------------------------------------------------------------------
        'RefreshClearCacheRequest()
        ucClearCacheRequestView_EVaccControl.RefreshClearCacheRequest()
        ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [End][Tommy L]

        MultiViewEOV.ActiveViewIndex = ViewIndexEOV.Button

    End Sub

    Protected Sub btnEOVNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewEOV.ActiveViewIndex = ViewIndexEOV.Button
    End Sub

    ' Clear [C]ache Request

    ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [Start][Tommy L]
    ' --------------------------------------------------------------------------------------------------
    'Protected Sub btnEOCRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    'RefreshClearCacheRequest()
    'End Sub

    'Protected Sub gvEOC_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
    'If e.Row.RowType = DataControlRowType.DataRow Then
    'Dim lblRequestDtm As Label = e.Row.FindControl("lblRequestDtm")
    'lblRequestDtm.Text = FormatDateTime(lblRequestDtm.Text.Trim)
    'End If
    'End Sub
    ' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [End][Tommy L]

    Protected Sub btnEOBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

    Private Sub RefreshCMSMode()
        ' CRE11-002
        lblEOMCurrentUsing.Text = Me.GetUsingEndpointValue
        lblEOMCurrentStandby.Text = Me.GetCMSStandbyEndpointValue()

        ' Hide Switch mode button if no standby endpoint
        If lblEOMCurrentStandby.Text = String.Empty Then
            lblEOMCurrentStandby.Text = "(Not In Use)"
            btnEOMSwitch.Visible = False
        Else
            btnEOMSwitch.Visible = True
        End If

        RefreshCMSLink()
    End Sub

    Private Sub RefreshCMSLink()
        Dim enumUsingEndPoint As EndpointEnum = GetUsingEndpoint()
        Dim strUsingLink As String = String.Empty
        Dim strStandbyLink As String = String.Empty
        Dim strCurrentStandbySelected As String = String.Empty

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim cllnURL As Collection = Nothing
        Dim intCount As Integer

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        ' Updated on 25 June 2018, always check [Parm_Value1] only

        ' OC4J: Primary Site -> Parm_Value1
        '       Secordary Site -> Parm_Value2
        ' WEBLOGIC: Primary Site -> Parm_Value1
        '         : Secordary Site 1-2 -> Parm_Value1 (CMS_Get_Vaccine_WS_WEBLOGIC_Url1, CMS_Get_Vaccine_WS_WEBLOGIC_Url2, CMS_Get_Vaccine_WS_WEBLOGIC_Url3)
        'udtInterfaceControlBLL.GetSystemParameter(String.Format("CMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, strStandbyLink)
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

        cllnURL = GetUsingEndpointURLListIgnoreServerCache()

        lblEOLCurrentUsing.Text = cllnURL(1)
        lblEOLCurrentStandby.Text = String.Empty

        lblEOLCurrentUsingResult.Text = String.Empty
        lblEOLCurrentUsingResultTime.Text = String.Empty
        lblEOLCurrentStandbyResult.Text = String.Empty
        lblEOLCurrentStandbyResultTime.Text = String.Empty

        ' Save current value
        If rdoEOLCurrentStandbyMULTIPLE.SelectedItem IsNot Nothing Then
            strCurrentStandbySelected = rdoEOLCurrentStandbyMULTIPLE.SelectedItem.Value
        End If
        ' Refresh site list (Standby site only)
        rdoEOLCurrentStandbyMULTIPLE.Items.Clear()
        'CRE13-015 add new URL for new EAI [Start][Karl]
        'If strUsingLink <> cllnURL(2) Then rdoEOLCurrentStandbyMULTIPLE.Items.Add(cllnURL(2))
        'If cllnURL.Count >= 3 AndAlso strUsingLink <> cllnURL(3) Then rdoEOLCurrentStandbyMULTIPLE.Items.Add(cllnURL(3))

        'For intCount = 2 To cllnURL.Count
        '    If strUsingLink <> cllnURL(intCount) Then
        '        rdoEOLCurrentStandbyMULTIPLE.Items.Add(cllnURL(intCount))
        '    End If
        'Next
        'CRE13-015 add new URL for new EAI [End][Karl]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        ' Standby Link
        If cllnURL.Count <= 1 Then
            btnEOLSwitch.Visible = False
            MultiViewEOLCurrentStandby.ActiveViewIndex = ViewIndexEOLCurrentStandy.NotInUse

        ElseIf cllnURL.Count = 2 Then
            btnEOLSwitch.Visible = True
            lblEOLCurrentStandby.Text = cllnURL(2)
            MultiViewEOLCurrentStandby.ActiveViewIndex = ViewIndexEOLCurrentStandy.ONE

        Else
            'Use radio button for Multiple standby link
            btnEOLSwitch.Visible = True

            For intCount = 2 To cllnURL.Count
                rdoEOLCurrentStandbyMULTIPLE.Items.Add(cllnURL(intCount))
            Next

            ' Restore previous value
            If strCurrentStandbySelected <> String.Empty Then
                If rdoEOLCurrentStandbyMULTIPLE.Items.FindByValue(strCurrentStandbySelected) IsNot Nothing Then
                    rdoEOLCurrentStandbyMULTIPLE.Items.FindByValue(strCurrentStandbySelected).Selected = True
                End If
            End If

            MultiViewEOLCurrentStandby.ActiveViewIndex = ViewIndexEOLCurrentStandy.MULTIPLE
        End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]
    End Sub

    Private Sub PollCMSLink()


        lblEOLCurrentStandbyResult.Text = String.Empty

        Dim cllnEndpointURL As Collection = GetUsingEndpointURLListIgnoreServerCache()

        ' INT11-0019
        ' Fix missing Certificate Validation after CRE11-002
        InitServicePointManager()

        'CRE14-020 New EAI infrastructure [Start][Karl]
        Dim enumCurrentEndPoint As EndpointEnum = GetUsingEndpoint()
        'CRE14-020 New EAI infrastructure [End][Karl]

        Try

            ' CRE11-002
            Dim objWSProxyCMS As New WSProxyCMS(Nothing)

            'CRE14-020 New EAI infrastructure [Start][Karl]            
            Dim udtHAVaccineResult As New HAVaccineResult(objWSProxyCMS.GetVaccineInvoke(GetRequestXml().InnerXml, cllnEndpointURL(1).ToString(), enumCurrentEndPoint))
            'CRE14-020 New EAI infrastructure [End][Karl]

            ' CRE11-002 (Handled health check message, Interface Spec 1.17)
            If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.ReturnForHealthCheck Then
                lblEOLCurrentUsingResult.Text = "OK"
            Else
                lblEOLCurrentUsingResult.Text = "Fail: ""Return <> ReturnForHealthCheck"""
            End If

        Catch ex As Exception
            lblEOLCurrentUsingResult.Text = String.Format("Fail: ""{0}""", ex.Message)
        End Try

        lblEOLCurrentUsingResultTime.Text = String.Format("as at {0}", Date.Now.ToString("HH:mm:ss"))

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        Dim lstStandbyUrl As List(Of String) = New List(Of String)

        If lblEOLCurrentStandby.Text <> String.Empty Then
            lstStandbyUrl.Add(lblEOLCurrentStandby.Text)
        End If

        If Me.rdoEOLCurrentStandbyMULTIPLE.Items.Count > 0 Then
            For Each itemURL As ListItem In Me.rdoEOLCurrentStandbyMULTIPLE.Items
                lstStandbyUrl.Add(itemURL.Value.ToString())
            Next
        End If

        ' Poll Standby Site
        Dim i As Integer = 0
        For Each strURL As String In lstStandbyUrl
            i += 1
            If i > 1 Then
                lblEOLCurrentStandbyResult.Text += "<br/>"
            End If
            Try
                Dim objWSProxyCMS As New WSProxyCMS(Nothing)
                Dim udtHAVaccineResult As New HAVaccineResult(objWSProxyCMS.GetVaccineInvoke(GetRequestXml().InnerXml, strURL, enumCurrentEndPoint))

                If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.ReturnForHealthCheck Then
                    lblEOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - OK", i)
                Else
                    lblEOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - Fail: ""Return <> ReturnForHealthCheck""", i)
                End If

            Catch ex As Exception
                lblEOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - Fail: ""{1}""", i, ex.Message)
            End Try
        Next

        If lblEOLCurrentStandbyResult.Text <> String.Empty Then
            lblEOLCurrentStandbyResultTime.Text = String.Format("as at {0}", Date.Now.ToString("HH:mm:ss"))
        End If

        ''CRE14-020 New EAI infrastructure [Start][Karl]
        'If enumCurrentEndPoint = EndpointEnum.EMULATE Then
        '    'CRE14-020 New EAI infrastructure [End][Karl]
        '    Try

        '        ' CRE11-002
        '        Dim objWSProxyCMS As New WSProxyCMS(Nothing)
        '        'CRE14-020 New EAI infrastructure [Start][Karl]
        '        Dim udtHAVaccineResult As New HAVaccineResult(objWSProxyCMS.GetVaccineInvoke(GetRequestXml().InnerXml, cllnEndpointURL(2).ToString(), enumCurrentEndPoint))
        '        'CRE14-020 New EAI infrastructure [End][Karl]

        '        ' CRE11-002 (Handled health check message, Interface Spec 1.17)
        '        If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.ReturnForHealthCheck Then
        '            lblEOLCurrentStandbyResult.Text = "OK"
        '        Else
        '            lblEOLCurrentStandbyResult.Text = "Fail: ""Return <> ReturnForHealthCheck"""
        '        End If

        '    Catch ex As Exception
        '        lblEOLCurrentStandbyResult.Text = String.Format("Fail: ""{0}""", ex.Message)
        '    End Try
        'Else

        '    ' CRE11-002
        '    ' Poll WebLogic 3 standy site
        '    Dim i As Integer = 0
        '    For Each itemURL As ListItem In Me.rdoEOLCurrentStandbyWEBLOGIC.Items
        '        i += 1
        '        If i > 1 Then
        '            lblEOLCurrentStandbyResult.Text += "<br/>"
        '        End If
        '        Try

        '            Dim objWSProxyCMS As New WSProxyCMS(Nothing)
        '            'CRE14-020 New EAI infrastructure [Start][Karl]
        '            Dim udtHAVaccineResult As New HAVaccineResult(objWSProxyCMS.GetVaccineInvoke(GetRequestXml().InnerXml, itemURL.Value, enumCurrentEndPoint))
        '            'CRE14-020 New EAI infrastructure [End][Karl]

        '            If udtHAVaccineResult.ReturnCode = HAVaccineResult.enumReturnCode.ReturnForHealthCheck Then
        '                lblEOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - OK", i)
        '            Else
        '                lblEOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - Fail: ""Return <> ReturnForHealthCheck""", i)
        '            End If

        '        Catch ex As Exception
        '            lblEOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} -Fail: ""{1}""", i, ex.Message)
        '        End Try
        '    Next

        'End If
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]
    End Sub

    Private Sub RefreshEVaccinationRecordStatus()
        Dim strStatus As String = (New InterfaceControlBLL).GetSystemParameter(CONST_SYS_PARAM_TurnOnCMS)
        lblEOVCurrentStatus.Text = strStatus

        btnEOVTurnOn.Visible = strStatus <> "Y"
        btnEOVSuspend.Visible = strStatus <> "S"

    End Sub

#End Region

#Region "[D]H CIMS eVaccination [C]heck"

    Private Sub SetupCIMSEVaccinationCheck()
        lblErrorDCS.Visible = False
        lblErrorDCC.Visible = False
        lblErrorDCT.Visible = False
        lblErrorDCA.Visible = False
        lblErrorDCE.Visible = False

        lblDCSLastUpdate.Text = String.Empty
        lblDCCLastUpdate.Text = String.Empty
        lblDCTLastUpdate.Text = String.Empty
        lblDCALastUpdate.Text = String.Empty
        lblDCELastUpdate.Text = String.Empty

        lblDCSResult1.Text = String.Empty
        lblDCSResult2.Text = String.Empty
        lblDCSResult3.Text = String.Empty
        lblDCSResult4.Text = String.Empty

        lblDCSResult1.BackColor = Nothing
        lblDCSResult2.BackColor = Nothing
        lblDCSResult3.BackColor = Nothing
        lblDCSResult4.BackColor = Nothing

        gvDCC1.Visible = False
        gvDCT.Visible = False
        gvDCA.Visible = False
        gvDCE.Visible = False

        ' Summary: From = Empty
        txtDCSFrom.Text = String.Empty

        ' Summary: To = Empty
        txtDCSTo.Text = String.Empty

        ' CIMS Health Check History: Top Row = 10
        txtDCCTopRow.Text = 10

        ' CIMS Health Check History: From = Today 00:00
        txtDCC2From.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' CIMS Health Check History: To = Empty
        txtDCC2To.Text = String.Empty

        ' Transaction External Reference Status History: From = Today 00:00
        txtDCTFrom.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' Transaction External Reference Status History: To = Empty
        txtDCTTo.Text = String.Empty

        ' HCSP Audit Log History: From = Today 00:00
        txtDCAFrom.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' HCSP Audit Log History: To = Empty
        txtDCATo.Text = String.Empty

        ' eHS eVaccination Record Service History: From = Today 00:00
        txtDCEFrom.Text = String.Format("{0} 00:00", Date.Now.ToString("yyyy-MM-dd"))

        ' eHS eVaccination Record Service History: To = Empty
        txtDCETo.Text = String.Empty

        ' CRE11-002
        ' Refresh Primary/Secondary site URL for user reference


        Call GenerateSummaryCIMSURL()

    End Sub

    Private Sub GenerateSummaryCIMSURL()
        Dim intCount As Integer
        Dim cllnURL As Collection = GetCIMSUsingEndpointURLList()

        For intCount = 1 To cllnURL.Count
            Dim lblSite As New Label
            Dim lblSiteURL As New Label

            lblSite.Width = 150
            lblSiteURL.Width = 600

            If intCount = 1 Then
                lblSite.ID = "lblDCSPrimarySite"
                lblSiteURL.ID = "lblDCSPrimarySiteURL"

                lblSite.Text = "Primary Site:"
                lblSiteURL.Text = cllnURL(intCount)
            Else
                lblSite.ID = "lblDCSSecondarySite" & (intCount - 1).ToString
                lblSiteURL.ID = "lblDCSSecondarySiteURL" & (intCount - 1).ToString

                lblSite.Text = "Secondary Site " & (intCount - 1).ToString & ":"
                lblSiteURL.Text = cllnURL(intCount)
            End If

            Me.pnlCIMSSites.Controls.Add(lblSite)
            Me.pnlCIMSSites.Controls.Add(lblSiteURL)
            Me.pnlCIMSSites.Controls.Add(New LiteralControl("<br />"))
            Me.pnlCIMSSites.Controls.Add(New LiteralControl("<br />"))
        Next
    End Sub

    ' [S]ummary

    Protected Sub btnDCSRefreshAll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCIMSSummary()
    End Sub

    ' (1) [C]IMS Health Check History

    Protected Sub btnDCCRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCIMSHealthCheckHistory("R")
    End Sub

    Protected Sub btnDCCRefresh2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCIMSHealthCheckHistory("T")
    End Sub

    Protected Sub gvDCC1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblLogID As Label = e.Row.FindControl("lblLogID")
            If Regex.IsMatch(lblLogID.Text.Trim, ConfigurationManager.AppSettings("CriterionDCCRegEx")) Then
                lblLogID.ForeColor = Drawing.Color.Red
            End If

        End If

    End Sub

    Protected Sub gvDCC2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblLogID As Label = e.Row.FindControl("lblLogID")
            If Regex.IsMatch(lblLogID.Text.Trim, ConfigurationManager.AppSettings("CriterionDCCRegEx")) Then
                lblLogID.ForeColor = Drawing.Color.Red
            End If

        End If

    End Sub

    Protected Sub btnDCCClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvDCC1.Visible = False
    End Sub

    Protected Sub lbtnDCC2GetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDCC2From.Text = AntiXssEncoder.HtmlEncode(txtDCSFrom.Text, True)
        txtDCC2To.Text = AntiXssEncoder.HtmlEncode(txtDCSTo.Text, True)
    End Sub

    ' (2) [T]ransaction External Reference Status History 

    Protected Sub btnDCTRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCIMSTransactionExternalReferenceStatusHistory()
    End Sub

    Protected Sub gvDCT_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblDHVaccRefStatus As Label = e.Row.FindControl("lblDHVaccRefStatus")
            If Regex.IsMatch(lblDHVaccRefStatus.Text.Trim, ConfigurationManager.AppSettings("CriterionDCTRegEx")) Then
                lblDHVaccRefStatus.ForeColor = Drawing.Color.Red
            End If

            Dim lblTransactionTime As Label = e.Row.FindControl("lblTransactionTime")
            lblTransactionTime.Text = FormatDateTime(lblTransactionTime.Text.Trim)

        End If

    End Sub

    Protected Sub btnDCTClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvDCT.Visible = False
    End Sub

    Protected Sub lbtnDCTGetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDCTFrom.Text = AntiXssEncoder.HtmlEncode(txtDCSFrom.Text, True)
        txtDCTTo.Text = AntiXssEncoder.HtmlEncode(txtDCSTo.Text, True)
    End Sub

    ' (3) HCSP [A]udit Log History

    Protected Sub btnDCARefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCIMSHCSPAuditLogHistory()
    End Sub

    Protected Sub gvDCA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblLogID As Label = e.Row.FindControl("lblLogID")
            If Regex.IsMatch(lblLogID.Text.Trim, ConfigurationManager.AppSettings("CriterionDCARegEx")) Then
                lblLogID.ForeColor = Drawing.Color.Red
            End If

            Dim lblDescription As Label = e.Row.FindControl("lblDescription")
            lblDescription.Text = lblDescription.Text.Substring(0, lblDescription.Text.IndexOf("<") - 2)

        End If

    End Sub

    Protected Sub btnDCAClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvDCA.Visible = False
    End Sub

    Protected Sub lbtnDCAGetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDCAFrom.Text = AntiXssEncoder.HtmlEncode(txtDCSFrom.Text, True)
        txtDCATo.Text = AntiXssEncoder.HtmlEncode(txtDCSTo.Text, True)
    End Sub

    ' (4) eHS [e]Vaccination Record Service History  

    Protected Sub btnDCERefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshCIMSEHSEVaccinationRecordServiceHistory()
    End Sub

    Protected Sub gvDCE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSystemDtm As Label = e.Row.FindControl("lblSystemDtm")
            lblSystemDtm.Text = FormatDateTime(lblSystemDtm.Text.Trim)

            Dim lblBatchEnquiry As Label = e.Row.FindControl("lblBatchEnquiry")
            Dim lblTimeDiff As Label = e.Row.FindControl("lblTimeDiff")

            If lblBatchEnquiry.Text.Trim = "N" Then
                If CInt(lblTimeDiff.Text.Trim) >= CInt(ConfigurationManager.AppSettings("CriterionDCE")) Then
                    lblTimeDiff.ForeColor = Drawing.Color.Red
                End If
            End If

            If lblBatchEnquiry.Text.Trim = "Y" Then
                If CInt(lblTimeDiff.Text.Trim) >= CInt(ConfigurationManager.AppSettings("CriterionDCE_Batch")) Then
                    lblTimeDiff.ForeColor = Drawing.Color.Red
                End If
            End If

        End If

    End Sub

    Protected Sub btnDCEClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        gvECE.Visible = False
    End Sub

    Protected Sub lbtnDCEGetSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDCEFrom.Text = AntiXssEncoder.HtmlEncode(txtDCSFrom.Text, True)
        txtDCETo.Text = AntiXssEncoder.HtmlEncode(txtDCSTo.Text, True)
    End Sub

    '

    Protected Sub btnDCBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

    '

    Private Sub RefreshCIMSSummary()
        Dim dtmFrom As Date = Date.MinValue

        If txtDCSFrom.Text = String.Empty Then
            dtmFrom = CDate(Date.Now.AddHours(CInt(ConfigurationManager.AppSettings("SummaryHourBefore"))).ToString("yyyy-MM-dd HH:mm:ss"))
            txtDCSFrom.Text = dtmFrom.ToString("yyyy-MM-dd HH:mm:ss")
        Else
            Try
                dtmFrom = CDate(txtDCSFrom.Text)
            Catch ex As Exception
                lblErrorDCS.Text = "From is invalid"
                lblErrorDCS.Visible = True
                Return
            End Try
        End If

        Dim dtmTo As Date = Date.MinValue

        If txtDCSTo.Text = String.Empty Then
            dtmTo = Date.Now()
            txtDCSTo.Text = dtmTo.ToString("yyyy-MM-dd HH:mm:ss")
        Else
            Try
                dtmTo = CDate(txtDCSTo.Text)

            Catch ex As Exception
                lblErrorDCS.Text = "To is invalid"
                lblErrorDCS.Visible = True
                Return
            End Try
        End If

        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        ' (1) CMS Health Check History
        Dim intError As Integer = 0
        Dim strRegExDCC As String = ConfigurationManager.AppSettings("CriterionDCCRegEx")

        For Each dr As DataRow In udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC_CIMS", Nothing, Nothing, dtmFrom, dtmTo).Rows
            If Regex.IsMatch(dr("Log_ID").ToString.Trim, strRegExDCC) Then
                intError += 1
            End If
        Next

        If intError = 0 Then
            lblDCSResult1.Text = "Normal"
            lblDCSResult1.BackColor = Drawing.Color.PaleGreen
        Else
            lblDCSResult1.Text = String.Format("{0} Error Cases", intError)
            lblDCSResult1.BackColor = Drawing.Color.Pink
        End If

        ' (2) Transaction External Reference Status History
        intError = 0
        Dim strDHVaccRefStatus As String = String.Empty
        Dim strRegExDCT As String = ConfigurationManager.AppSettings("CriterionDCTRegEx")

        For Each dr As DataRow In udtInterfaceControlBLL.GetVoucherTransactionByTransactionDtm(dtmFrom, dtmTo).Rows
            strDHVaccRefStatus = String.Empty

            If Not IsDBNull(dr("DH_Vaccine_Ref_Status")) Then strDHVaccRefStatus = dr("DH_Vaccine_Ref_Status").ToString.Trim

            If strDHVaccRefStatus <> String.Empty AndAlso Regex.IsMatch(strDHVaccRefStatus, strRegExDCT) Then
                intError += 1
            End If

        Next

        If intError = 0 Then
            lblDCSResult2.Text = "Normal"
            lblDCSResult2.BackColor = Drawing.Color.PaleGreen
        Else
            lblDCSResult2.Text = String.Format("{0} Error Cases", intError)
            lblDCSResult2.BackColor = Drawing.Color.Pink
        End If

        ' (3) HCSP Audit Log History
        intError = 0
        Dim strRegExDCA As String = ConfigurationManager.AppSettings("CriterionDCARegEx")

        For Each dr As DataRow In udtInterfaceControlBLL.GetHCSPAuditLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CIMS.ToString).Rows
            If Regex.IsMatch(dr("Log_ID").ToString.Trim, strRegExDCA) Then
                intError += 1
            End If

        Next

        If intError = 0 Then
            lblDCSResult3.Text = "Normal"
            lblDCSResult3.BackColor = Drawing.Color.PaleGreen
        Else
            lblDCSResult3.Text = String.Format("{0} Error Cases", intError)
            lblDCSResult3.BackColor = Drawing.Color.Pink
        End If

        ' (4) eHS eVaccination Record Service History  
        intError = 0

        Dim intTimeDiffLimit As Integer = CInt(ConfigurationManager.AppSettings("CriterionDCE"))
        Dim intTimeDiffLimit_Batch As Integer = CInt(ConfigurationManager.AppSettings("CriterionDCE_Batch"))
        Dim intSingleError As Integer = 0
        Dim intBatchError As Integer = 0

        For Each dr As DataRow In udtInterfaceControlBLL.GetInterfaceLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CIMS.ToString).Rows
            If dr("Batch_Enquiry").ToString.Trim = "N" AndAlso CInt(dr("Time_Diff").ToString.Trim) >= intTimeDiffLimit Then
                intError += 1
                intSingleError += 1
            End If

            If dr("Batch_Enquiry").ToString.Trim = "Y" AndAlso CInt(dr("Time_Diff").ToString.Trim) >= intTimeDiffLimit_Batch Then
                intError += 1
                intBatchError += 1
            End If
        Next

        lblDCSResult4.Text = String.Empty

        If intError = 0 Then
            lblDCSResult4.Text = "Normal"
            lblDCSResult4.BackColor = Drawing.Color.PaleGreen
        Else
            If intSingleError > 0 Then
                lblDCSResult4.Text = String.Format("{0} Cases >= {1} ms (Single)", intSingleError, intTimeDiffLimit)
            End If

            If intBatchError > 0 Then
                If lblDCSResult4.Text <> String.Empty Then
                    lblDCSResult4.Text += "<br>"
                End If
                lblDCSResult4.Text += String.Format("{0} Cases >= {1} ms (Batch)", intBatchError, intTimeDiffLimit_Batch)
            End If

            lblDCSResult4.BackColor = Drawing.Color.Pink
        End If

        ' Update Last update
        lblDCSLastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshCIMSHealthCheckHistory(ByVal strMode As String)
        Dim dt1 As DataTable = Nothing
        Dim dt2 As DataTable = Nothing

        If strMode = "R" Then
            lblErrorDCC.Visible = False

            ' Retrieve input-ed Top Row
            Dim intTopRow As Integer = -1

            Try
                intTopRow = CInt(txtDCCTopRow.Text.Trim)
            Catch ex As Exception
                lblErrorDCC.Text = "Top Row is invalid"
                lblErrorDCC.Visible = True
                Return
            End Try

            If intTopRow <= 0 Then
                lblErrorDCC.Text = "Top Row is invalid"
                lblErrorDCC.Visible = True
                Return
            End If

            ' Retrieve data
            Dim udtInterfaceControlBLL As New InterfaceControlBLL
            dt1 = udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC_CIMS", Nothing, intTopRow)

        ElseIf strMode = "T" Then
            lblErrorDCC2.Visible = False

            ' Retrieve input-ed From
            Dim dtmFrom As Date = DateTime.MinValue
            Dim dtmTo As Date = DateTime.MinValue

            Try
                dtmFrom = CDate(txtDCC2From.Text.Trim)
            Catch ex As Exception
                lblErrorDCC2.Text = "From is invalid"
                lblErrorDCC2.Visible = True
                Return
            End Try

            ' Retrieve input-ed To
            If txtDCC2To.Text.Trim = String.Empty Then
                dtmTo = DateTime.Now

            Else
                Try
                    dtmTo = CDate(txtDCC2To.Text.Trim)
                Catch ex As Exception
                    lblErrorDCC2.Text = "To is invalid"
                    lblErrorDCC2.Visible = True
                    Return
                End Try

            End If

            ' Retrieve data
            Dim udtInterfaceControlBLL As New InterfaceControlBLL
            dt1 = udtInterfaceControlBLL.GetInterfaceHealthCheckLog("EVACC_CIMS", Nothing, Nothing, dtmFrom, dtmTo)

        End If

        gvDCC1.DataSource = dt1
        gvDCC1.DataBind()
        gvDCC1.Visible = True

        ' Update Last update
        lblDCCLastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshCIMSTransactionExternalReferenceStatusHistory()
        lblErrorDCT.Visible = False

        Dim dtmFrom As Date = DateTime.MinValue
        Dim dtmTo As Date = DateTime.MinValue

        ' Retrieve input-ed From
        Try
            dtmFrom = CDate(txtDCTFrom.Text.Trim)
        Catch ex As Exception
            lblErrorDCT.Text = "From is invalid"
            lblErrorDCT.Visible = True
            Return
        End Try

        ' Retrieve input-ed To
        If txtDCTTo.Text.Trim = String.Empty Then
            dtmTo = DateTime.Now

        Else
            Try
                dtmTo = CDate(txtDCTTo.Text.Trim)
            Catch ex As Exception
                lblErrorDCT.Text = "To is invalid"
                lblErrorDCT.Visible = True
                Return
            End Try

        End If

        Dim dt As DataTable = (New InterfaceControlBLL).GetVoucherTransactionByTransactionDtm(dtmFrom, dtmTo)

        gvDCT.DataSource = dt
        gvDCT.DataBind()
        gvDCT.Visible = True

        ' Update Last update
        lblDCTLastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshCIMSHCSPAuditLogHistory()
        lblErrorDCA.Visible = False

        Dim dtmFrom As Date = DateTime.MinValue
        Dim dtmTo As Date = DateTime.MinValue

        ' Retrieve input-ed From
        Try
            dtmFrom = CDate(txtDCAFrom.Text.Trim)
        Catch ex As Exception
            lblErrorDCA.Text = "From is invalid"
            lblErrorDCA.Visible = True
            Return
        End Try

        ' Retrieve input-ed To
        If txtDCATo.Text.Trim = String.Empty Then
            dtmTo = DateTime.Now

        Else
            Try
                dtmTo = CDate(txtDCATo.Text.Trim)
            Catch ex As Exception
                lblErrorDCA.Text = "To is invalid"
                lblErrorDCA.Visible = True
                Return
            End Try

        End If

        Dim dt As DataTable = (New InterfaceControlBLL).GetHCSPAuditLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CIMS.ToString)

        gvDCA.DataSource = dt
        gvDCA.DataBind()
        gvDCA.Visible = True

        ' Update Last update
        lblDCALastUpdate.Text = GetLastUpdate()

    End Sub

    Private Sub RefreshCIMSEHSEVaccinationRecordServiceHistory()
        lblErrorDCE.Visible = False

        Dim dtmFrom As Date = DateTime.MinValue
        Dim dtmTo As Date = DateTime.MinValue

        ' Retrieve input-ed From
        Try
            dtmFrom = CDate(txtDCEFrom.Text.Trim)
        Catch ex As Exception
            lblErrorDCE.Text = "From is invalid"
            lblErrorDCE.Visible = True
            Return
        End Try

        ' Retrieve input-ed To
        If txtDCETo.Text.Trim = String.Empty Then
            dtmTo = DateTime.Now

        Else
            Try
                dtmTo = CDate(txtDCETo.Text.Trim)
            Catch ex As Exception
                lblErrorDCE.Text = "To is invalid"
                lblErrorDCE.Visible = True
                Return
            End Try

        End If

        Dim dt As DataTable = (New InterfaceControlBLL).GetInterfaceLogByDtm(dtmFrom, dtmTo, VaccinationBLL.VaccineRecordSystem.CIMS.ToString)

        gvDCE.DataSource = dt
        gvDCE.DataBind()
        gvDCE.Visible = True

        ' Update Last update
        lblDCELastUpdate.Text = GetLastUpdate()

    End Sub

#End Region

#Region "[D]H CIMS eVaccination C[o]ntrol"

    Private Sub SetupCIMSEVaccinationControl()
        RefreshCIMSMode()
        RefreshCIMSLink()
        RefreshCIMSEVaccinationRecordStatus()

        ucClearCacheRequestView_CIMSEVaccControl.RefreshClearCacheRequest()

        MultiViewDOM.ActiveViewIndex = ViewIndexDOM.Button
        MultiViewDOL.ActiveViewIndex = ViewIndexDOL.Button
        MultiViewDOV.ActiveViewIndex = ViewIndexDOV.Button

    End Sub


    ' Switch CIMS [M]ode

    Protected Sub btnDOMRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshCIMSMode()
    End Sub

    Protected Sub btnDOMSwitch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewDOM.ActiveViewIndex = ViewIndexDOM.Confirm
    End Sub

    Protected Sub btnDOMYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.SwitchCMSMode(VaccinationBLL.VaccineRecordSystem.CIMS.ToString, strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <New Using: {1}>", AuditLogDescription.SwitchCIMSMode, GetCIMSUsingEndpointValue)
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EVACC_CONTROL, AuditLogDescription.SwitchCIMSMode_ID, strDescription)

        ucClearCacheRequestView_CIMSEVaccControl.RefreshClearCacheRequest()


        RefreshCIMSMode()
        MultiViewDOM.ActiveViewIndex = ViewIndexDOM.Button

    End Sub

    Protected Sub btnDOMNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewDOM.ActiveViewIndex = ViewIndexDOM.Button
    End Sub

    ' Switch CIMS [L]ink

    Protected Sub btnDOLRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshCIMSLink()
    End Sub

    Protected Sub btnDOLPoll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PollCIMSLink()
    End Sub

    Protected Sub btnDOLSwitch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewDOL.ActiveViewIndex = ViewIndexDOL.Confirm
    End Sub

    Protected Sub btnDOLYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        If MultiViewDOLCurrentStandby.ActiveViewIndex = ViewIndexDOLCurrentStandy.MULTIPLE Then
            If rdoDOLCurrentStandbyMULTIPLE.SelectedIndex < 0 _
                OrElse String.IsNullOrEmpty(rdoDOLCurrentStandbyMULTIPLE.SelectedItem.Value) Then
                Return
            End If

            udtInterfaceControlBLL.SwitchCIMSLink(strStaffID, rdoDOLCurrentStandbyMULTIPLE.SelectedItem.Value)

        ElseIf MultiViewDOLCurrentStandby.ActiveViewIndex = ViewIndexDOLCurrentStandy.ONE Then
            udtInterfaceControlBLL.SwitchCIMSLink(strStaffID, lblDOLCurrentStandby.Text)
        End If


        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strUsingLink As String = String.Empty
        Dim strUsingMode As String = GetCIMSUsingEndpointValue()

        udtInterfaceControlBLL.GetSystemParameter(String.Format("CIMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, Nothing)

        Dim strDescription As String = String.Format("{0}: <New Using: {1}>", AuditLogDescription.SwitchCIMSLink, strUsingLink)

        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EVACC_CONTROL, AuditLogDescription.SwitchCIMSLink_ID, strDescription)

        ucClearCacheRequestView_CIMSEVaccControl.RefreshClearCacheRequest()

        RefreshCIMSLink()
        MultiViewDOL.ActiveViewIndex = ViewIndexDOL.Button

    End Sub

    Protected Sub btnDOLNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewDOL.ActiveViewIndex = ViewIndexDOL.Button
    End Sub

    ' Turn On / Suspend CIMS e[V]accination Record Sharing  

    Protected Sub btnDOVRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshCIMSEVaccinationRecordStatus()
    End Sub

    Protected Sub btnDOVTurnOn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.DOVAction) = "Y"
        MultiViewDOV.ActiveViewIndex = ViewIndexDOV.Confirm
    End Sub

    Protected Sub btnDOVSuspend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.DOVAction) = "S"
        MultiViewDOV.ActiveViewIndex = ViewIndexDOV.Confirm
    End Sub

    Protected Sub btnDOVYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.UpdateEVaccinationRecordStatus(VaccinationBLL.VaccineRecordSystem.CIMS.ToString, Session(SESS.DOVAction), strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <New Status: {1}>", AuditLogDescription.TurnOnSuspendCIMSEVaccinationRecord, Session(SESS.DOVAction))
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EVACC_CONTROL, AuditLogDescription.TurnOnSuspendCIMSEVaccinationRecord_ID, strDescription)

        RefreshCIMSEVaccinationRecordStatus()

        ucClearCacheRequestView_CIMSEVaccControl.RefreshClearCacheRequest()

        MultiViewDOV.ActiveViewIndex = ViewIndexDOV.Button

    End Sub

    Protected Sub btnDOVNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewDOV.ActiveViewIndex = ViewIndexDOV.Button
    End Sub

    Protected Sub btnDOBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

    Private Sub RefreshCIMSMode()
        lblDOMCurrentUsing.Text = Me.GetCIMSUsingEndpointValue
        lblDOMCurrentStandby.Text = Me.GetCIMSStandbyEndpointValue()

        ' Hide Switch mode button if no standby endpoint
        If lblDOMCurrentStandby.Text = String.Empty Then
            lblDOMCurrentStandby.Text = "(Not In Use)"
            btnDOMSwitch.Visible = False
        Else
            btnDOMSwitch.Visible = True
        End If

        RefreshCIMSLink()
    End Sub

    Private Sub RefreshCIMSLink()
        Dim enumUsingEndPoint As CIMSEndpoint = GetCIMSUsingEndpoint()
        Dim strUsingLink As String = String.Empty
        Dim strStandbyLink As String = String.Empty
        Dim strCurrentStandbySelected As String = String.Empty

        Dim cllnURL As Collection = Nothing
        Dim intCount As Integer

        cllnURL = GetCIMSUsingEndpointURLList()

        lblDOLCurrentUsing.Text = cllnURL(1)
        lblDOLCurrentStandby.Text = String.Empty

        lblDOLCurrentUsingResult.Text = String.Empty
        lblDOLCurrentUsingResultTime.Text = String.Empty
        lblDOLCurrentStandbyResult.Text = String.Empty
        lblDOLCurrentStandbyResultTime.Text = String.Empty

        ' Save current value
        If rdoDOLCurrentStandbyMULTIPLE.SelectedItem IsNot Nothing Then
            strCurrentStandbySelected = rdoDOLCurrentStandbyMULTIPLE.SelectedItem.Value
        End If
        ' Refresh site list (Standby site only)
        rdoDOLCurrentStandbyMULTIPLE.Items.Clear()

        ' Standby Link
        If cllnURL.Count <= 1 Then
            btnDOLSwitch.Visible = False
            MultiViewDOLCurrentStandby.ActiveViewIndex = ViewIndexDOLCurrentStandy.NotInUse

        ElseIf cllnURL.Count = 2 Then
            btnDOLSwitch.Visible = True
            lblDOLCurrentStandby.Text = cllnURL(2)
            MultiViewDOLCurrentStandby.ActiveViewIndex = ViewIndexDOLCurrentStandy.ONE

        Else
            'Use radio button for Multiple standby link
            btnDOLSwitch.Visible = True

            For intCount = 2 To cllnURL.Count
                rdoDOLCurrentStandbyMULTIPLE.Items.Add(cllnURL(intCount))
            Next

            ' Restore previous value
            If strCurrentStandbySelected <> String.Empty Then
                If rdoDOLCurrentStandbyMULTIPLE.Items.FindByValue(strCurrentStandbySelected) IsNot Nothing Then
                    rdoDOLCurrentStandbyMULTIPLE.Items.FindByValue(strCurrentStandbySelected).Selected = True
                End If
            End If

            MultiViewDOLCurrentStandby.ActiveViewIndex = ViewIndexDOLCurrentStandy.MULTIPLE
        End If
    End Sub

    Private Sub PollCIMSLink()

        lblDOLCurrentStandbyResult.Text = String.Empty

        InitServicePointManager()

        Dim enumCurrentEndPoint As EndpointEnum = GetCIMSUsingEndpoint()

        Dim udtDHVaccineResult As DHVaccineResult = Nothing
        Dim objWSProxyCIMS As New WSProxyDHCIMS(Nothing)

        Dim udtVaccineEnqReq As New vaccineEnqReq
        udtVaccineEnqReq.mode = WSProxyDHCIMS.Mode.HealthCheck

        ' Poll Using Site
        Try
            'Enquiry CIMS
            Dim udtVaccineEnqRsp As vaccineEnqRsp = objWSProxyCIMS.GetVaccineInvoke(udtVaccineEnqReq, lblDOLCurrentUsing.Text, enumCurrentEndPoint)

            ' Convert object "VaccineEnqRsp" to object "DHVaccineResult"      
            udtDHVaccineResult = New DHVaccineResult(udtVaccineEnqReq, udtVaccineEnqRsp)

            If udtDHVaccineResult.ReturnCode = DHVaccineResult.enumReturnCode.HealthCheck Then
                lblDOLCurrentUsingResult.Text = "OK"
            Else
                lblDOLCurrentUsingResult.Text = "Fail: ""Return <> HealthCheck"""
            End If

        Catch ex As Exception
            lblDOLCurrentUsingResult.Text = String.Format("Fail: ""{0}""", ex.Message)
        End Try

        lblDOLCurrentUsingResultTime.Text = String.Format("as at {0}", Date.Now.ToString("HH:mm:ss"))

        Dim lstStandbyUrl As List(Of String) = New List(Of String)

        If lblDOLCurrentStandby.Text <> String.Empty Then
            lstStandbyUrl.Add(lblDOLCurrentStandby.Text)
        End If

        If Me.rdoDOLCurrentStandbyMULTIPLE.Items.Count > 0 Then
            For Each itemURL As ListItem In Me.rdoDOLCurrentStandbyMULTIPLE.Items
                lstStandbyUrl.Add(itemURL.Value.ToString())
            Next
        End If

        ' Poll Standby Site
        Dim i As Integer = 0
        For Each strURL As String In lstStandbyUrl
            i += 1
            If i > 1 Then
                lblDOLCurrentStandbyResult.Text += "<br/>"
            End If
            Try
                'Enquiry CIMS
                Dim udtVaccineEnqRsp As vaccineEnqRsp = objWSProxyCIMS.GetVaccineInvoke(udtVaccineEnqReq, strURL, enumCurrentEndPoint)

                ' Convert object "VaccineEnqRsp" to object "DHVaccineResult"      
                udtDHVaccineResult = New DHVaccineResult(udtVaccineEnqReq, udtVaccineEnqRsp)

                If udtDHVaccineResult.ReturnCode = DHVaccineResult.enumReturnCode.HealthCheck Then
                    lblDOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - OK", i)
                Else
                    lblDOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - Fail: ""Return <> HealthCheck""", i)
                End If

            Catch ex As Exception
                lblDOLCurrentStandbyResult.Text += String.Format("Secondary Site {0} - Fail: ""{1}""", i, ex.Message)
            End Try
        Next

        If lblDOLCurrentStandbyResult.Text <> String.Empty Then
            lblDOLCurrentStandbyResultTime.Text = String.Format("as at {0}", Date.Now.ToString("HH:mm:ss"))
        End If
    End Sub

    Private Sub RefreshCIMSEVaccinationRecordStatus()
        Dim strStatus As String = (New InterfaceControlBLL).GetSystemParameter(CONST_SYS_PARAM_TurnOnCIMS)
        lblDOVCurrentStatus.Text = strStatus

        btnDOVTurnOn.Visible = strStatus <> "Y"
        btnDOVSuspend.Visible = strStatus <> "S"

    End Sub

#End Region

#Region "View 5: TSW Patient List Control"
    'INT15-0024 (Add PPI-ePR control to Interface Control Webpage) [End][Chris YIM]

    Private Sub SetupPPIePRControl()
        RefreshPPIePRSite()
        ucClearCacheRequestView_PPIePRControl.RefreshClearCacheRequest()

        MultiViewPPIePRSite.ActiveViewIndex = ViewIndexPPIePRSite.Button
    End Sub

    'INT15-0024 (Add PPI-ePR control to Interface Control Webpage) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub RefreshPPIePRSite()
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strUsing_PPIePRWSLink As String = String.Empty
        'Dim strUsing_SSO_PPI_App_IdP_WS_Url As String = String.Empty
        'Dim strUsing_SSO_PPI_App_SP_WS_Url As String = String.Empty
        'Dim strUsing_TokenReplacementWS_EHSToPPI_Url As String = String.Empty
        Dim strStandby_PPIePRWSLink As String = String.Empty
        'Dim strStandby_SSO_PPI_App_IdP_WS_Url As String = String.Empty
        'Dim strStandby_SSO_PPI_App_SP_WS_Url As String = String.Empty
        'Dim strStandby_TokenReplacementWS_EHSToPPI_Url As String = String.Empty

        udtInterfaceControlBLL.GetSystemParameter("PPIePRWSLink", strUsing_PPIePRWSLink, strStandby_PPIePRWSLink)
        'udtInterfaceControlBLL.GetSystemParameter(ConfigurationManager.AppSettings("SystemParameters_SSO_IdP_WS_Url"), strUsing_SSO_PPI_App_IdP_WS_Url, strStandby_SSO_PPI_App_IdP_WS_Url)
        'udtInterfaceControlBLL.GetSystemParameter(ConfigurationManager.AppSettings("SystemParameters_SSO_SP_WS_Url"), strUsing_SSO_PPI_App_SP_WS_Url, strStandby_SSO_PPI_App_SP_WS_Url)
        'udtInterfaceControlBLL.GetSystemParameter("TokenReplacementWS_EHSToPPI_Url", strUsing_TokenReplacementWS_EHSToPPI_Url, strStandby_TokenReplacementWS_EHSToPPI_Url)

        lblUsing_PPIePR.Text = strUsing_PPIePRWSLink
        'lblUsing_SSO_PPI_App_IdP_WS_Url.Text = strUsing_SSO_PPI_App_IdP_WS_Url
        'lblUsing_SSO_PPI_App_SP_WS_Url.Text = strUsing_SSO_PPI_App_SP_WS_Url
        'lblUsing_TokenReplacementWS_EHSToPPI_Url.Text = strUsing_TokenReplacementWS_EHSToPPI_Url

        lblStandby_PPIePR.Text = strStandby_PPIePRWSLink
        'lblStandby_SSO_PPI_App_IdP_WS_Url.Text = strStandby_SSO_PPI_App_IdP_WS_Url
        'lblStandby_SSO_PPI_App_SP_WS_Url.Text = strStandby_SSO_PPI_App_SP_WS_Url
        'lblStandby_TokenReplacementWS_EHSToPPI_Url.Text = strStandby_TokenReplacementWS_EHSToPPI_Url
    End Sub
    'INT15-0024 (Add PPI-ePR control to Interface Control Webpage) [End][Chris YIM]

    Protected Sub btnPPIePRSite_Refresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshPPIePRSite()
    End Sub

    Protected Sub btnPPIePRSite_Switch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()

        MultiViewPPIePRSite.ActiveViewIndex = ViewIndexPPIePRSite.Confirm
    End Sub

    'INT15-0024 (Add PPI-ePR control to Interface Control Webpage) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub btnPPIePRSite_Switch_Yes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()
        Dim strAuditLogDesc As String = String.Empty
        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        udtInterfaceControlBLL.SwitchPPIePRSite(strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        'strAuditLogDesc = String.Format("{0}: <New Using: ({1}){2}, ({3}){4}, ({5}){6}, ({7}){8}>", _
        '                                AuditLogDescription.SwitchPPIePRSite, _
        '                                "PPIePRWSLink", lblStandby_PPIePR.Text, _
        '                                "SSO_PPI_App_IdP_WS_Url", lblStandby_SSO_PPI_App_IdP_WS_Url.Text, _
        '                                "SSO_PPI_App_SP_WS_Url", lblStandby_SSO_PPI_App_SP_WS_Url.Text, _
        '                                "lblStandby_TokenReplacementWS_EHSToPPI_Url", lblStandby_TokenReplacementWS_EHSToPPI_Url.Text)
        strAuditLogDesc = String.Format("{0}: <New Using: ({1}){2}>", _
                                AuditLogDescription.SwitchPPIePRSite, _
                                "PPIePRWSLink", lblStandby_PPIePR.Text)

        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession(), FUNCT_CODE_PPIEPR_CONTROL, AuditLogDescription.SwitchPPIePRSite_ID, strAuditLogDesc)

        ucClearCacheRequestView_PPIePRControl.RefreshClearCacheRequest()

        RefreshPPIePRSite()

        MultiViewPPIePRSite.ActiveViewIndex = ViewIndexPPIePRSite.Button
    End Sub
    'INT15-0024 (Add PPI-ePR control to Interface Control Webpage) [End][Chris YIM]

    Protected Sub btnPPIePRSite_Switch_No_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()

        MultiViewPPIePRSite.ActiveViewIndex = ViewIndexPPIePRSite.Button
    End Sub

    Protected Sub btnBack_PPIePRControl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub
#End Region

#Region "View 6: Token Server Control"

    ' CRE15-001 RSA Server Upgrade [Start][Winnie]
    Private Sub SetupTokenServerControl()

        ucClearCacheRequestView_TokenServerControl.RefreshClearCacheRequest()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.GetSystemParameter("RSAAPIVersion", hfTMRSAVersion.Value, hfTSRSAVersion.Value)

        RefreshMainTokenServer()
        mvTMAction.SetActiveView(vTMButton)

        If hfTSRSAVersion.Value <> String.Empty Then
            RefreshSubTokenServer()
            mvTSAction.SetActiveView(vTSButton)
            tblRSASub.Visible = True
        Else
            tblRSASub.Visible = False
        End If

    End Sub

    Private Sub RefreshMainTokenServer()
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strRSAAPIVersionMain As String = hfTMRSAVersion.Value
        ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
        ' Token Server (Main)
        lblTMRSAVersion.Text = String.Format("RSA{0}", strRSAAPIVersionMain)
        lblTMLink.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}Link", strRSAAPIVersionMain))
        lblTMWebLogicUsername.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}WebLogicUsername", strRSAAPIVersionMain))
        lblTMAMUsername.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}AMUsername", strRSAAPIVersionMain))

        lblTMStatus.Text = "(Press Poll RSA Server)"
        lblTMStatus.ForeColor = Drawing.Color.Gray
        lblTMAMPasswordLastChange.Text = "(Press Poll RSA Server)"
        lblTMAMPasswordLastChange.ForeColor = Drawing.Color.Gray
        ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

        ' CRE15-001 RSA Server Upgrade [Start][Winnie]        
        If udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}AgentMethod", strRSAAPIVersionMain)).Equals("WS") Then
            lblTMWSLink.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}AgentWSLink", strRSAAPIVersionMain))

            lblTMWSStatus.Text = "(Press WS Health Check)"
            lblTMWSStatus.ForeColor = Drawing.Color.Gray

            btnTMWSHealthCheck.Visible = True
        Else
            lblTMWSLink.Text = "(Not In Use)"
            lblTMWSLink.ForeColor = Drawing.Color.Gray
            lblTMWSLink.Font.Italic = True

            lblTMWSStatus.Text = "(Not In Use)"
            lblTMWSStatus.ForeColor = Drawing.Color.Gray
            lblTMWSStatus.Font.Italic = True

            btnTMWSHealthCheck.Visible = False
        End If
        ' CRE15-001 RSA Server Upgrade [End][Winnie]
    End Sub


    Private Sub RefreshSubTokenServer()
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strRSAAPIVersionSub As String = hfTSRSAVersion.Value

        ' Token Server (Sub)
        lblTSRSAVersion.Text = String.Format("RSA{0}", strRSAAPIVersionSub)
        lblTSLink.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}Link", strRSAAPIVersionSub))
        lblTSWebLogicUsername.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}WebLogicUsername", strRSAAPIVersionSub))
        lblTSAMUsername.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}AMUsername", strRSAAPIVersionSub))

        lblTSStatus.Text = "(Press Poll RSA Server)"
        lblTSStatus.ForeColor = Drawing.Color.Gray
        lblTSAMPasswordLastChange.Text = "(Press Poll RSA Server)"
        lblTSAMPasswordLastChange.ForeColor = Drawing.Color.Gray

        ' CRE15-001 RSA Server Upgrade [Start][Winnie]
        If udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}AgentMethod", strRSAAPIVersionSub)).Equals("WS") Then
            lblTSWSLink.Text = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}AgentWSLink", strRSAAPIVersionSub))

            lblTSWSStatus.Text = "(Press WS Health Check)"
            lblTSWSStatus.ForeColor = Drawing.Color.Gray

            btnTSWSHealthCheck.Visible = True
        Else
            lblTSWSLink.Text = "(Not In Use)"
            lblTSWSLink.ForeColor = Drawing.Color.Gray
            lblTSWSLink.Font.Italic = True

            lblTSWSStatus.Text = "(Not In Use)"
            lblTSWSStatus.ForeColor = Drawing.Color.Gray
            lblTSWSStatus.Font.Italic = True

            btnTSWSHealthCheck.Visible = False
        End If
        ' CRE15-001 RSA Server Upgrade [End][Winnie]
    End Sub

    Protected Sub btnBack_TokenServerControl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

    '

    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
    Private Function InitRSA(ByVal strRSAAPIVersion As String) As IdentitySourceDTO
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim udtGeneralFunction As New GeneralFunction

        Dim lstrRSALink As String = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}Link", strRSAAPIVersion))
        Dim lstrRSAWebLogicUsername As String = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}WebLogicUsername", strRSAAPIVersion))
        Dim lstrRSAAMUsername As String = udtInterfaceControlBLL.GetSystemParameter(String.Format("RSA{0}AMUsername", strRSAAPIVersion))

        Dim lstrRSAWebLogicPassword As String = String.Empty
        Dim lstrRSAAMPassword As String = String.Empty

        udtGeneralFunction.getSystemParameterPassword(String.Format("RSA{0}WebLogicPassword", strRSAAPIVersion), lstrRSAWebLogicPassword)
        udtGeneralFunction.getSystemParameterPassword(String.Format("RSA{0}AMPassword", strRSAAPIVersion), lstrRSAAMPassword)

        Dim conn As New SOAPCommandTarget(lstrRSALink, lstrRSAWebLogicUsername, lstrRSAWebLogicPassword)

        If conn.Login(lstrRSAAMUsername, lstrRSAAMPassword) = False Then
            Throw New Exception(String.Format("InterfaceControl.InitRSA: Unable to connect to RSA Server (RSAAPIVersion={0}, Link={1}, WebLogicUsername={2}, WebLogicPassword={3}, AMUsername={4}, AMPassword={5})", _
                                strRSAAPIVersion, lstrRSALink, lstrRSAWebLogicUsername, lstrRSAWebLogicPassword, lstrRSAAMUsername, lstrRSAAMPassword))
        End If

        CommandTargetPolicy.setDefaultCommandTarget(conn)

        Dim c1 As New GetIdentitySourcesCommand
        c1.execute()

        If c1.identitySources.Length <> 1 Then
            Throw New Exception(String.Format("InterfaceControl.InitRSA: Unexpected value (identitySources.Length={0})", c1.identitySources.Length))
        End If

        Return c1.identitySources(0)

    End Function

    Protected Sub btnTMPoll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim ludtIDSource As IdentitySourceDTO = InitRSA(hfTMRSAVersion.Value)

            Dim c1 As New SearchPrincipalsCommand

            c1.filter = Filter.equal(PrincipalDTO._LOGINUID, lblTMAMUsername.Text)
            c1.limit = 1
            c1.identitySourceGuid = ludtIDSource.guid
            c1.securityDomainGuid = ludtIDSource.securityDomainGuid

            c1.execute()

            lblTMStatus.Text = String.Format("OK &nbsp;({0})", DateTime.Now.ToString("HH:mm:ss"))
            lblTMStatus.ForeColor = Drawing.Color.Green

            If c1.principals.Length = 1 Then
                lblTMAMPasswordLastChange.Text = String.Format("{0}, &nbsp;{1} days ago", _
                                                               c1.principals(0).passwordChangeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), _
                                                               Math.Floor(DateTime.Now.Subtract(c1.principals(0).passwordChangeDate.Value).TotalDays))

                lblTMAMPasswordLastChange.ForeColor = Drawing.Color.Black

            Else
                lblTMAMPasswordLastChange.Text = "User not found"
                lblTMAMPasswordLastChange.ForeColor = Drawing.Color.Gray

            End If

        Catch ex As Exception
            lblTMStatus.Text = String.Format("Fail: {0} ({1})", ex.Message, DateTime.Now.ToString("HH:mm:ss"))
            lblTMStatus.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    Protected Sub btnTMChangePassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvTMAction.SetActiveView(vTMChangePassword)
    End Sub

    Protected Sub btnTMPConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If txtTMPPassword.Text <> txtTMPPasswordRetype.Text Then
                Throw New Exception("Two passwords do not match")
            End If

            ConfirmChangePassword(True)

        Catch ex As Exception
            lblTMStatus.Text = String.Format("Fail: {0} ({1})", ex.Message, DateTime.Now.ToString("HH:mm:ss"))
            lblTMStatus.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    Protected Sub btnTMPCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvTMAction.SetActiveView(vTMButton)
    End Sub
    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

    ' CRE15-001 RSA Server Upgrade [Start][Winnie]
    Protected Sub btnTMWSHealthCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)
            Dim ws As New AuthService

            'Set timeout to 5s
            ws.Timeout = 5000

            ws.Url = lblTMWSLink.Text

            Dim Status As String = ws.HealthCheck()

            If Status = "0" Then
                lblTMWSStatus.Text = String.Format("OK &nbsp;({0})", DateTime.Now.ToString("HH:mm:ss"))
                lblTMWSStatus.ForeColor = Drawing.Color.Green
            Else
                lblTMWSStatus.Text = String.Format("Fail &nbsp;({0})", DateTime.Now.ToString("HH:mm:ss"))
                lblTMWSStatus.ForeColor = Drawing.Color.Red
            End If

        Catch ex As Exception
            lblTMWSStatus.Text = String.Format("Fail: {0} ({1})", ex.Message, DateTime.Now.ToString("HH:mm:ss"))
            lblTMWSStatus.ForeColor = Drawing.Color.Red

        End Try

    End Sub
    ' CRE15-001 RSA Server Upgrade [End][Winnie]

    '
    Private Sub ConfirmChangePassword(ByVal pblnMain As Boolean)
        Try
            Dim udtDB As New Database

            Try
                Dim ludtIDSource As IdentitySourceDTO
                Dim strNewPassword As String
                Dim strUsername As String

                If pblnMain Then
                    ludtIDSource = InitRSA(hfTMRSAVersion.Value)
                    strNewPassword = txtTMPPassword.Text
                    strUsername = lblTMAMUsername.Text
                Else
                    ludtIDSource = InitRSA(hfTSRSAVersion.Value)
                    strNewPassword = txtTSPPassword.Text
                    strUsername = lblTSAMUsername.Text
                End If

                ' Update SystemParameters
                udtDB.BeginTransaction()

                Dim udtInterfaceControlBLL As New InterfaceControlBLL
                udtInterfaceControlBLL.UpdateSystemParametersPassword(String.Format("RSA{0}AMPassword", IIf(pblnMain, hfTMRSAVersion.Value, hfTSRSAVersion.Value)),
                                                                      String.Empty, strNewPassword, GetStaffIDFromSession, udtDB)

                ' Update RSA Server
                Dim c1 As New SearchPrincipalsCommand

                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, strUsername)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = ludtIDSource.guid
                c1.securityDomainGuid = ludtIDSource.securityDomainGuid

                c1.execute()

                If c1.principals.Length <> 1 Then
                    Throw New Exception("User ID not found")
                End If

                Dim p As PrincipalDTO = c1.principals(0)

                Dim c2 As New UpdatePrincipalCommand
                c2.identitySourceGuid = ludtIDSource.guid

                Dim u As New UpdatePrincipalDTO
                u.guid = p.guid

                ' Copy the rowVersion to satisfy optimistic locking requirements
                u.rowVersion = p.rowVersion

                ' collect all modifications here
                Dim lstM As New List(Of ModificationDTO)
                Dim m As ModificationDTO

                ' Password
                m = New ModificationDTO
                m.operation = ModificationDTO._REPLACE_ATTRIBUTE
                m.name = PrincipalDTO._PASSWORD
                m.values = New Object() {strNewPassword}
                lstM.Add(m)

                u.modifications = lstM.ToArray()
                c2.principalModification = u

                c2.execute()

                udtDB.CommitTransaction()

                AddClearCacheSystemParameter()

                ucClearCacheRequestView_TokenServerControl.RefreshClearCacheRequest()

                ' Audit Log
                Dim strDescription As String = String.Format("Change API Password: <New Password: {0}>", strNewPassword)
                udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_TOKENSERVER_CONTROL, "00002", strDescription)

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw

            End Try

            SetupTokenServerControl()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]

    Protected Sub btnTSPoll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim ludtIDSource As IdentitySourceDTO = InitRSA(hfTSRSAVersion.Value)

            Dim c1 As New SearchPrincipalsCommand

            c1.filter = Filter.equal(PrincipalDTO._LOGINUID, lblTSAMUsername.Text)
            c1.limit = 1
            c1.identitySourceGuid = ludtIDSource.guid
            c1.securityDomainGuid = ludtIDSource.securityDomainGuid

            c1.execute()

            lblTSStatus.Text = String.Format("OK &nbsp;({0})", DateTime.Now.ToString("HH:mm:ss"))
            lblTSStatus.ForeColor = Drawing.Color.Green

            If c1.principals.Length = 1 Then
                lblTSAMPasswordLastChange.Text = String.Format("{0}, &nbsp;{1} days ago", _
                                                               c1.principals(0).passwordChangeDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), _
                                                               Math.Floor(DateTime.Now.Subtract(c1.principals(0).passwordChangeDate.Value).TotalDays))

                lblTSAMPasswordLastChange.ForeColor = Drawing.Color.Black

            Else
                lblTSAMPasswordLastChange.Text = "User not found"
                lblTSAMPasswordLastChange.ForeColor = Drawing.Color.Gray

            End If

        Catch ex As Exception
            lblTSStatus.Text = String.Format("Fail: {0} ({1})", ex.Message, DateTime.Now.ToString("HH:mm:ss"))
            lblTSStatus.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    Protected Sub btnTSChangePassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvTSAction.SetActiveView(vTSChangePassword)
    End Sub

    Protected Sub btnTSPConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If txtTSPPassword.Text <> txtTSPPasswordRetype.Text Then
                Throw New Exception("Two passwords do not match")
            End If

            ConfirmChangePassword(False)

        Catch ex As Exception
            lblTSStatus.Text = String.Format("Fail: {0} ({1})", ex.Message, DateTime.Now.ToString("HH:mm:ss"))
            lblTSStatus.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    Protected Sub btnTSPCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvTSAction.SetActiveView(vTSButton)
    End Sub
    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

    ' CRE15-001 RSA Server Upgrade [Start][Winnie]
    Protected Sub btnTSWSHealthCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)
            Dim ws As New AuthService

            'Set timeout to 5s
            ws.Timeout = 5000

            ws.Url = lblTSWSLink.Text

            Dim Status As String = ws.HealthCheck()

            If Status = "0" Then
                lblTSWSStatus.Text = String.Format("OK &nbsp;({0})", DateTime.Now.ToString("HH:mm:ss"))
                lblTSWSStatus.ForeColor = Drawing.Color.Green
            Else
                lblTSWSStatus.Text = String.Format("Fail &nbsp;({0})", DateTime.Now.ToString("HH:mm:ss"))
                lblTSWSStatus.ForeColor = Drawing.Color.Red
            End If

        Catch ex As Exception
            lblTSWSStatus.Text = String.Format("Fail: {0} ({1})", ex.Message, DateTime.Now.ToString("HH:mm:ss"))
            lblTSWSStatus.ForeColor = Drawing.Color.Red

        End Try

    End Sub
    ' CRE15-001 RSA Server Upgrade [End][Winnie]
#End Region

#Region "eH[R] - [C]ontrol Sites"

    Private Sub SetupEHRControlSite()
        RefreshEHRSite(1)
        RefreshEHRSite(2)
        RefreshEnableHCSPFunctionStatus()

        ucClearCacheRequestView_eHRControl.RefreshClearCacheRequest()

        MultiViewRCDC1.ActiveViewIndex = ViewIndexEHRSite.Button
        MultiViewRCDC2.ActiveViewIndex = ViewIndexEHRSite.Button
        MultiViewRCT.ActiveViewIndex = ViewIndexRCT.Button
    End Sub

    Private Sub RefreshEHRSite(ByVal strDC As String)
        Dim udtGeneralFunction As New GeneralFunction
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim strStatus As String = String.Empty
        Dim strWSUsingLink As String = String.Empty
        Dim strWSStandbyLink As String = String.Empty
        Dim strVSUsingLink As String = String.Empty
        Dim strVSStandbyLink As String = String.Empty
        Dim strPrimarySite As String = String.Empty

        Dim lbl_Status As Label = CType(Me.FindControl(String.Format("lblRCDC{0}Status", strDC)), Label)
        Dim lbl_Primary As Label = CType(Me.FindControl(String.Format("lblRCDC{0}Primary", strDC)), Label)
        Dim lbl_WS_Using_Link As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_WS_Using_Link", strDC)), Label)
        Dim lbl_WS_Standby_Link As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_WS_Standby_Link", strDC)), Label)
        Dim lbl_VS_Using_Link As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_VS_Using_Link", strDC)), Label)
        Dim lbl_VS_Standby_Link As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_VS_Standby_Link", strDC)), Label)

        Dim lbl_WS_Using_Link_Result As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_WS_Using_Link_Result", strDC)), Label)
        Dim lbl_WS_Using_Link_ResultTime As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_WS_Using_Link_ResultTime", strDC)), Label)
        Dim lbl_WS_Standby_Link_Result As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_WS_Standby_Link_Result", strDC)), Label)
        Dim lbl_WS_Standby_Link_ResultTime As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_WS_Standby_Link_ResultTime", strDC)), Label)
        Dim lbl_VS_Using_Link_Result As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_VS_Using_Link_Result", strDC)), Label)
        Dim lbl_VS_Using_Link_ResultTime As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_VS_Using_Link_ResultTime", strDC)), Label)
        Dim lbl_VS_Standby_Link_Result As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_VS_Standby_Link_Result", strDC)), Label)
        Dim lbl_VS_Standby_Link_ResultTime As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_VS_Standby_Link_ResultTime", strDC)), Label)

        Dim btn_Resume As Button = CType(Me.FindControl(String.Format("btnRCDC{0}_Resume", strDC)), Button)
        Dim btn_Suspend As Button = CType(Me.FindControl(String.Format("btnRCDC{0}_Suspend", strDC)), Button)

        strPrimarySite = udtGeneralFunction.GetSystemVariableValue("eHRSS_PrimarySite")
        udtInterfaceControlBLL.GetSystemParameter(String.Format("eHRSS_WS_DC{0}_Enable", strDC), strStatus, String.Empty)
        udtInterfaceControlBLL.GetSystemParameter(String.Format("eHRSS_WS_GetEhrWebSLink_DC{0}", strDC), strWSUsingLink, strWSStandbyLink)
        udtInterfaceControlBLL.GetSystemParameter(String.Format("eHRSS_WS_VerifySystemLink_DC{0}", strDC), strVSUsingLink, strVSStandbyLink)

        Dim i As Integer = Integer.Parse(Regex.Replace(strPrimarySite, "[^\d]", ""))

        lbl_Primary.Visible = (strDC = i)
        lbl_Status.Text = strStatus
        lbl_WS_Using_Link.Text = strWSUsingLink
        lbl_WS_Standby_Link.Text = strWSStandbyLink
        lbl_VS_Using_Link.Text = strVSUsingLink
        lbl_VS_Standby_Link.Text = strVSStandbyLink
        lbl_WS_Using_Link_Result.Text = String.Empty
        lbl_WS_Using_Link_ResultTime.Text = String.Empty
        lbl_WS_Standby_Link_Result.Text = String.Empty
        lbl_WS_Standby_Link_ResultTime.Text = String.Empty
        lbl_VS_Using_Link_Result.Text = String.Empty
        lbl_VS_Using_Link_ResultTime.Text = String.Empty
        lbl_VS_Standby_Link_Result.Text = String.Empty
        lbl_VS_Standby_Link_ResultTime.Text = String.Empty

        btn_Resume.Visible = strStatus <> "Y"
        btn_Suspend.Visible = strStatus <> "N"
    End Sub

    Protected Sub btnRC_Switch_WS_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()

        Dim btnSwitch_WS = CType(sender, Button)
        Dim i As Integer = Integer.Parse(Regex.Replace(btnSwitch_WS.id, "[^\d]", ""))
        Dim mv_DC As MultiView = CType(Me.FindControl(String.Format("MultiViewRCDC{0}", i)), MultiView)

        Session(String.Format("ICW_RCDC{0}Action", i)) = EHRRCAction.WebService
        mv_DC.ActiveViewIndex = ViewIndexEHRSite.Confirm
    End Sub

    Protected Sub btnRC_Switch_VS_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()

        Dim btnSwitch_VS = CType(sender, Button)
        Dim i As Integer = Integer.Parse(Regex.Replace(btnSwitch_VS.id, "[^\d]", ""))
        Dim mv_DC As MultiView = CType(Me.FindControl(String.Format("MultiViewRCDC{0}", i)), MultiView)

        Session(String.Format("ICW_RCDC{0}Action", i)) = EHRRCAction.VerifySystem
        mv_DC.ActiveViewIndex = ViewIndexEHRSite.Confirm
    End Sub

    Protected Sub btnRC_Yes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()
        Dim strAuditLogDesc As String = String.Empty
        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        Dim btnYes = CType(sender, Button)
        Dim i As Integer = Integer.Parse(Regex.Replace(btnYes.id, "[^\d]", ""))
        Dim lbl_WS_Standby_Link As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_WS_Standby_Link", i)), Label)
        Dim lbl_VS_Standby_Link As Label = CType(Me.FindControl(String.Format("lblRCDC{0}_VS_Standby_Link", i)), Label)
        Dim mv_DC As MultiView = CType(Me.FindControl(String.Format("MultiViewRCDC{0}", i)), MultiView)
        Dim strAction As String = Session(String.Format("ICW_RCDC{0}Action", i))

        Select Case strAction
            Case EHRRCAction.WebService
                udtInterfaceControlBLL.SwitchEHRWebService(i, strStaffID)

                '' Audit Log
                strAuditLogDesc = String.Format("{0}: <New Using: ({1}) {2}>", _
                                        AuditLogDescription.SwitchWebService, _
                                        String.Format("eHRSS_WS_GetEhrWebSLink_DC{0}", i), lbl_WS_Standby_Link.Text)

                udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession(), FUNCT_CODE_EHR_CONTROLSITE, AuditLogDescription.SwitchWebService_ID, strAuditLogDesc)

            Case EHRRCAction.VerifySystem
                udtInterfaceControlBLL.SwitchEHRVerifySystem(i, strStaffID)

                '' Audit Log
                strAuditLogDesc = String.Format("{0}: <New Using: ({1}) {2}>", _
                                        AuditLogDescription.SwitchVerifySystem, _
                                        String.Format("eHRSS_WS_VerifySystemLink_DC{0}", i), lbl_VS_Standby_Link.Text)

                udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession(), FUNCT_CODE_EHR_CONTROLSITE, AuditLogDescription.SwitchVerifySystem_ID, strAuditLogDesc)

            Case EHRRCAction.ResumeServer, EHRRCAction.SuspendServer
                udtInterfaceControlBLL.UpdateSystemParametersValue(String.Format("eHRSS_WS_DC{0}_Enable", i), strAction, strStaffID)

                ' Audit Log
                Dim strDescription As String = String.Format("{0}: <New Status: {1} ({2})>", _
                                                             AuditLogDescription.ResumeSuspendEHRServer, _
                                                             strAction, String.Format("eHRSS_WS_DC{0}_Enable", i))
                udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EHR_CONTROLSITE, AuditLogDescription.ResumeSuspendEHRServer_ID, strDescription)
        End Select

        AddClearCacheSystemParameter()

        ucClearCacheRequestView_eHRControl.RefreshClearCacheRequest()

        RefreshEHRSite(i)

        mv_DC.ActiveViewIndex = ViewIndexEHRSite.Button
    End Sub

    Protected Sub btnRC_No_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()

        Dim btnNo = CType(sender, Button)
        Dim i As Integer = Integer.Parse(Regex.Replace(btnNo.id, "[^\d]", ""))
        Dim mv_DC As MultiView = CType(Me.FindControl(String.Format("MultiViewRCDC{0}", i)), MultiView)

        mv_DC.ActiveViewIndex = ViewIndexEHRSite.Button
    End Sub

    Protected Sub btnRC_Resume_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()

        Dim btnResume = CType(sender, Button)
        Dim i As Integer = Integer.Parse(Regex.Replace(btnResume.id, "[^\d]", ""))
        Dim mv_DC As MultiView = CType(Me.FindControl(String.Format("MultiViewRCDC{0}", i)), MultiView)

        Session(String.Format("ICW_RCDC{0}Action", i)) = EHRRCAction.ResumeServer
        mv_DC.ActiveViewIndex = ViewIndexEHRSite.Confirm
    End Sub

    Protected Sub btnRC_Suspend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()

        Dim btnSuspend = CType(sender, Button)
        Dim i As Integer = Integer.Parse(Regex.Replace(btnSuspend.id, "[^\d]", ""))
        Dim mv_DC As MultiView = CType(Me.FindControl(String.Format("MultiViewRCDC{0}", i)), MultiView)

        Session(String.Format("ICW_RCDC{0}Action", i)) = EHRRCAction.SuspendServer
        mv_DC.ActiveViewIndex = ViewIndexEHRSite.Confirm
    End Sub

    Protected Sub btnRC_Refresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshEHRSite(1)
        RefreshEHRSite(2)
    End Sub

    Protected Sub btnRC_Poll_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'PollEHRLink()
    End Sub

    ' Turn On / Suspend [T]oken Sharing Function in HCSP Platform

    Protected Sub btnRCTRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshEnableHCSPFunctionStatus()
    End Sub

    Protected Sub btnRCTTurnOn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.RCTAction) = "Y"
        MultiViewRCT.ActiveViewIndex = ViewIndexRCT.Confirm
    End Sub

    Protected Sub btnRCTSuspend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.RCTAction) = "N"
        MultiViewRCT.ActiveViewIndex = ViewIndexRCT.Confirm
    End Sub

    Protected Sub btnRCTYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.UpdateSystemParametersValue("eHRSS_EnableHCSPFunction", Session(SESS.RCTAction), strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <New Status: {1}>", AuditLogDescription.TurnOnSuspendEHRFunctionHCSP, Session(SESS.RCTAction))
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_EHR_CONTROLSITE, AuditLogDescription.TurnOnSuspendEHRFunctionHCSP_ID, strDescription)

        RefreshEnableHCSPFunctionStatus()

        ucClearCacheRequestView_eHRControl.RefreshClearCacheRequest()

        MultiViewRCT.ActiveViewIndex = ViewIndexRCT.Button

    End Sub

    Protected Sub btnRCTNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewRCT.ActiveViewIndex = ViewIndexRCT.Button
    End Sub

    Private Sub RefreshEnableHCSPFunctionStatus()
        Dim strStatus As String = (New InterfaceControlBLL).GetSystemParameter("eHRSS_EnableHCSPFunction")
        lblRCTCurrentStatus.Text = strStatus

        btnRCTTurnOn.Visible = strStatus <> "Y"
        btnRCTSuspend.Visible = strStatus <> "N"

    End Sub

    Protected Sub btnRCBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

    'Private Sub PollEHRLink()
    '    Dim aryLinkLabel As List(Of String) = New List(Of String)
    '    Dim lbl_Link As Label
    '    Dim lbl_Result As Label
    '    Dim lbl_ResultTime As Label

    '    aryLinkLabel.Add("lblRCDC1_WS_Using_Link")
    '    aryLinkLabel.Add("lblRCDC1_WS_Standby_Link")
    '    aryLinkLabel.Add("lblRCDC2_WS_Using_Link")
    '    aryLinkLabel.Add("lblRCDC2_WS_Standby_Link")

    '    aryLinkLabel.Add("lblRCDC1_VS_Using_Link")
    '    aryLinkLabel.Add("lblRCDC1_VS_Standby_Link")
    '    aryLinkLabel.Add("lblRCDC2_VS_Using_Link")
    '    aryLinkLabel.Add("lblRCDC2_VS_Standby_Link")

    '    InitServicePointManager()

    '    For Each LinkLabel As String In aryLinkLabel
    '        lbl_Link = CType(Me.FindControl(LinkLabel), Label)
    '        lbl_Result = CType(Me.FindControl(String.Format("{0}_Result", LinkLabel)), Label)
    '        lbl_ResultTime = CType(Me.FindControl(String.Format("{0}_ResultTime", LinkLabel)), Label)

    '        Try
    '            Dim Request As HttpWebRequest = WebRequest.Create(lbl_Link.Text)

    '            Dim Response As HttpWebResponse = Request.GetResponse()

    '            If (Response IsNot Nothing AndAlso Response.StatusCode <> HttpStatusCode.OK) Then
    '                lbl_Result.Text = "OK"
    '            Else
    '                lbl_Result.Text = "Fail: ""Return <> OK"""
    '            End If

    '            Response.Close()

    '        Catch ex As Exception
    '            lbl_Result.Text = String.Format("Fail: ""{0}""", ex.Message)
    '        End Try

    '        lbl_ResultTime.Text = String.Format("as at {0}", Date.Now.ToString("HH:mm:ss"))
    '    Next
    'End Sub

#End Region

#Region "eH[R] - [E]nquire eHR"

    Private Sub SetupEHREnquireEHR()
        Dim udtGeneralFunction As New GeneralFunction

        txtREMode.Text = udtGeneralFunction.getSystemParameterValue1("eHRSS_Mode")
        txtREPrimarySite.Text = udtGeneralFunction.GetSystemVariableValue("eHRSS_PrimarySite")
        txtREVPLink.Text = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_VerifySystemLink_{1}", txtREMode.Text, txtREPrimarySite.Text))
        txtREGetWebSLink.Text = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_GetEhrWebSLink_{1}", txtREMode.Text, txtREPrimarySite.Text))
        txtRESystemID.Text = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_SystemID", txtREMode.Text))
        txtRECertificationMark.Text = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_EHRCertificationMark", txtREMode.Text))
        txtREServiceCode.Text = udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_ServiceCode", txtREMode.Text))

        If txtREMode.Text = "EMULATE" Then
            txtREVPLink.Enabled = False
            txtREGetWebSLink.Enabled = False
            txtRESystemID.Enabled = False
            txtRECertificationMark.Enabled = False
            txtREServiceCode.Enabled = False
        Else
            txtREVPLink.Enabled = True
            txtREGetWebSLink.Enabled = True
            txtRESystemID.Enabled = True
            txtRECertificationMark.Enabled = True
            txtREServiceCode.Enabled = True
        End If

        lblREHealthCheckLastUpdate.Text = "Last update: --"
        lblREHealthCheckResult.Text = String.Empty

        lblREGetEHRTokenLastUpdate.Text = "Last update: --"
        txtRETSPID.Text = String.Empty
        txtRETHKID.Text = String.Empty
        chkRETMaskHKID.Checked = True
        lblREGetEHRToken.Text = String.Empty

        lblREGetEHRLoginAliasLastUpdate.Text = "Last update: --"
        txtRELSPID.Text = String.Empty
        txtRELHKID.Text = String.Empty
        chkRELMaskHKID.Checked = True
        lblREGetEHRLoginAliasResult.Text = String.Empty

    End Sub

    '

    Protected Sub btnREHealthCheckCall_Click(sender As Object, e As EventArgs)
        lblREHealthCheckLastUpdate.Text = String.Format("Last update: {0}", DateTime.Now.ToString("HH:mm:ss"))

        Try
            Dim udteHRServiceBLL As New eHRServiceBLL(strMode:=txtREMode.Text.Trim, strPrimarySite:=txtREPrimarySite.Text.Trim, _
                                                      strVPLink:=txtREVPLink.Text.Trim, strGetWebSLink:=txtREGetWebSLink.Text.Trim, _
                                                      strSysID:=txtRESystemID.Text.Trim, strEHRCertificationMark:=txtRECertificationMark.Text.Trim, _
                                                      strServiceCode:=txtREServiceCode.Text.Trim)

            'udteHRServiceBLL.CustomDBFlag = "DBFlagInterfaceLog"
            udteHRServiceBLL.CheckDBEnable = False
            udteHRServiceBLL.AutoResilience = False

            Dim udtInXml As InHealthCheckeHRSSXmlModel = udteHRServiceBLL.HealthCheckeHRSS

            lblREHealthCheckResult.Text = String.Format("Timestamp=<b>{0}</b><br>ResultCode=<b>{1}</b><br>ResultDescription=<b>{2}</b>", _
                                                        udtInXml.Timestamp, udtInXml.ResultCode, udtInXml.ResultDescription)
            lblREHealthCheckResult.ForeColor = Nothing

        Catch ex As Exception
            lblREHealthCheckResult.Text = ex.ToString
            lblREHealthCheckResult.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    Protected Sub btnREGetEHRToken_Click(sender As Object, e As EventArgs)
        lblREGetEHRTokenLastUpdate.Text = String.Format("Last update: {0}", DateTime.Now.ToString("HH:mm:ss"))

        If txtRETSPID.Text = String.Empty AndAlso txtRETHKID.Text = String.Empty Then
            lblREGetEHRToken.Text = "Please input one parameter"
            lblREGetEHRToken.ForeColor = Drawing.Color.Red

            Return

        End If

        If txtRETSPID.Text <> String.Empty AndAlso txtRETHKID.Text <> String.Empty Then
            lblREGetEHRToken.Text = "Please don't input both parameters"
            lblREGetEHRToken.ForeColor = Drawing.Color.Red

            Return

        End If

        ' Gather input
        Dim strHKID As String = String.Empty

        If txtRETSPID.Text <> String.Empty Then
            Try
                strHKID = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, txtRETSPID.Text.Trim).HKID

            Catch ex As Exception
                lblREGetEHRToken.Text = "Cannot find the SP"
                lblREGetEHRToken.ForeColor = Drawing.Color.Red

                Return

            End Try

        ElseIf txtRETHKID.Text <> String.Empty Then
            If (New Regex("^.{8,9}$")).IsMatch(txtRETHKID.Text.Trim) = False Then
                lblREGetEHRToken.Text = "Invalid HKID"
                lblREGetEHRToken.ForeColor = Drawing.Color.Red

                Return

            End If

            strHKID = (New Formatter).formatHKIDInternal(txtRETHKID.Text)

        End If

        Try
            Dim udteHRServiceBLL As New eHRServiceBLL(strMode:=txtREMode.Text.Trim, strPrimarySite:=txtREPrimarySite.Text.Trim, _
                                                      strVPLink:=txtREVPLink.Text.Trim, strGetWebSLink:=txtREGetWebSLink.Text.Trim, _
                                                      strSysID:=txtRESystemID.Text.Trim, strEHRCertificationMark:=txtRECertificationMark.Text.Trim, _
                                                      strServiceCode:=txtREServiceCode.Text.Trim)

            'udteHRServiceBLL.CustomDBFlag = "DBFlagInterfaceLog"
            udteHRServiceBLL.CheckDBEnable = False
            udteHRServiceBLL.AutoResilience = False

            Dim udtInXml As InGeteHRTokenInfoXmlModel = udteHRServiceBLL.GeteHRSSTokenInfo(strHKID)

            Dim lstResult As New List(Of String)
            lstResult.Add(String.Format("Timestamp=<b>{0}</b>", udtInXml.Timestamp))
            lstResult.Add(String.Format("ResultCode=<b>{0}</b>", udtInXml.ResultCode))
            lstResult.Add(String.Format("ResultDescription=<b>{0}</b>", udtInXml.ResultDescription))

            If chkRETMaskHKID.Checked Then
                lstResult.Add(String.Format("HKID=<b>{0}{1}</b>", udtInXml.HKID.Substring(0, 4), Strings.StrDup(udtInXml.HKID.Length - 4, "x")))
            Else
                lstResult.Add(String.Format("HKID=<b>{0}</b>", udtInXml.HKID))
            End If

            If udtInXml.ResultCodeEnum = eHRResultCode.R1000_Success Then
                lstResult.Add(String.Format("IsCommonUser=<b>{0}</b>", udtInXml.IsCommonUser))
                lstResult.Add(String.Format("ExistingTokenID=<b>{0}</b>", udtInXml.ExistingTokenID))
                lstResult.Add(String.Format("ExistingTokenIssuer=<b>{0}</b>", udtInXml.ExistingTokenIssuer))
                lstResult.Add(String.Format("IsExistingTokenShared=<b>{0}</b>", udtInXml.IsExistingTokenShared))
                lstResult.Add(String.Format("NewTokenID=<b>{0}</b>", udtInXml.NewTokenID))
                lstResult.Add(String.Format("NewTokenIssuer=<b>{0}</b>", udtInXml.NewTokenIssuer))
                lstResult.Add(String.Format("IsNewTokenShared=<b>{0}</b>", udtInXml.IsNewTokenShared))
            End If

            lblREGetEHRToken.Text = String.Join("<br>", lstResult.ToArray)
            lblREGetEHRToken.ForeColor = Nothing

        Catch ex As Exception
            lblREGetEHRToken.Text = ex.ToString
            lblREGetEHRToken.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    Protected Sub btnREGetEHRLoginAlias_Click(sender As Object, e As EventArgs)
        lblREGetEHRLoginAliasLastUpdate.Text = String.Format("Last update: {0}", DateTime.Now.ToString("HH:mm:ss"))

        If txtRELSPID.Text = String.Empty AndAlso txtRELHKID.Text = String.Empty Then
            lblREGetEHRLoginAliasResult.Text = "Please input one parameter"
            lblREGetEHRLoginAliasResult.ForeColor = Drawing.Color.Red

            Return

        End If

        If txtRELSPID.Text <> String.Empty AndAlso txtRELHKID.Text <> String.Empty Then
            lblREGetEHRLoginAliasResult.Text = "Please don't input both parameters"
            lblREGetEHRLoginAliasResult.ForeColor = Drawing.Color.Red

            Return

        End If

        ' Gather input
        Dim strHKID As String = String.Empty

        If txtRELSPID.Text <> String.Empty Then
            Try
                strHKID = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, txtRELSPID.Text.Trim).HKID

            Catch ex As Exception
                lblREGetEHRLoginAliasResult.Text = "Cannot find the SP"
                lblREGetEHRLoginAliasResult.ForeColor = Drawing.Color.Red

                Return

            End Try

        ElseIf txtRELHKID.Text <> String.Empty Then
            If (New Regex("^.{8,9}$")).IsMatch(txtRELHKID.Text.Trim) = False Then
                lblREGetEHRLoginAliasResult.Text = "Invalid HKID"
                lblREGetEHRLoginAliasResult.ForeColor = Drawing.Color.Red

                Return

            End If

            strHKID = (New Formatter).formatHKIDInternal(txtRELHKID.Text)

        End If

        Try
            Dim udteHRServiceBLL As New eHRServiceBLL(strMode:=txtREMode.Text.Trim, strPrimarySite:=txtREPrimarySite.Text.Trim, _
                                                      strVPLink:=txtREVPLink.Text.Trim, strGetWebSLink:=txtREGetWebSLink.Text.Trim, _
                                                      strSysID:=txtRESystemID.Text.Trim, strEHRCertificationMark:=txtRECertificationMark.Text.Trim, _
                                                      strServiceCode:=txtREServiceCode.Text.Trim)

            'udteHRServiceBLL.CustomDBFlag = "DBFlagInterfaceLog"
            udteHRServiceBLL.CheckDBEnable = False
            udteHRServiceBLL.AutoResilience = False

            Dim udtInXml As InGeteHRSSLoginAliasXmlModel = udteHRServiceBLL.GeteHRSSLoginAlias(strHKID)

            Dim lstResult As New List(Of String)
            lstResult.Add(String.Format("Timestamp=<b>{0}</b>", udtInXml.Timestamp))
            lstResult.Add(String.Format("ResultCode=<b>{0}</b>", udtInXml.ResultCode))
            lstResult.Add(String.Format("ResultDescription=<b>{0}</b>", udtInXml.ResultDescription))

            If chkRELMaskHKID.Checked Then
                lstResult.Add(String.Format("HKID=<b>{0}{1}</b>", udtInXml.HKID.Substring(0, 4), Strings.StrDup(udtInXml.HKID.Length - 4, "x")))
            Else
                lstResult.Add(String.Format("HKID=<b>{0}</b>", udtInXml.HKID))
            End If

            If udtInXml.ResultCodeEnum = eHRResultCode.R1000_Success Then
                lstResult.Add(String.Format("LoginAlias=<b>{0}</b>", udtInXml.LoginAlias))
            End If

            lblREGetEHRLoginAliasResult.Text = String.Join("<br>", lstResult.ToArray)
            lblREGetEHRLoginAliasResult.ForeColor = Nothing

        Catch ex As Exception
            lblREGetEHRLoginAliasResult.Text = ex.ToString
            lblREGetEHRLoginAliasResult.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    '

    Protected Sub btnREBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

#End Region

#Region "eH[R] - [T]race Outsync Records"

    Private Sub SetupEHRTraceOutsyncRecord()
        txtRTFrom.Text = String.Empty
        txtRTTo.Text = String.Empty

        lblRTError.Visible = False
        gvRT.Visible = False

    End Sub

    '

    Protected Sub btnRTSearch_Click(sender As Object, e As EventArgs) Handles btnRTSearch.Click
        lblRTError.Visible = False
        gvRT.Visible = False

        ' --- Vaildation ---
        If txtRTFrom.Text.Trim = String.Empty Then
            lblRTError.Text = "Please input all fields"
            lblRTError.Visible = True

            Return

        End If

        If txtRTTo.Text.Trim = String.Empty Then
            lblRTError.Text = "Please input all fields"
            lblRTError.Visible = True

            Return

        End If
        ' --- End of Vaildation ---

        Dim dt As DataTable = (New InterfaceControlBLL).GetOutsyncRecord(txtRTFrom.Text.Trim, txtRTTo.Text.Trim)

        If dt.Rows.Count = 0 Then
            lblRTError.Text = String.Format("No record found from {0} to {1}", txtRTFrom.Text.Trim, txtRTTo.Text.Trim)
            lblRTError.Visible = True

        Else
            dt.Columns.Add("Set_Share_Action_Dtm_String", GetType(String))
            dt.Columns.Add("Set_Share_Notification_Dtm_String", GetType(String))
            dt.Columns.Add("Suspicious_EHS_Action_Dtm_String", GetType(String))

            Dim udtStaticDataBLL As New StaticDataBLL

            For Each dr As DataRow In dt.Rows
                dr("Set_Share_Action_Dtm_String") = DirectCast(dr("Set_Share_Action_Dtm"), DateTime).ToString("yyyy-MM-dd HH:mm:ss")
                dr("Set_Share_Notification_Dtm_String") = DirectCast(dr("Set_Share_Notification_Dtm"), DateTime).ToString("yyyy-MM-dd HH:mm:ss")
                dr("Suspicious_EHS_Action_Dtm_String") = DirectCast(dr("Suspicious_EHS_Action_Dtm"), DateTime).ToString("yyyy-MM-dd HH:mm:ss")

                If dr("Suspicious_EHS_Action").ToString.Trim = "REPLACETOKEN" Then
                    dr("Action_Remark") = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_REPLACE_REASON", dr("Action_Remark")).DataValue

                ElseIf dr("Suspicious_EHS_Action").ToString.Trim = "DELETETOKEN" Then
                    dr("Action_Remark") = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", dr("Action_Remark")).DataValue

                End If

                If IsDBNull(dr("Token_Serial_No_Replacement")) Then
                    dr("Token_Serial_No_Replacement") = "--"
                End If

            Next

            gvRT.DataSource = dt
            gvRT.DataBind()

            gvRT.Visible = True

        End If

    End Sub

    Protected Sub lbtnRTLastOneDay_Click(sender As Object, e As EventArgs)
        Dim dtmNow As DateTime = DateTime.Today

        txtRTFrom.Text = dtmNow.AddDays(-1).ToString("yyyy-MM-dd HH:mm")
        txtRTTo.Text = dtmNow.ToString("yyyy-MM-dd HH:mm")

    End Sub

    Protected Sub lbtnRTLast24Hour_Click(sender As Object, e As EventArgs)
        Dim dtmNow As DateTime = DateTime.Now

        txtRTFrom.Text = dtmNow.AddDays(-1).ToString("yyyy-MM-dd HH:mm")
        txtRTTo.Text = dtmNow.ToString("yyyy-MM-dd HH:mm")

    End Sub

    Protected Sub lbtnRTLast1Hour_Click(sender As Object, e As EventArgs)
        Dim dtmNow As DateTime = DateTime.Now

        txtRTFrom.Text = dtmNow.AddHours(-1).ToString("yyyy-MM-dd HH:mm")
        txtRTTo.Text = dtmNow.ToString("yyyy-MM-dd HH:mm")

    End Sub

    '

    Protected Sub btnRTBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

#End Region

#Region "[O]CSSS - [C]ontrol Sites"

    Private Sub SetupOCSSSControlSite()
        RefreshTurnOnOCSSSStatus()
        RefreshOCSSSLink()

        ucClearCacheRequestView_OCSSSControl.RefreshClearCacheRequest()

        MultiViewOCL.ActiveViewIndex = ViewIndexOCL.Button
        MultiViewOCT.ActiveViewIndex = ViewIndexOCT.Button
    End Sub

    ' Switch OCSSS [L]ink

    Protected Sub btnOCLRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshOCSSSLink()
    End Sub

    Protected Sub btnOCLSwitch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewOCL.ActiveViewIndex = ViewIndexOCL.Confirm
    End Sub

    Protected Sub btnOCLYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        udtInterfaceControlBLL.SwitchOCSSSLink(strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strUsingLink As String = String.Empty
        
        udtInterfaceControlBLL.GetSystemParameter("OCSSS_WS_Link", strUsingLink, Nothing)

        Dim strDescription As String = String.Format("{0}: <New Using: {1}>", AuditLogDescription.SwitchOCSSSLink, strUsingLink)

        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_OCSSS_CONTROLSITE, AuditLogDescription.SwitchOCSSSLink_ID, strDescription)

        ucClearCacheRequestView_OCSSSControl.RefreshClearCacheRequest()

        RefreshOCSSSLink()
        MultiViewOCL.ActiveViewIndex = ViewIndexOCL.Button

    End Sub

    Protected Sub btnOCLNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewOCL.ActiveViewIndex = ViewIndexOCL.Button
    End Sub

  Private Sub RefreshOCSSSLink()
        Dim strUsingLink As String = String.Empty
        Dim strStandbyLink As String = String.Empty
        
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        
        udtInterfaceControlBLL.GetSystemParameter("OCSSS_WS_Link", strUsingLink, strStandbyLink)
        
        lblOCLCurrentUsing.Text = strUsingLink

        If strStandbyLink <> String.Empty Then            
            lblOCLCurrentStandby.Text = strStandbyLink
            btnOCLSwitch.Visible = True
        Else            
            lblOCLCurrentStandby.Text = "(Not In Use)"
            btnOCLSwitch.Visible = False
        End If

    End Sub


    ' [T]urn On / Suspend OCSSS Function in HCSP Platform

    Protected Sub btnOCTRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        RefreshTurnOnOCSSSStatus()
    End Sub

    Protected Sub btnOCTTurnOn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.OCTAction) = "Y"
        MultiViewOCT.ActiveViewIndex = ViewIndexOCT.Confirm
    End Sub

    Protected Sub btnOCTSuspend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        Session(SESS.OCTAction) = "N"
        MultiViewOCT.ActiveViewIndex = ViewIndexOCT.Confirm
    End Sub

    Protected Sub btnOCTYes_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strStaffID As String = GetStaffIDFromSession()

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.UpdateSystemParametersValue(CONST_SYS_PARAM_TurnOnOCSSS, Session(SESS.OCTAction), strStaffID)

        AddClearCacheSystemParameter()

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <New Status: {1}>", AuditLogDescription.TurnOnSuspendOCSSSFunctionHCSP, Session(SESS.OCTAction))
        udtInterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_OCSSS_CONTROLSITE, AuditLogDescription.TurnOnSuspendOCSSSFunctionHCSP_ID, strDescription)

        RefreshTurnOnOCSSSStatus()

        ucClearCacheRequestView_OCSSSControl.RefreshClearCacheRequest()

        MultiViewOCT.ActiveViewIndex = ViewIndexOCT.Button
    End Sub

    Protected Sub btnOCTNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        GetStaffIDFromSession()
        MultiViewOCT.ActiveViewIndex = ViewIndexOCT.Button
    End Sub

    Private Sub RefreshTurnOnOCSSSStatus()
        Dim strStatus As String = (New InterfaceControlBLL).GetSystemParameter(CONST_SYS_PARAM_TurnOnOCSSS)
        lblOCTCurrentStatus.Text = strStatus

        btnOCTTurnOn.Visible = strStatus <> "Y"
        btnOCTSuspend.Visible = strStatus <> "N"

    End Sub

    Protected Sub btnOCBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub
#End Region

#Region "[O]CSSS - [E]nquire OCSSS"

    Private Sub SetupOCSSSEnquireOCSSS()
        Dim udtGeneralFunction As New GeneralFunction

        txtOEWSLink.Text = udtGeneralFunction.getSystemParameterValue1(CONST_SYS_PARAM_OCSSS_WS_Link)
        txtOEPassPhrase.Text = udtGeneralFunction.getSystemParameterValue1(CONST_SYS_PARAM_OCSSS_WS_PassPhrase)

        lblOEHealthCheckLastUpdate.Text = "Last update: --"
        lblOEHealthCheckResult.Text = String.Empty

        lblOECheckResidentialStatusLastUpdate.Text = "Last update: --"
        txtOEHKID.Text = String.Empty
        chkOEMaskHKID.Checked = True
        lblOECheckResidentialStatusResult.Text = String.Empty
    End Sub

    '

    Protected Sub btnOEHealthCheckCall_Click(sender As Object, e As EventArgs)
        lblOEHealthCheckLastUpdate.Text = String.Format("Last update: {0}", DateTime.Now.ToString("HH:mm:ss"))

        Try
            Dim udtOCSSSServiceBLL As New OCSSSServiceBLL()
            Dim udtOCSSSResult As OCSSSResult = udtOCSSSServiceBLL.HealthCheck(txtOEWSLink.Text.Trim, txtOEPassPhrase.Text.Trim)

            lblOEHealthCheckResult.Text = String.Format("ConnectionStatus=<b>{0}</b>", udtOCSSSResult.ConnectionStatus)
            lblOEHealthCheckResult.ForeColor = Nothing

            If udtOCSSSResult.Exception IsNot Nothing Then
                lblOEHealthCheckResult.Text = udtOCSSSResult.Exception.ToString
                lblOEHealthCheckResult.ForeColor = Drawing.Color.Red
            End If

        Catch ex As Exception
            lblOEHealthCheckResult.Text = ex.ToString
            lblOEHealthCheckResult.ForeColor = Drawing.Color.Red

        End Try
    End Sub
    
    Protected Sub btnOECheckResidentialStatus_Click(sender As Object, e As EventArgs)
        lblOECheckResidentialStatusLastUpdate.Text = String.Format("Last update: {0}", DateTime.Now.ToString("HH:mm:ss"))

        If txtOEHKID.Text = String.Empty Then
            lblOECheckResidentialStatusResult.Text = "Please input HKID"
            lblOECheckResidentialStatusResult.ForeColor = Drawing.Color.Red

            Return
        End If

        Dim strHKID As String = String.Empty

        If txtOEHKID.Text <> String.Empty Then
            If (New Regex("^.{8,9}$")).IsMatch(txtOEHKID.Text.Trim) = False Then
                lblOECheckResidentialStatusResult.Text = "Invalid HKID"
                lblOECheckResidentialStatusResult.ForeColor = Drawing.Color.Red

                Return
            End If

            strHKID = (New Formatter).formatHKIDInternal(txtOEHKID.Text)

        End If

        Try
            Dim udtOCSSSServiceBLL As New OCSSSServiceBLL()
            Dim udtOCSSSResult As OCSSSResult = udtOCSSSServiceBLL.IsEligibleForInternalUse(txtOEWSLink.Text.Trim, txtOEPassPhrase.Text.Trim, strHKID)

            Dim lstResult As New List(Of String)
            lstResult.Add(String.Format("ConnectionStatus=<b>{0}</b>", udtOCSSSResult.ConnectionStatus))

            If chkOEMaskHKID.Checked Then
                lstResult.Add(String.Format("HKID=<b>{0}{1}</b>", strHKID.Substring(0, 4), Strings.StrDup(strHKID.Length - 4, "x")))
            Else
                lstResult.Add(String.Format("HKID=<b>{0}</b>", strHKID))
            End If

            lstResult.Add(String.Format("EligibleResult=<b>{0}</b>", udtOCSSSResult.EligibleResult))
            lblOECheckResidentialStatusResult.Text = String.Join("<br>", lstResult.ToArray)
            lblOECheckResidentialStatusResult.ForeColor = Nothing

            If udtOCSSSResult.Exception IsNot Nothing Then
                lblOECheckResidentialStatusResult.Text = udtOCSSSResult.Exception.ToString
                lblOECheckResidentialStatusResult.ForeColor = Drawing.Color.Red
            End If

        Catch ex As Exception
            lblOECheckResidentialStatusResult.Text = ex.ToString
            lblOECheckResidentialStatusResult.ForeColor = Drawing.Color.Red

        End Try

    End Sub

    '
    Protected Sub btnOEBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

#End Region

#Region "View 7: Download Report and Data File"

    Protected Sub btnDRDownloadFile_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblDRError.Text = String.Empty

        Dim strDescription As String = String.Format("Download File: <Generation ID: {0}>", txtDRGenerationID.Text)
        Call (New InterfaceControlBLL).AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE_DOWNLOAD_REPORT, "00001", strDescription)

        ' --- Validation ---

        If txtDRGenerationID.Text.Trim = String.Empty Then
            lblDRError.Text = "Generation ID cannot be empty!"
            Return
        End If

        If txtDRExcelPassword.Text.Trim = String.Empty Then
            lblDRError.Text = "Password for Excel cannot be empty!"
            Return
        End If

        ' --- End of Validation ---

        Dim udtFileGenerationQueue As FileGenerationQueueModel = (New FileGenerationBLL).GetFileContent(txtDRGenerationID.Text.Trim)

        If IsNothing(udtFileGenerationQueue) Then
            lblDRError.Text = "Cannot find the file. Make sure the Generation ID exists in both dbEVS and dbEVS_File."
            Return
        End If

        If Not udtFileGenerationQueue.OutputFile.EndsWith("xls") AndAlso Not udtFileGenerationQueue.OutputFile.EndsWith("xlsx") Then
            lblDRError.Text = "File is not in Excel format."
            Return
        End If

        Dim udtGeneralFunction As New GeneralFunction

        Dim strFileFullPath As String = String.Empty
        Dim strTempFolder As String = Path.Combine(udtGeneralFunction.getSystemParameterValue1("GeneralFileStoragePath"), _
                                                   udtGeneralFunction.generateTempFolderPath(Session.SessionID))

        Try
            If Not Directory.Exists(strTempFolder) Then
                Directory.CreateDirectory(strTempFolder)
            End If

            strFileFullPath = Path.Combine(strTempFolder, udtFileGenerationQueue.OutputFile)

            File.WriteAllBytes(strFileFullPath, udtFileGenerationQueue.FileContent)

            Encrypt.Excel_ChangePassword(udtFileGenerationQueue.FilePassword, txtDRExcelPassword.Text, strFileFullPath)

            Dim f As New FileInfo(strFileFullPath)

            Response.Clear()
            Response.AddHeader("Content-Disposition", "attachment; filename=" & f.Name)
            Response.AddHeader("Content-Length", f.Length.ToString())
            Response.ContentType = "application/file"
            Response.WriteFile(f.FullName)
            Response.Flush()
            Response.Close()

        Catch ex As Exception
            Throw

        Finally
            ' Delete the folder afterwards
            If strFileFullPath <> String.Empty AndAlso File.Exists(strFileFullPath) Then File.Delete(strFileFullPath)
            If strTempFolder <> String.Empty AndAlso Directory.Exists(strTempFolder) Then Directory.Delete(strTempFolder)

        End Try

    End Sub

    Protected Sub btnDRBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        MultiViewCore.SetActiveView(ViewMenu)
    End Sub

#End Region

#Region "Webservice function"
    Private Function GetRequestXml() As XmlDocument
        Dim strRequestXMLPath As String = System.Configuration.ConfigurationManager.AppSettings("RequestXML")
        Dim xmlRequest As New XmlDocument
        Dim lstNode As XmlNodeList = Nothing
        Dim strMessageID As String = String.Empty

        xmlRequest.Load(strRequestXMLPath)

        ' CRE10-035
        ' Assign new message ID to request xml
        ' -------------------------------------------------------
        strMessageID = (New GeneralFunction).generateEVaccineMessageID()
        lstNode = xmlRequest.SelectNodes("/parameter/message_id")
        If lstNode IsNot Nothing AndAlso lstNode.Count > 0 Then
            lstNode(0).InnerText = strMessageID
        End If

        Return xmlRequest

    End Function
#End Region

#Region "Webservice function (CRE11-002: Obsolete)"

    ''' <summary>
    ''' INT11-0019
    ''' Fix missing Certificate Validation after CRE11-002
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitServicePointManager()
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
    End Sub

    ''' <summary>
    ''' INT11-0019
    ''' Fix missing Certificate Validation after CRE11-002
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="certificate"></param>
    ''' <param name="chain"></param>
    ''' <param name="sslPolicyErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' Return True to force the certificate to be accepted
        Return True
    End Function

    '''' <summary>
    '''' Common function for create web service to process request
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Private Function CreateWebServiceEndPoint(ByVal iParmValue As Integer) As ImmuEnquiryWebS_EndPoint
    '    Dim ws As ImmuEnquiryWebS_EndPoint = New ImmuEnquiryWebS_EndPoint
    '    If iParmValue = 1 Then
    '        ws.Url = GetWSUrl1()
    '    Else
    '        ws.Url = GetWSUrl2()
    '    End If

    '    Dim strCMS_Get_Vaccine_WS_Use_Proxy As String = String.Empty

    '    ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
    '    Dim strUserName As String = GetWSUsername()
    '    Dim strPassword As String = GetWSPassword()
    '    If strUserName <> String.Empty And strPassword <> String.Empty Then
    '        Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
    '        Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
    '        oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(strUserName, strPassword)
    '        oClientPolicy.Assertions.Add(oAssertion)
    '        ws.SetPolicy(oClientPolicy)
    '        ' TODO: Add Systerm Parameter to config enable proxy on production/disable on testing site

    '        strCMS_Get_Vaccine_WS_Use_Proxy = (New InterfaceControlBLL).GetSystemParameter("CMS_Get_Vaccine_WS_Use_Proxy")
    '        If strCMS_Get_Vaccine_WS_Use_Proxy = "Y" Then
    '            ws.Proxy = New System.Net.WebProxy()
    '        End If
    '    End If

    '    ws.Timeout = GetWSTimeout()
    '    Return ws
    'End Function

    'Private Const SYS_PARAM_URL As String = "CMS_Get_Vaccine_WS_Url"
    'Private Const SYS_PARAM_USERNAME As String = "CMS_Get_Vaccine_WS_Username"
    'Private Const SYS_PARAM_PASSWORD As String = "CMS_Get_Vaccine_WS_Password"
    'Private Const SYS_PARAM_TIMEOUT As String = "CMS_Get_Vaccine_WS_TimeLimit"

    'Private Function GetWSUrl1() As String
    '    Return (New InterfaceControlBLL).GetSystemParameter(SYS_PARAM_URL)
    'End Function

    'Private Function GetWSUrl2() As String
    '    Dim strUrl As String = String.Empty
    '    Dim udtInterfaceControlBLL As New InterfaceControlBLL
    '    udtInterfaceControlBLL.GetSystemParameter(SYS_PARAM_URL, String.Empty, strUrl)
    '    Return strUrl
    'End Function

    'Private Function GetWSUsername() As String
    '    Return (New InterfaceControlBLL).GetSystemParameter(SYS_PARAM_USERNAME)
    'End Function

    'Private Function GetWSPassword() As String
    '    Return (New InterfaceControlBLL).GetSystemParameter(SYS_PARAM_PASSWORD)
    'End Function

    'Private Function GetWSTimeout() As Integer
    '    Return CInt((New InterfaceControlBLL).GetSystemParameter(SYS_PARAM_TIMEOUT)) * 1000
    'End Function

#End Region

#Region "Supporting function"

    Private Function GetStaffIDFromSession() As String
        If IsNothing(Session(SESS.StaffID)) Then
            Session.Clear()
            MultiViewCore.SetActiveView(ViewLogin)

            Throw New Exception("Session Expired")

        End If

        Return Session(SESS.StaffID).ToString

    End Function

    Private Function FormatDateTime(ByVal strDateTime As String)
        Dim dtmDateTime As Date = CDate(strDateTime.Trim)
        Return String.Format("{0} &nbsp; {1}", dtmDateTime.ToString("dd MMM yyyy"), dtmDateTime.ToString("HH:mm:ss"))
    End Function

    Private Function GetLastUpdate() As String
        Return String.Format("Last update: {0}", Date.Now.ToString("HH:mm:ss"))
    End Function

    Private Sub AddClearCacheSystemParameter()
        Dim udtInterfaceControlBLL As New InterfaceControlBLL

        ' Insert clear cache
        Dim aryApplicationServer As String() = ConfigurationManager.AppSettings("ClearCacheApplicationServer").Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

        For Each strApplicationServer As String In aryApplicationServer
            udtInterfaceControlBLL.AddClearCache(strApplicationServer, "SystemParameters")
        Next
    End Sub

    ' CMS
    ' CRE11-002
    Private Function GetUsingEndpoint() As EndpointEnum
        Dim strUsingMode As String = GetUsingEndpointValue()
        Return [Enum].Parse(GetType(EndpointEnum), strUsingMode)
    End Function

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
    ' ----------------------------------------------------------
    Private Function GetUsingEndpointValue() As String
        Return GetCMSEndpointValue(True)
    End Function

    Private Function GetCMSStandbyEndpointValue() As String
        Return GetCMSEndpointValue(False)
    End Function

    Private Function GetCMSEndpointValue(ByVal blnInUse As String) As String
        Dim strUsingMode As String = String.Empty
        Dim strStandbyMode As String = String.Empty

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.GetSystemParameter("CMS_Get_Vaccine_WS_Endpoint", strUsingMode, strStandbyMode)

        If blnInUse Then
            Return strUsingMode
        Else
            Return strStandbyMode
        End If
    End Function
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]    

    ''' <summary>
    ''' CRE11-002
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUsingEndpointURLListIgnoreServerCache() As Collection
        Dim strUsingMode As String = String.Empty
        Dim enumUsingEndPoint As EndpointEnum = GetUsingEndpoint()

        Dim strUsingLink As String = String.Empty
        Dim intCount As Integer
        Dim strStandbyLink() As String = Nothing

        Dim udtGenFunc As New Common.ComFunction.GeneralFunction
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim cllnURL As New Collection

        strUsingMode = GetUsingEndpointValue()

        udtInterfaceControlBLL.GetSystemParameter(String.Format("CMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, Nothing)

        udtGenFunc.GetInternalSystemParameterByLikeClause(String.Format("CMS_Get_Vaccine_WS_{0}_Url%[0-9]", strUsingMode), Nothing, strStandbyLink, Nothing)


        cllnURL.Add(strUsingLink)

        If Not strStandbyLink Is Nothing AndAlso strStandbyLink.Length > 0 Then
            For intCount = 0 To strStandbyLink.Length - 1
                If strUsingLink.Trim <> strStandbyLink(intCount).Trim Then
                    cllnURL.Add(strStandbyLink(intCount).Trim)
                End If
            Next
        End If

        Return cllnURL
    End Function

    '

    ' CIMS

    Private Function GetCIMSUsingEndpoint() As CIMSEndpoint
        Dim strUsingMode As String = GetCIMSUsingEndpointValue()
        Return [Enum].Parse(GetType(CIMSEndpoint), strUsingMode)
    End Function

    Private Function GetCIMSUsingEndpointValue() As String
        Return GetCIMSEndpointValue(True)
    End Function

    Private Function GetCIMSStandbyEndpointValue() As String
        Return GetCIMSEndpointValue(False)
    End Function

    Private Function GetCIMSEndpointValue(ByVal blnInUse As String) As String
        Dim strUsingMode As String = String.Empty
        Dim strStandbyMode As String = String.Empty

        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        udtInterfaceControlBLL.GetSystemParameter("CIMS_Get_Vaccine_WS_Endpoint", strUsingMode, strStandbyMode)

        If blnInUse Then
            Return strUsingMode
        Else
            Return strStandbyMode
        End If
    End Function

    Private Function GetCIMSUsingEndpointURLList() As Collection
        Dim strUsingMode As String = String.Empty
        Dim strUsingLink As String = String.Empty
        Dim intCount As Integer
        Dim strStandbyLink() As String = Nothing

        Dim udtGenFunc As New Common.ComFunction.GeneralFunction
        Dim udtInterfaceControlBLL As New InterfaceControlBLL
        Dim cllnURL As New Collection

        strUsingMode = GetCIMSUsingEndpointValue()

        udtInterfaceControlBLL.GetSystemParameter(String.Format("CIMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, Nothing)

        udtGenFunc.GetInternalSystemParameterByLikeClause(String.Format("CIMS_Get_Vaccine_WS_{0}_Url%[0-9]", strUsingMode), Nothing, strStandbyLink, Nothing)


        cllnURL.Add(strUsingLink)

        If Not strStandbyLink Is Nothing AndAlso strStandbyLink.Length > 0 Then
            For intCount = 0 To strStandbyLink.Length - 1
                If strUsingLink.Trim <> strStandbyLink(intCount).Trim Then
                    cllnURL.Add(strStandbyLink(intCount).Trim)
                End If
            Next
        End If

        Return cllnURL
    End Function


    '

    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
    Private Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function
    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

#End Region


End Class
