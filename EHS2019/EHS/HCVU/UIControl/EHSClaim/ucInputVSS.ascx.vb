Imports Common
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports HCVU.BLL
Imports System.Web.Security.AntiXss

Partial Public Class ucInputVSS
    Inherits ucInputEHSClaimBase

    Dim _udtSessionHandler As New SessionHandlerBLL
    Dim _udtGeneralFunction As New GeneralFunction

#Region "Constants"

    Private Class ClinicType
        Public Const Clinic As String = "C"
        Public Const NonClinic As String = "N"
    End Class

    Private Class CategoryControlID
        Public Const PID As String = "ucInputVSSPID"
        Public Const DA As String = "ucInputVSSDA"
        Public Const VSSCOVID19 As String = "ucInputVSSCOVID19"
    End Class

    Private Class ViewIndexCategory
        Public Const NoCategory As Integer = 0
        Public Const VSS_PW As Integer = 1
        Public Const VSS_CHILD As Integer = 2
        Public Const VSS_ELDER As Integer = 3
        Public Const VSS_PID As Integer = 4
        Public Const VSS_DA As Integer = 5
        Public Const VSS_ADULT As Integer = 6
        Public Const VSS_COVID19 As Integer = 7
    End Class

    Private Class PlaceOfVaccinationOptions
        Public Const OTHERS As String = "OTHERS"
    End Class

#End Region

#Region "Properties"
    Public ReadOnly Property FunctCode() As String
        Get
            Return MyBase.FunctionCode
        End Get
    End Property

    Public ReadOnly Property PlaceOfVaccination() As String
        Get
            Return Me.Request.Form(Me.ddlPlaceOfVaccination.UniqueID)
        End Get
    End Property

    Public ReadOnly Property Category() As String
        Get
            If Me.rbCategorySelection.Visible Then
                If Me.rbCategorySelection.SelectedItem Is Nothing Then
                    Return String.Empty
                Else
                    Return Me.rbCategorySelection.SelectedItem.Value
                End If
            Else
                Return Me.lblCategory.Attributes("SelectedValue")
            End If
        End Get
    End Property

    Public ReadOnly Property EHSVSSTransaction() As EHSTransactionModel
        Get
            Return MyBase.EHSTransaction
        End Get
    End Property

    Public ReadOnly Property AvaibleSubsidy() As Boolean
        Get
            Dim blnAvaibleSubsidy As Boolean = False
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In MyBase.EHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.Available Then
                    blnAvaibleSubsidy = True
                End If
            Next
            Return blnAvaibleSubsidy
        End Get
    End Property

    Public ReadOnly Property HighRiskOptionShown() As String
        Get
            Dim strShowHighRiskOption As String = SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput

            'Check High Risk Option MinDate
            Dim strMinDate As String = String.Empty
            Dim dtmMinDate As Date
            Me._udtGeneralFunction.getSystemParameter("HighRiskOptionDateBackClaimMinDate", strMinDate, String.Empty, MyBase.SchemeClaim.SchemeCode)

            dtmMinDate = Convert.ToDateTime(strMinDate)

            If MyBase.ServiceDate < dtmMinDate Then
                Return strShowHighRiskOption
            End If

            'Check subsidize whether show the High Risk Option
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In MyBase.EHSClaimVaccine.SubsidizeList
                Select Case udtEHSClaimSubsidize.HighRiskOption
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput
                        strShowHighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput
                        Exit For
                    Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk
                        strShowHighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk
                End Select
            Next

            Return strShowHighRiskOption
        End Get
    End Property

    Public ReadOnly Property HighRiskOptionEnabled() As Boolean
        Get
            Return Me.rblRecipientCondition.Enabled
        End Get
    End Property

    Public ReadOnly Property HighRisk() As String
        Get
            If Me.rblRecipientCondition.Visible Then
                If Me.rblRecipientCondition.SelectedItem Is Nothing Then
                    Return String.Empty
                Else
                    If Me.rblRecipientCondition.SelectedItem.Value = HighRiskOption.HIGHRISK Then
                        Return YesNo.Yes
                    ElseIf Me.rblRecipientCondition.SelectedItem.Value = HighRiskOption.NOHIGHRISK Then
                        Return YesNo.No
                    End If

                    Return String.Empty
                End If
            Else
                Return IIf(Me.chkRecipientCondition.Checked, YesNo.Yes, YesNo.No)
            End If
        End Get
    End Property
#End Region

#Region "Event handlers"
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SearchOutreachClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.udcClaimVaccineInputVSS.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputVSS.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputVSS.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputVSS.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputVSS.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputVSS.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputVSS.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputVSS.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.udcClaimVaccineInputVSS.SubsidizeDisableDetail = Me.GetGlobalResourceObject("Text", "Detail")
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")

        'PlaceOfVaccination
        Me.lblPlaceOfVaccinationTitle.Text = Me.GetGlobalResourceObject("Text", "PlaceOfVaccination")
    End Sub

    Protected Overrides Sub Setup()
        Setup(False)
    End Sub

    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)

        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter

        Dim strLanguage As String = MyBase.SessionHandler.Language()
        Dim updateByTransactionModel As Boolean = False
        Dim strEnableClaimCategory As String = Nothing
        Dim noCategory As Boolean = False
        Dim blnVaccineCreate As Boolean = False

        SessionHandler.ClaimCategoryGetFromSession(FunctionCode)

        If MyBase.ClaimCategorys Is Nothing OrElse MyBase.ClaimCategorys.Count = 0 Then
            Me.panVSSCategory.Visible = False
            Me.ClearClaimDetail()
            Me.rbCategorySelection.Items.Clear()
            noCategory = True
        Else
            Me.panVSSCategory.Visible = True
        End If

        If Not noCategory Then
            Me.panVSSCategory.Visible = True

            Me.BindCategory(strLanguage, updateByTransactionModel, MyBase.ClaimCategorys)

            Me.ucInputVSSDA.BindDocumentaryProof(MyBase.EHSClaimVaccine, MyBase.ServiceDate)
            Me.ucInputVSSPID.BindDocumentaryProof(MyBase.EHSClaimVaccine)
            Me.ucInputVSSCOVID19.ServiceDate = MyBase.ServiceDate

            If Not MyBase.EHSClaimVaccine Is Nothing Then
                Me.udcClaimVaccineInputVSS.ShowLegend = MyBase.ShowLegend
                If Not blnPostbackRebuild Then
                    'Me.udcClaimVaccineInputVSS.Controls.Clear()
                    Me.udcClaimVaccineInputVSS.Build(MyBase.EHSClaimVaccine, MyBase.FunctionCode)
                    blnVaccineCreate = True
                End If
            Else
                Me.udcClaimVaccineInputVSS.Controls.Clear()
            End If

            'If non clinic, show dropdown of "Place of Vaccination"
            If Me.NonClinic Then
                BindPlaceOfVaccinationDropDown(MyBase.EHSClaimVaccine)
            End If

            If Not MyBase.EHSClaimVaccine Is Nothing AndAlso MyBase.EHSClaimVaccine.SubsidizeList.Count > 0 Then

                'If no avaiblie subsidy for claim, the drop-down list is not enabled.
                If AvaibleSubsidy() Then
                    Me.ucInputVSSDA.EnableDocumentaryProof(True)
                    Me.ucInputVSSPID.EnableDocumentaryProof(True)
                Else
                    Me.ucInputVSSDA.EnableDocumentaryProof(False)
                    Me.ucInputVSSPID.EnableDocumentaryProof(False)
                End If

                'If turned on high risk option, the high risk option is shown.
                If HighRiskOptionShown() = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                    panVSSRecipientCondition.Visible = True
                    BindRecipientCondition()
                Else
                    panVSSRecipientCondition.Visible = False
                End If

            End If

            'Load selected options from model if saved
            If Not MyBase.EHSTransaction Is Nothing AndAlso New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(MyBase.EHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VSS _
                AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.Count > 0 Then
                updateByTransactionModel = True

                If updateByTransactionModel Then
                    'Category
                    Me.rbCategorySelection.SelectedValue = Me.EHSTransaction.CategoryCode

                    Dim strSelectedCategoryCode As String = String.Empty
                    If Me.rbCategorySelection.Visible Then
                        strSelectedCategoryCode = Me.rbCategorySelection.SelectedValue
                    End If

                    If Me.lblCategory.Visible Then
                        strSelectedCategoryCode = lblCategory.Attributes("SelectedValue")
                    End If

                    showCategoryDetail(strSelectedCategoryCode)

                    Select Case mvCategory.ActiveViewIndex
                        Case ViewIndexCategory.VSS_DA
                            Me.ucInputVSSDA.SetDocumentaryProofOptions(Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof).AdditionalFieldValueCode)

                        Case ViewIndexCategory.VSS_PID
                            Dim strDocumentaryProofValue As String = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof).AdditionalFieldValueCode

                            If strDocumentaryProofValue = ucInputVSSPID.PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
                                Dim strPIDInstitutionCode As String = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PIDInstitutionCode).AdditionalFieldValueCode
                                Me.ucInputVSSPID.SetDocumentaryProofOptions(strDocumentaryProofValue, strPIDInstitutionCode)
                            Else
                                Me.ucInputVSSPID.SetDocumentaryProofOptions(strDocumentaryProofValue)
                            End If

                        Case ViewIndexCategory.VSS_COVID19
                            Me.ucInputVSSCOVID19.EHSTransaction = MyBase.EHSTransaction
                            Me.ucInputVSSCOVID19.SetupContent(True)
                    End Select

                    If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) AndAlso mvCategory.ActiveViewIndex <> ViewIndexCategory.VSS_COVID19 Then
                        Dim strPlaceVaccinationValue As String = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination).AdditionalFieldValueCode
                        Me.ddlPlaceOfVaccination.SelectedValue = strPlaceVaccinationValue

                        If strPlaceVaccinationValue = PlaceOfVaccinationOptions.OTHERS Then
                            trPlaceOfVaccinationOther.Style.Add("display", "initial")
                            txtPlaceOfVaccinationOther.Text = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination).AdditionalFieldValueDesc
                        Else
                            trPlaceOfVaccinationOther.Style.Add("display", "none")
                        End If
                    End If

                    'If Not blnPostbackRebuild And Not blnVaccineCreate And Not Session(claimTransManagement.SESS_OverrideReasonPopup) And Not Session(claimTransManagement.SESS_VaccinationRecordPopupShown) Then
                    '    'Me.udcClaimVaccineInputVSS.Controls.Clear()
                    '    Me.udcClaimVaccineInputVSS.Build(MyBase.EHSClaimVaccine, MyBase.FunctionCode)
                    'End If
                    udcClaimVaccineInputVSS.Visible = True
                End If
            End If

            'Fill data
            If Not IsPostBack Or _udtSessionHandler.ChangeSchemeInPracticeGetFromSession(FunctCode) Then
                FillClaimDetail()
                _udtSessionHandler.ChangeSchemeInPracticeSaveToSession(False, FunctCode)
            End If

            'Hide/Show Claim Detail
            If Not MyBase.EHSClaimVaccine Is Nothing AndAlso (Me.rbCategorySelection.SelectedValue <> String.Empty And Me.rbCategorySelection.Visible) Or lblCategory.Visible Then

                Dim strSelectedCategoryCode As String = String.Empty
                If Me.rbCategorySelection.Visible Then
                    strSelectedCategoryCode = Me.rbCategorySelection.SelectedValue
                End If

                If Me.lblCategory.Visible Then
                    strSelectedCategoryCode = lblCategory.Attributes("SelectedValue")
                End If

                If Me.panVSSRecipientCondition.Visible Then
                    'Category is selected and High risk option is shown

                    'Category List
                    showCategoryDetail(Me.Category)

                    'For add Event Handler for sub input control
                    Select Case Me.Category
                        Case CategoryCode.VSS_PID
                            RemoveHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick
                            AddHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick

                        Case CategoryCode.VSS_COVID19_Outreach
                            RemoveHandler ucInputVSSCOVID19.SearchButtonClick, AddressOf udcInputVSSCOVID19_SearchOutreachClick
                            AddHandler ucInputVSSCOVID19.SearchButtonClick, AddressOf udcInputVSSCOVID19_SearchOutreachClick

                    End Select

                    'Place of Vaccination
                    If Me.NonClinic AndAlso mvCategory.ActiveViewIndex <> ViewIndexCategory.VSS_COVID19 Then
                        Me.panVSSPlaceOfVaccination.Visible = True
                    End If

                    'Subsidize Claim Detail
                    Me.udcClaimVaccineInputVSS.Visible = True

                    AddHandler Me.udcClaimVaccineInputVSS.VaccineLegendClicked, AddressOf udcClaimVaccineInputVSS_VaccineLegendClicked
                    AddHandler Me.udcClaimVaccineInputVSS.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputVSS_SubsidizeDisabledRemarkClicked

                    'Recipient Condition
                    Me.rblRecipientCondition.Enabled = False

                    If Not MyBase.EHSClaimVaccine Is Nothing AndAlso MyBase.EHSClaimVaccine.IsSelectedSubsidizeWithHighRisk Then
                        Me.rblRecipientCondition.Enabled = True
                        SetHighRisk(SessionHandler.HighRiskGetFromSession(FunctCode))
                    End If

                    AddHandler Me.udcClaimVaccineInputVSS.SubsidizeCheckboxClickedEnableRecipientCondition, AddressOf udcClaimVaccineInputVSS_SubsidizeCheckboxClickedEnableRecipientCondition
                    'End If

                Else
                    'Category is selected but no High risk option and so show claim detail 
                    showCategoryDetail(strSelectedCategoryCode)
                    Me.panVSSPlaceOfVaccination.Visible = False
                    Me.udcClaimVaccineInputVSS.Visible = True

                    If Me.NonClinic AndAlso mvCategory.ActiveViewIndex <> ViewIndexCategory.VSS_COVID19 Then
                        Me.panVSSPlaceOfVaccination.Visible = True
                    End If

                    Select Case strSelectedCategoryCode
                        Case CategoryCode.VSS_PID
                            RemoveHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick
                            AddHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick

                        Case CategoryCode.VSS_COVID19_Outreach
                            RemoveHandler ucInputVSSCOVID19.SearchButtonClick, AddressOf udcInputVSSCOVID19_SearchOutreachClick
                            AddHandler ucInputVSSCOVID19.SearchButtonClick, AddressOf udcInputVSSCOVID19_SearchOutreachClick

                    End Select

                    AddHandler Me.udcClaimVaccineInputVSS.VaccineLegendClicked, AddressOf udcClaimVaccineInputVSS_VaccineLegendClicked
                    AddHandler Me.udcClaimVaccineInputVSS.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputVSS_SubsidizeDisabledRemarkClicked
                End If
            Else
                Me.mvCategory.ActiveViewIndex = 0
                Me.panVSSPlaceOfVaccination.Visible = False
                Me.udcClaimVaccineInputVSS.Visible = False
                Me.panVSSRecipientCondition.Visible = False
            End If

        End If

    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        If Not Me.udcClaimVaccineInputVSS Is Nothing Then
            Me.udcClaimVaccineInputVSS.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetPlaceOfVaccinationError(ByVal visible As Boolean)
        Me.imgPlaceOfVaccinationError.Visible = visible
        Me.imgPlaceOfVaccinationErrorOther.Visible = visible
    End Sub

    Public Sub SetCategoryError(ByVal visible As Boolean)
        Me.imgCategoryError.Visible = visible
    End Sub

    Public Sub SetRecipientConditionError(ByVal visible As Boolean)
        Me.imgRecipientConditionError.Visible = visible
    End Sub

    Public Sub SetCOVID19DetailError(ByVal blnVisible As Boolean)
        ucInputVSSCOVID19.SetDetailError(blnVisible)
    End Sub

#End Region

#Region "SetValue"

    Public Sub SetPIDCode(ByVal strPIDCode As String)
        Dim ucInputVSSPID As ucInputVSSPID = CType(Me.FindControl(CategoryControlID.PID), ucInputVSSPID)

        ucInputVSSPID.SetPIDCode(strPIDCode)
    End Sub

    Public Sub SetHighRisk(ByVal strHighRisk As String)
        If strHighRisk = String.Empty Then Return

        If Me.chkRecipientCondition.Visible Then
            Me.chkRecipientCondition.Checked = IIf(strHighRisk = YesNo.Yes, True, False)
        End If

        If Me.rblRecipientCondition.Visible Then
            Dim strSelectedValue As String = String.Empty

            Select Case strHighRisk
                Case YesNo.Yes
                    strSelectedValue = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strSelectedValue = HighRiskOption.NOHIGHRISK
            End Select

            For i As Integer = 0 To Me.rblRecipientCondition.Items.Count - 1
                If Me.rblRecipientCondition.Items(i).Value = strSelectedValue Then
                    Me.rblRecipientCondition.Items(i).Selected = True
                Else
                    Me.rblRecipientCondition.Items(i).Selected = False
                End If
            Next
        End If
    End Sub

    Public Sub SetOutreachCode(ByVal strOutreachCode As String)
        Dim ucInputVSSCOVID19 As ucInputVSSCOVID19 = CType(Me.FindControl(CategoryControlID.VSSCOVID19), ucInputVSSCOVID19)

        ucInputVSSCOVID19.SetOutreachCode(strOutreachCode)
    End Sub

    Public Sub DisplayOutreachInput(ByVal blnDisplay As Boolean)
        ucInputVSSCOVID19.DisplayOutreachInput(blnDisplay)
    End Sub

    Public Sub FillClaimDetail()

        'Select Case mvCategory.ActiveViewIndex
        '    Case ViewIndexCategory.VSS_PID
        'If Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode) Is Nothing And _
        '    Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode) = String.Empty Then

        'Dim strDocumentaryProofValue As String = _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode)

        '    If strDocumentaryProofValue = ucInputVSSPID.PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
        If Not _udtSessionHandler.PIDInstitutionCodeGetFromSession(FunctCode) Is Nothing And _
            Not _udtSessionHandler.PIDInstitutionCodeGetFromSession(FunctCode) = String.Empty Then

            Dim strPIDInstitutionCode As String = _udtSessionHandler.PIDInstitutionCodeGetFromSession(FunctCode)
            Me.ucInputVSSPID.SetPIDCode(strPIDInstitutionCode)
            'Else
            '    Me.ucInputVSSPID.SetDocumentaryProofOptions(strDocumentaryProofValue)
        End If
        '    Else
        'Me.ucInputVSSPID.SetDocumentaryProofOptions(strDocumentaryProofValue)
        '    End If
        'End If
        'End Select

        If ddlPlaceOfVaccination.Enabled Then
            If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
                If Not _udtSessionHandler.PlaceVaccinationGetFromSession(FunctCode) Is Nothing And _
                        Not _udtSessionHandler.PlaceVaccinationGetFromSession(FunctCode) = String.Empty Then
                    Dim strPlaceVaccinationValue As String = _udtSessionHandler.PlaceVaccinationGetFromSession(FunctCode)
                    Me.ddlPlaceOfVaccination.SelectedValue = strPlaceVaccinationValue

                    If strPlaceVaccinationValue = PlaceOfVaccinationOptions.OTHERS Then
                        If Not _udtSessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode) Is Nothing And _
                        Not _udtSessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode) = String.Empty Then
                            txtPlaceOfVaccinationOther.Text = _udtSessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode)
                            trPlaceOfVaccinationOther.Style.Add("display", "initial")
                        Else
                            trPlaceOfVaccinationOther.Style.Add("display", "none")
                        End If
                    Else
                        trPlaceOfVaccinationOther.Style.Add("display", "none")
                    End If
                End If
            End If
        Else
            Me.ddlPlaceOfVaccination.SelectedIndex = -1
            txtPlaceOfVaccinationOther.Text = String.Empty
            trPlaceOfVaccinationOther.Style.Add("display", "none")
        End If

    End Sub

    Public Sub ClearClaimDetail()
        Me.rbCategorySelection.SelectedIndex = -1
        Me.rbCategorySelection.SelectedValue = Nothing
        Me.rbCategorySelection.ClearSelection()
        Me.mvCategory.ActiveViewIndex = -1

        Me.ucInputVSSDA.SetDocumentaryProofOptions(String.Empty)

        Me.ucInputVSSPID.SetDocumentaryProofOptions(String.Empty)

        Me.InitialCOVID19ClaimDetail()

        Me.ddlPlaceOfVaccination.SelectedIndex = -1
        Me.ddlPlaceOfVaccination.SelectedValue = Nothing
        Me.ddlPlaceOfVaccination.ClearSelection()
        Me.panVSSPlaceOfVaccination.Visible = False
        trPlaceOfVaccinationOther.Style.Add("display", "none")
        txtPlaceOfVaccinationOther.Text = String.Empty

        Me.chkRecipientCondition.Checked = False
        Me.rblRecipientCondition.SelectedIndex = -1
        Me.rblRecipientCondition.SelectedValue = Nothing
        Me.rblRecipientCondition.ClearSelection()

    End Sub

    Public Sub ClearClaimDetailWithoutCategory()
        Me.ucInputVSSDA.SetDocumentaryProofOptions(String.Empty)

        Me.ucInputVSSPID.SetDocumentaryProofOptions(String.Empty)

        Me.ddlPlaceOfVaccination.SelectedIndex = -1
        Me.ddlPlaceOfVaccination.SelectedValue = Nothing
        Me.ddlPlaceOfVaccination.ClearSelection()
        trPlaceOfVaccinationOther.Style.Add("display", "none")
        txtPlaceOfVaccinationOther.Text = String.Empty
    End Sub

    Public Sub InitialCOVID19ClaimDetail()
        Me.ucInputVSSCOVID19.InitialClaimDetail()
    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputVSS_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub

    Protected Sub udcClaimVaccineInputVSS_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    Private Sub udcInputVSSPID_SearchPIDClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent SearchButtonClick(sender, e)
    End Sub

    Private Sub udcInputVSSCOVID19_SearchOutreachClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent SearchOutreachClick(sender, e)
    End Sub

    Private Sub rbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged
        SetPlaceOfVaccinationError(False)
        ucInputVSSPID.SetDocumentaryProofError(False)
        ucInputVSSPID.SetPIDCodeError(False)
        ucInputVSSDA.SetDocumentaryProofError(False)
        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        SetCOVID19DetailError(False)
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        Me.rbCategorySelection.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rbCategorySelection.UniqueID), True)

        Me.udcClaimVaccineInputVSS.Controls.Clear()
        Me.ClearClaimDetailWithoutCategory()

        ' Claim Detail Input by Category
        showCategoryDetail(Me.rbCategorySelection.SelectedValue)

        If String.IsNullOrEmpty(Me.rbCategorySelection.SelectedValue) Then
            Me.udcClaimVaccineInputVSS.Visible = False
        Else
            Me.udcClaimVaccineInputVSS.Visible = True
        End If

        RaiseEvent CategorySelected(sender, e)

    End Sub

    Private Sub chkRecipientCondition_CheckedChanged(sender As Object, e As EventArgs) Handles chkRecipientCondition.CheckedChanged
        Me.chkRecipientCondition.Checked = IIf(AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.chkRecipientCondition.UniqueID), True) = "on", True, False)

        Dim strHighRisk As String = IIf(Me.chkRecipientCondition.Checked = True, YesNo.Yes, YesNo.No)
        Me._udtSessionHandler.HighRiskSaveToSession(strHighRisk, FunctCode)

    End Sub

    Private Sub rblRecipientCondition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblRecipientCondition.SelectedIndexChanged
        Me.rblRecipientCondition.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rblRecipientCondition.UniqueID), True)

        Me._udtSessionHandler.HighRiskSaveToSession(IIf(Me.rblRecipientCondition.SelectedValue = HighRiskOption.HIGHRISK, YesNo.Yes, YesNo.No), FunctCode)

    End Sub

    Protected Sub udcClaimVaccineInputVSS_SubsidizeCheckboxClickedEnableRecipientCondition(ByVal blnEnabled As Boolean)
        Dim blnEnabledAsBefore As Boolean = Me.rblRecipientCondition.Enabled

        Me.rblRecipientCondition.Enabled = blnEnabled

        If Me.rblRecipientCondition.Enabled Then
            'Enabled
            If Not blnEnabledAsBefore Then
                SessionHandler.HighRiskRemoveFromSession(MyBase.FunctionCode)
            End If

            SetHighRisk(SessionHandler.HighRiskGetFromSession(FunctCode))
        Else
            'Disabled
            Me.rblRecipientCondition.SelectedIndex = -1
            Me.rblRecipientCondition.SelectedValue = Nothing
            Me.rblRecipientCondition.ClearSelection()

        End If
    End Sub

    Private Sub showCategoryDetail(ByVal strSelectedCategory As String)

        Select Case strSelectedCategory
            Case CategoryCode.VSS_PW
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_PW
            Case CategoryCode.VSS_CHILD
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_CHILD
            Case CategoryCode.VSS_ADULT
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_ADULT
            Case CategoryCode.VSS_ELDER
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_ELDER
            Case CategoryCode.VSS_PID
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_PID
            Case CategoryCode.VSS_DA
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_DA
                ' CRE20-0023 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case CategoryCode.VSS_COVID19
                Me.ucInputVSSCOVID19.NonClinic = False
                Me.ucInputVSSCOVID19.CategoryCode = Category.Trim
                Me.ucInputVSSCOVID19.DisplayOutreachInput(False)
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_COVID19
            Case CategoryCode.VSS_COVID19_Outreach
                Me.ucInputVSSCOVID19.NonClinic = MyBase.NonClinic
                Me.ucInputVSSCOVID19.CategoryCode = Category.Trim
                Me.ucInputVSSCOVID19.DisplayOutreachInput(MyBase.NonClinic)
                mvCategory.ActiveViewIndex = ViewIndexCategory.VSS_COVID19
                ' CRE20-0023 (Immu record) [End][Chris YIM]
            Case Else
                mvCategory.ActiveViewIndex = ViewIndexCategory.NoCategory
        End Select

    End Sub

    Private Sub ddlPlaceOfVaccination_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaceOfVaccination.SelectedIndexChanged
        Dim ddlPlaceOfVaccination As DropDownList = CType(sender, DropDownList)

        If Not ddlPlaceOfVaccination Is Nothing Then
            If ddlPlaceOfVaccination.SelectedValue.Trim = PlaceOfVaccinationOptions.OTHERS Then
                trPlaceOfVaccinationOther.Style.Add("display", "initial")
                'If Not SessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode) Is Nothing Then
                '    txtPlaceOfVaccinationOther.Text = SessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode)
                'End If
            Else
                trPlaceOfVaccinationOther.Style.Add("display", "none")
            End If

        End If

        Dim strSelectedCategoryCode As String = String.Empty
        If Me.rbCategorySelection.Visible Then
            strSelectedCategoryCode = Me.rbCategorySelection.SelectedValue
        End If

        If Me.lblCategory.Visible Then
            strSelectedCategoryCode = lblCategory.Attributes("SelectedValue")
        End If

        showCategoryDetail(strSelectedCategoryCode)

    End Sub
#End Region

#Region "PlaceOfVaccination"

    Private Sub BindPlaceOfVaccinationDropDown(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim udtStaticDataBLL As New StaticDataBLL

        ' Dim dtCoPaymentOption As DataTable
        Dim udtStaticDataModelCollection As StaticDataModelCollection
        Dim intSelectedPlaceOfVaccinationOption As Integer = -1
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem
        Dim blnEnableDropDownListPlaceOfVaccination As Boolean = False

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("VSS_PLACEOFVACCINATION")

        ' Save the User Input before clear it
        If Me.ddlPlaceOfVaccination.SelectedIndex > -1 AndAlso String.IsNullOrEmpty(Me.ddlPlaceOfVaccination.SelectedValue) = False Then
            intSelectedPlaceOfVaccinationOption = ddlPlaceOfVaccination.SelectedIndex
        End If

        ddlPlaceOfVaccination.Items.Clear()

        ddlPlaceOfVaccination.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

        For Each udtStaticData As StaticDataModel In udtStaticDataModelCollection
            listItem = New ListItem

            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                listItem.Text = udtStaticData.DataValueChi.ToString
            ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                listItem.Text = udtStaticData.DataValueCN.ToString
            Else
                listItem.Text = udtStaticData.DataValue.ToString
            End If

            listItem.Value = udtStaticData.ItemNo

            ddlPlaceOfVaccination.Items.Add(listItem)
        Next


        ' Restore the User Input after clear it
        ddlPlaceOfVaccination.SelectedIndex = intSelectedPlaceOfVaccinationOption

        'If no avaiblie subsidy for claim, the drop-down list is not enabled.
        If Not udtEHSClaimVaccine Is Nothing AndAlso udtEHSClaimVaccine.SubsidizeList.Count > 0 Then
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                blnEnableDropDownListPlaceOfVaccination = blnEnableDropDownListPlaceOfVaccination Or udtEHSClaimSubsidize.Available
            Next

            If blnEnableDropDownListPlaceOfVaccination Then
                ddlPlaceOfVaccination.Enabled = True
                txtPlaceOfVaccinationOther.Enabled = True
            Else
                ddlPlaceOfVaccination.Enabled = False
                ddlPlaceOfVaccination.SelectedIndex = -1
                txtPlaceOfVaccinationOther.Enabled = False
            End If
        End If

    End Sub

#End Region

#Region "RecipientCondition"

    Public Sub BindRecipientCondition()
        Dim udtStaticDataBLL As New StaticDataBLL

        Dim udtStaticDataModelCollection As StaticDataModelCollection
        Dim lstCheckboxListResult As List(Of Boolean) = Nothing
        Dim blnCheckedRecipientConditionOption As Boolean = False
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem
        Dim blnEnableRecipientCondition As Boolean = False

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("VSS_RECIPIENTCONDITION")

        If udtStaticDataModelCollection.Count > 0 Then
            If udtStaticDataModelCollection.Count > 1 Then
                Me.rblRecipientCondition.Visible = True
                Me.chkRecipientCondition.Visible = False

                rblRecipientCondition.Items.Clear()

                For Each udtStaticData As StaticDataModel In udtStaticDataModelCollection
                    listItem = New ListItem

                    If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                        listItem.Text = udtStaticData.DataValueChi.ToString
                    ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                        listItem.Text = udtStaticData.DataValueCN.ToString
                    Else
                        listItem.Text = udtStaticData.DataValue.ToString
                    End If

                    listItem.Value = udtStaticData.ItemNo

                    rblRecipientCondition.Items.Add(listItem)
                Next

            Else
                Me.rblRecipientCondition.Visible = False
                Me.chkRecipientCondition.Visible = True

                Dim udtStaticData As StaticDataModel = udtStaticDataModelCollection.Item(0)

                If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                    chkRecipientCondition.Text = udtStaticData.DataValueChi.ToString
                ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                    chkRecipientCondition.Text = udtStaticData.DataValueCN.ToString
                Else
                    chkRecipientCondition.Text = udtStaticData.DataValue.ToString
                End If

            End If
        End If

    End Sub

#End Region

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = _udtSessionHandler.EHSClaimVaccineGetFromSession(FunctCode)
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        'Check Category
        If String.IsNullOrEmpty(Me.Category) Then
            blnResult = False

            Me.SetCategoryError(True)

            objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00238)
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
        End If

        'Check Place of Vaccination
        If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) AndAlso mvCategory.ActiveViewIndex <> ViewIndexCategory.VSS_COVID19 Then
            objMsg = ValidatePlaceOfVaccination(blnShowErrorImage)
            If objMsg IsNot Nothing Then
                blnResult = False

                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            End If
        End If

        'Check Claim detail
        Select Case mvCategory.ActiveViewIndex
            Case ViewIndexCategory.VSS_DA
                blnResult = blnResult And Me.ucInputVSSDA.Validate(blnShowErrorImage, objMsgBox)
            Case ViewIndexCategory.VSS_PID
                blnResult = blnResult And Me.ucInputVSSPID.Validate(blnShowErrorImage, objMsgBox)
                ' CRE20-0023 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case ViewIndexCategory.VSS_COVID19
                blnResult = blnResult And Me.ucInputVSSCOVID19.Validate(blnShowErrorImage, objMsgBox)
                ' CRE20-0023 (Immu record) [End][Chris YIM]
        End Select

        'Check High Risk option if turned on 
        If HighRiskOptionShown = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
            'Check Recipient Condition whether it is contained invalid setting
            Dim blnManualInput As Boolean = False
            Dim blnAutoInput As Boolean = False
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                    blnManualInput = True
                End If
                If udtEHSClaimSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk Then
                    blnAutoInput = True
                End If
            Next

            If blnManualInput And blnAutoInput Then
                Throw New Exception("Subsidies are included different High Risk Option [M] and [A] at the same time.")
            End If

            'Check Recipient Condition whether it is nothing to input
            If Me.rblRecipientCondition.Enabled Then
                blnResult = blnResult And ValidateRecipientCondition(blnShowErrorImage, objMsgBox)
            End If
        End If

        'Select Vaccine Part
        Dim udtSMList As Dictionary(Of SystemMessage, List(Of String())) = Nothing

        udtEHSClaimVaccine = SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
        Dim blnVaccineValid As Boolean = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, udtSMList)
        If Not blnVaccineValid Then
            SetDoseErrorImage(True)
        End If

        blnResult = blnResult And blnVaccineValid

        Return blnResult
    End Function

    Public Function ValidatePlaceOfVaccination(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage

        Me.imgPlaceOfVaccinationError.Visible = False

        If String.IsNullOrEmpty(Me.PlaceOfVaccination) = True Then
            Me.imgPlaceOfVaccinationError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00385) ' Please select "Place Of Vaccination"
        End If

        If Me.PlaceOfVaccination = PlaceOfVaccinationOptions.OTHERS Then
            If String.IsNullOrEmpty(Me.txtPlaceOfVaccinationOther.Text) = True Then
                Me.imgPlaceOfVaccinationErrorOther.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00386) ' Please input "Place Of Vaccination"
            End If

            'Transform to byte (4 byte for each char)
            Dim bytChiName As Byte() = System.Text.Encoding.UTF32.GetBytes(Me.txtPlaceOfVaccinationOther.Text)
            If bytChiName.Length > 1020 Then 'Maximum 255 char
                Me.imgPlaceOfVaccinationErrorOther.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00390) ' "Place Of Vaccination" is invalid
            End If
        End If

        Return Nothing
    End Function

    Public Function ValidateRecipientCondition(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim blnResult As Boolean = True

        Me.imgRecipientConditionError.Visible = False

        If String.IsNullOrEmpty(HighRisk()) = True Then
            blnResult = False
            Me.imgRecipientConditionError.Visible = blnShowErrorImage
            objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00397)) ' Please select "Recipient Condition"
        End If

        Return blnResult
    End Function

#End Region

#Region "Save"

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        'Category
        If rbCategorySelection.Visible And rbCategorySelection.SelectedValue <> String.Empty Then
            udtEHSTransaction.CategoryCode = rbCategorySelection.SelectedValue.Trim
        End If

        If lblCategory.Visible And lblCategory.Attributes("SelectedValue") <> String.Empty Then
            udtEHSTransaction.CategoryCode = lblCategory.Attributes("SelectedValue")
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Select Case Me.HighRiskOptionShown
            Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput 'Manual Input
                If Me.rblRecipientCondition.Enabled Then
                    udtEHSTransaction.HighRisk = Me.HighRisk()
                Else
                    udtEHSTransaction.HighRisk = String.Empty
                End If
            Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'Auto Input
                udtEHSTransaction.HighRisk = String.Empty
            Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideWithoutInput 'No Input
                udtEHSTransaction.HighRisk = String.Empty
            Case Else
                udtEHSTransaction.HighRisk = String.Empty
        End Select
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        'Each Category Claim Detail
        Select Case mvCategory.ActiveViewIndex
            Case ViewIndexCategory.VSS_DA
                'Documentary Proof
                Me.ucInputVSSDA.Save(udtEHSTransaction, udtEHSClaimVaccine)
            Case ViewIndexCategory.VSS_PID
                'Documentary Proof, with/without PID Code
                Me.ucInputVSSPID.Save(udtEHSTransaction, udtEHSClaimVaccine)
            Case ViewIndexCategory.VSS_COVID19
                'Outreach Code
                Me.ucInputVSSCOVID19.Save(udtEHSTransaction, udtEHSClaimVaccine)
        End Select

        ' -----------------------------------------------
        ' Get Latest SchemeSeq Selected
        '------------------------------------------------
        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

        If Not udtSubsidizeLatest Is Nothing Then
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType
            udtTransactAdditionfield.AdditionalFieldValueCode = IIf(_udtSessionHandler.NonClinicSettingGetFromSession(FunctCode), ClinicType.NonClinic, ClinicType.Clinic)
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Place Of Vaccination
            If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) AndAlso Me.PlaceOfVaccination <> String.Empty AndAlso mvCategory.ActiveViewIndex <> ViewIndexCategory.VSS_COVID19 Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination
                udtTransactAdditionfield.AdditionalFieldValueCode = Me.PlaceOfVaccination
                udtTransactAdditionfield.AdditionalFieldValueDesc = IIf(Me.PlaceOfVaccination = PlaceOfVaccinationOptions.OTHERS, txtPlaceOfVaccinationOther.Text, Nothing)
                udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                '_udtSessionHandler.PlaceVaccinationSaveToSession(Me.PlaceOfVaccination, FunctCode)
                'If Me.PlaceOfVaccination = PlaceOfVaccinationOptions.OTHERS Then
                '    _udtSessionHandler.PlaceVaccinationOtherSaveToSession(txtPlaceOfVaccinationOther.Text, FunctCode)
                'Else
                '    _udtSessionHandler.PlaceVaccinationOtherRemoveFromSession(FunctCode)
                'End If

            End If
        End If



    End Sub

#End Region

#Region "Control Binding"

    Private Sub BindCategory(ByVal strLanguage As String, ByVal updateByTransactionModel As Boolean, ByVal udtClaimCategorys As ClaimCategoryModelCollection)
        Dim strSelectedValue As String
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctCode)
        Dim dtClaimCategory As DataTable

        Me.rbCategorySelection.Visible = False
        Me.lblCategory.Visible = False

        If Not udtClaimCategory Is Nothing Then
            strSelectedValue = udtClaimCategory.CategoryCode
        Else
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
            strSelectedValue = AntiXssEncoder.HtmlEncode(Me.rbCategorySelection.SelectedValue, True)
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
        End If

        'Check selected category whether exists when practice is changed
        Dim udtSelectedCategory As ClaimCategoryModel = udtClaimCategorys.Filter(strSelectedValue)
        If udtSelectedCategory Is Nothing Then
            strSelectedValue = String.Empty
            Me.rbCategorySelection.Items.Clear()
            Me.rbCategorySelection.SelectedIndex = -1
            Me.rbCategorySelection.SelectedValue = Nothing
            Me.rbCategorySelection.ClearSelection()
            showCategoryDetail(strSelectedValue)
            SessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
        End If

        'Build radio button List
        dtClaimCategory = ClaimCategoryBLL.ConvertCategoryToDatatable(udtClaimCategorys)

        If dtClaimCategory.Rows.Count > 1 Then
            Me.rbCategorySelection.Visible = True
            Me.rbCategorySelection.DataSource = dtClaimCategory

            Me.rbCategorySelection.DataValueField = ClaimCategoryModel._Category_Code

            If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.rbCategorySelection.DataTextField = ClaimCategoryModel._Category_Name_Chi
            Else
                Me.rbCategorySelection.DataTextField = ClaimCategoryModel._Category_Name
            End If

            Me.rbCategorySelection.DataBind()
            Me.rbCategorySelection.ClearSelection()

            If updateByTransactionModel Then
                strSelectedValue = MyBase.EHSTransaction.CategoryCode
            End If

            If Not String.IsNullOrEmpty(strSelectedValue) _
                AndAlso Not Me.rbCategorySelection.Items.FindByValue(strSelectedValue) Is Nothing Then

                Me.rbCategorySelection.SelectedValue = strSelectedValue
                showCategoryDetail(strSelectedValue)
                Me.udcClaimVaccineInputVSS.Visible = True
            Else
                Me.udcClaimVaccineInputVSS.Visible = False
            End If

        ElseIf dtClaimCategory.Rows.Count > 0 Then
            Me.lblCategory.Visible = True
            Me.lblCategory.Attributes("SelectedValue") = dtClaimCategory.Rows(0)(ClaimCategoryModel._Category_Code)

            If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.lblCategory.Text = dtClaimCategory.Rows(0)(ClaimCategoryModel._Category_Name_Chi)
            ElseIf MyBase.SessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                Me.lblCategory.Text = dtClaimCategory.Rows(0)(ClaimCategoryModel._Category_Name_CN)
            Else
                Me.lblCategory.Text = dtClaimCategory.Rows(0)(ClaimCategoryModel._Category_Name)
            End If

            Me.udcClaimVaccineInputVSS.Visible = True
        End If

    End Sub

#End Region

End Class
