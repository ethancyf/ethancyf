Imports AjaxControlToolkit
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.SearchCriteria
Imports HCVU.BLL
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Format



Partial Public Class ReprintVaccinationRecord
    Inherits BasePageWithControl

    Private _udtGeneralFunction As New GeneralFunction
    Private _udtSessionHandler As New SessionHandlerBLL

#Region "Private Classes"
    Private Class ViewIndex
        Public Const Search As Integer = 0
        Public Const Detail As Integer = 1
    End Class

#End Region

#Region "Session Constants"

    Private Const SESS_EHSTransaction As String = "010421_EHSTransaction"


#End Region

#Region "Fields"
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtDocTypeBLL As New DocTypeBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtSessionHandlerBLL As New SessionHandlerBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ' Set function code
            FunctionCode = FunctCode.FUNT010421

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Reprint Vaccination Record Page Loaded")
            InitializeDataValue()

            '==================================================================== Code for SmartID ============================================================================
            Dim udtSmartIDContent As SmartIDContentModel = _udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)

            Me.ReadSmartID(udtSmartIDContent)

            '==================================================================================================================================================================
        Else
            If MultiViewReprintVaccinationRecord.ActiveViewIndex = ViewIndex.Detail Then
                ' Rebind the details
                Dim udtEHSTransaction As EHSTransactionModel = Session(SESS_EHSTransaction)
                BuildClaimTransDetail(udtEHSTransaction.TransactionID)
            End If

        End If

        If SmartIDHandler.EnableSmartID Then
            ibtnSearchByCard.Visible = True
        Else
            ibtnSearchByCard.Visible = False
        End If

    End Sub
#End Region

#Region "Support Function"
    Private Sub BindDocumentType(ByVal ddlEHealthDocType As DropDownList)
        Dim udtDocTypeModelList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim udtDocTypeModelListFilter As New DocTypeModelCollection
        Dim strMajorDoc As String = String.Empty

        ' Display doc type for COVID19 scheme
        Dim udtSchemeDocTypeModelist As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(SchemeClaimModel.COVID19CVC)

        For Each udtDocType As DocTypeModel In udtDocTypeModelList
            For Each udtSchemeClaim As SchemeDocTypeModel In udtSchemeDocTypeModelist
                If udtSchemeClaim.DocCode = udtDocType.DocCode Then
                    udtDocTypeModelListFilter.Add(udtDocType)
                    If udtSchemeClaim.IsMajorDoc Then
                        strMajorDoc = udtDocType.DocCode
                    End If
                End If

            Next
        Next

        ddlEHealthDocType.Items.Clear()
        ddlEHealthDocType.DataSource = udtDocTypeModelListFilter
        ddlEHealthDocType.DataTextField = "DocName"
        ddlEHealthDocType.DataValueField = "DocCode"
        ddlEHealthDocType.DataBind()
        ddlEHealthDocType.SelectedValue = strMajorDoc
    End Sub

    Private Sub BuildClaimTransDetail(ByVal strTransactionNo As String)

        MultiViewReprintVaccinationRecord.ActiveViewIndex = ViewIndex.Detail
        Dim udtSearchCriteria As New SearchCriteria
        udtSearchCriteria.TransNum = strTransactionNo

        Dim ucMessageHistory As ClaimTransDetail = udcClaimTransDetail
        'Me.viewMessageHistory.FindControl("ucMessageHistory")
        udcClaimTransDetail.ClearDocumentType()
        udcClaimTransDetail.ClearEHSClaim()
        udcClaimTransDetail.ClearVaccineRecord()

        Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionNo, True, True)
        ucMessageHistory.ShowHKICSymbol = True
        ucMessageHistory.ShowAccountIDAsBtn = False
        ucMessageHistory.FunctionCode = FunctionCode

        If udtEHSTransaction IsNot Nothing Then
            ucMessageHistory.LoadTranInfo(udtEHSTransaction, udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria))

            Session(SESS_EHSTransaction) = udtEHSTransaction

            If Not udtSessionHandlerBLL.ClaimCOVID19ValidReprintGetFromSession(FunctionCode) Then
                ibtnReprintRecord.Enabled = False
                ibtnReprintRecord.ImageUrl = GetGlobalResourceObject("ImageUrl", "ReprintDisableBtn")

                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00050)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                ibtnReprintRecord.Enabled = True
                ibtnReprintRecord.ImageUrl = GetGlobalResourceObject("ImageUrl", "ReprintBtn")
            End If

        Else
            Session(SESS_EHSTransaction) = Nothing

        End If
    End Sub

    Private Sub InitializeDataValue()
        Me.ddleHSDocType.Items.Clear()
        BindDocumentType(Me.ddleHSDocType)
        Me.ddleHSDocType.Enabled = True
        Me.txteHSDocNo.Text = String.Empty
    End Sub


#End Region

#Region "Event Handler"
    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Doc Code", Me.ddleHSDocType.SelectedValue)
        udtAuditLogEntry.AddDescripton("Doc No", Me.txteHSDocNo.Text.Trim)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search Start")

        SearchCOVID19Transaction()

    End Sub

    Protected Sub ibtnSearchByCard_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Try
            udtAuditLogEntry.WriteLog(LogID.LOG00004, "Click Read Card and Search")

            Me.RedirectToIdeasCombo(IdeasBLL.EnumIdeasVersion.Combo)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Click Read Card and Search Complete")
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Click Read Card and Search Fail")
        End Try
    End Sub

    Protected Sub SearchCOVID19Transaction()
        Dim udtSearchCriteria = New SearchCriteria
        Dim dt As New DataTable
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing
        Dim strTransNum As String
        Dim strDocCode As String = String.Empty
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        imgeHSDocNoErr.Visible = False
        udcErrorMessage.Clear()
        udcInfoMessageBox.Clear()

        udtSessionHandlerBLL.ClaimCOVID19ValidReprintRemoveFromSession(FunctionCode)
        udtSessionHandlerBLL.ClaimCOVID19VaccinationCardRemoveFromSession(FunctionCode)

        udtSessionHandlerBLL.CMSVaccineResultRemoveFromSession(FunctionCode)
        udtSessionHandlerBLL.CIMSVaccineResultRemoveFromSession(FunctionCode)

        Try
            If Trim(Me.txteHSDocNo.Text) = String.Empty Then
                udcErrorMessage.AddMessage("990000", "E", "00211")
                imgeHSDocNoErr.Visible = True

            Else
                udtSearchCriteria.DocumentType = Me.ddleHSDocType.SelectedValue
                Dim aryDocumentNo As String() = Me.txteHSDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "").Split("/")
                If aryDocumentNo.Length > 1 Then
                    udtSearchCriteria.DocumentNo1 = aryDocumentNo(1)
                    udtSearchCriteria.DocumentNo2 = aryDocumentNo(0)
                Else
                    udtSearchCriteria.DocumentNo1 = aryDocumentNo(0)
                    udtSearchCriteria.DocumentNo2 = String.Empty
                End If

                'Search COVID19 transaction
                dt = udtCOVID19BLL.GetLatestCovid19TransactionByDocId(udtSearchCriteria)

                If dt.Rows.Count > 0 Then
                    strTransNum = dt.Rows(0).Item("Transaction_ID")

                    'Build EHS transaction Detail
                    BuildClaimTransDetail(strTransNum)

                    strDocCode = dt.Rows(0).Item("Doc_Code")
                    If Me.ddleHSDocType.SelectedValue <> Trim(strDocCode) Then
                        ' Message: Doc code not match
                        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                    End If
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search successful")
                Else
                    ' Message: No records found.
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMessageBox.AddMessage("990000", "I", "00001")
                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search and Hold fail")
                End If
            End If

            udcInfoMessageBox.BuildMessageBox()
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00003, "Search fail")

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search fail")
            Throw
        End Try

    End Sub

    Protected Sub ibtnDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, "Back Click")
        udcClaimTransDetail.ClearDocumentType()
        udcClaimTransDetail.ClearEHSClaim()
        udcClaimTransDetail.ClearVaccineRecord()

        udcInfoMessageBox.Visible = False
        udcErrorMessage.Visible = False

        MultiViewReprintVaccinationRecord.ActiveViewIndex = ViewIndex.Search
        InitializeDataValue()
    End Sub

    Protected Sub ibtnReprintRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnReprintRecord.Click

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSTransaction As EHSTransactionModel = Session(SESS_EHSTransaction)
        Dim udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL

        Try
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Reprint")

            'Find the nearest vaccination record for vaccination card
            Dim udtTransactionBenefitDetailList As TransactionDetailVaccineModelCollection = Nothing
            'Get EHS Vaccine to Benefit List
            udtTransactionBenefitDetailList = udtEHSTransactionBLL.getTransactionDetailVaccine(udtEHSTransaction.DocCode, udtEHSTransaction.EHSAcct.getPersonalInformation(udtEHSTransaction.DocCode).IdentityNum)
            If udtTransactionBenefitDetailList.Count > 0 Then
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtTransactionBenefitDetailList.FilterFindNearestRecord()

                If udtEHSTransaction.TransactionDetails(0).AvailableItemCode.Trim().ToUpper() <> udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() Then
                    If udtEHSTransaction.TransactionAdditionFields.VaccineBrand.Trim = udtVaccinationRecord.VaccineBrand.Trim.Trim().ToUpper() Then
                        udtSessionHandler.ClaimCOVID19VaccinationCardSaveToSession(udtVaccinationRecord, FunctionCode)
                    End If

                End If

            End If

            'HardCoded for print Covid19 Vaccination '
            Dim strCurrentPrintOption As String = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent

            Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
            udtEHSTransaction.PrintedConsentForm = True

            udtEHSTransaction.EHSAcct.SetSearchDocCode(udtEHSTransaction.DocCode)

            'Set the transaction is printed consent Form
            udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)
            udtSessionHandler.EHSAccountSaveToSession(udtEHSTransaction.EHSAcct, FunctionCode)
            ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
            ' --------------------------------------------------------------------------------------
            'Save the current function code to session (will be removed in the printout form)
            udtSessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctionCode)
            ' CRE20-0022 (Immu record) [End][Winnie SUEN]



            If strCurrentPrintOption = Common.Component.PrintFormOptionValue.PrintPurposeAndConsent Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/BasePrintoutForm.aspx?TID=" + strPrintDateTime + "');", True)
                udtAuditLogEntry.AddDescripton("Reprint", "Vaccination Card Printed")
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Reprint successful")
            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, "Reprint fail")
            Throw
        End Try

    End Sub

#End Region

#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer

    End Function

    Protected Overrides Sub SF_CancelSearch_Click()

    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()

    End Sub

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult

    End Function

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean

    End Function

#End Region

#Region "Read SmartIC"

    Private Sub RedirectToIdeasCombo(ByVal enumIDEASVersion As IdeasBLL.EnumIdeasVersion)
        Dim udtSmarIDContent As BLL.SmartIDContentModel = New BLL.SmartIDContentModel
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()
        Dim ideasTokenResponse As IdeasRM.TokenResponse = Nothing
        Dim isDemoVersion As String = ConfigurationManager.AppSettings("SmartIDDemoVersion")
        Dim strLang As String = "en_US"
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        ' Remove Card Setting Read From SystemParameters
        Dim strRemoveCard As String = String.Empty
        Me._udtGeneralFunction.getSystemParameter("SmartID_RemoveCard", strRemoveCard, String.Empty)
        If strRemoveCard = String.Empty Then
            strRemoveCard = "Y"
        End If

        Dim eIdeasVersion As IdeasBLL.EnumIdeasVersion = IdeasBLL.GetIdeasVersion(enumIDEASVersion)
        Dim strIdeasVersion As String = IdeasBLL.ConvertIdeasVersion(eIdeasVersion)
        udtSmarIDContent.IdeasVersion = eIdeasVersion

        AuditLogReadSamrtID(udtAuditLogEntry, Nothing, strIdeasVersion, Nothing)

        ' Enforce HCSP accept server cert for connecting IDEAS Testing server
        Net.ServicePointManager.ServerCertificateValidationCallback = New Net.Security.RemoteCertificateValidationCallback(AddressOf (New IdeasBLL).ValidateCertificate)

        ' Get Token From Ideas, input: the return URL from Ideas to eHS
        Select Case eIdeasVersion
            Case IdeasBLL.EnumIdeasVersion.One, IdeasBLL.EnumIdeasVersion.Two, IdeasBLL.EnumIdeasVersion.TwoGender
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, Me.Page.Request.Url.GetLeftPart(UriPartial.Path), strLang, strRemoveCard)

            Case IdeasBLL.EnumIdeasVersion.Combo, IdeasBLL.EnumIdeasVersion.ComboGender
                Dim strPageName As String = New IO.FileInfo(Me.Request.Url.LocalPath).Name
                Dim strComboReturnURL As String = Me.Page.Request.Url.GetLeftPart(UriPartial.Path)
                Dim strFolderName As String = "/ClaimManagement"

                strComboReturnURL = strComboReturnURL.Replace(strFolderName + "/" + strPageName, "/IDEASComboReader/IDEASComboReader.aspx")
                ideasTokenResponse = IdeasBLL.GetToken(eIdeasVersion, strComboReturnURL, strLang, strRemoveCard)

        End Select

        If Not ideasTokenResponse.ErrorCode Is Nothing Then
            Me.udcErrorMessage.AddMessageDesc(FunctionCode, ideasTokenResponse.ErrorCode, ideasTokenResponse.ErrorMessage)

            If isDemoVersion.Equals("Y") Then
                AuditLogConnectIdeasFail(udtAuditLogEntry, Nothing, ideasTokenResponse, "Y", strIdeasVersion)
            Else
                AuditLogConnectIdeasFail(udtAuditLogEntry, Nothing, ideasTokenResponse, "N", strIdeasVersion)
            End If

            Me.udcErrorMessage.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry, Common.Component.LogID.LOG00049, "Click 'Read and Search Card' and Token Request Fail")

        Else
            udtSmarIDContent.IsReadSmartID = True
            udtSmarIDContent.TokenResponse = ideasTokenResponse

            If isDemoVersion.Equals("Y") Then
                udtSmarIDContent.IsDemonVersion = True

                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                AuditLogConnectIdeasComboComplete(udtAuditLogEntry, Nothing, ideasTokenResponse, "Y", strIdeasVersion)

                ReadDemoSmartID(udtSmarIDContent)

            Else
                udtSmarIDContent.IsDemonVersion = False

                Me._udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmarIDContent)

                AuditLogConnectIdeasComboComplete(udtAuditLogEntry, Nothing, ideasTokenResponse, "N", strIdeasVersion)

                ' Prompt the popup include iframe to show IDEAS Combo UI
                ucIDEASCombo.ReadSmartIC(IdeasBLL.EnumIdeasVersion.Combo, ideasTokenResponse, FunctionCode)

            End If
        End If
    End Sub

    'Page Load : LOG00001
    Public Shared Sub AuditLogPageLoad(ByVal udtAuditLogEntry As AuditLogEntry, ByVal selectedPractice As Boolean, ByVal comeFromAccountCreation As Boolean)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00001, "Reprint Vaccination Record Page Loaded")
    End Sub

    'Search Account : Read Samrt ID: LOG00047
    Public Shared Sub AuditLogReadSamrtID(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strIdeasVersion As String, ByVal blnIsNewSmartIC As Nullable(Of Boolean))
        Dim strNewSmartIC As String = String.Empty

        If Not blnIsNewSmartIC Is Nothing Then
            strNewSmartIC = IIf(blnIsNewSmartIC, YesNo.Yes, YesNo.No)
        End If

        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("New Card", strNewSmartIC)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00047, "Click 'Read and Search Card' and Token Request")
    End Sub

    'Search Account : Connect Ideas Complete: LOG00048
    Public Shared Sub AuditLogConnectIdeasComboComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String, ByVal strIdeasVersion As String)
        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Ideas Broker URL", ideasTokenResponse.BrokerURL)
        udtAuditLogEntry.AddDescripton("Demo Version", strDemoVersion)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00048, "Click 'Read and Search Card' and Token Request Complete")
    End Sub

    'Search Account : Connect Ideas Fail: LOG00049
    Public Shared Sub AuditLogConnectIdeasFail(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal ideasTokenResponse As IdeasRM.TokenResponse, ByVal strDemoVersion As String, ByVal strIdeasVersion As String)
        udtAuditLogEntry.AddDescripton("Ideas Error Code", ideasTokenResponse.ErrorCode)
        udtAuditLogEntry.AddDescripton("Ideas Error Detail", ideasTokenResponse.ErrorDetail)
        udtAuditLogEntry.AddDescripton("Ideas Error Message", ideasTokenResponse.ErrorMessage)
        udtAuditLogEntry.AddDescripton("Demo Version", strDemoVersion)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)

        'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00049, "Click 'Read and Search Card' and Token Request Fail")
    End Sub

    'LOG00050
    Public Shared Sub AuditLogSearchNvaliatedACwithCFD(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strHKIC As String, ByVal strError As String, ByVal strIdeasVersion As String, ByVal strSearchFrom As String)
        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("HKIC No.", strHKIC)
        udtAuditLogEntry.AddDescripton("Error", strError)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.AddDescripton("Search From", strSearchFrom)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00050, "Search & validate account with CFD", New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocType.DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, strHKIC)))
    End Sub

    'From IDEAS  : Redirect from IDEAS Complete : LOG00051
    Public Shared Sub AuditLogSearchNvaliatedACwithCFDComplete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal udtSmartIDContent As BLL.SmartIDContentModel, ByVal blnGoToCreation As Boolean)
        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)

        If blnGoToCreation Then
            udtAuditLogEntry.AddDescripton("Go to Account Creation", "True")
        Else
            udtAuditLogEntry.AddDescripton("Go to Account Creation", "False")
        End If

        udtAuditLogEntry.AddDescripton("Smart IC Type", udtSmartIDContent.SmartIDReadStatus.ToString())
        udtAuditLogEntry.AddDescripton("IDEAS Version", IdeasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion))
        udtAuditLogEntry.AddDescripton("CFD", "->")
        AuditLogHKIC(udtAuditLogEntry, udtSmartIDContent.EHSAccount)

        If Not udtSmartIDContent.EHSValidatedAccount Is Nothing Then
            udtAuditLogEntry.AddDescripton("Validated EHS Account", "->")
            AuditLogHKIC(udtAuditLogEntry, udtSmartIDContent.EHSValidatedAccount)
        End If

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00051, "Search & validate account with CFD Complete")
    End Sub

    'From IDEAS  : Redirect from IDEAS Fail : LOG00052
    Public Shared Sub AuditLogSearchNvaliatedACwithCFDFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strStepLocation As String, ByVal strErrorCode As String, ByVal strErrorMessage As String)
        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("Steps Location", strStepLocation)
        udtAuditLogEntry.AddDescripton("Error Code", strErrorCode)
        udtAuditLogEntry.AddDescripton("Error Message", strErrorMessage)

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail")
    End Sub


    'Get CFD : Start : 00061
    Public Shared Sub AuditLogGetCFD(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00061, "Get CFD")
    End Sub

    'Get CFD Complete: 00062
    Public Shared Sub AuditLogGetCFDComplete(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00062, "Get CFD Complete")
    End Sub

    'Get CFD Fail: 00063
    Public Shared Sub AuditLogGetCFDFail(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strArtifact As String, ByVal strErrorCode As String, ByVal strErrorMsg As String, ByVal strIdeasVersion As String)
        udtAuditLogEntry.AddDescripton("Artifact", strArtifact)
        udtAuditLogEntry.AddDescripton("Error Code", strErrorCode)
        udtAuditLogEntry.AddDescripton("Error Message", strErrorMsg)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)

        'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00063, "Get CFD Fail")
    End Sub

    'From IDEAS  : Redirect from IDEAS : LOG00064
    Public Shared Sub AuditLogRedirectFormIDEAS(ByVal udtAuditLogEntry As AuditLogEntry, ByVal strSchemeCode As String, ByVal strIdeasVersion As String)
        If Not IsNothing(strSchemeCode) Then udtAuditLogEntry.AddDescripton("Scheme Code", strSchemeCode)
        udtAuditLogEntry.AddDescripton("IDEAS Version", strIdeasVersion)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00064, "Redirect from IDEAS after Token Request")
    End Sub

    '==================================================================== Code for SmartID ============================================================================
    Private Function ReadSmartID(ByVal udtSmartIDContent As BLL.SmartIDContentModel) As Boolean
        If IsNothing(udtSmartIDContent) OrElse udtSmartIDContent.IsReadSmartID = False OrElse udtSmartIDContent.IsEndOfReadSmartID Then Return False

        Dim isReadingSmartID As Boolean = False
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim ideasBLL As BLL.IdeasBLL = New BLL.IdeasBLL
        Dim strIdeasVersion As String = ideasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)

        'Write Start Audit log
        AuditLogRedirectFormIDEAS(udtAuditLogEntry, Nothing, strIdeasVersion)

        isReadingSmartID = True
        udtSmartIDContent.IsEndOfReadSmartID = True
        Me._udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)
        udtSmartIDContent = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Smart ID Form Ideas
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Get CFD
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry_GetCFD As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
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

        AuditLogGetCFD(udtAuditLogEntry_GetCFD, strArtifact)

        Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel
        Dim isValid As Boolean = True

        If strArtifact Is Nothing OrElse strArtifact = String.Empty Then
            '----------------------------- Error Handling -----------------------------------------------

            ' Error100 - 113
            If Not Request.QueryString("status") Is Nothing Then
                Dim strErrorCode As String = Request.QueryString("status").Trim()
                Dim strErrorMsg As String = IdeasRM.ErrorMessageMapper.MapMAStatus(strErrorCode)
                If Not strErrorMsg Is Nothing Then

                    'Me.Clear()
                    'Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                    'Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                    Me.udcErrorMessage.AddMessageDesc(FunctionCode, strErrorCode, strErrorMsg)

                    'Write End Audit log
                    AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, ideasBLL.Artifact, strErrorCode, strErrorMsg, strIdeasVersion)

                    Me.udcErrorMessage.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, Common.Component.LogID.LOG00063, "Get CFD Fail")

                    isValid = False
                End If
            End If
        End If

        If isValid Then

            If ideasSamlResponse.StatusCode.Equals("samlp:Success") Then
                AuditLogGetCFDComplete(udtAuditLogEntry_GetCFD, ideasBLL.Artifact)

                '[Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
                Dim udtEHSAccountExist As EHSAccountModel = Nothing
                Dim blnNotMatchAccountExist As Boolean = False
                Dim blnExceedDocTypeLimit As Boolean = False
                Dim udtEligibleResult As EligibleResult = Nothing
                Dim goToCreation As Boolean = True
                Dim strError As String = String.Empty

                Try
                    If udtSmartIDContent.IsDemonVersion Then
                        udtSmartIDContent.EHSAccount = SmartIDDummyCase.GetDummyEHSAccount(String.Empty, udtSmartIDContent.IdeasVersion)
                        udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).CName = VoucherAccountBLL.GetCName(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0))

                    Else
                        Dim udtCFD As IdeasRM.CardFaceData
                        udtCFD = ideasSamlResponse.CardFaceDate()
                        If IsNothing(udtCFD) Then
                            strError = "ideasSamlResponse.CardFaceDate() is nothing"
                        End If

                        udtSmartIDContent.EHSAccount = ideasBLL.GetCardFaceDataEHSAccount(udtCFD, String.Empty, FunctionCode, udtSmartIDContent)

                    End If
                Catch ex As Exception
                    udtSmartIDContent.EHSAccount = Nothing
                    strError = ex.Message
                End Try

                Dim udtAuditlogEntry_Search As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                Dim strHKICNo As String = String.Empty

                If Not udtSmartIDContent.EHSAccount Is Nothing Then
                    strHKICNo = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum.Trim
                End If

                AuditLogSearchNvaliatedACwithCFD(udtAuditlogEntry_Search, Nothing, strHKICNo, strError, strIdeasVersion, "Claim")

                If Not udtSmartIDContent.EHSAccount Is Nothing Then

                    udtPersonalInfoSmartID = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)

                    '------------------------------------------------------------------------------------------------------
                    'Card Face Data Validation
                    '------------------------------------------------------------------------------------------------------
                    Dim udtSystemMessage As SystemMessage = SmartIDCardFaceDataValidation(udtPersonalInfoSmartID)
                    If Not udtSystemMessage Is Nothing Then
                        isValid = False
                        If Not udtPersonalInfoSmartID.IdentityNum Is Nothing Then udtAuditlogEntry_Search.AddDescripton("HKID", udtPersonalInfoSmartID.IdentityNum)
                        If udtPersonalInfoSmartID.DateofIssue.HasValue Then udtAuditlogEntry_Search.AddDescripton("DOI", udtPersonalInfoSmartID.DateofIssue)
                        udtAuditlogEntry_Search.AddDescripton("DOB", udtPersonalInfoSmartID.DOB)

                        Me.udcErrorMessage.AddMessage(udtSystemMessage)
                    End If

                    SearchSmartID(udtSmartIDContent, isValid, udtAuditlogEntry_Search)

                Else
                    '---------------------------------------------------------------------------------------------------------------
                    ' udtSmartIDContent.EHSAccount is nothing, crad face data may not be able to return 
                    '---------------------------------------------------------------------------------------------------------------
                    'Me.Clear()
                    'Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                    'Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                    Me.udcErrorMessage.AddMessage(New SystemMessage("990000", "E", "00253"))
                    isValid = False
                End If

                Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditlogEntry_Search, Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail", _
                    New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strHKICNo)))
            Else
                '---------------------------------------------------------------------------------------------------------------
                ' ideasSamlResponse.StatusCode is not "samlp:Success"
                '---------------------------------------------------------------------------------------------------------------
                'Me.Clear()
                'Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                'Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                Me.udcErrorMessage.AddMessageDesc(FunctionCode, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail)

                'Write End Audit log
                AuditLogGetCFDFail(udtAuditLogEntry_GetCFD, ideasBLL.Artifact, ideasSamlResponse.StatusMessage, ideasSamlResponse.StatusDetail, strIdeasVersion)

                Me.udcErrorMessage.BuildMessageDescBox("SmartIDActionFail", udtAuditLogEntry_GetCFD, Common.Component.LogID.LOG00063, "Get CFD Fail")

                isValid = False
            End If
        End If

        Return isReadingSmartID
    End Function

    Public Shared Function SmartIDCardFaceDataValidation(ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel) As SystemMessage
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        Dim validator As Common.Validation.Validator = New Common.Validation.Validator
        Dim isValid As Boolean = True
        Dim udtCommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim strSmartIDIssue As String = String.Empty
        '---------------------------------------------------------------------------------------------------------------
        ' Check Identity No
        '---------------------------------------------------------------------------------------------------------------
        udtSystemMessage = validator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udtEHSPersonalInformation.IdentityNum, String.Empty)
        If Not udtSystemMessage Is Nothing Then
            isValid = False
            udtSystemMessage = New SystemMessage("990000", "E", "00244")
        End If

        '---------------------------------------------------------------------------------------------------------------
        ' Check Date of Issue
        '---------------------------------------------------------------------------------------------------------------
        If isValid Then

            udtSystemMessage = validator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.HKIC, formatter.formatDOI(DocType.DocTypeModel.DocTypeCode.HKIC, udtEHSPersonalInformation.DateofIssue), udtEHSPersonalInformation.DOB)
            If Not udtSystemMessage Is Nothing Then
                isValid = False
                udtSystemMessage = New SystemMessage("990000", "E", "00245")
            End If
        End If

        If isValid Then
            udtCommfunct.getSystemParameter("SmartIDIssueDate", strSmartIDIssue, String.Empty, "ALL")
            If udtEHSPersonalInformation.DateofIssue < CDate(strSmartIDIssue) Then
                isValid = False
                udtSystemMessage = New SystemMessage("990000", "E", "00245")
            End If
        End If

        '---------------------------------------------------------------------------------------------------------------
        ' Check Date of Brith
        '---------------------------------------------------------------------------------------------------------------
        If isValid Then

            Dim strDOB As String = formatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, CultureLanguage.English, Nothing, Nothing)
            udtSystemMessage = validator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, strDOB)
            If Not udtSystemMessage Is Nothing Then
                isValid = False
                udtSystemMessage = New SystemMessage("990000", "E", "00246")
            End If
        End If

        Return udtSystemMessage
    End Function

    Private Sub SearchSmartID(ByVal udtSmartIDContent As SmartIDContentModel, ByVal isValid As Boolean, ByVal udtAuditlogEntry As AuditLogEntry)
        Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel
        Dim udtEHSAccountExist As EHSAccountModel = Nothing
        Dim blnNotMatchAccountExist As Boolean = False
        Dim blnExceedDocTypeLimit As Boolean = False
        Dim udtEligibleResult As EligibleResult = Nothing
        Dim goToCreation As Boolean = True
        Dim strError As String = String.Empty

        udtPersonalInfoSmartID = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)

        ddleHSDocType.SelectedValue = DocTypeModel.DocTypeCode.HKIC
        txteHSDocNo.Text = udtPersonalInfoSmartID.IdentityNum

        SearchCOVID19Transaction()

        ''If isValid Then

        ''    ' ----------------------------------------------
        ''    ' 1. Search account in EHS 
        ''    ' ----------------------------------------------
        ''    Me._udtSystemMessage = Me._udtEHSClaimBLL.SearchEHSAccountSmartID(udtSchemeClaim.SchemeCode.Trim(), DocTypeModel.DocTypeCode.HKIC, udtPersonalInfoSmartID.IdentityNum, _
        ''                    Me._udtFormatter.formatDOB(udtPersonalInfoSmartID.DOB, udtPersonalInfoSmartID.ExactDOB, Common.Component.CultureLanguage.English, Nothing, Nothing), _
        ''                    udtEHSAccountExist, udtSmartIDContent.EHSAccount, udtSmartIDContent.SmartIDReadStatus, udtEligibleResult, blnNotMatchAccountExist, blnExceedDocTypeLimit, _
        ''                    FunctionCode, True)

        ''    ' ----------------------------------------------
        ''    ' 2. Call OCSSS to check HKIC if input is shown
        ''    ' ----------------------------------------------
        ''    If Me._udtSystemMessage Is Nothing Then
        ''        ' HKIC must be formated in 9 characters e.g. " A1234567" or "CD1234567"

        ''        If Me._udtSessionHandler.UIDisplayHKICSymbolGetFormSession(FunctCode) Then

        ''            ' INT18-XXX (Refine auditlog) [Start][Chris YIM]
        ''            ' --------------------------------------------------------------------------------------
        ''            'Log Enter Info
        ''            EHSClaimBasePage.AuditLogSearchOCSSSStart(New AuditLogEntry(FunctionCode, Me), DocTypeModel.DocTypeCode.HKIC, _
        ''                                                       _udtSessionHandler.HKICSymbolGetFormSession(FunctCode), udtPersonalInfoSmartID.IdentityNum)

        ''            CheckHKIDByOCSSS(udtPersonalInfoSmartID.IdentityNum, _udtSP.SPID, udtSchemeClaim.SchemeCode.Trim())

        ''            If Me._udtSystemMessage Is Nothing Then
        ''                EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), True)
        ''            Else
        ''                EHSClaimBasePage.AuditLogSearchOCSSSEnd(New AuditLogEntry(FunctionCode, Me), False)
        ''            End If

        ''            ' INT18-XXX (Refine auditlog) [End][Chris YIM]
        ''        Else
        ''            Me._udtSessionHandler.HKICSymbolRemoveFromSession(FunctCode)
        ''        End If
        ''    End If

        ''    ' ----------------------------------------------
        ''    ' Search Account Error Issue
        ''    ' ----------------------------------------------
        ''    If Not Me._udtSystemMessage Is Nothing Then
        ''        isValid = False

        ''        Select Case Me._udtSystemMessage.MessageCode
        ''            Case "00141", "00142"
        ''                Me.udcClaimSearch.SetHKICNoError(True)
        ''        End Select

        ''        'Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
        ''    Else
        ''        'Validation Success

        ''        'Store residential status in model
        ''        ' INT18-0018 (Fix read smart IC with HKBC account) [Start][Koala]
        ''        If udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC) IsNot Nothing Then
        ''            udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
        ''        End If
        ''        ' INT18-0018 (Fix read smart IC with HKBC account) [End][Koala]
        ''    End If
        ''    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ''End If

        ''If isValid Then
        ''    Dim strRuleResultKey As String = String.Empty

        ''    If udtEligibleResult Is Nothing Then
        ''        Me._udtSessionHandler.EligibleResultRemoveFromSession()
        ''    Else
        ''        Dim udtRuleResults As RuleResultCollection = New RuleResultCollection()

        ''        udtEligibleResult.PromptConfirmed = True
        ''        'Key = 1_G0002 -> not need prompt confirm popup dox -> reminder in step2a
        ''        strRuleResultKey = Me.RuleResultKey(ActiveViewIndex.Step1, udtEligibleResult.RuleType)

        ''        udtRuleResults.Add(strRuleResultKey, udtEligibleResult)
        ''        Me._udtSessionHandler.EligibleResultSaveToSession(udtRuleResults)
        ''    End If

        ''    udtEHSAccountExist.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)

        ''    'Only one case go to Claim directly -> Account validated && Search DocCode = PersonalInfo DocCode 
        ''    'udtSmartIDContent.SmartIDReadStatus = BLL.SmartIDHandler.CheckSmartIDCardStatus(udtSmartIDContent.EHSAccount, udtEHSAccountExist)
        ''    Me._udtSessionHandler.SmartIDContentSaveToSession(FunctCode, udtSmartIDContent)
        ''    Me._udtSessionHandler.NotMatchAccountExistSaveToSession(blnNotMatchAccountExist)
        ''    Me._udtSessionHandler.ExceedDocTypeLimitSaveToSession(blnExceedDocTypeLimit)
        ''    Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccountExist, FunctCode)

        ''    Select Case udtSmartIDContent.SmartIDReadStatus
        ''        Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_SameDetail
        ''            goToCreation = False

        ''        Case BLL.SmartIDHandler.SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB_NoCCCode

        ''            goToCreation = False
        ''            udtPersonalInfoSmartID.VoucherAccID = udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).VoucherAccID
        ''            udtPersonalInfoSmartID.Gender = udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).Gender
        ''            udtPersonalInfoSmartID.TSMP = udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).TSMP

        ''            Try
        ''                Me._udtEHSAccountBll.UpdateEHSAccountNameBySmartIC(udtPersonalInfoSmartID, Me._udtSP.SPID)
        ''            Catch eSQL As SqlClient.SqlException
        ''                If eSQL.Number = 50000 Then
        ''                    Me._udtSystemMessage = New Common.ComObject.SystemMessage("990001", Common.Component.SeverityCode.SEVD, eSQL.Message)
        ''                    Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
        ''                Else
        ''                    Throw eSQL
        ''                End If
        ''            End Try

        ''            If Me._udtSystemMessage Is Nothing Then
        ''                'udtEHSAccountExist = Me._udtEHSAccountBll.LoadEHSAccountByVRID(udtPersonalInfoSmartID.VoucherAccID)
        ''                'udtEHSAccountExist.SetSearchDocCode(udtEHSAccountExist.EHSPersonalInformationList(0).DocCode)
        ''                udtEHSAccountExist = Me._udtEHSAccountBll.LoadEHSAccountByIdentity(udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum, DocTypeModel.DocTypeCode.HKIC)
        ''                udtEHSAccountExist.SetSearchDocCode(DocTypeModel.DocTypeCode.HKIC)
        ''                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ''                ' ----------------------------------------------------------
        ''                udtEHSAccountExist.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = _udtSessionHandler.HKICSymbolGetFormSession(FunctCode)
        ''                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ''                Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccountExist, FunctCode)
        ''            End If

        ''    End Select
        ''Else
        ''    goToCreation = False
        ''End If

        ''If goToCreation Then

        ''    EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFDComplete(udtAuditlogEntry, udtSchemeClaim.SchemeCode, udtSmartIDContent, True)
        ''    Me._udtSessionHandler.AccountCreationComeFromClaimSaveToSession(True)

        ''    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        ''    RedirectHandler.ToURL("EHSAccountCreationV1.aspx")

        ''    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        ''Else
        ''    If Not Me._udtSystemMessage Is Nothing Then
        ''        '---------------------------------------------------------------------------------------------------------------
        ''        ' Block Case 
        ''        '---------------------------------------------------------------------------------------------------------------
        ''        If Not CheckFromVaccinationRecordEnquiry() Then
        ''            Me.Clear()
        ''            Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
        ''        End If

        ''        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
        ''        Me.udcMsgBoxErr.AddMessage(Me._udtSystemMessage)
        ''        isValid = False

        ''    Else
        ''        ' To Handle Concurrent Browser:
        ''        EHSClaimBasePage.AuditLogSearchNvaliatedACwithCFDComplete(udtAuditlogEntry, udtSchemeClaim.SchemeCode, udtSmartIDContent, False)
        ''        Me.EHSClaimTokenNumAssign()
        ''        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ''        ' ----------------------------------------------------------------------------------------                
        ''        Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step2a
        ''        EHSClaimBasePage.AuditLogEnterClaimDetailLoaded(New AuditLogEntry(FunctionCode, Me))
        ''        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        ''    End If

        ''End If
    End Sub

#End Region

#Region "Read SmartIC (Demo Version)"
    Private Function ReadDemoSmartID(ByVal udtSmartIDContent As BLL.SmartIDContentModel) As Boolean
        If IsNothing(udtSmartIDContent) OrElse udtSmartIDContent.IsReadSmartID = False OrElse udtSmartIDContent.IsEndOfReadSmartID Then Return False
        Dim isReadingSmartID As Boolean = False
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim ideasBLL As BLL.IdeasBLL = New BLL.IdeasBLL
        Dim strIdeasVersion As String = ideasBLL.ConvertIdeasVersion(udtSmartIDContent.IdeasVersion)

        isReadingSmartID = True
        udtSmartIDContent.IsEndOfReadSmartID = True
        Me._udtSessionHandler.SmartIDContentSaveToSession(FunctionCode, udtSmartIDContent)
        udtSmartIDContent = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Smart ID Form Ideas
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

        '--------------------------------------------------------------------------------------------------------------------------------------------------
        ' Get CFD
        '--------------------------------------------------------------------------------------------------------------------------------------------------
        Dim udtAuditLogEntry_GetCFD As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim strArtifact As String = String.Empty

        If udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.Combo Or _
            udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.ComboGender Then

            strArtifact = udtSmartIDContent.Artifact
        Else
            strArtifact = ideasBLL.Artifact

        End If

        AuditLogGetCFD(udtAuditLogEntry_GetCFD, strArtifact)

        Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel
        Dim isValid As Boolean = True

        If isValid Then

            AuditLogGetCFDComplete(udtAuditLogEntry_GetCFD, ideasBLL.Artifact)

            Dim udtEHSAccountExist As EHSAccountModel = Nothing
            Dim blnNotMatchAccountExist As Boolean = False
            Dim blnExceedDocTypeLimit As Boolean = False
            Dim udtEligibleResult As EligibleResult = Nothing
            Dim goToCreation As Boolean = True
            Dim strError As String = String.Empty

            Try
                ' dummy account for smart id
                ' ----------------------------------------------------------------------------------------
                udtSmartIDContent.EHSAccount = SmartIDDummyCase.GetDummyEHSAccount(String.Empty, udtSmartIDContent.IdeasVersion)
                udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).CName = VoucherAccountBLL.GetCName(udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0))
                'udtSmartIDContent.EHSAccount.EHSPersonalInformationList(0).DateofIssue = Convert.ToDateTime("2016-07-28")

            Catch ex As Exception
                udtSmartIDContent.EHSAccount = Nothing
                strError = ex.Message
            End Try

            Dim udtAuditlogEntry_Search As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            Dim strHKICNo As String = String.Empty

            If Not udtSmartIDContent.EHSAccount Is Nothing Then
                strHKICNo = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).IdentityNum.Trim
            End If

            AuditLogSearchNvaliatedACwithCFD(udtAuditlogEntry_Search, Nothing, strHKICNo, strError, strIdeasVersion, "Claim")

            If Not udtSmartIDContent.EHSAccount Is Nothing Then

                udtPersonalInfoSmartID = udtSmartIDContent.EHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)

                '------------------------------------------------------------------------------------------------------
                'Card Face Data Validation
                '------------------------------------------------------------------------------------------------------
                Dim udtSystemMessage As SystemMessage = SmartIDCardFaceDataValidation(udtPersonalInfoSmartID)
                If Not udtSystemMessage Is Nothing Then
                    isValid = False
                    If Not udtPersonalInfoSmartID.IdentityNum Is Nothing Then udtAuditlogEntry_Search.AddDescripton("HKID", udtPersonalInfoSmartID.IdentityNum)
                    If udtPersonalInfoSmartID.DateofIssue.HasValue Then udtAuditlogEntry_Search.AddDescripton("DOI", udtPersonalInfoSmartID.DateofIssue)
                    udtAuditlogEntry_Search.AddDescripton("DOB", udtPersonalInfoSmartID.DOB)

                    Me.udcErrorMessage.AddMessage(udtSystemMessage)
                End If

                SearchSmartID(udtSmartIDContent, isValid, udtAuditlogEntry_Search)

            Else
                '---------------------------------------------------------------------------------------------------------------
                ' udtSmartIDContent.EHSAccount is nothing, crad face data may not be able to return 
                '---------------------------------------------------------------------------------------------------------------
                'Me.Clear()
                'Me.udcStep1DocumentTypeRadioButtonGroup.SelectedValue = DocTypeModel.DocTypeCode.HKIC
                'Me.mvEHSClaim.ActiveViewIndex = ActiveViewIndex.Step1
                Me.udcErrorMessage.AddMessage(New SystemMessage("990000", "E", "00253"))
                isValid = False
            End If

            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditlogEntry_Search, Common.Component.LogID.LOG00052, "Search & validate account with CFD Fail", _
                New AuditLogInfo(Nothing, Nothing, Nothing, Nothing, DocTypeModel.DocTypeCode.HKIC, (New Formatter).formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, strHKICNo)))

        End If

        Return isReadingSmartID
    End Function

#End Region

#Region "Doc Type Log"
    'Hong Kong Identity Card
    Public Shared Function AuditLogHKIC(ByVal udtAuditLogEntry As AuditLogEntry, ByVal udtEHSAccount As EHSAccount.EHSAccountModel) As AuditLogEntry

        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC)

        udtAuditLogEntry.AddDescripton("Doc Type", DocType.DocTypeModel.DocTypeCode.HKIC)
        udtAuditLogEntry.AddDescripton("HKID No.", udtEHSPersonalInfo.IdentityNum)
        udtAuditLogEntry.AddDescripton("DOB", udtEHSPersonalInfo.DOB)
        udtAuditLogEntry.AddDescripton("ExactDOB", udtEHSPersonalInfo.ExactDOB)
        udtAuditLogEntry.AddDescripton("ENameSurName", udtEHSPersonalInfo.ENameSurName)
        udtAuditLogEntry.AddDescripton("ENameFirstName", udtEHSPersonalInfo.ENameFirstName)

        If Not udtEHSPersonalInfo.CName Is Nothing Then
            udtAuditLogEntry.AddDescripton("CName", udtEHSPersonalInfo.CName)
        Else
            udtAuditLogEntry.AddDescripton("CName", String.Empty)
        End If

        If Not udtEHSPersonalInfo.CCCode1 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode1", udtEHSPersonalInfo.CCCode1)
        Else
            udtAuditLogEntry.AddDescripton("CCCode1", String.Empty)
        End If

        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode2", udtEHSPersonalInfo.CCCode2)
        Else
            udtAuditLogEntry.AddDescripton("CCCode2", String.Empty)
        End If

        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode3", udtEHSPersonalInfo.CCCode3)
        Else
            udtAuditLogEntry.AddDescripton("CCCode3", String.Empty)
        End If

        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode4", udtEHSPersonalInfo.CCCode4)
        Else
            udtAuditLogEntry.AddDescripton("CCCode4", String.Empty)
        End If

        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode5", udtEHSPersonalInfo.CCCode5)
        Else
            udtAuditLogEntry.AddDescripton("CCCode5", String.Empty)
        End If

        If Not udtEHSPersonalInfo.CCCode2 Is Nothing Then
            udtAuditLogEntry.AddDescripton("CCCode6", udtEHSPersonalInfo.CCCode6)
        Else
            udtAuditLogEntry.AddDescripton("CCCode6", String.Empty)
        End If

        udtAuditLogEntry.AddDescripton("Date of Issue", udtEHSPersonalInfo.DateofIssue)
        udtAuditLogEntry.AddDescripton("Gender", udtEHSPersonalInfo.Gender)

        Return udtAuditLogEntry

    End Function

#End Region

End Class