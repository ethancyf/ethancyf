Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.Address

Partial Public Class BankAccount
    'Inherits System.Web.UI.Page
    Inherits BasePage

    Private Const LocalFunctionCode As String = FunctCode.FUNT020101
    Private Const GlobalFunctionCode As String = FunctCode.FUNT990000
    Private Const DatabaseFunctionCode As String = FunctCode.FUNT990001
    Private Const SESS_Practice As String = "PracticeBank"
    Private Const SESS_MO As String = "MO"
    Private Const SESS_PerviousPage As String = "PerviousPage"

    Private Const MaxRowNo As Integer = 5

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Class BankAccountConstraint
        Public Const BankAccountNameMaxLength As Integer = 300
    End Class
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    Private udtValidator As Common.Validation.Validator = New Common.Validation.Validator
    Private udtFormatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtSM As Common.ComObject.SystemMessage

    Private udtControlBLL As ControlBLL = New ControlBLL
    Private udtEFormBLL As eFormBLL = New eFormBLL
    Private udtSPBLL As ServiceProvider.ServiceProviderBLL = New ServiceProvider.ServiceProviderBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            Dim strAbleToAccessThisPage As String = String.Empty
            strAbleToAccessThisPage = Session(eFormBLL.SESS_Bank)
            udtEFormBLL.ClearRedirectPageSession()

            If IsNothing(strAbleToAccessThisPage) OrElse Not strAbleToAccessThisPage.Trim.Equals("Y") Then
                Response.Redirect("~/main.aspx")
            Else
                Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me) ''Begin Writing Audit Log

                Dim udtSP As ServiceProvider.ServiceProviderModel
                udtSP = udtSPBLL.GetSP

                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00031, "Bank Account Page Loaded")


                If Not IsNothing(Session(SESS_Practice)) Then
                    Dim dtPractice As DataTable = Session(SESS_Practice)
                    Me.gvPractice.DataSource = dtPractice
                    Me.gvPractice.DataBind()

                    If dtPractice.Rows.Count = 1 Then
                        CType(Me.gvPractice.Rows(0).FindControl("ibtnCopy"), ImageButton).ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CopyDisableSBtn")
                        CType(Me.gvPractice.Rows(0).FindControl("ibtnCopy"), ImageButton).Enabled = False
                    End If
                End If

                Session(SESS_PerviousPage) = String.Empty
                Session.Remove(SESS_PerviousPage)
            End If



        End If

    End Sub


    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
        Dim strAreacode As String
        strAreacode = udtEFormBLL.getAreaString(strDistrict)
        Return udtFormatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
    End Function

    Protected Function formatChiAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
        Dim strAreacode As String
        strAreacode = udtEFormBLL.getAreaString(strDistrict)
        Return udtFormatter.formatAddressChi(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return udtFormatter.formatChineseName(strChineseString)
    End Function


    Private Sub getBankAccFromGridView(ByVal checking As Boolean)
        Dim grid As GridView = Me.gvPractice

        Dim i(3) As Integer

        Dim s(3) As String

        Dim smBank(3) As Common.ComObject.SystemMessage
        'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strBankOwnerText As String = String.Empty
        'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]


        Dim dt As DataTable
        dt = Session(SESS_Practice)

        'Dim dr As DataRow

        For Each r As GridViewRow In grid.Rows
            'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If strBankOwnerText = String.Empty Then
                strBankOwnerText = CType(r.FindControl("lblRegBankOwnerText"), Label).Text.Trim
            End If
            'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

            'Dim strPracticeIndex As String = CType(r.FindControl("lblPracticeIndex"), Label).Text.Trim
            Dim intPracticeIndex As String = CInt(CType(r.FindControl("hfFormatedPracticeIndex"), HiddenField).Value.Trim)
            Dim strRegBankName As String = CType(r.FindControl("txtRegBankName"), TextBox).Text.Trim
            Dim strRegBranchName As String = CType(r.FindControl("txtRegBranchName"), TextBox).Text.Trim
            Dim strRegBankAcc As String = CType(r.FindControl("txtRegBankAcc"), TextBox).Text.Trim
            Dim strRegBankCode As String = CType(r.FindControl("txtRegBankCode"), TextBox).Text.Trim
            Dim strRegBranchCode As String = CType(r.FindControl("txtRegBranchCode"), TextBox).Text.Trim
            Dim strRegBankOwner As String = CType(r.FindControl("txtRegBankOwner"), TextBox).Text.Trim


            Dim imgBankNameAlert As Image = r.FindControl("imgBankNameAlert")
            Dim imgBranchNameAlert As Image = r.FindControl("imgBranchNameAlert")
            Dim imgBankAccAlert As Image = r.FindControl("imgBankAccAlert")
            Dim imgBankOwnerAlert As Image = r.FindControl("imgBankOwnerAlert")

            'imgBRNoAlert.Visible = False
            imgBankNameAlert.Visible = False
            imgBranchNameAlert.Visible = False
            imgBankAccAlert.Visible = False
            imgBankOwnerAlert.Visible = False

            dt.Rows(intPracticeIndex)("Bank") = strRegBankName
            dt.Rows(intPracticeIndex)("Branch") = strRegBranchName
            dt.Rows(intPracticeIndex)("BankAcc") = strRegBankAcc
            dt.Rows(intPracticeIndex)("BankCode") = strRegBankCode
            dt.Rows(intPracticeIndex)("BranchCode") = strRegBranchCode
            dt.Rows(intPracticeIndex)("Holder") = strRegBankOwner

            If checking Then
                ' INT17-0012 (Fix EForm Bank Account can Next with no input) [Start] [Winnie]
                '-----------------------------------------------------------------------------------------
                ' Apply checking no matter all fields empty                
                '-----------------------------------------------------------------------------------------
                'If udtValidator.IsEmpty(strRegBankName) And udtValidator.IsEmpty(strRegBranchName) And _
                '              udtValidator.IsEmpty(strRegBankAcc) And udtValidator.IsEmpty(strRegBankCode) And udtValidator.IsEmpty(strRegBranchCode) And _
                '              udtValidator.IsEmpty(strRegBankOwner) Then                    

                'Else
                ' INT17-0012 (Fix EForm Bank Account can Next with no input) [End] [Winnie]

                udtSM = udtValidator.chkBankName(strRegBankName)
                If Not udtSM Is Nothing Then
                    imgBankNameAlert.Visible = True
                    i(0) = i(0) + 1
                    s(0) = s(0) + ", " + (r.RowIndex + 1).ToString
                    smBank(0) = udtSM
                End If

                udtSM = udtValidator.chkBankAccount(strRegBankCode, strRegBranchCode, strRegBankAcc, String.Empty, False)
                If Not udtSM Is Nothing Then
                    imgBankAccAlert.Visible = True
                    i(1) = i(1) + 1
                    s(1) = s(1) + ", " + (r.RowIndex + 1).ToString
                    smBank(1) = udtSM
                End If

                udtSM = udtValidator.chkBankOwner(strRegBankOwner, BankAccountConstraint.BankAccountNameMaxLength)
                If Not udtSM Is Nothing Then
                    imgBankOwnerAlert.Visible = True
                    i(2) = i(2) + 1
                    s(2) = s(2) + ", " + (r.RowIndex + 1).ToString
                    smBank(2) = udtSM
                End If
            End If
            'End If
        Next


        If checking Then
            If i(0) > 0 Then
                udcMsgBox.AddMessage(smBank(0), "%s", s(0).Substring(1))
            End If
            If i(1) > 0 Then
                Dim str() As String = {s(1)}
                udcMsgBox.AddMessage(smBank(1), "%s", s(1).Substring(1))
            End If
            If i(2) > 0 Then
                Dim str() As String = {s(2)}
                'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'udcMsgBox.AddMessage(smBank(2), "%s", s(2).Substring(1))
                udcMsgBox.AddMessage(smBank(2), New String() {"%t", "%s"}, New String() {strBankOwnerText, s(2).Substring(1)})
                'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

            End If
        End If

        Session(SESS_Practice) = dt

    End Sub

    Protected Sub ibtnRegBankBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Me.getBankAccFromGridView(False)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00044, "Press Back to Practice")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_Practice) = "Y"
        Response.Redirect("~/Practice.aspx")
    End Sub

    Protected Sub ibtnRegBankNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)
        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00040, "Input Bank Account Information")

        getBankAccFromGridView(True)

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False

            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00041, "Input Bank Account Information Completed")

            Session(SESS_PerviousPage) = "CompleteBank"

            udtEFormBLL.ClearRedirectPageSession()

            Session(eFormBLL.SESS_SchemeSelection) = "Y"
            Response.Redirect("~/SchemeSelection.aspx")

        Else
            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00042, "Input Bank Account Information Fail")
        End If


    End Sub

    Protected Sub ibtnRegBankSkip_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        getBankAccFromGridView(False)
        Session(SESS_PerviousPage) = "Bank"

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00043, "Skip Bank Account")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_SchemeSelection) = "Y"
        Response.Redirect("~/SchemeSelection.aspx")

      
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) Then
                RenderLanguage()
            End If
        End If
    End Sub

    Private Sub RenderLanguage()
        If Not IsNothing(Session(SESS_Practice)) Then
            Dim dtPractice As DataTable = Session(SESS_Practice)

            Dim selectedLanguageValue As String
            selectedLanguageValue = LCase(Session("language"))

            'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If udcMsgBox.Visible Then
                getBankAccFromGridView(True)
                udcMsgBox.Visible = True
            End If
            'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

            If dtPractice.Rows.Count = 1 Then
                If selectedLanguageValue.Trim.Equals(English) Then
                    CType(Me.gvPractice.Rows(0).FindControl("ibtnCopy"), ImageButton).ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CopyDisableSBtn")
                Else
                    CType(Me.gvPractice.Rows(0).FindControl("ibtnCopy"), ImageButton).ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CopyDisableSBtn")
                End If

                CType(Me.gvPractice.Rows(0).FindControl("ibtnCopy"), ImageButton).Enabled = False
            End If
        End If

    End Sub

    Private Sub gvPractice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPractice.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblRegBankMOName As Label = CType(e.Row.FindControl("lblRegBankMOName"), Label)

            Dim dt As DataTable
            dt = Session(SESS_MO)
            If Not IsNothing(SESS_MO) Then
                lblRegBankMOName.Text = dt.Rows(CInt(lblRegBankMOName.Text.Trim) - 1).Item("MOEName")
            End If

            Dim txtRegBankCode As TextBox = CType(e.Row.FindControl("txtRegBankCode"), TextBox)
            Dim txtRegBranchCode As TextBox = CType(e.Row.FindControl("txtRegBranchCode"), TextBox)
            Dim txtRegBankAcc As TextBox = CType(e.Row.FindControl("txtRegBankAcc"), TextBox)
            'CRE17-013 (Extend bank account name to 300 chars) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim txtRegBankOwner As TextBox = CType(e.Row.FindControl("txtRegBankOwner"), TextBox)
            'CRE17-013 (Extend bank account name to 300 chars) [End][Chris YIM]

            txtRegBankCode.Attributes.Add("onKeyUp", "TabNext(this,'up',3," + txtRegBranchCode.ClientID + ")")

            txtRegBranchCode.Attributes.Add("onKeyUp", "TabNext(this,'up',3," + txtRegBankAcc.ClientID + ")")

            'CRE17-013 (Extend bank account name to 300 chars) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            txtRegBankOwner.Attributes.Add("onKeyUp", "return LimitLength(this, 300, event);")
            'CRE17-013 (Extend bank account name to 300 chars) [End][Chris YIM]
        End If
    End Sub

    Protected Sub lnkBtnPersonal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Me.getBankAccFromGridView(False)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00037, "Press Tab to Personal Particulars in Bank Account")

        udtEFormBLL.ClearRedirectPageSession()
        Session(eFormBLL.SESS_PersonalParticular) = "Y"
        Response.Redirect("~/PersonalPacticulars.aspx")
    End Sub

    Protected Sub lnkBtnMO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Me.getBankAccFromGridView(False)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00038, "Press Tab to MO in Bank Account")


        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_MedicalOrganization) = "Y"
        Response.Redirect("~/MedicalOrganization.aspx")
    End Sub

    Protected Sub lnkBtnPractice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Me.getBankAccFromGridView(False)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00039, "Press Tab to Practice in Bank Account")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_Practice) = "Y"
        Response.Redirect("~/Practice.aspx")
    End Sub

   
    Protected Sub ibtnCopy_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00032, "Copy Bank Account")


        Dim ibtnCopy As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnCopy.NamingContainer

        udcMsgBox.Visible = False
        txtPracticeIndex.Text = String.Empty

        If Not IsNothing(row) Then

            txtPracticeIndex.Text = CType(row.FindControl("hfFormatedPracticeIndex"), HiddenField).Value.Trim

            Dim intNoOfPracticeCannotCopy As Integer = 0

            Dim dt As New DataTable

            Dim dtDDL As New DataTable
            dtDDL.Columns.Add(New DataColumn("PracticeIndex"))
            dtDDL.Columns.Add(New DataColumn("PracticeName"))
            Dim drDDL As DataRow

            Me.ddlPracticeList.Items.Clear()

            getBankAccFromGridView(False)
            dt = Session(SESS_Practice)

            If Not IsNothing(dt) Then
                For Each dr As DataRow In dt.Rows

                    If CInt(dr.Item("PracticeIndex")) <> CInt(txtPracticeIndex.Text.Trim) Then
                        Dim strRegBankName As String = String.Empty
                        'Dim strRegBranchName As String = String.Empty
                        Dim strRegBankAcc As String = String.Empty
                        Dim strRegBankCode As String = String.Empty
                        Dim strRegBranchCode As String = String.Empty
                        Dim strRegBankOwner As String = String.Empty

                        strRegBankName = dr.Item("Bank")
                        'strRegBranchName
                        strRegBankAcc = dr.Item("BankAcc")
                        strRegBankCode = dr.Item("BankCode")
                        strRegBranchCode = dr.Item("BranchCode")
                        strRegBankOwner = dr.Item("Holder")

                        If Not udtValidator.IsEmpty(strRegBankName) AndAlso Not udtValidator.IsEmpty(strRegBankAcc) AndAlso Not udtValidator.IsEmpty(strRegBankCode) AndAlso _
                           Not udtValidator.IsEmpty(strRegBranchCode) AndAlso Not udtValidator.IsEmpty(strRegBankOwner) Then
                            drDDL = dtDDL.NewRow
                            drDDL(0) = dr.Item("PracticeIndex")
                            drDDL(1) = CStr(dr.Item("PracticeName")).Trim '+ udtFormatter.formatChineseName(CStr(dr.Item("PracticeNameChi")).Trim)
                            dtDDL.Rows.Add(drDDL)

                        Else
                            intNoOfPracticeCannotCopy = intNoOfPracticeCannotCopy + 1
                        End If
                    End If

                Next

                If intNoOfPracticeCannotCopy = dt.Rows.Count - 1 Then
                    udtSM = New SystemMessage(LocalFunctionCode, SeverityCode.SEVE, MsgCode.MSG00019)
                    udcMsgBox.AddMessage(udtSM)
                    'udcMsgBox.BuildMessageBox("ValidationFail")
                    udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                    udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00034, "Copy Bank Account Fail")

                Else

                    Me.ddlPracticeList.DataSource = dtDDL
                    Me.ddlPracticeList.DataTextField = "PracticeName"
                    Me.ddlPracticeList.DataValueField = "PracticeIndex"
                    Me.ddlPracticeList.DataBind()

                    If dtDDL.Rows.Count = 1 Then
                        Me.ddlPracticeList.Enabled = False
                    Else
                        Me.ddlPracticeList.Enabled = True
                    End If

                    lblCopyBankName.Text = dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("Bank")
                    lblCopyBranchName.Text = dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("Branch")
                    lblCopyBankAcc.Text = udtFormatter.formatBankAcct(dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("BankCode"), _
                                                                        dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("BranchCode"), _
                                                                        dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("BankAcc"))
                    lblCopyBankOwner.Text = dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("Holder")

                    Me.ModalPopupCopyList.Show()

                    udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00033, "Copy Bank Account Complete")
                End If

            End If
        End If

    End Sub

    Protected Sub ibtnDialogConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)
        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        Dim intCopyFormPractice As Integer = 0
        Dim intCopyToPractice As Integer = 0

        Dim dt As New DataTable
        dt = Session(SESS_Practice)

        If Not IsNothing(ddlPracticeList) Then
            intCopyFormPractice = CInt(ddlPracticeList.SelectedValue.Trim)

            If Not txtPracticeIndex.Text.Trim.Equals(String.Empty) Then
                intCopyToPractice = CInt(txtPracticeIndex.Text.Trim)

                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.AddDescripton("From Practice No", intCopyFormPractice + 1)
                udtAuditLogEntry.AddDescripton("To Practice No", intCopyToPractice + 1)
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00035, "Copy Bank Account Detail")


                dt.Rows(intCopyToPractice).Item("Bank") = dt.Rows(intCopyFormPractice).Item("Bank")
                dt.Rows(intCopyToPractice).Item("Branch") = dt.Rows(intCopyFormPractice).Item("Branch")
                dt.Rows(intCopyToPractice).Item("BankAcc") = dt.Rows(intCopyFormPractice).Item("BankAcc")
                dt.Rows(intCopyToPractice).Item("BankCode") = dt.Rows(intCopyFormPractice).Item("BankCode")
                dt.Rows(intCopyToPractice).Item("BranchCode") = dt.Rows(intCopyFormPractice).Item("BranchCode")
                dt.Rows(intCopyToPractice).Item("Holder") = dt.Rows(intCopyFormPractice).Item("Holder")

                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.AddDescripton("From Practice No", intCopyFormPractice + 1)
                udtAuditLogEntry.AddDescripton("To Practice No", intCopyToPractice + 1)
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00036, "Copy Bank Account Detail Completed")
           
            End If

            Session(SESS_Practice) = dt
            Me.gvPractice.DataSource = dt
            Me.gvPractice.DataBind()
        End If

    End Sub

    Protected Sub ibtnDialogCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ModalPopupCopyList.Hide()
    End Sub
   
    'Protected Sub ddlPracticeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim intSelectedPractice As Integer
    '    intSelectedPractice = CInt(ddlPracticeList.SelectedValue.Trim())

    '    For Each li As ListItem In choCopyList.Items
    '        If li.Value.Trim.Equals(ddlPracticeList.SelectedValue.Trim) Then
    '            li.Enabled = False
    '        Else
    '            li.Enabled = True
    '        End If
    '    Next

    '    Me.ModalPopupCopyList.Show()

    'End Sub

    Protected Sub ddlPracticeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dt As New DataTable
        dt = Session(SESS_Practice)
        If Not IsNothing(dt) Then
            lblCopyBankName.Text = dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("Bank")
            lblCopyBranchName.Text = dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("Branch")
            lblCopyBankAcc.Text = udtFormatter.formatBankAcct(dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("BankCode"), _
                                                                dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("BranchCode"), _
                                                                dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("BankAcc"))
            lblCopyBankOwner.Text = dt.Rows(CInt(ddlPracticeList.SelectedValue.Trim)).Item("Holder")
        End If

        Me.ModalPopupCopyList.Show()
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
End Class