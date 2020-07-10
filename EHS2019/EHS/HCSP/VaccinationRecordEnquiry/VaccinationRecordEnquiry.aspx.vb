Imports System.Net

Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.DataEntryUser
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL
Imports IdeasRM

Partial Public Class VaccinationRecordEnquiry
    Inherits BasePageWithGridView

    ' FunctionCode = FunctCode.FUNT020801

#Region "Private Class"

    Private Class ViewIndex
        Public Const NoVaccinationScheme As Integer = 0
        Public Const Search As Integer = 1
        Public Const InputRecipientInformation As Integer = 2
        Public Const InputRecipientInformationSmartID As Integer = 3
        Public Const ConfirmRecipientInformation As Integer = 4
        Public Const Result As Integer = 5
    End Class

    Private Class ViewIndexDOB
        Public Const Normal As Integer = 0
        Public Const HKBC As Integer = 1
        Public Const EC As Integer = 2
    End Class

    Private Class SESS
        Public Const EHSAccount As String = "020801_EHSAccount"
        Public Const SearchAccountRemark As String = "020801_SearchEHSAccountRemark"
    End Class

    Private Class AuditLogDescription
        Public Const Load As String = "Vaccination Record Enquiry load" ' 00000
        Public Const ChangeDocument As String = "Change Document" ' 00001
        Public Const DocumentLegendClick As String = "Document Legend click" ' 00002
        Public Const SearchClick As String = "Search click" ' 00003
        Public Const SearchStart As String = "Search start" ' 00004
        Public Const SearchComplete As String = "Search complete" ' 00006
        Public Const SearchFail As String = "Search fail" ' 00007
        Public Const InputRecipientInformationNext As String = "Input recipient information next click" ' 00008
        Public Const InputRecipientInformationNextSuccessful As String = "Input recipient information next successful" ' 00009
        Public Const InputRecipientInformationNextFail As String = "Input recipient information next fail" ' 00010
        Public Const InputRecipientInformationCancel As String = "Input recipient information cancel click" ' 00011
        Public Const ConfirmRecipientInformationProceed As String = "Confirm recipient information proceed click" ' 00012
        Public Const ConfirmRecipientInformationBack As String = "Confirm recipient information back click" ' 00013
        Public Const ResultInfoButton As String = "Result info button click" ' 00014
        Public Const ResultReturn As String = "Result return click" ' 00015
        Public Const ResultProceedToClaim As String = "Result proceed to claim click" ' 00016
        Public Const InputRecipientInformationSmartICChangeGender As String = "Input recipient information (Smart IC) change gender" ' 00017
        Public Const InputRecipientInformationSmartICCancel As String = "Input recipient information (Smart IC) cancel" ' 00018
        Public Const InputRecipientInformationSmartICProceedToEnquiry As String = "Input recipient information (Smart IC) proceed to enquiry" ' 00019
    End Class

    <Serializable()> Private Class SearchAccountRemarkModel
        Private _blnValidatedAccountButDifferentDocCodeFound As Boolean

        Public Sub New()
            _blnValidatedAccountButDifferentDocCodeFound = False
        End Sub

        Public Property ValidatedAccountButDifferentDocCodeFound() As Boolean
            Get
                Return _blnValidatedAccountButDifferentDocCodeFound
            End Get
            Set(ByVal value As Boolean)
                _blnValidatedAccountButDifferentDocCodeFound = value
            End Set
        End Property

    End Class

#End Region

#Region "Variables"
    Private udtSessionHandler As New SessionHandler

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Check session expire
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()

        Dim blnSmartIDError As Boolean = False

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT020801

            ' Check reading Smart ID

            Dim udtSmartIDContent As SmartIDContentModel = udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)

            If ReadSmartID(udtSmartIDContent, blnSmartIDError) Then Return

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.Load)

            Session.Remove(SESS.EHSAccount)

            ' Check available scheme
            Dim udtSP As ServiceProviderModel = Nothing
            Dim aryDataEntryPracticeList As ArrayList = Nothing

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtSP = udtUserAC
            Else
                Dim udtDataEntry As DataEntryUserModel = udtUserAC
                udtSP = udtDataEntry.ServiceProvider
                aryDataEntryPracticeList = udtDataEntry.PracticeList
            End If

            If IsSPContainVaccinationScheme(udtSP, aryDataEntryPracticeList) Then
                MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Search

            Else
                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.BuildMessageBox()

                MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.NoVaccinationScheme

            End If

        End If

        Select Case MultiViewVaccinationRecordEnquiry.ActiveViewIndex
            Case ViewIndex.Search
                If blnSmartIDError Then
                    BindControl(False)
                Else
                    BindControl(True)
                End If

            Case ViewIndex.InputRecipientInformation
                BindInputRecipientInformation(Session(SESS.EHSAccount))

            Case ViewIndex.InputRecipientInformationSmartID
                SetupInputRecipientInformationSmartID(Session(SESS.EHSAccount), (New SessionHandler).SmartIDContentGetFormSession(FunctionCode))

            Case ViewIndex.ConfirmRecipientInformation
                SetupConfirmRecipientInformation(Session(SESS.EHSAccount))

            Case ViewIndex.Result
                udcVaccinationRecord.BuildEHSAccount(Session(SESS.EHSAccount))
                udcVaccinationRecord.RebuildVaccinationRecordGrid()

        End Select

    End Sub

    Private Function IsSPContainVaccinationScheme(ByVal udtSP As ServiceProviderModel, ByVal aryDataEntryPracticeList As ArrayList) As Boolean
        ' Get all SP Scheme
        Dim udtSchemeList As SchemeInformationModelCollection = udtSP.SchemeInfoList

        ' Get all Practice Scheme
        Dim udtFilterPracticeSchemeList As New PracticeSchemeInfoModelCollection

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If Not IsNothing(aryDataEntryPracticeList) Then
                If Not aryDataEntryPracticeList.Contains(udtPractice.DisplaySeq) Then Continue For
            End If

            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                Dim strPracticeSchemeStatus As String = udtPracticeScheme.RecordStatus
                Dim strSchemeStatus As String = udtSchemeList.Filter(udtPracticeScheme.SchemeCode).RecordStatus

                If (strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active _
                            OrElse strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend _
                            OrElse strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist) _
                        AndAlso (strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.Active _
                            OrElse strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend _
                            OrElse strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist) Then
                    udtFilterPracticeSchemeList.Add(udtPracticeScheme)
                End If

            Next

        Next

        ' Convert Practice Scheme to Scheme Claim
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtFilterPracticeSchemeList)

        ' Get all Scheme Claim with Subsidize Group Claim as the udtSchemeClaimList does not contain subsidize group list
        Dim udtMasterSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup()

        ' Check the Scheme Claim contains vaccine
        For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
            For Each udtSubsidize As SubsidizeGroupClaimModel In udtMasterSchemeClaimList.Filter(udtSchemeClaim.SchemeCode).SubsidizeGroupClaimList
                If udtSubsidize.SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then Return True
            Next
        Next

        Return False

    End Function

    Private Sub BindControl(ByVal blnReset As Boolean)
        ' --- Reset message box ---
        If blnReset Then
            ResetMessageBox()
            udcClaimSearch.SetHKICError(False)
            udcClaimSearch.SetECError(False)
            udcClaimSearch.SetADOPCError(False)
            udcClaimSearch.SetSearchShortError(False)
        End If

        ' --- Document Type ---

        ' Remember the previous selected value
        Dim strDocumentTypeSelectedValue As String = udcDocumentTypeRadioButtonGroup.SelectedValue

        If strDocumentTypeSelectedValue = String.Empty Then
            strDocumentTypeSelectedValue = (New DocTypeBLL).getAllDocType(0).DocCode
        End If

        udcDocumentTypeRadioButtonGroup.SelectedValue = strDocumentTypeSelectedValue
        udcDocumentTypeRadioButtonGroup.ShowAllSelection = True
        udcDocumentTypeRadioButtonGroup.ShowLegend = True
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VaccinationRecordEnquriySearch)
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        ' INT12-011 Vaccination record enquiry viewstate fix [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Me.udcVaccinationRecord.Clear()
        ' INT12-011 Vaccination record enquiry viewstate fix [End][Koala]

        ' --- Search ---
        BuildClaimSearch()

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Dim blnCMSSuspend As Boolean = False
        Dim blnCIMSSuspend As Boolean = False
        Dim strMsgCode As String = String.Empty

        ' --- Check if the HA CMS Vaccination Record is turned on ---
        Select Case VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS)
            Case VaccinationBLL.EnumTurnOnVaccinationRecord.Y, VaccinationBLL.EnumTurnOnVaccinationRecord.N
                ' Nothing here

            Case VaccinationBLL.EnumTurnOnVaccinationRecord.S
                blnCMSSuspend = True
                strMsgCode = MsgCode.MSG00005

            Case Else
                Throw New Exception(String.Format("Check SystemParameter: Invalid TurnOnVaccinationRecord_{0}({1})", VaccinationBLL.VaccineRecordSystem.CMS.ToString, VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS)))

        End Select

        ' --- Check if the DH CIMS Vaccination Record is turned on ---
        Select Case VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS)
            Case VaccinationBLL.EnumTurnOnVaccinationRecord.Y, VaccinationBLL.EnumTurnOnVaccinationRecord.N
                ' Nothing here

            Case VaccinationBLL.EnumTurnOnVaccinationRecord.S
                blnCIMSSuspend = True
                strMsgCode = MsgCode.MSG00009

            Case Else
                Throw New Exception(String.Format("Check SystemParameter: Invalid TurnOnVaccinationRecord_{0}({1})", VaccinationBLL.VaccineRecordSystem.CIMS.ToString, VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS)))

        End Select

        If blnCMSSuspend And blnCIMSSuspend Then
            strMsgCode = MsgCode.MSG00010
        End If

        If blnCMSSuspend Or blnCIMSSuspend Then
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, strMsgCode)
            udcInfoMessageBox.BuildMessageBox()
        End If

        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]


        ' --- Clear session ---
        If blnReset Then
            Dim udtSessionHandler As New SessionHandler
            udtSessionHandler.ClearVREClaim()
        End If

    End Sub

    Private Sub BuildClaimSearch()
        ' Message

        Dim strDocCode As String = udcDocumentTypeRadioButtonGroup.SelectedValue
        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim strIDEASComboClientInstalled As String = IIf(udtSessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, udtSessionHandler.IDEASComboClientGetFormSession())
        Dim blnIDEASComboClientInstalled As Boolean = IIf(strIDEASComboClientInstalled = YesNo.Yes, True, False)
        Dim blnIDEASComboClientForceToUse As Boolean = IIf((New GeneralFunction).getSystemParameter("SmartID_IDEAS_Combo_Force_To_Use") = YesNo.Yes, True, False)

        ' SmartID Tips button
        If SmartIDHandler.EnableSmartID AndAlso strDocCode = DocTypeModel.DocTypeCode.HKIC Then
            If blnIDEASComboClientInstalled Or blnIDEASComboClientForceToUse Then
                btnReadSmartIDTips.Visible = False
            Else
                btnReadSmartIDTips.Visible = True
            End If

            lblSearchECAcctInputSearch.Text = Me.GetGlobalResourceObject("Text", "InputHKICSearchAccount")
        Else
            btnReadSmartIDTips.Visible = False
            lblSearchECAcctInputSearch.Text = Me.GetGlobalResourceObject("Text", "InputECSearchAccount")
        End If

        ' Help button
        Dim blnHelpAvailable As Boolean = (New DocTypeBLL).getAllDocType.Filter(strDocCode).HelpAvailable = "Y"
        ibtnInputTips.Visible = blnHelpAvailable

        ' Claim Search
        udcClaimSearch.ShowInputTips = Not blnHelpAvailable
        udcClaimSearch.SchemeSelected = True
        udcClaimSearch.UIEnableHKICSymbol = False
        udcClaimSearch.SchemeCode = String.Empty

        udcClaimSearch.IDEASComboClientInstalled = blnIDEASComboClientInstalled
        udcClaimSearch.IDEASComboClientForceToUse = blnIDEASComboClientForceToUse

        udcClaimSearch.Build(strDocCode)
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        ' CRP12-002 Fix HKIC Input [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
            Me.filtereditEnameSurname.ValidChars = "-' ."
            Me.filtereditEnameFirstname.ValidChars = "-' ."
        Else
            Me.filtereditEnameSurname.ValidChars = "-' "
            Me.filtereditEnameFirstname.ValidChars = "-' "
        End If

        ' CRP12-002 Fix HKIC Input [End][Koala]

    End Sub

    Private Function ReadSmartID(ByVal udtSmartIDContent As SmartIDContentModel, ByRef blnSmartIDError As Boolean) As Boolean
        If IsNothing(udtSmartIDContent) OrElse udtSmartIDContent.IsReadSmartID = False OrElse udtSmartIDContent.IsEndOfReadSmartID Then Return False

        Dim blnValid As Boolean = True

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim ideasBLL As New IdeasBLL
        Dim strIdeasVersion As String = ideasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)

        ' Write Start Audit log
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteLog(LogID.LOG00064, "Redirect from IDEAS after Token Request")

        udtSmartIDContent.IsEndOfReadSmartID = True

        Dim udtSessionHandler As New SessionHandler
        udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Smart ID From Ideas
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim ideasHelper As IHelper = HelpFactory.createHelper()

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Get CFD
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry_GetCFD As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim ideasSamlResponse As IdeasRM.IdeasResponse = Nothing
        Dim strArtifact As String = String.Empty

        If udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.Combo Or _
            udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.ComboGender Then

            strArtifact = udtSmartIDContent.Artifact
            ideasSamlResponse = udtSmartIDContent.IdeasSamlResponse
        Else
            strArtifact = ideasBLL.Artifact
            ideasSamlResponse = ideasHelper.getCardFaceData(udtSmartIDContent.TokenResponse, strArtifact, strIdeasVersion)

        End If

        EHSClaimBasePage.AuditLogGetCFD(udtAuditLogEntry_GetCFD, strArtifact)

        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel

        If strArtifact Is Nothing OrElse strArtifact = String.Empty Then
            '----------------------------- Error Handling -----------------------------------------------

            ' Error100 - 113
            If Not Request.QueryString("status") Is Nothing Then
                Dim strErrorCode As String = Request.QueryString("status").Trim()
                Dim strErrorMsg As String = IdeasRM.ErrorMessageMapper.MapMAStatus(strErrorCode)

                If Not strErrorMsg Is Nothing Then
                    udtSessionHandler.ClearVREClaim()

                    udcDocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                    MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Search

                    udcMessageBox.AddMessageDesc(FunctionCode, strErrorCode, strErrorMsg)

                    ' Write End Audit log
                    EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, strArtifact, strErrorCode, strErrorMsg, strIdeasVersion)

                    udcMessageBox.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, LogID.LOG00063, "Get CFD Fail")

                    blnValid = False

                End If
            End If
        End If

        If blnValid Then
            'Check Card Version
            Dim strNewCard As String = String.Empty
            If ideasSamlResponse.CardFaceDate IsNot Nothing Then
                If ideasSamlResponse.CardFaceDate.CardVersion = "1" Then
                    strNewCard = YesNo.No
                End If

                If ideasSamlResponse.CardFaceDate.CardVersion = "2" Then
                    strNewCard = YesNo.Yes
                End If
            End If

            'Success
            If ideasSamlResponse.StatusCode.Equals("samlp:Success") Then
                EHSClaimBasePage.AuditLogGetCFDComplete(udtAuditLogEntry_GetCFD, strArtifact)

                Dim udtEHSAccount As EHSAccountModel = Nothing
                Dim blnNotMatchAccountExist As Boolean = False
                Dim blnExceedDocTypeLimit As Boolean = False
                Dim udtEligibleResult As EligibleResult = Nothing
                Dim strError As String = String.Empty
                Dim goToCreation As Boolean = True

                Try
                    If udtSmartIDContent.IsDemonVersion Then
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        udtSmartIDContent.EHSAccount = SmartIDDummyCase.GetDummyEHSAccount(String.Empty, udtSmartIDContent.IdeasVersion)
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                        udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).CName = BLL.VoucherAccountMaintenanceBLL.GetCName(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0))

                    Else
                        Dim udtCFD As IdeasRM.CardFaceData
                        udtCFD = ideasSamlResponse.CardFaceDate()
                        If IsNothing(udtCFD) Then
                            strError = "ideasSamlResponse.CardFaceDate() is nothing"
                        End If

                        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        udtSmartIDContent.EHSAccount = ideasBLL.GetCardFaceDataEHSAccount(udtCFD, String.Empty, FunctionCode, udtSmartIDContent)
                        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
                    End If
                Catch ex As Exception
                    udtSmartIDContent.EHSAccount = Nothing
                    strError = ex.Message
                End Try

                Dim udtAuditlogEntry_Search As New AuditLogEntry(FunctionCode, Me)
                Dim strHKICNo As String = String.Empty

                If Not udtSmartIDContent.EHSAccount Is Nothing Then
                    strHKICNo = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum.Trim
                End If

                udtAuditlogEntry_Search.AddDescripton("HKIC No.", strHKICNo)
                udtAuditlogEntry_Search.AddDescripton("Error", strError)
                udtAuditlogEntry_Search.AddDescripton("IDEAS Version", strIdeasVersion)
                udtAuditlogEntry_Search.WriteStartLog(LogID.LOG00050, "Search & validate account with CFD", New AuditLogInfo(String.Empty, String.Empty, String.Empty, String.Empty, DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strHKICNo)))

                If Not udtSmartIDContent.EHSAccount Is Nothing Then

                    udtPersonalInfoSmartID = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)

                    '------------------------------------------------------------------------------------------------------
                    ' Card Face Data Validation
                    '------------------------------------------------------------------------------------------------------
                    Dim udtSystemMessage As SystemMessage = EHSClaimBasePage.SmartIDCardFaceDataValidation(udtPersonalInfoSmartID)

                    If Not udtSystemMessage Is Nothing Then
                        blnValid = False
                        If Not udtPersonalInfoSmartID.IdentityNum Is Nothing Then udtAuditlogEntry_Search.AddDescripton("HKID", udtPersonalInfoSmartID.IdentityNum)
                        If udtPersonalInfoSmartID.DateofIssue.HasValue Then udtAuditlogEntry_Search.AddDescripton("DOI", udtPersonalInfoSmartID.DateofIssue)
                        udtAuditlogEntry_Search.AddDescripton("DOB", udtPersonalInfoSmartID.DOB)

                        udcMessageBox.AddMessage(udtSystemMessage)

                    End If

                    If blnValid Then
                        'CRE16-017 (Block EHCP make voucher claim for themselves) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        udtSystemMessage = (New EHSClaimBLL).SearchEHSAccountSmartID("RVP", DocTypeModel.DocTypeCode.HKIC, udtPersonalInfoSmartID.IdentityNum, _
                                        (New Formatter).formatDOB(udtPersonalInfoSmartID.DOB, udtPersonalInfoSmartID.ExactDOB, Common.Component.CultureLanguage.English, Nothing, Nothing), _
                                        udtEHSAccount, udtSmartIDContent.EHSAccount, udtSmartIDContent.SmartIDReadStatus, udtEligibleResult, blnNotMatchAccountExist, blnExceedDocTypeLimit, _
                                        FunctionCode, False)
                        'CRE16-017 (Block EHCP make voucher claim for themselves) [End][Chris YIM]

                        '---------------------------------------------------------------------------------------------------------------
                        ' Search Account Error Issue
                        '---------------------------------------------------------------------------------------------------------------
                        If Not udtSystemMessage Is Nothing Then
                            ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [Start][Lawrence]
                            If udtSystemMessage.MessageCode = "00141" Then
                                udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)
                                udtSmartIDContent.EHSAccount.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)
                                Session(SESS.EHSAccount) = udtSmartIDContent.EHSAccount

                                SetupInputRecipientInformationSmartID(udtSmartIDContent.EHSAccount, udtSmartIDContent)
                                MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.InputRecipientInformationSmartID

                                Return True
                            End If
                            ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [End][Lawrence]

                            blnValid = False
                            If udtSystemMessage.MessageCode = "00142" OrElse udtSystemMessage.MessageCode = "00141" Then
                                'Me.udcClaimSearch.SetSearchShortIdentityNoError(True)
                                Me.udcClaimSearch.SetHKICNoError(True)
                            End If
                            udcMessageBox.AddMessage(udtSystemMessage)

                        End If
                    End If

                    If blnValid Then
                        If udtEligibleResult Is Nothing Then
                            udtSessionHandler.EligibleResultRemoveFromSession()
                        Else
                            Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()

                            udtEligibleResult.PromptConfirmed = True
                            'Key = 1_G0002 -> not need prompt confirm popup dox -> reminder in step2a
                            Dim strRuleResultKey As String = String.Format("{0}_{1}", 0, udtEligibleResult.RuleType)

                            udtRuleResults.Add(strRuleResultKey, udtEligibleResult)
                            udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
                        End If

                        udtEHSAccount.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)

                        'Only one case go to Claim directly -> Account validated && Search DocCode = PersonalInfo DocCode 
                        'udtSmartIDContent.SmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtSmartIDContent.EHSAccount, udtEHSAccountExist)
                        udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)
                        'Me._udtSessionHandler.NotMatchAccountExistSaveToSession(blnNotMatchAccountExist)
                        'Me._udtSessionHandler.ExceedDocTypeLimitSaveToSession(blnExceedDocTypeLimit)
                        Session(SESS.EHSAccount) = udtEHSAccount

                        Select Case udtSmartIDContent.SmartIDReadStatus
                            Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_SameDetail
                                goToCreation = False

                            Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB_NoCCCode
                                goToCreation = False

                                ' Update account
                                udtPersonalInfoSmartID.VoucherAccID = udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).VoucherAccID
                                udtPersonalInfoSmartID.Gender = udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender
                                udtPersonalInfoSmartID.TSMP = udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).TSMP

                                Dim udtEHSAccountBLL As New EHSAccountBLL
                                Dim udtSP As ServiceProviderModel = Nothing
                                Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

                                If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                                    udtSP = udtUserAC
                                Else
                                    Dim udtDataEntry As DataEntryUserModel = udtUserAC
                                    udtSP = udtDataEntry.ServiceProvider
                                End If

                                Try
                                    udtEHSAccountBLL.UpdateEHSAccountNameBySmartIC(udtPersonalInfoSmartID, udtSP.SPID)
                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        udtSystemMessage = New SystemMessage("990001", Common.Component.SeverityCode.SEVD, eSQL.Message)
                                        udcMessageBox.AddMessage(udtSystemMessage)
                                    Else
                                        Throw eSQL
                                    End If
                                End Try

                                If udtSystemMessage Is Nothing Then
                                    udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum, DocTypeModel.DocTypeCode.HKIC)
                                    udtEHSAccount.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)
                                    Session(SESS.EHSAccount) = udtEHSAccount
                                End If

                        End Select

                    Else
                        goToCreation = False

                    End If

                    udtAuditlogEntry_Search.AddDescripton("Smart IC Type", udtSmartIDContent.SmartIDReadStatus.ToString)
                    udtAuditlogEntry_Search.AddDescripton("IDEAS Version", strIdeasVersion)
                    udtAuditlogEntry_Search.AddDescripton("New Card", strNewCard)
                    udtAuditlogEntry_Search.AddDescripton("CFD", "->")
                    EHSAccountCreationBase.AuditLogHKIC(udtAuditlogEntry_Search, udtSmartIDContent.EHSAccount)

                    If Not udtSmartIDContent.EHSValidatedAccount Is Nothing Then
                        udtAuditlogEntry_Search.AddDescripton("Validated EHS Account", "->")
                        EHSAccountCreationBase.AuditLogHKIC(udtAuditlogEntry_Search, udtSmartIDContent.EHSValidatedAccount)
                    End If

                    udtAuditlogEntry_Search.WriteEndLog(LogID.LOG00051, "Search & validate account with CFD Complete")

                    If goToCreation Then
                        SetupInputRecipientInformationSmartID(udtEHSAccount, udtSmartIDContent)
                        MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.InputRecipientInformationSmartID

                        Return True

                    Else
                        If Not udtSystemMessage Is Nothing Then
                            '---------------------------------------------------------------------------------------------------------------
                            ' Block Case 
                            '---------------------------------------------------------------------------------------------------------------
                            udtSessionHandler.ClearVREClaim()
                            udcDocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                            MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Search
                            'udcMessageBox.AddMessage(udtSystemMessage)
                            blnValid = False

                        Else
                            ' Go to result
                            udcVaccinationRecord.Build(udtEHSAccount, New AuditLogEntry(FunctionCode, Me))
                            ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [Start][Koala]
                            ' -----------------------------------------------------------------------------------------
                            Me.GotoViewResult()
                            'MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Result
                            ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [End][Koala]

                            Return True

                        End If

                    End If

                Else
                    '---------------------------------------------------------------------------------------------------------------
                    ' udtSmartIDContent.EHSAccount is nothing, card face data may not be able to return 
                    '---------------------------------------------------------------------------------------------------------------
                    udtSessionHandler.ClearVREClaim()
                    udcDocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                    MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Search
                    udcMessageBox.AddMessage(New SystemMessage("990000", "E", "00253"))
                    blnValid = False
                End If

                udcMessageBox.BuildMessageBox("ValidationFail", udtAuditlogEntry_Search, LogID.LOG00052, "Search & validate account with CFD Fail", _
                        New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strHKICNo)))

            Else
                '---------------------------------------------------------------------------------------------------------------
                ' ideasSamlResponse.StatusCode is not "samlp:Success"
                '---------------------------------------------------------------------------------------------------------------
                udtSessionHandler.ClearVREClaim()
                udcDocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Search
                udcMessageBox.AddMessageDesc(FunctionCode, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail)

                ' Write End Audit log
                udtAuditLogEntry_GetCFD.AddDescripton("New Card", strNewCard)
                EHSClaimBasePage.AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, strArtifact, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail, strIdeasVersion)

                udcMessageBox.BuildMessageDescBox("SmartIDActionFail", New AuditLogEntry(FunctionCode, Me), Common.Component.LogID.LOG00063, "Get CFD Fail")

                blnValid = False

            End If
        End If

        blnSmartIDError = Not blnValid

        Return False

    End Function

#End Region

#Region "View 1 - Search"

    ' Event

    Protected Sub udcDocumentTypeRadioButtonGroup_CheckedChanged(ByVal sender As Object, ByVal documentTypeRadioButtonGroupArgs As CustomControls.DocumentTypeRadioButtonGroup.DocumentTypeRadioButtonGroupArgs) Handles udcDocumentTypeRadioButtonGroup.CheckedChanged
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Change to", udcDocumentTypeRadioButtonGroup.SelectedValue)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, AuditLogDescription.ChangeDocument)

        udcClaimSearch.CleanField()
        BuildClaimSearch()
    End Sub

    Protected Sub udcDocumentTypeRadioButtonGroup_LegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcDocumentTypeRadioButtonGroup.LegendClicked
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00002, AuditLogDescription.DocumentLegendClick)

        ucVaccinationRecordProvider.Build()
        ModalPopupExtenderVaccinationRecordProvider.Show()
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Protected Sub btnReadSmartIDTips_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReadSmartIDTips.Click
        Dim strReadSmartIDTipsUrl As String = Me.GetGlobalResourceObject("Url", "HCSPSmartIDCardUserGuideUrl")
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "ReadSmartIDTips", "javascript:openNewWin('" + ResolveClientUrl(strReadSmartIDTipsUrl) + "');", True)
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    Protected Sub ibtnInputTips_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "DocumentSmaple", String.Format("javascript:show{0}Help('{1}');", udcDocumentTypeRadioButtonGroup.SelectedValue.Trim.Replace("/", ""), Session("language")), True)
    End Sub

    Protected Sub udcClaimSearch_SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.SearchButtonClick
        Dim strDocCode As String = udcDocumentTypeRadioButtonGroup.SelectedValue
        udcClaimSearch.SetProperty(strDocCode)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00003, AuditLogDescription.SearchClick, New AuditLogInfo(String.Empty, String.Empty, String.Empty, String.Empty, _
                                    strDocCode, (New Formatter).formatDocumentIdentityNumber(strDocCode, udcClaimSearch.IdentityNo)))

        ResetMessageBox()

        Dim udtEHSAccount As EHSAccountModel = SearchEHSAccount()

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00007, AuditLogDescription.SearchFail, New AuditLogInfo(String.Empty, String.Empty, String.Empty, String.Empty, strDocCode, (New Formatter).formatDocumentIdentityNumber(strDocCode, udcClaimSearch.IdentityNo)))
            Return
        End If

        ' Save the EHS Account to session for future use
        Session(SESS.EHSAccount) = udtEHSAccount

        '--------------------------+------------------+------------------+------------------------------------------+
        '                          | HA available     | DH available     | Not available                            |
        '--------------------------+------------------+------------------+------------------------------------------+
        ' Validated Acccount Found | (1) Go to result | (1) Go to result | (2) Go to result                         |
        '--------------------------+------------------+------------------+------------------------------------------+
        ' Temporary Acccount Found | (3) Enter info   | (3) Enter info   | (4) Enter info                           |
        '--------------------------+------------------+------------------+------------------------------------------+
        ' No Account Found         | (5) Enter info   | (5) Enter info   | (6) No record found(Not to go to HA & DH)|
        '--------------------------+------------------+------------------+------------------------------------------+

        If udtEHSAccount.IsNew AndAlso _
                Not (New DocTypeBLL).CheckVaccinationRecordAvailable(udtEHSAccount.EHSPersonalInformationList(0).DocCode) Then
            udtAuditLogEntry.AddDescripton("Account Result", "No account")
            udtAuditLogEntry.AddDescripton("HA available", "N")
            udtAuditLogEntry.AddDescripton("Go to", "(6) No record found, directly go to result")
            udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditLogDescription.SearchComplete)

            ' (6) No record found, directly go to result
            udcVaccinationRecord.Build(udtEHSAccount, New AuditLogEntry(FunctionCode, Me))
            ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Me.GotoViewResult()
            'MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Result
            ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [End][Koala]

            Return

        End If

        If udtEHSAccount.AccountSource = SysAccountSource.ValidateAccount Then
            ' (1) and (2) Go to result

            ' Check whether the doc code is the same as input one
            If udtEHSAccount.EHSPersonalInformationList(0).DocCode = udcDocumentTypeRadioButtonGroup.SelectedValue Then
                udtAuditLogEntry.AddDescripton("Account result", "Validated account")
                udtAuditLogEntry.AddDescripton("HA available", "--")
                udtAuditLogEntry.AddDescripton("Go to", "(1) and (2) Go to result")
                udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditLogDescription.SearchComplete)

                udcVaccinationRecord.Build(udtEHSAccount, New AuditLogEntry(FunctionCode, Me))
                ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Me.GotoViewResult()
                'MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Result
                ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [End][Koala]


            Else
                udtAuditLogEntry.AddDescripton("Account result", "Validated account but different doc code")
                udtAuditLogEntry.AddDescripton("HA available", "--")
                udtAuditLogEntry.AddDescripton("Go to", "(1) and (2) Go to input")
                udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditLogDescription.SearchComplete)

                udtEHSAccount.EHSPersonalInformationList(0).DocCode = udcDocumentTypeRadioButtonGroup.SelectedValue
                Session(SESS.EHSAccount) = udtEHSAccount
                SetupInputAccountInformation(udtEHSAccount)
                MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.InputRecipientInformation

            End If

        Else
            ' (3), (4), and (5) Enter info
            If udtEHSAccount.IsNew Then
                Dim udtSearchAccountRemark As SearchAccountRemarkModel = Session(SESS.SearchAccountRemark)

                If Not IsNothing(udtSearchAccountRemark) AndAlso udtSearchAccountRemark.ValidatedAccountButDifferentDocCodeFound Then
                    udtAuditLogEntry.AddDescripton("Account result", "Validated account but different doc code")
                Else
                    udtAuditLogEntry.AddDescripton("Account result", "No account")
                End If
            Else
                udtAuditLogEntry.AddDescripton("Account result", "Temporary account")
            End If
            udtAuditLogEntry.AddDescripton("HA available", "--")
            udtAuditLogEntry.AddDescripton("Go to", "(3), (4), and (5) Enter info")
            udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditLogDescription.SearchComplete)

            SetupInputAccountInformation(udtEHSAccount)
            MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.InputRecipientInformation

        End If

    End Sub

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub udcClaimSearch_ReadSmartIDButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.ReadSmartIDButtonClick
        Me.RedirectToIdeas(IdeasBLL.EnumIdeasVersion.Two)
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub udcClaimSearch_ReadOldSmartIDButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.ReadOldSmartIDButtonClick
        Me.RedirectToIdeas(IdeasBLL.EnumIdeasVersion.One)
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub udcClaimSearch_ReadNewSmartIDComboButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimSearch.ReadNewSmartIDComboButtonClick
        Me.RedirectToIdeasCombo(IdeasBLL.EnumIdeasVersion.Combo)
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub RedirectToIdeas(ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
        Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)

        EHSClaimBasePage.AuditLogReadSamrtID(udtAuditLogEntry, Nothing, strIdeasVersion, Nothing)

        ' (1) Language
        Dim strLang As String = String.Empty

        If LCase(Session("language")) = CultureLanguage.TradChinese Then
            strLang = "zh_HK"
        Else
            strLang = "en_US"
        End If

        ' (2) Remove Card Setting
        Dim strRemoveCard As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        If strRemoveCard = String.Empty Then
            strRemoveCard = "Y"
        End If

        ' (3) IDEAS token
        Dim ideasHelper As IHelper = HelpFactory.createHelper()
        Dim ideasTokenResponse As TokenResponse

        ' Enforce HCSP accept server cert for connecting IDEAS Testing server
        ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

        ' Get Token From Ideas, input: the return URL from Ideas to eHS        
        ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

        Dim isDemoVersion As String = ConfigurationManager.AppSettings("SmartIDDemoVersion")
        If Not ideasTokenResponse.ErrorCode Is Nothing And Not isDemoVersion.Equals("Y") Then
            udcMessageBox.AddMessageDesc(FunctionCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)
            udcMessageBox.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, "00000", "00000")

        Else
            Dim udtSessionHandler As New SessionHandler
            Dim udtSmarIDContent As New SmartIDContentModel
            udtSmarIDContent.IsReadSmartID = True
            udtSmarIDContent.TokenResponse = ideasTokenResponse

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtSmarIDContent.IdeasVersion = eIdeasVersion
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            If isDemoVersion.Equals("Y") Then
                udtSmarIDContent.IsDemonVersion = True
                udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                EHSClaimBasePage.AuditLogConnectIdeasComplete(udtAuditLogEntry, Nothing, ideasTokenResponse, "Y", strIdeasVersion)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPageVRE").ToString().Replace("@", "&"))

            Else
                udtSmarIDContent.IsDemonVersion = False
                udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                EHSClaimBasePage.AuditLogConnectIdeasComplete(udtAuditLogEntry, Nothing, ideasTokenResponse, "N", strIdeasVersion)
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                ' Redirect to Ideas, no need to add page key
                Response.Redirect(ideasTokenResponse.IdeasMAURL)

            End If
        End If
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub RedirectToIdeasCombo(ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
        Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)

        EHSClaimBasePage.AuditLogReadSamrtID(udtAuditLogEntry, Nothing, strIdeasVersion, Nothing)

        ' (1) Language
        Dim strLang As String = String.Empty

        If LCase(Session("language")) = CultureLanguage.TradChinese Then
            strLang = "zh_HK"
        Else
            strLang = "en_US"
        End If

        ' (2) Remove Card Setting
        Dim strRemoveCard As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        If strRemoveCard = String.Empty Then
            strRemoveCard = "Y"
        End If

        ' (3) IDEAS token
        Dim ideasHelper As IHelper = HelpFactory.createHelper()
        Dim ideasTokenResponse As TokenResponse = Nothing

        ' Enforce HCSP accept server cert for connecting IDEAS Testing server
        ServicePointManager.ServerCertificateValidationCallback = New Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

        ' Get Token From Ideas, input: the return URL from Ideas to eHS
        Select Case eIdeasVersion
            Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

            Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                Dim strPageName As String = New IO.FileInfo(Me.Request.Url.LocalPath).Name
                Dim strComboReturnURL As String = Me.Page.Request.Url.GetLeftPart(UriPartial.Path)
                Dim strFolderName As String = "/VaccinationRecordEnquiry"

                strComboReturnURL = strComboReturnURL.Replace(strFolderName + "/" + strPageName, "/IDEASComboReader/IDEASComboReader.aspx")
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, strComboReturnURL, strLang, strRemoveCard)

        End Select

        Dim isDemoVersion As String = ConfigurationManager.AppSettings("SmartIDDemoVersion")

        If Not ideasTokenResponse.ErrorCode Is Nothing And Not isDemoVersion.Equals("Y") Then
            udcMessageBox.AddMessageDesc(FunctionCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)
            udcMessageBox.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, "00000", "00000")

        Else
            Dim udtSessionHandler As New SessionHandler
            Dim udtSmarIDContent As New SmartIDContentModel
            udtSmarIDContent.IsReadSmartID = True
            udtSmarIDContent.TokenResponse = ideasTokenResponse
            udtSmarIDContent.IdeasVersion = eIdeasVersion

            If isDemoVersion.Equals("Y") Then
                udtSmarIDContent.IsDemonVersion = True

                udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                EHSClaimBasePage.AuditLogConnectIdeasComboComplete(udtAuditLogEntry, Nothing, ideasTokenResponse, "Y", strIdeasVersion)

                RedirectHandler.ToURL(ConfigurationManager.AppSettings("SmartIDTestRedirectPageVRE").ToString().Replace("@", "&"))

            Else
                udtSmarIDContent.IsDemonVersion = False

                udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                EHSClaimBasePage.AuditLogConnectIdeasComboComplete(udtAuditLogEntry, Nothing, ideasTokenResponse, "N", strIdeasVersion)

                ' Prompt the popup include iframe to show IDEAS Combo UI
                ucIDEASCombo.ReadSmartIC(IdeasBLL.EnumIdeasVersion.Combo, ideasTokenResponse, FunctionCode)

            End If
        End If
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
    '

    Private Function SearchValidation(ByVal strDocCode As String, ByRef strDocNo As String, ByRef strDOB As String, ByRef strDocNoPrefix As String) As Boolean
        Dim blnValid As Boolean = True
        Dim udtFormatter As New Formatter()
        Dim udtValidator As New Validator()


        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Doc Code", strDocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udcClaimSearch.IdentityNo)
        udtAuditLogEntry.AddDescripton("Doc No. Prefix", udcClaimSearch.IdentityNoPrefix)
        udtAuditLogEntry.AddDescripton("DOB", udcClaimSearch.DOB)
        udtAuditLogEntry.WriteLog(LogID.LOG00004, AuditLogDescription.SearchStart, New AuditLogInfo(String.Empty, String.Empty, String.Empty, String.Empty, _
                                    strDocCode, (New Formatter).formatDocumentIdentityNumber(strDocCode, udcClaimSearch.IdentityNo)))

        ' Validate Document No.
        Dim udtSystemMessage As SystemMessage = Nothing

        udtSystemMessage = udtValidator.chkIdentityNumber(strDocCode, udcClaimSearch.IdentityNo.ToUpper(), udcClaimSearch.IdentityNoPrefix)

        ' CRE11-007
        ' Validate Death Record
        If udtSystemMessage Is Nothing Then
            udtSystemMessage = chkDeathRecord(strDocCode, udcClaimSearch.IdentityNo.ToUpper())
        End If

        If Not IsNothing(udtSystemMessage) Then
            blnValid = False

            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    udcClaimSearch.SetHKICNoError(True)
                Case DocTypeModel.DocTypeCode.EC
                    udcClaimSearch.SetECHKIDError(True)
                Case DocTypeModel.DocTypeCode.ADOPC
                    udcClaimSearch.SetADOPCIdentityNoError(True)
                Case Else
                    udcClaimSearch.SetSearchShortIdentityNoError(True)
            End Select

            udcMessageBox.AddMessage(udtSystemMessage)

        Else
            strDocNo = udcClaimSearch.IdentityNo
            strDocNoPrefix = udcClaimSearch.IdentityNoPrefix

        End If

        ' Validate DOB
        udtSystemMessage = udtValidator.chkDOB(strDocCode, udcClaimSearch.DOB)

        If Not IsNothing(udtSystemMessage) Then
            blnValid = False

            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    udcClaimSearch.SetHKICDOBError(True)
                Case DocTypeModel.DocTypeCode.EC
                    udcClaimSearch.SetECDOBError(True)
                Case DocTypeModel.DocTypeCode.ADOPC
                    udcClaimSearch.SetADOPCDOBError(True)
                Case Else
                    udcClaimSearch.SetSearchShortDOBError(True)
            End Select

            udcMessageBox.AddMessage(udtSystemMessage)

        Else
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'strDOB = udtFormatter.formatDate(udcClaimSearch.DOB)
            strDOB = udtFormatter.formatInputDate(udcClaimSearch.DOB)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            udcClaimSearch.SetSearchShortDOB(strDOB)

        End If

        Return blnValid

    End Function

    Private Function SearchValidationEC(ByRef strHKID As String, ByRef strDOB As String, ByRef strDOAge As String, ByRef dtmDateOfReg As DateTime) As Boolean
        Dim blnValid As Boolean = True
        Dim udtFormatter As New Formatter()
        Dim udtValidator As New Validator()
        Dim strDateOfReg As String = String.Empty

        udcClaimSearch.SetProperty(DocTypeModel.DocTypeCode.EC)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Doc Code", DocTypeModel.DocTypeCode.EC)
        udtAuditLogEntry.AddDescripton("Doc No.", udcClaimSearch.IdentityNo)
        udtAuditLogEntry.AddDescripton("Doc No. Prefix", udcClaimSearch.IdentityNoPrefix)
        udtAuditLogEntry.AddDescripton("DOB", udcClaimSearch.DOB)
        udtAuditLogEntry.WriteLog(LogID.LOG00004, AuditLogDescription.SearchStart, New AuditLogInfo(String.Empty, String.Empty, String.Empty, String.Empty, _
                                    DocTypeModel.DocTypeCode.EC, (New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.EC, udcClaimSearch.IdentityNo)))

        Dim udtSystemMessage As SystemMessage = Nothing

        ' Validate HKID
        udtSystemMessage = udtValidator.chkHKID(udcClaimSearch.IdentityNo.ToUpper())

        ' CRE11-007
        ' Validate Death Record
        If udtSystemMessage Is Nothing Then
            udtSystemMessage = chkDeathRecord(DocType.DocTypeModel.DocTypeCode.EC, udcClaimSearch.IdentityNo.ToUpper())
        End If

        If Not IsNothing(udtSystemMessage) Then
            blnValid = False
            udcClaimSearch.SetECHKIDError(True)
            udcMessageBox.AddMessage(udtSystemMessage)
        Else
            strHKID = udcClaimSearch.IdentityNo
        End If

        ' Validate DOB
        If udcClaimSearch.ECDOBSelected Then
            ' Date of Birth selected
            udtSystemMessage = udtValidator.chkDOB(DocTypeModel.DocTypeCode.EC, udcClaimSearch.DOB)
            If Not IsNothing(udtSystemMessage) Then
                blnValid = False
                udcClaimSearch.SetECDOBError(True)
                udcMessageBox.AddMessage(udtSystemMessage)
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'strDOB = udtFormatter.formatDate(udcClaimSearch.DOB)
                strDOB = udtFormatter.formatInputDate(udcClaimSearch.DOB)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

        Else
            ' "Age On" selected
            Dim strDOADay As String = udcClaimSearch.ECDOADay
            Dim strDOAMonth As String = udcClaimSearch.ECDOAMonth
            Dim strDOAYear As String = udcClaimSearch.ECDOAYear

            udtSystemMessage = udtValidator.chkECAge(udcClaimSearch.ECAge)
            If Not IsNothing(udtSystemMessage) Then
                blnValid = False
                udcClaimSearch.SetECDOAAgeError(True)
                udcMessageBox.AddMessage(udtSystemMessage)
            Else
                strDOAge = udcClaimSearch.ECAge
            End If

            ' Validate Date of Age
            udtSystemMessage = udtValidator.chkECDOAge(strDOADay, strDOAMonth, strDOAYear)
            If Not IsNothing(udtSystemMessage) Then
                blnValid = False
                udcClaimSearch.SetECDOAError(True)
                udcMessageBox.AddMessage(udtSystemMessage)
            Else
                strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDOADay), strDOAMonth, strDOAYear)

                dtmDateOfReg = CDate(udtFormatter.convertDate(strDateOfReg, Session("language")))
            End If

            ' Validate Age + Date of Age if Within Age
            If blnValid Then
                udtSystemMessage = udtValidator.chkECAgeAndDOAge(udcClaimSearch.ECAge, strDOADay, strDOAMonth, strDOAYear)

                If Not IsNothing(udtSystemMessage) Then
                    blnValid = False
                    udcClaimSearch.SetECDOAAgeError(True)
                    udcClaimSearch.SetECDOAError(True)
                    udcMessageBox.AddMessage(udtSystemMessage)
                Else
                    strDOB = (CDate(udtFormatter.convertDate(strDateOfReg, "E")).Year - Convert.ToInt32(strDOAge)).ToString()
                End If
            End If
        End If

        Return blnValid

    End Function

    Private Function chkDeathRecord(ByVal strDocCode As String, ByVal strDocNo As String) As SystemMessage
        Dim udtEHSClaimBLL As New EHSClaimBLL
        Dim udtValidator As New Common.Validation.Validator

        ' -------------------------------------
        ' CRE11-007
        ' Check DeathRecordEntry and block
        ' -------------------------------------
        If (New DocType.DocTypeBLL).getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
            If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(strDocNo).IsDead() Then

                Return udtValidator.GetMessageForIdentityNoIsNoLongerValid(strDocCode)
            End If
        End If

        Return Nothing
    End Function

    Private Function SearchEHSAccount() As EHSAccountModel
        Dim udtEHSAccount As EHSAccountModel = Nothing

        ' Initialize controls
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        udcClaimSearch.SetHKICError(False)
        udcClaimSearch.SetECError(False)
        udcClaimSearch.SetSearchShortError(False)

        Dim strDocCode As String = udcDocumentTypeRadioButtonGroup.SelectedValue
        Dim strDocNo As String = String.Empty
        Dim strDOB As String = String.Empty
        Dim strDocNoPrefix As String = String.Empty

        Dim blnValid As Boolean = True

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                ' Validation and get the property
                blnValid = SearchValidation(strDocCode, strDocNo, strDOB, strDocNoPrefix)

                If blnValid Then
                    strDocNo = strDocNo.Replace("(", String.Empty).Replace(")", String.Empty)

                    Dim udtSystemMessage As SystemMessage = GetEHSAccountModel(strDocCode, strDocNo, strDOB, Nothing, Nothing, udtEHSAccount)

                    If Not IsNothing(udtSystemMessage) Then
                        If udtSystemMessage.MessageCode = "00142" OrElse udtSystemMessage.MessageCode = "00141" Then
                            udcClaimSearch.SetHKICNoError(True)
                        ElseIf udtSystemMessage.MessageCode = "00110" Then
                            udcClaimSearch.SetHKICDOBError(True)
                        End If

                        udcMessageBox.AddMessage(udtSystemMessage)

                    End If

                End If

            Case DocTypeModel.DocTypeCode.HKBC, _
                    DocTypeModel.DocTypeCode.DI, _
                    DocTypeModel.DocTypeCode.REPMT, _
                    DocTypeModel.DocTypeCode.ID235B, _
                    DocTypeModel.DocTypeCode.VISA, _
                    DocTypeModel.DocTypeCode.ADOPC
                ' Validation and get the property
                blnValid = SearchValidation(strDocCode, strDocNo, strDOB, strDocNoPrefix)

                If blnValid Then
                    strDocNo = strDocNo.Replace("(", String.Empty).Replace(")", String.Empty)

                    Dim udtSystemMessage As SystemMessage = Nothing

                    If strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                        udtSystemMessage = GetEHSAccountModel(strDocCode, strDocNo, strDOB, Nothing, Nothing, udtEHSAccount, strDocNoPrefix)
                    Else
                        udtSystemMessage = GetEHSAccountModel(strDocCode, strDocNo, strDOB, Nothing, Nothing, udtEHSAccount)
                    End If

                    If Not IsNothing(udtSystemMessage) Then
                        If udtSystemMessage.MessageCode = "00142" OrElse udtSystemMessage.MessageCode = "00141" Then
                            udcClaimSearch.SetSearchShortIdentityNoError(True)
                        ElseIf udtSystemMessage.MessageCode = "00110" Then
                            udcClaimSearch.SetSearchShortDOBError(True)
                        End If

                        udcMessageBox.AddMessage(udtSystemMessage)

                    End If

                End If

            Case DocTypeModel.DocTypeCode.EC
                Dim strECAge As String = String.Empty
                Dim dtmDateOfReg As DateTime = Nothing

                blnValid = SearchValidationEC(strDocNo, strDOB, strECAge, dtmDateOfReg)

                If blnValid Then
                    strDocNo = strDocNo.Replace("(", String.Empty).Replace(")", String.Empty)

                    Dim udtSystemMessage As SystemMessage = Nothing

                    If Not IsNothing(strECAge) AndAlso strECAge.Trim <> String.Empty Then
                        udtSystemMessage = GetEHSAccountModel(strDocCode, strDocNo, Nothing, strECAge, dtmDateOfReg, udtEHSAccount)
                    Else
                        udtSystemMessage = GetEHSAccountModel(strDocCode, strDocNo, strDOB, Nothing, Nothing, udtEHSAccount)
                    End If

                    If Not IsNothing(udtSystemMessage) Then
                        blnValid = False

                        If udtSystemMessage.MessageCode = "00142" OrElse udtSystemMessage.MessageCode = "00141" Then
                            udcClaimSearch.SetECHKIDError(True)
                        ElseIf udtSystemMessage.MessageCode = "00110" Then
                            If udcClaimSearch.ECDOBSelected Then
                                udcClaimSearch.SetECDOBError(True)
                            Else
                                udcClaimSearch.SetECDOAAgeError(True)
                                udcClaimSearch.SetECDOAError(True)
                            End If
                        End If

                        udcMessageBox.AddMessage(udtSystemMessage)
                    End If
                End If

        End Select

        If Not IsNothing(udtEHSAccount) Then udtEHSAccount.SetSearchDocCode(strDocCode)
        Return udtEHSAccount

    End Function

    Private Function GetEHSAccountModel(ByVal strDocCode As String, ByVal strDocNo As String, ByVal strDOB As String, ByVal intAge As Nullable(Of Integer), ByVal dtmDOR As Nullable(Of Date), ByRef udtEHSAccountR As EHSAccountModel, Optional ByVal strDocNoPrefix As String = "") As SystemMessage
        Dim udtEHSAccountBLL As New EHSAccountBLL()
        Dim udtEHSClaimBLL As New HCSP.BLL.EHSClaimBLL()
        Dim udtValidator As New Common.Validation.Validator ' CRE11-007

        Dim udtEHSAccount As EHSAccountModel
        Dim strMsgCode As String = Nothing

        Dim strInputDocCode As String = strDocCode

        ' Get the Exact DOB from DOB
        Dim udtGeneralFunction As New GeneralFunction()
        Dim dtmDOB As Date = Nothing
        Dim strExactDOB As String = Nothing

        If intAge.HasValue Then
            strExactDOB = "A"
        Else
            udtGeneralFunction.chkDOBtype(strDOB, dtmDOB, strExactDOB)
        End If

        Dim udtSearchAccountRemark As New SearchAccountRemarkModel

        ' -------------------------------------
        ' Search validated account
        ' -------------------------------------
        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strDocNo, strInputDocCode)

        If IsNothing(udtEHSAccount) Then
            ' Check equivent Document

            ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [Start][Lawrence]
            Select Case strInputDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    ' Cross-search HKBC
                    strDocCode = DocTypeModel.DocTypeCode.HKBC
                    udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strDocNo, strDocCode)

                    If IsNothing(udtEHSAccount) Then
                        ' If HKBC is not found, continue to cross-search EC
                        strDocCode = DocTypeModel.DocTypeCode.EC
                        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strDocNo, strDocCode)
                    End If

                Case DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.EC
                    ' Cross-search HKIC
                    strDocCode = DocTypeModel.DocTypeCode.HKIC
                    udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strDocNo, strDocCode)

            End Select

            If Not IsNothing(udtEHSAccount) Then
                ' Block the search if the cross-search account is suspended/terminated
                If udtEHSAccount.RecordStatus = EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended Then
                    Return New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, "00108")
                End If

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                If udtEHSAccount.RecordStatus = EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated Then
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    Return New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, "00109")
                End If

                udtSearchAccountRemark.ValidatedAccountButDifferentDocCodeFound = True
                udtEHSAccount = Nothing

            End If

            ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [End][Lawrence]

        End If

        If Not IsNothing(udtEHSAccount) Then
            ' Block the search if the account is suspended/terminated
            If udtEHSAccount.RecordStatus = EHSAccountModel.ValidatedAccountRecordStatusClass.Suspended Then
                Return New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, "00108")
            End If

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            If udtEHSAccount.RecordStatus = EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                Return New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, "00109")
            End If

            If strMsgCode = String.Empty Then
                ' Check DOB
                Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)

                Dim blnDOBMatch As Boolean = False

                If intAge.HasValue Then
                    blnDOBMatch = udtEHSClaimBLL.chkEHSAccountInputDOBMatch(udtPersonalInfo, intAge.Value, dtmDOR.Value)
                Else
                    blnDOBMatch = udtEHSClaimBLL.chkEHSAccountInputDOBMatch(udtPersonalInfo, dtmDOB, strExactDOB)
                End If

                If Not blnDOBMatch Then
                    ' DOB Not Match
                    Select Case strInputDocCode
                        Case DocTypeModel.DocTypeCode.ADOPC
                            strMsgCode = "00222"
                        Case DocTypeModel.DocTypeCode.DI
                            strMsgCode = "00223"
                        Case DocTypeModel.DocTypeCode.EC
                            strMsgCode = "00110"
                        Case DocTypeModel.DocTypeCode.HKBC
                            strMsgCode = "00224"
                        Case DocTypeModel.DocTypeCode.HKIC
                            strMsgCode = "00110"
                        Case DocTypeModel.DocTypeCode.ID235B
                            strMsgCode = "00225"
                        Case DocTypeModel.DocTypeCode.REPMT
                            strMsgCode = "00226"
                        Case DocTypeModel.DocTypeCode.VISA
                            strMsgCode = "00227"
                        Case Else
                            strMsgCode = "00110"
                    End Select
                End If
            End If

            ' Return result
            If strMsgCode = String.Empty Then
                udtEHSAccountR = udtEHSAccount
                Return Nothing

            Else
                udtEHSAccountR = Nothing
                Return New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, strMsgCode)

            End If

        End If

        ' -------------------------------------
        ' Search temporary/special account
        ' -------------------------------------

        ' Temporary account
        Dim udtEHSAccountList As EHSAccountModelCollection = udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strDocNo, strInputDocCode)

        For Each udtEHSAccountNode As EHSAccountModel In udtEHSAccountList
            ' Only for claim and for validate
            If Not (udtEHSAccountNode.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim OrElse _
                    udtEHSAccountNode.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForValidate) Then Continue For

            ' Pending Confirmation, Pending Verify, Invalid only
            If Not (udtEHSAccountNode.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation OrElse _
                    udtEHSAccountNode.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify OrElse _
                    udtEHSAccountNode.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid) Then Continue For

            ' Not X account
            If Not (udtEHSAccountNode.OriginalAccID Is Nothing OrElse udtEHSAccountNode.OriginalAccID.Trim = String.Empty) Then Continue For

            ' Match DOB
            If intAge.HasValue AndAlso dtmDOR.HasValue Then
                ' EC with Age
                If udtEHSClaimBLL.chkEHSAccountInputDOBMatch(udtEHSAccountNode.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then
                    If udtEHSAccountR Is Nothing Then
                        udtEHSAccountR = udtEHSAccountNode
                    Else
                        If udtEHSAccountNode.CreateDtm > udtEHSAccountR.CreateDtm Then
                            udtEHSAccountR = udtEHSAccountNode
                        End If
                    End If

                End If

            Else
                If udtEHSClaimBLL.chkEHSAccountInputDOBMatch(udtEHSAccountNode.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                    If udtEHSAccountR Is Nothing Then
                        udtEHSAccountR = udtEHSAccountNode
                    Else
                        '==================================================================== Code for SmartID ============================================================================
                        If udtEHSAccountR.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) AndAlso _
                           udtEHSAccountNode.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

                            If udtEHSAccountR.EHSPersonalInformationList(0).CreateBySmartID Then
                                If udtEHSAccountNode.EHSPersonalInformationList(0).CreateBySmartID Then

                                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                    ' ---------------------------------------------------------------------------------------------------------
                                    ' Get latest version
                                    Dim strExistSmartIDVer As String = Replace(udtEHSAccountR.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    Dim strCurrentSmartIDVer As String = Replace(udtEHSAccountNode.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                                    If SmartIDHandler.IsExistingOldSmartIDVersion(strExistSmartIDVer, strCurrentSmartIDVer) Then
                                        udtEHSAccountR = udtEHSAccountNode

                                    ElseIf SmartIDHandler.CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "=") Then
                                        ' Get latest create date when same version
                                        If udtEHSAccountR.CreateDtm < udtEHSAccountNode.CreateDtm Then
                                            udtEHSAccountR = udtEHSAccountNode
                                        End If
                                    End If

                                End If
                            Else
                                If udtEHSAccountNode.EHSPersonalInformationList(0).CreateBySmartID Then
                                    udtEHSAccountR = udtEHSAccountNode
                                End If
                            End If
                        Else
                            If udtEHSAccountR.CreateDtm < udtEHSAccountNode.CreateDtm Then
                                udtEHSAccountR = udtEHSAccountNode
                            End If
                        End If
                        '==================================================================================================================================================================

                    End If
                End If
            End If
        Next

        ' Special account
        udtEHSAccountList = udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strDocNo, strInputDocCode)

        For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountList
            ' Only for claim and for validate
            If Not (udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim OrElse _
                    udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForValidate) Then Continue For

            ' Pending Verify only
            If Not (udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify) Then Continue For

            ' Not X Account
            If Not (udtCurEHSAccount.TempVouhcerAccID Is Nothing OrElse udtCurEHSAccount.TempVouhcerAccID.Trim() = String.Empty) Then Continue For

            ' Match DOB
            If intAge.HasValue AndAlso dtmDOR.HasValue Then
                'EC Case Report Age on Date of Registration
                If udtEHSClaimBLL.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then

                    If udtEHSAccountR Is Nothing Then
                        udtEHSAccountR = udtCurEHSAccount
                    Else
                        If udtEHSAccountR.CreateDtm < udtCurEHSAccount.CreateDtm Then
                            udtEHSAccountR = udtCurEHSAccount
                        End If
                    End If

                End If
            Else
                If udtEHSClaimBLL.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then

                    If udtEHSAccountR Is Nothing Then
                        udtEHSAccountR = udtCurEHSAccount
                    Else
                        '==================================================================== Code for SmartID ============================================================================
                        If udtEHSAccountR.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) AndAlso _
                           udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then

                            If udtEHSAccountR.EHSPersonalInformationList(0).CreateBySmartID Then
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                    ' ---------------------------------------------------------------------------------------------------------
                                    ' Get latest version
                                    Dim strExistSmartIDVer As String = Replace(udtEHSAccountR.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    Dim strCurrentSmartIDVer As String = Replace(udtCurEHSAccount.EHSPersonalInformationList(0).SmartIDVer, "C", "")
                                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                                    If SmartIDHandler.IsExistingOldSmartIDVersion(strExistSmartIDVer, strCurrentSmartIDVer) Then
                                        udtEHSAccountR = udtCurEHSAccount

                                    ElseIf SmartIDHandler.CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "=") Then
                                        ' Get latest create date when same version
                                        If udtEHSAccountR.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                            udtEHSAccountR = udtCurEHSAccount
                                        End If
                                    End If

                                End If
                            Else
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    udtEHSAccountR = udtCurEHSAccount
                                End If
                            End If
                        Else
                            If udtEHSAccountR.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                udtEHSAccountR = udtCurEHSAccount
                            End If
                        End If
                        '==================================================================================================================================================================

                    End If
                End If
            End If
        Next

        ' Save to session
        Session(SESS.SearchAccountRemark) = udtSearchAccountRemark

        ' -------------------------------------
        ' If account found , return
        ' -------------------------------------
        If Not IsNothing(udtEHSAccountR) Then Return Nothing

        ' -------------------------------------
        ' If no validated/temporary/special accounts found, construct new
        ' -------------------------------------
        udtEHSAccountR = New EHSAccountModel()
        udtEHSAccountR.VoucherAccID = String.Empty

        With udtEHSAccountR.EHSPersonalInformationList(0)
            .IdentityNum = (New Formatter).formatDocumentIdentityNumber(strInputDocCode, strDocNo)
            .ExactDOB = strExactDOB

            If Not intAge.HasValue Then
                ' Not EC Age on registration
                .DOB = dtmDOB
                .ECAge = Nothing
                .ECDateOfRegistration = Nothing
            Else
                ' EC Age on registration
                .DOB = dtmDOR.Value.AddYears(-intAge.Value)
                .ECAge = intAge.Value
                .ECDateOfRegistration = dtmDOR.Value
            End If

            .DocCode = strInputDocCode
            .AdoptionPrefixNum = strDocNoPrefix
            .CName = String.Empty
            .CCCode1 = String.Empty
            .CCCode2 = String.Empty
            .CCCode3 = String.Empty
            .CCCode4 = String.Empty
            .CCCode5 = String.Empty
            .CCCode6 = String.Empty
            .SetDOBTypeSelected(False)

        End With

        Return Nothing

    End Function

#End Region

#Region "View 2 - InputRecipientInformation"

    ' Event

    Protected Sub ibtnInputRecipientInformationCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        udtAuditLogEntry.AddDescripton("Doc Code", hfDocCode.Value)
        udtAuditLogEntry.AddDescripton("Doc No.", lblIADocumentNo.Text)
        udtAuditLogEntry.AddDescripton("Surname", txtIANameSurname.Text)
        udtAuditLogEntry.AddDescripton("Firstname", txtIANameFirstname.Text)
        udtAuditLogEntry.AddDescripton("Gender", rblIAGender.SelectedValue)

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        'If hfDocCode.Value = DocTypeModel.DocTypeCode.HKBC OrElse hfDocCode.Value = DocTypeModel.DocTypeCode.ADOPC Then
        '    Dim strHKBCDOBType As String = String.Empty
        '    If rbHKBCDOB.Checked Then strHKBCDOBType = "DOB"
        '    If rbHKBCDOBInWord.Checked Then strHKBCDOBType = "DOB in word"

        '    udtAuditLogEntry.AddDescripton("HKBC DOB Type", strHKBCDOBType)
        '    udtAuditLogEntry.AddDescripton("HKBC DOB in word", ddlHKBCDOBInWord.SelectedValue)
        'End If
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        If hfDocCode.Value = DocTypeModel.DocTypeCode.EC Then
            udtAuditLogEntry.AddDescripton("EC DOB type", rblECDOBType.SelectedValue)
        End If

        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditLogDescription.InputRecipientInformationCancel)

        Reset()
    End Sub

    Protected Sub ibtnInputRecipientInformationNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ResetMessageBox()
        ResetErrorImage()

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Doc Code", hfDocCode.Value)
        udtAuditLogEntry.AddDescripton("Doc No.", lblIADocumentNo.Text)
        udtAuditLogEntry.AddDescripton("Surname", txtIANameSurname.Text)
        udtAuditLogEntry.AddDescripton("Firstname", txtIANameFirstname.Text)
        udtAuditLogEntry.AddDescripton("Gender", rblIAGender.SelectedValue)

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        'If hfDocCode.Value = DocTypeModel.DocTypeCode.HKBC OrElse hfDocCode.Value = DocTypeModel.DocTypeCode.ADOPC Then
        '    Dim strHKBCDOBType As String = String.Empty
        '    If rbHKBCDOB.Checked Then strHKBCDOBType = "DOB"
        '    If rbHKBCDOBInWord.Checked Then strHKBCDOBType = "DOB in word"

        '    udtAuditLogEntry.AddDescripton("HKBC DOB Type", strHKBCDOBType)
        '    udtAuditLogEntry.AddDescripton("HKBC DOB in word", ddlHKBCDOBInWord.SelectedValue)
        'End If
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        If hfDocCode.Value = DocTypeModel.DocTypeCode.EC Then
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            udtAuditLogEntry.AddDescripton("EC Serial No.", txtECSerialNo.Text)
            ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
            udtAuditLogEntry.AddDescripton("EC DOB type", rblECDOBType.SelectedValue)
        End If

        udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditLogDescription.InputRecipientInformationNext)

        ' ----- Validation ------
        Dim udtValidator As New Validator
        Dim udtSystemMessage As SystemMessage = Nothing

        Dim udtEHSAccount As EHSAccountModel = Session(SESS.EHSAccount)

        ' Date of Birth
        Select Case hfDocCode.Value
            ' CRE16-012 Removal of DOB InWord [Start][Winnie]
            'Case DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.ADOPC
            '    If Not rbHKBCDOB.Checked AndAlso Not rbHKBCDOBInWord.Checked Then
            '        udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00085)
            '        imgHKBCDOBError.Visible = True
            '        imgHKBCDOBInWordError.Visible = True
            '    End If

            '    If rbHKBCDOBInWord.Checked AndAlso ddlHKBCDOBInWord.SelectedValue = String.Empty Then
            '        udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00160)
            '        imgHKBCDOBInWordError.Visible = True
            '    End If
            ' CRE16-012 Removal of DOB InWord [End][Winnie]

            Case DocTypeModel.DocTypeCode.EC
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                udtSystemMessage = udtValidator.chkSerialNo(txtECSerialNo.Text.Trim, cboECSerialNoNotProvided.Checked)
                If Not IsNothing(udtSystemMessage) Then
                    udcMessageBox.AddMessage(udtSystemMessage)
                    imgECSerialNo.Visible = True
                End If
                ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

                If udtEHSAccount.EHSPersonalInformationList(0).ExactDOB <> "A" AndAlso rblECDOBType.SelectedValue = String.Empty Then
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00085)
                    imgECDOBTypeError.Visible = True

                End If

        End Select

        ' Name in English
        udtSystemMessage = udtValidator.chkEngName(txtIANameSurname.Text.Trim, txtIANameFirstname.Text.Trim)
        If Not IsNothing(udtSystemMessage) Then
            udcMessageBox.AddMessage(udtSystemMessage)
            imgIANameError.Visible = True
        End If

        ' Gender
        udtSystemMessage = udtValidator.chkGender(rblIAGender.SelectedValue)
        If Not IsNothing(udtSystemMessage) Then
            udcMessageBox.AddMessage(udtSystemMessage)
            imgIAGenderError.Visible = True
        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDescription.InputRecipientInformationNextFail)
            Return
        End If

        ' ----- End of Validation ------

        ' Save the input information to EHSAccount

        ' Empty the Account ID to indicate it is not an account, but a recipient only
        If Not udtEHSAccount.IsNew Then udtEHSAccount.VoucherAccID = String.Empty

        With udtEHSAccount.EHSPersonalInformationList(0)
            .ENameSurName = txtIANameSurname.Text.Trim
            .ENameFirstName = txtIANameFirstname.Text.Trim
            .Gender = rblIAGender.SelectedValue

            ' Convert Exact DOB
            Select Case .DocCode
                ' CRE16-012 Removal of DOB InWord [Start][Winnie]
                Case DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.ADOPC
                    '    If rbHKBCDOBInWord.Checked Then
                    '        ' DOB in word
                    '        Select Case .ExactDOB
                    '            Case "D"
                    '                .ExactDOB = "T"
                    '            Case "M"
                    '                .ExactDOB = "U"
                    '            Case "Y"
                    '                .ExactDOB = "V"
                    '        End Select

                    '        .OtherInfo = ddlHKBCDOBInWord.SelectedValue

                    '    Else
                    ' DOB only
                    Select Case .ExactDOB
                        Case "T"
                            .ExactDOB = "D"
                        Case "U"
                            .ExactDOB = "M"
                        Case "V"
                            .ExactDOB = "Y"
                    End Select

                    .OtherInfo = String.Empty
                    '    End If
                    ' CRE16-012 Removal of DOB InWord [End][Winnie]

                Case DocTypeModel.DocTypeCode.EC
                    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                    ' ----------------------------------------------------------
                    .ECSerialNo = txtECSerialNo.Text.Trim
                    If txtECSerialNo.Text.Trim = String.Empty Then
                        .ECSerialNoNotProvided = True
                    Else
                        .ECSerialNoNotProvided = False
                    End If
                    ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

                    If .ExactDOB <> "A" Then
                        Select Case rblECDOBType.SelectedValue
                            Case "D"
                                Select Case .ExactDOB
                                    Case "T"
                                        .ExactDOB = "D"
                                    Case "U"
                                        .ExactDOB = "M"
                                    Case "V", "R"
                                        .ExactDOB = "Y"
                                End Select

                            Case "R"
                                .ExactDOB = "R"

                            Case "T"
                                Select Case .ExactDOB
                                    Case "D"
                                        .ExactDOB = "T"
                                    Case "M"
                                        .ExactDOB = "U"
                                    Case "Y", "R"
                                        .ExactDOB = "V"
                                End Select
                        End Select

                    End If

            End Select

            ' Clear other fields
            .DateofIssue = Nothing
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            '.ECSerialNo = String.Empty
            '.ECSerialNoNotProvided = False
            ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
            .ECReferenceNo = String.Empty
            .ECReferenceNoOtherFormat = False
            .Foreign_Passport_No = String.Empty
            .PermitToRemainUntil = Nothing
            .CCCode1 = String.Empty
            .CCCode2 = String.Empty
            .CCCode3 = String.Empty
            .CCCode4 = String.Empty
            .CCCode5 = String.Empty
            .CCCode6 = String.Empty
            .CName = String.Empty
            .CheckDtm = Nothing
        End With

        Session(SESS.EHSAccount) = udtEHSAccount

        SetupConfirmRecipientInformation(udtEHSAccount)

        udtAuditLogEntry.AddDescripton("Doc Code", hfDocCode.Value)
        udtAuditLogEntry.AddDescripton("Doc No.", lblIADocumentNo.Text)
        udtAuditLogEntry.AddDescripton("Surname", txtIANameSurname.Text)
        udtAuditLogEntry.AddDescripton("Firstname", txtIANameFirstname.Text)
        udtAuditLogEntry.AddDescripton("Gender", rblIAGender.SelectedValue)

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        'If hfDocCode.Value = DocTypeModel.DocTypeCode.HKBC OrElse hfDocCode.Value = DocTypeModel.DocTypeCode.ADOPC Then
        '    Dim strHKBCDOBType As String = String.Empty
        '    If rbHKBCDOB.Checked Then strHKBCDOBType = "DOB"
        '    If rbHKBCDOBInWord.Checked Then strHKBCDOBType = "DOB in word"

        '    udtAuditLogEntry.AddDescripton("HKBC DOB Type", strHKBCDOBType)
        '    udtAuditLogEntry.AddDescripton("HKBC DOB in word", ddlHKBCDOBInWord.SelectedValue)
        'End If
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        If hfDocCode.Value = DocTypeModel.DocTypeCode.EC Then
            udtAuditLogEntry.AddDescripton("EC DOB type", rblECDOBType.SelectedValue)
        End If

        udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditLogDescription.InputRecipientInformationNextSuccessful)

        MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.ConfirmRecipientInformation

    End Sub

    '

    Private Sub SetupInputAccountInformation(ByVal udtEHSAccount As EHSAccountModel)
        BindInputRecipientInformation(udtEHSAccount)

        ResetErrorImage()

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        ' Prevent default focus issue (Textbox input cursor will be shifted out of textbox)
        Me.txtIANameFirstname.Focus()
        ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

        Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        ' Save Doc Code to hidden field
        hfDocCode.Value = udtEHSAccount.SearchDocCode

        ' Document No.
        Dim udtFormatter As New Formatter
        lblIADocumentNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtPersonalInfo.DocCode, udtPersonalInfo.IdentityNum, False, udtPersonalInfo.AdoptionPrefixNum)

        ' Date of Birth
        Dim strDOBText As String = String.Empty

        If udtPersonalInfo.ExactDOB <> "A" Then
            strDOBText = udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)
        End If

        Select Case udtEHSAccount.SearchDocCode
            Case DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.ADOPC
                mvDOB.ActiveViewIndex = ViewIndexDOB.HKBC

                txtHKBCDOB.Text = strDOBText

                ' CRE16-012 Removal of DOB InWord [Start][Winnie]
                'ddlHKBCDOBInWord.SelectedIndex = 0
                'ddlHKBCDOBInWord.Enabled = False

                'txtHKBCDOBInWord.Text = strDOBText
                'rbHKBCDOB.Checked = False
                'rbHKBCDOBInWord.Checked = False
                ' CRE16-012 Removal of DOB InWord [End][Winnie]

                Dim udtSearchAccountRemark As SearchAccountRemarkModel = Session(SESS.SearchAccountRemark)

                If Not IsNothing(udtSearchAccountRemark) AndAlso udtSearchAccountRemark.ValidatedAccountButDifferentDocCodeFound Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                Else
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008)
                End If

            Case DocTypeModel.DocTypeCode.EC
                mvDOB.ActiveViewIndex = ViewIndexDOB.EC

                txtECDOB.Text = strDOBText
                rblECDOBType.ClearSelection()

                ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [Start][Lawrence]
                Dim udtSearchAccountRemark As SearchAccountRemarkModel = Session(SESS.SearchAccountRemark)

                If Not IsNothing(udtSearchAccountRemark) AndAlso udtSearchAccountRemark.ValidatedAccountButDifferentDocCodeFound Then
                    ' Show general message regardless the ExactDOB = "A"
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)

                Else
                    If udtPersonalInfo.ExactDOB = "A" Then
                        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                    Else
                        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008)
                    End If

                End If
                ' CRE14-007 - Fix VRE display for same doc no. in HKIC and EC [End][Lawrence]

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                ' Prevent default focus issue (Textbox input cursor will be shifted out of textbox)
                Me.txtECSerialNo.Focus()
                ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
            Case Else
                mvDOB.ActiveViewIndex = ViewIndexDOB.Normal

                lblIADOB.Text = strDOBText

                Dim udtSearchAccountRemark As SearchAccountRemarkModel = Session(SESS.SearchAccountRemark)

                If Not IsNothing(udtSearchAccountRemark) AndAlso udtSearchAccountRemark.ValidatedAccountButDifferentDocCodeFound Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                Else
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                End If

        End Select
        'EC Serial No.
        txtECSerialNo.Text = String.Empty
        txtECSerialNo.Enabled = True
        txtECSerialNo.BackColor = Nothing
        cboECSerialNoNotProvided.Checked = False

        ' Name in English
        txtIANameSurname.Text = String.Empty
        txtIANameFirstname.Text = String.Empty
        imgIANameError.Visible = False

        ' Gender
        rblIAGender.ClearSelection()
        imgIAGenderError.Visible = False

        ' Message Box
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub BindInputRecipientInformation(ByVal udtEHSAccount As EHSAccountModel)
        Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        Dim udtDocTypeBLL As New DocTypeBLL
        Dim udtDocType As DocTypeModel = udtDocTypeBLL.getAllDocType().Filter(udtPersonalInfo.DocCode)

        Dim strLanguage As String = Session("language")

        ' Document Type
        lblIADocumentType.Text = udtDocType.DocName(strLanguage)

        ' Document No.
        Dim udtFormatter As New Formatter

        lblIADocumentNoText.Text = udtDocType.DocIdentityDesc(strLanguage)

        ' Date of Birth
        Select Case udtPersonalInfo.DocCode
            ' CRE16-012 Removal of DOB InWord [Start][Winnie]
            'Case DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.ADOPC
            '    rbHKBCDOBInWord.Text = Me.GetGlobalResourceObject("Text", "DOBInWordShort")
            '    BuildHKBCDOBInWord(rbHKBCDOBInWord.Checked)
            ' CRE16-012 Removal of DOB InWord [End][Winnie]
            Case DocTypeModel.DocTypeCode.EC
                mvDOB.ActiveViewIndex = ViewIndexDOB.EC
                BuildECDOBType(udtPersonalInfo.ExactDOB)

                If udtPersonalInfo.ExactDOB = "A" Then
                    lblECDOB.Text = udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Session("language"), udtPersonalInfo.ECAge, udtPersonalInfo.ECDateOfRegistration)
                    lblECDOB.Visible = True
                    txtECDOB.Visible = False
                    lblIAECDOBTypeText.Visible = False
                    rblECDOBType.Visible = False
                Else
                    lblECDOB.Visible = False
                    txtECDOB.Visible = True
                    lblIAECDOBTypeText.Visible = True
                    rblECDOBType.Visible = True
                End If

        End Select

        ' Name in English
        lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

        ' Gender
        rblIAGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        rblIAGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

    End Sub

    Private Sub BuildECDOBType(ByVal strExactDOB As String)
        Dim strPreviousDOBType As String = rblECDOBType.SelectedValue

        rblECDOBType.Items.Clear()

        rblECDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "DOBReport"), "D"))

        If strExactDOB = "Y" OrElse strExactDOB = "R" OrElse strExactDOB = "V" Then
            rblECDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "YOB"), "R"))
        End If

        rblECDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "DOBTravel"), "T"))

        If strPreviousDOBType <> String.Empty Then rblECDOBType.SelectedValue = strPreviousDOBType

    End Sub

#End Region

#Region "View 3 - InputRecipientInformationSmartID"

    ' Event

    Private Sub udcStep1b3InputDocumentType_SelectGender(ByVal udcInputHKID As ucInputDocTypeBase, ByVal sender As Object, ByVal e As System.EventArgs) Handles ucInputDocumentType.SelectGender
        Dim udtSessionHandler As New SessionHandler
        Dim udtSmartIDContent As SmartIDContentModel = udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)
        Dim strGender As String = String.Empty
        Dim isShowSmartIDDiff As Boolean = False

        Select Case udtSmartIDContent.SmartIDReadStatus
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Case SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                    SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                Dim udcInputHKIDSmartID As ucInputHKIDSmartID = udcInputHKID
                udcInputHKIDSmartID.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                strGender = udcInputHKIDSmartID.Gender

            Case Else
                Dim udcInputHKIDSmartIDSignal As ucInputHKIDSmartIDSignal = udcInputHKID
                udcInputHKIDSmartIDSignal.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
                strGender = udcInputHKIDSmartIDSignal.Gender

        End Select

        If Not String.IsNullOrEmpty(strGender) Then
            udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender = strGender
            udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ibtnInputRecipientInformationSmartIDProceed.Enabled = True
            ibtnInputRecipientInformationSmartIDProceed.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToEnquiryBtn")
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        Else
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ibtnInputRecipientInformationSmartIDProceed.Enabled = False
            ibtnInputRecipientInformationSmartIDProceed.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToEnquiryDisableBtn")
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        End If

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("Gender", strGender)
        udtAuditLogEntry.WriteLog(LogID.LOG00017, AuditLogDescription.InputRecipientInformationSmartICChangeGender)

    End Sub

    Protected Sub ibtnInputRecipientInformationSmartIDCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = (New SessionHandler).SmartIDContentGetFormSession(FunctionCode).EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, AuditLogDescription.InputRecipientInformationSmartICCancel)

        Reset()
    End Sub

    Protected Sub ibtnInputRecipientInformationSmartIDProceed_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ResetMessageBox()

        ' ----- Validation ------

        Dim udtSessionHandler As New SessionHandler
        Dim udtSmartIDContent As SmartIDContentModel = udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)

        Dim udtEHSAccount As EHSAccountModel = udtSmartIDContent.EHSAccount
        Session(SESS.EHSAccount) = udtEHSAccount

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, AuditLogDescription.InputRecipientInformationSmartICProceedToEnquiry)

        udcVaccinationRecord.Build(udtEHSAccount, New AuditLogEntry(FunctionCode, Me))
        ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Me.GotoViewResult()
        'MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Result
        ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [End][Koala]

    End Sub

    '

    Private Sub SetupInputRecipientInformationSmartID(ByVal udtEHSAccount As EHSAccountModel, ByVal udtSmartIDContent As SmartIDContentModel)
        ucInputDocumentType.EHSAccount = udtEHSAccount
        ucInputDocumentType.DocType = DocType.DocTypeModel.DocTypeCode.HKIC
        ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.Creation
        ucInputDocumentType.FillValue = True
        'ucInputDocumentType.SchemeClaim = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode)
        ucInputDocumentType.SmartIDContent = udtSmartIDContent
        ucInputDocumentType.Built()

        ' Message box
        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Select Case udtSmartIDContent.SmartIDReadStatus
            ' Validate Acct exist
            Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI, _
                BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB, _
                BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName

                If udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.TwoGender Or _
                    udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.ComboGender Then

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00012)
                Else
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00013)
                End If

            Case Else
                If udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.TwoGender Or _
                    udtSmartIDContent.IdeasVersion = IdeasBLL.EnumIdeasVersion.ComboGender Then

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00011)
                Else
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00007)
                End If

        End Select
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        udcInfoMessageBox.BuildMessageBox()

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'ibtnInputRecipientInformationSmartIDProceed.Enabled = False
        If Not String.IsNullOrEmpty(udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender) Then
            ibtnInputRecipientInformationSmartIDProceed.Enabled = True
            ibtnInputRecipientInformationSmartIDProceed.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToEnquiryBtn")
        Else
            ibtnInputRecipientInformationSmartIDProceed.Enabled = False
            ibtnInputRecipientInformationSmartIDProceed.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToEnquiryDisableBtn")
        End If
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
    End Sub


#End Region

#Region "View 4 - ConfirmRecipientInformation"

    ' Event

    Protected Sub ibtnConfirmRecipientInformationBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        udtAuditLogEntry.AddDescripton("Doc Code", lblCADocumentType.Text)
        udtAuditLogEntry.AddDescripton("Doc No.", lblCADocumentNo.Text)
        udtAuditLogEntry.AddDescripton("Serial No.", lblCASerialNo.Text)
        udtAuditLogEntry.AddDescripton("DOB", lblCADOB.Text)
        udtAuditLogEntry.AddDescripton("Name", lblCAName.Text)
        udtAuditLogEntry.AddDescripton("Gender", lblCAGender.Text)

        udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditLogDescription.ConfirmRecipientInformationBack)

        ResetMessageBox()
        MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.InputRecipientInformation
    End Sub

    Protected Sub ibtnConfirmRecipientInformationProceed_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtSessionHandler As New SessionHandler
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        udtAuditLogEntry.AddDescripton("Doc Code", lblCADocumentType.Text)
        udtAuditLogEntry.AddDescripton("Doc No.", lblCADocumentNo.Text)
        udtAuditLogEntry.AddDescripton("Serial No.", lblCASerialNo.Text)
        udtAuditLogEntry.AddDescripton("DOB", lblCADOB.Text)
        udtAuditLogEntry.AddDescripton("Name", lblCAName.Text)
        udtAuditLogEntry.AddDescripton("Gender", lblCAGender.Text)

        udtAuditLogEntry.WriteLog(LogID.LOG00012, AuditLogDescription.ConfirmRecipientInformationProceed)

        ResetMessageBox()
        udcVaccinationRecord.Build(Session(SESS.EHSAccount), New AuditLogEntry(FunctionCode, Me))
        ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Me.GotoViewResult()
        'MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Result
        ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [End][Koala]
    End Sub

    '

    Private Sub SetupConfirmRecipientInformation(ByVal udtEHSAccount As EHSAccountModel)
        Dim udtPersonalInformation As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        Dim udtDocTypeBLL As New DocTypeBLL
        Dim udtDocType As DocTypeModel = udtDocTypeBLL.getAllDocType().Filter(udtPersonalInformation.DocCode)

        Dim strLanguage As String = Session("language")

        ' Document Type
        lblCADocumentType.Text = udtDocType.DocName(strLanguage)

        ' Document No.
        Dim udtFormatter As New Formatter

        lblCADocumentNoText.Text = udtDocType.DocIdentityDesc(strLanguage)

        lblCADocumentNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtPersonalInformation.DocCode, udtPersonalInformation.IdentityNum, False, udtPersonalInformation.AdoptionPrefixNum)

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        ' EC Serial No.
        If udtDocType.DocCode = DocType.DocTypeModel.DocTypeCode.EC Then
            If udtPersonalInformation.ECSerialNoNotProvided = True Then
                lblCASerialNo.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                lblCASerialNo.Text = udtPersonalInformation.ECSerialNo
            End If

            ViewConfirmRecipientInformation_SerialNoTableRow.Visible = True
        Else
            ViewConfirmRecipientInformation_SerialNoTableRow.Visible = False
        End If
        ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

        ' Date of Birth
        lblCADOB.Text = udtFormatter.formatDOB(udtPersonalInformation.DOB, udtPersonalInformation.ExactDOB, Session("language"), udtPersonalInformation.ECAge, udtPersonalInformation.ECDateOfRegistration)

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        '' In word (HKBC and ADOPC)
        'If udtPersonalInformation.DocCode = DocTypeModel.DocTypeCode.HKBC OrElse udtPersonalInformation.DocCode = DocTypeModel.DocTypeCode.ADOPC Then
        '    If udtPersonalInformation.ExactDOB = "T" OrElse udtPersonalInformation.ExactDOB = "U" OrElse udtPersonalInformation.ExactDOB = "V" Then
        '        Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("DOBInWordType", udtPersonalInformation.OtherInfo)

        '        If strLanguage = CultureLanguage.TradChinese Then
        '            lblCADOB.Text = udtStaticDataModel.DataValueChi.ToString.Trim + " " + lblCADOB.Text
        '        ElseIf strLanguage = CultureLanguage.SimpChinese Then
        '            lblCADOB.Text = udtStaticDataModel.DataValueCN.ToString.Trim + " " + lblCADOB.Text
        '        Else
        '            lblCADOB.Text = udtStaticDataModel.DataValue.ToString.Trim + " " + lblCADOB.Text
        '        End If
        '    End If
        'End If
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        ' Name in English
        lblCAName.Text = udtFormatter.formatEnglishName(udtPersonalInformation.ENameSurName, udtPersonalInformation.ENameFirstName)

        ' Gender
        If udtPersonalInformation.Gender = "M" Then
            lblCAGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
        Else
            lblCAGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        End If

    End Sub

#End Region

#Region "View 5 - Result"

    Protected Sub ibtnInfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = CType(Session(SESS.EHSAccount), EHSAccountModel).EHSPersonalInformationList(0)

        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("Surname", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("Firstname", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)

        udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLogDescription.ResultInfoButton)

        ucVaccinationRecordProvider.Build()
        ModalPopupExtenderVaccinationRecordProvider.Show()
    End Sub

    Protected Sub ibtnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = CType(Session(SESS.EHSAccount), EHSAccountModel).EHSPersonalInformationList(0)

        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("Surname", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("Firstname", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)

        udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditLogDescription.ResultReturn)

        Reset()
    End Sub

    Protected Sub ibtnProceedToClaim_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' --- Migrate variables ---
        Dim udtSessionHandler As New SessionHandler
        Dim udtEHSAccount As EHSAccountModel = Session(SESS.EHSAccount)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSPersonalInfo.DocCode)
        udtAuditLogEntry.AddDescripton("Doc No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("Surname", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("Firstname", udtEHSPersonalInfo.ENameFirstName)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)

        udtAuditLogEntry.WriteLog(LogID.LOG00016, AuditLogDescription.ResultProceedToClaim)

        ' (1) Current SP / Data Entry
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSessionHandler.CurrentUserSaveToSession(udtUserAC, Nothing)
        Else
            udtSessionHandler.CurrentUserSaveToSession((CType(udtUserAC, DataEntryUserModel).ServiceProvider), udtUserAC)
        End If

        ' (2) EHS Account
        If udtEHSAccount.AccountSource <> SysAccountSource.ValidateAccount Then
            Dim strOrigVoucherAccID As Object = udtEHSAccount.VoucherAccID
            udtEHSAccount = udtEHSAccount.CloneData
            udtEHSAccount.VoucherAccID = strOrigVoucherAccID

            If Not (New DocTypeBLL).CheckVaccinationRecordAvailable(udtEHSAccount.EHSPersonalInformationList(0).DocCode) Then
                udtEHSAccount.EHSPersonalInformationList(0).SetDOBTypeSelected(False)
            End If

        End If

        udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctCode.FUNT020201)
        udtSessionHandler.VREEHSAccountSaveToSession(udtEHSAccount)

        ' (3) Smart ID
        Dim udtSmartIDContent As SmartIDContentModel = udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)

        If Not IsNothing(udtSmartIDContent) Then
            If udtEHSAccount.AccountSource <> SysAccountSource.ValidateAccount Then
                'udtSmartIDContent.SmartIDReadStatus = SmartIDHandler.SmartIDResultStatus.EHSAccountNotfound
            End If

            udtSessionHandler.SmartIDContentSaveToSession(FunctCode.FUNT020201, udtSmartIDContent)

        End If


        udtSessionHandler.FromVaccinationRecordEnquirySaveToSession(True)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL("../EHSClaim/EHSClaimV1.aspx")

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

    ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Sub GotoViewResult()
        Dim udtSessionHandler As New SessionHandler
        udtSessionHandler.CMSVaccineResultSaveToSession(udcVaccinationRecord.HAVaccineResult, FunctionCode)
        Me.MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Result
    End Sub
    ' CRE11-028 Removing the 2nd eVaccination record sharing enquiry (backend) when 1st enquiry is success [End][Koala]
#End Region

#Region "Reset function"

    Private Sub Reset()
        MultiViewVaccinationRecordEnquiry.ActiveViewIndex = ViewIndex.Search
        udcDocumentTypeRadioButtonGroup.SelectedValue = String.Empty
        ' INT16-0020 Fix HCSP VRE EC Input DOB Type Error [Start][Lawrence]
        rblECDOBType.ClearSelection()
        rblIAGender.ClearSelection()
        ' INT16-0020 Fix HCSP VRE EC Input DOB Type Error [End][Lawrence]
        BindControl(True)
        udcClaimSearch.CleanField()
        Session.Remove(SESS.EHSAccount)
        Session.Remove(SESS.SearchAccountRemark)
        Dim udtSessionHandler As New SessionHandler
        udtSessionHandler.SmartIDContentRemoveFormSession(FunctionCode)
    End Sub

    Private Sub ResetMessageBox()
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False
    End Sub

    Private Sub ResetErrorImage()
        imgIANameError.Visible = False
        imgIAGenderError.Visible = False
        'imgHKBCDOBError.Visible = False
        'imgHKBCDOBInWordError.Visible = False

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        imgECSerialNo.Visible = False
        ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
        imgECDOBError.Visible = False
        imgECDOBTypeError.Visible = False
    End Sub

#End Region

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Dim udtEHSAccountModel As EHSAccountModel = Session(SESS.EHSAccount)
        If Not IsNothing(udtEHSAccountModel) Then
            Return udtEHSAccountModel.SearchDocCode
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSAccountModel As EHSAccountModel = Session(SESS.EHSAccount)
        If Not IsNothing(udtEHSAccountModel) Then
            Return udtEHSAccountModel
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
    ' ----------------------------------------------------------
    Protected Sub cboECSerialNoNotProvided_CheckedChanged(sender As Object, e As EventArgs) Handles cboECSerialNoNotProvided.CheckedChanged
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Previous Serial No.", txtECSerialNo.Text)
        udtAuditLogEntry.AddDescripton("Checked after", IIf(cboECSerialNoNotProvided.Checked, "Y", "N"))
        udtAuditLogEntry.WriteLog(LogID.LOG00017, "Serial No. Not Provided checked")

        'If cboECSerialNoNotProvided.Checked Then
        '    ViewState(VS.SerialNumberNotProvided) = "Y"
        'Else
        '    ViewState(VS.SerialNumberNotProvided) = "N"
        'End If

        EnableSerialNo(Not cboECSerialNoNotProvided.Checked)
    End Sub

    Private Sub EnableSerialNo(ByVal blnEnable As Boolean)
        If blnEnable = False Then
            txtECSerialNo.Enabled = False
            txtECSerialNo.Text = String.Empty
            txtECSerialNo.BackColor = Drawing.Color.LightGray
        Else
            txtECSerialNo.Enabled = True
            txtECSerialNo.BackColor = Nothing
        End If
    End Sub
    ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
End Class