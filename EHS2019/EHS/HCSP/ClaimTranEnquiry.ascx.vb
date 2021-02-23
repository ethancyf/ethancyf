Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL
Imports Common.Component.StaticData


Partial Public Class ClaimTranEnquiry
    Inherits System.Web.UI.UserControl

#Region "Fields"

    Private udtEHSClaimBLL As New EHSClaimBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtSchemeClaimBLL As New SchemeClaimBLL
    Private udtValidator As New Validator

#End Region

#Region "Session Constants"

    Public Const SESS_ShowSchemeLegend As String = "ShowSchemeLegend"

#End Region

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    End Sub

    'Public Sub buildClaimObject(ByVal strTransactionNo As String, ByVal blnMaskIdentityNo As Boolean)
    '    Dim udtEHSTransaction As New EHSTransactionModel
    '    buildClaimObject(strTransactionNo, udtEHSTransaction, blnMaskIdentityNo)
    'End Sub

    Public Sub buildClaimObject(ByVal strTransactionNo As String, ByRef udtEHSTransaction As EHSTransactionModel, ByVal blnMaskIdentityNo As Boolean)
        buildClaimObject(strTransactionNo, udtEHSTransaction, blnMaskIdentityNo, False)
    End Sub

    Public Sub buildClaimObject(ByVal strTransactionNo As String, ByRef udtEHSTransaction As EHSTransactionModel, ByVal blnMaskIdentityNo As Boolean, ByVal blnForceRebuildInput As Boolean)
        buildClaimObject(strTransactionNo, udtEHSTransaction, Nothing, blnMaskIdentityNo, False)
    End Sub

    Public Sub buildClaimObject(ByVal strTransactionNo As String, ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtEHSTransactionOriginal As EHSTransactionModel, ByVal blnMaskIdentityNo As Boolean, ByVal blnForceRebuildInput As Boolean)

        If IsNothing(udtEHSTransaction) OrElse udtEHSTransaction.TransactionID.Trim = String.Empty Then
            udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(strTransactionNo)
        End If

        ' --- Account Information ---

        If Not IsNothing(udtEHSTransaction.EHSAcct) Then
            udcReadOnlyDocumentType.DocumentType = udtEHSTransaction.DocCode.Trim
            udcReadOnlyDocumentType.EHSAccount = udtEHSTransaction.EHSAcct
            udcReadOnlyDocumentType.Vertical = False
            udcReadOnlyDocumentType.MaskIdentityNo = blnMaskIdentityNo
            udcReadOnlyDocumentType.ShowAccountRefNo = False
            udcReadOnlyDocumentType.ShowTempAccountNotice = False
            udcReadOnlyDocumentType.ShowAccountCreationDate = False
            udcReadOnlyDocumentType.Mode = ucInputDocTypeBase.BuildMode.Modification
            udcReadOnlyDocumentType.TableTitleWidth = 205
            udcReadOnlyDocumentType.SetEnableToShowHKICSymbol = True

            If udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.Invalidated Then
                udcReadOnlyDocumentType.IsInvalidAccount = True
            Else
                udcReadOnlyDocumentType.IsInvalidAccount = False
            End If

            udcReadOnlyDocumentType.Built()

        End If

        ' CRE20-0XX (Immu record)  [Start][Raiman]
        ' Display setting for COVID-19
        If IsClaimCOVID19(udtEHSTransaction) Then
            'trTransactionStatus.Style.Add("display", "none")
            'trPractice.Style.Add("display", "none")
            trBankAcct.Style.Add("display", "none")
            trServiceType.Style.Add("display", "none")
            'trServiceDate.Style.Add("display", "none")
            lblClaimInfo.Text = Me.GetGlobalResourceObject("Text", "VaccineInfo")
            panRecipinetContactInfo.Visible = True
            lblServiceDateText.Text = Me.GetGlobalResourceObject("Text", "InjectionDate")

            'Contact No. & Mobile
            If udtEHSTransaction.TransactionAdditionFields.ContactNo IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                trContactNo.Visible = True

                If udtEHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
                    lblContactNo.Text = udtEHSTransaction.TransactionAdditionFields.ContactNo

                    If udtEHSTransaction.TransactionAdditionFields.Mobile IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.Mobile = YesNo.Yes Then
                        lblContactNo.Text = lblContactNo.Text & " (" & GetGlobalResourceObject("Text", "Mobile") & ")"
                    End If
                Else
                    lblContactNo.Text = GetGlobalResourceObject("Text", "NotProvided")
                End If

            End If

            'Remarks
            If udtEHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
                lblRemark.Text = udtEHSTransaction.TransactionAdditionFields.Remarks
            Else
                lblRemark.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If

            'Join EHRSS
            If udtEHSTransaction.SchemeCode = SchemeClaimModel.COVID19CVC OrElse udtEHSTransaction.SchemeCode = SchemeClaimModel.COVID19CBD Then
                panJoinEHRSS.Visible = True
                If udtEHSTransaction.TransactionAdditionFields.JoinEHRSS IsNot Nothing And udtEHSTransaction.TransactionAdditionFields.JoinEHRSS <> String.Empty Then
                    lblJoinEHRSS.Text = IIf(udtEHSTransaction.TransactionAdditionFields.JoinEHRSS = YesNo.Yes, _
                                               GetGlobalResourceObject("Text", "Yes"), _
                                               GetGlobalResourceObject("Text", "No"))

                Else
                    lblJoinEHRSS.Text = GetGlobalResourceObject("Text", "NotProvided")
                End If
            Else
                panJoinEHRSS.Visible = False
            End If


        Else
            lblClaimInfo.Text = Me.GetGlobalResourceObject("Text", "ClaimInfo")
            panRecipinetContactInfo.Visible = False
            lblServiceDateText.Text = Me.GetGlobalResourceObject("Text", "ServiceDate")
        End If
        ' CRE20-0XX (Immu record)  [End][Raiman]
        
        ' --- Claim Information ---

        ' Transaction No.
        lblTransactionNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        lblTransactionDate.Text = String.Format(" ({0})", udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm, "EN-US"))
        lblTransactionDate_Chi.Text = String.Format(" ({0})", udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm, "ZH-TW"))

        ' Confirmed Time
        If udtEHSTransaction.ConfirmDate.HasValue Then
            lblConfirmTime.Text = udtFormatter.formatDateTime(udtEHSTransaction.ConfirmDate, String.Empty)
            lblConfirmTime_Chi.Text = udtFormatter.formatDateTime(udtEHSTransaction.ConfirmDate, "ZH-TW")
        Else
            lblConfirmTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
            lblConfirmTime_Chi.Text = Me.GetGlobalResourceObject("Text", "N/A")
        End If

        ' Scheme
        Dim udtSchemeClaim As SchemeClaimModel = Me.udtSchemeClaimBLL.getAllDistinctSchemeClaim.Filter(udtEHSTransaction.SchemeCode)

        lblScheme.Text = udtSchemeClaim.SchemeDesc
        lblScheme_Chi.Text = udtSchemeClaim.SchemeDescChi
        lblScheme_CN.Text = udtSchemeClaim.SchemeDescCN

        Dim udtTranTAF As TransactionAdditionalFieldModel = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType)

        If Not IsNothing(udtTranTAF) AndAlso udtTranTAF.AdditionalFieldValueCode = "N" Then
            lblScheme.Text += String.Format("<br>({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
            lblScheme_Chi.Text += String.Format("<br>({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
            lblScheme_CN.Text += String.Format("<br>({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))

        End If

        ' Transaction Status 
        Dim strTransactionStatus As String = udtEHSTransaction.RecordStatus
        If udtEHSTransaction.AuthorisedStatus = "R" Then strTransactionStatus = "R"

        If udtEHSTransaction.RecordStatus = "A" And udtEHSTransaction.ManualReimburse = True Then
            strTransactionStatus = "R"
        End If

        Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, strTransactionStatus, lblTransactionStatus.Text, lblTransactionStatus_Chi.Text, lblTransactionStatus_CN.Text)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
        ' -----------------------------------------------------------------------------------------
        If strTransactionStatus.ToUpper.Equals(ClaimTransStatus.Incomplete.ToUpper) Then
            lblTransactionStatus.CssClass = "tableTextAlert"
            lblTransactionStatus_Chi.CssClass = "tableTextAlertChi"
            lblTransactionStatus_CN.CssClass = "tableTextAlertChi"
        Else
            lblTransactionStatus.CssClass = "tableText"
            lblTransactionStatus_Chi.CssClass = "tableTextChi"
            lblTransactionStatus_CN.CssClass = "tableTextChi"
        End If
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        Dim udtStaticDataModel As StaticDataModel



        ' CRE13-001 EHAPP [Start][Karl]
        ' ----------------------------------------------------------------------------------------
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeClaimModel As SchemeClaimModel

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim blnReimbursementAvailable As Boolean = False

        'INT14-0017 (Fix HSCP Claim Trans Management search for expired scheme) [ Start][Karl]
        'udtSchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaim(udtEHSTransaction.SchemeCode)
        udtSchemeClaimModel = udtSchemeClaimBLL.getAllDistinctSchemeClaim.Filter(udtEHSTransaction.SchemeCode)
        'INT14-0017 (Fix HSCP Claim Trans Management search for expired scheme) [End][Karl]

        If udtSchemeClaimModel.ReimbursementMode = EnumReimbursementMode.All _
                     OrElse udtSchemeClaimModel.ReimbursementMode = EnumReimbursementMode.FirstAuthAndSecondAuth Then
            blnReimbursementAvailable = True
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ' Reimbursment Method
        'If udtEHSTransaction.ManualReimburse = True Then
        If udtEHSTransaction.ManualReimburse = True AndAlso blnReimbursementAvailable = True Then
            ' CRE13-001 EHAPP [End][Karl]
            Me.lblReimbursementMethod.Visible = True
            Me.lblReimbursementMethod_Chi.Visible = True
            Me.lblReimbursementMethod_CN.Visible = True

            If udtEHSTransaction.ManualReimburse Then
                udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementMethod", "O") ' O-Outsidehs I-In EHS
                Me.lblReimbursementMethod.Text = "(" + udtStaticDataModel.DataValue.ToString.Trim() + ")"
                Me.lblReimbursementMethod_Chi.Text = "(" + udtStaticDataModel.DataValueChi.ToString.Trim() + ")"
                Me.lblReimbursementMethod_CN.Text = "(" + udtStaticDataModel.DataValueCN.ToString.Trim() + ")"
            End If
        Else
            Me.lblReimbursementMethod.Text = String.Empty
            Me.lblReimbursementMethod_Chi.Text = String.Empty
            Me.lblReimbursementMethod_CN.Text = String.Empty
            Me.lblReimbursementMethod.Visible = False
            Me.lblReimbursementMethod_Chi.Visible = False
            Me.lblReimbursementMethod_CN.Visible = False
        End If

        'Invalidation Status
        If udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.Invalidated Then
            Dim strInvalidationStatus As String = ""
            Dim strInvalidationStatus_Chi As String = ""
            Dim strInvalidationStatus_CN As String = ""
            Status.GetDescriptionFromDBCode(EHSTransactionModel.InvalidationStatusClass.ClassCode, udtEHSTransaction.Invalidation, strInvalidationStatus, strInvalidationStatus_Chi, strInvalidationStatus_CN)
            lblInvalidationStatus.Text = " (" + strInvalidationStatus + ")"
            lblInvalidationStatus_Chi.Text = " (" + strInvalidationStatus_Chi + ")"
            lblInvalidationStatus_CN.Text = " (" + strInvalidationStatus_CN + ")"
            lblInvalidationStatus.Visible = True
            lblInvalidationStatus_Chi.Visible = True
            lblInvalidationStatus_CN.Visible = True
            lblInvalidationStatus.ForeColor = Drawing.Color.Blue
            lblInvalidationStatus_Chi.ForeColor = Drawing.Color.Blue
            lblInvalidationStatus_CN.ForeColor = Drawing.Color.Blue
        Else
            lblInvalidationStatus.Text = ""
            lblInvalidationStatus_Chi.Text = ""
            lblInvalidationStatus_CN.Text = ""
            lblInvalidationStatus.Visible = False
            lblInvalidationStatus_Chi.Visible = False
            lblInvalidationStatus_CN.Visible = False
        End If

        ' Service Date
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'lblServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate, "EN-US")
        'lblServiceDate_Chi.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate, "ZH-TW")
        lblServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, CultureLanguage.English)
        lblServiceDate_Chi.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, CultureLanguage.TradChinese)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' Service Provider
        lblSP.Text = udtEHSTransaction.ServiceProviderName
        lblSP_Chi.Text = udtEHSTransaction.ServiceProviderNameChi

        ' Practice
        lblPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID.ToString)
        lblPractice_Chi.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeNameChi, udtEHSTransaction.PracticeID.ToString)

        ' Bank Account No.
        lblBankAcct.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)

        ' Service Type
        lblServiceType.Text = udtEHSTransaction.ServiceTypeDesc
        lblServiceType_Chi.Text = udtEHSTransaction.ServiceTypeDesc_Chi
        lblServiceType_CN.Text = udtEHSTransaction.ServiceTypeDesc_CN

        ' Scheme-related fields

        ' Check if the Scheme's subsidize is in type of Vaccine
        Dim strSubsidizeType As String = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup() _
                                           .Filter(udtEHSTransaction.SchemeCode).SubsidizeGroupClaimList(0).SubsidizeType

        If strSubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
            udcReadOnlyEHSClaim.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If

        udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        udcReadOnlyEHSClaim.SchemeCode = udtEHSTransaction.SchemeCode
        udcReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [Start][Karl]
        udcReadOnlyEHSClaim.TableTitleWidth = 205
        'udcReadOnlyEHSClaim.TableTitleWidth = 185
        'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [End][Karl]
        udcReadOnlyEHSClaim.Built()

        'Dim udtSessionHandler As New SessionHandler
        'udtSessionHandler.SchemeSelectedSaveToSession(udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(udtEHSTransaction.SchemeCode), 

        ' CRE13-001 EHAPP [Start][Karl]
        ' ----------------------------------------------------------------------------------------
        If strSubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then
            ' CRE13-001 EHAPP [End][Karl]

            If blnForceRebuildInput Then udcInputEHSClaim.SetRebuildRequired()
            udcInputEHSClaim.EHSAccount = udtEHSTransaction.EHSAcct
            udcInputEHSClaim.EHSTransaction = udtEHSTransaction
            udcInputEHSClaim.EHSTransactionOriginal = udtEHSTransactionOriginal
            udcInputEHSClaim.SchemeType = udtEHSTransaction.SchemeCode
            udcInputEHSClaim.AvaliableForClaim = True
            'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [Start][Karl]
            udcInputEHSClaim.TableTitleWidth = 205
            'udcInputEHSClaim.TableTitleWidth = 185
            'INT14-0012 (Fix Net Service Fee Changed lead-zero input and DB value) [End][Karl]
            udcInputEHSClaim.ServiceDate = udtEHSTransaction.ServiceDate
            udcInputEHSClaim.IsModifyMode = True
            udcInputEHSClaim.Built()

            ' CRE13-001 EHAPP [Start][Karl]
            ' ----------------------------------------------------------------------------------------
        End If
        ' CRE13-001 EHAPP [End][Karl]
        AddHandler udcReadOnlyEHSClaim.VaccineLegendClicked, AddressOf udcReadOnlyEHSClaim_VaccineLegendClicked

        ' Void
        If udtEHSTransaction.RecordStatus = ClaimTransStatus.Inactive Then
            lblVoidNoText.Visible = True
            lblVoidNo.Visible = True
            lblVoidByText.Visible = True
            lblVoidReasonText.Visible = True
            lblVoidReason.Visible = True


            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            ' -----------------------------------------------------------------------------------------
            DisplayVoidNo(True)
            DisplayVoidReason(True)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

            lblVoidDate.Text = "(" + udtFormatter.convertDateTime(udtEHSTransaction.VoidDate, "EN-US") + ")"
            lblVoidDate_Chi.Text = "(" + udtFormatter.convertDateTime(udtEHSTransaction.VoidDate, "ZH-TW") + ")"

            If udtEHSTransaction.VoidByHCVU Then
                lblVoidBy.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")

                If Session("language") = CultureLanguage.SimpChinese Then
                    lblVoidBy_Chi.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealthCN")
                Else
                    lblVoidBy_Chi.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealthChi")
                End If

                lblVoidReasonText.Visible = False
                lblVoidReason.Visible = False
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                ' -----------------------------------------------------------------------------------------
                DisplayVoidReason(False)
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
            Else
                If udtEHSTransaction.VoidByDataEntry.Trim.Equals(String.Empty) Then
                    lblVoidBy.Text = udtEHSTransaction.VoidUser
                    lblVoidBy_Chi.Text = udtEHSTransaction.VoidUser
                Else
                    lblVoidBy.Text = udtEHSTransaction.VoidByDataEntry + " (" + udtEHSTransaction.VoidUser.Trim + ")"
                    lblVoidBy_Chi.Text = udtEHSTransaction.VoidByDataEntry + " (" + udtEHSTransaction.VoidUser.Trim + ")"
                End If

                lblVoidReason.Text = udtEHSTransaction.VoidReason
                lblVoidReasonText.Visible = True
                lblVoidReason.Visible = True
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                ' -----------------------------------------------------------------------------------------
                DisplayVoidReason(True)
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
            End If

            lblVoidNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo)

        Else
            lblVoidNo.Text = String.Empty
            lblVoidNoText.Visible = False
            lblVoidNo.Visible = False
            lblVoidByText.Visible = False
            lblVoidReasonText.Visible = False
            lblVoidReason.Visible = False

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            ' -----------------------------------------------------------------------------------------
            DisplayVoidNo(False)
            DisplayVoidReason(False)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
        End If


        'udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ClaimCreationReason", udtEHSTransaction.CreationReason)

        If udtEHSTransaction.ManualReimburse Then
            lblCreateBy.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")
            lblCreateBy_Chi.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealthChi")
            lblCreateBy_CN.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealthCN")

            lblPaymentMethodText.Visible = True
            lblPaymentMethod.Visible = True
            lblPaymentMethod_Chi.Visible = True
            lblPaymentMethod_CN.Visible = True

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            ' -----------------------------------------------------------------------------------------
            DisplayPaymentMethod(True)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]

        ElseIf udtEHSTransaction.SourceApp = EHSTransactionModel.AppSourceClass.SFUpload Then

            If (New Validator).chkSPID(udtEHSTransaction.CreateBy.Trim()) Is Nothing Then
                lblCreateBy.Text = udtEHSTransaction.CreateBy.Trim()
                lblCreateBy_Chi.Text = udtEHSTransaction.CreateBy.Trim()
                lblCreateBy_CN.Text = udtEHSTransaction.CreateBy.Trim()
            Else
                lblCreateBy.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")
                lblCreateBy_Chi.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealthChi")
                lblCreateBy_CN.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealthCN")
            End If

            lblPaymentMethodText.Visible = False
            lblPaymentMethod.Visible = False
            lblPaymentMethod_Chi.Visible = False
            lblPaymentMethod_CN.Visible = False

            DisplayPaymentMethod(False)
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]
        Else
            If udtEHSTransaction.DataEntryBy.Trim.Equals(String.Empty) Then
                lblCreateBy.Text = udtEHSTransaction.CreateBy.Trim()
                lblCreateBy_Chi.Text = udtEHSTransaction.CreateBy.Trim()
                lblCreateBy_CN.Text = udtEHSTransaction.CreateBy.Trim()
            Else
                lblCreateBy.Text = udtEHSTransaction.DataEntryBy.Trim() + " (" + udtEHSTransaction.CreateBy.Trim() + ")"
                lblCreateBy_Chi.Text = udtEHSTransaction.DataEntryBy.Trim() + " (" + udtEHSTransaction.CreateBy.Trim() + ")"
                lblCreateBy_CN.Text = udtEHSTransaction.DataEntryBy.Trim() + " (" + udtEHSTransaction.CreateBy.Trim() + ")"
            End If
            lblPaymentMethodText.Visible = False
            lblPaymentMethod.Visible = False
            lblPaymentMethod_Chi.Visible = False
            lblPaymentMethod_CN.Visible = False

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            ' -----------------------------------------------------------------------------------------
            DisplayPaymentMethod(False)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
        End If

        udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementPaymentMethod", udtEHSTransaction.PaymentMethod)
        lblPaymentMethod.Text = udtStaticDataModel.DataValue
        lblPaymentMethod_Chi.Text = udtStaticDataModel.DataValueChi
        lblPaymentMethod_CN.Text = udtStaticDataModel.DataValueCN

        If udtEHSTransaction.IsUpload Is Nothing Then
            hdnIsUpload.Value = "N"
        Else
            hdnIsUpload.Value = udtEHSTransaction.IsUpload
        End If

        If udtEHSTransaction.IsUpload = "Y" Then
            If LCase(Session("language")) = "zh-tw" OrElse LCase(Session("language")) = CultureLanguage.SimpChinese Then
                lblVia_Chi.Visible = True
            Else
                lblVia.Visible = True
            End If

            If Not udtEHSTransaction.DataEntryBy.Trim.Equals(String.Empty) Then
                lblVia_Chi.Text = Me.GetGlobalResourceObject("Text", "ViaChi") + " " + udtEHSTransaction.DataEntryBy.Trim()
                lblVia.Text = Me.GetGlobalResourceObject("Text", "Via") + " " + udtEHSTransaction.DataEntryBy.Trim()
                lblCreateBy.Text = " (" + udtEHSTransaction.CreateBy.Trim() + ")"
                lblCreateBy_Chi.Text = " (" + udtEHSTransaction.CreateBy.Trim() + ")"
                lblCreateBy_CN.Text = " (" + udtEHSTransaction.CreateBy.Trim() + ")"
            End If
        Else
            lblVia.Visible = False
            lblVia_Chi.Visible = False
        End If

    End Sub

    Public Sub chgLanguage()
        lblTransactionDate_Chi.Visible = False
        lblTransactionDate.Visible = False

        lblConfirmTime_Chi.Visible = False
        lblConfirmTime.Visible = False

        lblScheme_Chi.Visible = False
        lblScheme.Visible = False
        lblScheme_CN.Visible = False

        lblTransactionStatus.Visible = False
        lblTransactionStatus_Chi.Visible = False
        lblTransactionStatus_CN.Visible = False

        lblInvalidationStatus.Visible = False
        lblInvalidationStatus_Chi.Visible = False
        lblInvalidationStatus_CN.Visible = False

        lblServiceDate_Chi.Visible = False
        lblServiceDate.Visible = False

        lblSP.Visible = False
        lblSP_Chi.Visible = False

        lblServiceType_Chi.Visible = False
        lblServiceType.Visible = False
        lblServiceType_CN.Visible = False

        lblVoidDate_Chi.Visible = False
        lblVoidDate.Visible = False

        lblVoidBy_Chi.Visible = False
        lblVoidBy.Visible = False

        lblPractice_Chi.Visible = False
        lblPractice.Visible = False

        lblCreateBy.Visible = False
        lblCreateBy_Chi.Visible = False
        lblCreateBy_CN.Visible = False

        lblPaymentMethod.Visible = False
        lblPaymentMethod_Chi.Visible = False
        lblPaymentMethod_CN.Visible = False

        Me.lblReimbursementMethod.Visible = False
        Me.lblReimbursementMethod_Chi.Visible = False
        Me.lblReimbursementMethod_CN.Visible = False

        Me.lblVia.Visible = False
        Me.lblVia_Chi.Visible = False

        If Session("language") = CultureLanguage.TradChinese Then
            lblTransactionDate_Chi.Visible = True
            lblConfirmTime_Chi.Visible = True
            lblScheme_Chi.Visible = True
            lblTransactionStatus_Chi.Visible = True
            lblInvalidationStatus_Chi.Visible = True
            lblServiceDate_Chi.Visible = True

            If lblSP.Text.Trim = lblSP_Chi.Text.Trim Then
                lblSP.Visible = True
            Else
                lblSP_Chi.Visible = True
            End If

            lblServiceType_Chi.Visible = True

            If lblVoidNo.Text.Trim <> String.Empty Then
                lblVoidDate_Chi.Visible = True
                lblVoidBy_Chi.Visible = True
            End If

            ' Display Practice Chi Name
            If lblPractice.Text.Trim = lblPractice_Chi.Text.Trim Then
                lblPractice.Visible = True
            Else
                lblPractice_Chi.Visible = True
            End If

            lblCreateBy_Chi.Visible = True
            lblPaymentMethod_Chi.Visible = True
            Me.lblReimbursementMethod_Chi.Visible = True

            If hdnIsUpload.Value = "Y" Then
                lblVia_Chi.Visible = True
            End If

        ElseIf Session("language") = CultureLanguage.SimpChinese Then
            lblTransactionDate_Chi.Visible = True
            lblConfirmTime_Chi.Visible = True
            lblScheme_CN.Visible = True
            lblTransactionStatus_CN.Visible = True
            lblInvalidationStatus_CN.Visible = True
            lblServiceDate_Chi.Visible = True

            If lblSP.Text.Trim = lblSP_Chi.Text.Trim Then
                lblSP.Visible = True
            Else
                lblSP_Chi.Visible = True
            End If

            lblServiceType_CN.Visible = True

            If lblVoidNo.Text.Trim <> String.Empty Then
                lblVoidDate_Chi.Visible = True
                lblVoidBy_Chi.Visible = True
            End If

            ' Display Practice Chi Name
            If lblPractice.Text.Trim = lblPractice_Chi.Text.Trim Then
                lblPractice.Visible = True
            Else
                lblPractice_Chi.Visible = True
            End If

            lblCreateBy_CN.Visible = True
            lblPaymentMethod_CN.Visible = True
            Me.lblReimbursementMethod_CN.Visible = True

            If hdnIsUpload.Value = "Y" Then
                lblVia_Chi.Visible = True
            End If

        Else
            lblTransactionDate.Visible = True
            lblConfirmTime.Visible = True
            lblScheme.Visible = True
            lblTransactionStatus.Visible = True
            lblInvalidationStatus.Visible = True
            lblServiceDate.Visible = True
            lblSP.Visible = True
            lblServiceType.Visible = True

            If lblVoidNo.Text.Trim <> String.Empty Then
                lblVoidDate.Visible = True
                lblVoidBy.Visible = True
            End If

            ' Display Practice Chi Name
            lblPractice.Visible = True

            lblCreateBy.Visible = True
            lblPaymentMethod.Visible = True
            Me.lblReimbursementMethod.Visible = True

            If hdnIsUpload.Value = "Y" Then
                lblVia.Visible = True
            End If
        End If

        ' Force the invisible labels to get the text from resource again 
        lblVoidNoText.Text = Me.GetGlobalResourceObject("Text", "VoidTranID")
        lblVoidByText.Text = Me.GetGlobalResourceObject("Text", "VoidBy")
        lblVoidReasonText.Text = Me.GetGlobalResourceObject("Text", "VoidReason")
        lblCreateByText.Text = Me.GetGlobalResourceObject("Text", "CreateBy")
        lblPaymentMethodText.Text = Me.GetGlobalResourceObject("Text", "PaymentSettlement")

    End Sub

    Public Event VaccineLegendClicked1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Public Sub udcReadOnlyEHSClaim_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked1(sender, e)
    End Sub

    '** Must clear all dynamically added controls if not used used by caller
    Public Sub Clear()
        udcReadOnlyDocumentType.Clear()
        udcReadOnlyEHSClaim.Clear()
    End Sub

    Public Sub ClearInput()
        If udcInputEHSClaim IsNot Nothing Then
            Me.udcInputEHSClaim.Clear()
        End If

    End Sub

    Public Sub ChangeModifyMode()
        Me.udcReadOnlyEHSClaim.Visible = False
        Me.udcInputEHSClaim.Visible = True
        trTransactionNo.Style.Item("display") = "none"
        trTransactionStatus.Style.Item("display") = ""
        trConfirmTime.Style.Item("display") = "none"
        trBankAcct.Style.Item("display") = "none"
        trCreateBy.Style.Item("display") = "none"
    End Sub

    Public Sub ChangeViewMode()
        Me.udcReadOnlyEHSClaim.Visible = True
        Me.udcInputEHSClaim.Visible = False
        trTransactionNo.Style.Item("display") = ""
        trTransactionStatus.Style.Item("display") = ""
        trConfirmTime.Style.Item("display") = ""
        trBankAcct.Style.Item("display") = ""
        trCreateBy.Style.Item("display") = ""
    End Sub

    Public Sub ChangeConfirmDetailMode()
        Me.udcReadOnlyEHSClaim.Visible = True
        Me.udcInputEHSClaim.Visible = False
        trTransactionNo.Style.Item("display") = ""
        trTransactionStatus.Style.Item("display") = "none"
        trConfirmTime.Style.Item("display") = ""
        trBankAcct.Style.Item("display") = ""
        trCreateBy.Style.Item("display") = ""
    End Sub

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel)
        Dim udcInputHCVS As ucInputHCVS = Me.udcInputEHSClaim.GetHCVSControl()
        udcInputHCVS.Save(udtEHSTransaction)
    End Sub

    Public Function IsIncomplete(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Return udcInputEHSClaim.IsIncomplete(udtEHSTransaction)
    End Function

    Public Function Validate(ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim udcInputHCVS As ucInputHCVS = Me.udcInputEHSClaim.GetHCVSControl()
        Return udcInputHCVS.Validate(True, objMsgBox)
    End Function


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
    ' -----------------------------------------------------------------------------------------

    Private Sub DisplayVoidNo(ByVal blnDisplay As Boolean)
        Me.trVoidNo.Style.Item("display") = IIf(blnDisplay, "", "none")
        Me.trVoidBy.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayVoidReason(ByVal blnDisplay As Boolean)
        Me.trVoidReason.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayPaymentMethod(ByVal blnDisplay As Boolean)
        Me.trPaymentMethod.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

    Public Function IsClaimCOVID19(udtEHSTransaction) As Boolean

        Dim udtTranDetailList As TransactionDetailModelCollection = udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

        If udtTranDetailList.Count > 0 Then
            Return True
        End If

        Return False

    End Function

End Class