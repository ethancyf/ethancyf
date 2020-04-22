Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.UserAC
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL
Imports common.Component.DocType
Imports Common.Component.Scheme

' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
' -----------------------------------------------------------------------------------------

Imports Common.Component.EHSAccount

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

Partial Public Class VoidClaimSearch
    Inherits TextOnlyBasePage

    Public Class AublitLogDescription
        Public Const LoadVoidTransaction As String = "Void Transaction Loaded"
        Public Const SearchTransaction As String = "Search Transaction"
        Public Const SearchTransactionByTranNo As String = "Search Transaction By Transaction No"
        Public Const SearchTransactionByHKIDDOB As String = "Search Transaction By HKID and DOB"
        Public Const SearchTransactionByHKIDAge As String = "Search Transaction By HKID and Age Reported"
        Public Const SearchFail As String = "Search Fail"
        Public Const SearchSuccess As String = "Search Result"
    End Class

    Dim _udtGeneralFunction As GeneralFunction
    Dim _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Dim _udtSystemMessage As SystemMessage
    Dim _udtSP As ServiceProviderModel
    Dim _strValidationFail As String = "ValidationFail"
    Private _udtUserAC As UserACModel = New UserACModel()

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020303

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Public Enum SearchType
        Empty
        TransactionNo
        EHealthAccount
    End Enum

    Public Enum ActiveViewIndex
        SelectSearchType = 0
        SearchTransaction = 1
        SelectDocType = 2
        InputTip = 3
    End Enum



    'Private Class SearchType
    '    Public Const TransactionNo As String = "TransactionNo"
    '    Public Const eHealthAccount As String = "eHealthAccount"
    'End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load 'MyBase.Load, Me.Load

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
        If Not IsPostBack And Not Me.mvVoidClaimTransaction.ActiveViewIndex > 0 Then
            ' Init the Active View in Page Load only
            Me.mvVoidClaimTransaction.ActiveViewIndex = 0
        End If
        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        'Set no Cache
        Response.Cache.SetNoStore()
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(Now.AddDays(-1))

        Me._udtSessionHandler.EHSTransactionListPageRemoveFromSession(FunctCode)

        Dim strDocumentType As String = Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode)
        Dim masterPage As ClaimVoucherMaster = CType(Me.Master, ClaimVoucherMaster)
        Dim udtDataEntryUser As DataEntryUserModel = Nothing

        'Get Current USer Account for check Session Expired
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

        'Initialize MasterPage
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetGlobalResourceObject("Text", "VoidClaim")
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblTitle), Label).Text = Me.GetGlobalResourceObject("Text", "EVoucherSystem")

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblSubTitle), Label).Text = Me.GetGlobalResourceObject("Text", "ClaimTransactionManagement")

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        'Dim dtMenu As DataTable = 
        masterPage.BuildMenu(FunctCode, Me._udtSessionHandler.Language())

        AddHandler masterPage.MenuChanged, AddressOf MasterPage_MenuChanged

        If Not IsPostBack Then

            'Log Page Load 
            AuditLogPageLoaded()

            Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SelectSearchType

            If Not Me._udtSessionHandler.EHSTransactionSearchByNoGetFromSession(FunctCode) = Nothing Then
                If Me._udtSessionHandler.EHSTransactionSearchByNoGetFromSession(FunctCode) Then
                    Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SearchTransaction
                End If
            End If

            If Me._udtSessionHandler.TextOnlyVersionSearchTypeGetFromSession = SearchType.EHealthAccount Then
                SearchByEHealthAccount()
            End If

            If strDocumentType Is Nothing OrElse strDocumentType.Equals(String.Empty) Then
                Me._udtSessionHandler.DocumentTypeSelectedSaveToSession(Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC, FunctCode)
            End If

            Me.GetCurrentUserAccount(Me._udtSP, udtDataEntryUser, True)
        Else
            Select Case Me.mvVoidClaimTransaction.ActiveViewIndex

                Case ActiveViewIndex.SearchTransaction
                    Me.SetupSearchTransaction()

                Case ActiveViewIndex.SelectDocType
                    Me.SetupSelectDocType()

                Case ActiveViewIndex.InputTip
                    Me.SetupInputTips()
            End Select
        End If
    End Sub

    Protected Sub MasterPage_MenuChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me._udtSessionHandler.EHSTransactionSearchByNoRemoveFromSession(FunctCode)
        Me._udtSessionHandler.TextOnlyVersionSearchTypeRemoveFromSession()
        Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
    End Sub


    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    '    If IsPostBack Then
    '        Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
    '        If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) Then
    '            RenderLanguage()
    '        End If
    '    End If
    'End Sub

    Private Sub mvVoidClaimTransaction_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvVoidClaimTransaction.ActiveViewChanged
        Me.udcMsgBoxErr.Clear()

        Select Case Me.mvVoidClaimTransaction.ActiveViewIndex
            Case ActiveViewIndex.SearchTransaction
                Me.SetupSearchTransaction()
                Me.ResetSearchTransaction()
                Me.ClearWorkingData()

            Case ActiveViewIndex.SelectDocType

                Me.SetupSelectDocType()

            Case ActiveViewIndex.InputTip
                Me.SetupInputTips()
        End Select

    End Sub


#Region "Search Transaction"

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTranSearch.Click
        Dim udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL
        Dim systemMessage As SystemMessage = Nothing
        Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection = Nothing
        Dim strTransactionNo As String
        Dim isValid As Boolean = True
        Dim udtDataEntryUser As DataEntryUserModel = Nothing
        Dim udtEHSTransaction As EHSTransactionModel = Nothing
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSTransactions As EHSTransactionModelCollection = Nothing
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeClaimModel As SchemeClaimModel
        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me._udtSessionHandler.CurrentUserGetFromSession(Me._udtSP, udtDataEntryUser)
        Me.lblSearchTranNoError.Visible = False

        If Me._udtSessionHandler.TextOnlyVersionSearchTypeGetFromSession() = SearchType.TransactionNo Then
            strTransactionNo = Me.txtSearchTranTransactionNo.Text
            'AuditLog
            AuditLogSearchStart(udtAuditLogEntry, strTransactionNo)

            If Not Me.StepSearchTranTransactionNoValidation(strTransactionNo) Then
                Me.lblSearchTranNoError.Visible = True
                isValid = False
            Else
                udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(Formatter.ReverseSystemNumber(strTransactionNo))
            End If

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Not udtEHSTransaction Is Nothing Then
                Dim lstSchemeCode As New List(Of String)
                lstSchemeCode.Add(udtEHSTransaction.SchemeCode.Trim)
                udtSchemeClaimModelCollection = udtSchemeClaimBLL.getAllEffectiveSchemeClaimBySchemeCodeList(lstSchemeCode)

                For Each udtSchemeClaimModel In udtSchemeClaimModelCollection
                    If Not udtSchemeClaimModel.AvailableHCSPSubPlatform.ToString.Equals(Me.SubPlatform.ToString) Then
                        udtEHSTransaction = Nothing
                    End If
                Next
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If isValid Then

                ' CRE13-001 - EHAPP [Start][Koala]
                ' -------------------------------------------------------------------------------------
                If udtEHSTransaction Is Nothing Then
                    systemMessage = New SystemMessage(Common.Component.FunctCode.FUNT020303, "E", "00002")
                    'systemMessage = udtEHSTransactionBLL.chkEHSTranVaildForVoid(udtEHSTransaction, Me._udtSP, udtDataEntryUser)
                End If
                ' CRE13-001 - EHAPP [End][Koala]
                If Not systemMessage Is Nothing Then
                    isValid = False
                    Me.udcMsgBoxErr.AddMessage(systemMessage)
                Else
                    Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                    ' -----------------------------------------------------------------------------------------

                    Dim udtPracticeDisplays As BLL.PracticeDisplayModelCollection
                    udtPracticeDisplays = _udtSessionHandler.PracticeDisplayListGetFromSession()
                    Dim udtSelectedPracticeDisplay As BLL.PracticeDisplayModel = udtPracticeDisplays.Filter(udtEHSTransaction.PracticeID)
                    _udtSessionHandler.PracticeDisplaySaveToSession(udtSelectedPracticeDisplay, FunctCode)

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                    'AuditLog
                    AuditLogSearchComplete(udtAuditLogEntry, udtEHSTransaction)

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                    RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.ConfirmTransaction)

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                End If
            End If

            If Not isValid Then
                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00006, "Search By Transaction No. Failed")
            End If

        Else
            Dim dtmDOB As DateTime
            Dim dtmDOR As DateTime
            Dim intAge As Integer
            Dim strIdentity As String = String.Empty
            Dim strADOPCPrefix As String = String.Empty
            Dim strInputDOBFormat As String = String.Empty

            isValid = Me.SearchTranDocTypeValidation(strIdentity, strADOPCPrefix, dtmDOB, intAge, dtmDOR, strInputDOBFormat)

            'AuditLog
            AuditLogSearchStart(udtAuditLogEntry, Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode), strIdentity, dtmDOB, intAge, dtmDOR)

            If isValid Then

                Dim strDocumentType As String = Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode)
                Dim strDataEntryUser As String = String.Empty


                If strADOPCPrefix Is Nothing Then
                    strADOPCPrefix = String.Empty
                End If

                If Not udtDataEntryUser Is Nothing Then
                    strDataEntryUser = udtDataEntryUser.DataEntryAccount
                End If

                Me.udcSearchTranClaimSearch.SetProperty(strDocumentType)


                ' CRE13-001 - EHAPP [Start][Koala]
                ' -------------------------------------------------------------------------------------
                Select Case strDocumentType
                    Case DocTypeModel.DocTypeCode.EC
                        If intAge = 0 Then
                            udtEHSTransactions = udtEHSTransactionBLL.SearchEHSTransaction(strDocumentType, strIdentity, strADOPCPrefix, dtmDOB, strInputDOBFormat, Me._udtSP.SPID, strDataEntryUser, Me.SubPlatform)
                        Else
                            udtEHSTransactions = udtEHSTransactionBLL.SearchEHSTransaction(strDocumentType, strIdentity, intAge, dtmDOR, Me._udtSP.SPID, strDataEntryUser, Me.SubPlatform)
                        End If

                    Case Else
                        udtEHSTransactions = udtEHSTransactionBLL.SearchEHSTransaction(strDocumentType, strIdentity, strADOPCPrefix, dtmDOB, strInputDOBFormat, Me._udtSP.SPID, strDataEntryUser, Me.SubPlatform)
                End Select
                ' CRE13-001 - EHAPP [End][Koala]

                If Not udtEHSTransactions Is Nothing AndAlso udtEHSTransactions.Count > 0 Then

                    Me._udtSessionHandler.EHSTransactionListSaveToSession(udtEHSTransactions, FunctCode)

                    Me._udtSessionHandler.eHSAccDocTypeAndDocNumSaveToSession(strDocumentType, strIdentity, FunctCode)

                    'AuditLog
                    AuditLogSearchComplete(udtAuditLogEntry, udtEHSTransactions)

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                    RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.SelectTransation)

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                Else
                    isValid = False

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                    ' -----------------------------------------------------------------------------------------

                    Me.udcMsgBoxErr.AddMessage(New SystemMessage(Common.Component.FunctCode.FUNT020303, "E", "00002"))

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                End If

            End If

            If Not isValid Then

                Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00017, "Search By eHealth Account Failed", _
                                                New Common.ComObject.AuditLogInfo("", "", "", "", Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode), udtFormatter.formatDocumentIdentityNumber(Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode), strIdentity)))
            End If

        End If

    End Sub

    Protected Sub btnSearchTranCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTranCancel.Click
        Me._udtSessionHandler.TextOnlyVersionSearchTypeRemoveFromSession()
        Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SelectSearchType
    End Sub

    Protected Sub btnSearchTranSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTranSearch.Click

    End Sub

    Protected Sub btnSearchTranChangeDocType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTranChangeDocType.Click
        Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SelectDocType
    End Sub

    Private Sub udcSearchTranClaimSearch_ShowInputTipsClick(ByVal type As ucInputTips.InputTipsType) Handles udcSearchTranClaimSearch.ShowInputTipsClick
        Me.udcInputTips.LoadTip(type)
        Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.InputTip
    End Sub

    Private Sub SetupSearchTransaction()
        Dim strDocumentType As String = Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode)
        Me.panSearchTranTransactionNo.Visible = False
        Me.panSearchTranDocType.Visible = False

        If Me._udtSessionHandler.TextOnlyVersionSearchTypeGetFromSession() = SearchType.TransactionNo Then
            Me.panSearchTranTransactionNo.Visible = True
        Else
            Me.lblSearchTranDocType.Text = Me.GetDocumentTypeDesc(strDocumentType)
            ' Me.btnSearchTranChangeDocType.Text = Me.GetGlobalResourceObject("AlternateText", "ChangeDocumentTypeBtn")
            Me.btnSearchTranChangeDocType.Text = Me.GetGlobalResourceObject("AlternateText", "Change")
            Me.panSearchTranDocType.Visible = True
        End If

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.udcSearchTranClaimSearch.UIEnableHKICSymbol = False
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        Me.udcSearchTranClaimSearch.Build(strDocumentType)

    End Sub

    Private Sub ResetSearchTransaction()
        Me.udcMsgBoxErr.Clear()
        Me.lblSearchTranNoError.Visible = False
        Me.txtSearchTranTransactionNo.Text = String.Empty

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.udcSearchTranClaimSearch.ResetHKIC()
        Me.udcSearchTranClaimSearch.ResetADOPC()
        Me.udcSearchTranClaimSearch.ResetEC()
        Me.udcSearchTranClaimSearch.ResetSearchShort()

        Me.udcSearchTranClaimSearch.SetHKICError(False)
        Me.udcSearchTranClaimSearch.SetADOPCError(False)
        Me.udcSearchTranClaimSearch.SetECError(False)
        Me.udcSearchTranClaimSearch.SetSearchShortError(False)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
    End Sub

#End Region

#Region "Select Search Type"

    Protected Sub btnSearchTranType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTranType.Click
        Me._udtSessionHandler.TextOnlyVersionSearchTypeSaveToSession(SearchType.TransactionNo)
        AuditLogSearchType(True)
        Me._udtSessionHandler.EHSTransactionSearchByNoSaveToSession(True, FunctCode)
        Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SearchTransaction
    End Sub

    Protected Sub btnSearchTranTypeEHealthAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTranTypeEHealthAccount.Click
        SearchByEHealthAccount()
    End Sub

    Private Sub SearchByEHealthAccount()
        Me._udtSessionHandler.TextOnlyVersionSearchTypeSaveToSession(SearchType.EHealthAccount)
        AuditLogSearchType(False)
        Me._udtSessionHandler.EHSTransactionSearchByNoSaveToSession(False, FunctCode)
        Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SearchTransaction
    End Sub

#End Region

#Region "Search Transaction Validation"

    'Transaction Valdiation
    Private Function StepSearchTranTransactionNoValidation(ByVal strTransactionNo As String) As Boolean
        Dim isValid As Boolean = True
        Dim udtValidator As Validator = New Validator()
        strTransactionNo = strTransactionNo.Trim().ToUpper()

        If strTransactionNo.Trim().Equals(String.Empty) Then

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Me.udcMsgBoxErr.AddMessage(New SystemMessage("020303", "E", "00001"))

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            isValid = False
        Else

            If Not udtValidator.chkSystemNumber(strTransactionNo) Then

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------

                Me.udcMsgBoxErr.AddMessage(New SystemMessage("020303", "E", "00002"))

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                isValid = False
            End If

        End If

        Return isValid
    End Function

    'Swtich Doc Type
    Private Function SearchTranDocTypeValidation(ByRef strIdendityNo As String, ByRef strADOPCPrefix As String, ByRef dtmDOB As DateTime, ByRef intAge As Integer, ByRef dtmDOA As DateTime, ByRef strInputDOBFormat As String) As Boolean
        Dim isValid As Boolean = True
        Dim udtValidator As Validator = New Validator()
        Dim strDocCode As String = Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode).Trim()

        Me.udcSearchTranClaimSearch.SetProperty(strDocCode)
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.udcSearchTranClaimSearch.SetHKICError(False)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        Me.udcSearchTranClaimSearch.SetECError(False)
        Me.udcSearchTranClaimSearch.SetADOPCIdentityNoError(False)
        Me.udcSearchTranClaimSearch.SetSearchShortIdentityNoError(False)

        Me._udtSystemMessage = udtValidator.chkIdentityNumber(strDocCode, Me.udcSearchTranClaimSearch.IdentityNo.ToUpper(), Me.udcSearchTranClaimSearch.IdentityNoPrefix)
        If Not Me._udtSystemMessage Is Nothing Then
            isValid = False
            Select Case strDocCode.Trim()
                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Case DocTypeModel.DocTypeCode.HKIC
                    Me.udcSearchTranClaimSearch.SetHKICError(True)
                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                Case DocTypeModel.DocTypeCode.EC
                    Me.udcSearchTranClaimSearch.SetECHKIDError(True)
                Case DocTypeModel.DocTypeCode.ADOPC
                    Me.udcSearchTranClaimSearch.SetADOPCIdentityNoError(True)
                Case Else
                    Me.udcSearchTranClaimSearch.SetSearchShortIdentityNoError(True)
            End Select

            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
            'Else
            strIdendityNo = Me.udcSearchTranClaimSearch.IdentityNo.ToUpper()
            strADOPCPrefix = Me.udcSearchTranClaimSearch.IdentityNoPrefix.ToUpper()
        End If

        strIdendityNo = Me.udcSearchTranClaimSearch.IdentityNo.ToUpper()
        strADOPCPrefix = Me.udcSearchTranClaimSearch.IdentityNoPrefix.ToUpper()

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC, _
                 DocTypeModel.DocTypeCode.HKBC, _
                 DocTypeModel.DocTypeCode.DI, _
                 DocTypeModel.DocTypeCode.REPMT, _
                 DocTypeModel.DocTypeCode.ID235B, _
                 DocTypeModel.DocTypeCode.VISA

                If Not Me.SearchTranShotIdentityNoValidation(dtmDOB, strInputDOBFormat) Then
                    isValid = False
                Else
                    strInputDOBFormat = strInputDOBFormat.ToUpper()
                End If

            Case DocTypeModel.DocTypeCode.EC
                If Not Me.SearchTranECValidation(dtmDOB, intAge, dtmDOA, strInputDOBFormat) Then
                    isValid = False
                Else
                    strInputDOBFormat = strInputDOBFormat.ToUpper()
                End If

            Case DocTypeModel.DocTypeCode.ADOPC
                If Not Me.SearchTranADOPCValidation(dtmDOB, strInputDOBFormat) Then
                    isValid = False
                Else
                    strInputDOBFormat = strInputDOBFormat.ToUpper()
                End If

        End Select

        Return isValid
    End Function

    'EC Validation
    Private Function SearchTranECValidation(ByRef dtmDOB As DateTime, ByRef intAge As Integer, ByRef dtmDOA As DateTime, ByRef strInputDOBFormat As String) As Boolean
        Dim isValid As Boolean = True
        Dim udtValidator As Validator = New Validator()
        Dim formatter As Formatter = New Formatter()

        Me.udcSearchTranClaimSearch.SetProperty(DocTypeModel.DocTypeCode.EC)

        ' Validate DOB
        If Me.udcSearchTranClaimSearch.ECDOBSelected Then
            Dim udtSysMsgValidateDOB As SystemMessage = udtValidator.chkDOB(DocTypeModel.DocTypeCode.EC, Me.udcSearchTranClaimSearch.DOB, dtmDOB, strInputDOBFormat)
            If Not udtSysMsgValidateDOB Is Nothing Then
                ' Error
                isValid = False

                Me.udcSearchTranClaimSearch.SetECDOBError(True)

                Me.udcMsgBoxErr.AddMessage(udtSysMsgValidateDOB)
            End If
        Else
            ' Validate EC Age
            Dim udtSystemMessage As SystemMessage
            Dim strDOADay As String = Me.udcSearchTranClaimSearch.ECDOADay
            Dim strDOAMonth As String = Me.udcSearchTranClaimSearch.ECDOAMonth
            Dim strDOAYear As String = Me.udcSearchTranClaimSearch.ECDOAYear

            udtSystemMessage = udtValidator.chkECAge(Me.udcSearchTranClaimSearch.ECAge)
            If Not udtSystemMessage Is Nothing Then
                ' Error
                isValid = False

                Me.udcSearchTranClaimSearch.SetECDOAAgeError(True)

                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            Else
                intAge = CInt(Me.udcSearchTranClaimSearch.ECAge)
            End If

            ' Validate Date of Age
            udtSystemMessage = udtValidator.chkECDOAge(strDOADay, strDOAMonth, strDOAYear)
            If Not udtSystemMessage Is Nothing Then
                ' Error
                isValid = False

                Me.udcSearchTranClaimSearch.SetECDOAError(True)

                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            Else
                Dim strDateOfReg As String = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDOADay), strDOAMonth, strDOAYear)

                dtmDOA = CDate(formatter.convertDate(strDateOfReg, Me._udtSessionHandler.Language))
            End If

            'If isValid Then
            '    dtmDOB = CDate(dtmDOA.Year - intAge)
            'End If

            '' Validate Age + Date of Age if Within Age
            'If isValid Then
            '    udtSystemMessage = udtValidator.chkECAgeAndDOAge(Me.udcSearchTranClaimSearch.ECAge, strDOADay, strDOAMonth, strDOAYear)
            '    If Not udtSystemMessage Is Nothing Then
            '        ' Error
            '        isValid = False

            '        Me.udcSearchTranClaimSearch.SetECDOAAgeError(True)
            '        Me.udcSearchTranClaimSearch.SetECDOAError(True)

            '        Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
            '    End If
            'End If
        End If

        Return isValid
    End Function

    'HKID, HKBC, DI................. Validation 
    Private Function SearchTranShotIdentityNoValidation(ByRef dtmDOB As DateTime, ByRef strInputDOBFormat As String) As Boolean
        Dim isValid As Boolean = True
        Dim udtValidator As Validator = New Validator()
        Dim strDocCode As String = Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode).Trim()
        Dim formatter As Formatter = New Formatter()

        Me.udcSearchTranClaimSearch.SetProperty(strDocCode)

        ' Validate DOB
        Me._udtSystemMessage = udtValidator.chkDOB(strDocCode, Me.udcSearchTranClaimSearch.DOB, dtmDOB, strInputDOBFormat)
        If Not Me._udtSystemMessage Is Nothing Then
            ' Error
            isValid = False
            Me.udcSearchTranClaimSearch.SetSearchShortDOBError(True)

            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
        End If


        Return isValid
    End Function

    'ADOPC Validation
    Private Function SearchTranADOPCValidation(ByRef dtmDOB As DateTime, ByRef strInputDOBFormat As String) As Boolean
        Dim isValid As Boolean = True
        Dim udtValidator As Validator = New Validator()
        Dim strDocCode As String = Me._udtSessionHandler.DocumentTypeSelectedGetFromSession(FunctCode).Trim()
        Dim formatter As Formatter = New Formatter()

        Me.udcSearchTranClaimSearch.SetProperty(DocTypeModel.DocTypeCode.ADOPC)

        ' Validate DOB
        Me._udtSystemMessage = udtValidator.chkDOB(strDocCode, Me.udcSearchTranClaimSearch.DOB, dtmDOB, strInputDOBFormat)
        If Not Me._udtSystemMessage Is Nothing Then
            ' Error
            isValid = False
            Me.udcSearchTranClaimSearch.SetADOPCDOBError(True)

            Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
        End If


        Return isValid
    End Function

#End Region

#Region "Select Doc Type"

    Protected Sub btnStepSelectDocTypeSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStepSelectDocTypeSelect.Click
        Me._udtSessionHandler.DocumentTypeSelectedSaveToSession(Me.ucDocumentTypeRadioButtonGroupText.SelectedValue, FunctCode)
        Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SearchTransaction

    End Sub

    Private Sub SetupSelectDocType()
        Me.lblStepSelectDocTypeDocTypeText.Text = Me.GetGlobalResourceObject("Text", "SelectDocType")

        Me.btnStepSelectDocTypeSelect.Text = Me.GetGlobalResourceObject("AlternateText", "SelectBtn")
        Me.ucDocumentTypeRadioButtonGroupText.ShowAll = True
        Me.ucDocumentTypeRadioButtonGroupText.EnableFilterDocCode = CustomControls.DocumentTypeRadioButtonGroupText.FilterDocCode.VaccinationRecordEnquriySearch
        Me.ucDocumentTypeRadioButtonGroupText.Build()

    End Sub

#End Region

#Region "Input Tips"

    Private Sub btnInputTipBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputTipBack.Click
        Me.mvVoidClaimTransaction.ActiveViewIndex = ActiveViewIndex.SearchTransaction
    End Sub

    Private Sub SetupInputTips()
        Me.udcInputTips.LoadTip()
    End Sub

#End Region

#Region "Other function"

    '-------------------------------------------------------------------------------------------------------------------
    ' Get current user Account from database and save to session
    '-------------------------------------------------------------------------------------------------------------------
    Private Sub GetCurrentUserAccount(ByRef passedSP As ServiceProviderModel, ByRef passedDataEntry As DataEntryUserModel, ByVal getFormDataBase As Boolean)
        Dim udtClaimVoucherBLL As BLL.ClaimVoucherBLL = New BLL.ClaimVoucherBLL()
        'Get Current USer Account
        Me._udtUserAC = UserACBLL.GetUserAC

        If getFormDataBase Then

            If Me._udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                'Get SP from form database
                passedSP = CType(_udtUserAC, ServiceProviderModel)
                passedSP = udtClaimVoucherBLL.loadSP(passedSP.SPID)

                passedDataEntry = Nothing

            ElseIf Me._udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
                passedDataEntry = CType(_udtUserAC, DataEntryUserModel)

                'Get the latest data entry account mordel form database
                Dim udtDataEntryAcctBLL As BLL.DataEntryAcctBLL = New BLL.DataEntryAcctBLL
                passedDataEntry = udtDataEntryAcctBLL.LoadDataEntry(passedDataEntry.SPID, passedDataEntry.DataEntryAccount)

                'Get the latest service provider account mordel 
                passedSP = udtClaimVoucherBLL.loadSP(passedDataEntry.SPID)
            End If

            Me._udtSessionHandler.CurrentUserSaveToSession(passedSP, passedDataEntry)
        Else
            Me._udtSessionHandler.CurrentUserGetFromSession(passedSP, passedDataEntry)
        End If

    End Sub

    '-------------------------------------------------------------------------------------------------------------------
    ' Fill in Session's Select Document Type into a label
    '-------------------------------------------------------------------------------------------------------------------
    Private Function GetDocumentTypeDesc(ByVal strDocCode As String) As String
        Dim udtDocTypeBLL As New DocTypeBLL()
        Dim udtDocType As Common.Component.DocType.DocTypeModel

        If String.IsNullOrEmpty(strDocCode) Then
            udtDocType = udtDocTypeBLL.getAllDocType().Filter(Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC)
        Else
            udtDocType = udtDocTypeBLL.getAllDocType().Filter(strDocCode)
        End If

        Return IIf(Me._udtSessionHandler.Language = Common.Component.CultureLanguage.English, udtDocType.DocName, udtDocType.DocNameChi)
    End Function

#End Region

#Region "Audit Log"

    'Page Loaded : LOG00000
    Public Sub AuditLogPageLoaded()

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, "Void Claim Transaction Page Loaded")
    End Sub

    'Page Load : LOG00001
    Public Sub AuditLogSearchType(ByVal searchByTransaction As Boolean)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctCode, Me)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        If searchByTransaction Then
            udtAuditLogEntry.AddDescripton("Search Type", "Transaction No")
        Else
            udtAuditLogEntry.AddDescripton("Search Type", "EHS Account")
        End If

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00001, "Search Type Selected")
    End Sub

    'Search by Transaction No : LOG00002
    Public Sub AuditLogSearchStart(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strTransactionNo As String)

        udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00002, "Search by Transaction No. Start")
    End Sub

    'Search by EHS Account : LOG00003 
    Public Sub AuditLogSearchStart(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strDoctype As String, ByVal strHKID As String, ByVal strDOB As String, ByVal strAge As String, ByVal strDateOfReg As String)

        udtAuditLogEntry.AddDescripton("Search Doc Code", strDoctype)
        udtAuditLogEntry.AddDescripton("HKID", strHKID)
        udtAuditLogEntry.AddDescripton("DOB", strDOB)

        If Not String.IsNullOrEmpty(strAge) Then
            udtAuditLogEntry.AddDescripton("Age", strAge)
        End If
        If Not String.IsNullOrEmpty(strDateOfReg) Then
            udtAuditLogEntry.AddDescripton("Date Of Reg", strDateOfReg)
        End If

        Dim udtFormatter As New Common.Format.Formatter

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00003, "Search by eHealth Account Start", New Common.ComObject.AuditLogInfo("", "", "", "", strDoctype, udtFormatter.formatDocumentIdentityNumber(strDoctype, strHKID)))
    End Sub

    'Search by EHS Account : LOG00004 : Complete
    Public Sub AuditLogSearchComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel)
        udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
        udtAuditLogEntry.AddDescripton("Transaction Date", udtEHSTransaction.TransactionDtm)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
        udtAuditLogEntry.AddDescripton("Create By", udtEHSTransaction.CreateBy)
        udtAuditLogEntry.AddDescripton("Update By", udtEHSTransaction.UpdateBy)
        udtAuditLogEntry.AddDescripton("Data Entry By", udtEHSTransaction.DataEntryBy)
        udtAuditLogEntry.AddDescripton("Service Date", udtEHSTransaction.ServiceDate)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode)
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHCVS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                'no text version

            Case SchemeClaimModel.EnumControlType.EVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogEVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.CIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogCIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHSIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.RVP
                udtAuditLogEntry = EHSClaimBasePage.AuditLogRVP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogPIDVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udtAuditLogEntry = EHSClaimBasePage.AuditLogENHVSSO(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PPP
                'no text version
        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00004, "Search by Transaction No. Complete")
    End Sub

    'udtEHSTransactions As EHSTransactionModelCollection

    'Search by EHS Account : LOG00005 : Complete
    Public Sub AuditLogSearchComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransactions As EHSTransactionModelCollection)
        For Each udtEHSTransaction As EHSTransactionModel In udtEHSTransactions
            udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
            udtAuditLogEntry.AddDescripton("Transaction Date", udtEHSTransaction.TransactionDtm)
            udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
            udtAuditLogEntry.AddDescripton("Scheme Code", udtEHSTransaction.SchemeCode)
        Next

        Dim strDocType As String = String.Empty
        Dim strIdentityNum As String = String.Empty

        Me._udtSessionHandler.eHSAccDocTypeAndDocNumGetFromSession(strDocType, strIdentityNum, FunctCode)

        Dim udtFormatter As New Common.Format.Formatter

        strIdentityNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00005, "Search by eHealth Account Complete", New Common.ComObject.AuditLogInfo("", "", "", "", strDocType, strIdentityNum))
    End Sub

    'Search by EHS Account : LOG00006 : Failed

#End Region


#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.DocCode
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.EHSAcct
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
        Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
    End Sub

#End Region

End Class


