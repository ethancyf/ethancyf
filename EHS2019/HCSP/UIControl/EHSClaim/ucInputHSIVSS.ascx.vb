Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory

Partial Public Class ucInputHSIVSS
    Inherits ucInputEHSClaimBase

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Input Vaccine contorl Fields
        Me.udcClaimVaccineInputHSIVSS.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputHSIVSS.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputHSIVSS.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputHSIVSS.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputHSIVSS.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputHSIVSS.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputHSIVSS.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputHSIVSS.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

        'Text Field
        Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
        Me.lblPreConditionsText.Text = Me.GetGlobalResourceObject("Text", "PreConditions")

    End Sub

    Protected Overrides Sub Setup()
        Dim strLanguage As String = MyBase.SessionHandler.Language()
        Dim updateByTransactionModel As Boolean = False
        'Dim udtClaimCategorys As ClaimCategoryModelCollection
        Dim udtClaimCategory As ClaimCategoryModel
        'Dim udtPersonalInformation As EHSPersonalInformationModel = MyBase.EHSAccount.EHSPersonalInformationList.Filter(MyBase.EHSAccount.SearchDocCode)
        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctCode)

        'udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation.DOB, udtPersonalInformation.ExactDOB, MyBase.ServiceDate)

        If Not MyBase.ClaimCategorys Is Nothing AndAlso MyBase.ClaimCategorys.Count > 0 Then
            Me.panHSIVSSDetail.Visible = True

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            If Not MyBase.EHSTransaction Is Nothing AndAlso New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(MyBase.EHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.HSIVSS _
                AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.Count > 0 Then
                updateByTransactionModel = True
            End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            If Not MyBase.EHSClaimVaccine Is Nothing Then
                Me.udcClaimVaccineInputHSIVSS.Build(MyBase.EHSClaimVaccine)
            End If

            Me.BindCategory(strLanguage, updateByTransactionModel, MyBase.ClaimCategorys)


            If MyBase.AvaliableForClaim Then

                '---------------------------------------------------------------------------------------------
                'Setup Pre condition Drop down List
                '---------------------------------------------------------------------------------------------
                If Not String.IsNullOrEmpty(Me.Category) Then

                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    ' Remove SchemeSeq
                    'udtClaimCategory = MyBase.ClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, udtSchemeClaim.SchemeSeq, Me.Category)
                    udtClaimCategory = MyBase.ClaimCategorys.FilterByCategoryCode(udtSchemeClaim.SchemeCode, Me.Category)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                    If udtClaimCategory.IsMedicalCondition = "Y" Then
                        Me.panPreConditions.Visible = True
                        Me.BindPreCondition(MyBase.SessionHandler.Language())

                        If updateByTransactionModel Then
                            Me.ddlPreConditionSelection.SelectedValue = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition").AdditionalFieldValueCode
                        End If
                    Else
                        Me.panPreConditions.Visible = False
                    End If
                Else
                    Me.panPreConditions.Visible = False
                End If

            Else
                Me.panPreConditions.Visible = False
            End If

            'Add Event Handler
            AddHandler Me.udcClaimVaccineInputHSIVSS.VaccineLegendClicked, AddressOf udcClaimVaccineInputHSIVSS_VaccineLegendClicked
        Else
            Me.panHSIVSSDetail.Visible = False
            Me.panPreConditions.Visible = False
            Me.ddlPreConditionSelection.Items.Clear()
            Me.rbCategorySelection.Items.Clear()
            Me.lblCategory.Attributes.Remove("SelectedValue")
        End If
    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.lblPreConditionsText.Width = width
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]            
            Me.lblCategoryText.Width = width
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        Else
            Me.lblPreConditionsText.Width = 200
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]            
            Me.lblCategoryText.Width = 200
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
        End If
    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        'MyBase.SetDoseErrorImage(blnVisible)
        If Not Me.udcClaimVaccineInputHSIVSS Is Nothing Then
            Me.udcClaimVaccineInputHSIVSS.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetPreConditionError(ByVal visible As Boolean)
        Me.imgPreConditionsError.Visible = visible
    End Sub

    Public Sub SetCategoryError(ByVal visible As Boolean)
        Me.imgCategoryError.Visible = visible
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property PreCondition() As String
        Get
            Return Me.ddlPreConditionSelection.SelectedValue
        End Get
    End Property

    Public ReadOnly Property PreConditionDesc() As String
        Get
            Return Me.ddlPreConditionSelection.SelectedItem.Text
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

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputHSIVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputHSIVSS_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    Private Sub ddlRoleSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged

        If String.IsNullOrEmpty(Me.rbCategorySelection.SelectedValue) Then
            Me.udcClaimVaccineInputHSIVSS.Visible = False
            Me.panPreConditions.Visible = False
        Else
            If Me.rbCategorySelection.SelectedValue = "M" Then
                Me.panPreConditions.Visible = True

                Me.BindPreCondition(MyBase.SessionHandler.Language())
            Else
                Me.panPreConditions.Visible = False
            End If
            Me.udcClaimVaccineInputHSIVSS.Visible = True
        End If
        RaiseEvent CategorySelected(sender, e)
    End Sub
#End Region

#Region "Other functions"

    Private Sub BindPreCondition(ByVal strLanguage As String)
        Dim strSelectedValue As String = Me.ddlPreConditionSelection.SelectedValue
        Dim dtPreCondition As DataTable
        Dim udtStaticDataBLL As New StaticDataBLL()
        Dim dataRow As DataRow

        'Build Drop Down List
        dtPreCondition = udtStaticDataBLL.GetStaticDataList("PreCondition")
        dataRow = dtPreCondition.NewRow
        dataRow(StaticDataModel.Item_No) = String.Empty
        dataRow(StaticDataModel.Data_Value) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect") '"Please select----" ''Should be replaced with GobalResource
        dataRow(StaticDataModel.Data_Value_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi") '"請選擇----" ''Should be replaced with GobalResource
        dataRow(StaticDataModel.Data_Value_CN) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN") '"請選擇----" ''Should be replaced with GobalResource
        dtPreCondition.Rows.InsertAt(dataRow, 0)

        Me.ddlPreConditionSelection.DataSource = dtPreCondition

        Me.ddlPreConditionSelection.DataValueField = StaticDataModel.Item_No

        If LCase(Session("language")) = CultureLanguage.TradChinese Then
            Me.ddlPreConditionSelection.DataTextField = StaticDataModel.Data_Value_Chi
        ElseIf LCase(Session("language")) = CultureLanguage.SimpChinese Then
            Me.ddlPreConditionSelection.DataTextField = StaticDataModel.Data_Value_CN
        Else
            Me.ddlPreConditionSelection.DataTextField = StaticDataModel.Data_Value
        End If

        Me.ddlPreConditionSelection.DataBind()

        If Not strSelectedValue Is Nothing AndAlso strSelectedValue.Trim() <> "" Then
            If Not Me.ddlPreConditionSelection.Items.FindByValue(strSelectedValue) Is Nothing Then
                Me.ddlPreConditionSelection.SelectedValue = strSelectedValue
            End If
        End If

    End Sub

    Private Sub BindCategory(ByVal strLanguage As String, ByVal updateByTransactionModel As Boolean, ByVal udtClaimCategorys As ClaimCategoryModelCollection)
        Dim strSelectedValue As String
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctCode)
        Dim dtClaimCategory As DataTable

        Me.rbCategorySelection.Visible = False
        Me.lblCategory.Visible = False

        dtClaimCategory = ClaimCategoryBLL.ConvertCategoryToDatatable(udtClaimCategorys)

        If dtClaimCategory.Rows.Count > 1 Then
            Me.rbCategorySelection.Visible = True

            If Not udtClaimCategory Is Nothing Then
                strSelectedValue = udtClaimCategory.CategoryCode
            Else
                strSelectedValue = Me.rbCategorySelection.SelectedValue
            End If
            Me.rbCategorySelection.DataSource = dtClaimCategory

            Me.rbCategorySelection.DataValueField = ClaimCategoryModel._Category_Code

            If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.rbCategorySelection.DataTextField = ClaimCategoryModel._Category_Name_Chi
            ElseIf MyBase.SessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                Me.rbCategorySelection.DataTextField = ClaimCategoryModel._Category_Name_CN
            Else
                Me.rbCategorySelection.DataTextField = ClaimCategoryModel._Category_Name
            End If

            Me.rbCategorySelection.DataBind()

            If updateByTransactionModel Then
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                strSelectedValue = MyBase.EHSTransaction.CategoryCode
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            End If

            If Not String.IsNullOrEmpty(strSelectedValue) _
                AndAlso Not Me.rbCategorySelection.Items.FindByValue(strSelectedValue) Is Nothing Then

                Me.rbCategorySelection.SelectedValue = strSelectedValue
                Me.udcClaimVaccineInputHSIVSS.Visible = True
            Else
                Me.udcClaimVaccineInputHSIVSS.Visible = False
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

            Me.udcClaimVaccineInputHSIVSS.Visible = True
        End If

    End Sub

    Public Overrides Sub Clear()
        MyBase.Clear()
        Me.lblCategory.Attributes.Remove("SelectedValue")
    End Sub

#End Region

End Class