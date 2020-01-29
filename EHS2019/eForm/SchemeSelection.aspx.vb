Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.Address
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.PracticeType_PCD
Imports Common.Component.Practice

Imports Common.Component.BankAcct
Imports Common.Component.Professional
Imports Common.Component.District
Imports Common.Component.Area
Imports Common.Component.SchemeInformation
Imports Common.Component.MedicalOrganization

Imports Common.Component.ERNProcessed
Imports Common.Component.PracticeSchemeInfo

Imports Common.Component.StaticData

Imports Common.Component.Profession
Imports Common.Component.UserAC

Imports Common.PCD

Partial Public Class SchemeSelection
    'Inherits System.Web.UI.Page
    Inherits BasePage

#Region "Constant"

    Private Const LocalFunctionCode As String = FunctCode.FUNT020101
    Private Const GlobalFunctionCode As String = FunctCode.FUNT990000
    Private Const DatabaseFunctionCode As String = FunctCode.FUNT990001
    Private Const SESS_Practice As String = "PracticeBank"
    Private Const SESS_MO As String = "MO"
    Private Const SESS_PerviousPage As String = "PerviousPage"
    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Const SESS_Validation As String = "ValidationOnServiceFee"
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE15-004 (TIV and QIV) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Const VS_MESSAGEBOX_CODETABLE As String = "VS_MESSAGEBOX_CODETABLE"
    'CRE15-004 (TIV and QIV) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Class DT_NAME
        Public Const DT_NAME_SUBSIDY As String = "DATATABLE_SUBSIDY_DESCRIPTION_DISPLAY_SETTING"
        Public Const DT_NAME_CATEGORY As String = "DATATABLE_CATEGORY_DISPLAY_SETTING"
        Public Const DT_NAME_ENROLLED_SCHEME As String = "DATATABLE_ENROLLED_SCHEME_DISPLAY_SETTING"

        Public Const DT_FIELD_NAME_DISPLAY_SEQ As String = "DISPLAY_SEQ"
        Public Const DT_FIELD_NAME_SUBSIDIZE_CODE As String = "SUBSIDIZE_CODE"
        Public Const DT_FIELD_NAME_SUBSIDIZE_DESC As String = "SUBSIDIZE_DESC"
        Public Const DT_FIELD_NAME_SUBSIDIZE_DESC_CHI As String = "SUBSIDIZE_DESC_CHI"

        Public Const DT_FIELD_NAME_CATEGORY_SCHEME As String = "CATEGORY_SCHEME"
        Public Const DT_FIELD_NAME_CATEGORY_ROW_SEQ As String = "CATEGORY_ROW_SEQ"
        Public Const DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ As String = "CATEGORY_DISPLAY_SEQ"
        Public Const DT_FIELD_NAME_CATEGORY_ROW_SPAN As String = "CATEGORY_ROW_SPAN"

        Public Const DT_FIELD_NAME_ENROLLED_SCHEME_CODE As String = "ENROLLED_SCHEME_CODE"
        Public Const DT_FIELD_NAME_ENROLLED_SCHEME_DESC As String = "ENROLLED_SCHEME_DESC"
        Public Const DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM As String = "ENROLLED_SCHEME_DISPLAY_ITEM"

    End Class
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    Private Const MaxRowNo As Integer = 5
#End Region

    Private udtValidator As Common.Validation.Validator = New Common.Validation.Validator
    Private udtFormatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtSM As Common.ComObject.SystemMessage
    Private udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

    Private udtControlBLL As ControlBLL = New ControlBLL
    Private udtEFormBLL As eFormBLL = New eFormBLL
    Private udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
    Private udtSchemeEFormBLL As SchemeEFormBLL = New SchemeEFormBLL
    Private udtPracticeType_PCDBLL As PracticeType_PCDBLL = New PracticeType_PCDBLL

    Private strCurrentProf As String = String.Empty
    Private blnServiceFeeCompulsory As Boolean = False
    Private intServiceFeeCompulsoryText As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Dim strProf As String = String.Empty
        Dim dtmCurrentDtm As DateTime = udtGeneralFunction.GetSystemDateTime
        Dim blnShowScheme As Boolean = False
        Dim blnShowList As Boolean = False
        Dim blnEnrolDtm As Boolean = False

        Dim intNoOfChk As Integer = 0
        Dim strChkBox As String = String.Empty

        Dim dtPractice As DataTable = Session(SESS_Practice)
        If Not IsNothing(dtPractice) Then
            strProf = CStr(dtPractice.Rows(0).Item("ServiceCategoryCode")).Trim
            strCurrentProf = strProf
        End If

        Dim selectedLanguageValue As String
        selectedLanguageValue = LCase(Session("language"))

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Generate dynamic checkbox list for selected scheme
        Dim blnTable As Boolean = False
        Dim tbl As New Table
        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
            Dim udtSchemeEFormList As SchemeEFormModelCollection

            udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

            For Each udtschemeEForm As SchemeEFormModel In udtSchemeEFormList
                If udtschemeEForm.EligibleProfesional(strProf) Then

                    Dim blnChkBoxChecked As Boolean = False
                    If dtPractice.Rows.Count > 0 Then
                        For Each dr As DataRow In dtPractice.Rows
                            If CStr(dr.Item(udtschemeEForm.SchemeCode + "_EligibleForProfession")).Trim = YesNo.Yes And _
                                CStr(dr.Item(udtschemeEForm.SchemeCode + "_Selected")).Trim = YesNo.Yes Then
                                blnChkBoxChecked = True
                            End If
                        Next
                    End If

                    Dim rw As TableRow
                    Dim cl As TableCell

                    'Scheme
                    Dim chkBox As New CheckBox
                    chkBox.Enabled = False
                    chkBox.Checked = blnChkBoxChecked
                    chkBox.ID = "chk" & udtschemeEForm.SchemeCode.Trim

                    Dim lblScheme As New Label
                    If selectedLanguageValue.Equals(English) Then
                        lblScheme.Text = udtschemeEForm.SchemeDesc
                    Else
                        lblScheme.Text = udtschemeEForm.SchemeDescChi
                    End If
                    'chkBox.AutoPostBack = True
                    'AddHandler chkBox.CheckedChanged, AddressOf chkMScheme_CheckedChanged

                    blnTable = True

                    rw = New TableRow
                    cl = New TableCell
                    cl.Controls.Add(chkBox)
                    cl.Controls.Add(lblScheme)

                    intNoOfChk = intNoOfChk + 1

                    strChkBox = chkBox.ID

                    'Subsidy
                    If udtschemeEForm.DisplaySubsidizeDesc Then
                        If Not IsNothing(udtschemeEForm.SubsidizeGroupEFormList) Then

                            'Sorting
                            Dim dtSubsidy As DataTable = setupDataTable(DT_NAME.DT_NAME_SUBSIDY)

                            Dim drSubsidy As DataRow

                            For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtschemeEForm.SubsidizeGroupEFormList
                                'Prepare setting for display subsidy description
                                If udtSubsidizeGroupEForm.DisplaySubsidizeDesc Then
                                    If Not dtSubsidy.Select(DT_NAME.DT_FIELD_NAME_DISPLAY_SEQ + " = " + udtSubsidizeGroupEForm.DisplaySeq.ToString).GetLength(0) > 0 Then
                                        drSubsidy = dtSubsidy.NewRow
                                        drSubsidy(DT_NAME.DT_FIELD_NAME_DISPLAY_SEQ) = udtSubsidizeGroupEForm.DisplaySeq.ToString.Trim
                                        drSubsidy(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_CODE) = udtSubsidizeGroupEForm.SubsidizeCode.ToString.Trim
                                        drSubsidy(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_DESC) = udtSubsidizeGroupEForm.SubsidizeItemDesc.Trim
                                        drSubsidy(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_DESC_CHI) = udtSubsidizeGroupEForm.SubsidizeItemDescChi.Trim
                                        dtSubsidy.Rows.Add(drSubsidy)
                                    End If
                                End If
                            Next

                            'Add Subsidy Description
                            'For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtschemeEForm.SubsidizeGroupEFormList
                            For Each dr As DataRow In dtSubsidy.Select("", DT_NAME.DT_FIELD_NAME_DISPLAY_SEQ + " ASC")
                                Dim newLi As New HtmlGenericControl
                                'newLi.ID = "liScheme" & udtSubsidizeGroupEForm.SubsidizeCode.Trim
                                newLi.ID = "liScheme" & dr(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_CODE).ToString.Trim
                                newLi.TagName = "li"
                                newLi.Attributes.Add("style", "margin-left:25pt")

                                If selectedLanguageValue.Equals(English) Then
                                    'newLi.InnerText = udtSubsidizeGroupEForm.SubsidizeItemDesc.Trim
                                    newLi.InnerText = dr(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_DESC).ToString.Trim
                                Else
                                    'newLi.InnerText = udtSubsidizeGroupEForm.SubsidizeItemDescChi.Trim
                                    newLi.InnerText = dr(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_DESC_CHI).ToString.Trim
                                End If
                                cl.Controls.Add(newLi)

                            Next

                            rw.Controls.Add(cl)
                            tbl.Controls.Add(rw)
                        End If
                    Else
                        rw.Controls.Add(cl)
                        tbl.Controls.Add(rw)
                    End If
                Else
                    Session(udtschemeEForm.SchemeCode.Trim) = "N"
                End If
            Next

            If blnTable Then
                pnlSchemeSelection.Controls.Add(tbl)
            End If

            getSchemeFromPanel()

        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        If Not IsPostBack Then
            Dim strAbleToAccessThisPage As String = String.Empty
            strAbleToAccessThisPage = Session(eFormBLL.SESS_SchemeSelection)
            udtEFormBLL.ClearRedirectPageSession()

            If IsNothing(strAbleToAccessThisPage) OrElse Not strAbleToAccessThisPage.Trim.Equals("Y") Then
                Response.Redirect("~/main.aspx")

            Else
                Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me) ''Begin Writing Audit Log

                Dim udtTSP As ServiceProvider.ServiceProviderModel
                udtTSP = udtSPBLL.GetSP

                udtAuditLogEntry.AddDescripton("HKID", udtTSP.HKID)
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00045, "Scheme Selection Page Loaded")

                If Not IsNothing(Session(SESS_PerviousPage)) Then

                    ' CRE12-001 eHS and PCD integration [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    LoadEHRSS()
                    ' CRE12-001 eHS and PCD integration [End][Koala]

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim blnShowRemark As Boolean = False

                    Session(SESS_Validation) = Nothing

                    If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                        Dim udtSchemeEFormList As SchemeEFormModelCollection
                        Dim intCount As Integer = 0

                        Dim dtCategory As DataTable

                        dtCategory = setupDataTable(DT_NAME.DT_NAME_CATEGORY)

                        udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                        For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                            If udtSchemeEForm.EligibleProfesional(strCurrentProf) Then
                                If Not IsNothing(udtSchemeEForm.SubsidizeGroupEFormList) Then

                                    For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                        'Calculate whether shows input for service fee
                                        If Not IsNothing(udtSubsidizeGroupEForm) Then

                                            If udtSubsidizeGroupEForm.ServiceFeeEnabled Then
                                                If dtPractice.Rows.Count > 0 Then
                                                    For Each dr As DataRow In dtPractice.Rows
                                                        If CStr(dr.Item(udtSchemeEForm.SchemeCode + "_EligibleForProfession")).Trim = YesNo.Yes And _
                                                            CStr(dr.Item(udtSchemeEForm.SchemeCode + "_Selected")).Trim = YesNo.Yes Then
                                                            blnShowRemark = True
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        End If

                                        'Prepare setting for display category in each practice
                                        If udtSubsidizeGroupEForm.ServiceFeeEnabled Then

                                            Dim drCategory As DataRow

                                            Dim intCountCategory As Integer = dtCategory.Select(DT_NAME.DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ + " = " + udtSubsidizeGroupEForm.CategoryDisplaySeq.ToString).GetLength(0)

                                            If intCountCategory > 0 Then
                                                drCategory = dtCategory.NewRow
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_SCHEME) = udtSchemeEForm.SchemeCode.Trim
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ) = udtSubsidizeGroupEForm.CategoryDisplaySeq.ToString.Trim
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SEQ) = intCountCategory + 1
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN) = 0
                                                dtCategory.Rows.Add(drCategory)

                                                If intCount - intCountCategory >= 0 Then
                                                    dtCategory.Rows(intCount - intCountCategory)(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN) += 1
                                                End If
                                            Else
                                                drCategory = dtCategory.NewRow
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_SCHEME) = udtSchemeEForm.SchemeCode.Trim
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_DISPLAY_SEQ) = udtSubsidizeGroupEForm.CategoryDisplaySeq.ToString.Trim
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SEQ) = 1
                                                drCategory(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN) = 1
                                                dtCategory.Rows.Add(drCategory)
                                            End If

                                            intCount = intCount + 1
                                        End If

                                    Next

                                End If

                            End If
                        Next
                        Session(DT_NAME.DT_NAME_CATEGORY) = dtCategory
                    End If
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]

                    If Not udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
                        Dim udtSubsidizeGroupEFormList As SubsidizeGroupEFormModelCollection

                        udtSubsidizeGroupEFormList = udtSchemeEFormBLL.GetAllEffectiveSubsidizeGroupFromCache
                        udtSchemeEFormBLL.SaveToSession(udtSubsidizeGroupEFormList)
                    End If

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    lblServiceFeeStatement.Visible = False

                    If IsNothing(dtPractice) Then
                        pnlPractice.Visible = False
                    Else
                        blnServiceFeeCompulsory = False

                        pnlPractice.Visible = True
                        Me.gvPractice.DataSource = dtPractice
                        Me.gvPractice.DataBind()

                        If blnShowRemark Then
                            lblServiceFeeStatement.Visible = True
                        End If
                    End If

                'CRE16-002 (Revamp VSS) [End][Chris YIM]
                End If

            End If

            ' CRE12-001 eHS and PCD integration [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Me.iBtnLoadDemoData.Visible = Me.IsDemo
            ' CRE12-001 eHS and PCD integration [End][Koala]

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
        Else
            getServiceFeeFromGridView(False)

            RenderGridview()
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If


        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        If Not IsPostBack Then
            LoadPCDChoice()
        End If

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        If Not Me.IsPostBack Or Me.LanguageChanged Then

            SaveTypeOfPractice(udtSPBLL.GetSP)

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

            udtEFormBLL.UpdateSPModel(udtSPBLL.GetSP, udtSchemeInfoList, dtMO, dtPracticeBank, False)

            ' CRE12-001 eHS and PCD integration [Start][Tommy]

            Me.ucTypeOfPracticeGrid.Mode = eForm.ucTypeOfPracticeGrid.EnumMode.Create
            Me.ucTypeOfPracticeGrid.LoadPractice(udtSPBLL.GetSP)

            Me.udcMsgBox.ScrollToHeight = True

            ' CRE12-001 eHS and PCD integration [End][Tommy]

        End If

    End Sub

    Protected Sub ibtnSchemeSelectBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        'getSchemeFromPanel()

        If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
            getServiceFeeFromGridView(False)
        End If

        If panEHRSS.Visible Then
            If rboHadJoinedEHRSS.SelectedValue.Trim = "Y" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.Yes
            ElseIf rboHadJoinedEHRSS.SelectedValue.Trim = "N" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.No
            Else
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
            End If

        Else
            udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        CheckTypeOfPractice()
        SaveTypeOfPractice(udtSP)

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        If IsNothing(Session(SESS_PerviousPage)) Then
            udtEFormBLL.ClearRedirectPageSession()

            Session(eFormBLL.SESS_Bank) = "Y"
            Response.Redirect("~/BankAccount.aspx")
        Else
            Dim s As String = Session(SESS_PerviousPage)

            udtSP = udtSPBLL.GetSP

            WriteAuditLog(udtSP, udtAuditLogEntry)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00046, "Press Back to Bank Account")

            udtEFormBLL.ClearRedirectPageSession()

            Session(eFormBLL.SESS_Bank) = "Y"
            Response.Redirect("~/BankAccount.aspx")
        End If

    End Sub

    Protected Sub ibtnSchemeSelectNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)
        imgHadJoinedEHRSSAlert.Visible = False
        imgWillJoinPCDAlert.Visible = False
        imgPCDTypeOfPracticeAlert.Visible = False

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        getSchemeFromPanel()
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
            getServiceFeeFromGridView(True)
        End If

        If panEHRSS.Visible Then
            udtSM = udtValidator.chkHadJoinedEHRSS(rboHadJoinedEHRSS.SelectedValue)
            If Not udtSM Is Nothing Then
                imgHadJoinedEHRSSAlert.Visible = True
                udcMsgBox.AddMessage(udtSM)
            Else

                If rboHadJoinedEHRSS.SelectedValue.Trim = "Y" Then

                    udtSP.AlreadyJoinEHR = JoinEHRSSStatus.Yes
                ElseIf rboHadJoinedEHRSS.SelectedValue.Trim = "N" Then

                    udtSP.AlreadyJoinEHR = JoinEHRSSStatus.No
                Else
                    udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
                End If

            End If

        Else
            rboHadJoinedEHRSS.ClearSelection()

            udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        CheckTypeOfPractice()
        SaveTypeOfPractice(udtSP)

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        WriteAuditLog(udtSP, udtAuditLogEntry)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00047, "Input Scheme Selection")

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False

            WriteAuditLog(udtSP, udtAuditLogEntry)

            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00048, "Input Scheme Selection Completed")

            udtEFormBLL.ClearRedirectPageSession()
            Session(eFormBLL.SESS_ConfirmDetails) = "Y"
            Response.Redirect("~/ConfirmDetails.aspx")
        Else
            WriteAuditLog(udtSP, udtAuditLogEntry)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00049, "Input Scheme Selection Fail")
        End If
    End Sub

    ' CRE15-018 Remove PPIePR Enrolment [Start][Winnie]
    'Private Function JoinScheme() As Boolean()
    '    Dim blnRes() As Boolean = {False, False, False}
    '    Dim blnAskHadJoinedPPIePRProfCode As Boolean = False
    '    Dim blnAskWillJoinPPIePRProfCode As Boolean = False
    '    Dim blnAskJoinIVSS As Boolean = False

    '    If Not IsNothing(Session(SESS_Practice)) Then
    '        Dim dt As DataTable
    '        dt = Session(SESS_Practice)

    '        For Each dr As DataRow In dt.Rows

    '            Dim strHealthProf As String = dr.Item("ServiceCategoryCode")
    '            If udtEFormBLL.AskHadJoinedPPIePRProfCode(strHealthProf.Trim) Then
    '                blnAskHadJoinedPPIePRProfCode = True
    '            End If
    '            If udtEFormBLL.AskWillJoinPPIePRProfCode(strHealthProf.Trim) Then
    '                blnAskWillJoinPPIePRProfCode = True
    '            End If
    '        Next

    '    End If

    '    blnRes(0) = blnAskHadJoinedPPIePRProfCode
    '    blnRes(1) = blnAskWillJoinPPIePRProfCode

    '    Return blnRes

    'End Function
    ' CRE15-018 Remove PPIePR Enrolment [End][Winnie]

    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Private Function AbleToJoinIVSSPractice(ByVal dt As DataTable) As Boolean
    '    Dim blnRes As Boolean = False
    '    Dim i As Integer = 0

    '    For Each row As DataRow In dt.Rows
    '        Dim strHealthProf As String = row.Item("ServiceCategoryCode")
    '        If udtEFormBLL.AskJoinIVSS(strHealthProf.Trim) Then
    '            i = i + 1
    '        End If
    '    Next

    '    If i > 0 Then
    '        blnRes = True
    '    End If

    '    Return blnRes

    'End Function
    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

    Private Function getSchemeFromPanel() As Boolean
        Dim blnChecked As Boolean = False

        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
            Dim udtSchemeEFormList As SchemeEFormModelCollection

            udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

            For Each udtschemeEForm As SchemeEFormModel In udtSchemeEFormList
                Dim chkScheme As CheckBox = Me.pnlSchemeSelection.FindControl("chk" + udtschemeEForm.SchemeCode.Trim)

                If Not IsNothing(chkScheme) Then
                    If chkScheme.Checked Then
                        If udtschemeEForm.EligibleProfesional(strCurrentProf.Trim) Then
                            Session(udtschemeEForm.SchemeCode.Trim) = "Y"
                            blnChecked = True
                        Else
                            Session(udtschemeEForm.SchemeCode.Trim) = "N"
                        End If

                    Else
                        Session(udtschemeEForm.SchemeCode.Trim) = "N"
                    End If
                End If
            Next

        End If

        Return blnChecked
    End Function

    Private Sub WriteAuditLogMScheme(ByVal udtAuditLogEntry As AuditLogEntry)
        Dim strSelectedScheme As String = String.Empty
        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
            Dim udtSchemeEFormList As SchemeEFormModelCollection

            udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

            For Each udtschemeEForm As SchemeEFormModel In udtSchemeEFormList
                Dim chkScheme As CheckBox = Me.pnlSchemeSelection.FindControl("chk" + udtschemeEForm.SchemeCode.Trim)

                If Not IsNothing(chkScheme) Then
                    If chkScheme.Checked Then
                        strSelectedScheme = strSelectedScheme + ", " + udtschemeEForm.SchemeCode.Trim
                    End If
                End If

            Next
        End If

        If strSelectedScheme.Length > 2 Then
            strSelectedScheme = strSelectedScheme.Substring(2)
        End If

        udtAuditLogEntry.AddDescripton("Selected Scheme", strSelectedScheme)
    End Sub

    Private Function createDataTableForServiceFeeFromGridView() As DataTable
        Dim dt As DataTable
        Dim dc As DataColumn

        dt = New DataTable("TableValdiation")

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.Int32")
        dc.ColumnName = "TableValdiationNo"
        dt.Columns.Add(dc)

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.Int32")
        dc.ColumnName = "PracticeRowNo"
        dt.Columns.Add(dc)

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.Int32")
        dc.ColumnName = "PracticeSchemeRowNo"
        dt.Columns.Add(dc)
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.Int32")
        dc.ColumnName = "SubsidyRowNo"
        dt.Columns.Add(dc)

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.String")
        dc.ColumnName = "SchemeCode"
        dt.Columns.Add(dc)

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.String")
        dc.ColumnName = "SubsidizeCode"
        dt.Columns.Add(dc)

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.String")
        dc.ColumnName = "ServiceFeeEnabled"
        dt.Columns.Add(dc)

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.String")
        dc.ColumnName = "SubsidyCompulsory"
        dt.Columns.Add(dc)

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.String")
        dc.ColumnName = "ServiceFeeCompulsory"
        dt.Columns.Add(dc)

        dc = New DataColumn()
        dc.DataType = System.Type.GetType("System.String")
        dc.ColumnName = "ShowAlertImage"
        dt.Columns.Add(dc)
        Return dt
    End Function

    Private Sub getServiceFeeFromGridView(ByVal checking As Boolean)

        Dim dtValidation As DataTable = Nothing

        If checking Then
            dtValidation = createDataTableForServiceFeeFromGridView()
            Session(SESS_Validation) = Nothing
        End If

        Dim dt As DataTable
        dt = Session(SESS_Practice)

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
            Dim udtSchemeEFormList As SchemeEFormModelCollection

            udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

            Dim intTableValdiationNo As Integer = 0

            For Each r As GridViewRow In gvPractice.Rows
                If r.Visible = True Then
                    Dim strPracticeIndex As String = CType(r.FindControl("hfFormatedPracticeIndex"), HiddenField).Value.Trim
                    Dim strPracticeName As String = CType(r.FindControl("lblRegBankPracticeEName"), Label).Text.Trim

                    Dim intIndex As Integer = CInt(strPracticeIndex)

                    Dim gvPracticeScheme As GridView = CType(r.FindControl("gvPracticeScheme"), GridView)

                    For Each gvr_Parent As GridViewRow In gvPracticeScheme.Rows

                        Dim strSchemeCode As String = String.Empty
                        Dim gvServiceFee As GridView = CType(gvr_Parent.FindControl("gvServiceFee"), GridView)

                        For Each gvr As GridViewRow In gvServiceFee.Rows
                            Dim chkProvideSubsidy As CheckBox = CType(gvr.FindControl("chkProvideSubsidy"), CheckBox)
                            Dim lblSubsidizeDisplayCode As Label = CType(gvr.FindControl("lblSubsidizeDisplayCode"), Label)
                            Dim txtServiceFee As TextBox = CType(gvr.FindControl("txtServiceFee"), TextBox)
                            Dim chkNotProviderServiceFee As CheckBox = CType(gvr.FindControl("chkNotProviderServiceFee"), CheckBox)
                            Dim imgServiceFeeAlert As Image = CType(gvr.FindControl("imgServiceFeeAlert"), Image)
                            Dim hfSchemeCode As HiddenField = CType(gvr.FindControl("hfSchemeCode"), HiddenField)
                            Dim hfSubsidizeCode As HiddenField = CType(gvr.FindControl("hfSubsidizeCode"), HiddenField)
                            'Dim strSubsidizeCode As String = gvr.Cells(3).Text
                            strSchemeCode = hfSchemeCode.Value.Trim
                            Dim strSubsidizeCode As String = hfSubsidizeCode.Value.Trim

                            If checking Then
                                Dim dr As DataRow = dtValidation.NewRow
                                dr("TableValdiationNo") = intTableValdiationNo
                                intTableValdiationNo += 1
                                dr("PracticeRowNo") = intIndex
                                dr("PracticeSchemeRowNo") = gvr_Parent.RowIndex
                                dr("SubsidyRowNo") = gvr.RowIndex
                                dr("SchemeCode") = strSchemeCode
                                dr("SubsidizeCode") = strSubsidizeCode
                                If lblSubsidizeDisplayCode.Visible Then
                                    dr("ServiceFeeEnabled") = YesNo.Yes
                                Else
                                    dr("ServiceFeeEnabled") = YesNo.No
                                End If

                                If chkProvideSubsidy.Visible Then
                                    dr("SubsidyCompulsory") = YesNo.No
                                Else
                                    dr("SubsidyCompulsory") = YesNo.Yes
                                End If

                                If chkNotProviderServiceFee.Visible Then
                                    dr("ServiceFeeCompulsory") = YesNo.No
                                Else
                                    dr("ServiceFeeCompulsory") = YesNo.Yes
                                End If

                                dr("ShowAlertImage") = YesNo.No
                                dtValidation.Rows.Add(dr)
                            End If

                            imgServiceFeeAlert.Visible = False

                            dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsProvided") = String.Empty
                            dt.Rows(intIndex)(strSubsidizeCode.Trim + "_ServiceFee") = String.Empty
                            dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsNoServiceFee") = String.Empty

                            Dim udtSchemeEForm As SchemeEFormModel = udtSchemeEFormList.Filter(strSchemeCode)

                            If dt.Rows(intIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then
                                If Not IsNothing(udtSchemeEForm.SubsidizeGroupEFormList) Then

                                    Dim udtSubsidizeGroupEForm As SubsidizeGroupEFormModel = udtSchemeEForm.SubsidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode)
                                    If strSubsidizeCode.Trim.Equals(udtSubsidizeGroupEForm.SubsidizeCode.Trim) Then

                                        If udtSubsidizeGroupEForm.SubsidyCompulsory Then
                                            dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsProvided") = YesNo.Yes
                                        Else
                                            If chkProvideSubsidy.Checked Then
                                                dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsProvided") = YesNo.Yes
                                            Else
                                                dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsProvided") = YesNo.No
                                            End If
                                        End If

                                        If udtSubsidizeGroupEForm.ServiceFeeCompulsory Then
                                            dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsNoServiceFee") = YesNo.No
                                        Else
                                            If chkNotProviderServiceFee.Checked Then
                                                dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsNoServiceFee") = YesNo.Yes
                                            Else
                                                dt.Rows(intIndex)(strSubsidizeCode.Trim + "_IsNoServiceFee") = YesNo.No
                                            End If
                                        End If

                                        dt.Rows(intIndex)(strSubsidizeCode.Trim + "_ServiceFee") = txtServiceFee.Text.Trim

                                    End If
                                End If
                            End If
                        Next

                        'If checking is true, start the validation
                        If checking Then
                            Dim udtSchemeEForm As SchemeEFormModel = udtSchemeEFormList.Filter(strSchemeCode.Trim)
                            If udtSchemeEForm.SubsidizeGroupEFormList.Count > 0 Then

                                'Validation is grouped by scheme
                                If dt.Rows(intIndex)(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes And _
                                    dt.Rows(intIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then
                                    Dim strProvideSubsidy(udtSchemeEForm.SubsidizeGroupEFormList.Count) As String
                                    Dim intProvideSubsidy As Integer = 0

                                    'Check service fee in each subsidy
                                    For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                        If udtSubsidizeGroupEForm.ServiceFeeEnabled Then
                                            Dim dtRes As DataTable

                                            udtSM = Nothing

                                            dtRes = dtValidation.Select("PracticeRowNo=" & intIndex & " And SubsidizeCode='" & udtSubsidizeGroupEForm.SubsidizeCode.Trim & "'").CopyToDataTable

                                            If dtRes.Rows.Count > 0 Then
                                                'Criteria of Subsidy Compulsory: 
                                                '1. Visible and checked; 2. Invisible
                                                If (Not dtRes.Rows(0).Item("SubsidyCompulsory").Equals(YesNo.Yes) And dt.Rows(intIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided").Equals(YesNo.Yes)) _
                                                    Or dtRes.Rows(0).Item("SubsidyCompulsory").Equals(YesNo.Yes) Then
                                                    'Criteria of Service Fee Compulsory: 
                                                    '1. Visible and checked; 2. Invisible
                                                    If (Not dtRes.Rows(0).Item("ServiceFeeCompulsory").Equals(YesNo.Yes) And Not (dt.Rows(intIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsNoServiceFee").Equals(YesNo.Yes))) _
                                                        Or dtRes.Rows(0).Item("ServiceFeeCompulsory").Equals(YesNo.Yes) Then
                                                        udtSM = udtValidator.chkServiceFee(dt.Rows(intIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_ServiceFee"))
                                                    End If
                                                End If

                                                If Not IsNothing(udtSM) Then

                                                    dtValidation.Rows(CInt(dtRes.Rows(0).Item("TableValdiationNo").ToString.Trim)).Item("ShowAlertImage") = YesNo.Yes
                                                    'udcMsgBox.AddMessage(udtSM, New String() {"%s", "%d"}, New String() {(intIndex + 1).ToString, udtSubsidizeGroupEForm.SubsidizeDisplayCode})
                                                    udcMsgBox.AddMessage(udtSM, _
                                                                         New String() {"%en", "%tc", "%s"}, _
                                                                         New String() {udtSubsidizeGroupEForm.SubsidizeDisplayCode + " for " + udtSubsidizeGroupEForm.CategoryName, _
                                                                                       udtSubsidizeGroupEForm.CategoryNameChi + " " + udtSubsidizeGroupEForm.SubsidizeDisplayCode, _
                                                                                       (intIndex + 1).ToString})
                                                    'udtSM = Nothing

                                                End If
                                            End If

                                            'Collect the result of CheckBox of "Subsidy Compulsory" into string array

                                            'If Not udtSubsidizeGroupEForm.SubsidyCompulsory Then
                                            strProvideSubsidy(intProvideSubsidy) = dt.Rows(intIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided")
                                            intProvideSubsidy += 1
                                            'End If

                                        End If

                                    Next

                                    'Check at least one subsidy is enrolled.
                                    If intProvideSubsidy > 0 Then
                                        udtSM = udtValidator.chkProvideSubsidy(strProvideSubsidy)

                                        If Not udtSM Is Nothing Then

                                            udcMsgBox.AddMessage(udtSM, New String() {"%en", "%tc", "%d"}, New String() {udtSchemeEForm.SchemeDesc, udtSchemeEForm.SchemeDescChi, (intIndex + 1).ToString})

                                            For Each dr As DataRow In dtValidation.Select("SchemeCode ='" & udtSchemeEForm.SchemeCode.Trim & "' AND PracticeRowNo=" & intIndex)
                                                dtValidation.Rows(CInt(dr("TableValdiationNo").ToString.Trim)).Item("ShowAlertImage") = YesNo.Yes
                                            Next

                                        End If
                                    End If
                                End If

                            End If

                        End If

                    Next
                End If
            Next

        End If

        'If has/have alert(s), raise the message box and alert image(s)
        If checking Then

            Dim dtResValidation As DataTable

            Dim drResValidation() As DataRow = dtValidation.Select("ShowAlertImage ='" & YesNo.Yes & "'")

            If drResValidation.Length > 0 Then
                dtResValidation = drResValidation.CopyToDataTable

                For Each dr As DataRow In dtResValidation.Select()
                    Dim gvSF As GridView = CType(CType(gvPractice.Rows(dr("PracticeRowNo")).FindControl("gvPracticeScheme"), GridView).Rows(dr("PracticeSchemeRowNo")).FindControl("gvServiceFee"), GridView)

                    Dim imgServiceFeeAlert As Image = CType(gvSF.Rows(dr("SubsidyRowNo")).FindControl("imgServiceFeeAlert"), Image)
                    imgServiceFeeAlert.Visible = True
                Next

                Session(SESS_Validation) = dtResValidation
            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Session(SESS_Practice) = dt
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not IsNothing(Session(SESS_PerviousPage)) Then
            Dim s As String = Session(SESS_PerviousPage)

            If s.Equals("Practice") Then
                lnkBtnMO.Enabled = True
                lnkBtnPractice.Enabled = True
                lnkBtnBank.Enabled = False
            ElseIf s.Equals("Bank") OrElse s.Equals("CompleteBank") Then
                lnkBtnMO.Enabled = True
                lnkBtnPractice.Enabled = True
                lnkBtnBank.Enabled = True
            ElseIf s.Equals("MO") Then
                lnkBtnMO.Enabled = False
                lnkBtnPractice.Enabled = False
                lnkBtnBank.Enabled = False
            End If
        End If

        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) Then
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                getServiceFeeFromGridView(False)

                RenderGridview()
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            End If
        End If
    End Sub

    Private Sub RenderGridview()
        Dim selectedLanguageValue As String
        selectedLanguageValue = LCase(Session("language"))

        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then

            Dim udtSchemeEFormList As SchemeEFormModelCollection

            udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            For Each gvr As GridViewRow In Me.gvPractice.Rows

                Dim dtSchemeToEnroll As New DataTable(DT_NAME.DT_NAME_ENROLLED_SCHEME)
                Dim dcSchemeToEnroll As DataColumn
                Dim drSchemeToEnroll As DataRow

                Dim blnShowInputServiceFee As Boolean = False

                Dim dtPractice As DataTable = Session(SESS_Practice)

                If Not dtPractice Is Nothing Then
                    Dim drPractice As DataRow = dtPractice.Rows(gvr.RowIndex)

                    dcSchemeToEnroll = New DataColumn(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE, Type.GetType("System.String"))
                    dtSchemeToEnroll.Columns.Add(dcSchemeToEnroll)
                    dcSchemeToEnroll = New DataColumn(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC, Type.GetType("System.String"))
                    dtSchemeToEnroll.Columns.Add(dcSchemeToEnroll)
                    dcSchemeToEnroll = New DataColumn(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM, Type.GetType("System.String"))
                    dtSchemeToEnroll.Columns.Add(dcSchemeToEnroll)


                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        If drPractice(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes And _
                            drPractice(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then

                            Dim blnShowPracticeScheme As Boolean = False

                            'Determine whether show Service Fee for Input
                            For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                'Calculate whether shows input for service fee
                                If Not IsNothing(udtSubsidizeGroupEForm) Then
                                    If udtSubsidizeGroupEForm.ServiceFeeEnabled Then
                                        blnShowInputServiceFee = True
                                        blnShowPracticeScheme = True
                                    End If
                                End If
                            Next

                            'Prepare Scheme to Enroll
                            drSchemeToEnroll = dtSchemeToEnroll.NewRow

                            If blnShowPracticeScheme Then
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE) = udtSchemeEForm.SchemeCode

                                If selectedLanguageValue.Equals(English) Then
                                    drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC) = udtSchemeEForm.SchemeDesc
                                Else
                                    drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC) = udtSchemeEForm.SchemeDescChi
                                End If
                            End If

                            If selectedLanguageValue.Equals(English) Then
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM) = "<li>" + udtSchemeEForm.SchemeDesc + "</li>"
                            Else
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM) = "<li>" + udtSchemeEForm.SchemeDescChi + "</li>"
                            End If

                            dtSchemeToEnroll.Rows.Add(drSchemeToEnroll)

                            'If Non-Clinic, add remark
                            If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes AndAlso _
                                dtPractice.Rows(gvr.RowIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                drSchemeToEnroll = dtSchemeToEnroll.NewRow
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM) = "&nbsp;&nbsp;&nbsp;&nbsp;(" + GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting") + ")"
                                dtSchemeToEnroll.Rows.Add(drSchemeToEnroll)
                            End If

                        End If
                    Next

                    Dim gvSchemeToEnroll As GridView = CType(gvr.FindControl("gvSchemeToEnroll"), GridView)

                    gvSchemeToEnroll.DataSource = dtSchemeToEnroll
                    gvSchemeToEnroll.DataBind()
                End If

                Dim pnlInputServiceFee As Panel = CType(gvr.FindControl("pnlInputServiceFee"), Panel)
                If Not pnlInputServiceFee Is Nothing Then
                    If blnShowInputServiceFee Then

                        Dim dtFilteredScheme As DataTable = dtSchemeToEnroll.Clone
                        dtFilteredScheme.Columns.Remove(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM)

                        For Each dr As DataRow In dtSchemeToEnroll.Select(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE + "<>''")
                            Dim drFilteredScheme As DataRow = dtFilteredScheme.NewRow

                            drFilteredScheme(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE) = dr(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE)
                            drFilteredScheme(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC) = dr(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC)

                            dtFilteredScheme.Rows.Add(drFilteredScheme)
                        Next

                        Dim gvPracticeScheme As GridView = CType(gvr.FindControl("gvPracticeScheme"), GridView)

                        gvPracticeScheme.DataSource = dtFilteredScheme
                        gvPracticeScheme.DataBind()

                        pnlInputServiceFee.Visible = True
                    Else
                        pnlInputServiceFee.Visible = False
                    End If
                End If

            Next
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
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

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim selectedLanguageValue As String
            selectedLanguageValue = LCase(Session("language"))

            Dim dtSchemeToEnroll As New DataTable(DT_NAME.DT_NAME_ENROLLED_SCHEME)
            Dim dcSchemeToEnroll As DataColumn
            Dim drSchemeToEnroll As DataRow

            Dim blnShowInputServiceFee As Boolean = False

            Dim dtPractice As DataTable = Session(SESS_Practice)

            If Not dtPractice Is Nothing Then
                Dim drPractice As DataRow = dtPractice.Rows(e.Row.RowIndex)

                dcSchemeToEnroll = New DataColumn(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE, Type.GetType("System.String"))
                dtSchemeToEnroll.Columns.Add(dcSchemeToEnroll)
                dcSchemeToEnroll = New DataColumn(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC, Type.GetType("System.String"))
                dtSchemeToEnroll.Columns.Add(dcSchemeToEnroll)
                dcSchemeToEnroll = New DataColumn(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM, Type.GetType("System.String"))
                dtSchemeToEnroll.Columns.Add(dcSchemeToEnroll)

                If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
                    Dim udtSchemeEFormList As SchemeEFormModelCollection
                    udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        If drPractice(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes And _
                            drPractice(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then

                            Dim blnShowPracticeScheme As Boolean = False

                            'Determine whether show Service Fee for Input
                            For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                'Calculate whether shows input for service fee
                                If Not IsNothing(udtSubsidizeGroupEForm) Then
                                    If udtSubsidizeGroupEForm.ServiceFeeEnabled Then
                                        blnShowInputServiceFee = True
                                        blnShowPracticeScheme = True
                                    End If
                                End If
                            Next

                            'Prepare Scheme to Enroll
                            drSchemeToEnroll = dtSchemeToEnroll.NewRow

                            If blnShowPracticeScheme Then
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE) = udtSchemeEForm.SchemeCode

                                If selectedLanguageValue.Equals(English) Then
                                    drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC) = udtSchemeEForm.SchemeDesc
                                Else
                                    drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC) = udtSchemeEForm.SchemeDescChi
                                End If
                            End If

                            If selectedLanguageValue.Equals(English) Then
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM) = "<li>" + udtSchemeEForm.SchemeDesc + "</li>"
                            Else
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM) = "<li>" + udtSchemeEForm.SchemeDescChi + "</li>"
                            End If

                            dtSchemeToEnroll.Rows.Add(drSchemeToEnroll)

                            'If Non-Clinic, add remark
                            If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes AndAlso _
                                dtPractice.Rows(e.Row.RowIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                drSchemeToEnroll = dtSchemeToEnroll.NewRow
                                drSchemeToEnroll(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM) = "&nbsp;&nbsp;&nbsp;&nbsp;(" + GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting") + ")"
                                dtSchemeToEnroll.Rows.Add(drSchemeToEnroll)
                            End If

                        End If
                    Next

                End If

                Dim gvSchemeToEnroll As GridView = CType(e.Row.FindControl("gvSchemeToEnroll"), GridView)

                gvSchemeToEnroll.DataSource = dtSchemeToEnroll
                gvSchemeToEnroll.DataBind()
            End If

            Dim pnlInputServiceFee As Panel = CType(e.Row.FindControl("pnlInputServiceFee"), Panel)
            If Not pnlInputServiceFee Is Nothing Then
                If blnShowInputServiceFee Then

                    Dim dtFilteredScheme As DataTable = dtSchemeToEnroll.Clone
                    dtFilteredScheme.Columns.Remove(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM)

                    For Each dr As DataRow In dtSchemeToEnroll.Select(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE + "<>''")
                        Dim drFilteredScheme As DataRow = dtFilteredScheme.NewRow

                        drFilteredScheme(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE) = dr(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE)
                        drFilteredScheme(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC) = dr(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC)

                        dtFilteredScheme.Rows.Add(drFilteredScheme)
                    Next

                    Dim gvPracticeScheme As GridView = CType(e.Row.FindControl("gvPracticeScheme"), GridView)

                    gvPracticeScheme.DataSource = dtFilteredScheme
                    gvPracticeScheme.DataBind()

                    pnlInputServiceFee.Visible = True
                Else
                    pnlInputServiceFee.Visible = False
                End If
            End If

            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If
    End Sub

    Protected Sub gvServiceFee_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowIndex >= 0 Then
            If Not Session(DT_NAME.DT_NAME_CATEGORY) Is Nothing Then
                Dim dt As DataTable = Session(DT_NAME.DT_NAME_CATEGORY)

                Dim gvServiceFee As GridView = CType(sender, GridView)
                Dim gvr_Parent As GridViewRow = gvServiceFee.NamingContainer

                Dim strSchemeCode As String
                Dim udtSubsidizeGroupEForm As SubsidizeGroupEFormModel = CType(e.Row.DataItem, SubsidizeGroupEFormModel)

                If Not udtSubsidizeGroupEForm Is Nothing Then
                    strSchemeCode = udtSubsidizeGroupEForm.SchemeCode.Trim
                    Dim drFilteredCategory() As DataRow = dt.Select(DT_NAME.DT_FIELD_NAME_CATEGORY_SCHEME + "='" + strSchemeCode + "'")
                    'Dim drFilteredCategory() As DataRow = dt.Select("")

                    If drFilteredCategory.Length > 0 AndAlso drFilteredCategory.Length > e.Row.RowIndex Then
                        If Not drFilteredCategory(e.Row.RowIndex)(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN).Equals(0) Then

                            e.Row.Cells(0).RowSpan = drFilteredCategory(e.Row.RowIndex)(DT_NAME.DT_FIELD_NAME_CATEGORY_ROW_SPAN)
                        Else
                            e.Row.Cells(0).Visible = False
                        End If
                    End If
                End If

            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

    End Sub

    Protected Sub gvServiceFee_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim selectedLanguageValue As String
            selectedLanguageValue = LCase(Session("language"))

            Dim gvServiceFee As GridView = CType(sender, GridView)
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim gvr_Parent As GridViewRow = gvServiceFee.NamingContainer
            Dim gvr_Parent_Parent As GridViewRow = gvServiceFee.NamingContainer.NamingContainer.NamingContainer

            Dim lblCategory As Label = CType(e.Row.FindControl("lblCategory"), Label)
            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            Dim chkProvideSubsidy As CheckBox = CType(e.Row.FindControl("chkProvideSubsidy"), CheckBox)
            Dim lblSubsidizeDisplayCode As Label = CType(e.Row.FindControl("lblSubsidizeDisplayCode"), Label)
            Dim lblDollarSign As Label = CType(e.Row.FindControl("lblDollarSign"), Label)
            Dim txtServiceFee As TextBox = CType(e.Row.FindControl("txtServiceFee"), TextBox)
            Dim chkNotProviderServiceFee As CheckBox = CType(e.Row.FindControl("chkNotProviderServiceFee"), CheckBox)
            Dim lblNotProviderServiceFee As Label = CType(e.Row.FindControl("lblNotProviderServiceFee"), Label)
            Dim imgServiceFeeAlert As Image = CType(e.Row.FindControl("imgServiceFeeAlert"), Image)
            Dim hfSchemeCode As HiddenField = CType(e.Row.FindControl("hfSchemeCode"), HiddenField)
            Dim hfSubsidizeCode As HiddenField = CType(e.Row.FindControl("hfSubsidizeCode"), HiddenField)

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            chkProvideSubsidy.AutoPostBack = True
            AddHandler chkProvideSubsidy.CheckedChanged, AddressOf chkProvideSubsidy_CheckedChanged

            Dim strSchemeCode As String = CType(e.Row.DataItem, SubsidizeGroupEFormModel).SchemeCode.Trim
            Dim strSubsidizeCode As String = CType(e.Row.DataItem, SubsidizeGroupEFormModel).SubsidizeCode.Trim

            hfSchemeCode.Value = strSchemeCode
            hfSubsidizeCode.Value = strSubsidizeCode

            Dim intPractice As Integer = CInt(CType(gvr_Parent_Parent.FindControl("hfFormatedPracticeIndex"), HiddenField).Value.Trim)
            Dim intPracticeScheme As Integer = CInt(CType(gvr_Parent.FindControl("hfFormatedPracticeSchemeIndex"), HiddenField).Value.Trim)

            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            Dim dt As DataTable
            dt = Session(SESS_Practice)

            Dim udtSchemeEFormList As SchemeEFormModelCollection = Nothing
            Dim udtSubSidizeGroupEFormList As SubsidizeGroupEFormModelCollection = Nothing

            Dim udtSchemeEForm As SchemeEFormModel = Nothing
            Dim udtSubSidizeGroupEForm As SubsidizeGroupEFormModel = Nothing

            If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup
            End If

            If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
                udtSubSidizeGroupEFormList = udtSchemeEFormBLL.GetSession_SubsidizeGroupEForm
            End If

            If Not IsNothing(udtSchemeEFormList) AndAlso Not IsNothing(udtSubSidizeGroupEFormList) Then

                udtSchemeEForm = udtSchemeEFormList.Filter(strSchemeCode)
                udtSubSidizeGroupEForm = udtSubSidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode)

                If udtSchemeEForm.EligibleProfesional(strCurrentProf) Then
                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    If selectedLanguageValue.Equals(English) Then
                        lblCategory.Text = udtSchemeEFormList.Filter(strSchemeCode).SubsidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode).CategoryName
                        lblSubsidizeDisplayCode.Text = Replace(udtSchemeEFormList.Filter(strSchemeCode).SubsidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode).SubsidizeDisplayCode, "+", "+<br>")
                        lblSubsidizeDisplayCode.ToolTip = udtSchemeEFormList.Filter(strSchemeCode).SubsidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode).SubsidizeItemDesc
                        lblNotProviderServiceFee.Text = udtSubSidizeGroupEForm.ServiceFeeCompulsoryWording
                    Else
                        lblCategory.Text = udtSchemeEFormList.Filter(strSchemeCode).SubsidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode).CategoryNameChi
                        lblSubsidizeDisplayCode.Text = Replace(udtSchemeEFormList.Filter(strSchemeCode).SubsidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode).SubsidizeDisplayCodeChi, "+", "+<br>")
                        lblSubsidizeDisplayCode.ToolTip = udtSchemeEFormList.Filter(strSchemeCode).SubsidizeGroupEFormList.Filter(strSchemeCode, strSubsidizeCode).SubsidizeItemDescChi
                        lblNotProviderServiceFee.Text = udtSubSidizeGroupEForm.ServiceFeeCompulsoryWordingChi
                    End If
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]

                    If udtSubSidizeGroupEForm.ServiceFeeEnabled Then
                        If udtSubSidizeGroupEForm.SubsidyCompulsory Then
                            chkProvideSubsidy.Visible = False
                        Else
                            chkProvideSubsidy.Visible = True
                        End If

                        If udtSubSidizeGroupEForm.ServiceFeeCompulsory Then
                            chkNotProviderServiceFee.Visible = False
                            chkNotProviderServiceFee.Style.Add("display", "none")
                            lblNotProviderServiceFee.Visible = False
                            lblNotProviderServiceFee.Style.Add("display", "none")
                        Else
                            chkNotProviderServiceFee.Visible = True
                            chkNotProviderServiceFee.Style.Add("display", "inline-block")
                            lblNotProviderServiceFee.Visible = True
                            lblNotProviderServiceFee.Style.Add("display", "inline-block")
                            blnServiceFeeCompulsory = True
                            If lblNotProviderServiceFee.Text.Length > intServiceFeeCompulsoryText Then
                                Select Case LCase(Session("language"))
                                    Case English
                                        intServiceFeeCompulsoryText = lblNotProviderServiceFee.Text.Length
                                    Case TradChinese
                                        intServiceFeeCompulsoryText = lblNotProviderServiceFee.Text.Length * 2
                                    Case SimpChinese
                                        intServiceFeeCompulsoryText = lblNotProviderServiceFee.Text.Length * 2
                                    Case Else
                                        intServiceFeeCompulsoryText = lblNotProviderServiceFee.Text.Length
                                End Select
                            End If
                        End If

                        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        If Not IsNothing(Session(strSchemeCode)) Then
                            If Not IsNothing(udtSubSidizeGroupEForm) Then
                                If CStr(Session(strSchemeCode)).Trim.Equals("Y") Then

                                    If CStr(dt.Rows(intPractice)(strSubsidizeCode + "_IsProvided")).Trim.Equals(YesNo.Yes) Then
                                        chkProvideSubsidy.Checked = True
                                        chkProvideSubsidy.Enabled = True

                                        lblSubsidizeDisplayCode.CssClass = ""

                                        If CStr(dt.Rows(intPractice)(strSubsidizeCode + "_IsNoServiceFee")).Trim.Equals(YesNo.Yes) Then
                                            lblDollarSign.CssClass = ""

                                            txtServiceFee.Text = String.Empty
                                            txtServiceFee.Attributes.Add("readonly", "readonly")
                                            txtServiceFee.CssClass = "gvTxtBoxReadOnlyBg"

                                            chkNotProviderServiceFee.Checked = True
                                            chkNotProviderServiceFee.Enabled = True
                                            lblNotProviderServiceFee.CssClass = ""
                                        Else
                                            lblDollarSign.CssClass = ""

                                            txtServiceFee.Text = dt.Rows(intPractice)(strSubsidizeCode + "_ServiceFee")
                                            txtServiceFee.Attributes.Remove("readonly")
                                            txtServiceFee.CssClass = "gvTxtBoxBg"

                                            chkNotProviderServiceFee.Checked = False
                                            chkNotProviderServiceFee.Enabled = True
                                            lblNotProviderServiceFee.CssClass = ""
                                        End If
                                    Else
                                        If udtSubSidizeGroupEForm.SubsidyCompulsory Then
                                            chkProvideSubsidy.Checked = False
                                            chkProvideSubsidy.Enabled = True

                                            lblSubsidizeDisplayCode.CssClass = ""

                                            lblDollarSign.CssClass = ""

                                            txtServiceFee.Text = dt.Rows(intPractice)(strSubsidizeCode + "_ServiceFee")
                                            txtServiceFee.Attributes.Remove("readonly")
                                            txtServiceFee.CssClass = "gvTxtBoxBg"

                                            chkNotProviderServiceFee.Checked = False
                                            chkNotProviderServiceFee.Enabled = True
                                            lblNotProviderServiceFee.CssClass = ""
                                        Else
                                            chkProvideSubsidy.Checked = False
                                            chkProvideSubsidy.Enabled = True

                                            lblSubsidizeDisplayCode.CssClass = ""

                                            lblDollarSign.CssClass = "dimText"

                                            txtServiceFee.Text = String.Empty
                                            txtServiceFee.Attributes.Add("readonly", "readonly")
                                            txtServiceFee.CssClass = "gvTxtBoxReadOnlyBg"

                                            chkNotProviderServiceFee.Checked = False
                                            chkNotProviderServiceFee.Enabled = False
                                            lblNotProviderServiceFee.CssClass = "dimText"
                                        End If
                                    End If
                                Else
                                    chkProvideSubsidy.Checked = False
                                    chkProvideSubsidy.Enabled = True

                                    lblSubsidizeDisplayCode.CssClass = ""

                                    lblDollarSign.CssClass = "dimText"

                                    txtServiceFee.Text = String.Empty
                                    txtServiceFee.Attributes.Add("readonly", "readonly")
                                    txtServiceFee.CssClass = "gvTxtBoxReadOnlyBg"

                                    chkNotProviderServiceFee.Checked = False
                                    chkNotProviderServiceFee.Enabled = False
                                    lblNotProviderServiceFee.CssClass = "dimText"
                                End If
                            Else
                                chkProvideSubsidy.Checked = False
                                chkProvideSubsidy.Enabled = True

                                lblSubsidizeDisplayCode.CssClass = ""

                                lblDollarSign.CssClass = "dimText"

                                txtServiceFee.Text = String.Empty
                                txtServiceFee.Attributes.Add("readonly", "readonly")
                                txtServiceFee.CssClass = "gvTxtBoxReadOnlyBg"

                                chkNotProviderServiceFee.Checked = False
                                chkNotProviderServiceFee.Enabled = False
                                lblNotProviderServiceFee.CssClass = "dimText"
                            End If
                        Else
                            chkProvideSubsidy.Checked = False
                            chkProvideSubsidy.Enabled = True

                            lblSubsidizeDisplayCode.CssClass = ""

                            lblDollarSign.CssClass = "dimText"

                            txtServiceFee.Text = String.Empty
                            txtServiceFee.Attributes.Add("readonly", "readonly")
                            txtServiceFee.CssClass = "gvTxtBoxReadOnlyBg"

                            chkNotProviderServiceFee.Checked = False
                            chkNotProviderServiceFee.Enabled = False
                            lblNotProviderServiceFee.CssClass = "dimText"
                        End If
                        'CRE16-002 (Revamp VSS) [End][Chris YIM]
                    Else
                        e.Row.Visible = False
                    End If
                Else
                    e.Row.Visible = False
                End If
            End If

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Not Session(SESS_Validation) Is Nothing Then
                Dim dtValidation As DataTable = Session(SESS_Validation)
                If dtValidation.Rows.Count > 0 Then

                    Dim drValidation() As DataRow = dtValidation.Select("PracticeRowNo = '" + intPractice.ToString.Trim + _
                                            "' AND PracticeSchemeRowNo = '" + intPracticeScheme.ToString.Trim + _
                                            "' AND SubsidyRowNo = '" + e.Row.RowIndex.ToString.Trim + _
                                            "' AND SchemeCode = '" + strSchemeCode + _
                                            "' AND SubsidizeCode = '" + strSubsidizeCode + _
                                            "'")

                    If drValidation.Length > 0 Then
                        imgServiceFeeAlert.Visible = True
                    End If

                End If
            End If
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If
    End Sub

    Protected Sub chkProvideSubsidy_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkBox As CheckBox = CType(sender, CheckBox)
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------

        'Get No. of Row in gvPractice in Event CheckChanged
        Dim intPracticeRowNum As Integer = CType(chkBox.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent, GridViewRow).RowIndex

        'Get No. of Row in gvPracticeScheme in Event CheckChanged
        Dim intPracticeSchemeRowNum As Integer = CType(chkBox.Parent.Parent.Parent.Parent.Parent.Parent, GridViewRow).RowIndex

        'Get No. of Row in gvServiceFee in Event CheckChanged
        Dim intServiceFeeRowNum As Integer = CType(chkBox.Parent.Parent, GridViewRow).RowIndex


        'Find which gvPracticeScheme is in gvPractice by row index
        Dim gvPracticeScheme As GridView = CType(Me.gvPractice.Rows(intPracticeRowNum).FindControl("gvPracticeScheme"), GridView)

        'Find which gvServiceFee is in gvPracticeScheme by row index
        Dim gvServiceFee As GridView = CType(gvPracticeScheme.Rows(intPracticeSchemeRowNum).FindControl("gvServiceFee"), GridView)

        'Find which controls are in gvServiceFee by row index
        Dim gvServiceFeeRow As GridViewRow = gvServiceFee.Rows(intServiceFeeRowNum)

        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim chkProvideSubsidy As CheckBox = CType(gvServiceFeeRow.FindControl("chkProvideSubsidy"), CheckBox)
        Dim lblDollarSign As Label = CType(gvServiceFeeRow.FindControl("lblDollarSign"), Label)
        Dim txtServiceFee As TextBox = CType(gvServiceFeeRow.FindControl("txtServiceFee"), TextBox)
        Dim chkNotProviderServiceFee As CheckBox = CType(gvServiceFeeRow.FindControl("chkNotProviderServiceFee"), CheckBox)
        Dim lblNotProviderServiceFee As Label = CType(gvServiceFeeRow.FindControl("lblNotProviderServiceFee"), Label)

        If chkProvideSubsidy.Visible Then
            If chkProvideSubsidy.Checked Then

                lblDollarSign.CssClass = ""

                txtServiceFee.Attributes.Remove("readonly")
                txtServiceFee.CssClass = "gvTxtBoxBg"

                chkNotProviderServiceFee.Enabled = True
                lblNotProviderServiceFee.CssClass = ""
            Else
                lblDollarSign.CssClass = "dimText"

                txtServiceFee.Text = String.Empty
                txtServiceFee.Attributes.Add("readonly", "readonly")
                txtServiceFee.CssClass = "gvTxtBoxReadOnlyBg"

                chkNotProviderServiceFee.Checked = False
                chkNotProviderServiceFee.Enabled = False
                lblNotProviderServiceFee.CssClass = "dimText"
            End If
        End If


    End Sub

    Protected Sub chkNotProviderServiceFee_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkBox As CheckBox = CType(sender, CheckBox)
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Get No. of Row in gvPractice in Event CheckChanged
        Dim intPracticeRowNum As Integer = CType(chkBox.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent, GridViewRow).RowIndex

        'Get No. of Row in gvPracticeScheme in Event CheckChanged
        Dim intPracticeSchemeRowNum As Integer = CType(chkBox.Parent.Parent.Parent.Parent.Parent.Parent, GridViewRow).RowIndex

        'Get No. of Row in gvServiceFee in Event CheckChanged
        Dim intServiceFeeRowNum As Integer = CType(chkBox.Parent.Parent, GridViewRow).RowIndex


        'Find which gvPracticeScheme is in gvPractice by row index
        Dim gvPracticeScheme As GridView = CType(Me.gvPractice.Rows(intPracticeRowNum).FindControl("gvPracticeScheme"), GridView)

        'Find which gvServiceFee is in gvPracticeScheme by row index
        Dim gvServiceFee As GridView = CType(gvPracticeScheme.Rows(intPracticeSchemeRowNum).FindControl("gvServiceFee"), GridView)

        'Find which controls are in gvServiceFee by row index
        Dim gvServiceFeeRow As GridViewRow = gvServiceFee.Rows(intServiceFeeRowNum)

        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim txtServiceFee As TextBox = CType(gvServiceFeeRow.FindControl("txtServiceFee"), TextBox)
        Dim chkNotProviderServiceFee As CheckBox = CType(gvServiceFeeRow.FindControl("chkNotProviderServiceFee"), CheckBox)

        If chkNotProviderServiceFee.Visible Then
            If chkNotProviderServiceFee.Checked Then
                txtServiceFee.Text = String.Empty
                txtServiceFee.Attributes.Add("readonly", "readonly")
                txtServiceFee.CssClass = "gvTxtBoxReadOnlyBg"
            Else
                txtServiceFee.Attributes.Remove("readonly")
                txtServiceFee.CssClass = "gvTxtBoxBg"
            End If
        End If

    End Sub

    Protected Sub lnkBtnPersonal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
            getServiceFeeFromGridView(False)
        End If

        If panEHRSS.Visible Then
            If rboHadJoinedEHRSS.SelectedValue.Trim = "Y" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.Yes
            ElseIf rboHadJoinedEHRSS.SelectedValue.Trim = "N" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.No
            Else
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
            End If

        Else
            udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        udtSP.JoinPCD = SetJoinProjectChoice(panWillJoinPCD, rboWillJoinPCD)

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        udtSP = udtSPBLL.GetSP

        WriteAuditLog(udtSP, udtAuditLogEntry)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00050, "Press Tab to Personal Particulars in Scheme Selection")

        udtEFormBLL.ClearRedirectPageSession()
        Session(eFormBLL.SESS_PersonalParticular) = "Y"
        Response.Redirect("~/PersonalPacticulars.aspx")
    End Sub

    Protected Sub lnkBtnMO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
            getServiceFeeFromGridView(False)
        End If

        If panEHRSS.Visible Then
            If rboHadJoinedEHRSS.SelectedValue.Trim = "Y" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.Yes
            ElseIf rboHadJoinedEHRSS.SelectedValue.Trim = "N" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.No
            Else
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
            End If

        Else
            udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        udtSP.JoinPCD = SetJoinProjectChoice(panWillJoinPCD, rboWillJoinPCD)

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        WriteAuditLog(udtSP, udtAuditLogEntry)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00051, "Press Tab to MO in Scheme Selection")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_MedicalOrganization) = "Y"
        Response.Redirect("~/MedicalOrganization.aspx")
    End Sub

    Protected Sub lnkBtnPractice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
            getServiceFeeFromGridView(False)
        End If

        If panEHRSS.Visible Then
            If rboHadJoinedEHRSS.SelectedValue.Trim = "Y" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.Yes
            ElseIf rboHadJoinedEHRSS.SelectedValue.Trim = "N" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.No
            Else
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
            End If

        Else
            udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        udtSP.JoinPCD = SetJoinProjectChoice(panWillJoinPCD, rboWillJoinPCD)

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        WriteAuditLog(udtSP, udtAuditLogEntry)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00052, "Press Tab to Practice in Scheme Selection")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_Practice) = "Y"
        Response.Redirect("~/Practice.aspx")

    End Sub

    Protected Sub lnkBtnBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
            getServiceFeeFromGridView(False)
        End If

        If panEHRSS.Visible Then
            If rboHadJoinedEHRSS.SelectedValue.Trim = "Y" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.Yes
            ElseIf rboHadJoinedEHRSS.SelectedValue.Trim = "N" Then
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.No
            Else
                udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
            End If

        Else
            udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
        End If

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        udtSP.JoinPCD = SetJoinProjectChoice(panWillJoinPCD, rboWillJoinPCD)

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        WriteAuditLog(udtSP, udtAuditLogEntry)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00053, "Press Tab to Bank Account in Scheme Selection")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_Bank) = "Y"
        Response.Redirect("~/BankAccount.aspx")
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
    Private Sub LoadEHRSS()
        Dim strProf As String = String.Empty
        Dim dtPractice As DataTable = Session(SESS_Practice)
        If Not IsNothing(dtPractice) Then
            strProf = CStr(dtPractice.Rows(0).Item("ServiceCategoryCode")).Trim
        End If

        If udtEFormBLL.AskHadJoinedEHRSSProfCode(strProf) Then
            panEHRSS.Visible = True
            If udtSPBLL.Exist Then

                Dim udtSP As ServiceProviderModel
                udtSP = udtSPBLL.GetSP

                If udtSP.AlreadyJoinEHR.Equals(JoinEHRSSStatus.Yes) Then
                    rboHadJoinedEHRSS.SelectedValue = JoinEHRSSStatus.Yes
                ElseIf udtSP.AlreadyJoinEHR.Equals(JoinEHRSSStatus.No) Then
                    rboHadJoinedEHRSS.SelectedValue = JoinEHRSSStatus.No
                End If

            End If

        Else
            panEHRSS.Visible = False
        End If

    End Sub

    Private Sub iBtnLoadDemoData_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnLoadDemoData.Click
        Dim ds As DataSet = Me.GetDemoData
        If ds Is Nothing Then Exit Sub

        Dim dt As DataTable = ds.Tables("PersonalParticular")
        Dim dr As DataRow
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dr = dt.Rows(0)

            Dim udtSP As ServiceProviderModel
            udtSP = udtSPBLL.GetSP
            udtSP.AlreadyJoinEHR = dr("AlreadyJoinEHR")
            udtSP.JoinPCD = dr("JoinPCD")
            LoadEHRSS()
        Else
            Exit Sub
        End If
    End Sub
    ' CRE12-001 eHS and PCD integration [End][Koala]


    ' CRE12-001 eHS and PCD integration [Start][Tommy]

    Private Sub WriteAuditLog(ByVal udtSP As ServiceProviderModel, ByRef udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("AlreadyJoinEHRSS", udtSP.AlreadyJoinEHR)
        ' CRE15-018 Remove PPIePR Enrolment [Start][Winnie]
        'udtAuditLogEntry.AddDescripton("WillJoinPPIePR", udtSP.JoinHAPPI)
        ' CRE15-018 Remove PPIePR Enrolment [End][Winnie]
        udtAuditLogEntry.AddDescripton("WillJoinPCD", udtSP.JoinPCD)
        WriteAuditLogMScheme(udtAuditLogEntry)
    End Sub

    Public Sub CheckTypeOfPractice()
        ' CRE19-008 (Rename VO) [Start][Koala]
        If panPCD.Visible And panWillJoinPCD.Visible Then
            'If panPCD.Visible Then
            ' CRE19-008 (Rename VO) [Start][Koala]

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Check whether rboWillJoinPCD is selected 
            udtSM = udtValidator.chkWillJoinPCD(rboWillJoinPCD.SelectedValue)

            If Not udtSM Is Nothing Then
                imgWillJoinPCDAlert.Visible = True
                udcMsgBox.AddMessage(udtSM)
            Else
                ' Check whether compulsory to join PCD for selected scheme
                Dim blnCompulsory As Boolean = False
                Dim strCompulsory_SchemeDesc As New List(Of String)
                Dim strCompulsory_SchemeDescChi As New List(Of String)
                Dim udtSchemeEFormBLL As SchemeEFormBLL = New SchemeEFormBLL

                If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                    Dim udtSchemeEFormList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        If udtSchemeEForm.JoinPCDCompulsory = YesNo.Yes Then
                            If Not IsNothing(Session(udtSchemeEForm.SchemeCode.Trim)) Then
                                If CStr(Session(udtSchemeEForm.SchemeCode.Trim)).Equals("Y") Then
                                    blnCompulsory = True
                                End If
                            End If

                            strCompulsory_SchemeDesc.Add(String.Format("""{0}""", udtSchemeEForm.SchemeDesc))
                            strCompulsory_SchemeDescChi.Add(udtSchemeEForm.SchemeDescChi)
                        End If
                    Next

                    ' Must join PCD but SP selected not to join
                    If blnCompulsory AndAlso rboWillJoinPCD.SelectedValue = JoinPCDStatus.No Then
                        imgWillJoinPCDAlert.Visible = True
                        udcMsgBox.AddMessage(FunctCode.FUNT020101, SeverityCode.SEVE, MsgCode.MSG00021, _
                                             New String() {"%en", "%tc"}, _
                                             New String() {String.Join("/", strCompulsory_SchemeDesc), _
                                                           String.Join("¡þ", strCompulsory_SchemeDescChi)})
                    End If
                End If
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

            If panSelectPracticeType.Style("display") <> "none" Then

                'Check whether Practice is selected
                If Not ucTypeOfPracticeGrid.PracticeSelected Then
                    imgPCDTypeOfPracticeAlert.Visible = True
                    udcMsgBox.AddMessage(FunctCode.FUNT990003, SeverityCode.SEVE, MsgCode.MSG00501)
                End If

                'Check whether Type of Practice is selected
                If Not ucTypeOfPracticeGrid.TypeOfPracticeSelected Then
                    'imgPCDTypeOfPracticeAlert.Visible = True
                    udcMsgBox.AddMessage(FunctCode.FUNT990003, SeverityCode.SEVE, MsgCode.MSG00502)
                End If
            End If
        End If

    End Sub

    Public Sub SaveTypeOfPractice(ByRef udtSP As ServiceProviderModel)

        udtSP.JoinPCD = SetJoinProjectChoice(panWillJoinPCD, rboWillJoinPCD)

        SetSessionTypeOfPractice(Me.ucTypeOfPracticeGrid.GetThirdPartyAdditionalFieldEnrolmentCollection())

    End Sub

    Private Sub SetSessionTypeOfPractice(ByVal udtThirdPartyAdditionalFieldEnrolmentList As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection)
        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If Not IsNothing(udtSP) Then
            udtSP.ThirdPartyAdditionalFieldEnrolmentList = udtThirdPartyAdditionalFieldEnrolmentList
            udtSPBLL.SaveToSession(udtSP)
        End If
    End Sub

    Private Sub LoadPCDChoice()
        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If Not IsNothing(udtSP) Then
            If Not IsNothing(udtSP.JoinPCD) Then

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                Select Case udtSP.JoinPCD
                    Case JoinPCDStatus.Yes
                        rboWillJoinPCD.SelectedValue = JoinPCDStatus.Yes
                    Case JoinPCDStatus.No
                        rboWillJoinPCD.SelectedValue = JoinPCDStatus.No
                    Case JoinPCDStatus.Enrolled
                        rboWillJoinPCD.SelectedValue = JoinPCDStatus.Enrolled
                End Select
                'If udtSP.JoinPCD.Equals(JoinProjectStatus.Yes) Then
                '    rboWillJoinPCD.SelectedValue = JoinProjectStatus.Yes
                'End If

                'If udtSP.JoinPCD.Equals(JoinProjectStatus.No) Then
                '    rboWillJoinPCD.SelectedValue = JoinProjectStatus.No
                'End If
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            End If
        End If

        ShowPCDPanel()
    End Sub

    Private Sub rboWillJoinPCD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboWillJoinPCD.SelectedIndexChanged
        Dim udtSP As ServiceProviderModel = Nothing
        If udtSPBLL.Exist Then
            udtSP = udtSPBLL.GetSP
        End If

        If Not IsNothing(udtSP) Then
            udtSP.JoinPCD = SetJoinProjectChoice(panWillJoinPCD, rboWillJoinPCD)
        End If

        ShowPCDPanel()

        Me._ScriptManager.SetFocus(Me.ibtnSchemeSelectNext)

    End Sub

    Private Function SetJoinProjectChoice(ByRef panProject As Panel, ByRef rdoChoiceList As RadioButtonList) As String
        Dim strChoice As String = JoinPCDStatus.NA
        If panProject.Visible Then
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' ==========================================================
            Select Case rdoChoiceList.SelectedValue.Trim
                Case JoinPCDStatus.Yes
                    strChoice = JoinPCDStatus.Yes
                Case JoinPCDStatus.No
                    strChoice = JoinPCDStatus.No
                Case JoinPCDStatus.Enrolled
                    strChoice = JoinPCDStatus.Enrolled
            End Select
            'If rdoChoiceList.SelectedValue.Trim = "Y" Then
            '    strChoice = JoinProjectStatus.Yes
            'End If
            'If rdoChoiceList.SelectedValue.Trim = "N" Then
            '    strChoice = JoinProjectStatus.No
            'End If
            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
        Else
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' ==========================================================
            strChoice = JoinPCDStatus.NA
            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
        End If
        Return strChoice
    End Function

    Private Sub ShowPCDPanel()
        If Common.Component.Profession.ProfessionBLL.GetProfessionListByServiceCategoryCode(Me.strCurrentProf).AllowJoinPCD Then
            panPCD.Visible = True
            ShowPCDPracticeTypePanel()
        Else
            panPCD.Visible = False
            rboWillJoinPCD.SelectedIndex = -1
        End If
    End Sub

    Private Sub ShowPCDPracticeTypePanel()

        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)
        udtAuditLogEntry.AddDescripton("rboWillJoinPCD.SelectedValue", rboWillJoinPCD.SelectedValue)

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' ==========================================================
        If rboWillJoinPCD.SelectedValue = JoinPCDStatus.Yes Then
            panSelectPracticeType.Style("display") = String.Empty
            Me.ucTypeOfPracticeGrid.LoadPractice(udtSPBLL.GetSP)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00069, "Will Join PCD")
        ElseIf rboWillJoinPCD.SelectedValue = JoinPCDStatus.Enrolled Then
            panSelectPracticeType.Style("display") = "none"
            'Clear ThirdPartyEnrolment Session
            SetSessionTypeOfPractice(Nothing)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00081, "Will Not Join PCD (Already Enrolled)")
        ElseIf rboWillJoinPCD.SelectedValue = JoinPCDStatus.No Then
            panSelectPracticeType.Style("display") = "none"
            'Clear ThirdPartyEnrolment Session
            SetSessionTypeOfPractice(Nothing)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00070, "Will Not Join PCD")
        Else
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00068, "Will Join PCD Not Selected")
        End If
        ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
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
            Case DT_NAME.DT_NAME_SUBSIDY
                dt = New DataTable(DT_NAME.DT_NAME_SUBSIDY)
                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_DISPLAY_SEQ, Type.GetType("System.Int32"))
                dt.Columns.Add(dc)

                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_CODE, Type.GetType("System.String"))
                dt.Columns.Add(dc)

                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_DESC, Type.GetType("System.String"))
                dt.Columns.Add(dc)

                dc = New DataColumn(DT_NAME.DT_FIELD_NAME_SUBSIDIZE_DESC_CHI, Type.GetType("System.String"))
                dt.Columns.Add(dc)

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

    Protected Sub gvSchemeToEnroll_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSchemeToEnroll As Label = CType(e.Row.FindControl("lblSchemeToEnroll"), Label)

            If Not lblSchemeToEnroll Is Nothing Then
                Dim row As DataRow = CType(e.Row.DataItem, DataRowView).Row
                lblSchemeToEnroll.Text = row.Field(Of String)(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DISPLAY_ITEM)
            End If
        End If
    End Sub

    Protected Sub gvSchemeToEnroll_RowCreated(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Style.Add("padding-left", "0px")
            e.Row.Cells(0).Style.Add("padding-top", "0px")
            e.Row.Cells(0).Style.Add("padding-right", "0px")
            e.Row.Cells(0).Style.Add("padding-bottom", "1px")
        End If
    End Sub

    Protected Sub gvPracticeScheme_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowIndex > 0 Then
                e.Row.Cells(0).Controls.AddAt(0, New LiteralControl("<br/>"))
            End If

            Dim strSchemeCode As String

            Dim row As DataRow = CType(e.Row.DataItem, DataRowView).Row
            strSchemeCode = row.Field(Of String)(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_CODE).Trim

            Dim lblSchemeDescText As Label = CType(e.Row.FindControl("lblSchemeDescText"), Label)
            Dim hfFormatedPracticeSchemeIndex As HiddenField = CType(e.Row.FindControl("hfFormatedPracticeSchemeIndex"), HiddenField)

            If Not lblSchemeDescText Is Nothing Then
                lblSchemeDescText.Text = row.Field(Of String)(DT_NAME.DT_FIELD_NAME_ENROLLED_SCHEME_DESC)
            End If

            If Not hfFormatedPracticeSchemeIndex Is Nothing Then
                hfFormatedPracticeSchemeIndex.Value = e.Row.RowIndex
            End If

            Dim gvServiceFee As GridView = CType(e.Row.FindControl("gvServiceFee"), GridView)

            If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                Dim udtSchemeEForm As SchemeEFormModel
                udtSchemeEForm = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup.Filter(strSchemeCode)

                Dim udtFilteredSubsidizeGroupEFormList As New SubsidizeGroupEFormModelCollection

                For Each udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                    If udtSubsidizeGroupEFormModel.ServiceFeeEnabled Then
                        udtFilteredSubsidizeGroupEFormList.Add(udtSubsidizeGroupEFormModel)
                    End If
                Next

                If Not udtFilteredSubsidizeGroupEFormList Is Nothing Then
                    gvServiceFee.DataSource = udtFilteredSubsidizeGroupEFormList
                    gvServiceFee.DataBind()
                End If
            End If
        End If
    End Sub

End Class

