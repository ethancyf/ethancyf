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
Imports Common.Component.COVID19
Imports System.ComponentModel



Partial Public Class ReprintVaccinationRecord
    Inherits BasePageWithControl

    Private _udtGeneralFunction As New GeneralFunction
    Private _udtSessionHandler As New SessionHandlerBLL

#Region "Private Classes"
    Private Class ViewIndex
        Public Const Search As Integer = 0
        Public Const SearchResultMEC As Integer = 1
        Public Const Detail As Integer = 2
    End Class

    Private Class AuditLogDescription
        Public Const LOG00000 As String = "Reprint Vaccination Record Page Loaded"
        Public Const LOG00001 As String = "Search Button Click"
        Public Const LOG00002 As String = "Search Start"
        Public Const LOG00003 As String = "Search successful"
        Public Const LOG00004 As String = "Search fail"
        Public Const LOG00005 As String = "Click Read Card and Search"
        Public Const LOG00006 As String = "Click Read Card and Search Complete"
        Public Const LOG00007 As String = "Click Read Card and Search Fail"
        Public Const LOG00008 As String = "Reprint Button Click"
        Public Const LOG00009 As String = "Reprint Vaccination Record successful"        
        Public Const LOG00010 As String = "Reprint fail"
        Public Const LOG00011 As String = "Detail - Back Btton Click"
        Public Const LOG00012 As String = "Load Transaction"
        Public Const LOG00013 As String = "Load Transaction end"
        Public Const LOG00014 As String = "Load Transaction fail"
        Public Const LOG00015 As String = "Search Result - Back Button Click"
        Public Const LOG00016 As String = "Search Result - Link Button Click"
        Public Const LOG00017 As String = "Reprint Medical Exemption Record successful"
        Public Const LOG00018 As String = "Reprint Medical Exemption Record (Expired) successful"

    End Class

    Public Enum RecordTypeEnumClass
        <Description("")> NA
        <Description("VR")> VaccinationRecord
        <Description("ME")> MedicalExemption
    End Enum

    Public Class SESS
        Public Const EHSTransaction As String = "010421_EHSTransaction"
        Public Const SearchCriteria As String = "010421_SearchCriteria"
        Public Const SearchResultTable As String = "010421_SearchResultTable"
        Public Const SelectedRecordType As String = "010421_SelectedRecordType"
    End Class

    Private Class VS    
        Public Const SelectedDocumentType As String = "SelectedDocumentType"
        Public Const PrevActiveViewIndex As String = "PrevActiveViewIndex"
    End Class
#End Region


#Region "Fields"
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtDocTypeBLL As New DocTypeBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtSessionHandlerBLL As New SessionHandlerBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
    Private udtFormatter As New Formatter
    Private udtCOVID19BLL As New COVID19BLL
#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'CRE20-023 add session on the reprint form [Start][Nichole]
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
        'CRE20-023 add session on the reprint form [End][Nichole]

        If Not IsPostBack Then
            ' Set function code
            FunctionCode = FunctCode.FUNT010421

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            AuditLogUserType(udtAuditLogEntry)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.LOG00000)
            InitializeDataValue()

            '==================================================================== Code for SmartID ============================================================================
            Dim udtSmartIDContent As SmartIDContentModel = _udtSessionHandler.SmartIDContentGetFormSession(FunctionCode)

            Me.ReadSmartID(udtSmartIDContent)

            '==================================================================================================================================================================
        Else
            If MultiViewReprintVaccinationRecord.ActiveViewIndex = ViewIndex.Detail Then
                ' Rebind the details
                Dim udtEHSTransaction As EHSTransactionModel = Session(SESS.EHSTransaction)
                BuildClaimTransDetail(udtEHSTransaction)
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
        Dim udtCOVID19BLL As New COVID19.COVID19BLL

        ddlEHealthDocType.Items.Clear()
        ddlEHealthDocType.DataSource = udtCOVID19BLL.GenerateC19DocumentTypeList()
        ddlEHealthDocType.DataTextField = "DocName"
        ddlEHealthDocType.DataValueField = "DocCode"
        ddlEHealthDocType.DataBind()
    End Sub


    Private Sub BuildClaimTransDetail(ByVal udtEHSTransaction As EHSTransactionModel)

        ChangeView(ViewIndex.Detail)

        ibtnReprintRecord.Enabled = True
        ibtnReprintRecord.ImageUrl = GetGlobalResourceObject("ImageUrl", "ReprintBtn")

        Dim strSelectedRecordType As String = Session(SESS.SelectedRecordType)
        Session(SESS.EHSTransaction) = Nothing
        If udtEHSTransaction Is Nothing Then Return

        ' Load Claim Detail
        udcClaimTransDetail.ClearDocumentType()
        udcClaimTransDetail.ClearEHSClaim()
        udcClaimTransDetail.ClearVaccineRecord()

        udcClaimTransDetail.ShowHKICSymbol = True
        udcClaimTransDetail.ShowAccountIDAsBtn = False
        udcClaimTransDetail.FunctionCode = FunctionCode

        Dim udtSearchCriteria As New SearchCriteria
        udtSearchCriteria.TransNum = udtEHSTransaction.TransactionID

        udcClaimTransDetail.LoadTranInfo(udtEHSTransaction, udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria))

        Session(SESS.EHSTransaction) = udtEHSTransaction


        If strSelectedRecordType = Formatter.EnumToString(RecordTypeEnumClass.VaccinationRecord) Then

            If Not udtSessionHandlerBLL.ClaimCOVID19ValidReprintGetFromSession(FunctionCode) Then
                ibtnReprintRecord.Enabled = False
                ibtnReprintRecord.ImageUrl = GetGlobalResourceObject("ImageUrl", "ReprintDisableBtn")

                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00050)
            End If

        ElseIf strSelectedRecordType = Formatter.EnumToString(RecordTypeEnumClass.MedicalExemption) Then
            ' Check Exemption Expired        
            If IsExemptionRecordValid(udtEHSTransaction) = False Then
                ' Message: The Medicial Exemption Record has been expired.
                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
            End If

        End If

        ' Check Document Type
        If ViewState(VS.SelectedDocumentType) <> udtEHSTransaction.DocCode Then

            If strSelectedRecordType = Formatter.EnumToString(RecordTypeEnumClass.VaccinationRecord) Then
                ' Message: The document type of the last COVID19 transaction with same document no. is different with the searching document.
                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            Else
                ' Message: The Document Type of eHealth (Subsidies) Account is different with the searching document.
                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
            End If

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        End If

        udcInfoMessageBox.BuildMessageBox()
    End Sub

    Private Sub InitializeDataValue()
        udcClaimTransDetail.ClearDocumentType()
        udcClaimTransDetail.ClearEHSClaim()
        udcClaimTransDetail.ClearVaccineRecord()

        udcInfoMessageBox.Visible = False
        udcErrorMessage.Visible = False

        Me.ddleHSDocType.Items.Clear()
        BindDocumentType(Me.ddleHSDocType)
        Me.ddleHSDocType.Enabled = True
        Me.txteHSDocNo.Text = String.Empty
        rblRPRecordType.SelectedIndex = -1

    End Sub


#End Region

#Region "Event Handler"
    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim blnIsValid As Boolean = False
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        imgRPRecordTypeErr.Visible = False
        imgeHSDocTypeErr.Visible = False
        imgeHSDocNoErr.Visible = False

        udcErrorMessage.Clear()
        udcInfoMessageBox.Clear()

        udtAuditLogEntry.AddDescripton("Record Type", Me.rblRPRecordType.SelectedValue)
        udtAuditLogEntry.AddDescripton("Doc Code", Me.ddleHSDocType.SelectedValue)
        udtAuditLogEntry.AddDescripton("Doc No", Me.txteHSDocNo.Text)
        AuditLogUserType(udtAuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        Session(SESS.SelectedRecordType) = Me.rblRPRecordType.SelectedValue

        blnIsValid = SearchValidation(False)

        If blnIsValid Then
            SearchCOVID19Transaction(Me.rblRPRecordType.SelectedValue, Me.txteHSDocNo.Text, Me.ddleHSDocType.SelectedValue)
        Else
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00004, AuditLogDescription.LOG00004)
        End If

    End Sub

    Protected Sub ibtnSearchByCard_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnIsValid As Boolean = False

        imgRPRecordTypeErr.Visible = False
        imgeHSDocTypeErr.Visible = False
        imgeHSDocNoErr.Visible = False

        udcErrorMessage.Clear()
        udcInfoMessageBox.Clear()


        Session(SESS.SelectedRecordType) = Me.rblRPRecordType.SelectedValue

        Try
            udtAuditLogEntry.AddDescripton("Record Type", Me.rblRPRecordType.SelectedValue)
            AuditLogUserType(udtAuditLogEntry)
            udtAuditLogEntry.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)


            blnIsValid = SearchValidation(True)

            If blnIsValid Then
                Me.RedirectToIdeasCombo(IdeasBLL.EnumIdeasVersion.Combo)
                AuditLogUserType(udtAuditLogEntry)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, AuditLogDescription.LOG00006)

            Else
                udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00007, AuditLogDescription.LOG00007)
            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            AuditLogUserType(udtAuditLogEntry)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDescription.LOG00007)
        End Try
    End Sub

    Protected Sub SearchCOVID19Transaction(ByVal strRecordType As String, ByVal streHSDocNo As String, ByVal strSearchDocType As String)
        Dim udtSearchCriteria = New SearchCriteria
        Dim dt As New DataTable
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing
        Dim strTransNum As String = String.Empty
        Dim strDocType As String = String.Empty
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnIsValid As Boolean = False

        udtAuditLogEntry.AddDescripton("Record Type", strRecordType)
        udtAuditLogEntry.AddDescripton("Doc Code", streHSDocNo)
        udtAuditLogEntry.AddDescripton("Doc No", strSearchDocType)
        AuditLogUserType(udtAuditLogEntry)

        udtAuditLogEntry.WriteStartLog(LogID.LOG00002, AuditLogDescription.LOG00002)

        udtSessionHandlerBLL.ClaimCOVID19ValidReprintRemoveFromSession(FunctionCode)
        udtSessionHandlerBLL.ClaimCOVID19VaccinationCardRemoveFromSession(FunctionCode)

        udtSessionHandlerBLL.CMSVaccineResultRemoveFromSession(FunctionCode)
        udtSessionHandlerBLL.CIMSVaccineResultRemoveFromSession(FunctionCode)

        ViewState(VS.SelectedDocumentType) = strSearchDocType

        Try

            ' Search
            Select Case Formatter.StringToEnum(GetType(RecordTypeEnumClass), strRecordType)
                Case RecordTypeEnumClass.VaccinationRecord
                    SearchVaccinationRecord(streHSDocNo, strSearchDocType)

                Case RecordTypeEnumClass.MedicalExemption
                    SearchMedicalExemption(streHSDocNo, strSearchDocType)

                Case Else
                    'Nothing

            End Select

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            AuditLogUserType(udtAuditLogEntry)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDescription.LOG00004)
            Throw
        End Try

    End Sub

    Protected Sub ibtnDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        AuditLogUserType(udtAuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditLogDescription.LOG00011)

        If ViewState(VS.PrevActiveViewIndex) = ViewIndex.SearchResultMEC Then
            ChangeView(ViewIndex.SearchResultMEC)
        Else
            BackToSearchPage()
        End If
    End Sub

    Protected Sub ibtnReprintRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnReprintRecord.Click

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim udtEHSTransaction As EHSTransactionModel = Session(SESS.EHSTransaction)
        Dim udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL

        Try
            Dim strRecordType As String = Session(SESS.SelectedRecordType)

            udtAuditLogEntry.AddDescripton("Record Type", strRecordType)
            AuditLogUserType(udtAuditLogEntry)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDescription.LOG00008)

            Dim strPrintDateTime As String = String.Format("DH_HCV103{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
            udtEHSTransaction.PrintedConsentForm = True

            udtEHSTransaction.EHSAcct.SetSearchDocCode(udtEHSTransaction.DocCode)

            'Check Discharge List
            If strRecordType = Formatter.EnumToString(RecordTypeEnumClass.VaccinationRecord) Then
                udtSessionHandlerBLL.ClaimCOVID19DischargeRecordRemoveFromSession(FunctionCode)
                CheckCOVID19DischargeRecord(udtEHSTransaction)
            End If

            'Set the transaction is printed consent Form
            udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)
            udtSessionHandler.EHSAccountSaveToSession(udtEHSTransaction.EHSAcct, FunctionCode)

            'Save the current function code to session (will be removed in the printout form)
            udtSessionHandler.EHSClaimPrintoutFunctionCodeSaveToSession(FunctionCode)

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherConsentFormScript", "javascript:openNewWin('../Printout/BasePrintoutForm.aspx?TID=" + strPrintDateTime + "');", True)

            AuditLogUserType(udtAuditLogEntry)

            If strRecordType = Formatter.EnumToString(RecordTypeEnumClass.VaccinationRecord) Then
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDescription.LOG00009)

            Else
                If IsExemptionRecordValid(udtEHSTransaction) Then
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, AuditLogDescription.LOG00017)
                Else
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00018, AuditLogDescription.LOG00018)
                End If
            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            AuditLogUserType(udtAuditLogEntry)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00010, AuditLogDescription.LOG00010)
            Throw
        End Try

    End Sub

    Private Sub BackToSearchPage()
        ChangeView(ViewIndex.Search)
        InitializeDataValue()
    End Sub

#End Region

#Region "Search"
    Private Function SearchValidation(ByVal blnReadCard As Boolean) As Boolean
        Dim blnValid As Boolean = True

        If rblRPRecordType.SelectedValue = String.Empty Then
            udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367, "%s", lblRPRecordTypeText.Text)
            imgRPRecordTypeErr.Visible = True
            blnValid = False
        End If

        If blnReadCard = False AndAlso ddleHSDocType.SelectedValue = String.Empty Then
            udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367, "%s", lbleHSDocTypeText.Text)
            imgeHSDocTypeErr.Visible = True
            blnValid = False
        End If

        If blnReadCard = False AndAlso txteHSDocNo.Text.Trim = String.Empty Then
            udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00211)
            imgeHSDocNoErr.Visible = True
            blnValid = False
        End If

        If udcErrorMessage.GetCodeTable.Rows.Count = 0 Then
            udcErrorMessage.Visible = False
        End If

        Return blnValid
    End Function

    Private Sub SearchVaccinationRecord(ByVal streHSDocNo As String, ByVal strSearchDocType As String)
        Dim dt As New DataTable
        Dim udtSearchCriteria As New SearchCriteria
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing
        Dim strTransNum As String = String.Empty
        Dim strDocType As String = String.Empty
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnIsValid As Boolean = False

        udtSearchCriteria.DocumentType = strSearchDocType
        Dim aryDocumentNo As String()

        Select Case strSearchDocType
            Case DocTypeModel.DocTypeCode.HKIC, DocTypeModel.DocTypeCode.EC, DocTypeModel.DocTypeCode.DI, _
                DocTypeModel.DocTypeCode.REPMT, DocTypeModel.DocTypeCode.ID235B, DocTypeModel.DocTypeCode.VISA, _
                DocTypeModel.DocTypeCode.ADOPC, DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.CCIC, _
                DocTypeModel.DocTypeCode.ROP140
                aryDocumentNo = streHSDocNo.Replace("(", "").Replace(")", "").Replace("-", "").Split("/")
            Case DocTypeModel.DocTypeCode.DS
                aryDocumentNo = streHSDocNo.Replace("(", "").Replace(")", "").Replace("/", "").Split("")
            Case Else
                'DocTypeModel.DocTypeCode.OW, DocTypeModel.DocTypeCode.PASS
                aryDocumentNo = streHSDocNo.Split("")
        End Select


        If aryDocumentNo.Length > 1 Then
            udtSearchCriteria.DocumentNo1 = aryDocumentNo(1)
            udtSearchCriteria.DocumentNo2 = aryDocumentNo(0)
        Else
            udtSearchCriteria.DocumentNo1 = aryDocumentNo(0)
            udtSearchCriteria.DocumentNo2 = String.Empty
        End If
        udtSearchCriteria.RawIdentityNum = streHSDocNo

        'Search COVID19 Vaccination Record
        dt = udtCOVID19BLL.GetLatestCovid19TransactionByDocId(udtSearchCriteria)

        udtAuditLogEntry.AddDescripton("No. of Records", dt.Rows.Count)

        If dt.Rows.Count > 0 Then
            Dim udtEHSTransactionLatest As EHSTransactionModel = Nothing
            Dim udtEHSAccountLatest As EHSAccountModel = Nothing
            Dim udtPersonalInfoLatest As EHSAccountModel.EHSPersonalInformationModel = Nothing

            Dim udtEHSTransactionCurrent As EHSTransactionModel = Nothing
            Dim udtPersonalInfoCurrent As EHSAccountModel.EHSPersonalInformationModel = Nothing

            Dim blnValidToPrint As Boolean = True

            strDocType = dt.Rows(0).Item("Doc_Code").ToString.Trim
            strTransNum = dt.Rows(0).Item("Transaction_ID").ToString.Trim

            udtEHSTransactionLatest = udtEHSTransactionBLL.LoadClaimTran(strTransNum, True, True)
            udtEHSAccountLatest = udtEHSTransactionLatest.EHSAcct

            For i As Integer = 1 To dt.Rows.Count - 1
                If dt.Rows(i).Item("Doc_Code").ToString.Trim = strDocType Then
                    strTransNum = dt.Rows(i).Item("Transaction_ID")

                    'Get Transaction
                    udtEHSTransactionCurrent = udtEHSTransactionBLL.LoadClaimTran(strTransNum, True, True)

                    'Get EHSAccount
                    If udtEHSAccountLatest IsNot Nothing Then
                        udtPersonalInfoLatest = udtEHSAccountLatest.EHSPersonalInformationList.Filter(strDocType)

                        udtPersonalInfoCurrent = udtEHSTransactionCurrent.EHSAcct.EHSPersonalInformationList.Filter(strDocType)

                        If udtPersonalInfoCurrent.UpdateDtm >= udtPersonalInfoLatest.UpdateDtm Then
                            If udtPersonalInfoCurrent.UpdateDtm = udtPersonalInfoLatest.UpdateDtm AndAlso udtEHSAccountLatest.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                                'Nothing to do
                            Else
                                udtEHSAccountLatest = udtEHSTransactionCurrent.EHSAcct
                            End If

                        End If

                    Else
                        udtEHSAccountLatest = udtEHSTransactionCurrent.EHSAcct

                    End If

                End If

            Next

            'Merge: Latest transaction + Latest account
            udtEHSTransactionLatest.EHSAcct = udtEHSAccountLatest

            'Get vaccination record
            udcClaimTransDetail.FunctionCode = Me.FunctionCode
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = udcClaimTransDetail.GetVaccinationRecordFromSession(udtEHSAccountLatest, udtEHSTransactionLatest.SchemeCode)

            If Session(SESS_UserType) = "HCSPUser" Then
                'Search HCSPUser account, set false at default
                blnValidToPrint = False

                Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
                Dim strUserID As String = String.Empty

                If udtHCVUUser.UserID.Contains("_") Then
                    Dim strUser() As String = Split(udtHCVUUser.UserID, "_")
                    strUserID = strUser(0)
                Else
                    strUserID = udtHCVUUser.UserID
                End If

                For Each udtVaccineRecord As TransactionDetailVaccineModel In udtTranDetailVaccineList
                    Dim udtCurEHSTransaction As EHSTransactionModel = Nothing

                    'Not COVID-19, not check 
                    If udtVaccineRecord.SubsidizeItemCode <> SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19 Then Continue For

                    'CMS / CIMS record, not check 
                    If udtVaccineRecord.TransactionID Is Nothing OrElse udtVaccineRecord.TransactionID = String.Empty Then Continue For

                    If udtVaccineRecord.TransactionID.Trim.ToUpper = udtEHSTransactionLatest.TransactionID.Trim.ToUpper Then
                        udtCurEHSTransaction = udtEHSTransactionLatest
                    Else
                        udtCurEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(udtVaccineRecord.TransactionID, True, True)
                    End If

                    ' For Centre only
                    If udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19CVC OrElse _
                       udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19DH OrElse _
                       udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19RVP OrElse _
                       udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19OR OrElse _
                       udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19SR OrElse _
                       udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19SB Then

                        Dim dtLoginUserVaccineCentre As DataTable = udtCOVID19BLL.GetCOVID19VaccineCentreBySPID(strUserID)

                        ' User from Centre
                        If dtLoginUserVaccineCentre IsNot Nothing Then

                            Dim dtTransactionVaccineCentre As DataTable = udtCOVID19BLL.GetCOVID19VaccineCentreBySPIDPracticeDisplaySeq(udtCurEHSTransaction.ServiceProviderID, _
                                                                                                                                        udtCurEHSTransaction.PracticeID)

                            Dim drLoginUserVaccineCentre() As DataRow

                            If dtTransactionVaccineCentre IsNot Nothing AndAlso dtTransactionVaccineCentre.Rows.Count > 0 Then
                                For intIdx As Integer = 0 To dtTransactionVaccineCentre.Rows.Count - 1
                                    Dim strTransVaccineCentreID As String = dtTransactionVaccineCentre.Rows(intIdx)("Centre_ID").ToString

                                    drLoginUserVaccineCentre = dtLoginUserVaccineCentre.Select(String.Format("Centre_ID = '{0}'", strTransVaccineCentreID))

                                    If drLoginUserVaccineCentre.Length > 0 Then
                                        blnValidToPrint = True
                                        Exit For
                                    End If
                                Next
                            End If

                            If blnValidToPrint Then
                                Exit For
                            End If
                        End If
                    End If

                    'For Private: VSS, RVP
                    If udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.VSS OrElse _
                        udtCurEHSTransaction.SchemeCode.Trim.ToUpper = SchemeClaimModel.RVP Then

                        If udtCurEHSTransaction.ServiceProviderID = strUserID Then
                            blnValidToPrint = True
                            Exit For
                        End If

                    End If

                Next

            End If

            udtAuditLogEntry.AddDescripton("Valid to print", IIf(blnValidToPrint, "Y", "N"))
            AuditLogUserType(udtAuditLogEntry)
            If blnValidToPrint Then
                'Build EHS transaction Detail
                ViewState(VS.PrevActiveViewIndex) = ViewIndex.Search
                BuildClaimTransDetail(udtEHSTransactionLatest)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)

            Else
                ' Message: No records found.
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.AddMessage("010421", "I", "00002")
                udcInfoMessageBox.BuildMessageBox()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)
            End If

        Else
            ' Message: No records found.
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.AddMessage("990000", "I", "00001")
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)
        End If

    End Sub


    Private Sub SearchMedicalExemption(ByVal streHSDocNo As String, ByVal strSearchDocType As String)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum
        Dim udtSearchCriteria As New SearchCriteria

        ' Implement Collapsible Search Criteria Review
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"

        Try

            udtSearchCriteria.DocumentType = strSearchDocType
            Dim aryDocumentNo As String()

            Select Case strSearchDocType
                Case DocTypeModel.DocTypeCode.HKIC, DocTypeModel.DocTypeCode.EC, DocTypeModel.DocTypeCode.DI, _
                    DocTypeModel.DocTypeCode.REPMT, DocTypeModel.DocTypeCode.ID235B, DocTypeModel.DocTypeCode.VISA, _
                    DocTypeModel.DocTypeCode.ADOPC, DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.CCIC, _
                    DocTypeModel.DocTypeCode.ROP140
                    aryDocumentNo = streHSDocNo.Replace("(", "").Replace(")", "").Replace("-", "").Split("/")
                Case DocTypeModel.DocTypeCode.DS
                    aryDocumentNo = streHSDocNo.Replace("(", "").Replace(")", "").Replace("/", "").Split("")
                Case Else
                    'DocTypeModel.DocTypeCode.OW, DocTypeModel.DocTypeCode.PASS
                    aryDocumentNo = streHSDocNo.Split("")
            End Select

            If aryDocumentNo.Length > 1 Then
                udtSearchCriteria.DocumentNo1 = aryDocumentNo(1)
                udtSearchCriteria.DocumentNo2 = aryDocumentNo(0)
            Else
                udtSearchCriteria.DocumentNo1 = aryDocumentNo(0)
                udtSearchCriteria.DocumentNo2 = String.Empty
            End If
            udtSearchCriteria.RawIdentityNum = streHSDocNo

            Session(SESS.SearchCriteria) = udtSearchCriteria

            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcErrorMessage, udcInfoMessageBox, False, True)

        Catch eSQL As SqlClient.SqlException
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
            udtAuditLogEntry.AddDescripton("Message", eSQL.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDescription.LOG00004)
            Throw

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDescription.LOG00004)
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)

                If Me.gvSearchResult.Rows.Count = 1 Then
                    ' Go to Detail
                    Dim linkBtn As LinkButton = Me.gvSearchResult.Rows(0).FindControl("lbtn_transNum")
                    ResultLinkButton_Click(linkBtn, Nothing)
                Else
                    ' Go to Search Result
                    ChangeView(ViewIndex.SearchResultMEC)
                End If

            Case SearchResultEnum.ValidationFail
                udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDescription.LOG00004)

            Case SearchResultEnum.NoRecordFound
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDescription.LOG00004)

            Case SearchResultEnum.OverResultList1stLimit_Alert
                udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDescription.LOG00004)

            Case SearchResultEnum.OverResultListOverrideLimit
                udtAuditLogEntry.WriteEndLog(LogID.LOG00004, AuditLogDescription.LOG00004)

            Case Else
                Throw New Exception("Error: Class = [HCVU.ReprintVaccinationRecord], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

        End Select


    End Sub

    Private Function GetMedicalExemption(Optional ByVal udtSearchCriteria As SearchCriteria = Nothing, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim dtResult As New DataTable
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        If IsNothing(udtSearchCriteria) Then
            udtSearchCriteria = Session(SESS.SearchCriteria)
        End If

        ' Get Execption Cert List
        dtResult = udtCOVID19BLL.GetCovid19ExemptionCertByDocID(udtSearchCriteria.DocumentType, udtSearchCriteria.DocumentNo1, udtSearchCriteria.DocumentNo2)

        If Session(SESS_UserType) = "HCSPUser" Then
            'Filter By SP
            Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
            Dim strUserID As String = String.Empty

            If udtHCVUUser.UserID.Contains("_") Then
                Dim strUser() As String = Split(udtHCVUUser.UserID, "_")
                strUserID = strUser(0)
            Else
                strUserID = udtHCVUUser.UserID
            End If

            Dim dvResult As New DataView(dtResult)
            dvResult.RowFilter = "SP_ID ='" + strUserID + "'"
            dtResult = dvResult.ToTable()

        End If

        udtBLLSearchResult = New BaseBLL.BLLSearchResult(dtResult, True, True, BaseBLL.EnumSqlErrorMessage.Normal)

        Return udtBLLSearchResult

    End Function
#End Region

#Region "Search Result"
    Private Sub BuildSearchCriteriaReview(ByVal udtSearchCriteria As SearchCriteria)
        Dim udtValidator As New Common.Validation.Validator

        ' Record Type
        lblRRecordType.Text = FillAnyToEmptyString(Me.rblRPRecordType.SelectedItem.Text)

        ' eHealth Account Identity Document Type
        If udtValidator.IsEmpty(udtSearchCriteria.DocumentType) Then
            lblREHealthDocType.Text = FillAnyToEmptyString(udtSearchCriteria.DocumentType)
        Else
            Dim udtDocType As DocTypeModel = udtDocTypeBLL.getAllDocType().Filter(udtSearchCriteria.DocumentType)
            lblREHealthDocType.Text = udtDocType.DocName
        End If

        ' eHealth Account Identity Document No.
        lblREHealthDocNo.Text = FillAnyToEmptyString(udtSearchCriteria.RawIdentityNum)

    End Sub

    Private Function FillAnyToEmptyString(ByVal value As String) As String
        If IsNothing(value) OrElse value.Trim = String.Empty Then
            Return Me.GetGlobalResourceObject("Text", "Any")
        End If

        Return value
    End Function

    Protected Sub gvSearchResult_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            ' Transaction No.
            Dim lbtnTransactionNo As LinkButton = e.Row.FindControl("lbtn_transNum")
            lbtnTransactionNo.Text = udtFormatter.formatSystemNumber(dr.Item("Transaction_ID"))

            ' Service Date
            Dim lblServiceDate As Label = CType(e.Row.FindControl("lblGServiceDate"), Label)
            lblServiceDate.Text = udtFormatter.formatDisplayDate(CDate(dr.Item("Service_Receive_Dtm")))

            ' Valid Until
            Dim lblValidUntil As Label = CType(e.Row.FindControl("lblGValidUntil"), Label)
            lblValidUntil.Text = udtFormatter.formatDisplayDate(CDate(dr.Item("ValidUntil")))
        End If
    End Sub

    Protected Sub gvSearchResult_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udcInfoMessageBox.Visible = False
        udcErrorMessage.Visible = False

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.Equals("Sort")) Then
            Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim linkBtn As LinkButton = r.FindControl("lbtn_transNum")
            Dim strTransactionNo As String = linkBtn.CommandArgument.ToString.Trim

            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00016, AuditLogDescription.LOG00016)

            ResultLinkButton_Click(linkBtn, Nothing)
        End If
    End Sub

    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.SearchResultTable)
    End Sub

    Protected Sub gvSearchResult_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.SearchResultTable)
    End Sub

    Protected Sub gvSearchResult_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS.SearchResultTable)
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        AuditLogUserType(udtAuditLogEntry)
        udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditLogDescription.LOG00015)

        BackToSearchPage()

    End Sub

    'Result LinkButton Click -> 3. Detail Page
    Protected Sub ResultLinkButton_Click(sender As Object, e As EventArgs)
        udcInfoMessageBox.Visible = False
        udcErrorMessage.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim strTransactionNo As String = sender.CommandArgument.ToString.Trim

        udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00012, AuditLogDescription.LOG00012)

        Try
            Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionNo, True, True)

            ViewState(VS.PrevActiveViewIndex) = MultiViewReprintVaccinationRecord.ActiveViewIndex
            BuildClaimTransDetail(udtEHSTransaction)

            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00013, AuditLogDescription.LOG00013)

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditLogDescription.LOG00014)
            Throw
        End Try

    End Sub

    Private Sub ChangeView(ByVal ViewIndex As Integer)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False
        MultiViewReprintVaccinationRecord.ActiveViewIndex = ViewIndex
    End Sub

#End Region

#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Dim dtResult As DataTable
        Dim intRowCount As Integer

        Try
            dtResult = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        intRowCount = dtResult.Rows.Count

        If intRowCount > 0 Then

            GridViewDataBind(gvSearchResult, dtResult, "Service_Receive_Dtm", "DESC", False)
            Session(SESS.SearchResultTable) = dtResult

            Dim udtSearchCriteria As SearchCriteria = Session(SESS.SearchCriteria)
            BuildSearchCriteriaReview(udtSearchCriteria)

        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_CancelSearch_Click()

    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()

    End Sub

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            Return GetMedicalExemption(Nothing, True)
        Else
            Return GetMedicalExemption()
        End If
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

    'Page Load : LOG00000
    Public Shared Sub AuditLogPageLoad(ByVal udtAuditLogEntry As AuditLogEntry, ByVal selectedPractice As Boolean, ByVal comeFromAccountCreation As Boolean)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AuditLogDescription.LOG00000)
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

        Me.rblRPRecordType.SelectedValue = Session(SESS.SelectedRecordType)
        ddleHSDocType.SelectedValue = DocTypeModel.DocTypeCode.HKIC
        SearchCOVID19Transaction(Me.rblRPRecordType.SelectedValue, udtPersonalInfoSmartID.IdentityNum, ddleHSDocType.SelectedValue)

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


    Public Sub AuditLogUserType(ByRef udtAuditLogEntry As AuditLogEntry)
        If Session(SESS_UserType) = "HCSPUser" Then
            udtAuditLogEntry.AddDescripton("User Type", "SP")
        Else
            udtAuditLogEntry.AddDescripton("User Type", "BO")
        End If
    End Sub


#End Region

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
#Region "Check Discharge List"
    Private Sub CheckCOVID19DischargeRecord(ByVal udtEHSTransaction As EHSTransactionModel)
        'Check discharge list
        udtEHSTransaction.EHSAcct.SetSearchDocCode(udtEHSTransaction.DocCode)

        Dim udtDischargeResult As DischargeResultModel = (New COVID19.COVID19BLL).GetCovid19DischargePatientByDocCodeDocNo(udtEHSTransaction.EHSAcct)

        If udtDischargeResult IsNot Nothing AndAlso _
            (udtDischargeResult.DemographicResult = COVID19.DischargeResultModel.Result.ExactMatch OrElse _
            udtDischargeResult.DemographicResult = COVID19.DischargeResultModel.Result.PartialMatch) Then

            udtSessionHandlerBLL.ClaimCOVID19DischargeRecordSaveToSession(udtDischargeResult, FunctionCode)
        End If

    End Sub

#End Region
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-023-71 (COVID19 - Medical Exemption Record) [Start][Winnie SUEN]
    ' -----------------------------------------------------------------------
#Region "Check Exemption Record"
    Private Function IsExemptionRecordValid(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Dim dtmValidUntil As Date
        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        If Date.TryParse(udtEHSTransaction.TransactionAdditionFields.ValidUntil, dtmValidUntil) AndAlso dtmValidUntil >= dtmCurrentDate Then
            Return True
        End If

        Return False
    End Function

#End Region
    ' CRE20-023-71 (COVID19 - Medical Exemption Record) [End][Winnie SUEN]
End Class