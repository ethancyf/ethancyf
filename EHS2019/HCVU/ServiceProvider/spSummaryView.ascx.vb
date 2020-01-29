Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.BankAcct
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component.VoucherScheme
Imports Common.DataAccess
Imports Common.Format

Imports HCVU.AccountChangeMaintenance
Imports HCVU.spProfile

Partial Public Class spSummaryView
    Inherits System.Web.UI.UserControl

#Region "Fields"

    Public udtSPProfileBLL As New SPProfileBLL

    Private udtAccountChangeMaintenanceBLL As New AccountChangeMaintenanceBLL
    Private udtFormatter As New Formatter
    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
    Private udtServiceProviderBLL As New ServiceProviderBLL
    Private udtSPAccountUpateBLL As New SPAccountUpdateBLL
    Private udtSPVerificationBLL As New ServiceProviderVerificationBLL

#End Region

#Region "Constants"

    Private Const strNew As String = "New"
    Private Const strUnderAmendment As String = "Under Amendment"
    Private Const strMONotAvailable As String = "0"
    Private Const strNo As String = "N"

#End Region

#Region "Session Constants"

    Private Const SESS_SPSummaryViewCurrentServiceProvider As String = "SPSummaryViewCurrentServiceProvider"
    Private Const SESS_SPSummaryViewCurrentServiceProvider_Staging As String = "SPSummaryViewCurrentServiceProvider_Staging"
    Private Const SESS_SPSummaryViewCurrentServiceProvider_Enrolment As String = "SPSummaryViewCurrentServiceProvider_Enrolment"
    Private Const SESS_SPSummaryViewPracticeSchemeInfoList As String = "SESS_SPSummaryViewPracticeSchemeInfoList"

#End Region

    ' Called from other pages

    Public Sub buildSpProfileObject(ByVal udtSP As ServiceProviderModel, ByVal strTableLocation As String, ByVal udtSPPermanent As ServiceProviderModel)
        buildSpProfileObject(udtSP, strTableLocation)

        If strTableLocation.Equals(TableLocation.Staging) Then
            ' Add indicators to the changed fields
            Dim udtComparator As New ServiceProviderComparator(udtSPPermanent)

            ' Compare SP (6 items - English Name, Chinese Name, Address, Email, Phone, Fax)
            Dim aryCompareSP As New ArrayList
            aryCompareSP.Add(ServiceProviderComparator.EnglishName)
            aryCompareSP.Add(ServiceProviderComparator.ChineseName)
            aryCompareSP.Add(ServiceProviderComparator.SpAddress)
            aryCompareSP.Add(ServiceProviderComparator.Email)
            aryCompareSP.Add(ServiceProviderComparator.Phone)
            aryCompareSP.Add(ServiceProviderComparator.Fax)

            For Each strCompare As String In aryCompareSP
                EnableIndicator(IIf(udtComparator.IsServiceProviderChanged(strCompare, udtSP), True, False), strCompare)
            Next

            ' Compare SP Scheme (1 item - Status)
            lblEnrolledSchemeTextInd.Visible = False

            For Each rScheme As GridViewRow In gvEnrolledScheme.Rows
                Dim lblERecordStatus As Label = CType(rScheme.FindControl("lblERecordStatus"), Label)
                If lblERecordStatus.Text = strNew Then
                    lblERecordStatus.ForeColor = Drawing.Color.Red
                    lblEnrolledSchemeTextInd.Visible = True
                End If
            Next

            ' Compare MO (5 items - Status, Phone, Email, Fax, Address)
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                Dim rMO As GridViewRow = gvMO.Rows(CInt(udtMO.DisplaySeq) - 1)
                Dim lblMOStatus As Label = rMO.FindControl("lblMOStatus")

                ' If the "Medical Organization Status" is New, just turn the Status to red
                If lblMOStatus.Text = strNew Then
                    lblMOStatus.ForeColor = Drawing.Color.Red
                    CType(rMO.FindControl("lblMOStatusTextInd"), Label).Visible = True
                Else
                    If lblMOStatus.Text = strUnderAmendment Then
                        lblMOStatus.ForeColor = Drawing.Color.Red
                        CType(rMO.FindControl("lblMOStatusTextInd"), Label).Visible = True
                    End If

                    EnableMOChangeIndicator(udtMO.DisplaySeq, udtComparator.GetMOChangedField(udtMO.DisplaySeq, udtSP))
                End If

            Next

            ' Compare Practice (5 items - Status of Practice, Medical Organization, Name of Practice, Address, Phone)
            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                Dim rPractice As GridViewRow = gvPracticeBank.Rows(udtPractice.DisplaySeq - 1)
                Dim lblPracticeStatus As Label = rPractice.FindControl("lblPracticeStatus")

                ' If the "Status of Practice" is New, just turn the Status to red
                If lblPracticeStatus.Text = strNew Then
                    lblPracticeStatus.ForeColor = Drawing.Color.Red
                    CType(rPractice.FindControl("lblPracticeStatusTextInd"), Label).Visible = True
                Else
                    If lblPracticeStatus.Text = strUnderAmendment Then
                        lblPracticeStatus.ForeColor = Drawing.Color.Red
                        CType(rPractice.FindControl("lblPracticeStatusTextInd"), Label).Visible = True
                    End If

                    EnablePracticeChangeIndicator(udtPractice.DisplaySeq, udtComparator.GetPracticeChangedField(udtPractice.DisplaySeq, udtSP))
                End If

                ' Compare Practice Scheme (2 items - Status, Service Fee)
                Dim gvPracticeSchemeInfo As GridView = CType(rPractice.FindControl("gvPracticeSchemeInfo"), GridView)
                For Each rPScheme As GridViewRow In gvPracticeSchemeInfo.Rows
                    ' If the "Status" is New, just turn the Status to red
                    Dim lblPracticeSchemeStatus As Label = CType(rPScheme.FindControl("lblPracticeSchemeStatus"), Label)
                    If lblPracticeSchemeStatus.Text = strNew Then
                        lblPracticeSchemeStatus.ForeColor = Drawing.Color.Red
                        CType(rPractice.FindControl("lblPracticeSchemeTextInd"), Label).Visible = True
                    Else
                        If lblPracticeSchemeStatus.Text = strUnderAmendment Then
                            lblPracticeSchemeStatus.ForeColor = Drawing.Color.Red
                        End If

                        If udtComparator.IsPracticeSchemeInfoChanged(udtPractice.DisplaySeq, CType(rPScheme.FindControl("hfPracticeSchemeCode"), HiddenField).Value, CType(rPScheme.FindControl("hfPracticeSubsidizeCode"), HiddenField).Value, udtSP) Then
                            CType(rPScheme.FindControl("lblPracticeServiceFee"), Label).ForeColor = Drawing.Color.Red
                            CType(rPractice.FindControl("lblPracticeSchemeTextInd"), Label).Visible = True
                        End If

                        Dim hfPracticeSchemeStatus As HiddenField = rPScheme.FindControl("hfPracticeSchemeStatus")

                        If hfPracticeSchemeStatus.Value.Trim = "U" OrElse hfPracticeSchemeStatus.Value.Trim = "A" Then
                            CType(rPScheme.FindControl("lblPracticeServiceFee"), Label).ForeColor = Drawing.Color.Red
                        End If
                    End If
                Next

            Next
        End If

    End Sub

    Public Sub buildSpProfileObject(ByVal udtSP As ServiceProviderModel, ByVal strTableLocation As String, Optional ByVal blnShowDuplicateMark As Boolean = False)
        ' Hidden fields for later usage
        hfERN.Value = udtSP.EnrolRefNo
        hfTableLocation.Value = strTableLocation.Trim
        hfShowDuplicateMark.Value = IIf(blnShowDuplicateMark = False, strNo, String.Empty)

        ' Reset the indicators in Personal Particulars
        lblNameInd.Visible = False
        lblAddressInd.Visible = False
        lblEmailInd.Visible = False
        lblContactNoInd.Visible = False
        lblFaxInd.Visible = False
        lblEnrolledSchemeTextInd.Visible = False

        ' Save the SP to session for usage in gridviews
        Select Case strTableLocation.Trim
            Case TableLocation.Enrolment
                Session(SESS_SPSummaryViewCurrentServiceProvider_Enrolment) = udtSP
            Case TableLocation.Staging
                Session(SESS_SPSummaryViewCurrentServiceProvider_Staging) = udtSP
            Case TableLocation.Permanent
                Session(SESS_SPSummaryViewCurrentServiceProvider) = udtSP
        End Select

        ' Enrolment Reference No.
        lblERN.Text = udtFormatter.formatSystemNumber(udtSP.EnrolRefNo.Trim)

        If udtSP.SPID = String.Empty Then
            ' Service Provider ID
            lblSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' Submission Time
            lblDateText.Text = Me.GetGlobalResourceObject("Text", "SubmissionDtmTime")
            lblDate.Text = udtFormatter.convertDateTime(udtSP.EnrolDate)

        Else
            ' Service Provider ID
            lblSPID.Text = udtSP.SPID + IIf(udtSP.UnderModification.Equals(String.Empty), "", " (Under Amendment)")

            ' Effective Time / Data Entry Processing Time
            Select Case strTableLocation
                Case TableLocation.Permanent
                    lblDateText.Text = Me.GetGlobalResourceObject("Text", "EffectiveTime")
                    lblDate.Text = udtFormatter.convertDateTime(udtSP.EffectiveDtm)

                Case TableLocation.Staging
                    lblDateText.Text = Me.GetGlobalResourceObject("Text", "DataEntryProcessingTime")
                    lblDate.Text = udtFormatter.convertDateTime(udtSP.CreateDtm)

            End Select

        End If

        ' Name
        lblEname.Text = udtSP.EnglishName
        lblCname.Text = udtFormatter.formatChineseName(udtSP.ChineseName)

        ' HKIC No.
        lblHKID.Text = udtFormatter.formatHKID(udtSP.HKID, False)

        ' Correspondence Address
        lblAddress.Text = udtFormatter.formatAddress(udtSP.SpAddress.Room, udtSP.SpAddress.Floor, udtSP.SpAddress.Block, _
                                                        udtSP.SpAddress.Building, udtSP.SpAddress.District, udtSP.SpAddress.AreaCode)

        ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
        ' Email Address
        lblEmail.Text = udtSP.Email

        ' Pending Email Address
        If strTableLocation = TableLocation.Permanent AndAlso udtSP.EmailChanged = EmailChanged.Changed Then
            trPendingEmail.Visible = True
            lblPendingEmail.Text = udtSP.TentativeEmail
            imgEditEmail.Visible = True

        Else
            trPendingEmail.Visible = False
            lblPendingEmail.Text = String.Empty
            imgEditEmail.Visible = False
        End If
        ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]

        ' Daytime Contact Phone No.
        lblContactNo.Text = udtSP.Phone

        ' Fax No.
        If udtSP.Fax.Equals(String.Empty) Then
            lblFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
        Else
            lblFax.Text = udtSP.Fax
        End If

        ' Service Provider Status - Complicated!!
        Select Case strTableLocation
            Case TableLocation.Permanent
                Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, udtSP.RecordStatus, lblSPStatus.Text, String.Empty)

                'lblSPStatusText.Visible = False
                'lblSPStatus.Visible = False

            Case TableLocation.Staging
                Dim udtSPAccUpdate As SPAccountUpdateModel

                If udtSPAccountUpateBLL.Exist Then
                    udtSPAccUpdate = udtSPAccountUpateBLL.GetSPAccountUpdate
                Else
                    udtSPAccUpdate = udtSPAccountUpateBLL.GetSPAccountUpdateByERN(udtSP.EnrolRefNo, New Database)

                    If IsNothing(udtSPAccUpdate) Then
                        udtSPAccUpdate = New SPAccountUpdateModel
                    End If
                End If

                If IsNothing(udtSPAccUpdate) _
                        OrElse IsNothing(udtSPAccUpdate.ProgressStatus) _
                        OrElse udtSPAccUpdate.ProgressStatus.Equals(String.Empty) Then
                    udtSPAccUpdate.ProgressStatus = SPAccountUpdateProgressStatus.DataEntryStage
                End If

                Dim strProgressStatus As String = String.Empty
                Status.GetDescriptionFromDBCode(SPAccountUpdateProgressStatus.ClassCode, udtSPAccUpdate.ProgressStatus, strProgressStatus, String.Empty)

                Dim strVerificationStatus As String = String.Empty

                Select Case udtSPAccUpdate.ProgressStatus
                    Case SPAccountUpdateProgressStatus.DataEntryStage
                        lblRecordStatus.Text = strProgressStatus

                    Case SPAccountUpdateProgressStatus.VettingStage
                        Dim udtSPVerificationModel As ServiceProviderVerificationModel

                        If udtSPVerificationBLL.Exist Then
                            udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
                        Else
                            udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(udtSP.EnrolRefNo, New Database)
                        End If

                        Status.GetDescriptionFromDBCode(ServiceProviderVerificationStatus.ClassCode, udtSPVerificationModel.RecordStatus, strVerificationStatus, String.Empty)

                        lblRecordStatus.Text = strVerificationStatus + " (" + strProgressStatus + ")"

                    Case SPAccountUpdateProgressStatus.BankAcctVerification
                        Dim dtBankVer As DataTable = udtSearchEngineBLL.SearchBankTSMP(udtSP.EnrolRefNo)

                        If dtBankVer.Rows.Count = 1 Then
                            strVerificationStatus = dtBankVer.Rows(0).Item("bankStatus")
                            lblRecordStatus.Text = strVerificationStatus + " (" + strProgressStatus + ")"
                        Else
                            lblRecordStatus.Text = strProgressStatus
                        End If

                    Case SPAccountUpdateProgressStatus.ProfessionalVerification, SPAccountUpdateProgressStatus.WaitingForIssueToken
                        lblRecordStatus.Text = strProgressStatus

                    Case Else
                        lblRecordStatus.Text = strProgressStatus

                End Select

                If udtSP.SPID.Equals(String.Empty) Then
                    lblSPStatusText.Visible = True
                    lblSPStatus.Visible = True
                    lblSPStatus.Text = "New Enrolment (" + strProgressStatus + ")"
                Else
                    lblSPStatusText.Visible = False
                    lblSPStatus.Visible = False
                End If

            Case TableLocation.Enrolment
                lblSPStatus.Text = "Unprocessed"

        End Select

        ' Account Status
        Dim dtAccountStatus As DataTable = udtSPProfileBLL.GetHCSPUserACStatus(udtSP.SPID, New Database)
        If dtAccountStatus.Rows.Count = 0 Then
            lblAccountStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
        Else
            Status.GetDescriptionFromDBCode(SPAccountStatus.ClassCode, dtAccountStatus.Rows(0)("UserAcc_RecordStatus").ToString.Trim, lblAccountStatus.Text, String.Empty)
        End If

        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
        ' PCD Status
        Dim strPCDStatusOutputText As String = String.Empty

        If Not udtSP.PCDAccountStatus = PCDAccountStatus.Unavailable Then

            If udtSP.PCDStatusLastCheckDtm IsNot Nothing Then
                strPCDStatusOutputText = String.Format("{0} ({1})", Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.GetPCDStatusDescByValue(udtSP.PCDAccountStatus, udtSP.PCDEnrolmentStatus), Me.GetGlobalResourceObject("Text", "PCDStatusLastCheck"))
                strPCDStatusOutputText = strPCDStatusOutputText.Replace("%s", udtFormatter.convertDate(udtSP.PCDStatusLastCheckDtm))
            Else
                strPCDStatusOutputText = Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.GetPCDStatusDescByValue(udtSP.PCDAccountStatus, udtSP.PCDEnrolmentStatus)
            End If
        End If
        DisplayPCDStatus(strPCDStatusOutputText, udtSP.PCDProfessional, False)
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

        ' Scheme Information
        If udtSP.SchemeInfoList.Values.Count = 0 Then
            lblEnrolledSchemeNA.Visible = True
            gvEnrolledScheme.Visible = False
        Else
            gvEnrolledScheme.DataSource = udtSP.SchemeInfoList.Values
            gvEnrolledScheme.DataBind()
            gvEnrolledScheme.Visible = True
            lblEnrolledSchemeNA.Visible = False
        End If

        If strTableLocation = TableLocation.Permanent Then
            Dim strRecordStatus As String = udtSP.RecordStatus.Trim

            ' If the SP is delisted, show the Token Return Date
            If strRecordStatus = ServiceProviderStatus.Delisted Then
                If udtSP.TokenReturnDtm.HasValue Then
                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'lblTokenReturn.Text = udtFormatter.formatDate(udtSP.TokenReturnDtm, String.Empty)
                    lblTokenReturn.Text = udtFormatter.formatDisplayDate(CDate(udtSP.TokenReturnDtm))
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                Else
                    lblTokenReturn.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                lblTokenReturnText.Visible = True
                lblTokenReturn.Visible = True

            Else
                lblTokenReturnText.Visible = False
                lblTokenReturn.Visible = False

            End If

            ' Token Serial No.

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

            Dim udtTokenModel As TokenModel = udtSPProfileBLL.GetTokenModelBySPID(udtSP.SPID, False)
            Dim strTokenSerialNoDisplay As String = String.Empty
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            imgShareToken.Visible = False
            imgShareToken.ToolTip = String.Empty
            imgTokenActivateDate.Visible = False
            imgTokenActivateDate.ToolTip = String.Empty
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            If Not IsNothing(udtTokenModel) Then
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'strTokenSerialNoDisplay = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNo, udtTokenModel.Project)

                '' CRE13-003 - Token Replacement [Start][Tommy L]
                '' -------------------------------------------------------------------------------------
                'If udtTokenModel.Project = TokenProjectType.EHCVS Then
                '    lblTokenIssueDate.Text = " (" + udtFormatter.convertDateTime(udtTokenModel.IssueDtm.Value) + ")"
                'Else
                '    lblTokenIssueDate.Text = ""
                'End If
                '' CRE13-003 - Token Replacement [End][Tommy L]

                If udtTokenModel.Project = TokenProjectType.EHCVS Then
                    strTokenSerialNoDisplay = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNo, udtTokenModel.Project, False, False, True)
                    imgTokenActivateDate.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "TokenActivateDate").ToString.Replace("%s", udtFormatter.convertDateTime(udtTokenModel.IssueDtm.Value).ToString)
                    imgTokenActivateDate.Style.Add("vertical-align", "text-top")
                    imgTokenActivateDate.Visible = True
                Else
                    strTokenSerialNoDisplay = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNo, udtTokenModel.Project, False, True, True)
                    imgTokenActivateDate.Visible = False
                End If

                If udtTokenModel.IsShareToken Then
                    imgShareToken.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "ShareToken").ToString
                    imgShareToken.Style.Add("vertical-align", "text-top")
                    imgShareToken.Visible = True
                Else
                    imgShareToken.Visible = False
                End If
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

            Else
                strTokenSerialNoDisplay = Me.GetGlobalResourceObject("Text", "N/A")
                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblTokenIssueDate.Text = ""
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                ' CRE13-003 - Token Replacement [End][Tommy L]
            End If

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            imgShareTokenReplacement.Visible = False
            imgShareTokenReplacement.ToolTip = String.Empty
            imgTokenAssignDate.Visible = False
            imgTokenAssignDate.ToolTip = String.Empty
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            If IsNothing(udtTokenModel) OrElse udtTokenModel.TokenSerialNoReplacement.Equals(String.Empty) Then
                lblTokenReplacedSNText.Visible = False
                lblTokenReplacedSN.Visible = False
                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblTokenReplacedDate.Visible = False
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                ' CRE13-003 - Token Replacement [End][Tommy L]
            Else
                lblTokenReplacedSNText.Visible = True
                lblTokenReplacedSN.Visible = True

                ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Not to display PPI-ePR token serial no.

                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblTokenReplacedSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNoReplacement.Trim, udtTokenModel.Project)
                '' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [End][Koala]
                '' CRE13-003 - Token Replacement [Start][Tommy L]
                '' -------------------------------------------------------------------------------------
                'If udtTokenModel.Project = TokenProjectType.EHCVS Then
                '    lblTokenReplacedDate.Visible = True
                '    lblTokenReplacedDate.Text = " (" + udtFormatter.convertDateTime(udtTokenModel.LastReplacementDtm.Value) + ")"
                'Else
                '    lblTokenReplacedDate.Visible = False
                'End If
                '' CRE13-003 - Token Replacement [End][Tommy L]

                If udtTokenModel.ProjectReplacement = TokenProjectType.EHCVS Then
                    lblTokenReplacedSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNoReplacement.Trim, udtTokenModel.ProjectReplacement, False, False, True)
                    imgTokenAssignDate.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "TokenAssignDate").ToString.Replace("%s", udtFormatter.convertDateTime(udtTokenModel.LastReplacementDtm.Value).ToString)
                    imgTokenAssignDate.Style.Add("vertical-align", "text-top")
                    imgTokenAssignDate.Visible = True
                Else
                    lblTokenReplacedSN.Text = TokenModel.DisplayTokenSerialNo(udtTokenModel.TokenSerialNoReplacement.Trim, udtTokenModel.ProjectReplacement, False, True, True)
                    imgTokenAssignDate.Visible = False
                End If

                If udtTokenModel.IsShareTokenReplacement Then
                    imgShareTokenReplacement.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "ShareToken").ToString
                    imgShareTokenReplacement.Style.Add("vertical-align", "text-top")
                    imgShareTokenReplacement.Visible = True
                Else
                    imgShareTokenReplacement.Visible = False
                End If

                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            End If

            ' Check the token status (Pending Deactivate and Pending Activate)
            Dim strTokenUpdateStatus As String = String.Empty

            For Each dr As DataRow In udtAccountChangeMaintenanceBLL.GetRecordDataTableByKeyValue(udtSP.SPID, "DT").Rows
                strTokenUpdateStatus = TokenPendingStatus.PendingDeactivate
            Next

            If strTokenUpdateStatus = String.Empty Then
                For Each dr As DataRow In udtAccountChangeMaintenanceBLL.GetRecordDataTableByKeyValue(udtSP.SPID, "AT").Rows
                    strTokenUpdateStatus = TokenPendingStatus.PendingReactivate
                Next
            End If

            ' CRE13-003 - Token Replacement [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            lblTokenRemark.Text = ""
            ' CRE13-003 - Token Replacement [End][Tommy L]

            Select Case strTokenUpdateStatus
                Case TokenPendingStatus.PendingDeactivate, TokenPendingStatus.PendingReactivate
                    Dim strTokenUpdateStatusText As String = Nothing
                    Status.GetDescriptionFromDBCode(TokenPendingStatus.Classcode, strTokenUpdateStatus, strTokenUpdateStatusText, String.Empty)

                    ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

                    ' CRE13-003 - Token Replacement [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                    'strTokenSerialNoDisplay += " (" + strTokenUpdateStatusText + " Token)"
                    lblTokenRemark.Text = " (" + strTokenUpdateStatusText + " Token)"
                    ' CRE13-003 - Token Replacement [End][Tommy L]

                    ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

            End Select

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

            lblTokenSN.Text = strTokenSerialNoDisplay

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

            ' Web Account Username
            If udtSP.AliasAccount.Equals(String.Empty) Then
                lblSPUsernameText.Visible = False
                lblSPUsername.Visible = False
            Else
                lblSPUsernameText.Visible = True
                lblSPUsername.Visible = True
                lblSPUsername.Text = udtSP.AliasAccount
            End If

        Else
            ' In Staging and Enrolment tables, the following 4 items do not need to show
            lblTokenSNText.Visible = False
            lblTokenSN.Visible = False
            ' CRE13-003 - Token Replacement [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblTokenIssueDate.Visible = False
            imgShareToken.Visible = False
            imgTokenActivateDate.Visible = False
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            lblTokenRemark.Visible = False
            ' CRE13-003 - Token Replacement [End][Tommy L]

            lblTokenReplacedSNText.Visible = False
            lblTokenReplacedSN.Visible = False
            ' CRE13-003 - Token Replacement [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblTokenReplacedDate.Visible = False
            imgShareTokenReplacement.Visible = False
            imgTokenAssignDate.Visible = False
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
            ' CRE13-003 - Token Replacement [End][Tommy L]

            lblTokenReturnText.Visible = False
            lblTokenReturn.Visible = False

            lblSPUsernameText.Visible = False
            lblSPUsername.Visible = False

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            lblPCDStatusText.Visible = False
            lblPCDStatus.Visible = False

            lblPCDProfessionalText.Visible = False
            lblPCDProfessional.Visible = False
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
        End If

        ' Medical Organization Information
        If udtSP.MOList.Values.Count = 0 Then
            lblMONA.Visible = True
            gvMO.Visible = False
        Else
            gvMO.DataSource = udtSP.MOList.Values
            gvMO.DataBind()
            lblMONA.Visible = False
            gvMO.Visible = True
        End If

        ' Practice and Bank Information
        If udtSP.PracticeList.Values.Count = 0 Then
            lblPracticeBankNA.Visible = True
            gvPracticeBank.Visible = False
        Else
            gvPracticeBank.DataSource = udtSP.PracticeList.Values
            gvPracticeBank.DataBind()
            gvPracticeBank.Visible = True
            lblPracticeBankNA.Visible = False
        End If

    End Sub

    Public Sub DisplayRecordStatus(ByVal blnShowRecordStatus As Boolean, ByVal strTableLocation As String)
        Dim blnShowSPStatus As Boolean

        Select Case strTableLocation
            Case TableLocation.Permanent, TableLocation.Enrolment
                blnShowSPStatus = True
            Case TableLocation.Staging
                blnShowSPStatus = Not blnShowRecordStatus
        End Select

        ' Only one of "RecordStatus" or "SPStatus" can be shown, and "RecordStatus" is shown, "SPStatus" cannot be shown, and vice verse
        lblRecordStatusText.Visible = Not blnShowSPStatus
        lblRecordStatus.Visible = Not blnShowSPStatus

        lblSPStatus.Visible = blnShowSPStatus
        lblSPStatusText.Visible = blnShowSPStatus

    End Sub

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Sub DisplayPCDStatus(ByVal strPCDStatus As String, ByVal strPCDProfessional As String, ByVal blnHighlight As Boolean)

        If Not strPCDStatus.Equals(String.Empty) Then
            lblPCDStatusText.Visible = True
            lblPCDStatus.Visible = True
            lblPCDStatus.Text = strPCDStatus

            lblPCDProfessionalText.Visible = True
            lblPCDProfessional.Visible = True

            lblPCDProfessional.Text = Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.GetPCDProfessionalDescByValue(strPCDProfessional)

            If blnHighlight Then
                lblPCDStatus.ForeColor = Drawing.Color.Red
            Else
                lblPCDStatus.ForeColor = Drawing.Color.Empty
            End If

        Else
            lblPCDStatusText.Visible = False
            lblPCDStatus.Visible = False

            lblPCDProfessionalText.Visible = False
            lblPCDProfessional.Visible = False
        End If

    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    ' Change indicator (*)

    Private Sub EnableIndicator(ByVal blnEnable As Boolean, ByVal strField As String)
        Dim lblControl As Label = Nothing

        If blnEnable Then
            Select Case strField
                Case ServiceProviderComparator.EnglishName, ServiceProviderComparator.ChineseName
                    lblControl = lblNameInd
                Case ServiceProviderComparator.SpAddress
                    lblControl = lblAddressInd
                Case ServiceProviderComparator.Email
                    lblControl = lblEmailInd
                Case ServiceProviderComparator.Phone
                    lblControl = lblContactNoInd
                Case ServiceProviderComparator.Fax
                    lblControl = lblFaxInd
                Case Else
                    Return
            End Select

            lblControl.Visible = blnEnable
        Else
            Return
        End If
    End Sub

    Private Sub EnableMOChangeIndicator(ByVal intSeq As Integer, ByVal dicMOChanged As Dictionary(Of String, Boolean))
        Dim r As GridViewRow = gvMO.Rows(intSeq - 1)

        If dicMOChanged(ServiceProviderComparator.Phone) Then CType(r.FindControl("lblMOContactNoInd"), Label).Visible = True
        If dicMOChanged(ServiceProviderComparator.Email) Then CType(r.FindControl("lblMOEmailInd"), Label).Visible = True
        If dicMOChanged(ServiceProviderComparator.Fax) Then CType(r.FindControl("lblMOFaxInd"), Label).Visible = True
        If dicMOChanged(ServiceProviderComparator.Address) Then CType(r.FindControl("lblMOAddressInd"), Label).Visible = True

    End Sub

    Private Sub EnablePracticeChangeIndicator(ByVal intSeq As Integer, ByVal dicPracticeChanged As Dictionary(Of String, Boolean))
        Dim r As GridViewRow = gvPracticeBank.Rows(intSeq - 1)

        If dicPracticeChanged(ServiceProviderComparator.MedicalOrganization) Then CType(r.FindControl("lblPracticeMOTextInd"), Label).Visible = True
        If dicPracticeChanged(ServiceProviderComparator.NameOfPractice) Then CType(r.FindControl("lblPracticeNameTextInd"), Label).Visible = True
        If dicPracticeChanged(ServiceProviderComparator.Address) Then CType(r.FindControl("lblPracticeAddressInd"), Label).Visible = True
        If dicPracticeChanged(ServiceProviderComparator.Phone) Then CType(r.FindControl("lblPracticePhoneInd"), Label).Visible = True

    End Sub

    '

    Protected Sub gvEnrolledScheme_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEnrolledScheme.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Scheme Name
            Dim lblESchemeName As Label = e.Row.FindControl("lblESchemeName")
            For Each udtMScheme As SchemeBackOfficeModel In udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup
                If udtMScheme.SchemeCode.Trim = lblESchemeName.Text.Trim Then
                    lblESchemeName.Text = udtMScheme.DisplayCode.Trim
                    Exit For
                End If
            Next

            ' Status [Remarks]
            gvEnrolledScheme.HeaderRow.Cells(1).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

            Dim lblERecordStatus As Label = CType(e.Row.FindControl("lblERecordStatus"), Label)
            Select Case hfTableLocation.Value
                Case TableLocation.Permanent
                    Status.GetDescriptionFromDBCode(SchemeInformationMaintenanceDisplayStatus.ClassCode, lblERecordStatus.Text.Trim, lblERecordStatus.Text, String.Empty)
                Case TableLocation.Staging
                    Status.GetDescriptionFromDBCode(SchemeInformationStagingStatus.ClassCode, lblERecordStatus.Text.Trim, lblERecordStatus.Text, String.Empty)
                Case TableLocation.Enrolment
                    lblERecordStatus.Text = strNew
            End Select

            Dim lblERemark As Label = CType(e.Row.FindControl("lblERemark"), Label)
            If lblERemark.Text.Trim <> String.Empty Then
                lblERecordStatus.Text += " [" + lblERemark.Text.Trim + "]"
            End If

            ' Effective Date
            Dim lblEEffectiveDtm As Label = CType(e.Row.FindControl("lblEEffectiveDtm"), Label)
            If lblEEffectiveDtm.Text.Trim = String.Empty Then
                lblEEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblEEffectiveDtm.Text = udtFormatter.convertDateTime(lblEEffectiveDtm.Text.Trim)
            End If

            ' Delisting Date
            Dim lblEDelistDtm As Label = CType(e.Row.FindControl("lblEDelistDtm"), Label)
            If lblEDelistDtm.Text.Trim = String.Empty Then
                lblEDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblEDelistDtm.Text = udtFormatter.convertDateTime(lblEDelistDtm.Text.Trim)
            End If

            ' Logo Return Date
            Dim lblELogoReturnDate As Label = CType(e.Row.FindControl("lblELogoReturnDate"), Label)
            If lblELogoReturnDate.Text.Trim = String.Empty Then
                lblELogoReturnDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblELogoReturnDate.Text = udtFormatter.formatDate(Convert.ToDateTime(lblELogoReturnDate.Text.Trim))
                lblELogoReturnDate.Text = udtFormatter.formatDisplayDate(Convert.ToDateTime(lblELogoReturnDate.Text.Trim))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

        End If

    End Sub

    Protected Sub gvMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMO.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Email Address
            Dim lblMOEmail As Label = CType(e.Row.FindControl("lblMOEmail"), Label)
            If lblMOEmail.Text.Trim = String.Empty Then
                lblMOEmail.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            ' Fax No.
            Dim lblMOFax As Label = CType(e.Row.FindControl("lblMOFax"), Label)
            If lblMOFax.Text.Trim = String.Empty Then
                lblMOFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            ' Medical Organization Status
            Dim lblMOStatus As Label = CType(e.Row.FindControl("lblMOStatus"), Label)
            Select Case hfTableLocation.Value
                Case TableLocation.Permanent
                    Status.GetDescriptionFromDBCode(MedicalOrganizationStatus.ClassCode, lblMOStatus.Text.Trim, lblMOStatus.Text, String.Empty)
                Case TableLocation.Staging
                    Status.GetDescriptionFromDBCode(MedicalOrganizationStagingStatus.ClassCode, lblMOStatus.Text.Trim, lblMOStatus.Text, String.Empty)
                Case TableLocation.Enrolment
                    lblMOStatus.Text = strNew
            End Select

            ' Duplicate Mark
            If hfShowDuplicateMark.Value = strNo Then CType(e.Row.FindControl("ibtnDuplicateMO"), ImageButton).Visible = False
        End If
    End Sub

    Protected Sub gvPracticeBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPracticeBank.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Convert Medical Organization No. to Medical Organization Name
            Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblPracticeMO"), Label)
            Dim intMODisplaySeq As Integer = CInt(lblPracticeMO.Text.Trim)

            If Not IsNothing(gvMO.DataSource) Then
                For Each udtMO As MedicalOrganization.MedicalOrganizationModel In gvMO.DataSource
                    If udtMO.DisplaySeq.Value = intMODisplaySeq Then
                        lblPracticeMO.Text = udtMO.DisplaySeqMOName
                        Exit For
                    End If
                Next
            End If

            If lblPracticeMO.Text = strMONotAvailable Then lblPracticeMO.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' Phone No. of Practice
            Dim lblPracticePhone As Label = e.Row.FindControl("lblPracticePhone")
            If lblPracticePhone.Text = String.Empty Then lblPracticePhone.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' Status of Practice
            Dim lblPracticeStatus As Label = CType(e.Row.FindControl("lblPracticeStatus"), Label)
            Select Case hfTableLocation.Value
                Case TableLocation.Permanent
                    Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblPracticeStatus.Text.Trim, lblPracticeStatus.Text, String.Empty)
                Case TableLocation.Staging
                    Status.GetDescriptionFromDBCode(PracticeStagingStatus.ClassCode, lblPracticeStatus.Text.Trim, lblPracticeStatus.Text, String.Empty)
                Case TableLocation.Enrolment
                    lblPracticeStatus.Text = strNew
            End Select

            ' If there is no schemes, show "N/A"
            'PracticeSchemeInfoList.Values()
            Dim udtPractice As PracticeModel = e.Row.DataItem
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList
            Dim gvPracticeSchemeInfo As GridView = e.Row.FindControl("gvPracticeSchemeInfo")

            If IsNothing(udtPracticeSchemeInfoList) OrElse udtPracticeSchemeInfoList.Count = 0 Then
                DirectCast(e.Row.FindControl("lblPracticeSchemeInfoNA"), Label).Visible = True
                gvPracticeSchemeInfo.Visible = False

            Else
                Session(SESS_SPSummaryViewPracticeSchemeInfoList) = udtPracticeSchemeInfoList

                gvPracticeSchemeInfo.DataSource = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache.ToSPProfileDataTable
                gvPracticeSchemeInfo.DataBind()

            End If

            ' If there is no banks, show "N/A"
            If CType(e.Row.FindControl("lblBankName"), Label).Text = String.Empty Then
                CType(e.Row.FindControl("pnlBankNA"), Panel).Visible = True
            Else
                CType(e.Row.FindControl("pnlBank"), Panel).Visible = True
            End If

            ' Duplicate Mark
            If hfShowDuplicateMark.Value = strNo Then CType(e.Row.FindControl("ibtnDuplicatePractice"), ImageButton).Visible = False

        End If
    End Sub

    '

    Protected Sub gvPracticeSchemeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Select Case e.Row.RowType
            Case DataControlRowType.DataRow
                Dim strSchemeCode As String = DirectCast(e.Row.FindControl("hfPracticeSchemeCode"), HiddenField).Value
                Dim strSubsidizeCode As String = DirectCast(e.Row.FindControl("hfPracticeSubsidizeCode"), HiddenField).Value

                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Session(SESS_SPSummaryViewPracticeSchemeInfoList)
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

                ' Scheme Code
                If udtPracticeSchemeInfo.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                    Dim lblPracticeSchemeCode As Label = e.Row.FindControl("lblPracticeSchemeCode")
                    lblPracticeSchemeCode.Text += String.Format("<br />({0})", Me.GetGlobalResourceObject("Text", "NonClinic"))
                End If


                ' Service Fee
                Dim udtSubsidizeGpBO As SubsidizeGroupBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup.Filter(strSchemeCode).SubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                Dim lblPracticeServiceFee As Label = CType(e.Row.FindControl("lblPracticeServiceFee"), Label)

                If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                    If udtPracticeSchemeInfo.ProvideServiceFee.Value AndAlso udtPracticeSchemeInfo.ServiceFee.HasValue Then
                        lblPracticeServiceFee.Text = udtFormatter.formatMoney(udtPracticeSchemeInfo.ServiceFee, True)

                    Else
                        lblPracticeServiceFee.Text = udtSubsidizeGpBO.ServiceFeeCompulsoryWording

                    End If

                Else
                    If udtSubsidizeGpBO.ServiceFeeEnabled Then
                        lblPracticeServiceFee.Text = "--"
                    Else
                        lblPracticeServiceFee.Text = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
                    End If

                End If

                ' Status [Remark]
                Dim lblPracticeSchemeStatus As Label = CType(e.Row.FindControl("lblPracticeSchemeStatus"), Label)

                Dim intTargetPracticeSeq As Integer = udtPracticeSchemeInfo.PracticeDisplaySeq
                Dim udtTargetPractice As PracticeModel = Nothing

                For Each udtPractice As PracticeModel In gvPracticeBank.DataSource
                    If udtPractice.DisplaySeq = intTargetPracticeSeq Then
                        udtTargetPractice = udtPractice
                        Exit For
                    End If
                Next

                lblPracticeSchemeStatus.Text = udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtTargetPractice, strSchemeCode, hfTableLocation.Value)

                Dim lblPracticeSchemeRemark As Label = CType(e.Row.FindControl("lblPracticeSchemeRemark"), Label)
                ' INT16-0007 Fix HCVU SPEnquiry cannot show Practice Scheme Remark [Start][Lawrence]
                lblPracticeSchemeRemark.Text = udtPracticeSchemeInfo.Remark
                ' INT16-0007 Fix HCVU SPEnquiry cannot show Practice Scheme Remark [End][Lawrence]

                Select Case hfTableLocation.Value
                    Case TableLocation.Permanent
                        Status.GetDescriptionFromDBCode(PracticeSchemeInfoMaintenanceDisplayStatus.ClassCode, lblPracticeSchemeStatus.Text.Trim, lblPracticeSchemeStatus.Text, String.Empty)

                    Case TableLocation.Staging

                        'CRE15-004 TIV & QIV [Start][Winnie]
                        'Clear Remark if scheme is under admendment
                        If lblPracticeSchemeStatus.Text = PracticeSchemeInfoStagingStatus.Update Then
                            lblPracticeSchemeRemark.Text = String.Empty
                        End If
                        'CRE15-004 TIV & QIV [End][Winnie]   

                        Status.GetDescriptionFromDBCode(PracticeSchemeInfoStagingStatus.ClassCode, lblPracticeSchemeStatus.Text.Trim, lblPracticeSchemeStatus.Text, String.Empty)
                    Case TableLocation.Enrolment
                        lblPracticeSchemeStatus.Text = strNew
                End Select

                If lblPracticeSchemeRemark.Text.Trim <> String.Empty Then
                    lblPracticeSchemeRemark.Text = "[" + lblPracticeSchemeRemark.Text.Trim + "]"
                End If

                ' Effective Time & Delisting Time
                Dim lblPracticeSchemeEffectiveDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeEffectiveDtm"), Label)
                Dim lblPracticeSchemeDelistDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeDelistDtm"), Label)

                udtSPProfileBLL.GetPracticeSchemeInfoEarliestTime(udtPracticeSchemeInfoList, strSchemeCode,
                                                                  lblPracticeSchemeEffectiveDtm.Text, lblPracticeSchemeDelistDtm.Text)

                If lblPracticeSchemeEffectiveDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                If lblPracticeSchemeDelistDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

            Case DataControlRowType.Header
                e.Row.Cells(1).ColumnSpan = 2
                e.Row.Cells(2).Visible = False

                ' Change Header Text: Status -> Status [Remark]
                e.Row.Cells(3).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

        End Select

    End Sub

    Protected Sub gvPracticeSchemeInfo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvPracticeSchemeInfo As GridView = sender

        ' Handle Category
        For Each gvr As GridViewRow In gvPracticeSchemeInfo.Rows
            If DirectCast(gvr.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
                ' Check whether this category is visible
                Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfPracticeSchemeCode"), HiddenField).Value
                Dim strCategoryName As String = DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value
                Dim blnVisible As Boolean = False

                For Each r As GridViewRow In gvPracticeSchemeInfo.Rows
                    If DirectCast(r.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "N" _
                            AndAlso DirectCast(r.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode _
                            AndAlso DirectCast(r.FindControl("hfGCategoryName"), HiddenField).Value = strCategoryName _
                            AndAlso r.Visible Then
                        blnVisible = True
                        Exit For
                    End If

                Next

                If blnVisible Then
                    ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
                    gvr.Cells(1).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value, True)
                    ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
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

            Dim strSchemeCode As String = DirectCast(gvr.FindControl("hfPracticeSchemeCode"), HiddenField).Value

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
                        If DirectCast(gvrow.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode Then
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
                    If DirectCast(gvrow.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode AndAlso DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
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
                    If DirectCast(gvrow.FindControl("hfPracticeSchemeCode"), HiddenField).Value = strSchemeCode AndAlso DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
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

    End Sub

    '

    Public Shared Sub GridViewGroupMasterSchemeCode(ByRef gv As GridView, ByVal aryIntColumn_Code As Integer(), ByVal aryIntColumn_Merge As Integer(), ByVal strLblScheme As String, ByVal strLblSubsidize As String)
        If gv.Rows.Count = 0 Then Return

        ' If previously grouped, no need to run again (checked by the first cell in header row - ColumnSpan 2)
        If gv.HeaderRow.Cells(aryIntColumn_Code(0)).ColumnSpan = 2 Then Return

        Dim intRow As Integer = 0
        Dim intLastRow As Integer = -1
        Dim strLastSchemeCode As String = String.Empty

        ' Change the heading
        gv.HeaderRow.Cells(aryIntColumn_Code(0)).ColumnSpan = 2
        gv.HeaderRow.Cells(aryIntColumn_Code(1)).Visible = False

        ' Get the SchemeBackOfficeModelCollection first (to avoid over-accessing SQL or cache)
        Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
        Dim udtSchemeBOList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup()

        Dim aryColumnSpanMScheme As New ArrayList

        For Each r As GridViewRow In gv.Rows

            'CRE15-004 TIV & QIV [Start][Winnie]
            If Not r.Visible Then
                intRow += 1
                Continue For
            End If
            'CRE15-004 TIV & QIV [End][Winnie]
            Dim strCurrentSchemeCode As String = CType(r.FindControl(strLblScheme), Label).Text.Trim

            If udtSchemeBOList.Filter(strCurrentSchemeCode).DisplaySubsidizeDesc = False Then
                If aryColumnSpanMScheme.Contains(strCurrentSchemeCode) Then
                    r.Visible = False

                    intRow += 1
                    Continue For

                Else
                    r.Cells(aryIntColumn_Code(0)).ColumnSpan = 2
                    r.Cells(aryIntColumn_Code(1)).Visible = False

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

    ' Duplicated MO / Practices popup

    Protected Sub ibtnDuplicateMO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim ibtnDuplicateMO As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnDuplicateMO.NamingContainer

        If Not row Is Nothing Then
            Dim lblMODispalySeq As Label = CType(row.FindControl("lblMOIndex"), Label)

            Dim udtSP As ServiceProviderModel = Nothing

            Select Case hfTableLocation.Value
                Case TableLocation.Enrolment
                    udtSP = Session(SESS_SPSummaryViewCurrentServiceProvider_Enrolment)
                Case TableLocation.Staging
                    udtSP = Session(SESS_SPSummaryViewCurrentServiceProvider_Staging)
                Case TableLocation.Permanent
                    udtSP = Session(SESS_SPSummaryViewCurrentServiceProvider)
            End Select

            MOPracticeLists1.buildMOObject(udtSP.MOList, CInt(lblMODispalySeq.Text.Trim), hfTableLocation.Value)

            ModalPopupExtenderDuplicated.Show()

        End If
    End Sub

    Protected Sub ibtnDuplicatePractice_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim ibtnDuplicatePractice As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnDuplicatePractice.NamingContainer

        If Not row Is Nothing Then
            Dim lblPracticeDispalySeq As Label = CType(row.FindControl("lblPracticeBankIndex"), Label)

            Dim udtSP As ServiceProviderModel = Nothing

            Select Case Me.hfTableLocation.Value
                Case TableLocation.Enrolment
                    udtSP = Session(SESS_SPSummaryViewCurrentServiceProvider_Enrolment)
                Case TableLocation.Staging
                    udtSP = Session(SESS_SPSummaryViewCurrentServiceProvider_Staging)
                Case TableLocation.Permanent
                    udtSP = Session(SESS_SPSummaryViewCurrentServiceProvider)
            End Select

            MOPracticeLists1.buildPracticeObject(udtSP.PracticeList, CInt(lblPracticeDispalySeq.Text.Trim), hfTableLocation.Value)

            ModalPopupExtenderDuplicated.Show()

        End If
    End Sub

    Protected Sub ibtnDuplicatedClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Nothing here
    End Sub

    ' Used in .aspx

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If IsNothing(strPracticeCode) Then
            strPracticeTypeName = String.Empty
        Else
            If strPracticeCode.Equals(String.Empty) Then
                strPracticeTypeName = String.Empty
            Else
                If Session("language") = "zh-tw" Then
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValueChi
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

                If Session("language") = "zh-tw" Then
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
                Else
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc
                End If

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            End If
        End If

        Return strHealthProfName
    End Function

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return udtFormatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
    End Function

    Protected Function formatAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddress(udtAddressModel)
    End Function

    Protected Function formatChiAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddressChi(udtAddressModel)
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return udtFormatter.formatChineseName(strChineseString)
    End Function

    Protected Function formatDate(ByVal d As Object) As String
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Return udtFormatter.formatDate(Convert.ToDateTime(d))
        Return udtFormatter.formatDisplayDate(Convert.ToDateTime(d))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
    End Function

End Class