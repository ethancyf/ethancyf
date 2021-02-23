Imports Common
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory
Imports HCSP.BLL
Imports System.Web.Security.AntiXss
Imports System.Web.Script.Serialization
Imports System.Linq


Partial Public Class ucInputVSS
    Inherits ucInputEHSClaimBase

    Public Shared FunctCode As String = (New BLL.SessionHandler).ClaimFunctCodeGetFromSession()
    'Public Const functcode As String = Common.Component.FunctCode.FUNT020201

    Dim _udtSessionHandler As New SessionHandler
    Dim _udtGeneralFunction As New GeneralFunction

#Region "Constants"

    Private Class ClinicType
        Public Const Clinic As String = "C"
        Public Const NonClinic As String = "N"
    End Class

    Private Class CategoryControlID
        Public Const PID As String = "ucInputVSSPID"
        Public Const DA As String = "ucInputVSSDA"
    End Class

    Private Class ViewIndexCategory
        Public Const NoCategory As Integer = 0
        Public Const VSS_PW As Integer = 1
        Public Const VSS_CHILD As Integer = 2
        Public Const VSS_ELDER As Integer = 3
        Public Const VSS_PID As Integer = 4
        Public Const VSS_DA As Integer = 5
        Public Const VSS_ADULT As Integer = 6
    End Class

    Private Class PlaceOfVaccinationOptions
        Public Const OTHERS As String = "OTHERS"
    End Class

#End Region

#Region "Properties"
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

    Public ReadOnly Property AvaibleSubsidy() As Boolean
        Get
            Dim blnAvaibleSubsidy As Boolean = False
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In MyBase.EHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.Available Then
                    blnAvaibleSubsidy = True
                    Exit For
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

    Public ReadOnly Property EHSVSSTransaction() As EHSTransactionModel
        Get
            Return MyBase.EHSTransaction
        End Get
    End Property

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property IsClaimCOVID19() As Boolean
        Get
            Return (New BLL.SessionHandler).ClaimCOVID19GetFromSession()
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property Booth() As String
        Get
            If Me.ddlCBooth.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCBooth.SelectedItem.Value
            End If
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property CategoryForCOVID19() As String
        Get
            If Me.ddlCCategory.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCCategory.SelectedItem.Value
            End If
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property VaccineBrand() As String
        Get
            If Me.txtCVaccineBrand.Text = "" Then
                Return String.Empty
            Else
                Return Me.txtCVaccineBrand.Text
            End If
            'If Me.ddlCVaccineBrandCovid19.SelectedItem Is Nothing Then
            '    Return String.Empty
            'Else
            '    Return Me.ddlCVaccineBrandCovid19.SelectedItem.Value
            'End If
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

    ' CRE20-0022 (Immu record) [Start][Martin Tang]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property VaccineLotNo() As String
        Get

            If Me.txtCVaccineLotNo.Text = "" Then
                Return String.Empty
            Else
                Return Me.txtCVaccineLotNo.Text
            End If


            'If Me.ddlCVaccineLotNo.SelectedItem Is Nothing Then
            '    Return String.Empty
            'Else
            '    Return Me.ddlCVaccineLotNo.SelectedItem.Value
            'End If
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Martin Tang]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property DoseForCOVID19() As String
        Get
            If Me.ddlCDose.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCDose.SelectedItem.Value
            End If
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Chris YIM]
#End Region

#Region "Event handlers"
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event RecipientConditionHelpClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
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

        If Not Me.IsClaimCOVID19() Then
            panClaim.Visible = True
            panClaimCOVID19.Visible = False

            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strLanguage As String = MyBase.SessionHandler.Language()
            Dim updateByTransactionModel As Boolean = False
            Dim strEnableClaimCategory As String = Nothing
            Dim noCategory As Boolean = False
            Dim strCategoryCode As String = String.Empty

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

                If _udtSessionHandler.ClaimForSamePatientGetFromSession(FunctCode) Then
                    _udtSessionHandler.ClaimForSamePatientSaveToSession(False, FunctCode)
                    Me.rbCategorySelection.SelectedIndex = -1
                    Me.rbCategorySelection.SelectedValue = Nothing
                    Me.rbCategorySelection.ClearSelection()
                    MyBase.EHSTransaction = Nothing
                End If

                Me.BindCategory(strLanguage, updateByTransactionModel, MyBase.ClaimCategorys)

                ' Dim _udcInputEHSClaim As ucInputEHSClaimBase

                'CRE20-009 VSS Disabled with CSSA [Start][Nichole]
                Dim strServiceDate = Me.ServiceDate()
                Me.ucInputVSSDA.BindDocumentaryProof(MyBase.EHSClaimVaccine, strServiceDate)
                'CRE20-009 VSS Disabled with CSSA [End][Nichole]

                ucInputVSSPID.BindDocumentaryProof(MyBase.EHSClaimVaccine)

                If Not MyBase.EHSClaimVaccine Is Nothing Then
                    Me.udcClaimVaccineInputVSS.Build(MyBase.EHSClaimVaccine)
                Else
                    Me.udcClaimVaccineInputVSS.Controls.Clear()
                End If

                'Check VSS Eligible Result, if child is over 12 years and the first dose is avalible for the child
                Dim udtRuleResults As RuleResultCollection
                udtRuleResults = Me._udtSessionHandler.EligibleResultVSSReminderGetFromSession()
                If udtRuleResults Is Nothing Then
                    udtRuleResults = Me._udtSessionHandler.EligibleResultGetFromSession()
                End If

                Me.panReminder.Visible = False
                If Not udtRuleResults Is Nothing Then
                    Dim udtClaimCategorysForCheck As ClaimCategoryModelCollection = New ClaimCategoryBLL().getAllCategoryCache()

                    For Each udtRuleResult As RuleResult In udtRuleResults.Values

                        If udtRuleResult.RuleType = RuleTypeENum.EligibleResult AndAlso udtRuleResult.HandleMethod = HandleMethodENum.Declaration AndAlso udtRuleResult.PromptConfirmed Then

                            If udtRuleResult.SchemeCode.Trim() = CType(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctCode), SchemeClaimModel).SchemeCode.Trim Then

                                Dim udtFilterClaimCategoryList As ClaimCategoryModelCollection = udtClaimCategorysForCheck.Filter(udtRuleResult.SchemeCode, udtRuleResult.SchemeSeq, udtRuleResult.SubsidizeCode)

                                If udtFilterClaimCategoryList(0).CategoryCode = Me.Category Then

                                    If CType(udtRuleResult, EligibleResult).IsEligible Then

                                        Dim udtEligibleResult As EligibleResult = CType(udtRuleResult, EligibleResult)

                                        Dim strObjectName2 As String = String.Empty

                                        If Not udtEligibleResult.RelatedEligibleRule Is Nothing Then
                                            strObjectName2 = udtEligibleResult.RelatedEligibleRule.ObjectName2

                                        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing Then
                                            strObjectName2 = udtEligibleResult.RelatedEligibleExceptionRule.ObjectName2
                                        End If

                                        If Not String.IsNullOrEmpty(strObjectName2) Then
                                            Me.panReminder.Visible = True
                                            Me.lblStep2aReminder.Text = Me.GetGlobalResourceObject("Text", strObjectName2)
                                        Else
                                            Me.panReminder.Visible = False
                                        End If
                                    End If

                                End If

                            End If

                        End If
                    Next
                End If

                'Initial hide claim detail
                Me.panVSSPlaceOfVaccination.Visible = False
                Me.udcClaimVaccineInputVSS.Visible = False
                Me.panVSSRecipientCondition.Visible = False

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
                    If HighRiskOptionShown = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
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
                        End Select

                        If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
                            Dim strPlaceVaccinationValue As String = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination).AdditionalFieldValueCode
                            Me.ddlPlaceOfVaccination.SelectedValue = strPlaceVaccinationValue

                            If strPlaceVaccinationValue = PlaceOfVaccinationOptions.OTHERS Then
                                trPlaceOfVaccinationOther.Style.Add("display", "initial")
                                txtPlaceOfVaccinationOther.Text = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination).AdditionalFieldValueDesc
                            Else
                                trPlaceOfVaccinationOther.Style.Add("display", "none")
                            End If
                        End If
                    End If
                End If

                'Fill data
                If Not IsPostBack Or _udtSessionHandler.ChangeSchemeInPracticeGetFromSession(FunctCode) Or _udtSessionHandler.ClaimForSamePatientGetFromSession(FunctCode) Then
                    FillClaimDetail(False)
                    If _udtSessionHandler.ChangeSchemeInPracticeGetFromSession(FunctCode) Then
                        _udtSessionHandler.ChangeSchemeInPracticeSaveToSession(False, FunctCode)
                    End If
                    If _udtSessionHandler.ClaimForSamePatientGetFromSession(FunctCode) Then
                        _udtSessionHandler.ClaimForSamePatientSaveToSession(False, FunctCode)
                    End If
                End If

                If (Me.rbCategorySelection.SelectedValue <> String.Empty And Me.rbCategorySelection.Visible) Or lblCategory.Visible Then

                    If Me.panVSSRecipientCondition.Visible Then
                        'Category is selected and High risk option is shown

                        'Category List
                        showCategoryDetail(Me.Category)

                        'Fill detail when only one category is existed 
                        If Not String.IsNullOrEmpty(Me.Category) And lblCategory.Visible Then
                            FillClaimDetail(True)
                        End If

                        'For PID Event Handler
                        If Me.Category = CategoryCode.VSS_PID Then
                            RemoveHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick
                            AddHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick
                        End If

                        'Place of Vaccination
                        If Me.NonClinic Then
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
                        showCategoryDetail(Me.Category)

                        'Fill detail when only one category is existed 
                        If Not String.IsNullOrEmpty(Me.Category) And lblCategory.Visible Then
                            FillClaimDetail(True)
                        End If

                        If Me.NonClinic Then
                            Me.panVSSPlaceOfVaccination.Visible = True
                        End If

                        Me.udcClaimVaccineInputVSS.Visible = True

                        If Me.Category = CategoryCode.VSS_PID Then
                            RemoveHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick
                            AddHandler ucInputVSSPID.SearchPIDClick, AddressOf udcInputVSSPID_SearchPIDClick
                        End If

                        AddHandler Me.udcClaimVaccineInputVSS.VaccineLegendClicked, AddressOf udcClaimVaccineInputVSS_VaccineLegendClicked
                        AddHandler Me.udcClaimVaccineInputVSS.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputVSS_SubsidizeDisabledRemarkClicked
                    End If

                End If
            End If

        Else
            'Is Covid 19

            panClaim.Visible = False
            panClaimCOVID19.Visible = True

            'Bind DropDownList Booth
            BindBooth()

            'Bind DropDownList Category
            BindCOVID19Category(MyBase.ClaimCategorys)

            'Get Vaccine Brand & Lot No.
            Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
            Dim dtVaccineLotNo As DataTable = Nothing

            dtVaccineLotNo = udtCOVID19BLL.GetCOVID19VaccineLotMapping(CurrentPractice.SPID, CurrentPractice.PracticeID)

            If dtVaccineLotNo.Rows.Count > 0 Then

                Dim drVaccineLotNo() As DataRow = Nothing

                drVaccineLotNo = dtVaccineLotNo.Select(String.Format("'{0}' >= Service_Period_From AND '{1}' < Service_Period_To", _
                                                    ServiceDate, _
                                                    ServiceDate))

                'Convert DataRow to Dictionary(select Vaccine_Lot_No group by Brand_ID)
                Dim dicVaccineLotNo As Dictionary(Of String, String()) = drVaccineLotNo _
                    .GroupBy(Function(row) row.Field(Of String)("Brand_ID")) _
                    .ToDictionary(
                        Function(gp) gp.Key,
                        Function(gp) gp.Select(Function(gpRow) gpRow.Field(Of String)("Vaccine_Lot_No")).ToArray()
                    )

                'Convert Dictionary to josn
                Dim serializer As New JavaScriptSerializer()
                Dim jsonVaccineLotNo As String = serializer.Serialize(dicVaccineLotNo)
                'The Javascripts for binding the cascading dropdown
                Dim strPleaseSelect As String = Me.GetGlobalResourceObject("Text", "PleaseSelect")

                Dim jsBindingddlCVaccineLotNoCovid19 As String = ""
                'jsBindingddlCVaccineLotNoCovid19 += "var Vaccine = {'1': ['SNV202100001', 'SNV202100002'],'2': ['BNT202100001']};"
                jsBindingddlCVaccineLotNoCovid19 += "var Vaccine = " & jsonVaccineLotNo & ";"
                jsBindingddlCVaccineLotNoCovid19 += "var $lotNo = $(""[id$='ddlCVaccineLotNo']"");"
                'When the ddlCVaccineBrandCovid19 is changed, it focus to rerender the ddlCVaccineLotNoCovid19.
                jsBindingddlCVaccineLotNoCovid19 += "$(""[id$='ddlCVaccineBrand']"").change(function () {"
                jsBindingddlCVaccineLotNoCovid19 += "var Vacc = $(this).val(), lcns = Vaccine[Vacc] || [];"
                jsBindingddlCVaccineLotNoCovid19 += "$(""[id$='txtCVaccineBrand']"").val(Vacc);"
                jsBindingddlCVaccineLotNoCovid19 += "console.log(""checked ddl:  ""  + Vacc);"
                jsBindingddlCVaccineLotNoCovid19 += "console.log(""checked txtCVaccineBrand:  ""  + $(""[id$='txtCVaccineBrand']"").val());"
                'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
                jsBindingddlCVaccineLotNoCovid19 += "var html = (lcns.length > 1 ?'<option value>" & strPleaseSelect & "</option>': '');"
                jsBindingddlCVaccineLotNoCovid19 += "html += $.map(lcns, function(lcn){"
                jsBindingddlCVaccineLotNoCovid19 += "return ('<option value=""' + lcn + '"">' + lcn + '</option>')"
                jsBindingddlCVaccineLotNoCovid19 += "}).join('');"
                jsBindingddlCVaccineLotNoCovid19 += "$lotNo.html(html).change();"
                jsBindingddlCVaccineLotNoCovid19 += "(lcns.length > 0) ? $lotNo.prop('disabled', false) : $lotNo.prop('disabled', true);"
                jsBindingddlCVaccineLotNoCovid19 += "});"
                'Save the ddlCVaccineLotNoCovid19 selected value to hidden text box for postback
                jsBindingddlCVaccineLotNoCovid19 += "$lotNo.change(function () {"
                jsBindingddlCVaccineLotNoCovid19 += "var SelectedLotNo = $(this).val();"
                jsBindingddlCVaccineLotNoCovid19 += "$(""[id$='txtCVaccineLotNo']"").val(SelectedLotNo);"
                jsBindingddlCVaccineLotNoCovid19 += "console.log(""checked ddl:  ""  + SelectedLotNo);"
                jsBindingddlCVaccineLotNoCovid19 += "console.log(""checked txtCVaccineLotNo:  ""  + $(""[id$='txtCVaccineLotNo']"").val());"
                jsBindingddlCVaccineLotNoCovid19 += "});"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "scriptVSS", jsBindingddlCVaccineLotNoCovid19, True)

                'Bind DropDownList Vaccine Brand
                BindCOVID19VaccineBrand(drVaccineLotNo)

                'Bind DropDownList Vaccine Lot No.
                BindCOVID19VaccineLotNo(drVaccineLotNo)

            Else
                ddlCVaccineBrand.Enabled = False
                ddlCVaccineLotNo.Enabled = False

            End If

            'Assign Subsidize Display Code
            If EHSClaimVaccine IsNot Nothing Then
                lblCVaccine.Text = EHSClaimVaccine.SubsidizeList(0).SubsidizeDisplayCode
            End If



            'Bind DropDownList Dose
            BindCOVID19Dose()

            ' Fill value by temp save
            If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

                If MyBase.EHSTransaction.CategoryCode IsNot Nothing Then
                    Dim udtClaimCategory As ClaimCategoryModel = Nothing

                    udtClaimCategory = (New ClaimCategoryBLL).getAllCategoryCache().FilterByCategoryCode(MyBase.EHSTransaction.SchemeCode, MyBase.EHSTransaction.CategoryCode)

                    For Each li As ListItem In ddlCCategory.Items
                        If udtClaimCategory.SubsidizeCode.ToUpper.Trim = li.Value Then
                            ddlCCategory.SelectedValue = li.Value
                        End If
                    Next
                End If
            End If
        End If

    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        'MyBase.SetDoseErrorImage(blnVisible)
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

    Public Sub SetClaimDetailError(ByVal visible As Boolean)
        ucInputVSSPID.SetDocumentaryProofError(False)
        ucInputVSSPID.SetPIDCodeError(False)
        ucInputVSSDA.SetDocumentaryProofError(False)
    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Sub SetRecipientConditionError(ByVal visible As Boolean)
        Me.imgRecipientConditionError.Visible = visible
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetBoothError(ByVal visible As Boolean)
        Me.imgCBoothError.Visible = visible
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetCategoryForCOVID19Error(ByVal visible As Boolean)
        Me.imgCCategoryError.Visible = visible
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetVaccineBrandError(ByVal visible As Boolean)
        Me.imgCVaccineBrandError.Visible = visible
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetVaccineLotNoError(ByVal visible As Boolean)
        Me.imgCVaccineLotNoError.Visible = visible
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetDoseForCOVID19Error(ByVal visible As Boolean)
        Me.imgCDoseError.Visible = visible
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

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

    Public Sub FillClaimDetail(ByVal blnForceToFill As Boolean)

        'Select Case mvCategory.ActiveViewIndex
        '    Case ViewIndexCategory.VSS_PID
        'If Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode) Is Nothing And _
        '    Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode) = String.Empty Then

        '    Dim strDocumentaryProofValue As String = _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode)

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

        If ddlPlaceOfVaccination.Enabled Or blnForceToFill Then
            If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
                If Not _udtSessionHandler.PlaceVaccinationGetFromSession(FunctCode, SchemeClaimModel.VSS) Is Nothing And _
                        Not _udtSessionHandler.PlaceVaccinationGetFromSession(FunctCode, SchemeClaimModel.VSS) = String.Empty Then
                    Dim strPlaceVaccinationValue As String = _udtSessionHandler.PlaceVaccinationGetFromSession(FunctCode, SchemeClaimModel.VSS)
                    Me.ddlPlaceOfVaccination.SelectedValue = strPlaceVaccinationValue

                    If strPlaceVaccinationValue = PlaceOfVaccinationOptions.OTHERS Then
                        If Not _udtSessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode, SchemeClaimModel.VSS) Is Nothing And _
                        Not _udtSessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode, SchemeClaimModel.VSS) = String.Empty Then
                            txtPlaceOfVaccinationOther.Text = _udtSessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode, SchemeClaimModel.VSS)
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

        Me.ddlPlaceOfVaccination.SelectedIndex = -1
        Me.ddlPlaceOfVaccination.SelectedValue = Nothing
        Me.ddlPlaceOfVaccination.ClearSelection()
        Me.panVSSPlaceOfVaccination.Visible = False
        trPlaceOfVaccinationOther.Style.Add("display", "none")
        txtPlaceOfVaccinationOther.Text = String.Empty

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.chkRecipientCondition.Checked = False
        Me.rblRecipientCondition.SelectedIndex = -1
        Me.rblRecipientCondition.SelectedValue = Nothing
        Me.rblRecipientCondition.ClearSelection()
        'CRE16-026 (Add PCV13) [End][Chris YIM]

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

    Private Sub rbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged
        SetPlaceOfVaccinationError(False)
        ucInputVSSPID.SetDocumentaryProofError(False)
        ucInputVSSPID.SetPIDCodeError(False)
        ucInputVSSDA.SetDocumentaryProofError(False)
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        SetRecipientConditionError(False)
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
        Me.rbCategorySelection.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rbCategorySelection.UniqueID), True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]

        ' Claim Detail Input by Category
        showCategoryDetail(Me.Category)
        If Not String.IsNullOrEmpty(Me.Category) Then
            FillClaimDetail(True)
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
        Me.rblRecipientCondition.Enabled = blnEnabled

        If Me.rblRecipientCondition.Enabled Then
            'Enabled
            SetHighRisk(SessionHandler.HighRiskGetFromSession(FunctCode))
        Else
            'Disabled
            Me.rblRecipientCondition.SelectedIndex = -1
            Me.rblRecipientCondition.SelectedValue = Nothing
            Me.rblRecipientCondition.ClearSelection()

        End If
    End Sub

    Private Sub showCategoryDetail(ByVal strSelectedCategory As String)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
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
            Case Else
                mvCategory.ActiveViewIndex = ViewIndexCategory.NoCategory
        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    End Sub

    Private Sub ddlPlaceOfVaccination_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPlaceOfVaccination.SelectedIndexChanged
        Dim ddlPlaceOfVaccination As DropDownList = CType(sender, DropDownList)

        If Not ddlPlaceOfVaccination Is Nothing Then
            If ddlPlaceOfVaccination.SelectedValue.Trim = PlaceOfVaccinationOptions.OTHERS Then
                trPlaceOfVaccinationOther.Style.Add("display", "initial")
                If Not SessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode, SchemeClaimModel.VSS) Is Nothing Then
                    txtPlaceOfVaccinationOther.Text = SessionHandler.PlaceVaccinationOtherGetFromSession(FunctCode, SchemeClaimModel.VSS)
                End If
            Else
                trPlaceOfVaccinationOther.Style.Add("display", "none")
            End If

        End If

        showCategoryDetail(Me.Category)

    End Sub

    Private Sub txtPlaceOfVaccinationOther_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPlaceOfVaccinationOther.TextChanged
        Dim txtPlaceOfVaccinationOther As TextBox = CType(sender, TextBox)

        If Not txtPlaceOfVaccinationOther Is Nothing Then
            If ddlPlaceOfVaccination.SelectedValue.Trim = PlaceOfVaccinationOptions.OTHERS Then
                SessionHandler.PlaceVaccinationOtherSaveToSession(txtPlaceOfVaccinationOther.Text, FunctCode, SchemeClaimModel.VSS)
            End If
        End If

        showCategoryDetail(Me.Category)

    End Sub

    Private Sub ImgBtnRecipientConditionHelp_Click(sender As Object, e As ImageClickEventArgs) Handles ImgBtnRecipientConditionHelp.Click
        RaiseEvent RecipientConditionHelpClick(sender, e)
    End Sub

    Private Sub ddlCBooth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCBooth.SelectedIndexChanged
        Me.ddlCBooth.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCBooth.UniqueID), True)
        SessionHandler.ClaimCOVID19BoothSaveToSession(Me.ddlCBooth.SelectedValue, FunctCode)

    End Sub

    'Private Sub ddlCCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCCategory.SelectedIndexChanged
    '    Me.ddlCCategory.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCCategory.UniqueID), True)
    '    SessionHandler.ClaimCOVID19CategorySaveToSession(Me.ddlCCategory.SelectedValue, FunctCode)
    'End Sub

    'Private Sub ddlCVaccineBrand_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCVaccineBrand.SelectedIndexChanged
    '    Me.ddlCVaccineBrand.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCVaccineBrand.UniqueID), True)
    '    SessionHandler.ClaimCOVID19VaccineBrandSaveToSession(Me.ddlCVaccineBrand.SelectedValue, FunctCode)
    'End Sub

    'Private Sub ddlCVaccineLotNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCVaccineLotNo.SelectedIndexChanged
    '    Me.ddlCVaccineLotNo.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCVaccineLotNo.UniqueID), True)
    '    SessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(Me.ddlCVaccineLotNo.SelectedValue, FunctCode)
    'End Sub

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

        ddlPlaceOfVaccination.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "EHSClaimPleaseSelect"), String.Empty))

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
                trPlaceOfVaccinationOther.Style.Add("display", "none")
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
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = _udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        If Not IsClaimCOVID19 Then
            'Check Category
            If String.IsNullOrEmpty(Me.Category) Then
                blnResult = False

                Me.SetCategoryError(True)

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00238)
                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            End If

            'Check Place of Vaccination
            If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
                blnResult = blnResult And ValidatePlaceOfVaccination(blnShowErrorImage, objMsgBox)
            End If

            'Check Claim detail
            Select Case mvCategory.ActiveViewIndex
                Case ViewIndexCategory.VSS_DA
                    blnResult = blnResult And Me.ucInputVSSDA.Validate(blnShowErrorImage, objMsgBox)
                Case ViewIndexCategory.VSS_PID
                    blnResult = blnResult And Me.ucInputVSSPID.Validate(blnShowErrorImage, objMsgBox)
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
            udtEHSClaimVaccine = SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
            Dim blnVaccineValid As Boolean = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, objMsgBox)
            If Not blnVaccineValid Then
                SetDoseErrorImage(True)
            End If

            blnResult = blnResult And blnVaccineValid

        Else
            'Check Booth
            If panBooth.Visible AndAlso String.IsNullOrEmpty(Me.Booth) Then
                blnResult = False

                Me.SetBoothError(True)

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {HttpContext.GetGlobalResourceObject("Text", "Booth", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "Booth", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "Booth", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                       })
                End If
            End If

            'Check Category
            If String.IsNullOrEmpty(Me.CategoryForCOVID19) Then
                blnResult = False

                Me.SetCategoryForCOVID19Error(True)

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {HttpContext.GetGlobalResourceObject("Text", "Category", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "Category", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "Category", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                       })
                End If
            End If

            'Check Vaccine Brand
            If String.IsNullOrEmpty(Me.VaccineBrand) Then
                blnResult = False

                Me.SetVaccineBrandError(True)

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {HttpContext.GetGlobalResourceObject("Text", "Vaccines", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "Vaccines", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "Vaccines", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                       })
                End If
            End If

            'Check Vaccine Lot No.
            If String.IsNullOrEmpty(Me.VaccineLotNo) Then
                blnResult = False

                Me.SetVaccineLotNoError(True)

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {HttpContext.GetGlobalResourceObject("Text", "VaccineLotNumber", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "VaccineLotNumber", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "VaccineLotNumber", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                       })
                End If
            End If

            'Check Dose
            If String.IsNullOrEmpty(Me.DoseForCOVID19) Then
                blnResult = False

                Me.SetDoseForCOVID19Error(True)

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {HttpContext.GetGlobalResourceObject("Text", "DoseSeq", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "DoseSeq", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "DoseSeq", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                       })
                End If
            Else
                'Dose
                If ddlCDose.SelectedValue <> String.Empty Then
                    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                        If udtEHSClaimSubsidize.SubsidizeCode.ToUpper.Trim = ddlCCategory.SelectedValue.ToUpper.Trim Then
                            udtEHSClaimSubsidize.Selected = True

                            For Each udtEHSClaimSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                                If udtEHSClaimSubsidizeDetail.AvailableItemCode.ToUpper.Trim = ddlCDose.SelectedValue.ToUpper.Trim Then
                                    udtEHSClaimSubsidizeDetail.Selected = True
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        End If

        Return blnResult
    End Function

    Public Function ValidatePlaceOfVaccination(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim blnResult As Boolean = True

        Me.imgPlaceOfVaccinationError.Visible = False

        If String.IsNullOrEmpty(Me.PlaceOfVaccination) = True Then
            blnResult = False
            Me.imgPlaceOfVaccinationError.Visible = blnShowErrorImage
            objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00385)) ' Please select "Place Of Vaccination"
        End If

        If Me.PlaceOfVaccination = PlaceOfVaccinationOptions.OTHERS Then
            If String.IsNullOrEmpty(Me.txtPlaceOfVaccinationOther.Text) = True Then
                blnResult = False
                Me.imgPlaceOfVaccinationErrorOther.Visible = blnShowErrorImage
                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00386)) ' Please input "Place Of Vaccination"
            End If

            'Transform to byte (4 byte for each char)
            Dim bytChiName As Byte() = System.Text.Encoding.UTF32.GetBytes(Me.txtPlaceOfVaccinationOther.Text)
            If bytChiName.Length > 1020 Then 'Maximum 255 char
                blnResult = False

                Me.imgPlaceOfVaccinationErrorOther.Visible = blnShowErrorImage
                objMsgBox.AddMessage(New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00390)) ' "Place Of Vaccination" is invalid
            End If
        End If

        Return blnResult
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

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtEHSClaimVaccine As EHSClaimVaccineModel)

        If Not IsClaimCOVID19 Then
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
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.HideButForceHighRisk 'No Input
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
                If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) AndAlso Me.PlaceOfVaccination <> String.Empty Then
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination
                    udtTransactAdditionfield.AdditionalFieldValueCode = Me.PlaceOfVaccination
                    udtTransactAdditionfield.AdditionalFieldValueDesc = IIf(Me.PlaceOfVaccination = PlaceOfVaccinationOptions.OTHERS, txtPlaceOfVaccinationOther.Text, Nothing)
                    udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    _udtSessionHandler.PlaceVaccinationSaveToSession(Me.PlaceOfVaccination, FunctCode, SchemeClaimModel.VSS)

                    If Me.PlaceOfVaccination = PlaceOfVaccinationOptions.OTHERS Then
                        _udtSessionHandler.PlaceVaccinationOtherSaveToSession(txtPlaceOfVaccinationOther.Text, FunctCode, SchemeClaimModel.VSS)
                    Else
                        _udtSessionHandler.PlaceVaccinationOtherRemoveFromSession(FunctCode)
                    End If

                End If
            End If

        Else
            'is covid-19
            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
            udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

            Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

            'Category
            Dim udtClaimCategory As ClaimCategoryModel = Nothing
            Dim udtClaimCategoryList As ClaimCategoryModelCollection = Nothing

            If ddlCCategory.SelectedValue <> String.Empty Then
                For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                    If udtEHSClaimSubsidize.Selected Then
                        udtClaimCategoryList = (New ClaimCategoryBLL).getAllCategoryCache().Filter(udtEHSClaimSubsidize.SchemeCode, udtEHSClaimSubsidize.SchemeSeq, udtEHSClaimSubsidize.SubsidizeCode)
                    End If
                Next

                If udtClaimCategoryList.Count > 0 Then
                    udtClaimCategory = udtClaimCategoryList(0)
                    udtEHSTransaction.CategoryCode = udtClaimCategory.CategoryCode

                    '_udtSessionHandler.ClaimCOVID19CategorySaveToSession(udtClaimCategory.SubsidizeCode, FunctCode)
                End If

            End If

            'Clinic Type
            If Not udtSubsidizeLatest Is Nothing Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType
                udtTransactAdditionfield.AdditionalFieldValueCode = IIf(_udtSessionHandler.NonClinicSettingGetFromSession(FunctCode), ClinicType.NonClinic, ClinicType.Clinic)
                udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If

            'Booth
            If panBooth.Visible Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Booth
                udtTransactAdditionfield.AdditionalFieldValueCode = ddlCBooth.SelectedValue.Trim
                udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If

            'Vaccine Brand
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineBrand
            udtTransactAdditionfield.AdditionalFieldValueCode = txtCVaccineBrand.Text.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Vaccine Lot No.
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineLotNo
            'udtTransactAdditionfield.AdditionalFieldValueCode = ddlCVaccineLotNo.SelectedValue.Trim
            udtTransactAdditionfield.AdditionalFieldValueCode = txtCVaccineLotNo.Text.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

        End If
    End Sub

#End Region

#Region "Other functions"

    Private Sub BindCategory(ByVal strLanguage As String, ByVal updateByTransactionModel As Boolean, ByVal udtClaimCategorys As ClaimCategoryModelCollection)
        Dim strSelectedValue As String
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctCode)
        Dim dtClaimCategory As DataTable

        Me.rbCategorySelection.Visible = False
        Me.lblCategory.Visible = False

        If Not udtClaimCategory Is Nothing Then
            strSelectedValue = udtClaimCategory.CategoryCode
        Else
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
            strSelectedValue = AntiXssEncoder.HtmlEncode(Me.rbCategorySelection.SelectedValue, True)
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]
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

            ' INT20-0023 (Fix to hide SIV on season end) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.rbCategorySelection.ClearSelection()
            Me.rbCategorySelection.DataBind()
            ' INT20-0023 (Fix to hide SIV on season end) [End][Chris YIM]

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

    Private Sub BindBooth()
        If panBooth.Visible Then
            Dim strSelectedValue As String = Nothing

            If Not SessionHandler.ClaimCOVID19BoothGetFromSession(FunctCode) Is Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19BoothGetFromSession(FunctCode)
            Else
                strSelectedValue = AntiXssEncoder.HtmlEncode(Me.ddlCBooth.SelectedValue, True)
            End If

            'Bind Booth into dropdownlist
            Me.ddlCBooth.Items.Clear()
            Me.ddlCBooth.SelectedIndex = -1
            Me.ddlCBooth.SelectedValue = Nothing
            Me.ddlCBooth.ClearSelection()

            If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
                SessionHandler.ClaimCOVID19BoothRemoveFromSession(FunctCode)
            End If

            'Build Booth dropdownlist
            Dim lstItem As ListItem

            For i As Integer = 1 To 10
                lstItem = New ListItem
                lstItem.Text = i
                lstItem.Value = i

                Me.ddlCBooth.Items.Add(lstItem)
            Next

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCBooth.Items.Count > 1 Then
                ddlCBooth.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            ddlCBooth.SelectedIndex = 0

            'Restore the selected value if has value
            If strSelectedValue IsNot Nothing Then
                For Each li As ListItem In ddlCBooth.Items
                    If strSelectedValue = li.Value Then
                        ddlCBooth.SelectedValue = li.Value
                    End If
                Next
            End If
        End If

    End Sub

    Private Sub BindCOVID19Category(ByVal udtClaimCategorys As ClaimCategoryModelCollection)
        If udtClaimCategorys IsNot Nothing Then
            Dim strSelectedValue As String = Nothing
            Dim strCategoryCode As String = Nothing

            Dim udtClaimCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctCode)
            Dim dtClaimCategory As DataTable

            If Not udtClaimCategory Is Nothing Then
                strSelectedValue = udtClaimCategory.CategoryCode
            ElseIf Not SessionHandler.ClaimCOVID19CategoryGetFromSession(FunctCode) Is Nothing Then
                Dim udtClaimCategoryList As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getAllCategoryCache().Filter(udtClaimCategorys(0).SchemeCode, udtClaimCategorys(0).SchemeSeq, SessionHandler.ClaimCOVID19CategoryGetFromSession(FunctCode))
                If udtClaimCategoryList.Count > 0 Then
                    strCategoryCode = udtClaimCategoryList(0).CategoryCode
                Else
                    strCategoryCode = String.Empty
                End If
                strSelectedValue = SessionHandler.ClaimCOVID19CategoryGetFromSession(FunctCode)
            Else
                strSelectedValue = AntiXssEncoder.HtmlEncode(Me.ddlCCategory.SelectedValue, True)
            End If

            'Check selected category whether exists when practice is changed
            Dim udtSelectedCategory As ClaimCategoryModel = udtClaimCategorys.Filter(strCategoryCode)
            If udtSelectedCategory Is Nothing Then
                strSelectedValue = String.Empty
                Me.ddlCCategory.Items.Clear()
                Me.ddlCCategory.SelectedIndex = -1
                Me.ddlCCategory.SelectedValue = Nothing
                Me.ddlCCategory.ClearSelection()
                SessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
            End If

            'Build Category dropdownlist
            dtClaimCategory = ClaimCategoryBLL.ConvertCategoryToDatatable(udtClaimCategorys)

            Me.ddlCCategory.DataSource = dtClaimCategory

            Me.ddlCCategory.DataValueField = ClaimCategoryModel._Subsidize_Code

            If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlCCategory.DataTextField = ClaimCategoryModel._Category_Name_Chi
            Else
                Me.ddlCCategory.DataTextField = ClaimCategoryModel._Category_Name
            End If

            Me.ddlCCategory.Items.Clear()
            Me.ddlCCategory.SelectedIndex = -1
            Me.ddlCCategory.SelectedValue = Nothing
            Me.ddlCCategory.ClearSelection()
            Me.ddlCCategory.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCCategory.Items.Count > 1 Then
                ddlCCategory.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
                'ddlCCategory.SelectedIndex = 1
            Else
                ddlCCategory.SelectedIndex = 0
            End If

            'Restore the selected value if has value
            If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                For Each li As ListItem In ddlCCategory.Items
                    If strSelectedValue = li.Value Then
                        ddlCCategory.SelectedValue = li.Value
                    End If
                Next
            End If
        End If

    End Sub

    ' CRE20-0022 (Immu record) [Start][Martin Tang]
    Private Sub BindCOVID19VaccineBrand(drVaccineLotNo() As DataRow)
        Dim strSelectedValue As String = Nothing

        Dim dtVaccineBrand As DataTable = Nothing

        strSelectedValue = Me.Request.Form(Me.txtCVaccineBrand.UniqueID)

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            If Not SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode) Is Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode)
            End If
        Else
            SessionHandler.ClaimCOVID19VaccineBrandSaveToSession(strSelectedValue, FunctCode)
        End If

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            strSelectedValue = String.Empty
            SessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctCode)
        End If

        Me.ddlCVaccineBrand.Items.Clear()
        Me.ddlCVaccineBrand.SelectedIndex = -1
        Me.ddlCVaccineBrand.SelectedValue = Nothing
        Me.ddlCVaccineBrand.ClearSelection()

        'Build Vaccine Brand dropdownlist
        If drVaccineLotNo.Length > 0 Then
            Me.ddlCVaccineBrand.Enabled = True

            dtVaccineBrand = drVaccineLotNo.CopyToDataTable.DefaultView.ToTable(True, New String() {"Brand_ID", "Brand_Name", "Brand_Name_Chi"})

            Me.ddlCVaccineBrand.DataSource = dtVaccineBrand

            Me.ddlCVaccineBrand.DataValueField = "Brand_ID"

            If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlCVaccineBrand.DataTextField = "Brand_Name_Chi"
            Else
                Me.ddlCVaccineBrand.DataTextField = "Brand_Name"
            End If

            Me.ddlCVaccineBrand.ClearSelection()
            Me.ddlCVaccineBrand.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCVaccineBrand.Items.Count > 1 Then
                ddlCVaccineBrand.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            Me.ddlCVaccineBrand.SelectedIndex = 0
            Me.txtCVaccineBrand.Text = ddlCVaccineBrand.Items(0).Value.Trim

            'Restore the selected value if has value
            If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                For Each li As ListItem In ddlCVaccineBrand.Items
                    If strSelectedValue = li.Value Then
                        Me.ddlCVaccineBrand.SelectedValue = li.Value
                        Me.txtCVaccineBrand.Text = Me.ddlCVaccineBrand.SelectedValue
                    End If
                Next
            End If

        Else
            Me.ddlCVaccineBrand.Items.Clear()
            Me.ddlCVaccineBrand.SelectedIndex = -1
            Me.ddlCVaccineBrand.SelectedValue = Nothing
            Me.ddlCVaccineBrand.ClearSelection()
            Me.ddlCVaccineBrand.Enabled = False

        End If

    End Sub

    Private Sub BindCOVID19VaccineLotNo(drVaccineLotNo() As DataRow)
        Dim strSelectedValue As String = Nothing

        strSelectedValue = Me.Request.Form(Me.txtCVaccineLotNo.UniqueID)

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            If SessionHandler.ClaimCOVID19VaccineLotNoGetFromSession(FunctCode) IsNot Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19VaccineLotNoGetFromSession(FunctCode)
            End If
        Else
            SessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(strSelectedValue, FunctCode)
        End If

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            strSelectedValue = String.Empty
            SessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)
        End If

        Me.ddlCVaccineLotNo.Items.Clear()
        Me.ddlCVaccineLotNo.SelectedIndex = -1
        Me.ddlCVaccineLotNo.SelectedValue = Nothing
        Me.ddlCVaccineLotNo.ClearSelection()

        'Build Vaccine Lot No. dropdownlist Filter With Band id
        Dim drVaccineLotNoFilterWithBand() As DataRow = Nothing
        Dim strVaccinBandID As String = Me.txtCVaccineBrand.Text.Trim  ' for postback after validation
        If strVaccinBandID Is Nothing Then
            strVaccinBandID = Me.ddlCVaccineLotNo.SelectedValue ' For the first time render (the Practice only has one brand)
        End If
        'If stVaccinBandID = "", it means no brand is selected. (the Practice has more than one brand = > drVaccineLotNoFilterWithBand.length = 0) 
        drVaccineLotNoFilterWithBand = drVaccineLotNo.CopyToDataTable().Select(String.Format("Brand_ID = '{0}'", strVaccinBandID))

        'For render the server side dropdown
        If drVaccineLotNoFilterWithBand.Length > 0 Then
            Me.ddlCVaccineLotNo.Enabled = True
            Me.ddlCVaccineLotNo.DataSource = drVaccineLotNoFilterWithBand.CopyToDataTable()

            Me.ddlCVaccineLotNo.DataValueField = "Vaccine_Lot_No"
            Me.ddlCVaccineLotNo.DataTextField = "Vaccine_Lot_No"
            Me.ddlCVaccineLotNo.ClearSelection()
            Me.ddlCVaccineLotNo.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCVaccineLotNo.Items.Count > 1 Then
                ddlCVaccineLotNo.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            Me.ddlCVaccineLotNo.SelectedIndex = 0
            Me.txtCVaccineLotNo.Text = ddlCVaccineLotNo.Items(0).Value.Trim

            'Restore the selected value if has value
            If ddlCVaccineLotNo.Items.Count > 1 Then
                If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                    For Each li As ListItem In ddlCVaccineLotNo.Items
                        If strSelectedValue = li.Value Then
                            Me.ddlCVaccineLotNo.SelectedValue = li.Value
                            Me.txtCVaccineLotNo.Text = Me.ddlCVaccineLotNo.SelectedValue
                        End If
                    Next
                End If
            End If

            'Me.txtCVaccineLotNo.Text = Me.ddlCVaccineLotNoCovid19.SelectedValue

            'If Me.Request.Form(Me.txtCVaccineLotNo.UniqueID) <> "" Then
            '    ddlCVaccineLotNoCovid19.SelectedValue = Me.Request.Form(Me.txtCVaccineLotNo.UniqueID)
            '    txtCVaccineLotNo.Text = Me.Request.Form(Me.txtCVaccineLotNo.UniqueID)
            'End If

        Else
            Me.ddlCVaccineLotNo.Items.Clear()
            Me.ddlCVaccineLotNo.SelectedIndex = -1
            Me.ddlCVaccineLotNo.SelectedValue = Nothing
            Me.ddlCVaccineLotNo.ClearSelection()
            Me.ddlCVaccineLotNo.Enabled = False

        End If
    End Sub
    ' CRE20-0022 (Immu record) [END][Martin Tang]

    Private Sub BindCOVID19Dose()
        Dim strSelectedValue As String = Nothing

        strSelectedValue = AntiXssEncoder.HtmlEncode(Me.ddlCDose.SelectedValue, True)

        'Set selected if "1st Dose" exists
        If strSelectedValue = String.Empty AndAlso _udtSessionHandler.ClaimCOVID19DoseGetFromSession(FunctCode) Is Nothing Then
            For Each li As ListItem In ddlCDose.Items
                If li.Value = "1STDOSE" Then
                    strSelectedValue = "1STDOSE"
                End If
            Next
        End If

        'If nothing, get value form session
        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            If Not SessionHandler.ClaimCOVID19DoseGetFromSession(FunctCode) Is Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19DoseGetFromSession(FunctCode)
            End If
        Else
            If ddlCDose.Items.Count > 0 Then
                SessionHandler.ClaimCOVID19DoseSaveToSession(strSelectedValue, FunctCode)
            End If
        End If

        'Bind Dose into dropdownlist
        Me.ddlCDose.Items.Clear()
        Me.ddlCDose.SelectedIndex = -1
        Me.ddlCDose.SelectedValue = Nothing
        Me.ddlCDose.ClearSelection()

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            strSelectedValue = String.Empty
        End If

        'Build Booth dropdownlist
        Dim lstItem As ListItem = Nothing

        If EHSClaimVaccine IsNot Nothing Then
            For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In EHSClaimVaccine.SubsidizeList(0).SubsidizeDetailList
                If udtEHSClaimSubidizeDetail.Available Then
                    lstItem = New ListItem
                    lstItem.Value = udtEHSClaimSubidizeDetail.AvailableItemCode

                    If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                        lstItem.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi
                    Else
                        lstItem.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc
                    End If

                    ddlCDose.Items.Add(lstItem)
                End If
            Next

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCDose.Items.Count > 1 Then
                ddlCDose.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            If ddlCDose.Items.Count > 0 Then
                ddlCDose.SelectedIndex = 0
            Else
                ddlCDose.Enabled = False
                ddlCDose.Dispose()
            End If


        End If

        'Restore the selected value if has value
        If strSelectedValue IsNot Nothing Then
            For Each li As ListItem In ddlCDose.Items
                If strSelectedValue = li.Value Then
                    ddlCDose.SelectedValue = li.Value
                End If
            Next
        End If

    End Sub

#End Region



End Class
