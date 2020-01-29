Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.Address
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation

Imports Common.PCD
Imports Common.PCD.WebService

Imports Common.Component.BankAcct
Imports Common.Component.Professional
Imports Common.Component.District
Imports Common.Component.Area

Imports Common.Component.PracticeSchemeInfo

Imports Common.Component.StaticData

Imports Common.Component.Profession
Imports Common.Component.UserAC

Imports Common.Component.MedicalOrganization

Imports Common.Component.PracticeType_PCD
Imports Common.Component.Practice

Partial Public Class ConfirmDetails
    'Inherits System.Web.UI.Page
    Inherits BasePage

    Private Const LocalFunctionCode As String = FunctCode.FUNT020101
    Private Const GlobalFunctionCode As String = FunctCode.FUNT990000
    Private Const DatabaseFunctionCode As String = FunctCode.FUNT990001
    Private Const SESS_Practice As String = "PracticeBank"
    Private Const SESS_MO As String = "MO"
    Private Const SESS_HCVS As String = "HCVS"
    Private Const SESS_IVSS As String = "IVSS"
    Private Const SESS_PerviousPage As String = "PerviousPage"
    Private Const SESS_SelectedScheme As String = "SelectedScheme"
    Private Const SESS_SubScheme = "SubScheme"

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Class DT_NAME
        Public Const DT_NAME_CATEGORY As String = "DATATABLE_CONFIRM_DETAIL_CATEGORY_DISPLAY_SETTING"

        Public Const DT_FIELD_NAME_CATEGORY_SCHEME As String = "CATEGORY_SCHEME"
        Public Const DT_FIELD_NAME_CATEGORY_ROW_SEQ As String = "CATEGORY_ROW_SEQ"
        Public Const DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ As String = "CATEGORY_DISPLAY_SEQ"
        Public Const DT_FIELD_NAME_CATEGORY_ROW_SPAN As String = "CATEGORY_ROW_SPAN"
    End Class
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    Private udtFormatter As Common.Format.Formatter = New Common.Format.Formatter

    Private udtControlBLL As ControlBLL = New ControlBLL
    Private udtEFormBLL As eFormBLL = New eFormBLL
    Private udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
    Private udtSchemeEFormBLL As SchemeEFormBLL = New SchemeEFormBLL
    Private udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            Dim strAbleToAccessThisPage As String = String.Empty
            strAbleToAccessThisPage = Session(eFormBLL.SESS_ConfirmDetails)
            udtEFormBLL.ClearRedirectPageSession()

            If IsNothing(strAbleToAccessThisPage) OrElse Not strAbleToAccessThisPage.Trim.Equals("Y") Then
                Response.Redirect("~/main.aspx")
            Else
                Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me) ''Begin Writing Audit Log

                Dim udtSP As ServiceProvider.ServiceProviderModel
                udtSP = udtSPBLL.GetSP

                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00054, "Confirm Detail Page Loaded")

                If udtSPBLL.Exist Then
                    udtSP = udtSPBLL.GetSP

                    panEditPersonalTop.Visible = True
                    panEditPersonalBottom.Visible = True

                    panEditSchemeTop.Visible = True
                    panEditSchemeBottom.Visible = True


                    With udtSP
                        lblConfirmEname.Text = .EnglishName
                        lblConfirmCname.Text = udtFormatter.formatChineseName(.ChineseName)
                        lblConfirmHKID.Text = udtFormatter.formatHKID(.HKID, False)
                        lblConfirmAddress.Text = formatAddress(.SpAddress.Room, .SpAddress.Floor, .SpAddress.Block, .SpAddress.Building, .SpAddress.District)
                        lblConfirmEmail.Text = .Email
                        lblConfirmContactNo.Text = .Phone
                        If .Fax.Equals(String.Empty) Then
                            lblConfirmFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
                        Else
                            lblConfirmFax.Text = .Fax
                        End If

                        If .AlreadyJoinEHR.Equals(JoinEHRSSStatus.NA) Then
                            panHadJoinEHRSS.Visible = False
                        ElseIf .AlreadyJoinEHR.Equals(JoinEHRSSStatus.Yes) Then
                            panHadJoinEHRSS.Visible = True
                            lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "Yes")
                        ElseIf .AlreadyJoinEHR.Equals(JoinEHRSSStatus.No) Then
                            panHadJoinEHRSS.Visible = True
                            lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "No")
                        End If


                        panEHRSS.Visible = panHadJoinEHRSS.Visible

                        panPCD.Visible = True
                        If .JoinPCD.Equals(JoinPCDStatus.Yes) Then
                            lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "Yes")
                            chkConfirmPCDConditions.Visible = True
                            panSelectPracticeType.Visible = True

                            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                            ' ==========================================================
                        ElseIf .JoinPCD.Equals(JoinPCDStatus.Enrolled) Then
                            lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_JoinedPCD")
                            chkConfirmPCDConditions.Visible = False
                            panSelectPracticeType.Visible = False
                            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                            ' CRE19-008 (Rename VO) [Start][Koala]
                        ElseIf .JoinPCD.Equals(JoinPCDStatus.No) Then
                            lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_NotJoinPCD")
                            chkConfirmPCDConditions.Visible = False
                            panSelectPracticeType.Visible = False
                            ' CRE19-008 (Rename VO) [Start][Koala]
                        Else
                            panPCD.Visible = False
                            chkConfirmPCDConditions.Visible = False
                            panSelectPracticeType.Visible = False
                        End If


                    End With

                    Dim selectedLanguageValue As String
                    selectedLanguageValue = LCase(Session("language"))

                    If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                        Dim udtSchemeEFormList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                        For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                            If Not IsNothing(Session(udtSchemeEForm.SchemeCode.Trim)) Then
                                If CStr(Session(udtSchemeEForm.SchemeCode.Trim)).Equals("Y") Then
                                    Dim newLi As New HtmlGenericControl
                                    newLi.ID = "li" & udtSchemeEForm.SchemeCode.Trim
                                    newLi.TagName = "li"
                                    If selectedLanguageValue.Equals(English) Then
                                        newLi.InnerText = udtSchemeEForm.SchemeDesc
                                    Else
                                        newLi.InnerText = udtSchemeEForm.SchemeDescChi
                                    End If
                                    pnlConfirmScheme.Controls.Add(newLi)
                                End If
                            End If
                        Next
                    End If


                    Dim s As String = String.Empty

                    If IsNothing(Session(SESS_PerviousPage)) Then
                        s = String.Empty
                    Else
                        s = Session(SESS_PerviousPage)
                    End If

                    If s.Equals("MO") Then
                        panEditMO.Visible = False
                        panEditMOTop.Visible = False
                        panEditMOBottom.Visible = False

                        panEditPracticeBank.Visible = False

                        panEditPracticeTop.Visible = False
                        panEditPracticeBottom.Visible = False

                        panEditBankTop.Visible = False
                        panEditBankBottom.Visible = False

                    ElseIf s.Equals("Practice") Then

                        panEditPracticeBank.Visible = False
                        panEditPracticeTop.Visible = False
                        panEditPracticeBottom.Visible = False

                        If Not IsNothing(Session(SESS_MO)) Then
                            Dim dtMO As DataTable = Session(SESS_MO)
                            gvMO.Visible = True
                            Me.gvMO.DataSource = dtMO
                            Me.gvMO.DataBind()
                            panEditMO.Visible = True
                            panEditMOTop.Visible = True
                            panEditMOBottom.Visible = True
                        Else
                            panEditMO.Visible = False

                            panEditMOTop.Visible = False
                            panEditMOBottom.Visible = False
                        End If

                    ElseIf s.Equals("Bank") OrElse s.Equals("CompleteBank") Then

                        If Not IsNothing(Session(SESS_MO)) Then
                            Dim dtMO As DataTable = Session(SESS_MO)
                            gvMO.Visible = True
                            Me.gvMO.DataSource = dtMO
                            Me.gvMO.DataBind()

                            panEditMO.Visible = True

                            panEditMOTop.Visible = True
                            panEditMOBottom.Visible = True
                        Else
                            panEditMO.Visible = False

                            panEditMOTop.Visible = False
                            panEditMOBottom.Visible = False

                        End If

                        If Not IsNothing(Session(SESS_Practice)) Then
                            panEditPracticeBank.Visible = True

                            panEditPracticeTop.Visible = True
                            panEditPracticeBottom.Visible = True

                            Dim dtPractice As DataTable = Session(SESS_Practice)

                            gvPractice.Visible = True

                            Me.gvPractice.DataSource = dtPractice
                            Me.gvPractice.DataBind()

                        Else
                            panEditPracticeBank.Visible = False

                            panEditPracticeTop.Visible = False
                            panEditPracticeBottom.Visible = False


                        End If

                        If s.Equals("Bank") Then
                            panEditBankTop.Visible = False
                            panEditBankBottom.Visible = False
                        ElseIf s.Equals("CompleteBank") Then
                            panEditBankTop.Visible = True
                            panEditBankBottom.Visible = True

                        End If
                    Else

                        panEditMO.Visible = False

                        panEditMOTop.Visible = False
                        panEditMOBottom.Visible = False

                        panEditPracticeBank.Visible = False

                        panEditPracticeTop.Visible = False
                        panEditPracticeBottom.Visible = False

                        panEditBankTop.Visible = False
                        panEditBankBottom.Visible = False
                    End If
                End If
            End If

            Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
            Me.ibtnConfirm.Enabled = False
        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        If Not Me.IsPostBack Or Me.LanguageChanged Then
            Me.ucTypeOfPracticeGrid.Mode = eForm.ucTypeOfPracticeGrid.EnumMode.View
            Me.ucTypeOfPracticeGrid.LoadPractice(udtSPBLL.GetSP)
        End If

        showConfirmBtn()

        txtConfirmImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
        txtConfirmDisableImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")

        ShowPanelPCD()

        ' CRE12-001 eHS and PCD integration [End][Tommy]

    End Sub

    Public Sub ShowPanelPCD()
        Dim strCurrentProf As String = String.Empty
        Dim dtPractice As DataTable = Session(SESS_Practice)

        If Not IsNothing(dtPractice) Then
            strCurrentProf = CStr(dtPractice.Rows(0).Item("ServiceCategoryCode")).Trim
        End If

        If Common.Component.Profession.ProfessionBLL.GetProfessionListByServiceCategoryCode(strCurrentProf).AllowJoinPCD Then
            ' CRE19-008 (Rename VO) [Start][Koala]
            Dim udtSP As ServiceProvider.ServiceProviderModel
            udtSP = udtSPBLL.GetSP

            If udtSP.JoinPCD <> JoinPCDStatus.NA Then
                panPCD.Visible = True
            End If
            ' CRE19-008 (Rename VO) [End][Koala]
        Else
            panPCD.Visible = False
        End If
    End Sub

    Protected Sub ibtnConfirmBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)
        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00059, "Press Back to Scheme Selection")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_SchemeSelection) = "Y"
        Response.Redirect("~/SchemeSelection.aspx")
    End Sub

    Protected Sub ibtnConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If blnShowConfirmBtn() Then
            Dim udtAuditLogCheckConfirm As New AuditLogEntry(LocalFunctionCode, Me)
            Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)


            Dim udtSP As ServiceProviderModel = udtSPBLL.GetSP

            udtAuditLogCheckConfirm.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogCheckConfirm.WriteLog(LogID.LOG00055, "Checked the confirm declaration")

            'udtAuditLogCheckConfirm.AddDescripton("Name", txtRegSurname.Text.Trim + " " + txtRegEname.Text.Trim)
            'udtAuditLogCheckConfirm.AddDescripton("HKID", strHKID)
            'udtAuditLogCheckConfirm.AddDescripton("Email", txtRegEmail.Text.Trim)
            'udtAuditLogCheckConfirm.AddDescripton("Scheme", strSchemeCode)

            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00056, "Save Service Provider Profile")

            'Dim strERN() As String = {}
            Dim strERN As New List(Of String)

            Try
                Dim s As String = String.Empty

                If Not IsNothing(Session(SESS_PerviousPage)) Then
                    s = Session(SESS_PerviousPage)

                    If udtSPBLL.Exist Then

                        Dim dtMO As DataTable = Nothing
                        Dim dtPracticeBank As DataTable = Nothing

                        If Not IsNothing(Session(SESS_MO)) Then
                            dtMO = Session(SESS_MO)
                        End If

                        If Not IsNothing(Session(SESS_Practice)) Then
                            dtPracticeBank = Session(SESS_Practice)
                        End If

                        Dim udtSchemeInfoList As SchemeInformationModelCollection = New SchemeInformationModelCollection

                        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                            Dim udtSchemeEFormList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                            For Each udtSchemeEFrom As SchemeEFormModel In udtSchemeEFormList
                                If Not IsNothing(Session(udtSchemeEFrom.SchemeCode.Trim)) Then
                                    If CStr(Session(udtSchemeEFrom.SchemeCode.Trim)).Equals("Y") Then
                                        udtSchemeInfoList.Add(New SchemeInformationModel(String.Empty, _
                                                                                         String.Empty, _
                                                                                         udtSchemeEFrom.SchemeCode.Trim, _
                                                                                         String.Empty, _
                                                                                         String.Empty, _
                                                                                         String.Empty, _
                                                                                         Nothing, _
                                                                                         Nothing, _
                                                                                         Nothing, _
                                                                                         Nothing, _
                                                                                         String.Empty, _
                                                                                         Nothing, _
                                                                                         String.Empty, _
                                                                                         Nothing, _
                                                                                         udtSchemeEFrom.DisplaySeq))
                                    End If

                                End If
                            Next
                        End If

                        Select Case s
                            'Case "MO"
                            'strERN = udtEFormBLL.AddServiceProviderProfileToEnrolment(udtsp, Nothing, Nothing, False, True, False)
                            'Case "Practice"
                            'strERN = udtEFormBLL.AddServiceProviderProfileToEnrolment(udtsp, dtMO, Nothing, False, True, False)
                            Case "Bank"
                                strERN = udtEFormBLL.AddServiceProviderProfileToEnrolment(udtSP, udtSchemeInfoList, dtMO, dtPracticeBank, False)
                            Case "CompleteBank"
                                strERN = udtEFormBLL.AddServiceProviderProfileToEnrolment(udtSP, udtSchemeInfoList, dtMO, dtPracticeBank, True)
                        End Select

                        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00057, "Save Service Provider Profile Complete")

                        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                        ' ==========================================================

                        If udtSP.JoinPCD = JoinPCDStatus.Yes Then
                            Session("IsJoinPCD") = True
                            udtEFormBLL.UpdateSPModel(udtSPBLL.GetSP, udtSchemeInfoList, dtMO, dtPracticeBank, False)
                            ToPCD()
                        Else
                            Session("IsJoinPCD") = False
                        End If

                        ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                    End If

                    If Not IsNothing(strERN) Then
                        Session("EnrolmentRefNo") = strERN
                        Response.Redirect("~/CompletedEnrolment.aspx")
                    End If
                End If
            Catch exThread As Threading.ThreadAbortException

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00058, "Save Service Provider Profile Fail")
                Throw ex
            End Try



        End If
    End Sub

    Protected Function formatBankAcct(ByVal strBankcode As String, ByVal strBranchCode As String, ByVal strBankAcct As String) As String
        Return udtFormatter.formatBankAcct(strBankcode, strBranchCode, strBankAcct)
    End Function

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If strPracticeCode.Equals(String.Empty) Then
            strPracticeTypeName = String.Empty
        Else
            If Session("language") = "zh-tw" Then
                strPracticeTypeName = udtEFormBLL.GetPracticeTypeName(strPracticeCode).DataValueChi
            Else
                strPracticeTypeName = udtEFormBLL.GetPracticeTypeName(strPracticeCode).DataValue
            End If
        End If

        Return strPracticeTypeName
    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        Dim strHealthProfName As String

        If strHealthProfCode.Equals(String.Empty) Then
            strHealthProfName = String.Empty
        Else

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            If Session("language") = "zh-tw" Then
                strHealthProfName = udtEFormBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
            Else
                strHealthProfName = udtEFormBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc
            End If

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        End If

        Return strHealthProfName
    End Function

    Private Sub gvMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMO.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblMOEmail As Label = CType(e.Row.FindControl("lblMOEmail"), Label)
            Dim lblMOFax As Label = CType(e.Row.FindControl("lblMOFax"), Label)
            Dim lblMOBRCode As Label = CType(e.Row.FindControl("lblMOBRCode"), Label)

            If lblMOEmail.Text.Trim.Equals(String.Empty) Then
                lblMOEmail.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblMOFax.Text.Trim.Equals(String.Empty) Then
                lblMOFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblMOBRCode.Text.Trim.Equals(String.Empty) Then
                lblMOBRCode.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If
        End If
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

        Dim selectedLanguageValue As String
        selectedLanguageValue = LCase(Session("language"))

        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
            Dim udtSchemeEFormList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

            For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                If Not IsNothing(Session(udtSchemeEForm.SchemeCode.Trim)) Then
                    If CStr(Session(udtSchemeEForm.SchemeCode.Trim)).Equals("Y") Then
                        Dim newLi As New HtmlGenericControl
                        newLi.ID = "li" & udtSchemeEForm.SchemeCode.Trim
                        newLi.TagName = "li"
                        If selectedLanguageValue.Equals(English) Then
                            newLi.InnerText = udtSchemeEForm.SchemeDesc
                        Else
                            newLi.InnerText = udtSchemeEForm.SchemeDescChi
                        End If
                        pnlConfirmScheme.Controls.Add(newLi)
                    End If
                End If
            Next
        End If

        If udtSPBLL.Exist Then
            Dim udtSP As ServiceProviderModel = New ServiceProviderModel
            udtSP = udtSPBLL.GetSP

            With udtSP
                If .Fax.Equals(String.Empty) Then
                    lblConfirmFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
                Else
                    lblConfirmFax.Text = .Fax
                End If

                If .AlreadyJoinEHR.Equals(JoinEHRSSStatus.NA) Then
                    panHadJoinEHRSS.Visible = False
                ElseIf .AlreadyJoinEHR.Equals(JoinEHRSSStatus.Yes) Then
                    panHadJoinEHRSS.Visible = True
                    lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "Yes")
                ElseIf .AlreadyJoinEHR.Equals(JoinEHRSSStatus.No) Then
                    panHadJoinEHRSS.Visible = True
                    lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "No")
                End If

                ' CRE14-008 - QIV [Start] [Winnie]
                ' -----------------------------------------------------------------------------------------
                panEHRSS.Visible = panHadJoinEHRSS.Visible

                If .JoinPCD.Equals(JoinPCDStatus.Yes) Then
                    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "Yes")
                    chkConfirmPCDConditions.Visible = True
                    panSelectPracticeType.Visible = True
                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' ==========================================================
                ElseIf .JoinPCD.Equals(JoinPCDStatus.Enrolled) Then
                    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_JoinedPCD")
                    chkConfirmPCDConditions.Visible = False
                    panSelectPracticeType.Visible = False
                    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
                Else
                    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_NotJoinPCD")
                    chkConfirmPCDConditions.Visible = False
                    panSelectPracticeType.Visible = False
                End If
                ' CRE14-008 - QIV [End] [Winnie]

            End With
        End If

        If Not IsNothing(Session(SESS_MO)) Then
            Dim dtMO As DataTable = Session(SESS_MO)
            Me.gvMO.DataSource = dtMO
            Me.gvMO.DataBind()
        End If

        If Not IsNothing(Session(SESS_Practice)) Then
            Dim dtPractice As DataTable = Session(SESS_Practice)
            Me.gvPractice.DataSource = dtPractice
            Me.gvPractice.DataBind()

        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        showConfirmBtn()

        ' CRE12-001 eHS and PCD integration [End][Tommy]

    End Sub

    Private Sub gvPractice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPractice.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblPracticeIndex As Label = CType(e.Row.FindControl("lblPracticeIndex"), Label)
            Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblPracticeMO"), Label)

            Dim dt As DataTable
            dt = Session(SESS_MO)
            If Not IsNothing(SESS_MO) Then
                lblPracticeMO.Text = dt.Rows(CInt(lblPracticeMO.Text.Trim) - 1).Item("MOEName")
            End If

            Dim trServiceFee As HtmlControls.HtmlTableRow = e.Row.FindControl("trServiceFee")
            trServiceFee.Visible = False

            Dim pnlServiceFee As Panel = e.Row.FindControl("pnlServiceFee")

            Dim lblBankName As Label = e.Row.FindControl("lblBankName")
            Dim lblBranchName As Label = e.Row.FindControl("lblBranchName")
            Dim lblBankAcc As Label = e.Row.FindControl("lblBankAcc")
            Dim lblBankOwner As Label = e.Row.FindControl("lblBankOwner")

            Dim s As String = String.Empty

            If IsNothing(Session(SESS_PerviousPage)) Then
                s = String.Empty
            Else
                s = Session(SESS_PerviousPage)
            End If

            If s.Equals("Bank") Then
                lblBankName.Text = String.Empty
                lblBranchName.Text = String.Empty
                lblBankAcc.Text = String.Empty
                lblBankOwner.Text = String.Empty
            End If

            If lblBankName.Text.Trim.Equals(String.Empty) Then
                lblBankName.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblBranchName.Text.Trim.Equals(String.Empty) Then
                lblBranchName.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblBankAcc.Text.Trim.Equals(String.Empty) Then
                lblBankAcc.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblBankOwner.Text.Trim.Equals(String.Empty) Then
                lblBankOwner.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Dim udtSubsidzeGroupEFormList As New SubsidizeGroupEFormModelCollection

            'If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
            '    udtSubsidzeGroupEFormList = udtSchemeEFormBLL.GetSession_SubsidizeGroupEForm
            'End If

            Dim udtSchemeEFormList As SchemeEFormModelCollection = Nothing
            Dim udtFilteredSchemeEFormList As New SchemeEFormModelCollection

            If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup
            End If

            Dim selectedLanguageValue As String
            selectedLanguageValue = LCase(Session("language"))

            Dim dtSchemeToEnroll As New DataTable("SchemeToEnroll")
            Dim dcSchemeToEnroll As DataColumn
            Dim drSchemeToEnroll As DataRow

            Dim dtPractice As DataTable = Session(SESS_Practice)

            If Not dtPractice Is Nothing Then
                Dim drPractice As DataRow = dtPractice.Rows(e.Row.RowIndex)

                dcSchemeToEnroll = New DataColumn("Info", Type.GetType("System.String"))
                dtSchemeToEnroll.Columns.Add(dcSchemeToEnroll)

                If Not udtSchemeEFormList Is Nothing Then
                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        If drPractice(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes And _
                            drPractice(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then

                            'Generate data table of Scheme To Enrol
                            drSchemeToEnroll = dtSchemeToEnroll.NewRow

                            If selectedLanguageValue.Equals(English) Then
                                drSchemeToEnroll("Info") = "<li>" + udtSchemeEForm.SchemeDesc + "</li>"
                            Else
                                drSchemeToEnroll("Info") = "<li>" + udtSchemeEForm.SchemeDescChi + "</li>"
                            End If
                            dtSchemeToEnroll.Rows.Add(drSchemeToEnroll)

                            If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes AndAlso _
                                dtPractice.Rows(e.Row.RowIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                drSchemeToEnroll = dtSchemeToEnroll.NewRow
                                drSchemeToEnroll("Info") = "&nbsp;&nbsp;&nbsp;&nbsp;(" + GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting") + ")"
                                dtSchemeToEnroll.Rows.Add(drSchemeToEnroll)
                            End If

                            'Determine show or hide the row of Service Fee
                            If Not IsNothing(Session(udtSchemeEForm.SchemeCode.Trim)) Then
                                If CStr(Session(udtSchemeEForm.SchemeCode.Trim)).Equals("Y") Then
                                    If dtPractice.Rows(e.Row.RowIndex)(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes And _
                                        dtPractice.Rows(e.Row.RowIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then

                                        Dim blnShowPracticeScheme = False
                                        For Each udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                            If udtSubsidizeGroupEFormModel.ServiceFeeEnabled Then
                                                trServiceFee.Visible = True
                                                blnShowPracticeScheme = True
                                            End If
                                        Next

                                        If blnShowPracticeScheme Then
                                            udtFilteredSchemeEFormList.Add(udtSchemeEForm)
                                        End If

                                    End If
                                End If
                            End If

                        End If
                    Next

                    Dim gvSchemeToEnroll As GridView = CType(e.Row.FindControl("gvSchemeToEnroll"), GridView)

                    gvSchemeToEnroll.DataSource = dtSchemeToEnroll
                    gvSchemeToEnroll.DataBind()
                End If

                'Row for Service Fee
                If trServiceFee.Visible Then

                    Dim intPracticeIndex As Integer = CInt(lblPracticeIndex.Text.Trim) - 1

                    Dim intCountScheme As Integer = 0

                    For Each udtSchemeEForm As SchemeEFormModel In udtFilteredSchemeEFormList
                        '4. Line break for more than 1 Practice Scheme
                        If intCountScheme > 0 Then
                            pnlServiceFee.Controls.Add(New LiteralControl("<br/>"))
                        End If


                        '1. Scheme Title 
                        Dim lblPracticeSchemeText As New Label

                        If selectedLanguageValue.Equals(English) Then
                            lblPracticeSchemeText.Text = udtSchemeEForm.SchemeDesc.Trim
                        Else
                            lblPracticeSchemeText.Text = udtSchemeEForm.SchemeDescChi.Trim
                        End If
                        lblPracticeSchemeText.CssClass = "tableText"
                        lblPracticeSchemeText.Style.Add("padding-bottom", "1px")

                        pnlServiceFee.Controls.Add(lblPracticeSchemeText)

                        pnlServiceFee.Controls.Add(New LiteralControl("<br/>"))

                        '2. Table for service fee
                        Dim tbl As New Table

                        tbl.BorderStyle = BorderStyle.Solid
                        tbl.BorderWidth = "1"
                        tbl.CellPadding = "4"
                        tbl.CellSpacing = "1"
                        tbl.GridLines = GridLines.Both
                        tbl.CssClass = "confirmDetails"
                        tbl.Style.Add("margin-left", "20px")

                        Dim rw As TableRow
                        Dim cll As TableCell

                        rw = New TableRow

                        cll = New TableCell
                        cll.Text = GetGlobalResourceObject("Text", "Category")
                        cll.BackColor = System.Drawing.ColorTranslator.FromHtml("#F5F5F5")
                        cll.VerticalAlign = VerticalAlign.Top
                        cll.HorizontalAlign = HorizontalAlign.Left
                        cll.Width = Unit.Pixel(300)
                        rw.Controls.Add(cll)

                        cll = New TableCell
                        cll.Text = GetGlobalResourceObject("Text", "Vaccine")
                        cll.BackColor = System.Drawing.ColorTranslator.FromHtml("#F5F5F5")
                        cll.VerticalAlign = VerticalAlign.Top
                        cll.HorizontalAlign = HorizontalAlign.Left
                        cll.Width = Unit.Pixel(100)
                        rw.Controls.Add(cll)

                        cll = New TableCell
                        cll.Text = GetGlobalResourceObject("Text", "ServiceFee")
                        cll.BackColor = System.Drawing.ColorTranslator.FromHtml("#F5F5F5")
                        cll.VerticalAlign = VerticalAlign.Top
                        cll.HorizontalAlign = HorizontalAlign.Left
                        cll.Width = Unit.Pixel(100)
                        rw.Controls.Add(cll)

                        tbl.Rows.Add(rw)

                        Dim intCountTableRow As Integer = 0

                        Dim dtCategory As DataTable
                        dtCategory = setupDataTable(DT_NAME.DT_NAME_CATEGORY)

                        'Prepare setting for display category in each practice

                        For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList

                            If udtSubsidizeGroupEForm.ServiceFeeEnabled Then
                                If Not IsNothing(dtPractice.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided")) Then
                                    If dtPractice.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided").Equals(YesNo.Yes) Then

                                        Dim drCategory As DataRow

                                        Dim intCountCategory As Integer = dtCategory.Select(DT_NAME.DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ + " = " + udtSubsidizeGroupEForm.CategoryDisplaySeq.ToString).GetLength(0)

                                        If intCountCategory > 0 Then
                                            drCategory = dtCategory.NewRow
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_SCHEME) = udtSubsidizeGroupEForm.SchemeCode.Trim
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ) = udtSubsidizeGroupEForm.CategoryDisplaySeq.ToString.Trim
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SEQ) = intCountCategory + 1
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN) = 0
                                            dtCategory.Rows.Add(drCategory)

                                            If intCountTableRow - intCountCategory >= 0 Then
                                                dtCategory.Rows(intCountTableRow - intCountCategory)(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN) += 1
                                            End If
                                        Else
                                            drCategory = dtCategory.NewRow
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_SCHEME) = udtSubsidizeGroupEForm.SchemeCode.Trim
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ) = udtSubsidizeGroupEForm.CategoryDisplaySeq.ToString.Trim
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SEQ) = 1
                                            drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN) = 1
                                            dtCategory.Rows.Add(drCategory)
                                        End If

                                        intCountTableRow = intCountTableRow + 1
                                    End If
                                End If
                            End If
                        Next

                        'Generate HTML table by above setting
                        If dtCategory.Rows.Count > 0 Then
                            intCountTableRow = 0

                            For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList

                                If udtSubsidizeGroupEForm.ServiceFeeEnabled Then
                                    If Not IsNothing(dtPractice.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided")) Then
                                        If dtPractice.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided").Equals(YesNo.Yes) Then
                                            Dim strServiceFee As String = dtPractice.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_ServiceFee")
                                            Dim strNoServiceFee As String = dtPractice.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsNoServiceFee")

                                            rw = New TableRow

                                            'Cell 1: Category
                                            If intCountTableRow >= 0 And intCountTableRow < dtCategory.Rows.Count Then
                                                If Not dtCategory.Rows(intCountTableRow)(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN).Equals(0) Then
                                                    cll = New TableCell

                                                    If selectedLanguageValue.Equals(English) Then
                                                        cll.Text = udtSubsidizeGroupEForm.CategoryName.Trim
                                                    Else
                                                        cll.Text = udtSubsidizeGroupEForm.CategoryNameChi.Trim
                                                    End If

                                                    cll.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
                                                    cll.VerticalAlign = VerticalAlign.Top
                                                    cll.RowSpan = dtCategory.Rows(intCountTableRow)(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN).ToString.Trim
                                                    rw.Controls.Add(cll)
                                                End If
                                            End If

                                            'Cell 2: Subsidy
                                            cll = New TableCell

                                            If selectedLanguageValue.Equals(English) Then
                                                cll.Text = Replace(udtSubsidizeGroupEForm.SubsidizeDisplayCode.Trim, "#", "").Trim
                                            Else
                                                cll.Text = Replace(udtSubsidizeGroupEForm.SubsidizeDisplayCodeChi.Trim, "#", "").Trim
                                            End If

                                            cll.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
                                            cll.VerticalAlign = VerticalAlign.Top
                                            rw.Controls.Add(cll)

                                            'Cell 3: Service Fee
                                            cll = New TableCell

                                            If strNoServiceFee.Trim.Equals(YesNo.Yes) Then
                                                If selectedLanguageValue.Equals(English) Then
                                                    cll.Text = udtSubsidizeGroupEForm.ServiceFeeCompulsoryWording.Trim
                                                Else
                                                    cll.Text = udtSubsidizeGroupEForm.ServiceFeeCompulsoryWordingChi.Trim
                                                End If
                                            Else
                                                If Not strServiceFee.Equals(String.Empty) Then
                                                    cll.Text = udtFormatter.formatMoney(strServiceFee, True)
                                                End If
                                            End If

                                            cll.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
                                            cll.VerticalAlign = VerticalAlign.Top
                                            cll.Wrap = False
                                            rw.Controls.Add(cll)

                                            tbl.Controls.Add(rw)

                                            intCountTableRow += 1
                                        End If
                                    End If
                                End If
                            Next
                        End If

                        pnlServiceFee.Controls.Add(tbl)

                        intCountScheme += 1

                    Next
                End If

            End If
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If

    End Sub

    Protected Sub lnkEditPersonalParticulars_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_PersonalParticular) = "Y"
        Response.Redirect("~/PersonalPacticulars.aspx")
    End Sub

    Protected Sub lnkEditMO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_MedicalOrganization) = "Y"
        Response.Redirect("~/MedicalOrganization.aspx")
    End Sub

    Protected Sub lnkEditPracitce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_Practice) = "Y"
        Response.Redirect("~/Practice.aspx")
    End Sub

    Protected Sub lnkEditBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_Bank) = "Y"
        Response.Redirect("~/BankAccount.aspx")
    End Sub

    Protected Sub lnkEditScheme_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_SchemeSelection) = "Y"
        Response.Redirect("~/SchemeSelection.aspx")
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

    ' CRE12-001 eHS and PCD integration [Start][Tommy]

    Private Function GetTypeOfPracticeName(ByVal code As String)
        Dim selectedLanguageValue As String = LCase(Session("language"))
        Dim strName As String = String.Empty
        If selectedLanguageValue.Equals(TradChinese) Then
            strName = Common.Component.PracticeType_PCD.PracticeType_PCDBLL.GetPracticeTypeByCode(code).DataValueChi
        Else
            strName = Common.Component.PracticeType_PCD.PracticeType_PCDBLL.GetPracticeTypeByCode(code).DataValue
        End If
        Return strName
    End Function

    Private Function blnShowConfirmBtn() As Boolean
        Dim beShowConfirmBtn As Boolean = True
        beShowConfirmBtn = chkConfirmDetails.Checked

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' ==========================================================
        If udtSP.JoinPCD.Equals(Common.Component.JoinPCDStatus.Yes) And Not chkConfirmPCDConditions.Checked Then
            beShowConfirmBtn = False
        End If
        ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

        Return beShowConfirmBtn
    End Function

    Private Sub showConfirmBtn()
        If blnShowConfirmBtn() Then
            Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
            Me.ibtnConfirm.Enabled = True
        Else
            Me.ibtnConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
            Me.ibtnConfirm.Enabled = False
        End If
    End Sub

    Public Sub ToPCD()
        Dim PCDWS As New PCDWebService(LocalFunctionCode)
        Dim objEnrollRecord As ThirdParty.ThirdPartyEnrollRecordModel = Nothing

        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSP As ServiceProviderModel = udtSPBLL.GetSP

        'If Not udtSPBLL.GetSP Is Nothing Then
        If Not udtSP Is Nothing Then

            udtSP.PracticeList = udtSP.PracticeList.FilterByPCD(TableLocation.Enrolment)
            'objEnrollRecord = PCDWS.CreateEnrolRecord(udtSPBLL.GetSP, New Common.DataAccess.Database)
            objEnrollRecord = PCDWS.CreateEnrolRecord(udtSP, New Common.DataAccess.Database)
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

            If Not objEnrollRecord Is Nothing Then

                ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Session("SP") = udtSP
                ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]


                Session("PCD_ERN") = Common.PCD.ComFunction.FormatPCDEnrolRefNo(objEnrollRecord.EnrolmentRefNo)
                Session("PCD_SubmissionTime") = udtFormatter.convertDateTime(objEnrollRecord.EnrolmentSubmissionDate)

                udtAuditLogEntry.AddDescripton("PCD_ERN", Session("PCD_ERN"))
                udtAuditLogEntry.AddDescripton("PCD_SubmissionTime", Session("PCD_SubmissionTime"))
            End If
        End If
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00071, "Get PCD Enrolment Information")
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

    ' CRE12-001 eHS and PCD integration [End][Tommy]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function setupDataTable(dtName As String) As DataTable
        Dim dt As DataTable = Nothing
        Dim dc As DataColumn

        Select Case dtName
            Case DT_NAME.DT_NAME_CATEGORY
                dt = New DataTable(DT_NAME.DT_NAME_CATEGORY)
                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_CATEGORY_SCHEME, Type.GetType("System.String"))
                dt.Columns.Add(dc)

                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ, Type.GetType("System.Int32"))
                dt.Columns.Add(dc)

                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SEQ, Type.GetType("System.Int32"))
                dt.Columns.Add(dc)

                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN, Type.GetType("System.Int32"))
                dt.Columns.Add(dc)
        End Select

        Return dt
    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]


    Protected Sub gvSchemeToEnroll_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Style.Add("padding-left", "0px")
            e.Row.Cells(0).Style.Add("padding-top", "0px")
            e.Row.Cells(0).Style.Add("padding-right", "0px")
            e.Row.Cells(0).Style.Add("padding-bottom", "1px")
        End If
    End Sub
End Class