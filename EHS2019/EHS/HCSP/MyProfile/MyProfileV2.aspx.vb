Imports System.Web.Security.AntiXss
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.DataAccess
Imports Common.Component.DataEntryUser
Imports Common.Component.UserAC
Imports Common.ComObject
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.StaticData
Imports Common.Component.VoucherScheme
Imports Common.Component
Imports Common.PCD
Imports Common.Component.Scheme
Imports HCSP.BLL
Imports Common.Component.Token
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports CustomControls
Imports Common.ComFunction
Imports Common.ComFunction.AccountSecurity
Imports Common.PCD.WebService.Interface

Partial Public Class MyProfileV2
    Inherits BasePageWithGridView

#Region "Audit Log Description"
    Public Class AublitLogDescription
        Public Const MyProfileloaded As String = "MyProfile loaded"
        Public Const CheckUsernameAvailability As String = "Check Username Availability"
        Public Const UsernameIsAvailable As String = "Username Is Available "
        Public Const UsernameIsNotAvailable As String = "Username Is Not Available"

        Public Const SPUpdateProfile As String = "SP Update Profile"
        Public Const SPUpdateProfileSuccess As String = "SP Update Profile success"
        Public Const SPUpdateProfileFail As String = "SP Update Profile fail"
        Public Const SPUpdateProfileEdit As String = "SP Update Profile Edit"
        Public Const SPUpdateProfileCancel As String = "SP Update Profile Cancel"

        Public Const DataEntryUpdateAccount As String = "Data Entry Update Account"
        Public Const DataEntryUpdateAccountSuccess As String = "Data Entry Update Account Success"
        Public Const DataEntryUpdateAccountFail As String = "Data Entry Update Account fail"


        Public Const CreateDataEntryAccount As String = "Create Data Entry Account"
        Public Const CreateDataEntryAccountSuccess As String = "Create Data Entry Account Success"
        Public Const CreateDataEntryAccountFail As String = "Create Data Entry Account Fail"

        Public Const SPUpdateDataEntryAccount As String = "SP Update Data Entry Account"
        Public Const SPUpdateDataEntryAccountSuccess As String = "SP Update Data Entry Account Success"
        Public Const SPUpdateDataEntryAccountFail As String = "SP Update Data Entry Account Fail"

        ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
        Public Const GetEHRSSUsernameClick As String = "Get Username From eHRSS Click"
        Public Const GetEHRSSConsentConfirmClick As String = "Get Username From eHRSS Consent - Confirm Click"
        Public Const GetEHRSSConsentCancelClick As String = "Get Username From eHRSS Consent - Cancel Click"
        Public Const GetEHRSSUsername As String = "Get eHRSS Username"
        Public Const GetEHRSSUsernameSuccess As String = "Get eHRSS Username Success"
        Public Const GetEHRSSUsernameFail As String = "Get eHRSS Username eHRSS Fail"
        ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]


        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Public Const DataEntryAccountLoadInfo As String = "Load Data Entry Account information"
        Public Const DataEntryAccountAddClick As String = "SP create Data Entry Account Add click"
        Public Const DataEntryAccountAddCancelClick As String = "SP create Data Entry Account Cancel click"
        Public Const DataEntryAccountEditClick As String = "SP edit Data Entry Account Edit click"
        Public Const DataEntryAccountEditCancelClick As String = "SP edit Data Entry Account Cancel click"
        Public Const DataEntryUpdateEdit As String = "DataEntry Update Edit Click"
        Public Const DataEntryUpdateCancel As String = "DataEntry Update Cancel Click"
        'Public Const LoadDataEntryAccountInfo As String = "Create Data Entry Account Save click"
        'Public Const LoadDataEntryAccountInfo As String = "Edit Data Entry Account Save click"
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]



    End Class
#End Region

    Dim udtSP As ServiceProviderModel = New ServiceProviderModel
    Dim udtDataEntry As DataEntryUserModel = New DataEntryUserModel
    Dim udtUserAC As UserACModel = New UserACModel
    Dim udtformatter As Common.Format.Formatter = New Common.Format.Formatter
    Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
    Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
    Dim udtConsentFormPrintOptionBLL As New ConsentFormPrintOptionBLL()
    Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    Dim udtDataEntryAcctBLL As DataEntryAcctBLL = New DataEntryAcctBLL
    Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
    Dim udtDataEntryUserBLL As DataEntryUserBLL = New DataEntryUserBLL

    Private udtDB As Database = New Database



#Region "Constant Value"
    Dim strFuncCode As String = Common.Component.FunctCode.FUNT020601
    Dim strCommFuncCode As String = Common.Component.FunctCode.FUNT990000
    'Session Name
    Public Const DataEntryAcct As String = "DataEntryAcct"
    Public Const IncludePrintingEnabledScheme As String = "IncludePrintingEnabledScheme"
    Public Const IncludeIVRSEnabledScheme As String = "IncludeIVRSEnabledScheme"

    Public Class PrintOption
        Public Const PrintOption_FullVersion As String = "FULL"
        Public Const PrintOption_CondensedVersion As String = "CONDENSE"
        Public Const PrintOption_BothVersion As String = "BOTH"
    End Class
#End Region

#Region "Session Contants"

    Public Const SESS_MyProfilePracticeSchemeInfoList As String = "SESS_MyProfilePracticeSchemeInfoList"

#End Region

#Region "Page Event"
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If IsPostBack Then

            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)

            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectSimpChinese) OrElse controlID.Equals(SelectEnglish) Then

                'Me.RerenderLanguage()
                udtSP = Me.udtServiceProviderBLL.GetSP()
                ''Practice Tab
                Session("MyProfilePracticeSchemeGridFormatted") = "N"
                gvPracticeInfo.DataSource = udtSP.PracticeList.Values
                gvPracticeInfo.DataBind()


                ' Save the Checked Item and check it after

                Dim i As Integer
                Dim lstStrPracticeValue As New List(Of String)

                For i = 0 To Me.chkPracticeList.Items.Count - 1
                    If Me.chkPracticeList.Items(i).Selected Then
                        lstStrPracticeValue.Add(Me.chkPracticeList.Items(i).Value.Trim())
                    End If
                Next

                udtSP = Me.udtServiceProviderBLL.GetSP()
                Me.constructPracticeList(udtSP)

                For i = 0 To Me.chkPracticeList.Items.Count - 1
                    If lstStrPracticeValue.Contains(Me.chkPracticeList.Items(i).Value.Trim()) Then
                        Me.chkPracticeList.Items(i).Selected = True
                    End If
                Next

                ' Render Data Entry Account Maintenance tab Practice

                If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
                    chkPracticeList.CssClass = "tableTextChi"
                Else
                    chkPracticeList.CssClass = String.Empty
                End If

            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim udtsp As ServiceProviderModel
        Dim dt As DataTable = New DataTable
        Dim udtUserACBLL As New UserACBLL
        Dim udcGeneralFun As New Common.ComFunction.GeneralFunction
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim intPlatform As Integer = Me.SubPlatform()
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        ' Dim udtdb As Database = New Database

        FunctionCode = Common.Component.FunctCode.FUNT020601

        udtUserAC = UserACBLL.GetUserAC

        If Not IsPostBack Then

            If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                udtsp = CType(udtUserAC, ServiceProviderModel)

                udtsp = udtSPProfileBLL.loadSP(udtsp.SPID)

                udtServiceProviderBLL.SaveToSession(udtsp)

                Me.mvMyProfile.ActiveViewIndex = 0

            Else
                udtDataEntry = CType(udtUserAC, DataEntryUserModel)
                'load update data entry account
                udtDataEntry = udtDataEntryAcctBLL.LoadDataEntry(udtDataEntry.SPID, udtDataEntry.DataEntryAccount)
                Me.udtDataEntryUserBLL.SaveToSession(udtDataEntry)

                'Load Update SP account
                udtsp = udtSPProfileBLL.loadSP(udtDataEntry.SPID)
                udtServiceProviderBLL.SaveToSession(udtsp)

                Me.lblDEUsername.Text = udtDataEntry.DataEntryAccount
                Me.mvMyProfile.ActiveViewIndex = 1
            End If

            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)  ''Begin Writing Audit Log
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AublitLogDescription.MyProfileloaded)


            ''Data Entry Account
            chkChgWebPWD_CheckedChanged(Nothing, Nothing)
            Me.chkChgIVRSPwd_CheckedChanged(Nothing, Nothing)
            Dim dtDataEntryAcct As DataTable
            dtDataEntryAcct = udtDataEntryAcctBLL.getDataEntryAcct(udtsp.SPID)
            Session(DataEntryAcct) = dtDataEntryAcct
            Me.GridViewDataBind(Me.gvdataEntryAcc, dtDataEntryAcct)
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Overwrite the Base GridView
            Me.gvdataEntryAcc.PageSize = 20
            Me.gvdataEntryAcc.DataSource = dtDataEntryAcct
            Me.gvdataEntryAcc.DataBind()
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'Disable Search Function if no data entry acct
            If dtDataEntryAcct.Rows.Count = 0 Then
                Me.tbDEFilter.Visible = False
            End If
            'CRE13-019-02 Extend HCVS to China [End][Winnie]


            Me.txtDEloginID.Enabled = False
            Me.txtDEloginID.Text = String.Empty
            Me.txtDEloginID.Visible = True
            Me.lblDELoginID.Visible = False
            Me.chkPracticeList.Enabled = False
            Me.chkChgDEPWD.Enabled = False
            Me.chkDESuspend.Enabled = False

            'Me.ddlDEDefaultPrintOption.Enabled = False

            Me.btnAddDEAcct.Visible = True
            Me.chkChgDEPWD.Visible = False
            Me.btnEditDEAcct.Visible = False
            Me.btnSaveDEAcct.Visible = False
            Me.btnCancelDEAcct.Visible = False

            constructPracticeList(udtsp)

            'Init Print Option 

            Me.InitLoginUserPrintOptionList(True)

            ''System Info
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.tdUsernameTips.Style.Add("display", "none")
            'ChkExistenceOfIVRSPrintOptionSchemes(udtsp.SPID)
            Me.ChkExistenceOfIVRSPrintOptionSchemes(udtsp)

            If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                Me.EnablePrintOptionList(udtsp.SPID)
            Else
                Me.EnablePrintOptionList(udtsp.SPID, udtDataEntry.DataEntryAccount)
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


            'By Paul, modified on 22/9, second parameter is longer be used
            dt = udtSPProfileBLL.loadSPLoginProfile(udtsp.SPID, String.Empty)
            Me.txtUsername.Text = dt.Rows(0).Item("Alias_Account")
            Me.lblUsername.Text = dt.Rows(0).Item("Alias_Account")

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Me.lblUsername.Text.Equals(String.Empty) Then
                Me.lblUsername.Text = Me.GetGlobalResourceObject("Text", "--").ToString()
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

            ' Moved outside this if-then-else conditional clause 
            ' Because the token serial no. display would not be affected no matter the page is postback

            'If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
            '    If udtUserAC.TokenSerialNo.Trim.Equals(String.Empty) Then
            '        Me.lblTokenSerialNo.Text = dt.Rows(0).Item("Token_Serial_No")
            '    Else
            '        Me.lblTokenSerialNo.Text = udtUserAC.TokenSerialNo.Trim
            '    End If
            'End If

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

            If dt.Rows(0).Item("Default_Language").ToString.Trim.Equals(String.Empty) Then
                Me.ddlDefaultLang.SelectedValue = "E"
                Me.udtSP.DefaultLanguage = "E"
            Else
                Me.ddlDefaultLang.SelectedValue = dt.Rows(0).Item("Default_Language")
                Me.udtSP.DefaultLanguage = dt.Rows(0).Item("Default_Language")
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Select Case intPlatform
                Case EnumHCSPSubPlatform.NA
                    Me.PanelSPDefaultLang.Visible = True
                    Me.PanelIVRSPasswordSettings.Visible = True
                Case EnumHCSPSubPlatform.HK
                    Me.PanelSPDefaultLang.Visible = True
                    Me.PanelIVRSPasswordSettings.Visible = True
                Case EnumHCSPSubPlatform.CN
                    Me.PanelSPDefaultLang.Visible = False
                    Me.PanelIVRSPasswordSettings.Visible = False
            End Select

            If PanelPrintingOptionSetting.Visible = False And PanelSPDefaultLang.Visible = False Then
                PanelSPSystemSettings.Visible = False
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If Session(IncludeIVRSEnabledScheme) Then
                If dt.Rows(0).Item("SP_IVRS_Password").ToString.Trim.Equals(String.Empty) Then
                    Me.chkActivateIVRSPwd.Visible = True
                    Me.chkChgIVRSPwd.Visible = False
                    Me.txtIVRSOldPwd.Visible = False
                    Me.lblIVRSOldPwdText.Visible = False
                Else
                    Me.chkActivateIVRSPwd.Visible = False
                    Me.chkChgIVRSPwd.Visible = True
                    Me.txtIVRSOldPwd.Visible = True
                    Me.lblIVRSOldPwdText.Visible = True
                    Me.lblIVRSOldPwdText.Text = HttpContext.GetGlobalResourceObject("Text", "OldPassword")
                End If
            Else
                Me.chkActivateIVRSPwd.Enabled = False
                Me.chkActivateIVRSPwd.Visible = True
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Me.PanelIVRSPasswordSettings.Visible = False
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

            udtSPProfileBLL.ClearSession()
            udtSPProfileBLL.SaveToSession(dt.Rows(0).Item("TSMP"))


            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            'Me.btnCancel_Click(Nothing, Nothing)
            CancelClick_Clear()
            'Me.btnCancelDEAcct_Click(Nothing, Nothing)
            ResetControl(False)
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]


            'Init Data Entry change profile Control
            InitDataEntryProfileControl()
            ''Personal Info Tab
            lblSPID.Text = udtsp.SPID
            lblSPEname.Text = udtsp.EnglishName
            lblSPCname.Text = udtformatter.formatChineseName(udtsp.ChineseName)
            lblSPHKID.Text = udtformatter.formatHKID(udtsp.HKID, False)
            lblSPAddress.Text = udtformatter.formatAddress(udtsp.SpAddress.Room, udtsp.SpAddress.Floor, udtsp.SpAddress.Block, _
                                                            udtsp.SpAddress.Building, udtsp.SpAddress.District, udtsp.SpAddress.AreaCode)
            lblSPEmail.Text = udtsp.Email
            lblSPContactNo.Text = udtsp.Phone
            lblSPFax.Text = udtsp.Fax

            Session("MyProfilePracticeSchemeGridFormatted") = "N"

            gvPracticeInfo.DataSource = udtsp.PracticeList.Values
            gvPracticeInfo.DataBind()

            'Integration Start
            Me.ModalPopupExtenderTypeOfPractice.PopupDragHandleControlID = Me.ucTypeOfPracticePopup.Header.ClientID
            Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopUp.Header.ClientID
            Me.ModalPopupExtenderNoticeLogoutEHS.PopupDragHandleControlID = Me.ucNoticeLogoutEHSPopUp.Header.ClientID
            Me.ModalPopupExtenderToken.PopupDragHandleControlID = Me.ucInputTokenPopup.Header.ClientID
            Me.ModalPopupExtenderPCDEnrolled.PopupDragHandleControlID = Me.ucPCDEnrolledPopup.Header.ClientID
            'Integration End

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Me.udcSchemeLegend.ShowSeasonalDisplayCode = False
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
            Me.trGetUsernameFromEHRSS.Visible = False

            ' Check if able to Get Username from eHRSS
            Select Case intPlatform
                Case EnumHCSPSubPlatform.NA, EnumHCSPSubPlatform.HK
                    If udcGeneralFun.CheckEnableEHRSSinHCSP = GeneralFunction.EnumTurnOnStatus.Yes Then
                        Me.trGetUsernameFromEHRSS.Visible = True
                    End If
            End Select
            ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]
        Else
            udtsp = Me.udtServiceProviderBLL.GetSP()
            If udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
                udtDataEntry = Me.udtDataEntryUserBLL.GetDataEntry()
            End If

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

            dt = udtSPProfileBLL.loadSPLoginProfile(udtsp.SPID, String.Empty)

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

            'Integration Start
            If Me.ucTypeOfPracticePopup.Showing Then
                Me.ModalPopupExtenderTypeOfPractice.Show()
            End If
            'Integration End
        End If

        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
            If udtUserAC.TokenSerialNo.Trim.Equals(String.Empty) Then
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"))
                Select Case dt.Rows(0).Item("Project").Trim()
                    Case TokenProjectType.EHCVS
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"), False, False, False, LCase(Session("language")))

                    Case TokenProjectType.EHR
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"), True, True, False, LCase(Session("language")))

                    Case Else
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"), False, False, False, LCase(Session("language")))
                End Select
            Else
                'Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(udtUserAC.TokenSerialNo.Trim, dt.Rows(0).Item("Project"))
                Select Case dt.Rows(0).Item("Project").Trim()
                    Case TokenProjectType.EHCVS
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(udtUserAC.TokenSerialNo.Trim, dt.Rows(0).Item("Project"), False, False, False, LCase(Session("language")))

                    Case TokenProjectType.EHR
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(udtUserAC.TokenSerialNo.Trim, dt.Rows(0).Item("Project"), True, True, False, LCase(Session("language")))

                    Case Else
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(udtUserAC.TokenSerialNo.Trim, dt.Rows(0).Item("Project"), False, False, False, LCase(Session("language")))
                End Select
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            End If
        End If

        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]


        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Select Case intPlatform
            Case EnumHCSPSubPlatform.NA
                If IsPCDSP(udtsp) Then
                    Me.ibtnJoinPCD.Visible = True
                Else
                    Me.ibtnJoinPCD.Visible = False
                End If
            Case EnumHCSPSubPlatform.HK
                If IsPCDSP(udtsp) Then
                    Me.ibtnJoinPCD.Visible = True
                Else
                    Me.ibtnJoinPCD.Visible = False
                End If
            Case EnumHCSPSubPlatform.CN
                Me.ibtnJoinPCD.Visible = False
        End Select
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        RerenderLanguage()

        ''MO Tab
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'FilterMOList(udtsp.MOList, udtsp.PracticeList)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        gvMOInfo.DataSource = udtsp.MOList.Values
        gvMOInfo.DataBind()

        ''Bank Account Tab
        gvBankInfo.DataSource = udtsp.PracticeList.Values
        gvBankInfo.DataBind()

        gvSPEnrolledScheme.DataSource = udtsp.SchemeInfoList.Values
        gvSPEnrolledScheme.DataBind()

        Dim strvalue As String = String.Empty
        Dim strvalue2 As String = String.Empty

        udcGeneralFun.getSystemParameter("PasswordRuleNumber", strvalue, strvalue2)

        'Me.txtDENewPWD.Attributes.Add("onKeyUp", "checkPassword(this.value,'strength1','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrength") & "');")
        Me.txtDENewPWD.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue2.Trim) & "', '" & CInt(strvalue2.Trim) & "','strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
        Me.txtNewWebPwd.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue.Trim) & "', '" & CInt(strvalue2.Trim) & "','strength1a','strength2a','strength3a','progressBar2', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2a','direction1a');")
        Me.txtDENEWPassword.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue2.Trim) & "', '" & CInt(strvalue2.Trim) & "','strength1DE','strength2DE','strength3DE','progressBarDE', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2DE','direction1DE');")

        If Me.chkChgDEPWD.Checked Then
            Me.txtDENewPWD.BackColor = Drawing.Color.White
            Me.txtDEConfirmNewPWD.BackColor = Drawing.Color.White
        End If

        'Set tabIndex
        If Not IsNothing(txtTabIndex.Text) Then
            Dim index As Integer = 0
            If Integer.TryParse(txtTabIndex.Text, index) Then
                TabContainer1.ActiveTabIndex = index
            End If
        End If

    End Sub
#End Region

#Region "Tab - Personal Information"
    Protected Sub gvSPEnrolledScheme_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSPEnrolledScheme.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSPSchemeName As Label = CType(e.Row.FindControl("lblSPSchemeName"), Label)
            Dim lblSPRecordStatus As Label = CType(e.Row.FindControl("lblSPRecordStatus"), Label)
            Dim lblEffectiveTime As Label = CType(e.Row.FindControl("lblEffectiveTime"), Label)
            Dim lblDelistTime As Label = CType(e.Row.FindControl("lblDelistTime"), Label)

            ' Add tooltips to Scheme Name
            Dim strLang As String = Session("language")

            ' Get the MasterSchemeCollection and SubSchemeCollection first (to avoid over-accessing SQL)
            Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
            lblSPSchemeName.Text = GetMasterSchemeDesc(udtSchemeBackOfficeModelCollection, lblSPSchemeName.Text, strLang)

            Dim strStatus As String = String.Empty
            If Session("language") = CultureLanguage.TradChinese Then
                Status.GetDescriptionFromDBCode("MyProfileSchemeInfoDisplayStatus", lblSPRecordStatus.Text.Trim, String.Empty, strStatus)
            ElseIf Session("language") = CultureLanguage.SimpChinese Then
                Status.GetDescriptionFromDBCode("MyProfileSchemeInfoDisplayStatus", lblSPRecordStatus.Text.Trim, String.Empty, String.Empty, strStatus)
            Else
                Status.GetDescriptionFromDBCode("MyProfileSchemeInfoDisplayStatus", lblSPRecordStatus.Text.Trim, strStatus, String.Empty)
            End If
            lblSPRecordStatus.Text = strStatus

            If Not lblEffectiveTime.Text.Trim.ToString().Trim.Equals(String.Empty) Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL

                'lblEffectiveTime.Text = udtformatter.formatDate(lblEffectiveTime.Text, Session("language"))
                lblEffectiveTime.Text = udtformatter.formatDisplayDate(CDate(lblEffectiveTime.Text), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                lblEffectiveTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If Not lblDelistTime.Text.Trim.Equals(String.Empty) Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'lblDelistTime.Text = udtformatter.formatDate(lblDelistTime.Text, Session("language"))
                lblDelistTime.Text = udtformatter.formatDisplayDate(CDate(lblDelistTime.Text), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                lblDelistTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If
        End If
    End Sub
#End Region

#Region "Tab - Medical Organization"

    Private Sub gvMOInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMOInfo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As MedicalOrganization.MedicalOrganizationModel = CType(e.Row.DataItem, MedicalOrganization.MedicalOrganizationModel)
            Dim lblMOStatus As Label
            Dim strMOStatus As String = String.Empty

            lblMOStatus = CType(e.Row.FindControl("lblMOStatus"), Label)
            strMOStatus = dr.RecordStatus

            'Select Case strMOStatus
            '    Case "A"
            '        strMOStatus = Me.GetGlobalResourceObject("Text", "Active")
            '    Case Else
            '        strMOStatus = Me.GetGlobalResourceObject("Text", "Inactive")
            'End Select
            'lblMOStatus.Text = strMOStatus

            Dim strMOStatusDesc As String = String.Empty
            If Session("language") = CultureLanguage.TradChinese Then
                Status.GetDescriptionFromDBCode("MyProfileMedicalOrganizationStatus", strMOStatus, String.Empty, strMOStatusDesc)
            ElseIf Session("language") = CultureLanguage.SimpChinese Then
                Status.GetDescriptionFromDBCode("MyProfileMedicalOrganizationStatus", strMOStatus, String.Empty, String.Empty, strMOStatusDesc)
            Else
                Status.GetDescriptionFromDBCode("MyProfileMedicalOrganizationStatus", strMOStatus, strMOStatusDesc, String.Empty)
            End If
            lblMOStatus.Text = strMOStatusDesc

            'If strMOStatus <> MedicalOrganizationStatus.Active And (chkMOShowActiveOnly.Checked) Then
            '    e.Row.Visible = False
            'Else
            '    e.Row.Visible = True
            'End If
            If (chkMOShowActiveOnly.Checked) Then
                Select Case strMOStatus
                    Case PracticeStatus.Active
                        e.Row.Visible = True
                    Case Else
                        e.Row.Visible = False
                End Select
            Else
                e.Row.Visible = True
            End If
        End If
    End Sub

#End Region

#Region "Tab - Practice Information"

    Private Sub gvPracticeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPracticeInfo.RowDataBound
        udtSP = udtServiceProviderBLL.GetSP
        Dim objListItem As DataControlRowState = e.Row.RowState
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
            ' ------------------------------------------------------------------------
            Dim udtPractice As PracticeModel = DirectCast(e.Row.DataItem, PracticeModel)

            ' Mobile Clinic
            Dim lblPracticeMobileClinic As Label = CType(e.Row.FindControl("lblPracticeMobileClinic"), Label)
            If lblPracticeMobileClinic.Text = YesNo.Yes Then
                lblPracticeMobileClinic.Text = Me.GetGlobalResourceObject("Text", "Yes")
            Else
                lblPracticeMobileClinic.Text = Me.GetGlobalResourceObject("Text", "No")
            End If

            ' Practice Remarks
            Dim lblPracticeRemarks As Label = e.Row.FindControl("lblPracticeRemarks")
            Dim lblPracticeRemarksChi As Label = e.Row.FindControl("lblPracticeRemarksChi")
            Dim strRemarksDescEng As String = udtPractice.RemarksDesc
            Dim strRemarksDescChi As String = udtPractice.RemarksDescChi

            If strRemarksDescEng = String.Empty AndAlso strRemarksDescChi = String.Empty Then
                lblPracticeRemarks.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ElseIf strRemarksDescEng <> String.Empty Then
                lblPracticeRemarks.Text = strRemarksDescEng
                lblPracticeRemarksChi.Text = formatChineseString(strRemarksDescChi)

            Else
                lblPracticeRemarks.Text = strRemarksDescChi
            End If
            ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

            ' Scheme Information
            Dim lblPracticeDisplaySeq As Label = CType(e.Row.FindControl("lblPracticeDisplaySeq"), Label)
            Dim gvPracticeSchemeInfo As GridView = CType(e.Row.FindControl("gvPracticeSchemeInfo"), GridView)
            Dim hrRowSpan As HiddenField = CType(gvPracticeSchemeInfo.FindControl("hrRowSpan"), HiddenField)

            Session(SESS_MyProfilePracticeSchemeInfoList) = udtSP.PracticeList(CInt(lblPracticeDisplaySeq.Text)).PracticeSchemeInfoList

            gvPracticeSchemeInfo.DataSource = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache.ToSPProfileDataTable
            gvPracticeSchemeInfo.DataBind()


            'handle both scheme are not Active
            Dim lstMscheme As String = String.Empty
            Dim blnActivePracticeScheme As Boolean = False
            For Each udtPracticeScheme As PracticeSchemeInfo.PracticeSchemeInfoModel In udtSP.PracticeList(CInt(lblPracticeDisplaySeq.Text)).PracticeSchemeInfoList.Values
                If udtPracticeScheme.RecordStatus.Equals(PracticeSchemeInfoStatus.Active) Or udtPracticeScheme.RecordStatus.Equals(PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist) Or udtPracticeScheme.RecordStatus.Equals(PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend) Then
                    blnActivePracticeScheme = True
                    'Exit For 
                End If
            Next

            ' Hide the subsidy legend if in CN platform
            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                gvPracticeSchemeInfo.HeaderRow.FindControl("btnSubsidyHelp").Visible = False
            End If

            If (Me.chkShowActive.Checked Or Me.chkBankAcctShowActiveOnly.Checked) And (Not blnActivePracticeScheme) Then
                e.Row.Visible = False
            Else
                e.Row.Visible = True
            End If


        End If

    End Sub


#Region "Practice Scheme Information"
    Protected Sub gvPracticeSchemeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.DataRow
                Dim strSchemeCode As String = DirectCast(e.Row.FindControl("hfMScheme"), HiddenField).Value
                Dim strSubsidizeCode As String = DirectCast(e.Row.FindControl("hfScheme"), HiddenField).Value

                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Session(SESS_MyProfilePracticeSchemeInfoList)
                Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel = udtPracticeSchemeInfoList.Filter(strSchemeCode, strSubsidizeCode)

                e.Row.Visible = False

                'INT17-0014 (Fix sp profile display when no service provided) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtPracticeSchemeInfo Is Nothing Then
                    Return
                End If
                'INT17-0014 (Fix sp profile display when no service provided) [End][Chris YIM]

                ' Hide the row if not enrolled or not providing service
                If DirectCast(e.Row.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                        If udtPSINode.SchemeCode = strSchemeCode Then
                            udtPracticeSchemeInfo = udtPSINode
                            Exit For
                        End If
                    Next

                    If IsNothing(udtPracticeSchemeInfo) Then
                        Return
                    End If

                Else
                    If IsNothing(udtPracticeSchemeInfoList.Filter(strSchemeCode)) Then
                        Return
                    End If

                    ' Check all not provide service
                    Dim blnAllNotProvideService As Boolean = True

                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Filter(strSchemeCode).Values
                        If udtPSINode.ProvideService Then
                            blnAllNotProvideService = False
                            Exit For
                        End If
                    Next

                    If blnAllNotProvideService Then
                        DirectCast(e.Row.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y"

                    Else
                        If IsNothing(udtPracticeSchemeInfo) OrElse udtPracticeSchemeInfo.ProvideService = False Then
                            Return

                        End If

                    End If

                End If

                e.Row.Visible = True

                ' Controls
                Dim imgButton As ImageButton = CType(e.Row.FindControl("btnSchemeNameHelp"), ImageButton)

                If Not imgButton Is Nothing Then
                    Me.ClientScript.RegisterForEventValidation(imgButton.UniqueID)
                End If

                ' Show/Hide inactive items
                If (Me.chkShowActive.Checked Or Me.chkBankAcctShowActiveOnly.Checked) Then
                    If (Not udtPracticeSchemeInfo.RecordStatus.Equals(PracticeSchemeInfoStatus.Delisted)) And (Not udtPracticeSchemeInfo.RecordStatus.Equals(PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary)) And (Not udtPracticeSchemeInfo.RecordStatus.Equals(PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary)) Then
                        e.Row.Visible = True
                    Else
                        e.Row.Visible = False
                    End If
                Else
                    e.Row.Visible = True
                End If

                ' Scheme Code
                If udtPracticeSchemeInfo.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                    Dim lblMScheme As Label = e.Row.FindControl("lblMScheme")
                    lblMScheme.Text += String.Format("<br />({0})", Me.GetGlobalResourceObject("Text", "NonClinic"))
                End If

                ' Service Fee
                Dim udtSubsidizeGpBO As SubsidizeGroupBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup.Filter(strSchemeCode).SubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                Dim lblServiceFee As Label = CType(e.Row.FindControl("lblServiceFee"), Label)

                If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                    If udtPracticeSchemeInfo.ProvideServiceFee.Value AndAlso udtPracticeSchemeInfo.ServiceFee.HasValue Then
                        lblServiceFee.Text = udtformatter.formatMoney(udtPracticeSchemeInfo.ServiceFee, True)

                    Else
                        lblServiceFee.Text = udtSubsidizeGpBO.ServiceFeeCompulsoryWording

                    End If

                Else
                    If udtSubsidizeGpBO.ServiceFeeEnabled Then
                        lblServiceFee.Text = "--"
                    Else
                        lblServiceFee.Text = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
                    End If

                End If

                ' Status
                Dim lblStatus As Label = CType(e.Row.FindControl("lblStatus"), Label)

                If Session("language") = CultureLanguage.TradChinese Then
                    Status.GetDescriptionFromDBCode("MyProfilePracticeSchemeInfoDisplayStatus", udtPracticeSchemeInfo.RecordStatus, String.Empty, lblStatus.Text)
                ElseIf Session("language") = CultureLanguage.SimpChinese Then
                    Status.GetDescriptionFromDBCode("MyProfilePracticeSchemeInfoDisplayStatus", udtPracticeSchemeInfo.RecordStatus, String.Empty, String.Empty, lblStatus.Text)
                Else
                    Status.GetDescriptionFromDBCode("MyProfilePracticeSchemeInfoDisplayStatus", udtPracticeSchemeInfo.RecordStatus, lblStatus.Text, String.Empty)
                End If

                ' Effective Time
                Dim lblEffectiveTime As Label = e.Row.FindControl("lblEffectiveTime")
                Dim lblDelistTime As Label = e.Row.FindControl("lblDelistTime")
                Dim udtSubPlatformBLL As New SubPlatformBLL

                If udtPracticeSchemeInfo.EffectiveDtm.HasValue Then
                    ' Look for the earliest Effective Date within the scheme
                    Dim dtmTargetTime As DateTime = Date.Now

                    For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                        If udtPracticeScheme.SchemeCode = strSchemeCode Then
                            If udtPracticeScheme.EffectiveDtm.Value < dtmTargetTime Then
                                dtmTargetTime = udtPracticeScheme.EffectiveDtm.Value
                            End If
                        End If
                    Next

                    lblEffectiveTime.Text = udtformatter.formatDisplayDate(dtmTargetTime, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

                Else
                    lblEffectiveTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                ' Delisting Time
                If udtPracticeSchemeInfo.DelistDtm.HasValue Then
                    lblDelistTime.Text = udtformatter.formatDisplayDate(udtPracticeSchemeInfo.DelistDtm.Value, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                Else
                    lblDelistTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

            Case DataControlRowType.Header
                e.Row.Cells(1).ColumnSpan = 2
                e.Row.Cells(2).Visible = False

        End Select

    End Sub

    Protected Sub gvPracticeSchemeInfo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvPracticeSchemeInfo As GridView = sender

        ' Handle Category
        For Each gvr As GridViewRow In gvPracticeSchemeInfo.Rows
            If DirectCast(gvr.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
                ' Check whether this category is visible
                Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfMScheme"), HiddenField).Value
                Dim strCategoryName As String = DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value
                Dim blnVisible As Boolean = False

                For Each r As GridViewRow In gvPracticeSchemeInfo.Rows
                    If DirectCast(r.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "N" _
                            AndAlso DirectCast(r.FindControl("hfMScheme"), HiddenField).Value = strSchemeCode _
                            AndAlso DirectCast(r.FindControl("hfGCategoryName"), HiddenField).Value = strCategoryName _
                            AndAlso r.Visible Then
                        blnVisible = True
                        Exit For
                    End If

                Next

                If blnVisible Then
                    ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
                    Select Case Session("language")
                        Case CultureLanguage.TradChinese
                            gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("hfGCategoryNameChi"), HiddenField).Value, True)
                        Case CultureLanguage.SimpChinese
                            gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("hfGCategoryNameCN"), HiddenField).Value, True)
                        Case Else
                            gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value, True)
                    End Select
                    ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]

                    gvr.Cells(1).ColumnSpan = 2
                    gvr.Cells(1).CssClass = "SubsidizeCategoryHeader"
                    gvr.Cells(2).Visible = False

                Else
                    gvr.Visible = False

                End If

            End If

        Next

        ' End of Handle Category


        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        Dim strPreviousScheme As String = String.Empty

        For Each gvr As GridViewRow In gvPracticeSchemeInfo.Rows
            If Not gvr.Visible Then
                Continue For
            End If

            Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfMScheme"), HiddenField).Value

            If Not udtSchemeBackOfficeList.Filter(strSchemeCode).DisplaySubsidizeDesc Then
                gvr.Cells(1).ColumnSpan = 2
                gvr.Cells(2).Visible = False
                gvr.Cells(1).Text = Me.GetGlobalResourceObject("Text", "N/A")

            End If

            ' Grouping depends on gridview instead of subsidizelist
            Dim RowCount As Integer = 0

            If Not strPreviousScheme.Equals(strSchemeCode) Then

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    If gvrow.Visible Then
                        If DirectCast(gvrow.FindControl("hfMScheme"), HiddenField).Value = strSchemeCode Then
                            RowCount += 1
                        End If
                    End If
                Next

                gvr.Cells(0).RowSpan = RowCount
                gvr.Cells(3).RowSpan = RowCount
                gvr.Cells(4).RowSpan = RowCount
                gvr.Cells(5).RowSpan = RowCount

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    If DirectCast(gvrow.FindControl("hfMScheme"), HiddenField).Value = strSchemeCode AndAlso DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(1).Text = Me.GetGlobalResourceObject("Text", "NoServiceFeesProvided")
                    gvr.Cells(1).CssClass = "tableText"
                    gvr.Cells(1).RowSpan = RowCount
                    gvr.Cells(1).ColumnSpan = 2
                    gvr.Cells(2).Visible = False
                End If

            Else
                gvr.Cells(0).Visible = False
                gvr.Cells(3).Visible = False
                gvr.Cells(4).Visible = False
                gvr.Cells(5).Visible = False

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvPracticeSchemeInfo.Rows
                    If DirectCast(gvrow.FindControl("hfMScheme"), HiddenField).Value = strSchemeCode AndAlso DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(1).Visible = False
                    gvr.Cells(2).Visible = False
                End If

            End If

            strPreviousScheme = strSchemeCode

        Next

        ' Add tooltips to Scheme Name
        Dim strLang As String = Session("language")

        Dim udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache

        For Each r As GridViewRow In CType(sender, GridView).Rows
            Dim lblMScheme As Label = CType(r.FindControl("lblMScheme"), Label)
            Dim hfMScheme As HiddenField = CType(r.FindControl("hfMScheme"), HiddenField)
            lblMScheme.ToolTip = GetMasterSchemeDesc(udtSchemeBackOfficeModelCollection, hfMScheme.Value, strLang)

            Dim lblScheme As Label = r.FindControl("lblScheme")

            If Not IsNothing(lblScheme) Then
                Dim dt As DataTable = udtSchemeBackOfficeBLL.GetSubsidizeItemDetailsBySubsidizeCode(CType(r.FindControl("hfScheme"), HiddenField).Value)
                CType(r.FindControl("lblScheme"), Label).ToolTip = GetSubSchemeDesc(dt, strLang)
            End If

            Dim imgButton As ImageButton = CType(r.FindControl("btnSchemeNameHelp"), ImageButton)
            If Not imgButton Is Nothing Then
                Me.ClientScript.RegisterForEventValidation(imgButton.UniqueID)
            End If
        Next

    End Sub

    '

    Private Sub GridViewGroupMasterSchemeCode(ByRef gv As GridView, ByVal aryIntColumn_MSSchemeCode As Integer(), ByVal aryIntColumn_Merge As Integer(), ByVal strLblScheme As String, ByVal strLblSubsidize As String)
        If gv.Rows.Count = 0 Then Return

        ' If previously grouped, no need to run again (checked by the first cell in header row - ColumnSpan 2)
        If gv.HeaderRow.Cells(aryIntColumn_MSSchemeCode(0)).ColumnSpan = 2 Then Return

        Dim intRow As Integer = 0
        Dim intLastRow As Integer = -1
        Dim strLastSchemeCode As String = String.Empty

        ' Change the heading
        gv.HeaderRow.Cells(aryIntColumn_MSSchemeCode(0)).ColumnSpan = 2
        gv.HeaderRow.Cells(aryIntColumn_MSSchemeCode(1)).Visible = False

        ' Get the SchemeBackOfficeModelCollection first (to avoid over-accessing SQL or cache)
        Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
        Dim udtSchemeBOList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup()

        Dim aryColumnSpanMScheme As New ArrayList

        For Each r As GridViewRow In gv.Rows
            ' CRE15-004 TIV & QIV [Start][Lawrence]
            If r.Visible = False Then
                intRow += 1
                Continue For
            End If
            ' CRE15-004 TIV & QIV [End][Lawrence]

            Dim strCurrentSchemeCode As String = CType(r.FindControl(strLblScheme), Label).Text.Trim

            If udtSchemeBOList.Filter(strCurrentSchemeCode).DisplaySubsidizeDesc = False Then
                If aryColumnSpanMScheme.Contains(strCurrentSchemeCode) Then
                    r.Visible = False

                    intRow += 1
                    Continue For

                Else
                    r.Cells(aryIntColumn_MSSchemeCode(0)).ColumnSpan = 2
                    r.Cells(aryIntColumn_MSSchemeCode(1)).Visible = False

                    aryColumnSpanMScheme.Add(strCurrentSchemeCode)

                End If

            Else
                ' Convert subsidize code to display code
                Dim lblScheme As Label = r.FindControl(strLblScheme)
                Dim lblSubsidize As Label = r.FindControl(strLblSubsidize)
                lblSubsidize.Text = udtSchemeBOList.Filter(lblScheme.Text.Trim).SubsidizeGroupBackOfficeList.Filter(lblScheme.Text.Trim, lblSubsidize.Text.Trim).SubsidizeDisplayCode
            End If

            If intRow = 0 Then
                strLastSchemeCode = CType(r.FindControl(strLblScheme), Label).Text.Trim
                intLastRow = 0

                ' Convert scheme code to display code
                Dim lblScheme As Label = r.FindControl(strLblScheme)
                lblScheme.Text = udtSchemeBOList.Filter(lblScheme.Text.Trim).DisplayCode

            Else
                If strCurrentSchemeCode = strLastSchemeCode Then
                    For Each intColumn As Integer In aryIntColumn_Merge
                        If gv.Rows(intLastRow).Cells(intColumn).RowSpan = 0 Then gv.Rows(intLastRow).Cells(intColumn).RowSpan = 1
                        gv.Rows(intLastRow).Cells(intColumn).RowSpan += 1
                        r.Cells(intColumn).Visible = False
                    Next
                Else
                    strLastSchemeCode = strCurrentSchemeCode
                    intLastRow = intRow

                    ' Convert scheme code to display code
                    Dim lblScheme As Label = r.FindControl(strLblScheme)
                    lblScheme.Text = udtSchemeBOList.Filter(lblScheme.Text.Trim).DisplayCode
                End If

            End If

            intRow += 1

        Next
    End Sub

    Private Function GetSubSchemeDisplayCode(ByVal dtSubsidizeItemDetails As DataTable, ByVal strSScheme As String) As String
        If dtSubsidizeItemDetails.Rows(0).Item("Subsidize_code").ToString.Trim = strSScheme.Trim Then
            Return dtSubsidizeItemDetails.Rows(0).Item("display_code").ToString.Trim
        End If

        Return Nothing
    End Function

    Private Function GetMasterSchemeDesc(ByVal udtSchemeBackOfficeModelCollection As SchemeBackOfficeModelCollection, ByVal strMScheme As String, ByVal strLang As String) As String
        For Each udtSchemeBackOfficeModel As SchemeBackOfficeModel In udtSchemeBackOfficeModelCollection
            If udtSchemeBackOfficeModel.SchemeCode.Trim = strMScheme Then
                Return udtSchemeBackOfficeModel.SchemeDesc(strLang)
            End If
        Next

        Return Nothing

    End Function

    Private Function GetSubSchemeDesc(ByVal dtSubsidizeItemDetails As DataTable, ByVal strLang As String) As String

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If strLang = English Then
        '    Return dtSubsidizeItemDetails.Rows(0).Item("subsidize_item_desc").ToString.Trim
        'Else
        '    Return dtSubsidizeItemDetails.Rows(0).Item("subsidize_item_desc_chi").ToString.Trim
        'End If

        Select Case strLang
            Case English
                Return dtSubsidizeItemDetails.Rows(0).Item("subsidize_item_desc").ToString.Trim
            Case TradChinese
                Return dtSubsidizeItemDetails.Rows(0).Item("subsidize_item_desc_chi").ToString.Trim
            Case SimpChinese
                Return dtSubsidizeItemDetails.Rows(0).Item("subsidize_item_desc_cn").ToString.Trim
            Case Else
                Return dtSubsidizeItemDetails.Rows(0).Item("subsidize_item_desc").ToString.Trim
        End Select
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Return Nothing
    End Function

#End Region

#End Region

#Region "Tab - Bank Account Information"
    Private Sub gvBankInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBankInfo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As PracticeModel = CType(e.Row.DataItem, PracticeModel)
            Dim lblBankPracticeStatus As Label
            Dim strPracticeStatus As String = String.Empty

            lblBankPracticeStatus = CType(e.Row.FindControl("lblBankPracticeStatus"), Label)
            strPracticeStatus = dr.RecordStatus

            Dim strPracticeStatusDesc As String = String.Empty
            If Session("language") = CultureLanguage.TradChinese Then
                Status.GetDescriptionFromDBCode("MyProfilePracticeStatus", strPracticeStatus, String.Empty, strPracticeStatusDesc)
            ElseIf Session("language") = CultureLanguage.SimpChinese Then
                Status.GetDescriptionFromDBCode("MyProfilePracticeStatus", strPracticeStatus, String.Empty, String.Empty, strPracticeStatusDesc)
            Else
                Status.GetDescriptionFromDBCode("MyProfilePracticeStatus", strPracticeStatus, strPracticeStatusDesc, String.Empty)
            End If
            lblBankPracticeStatus.Text = strPracticeStatusDesc

            If (Me.chkShowActive.Checked Or Me.chkBankAcctShowActiveOnly.Checked) Then
                Select Case strPracticeStatus
                    Case PracticeStatus.Active
                        e.Row.Visible = True
                    Case Else
                        e.Row.Visible = False
                End Select
            Else
                e.Row.Visible = True
            End If
        End If
    End Sub

    Protected Sub chkBankAcctShowActiveOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.chkShowActive.Checked = Me.chkBankAcctShowActiveOnly.Checked

        udtSP = Me.udtServiceProviderBLL.GetSP()
        gvPracticeInfo.DataSource = udtSP.PracticeList.Values
        gvPracticeInfo.DataBind()
        gvBankInfo.DataBind()

    End Sub
#End Region

#Region "Tab - System Information"
    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim blnErr As Boolean = False
        Dim strOriUsername As String = String.Empty
        Dim strNewUsername As String = String.Empty
        Dim strOldWebPWD As String = String.Empty
        Dim strNewWebPWD As String = String.Empty
        Dim strConfirmNewWebPWD As String = String.Empty
        Dim strOldIVRSPWD As String = String.Empty
        Dim strNewIVRSPWD As String = String.Empty
        Dim strConfirmNewIVRSPWD As String = String.Empty
        Dim strDefaultLang As String = String.Empty
        Dim strDefaultPrintOption As String = String.Empty
        Dim strChgUsername As String = "N"
        Dim strChgWebPWD As String = "N"
        Dim strChgIVRSPWD As String = "N"
        Dim blnSavePWDErr As Boolean = False
        Dim dt As DataTable
        Dim strSPID As String = String.Empty
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim intPlatform As Integer = SubPlatform()
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'Dim udtAuditLogEntry As New AuditLogEntry ''Begin Writing Audit Log
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text.Trim)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, AublitLogDescription.SPUpdateProfile)

        Me.udcInfoMsgBox.Visible = False

        Me.imgSPSelectPrintOptionError.Visible = False
        Me.imgSPConfirmNewIVRSPWDError.Visible = False
        Me.imgSPConfirmNewPWDError.Visible = False
        Me.imgSPNewIVRSPWDError.Visible = False
        Me.imgSPNewPWDError.Visible = False
        Me.imgSPOldIVRSPWDError.Visible = False
        Me.imgSPOldPWDError.Visible = False
        Me.imgUsernameError.Visible = False

        strSPID = Me.lblSPID.Text.Trim
        'By Paul, modified on 22/9, second parameter is longer be used
        dt = udtSPProfileBLL.loadSPLoginProfile(strSPID, String.Empty)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Me.lblUsername.Text.Equals(Me.GetGlobalResourceObject("Text", "--").ToString()) Then
            Me.lblUsername.Text = String.Empty
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        strOriUsername = Me.lblUsername.Text.Trim
        strNewUsername = Me.txtUsername.Text.Trim

        If Not strOriUsername.Equals(strNewUsername) Then
            If chkInValidUsername(strOriUsername, strNewUsername) Then
                blnErr = True
                Me.imgUsernameError.Visible = True
            Else
                strChgUsername = "Y"
            End If
        End If

        If Me.chkChgWebPWD.Checked Then
            strOldWebPWD = Me.txtOldWebPwd.Text
            strNewWebPWD = Me.txtNewWebPwd.Text
            strConfirmNewWebPWD = Me.txtConfirmNewWebPwd.Text

            If Not udtSPProfileBLL.chkIsEmpty(strOldWebPWD) Then
                If udtSPProfileBLL.chkMatchWebPWD(dt, strOldWebPWD) Then
                    If Not udtSPProfileBLL.chkIsEmpty(strNewWebPWD) Then
                        If Not udtSPProfileBLL.chkIsSamePWD(strOldWebPWD, strNewWebPWD) Then
                            If udtSPProfileBLL.chkValidPassword(strNewWebPWD) Then
                                If udtSPProfileBLL.chkIsIdenticalPassword(strNewWebPWD, strConfirmNewWebPWD) Then
                                    strChgWebPWD = "Y"
                                Else
                                    blnErr = True
                                    Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00017")
                                    Me.imgSPNewPWDError.Visible = True
                                    Me.imgSPConfirmNewPWDError.Visible = True
                                End If
                            Else
                                blnErr = True
                                Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00011")
                                Me.imgSPNewPWDError.Visible = True
                            End If
                        Else
                            blnErr = True
                            blnSavePWDErr = True
                            Me.imgSPNewPWDError.Visible = True
                        End If
                    Else
                        blnErr = True
                        Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00010")
                        Me.imgSPNewPWDError.Visible = True
                    End If

                Else
                    'Incorrect OLD Password
                    blnErr = True
                    Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00009")
                    Me.imgSPOldPWDError.Visible = True
                End If
            Else
                'Empty Old Password
                blnErr = True
                Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00008")
                Me.imgSPOldPWDError.Visible = True
            End If
        End If

        If Me.chkChgIVRSPwd.Checked Or Me.chkActivateIVRSPwd.Checked Then
            strOldIVRSPWD = Me.txtIVRSOldPwd.Text
            strNewIVRSPWD = Me.txtIVRSNewPwd.Text
            strConfirmNewIVRSPWD = Me.txtConfirmNewIVRSPwd.Text

            If Not udtSPProfileBLL.chkIsEmpty(strOldIVRSPWD) Or Me.chkActivateIVRSPwd.Visible Then
                If udtSPProfileBLL.chkMatchIVRSPWD(dt, strOldIVRSPWD) Or Me.chkActivateIVRSPwd.Visible Then
                    If Not udtSPProfileBLL.chkIsEmpty(strNewIVRSPWD) Then
                        If Not udtSPProfileBLL.chkIsSamePWD(strOldIVRSPWD, strNewIVRSPWD) Or Me.chkActivateIVRSPwd.Visible Then
                            If udtSPProfileBLL.chkValidIVRSPassword(strNewIVRSPWD) Then
                                If udtSPProfileBLL.chkIsIdenticalPassword(strNewIVRSPWD, strConfirmNewIVRSPWD) Then
                                    strChgIVRSPWD = "Y"
                                Else
                                    blnErr = True
                                    Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00016")
                                    Me.imgSPNewIVRSPWDError.Visible = True
                                    Me.imgSPConfirmNewIVRSPWDError.Visible = True
                                End If
                            Else
                                blnErr = True
                                Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00015")
                                Me.imgSPNewIVRSPWDError.Visible = True
                            End If
                        Else
                            blnErr = True
                            blnSavePWDErr = True
                            Me.imgSPNewIVRSPWDError.Visible = True
                        End If
                    Else
                        blnErr = True
                        Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00014")
                        Me.imgSPNewIVRSPWDError.Visible = True
                    End If
                Else
                    'Incorrect OLD Password
                    blnErr = True
                    Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00013")
                    Me.imgSPOldIVRSPWDError.Visible = True
                End If
            Else
                'Empty Old Password
                blnErr = True
                Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00012")
                Me.imgSPOldIVRSPWDError.Visible = True
            End If

        End If

        If blnSavePWDErr Then
            Me.udcErrMsgBox.AddMessage(strCommFuncCode, "E", "00052")
        End If

        strDefaultLang = Me.ddlDefaultLang.SelectedValue
        strDefaultPrintOption = Me.GetPrintOptionSelectedValue()

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If PanelPrintingOptionSetting.Visible = True Then
            Dim sm As SystemMessage
            Dim validator As Common.Validation.Validator = New Common.Validation.Validator
            sm = validator.chkSelectedPrintFormOption(strDefaultPrintOption)
            If Not sm Is Nothing Then
                blnErr = True
                Me.udcErrMsgBox.AddMessage(sm)
                imgSPSelectPrintOptionError.Visible = True
            End If
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If Me.chkActivateIVRSPwd.Checked Or Me.chkChgIVRSPwd.Checked Then
            Me.txtIVRSOldPwd.BackColor = Drawing.Color.White
            Me.txtIVRSNewPwd.BackColor = Drawing.Color.White
            Me.txtConfirmNewIVRSPwd.BackColor = Drawing.Color.White
        End If


        If Not blnErr Then
            Try
                udtSPProfileBLL.saveSPLoginProfile(strSPID, strOriUsername, strNewUsername, strChgWebPWD, strNewWebPWD, strChgIVRSPWD, strNewIVRSPWD, strDefaultLang, CType(udtSPProfileBLL.LoadToSession, Byte()), strDefaultPrintOption)
            Catch eSql As SqlClient.SqlException
                blnErr = True
                If eSql.Number = 50000 Then
                    Dim strMsg As String = eSql.Message
                    Me.udcErrMsgBox.AddMessage("990001", "D", strMsg)
                Else
                    Throw eSql
                End If
            Catch ex As Exception
                Throw ex
            End Try

            udtAuditLogEntry.AddDescripton("SP_ID", strSPID)
            udtAuditLogEntry.AddDescripton("Username", strNewUsername)
            udtAuditLogEntry.AddDescripton("Default Language", strDefaultLang)
            udtAuditLogEntry.AddDescripton("Default Printing Option", strDefaultPrintOption)
            udtAuditLogEntry.AddDescripton("Web Password Updated", Me.chkChgWebPWD.Checked)
            udtAuditLogEntry.AddDescripton("IVRS Password Updated", Me.chkChgIVRSPwd.Checked)

            If Not blnErr Then
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00005, AublitLogDescription.SPUpdateProfileSuccess)
            Else
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, AublitLogDescription.SPUpdateProfileFail)
            End If

            If Not blnErr Then

                Me.udcInfoMsgBox.AddMessage(strFuncCode, "I", "00004")
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMsgBox.BuildMessageBox()

                'By Paul, modified on 22/9, second parameter is longer be used
                dt = udtSPProfileBLL.loadSPLoginProfile(strSPID, String.Empty)
                Me.txtUsername.Text = dt.Rows(0).Item("Alias_Account")
                Me.lblUsername.Text = dt.Rows(0).Item("Alias_Account")

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If Me.lblUsername.Text.Equals(String.Empty) Then
                    Me.lblUsername.Text = Me.GetGlobalResourceObject("Text", "--").ToString()
                End If
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"))
                Select Case dt.Rows(0).Item("Project").Trim()
                    Case TokenProjectType.EHCVS
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"), False, False, False, LCase(Session("language")))

                    Case TokenProjectType.EHR
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"), True, True, False, LCase(Session("language")))

                    Case Else
                        Me.lblTokenSerialNo.Text = TokenModel.DisplayTokenSerialNoSP(dt.Rows(0).Item("Token_Serial_No"), dt.Rows(0).Item("Project"), False, False, False, LCase(Session("language")))
                End Select
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

                ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

                If dt.Rows(0).Item("Default_Language").ToString.Trim.Equals(String.Empty) Then
                    Me.ddlDefaultLang.SelectedValue = "E"
                Else
                    Me.ddlDefaultLang.SelectedValue = dt.Rows(0).Item("Default_Language")

                End If

                Me.udtSP = Me.udtServiceProviderBLL.GetSP()

                If dt.Rows(0).Item("ConsentPrintOption") Is DBNull.Value Then
                    Dim strvalue As String = String.Empty
                    Dim udcGeneralFun As New Common.ComFunction.GeneralFunction
                    udcGeneralFun.getSystemParameter("DefaultConsentPrintOption", strvalue, String.Empty)
                    Me.udtSP.PrintOption = strvalue
                Else
                    Me.udtSP.PrintOption = dt.Rows(0).Item("ConsentPrintOption").ToString()
                End If

                Me.udtSP.AliasAccount = dt.Rows(0).Item("Alias_Account")
                Me.udtSP.TokenSerialNo = dt.Rows(0).Item("Token_Serial_No")
                'Me.udtSP.PrintOption = Me.ddlDefaultPrintOption.SelectedValue
                Me.udtSP.DefaultLanguage = Me.ddlDefaultLang.SelectedValue
                Me.udtSP.TSMP = dt.Rows(0).Item("TSMP")
                Me.udtServiceProviderBLL.SaveToSession(Me.udtSP)

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Select Case intPlatform
                    Case EnumHCSPSubPlatform.NA
                        Me.PanelSPDefaultLang.Visible = True
                        'Me.PanelIVRSPasswordSettings.Visible = True
                    Case EnumHCSPSubPlatform.HK
                        Me.PanelSPDefaultLang.Visible = True
                        'Me.PanelIVRSPasswordSettings.Visible = True
                    Case EnumHCSPSubPlatform.CN
                        Me.PanelSPDefaultLang.Visible = False
                        'Me.PanelIVRSPasswordSettings.Visible = False
                End Select
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                If dt.Rows(0).Item("SP_IVRS_Password").ToString.Trim.Equals(String.Empty) Then
                    Me.chkActivateIVRSPwd.Visible = True
                    Me.chkChgIVRSPwd.Visible = False
                    Me.chkChgIVRSPwd.Checked = False
                    Me.txtIVRSOldPwd.Visible = False
                    Me.lblIVRSOldPwdText.Visible = False
                    'Me.chkChgIVRSPwd.Enabled = False
                Else
                    Me.chkActivateIVRSPwd.Visible = False
                    Me.chkChgIVRSPwd.Visible = True
                    Me.chkChgIVRSPwd.Checked = False
                    Me.txtIVRSOldPwd.Visible = True
                    Me.lblIVRSOldPwdText.Visible = True
                    Me.lblIVRSOldPwdText.Text = HttpContext.GetGlobalResourceObject("Text", "OldPassword")
                    'Me.chkChgIVRSPwd.Enabled = False
                End If
                Me.chkChgWebPWD.Checked = False
                Me.chkChgWebPWD.Enabled = False
                Me.chkChgIVRSPwd.Enabled = False
                Me.chkActivateIVRSPwd.Checked = False
                Me.chkActivateIVRSPwd.Enabled = False

                Me.chkChgWebPWD_CheckedChanged(Nothing, Nothing)
                Me.chkChgIVRSPwd_CheckedChanged(Nothing, Nothing)

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.lblUsername.Text = Me.txtUsername.Text
                Me.lblUsername.Visible = True
                Me.txtUsername.Visible = False
                Me.lblSPUsernameTip.Visible = False
                Me.lblSPUsernameTip1.Visible = False
                Me.lblSPUsernameTip2.Visible = False
                Me.lblSPUsernameTip2a.Visible = False
                Me.lblSPUsernameTip2b.Visible = False
                Me.lblSPUsernameTip2c.Visible = False
                Me.tdUsernameTips.Style.Add("display", "none")
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                Me.ddlDefaultLang.Enabled = False
                Me.btnChkAvail.Visible = False
                Me.btnGetUsernameFromEHRSS.Visible = False

                'Init Print option 
                Me.InitLoginUserPrintOptionList(False)

                udtSPProfileBLL.ClearSession()
                udtSPProfileBLL.SaveToSession(dt.Rows(0).Item("TSMP"))

                Me.btnEdit.Visible = True
                Me.btnSave.Visible = False
                Me.btnCancel.Visible = False

            End If

        End If
        Me.udcErrMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, AublitLogDescription.SPUpdateProfileFail)

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text.Trim)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00016, AublitLogDescription.SPUpdateProfileEdit)
        ' CRE11-021 log the missed essential information [End]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strSPID As String = Me.lblSPID.Text.Trim
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.udcInfoMsgBox.Visible = False
        Me.udcErrMsgBox.Visible = False

        Me.imgSPSelectPrintOptionError.Visible = False
        Me.imgSPConfirmNewIVRSPWDError.Visible = False
        Me.imgSPConfirmNewPWDError.Visible = False
        Me.imgSPNewIVRSPWDError.Visible = False
        Me.imgSPNewPWDError.Visible = False
        Me.imgSPOldIVRSPWDError.Visible = False
        Me.imgSPOldPWDError.Visible = False
        Me.imgUsernameError.Visible = False

        Me.chkChgWebPWD.Enabled = True
        Me.chkChgWebPWD.Checked = False
        Me.chkChgWebPWD_CheckedChanged(Nothing, Nothing)

        If Session(IncludeIVRSEnabledScheme) Then
            Me.chkActivateIVRSPwd.Enabled = True
            Me.chkActivateIVRSPwd.Checked = False

            Me.chkChgIVRSPwd.Enabled = True
            Me.chkChgIVRSPwd.Checked = False
            Me.chkChgIVRSPwd_CheckedChanged(Nothing, Nothing)
        End If

        Me.ddlDefaultLang.Enabled = True

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Me.ddlDefaultPrintOption.Enabled = True
        Me.EnablePrintOptionList(strSPID)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


        'Me.lblDefaultPrintOptionRemind.Visible = False
        'If Me.ddlDefaultPrintOption.SelectedValue = PrintFormOptionSelection.PrintFormOptionValue.PreprintForm Then
        '    Me.lblDefaultPrintOptionRemind.Visible = True
        'ElseIf Me.ddlDefaultPrintOption.SelectedValue = PrintFormOptionSelection.PrintFormOptionValue.PrintConsentOnly Then
        '    Me.lblDefaultPrintOptionRemind.Visible = True
        'End If

        'Me.ChangePrintOptionRemindText(Me.lblDefaultPrintOptionRemind, Me.ddlDefaultPrintOption.SelectedValue)

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Me.txtUsername.Text = Me.lblUsername.Text
        If Me.lblUsername.Text.Equals(Me.GetGlobalResourceObject("Text", "--").ToString()) Then
            Me.txtUsername.Text = String.Empty
        Else
            Me.txtUsername.Text = Me.lblUsername.Text
        End If

        Me.txtUsername.Visible = True
        Me.lblSPUsernameTip.Visible = True
        Me.lblSPUsernameTip1.Visible = True
        Me.lblSPUsernameTip2.Visible = True
        Me.lblSPUsernameTip2a.Visible = True
        Me.lblSPUsernameTip2b.Visible = True
        Me.lblSPUsernameTip2c.Visible = True
        Me.tdUsernameTips.Style.Add("display", "initial")
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Me.lblUsername.Visible = False
        Me.btnChkAvail.Visible = True
        Me.btnGetUsernameFromEHRSS.Visible = True

        Me.btnEdit.Visible = False
        Me.btnSave.Visible = True
        Me.btnCancel.Visible = True
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text.Trim)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00017, AublitLogDescription.SPUpdateProfileCancel)
        ' CRE11-021 log the missed essential information [End]


        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]

        'Me.udcInfoMsgBox.Visible = False
        'Me.udcErrMsgBox.Visible = False
        '
        'Me.imgSPSelectPrintOptionError.Visible = False
        'Me.imgSPConfirmNewIVRSPWDError.Visible = False
        'Me.imgSPConfirmNewPWDError.Visible = False
        'Me.imgSPNewIVRSPWDError.Visible = False
        'Me.imgSPNewPWDError.Visible = False
        'Me.imgSPOldIVRSPWDError.Visible = False
        'Me.imgSPOldPWDError.Visible = False
        'Me.imgUsernameError.Visible = False
        '
        'Me.chkChgWebPWD.Enabled = False
        'Me.chkChgWebPWD.Checked = False
        'Me.chkChgWebPWD_CheckedChanged(Nothing, Nothing)
        '
        'Me.chkActivateIVRSPwd.Enabled = False
        'Me.chkActivateIVRSPwd.Checked = False
        '
        'Me.chkChgIVRSPwd.Enabled = False
        'Me.chkChgIVRSPwd.Checked = False
        'Me.chkChgIVRSPwd_CheckedChanged(Nothing, Nothing)
        '
        'Me.ddlDefaultLang.Enabled = False

        'Print Option
        'Me.InitLoginUserPrintOptionList(False)

        udtUserAC = UserACBLL.GetUserAC

        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider And Not sender Is Nothing Then
            udtSP = CType(udtUserAC, ServiceProviderModel)
            udtSP = udtSPProfileBLL.loadSP(udtSP.SPID)
            udtServiceProviderBLL.SaveToSession(udtSP)

            'Me.ddlDefaultPrintOption.SelectedValue = udtSP.PrintOption
        End If

        CancelClick_Clear()


        'Me.ddlDefaultLang.SelectedValue = udtSP.DefaultLanguage
        '
        'Me.txtUsername.Text = Me.lblUsername.Text
        'Me.txtUsername.Visible = False
        'Me.lblSPUsernameTip.Visible = False
        'Me.lblSPUsernameTip1.Visible = False
        ''CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        ''-----------------------------------------------------------------------------------------
        'Me.lblSPUsernameTip2.Visible = False
        'Me.lblSPUsernameTip2a.Visible = False
        'Me.lblSPUsernameTip2b.Visible = False
        'Me.lblSPUsernameTip2c.Visible = False
        'Me.tdUsernameTips.Style.Add("display", "none")
        ''CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        'Me.lblUsername.Visible = True
        'Me.btnChkAvail.Visible = False
        'Me.btnGetUsernameFromEHRSS.Visible = False
        '
        'Me.btnEdit.Visible = True
        'Me.btnSave.Visible = False
        'Me.btnCancel.Visible = False

        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
    End Sub

    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Winnie]
    Protected Sub btnGetUsernameFromeHRSS_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00044, AublitLogDescription.GetEHRSSUsernameClick)

        chkGetUsernameFromEHRSS.Checked = False
        ModalPopupExtenderConfirmConsent.Show()
    End Sub

    Protected Sub ibtnConsentCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00046, AublitLogDescription.GetEHRSSConsentCancelClick)

        ModalPopupExtenderConfirmConsent.Hide()
    End Sub

    Protected Sub ibtnConsentConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
        Me.imgUsernameError.Visible = False

        udtSP = Me.udtServiceProviderBLL.GetSP()
        Dim strSPHKID As String = udtSP.HKID

        ModalPopupExtenderConfirmConsent.Hide()

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00045, AublitLogDescription.GetEHRSSConsentConfirmClick)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00047, AublitLogDescription.GetEHRSSUsername)

        Dim udtInXml As InGeteHRSSLoginAliasXmlModel = Nothing

        Try
            udtInXml = (New eHRServiceBLL).GeteHRSSLoginAlias(strSPHKID)

        Catch ex As Exception
            ' Message: The username cannot be obtained from eHRSS, please try again later.
            udcErrMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00392)
            udcErrMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00049, AublitLogDescription.GetEHRSSUsernameFail & String.Format("StackTrace={0}", ex.Message))
            Return
        End Try

        udtAuditLogEntry.AddDescripton("Result Code", udtInXml.ResultCode)
        udtAuditLogEntry.AddDescripton("Result Description", udtInXml.ResultDescription)

        Select Case udtInXml.ResultCodeEnum

            Case eHRResultCode.R9002_UserNotFound
                ' Message: You are not a registered user in eHRSS.
                udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00039)
                udcInfoMsgBox.Type = InfoMessageBoxType.Information
                udcInfoMsgBox.BuildMessageBox()
                udcErrMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00049, AublitLogDescription.GetEHRSSUsernameFail)
                Return

            Case eHRResultCode.R1000_Success

            Case Else
                ' Message: The username cannot be obtained from eHRSS, please try again later.
                udcErrMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00392)
                udcErrMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00049, AublitLogDescription.GetEHRSSUsernameFail)
                Return
        End Select

        Dim strOriUsername As String = String.Empty
        Dim strEHRSSUsername As String = String.Empty
        Dim blnErr As Boolean = False

        strOriUsername = Me.lblUsername.Text.Trim
        strEHRSSUsername = udtInXml.LoginAlias.Trim.ToUpper
        udtAuditLogEntry.AddDescripton("Username", strEHRSSUsername)

        If Not strEHRSSUsername.Equals(strOriUsername) Then

            ' Check username availability
            If Not udtSPProfileBLL.chkValidLoginID(strEHRSSUsername) Then
                blnErr = True
                Me.udcErrMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00393, "%s", strEHRSSUsername)

            ElseIf udtSPProfileBLL.chkDuplicateUsername(strEHRSSUsername) Then
                blnErr = True
                Me.udcErrMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00394, "%s", strEHRSSUsername)
            End If
        End If

        If blnErr Then
            Me.udcErrMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00049, AublitLogDescription.GetEHRSSUsernameFail)
        Else
            ' Fill the value
            Me.txtUsername.Text = strEHRSSUsername

            ' Message: The Username is retrieved from eHRSS successfully.
            udcInfoMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00040)
            udcInfoMsgBox.Type = InfoMessageBoxType.Complete
            udcInfoMsgBox.BuildMessageBox()

            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00048, AublitLogDescription.GetEHRSSUsernameSuccess)
        End If
    End Sub
    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Winnie]

    Protected Sub btnChkAvail_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strNewUsername As String = String.Empty
        Dim strOriUsername As String = String.Empty
        Dim blnErr As Boolean = False
        'Dim udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Common.Component.LogID.LOG00001)
        Dim udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Dim i As Integer = 0

        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False

        Me.imgUsernameError.Visible = False
        strNewUsername = Me.txtUsername.Text.Trim
        strOriUsername = Me.lblUsername.Text.Trim

        If Not strNewUsername.Equals(strOriUsername) Then
            blnErr = chkInValidUsername(strOriUsername, strNewUsername)

            udtAuditLogEntry.AddDescripton("Username", strNewUsername)
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AublitLogDescription.CheckUsernameAvailability)

            If Not blnErr Then
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcInfoMsgBox.AddMessage(strFuncCode, "I", "00003")
                'udtAuditLogEntry.WriteLog(strFuncCode, Common.Component.MsgCode.MSG00001, "New Username " + strNewUsername + " is available to use.")
                udtAuditLogEntry.AddDescripton("Username", strNewUsername)
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AublitLogDescription.UsernameIsAvailable)
            End If

            Me.udcErrMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00003, AublitLogDescription.UsernameIsNotAvailable)
            Me.udcInfoMsgBox.BuildMessageBox()
        End If
    End Sub

    Protected Sub chkChgIVRSPwd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        changeIVRSPwd(Me.chkChgIVRSPwd.Checked)
    End Sub

    Protected Sub chkChgWebPWD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If Me.chkChgWebPWD.Checked Then
        '    Me.txtOldWebPwd.ReadOnly = False
        '    Me.txtNewWebPwd.ReadOnly = False
        '    Me.txtConfirmNewWebPwd.ReadOnly = False
        '    Me.txtOldWebPwd.BackColor = Drawing.Color.White
        '    Me.txtNewWebPwd.BackColor = Drawing.Color.White
        '    Me.txtConfirmNewWebPwd.BackColor = Drawing.Color.White
        'Else
        '    Me.txtOldWebPwd.ReadOnly = True
        '    Me.txtNewWebPwd.ReadOnly = True
        '    Me.txtConfirmNewWebPwd.ReadOnly = True
        '    Me.txtOldWebPwd.BackColor = Drawing.Color.Silver
        '    Me.txtNewWebPwd.BackColor = Drawing.Color.Silver
        '    Me.txtConfirmNewWebPwd.BackColor = Drawing.Color.Silver
        'End If

        If Me.chkChgWebPWD.Checked Then
            Me.PanelWebPasswordSettings.CssClass = "SettingsPanelStyle"

            Me.tdPwdCell01.Attributes("class") = "SettingTDRightStyle"
            Me.tdPwdCell02.Attributes("class") = "SettingTDRightStyle"
            Me.tdPwdCell03.Attributes("class") = "SettingTDRightStyle"

            Me.PanelWebPassword01.Style("display") = "block"
            Me.PanelWebPassword02.Style("display") = "block"
            Me.PanelWebPasswordTips.Style("display") = "block"

            Me.txtOldWebPwd.BackColor = Drawing.Color.White
            Me.txtNewWebPwd.BackColor = Drawing.Color.White
            Me.txtConfirmNewWebPwd.BackColor = Drawing.Color.White

            Me.txtOldWebPwd.Enabled = True
            Me.txtNewWebPwd.Enabled = True
            Me.txtConfirmNewWebPwd.Enabled = True
        Else
            Me.PanelWebPasswordSettings.CssClass = ""
            Me.tdPwdCell01.Attributes("class") = ""
            Me.tdPwdCell02.Attributes("class") = ""
            Me.tdPwdCell03.Attributes("class") = ""

            Me.PanelWebPassword01.Style("display") = "none"
            Me.PanelWebPassword02.Style("display") = "none"
            Me.PanelWebPasswordTips.Style("display") = "none"

            Me.txtOldWebPwd.BackColor = Drawing.Color.LightGray
            Me.txtNewWebPwd.BackColor = Drawing.Color.LightGray
            Me.txtConfirmNewWebPwd.BackColor = Drawing.Color.LightGray

            Me.txtOldWebPwd.Enabled = False
            Me.txtNewWebPwd.Enabled = False
            Me.txtConfirmNewWebPwd.Enabled = False
        End If

    End Sub

    Protected Sub chkChgDEPWD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not Me.chkChgDEPWD.Visible Then
            PanelDEWebPasswordSettings.CssClass = "SettingsPanelStyle"
            PanelDEWebPassword.Style("display") = "block"
        End If

        If Me.chkChgDEPWD.Checked Then
            If Me.chkChgDEPWD.Visible Then
                PanelDEWebPasswordSettings.CssClass = "SettingsPanelStyle"
                PanelDEWebPassword.Style("display") = "block"
            End If

            Me.txtDENewPWD.Enabled = True
            Me.txtDEConfirmNewPWD.Enabled = True
            Me.txtDENewPWD.BackColor = Drawing.Color.White
            Me.txtDEConfirmNewPWD.BackColor = Drawing.Color.White
        Else

            If Me.chkChgDEPWD.Visible Then
                PanelDEWebPasswordSettings.CssClass = ""
                PanelDEWebPassword.Style("display") = "none"
            End If

            Me.txtDENewPWD.Enabled = False
            Me.txtDEConfirmNewPWD.Enabled = False
            Me.txtDENewPWD.BackColor = Drawing.Color.LightGray
            Me.txtDEConfirmNewPWD.BackColor = Drawing.Color.LightGray
        End If

    End Sub

    Protected Sub chkActivateIVRSPwd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        changeIVRSPwd(Me.chkActivateIVRSPwd.Checked)
    End Sub
#End Region

#Region "Tab - Data Entry Account Maintenance"
    Private Sub gvdataEntryAcc_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvdataEntryAcc.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, DataEntryAcct)
    End Sub

    Private Sub gvdataEntryAcc_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvdataEntryAcc.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Me.udcErrMsgBox.Visible = False
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Me.imgDEFilterError.Visible = False
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
            Me.udcInfoMsgBox.Visible = False

            clearSelectedPractice()
            Dim strCommandArgument As String
            Dim strDataEntryAcct As String = String.Empty
            Dim dt As DataTable
            Dim strStatus As String = String.Empty
            Dim strAcctLocked As String = String.Empty

            Me.lblDEUsernameTip.Visible = False
            Me.lblDEUsernameTip1.Visible = False
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.lblDEUsernameTip2.Visible = False
            Me.lblDEUsernameTip2a.Visible = False
            Me.lblDEUsernameTip2b.Visible = False
            Me.lblDEUsernameTip2c.Visible = False
            Me.tdDEUsernameTip.Style.Add("display", "none")
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            strCommandArgument = e.CommandArgument.ToString.Trim
            Me.lblDELoginID.Text = strCommandArgument
            Me.lblDELoginID.Visible = True

            Me.txtDEloginID.Text = strCommandArgument
            Me.txtDEloginID.Visible = False
            Me.txtDEloginID.Enabled = False
            'Me.ddlDEDefaultPrintOption.Enabled = False

            strDataEntryAcct = strCommandArgument
            udtUserAC = UserACBLL.GetUserAC
            If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                udtSP = CType(udtUserAC, ServiceProviderModel)
            End If

            Me.chkChgDEPWD.Visible = True
            Me.chkChgDEPWD.Checked = False
            Me.chkChgDEPWD_CheckedChanged(Nothing, Nothing)

            dt = udtDataEntryAcctBLL.getDataEntryAcctDetails(udtSP.SPID, strDataEntryAcct)
            strStatus = dt.Rows(0).Item("Record_Status").ToString
            strAcctLocked = dt.Rows(0).Item("Account_Locked").ToString

            'If dt.Rows(0).Item("ConsentPrintOption") Is DBNull.Value Then
            '    Dim strConsentPrintOption As String
            '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            '    udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
            '    InitDataEntryMaintenancePrintOptionList(False, strConsentPrintOption, False)
            '    'Me.ddlDEDefaultPrintOption.SelectedValue = strConsentPrintOption
            'Else
            '    InitDataEntryMaintenancePrintOptionList(False, dt.Rows(0).Item("ConsentPrintOption"), False)
            '    'Me.ddlDEDefaultPrintOption.SelectedValue = dt.Rows(0).Item("ConsentPrintOption")
            'End If

            Me.chkDESuspend.Enabled = False
            If strStatus <> "A" Then
                Me.chkDESuspend.Checked = True
            Else
                Me.chkDESuspend.Checked = False
            End If

            Me.chkDEAccountLocked.Enabled = False
            If strAcctLocked = "Y" Then
                Me.chkDEAccountLocked.Checked = True
            Else
                Me.chkDEAccountLocked.Checked = False
            End If


            For Each dr As DataRow In dt.Rows
                If dr.Item("Practice_Status") = "A" Then
                    If dr.Item("SP_Practice_Display_Seq") > 0 Then
                        Me.chkPracticeList.Items.FindByValue(dr.Item("SP_Practice_Display_Seq").ToString + "-" + dr.Item("SP_Bank_Acc_display_Seq").ToString).Selected = True
                    End If
                End If
            Next

            'Me.btnAddDEAcct.Visible = True
            Me.btnAddDEAcct.Visible = Me.chkPracticeList.Visible
            Me.btnEditDEAcct.Visible = True
            Me.btnSaveDEAcct.Visible = False
            Me.btnCancelDEAcct.Visible = False
            'Me.lblDEPrintOptionRemind.Visible = False

            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            Dim intPasswordLevel As Integer = dt.Rows(0).Item("Password_Level")
            displayInfoResetPWMsgBox(intPasswordLevel)

            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
            'udtAuditLogEntry.AddDescripton("SP_ID", udtSP.SPID)
            udtAuditLogEntry.AddDescripton("Data Entry Account ID", lblDELoginID.Text)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00050, AublitLogDescription.DataEntryAccountLoadInfo)
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]


        End If

    End Sub

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Protected Sub btnFilterDEAcct_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("Username", txtDEFilter.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00039, "Data Entry Account List Filter click")

        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
        Me.imgDEFilterError.Visible = False

        Dim dtDataEntryAcct As DataTable = Nothing
        Dim dtDataEntryAcctFilter As DataTable = Nothing
        Dim strFilter As String = String.Empty

        txtDEFilter.Text = txtDEFilter.Text.Trim

        If Not udtDataEntryAcctBLL.chkIsEmpty(txtDEFilter.Text) Then
            Me.trUsername.Visible = True
            Me.trUsernameFound.Visible = False
            Me.trUsernameNotFound.Visible = False
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            Me.hfDEFilter.Value = txtDEFilter.Text

            DEAcctFilter(AntiXssEncoder.HtmlEncode(txtDEFilter.Text, True))
            ' I-CRE16-003 Fix XSS [End][Lawrence]

            udtAuditLogEntry.WriteEndLog(LogID.LOG00040, "Data Entry Account List Filter click success")

        Else
            Me.imgDEFilterError.Visible = True
            Me.udcErrMsgBox.AddMessage(New SystemMessage(strFuncCode, "E", "00020"))
            Me.udcErrMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00041, "Data Entry Account List Filter click fail")

        End If

    End Sub

    Protected Sub btnClearDEAcctSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00042, "Data Entry Account List Clear click")

        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
        Me.imgDEFilterError.Visible = False

        Me.trUsername.Visible = True
        Me.trUsernameFound.Visible = False
        Me.trUsernameNotFound.Visible = False

        'Clear Value
        hfDEFilter.Value = String.Empty
        Me.txtDEFilter.Text = String.Empty

        udtSP = Me.udtServiceProviderBLL.GetSP()

        'Reset the list
        Dim dtDataEntryAcct As DataTable
        dtDataEntryAcct = udtDataEntryAcctBLL.getDataEntryAcct(udtSP.SPID)
        Session(DataEntryAcct) = dtDataEntryAcct
        Me.GridViewDataBind(Me.gvdataEntryAcc, dtDataEntryAcct)

        'Overwrite the Base GridView
        Me.gvdataEntryAcc.PageSize = 20
        Me.gvdataEntryAcc.DataSource = dtDataEntryAcct
        Me.gvdataEntryAcc.DataBind()

        udtAuditLogEntry.WriteEndLog(LogID.LOG00043, "Data Entry Account List Clear click success")

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Protected Sub btnAddDEAcct_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False

        Me.gvdataEntryAcc.Enabled = False

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        InitDEFilterControl(False)
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Me.lblDELoginID.Visible = False
        Me.txtDEloginID.Visible = True
        Me.lblDEUsernameTip.Visible = True
        Me.lblDEUsernameTip1.Visible = True
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDEUsernameTip2.Visible = True
        Me.lblDEUsernameTip2a.Visible = True
        Me.lblDEUsernameTip2b.Visible = True
        Me.lblDEUsernameTip2c.Visible = True
        Me.tdDEUsernameTip.Style.Add("display", "initial")
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.txtDEloginID.BackColor = Drawing.Color.White
        Me.txtDEloginID.Enabled = True
        Me.txtDEloginID.Text = String.Empty

        'Me.ddlDEDefaultPrintOption.Enabled = True
        clearSelectedPractice()

        Me.chkPracticeList.Enabled = True
        Me.chkDESuspend.Enabled = False
        Me.chkDESuspend.Checked = False
        Me.chkDEAccountLocked.Enabled = False
        Me.chkDEAccountLocked.Checked = False
        'Me.InitDataEntryMaintenancePrintOptionList(True, "", True)

        'Me.ddlDEDefaultPrintOption.SelectedValue = String.Empty

        Me.lblDELoginID.Text = String.Empty
        Me.txtDEloginID.Text = String.Empty
        Me.chkChgDEPWD.Checked = True
        Me.chkChgDEPWD.Visible = False
        Me.chkChgDEPWD_CheckedChanged(Nothing, Nothing)
        Me.txtDENewPWD.Text = String.Empty
        Me.txtDEConfirmNewPWD.Text = String.Empty

        Me.btnAddDEAcct.Visible = False
        Me.btnEditDEAcct.Visible = False
        Me.btnSaveDEAcct.Visible = True
        Me.btnCancelDEAcct.Visible = True

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00051, AublitLogDescription.DataEntryAccountAddClick)
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
    End Sub

    Protected Sub btnSaveDEAcct_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strDEloginID As String = String.Empty
        Dim strPracticeSelection As String = String.Empty
        Dim strStatus As String = String.Empty
        Dim strNewPWD As String = String.Empty
        Dim strConfirmNewPWD As String = String.Empty
        Dim strPrintOption As String = String.Empty
        Dim blnErr As Boolean = False
        Dim blnSaveSuccess As Boolean = False
        Dim sm As SystemMessage
        Dim i As Integer = 0
        Dim strlogcode, strlogsuccess, strlogfail As String
        Dim strLogDesc As String = String.Empty
        Dim strAcctLocked As String = String.Empty

        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False

        Me.imgDELoginIDError.Visible = False
        Me.imgDEPracticeError.Visible = False
        Me.imgDENewPWDError.Visible = False
        Me.imgDEConfirmNewPWDError.Visible = False
        'Me.imgDefaultPritingOptionError.Visible = False

        udtUserAC = UserACBLL.GetUserAC
        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
            udtSP = CType(udtUserAC, ServiceProviderModel)
        End If

        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        strDEloginID = AntiXssEncoder.HtmlEncode(txtDEloginID.Text, True)
        ' I-CRE16-003 Fix XSS [End][Lawrence]

        If Me.txtDEloginID.Visible Then
            strlogcode = Common.Component.LogID.LOG00010
            strLogDesc = AublitLogDescription.CreateDataEntryAccount
            strlogsuccess = Common.Component.LogID.LOG00011
            strlogfail = Common.Component.LogID.LOG00012
        Else
            strlogcode = Common.Component.LogID.LOG00013
            strLogDesc = AublitLogDescription.SPUpdateDataEntryAccount
            strlogsuccess = Common.Component.LogID.LOG00014
            strlogfail = Common.Component.LogID.LOG00015
        End If
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)  ''Begin Writing Audit Log
        udtAuditLogEntry.AddDescripton("SP_ID", udtSP.SPID)
        udtAuditLogEntry.AddDescripton("Data Entry Login ID", strDEloginID)
        udtAuditLogEntry.WriteStartLog(strlogcode, strLogDesc)

        If Not Me.chkDESuspend.Enabled Then
            If Not udtDataEntryAcctBLL.chkIsEmpty(strDEloginID) Then
                If udtDataEntryAcctBLL.chkValidLoginID(strDEloginID) Then
                    If udtDataEntryAcctBLL.chkDuplicateDataEntryAcct(udtSP.SPID, strDEloginID) Then
                        blnErr = True
                        sm = New SystemMessage(strFuncCode, "E", "00001")
                        Me.imgDELoginIDError.Visible = True
                        Me.udcErrMsgBox.AddMessage(sm, "%s", strDEloginID, True)
                    End If
                Else
                    blnErr = True
                    sm = New SystemMessage(strFuncCode, "E", "00019")
                    Me.imgDELoginIDError.Visible = True
                    Me.udcErrMsgBox.AddMessage(sm)
                End If
            Else
                blnErr = True
                sm = New SystemMessage(strFuncCode, "E", "00018")
                Me.imgDELoginIDError.Visible = True
                Me.udcErrMsgBox.AddMessage(sm)
            End If
        End If

        If Me.chkPracticeList.Visible = True Then
            For i = 0 To Me.chkPracticeList.Items.Count - 1
                If Me.chkPracticeList.Items(i).Selected Then
                    strPracticeSelection = strPracticeSelection + chkPracticeList.Items(i).Value.ToString + ","
                End If
            Next

            If strPracticeSelection.Trim.Equals(String.Empty) Then
                blnErr = True
                sm = New SystemMessage(strFuncCode, "E", "00002")
                Me.imgDEPracticeError.Visible = True
                Me.udcErrMsgBox.AddMessage(sm)
            End If

        End If

        If Me.chkChgDEPWD.Checked Then
            strNewPWD = Me.txtDENewPWD.Text
            strConfirmNewPWD = Me.txtDEConfirmNewPWD.Text

            If Not udtDataEntryAcctBLL.chkIsEmpty(strNewPWD) Then
                If udtDataEntryAcctBLL.chkValidPassword(strNewPWD) Then
                    If Not udtDataEntryAcctBLL.chkIsIdenticalPassword(strNewPWD, strConfirmNewPWD) Then
                        blnErr = True
                        sm = New SystemMessage(strFuncCode, "E", "00004")
                        Me.imgDEConfirmNewPWDError.Visible = True
                        Me.udcErrMsgBox.AddMessage(sm)
                    End If
                Else
                    blnErr = True
                    sm = New SystemMessage(strFuncCode, "E", "00003")
                    Me.imgDENewPWDError.Visible = True
                    Me.udcErrMsgBox.AddMessage(sm)
                End If
            Else
                blnErr = True
                sm = New SystemMessage(strFuncCode, "E", "00010")
                Me.imgDENewPWDError.Visible = True
                Me.udcErrMsgBox.AddMessage(sm)
            End If

        End If

        'validate print option
        'If Session(IncludePrintingEnabledScheme) Is Nothing Then
        '    Session(IncludePrintingEnabledScheme) = udtSPProfileBLL.IncludePrintingEnabledScheme(udtSP.SPID)
        'End If
        'If Session(IncludePrintingEnabledScheme) Then
        '    strPrintOption = GetDataEntryMaintenancePrintOptionSelectedValue()
        '    Dim validator As Common.Validation.Validator = New Common.Validation.Validator
        '    sm = validator.chkSelectedPrintFormOption(strPrintOption)
        '    If Not sm Is Nothing Then
        '        blnErr = True
        '        Me.imgDefaultPritingOptionError.Visible = True
        '        Me.udcErrMsgBox.AddMessage(sm)
        '    End If
        'End If

        If Me.chkDESuspend.Checked Then
            strStatus = "I"
        Else
            strStatus = "A"
        End If

        If Me.chkDEAccountLocked.Checked Then
            strAcctLocked = "Y"
        Else
            strAcctLocked = "N"
        End If

        If Not blnErr Then
            If Me.chkDESuspend.Enabled Then
                blnSaveSuccess = udtDataEntryAcctBLL.saveDataEntryAcct(udtSP.SPID, txtDEloginID.Text.Trim, strNewPWD, strStatus, strPracticeSelection, strAcctLocked)
            Else
                strPrintOption = udtSP.PrintOption.Trim()
                blnSaveSuccess = udtDataEntryAcctBLL.addDataEntryAcct(udtSP.SPID, txtDEloginID.Text.Trim, strNewPWD, strPracticeSelection, strPrintOption)
            End If

            If blnSaveSuccess Then


                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.lblDELoginID.Text = AntiXssEncoder.HtmlEncode(txtDEloginID.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
                Me.lblDELoginID.Visible = True
                Me.txtDEloginID.Visible = False
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                udtAuditLogEntry.AddDescripton("Data Entry Account Login ID", strDEloginID)
                udtAuditLogEntry.AddDescripton("Practice", strPracticeSelection)
                udtAuditLogEntry.AddDescripton("Default Printing Option", strPrintOption)
                If Me.chkDESuspend.Enabled Then

                    sm = New SystemMessage(strFuncCode, "I", "00002")
                    Me.udcInfoMsgBox.AddMessage(sm)
                    'udtAuditLogEntry.WriteLog(strFuncCode, Common.Component.MsgCode.MSG00009, "Update Data Entry Account Profile fail")
                    udtAuditLogEntry.AddDescripton("Password Updated", Me.chkChgDEPWD.Checked)
                    udtAuditLogEntry.WriteEndLog(strlogsuccess, AublitLogDescription.SPUpdateDataEntryAccountSuccess)
                Else
                    sm = New SystemMessage(strFuncCode, "I", "00001")
                    Me.udcInfoMsgBox.AddMessage(sm)
                    'udtAuditLogEntry.WriteLog(strFuncCode, Common.Component.MsgCode.MSG00008, "Create Data Entry Account Profile success")
                    udtAuditLogEntry.WriteEndLog(strlogsuccess, AublitLogDescription.CreateDataEntryAccountSuccess)
                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Me.tbDEFilter.Visible = True
                Me.trUsername.Visible = True
                Me.trUsernameFound.Visible = False
                Me.trUsernameNotFound.Visible = False

                If Not udtDataEntryAcctBLL.chkIsEmpty(txtDEFilter.Text) Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    DEAcctFilter(AntiXssEncoder.HtmlEncode(txtDEFilter.Text, True))
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                Else
                    Dim dtDataEntryAcct As DataTable
                    dtDataEntryAcct = udtDataEntryAcctBLL.getDataEntryAcct(udtSP.SPID)

                    Session(DataEntryAcct) = dtDataEntryAcct
                    Me.GridViewDataBind(Me.gvdataEntryAcc, dtDataEntryAcct)

                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Overwrite the Base GridView
                    Me.gvdataEntryAcc.PageSize = 20
                    Me.gvdataEntryAcc.DataSource = dtDataEntryAcct
                    Me.gvdataEntryAcc.DataBind()
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                End If

                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                'Reset Controls 
                'Me.btnCancelDEAcct_Click(Nothing, Nothing)
                ResetControl(True)
                ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

                Me.udcInfoMsgBox.BuildMessageBox()

            End If
        End If

        If (strlogfail = Common.Component.LogID.LOG00012) Then
            Me.udcErrMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, strlogfail, AublitLogDescription.CreateDataEntryAccountFail)
        Else
            Me.udcErrMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, strlogfail, AublitLogDescription.SPUpdateDataEntryAccountFail)
        End If
    End Sub

    Protected Sub btnCancelDEAcct_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        'udcErrMsgBox.Clear()
        'Dim dt As DataTable
        'Dim strStatus As String = String.Empty
        'Dim strAcctLocked As String = String.Empty
        '
        'Me.udcErrMsgBox.Visible = False
        'Me.udcInfoMsgBox.Visible = False
        '
        'Me.imgDELoginIDError.Visible = False
        'Me.imgDEPracticeError.Visible = False
        'Me.imgDENewPWDError.Visible = False
        'Me.imgDEConfirmNewPWDError.Visible = False
        ''Me.imgDefaultPritingOptionError.Visible = False
        '
        ''Me.txtDEloginID.Text = String.Empty
        '
        'Me.txtDENewPWD.Enabled = False
        'Me.txtDEConfirmNewPWD.Enabled = False
        '
        'Me.txtDEloginID.Enabled = False
        'Me.lblDEUsernameTip.Visible = True
        'Me.lblDEUsernameTip1.Visible = True
        ''CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        ''-----------------------------------------------------------------------------------------
        'Me.lblDEUsernameTip2.Visible = True
        'Me.lblDEUsernameTip2a.Visible = True
        'Me.lblDEUsernameTip2b.Visible = True
        'Me.lblDEUsernameTip2c.Visible = True
        ''CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        '
        'Me.txtDEloginID.BackColor = Drawing.Color.LightGray
        'Me.gvdataEntryAcc.Enabled = True
        '
        ''CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'InitDEFilterControl(True)
        ''CRE13-019-02 Extend HCVS to China [End][Winnie]
        '
        ''print option
        ''Me.lblDEPrintOptionRemind.Visible = False
        '
        'Me.chkChgDEPWD.Checked = False
        'Me.chkChgDEPWD.Visible = False
        'Me.chkChgDEPWD_CheckedChanged(Nothing, Nothing)
        '
        'Me.txtDENewPWD.Text = String.Empty
        'Me.txtDEConfirmNewPWD.Text = String.Empty
        '
        'Me.chkChgDEPWD.Enabled = False
        'Me.chkPracticeList.Enabled = False
        '
        ''Me.ddlDEDefaultPrintOption.Enabled = False
        '
        ''Me.btnAddDEAcct.Visible = True
        'Me.btnAddDEAcct.Visible = Me.chkPracticeList.Visible
        'Me.btnEditDEAcct.Visible = False
        'Me.btnSaveDEAcct.Visible = False
        'Me.btnCancelDEAcct.Visible = False
        '
        'Me.chkDESuspend.Enabled = False
        'clearSelectedPractice()
        '
        'Me.chkDEAccountLocked.Enabled = False
        'Me.chkDEAccountLocked.Checked = False
        '
        'If Not Me.lblDELoginID.Text.Trim.Equals(String.Empty) Then
        '    udtUserAC = UserACBLL.GetUserAC
        '    If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
        '        udtSP = CType(udtUserAC, ServiceProviderModel)
        '    End If
        '
        '    Me.chkChgDEPWD.Visible = True
        '    Me.chkChgDEPWD.Checked = False
        '    Me.chkChgDEPWD_CheckedChanged(Nothing, Nothing)
        '
        '    Me.lblDEUsernameTip.Visible = False
        '    Me.lblDEUsernameTip1.Visible = False
        '    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '    '-----------------------------------------------------------------------------------------
        '    Me.lblDEUsernameTip2.Visible = False
        '    Me.lblDEUsernameTip2a.Visible = False
        '    Me.lblDEUsernameTip2b.Visible = False
        '    Me.lblDEUsernameTip2c.Visible = False
        '    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        '
        '
        '    dt = udtDataEntryAcctBLL.getDataEntryAcctDetails(udtSP.SPID, txtDEloginID.Text.Trim)
        '
        '    strStatus = dt.Rows(0).Item("Record_Status").ToString
        '    strAcctLocked = dt.Rows(0).Item("Account_Locked").ToString
        '
        '    Me.chkDESuspend.Enabled = False
        '    If strStatus = "A" Then
        '        Me.chkDESuspend.Checked = False
        '    Else
        '        Me.chkDESuspend.Checked = True
        '    End If
        '
        '    Me.chkDEAccountLocked.Enabled = False
        '    If strAcctLocked = "Y" Then
        '        Me.chkDEAccountLocked.Checked = True
        '    Else
        '        Me.chkDEAccountLocked.Checked = False
        '    End If
        '
        '    For Each dr As DataRow In dt.Rows
        '        If dr.Item("Practice_Status") = "A" Then
        '            If dr.Item("SP_Practice_Display_Seq") > 0 Then
        '                Me.chkPracticeList.Items.FindByValue(dr.Item("SP_Practice_Display_Seq").ToString + "-" + dr.Item("SP_Bank_Acc_display_Seq").ToString).Selected = True
        '            End If
        '        End If
        '    Next
        '
        '    Me.btnEditDEAcct.Visible = True
        '
        '    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        '    Dim intPasswordLevel As Integer = dt.Rows(0).Item("Password_Level")
        '    displayInfoResetPWMsgBox(intPasswordLevel)
        '    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
        '
        '
        '    'reset print option value
        '    'If dt.Rows(0).Item("ConsentPrintOption") Is DBNull.Value Then
        '    '    Dim strConsentPrintOption As String
        '    '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        '    '    udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
        '
        '    '    Me.InitDataEntryMaintenancePrintOptionList(False, strConsentPrintOption, False)
        '    '    'Me.ddlDEDefaultPrintOption.SelectedValue = strConsentPrintOption
        '    'Else
        '
        '    '    Me.InitDataEntryMaintenancePrintOptionList(False, dt.Rows(0).Item("ConsentPrintOption"), False)
        '    '    'Me.ddlDEDefaultPrintOption.SelectedValue = 
        '    'End If
        'Else
        '    Me.txtDEloginID.Text = String.Empty
        '    'Me.InitDataEntryMaintenancePrintOptionList(False, "", True)
        '    'reset print option value
        '    'Me.ddlDEDefaultPrintOption.SelectedValue = String.Empty
        'End If

        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        If Me.chkDESuspend.Enabled Then
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00054, AublitLogDescription.DataEntryAccountEditCancelClick)
        Else
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00052, AublitLogDescription.DataEntryAccountAddCancelClick)
        End If
        ResetControl(False)
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

    End Sub

    Protected Sub btnEditDEAcct_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)


        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Dim dt As DataTable
        Dim strDataEntryAcct As String = Me.lblDELoginID.Text

        udtUserAC = UserACBLL.GetUserAC
        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
            udtSP = CType(udtUserAC, ServiceProviderModel)
        End If

        dt = udtDataEntryAcctBLL.getDataEntryAcctDetails(udtSP.SPID, strDataEntryAcct)
        Dim intPasswordLevel As Integer = dt.Rows(0).Item("Password_Level")
        displayInfoResetPWMsgBox(intPasswordLevel)
        'udcInfoMsgBox.Visible = False
        Me.udcErrMsgBox.Visible = False
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        Me.txtDEloginID.Enabled = False
        Me.txtDEloginID.Visible = False
        Me.lblDEUsernameTip.Visible = False
        Me.lblDEUsernameTip1.Visible = False
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDEUsernameTip2.Visible = False
        Me.lblDEUsernameTip2a.Visible = False
        Me.lblDEUsernameTip2b.Visible = False
        Me.lblDEUsernameTip2c.Visible = False
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'Me.ddlDEDefaultPrintOption.Enabled = False
        Me.gvdataEntryAcc.Enabled = False
        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        InitDEFilterControl(False)
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
        Me.chkPracticeList.Enabled = True
        Me.chkChgDEPWD.Enabled = True
        Me.chkDESuspend.Enabled = True
        'Me.ddlDEDefaultPrintOption.Enabled = True
        If Me.chkDEAccountLocked.Checked Then
            Me.chkDEAccountLocked.Enabled = True
        Else
            Me.chkDEAccountLocked.Enabled = False
        End If

        Me.btnAddDEAcct.Visible = False
        Me.btnEditDEAcct.Visible = False
        Me.btnSaveDEAcct.Visible = True
        Me.btnCancelDEAcct.Visible = True

        'Show Print Option Remind
        'Me.InitDataEntryMaintenancePrintOptionList(True, "", False)

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00053, AublitLogDescription.DataEntryAccountEditClick)
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
    End Sub
#End Region

#Region "Pop-up related functions"

    Protected Sub btnSchemeNameHelp_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        popupSchemeNameHelp.Show()

        udcSchemeLegend.ShowFilteredSubsidy = False
        udcSchemeLegend.ShowScheme = True
        udcSchemeLegend.ShowSubsidy = False
        udcSchemeLegend.SchemeLegendSubPlatform = Me.SubPlatform
        udcSchemeLegend.BindSchemeClaim(Session("language"), Me.SubPlatform)

    End Sub

    Protected Sub btnSubsidyHelp_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        popupSchemeNameHelp.Show()

        udcSchemeLegend.ShowFilteredSubsidy = False
        udcSchemeLegend.ShowScheme = False
        udcSchemeLegend.ShowSubsidy = True
        udcSchemeLegend.SchemeLegendSubPlatform = Me.SubPlatform
        udcSchemeLegend.BindSchemeClaim(Session("language"), Me.SubPlatform)

    End Sub

    Protected Sub ibtnCloseSchemeNameHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseSchemeNameHelp.Click
        popupSchemeNameHelp.Hide()
    End Sub

#End Region

#Region "Setup Controls and contructs data in controls"
    'Practice Check list in Data Entry Account Tab
    Protected Sub constructPracticeList(ByVal udtSP As ServiceProviderModel)
        Dim i As Integer = 0
        Me.chkPracticeList.Items.Clear()

        For Each udcPractice As Common.Component.Practice.PracticeModel In udtSP.PracticeList.Values
            If udcPractice.RecordStatus = Common.Component.PracticeStatus.Active Then

                Dim strPracticeName As String = String.Empty
                Dim strPracticeNameChi As String = String.Empty

                strPracticeName = udcPractice.PracticeName.Trim()
                strPracticeNameChi = udcPractice.PracticeNameChi.Trim()

                If strPracticeNameChi.Trim() = "" Then
                    strPracticeNameChi = strPracticeName
                End If

                If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
                    Me.chkPracticeList.Items.Add(strPracticeNameChi)
                    Me.chkPracticeList.Items(i).Text = strPracticeNameChi
                Else
                    Me.chkPracticeList.Items.Add(strPracticeName)
                    Me.chkPracticeList.Items(i).Text = strPracticeName
                End If

                'Me.chkPracticeList.Items.Add(udcPractice.PracticeName)
                'Me.chkPracticeList.Items(i).Text = udcPractice.PracticeName
                Me.chkPracticeList.Items(i).Value = udcPractice.DisplaySeq.ToString + "-" + udcPractice.BankAcct.DisplaySeq.ToString
                i = i + 1
            End If
        Next

        If i = 0 Then
            Me.lblDENoPractice.Visible = True
            Me.chkPracticeList.Visible = False
            Me.btnAddDEAcct.Visible = False
        Else
            Me.lblDENoPractice.Visible = False
            Me.chkPracticeList.Visible = True
        End If

    End Sub

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Protected Sub InitDEFilterControl(ByVal blnEnable As Boolean)
        Me.imgDEFilterError.Visible = False

        If blnEnable Then
            Me.txtDEFilter.Enabled = True
            Me.btnFilterDEAcct.Enabled = True
            Me.btnFilterDEAcct.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "FilterDEAcctBtn")
            Me.btnClearDEAcctSearch.Enabled = True
            Me.btnClearDEAcctSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDEAcctSearchBtn")
        Else
            Me.txtDEFilter.Enabled = False
            Me.btnFilterDEAcct.Enabled = False
            Me.btnFilterDEAcct.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "FilterDEAcctDisableBtn")
            Me.btnClearDEAcctSearch.Enabled = False
            Me.btnClearDEAcctSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDEAcctSearchDisableBtn")
        End If
    End Sub

    Protected Sub DEAcctFilter(ByVal strFilter As String)

        Dim dtDataEntryAcct As DataTable = Nothing
        Dim dtDataEntryAcctFilter As DataTable = Nothing

        Me.trUsername.Visible = False

        udtSP = Me.udtServiceProviderBLL.GetSP()

        dtDataEntryAcct = udtDataEntryAcctBLL.getDataEntryAcct(udtSP.SPID)

        If dtDataEntryAcct.Rows.Count > 0 Then
            'Search
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            Dim drDataEntryAcctFilter As DataRow() = dtDataEntryAcct.Select("Data_Entry_Account Like '%" + strFilter + "%'")
            ' I-CRE16-003 Fix XSS [End][Lawrence]
            If drDataEntryAcctFilter.Length > 0 Then
                dtDataEntryAcctFilter = drDataEntryAcctFilter.CopyToDataTable()
                Me.GridViewDataBind(Me.gvdataEntryAcc, dtDataEntryAcctFilter)

                Me.trUsernameFound.Visible = True
                Me.lblUsernameFound.Text = Me.GetGlobalResourceObject("Text", "UsernameContains")
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.lblUsernameFound.Text = Me.lblUsernameFound.Text.Replace("%s", strFilter)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            Else
                'No Result            
                Me.trUsernameNotFound.Visible = True
                Me.lblUsernameNotFound.Text = Me.GetGlobalResourceObject("Text", "UsernameContainsNotFound")
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.lblUsernameNotFound.Text = Me.lblUsernameNotFound.Text.Replace("%s", strFilter)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If

            Session(DataEntryAcct) = dtDataEntryAcctFilter
            'Overwrite the Base GridView
            Me.gvdataEntryAcc.PageSize = 20
            Me.gvdataEntryAcc.DataSource = dtDataEntryAcctFilter
            Me.gvdataEntryAcc.DataBind()
        End If

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Protected Sub clearSelectedPractice()
        Dim i As Integer = 0
        For i = 0 To Me.chkPracticeList.Items.Count - 1
            Me.chkPracticeList.Items(i).Selected = False
        Next
    End Sub

    Private Sub InitDataEntryChangePasswordControl(ByVal blnEnable As Boolean)
        Me.txtDEOLDPassword.Enabled = blnEnable
        Me.txtDENEWPassword.Enabled = blnEnable
        Me.txtDEConfirmNEWPassword.Enabled = blnEnable

        If blnEnable Then
            Me.PanelDEChangeWebPasswordSettings.CssClass = "SettingsPanelStyle"
            Me.PanelDEChangeWebPassword01.Style("display") = "block"
            Me.PanelDEChangeWebPassword02.Style("display") = "block"
            Me.PanelDEChangeWebPasswordTips.Style("display") = "block"

            Me.tdDEChgPwdCell01.Attributes("class") = "SettingTDRightStyle"
            Me.tdDEChgPwdCell02.Attributes("class") = "SettingTDRightStyle"
            Me.tdDEChgPwdCell03.Attributes("class") = "SettingTDRightStyle"

            Me.txtDEOLDPassword.BackColor = Drawing.Color.White
            Me.txtDENEWPassword.BackColor = Drawing.Color.White
            Me.txtDEConfirmNEWPassword.BackColor = Drawing.Color.White
        Else
            Me.PanelDEChangeWebPasswordSettings.CssClass = ""
            Me.PanelDEChangeWebPassword01.Style("display") = "none"
            Me.PanelDEChangeWebPassword02.Style("display") = "none"
            Me.PanelDEChangeWebPasswordTips.Style("display") = "none"

            Me.tdDEChgPwdCell01.Attributes("class") = ""
            Me.tdDEChgPwdCell02.Attributes("class") = ""
            Me.tdDEChgPwdCell03.Attributes("class") = ""

            Me.txtDEOLDPassword.BackColor = Drawing.Color.LightGray
            Me.txtDENEWPassword.BackColor = Drawing.Color.LightGray
            Me.txtDEConfirmNEWPassword.BackColor = Drawing.Color.LightGray
        End If
    End Sub

    Private Sub InitDataEntryProfileControl()
        ' Removed @ 2009-Feb-24
        'Me.ddlDEChangePrintOption.Enabled = False
        Me.chkDEChangePassword.Checked = False
        Me.chkDEChangePassword.Enabled = False
        ' Removed @ 2009-Feb-24

        Me.InitDataEntryChangePasswordControl(False)

        'Print Option 
        ' Removed @ 2009-Feb-24
        Me.InitLoginUserPrintOptionList(False)
        'Me.ddlDEChangePrintOption.SelectedValue = Me.getUserPrintOption()
        'Me.lblDEChangePrintOptionRemind.Visible = False

        'button
        Me.btnDEEdit.Visible = True
        Me.btnDESave.Visible = False
        Me.btnDECancel.Visible = False
    End Sub

    Private Sub InitDataEntryEditProfileControl()
        ' Removed @ 2009-Feb-24
        'Me.ddlDEChangePrintOption.Enabled = True
        Me.chkDEChangePassword.Enabled = True
        Me.InitDataEntryChangePasswordControl(False)
        'Me.ShowPrintOptionRemindText()

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.imgSPSelectPrintOptionError.Visible = False

        'Print Option List
        'Me.EnablePrintOptionList()
        Me.EnablePrintOptionList(Me.lblSPID.Text.Trim, Me.lblDEUsername.Text.Trim)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'button
        Me.btnDEEdit.Visible = False
        Me.btnDESave.Visible = True
        Me.btnDECancel.Visible = True
    End Sub

    Protected Sub changeIVRSPwd(ByVal strChecked As Boolean)
        If strChecked Then
            Me.PanelIVRSPasswordSettings.CssClass = "SettingsPanelStyle"
            Me.PanelIVRSPassword01.Style("display") = "block"
            Me.PanelIVRSPassword02.Style("display") = "block"
            Me.PanelIVRSPasswordTips.Style("display") = "block"
            Me.tdIVRSPwdCell01.Attributes("class") = "SettingTDRightStyle"
            Me.tdIVRSPwdCell02.Attributes("class") = "SettingTDRightStyle"
            Me.tdIVRSPwdCell03.Attributes("class") = "SettingTDRightStyle"

            Me.txtIVRSOldPwd.Enabled = True
            Me.txtIVRSNewPwd.Enabled = True
            Me.txtConfirmNewIVRSPwd.Enabled = True
            Me.txtIVRSOldPwd.BackColor = Drawing.Color.White
            Me.txtIVRSNewPwd.BackColor = Drawing.Color.White
            Me.txtConfirmNewIVRSPwd.BackColor = Drawing.Color.White
        Else
            Me.PanelIVRSPasswordSettings.CssClass = ""
            Me.PanelIVRSPassword01.Style("display") = "none"
            Me.PanelIVRSPassword02.Style("display") = "none"
            Me.PanelIVRSPasswordTips.Style("display") = "none"

            Me.tdIVRSPwdCell01.Attributes("class") = ""
            Me.tdIVRSPwdCell02.Attributes("class") = ""
            Me.tdIVRSPwdCell03.Attributes("class") = ""


            Me.txtIVRSOldPwd.Enabled = False
            Me.txtIVRSNewPwd.Enabled = False
            Me.txtConfirmNewIVRSPwd.Enabled = False
            Me.txtIVRSOldPwd.BackColor = Drawing.Color.LightGray
            Me.txtIVRSNewPwd.BackColor = Drawing.Color.LightGray
            Me.txtConfirmNewIVRSPwd.BackColor = Drawing.Color.LightGray
        End If
    End Sub
#End Region

#Region "Formatting functions"
    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return udtformatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
    End Function

    Protected Function formatAddressChi(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return formatChineseString(udtformatter.formatAddressChi(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea))
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        If IsNothing(strChineseString) Then
            Return ""
        Else
            Return udtformatter.formatChineseName(strChineseString.ToString.Trim)
        End If
    End Function
#End Region

#Region "Get Functions (mainly for screen display)"
    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If IsNothing(strPracticeCode) Then
            strPracticeTypeName = String.Empty
        Else
            If strPracticeCode.Equals(String.Empty) Then
                strPracticeTypeName = String.Empty
            Else
                If Session("language") = CultureLanguage.TradChinese Then
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValueChi
                ElseIf Session("language") = CultureLanguage.SimpChinese Then
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValueCN
                Else
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValue
                End If
            End If
        End If
        Return strPracticeTypeName

    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        Dim strHealthProfName As String

        If IsNothing(strHealthProfCode) Then
            strHealthProfName = String.Empty
        Else
            If strHealthProfCode.Equals(String.Empty) Then
                strHealthProfName = String.Empty
            Else

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------

                If Session("language") = CultureLanguage.TradChinese Then
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
                ElseIf Session("language") = CultureLanguage.SimpChinese Then
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescCN
                Else
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc

                End If

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            End If
        End If

        Return strHealthProfName
    End Function

    Protected Function GetMOName(ByVal strMODisplaySeq As String) As String
        Dim strMOName As String
        Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL

        If IsNothing(strMODisplaySeq) Then
            strMOName = String.Empty
        Else
            If strMODisplaySeq.Equals(String.Empty) Then
                strMOName = String.Empty
            Else
                If CInt(strMODisplaySeq.Trim) > 0 Then
                    strMOName = udtMOBLL.GetMOName(CInt(strMODisplaySeq.Trim))
                Else
                    strMOName = String.Empty
                End If
            End If
        End If

        Return strMOName
    End Function

    Protected Function GetChiMOName(ByVal strMODisplaySeq As String) As String
        Dim strMOName As String
        Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL

        If IsNothing(strMODisplaySeq) Then
            strMOName = String.Empty
        Else
            If strMODisplaySeq.Equals(String.Empty) Then
                strMOName = String.Empty
            Else
                If CInt(strMODisplaySeq.Trim) > 0 Then
                    strMOName = udtMOBLL.GetMOChiName(CInt(strMODisplaySeq.Trim))
                Else
                    strMOName = String.Empty
                End If
            End If
        End If

        Return strMOName
    End Function

#End Region

#Region "All Printing Option related functions (Including control setup, messages, etc.)"

    Private Sub InitLoginUserPrintOptionList(ByVal blnInitAll As Boolean)

        If blnInitAll Then
            Me.rbNotPrint.Enabled = False
            Me.rbPrintFull.Enabled = False
            Me.rbPrintCondensed.Enabled = False

            Me.rbDEChangeNotPrint.Enabled = False
            Me.rbDEChangePrintFull.Enabled = False
            Me.rbDEChangePrintCondensed.Enabled = False

            Me.SetPrintOptionSelectedValue(Me.getUserPrintOption())
        Else

            Dim noPrintRadioButton As RadioButton = Nothing
            Dim printFullRadioButton As RadioButton = Nothing
            Dim printCondenceRadioButton As RadioButton = Nothing

            Me.GetPrintOptionRadionButtons(noPrintRadioButton, printFullRadioButton, printCondenceRadioButton)

            noPrintRadioButton.Enabled = False
            printFullRadioButton.Enabled = False
            printCondenceRadioButton.Enabled = False

            Me.SetPrintOptionSelectedValue(Me.getUserPrintOption())
        End If
    End Sub

    'Control Print Option Remind Message
    Private Function getUserPrintOption() As String
        Dim strPrintOption As String = ""
        udtUserAC = UserACBLL.GetUserAC
        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
            udtSP = Me.udtServiceProviderBLL.GetSP()
            strPrintOption = udtSP.PrintOption
        Else
            udtDataEntry = Me.udtDataEntryUserBLL.GetDataEntry()
            strPrintOption = udtDataEntry.PrintOption
        End If
        Return strPrintOption
    End Function

    Private Sub GetPrintOptionRadionButtons(ByRef noPrintRadioButton As RadioButton, ByRef printFullRadioButton As RadioButton, ByRef printCondenceRadioButton As RadioButton)
        udtUserAC = UserACBLL.GetUserAC
        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
            'Service Provider account settigns
            noPrintRadioButton = Me.rbNotPrint
            printFullRadioButton = Me.rbPrintFull
            printCondenceRadioButton = Me.rbPrintCondensed
        ElseIf udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
            noPrintRadioButton = Me.rbDEChangeNotPrint
            printFullRadioButton = Me.rbDEChangePrintFull
            printCondenceRadioButton = Me.rbDEChangePrintCondensed
        End If
    End Sub

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub GetPrintOptionTableRow(ByRef printOptionSettingPanel As Panel, ByRef printFullTableRow As TableRow, ByRef printCondensedTableRow As TableRow)
        udtUserAC = UserACBLL.GetUserAC
        If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
            'Service Provider account settigns
            printFullTableRow = Me.trPrintFullVersion
            printCondensedTableRow = Me.trPrintCondensed
            printOptionSettingPanel = Me.PanelPrintingOptionSetting
        ElseIf udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
            printFullTableRow = Me.trDEPrintFullVersion
            printCondensedTableRow = Me.trDEPrintCondensed
            printOptionSettingPanel = Me.PanelDataEntryPrintingOptionSetting
        End If
    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub EnablePrintOptionList(ByVal strSPID As String, Optional ByVal strDataEntryAccount As String = "")
        Dim printOptionSettingPanel As Panel = Nothing
        Dim noPrintRadioButton As RadioButton = Nothing
        Dim printFullRadioButton As RadioButton = Nothing
        Dim printCondenceRadioButton As RadioButton = Nothing
        Dim printFullVersionTableRow As TableRow = Nothing
        Dim printCondensedTableRow As TableRow = Nothing

        Me.GetPrintOptionRadionButtons(noPrintRadioButton, printFullRadioButton, printCondenceRadioButton)
        Me.GetPrintOptionTableRow(printOptionSettingPanel, printFullVersionTableRow, printCondensedTableRow)

        If Session(IncludePrintingEnabledScheme) Is Nothing Then
            If Not strDataEntryAccount.Equals(String.Empty) Then
                Session(IncludePrintingEnabledScheme) = Me.udtConsentFormPrintOptionBLL.CheckPrintOptionEnabled(strSPID, Me.SubPlatform, strDataEntryAccount)
            Else
                'Session(IncludePrintingEnabledScheme) = udtSPProfileBLL.IncludePrintingEnabledScheme(udtSP.SPID)
                Session(IncludePrintingEnabledScheme) = Me.udtConsentFormPrintOptionBLL.CheckPrintOptionEnabled(strSPID, Me.SubPlatform)
            End If
        End If

        If Not Session(IncludePrintingEnabledScheme) Is Nothing Then
            printOptionSettingPanel.Visible = True
            Select Case Session(IncludePrintingEnabledScheme)
                Case PrintOption.PrintOption_BothVersion
                    printFullVersionTableRow.Style.Add("display", "default")
                    printCondensedTableRow.Style.Add("display", "default")
                    printFullRadioButton.Enabled = True
                    printCondenceRadioButton.Enabled = True
                Case PrintOption.PrintOption_FullVersion
                    printFullVersionTableRow.Style.Add("display", "default")
                    printCondensedTableRow.Style.Add("display", "none")
                    printFullRadioButton.Enabled = True
                Case PrintOption.PrintOption_CondensedVersion
                    printFullVersionTableRow.Style.Add("display", "none")
                    printCondensedTableRow.Style.Add("display", "default")
                    printCondenceRadioButton.Enabled = True
            End Select
            noPrintRadioButton.Enabled = True
        Else
            printOptionSettingPanel.Visible = False
        End If
    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    Public Sub SetPrintOptionSelectedValue(ByVal strSelectedValue As String)
        Dim noPrintRadioButton As RadioButton = Nothing
        Dim printFullRadioButton As RadioButton = Nothing
        Dim printCondenceRadioButton As RadioButton = Nothing

        Me.GetPrintOptionRadionButtons(noPrintRadioButton, printFullRadioButton, printCondenceRadioButton)

        noPrintRadioButton.Checked = False
        printFullRadioButton.Checked = False
        printCondenceRadioButton.Checked = False

        Select Case strSelectedValue
            Case PrintFormOptionValue.PreprintForm
                noPrintRadioButton.Checked = True
            Case PrintFormOptionValue.PrintPurposeAndConsent
                printFullRadioButton.Checked = True
            Case PrintFormOptionValue.PrintConsentOnly
                printCondenceRadioButton.Checked = True
        End Select

    End Sub

    Public Function GetPrintOptionSelectedValue() As String
        Dim noPrintRadioButton As RadioButton = Nothing
        Dim printFullRadioButton As RadioButton = Nothing
        Dim printCondenceRadioButton As RadioButton = Nothing

        Me.GetPrintOptionRadionButtons(noPrintRadioButton, printFullRadioButton, printCondenceRadioButton)

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If noPrintRadioButton.Checked And noPrintRadioButton.Enabled = True Then
            Return PrintFormOptionValue.PreprintForm
        ElseIf printFullRadioButton.Checked And printFullRadioButton.Enabled = True Then
            Return PrintFormOptionValue.PrintPurposeAndConsent
        ElseIf printCondenceRadioButton.Checked And printCondenceRadioButton.Enabled = True Then
            Return PrintFormOptionValue.PrintConsentOnly
        Else
            Return ""
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    End Function
#End Region

    Protected Sub RerenderLanguage()
        Dim i As Integer

        Dim selectLanguage As String = Me.ddlDefaultLang.SelectedValue
        For i = 0 To Me.ddlDefaultLang.Items.Count - 1
            If Me.ddlDefaultLang.Items(i).Value = "E" Then
                Me.ddlDefaultLang.Items(i).Text = Me.GetGlobalResourceObject("Text", "English")
            ElseIf Me.ddlDefaultLang.Items(i).Value = "C" Then
                Me.ddlDefaultLang.Items(i).Text = Me.GetGlobalResourceObject("Text", "Chinese")
            Else
                Me.ddlDefaultLang.Items(i).Text = Me.ddlDefaultLang.Items(i).Value.ToString
            End If
        Next
        Me.ddlDefaultLang.SelectedValue = selectLanguage

        Me.lblDENoPractice.Text = Me.GetGlobalResourceObject("Text", "NoActivePractice")
        Me.lblDEUsernameTip.Text = Me.GetGlobalResourceObject("Text", "UsernameTips")
        Me.lblDEUsernameTip1.Text = Me.GetGlobalResourceObject("Text", "UsernameTips1")
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDEUsernameTip2.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2")
        Me.lblDEUsernameTip2a.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2a")
        Me.lblDEUsernameTip2b.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2b")
        Me.lblDEUsernameTip2c.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2c")
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.lblSPUsernameTip.Text = Me.GetGlobalResourceObject("Text", "UsernameTips")
        Me.lblSPUsernameTip1.Text = Me.GetGlobalResourceObject("Text", "UsernameTips1")
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblSPUsernameTip2.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2")
        Me.lblSPUsernameTip2a.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2a")
        Me.lblSPUsernameTip2b.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2b")
        Me.lblSPUsernameTip2c.Text = Me.GetGlobalResourceObject("Text", "UsernameTips2c")
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.btnDataEntryChgPWDReturn.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReturnBtn")
        Me.btnDataEntryChgPWDReturn.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ReturnBtn")

        Me.lblIVRSOldPwdText.Text = HttpContext.GetGlobalResourceObject("Text", "OldPassword")

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        If btnFilterDEAcct.Enabled Then
            Me.btnFilterDEAcct.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "FilterDEAcctBtn")
            Me.btnClearDEAcctSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDEAcctSearchBtn")
        Else
            Me.btnFilterDEAcct.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "FilterDEAcctDisableBtn")
            Me.btnClearDEAcctSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDEAcctSearchDisableBtn")
        End If

        If hfDEFilter.Value <> String.Empty Then
            If Me.trUsernameNotFound.Visible Then
                Me.lblUsernameNotFound.Text = Me.GetGlobalResourceObject("Text", "UsernameContainsNotFound")
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.lblUsernameNotFound.Text = Me.lblUsernameNotFound.Text.Replace("%s", AntiXssEncoder.HtmlEncode(hfDEFilter.Value, True))
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            ElseIf Me.trUsernameFound.Visible Then
                Me.lblUsernameFound.Text = Me.GetGlobalResourceObject("Text", "UsernameContains")
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.lblUsernameFound.Text = Me.lblUsernameFound.Text.Replace("%s", AntiXssEncoder.HtmlEncode(hfDEFilter.Value, True))
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If
        End If
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Sub

    Protected Sub chkShowActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.chkBankAcctShowActiveOnly.Checked = Me.chkShowActive.Checked

        udtSP = Me.udtServiceProviderBLL.GetSP()
        gvPracticeInfo.DataSource = udtSP.PracticeList.Values
        gvPracticeInfo.DataBind()
        gvBankInfo.DataBind()
    End Sub

    Protected Function chkInValidUsername(ByVal strOldUsername As String, ByVal strNewUsername As String) As Boolean
        Dim blnRes As Boolean = False

        If udtSPProfileBLL.chkIsEmpty(strNewUsername) Then
            blnRes = True
            Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00006")
            Me.imgUsernameError.Visible = True
        Else
            If Not udtSPProfileBLL.chkValidLoginID(strNewUsername) Then
                blnRes = True
                Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00007")
                Me.imgUsernameError.Visible = True
            Else
                If udtSPProfileBLL.chkDuplicateUsername(strNewUsername) Then
                    blnRes = True
                    Me.udcErrMsgBox.AddMessage(strFuncCode, "E", "00005")
                    Me.imgUsernameError.Visible = True
                End If
            End If
        End If

        Return blnRes
    End Function

    Private Sub ChkExistenceOfIVRSPrintOptionSchemes(ByVal udtSP As ServiceProviderModel)
        Session(IncludePrintingEnabledScheme) = Me.udtConsentFormPrintOptionBLL.CheckPrintOptionEnabled(udtSP.SPID, Me.SubPlatform)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Session(IncludeIVRSEnabledScheme) = Me.udtSchemeClaimBLL.CheckSchemeClaimIVRSEnabled(strSPID)

        ' Retrieve ServiceProvider Enrolled Scheme (SchemeBackOffice)
        Dim udtSchemeInfoBLL As New SchemeInformation.SchemeInformationBLL()
        Dim udtSchemeInformationModelCollection As SchemeInformation.SchemeInformationModelCollection = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, New Common.DataAccess.Database)

        ' Convert the Enrolled Scheme (SchemeBackOffice) to SchemeClaim
        Dim lstStrSchemeClaimCode As List(Of String) = Me.udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtSchemeInformationModelCollection)

        Dim udtSchemeClaimList As SchemeClaimModelCollection = Me.udtSchemeClaimBLL.getAllEffectiveSchemeClaimBySchemeCodeList(lstStrSchemeClaimCode)

        ' Any of the Entitled Scheme Contain IVRS Setting, then return true
        For Each udtSchemeInfoModel As SchemeInformation.SchemeInformationModel In udtSP.SchemeInfoList.Values
            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
                If (udtSchemeInfoModel.SchemeCode = udtSchemeClaim.SchemeCode) And udtSchemeClaim.IVRSAvailable Then
                    Session(IncludeIVRSEnabledScheme) = True
                    Return
                End If
            Next
        Next
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    End Sub


#Region "-------------------------------------Data Entry View-----------------------------------------"
    Protected Sub btnSaveDE_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strOldPWD As String = String.Empty
        Dim strNewPWD As String = String.Empty
        Dim strConfirmNewPWD As String = String.Empty
        Dim blnErr As Boolean = False
        Dim sm As SystemMessage

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me) ''Begin Writing Audit Log
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)
        udtAuditLogEntry.AddDescripton("Data Entry Account Login ID", Me.lblDEUsername.Text)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00007, AublitLogDescription.DataEntryUpdateAccount)

        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
        Me.imgDEConfirmNEWPasswordError.Visible = False
        Me.imgDENEWPasswordError.Visible = False
        Me.imgDEOLDPasswordError.Visible = False

        strOldPWD = Me.txtDEOLDPassword.Text
        strNewPWD = Me.txtDENEWPassword.Text
        strConfirmNewPWD = Me.txtDEConfirmNEWPassword.Text

        If Me.chkDEChangePassword.Checked Then
            If Not udtDataEntryAcctBLL.chkIsEmpty(strOldPWD) Then

                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                'If udtDataEntryAcctBLL.chkDEOldPassword(Me.lblSPID.Text, Me.lblDEUsername.Text, strOldPWD) Then
                Dim dt As DataTable = udtDataEntryAcctBLL.getDataEntryAcctDetails(Me.lblSPID.Text, Me.lblDEUsername.Text)
                Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.DE, dt, strOldPWD)
                If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                    ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                    If Not udtDataEntryAcctBLL.chkIsEmpty(strNewPWD) Then
                        If Not udtDataEntryAcctBLL.chkIsSamePWD(strOldPWD, strNewPWD) Then
                            If udtDataEntryAcctBLL.chkValidPassword(strNewPWD) Then
                                If Not udtDataEntryAcctBLL.chkIsIdenticalPassword(strNewPWD, strConfirmNewPWD) Then
                                    blnErr = True
                                    sm = New SystemMessage(strFuncCode, "E", "00017")
                                    Me.imgDEConfirmNEWPasswordError.Visible = True
                                    Me.udcErrMsgBox.AddMessage(sm)
                                End If
                            Else
                                blnErr = True
                                sm = New SystemMessage(strFuncCode, "E", "00011")
                                Me.imgDENEWPasswordError.Visible = True
                                Me.udcErrMsgBox.AddMessage(sm)
                            End If
                        Else
                            blnErr = True
                            sm = New SystemMessage(strCommFuncCode, "E", "00052")
                            Me.imgDENEWPasswordError.Visible = True
                            Me.udcErrMsgBox.AddMessage(sm)
                        End If
                    Else
                        blnErr = True
                        sm = New SystemMessage(strFuncCode, "E", "00010")
                        Me.imgDENEWPasswordError.Visible = True
                        Me.udcErrMsgBox.AddMessage(sm)
                    End If
                Else
                    blnErr = True
                    sm = New SystemMessage(strFuncCode, "E", "00009")
                    Me.imgDEOLDPasswordError.Visible = True
                    Me.udcErrMsgBox.AddMessage(sm)
                End If
            Else
                blnErr = True
                sm = New SystemMessage(strFuncCode, "E", "00008")
                Me.imgDEOLDPasswordError.Visible = True
                Me.udcErrMsgBox.AddMessage(sm)
            End If
        End If

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If Session(IncludePrintingEnabledScheme) Is Nothing Then
        '    Session(IncludePrintingEnabledScheme) = Me.udtSchemeClaimBLL.CheckPrintOptionEnabled(Me.lblSPID.Text, Me.SubPlatform)
        'End If

        Dim strPrintOption As String = ""

        'If Session(IncludePrintingEnabledScheme) Then
        If PanelDataEntryPrintingOptionSetting.Visible = True Then
            'validate print option
            ' Removed @ 2009-Feb-24
            strPrintOption = Me.GetPrintOptionSelectedValue()
            Dim validator As Common.Validation.Validator = New Common.Validation.Validator
            sm = validator.chkSelectedPrintFormOption(strPrintOption)
            If Not sm Is Nothing Then
                blnErr = True
                Me.udcErrMsgBox.AddMessage(sm)
                imgDESelectPrintOptionError.Visible = True
            End If
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If blnErr Then
            Me.udcErrMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00009, AublitLogDescription.DataEntryUpdateAccountFail)
        Else
            If udtDataEntryAcctBLL.changeDEPassword(Me.lblSPID.Text, Me.lblDEUsername.Text, strNewPWD, strPrintOption, Me.chkDEChangePassword.Checked) Then

                'reload data entry accunt
                Dim udtUserACBLL As New UserACBLL
                udtDataEntry = udtDataEntryAcctBLL.LoadDataEntry(Me.lblSPID.Text, Me.lblDEUsername.Text)
                Me.udtDataEntryUserBLL.SaveToSession(udtDataEntry)
                udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)
                udtAuditLogEntry.AddDescripton("Data Entry Account Login ID", Me.lblDEUsername.Text)
                udtAuditLogEntry.AddDescripton("Default Printing Option", strPrintOption)
                udtAuditLogEntry.AddDescripton("Password Updated", Me.chkDEChangePassword.Checked)
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AublitLogDescription.DataEntryUpdateAccountSuccess)
                sm = New SystemMessage(strFuncCode, "I", "00002")

                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMsgBox.AddMessage(sm)
                Me.udcInfoMsgBox.BuildMessageBox()
                Me.panDataEntryChgPwd.Visible = False
                Me.btnDataEntryChgPWDReturn.Visible = True
            Else
                sm = New SystemMessage(strFuncCode, "E", "00011")
                Me.udcErrMsgBox.AddMessage(sm)
                Me.udcErrMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00009, AublitLogDescription.DataEntryUpdateAccountFail)
            End If
        End If
    End Sub

    Protected Sub btnDEEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.InitDataEntryEditProfileControl()
        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00055, AublitLogDescription.DataEntryUpdateEdit)
        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    End Sub

    Protected Sub btnDECancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcErrMsgBox.Clear()
        Me.imgDESelectPrintOptionError.Visible = False
        Me.imgDEOLDPasswordError.Visible = False
        Me.imgDENEWPasswordError.Visible = False
        Me.imgDEConfirmNEWPasswordError.Visible = False

        Me.InitDataEntryProfileControl()
        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00056, AublitLogDescription.DataEntryUpdateCancel)
        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    End Sub

    Private Sub btnDataEntryChgPWDReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDataEntryChgPWDReturn.Click
        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False
        Me.imgDESelectPrintOptionError.Visible = False
        Me.panDataEntryChgPwd.Visible = True
        Me.btnDataEntryChgPWDReturn.Visible = False
        Me.InitDataEntryProfileControl()
    End Sub

    Protected Sub chkDEChangePassword_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDEChangePassword.CheckedChanged
        Me.InitDataEntryChangePasswordControl(Me.chkDEChangePassword.Checked)
    End Sub
#End Region

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
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

    Private Function IsPCDSP(ByVal udtSP As ServiceProviderModel) As Boolean
        Dim blnIsPCD As Boolean = False

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
        '    If udtPractice.IsAllowJoinPCD Then
        '        blnIsPCD = True
        '    End If
        'Next

        Dim udtPracticeList As PracticeModelCollection = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)
        If udtPracticeList.Count > 0 Then
            blnIsPCD = True
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
        End If

        Return blnIsPCD
    End Function

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------

    Private Sub ibtnJoinPCD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnJoinPCD.Click
        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00020, "Click Join / Update PCD")
        Me.ucInputTokenPopup.Build()
        Me.ModalPopupExtenderToken.Show()
    End Sub

    Private Sub ucInputTokenPopup_Success_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucInputTokenPopup.Success_Click

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00022, "Join / Update PCD Token Success")

        udtSP = Me.udtServiceProviderBLL.GetSP()

        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
        'Update PCD Status
        Dim blnRes As Boolean = CheckAndUpdateAccountStatus(udtSP, udtAuditLogEntry)
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

        If blnRes Then
            Dim udtPCDWebService As PCDWebService = New PCDWebService(Me.FunctionCode)

            ' CRE19-024 (Not Create PCD Account in SP platform) [Start][Winnie SUEN]
            ' ---------------------------------------------------------------------------------------------------------
            'Dim udtResult As WebService.Interface.PCDCheckIsActiveSPResult

            'udtAuditLogEntry.AddDescripton("WebMethod", "PCDCheckIsActiveSP")
            'udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00023, "Check PCD Account Start")

            'udtResult = udtPCDWebService.PCDCheckIsActiveSP(udtSP.HKID)

            'udtAuditLogEntry.AddDescripton("ReturnCode", udtResult.ReturnCode.ToString)
            'udtAuditLogEntry.AddDescripton("MessageID", udtResult.MessageID.ToString)

            'Select Case udtResult.ReturnCode
            '    Case WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.IsActive
            '        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, "Active PCD Account Found")
            '        Me.ModalPopupExtenderPCDEnrolled.Show()
            '    Case WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.NotActive
            '        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00025, "Active PCD Account Not Found")
            '        ShowTypeOfPractice(ucTypeOfPracticeGrid.EnumMode.Create)
            '    Case Else
            '        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00026, "PCDCheckIsActiveSPResult Failed")
            '        Me.ucReturnCodeHandlingPopUp.MessageText = udtResult.ReturnCodeDesc
            '        Me.ModalPopupExtenderReturnCodeHandling.Show()
            'End Select

            ' Check SP exists in PCD (Save the variable to ServiceProviderBLL)
            Dim udtSPBLL As New ServiceProviderBLL
            Dim objPCDWS As New Common.PCD.PCDWebService(CType(Me.Page, BasePage).FunctionCode)
            Dim objResult As Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult = Nothing
            
            udtAuditLogEntry.AddDescripton("WebMethod", "CheckAvailableForVerifiedEnrolment")
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00023, "CheckAvailableForVerifiedEnrolment Start")

            objResult = objPCDWS.PCDCheckAvailableForVerifiedEnrolment(udtSP)

            udtAuditLogEntry.AddDescripton("WebMethod", "CheckAvailableForVerifiedEnrolment")
            udtAuditLogEntry.AddDescripton("ReturnCode", objResult.ReturnCode)
            udtAuditLogEntry.AddDescripton("MessageID", objResult.MessageID)
            
            Select Case objResult.ReturnCode
                Case PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.Available
                    ' Display Type of Practice Popup
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00057, "CheckAvailableForVerifiedEnrolment Success")
                    ShowTypeOfPractice(ucTypeOfPracticeGrid.EnumMode.Create)

                Case PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ServiceProviderAlreadyExisted
                    ' Display add Practice to PCD
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00058, "CheckAvailableForVerifiedEnrolment Success")
                    Me.ModalPopupExtenderPCDEnrolled.Show()

                Case PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.EnrolmentAlreadyExisted, _
                        PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.VerifiedEnrolmentAlreadyExisted
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00058, "CheckAvailableForVerifiedEnrolment Success")

                    ucNoticePopUp.NoticeMode = ucNoticePopUp.enumNoticeMode.Notification
                    ucNoticePopUp.MessageText = objResult.ReturnCodeDesc
                    Me.ModalPopupExtenderTypeOfPractice.Hide()
                    Me.ShowPCDNoticePopup()

                Case Else
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00059, "CheckAvailableForVerifiedEnrolment Failed")
                    Me.ucReturnCodeHandlingPopUp.MessageText = objResult.ReturnCodeDesc
                    Me.ModalPopupExtenderReturnCodeHandling.Show()

            End Select

        End If
    End Sub

    Private Sub ucInputTokenPopup_Failure_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucInputTokenPopup.Failure_Click
        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00021, "Join / Update PCD Token Fail")
        Me.ModalPopupExtenderToken.Show()
    End Sub

    'Integration End

    Private Sub ucTypeOfPracticePopup_ButtonClick(ByVal e As ucTypeOfPracticePopup.enumButtonClick) Handles ucTypeOfPracticePopup.ButtonClick

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)

        Select Case e
            Case HCSP.ucTypeOfPracticePopup.enumButtonClick.Cancel
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00029, "Type Of Practice Popup Cancel Clicked")
                Me.ucTypeOfPracticePopup.Showing = False
                Me.ModalPopupExtenderTypeOfPractice.Hide()
            Case HCSP.ucTypeOfPracticePopup.enumButtonClick.CreatePCDAccount
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00030, "Type Of Practice Popup Create PCD Account Clicked")

                ' CRE19-024 (Not Create PCD Account in SP platform) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                'If Not IsNothing(ucTypeOfPracticePopup.CreatePCDSPAcctResult) Then
                If Not IsNothing(ucTypeOfPracticePopup.CreatePCDUploadVerifiedEnrolmentResult) Then
                    Me.ucTypeOfPracticePopup.Showing = False
                    Me.ModalPopupExtenderTypeOfPractice.Hide()
                    'Select Case ucTypeOfPracticePopup.CreatePCDSPAcctResult.ReturnCode
                    '    Case WebService.Interface.PCDCreatePCDSPAcctResult.enumReturnCode.SuccessWithData
                    '        Session("PCDActivationLink") = GetActivationLink()

                    '        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
                    '        ' Enquire PCD
                    '        Dim objWS As New Common.PCD.PCDWebService(Me.FunctionCode)
                    '        Dim objResult As WebService.Interface.PCDCheckAccountStatusResult = Nothing
                    '        Dim strMessage As String = String.Empty

                    '        Dim udtSPModel As ServiceProviderModel = Me.udtServiceProviderBLL.GetSP()
                    '        Dim strSPID As String = udtSPModel.SPID
                    '        Dim strUpdateBy As String = udtSPModel.SPID
                    '        objResult = objWS.PCDCheckAccountStatus(udtSPModel.HKID)

                    '        Dim blnUpdateRes As Boolean = objResult.UpdateJoinPCDStatus(strSPID, strUpdateBy, strMessage, udtSPModel)
                    '        If Not blnUpdateRes Then
                    '            Throw New Exception(strMessage)
                    '        End If
                    '        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) 

                    '        Me.ShowPCDActivationNoticePopup()
                    '    Case WebService.Interface.PCDCreatePCDSPAcctResult.enumReturnCode.ServiceProviderAlreadyExisted
                    '        Me.ucNoticePopUp.MessageText = ucTypeOfPracticePopup.CreatePCDSPAcctResult.ReturnCodeDesc
                    '        Me.ModalPopupExtenderTypeOfPractice.Hide()
                    '        Me.ShowPCDNoticePopup()
                    '    Case WebService.Interface.PCDCreatePCDSPAcctResult.enumReturnCode.EnrolmentProcessingByPCO
                    '        Me.ucNoticePopUp.MessageText = ucTypeOfPracticePopup.CreatePCDSPAcctResult.ReturnCodeDesc
                    '        Me.ModalPopupExtenderTypeOfPractice.Hide()
                    '        Me.ShowPCDNoticePopup()
                    '    Case Else
                    '        Me.ucNoticePopUp.MessageText = ucTypeOfPracticePopup.CreatePCDSPAcctResult.ReturnCodeDesc
                    '        Me.ModalPopupExtenderTypeOfPractice.Hide()
                    '        Me.ShowPCDNoticePopupExclamation()
                    'End Select

                    ' Show Notice Popup For JoinPCD

                    Select Case ucTypeOfPracticePopup.CreatePCDUploadVerifiedEnrolmentResult.ReturnCode
                        Case PCDUploadVerifiedEnrolmentResult.enumReturnCode.UploadedSuccessfully, _
                                PCDUploadVerifiedEnrolmentResult.enumReturnCode.ServiceProviderAlreadyExisted, _
                                PCDUploadVerifiedEnrolmentResult.enumReturnCode.EnrolmentAlreadyExisted
                            ucNoticePopUp.NoticeMode = ucNoticePopUp.enumNoticeMode.Notification

                        Case Else
                            ucNoticePopUp.NoticeMode = ucNoticePopUp.enumNoticeMode.Custom
                            ucNoticePopUp.ButtonMode = ucNoticePopUp.enumButtonMode.OK
                            ucNoticePopUp.IconMode = ucNoticePopUp.enumIconMode.ExclamationIcon
                            ucNoticePopUp.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")

                    End Select

                    ucNoticePopUp.MessageText = ucTypeOfPracticePopup.CreatePCDUploadVerifiedEnrolmentResult.ReturnCodeDesc
                    'ModalPopupExtenderNotice.PopupDragHandleControlID = ucNoticePopUp.Header.ClientID
                    'ModalPopupExtenderNotice.Show()

                    ' CRE19-024 (Not Create PCD Account in SP platform) [End][Chris YIM]	

                    Me.ModalPopupExtenderTypeOfPractice.Hide()
                    Me.ShowPCDNoticePopup()

                End If
            Case HCSP.ucTypeOfPracticePopup.enumButtonClick.TransferToPCD
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00031, "Type Of Practice Popup Transfer To PCD Clicked")
                If Not IsNothing(ucTypeOfPracticePopup.TransferPCDResult) Then
                    Select Case ucTypeOfPracticePopup.TransferPCDResult.ReturnCode
                        Case Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult.enumReturnCode.SuccessWithData
                            Me.ModalPopupExtenderTypeOfPractice.Hide()
                            Me.ShowPCDTransferPracticeLogout()
                        Case Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult.enumReturnCode.ServiceProviderNotExistNorActive, _
                             Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult.enumReturnCode.ServiceProviderUnderAmendment, _
                             Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult.enumReturnCode.ServiceProviderNotYetActivated
                            Me.ucNoticePopUp.MessageText = ucTypeOfPracticePopup.TransferPCDResult.ReturnCodeDesc
                            Me.ModalPopupExtenderTypeOfPractice.Hide()
                            Me.ShowPCDNoticePopup()
                        Case Else
                            Me.ucNoticePopUp.MessageText = ucTypeOfPracticePopup.TransferPCDResult.ReturnCodeDesc
                            Me.ModalPopupExtenderTypeOfPractice.Hide()
                            Me.ShowPCDNoticePopupExclamation()
                    End Select
                Else
                    Me.ModalPopupExtenderTypeOfPractice.Hide()
                    Me.ucReturnCodeHandlingPopUp.MessageText = (New SystemMessage("990000", "E", "00321")).GetMessage
                    Me.ModalPopupExtenderReturnCodeHandling.Show()
                End If
        End Select
    End Sub

    Private Function GetActivationLink() As String
        If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
            Return Me.ucTypeOfPracticePopup.CreatePCDSPAcctResult.ActivationLinkTC
        Else
            Return Me.ucTypeOfPracticePopup.CreatePCDSPAcctResult.ActivationLink
        End If
    End Function

    Private Sub ucNoticePopUp_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUp.ButtonClick

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)

        Select Case e
            Case HCSP.ucNoticePopUp.enumButtonClick.Cancel
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00032, "Notice Popup Cancel Clicked")
                Me.ucTypeOfPracticePopup.Showing = False
                Me.ModalPopupExtenderNotice.Hide()
                Me.ModalPopupExtenderTypeOfPractice.Hide()
                Session("PCDActivationLink") = Nothing
            Case HCSP.ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00033, "Notice Popup OK Clicked")
                Me.ucTypeOfPracticePopup.Showing = False
                Me.ModalPopupExtenderNotice.Hide()
                Me.ModalPopupExtenderTypeOfPractice.Hide()
                Dim strActivationLink As String = String.Empty
                If Not IsNothing(Session("PCDActivationLink")) Then
                    strActivationLink = Session("PCDActivationLink")
                End If
                If Len(strActivationLink) > 0 Then
                    Me.ShowPCDActivationLogout()
                End If
        End Select
    End Sub

    Private Sub ucPCDEnrolledPopup_ButtonClick(ByVal e As ucPCDEnrolledPopup.enumButtonClick) Handles ucPCDEnrolledPopup.ButtonClick

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)

        Select Case e
            Case HCSP.ucPCDEnrolledPopup.enumButtonClick.AddPractice
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00034, "PCD Enrolled Popup Add Practice Clicked")
                Me.ucTypeOfPracticePopup.Mode = ucTypeOfPracticeGrid.EnumMode.Transfer
                Me.ShowTypeOfPractice(ucTypeOfPracticeGrid.EnumMode.Transfer)
            Case HCSP.ucPCDEnrolledPopup.enumButtonClick.LoginPCD
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00035, "PCD Enrolled Popup Login PCD Clicked")
                Me.ShowPCDConfirmationLogout()
            Case HCSP.ucPCDEnrolledPopup.enumButtonClick.Cancel
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00036, "PCD Enrolled Popup Cancel Clicked")
        End Select
    End Sub

    Private Sub ucNoticeLogoutEHSPopUp_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticeLogoutEHSPopUp.ButtonClick

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)

        clearSelectedPractice()
        Select Case e
            Case HCSP.ucNoticePopUp.enumButtonClick.Cancel
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00037, "Notice Logout EHS PopUp Cancel Clicked")
                Me.ModalPopupExtenderNoticeLogoutEHS.Hide()
                Me.ucTypeOfPracticePopup.Showing = False
                Me.ModalPopupExtenderTypeOfPractice.Hide()
                Session("PCDActivationLink") = Nothing
            Case HCSP.ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00038, "Notice Logout EHS PopUp OK Clicked")
                Me.ModalPopupExtenderNoticeLogoutEHS.Hide()
                Dim strActivationLink As String = String.Empty
                If Not IsNothing(Session("PCDActivationLink")) Then
                    strActivationLink = Session("PCDActivationLink")
                End If
                If Len(strActivationLink) > 0 Then
                    LogoutEHSAndActivatePCD()
                Else
                    LogoutEHSAndLoginPCD()
                End If
        End Select

    End Sub

    Private Sub LogoutEHSAndLoginPCD()
        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00027, "Logout EHS And Login PCD")
        CType(Me.Master, MasterPage).Logout()
        Response.Redirect(GetPCDMainPage)
    End Sub

    Private Sub LogoutEHSAndActivatePCD()
        Dim strActivationLink As String = String.Empty
        If Not IsNothing(Session("PCDActivationLink")) Then
            strActivationLink = Session("PCDActivationLink")
        End If
        Session("PCDActivationLink") = Nothing
        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00028, "Logout EHS And Activate PCD")
        CType(Me.Master, MasterPage).Logout()
        Response.Redirect(strActivationLink)
    End Sub

    Private Function GetPCDMainPage() As String
        Dim udcGeneralFun As New Common.ComFunction.GeneralFunction
        Dim strURL As String = String.Empty
        If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
            udcGeneralFun.getSystemParameter("PCDMainPageURL_CHI", strURL, String.Empty)
        Else
            udcGeneralFun.getSystemParameter("PCDMainPageURL_ENG", strURL, String.Empty)
        End If
        Return strURL
    End Function

    Private Sub ShowTypeOfPractice(ByVal enumMode As ucTypeOfPracticeGrid.EnumMode)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("SP_ID", Me.lblSPID.Text)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00134, "Notice Logout EHS PopUp OK Clicked")

        Me.ucTypeOfPracticePopup.Reset()
        Me.ucTypeOfPracticePopup.Showing = True
        Me.ucTypeOfPracticePopup.Mode = enumMode

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------        
        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetSP
        Dim udtPracticeList As PracticeModelCollection = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)
        Me.ucTypeOfPracticePopup.LoadPractice(udtPracticeList)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
        Me.ModalPopupExtenderTypeOfPractice.Show()
    End Sub

    Private Sub ShowPCDActivationLogout()
        Me.ucNoticeLogoutEHSPopUp.HeaderText = Me.GetGlobalResourceObject("Text", "AccountActivationBoxTitle")
        Me.ModalPopupExtenderNoticeLogoutEHS.Show()
    End Sub

    Private Sub ShowPCDTransferPracticeLogout()
        Me.ucNoticeLogoutEHSPopUp.HeaderText = Me.GetGlobalResourceObject("Text", "UpdatePCD")
        Me.ModalPopupExtenderNoticeLogoutEHS.Show()
    End Sub

    Private Sub ShowPCDConfirmationLogout()
        Me.ucNoticeLogoutEHSPopUp.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmBoxTitle")
        Me.ModalPopupExtenderNoticeLogoutEHS.Show()
    End Sub

    Private Sub ShowPCDNoticePopupExclamation()
        Me.ucNoticePopUp.NoticeMode = HCSP.ucNoticePopUp.enumNoticeMode.Custom
        Me.ucNoticePopUp.ButtonMode = HCSP.ucNoticePopUp.enumButtonMode.OK
        Me.ucNoticePopUp.IconMode = HCSP.ucNoticePopUp.enumIconMode.ExclamationIcon
        Me.ucNoticePopUp.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
        Me.ModalPopupExtenderNotice.Show()
    End Sub

    Private Sub ShowPCDNoticePopup()
        Me.ucNoticePopUp.NoticeMode = HCSP.ucNoticePopUp.enumNoticeMode.Notification
        Me.ucNoticePopUp.ButtonMode = HCSP.ucNoticePopUp.enumButtonMode.OK
        Me.ucNoticePopUp.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
        Me.ModalPopupExtenderNotice.Show()
    End Sub

    Private Sub ShowPCDActivationNoticePopup()
        Me.ucNoticePopUp.NoticeMode = HCSP.ucNoticePopUp.enumNoticeMode.Custom
        Me.ucNoticePopUp.ButtonMode = HCSP.ucNoticePopUp.enumButtonMode.YesNo
        Me.ucNoticePopUp.HeaderText = Me.GetGlobalResourceObject("Text", "AccountActivationBoxTitle")
        Me.ucNoticePopUp.MessageText = Me.GetGlobalResourceObject("Text", "ActivatePCD")
        ModalPopupExtenderNotice.Show()
    End Sub

    ' CRE12-001 eHS and PCD integration [End][Koala]

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Popup legend again if legend data changed
    Private Sub udcSchemeLegend_DataChanged() Handles udcSchemeLegend.DataChanged
        popupSchemeNameHelp.Show()
    End Sub

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub FilterPracticeList(ByRef PracticeList As PracticeModelCollection)
        Dim IsPracticeSchemeCoexist As Boolean
        Dim udtPracticeModel As PracticeModel

        For cnt As Integer = 1 To PracticeList.Count
            IsPracticeSchemeCoexist = False
            If cnt <= PracticeList.Count Then
                udtPracticeModel = PracticeList.GetValueList(cnt - 1)
                If udtPracticeModel.PracticeSchemeInfoList.Count > 0 Then
                    IsPracticeSchemeCoexist = True
                End If

                If Not IsPracticeSchemeCoexist Then
                    PracticeList.Remove(udtPracticeModel)
                    cnt -= 1
                End If
            End If
        Next
    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub FilterMOList(ByRef MOList As MedicalOrganization.MedicalOrganizationModelCollection, ByVal PracticeList As PracticeModelCollection)
        Dim dtMODisplaySeqInPractce As New DataTable
        Dim dtMODisplaySeq As New DataTable
        Dim drMODisplaySeq As DataRow
        Dim dvMODisplaySeq As DataView

        dtMODisplaySeqInPractce.Columns.Add("MO_Display_Seq")

        For Each udtPracticeModel As Practice.PracticeModel In PracticeList.Values
            drMODisplaySeq = dtMODisplaySeqInPractce.NewRow
            drMODisplaySeq("MO_Display_Seq") = udtPracticeModel.MODisplaySeq
            dtMODisplaySeqInPractce.Rows.Add(drMODisplaySeq)
        Next

        dvMODisplaySeq = New DataView(dtMODisplaySeqInPractce)
        dtMODisplaySeq = dvMODisplaySeq.ToTable(True, "MO_Display_Seq")

        Dim IsMOPracticeCoexist As Boolean
        Dim udtMOModel As MedicalOrganization.MedicalOrganizationModel

        For cnt As Integer = 1 To MOList.Count
            IsMOPracticeCoexist = False
            If cnt <= MOList.Count Then
                udtMOModel = MOList.GetValueList(cnt - 1)
                For Each drResultErrorSPID As DataRow In dtMODisplaySeq.Select()
                    If udtMOModel.DisplaySeq = drResultErrorSPID.Item("MO_Display_Seq") Then
                        IsMOPracticeCoexist = True
                    End If
                Next

                If Not IsMOPracticeCoexist Then
                    MOList.Remove(udtMOModel)
                    cnt -= 1
                End If
            End If
        Next

    End Sub

    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    Private Sub CancelClick_Clear()
        Me.udcInfoMsgBox.Visible = False
        Me.udcErrMsgBox.Visible = False

        Me.imgSPSelectPrintOptionError.Visible = False
        Me.imgSPConfirmNewIVRSPWDError.Visible = False
        Me.imgSPConfirmNewPWDError.Visible = False
        Me.imgSPNewIVRSPWDError.Visible = False
        Me.imgSPNewPWDError.Visible = False
        Me.imgSPOldIVRSPWDError.Visible = False
        Me.imgSPOldPWDError.Visible = False
        Me.imgUsernameError.Visible = False

        Me.chkChgWebPWD.Enabled = False
        Me.chkChgWebPWD.Checked = False
        Me.chkChgWebPWD_CheckedChanged(Nothing, Nothing)

        Me.chkActivateIVRSPwd.Enabled = False
        Me.chkActivateIVRSPwd.Checked = False

        Me.chkChgIVRSPwd.Enabled = False
        Me.chkChgIVRSPwd.Checked = False
        Me.chkChgIVRSPwd_CheckedChanged(Nothing, Nothing)

        Me.ddlDefaultLang.Enabled = False

        'Print Option
        Me.InitLoginUserPrintOptionList(False)

        Me.ddlDefaultLang.SelectedValue = udtSP.DefaultLanguage

        Me.txtUsername.Text = Me.lblUsername.Text
        Me.txtUsername.Visible = False
        Me.lblSPUsernameTip.Visible = False
        Me.lblSPUsernameTip1.Visible = False
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblSPUsernameTip2.Visible = False
        Me.lblSPUsernameTip2a.Visible = False
        Me.lblSPUsernameTip2b.Visible = False
        Me.lblSPUsernameTip2c.Visible = False
        Me.tdUsernameTips.Style.Add("display", "none")
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Me.lblUsername.Visible = True
        Me.btnChkAvail.Visible = False
        Me.btnGetUsernameFromEHRSS.Visible = False

        Me.btnEdit.Visible = True
        Me.btnSave.Visible = False
        Me.btnCancel.Visible = False
    End Sub


    Private Sub ResetControl(ByVal blnSaveReset As Boolean)
        udcErrMsgBox.Clear()
        Dim dt As DataTable
        Dim strStatus As String = String.Empty
        Dim strAcctLocked As String = String.Empty

        Me.udcErrMsgBox.Visible = False
        Me.udcInfoMsgBox.Visible = False

        Me.imgDELoginIDError.Visible = False
        Me.imgDEPracticeError.Visible = False
        Me.imgDENewPWDError.Visible = False
        Me.imgDEConfirmNewPWDError.Visible = False
        'Me.imgDefaultPritingOptionError.Visible = False

        'Me.txtDEloginID.Text = String.Empty

        Me.txtDENewPWD.Enabled = False
        Me.txtDEConfirmNewPWD.Enabled = False

        Me.txtDEloginID.Enabled = False
        Me.lblDEUsernameTip.Visible = True
        Me.lblDEUsernameTip1.Visible = True
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDEUsernameTip2.Visible = True
        Me.lblDEUsernameTip2a.Visible = True
        Me.lblDEUsernameTip2b.Visible = True
        Me.lblDEUsernameTip2c.Visible = True
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.txtDEloginID.BackColor = Drawing.Color.LightGray
        Me.gvdataEntryAcc.Enabled = True

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        InitDEFilterControl(True)
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        'print option
        'Me.lblDEPrintOptionRemind.Visible = False

        Me.chkChgDEPWD.Checked = False
        Me.chkChgDEPWD.Visible = False
        Me.chkChgDEPWD_CheckedChanged(Nothing, Nothing)

        Me.txtDENewPWD.Text = String.Empty
        Me.txtDEConfirmNewPWD.Text = String.Empty

        Me.chkChgDEPWD.Enabled = False
        Me.chkPracticeList.Enabled = False

        'Me.ddlDEDefaultPrintOption.Enabled = False

        'Me.btnAddDEAcct.Visible = True
        Me.btnAddDEAcct.Visible = Me.chkPracticeList.Visible
        Me.btnEditDEAcct.Visible = False
        Me.btnSaveDEAcct.Visible = False
        Me.btnCancelDEAcct.Visible = False

        Me.chkDESuspend.Enabled = False
        clearSelectedPractice()

        Me.chkDEAccountLocked.Enabled = False
        Me.chkDEAccountLocked.Checked = False

        If Not Me.lblDELoginID.Text.Trim.Equals(String.Empty) Then
            udtUserAC = UserACBLL.GetUserAC
            If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                udtSP = CType(udtUserAC, ServiceProviderModel)
            End If

            Me.chkChgDEPWD.Visible = True
            Me.chkChgDEPWD.Checked = False
            Me.chkChgDEPWD_CheckedChanged(Nothing, Nothing)

            Me.lblDEUsernameTip.Visible = False
            Me.lblDEUsernameTip1.Visible = False
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.lblDEUsernameTip2.Visible = False
            Me.lblDEUsernameTip2a.Visible = False
            Me.lblDEUsernameTip2b.Visible = False
            Me.lblDEUsernameTip2c.Visible = False
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


            dt = udtDataEntryAcctBLL.getDataEntryAcctDetails(udtSP.SPID, txtDEloginID.Text.Trim)

            strStatus = dt.Rows(0).Item("Record_Status").ToString
            strAcctLocked = dt.Rows(0).Item("Account_Locked").ToString

            Me.chkDESuspend.Enabled = False
            If strStatus = "A" Then
                Me.chkDESuspend.Checked = False
            Else
                Me.chkDESuspend.Checked = True
            End If

            Me.chkDEAccountLocked.Enabled = False
            If strAcctLocked = "Y" Then
                Me.chkDEAccountLocked.Checked = True
            Else
                Me.chkDEAccountLocked.Checked = False
            End If

            For Each dr As DataRow In dt.Rows
                If dr.Item("Practice_Status") = "A" Then
                    If dr.Item("SP_Practice_Display_Seq") > 0 Then
                        Me.chkPracticeList.Items.FindByValue(dr.Item("SP_Practice_Display_Seq").ToString + "-" + dr.Item("SP_Bank_Acc_display_Seq").ToString).Selected = True
                    End If
                End If
            Next

            Me.btnEditDEAcct.Visible = True

            If blnSaveReset Then

            Else
                Dim intPasswordLevel As Integer = dt.Rows(0).Item("Password_Level")
                displayInfoResetPWMsgBox(intPasswordLevel)
            End If


            'reset print option value
            'If dt.Rows(0).Item("ConsentPrintOption") Is DBNull.Value Then
            '    Dim strConsentPrintOption As String
            '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            '    udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)

            '    Me.InitDataEntryMaintenancePrintOptionList(False, strConsentPrintOption, False)
            '    'Me.ddlDEDefaultPrintOption.SelectedValue = strConsentPrintOption
            'Else

            '    Me.InitDataEntryMaintenancePrintOptionList(False, dt.Rows(0).Item("ConsentPrintOption"), False)
            '    'Me.ddlDEDefaultPrintOption.SelectedValue = 
            'End If
        Else
            Me.txtDEloginID.Text = String.Empty
            'Me.InitDataEntryMaintenancePrintOptionList(False, "", True)
            'reset print option value
            'Me.ddlDEDefaultPrintOption.SelectedValue = String.Empty
        End If
    End Sub


    Private Sub displayInfoResetPWMsgBox(ByVal intPasswordLevel As Integer)
        Dim blnVerifyPWLevel As Boolean = VerifyPasswordLevel(intPasswordLevel)
        Dim sm As SystemMessage
        'Dim verifyPW As String = VerifyPassword(EnumPlatformType.DE, dt, strPassword).VerifyResult
        If blnVerifyPWLevel Then
            udcInfoMsgBox.Visible = False
        Else
            sm = New SystemMessage(strFuncCode, "I", "00006")
            Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcInfoMsgBox.AddMessage(sm)
            Me.udcInfoMsgBox.BuildMessageBox()
        End If
    End Sub
    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    'Public Sub GridViewDataBind(ByRef gvSort As GridView, ByRef dt As Object)
    '    Dim intPageIndex As Integer = 0
    '    intPageIndex = gvSort.PageIndex
    '    Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))

    '    gvFunction.GridViewSortDirection = ViewState("SortDirection_" & gvSort.ID)
    '    gvFunction.GridViewSortExpression = ViewState("SortExpression_" & gvSort.ID)

    '    gvSort.PageSize = (New GeneralFunction).GetPageSizeHCSP()
    '    gvSort.PageIndex = intPageIndex
    '    gvSort.DataSource = gvFunction.SortDataTable(dt, True)
    '    gvSort.DataBind()
    'End Sub

    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start]   (Marco) ---
    Public Function CheckAndUpdateAccountStatus(ByRef udtSPModel As ServiceProviderModel, ByRef udtAuditLogEntry As AuditLogEntry)
        Dim blnRes As Boolean = True

        ' Check Service Provider's Join PCD Status    
        Dim objResult As WebService.Interface.PCDCheckAccountStatusResult = Nothing

        udtAuditLogEntry.AddDescripton("WebMethod", "PCDCheckAccountStatus")
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00057, "CheckPCDAccountStatus Start")

        Dim objWS As New Common.PCD.PCDWebService(Me.FunctionCode)
        objResult = objWS.PCDCheckAccountStatus(udtSPModel.HKID)

        udtAuditLogEntry.AddDescripton("ReturnCode", objResult.ReturnCode.ToString)
        udtAuditLogEntry.AddDescripton("MessageID", objResult.MessageID.ToString)

        'Variable
        Dim strSPID As String = udtSPModel.SPID
        Dim strUpdateBy As String = udtSPModel.SPID
        Dim strMessage As String = String.Empty

        ' Update Service Provider's Join PCD Status  
        Select Case objResult.ReturnCode
            Case WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.Success
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00058, "CheckPCDAccountStatus Success")
                If strSPID <> String.Empty Then
                    Dim blnUpdateRes As Boolean = objResult.UpdateJoinPCDStatus(strSPID, strUpdateBy, strMessage, udtSPModel)
                    If Not blnUpdateRes Then
                        Throw New Exception(strMessage)
                    End If
                End If
            Case Else
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00059, "CheckPCDAccountStatus Fail")
                Me.ucReturnCodeHandlingPopUp.MessageText = objResult.ReturnCodeDesc
                Me.ModalPopupExtenderReturnCodeHandling.Show()
                blnRes = False
        End Select

        Return blnRes
    End Function
    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---
End Class





