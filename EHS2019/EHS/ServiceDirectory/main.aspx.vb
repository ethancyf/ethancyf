Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.Profession
Imports Common.Format

Partial Public Class main
    Inherits BasePageWithGridView

    Private udtServiceDirectoryBLL As New ServiceDirectory.ServiceDirectoryBLL

#Region "Session Constants"

    Const SESS_udtProfessional As String = "040101_udtProfessional"
    Const SESS_dtService As String = "040101_dtService"
    Const SESS_dtEligibleService As String = "040101_dtEligibleService"
    Const SESS_dtDistrict As String = "040101_dtDistrict"

    Const SESS_SearchRemarkDataTable As String = "040101_SearchRemarkDataTable"

    Const SESS_SearchResultDataTable As String = "040101_SearchResultDataTable"

    Const SESS_isReRenderPage As String = "040101_isReRenderPage"

    ' INT16-0010 Fix concurrent search problem [Start][Winnie]
    ' Remove unused session
    ' ViewState
    Const VS_SelectedProfessional As String = "040101_strSelectedProfessional"
    Const VS_SelectedService As String = "040101_strSelectedService"
    Const VS_SelectedServiceList As String = "040101_lstSelectedService"
    Const VS_SelectedDistrictList As String = "040101_strSelectedDistrictList"
    ' INT16-0010 Fix concurrent search problem [End][Winnie]
    ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]
    Const VS_ResultDisplayServiceList As String = "040101_lstResultDisplayService"
    ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]

    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
    Const VS_KwdSearchServiceProviderName As String = "040101_strQSearchServiceProviderName"
    Const VS_KwdSearchPracticeName As String = "040101_strKwdSearchPracticeName"
    Const VS_KwdSearchPracticeAddr As String = "040101_strKwdSearchPracticeAddr"
    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

#End Region

#Region "Retrieve Setting"

    Private Function GetHealthcareProfessional() As ProfessionModelCollection
        Dim udtProfessional As ProfessionModelCollection = Session(SESS_udtProfessional)

        If IsNothing(udtProfessional) Then
            udtProfessional = (New ServiceDirectoryBLL).GetHealthProf()
            Session(SESS_udtProfessional) = udtProfessional

        End If

        Return udtProfessional

    End Function

    Private Function GetService() As DataTable
        Dim dt As DataTable = Session(SESS_dtService)

        If IsNothing(dt) Then
            dt = (New ServiceDirectoryBLL).GetService
            Session(SESS_dtService) = dt

        End If

        Return dt

    End Function

    Private Function GetEligibleService() As DataTable
        Dim dt As DataTable = Session(SESS_dtEligibleService)

        If IsNothing(dt) Then
            dt = (New ServiceDirectoryBLL).GetEligibleService
            Session(SESS_dtEligibleService) = dt
        End If

        Return dt

    End Function

    Private Function GetDistrict() As DataTable
        Dim dtDistrictList As DataTable = Session(SESS_dtDistrict)
        If IsNothing(dtDistrictList) Then
            dtDistrictList = (New ServiceDirectoryBLL).GetDistrictBoard()
            Session(SESS_dtDistrict) = dtDistrictList

        End If

        Return dtDistrictList

    End Function

#End Region

#Region "Page Events"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.basetag.Attributes("href") = (New GeneralFunction).getPageBasePath()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        SetLangage()

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

        Select Case Session("language")
            Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                tdAppEnvironment.Attributes("class") = "AppEnvironmentZH"
            Case Else
                tdAppEnvironment.Attributes("class") = "AppEnvironment"
        End Select
        ' CRE15-006 Rename of eHS [End][Lawrence]

        Dim selectedLang As String
        selectedLang = LCase(Session("language"))

        If Not IsPostBack Then
            FunctionCode = Common.Component.FunctCode.FUNT040101

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00024, "SDIR Page Load (Obsoleted)")

            InitializeSessionVariable()

            Session(SESS_isReRenderPage) = "dummy"

            ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
            InitializeKeywordsSearchControl()
            ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---
            InitializeHealthcareProfessional()
            InitializeDistrict()
            InitializeService()

            tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner") + ")"
            tblBanner.Style.Item("background-repeat") = "no-repeat"

            Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "ServiceDirectory")

            ' Render the static links
            Dim udtGeneralFunction As New GeneralFunction
            Dim strContactUsLink As String = String.Empty

            If selectedLang.Equals(English) Then
                udtGeneralFunction.getSystemParameter("ContactUsLink", strContactUsLink, String.Empty)
            ElseIf selectedLang.Equals(TradChinese) Then
                udtGeneralFunction.getSystemParameter("ContactUsLink_CHI", strContactUsLink, String.Empty)
            End If

            Me.lnkBtnContactUs.OnClientClick = "javascript:window.openNewWin('" + strContactUsLink + "');return false;"

            Dim strFAQsLink As String = String.Empty

            If selectedLang.Equals(English) Then
                udtGeneralFunction.getSystemParameter("FAQsLink", strFAQsLink, String.Empty)
            ElseIf selectedLang.Equals(TradChinese) Then
                udtGeneralFunction.getSystemParameter("FAQsLink_CHI", strFAQsLink, String.Empty)
            End If

            Me.lnkBtnFAQs.OnClientClick = "javascript:window.openNewWin('" + strFAQsLink + "?view=Public');return false;"

            Dim strPrivacyPolicyLink As String = String.Empty

            If selectedLang.Equals(English) Then
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
            ElseIf selectedLang.Equals(TradChinese) Then
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
            End If

            Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewWin('" + strPrivacyPolicyLink + "');return false;"

            Dim strDisclaimerLink As String = String.Empty

            If selectedLang.Equals(English) Then
                udtGeneralFunction.getSystemParameter("DisclaimerLink", strDisclaimerLink, String.Empty)
            ElseIf selectedLang.Equals(TradChinese) Then
                udtGeneralFunction.getSystemParameter("DisclaimerLink_CHI", strDisclaimerLink, String.Empty)
            End If

            Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewWin('" + strDisclaimerLink + "');return false;"

            Dim strSysMaintLink As String = String.Empty

            If selectedLang.Equals(English) Then
                udtGeneralFunction.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
            ElseIf selectedLang.Equals(TradChinese) Then
                udtGeneralFunction.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
            End If
            Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewWin('" + strSysMaintLink + "');return false;"

            ' Prevent multi-click
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
            Me.iBtnSearch.OnClientClick = "javascript:return CheckTextInput('" + Me.GetGlobalResourceObject("Text", "HalfWidthCharacterInputNotice") + "');"
            ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

            preventMultiImgClick(Me.Page.ClientScript, Me.iBtnSearch)
            preventMultiImgClick(Me.Page.ClientScript, Me.iBtnReset)

        End If

        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "reconn key", KeepSessionAlive())

        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        ' Retrieve the result again and store into session
        If (MyBase.IsPageRefreshed) Then
            If Me.mvResult.Visible = True Then

                Dim strProfessional As String = ViewState(VS_SelectedProfessional)
                Dim strSelectedService As String = ViewState(VS_SelectedService)
                Dim strDistrictList As String = ViewState(VS_SelectedDistrictList)
                ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                Dim strServiceProviderName As String = ViewState(VS_KwdSearchServiceProviderName)
                Dim strPracticeName As String = ViewState(VS_KwdSearchPracticeName)
                Dim strPracticeAddr As String = ViewState(VS_KwdSearchPracticeAddr)
                ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

                Dim dtResult As New DataTable

                ' Init
                Session(SESS_SearchResultDataTable) = Nothing

                ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                dtResult = getResultList(strProfessional, strSelectedService, strDistrictList, selectedLang.ToString.ToUpper, strServiceProviderName, strPracticeName, strPracticeAddr)
                ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

                ' Save to session
                Session(SESS_SearchResultDataTable) = dtResult
            End If
        End If
        ' INT16-0010 Fix concurrent search problem [End][Winnie]

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ObsoletedOSHandler.HandleObsoleteOS(CommonSessionHandler.OS, ModalPopupExtenderReminderWindowsVersion, ObsoletedOSHandler.Version.Full, _
                                            Me.FunctionCode, LogID.LOG00021, Me, Nothing)
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

        ' Bind the gridview for view01 when page_load, to make it displays properly 
        If Not Session(SESS_SearchResultDataTable) Is Nothing And Me.mvResult.ActiveViewIndex = 0 And Me.mvResult.Visible = True Then
            GridViewDataBind(Me.gvRemarks, Session(SESS_SearchRemarkDataTable))
            GridViewDataBind(Me.gvRemarks_noFee, Session(SESS_SearchRemarkDataTable))
            GridViewDataBind(Me.gvResult, Session(SESS_SearchResultDataTable))
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)

            ' INT16-0010 Fix concurrent search problem [Start][Winnie]
            If MyBase.IsPageRefreshed OrElse _
                controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse _
                controlID.Equals(_SelectTradChinese) OrElse controlID.Equals(_SelectEnglish) Then
                Session(SESS_isReRenderPage) = "ReRenderPage"
                ReRenderPage()
                Session(SESS_isReRenderPage) = "dummy"
            End If
            ' INT16-0010 Fix concurrent search problem [End][Winnie]

            'INT16-0003 (Fix subsidy checkbox indent in SDIR) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "checkBoxIndent", "selectValue()", True)
            'INT16-0003 (Fix subsidy checkbox indent in SDIR) [End][Chris YIM]
        End If

    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        '' handle if page is refreshed
        'If _blnIsRequireHandlePageRefresh = True Then
        '    HandlePageRefreshed()
        'End If
        ' INT16-0010 Fix concurrent search problem [End][Winnie]
        MyBase.OnPreRender(e)
    End Sub

    '

    Private Sub InitializeSessionVariable()
        Session(SESS_dtEligibleService) = Nothing

        Session(SESS_dtService) = Nothing
        Session(SESS_dtDistrict) = Nothing
        Session(SESS_udtProfessional) = Nothing

        Session(SESS_SearchRemarkDataTable) = Nothing
        Session(SESS_SearchResultDataTable) = Nothing

        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        'Session(SESS_pageSize) = Nothing
        'Session(SESS_pageSize_noFee) = Nothing
        'Session(SESS_dtRemarks) = Nothing
        ' INT16-0010 Fix concurrent search problem [End][Winnie]
    End Sub

    Public Sub preventMultiImgClick(cs As ClientScriptManager, ibtn As ImageButton)
        Dim strScript As String = "if (this.style.cursor != 'wait') { this.style.cursor = 'wait'; return true; } else { this.disabled = true; return false; }"

        ibtn.Attributes.Add("onclick", strScript)

    End Sub

    Private Sub SetLangage()
        Dim selectedValue As String

        selectedValue = Session("language")

        Select Case selectedValue
            Case English
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = True
            Case TradChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageSelectedText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
                lnkbtnSimpChinese.Enabled = True
            Case SimpChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageSelectedText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = False
            Case Else
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageSelectedText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
                lnkbtnSimpChinese.Enabled = True
        End Select
    End Sub

    Private Sub ReRenderPage()
        Dim selectedLang As String
        Dim strSelectedProf As String
        Dim strSelectedDistrictList, strSelectedDistrictMaster As String
        Dim strSelectedServiceList As String
        Dim strSelectedResultsPerPage As String
        Dim i, j, parentNode, childNode As Integer
        Dim cb_area As System.Web.UI.WebControls.CheckBox
        Dim cbl_area As System.Web.UI.WebControls.CheckBoxList
        Dim cbl_area_item As System.Web.UI.WebControls.ListItem
        Dim selectedDistricts(), selectdArea(), selectedSchemes() As String
        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        Dim strKwdSearchServiceProviderName As String
        Dim strKwdSearchPracticeName As String
        Dim strKwdSearchPracticeAddr As String
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        selectedLang = LCase(Session("language"))

        strSelectedProf = String.Empty
        strSelectedDistrictList = String.Empty
        strSelectedDistrictMaster = String.Empty
        strSelectedServiceList = String.Empty
        strSelectedResultsPerPage = String.Empty
        strKwdSearchServiceProviderName = String.Empty
        strKwdSearchPracticeName = String.Empty
        strKwdSearchPracticeAddr = String.Empty

        ' Get the selected professional option
        strSelectedProf = Me.rboProfessional.SelectedValue.ToString.Trim & ""

        InitializeHealthcareProfessional()

        ' Check the radio button of the selected professional option
        If Not strSelectedProf = "" Then
            Me.rboProfessional.SelectedValue = strSelectedProf
        End If

        ' Get the selected service option
        For i = 0 To Me.TreeViewService.Nodes.Count - 1
            For j = 0 To Me.TreeViewService.Nodes(i).ChildNodes.Count - 1
                If Me.TreeViewService.Nodes(i).ChildNodes(j).Checked Then
                    strSelectedServiceList += Me.TreeViewService.Nodes(i).ChildNodes(j).Value.ToString() + ";"
                End If
            Next
        Next

        InitializeService()

        ' Enable the eligible service checkbox if a professional is selected
        If rboProfessional.SelectedValue.Length > 0 Then
            RenderEligibleService()
        End If

        ' Get the selected district options
        For i = 0 To 2
            cb_area = CType(Me.FindControl("cb_area_" + CStr(i + 1)), System.Web.UI.WebControls.CheckBox)
            If cb_area.Checked Then
                strSelectedDistrictMaster += CStr(i + 1) + ";"
            End If

            cbl_area = CType(Me.FindControl("cbl_area_" + CStr(i + 1)), System.Web.UI.WebControls.CheckBoxList)
            For Each cbl_area_item In cbl_area.Items
                If cbl_area_item.Selected Then
                    strSelectedDistrictList += cbl_area_item.Value.Trim + ";"
                End If
            Next
        Next

        InitializeDistrict()

        ' Check the checkbox of the selected district options, HK, KLN and NT
        selectdArea = strSelectedDistrictMaster.Split(";")
        If selectdArea.Length > 0 Then
            For i = 0 To selectdArea.Length - 1
                If selectdArea(i).Trim <> "" Then
                    cb_area = CType(Me.FindControl("cb_area_" + selectdArea(i)), System.Web.UI.WebControls.CheckBox)
                    cb_area.Checked = True
                End If
            Next
        End If

        ' Check the checkbox of the selected district options, 18 districts
        selectedDistricts = Split(strSelectedDistrictList, ";")
        If selectedDistricts.Length > 0 Then
            For i = 0 To 2
                cbl_area = CType(Me.FindControl("cbl_area_" + CStr(i + 1)), System.Web.UI.WebControls.CheckBoxList)
                For Each cbl_area_item In cbl_area.Items
                    For j = 0 To selectedDistricts.Length - 1
                        If cbl_area_item.Value.Trim = selectedDistricts(j) Then
                            cbl_area_item.Selected = True
                        End If
                    Next
                Next
            Next
        End If

        ' Check the checkbox of the selected service options
        selectedSchemes = Split(strSelectedServiceList, ";")
        For i = 0 To selectedSchemes.Length - 1
            For parentNode = 0 To Me.TreeViewService.Nodes.Count - 1
                For childNode = 0 To Me.TreeViewService.Nodes(parentNode).ChildNodes.Count - 1
                    If TreeViewService.Nodes(parentNode).ChildNodes(childNode).Value = selectedSchemes(i) Then
                        TreeViewService.Nodes(parentNode).ChildNodes(childNode).Checked = True
                    End If
                Next
            Next
        Next

        tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner") + ")"
        tblBanner.Style.Item("background-repeat") = "no-repeat"

        Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "ServiceDirectory")

        lnkBtnPrivacyPolicy.Text = Me.GetGlobalResourceObject("Text", "PrivacyPolicy")
        lnkBtnDisclaimer.Text = Me.GetGlobalResourceObject("Text", "ImportantNotices")
        lnkBtnSysMaint.Text = Me.GetGlobalResourceObject("Text", "SysMaint")
        lnkBtnContactUs.Text = Me.GetGlobalResourceObject("Text", "ContactUs")
        lnkBtnFAQs.Text = Me.GetGlobalResourceObject("Text", "Faqs")

        iBtnSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")
        iBtnSearch.AlternateText = Me.GetGlobalResourceObject("AlternateText", "SearchBtn")

        iBtnReset.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearAllBtn")
        iBtnReset.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ClearAllBtn")

        ' Render static links
        Dim udtGeneralFunction As New GeneralFunction
        Dim strContactUsLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("ContactUsLink", strContactUsLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("ContactUsLink_CHI", strContactUsLink, String.Empty)
        End If

        Me.lnkBtnContactUs.OnClientClick = "javascript:window.openNewWin('" + strContactUsLink + "');return false;"

        Dim strFAQsLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("FAQsLink", strFAQsLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("FAQsLink_CHI", strFAQsLink, String.Empty)
        End If

        Me.lnkBtnFAQs.OnClientClick = "javascript:window.openNewWin('" + strFAQsLink + "?view=Public');return false;"

        Dim strPrivacyPolicyLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
        End If

        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewWin('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("DisclaimerLink", strDisclaimerLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("DisclaimerLink_CHI", strDisclaimerLink, String.Empty)
        End If

        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewWin('" + strDisclaimerLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        If selectedLang.Equals(English) Then
            udtGeneralFunction.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
        ElseIf selectedLang.Equals(TradChinese) Then
            udtGeneralFunction.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewWin('" + strSysMaintLink + "');return false;"

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        Me.iBtnSearch.OnClientClick = "javascript:return CheckTextInput('" + Me.GetGlobalResourceObject("Text", "HalfWidthCharacterInputNotice") + "');"
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        If Me.mvResult.Visible = True Then
            ReRenderResultArea()
        End If

    End Sub

    Private Sub ReRenderResultArea()

        Select Case mvResult.ActiveViewIndex
            Case 0
                ' -- Heading --
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
                '--- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                If String.IsNullOrEmpty(rboProfessional.SelectedValue) Then
                    lblSearchResult.Text = AntiXssEncoder.HtmlEncode(Me.GetGlobalResourceObject("Text", "SearchResultLabel"), True)
                Else
                    lblSearchResult.Text = AntiXssEncoder.HtmlEncode(Me.GetGlobalResourceObject("Text", "SearchResult").ToString.Replace("%s", rboProfessional.SelectedItem.Text), True)
                End If
                '--- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]

                ' -- Result per page --
                initResultsPerPage(Me.mvResult.ActiveViewIndex)

                ' -- Scheme legend --
                Dim index As Integer = 1

                ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
                'For Each dr As DataRow In GetService.Rows
                '    DirectCast(Me.Page.FindControl(String.Format("imgSchemeLegend{0:00}", index)), ImageButton).OnClientClick = _
                '        String.Format("javascript:window.openNewWin('{0}');return false;", _
                '            IIf(Session("language") = TradChinese, dr("Scheme_Url_Chi"), dr("Scheme_Url")))
                '
                '    index += 1
                '
                'Next

                ' -- Remarks --
                'Select Case Session("language")
                '    Case TradChinese
                '        gvRemarks.Columns(1).Visible = False
                '        gvRemarks.Columns(2).Visible = True
                '
                '    Case Else
                '        gvRemarks.Columns(1).Visible = True
                '        gvRemarks.Columns(2).Visible = False
                '
                'End Select


                Me.GridViewDataBind(gvRemarks, Session(SESS_SearchRemarkDataTable))

                Select Case Session("language")
                    Case TradChinese
                        gvRemarks.Columns(3).Visible = True
                        gvRemarks.Columns(2).Visible = False
                    Case Else
                        gvRemarks.Columns(3).Visible = False
                        gvRemarks.Columns(2).Visible = True
                End Select

                ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

                ' -- Result grid --
                Me.GridViewDataBind(Me.gvResult, Session(SESS_SearchResultDataTable))

                ' -- Last update date --
                lblUpdateDate.Text = Replace(Me.GetGlobalResourceObject("Text", "UpdateDate"), "%s", (New Formatter).formatDisplayDate(udtServiceDirectoryBLL.GetLastUpdate))
                lnkBtnTop1.Text = Trim(Me.GetGlobalResourceObject("Text", "GoToTop").ToString)

            Case 1

                ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
                ' -- Remarks --
                'Select Case Session("language")
                '    Case TradChinese
                '        gvRemarks_noFee.Columns(1).Visible = False
                '        gvRemarks_noFee.Columns(2).Visible = True
                '
                '    Case Else
                '        gvRemarks_noFee.Columns(1).Visible = True
                '        gvRemarks_noFee.Columns(2).Visible = False
                '
                'End Select
                Me.GridViewDataBind(gvRemarks_noFee, Session(SESS_SearchRemarkDataTable))

                Select Case Session("language")
                    Case TradChinese
                        gvRemarks_noFee.Columns(3).Visible = True
                        gvRemarks_noFee.Columns(2).Visible = False
                    Case Else
                        gvRemarks_noFee.Columns(3).Visible = False
                        gvRemarks_noFee.Columns(2).Visible = True
                End Select


                ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

                ' re-bind the grid
                Me.GridViewDataBind(Me.gvResultWithoutFee, Session(SESS_SearchResultDataTable))

                lnkBtnTop1_noFee.Text = Trim(Me.GetGlobalResourceObject("Text", "GoToTop").ToString)
                lblUpdateDate_noFee.Text = Replace(Me.GetGlobalResourceObject("Text", "UpdateDate"), "%s", (New Formatter).formatDisplayDate(udtServiceDirectoryBLL.GetLastUpdate))
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
                ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                If String.IsNullOrEmpty(rboProfessional.SelectedValue) Then
                    lblSearchResult_noFee.Text = AntiXssEncoder.HtmlEncode(Me.GetGlobalResourceObject("Text", "SearchResultLabel"), True)
                Else
                    lblSearchResult_noFee.Text = AntiXssEncoder.HtmlEncode(Me.GetGlobalResourceObject("Text", "SearchResult").ToString.Replace("%s", rboProfessional.SelectedItem.Text), True)
                End If
                ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]

                ' -- Result per page --
                initResultsPerPage(Me.mvResult.ActiveViewIndex)

                ' -- Scheme legend --
                Dim index As Integer = 1

                ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
                'For Each dr As DataRow In GetService.Rows
                '    DirectCast(Me.Page.FindControl(String.Format("imgSchemeLegendNoFee{0:00}", index)), ImageButton).OnClientClick = _
                '        String.Format("javascript:window.openNewWin('{0}');return false;", _
                '            IIf(Session("language") = TradChinese, dr("Scheme_Url_Chi"), dr("Scheme_Url")))
                '
                '    index += 1
                '
                'Next
                ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

        End Select


        changeLblShowHideSearchCriteria()

    End Sub

    Private Function KeepSessionAlive() As String
        Dim int_MilliSecondsTimeOut As Integer = (HttpContext.Current.Session.Timeout * 60000) - 30000
        Dim sScript As New StringBuilder
        sScript.Append("<script type='text/javascript'>" & vbNewLine)
        'Number Of Reconnects   
        sScript.Append("var count=0;" & vbNewLine)
        'Maximum reconnects Setting   
        sScript.Append("var max = 10;" & vbNewLine)
        sScript.Append("function Reconnect(){" & vbNewLine)
        sScript.Append("count++;" & vbNewLine)
        sScript.Append("var d = new Date();" & vbNewLine)
        sScript.Append("var curr_hour = d.getHours();" & vbNewLine)
        sScript.Append("var curr_min = d.getMinutes();" & vbNewLine)
        sScript.Append("if (count < max){" & vbNewLine)
        sScript.Append("window.status = 'Refreshed ' + count.toString() + ' time(s) [' + curr_hour + ':' + curr_min + ']';" & vbNewLine)
        sScript.Append("var img = new Image(1,1);" & vbNewLine)
        sScript.Append("img.src = 'reconnect.aspx';" & vbNewLine)

        sScript.Append("}" & vbNewLine)
        sScript.Append("}" & vbNewLine)
        sScript.Append("window.setInterval('Reconnect()'," & int_MilliSecondsTimeOut.ToString() & "); //Set to length required" & vbNewLine)
        sScript.Append("</script>")

        Return sScript.ToString

    End Function

#End Region

    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
    Private Sub InitializeKeywordsSearchControl()
        txtServiceProvider.Text = String.Empty
        txtPracticeName.Text = String.Empty
        txtPracticeAddr.Text = String.Empty
    End Sub
    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

    Private Sub InitializeHealthcareProfessional()
        rboProfessional.DataSource = GetHealthcareProfessional()

        rboProfessional.DataValueField = "ServiceCategoryCodeSD"

        Select Case Session("language")
            Case TradChinese
                rboProfessional.DataTextField = "ServiceCategoryDescSDChi"
            Case Else
                rboProfessional.DataTextField = "ServiceCategoryDescSD"

        End Select

        rboProfessional.DataBind()

    End Sub

    Private Sub InitializeService()
        ' Clear the tree
        If TreeViewService.Nodes.Count > 0 Then
            For Each tn As TreeNode In TreeViewService.Nodes
                tn.ChildNodes.Clear()
            Next

            TreeViewService.Nodes.Clear()

        End If

        Dim lang As String = Session("language")

        ' Get the Scheme and SubsidizeGroup from DB
        Dim dtService As DataTable = GetService()

        ' Bind the tree
        For Each drScheme As DataRow In dtService.Rows
            ' Level 1 - Scheme
            Dim nodeScheme As New TreeNode

            ' Text
            If lang = TradChinese Then
                nodeScheme.Text = String.Format("&nbsp;<U>{0}</U>", drScheme("Scheme_Desc_Chi"))
            Else
                nodeScheme.Text = String.Format("&nbsp;<U>{0}</U>", drScheme("Scheme_Desc"))
            End If

            ' Value
            nodeScheme.Value = drScheme("Scheme_Code")

            ' Logo
            If drScheme("Logo_Available") = "Y" Then
                nodeScheme.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SDSchemeLogoPath").ToString.Replace("{SchemeCode}", drScheme("Scheme_Code"))
            End If

            ' Url
            If lang = TradChinese Then
                If Not IsDBNull(drScheme("Scheme_Url_Chi")) Then
                    nodeScheme.NavigateUrl = drScheme("Scheme_Url_Chi")
                    nodeScheme.Target = "_blank"
                End If

            Else
                If Not IsDBNull(drScheme("Scheme_Url")) Then
                    nodeScheme.NavigateUrl = drScheme("Scheme_Url")
                    nodeScheme.Target = "_blank"
                End If

            End If

            ' Level 2 - Subsidize
            For Each drSubsidize As DataRow In DirectCast(drScheme("SubsidizeGroup"), DataTable).Rows
                Dim nodeSubsidize As New TreeNode

                ' Text
                If lang = TradChinese Then
                    nodeSubsidize.Text = drSubsidize("Subsidize_Desc_Chi")
                Else
                    nodeSubsidize.Text = drSubsidize("Subsidize_Desc")
                End If

                ' Value
                nodeSubsidize.Value = drSubsidize("Search_Group")

                nodeSubsidize.NavigateUrl = String.Empty
                nodeSubsidize.ShowCheckBox = True
                nodeSubsidize.SelectAction = TreeNodeSelectAction.SelectExpand

                nodeScheme.ChildNodes.Add(nodeSubsidize)

            Next

            TreeViewService.Nodes.Add(nodeScheme)

        Next

        ' Expand the tree by default (and cannot be collapsed)
        TreeViewService.ExpandAll()

    End Sub

    Private Sub InitializeDistrict()
        Dim selectedLang As String = Session("language")

        ' Initialize the 18 Districts treeView
        Dim dvDistrictList As DataView = GetDistrict.DefaultView

        dvDistrictList.RowFilter = "district_board = ''"
        dvDistrictList.Sort = "area_code"

        Dim cb_Area As CheckBox
        ' Set the level 1 node for the 18 Districts treeView
        For i As Integer = 0 To dvDistrictList.Count - 1
            cb_Area = CType(Me.FindControl("cb_area_" + dvDistrictList.ToTable().Rows(i)("area_code").ToString().Trim), CheckBox)
            If selectedLang.Equals(English) Then
                cb_Area.Text = Trim(dvDistrictList.ToTable().Rows(i)("area_name").ToString())
            ElseIf selectedLang.Equals(TradChinese) Then
                cb_Area.Text = Trim(dvDistrictList.ToTable().Rows(i)("area_name_chi").ToString())
            End If
        Next

        ' Set the level 2 node for the 18 Districts treeView
        dvDistrictList.RowFilter = "district_board <> '' and area_code = 1"
        dvDistrictList.Sort = "area_code, district_board"
        Me.cbl_area_1.DataSource = dvDistrictList
        If selectedLang.Equals(English) Then
            Me.cbl_area_1.DataTextField = "district_board"
        ElseIf selectedLang.Equals(TradChinese) Then
            Me.cbl_area_1.DataTextField = "district_board_chi"
        End If
        Me.cbl_area_1.DataValueField = "district_board_shortname_SD"
        Me.cbl_area_1.DataBind()

        ' Set the level 2 node for the 18 Districts treeView
        dvDistrictList.RowFilter = "district_board <> '' and area_code = 2"
        dvDistrictList.Sort = "area_code, district_board"
        Me.cbl_area_2.DataSource = dvDistrictList
        If selectedLang.Equals(English) Then
            Me.cbl_area_2.DataTextField = "district_board"
        ElseIf selectedLang.Equals(TradChinese) Then
            Me.cbl_area_2.DataTextField = "district_board_chi"
        End If
        Me.cbl_area_2.DataValueField = "district_board_shortname_SD"
        Me.cbl_area_2.DataBind()

        ' Set the level 2 node for the 18 Districts treeView
        dvDistrictList.RowFilter = "district_board <> '' and area_code = 3"
        dvDistrictList.Sort = "area_code, district_board"
        Me.cbl_area_3.DataSource = dvDistrictList
        If selectedLang.Equals(English) Then
            Me.cbl_area_3.DataTextField = "district_board"
        ElseIf selectedLang.Equals(TradChinese) Then
            Me.cbl_area_3.DataTextField = "district_board_chi"
        End If
        Me.cbl_area_3.DataValueField = "district_board_shortname_SD"
        Me.cbl_area_3.DataBind()

        If selectedLang.Equals(English) Then
            Me.cbl_area_1.RepeatColumns = 1
            Me.cbl_area_1.RepeatDirection = RepeatDirection.Vertical
            Me.cbl_area_2.RepeatColumns = 1
            Me.cbl_area_2.RepeatDirection = RepeatDirection.Vertical
            Me.cbl_area_3.RepeatColumns = 1
            Me.cbl_area_3.RepeatDirection = RepeatDirection.Vertical

            Me.cbl_area_1.Style.Add("font-size", "10pt")
            Me.cbl_area_2.Style.Add("font-size", "10pt")
            Me.cbl_area_3.Style.Add("font-size", "10pt")
            'CRE15-005 New IDPSIVSS scheme [Start][Philip]
            'tblDistrictList.Style.Item("width") = "385px"
            tblDistrictList.Style.Item("width") = "377px"
            'CRE15-005 New IDPSIVSS scheme [End][Philip]
        Else
            Me.cbl_area_1.RepeatColumns = 1
            Me.cbl_area_1.RepeatDirection = RepeatDirection.Vertical
            Me.cbl_area_2.RepeatColumns = 1
            Me.cbl_area_2.RepeatDirection = RepeatDirection.Vertical
            Me.cbl_area_3.RepeatColumns = 1
            Me.cbl_area_3.RepeatDirection = RepeatDirection.Vertical

            Me.cbl_area_1.Style.Add("font-size", "12pt")
            Me.cbl_area_2.Style.Add("font-size", "12pt")
            Me.cbl_area_3.Style.Add("font-size", "12pt")

            tblDistrictList.Style.Item("width") = "280px"
        End If

    End Sub

    Private Sub initResultsPerPage(ByVal activeViewIndex As Integer)
        Dim i As Integer
        Dim strResultPerPage As String
        Dim strResultPerPageList As String()
        Dim newItem As System.Web.UI.WebControls.ListItem
        Dim strSelectedResultsPerPage As String = String.Empty

        ' get the dropdown list value from system parm, parm_value1 for vaccine, parm_value2 for voucher
        strResultPerPage = String.Empty
        If activeViewIndex = 0 Then
            Call (New GeneralFunction).getSystemParameter("SDIR_ddlResultPerPage", strResultPerPage, String.Empty)
        ElseIf activeViewIndex = 1 Then
            Call (New GeneralFunction).getSystemParameter("SDIR_ddlResultPerPage", String.Empty, strResultPerPage)
        End If

        strResultPerPage = strResultPerPage.Trim
        If Right(strResultPerPage, 1) = ";" Then
            strResultPerPage = Left(strResultPerPage, Len(strResultPerPage) - 1)
        End If

        strResultPerPageList = strResultPerPage.Split(";")
        If activeViewIndex = 0 Then
            ' INT16-0010 Fix concurrent search problem [Start][Winnie]
            strSelectedResultsPerPage = Me.ddlResultPerPage.SelectedValue
            ' INT16-0010 Fix concurrent search problem [End][Winnie]

            If Me.ddlResultPerPage.Items.Count > 0 Then
                Me.ddlResultPerPage.Items.Clear()
            End If

            For i = 0 To strResultPerPageList.Length - 1
                newItem = New System.Web.UI.WebControls.ListItem
                newItem.Text = Replace(Me.GetGlobalResourceObject("Text", "ResultsPerPageDropDown"), "%s", strResultPerPageList(i).Trim)
                newItem.Value = strResultPerPageList(i).Trim
                newItem.Enabled = True
                ddlResultPerPage.Items.Add(newItem)
            Next

            ' INT16-0010 Fix concurrent search problem [Start][Winnie]
            If strSelectedResultsPerPage <> String.Empty Then
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
                ddlResultPerPage.SelectedValue = AntiXssEncoder.HtmlEncode(strSelectedResultsPerPage, True)
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
            End If

            Dim pagesize As Integer
            pagesize = CInt(ddlResultPerPage.SelectedValue)
            Me.gvResult.PageSize = pagesize
            'If Not Session(SESS_pageSize) Is Nothing Then
            '    ddlResultPerPage.SelectedValue = Session(SESS_pageSize)
            'End If
            ' INT16-0010 Fix concurrent search problem [End][Winnie]

        ElseIf activeViewIndex = 1 Then
            ' INT16-0010 Fix concurrent search problem [Start][Winnie]
            strSelectedResultsPerPage = Me.ddlResultPerPage_noFee.SelectedValue
            ' INT16-0010 Fix concurrent search problem [End][Winnie]

            If ddlResultPerPage_noFee.Items.Count > 0 Then
                ddlResultPerPage_noFee.Items.Clear()
            End If

            For i = 0 To strResultPerPageList.Length - 1
                newItem = New System.Web.UI.WebControls.ListItem
                newItem.Text = Replace(Me.GetGlobalResourceObject("Text", "ResultsPerPageDropDown"), "%s", strResultPerPageList(i).Trim)
                newItem.Value = strResultPerPageList(i).Trim
                newItem.Enabled = True
                ddlResultPerPage_noFee.Items.Add(newItem)
            Next

            ' INT16-0010 Fix concurrent search problem [Start][Winnie]
            If strSelectedResultsPerPage <> String.Empty Then
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
                ddlResultPerPage_noFee.SelectedValue = AntiXssEncoder.HtmlEncode(strSelectedResultsPerPage, True)
                ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
            End If

            Dim pagesize As Integer
            pagesize = CInt(ddlResultPerPage_noFee.SelectedValue)
            Me.gvResultWithoutFee.PageSize = pagesize
            'If Not Session(SESS_pageSize_noFee) Is Nothing Then
            '    ddlResultPerPage_noFee.SelectedValue = Session(SESS_pageSize_noFee)
            'End If
            ' INT16-0010 Fix concurrent search problem [End][Winnie]

        End If

    End Sub

    Private Sub rboProfessional_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboProfessional.SelectedIndexChanged
        If Not TreeViewService.SelectedNode Is Nothing Then TreeViewService.SelectedNode.Selected = False
        RenderEligibleService()
    End Sub

    Private Sub RenderEligibleService()
        Dim strEligibleService As String = GetEligibleService.Select(String.Format("service_category_code_SD = '{0}'", rboProfessional.SelectedValue))(0)("eligible_service")
        Dim intActiveNodeCount As Integer = 0
        Dim tnLastActiveNode As TreeNode = Nothing

        If strEligibleService = "ALL" Then
            For Each n1 As TreeNode In TreeViewService.Nodes
                For Each n2 As TreeNode In n1.ChildNodes
                    Dim blnChecked As Boolean = n2.Checked
                    'n2.Checked = blnChecked
                    n2.ImageUrl = Nothing
                    intActiveNodeCount += 1
                    tnLastActiveNode = n2

                Next
            Next

        Else
            Dim lstEligService As New List(Of String)(strEligibleService.Split(";".ToCharArray, StringSplitOptions.RemoveEmptyEntries))

            For Each n1 As TreeNode In TreeViewService.Nodes
                For Each n2 As TreeNode In n1.ChildNodes
                    If lstEligService.Contains(n2.Value) Then
                        ' Enable the checkbox
                        Dim blnChecked As Boolean = n2.Checked
                        'n2.Checked = blnChecked
                        n2.ImageUrl = String.Empty
                        intActiveNodeCount += 1
                        tnLastActiveNode = n2

                    Else
                        ' Disable the checkbox
                        n2.Checked = False
                        n2.ImageUrl = ("~/Images/others/checkbox_D.png")

                    End If

                Next
            Next

        End If

        ' Check the checkbox if only one option is available for that professional
        'If intActiveNodeCount = 1 AndAlso Not IsNothing(Session(SESS_isReRenderPage)) AndAlso Session(SESS_isReRenderPage) <> "ReRenderPage" Then
        If intActiveNodeCount = 1 Then
            'tnLastActiveNode.Checked = True
        End If

        Me.TreeViewService.NodeStyle.CssClass = "searchCriteriaText"

    End Sub

    '
    Protected Sub iBtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnSearch.Click
        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        'If (MyBase.IsPageRefreshed) Then
        '    _blnIsRequireHandlePageRefresh = True
        '    Return
        'End If
        ' INT16-0010 Fix concurrent search problem [End][Winnie]

        ' Init
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        mvResult.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Search click")

        If Not TreeViewService.SelectedNode Is Nothing Then TreeViewService.SelectedNode.Selected = False

        Dim strLanguage As String = Session("language")

        ' Set the show/hide search criteria button to invisible
        imgShowHideSearchCriteria.Style.Add("display", "none")
        changeLblShowHideSearchCriteria()

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        'Gather Quick Search field Inputted value
        Dim strServiceProviderName As String = txtServiceProvider.Text.Trim
        Dim strPracticeName As String = txtPracticeName.Text.Trim
        Dim strPracticeAddr As String = txtPracticeAddr.Text.Trim

        ' Truncate max length in server side
        If Not String.IsNullOrEmpty(strServiceProviderName) And strServiceProviderName.Length > 40 Then
            strServiceProviderName = strServiceProviderName.Substring(0, 40)
        End If
        If Not String.IsNullOrEmpty(strPracticeName) And strPracticeName.Length > 100 Then
            strPracticeName = strPracticeName.Substring(0, 100)
        End If
        If Not String.IsNullOrEmpty(strPracticeAddr) And strPracticeAddr.Length > 100 Then
            strPracticeAddr = strPracticeAddr.Substring(0, 100)
        End If
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        ' Gather the Healthcare Professional input (well... just 1 sentence)
        Dim strProfessional As String = rboProfessional.SelectedValue

        ' Gather the Service input
        Dim lstSelectedService As New List(Of String)
        ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]
        Dim lstResultDisplayService As New List(Of String)
        ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]
        For Each n1 As TreeNode In TreeViewService.Nodes

            ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]
            Dim blnAllNoChecked As Boolean = True
            For Each n2 As TreeNode In n1.ChildNodes
                If n2.Checked Then
                    lstSelectedService.Add(n2.Value)
                    blnAllNoChecked = False
                End If
            Next

            If blnAllNoChecked Then
                For Each n2 As TreeNode In n1.ChildNodes
                    lstResultDisplayService.Add(n2.Value)
                Next
            Else
                For Each n2 As TreeNode In n1.ChildNodes
                    If n2.Checked Then
                        lstResultDisplayService.Add(n2.Value)
                    End If
                Next
            End If
            ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]
        Next

        ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]
        Dim strSelectedService As String = String.Join(";", lstSelectedService.ToArray)

        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        ViewState(VS_SelectedServiceList) = lstSelectedService
        ' INT16-0010 Fix concurrent search problem [End][Winnie]
        ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]
        ViewState(VS_ResultDisplayServiceList) = lstResultDisplayService
        ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]

        ' Gather the District input
        Dim strDistrictList As String = String.Empty
        Dim cbl_area As WebControls.CheckBoxList
        Dim cbl_area_item As WebControls.ListItem

        For i As Integer = 0 To 2
            cbl_area = CType(Me.FindControl("cbl_area_" + CStr(i + 1)), System.Web.UI.WebControls.CheckBoxList)
            For Each cbl_area_item In cbl_area.Items
                If cbl_area_item.Selected Then
                    strDistrictList += cbl_area_item.Value.Trim + ";"
                End If
            Next
        Next

        ' --- Validation ---
        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        If strServiceProviderName = String.Empty And
            strPracticeName = String.Empty And
            strPracticeAddr = String.Empty And
            strProfessional = String.Empty And
            strSelectedService = String.Empty And
            strDistrictList = String.Empty Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)

            udtAuditLogEntry.AddDescripton("StackTrace", "No Searching Criteria is inputted")

        End If

        If udcMessageBox.GetCodeTable.Rows.Count > 0 Then
            udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00020, "Search fail")

            udtAuditLogEntry.WriteLog(LogID.LOG00015, "Search end")

            Return

        End If

        ' --- End of Validation ---

        Dim udtServiceDirectoryBLL As New ServiceDirectoryBLL
        Dim dtSDSubsidizeGroup As DataTable = udtServiceDirectoryBLL.GetSDSubsidizeGroupDT

        ' Init
        Session(SESS_SearchResultDataTable) = Nothing

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        udtAuditLogEntry.AddDescripton("Service Provider", strServiceProviderName)
        udtAuditLogEntry.AddDescripton("Practice Name", strPracticeName)
        udtAuditLogEntry.AddDescripton("Practice Address", strPracticeAddr)
        udtAuditLogEntry.AddDescripton("Healthcare Professional", strProfessional)
        udtAuditLogEntry.AddDescripton("Service", strSelectedService)
        udtAuditLogEntry.AddDescripton("District", strDistrictList)
        udtAuditLogEntry.AddDescripton("Language", strLanguage)
        udtAuditLogEntry.WriteLog(LogID.LOG00005, "Process searching start")

        Dim dtResult As DataTable = getResultList(strProfessional, strSelectedService, strDistrictList, strLanguage.ToString.ToUpper, strServiceProviderName, strPracticeName, strPracticeAddr)

        udtAuditLogEntry.AddDescripton("Service Provider", strServiceProviderName)
        udtAuditLogEntry.AddDescripton("Practice Name", strPracticeName)
        udtAuditLogEntry.AddDescripton("Practice Address", strPracticeAddr)
        udtAuditLogEntry.AddDescripton("Healthcare Professional", strProfessional)
        udtAuditLogEntry.AddDescripton("Service", strSelectedService)
        udtAuditLogEntry.AddDescripton("District", strDistrictList)
        udtAuditLogEntry.AddDescripton("Language", strLanguage)
        udtAuditLogEntry.WriteLog(LogID.LOG00006, "Process searching complete")

        If IsNothing(dtResult) Then
            ' No record found
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLogEntry.WriteLog(LogID.LOG00009, "Search fail: No records found")

            udtAuditLogEntry.WriteLog(LogID.LOG00015, "Search end")

            Return

        End If

        ' Contains records
        Dim strRecordLimit As String = (New GeneralFunction).getSystemParameterValue1("SDIR_returnRecordLimit")

        ' Check result over limit
        If dtResult.Rows.Count > CInt(strRecordLimit) Then
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00002)
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLogEntry.AddDescripton("StackTrace", String.Format("No. of records = {0}, Record limit = {1}", dtResult.Rows.Count, strRecordLimit))

            udtAuditLogEntry.WriteLog(LogID.LOG00008, "Search fail: Too many records found")

            udtAuditLogEntry.WriteLog(LogID.LOG00015, "Search end")

            Return

        End If

        ' Display the result
        udtAuditLogEntry.AddDescripton("No. of record", dtResult.Rows.Count)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, "Search successful")

        ' Save to session
        Session(SESS_SearchResultDataTable) = dtResult

        ' -- Heading --
        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        Dim intDummyCol As Integer = 5
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
        Dim strgroupScheme As String = ""
        For Each dtResultRow As DataRow In dtResult.Rows
            strgroupScheme += dtResultRow("joined_scheme").ToString()
        Next

        Dim arrSplitScheme As String() = strgroupScheme.Split("|")

        Dim strResultScheme As String = ""

        For Each r As String In arrSplitScheme
            If Not strResultScheme.Contains(r) Then
                strResultScheme = strResultScheme + ";" + r
            End If
        Next


        Session(SESS_SearchRemarkDataTable) = Nothing

        Dim dtRemarks As DataTable = udtServiceDirectoryBLL.GetRemarksByScheme(strResultScheme)

        Session(SESS_SearchRemarkDataTable) = dtRemarks

        ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

        If CheckResultwithServiceFee(dtResult) Then
            ' --- The search involves an item with Service Fee ---

            ' -- Heading --
            ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
            If String.IsNullOrEmpty(strProfessional) Then
                lblSearchResult.Text = Me.GetGlobalResourceObject("Text", "SearchResultLabel")
            Else
                lblSearchResult.Text = Me.GetGlobalResourceObject("Text", "SearchResult").ToString.Replace("%s", rboProfessional.SelectedItem.Text)
            End If
            ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

            ' -- Result per page --
            initResultsPerPage(0)

            ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
            ' -- Remarks --
            'gvRemarks.DataSource = udtServiceDirectoryBLL.GetRemarksByProf(strResultScheme)
            'gvRemarks.DataSource = udtServiceDirectoryBLL.GetRemarksByProf("MP", "01")
            ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

            'Select Case Session("language")
            '    Case TradChinese
            '        gvRemarks.Columns(1).Visible = False
            '        gvRemarks.Columns(2).Visible = True
            '
            '    Case Else
            '        gvRemarks.Columns(1).Visible = True
            '        gvRemarks.Columns(2).Visible = False
            '
            'End Select
            Me.GridViewDataBind(gvRemarks, Session(SESS_SearchRemarkDataTable), "Scheme", "ASC", False)

            Select Case Session("language")
                Case TradChinese
                    gvRemarks.Columns(3).Visible = True
                    gvRemarks.Columns(2).Visible = False
                Case Else
                    gvRemarks.Columns(3).Visible = False
                    gvRemarks.Columns(2).Visible = True
            End Select

            ' -- Legend --
            'For i As Integer = 1 To 10
            '    Me.Page.FindControl(String.Format("divSchemeLegend{0:00}", i)).Visible = False
            'Next
            '
            'Dim index As Integer = 1
            '
            'For Each dr As DataRow In GetService.Rows
            '    Me.Page.FindControl(String.Format("divSchemeLegend{0:00}", index)).Visible = True
            '
            '    Dim imgSchemeLegend As ImageButton = Me.Page.FindControl(String.Format("imgSchemeLegend{0:00}", index))
            '    Dim lblSchemeLegend As Label = Me.Page.FindControl(String.Format("lblSchemeLegend{0:00}", index))
            '
            '    imgSchemeLegend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SDSchemeLogoPath").ToString.Replace("{SchemeCode}", dr("Scheme_Code"))
            '
            '    Select Case strLanguage
            '        Case TradChinese
            '            imgSchemeLegend.OnClientClick = String.Format("javascript:window.openNewWin('{0}');return false;", dr("Scheme_Url_Chi"))
            '            lblSchemeLegend.Text = dr("Scheme_Short_Form_Chi")
            '
            '        Case Else
            '            imgSchemeLegend.OnClientClick = String.Format("javascript:window.openNewWin('{0}');return false;", dr("Scheme_Url"))
            '            lblSchemeLegend.Text = dr("Scheme_Short_Form")
            '
            '    End Select
            '
            '    index += 1
            '
            'Next

            'tdSchemeLegend.Width = Me.GetGlobalResourceObject("Text", "SDIR_colWidth_SchemeLegend_view01")
            ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

            ' -- Data grid --

            ' Init
            For intNumber As Integer = 1 To 10
                gvResult.Columns(intDummyCol + intNumber - 1).Visible = False
            Next

            ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
            Dim blnInvolveServiceFee As Boolean = False

            For Each strSubsidize As String In lstSelectedService
                If Not IsDBNull(dtSDSubsidizeGroup.Select(String.Format("Search_Group = '{0}'", strSubsidize))(0)("Subsidize_Fee_Column_Name")) Then
                    blnInvolveServiceFee = True
                    Exit For
                End If
            Next

            ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]

            'If Not blnInvolveServiceFee Then
            '    For Each dr As DataRow In dtSDSubsidizeGroup.Rows
            '        ' Skip the item with no fee
            '        If IsDBNull(dr("Subsidize_Fee_Column_Name")) Then Continue For

            '        Dim intNumber As Integer = CInt(dr("Subsidize_Fee_Column_Name").ToString.Replace("subsidize_fee_", String.Empty))
            '        gvResult.Columns(intDummyCol + intNumber - 1).Visible = True

            '        ' Change the header text to Subsidize Short Form
            '        If Session("language") = TradChinese Then
            '            gvResult.Columns(intDummyCol + intNumber - 1).HeaderText = dr("Subsidize_Short_Form_Chi")
            '        Else
            '            gvResult.Columns(intDummyCol + intNumber - 1).HeaderText = dr("Subsidize_Short_Form")
            '        End If

            '    Next
            'Else
            ' Show column based on the selected service in searching
            For Each strService As String In lstResultDisplayService
                'For Each strService As String In lstSelectedService
                For Each dr As DataRow In dtSDSubsidizeGroup.Select(String.Format("Search_Group = '{0}'", strService))
                    ' Skip the item with no fee
                    If IsDBNull(dr("Subsidize_Fee_Column_Name")) Then Continue For

                    Dim intNumber As Integer = CInt(dr("Subsidize_Fee_Column_Name").ToString.Replace("subsidize_fee_", String.Empty))
                    gvResult.Columns(intDummyCol + intNumber - 1).Visible = True

                    ' Change the header text to Subsidize Short Form
                    If Session("language") = TradChinese Then
                        gvResult.Columns(intDummyCol + intNumber - 1).HeaderText = dr("Subsidize_Short_Form_Chi")
                    Else
                        gvResult.Columns(intDummyCol + intNumber - 1).HeaderText = dr("Subsidize_Short_Form")
                    End If
                Next
            Next
            'End If
            ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]

            ' Bind the grid
            If strLanguage = TradChinese Then
                Me.GridViewDataBind(gvResult, dtResult, "district_board_chi", "ASC", False)
            Else
                Me.GridViewDataBind(gvResult, dtResult, "district_board_shortname_SD", "ASC", False)
            End If

            ' -- Last update date --
            lblUpdateDate.Text = Replace(Me.GetGlobalResourceObject("Text", "UpdateDate"), "%s", (New Formatter).formatDisplayDate(udtServiceDirectoryBLL.GetLastUpdate))
            lnkBtnTop1.Text = Me.GetGlobalResourceObject("Text", "GoToTop")

            ' Show the Show/Hide search criteria button
            imgShowHideSearchCriteria.Style.Add("display", "")
            changeLblShowHideSearchCriteria()

            mvResult.ActiveViewIndex = 0

            ' init the scheme logo ledger
            'initSchemeLedger()
            mvResult.Visible = True

            ' Scroll to the result gridview
            Dim strScript As String
            strScript = "function GoToResult() {"
            strScript += "var iReturnValue = 0;" + vbCrLf
            strScript += "elementid=document.getElementById('imgMarker_withFee');" + vbCrLf
            strScript += "while( elementid != null ){" + vbCrLf
            strScript += "iReturnValue += elementid.offsetTop;" + vbCrLf
            strScript += "elementid = elementid.offsetParent;" + vbCrLf
            strScript += "}" + vbCrLf
            strScript += "scrollTo(0, iReturnValue);" + vbCrLf
            strScript += "}" + vbCrLf
            strScript += "setTimeout('GoToResult(); ',5);" + vbCrLf
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "ClientScript", strScript, True)


        Else
            ' --- The search does not involve any items with Service Fee ---

            '' Save to session
            'Session(SESS_SearchResultDataTable) = dtResult

            ' -- Heading --
            ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
            If String.IsNullOrEmpty(strProfessional) Then
                lblSearchResult_noFee.Text = Me.GetGlobalResourceObject("Text", "SearchResultLabel")
            Else
                lblSearchResult_noFee.Text = Me.GetGlobalResourceObject("Text", "SearchResult").ToString.Replace("%s", rboProfessional.SelectedItem.Text)
            End If
            ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

            ' -- Result per page --
            initResultsPerPage(1)

            ' -- Data Grid --
            If strLanguage = TradChinese Then
                Me.GridViewDataBind(gvResultWithoutFee, dtResult, "district_board_chi", "ASC", False)
            Else
                Me.GridViewDataBind(gvResultWithoutFee, dtResult, "district_board_shortname_SD", "ASC", False)
            End If

            ' -- Remarks --

            ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
            'gvRemarks_noFee.DataSource = udtServiceDirectoryBLL.GetRemarksByProf(strProfessional, "02")
            'gvRemarks_noFee.DataBind()
            '
            'Select Case Session("language")
            '    Case TradChinese
            '        gvRemarks_noFee.Columns(1).Visible = False
            '        gvRemarks_noFee.Columns(2).Visible = True
            '
            '    Case Else
            '        gvRemarks_noFee.Columns(1).Visible = True
            '        gvRemarks_noFee.Columns(2).Visible = False
            '
            'End Select

            Me.GridViewDataBind(gvRemarks_noFee, Session(SESS_SearchRemarkDataTable), "Scheme", "ASC", False)

            Select Case Session("language")
                Case TradChinese
                    gvRemarks_noFee.Columns(3).Visible = True
                    gvRemarks_noFee.Columns(2).Visible = False
                Case Else
                    gvRemarks_noFee.Columns(3).Visible = False
                    gvRemarks_noFee.Columns(2).Visible = True
            End Select

            ' -- Last update date --
            lblUpdateDate_noFee.Text = Replace(Me.GetGlobalResourceObject("Text", "UpdateDate"), "%s", (New Formatter).formatDisplayDate(udtServiceDirectoryBLL.GetLastUpdate))
            lnkBtnTop1_noFee.Text = Me.GetGlobalResourceObject("Text", "GoToTop")

            imgShowHideSearchCriteria.Style.Add("display", "")
            changeLblShowHideSearchCriteria()

            mvResult.ActiveViewIndex = 1
            mvResult.Visible = True

            ' -- Legend --
            'For i As Integer = 1 To 10
            '    Me.Page.FindControl(String.Format("divSchemeLedgerNoFee{0:00}", i)).Visible = False
            'Next

            'Dim index As Integer = 1
            '
            'For Each dr As DataRow In GetService.Rows
            '    ' Me.Page.FindControl(String.Format("divSchemeLedgerNoFee{0:00}", index)).Visible = True
            '
            '    Dim imgSchemeLegend As ImageButton = Me.gvRemarks_noFee.FindControl(String.Format("imgSchemeLegendNoFee{0:00}", index))
            '    '  Dim lblSchemeLegend As Label = Me.Page.FindControl(String.Format("lblSchemeLegendNoFee{0:00}", index))
            '
            '    'imgSchemeLegend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SDSchemeLogoPath").ToString.Replace("{SchemeCode}", dr("Scheme_Code"))
            '
            '    Select Case strLanguage
            '        Case TradChinese
            '            imgSchemeLegend.OnClientClick = String.Format("javascript:window.openNewWin('{0}');return false;", dr("Scheme_Url_Chi"))
            '            'lblSchemeLegend.Text = dr("Scheme_Short_Form_Chi")
            '
            '        Case Else
            '            imgSchemeLegend.OnClientClick = String.Format("javascript:window.openNewWin('{0}');return false;", dr("Scheme_Url"))
            '            ' lblSchemeLegend.Text = dr("Scheme_Short_Form")
            '
            '    End Select
            '
            '    index += 1
            '
            'Next

            'tdSchemeLedgerNoFee.Width = Me.GetGlobalResourceObject("Text", "SDIR_colWidth_SchemeLegend_view02")

            ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

            ' Scroll to top of the result
            Dim strScript As String
            strScript = "function GoToResult() {"
            strScript += "var iReturnValue = 0;" + vbCrLf
            strScript += "elementid=document.getElementById('imgMarker_noFee');" + vbCrLf
            strScript += "while( elementid != null ){" + vbCrLf
            strScript += "iReturnValue += elementid.offsetTop;" + vbCrLf
            strScript += "elementid = elementid.offsetParent;" + vbCrLf
            strScript += "}" + vbCrLf
            strScript += "scrollTo(0, iReturnValue);" + vbCrLf
            strScript += "}" + vbCrLf
            strScript += "setTimeout('GoToResult(); ',5);" + vbCrLf
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "ClientScript", strScript, True)


        End If

        udtAuditLogEntry.WriteLog(LogID.LOG00015, "Search end")

        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        ViewState(VS_SelectedProfessional) = strProfessional
        ViewState(VS_SelectedService) = strSelectedService
        ViewState(VS_SelectedDistrictList) = strDistrictList
        ' INT16-0010 Fix concurrent search problem [End][Winnie]

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        ViewState(VS_KwdSearchServiceProviderName) = strServiceProviderName
        ViewState(VS_KwdSearchPracticeName) = strPracticeName
        ViewState(VS_KwdSearchPracticeAddr) = strPracticeAddr
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

    End Sub

    ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
    Private Sub gvRemarks_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvRemarks.RowDataBound, gvRemarks_noFee.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).ColumnSpan = 2

        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            If Not e.Row.DataItem Is Nothing Then
                Dim i As Integer = e.Row.RowIndex + 1
                Dim strSchemeCode As String = DataBinder.Eval(e.Row.DataItem, "Scheme").ToString.Trim()
                Dim findHeader As String = DataBinder.Eval(e.Row.DataItem, "Num").ToString.Trim()
                Dim SchemeHeader As String = ""

                If findHeader = "0" Then
                    Dim imgSchemeLegend As New ImageButton()
                    imgSchemeLegend.ID = "imgSchemeLLegend" & i.ToString
                    imgSchemeLegend.Attributes.Add("Style", "vertical-align: middle; padding-bottom: 4px; margin-left: 5px;")

                    For Each dr As DataRow In GetService.Rows
                        If dr("Scheme_Code") = strSchemeCode Then
                            Select Case Session("language")
                                Case TradChinese
                                    SchemeHeader = DataBinder.Eval(e.Row.DataItem, "Description_Chi")
                                    imgSchemeLegend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SDSchemeLogoPath").ToString.Replace("{SchemeCode}", dr("Scheme_Code"))
                                    imgSchemeLegend.OnClientClick = String.Format("javascript:window.openNewWin('{0}');return false;", dr("Scheme_Url_Chi"))
                                Case Else
                                    SchemeHeader = DataBinder.Eval(e.Row.DataItem, "Description")
                                    imgSchemeLegend.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SDSchemeLogoPath").ToString.Replace("{SchemeCode}", dr("Scheme_Code"))
                                    imgSchemeLegend.OnClientClick = String.Format("javascript:window.openNewWin('{0}');return false;", dr("Scheme_Url"))
                            End Select
                        End If
                    Next
                    Dim lblSchemeHeader As New Label()
                    lblSchemeHeader.ID = "lblSchemeHeader" & i.ToString
                    lblSchemeHeader.Text = SchemeHeader
                    e.Row.Cells(1).ColumnSpan = 2
                    e.Row.Cells(1).Text = ""
                    e.Row.Cells(1).Controls.Add(lblSchemeHeader)
                    e.Row.Cells(1).Controls.Add(imgSchemeLegend)
                    e.Row.Cells(1).Attributes.Add("Style", "padding-top: 10px;")
                    e.Row.Cells(2).Text = ""
                    e.Row.Cells(3).Text = ""
                End If
            End If
        End If
    End Sub
    ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

    Protected Sub iBtnReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnReset.Click
        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        'If (MyBase.IsPageRefreshed) Then
        '    _blnIsRequireHandlePageRefresh = True
        '    Return
        'End If
        ' INT16-0010 Fix concurrent search problem [End][Winnie]

        ' Reset the page
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, "Reset click")

        mvResult.Visible = False
        Me.mvResult.ActiveViewIndex = -1

        imgShowHideSearchCriteria.Style.Add("display", "none")
        changeLblShowHideSearchCriteria()

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        ResetProfessionalControl()
        ResetDistrictControl()
        ResetServiceControl()
        ResetKeywordsSearchControl()
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        Session(SESS_SearchRemarkDataTable) = Nothing
        Session(SESS_SearchResultDataTable) = Nothing
        'Session(SESS_dtRemarks) = Nothing

        ' Reset ALL gridview pagesize
        Dim strResultPerPage As String
        Dim pageSize As Integer
        'If Me.mvResult.ActiveViewIndex = 0 Then
        'Session(SESS_pageSize) = Nothing
        strResultPerPage = String.Empty
        Call (New GeneralFunction).getSystemParameter("SDIR_ddlResultPerPage", strResultPerPage, String.Empty)
        strResultPerPage = strResultPerPage.Trim
        pageSize = CInt(Left(strResultPerPage, strResultPerPage.IndexOf(";")))

        Me.gvResult.PageSize = pageSize
        'ElseIf Me.mvResult.ActiveViewIndex = 1 Then
        'Session(SESS_pageSize_noFee) = Nothing
        strResultPerPage = String.Empty
        Call (New GeneralFunction).getSystemParameter("SDIR_ddlResultPerPage", String.Empty, strResultPerPage)
        strResultPerPage = strResultPerPage.Trim
        pageSize = CInt(Left(strResultPerPage, strResultPerPage.IndexOf(";")))

        Me.gvResultWithoutFee.PageSize = pageSize
        'End If

        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        ddlResultPerPage.ClearSelection()
        ddlResultPerPage_noFee.ClearSelection()
        ' INT16-0010 Fix concurrent search problem [End][Winnie]
    End Sub
    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
    Private Function getResultList(ByVal strProfessional As String, ByVal strService As String, ByVal strDistrictList As String, ByVal strLanguage As String, ByVal strServiceProviderName As String, ByVal strPracticeName As String, ByVal strPracticeAddr As String) As DataTable

        Dim dtResult As DataTable = (New ServiceDirectoryBLL).GetSearchResult(strProfessional, strService, strDistrictList, strLanguage, strServiceProviderName, strPracticeName, strPracticeAddr)

        If Not IsNothing(dtResult) Then
            Dim blnResultwithServiceFee As Boolean = CheckResultwithServiceFee(dtResult)

            'Massage Data
            Dim udtHealthCarePro As ProfessionModelCollection = GetHealthcareProfessional()
            Dim udtProfessionList As New Dictionary(Of String, ProfessionModel)
            For Each udtProfession As ProfessionModel In udtHealthCarePro
                udtProfessionList.Add(udtProfession.ServiceCategoryCodeSD, udtProfession)
            Next

            If String.IsNullOrEmpty(strProfessional) Then
                dtResult.Columns.Add("HealthCarePro", GetType(String))
                dtResult.Columns.Add("HealthCarePro_chi", GetType(String))

                For Each dr As DataRow In dtResult.Rows
                    dr("HealthCarePro") = udtProfessionList(dr("service_category_code_SD").ToString.Trim).ServiceCategoryDescSD
                    dr("HealthCarePro_chi") = udtProfessionList(dr("service_category_code_SD").ToString.Trim).ServiceCategoryDescSDChi
                Next

            End If

            If blnResultwithServiceFee Then
                dtResult.Columns.Add("practiceInfo", GetType(String))
                dtResult.Columns.Add("practiceInfo_chi", GetType(String))

                For Each dr As DataRow In dtResult.Rows
                    ' SP Name
                    dr("sp_chi_name") = CStr(IIf(dr("sp_chi_name").ToString.Trim.Length = 0, dr("sp_name"), dr("sp_chi_name"))).Trim()

                    ' Practice Information (EN)
                    Dim strPracticeInfoEN As String = dr("practice_name").ToString().Trim()
                    strPracticeInfoEN += "<BR />" + dr("address_eng").ToString().Trim()
                    strPracticeInfoEN += "<BR />" + Trim(dr("phone_daytime").ToString().Trim() & " ")

                    dr("practiceInfo") = strPracticeInfoEN

                    ' Practice Information (CH)
                    Dim strPracticeInfoCH As String = IIf(dr("practice_name_chi").ToString.Trim.Length = 0, dr("practice_name").ToString().Trim(), dr("practice_name_chi").ToString().Trim())
                    strPracticeInfoCH += "<BR />" + IIf(dr("address_chi").ToString.Trim.Length = 0, dr("address_eng").ToString().Trim(), dr("address_chi").ToString().Trim())
                    strPracticeInfoCH += "<BR />" + Trim(dr("phone_daytime").ToString().Trim() & " ")

                    dr("practiceInfo_chi") = strPracticeInfoCH
                Next

            Else
                dtResult.Columns.Add("Joined_Scheme_Order", GetType(String))

                Dim dtSDScheme As DataTable = udtServiceDirectoryBLL.GetSDSchemeDT

                For Each dr As DataRow In dtResult.Rows
                    dr("sp_chi_name") = CStr(IIf(dr("sp_chi_name").ToString.Trim.Length = 0, dr("sp_name"), dr("sp_chi_name"))).Trim()
                    dr("practice_name_chi") = CStr(IIf(dr("practice_name_chi").ToString.Trim.Length = 0, dr("practice_name"), dr("practice_name_chi"))).Trim()
                    dr("address_chi") = CStr(IIf(dr("address_chi").ToString.Trim.Length = 0, dr("address_eng"), dr("address_chi"))).Trim()

                    If Not IsDBNull(dr("joined_scheme")) Then
                        dr("Joined_Scheme_Order") = String.Empty

                        For Each strSchemeCode As String In dr("joined_scheme").ToString.Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                            dr("Joined_Scheme_Order") += dtSDScheme.Select(String.Format("Scheme_Code = '{0}'", strSchemeCode.Trim))(0)("Display_Seq").ToString.PadLeft(2, "0")
                        Next

                    End If

                Next

            End If
        End If

        Return dtResult

    End Function

    Private Function CheckResultwithServiceFee(ByVal dt As DataTable) As Boolean
        Dim blnInvolveServiceFee As Boolean = False

        If Not IsNothing(dt) Then
            For Each r As DataRow In dt.Rows
                If CInt(r("Subsidy_with_fee")) > 0 Then
                    blnInvolveServiceFee = True
                    Exit For
                End If
            Next
        End If

        Return blnInvolveServiceFee
    End Function

    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            ' Service Provider / Practice Name, Address and Telephone Number
            Dim lblSPname As Label = e.Row.FindControl("lblSPname")
            Dim lblPracticeInfo As Label = CType(e.Row.FindControl("lblPracticeInfo"), Label)
            Dim lblDistrictBoard As Label = CType(e.Row.FindControl("lblDistrictBoard"), Label)
            ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
            Dim lblHealthCareProfession As Label = CType(e.Row.FindControl("lblHealthCarePro"), Label)
            ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

            Select Case Session("language")
                Case TradChinese
                    lblSPname.Text = CStr(dr("sp_chi_name")).Trim
                    lblSPname.CssClass = "tableTextChi"
                    lblPracticeInfo.Text = CStr(dr("practiceInfo_chi")).Trim
                    lblPracticeInfo.CssClass = "tableTextChi"
                    lblDistrictBoard.Text = CStr(dr("district_board_chi")).Trim
                    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                    If dr.Table.Columns.Contains("HealthCarePro_chi") Then
                        lblHealthCareProfession.Text = CStr(dr("HealthCarePro_chi")).Trim
                    End If
                    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

                Case Else
                    lblSPname.Text = CStr(dr("sp_name")).Trim
                    lblPracticeInfo.Text = CStr(dr("practiceInfo")).Trim
                    lblDistrictBoard.Text = CStr(dr("district_board_shortname_SD")).Trim
                    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                    If dr.Table.Columns.Contains("HealthCarePro") Then
                        lblHealthCareProfession.Text = CStr(dr("HealthCarePro")).Trim
                    End If
                    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

            End Select

            ' Enrolled Scheme Logo
            Dim intLogo As Integer = 1
            Dim strSDSchemeLogoPath As String = Me.GetGlobalResourceObject("ImageUrl", "SDSchemeLogoPath")

            For Each strSchemeCode As String In dr("joined_scheme").ToString.Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                Dim imgLogo As Image = e.Row.FindControl(String.Format("imgJoinedScheme{0}", intLogo.ToString.PadLeft(2, "0")))
                imgLogo.ImageUrl = strSDSchemeLogoPath.Replace("{SchemeCode}", strSchemeCode)
                imgLogo.Visible = True

                intLogo += 1

            Next

            ' Service Fee
            Dim strSDServiceFeeFormat As String = Me.GetGlobalResourceObject("Text", "SDServiceFeeFormat")
            Dim strJoinedSchemeCode As String = dr("joined_scheme").ToString

            For i As Integer = 1 To 10
                Dim lblServiceFee As Label = e.Row.FindControl(String.Format("lblServiceFee{0:00}", i))

                ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                If lblServiceFee.Text = String.Empty Then
                    lblServiceFee.Text = Me.GetGlobalResourceObject("Text", "SDServiceFee_NoJoinScheme")

                ElseIf lblServiceFee.Text = "{NA}" Then
                    lblServiceFee.Text = Me.GetGlobalResourceObject("Text", "SDServiceFeeNA")
                    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

                ElseIf lblServiceFee.Text = "{TBP}" Then
                    lblServiceFee.Text = Me.GetGlobalResourceObject("Text", "SDServiceFeeTBP")

                ElseIf IsNumeric(lblServiceFee.Text) Then
                    ' --- CRE19-001-03 (Service Directory LAIV) [Start] (Lawrence) ---
                    If CInt(lblServiceFee.Text) = 0 Then
                        lblServiceFee.Text = Me.GetGlobalResourceObject("Text", "Free")
                    Else
                        lblServiceFee.Text = String.Format(strSDServiceFeeFormat, CInt(lblServiceFee.Text))
                    End If
                    ' --- CRE19-001-03 (Service Directory LAIV) [End] (Lawrence) ---

                End If

            Next
        End If

    End Sub

    Private Sub gvResult_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.DataBound

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        'Professional => gvResult.Columns(2)
        'Show/Hide Professional
        Dim dt As DataTable
        If TypeOf gvResult.DataSource Is DataView Then
            Dim dv As DataView = gvResult.DataSource
            dt = dv.Table
        Else
            dt = gvResult.DataSource
        End If

        Dim intDummyCol As Integer = 5

      
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        ' [ ILLUSTRATION ]
        '
        '                         +------------------------------------------+
        ' First Level  ------>    |                Service Fee               |
        '                         +----------------+-------------------------+
        ' Second Level ------>    | Pregnant Women | Children |    Elders    | 
        '                         +----------------+-------------------------+
        ' Third Level  ----->     |      SIV       |    SIV   | 23vPPV | SIV |
        '                         +----------------+----------+--------+-----+

        ' --- Handle Third Level ---

        ' Show column based on the selected service in searching
        Dim dicColumnMapping As New SortedDictionary(Of Integer, String)

        ' INT16-0010 Fix concurrent search problem [Start][Winnie]
        Dim lstSelectedService As List(Of String) = ViewState(VS_SelectedServiceList)
        ' INT16-0010 Fix concurrent search problem [End][Winnie]
        ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]
        Dim lstResultDisplayService As List(Of String) = ViewState(VS_ResultDisplayServiceList)
        ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]

        Dim dtSDSubsidizeGroup As DataTable = (New ServiceDirectoryBLL).GetSDSubsidizeGroupDT


        ' CRE17-018-02 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 2 - SDIR [Start][Koala]
        For intNumber As Integer = 1 To 10
            gvResult.Columns(intDummyCol + intNumber - 1).Visible = False
        Next

        Dim intServiceFeeCount As Integer = 0
        For Each strService As String In lstResultDisplayService
            'For Each strService As String In lstSelectedService
            For Each dr As DataRow In dtSDSubsidizeGroup.Select(String.Format("Search_Group = '{0}'", strService))
                ' Skip the item with no fee
                If IsDBNull(dr("Subsidize_Fee_Column_Name")) Then Continue For

                Dim intNumber As Integer = CInt(dr("Subsidize_Fee_Column_Name").ToString.Replace("subsidize_fee_", String.Empty))
                gvResult.Columns(intDummyCol + intNumber - 1).Visible = True

                ' Change the header text to Subsidize Short Form
                If Session("language") = TradChinese Then
                    gvResult.Columns(intDummyCol + intNumber - 1).HeaderText = dr("Subsidize_Short_Form_Chi")
                Else
                    gvResult.Columns(intDummyCol + intNumber - 1).HeaderText = dr("Subsidize_Short_Form")
                End If

                intServiceFeeCount += 1
            Next
        Next

        ' Enlarge Service provider name column if only show 23vPPV and PCV13 service fee only
        If intServiceFeeCount <= 2 Then
            'gvResult.Columns(3).ItemStyle.Width = New Unit(100, UnitType.Percentage)
            gvResult.Width = New Unit(980, UnitType.Pixel)
            gvResult.Columns(3).ItemStyle.Width = Nothing
        Else
            gvResult.Width = Nothing
            gvResult.Columns(3).ItemStyle.Width = New Unit(230, UnitType.Pixel)
        End If

        If dt.Columns.Contains("HealthCarePro") Then
            gvResult.Columns(2).Visible = True
        Else
            gvResult.Columns(2).Visible = False
            intDummyCol = 4
        End If
        ' CRE17-018-02 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 2 - SDIR [End][Koala]

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        'Dim blnInvolveServiceFee As Boolean = False
        'If Not IsNothing(lstSelectedService) Then
        '    For Each strSubsidize As String In lstSelectedService
        '        If Not IsDBNull(dtSDSubsidizeGroup.Select(String.Format("Search_Group = '{0}'", strSubsidize))(0)("Subsidize_Fee_Column_Name")) Then
        '            blnInvolveServiceFee = True
        '            Exit For
        '        End If
        '    Next
        'End If

        ' INT18-0010 (Fix SDIR to hide SIV) [Start][Koala CHENG]

        'If blnInvolveServiceFee Then
        For Each strService As String In lstResultDisplayService
            'For Each strService As String In lstSelectedService
            For Each dr As DataRow In dtSDSubsidizeGroup.Select(String.Format("Search_Group = '{0}'", strService))
                ' Skip the item with no fee
                If IsDBNull(dr("Subsidize_Fee_Column_Name")) Then Continue For

                Dim intNumber As Integer = CInt(dr("Subsidize_Fee_Column_Name").ToString.Replace("subsidize_fee_", String.Empty))
                dicColumnMapping.Add(intNumber, String.Format("{0},{1}", dr("Scheme_Code"), dr("Subsidize_Code")))

            Next
        Next
        'Else
        '    For Each dr As DataRow In dtSDSubsidizeGroup.Rows
        '        ' Skip the item with no fee
        '        If IsDBNull(dr("Subsidize_Fee_Column_Name")) Then Continue For

        '        Dim intNumber As Integer = CInt(dr("Subsidize_Fee_Column_Name").ToString.Replace("subsidize_fee_", String.Empty))
        '        dicColumnMapping.Add(intNumber, String.Format("{0},{1}", dr("Scheme_Code"), dr("Subsidize_Code")))

        '    Next
        'End If
        ' INT18-0010 (Fix SDIR to hide SIV) [End][Koala CHENG]
        'End If

        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        ' --- Handle Second Level ---

        ' Add category
        gvResult.BorderStyle = BorderStyle.None
        gvResult.BorderWidth = 0

        Dim gvHeaderRow As GridViewRow
        gvHeaderRow = New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)

        Dim strFilterScheme As String
        strFilterScheme = String.Empty

        ' Insert dummy invisible header cells at the left
        Dim tc1 As TableCell
        tc1 = New TableCell
        tc1.Text = String.Empty
        tc1.ColumnSpan = intDummyCol
        tc1.CssClass = "gvHeaderTransparentCell"
        gvHeaderRow.Cells.Add(tc1)

        ' Insert the visible cells afterwards
        Dim dtCategory As DataTable = (New ClaimCategoryBLL).GetSubsidizeGroupCategoryCache

        Dim dtColumnToAdd As New DataTable
        dtColumnToAdd.Columns.Add("Category_Name", GetType(String))
        dtColumnToAdd.Columns.Add("Column_Span", GetType(Integer))

        Dim index As Integer = -1
        Dim strPreviousCategory As String = String.Empty

        For Each kvp As KeyValuePair(Of Integer, String) In dicColumnMapping
            Dim drCategory As DataRow = dtCategory.Select(String.Format("Scheme_Code = '{0}' AND Subsidize_Code = '{1}'", _
                                                                        kvp.Value.Split(",".ToCharArray)(0), kvp.Value.Split(",".ToCharArray)(1)))(0)
            Dim strCategory As String = String.Empty

            If Session("language") = TradChinese Then
                strCategory = drCategory("SD_Category_Name_Chi")
            Else
                strCategory = drCategory("SD_Category_Name")
            End If

            If strCategory = strPreviousCategory Then
                dtColumnToAdd.Rows(index)("Column_Span") += 1
                Continue For

            Else
                Dim dr As DataRow = dtColumnToAdd.NewRow
                dr("Category_Name") = strCategory
                dr("Column_Span") = 1
                dtColumnToAdd.Rows.Add(dr)

                index += 1

            End If

            strPreviousCategory = strCategory
        Next

        For Each dr As DataRow In dtColumnToAdd.Rows
            tc1 = New TableCell
            'tc1.ID = strFilterScheme
            tc1.Text = dr("Category_Name")
            tc1.ColumnSpan = dr("Column_Span")
            tc1.BorderWidth = 1
            tc1.CssClass = "gvHeaderRow"
            gvHeaderRow.Cells.Add(tc1)

        Next

        gvResult.Controls(0).Controls.AddAt(0, gvHeaderRow)

        gvHeaderRow = New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)

        ' --- Handle First Level ---

        ' Insert dummy invisible header cells at the left
        tc1 = New TableCell
        tc1.Text = String.Empty
        tc1.ColumnSpan = intDummyCol
        tc1.CssClass = "gvHeaderTransparentCell"

        gvHeaderRow.Cells.Add(tc1)

        ' Add the top "Service Fee" header
        tc1 = New TableCell
        tc1.ID = strFilterScheme
        tc1.Text = Me.GetGlobalResourceObject("Text", "headerMaster")
        tc1.ColumnSpan = dicColumnMapping.Count
        tc1.BorderWidth = 1
        tc1.CssClass = "gvHeaderRow"
        gvHeaderRow.Cells.Add(tc1)

        gvResult.Controls(0).Controls.AddAt(0, gvHeaderRow)


        For i As Integer = 0 To Me.gvResult.HeaderRow.Cells.Count - 1
            Me.gvResult.HeaderRow.Cells(i).BorderWidth = 1
            Me.gvResult.HeaderRow.Cells(i).CssClass = "gvHeaderRow"
        Next

        For r As Integer = 0 To Me.gvResult.Rows.Count - 1
            For i As Integer = 0 To Me.gvResult.Rows(r).Cells.Count - 1
                Me.gvResult.Rows(r).Cells(i).BorderWidth = 1
                Me.gvResult.Rows(r).Cells(i).CssClass = "gvDataRow"
            Next
        Next

    End Sub

    ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        GridViewDataBind(Me.gvRemarks, Session(SESS_SearchRemarkDataTable))
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultDataTable)
    End Sub

    Private Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        GridViewDataBind(Me.gvRemarks, Session(SESS_SearchRemarkDataTable))
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultDataTable)
    End Sub

    Private Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        If Not TreeViewService.SelectedNode Is Nothing Then TreeViewService.SelectedNode.Selected = False
        GridViewDataBind(Me.gvRemarks, Session(SESS_SearchRemarkDataTable))
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchResultDataTable)
    End Sub
    ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

    Private Sub gvResultWithoutFee_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResultWithoutFee.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            ' Service Provider / Practice Name / District / Address
            Dim lblSPname_noFee As Label = e.Row.FindControl("lblSPname_noFee")
            Dim lblPracticeName_noFee As Label = e.Row.FindControl("lblPracticeName_noFee")
            Dim lblDistrictBoard_noFee As Label = e.Row.FindControl("lblDistrictBoard_noFee")
            Dim lblAddress_noFee As Label = e.Row.FindControl("lblAddress_noFee")
            ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
            Dim lblHealthCareProfession_noFee As Label = CType(e.Row.FindControl("lblHealthCarePro_noFee"), Label)
            ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

            Select Case Session("language")
                Case TradChinese
                    lblSPname_noFee.Text = CStr(dr("sp_chi_name")).Trim
                    lblSPname_noFee.CssClass = "tableTextChi"
                    lblPracticeName_noFee.Text = CStr(dr("practice_name_chi")).Trim
                    lblPracticeName_noFee.CssClass = "tableTextChi"
                    lblDistrictBoard_noFee.Text = CStr(dr("district_board_chi")).Trim
                    lblAddress_noFee.Text = CStr(dr("address_chi")).Trim
                    lblAddress_noFee.CssClass = "tableTextChi"
                    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                    If dr.Table.Columns.Contains("HealthCarePro_chi") Then
                        lblHealthCareProfession_noFee.Text = CStr(dr("HealthCarePro_chi")).Trim
                    End If
                    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

                Case Else
                    lblSPname_noFee.Text = CStr(dr("sp_name")).Trim
                    lblPracticeName_noFee.Text = CStr(dr("practice_name")).Trim
                    lblDistrictBoard_noFee.Text = CStr(dr("district_board_shortname_SD")).Trim
                    lblAddress_noFee.Text = CStr(dr("address_eng")).Trim
                    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
                    If dr.Table.Columns.Contains("HealthCarePro_chi") Then
                        lblHealthCareProfession_noFee.Text = CStr(dr("HealthCarePro")).Trim
                    End If
                    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

            End Select

            ' Enrolled Scheme Logo
            Dim intLogo As Integer = 1
            Dim strSDSchemeLogoPath As String = Me.GetGlobalResourceObject("ImageUrl", "SDSchemeLogoPath")

            For Each strSchemeCode As String In dr("joined_scheme").ToString.Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                Dim imgLogo As Image = e.Row.FindControl(String.Format("imgJoinedScheme{0}", intLogo.ToString.PadLeft(2, "0")))
                imgLogo.ImageUrl = strSDSchemeLogoPath.Replace("{SchemeCode}", strSchemeCode)
                imgLogo.Visible = True

                intLogo += 1
            Next
        End If

    End Sub

    Private Sub gvResultWithoutFee_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResultWithoutFee.DataBound
        ' Set column width by retrieving value from system resource

        ' Service Provider
        gvResultWithoutFee.Columns(1).ItemStyle.Width = Unit.Pixel(CInt(Me.GetGlobalResourceObject("Text", "SDIR_colWidth_SPName_view02")))

        ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
        'Professional => gvResultWithoutFee.Columns(2)
        'Show/Hide Professional
        Dim dt As DataTable
        If TypeOf gvResultWithoutFee.DataSource Is DataView Then
            Dim dv As DataView = gvResultWithoutFee.DataSource
            dt = dv.Table
        Else
            dt = gvResultWithoutFee.DataSource
        End If

        If dt.Columns.Contains("HealthCarePro") Then
            gvResultWithoutFee.Columns(2).Visible = True
        Else
            gvResultWithoutFee.Columns(2).Visible = False
        End If
        ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

        ' Practice Name
        gvResultWithoutFee.Columns(3).ItemStyle.Width = Unit.Pixel(CInt(Me.GetGlobalResourceObject("Text", "SDIR_colWidth_PracticeName_view02")))

        ' Distrct
        gvResultWithoutFee.Columns(4).ItemStyle.Width = Unit.Pixel(CInt(Me.GetGlobalResourceObject("Text", "SDIR_colWidth_District_view02")))

        ' Telephone Number
        gvResultWithoutFee.Columns(6).ItemStyle.Width = Unit.Pixel(CInt(Me.GetGlobalResourceObject("Text", "SDIR_colWidth_tel_view02")))

        ' Enrolled Scheme
        gvResultWithoutFee.Columns(7).ItemStyle.Width = Unit.Pixel(CInt(Me.GetGlobalResourceObject("Text", "SDIR_colWidth_EnrolledScheme_view02")))

        For i As Integer = 0 To Me.gvResultWithoutFee.HeaderRow.Cells.Count - 1
            Me.gvResultWithoutFee.HeaderRow.Cells(i).BorderWidth = 1
            Me.gvResultWithoutFee.HeaderRow.Cells(i).CssClass = "gvHeaderRow"
        Next

        For r As Integer = 0 To Me.gvResultWithoutFee.Rows.Count - 1
            For i As Integer = 0 To Me.gvResultWithoutFee.Rows(r).Cells.Count - 1
                Me.gvResultWithoutFee.Rows(r).Cells(i).BorderWidth = 1
                Me.gvResultWithoutFee.Rows(r).Cells(i).CssClass = "gvDataRow"
            Next
        Next

    End Sub

    ' CRE17-009 Add SDIR HCVS Note [Start] [Dickson]
    Private Sub gvResultWithoutFee_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResultWithoutFee.Sorting
        GridViewDataBind(Me.gvRemarks_noFee, Session(SESS_SearchRemarkDataTable))
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultDataTable)
    End Sub

    Private Sub gvResultWithoutFee_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResultWithoutFee.PageIndexChanging
        GridViewDataBind(Me.gvRemarks_noFee, Session(SESS_SearchRemarkDataTable))
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultDataTable)
    End Sub

    Private Sub gvResultWithoutFee_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResultWithoutFee.PreRender
        If Not TreeViewService.SelectedNode Is Nothing Then TreeViewService.SelectedNode.Selected = False
        GridViewDataBind(Me.gvRemarks_noFee, Session(SESS_SearchRemarkDataTable))
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchResultDataTable)
    End Sub

    ' CRE17-009 Add SDIR HCVS Note [End] [Dickson]

    Private Sub ddlResultPerPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlResultPerPage.SelectedIndexChanged
        Dim pagesize As Integer
        pagesize = CInt(ddlResultPerPage.SelectedValue)
        'Session(SESS_pageSize) = pagesize
        Me.gvResult.PageSize = pagesize
        If Me.mvResult.Views(0).Visible = True Then
            Me.GridViewDataBind(Me.gvResult, Session(SESS_SearchResultDataTable))
            ' ElseIf Me.mvResult.Views(1).Visible = True Then
            '  Me.GridViewDataBind(Me.gvResultWithoutFee, Session("dtResultDisplay_noFee"))
        End If

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode)
        udtAuditLogEntry.AddDescripton("Pagesize", CStr(pagesize))
        udtAuditLogEntry.WriteLog(LogID.LOG00017, "Change Result-per-page")

    End Sub

    Private Sub ddlResultPerPage_noFee_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlResultPerPage_noFee.SelectedIndexChanged
        Dim pagesize As Integer
        pagesize = CInt(ddlResultPerPage_noFee.SelectedValue)
        'Session(SESS_pageSize_noFee) = pagesize
        Me.gvResultWithoutFee.PageSize = pagesize
        'If Me.mvResult.Views(0).Visible = True Then
        'Me.GridViewDataBind(Me.gvResult, Session("dtResultDisplay"))
        If Me.mvResult.Views(1).Visible = True Then
            Me.GridViewDataBind(Me.gvResultWithoutFee, Session(SESS_SearchResultDataTable))
        End If

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Pagesize", CStr(pagesize))
        udtAuditLogEntry.WriteLog(LogID.LOG00018, "Change Result-per-page")

    End Sub

    Protected Sub imgShowHideSearchCriteria_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim selectedLang As String
        Dim strShowHideSearchCriteria As String
        selectedLang = LCase(Session("language"))
        strShowHideSearchCriteria = String.Empty
        strShowHideSearchCriteria = Me.GetGlobalResourceObject("Text", "ShowHideSearchCriteria")

        If imgShowHideSearchCriteria.ImageUrl = "~/Images/others/collapse.png" Then
            imgShowHideSearchCriteria.ImageUrl = "~/Images/others/expand.png"
            TblFilterCriteria.Style.Add("display", "none")
            strShowHideSearchCriteria = Left(strShowHideSearchCriteria, strShowHideSearchCriteria.IndexOf(";"))
            If Me.mvResult.Visible = True Then
                If Me.mvResult.ActiveViewIndex = 0 Then
                    tblRowBlank_withFee.Style.Add("display", "none")
                    tblRowBlank_withFee1.Style.Add("display", "none")
                    lnkBtnTop1.Style.Add("display", "none")
                Else
                    tblRowBlank_noFee.Style.Add("display", "none")
                    tblRowBlank_noFee1.Style.Add("display", "none")
                    lnkBtnTop1_noFee.Style.Add("display", "none")
                End If
            End If
        Else
            imgShowHideSearchCriteria.ImageUrl = "~/Images/others/collapse.png"
            TblFilterCriteria.Style.Add("display", "")
            strShowHideSearchCriteria = Right(strShowHideSearchCriteria, Len(strShowHideSearchCriteria) - strShowHideSearchCriteria.IndexOf(";") - 1)
            If Me.mvResult.Visible = True Then
                If Me.mvResult.ActiveViewIndex = 0 Then
                    tblRowBlank_withFee.Style.Add("display", "")
                    tblRowBlank_withFee1.Style.Add("display", "")
                    lnkBtnTop1.Style.Add("display", "")
                Else
                    tblRowBlank_noFee.Style.Add("display", "")
                    tblRowBlank_noFee1.Style.Add("display", "")
                    lnkBtnTop1_noFee.Style.Add("display", "")
                End If
            End If
        End If
        lblShowHideSearchCriteria.Text = strShowHideSearchCriteria
        imgShowHideSearchCriteria.AlternateText = strShowHideSearchCriteria

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim strAction As String
        strAction = imgShowHideSearchCriteria.ImageUrl
        strAction = (strAction.Replace("~/Images/others/", "")).Replace(".png", "")
        If strAction = "collapse" Then
            strAction = "expand"
        Else
            strAction = "collapse"
        End If
        udtAuditLogEntry.AddDescripton("Action", strAction)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, "Show Hide Search Criteria", "")
        If Not TreeViewService.SelectedNode Is Nothing Then TreeViewService.SelectedNode.Selected = False

    End Sub

    Private Sub changeLblShowHideSearchCriteria()
        Dim selectedLang As String
        Dim strShowHideSearchCriteria As String
        selectedLang = LCase(Session("language"))
        strShowHideSearchCriteria = String.Empty
        strShowHideSearchCriteria = Me.GetGlobalResourceObject("Text", "ShowHideSearchCriteria")

        If Len(imgShowHideSearchCriteria.Style.Item("display")) > 0 Then
            strShowHideSearchCriteria = ""
        Else
            If imgShowHideSearchCriteria.ImageUrl = "~/Images/others/collapse.png" Then
                strShowHideSearchCriteria = Right(strShowHideSearchCriteria, Len(strShowHideSearchCriteria) - strShowHideSearchCriteria.IndexOf(";") - 1)
            Else
                strShowHideSearchCriteria = Left(strShowHideSearchCriteria, strShowHideSearchCriteria.IndexOf(";"))
            End If
            imgShowHideSearchCriteria.AlternateText = strShowHideSearchCriteria
        End If
        lblShowHideSearchCriteria.Text = strShowHideSearchCriteria
        imgShowHideSearchCriteria.AlternateText = strShowHideSearchCriteria
    End Sub

    Private Sub TreeViewService_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeViewService.SelectedNodeChanged

        'INT16-0003 (Fix subsidy checkbox indent in SDIR) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If TreeViewService.SelectedNode.ShowCheckBox Then
        If TreeViewService.SelectedNode.ShowCheckBox AndAlso TreeViewService.SelectedNode.ImageUrl = String.Empty Then
            'INT16-0003 (Fix subsidy checkbox indent in SDIR) [End][Chris YIM]

            TreeViewService.SelectedNode.Checked = Not TreeViewService.SelectedNode.Checked

            Dim selectedNode As TreeNode = TreeViewService.SelectedNode
            selectedNode.Parent.Selected = True
        End If

    End Sub

    ' INT16-0010 Fix concurrent search problem [Start][Winnie]
    '' Handle Page Refresh
    'Private Sub HandlePageRefreshed()
    '    Response.Redirect("main.aspx")
    'End Sub
    ' INT16-0010 Fix concurrent search problem [End][Winnie]

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
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
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


    ' --- CRE17-005 (Enhance EHCP list with search function) [Start] (Marco) ---
    Private Sub ResetProfessionalControl()
        Me.rboProfessional.SelectedValue = Nothing

        ViewState.Remove(VS_SelectedProfessional)
    End Sub

    Private Sub ResetDistrictControl()
        Dim i As Integer
        Dim cb_area As System.Web.UI.WebControls.CheckBox
        Dim cbl_area As System.Web.UI.WebControls.CheckBoxList
        Dim cbl_area_item As System.Web.UI.WebControls.ListItem

        For i = 0 To 2
            cb_area = CType(Me.FindControl("cb_area_" + CStr(i + 1)), System.Web.UI.WebControls.CheckBox)
            cb_area.Checked = False

            cbl_area = CType(Me.FindControl("cbl_area_" + CStr(i + 1)), System.Web.UI.WebControls.CheckBoxList)
            For Each cbl_area_item In cbl_area.Items
                cbl_area_item.Selected = False
            Next
        Next

        ViewState.Remove(VS_SelectedDistrictList)
    End Sub

    Private Sub ResetServiceControl()
        Dim parentNode, childNode As Integer

        For parentNode = 0 To Me.TreeViewService.Nodes.Count - 1
            For childNode = 0 To Me.TreeViewService.Nodes(parentNode).ChildNodes.Count - 1
                Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).ShowCheckBox = True
                Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).Checked = False
                Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).Selected = False
                Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).ImageUrl = Nothing
            Next
        Next

        ViewState.Remove(VS_SelectedService)
        ViewState.Remove(VS_SelectedServiceList)

        If Not String.IsNullOrEmpty(rboProfessional.SelectedValue) Then
            RenderEligibleService()
        End If
    End Sub

    Private Sub ResetKeywordsSearchControl()
        txtServiceProvider.Text = String.Empty
        txtPracticeName.Text = String.Empty
        txtPracticeAddr.Text = String.Empty

        ViewState.Remove(VS_KwdSearchServiceProviderName)
        ViewState.Remove(VS_KwdSearchPracticeName)
        ViewState.Remove(VS_KwdSearchPracticeAddr)
    End Sub

    Protected Sub btnDistrictClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkbtnDistrictClear.Click
        ResetDistrictControl()
    End Sub

    Protected Sub btnHealthCareProClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkbtnHealthCareProClear.Click
        ResetProfessionalControl()

        Dim parentNode, childNode As Integer
        For parentNode = 0 To Me.TreeViewService.Nodes.Count - 1
            For childNode = 0 To Me.TreeViewService.Nodes(parentNode).ChildNodes.Count - 1
                Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).ShowCheckBox = True

                If Not Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).Checked Then
                    Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).Checked = False
                    Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).Selected = False
                End If

                Me.TreeViewService.Nodes(parentNode).ChildNodes(childNode).ImageUrl = Nothing
            Next
        Next
    End Sub

    Protected Sub btnServiceClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkbtnServiceClear.Click
        ResetServiceControl()
    End Sub

    Protected Sub btnKeywordsSearchClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkbtnKeywordsSearchClear.Click
        ResetKeywordsSearchControl()
    End Sub
    ' --- CRE17-005 (Enhance EHCP list with search function) [End] (Marco) ---

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Protected Sub ibtnReminderWindowsVersion_OK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00022, "Reminder - Obsolete Windows Version - OK Click")

        Me.ModalPopupExtenderReminderWindowsVersion.Hide()
    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

End Class
