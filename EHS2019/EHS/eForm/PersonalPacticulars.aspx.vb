Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.ServiceProvider
Imports Common.Component.Address

Partial Public Class PersonalPacticulars
    Inherits BasePage
    'Inherits System.Web.UI.Page

    Private Const LocalFunctionCode As String = FunctCode.FUNT020101
    Private Const GlobalFunctionCode As String = FunctCode.FUNT990000
    Private Const DatabaseFunctionCode As String = FunctCode.FUNT990001

    Private udtValidator As Common.Validation.Validator = New Common.Validation.Validator
    Private udtFormatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtSM As Common.ComObject.SystemMessage

    Private udtControlBLL As ControlBLL = New ControlBLL
    Private udtEFormBLL As eFormBLL = New eFormBLL
    Private udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            Dim strAbleToAccessThisPage As String = String.Empty
            strAbleToAccessThisPage = Session(eFormBLL.SESS_PersonalParticular)
            udtEFormBLL.ClearRedirectPageSession()

            If IsNothing(strAbleToAccessThisPage) OrElse Not strAbleToAccessThisPage.Trim.Equals("Y") Then
                Response.Redirect("~/main.aspx")
            Else
                If Not Me.udtSPBLL.Exist Then
                    Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me) ''Begin Writing Audit Log
                    udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, "e-Enrolment Form Loaded")
                End If

                Session("Time") = Now.ToString



                'If IsNothing(Session("FromDisclaimer")) Then
                '    Response.Redirect("~/main.aspx")
                'Else
                '    Dim strFromMain As String = Session("FromDisclaimer")

                '    If Not strFromMain.Equals("Y") Then
                '        Response.Redirect("~/main.aspx")
                '    End If
                'End If

                'Session("FromDisclaimer") = Nothing

                udtControlBLL.bindDistrict(ddlRegDistrict, String.Empty, False)

                BindSPToControl()
            End If

            ' CRE12-001 eHS and PCD integration [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Me.iBtnLoadDemoData.Visible = Me.IsDemo
            ' CRE12-001 eHS and PCD integration [End][Koala]
        End If

        If IsNothing(Session("Time")) Then
            Throw New Exception("Session Expired!")
        End If



    End Sub

    Protected Sub ibtnRegNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim strHKIC As String = UCase(txtRegHKIDPrefix.Text.Trim) + txtRegHKID.Text.Trim + UCase(txtRegHKIDdigit.Text.Trim)

        udtAuditLogEntry.AddDescripton("HKID", strHKIC)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00003, "Input Personal Particulars", New Common.ComObject.AuditLogInfo(Nothing, strHKIC, Nothing, Nothing, Nothing, Nothing))

        GetPersonalFormControl()

        Dim udtSP As ServiceProviderModel

        If udtMsgBox.GetCodeTable.Rows.Count = 0 Then
            udtMsgBox.Visible = False

            If udtSPBLL.Exist Then
                udtSP = udtSPBLL.GetSP

            Else
                udtSP = New ServiceProviderModel

                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA

            End If

            udtSP.EnglishName = udtFormatter.formatEnglishName(txtRegSurname.Text.Trim, txtRegEname.Text.Trim)
            udtSP.ChineseName = txtRegCname.Text.Trim
            udtSP.HKID = txtRegHKIDPrefix.Text.Trim + txtRegHKID.Text.Trim + txtRegHKIDdigit.Text.Trim
            udtSP.SpAddress = New AddressModel(txtRegRoom.Text.Trim, txtRegFloor.Text.Trim, txtRegBlock.Text.Trim, txtRegEAddress.Text.Trim, String.Empty, ddlRegDistrict.SelectedValue.Trim, Nothing)
            udtSP.Email = txtRegConfirmEmail.Text.Trim
            udtSP.Phone = txtRegContactNo.Text.Trim
            udtSP.Fax = txtRegFaxNo.Text.Trim

            udtSPBLL.SaveToSession(udtSP)

            udtEFormBLL.ClearRedirectPageSession()

            Session(eFormBLL.SESS_MedicalOrganization) = "Y"

            udtAuditLogEntry.AddDescripton("HKID", strHKIC)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00004, "Input Personal Particulars Completed", New Common.ComObject.AuditLogInfo(Nothing, strHKIC, Nothing, Nothing, Nothing, Nothing))

            Response.Redirect("~/MedicalOrganization.aspx")

        Else
            'msgBox.BuildMessageBox("ValidationFail")
            udtAuditLogEntry.AddDescripton("HKID", strHKIC)
            udtMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00005, "Input Personal Particulars Fail", New Common.ComObject.AuditLogInfo(Nothing, strHKIC, Nothing, Nothing, Nothing, Nothing))
        End If
    End Sub

    Private Sub GetPersonalFormControl()
        'initialize the "visible" of  error icon to FALSE
        imgEnameAlert.Visible = False
        imgHKIdAlert.Visible = False
        imgEAddressAlert.Visible = False
        imgDistrictAlert.Visible = False
        imgEmailAlert.Visible = False
        imgConfirmEmailAlert.Visible = False
        imgContactNoAlert.Visible = False

        udtMsgBox.Visible = False

        'Check Name (in English)
        udtSM = udtValidator.chkEngName(UCase(txtRegSurname.Text.Trim), UCase(txtRegEname.Text.Trim), String.Empty)
        If Not udtSM Is Nothing Then
            imgEnameAlert.Visible = True
            udtMsgBox.AddMessage(udtSM)
        End If

        'Check HKID
        Dim strHKID As String = UCase(txtRegHKIDPrefix.Text.Trim) + txtRegHKID.Text.Trim + UCase(txtRegHKIDdigit.Text.Trim)
        udtSM = udtValidator.chkHKID(strHKID)
        'SM = Validator.chkHKID(txtRegHKID.Text.Trim)
        If Not udtSM Is Nothing Then
            imgHKIdAlert.Visible = True
            udtMsgBox.AddMessage(udtSM)
        End If

        Dim strArea As String = String.Empty
        strArea = udtEFormBLL.getAreaString(ddlRegDistrict.SelectedValue.Trim)

        udtSM = udtValidator.chkAddress(txtRegEAddress.Text.Trim, ddlRegDistrict.SelectedValue.Trim, strArea)

        If Not udtSM Is Nothing Then
            If udtValidator.IsEmpty(txtRegEAddress.Text.Trim) Then
                imgEAddressAlert.Visible = True
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'If udtValidator.IsEmpty(ddlRegDistrict.SelectedValue.Trim) OrElse ddlRegDistrict.SelectedValue.Trim.Equals(".H") OrElse _
            '    ddlRegDistrict.SelectedValue.Trim.Equals(".K") OrElse ddlRegDistrict.SelectedValue.Trim.Equals(".N") Then
            If udtValidator.IsEmpty(ddlRegDistrict.SelectedValue.Trim) OrElse ddlRegDistrict.SelectedValue.Trim.StartsWith(".") Then
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
                imgDistrictAlert.Visible = True
            End If

            udtMsgBox.AddMessage(udtSM)
        End If


        'Check Email Address
        udtSM = udtValidator.chkEmailAddress(txtRegEmail.Text.Trim)
        If udtSM Is Nothing Then
            udtSM = udtValidator.chkConfirmEmail(txtRegEmail.Text.Trim, txtRegConfirmEmail.Text.Trim)
            If Not udtSM Is Nothing Then
                imgConfirmEmailAlert.Visible = True
                udtMsgBox.AddMessage(udtSM)
            End If
        Else
            imgEmailAlert.Visible = True
            udtMsgBox.AddMessage(udtSM)
        End If

        'Check ContactNo
        udtSM = udtValidator.chkContactNo(txtRegContactNo.Text.Trim)

        If Not udtSM Is Nothing Then
            imgContactNoAlert.Visible = True
            udtMsgBox.AddMessage(udtSM)
        End If

    End Sub

    Protected Sub ibtnRegReset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me) ''Begin Writing Audit Log

        Dim strHKIC As String = UCase(txtRegHKIDPrefix.Text.Trim) + txtRegHKID.Text.Trim + UCase(txtRegHKIDdigit.Text.Trim)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00002, "Clear Personal Particulars", New Common.ComObject.AuditLogInfo(Nothing, strHKIC, Nothing, Nothing, Nothing, Nothing))

        udtMsgBox.Visible = False

        imgEnameAlert.Visible = False
        imgHKIdAlert.Visible = False
        imgEAddressAlert.Visible = False
        imgDistrictAlert.Visible = False
        imgEmailAlert.Visible = False
        imgConfirmEmailAlert.Visible = False
        imgContactNoAlert.Visible = False

        udtSPBLL.ClearSession()

        BindSPToControl()

    End Sub

    Private Sub RenderLanguage()
        If Not IsNothing(ddlRegDistrict) Then
            Me.ddlRegDistrict.Items(0).Text = Me.GetGlobalResourceObject("Text", "SelectDistrict")
        End If
    End Sub


    Private Sub PersonalPacticulars_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) Then
                RenderLanguage()
            End If
        End If
    End Sub

    Private Sub BindSPToControl()
        If udtSPBLL.Exist Then
            Dim udtSP As ServiceProviderModel = udtSPBLL.GetSP

            udtFormatter.seperateEName(udtSP.EnglishName, txtRegSurname.Text, txtRegEname.Text)
            txtRegCname.Text = udtSP.ChineseName
            udtFormatter.seperateHKID(udtSP.HKID, txtRegHKIDPrefix.Text, txtRegHKID.Text, txtRegHKIDdigit.Text)
            txtRegRoom.Text = udtSP.SpAddress.Room
            txtRegFloor.Text = udtSP.SpAddress.Floor
            txtRegBlock.Text = udtSP.SpAddress.Block
            txtRegEAddress.Text = udtSP.SpAddress.Building
            ddlRegDistrict.SelectedValue = udtSP.SpAddress.District
            txtRegEmail.Text = udtSP.Email
            txtRegConfirmEmail.Text = udtSP.Email
            txtRegContactNo.Text = udtSP.Phone
            txtRegFaxNo.Text = udtSP.Fax
        Else
            txtRegSurname.Text = String.Empty
            txtRegEname.Text = String.Empty
            txtRegCname.Text = String.Empty
            txtRegHKIDPrefix.Text = String.Empty
            txtRegHKID.Text = String.Empty
            txtRegHKIDdigit.Text = String.Empty
            txtRegRoom.Text = String.Empty
            txtRegFloor.Text = String.Empty
            txtRegBlock.Text = String.Empty
            txtRegEAddress.Text = String.Empty
            udtControlBLL.bindDistrict(ddlRegDistrict, String.Empty, True)
            txtRegEmail.Text = String.Empty
            txtRegConfirmEmail.Text = String.Empty
            txtRegContactNo.Text = String.Empty
            txtRegFaxNo.Text = String.Empty
        End If
    End Sub

#Region "Implement IWorkingData (CRE11-004)"

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
        If IsNothing(Me.udtSPBLL.GetSP) Then
            Return Nothing
        Else
            Return Me.udtSPBLL.GetSP
        End If
    End Function

#End Region

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Sub iBtnLoadDemoData_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnLoadDemoData.Click
        LoadDemoData()
    End Sub

    Private Sub LoadDemoData()
        Dim ds As DataSet = Me.GetDemoData
        If ds Is Nothing Then Exit Sub

        Dim dt As DataTable = ds.Tables("PersonalParticular")
        Dim dr As DataRow
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            txtRegSurname.Text = dr("Surname")
            txtRegEname.Text = dr("Ename")
            txtRegCname.Text = dr("Cname")
            txtRegHKIDPrefix.Text = dr("HKIDPrefix")
            txtRegHKID.Text = dr("HKID")
            txtRegHKIDdigit.Text = dr("HKIDdigit")
            txtRegRoom.Text = dr("Room")
            txtRegFloor.Text = dr("Floor")
            txtRegBlock.Text = dr("Block")
            txtRegEAddress.Text = dr("EAddress")
            ddlRegDistrict.SelectedValue = dr("District")
            txtRegEmail.Text = dr("Email")
            txtRegConfirmEmail.Text = dr("Email")
            txtRegContactNo.Text = dr("ContactNo")
            txtRegFaxNo.Text = dr("FaxNo")
        Else
            Exit Sub
        End If

    End Sub
   
    ' CRE12-001 eHS and PCD integration [End][Koala]
End Class