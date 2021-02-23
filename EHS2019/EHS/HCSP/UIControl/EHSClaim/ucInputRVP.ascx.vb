Imports Common
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports HCSP.BLL
Imports System.Web.Security.AntiXss

Partial Public Class ucInputRVP
    Inherits ucInputEHSClaimBase

    'Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201
    Public FunctCode As String = (New BLL.SessionHandler).ClaimFunctCodeGetFromSession() 'Cre20-0xx Immue record [Nichole]

    'Events 
    Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event RecipientConditionHelpClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
    Dim _udtSessionHandler As New SessionHandler
    Dim _udtGeneralFunction As New GeneralFunction

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Input Vaccine contorl Fields
        Me.udcClaimVaccineInputRVP.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputRVP.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputRVP.AmountText = Me.GetGlobalResourceObject("Text", "InjectionCost")
        Me.udcClaimVaccineInputRVP.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputRVP.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalInjectionCost")
        Me.udcClaimVaccineInputRVP.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputRVP.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputRVP.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.udcClaimVaccineInputRVP.SubsidizeDisableDetail = Me.GetGlobalResourceObject("Text", "Detail")
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        'Text Field
        Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")

        Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
        Me.lblRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")

        If Me.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblRCHName.Visible = False
            Me.lblRCHNameChi.Visible = True
            Me.lblRCHNameChi.CssClass = "tableTextChi"
        Else
            Me.lblRCHName.Visible = True
            Me.lblRCHName.CssClass = "tableText"
            Me.lblRCHNameChi.Visible = False
        End If

    End Sub

    Protected Overrides Sub Setup()
        'If Me.rbCategorySelection.SelectedValue <> String.Empty Then
        '    If Me.rbCategorySelection.SelectedValue <> Me.Request.Form(Me.rbCategorySelection.UniqueID) Then
        '        rbCategory_SelectedIndexChanged(Me.rbCategorySelection, New EventArgs())
        '        Exit Sub
        '    End If
        'End If

        'BLL 
        Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        'Session Values
        Dim udtPersonalInformation As EHSPersonalInformationModel = Nothing
        If MyBase.EHSTransaction Is Nothing Then
            udtPersonalInformation = MyBase.EHSAccount.EHSPersonalInformationList.Filter(MyBase.EHSAccount.SearchDocCode)
        Else
            udtPersonalInformation = MyBase.EHSAccount.EHSPersonalInformationList.Filter(MyBase.EHSTransaction.DocCode)
        End If

        Dim udtSchemeClaim As SchemeClaimModel = MyBase.SessionHandler.SchemeSelectedGetFromSession(FunctCode)

        'Parameter 
        Dim strLanguage As String = MyBase.SessionHandler.Language()
        Dim dtRVPhomeList As DataTable
        Dim updateByTransactionModel As Boolean = False
        Dim strEnableClaimCategory As String = Nothing
        Dim noCategory As Boolean = False

        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

        If strEnableClaimCategory = "Y" Then
            If MyBase.ClaimCategorys Is Nothing OrElse MyBase.ClaimCategorys.Count = 0 Then
                Me.panClaimCategory.Visible = False
                Me.panRCHCode.Visible = False
                Me.rbCategorySelection.Items.Clear()
                updateByTransactionModel = False
                noCategory = True
            Else
                Me.panRCHCode.Visible = True
                noCategory = False
            End If
        Else
            Me.panRCHCode.Visible = True
            noCategory = False
        End If

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        If Not MyBase.EHSTransaction Is Nothing AndAlso New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(MyBase.EHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.RVP _
            AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.Count > 0 Then
            updateByTransactionModel = True
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        If strEnableClaimCategory = "Y" Then

            If Not noCategory Then
                Me.panClaimCategory.Visible = True

                Me.BindCategory(strLanguage, updateByTransactionModel, MyBase.ClaimCategorys)

                If Not MyBase.EHSClaimVaccine Is Nothing Then
                    Me.udcClaimVaccineInputRVP.Build(MyBase.EHSClaimVaccine)
                Else
                    Me.udcClaimVaccineInputRVP.Controls.Clear()
                End If

            End If

        Else
            Me.panClaimCategory.Visible = False
            Me.udcClaimVaccineInputRVP.Build(MyBase.EHSClaimVaccine)
        End If

        If updateByTransactionModel Then

            'RCH Code
            dtRVPhomeList = udtRVPHomeListBLL.getRVPHomeListActiveByCode(Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode)

            If dtRVPhomeList.Rows.Count > 0 Then
                Me.SetUpRCHInfo(dtRVPhomeList.Rows(0))
            End If

            'Category
            If strEnableClaimCategory = "Y" Then
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Me.rbCategorySelection.SelectedValue = Me.EHSTransaction.CategoryCode
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            End If

            Me.udcClaimVaccineInputRVP.Visible = True


        Else

            If strEnableClaimCategory = "N" OrElse (strEnableClaimCategory = "Y" AndAlso Not noCategory) Then
                ' Default Load From Session
                Dim udtSessionHandler As New BLL.SessionHandler()
                Dim strRCHCodeFromSession As String = udtSessionHandler.RVPRCHCodeGetFromSession(Common.Component.FunctCode.FUNT020201)

                If Not strRCHCodeFromSession Is Nothing AndAlso strRCHCodeFromSession.Trim() <> "" Then

                    ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    ' Reload inputted RCH Code if not empty, else reload from session
                    If Trim(Me.txtRCHCodeText.Text).Length > 0 Then
                        dtRVPhomeList = udtRVPHomeListBLL.getRVPHomeListActiveByCode(Me.txtRCHCodeText.Text)
                        If dtRVPhomeList.Rows.Count > 0 Then
                            Me.SetUpRCHInfo(dtRVPhomeList.Rows(0))
                        End If
                    Else
                        dtRVPhomeList = udtRVPHomeListBLL.getRVPHomeListActiveByCode(strRCHCodeFromSession)
                        If dtRVPhomeList.Rows.Count > 0 Then
                            Me.SetUpRCHInfo(dtRVPhomeList.Rows(0))
                        End If
                    End If
                    ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [End][Koala]
                End If
            End If
        End If


        If MyBase.AvaliableForClaim Then
            Me.txtRCHCodeText.Enabled = True
            Me.btnSearchRCH.Enabled = True
        Else
            Me.txtRCHCodeText.Enabled = False
            Me.btnSearchRCH.Enabled = False
        End If

        AddHandler Me.udcClaimVaccineInputRVP.VaccineLegendClicked, AddressOf udcClaimVaccineInputRVP_VaccineLegendClicked
        AddHandler Me.udcClaimVaccineInputRVP.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputRVP_SubsidizeDisabledRemarkClicked

        If Not MyBase.EHSClaimVaccine Is Nothing AndAlso MyBase.EHSClaimVaccine.SubsidizeList.Count > 0 Then

            'If turned on high risk option, the high risk option is shown.
            If HighRiskOptionShown = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                panRVPRecipientCondition.Visible = True
                BindRecipientCondition()

                'Recipient Condition
                Me.rblRecipientCondition.Enabled = False

                If MyBase.EHSClaimVaccine.IsSelectedSubsidizeWithHighRisk Then
                    Me.rblRecipientCondition.Enabled = True
                    SetHighRisk(SessionHandler.HighRiskGetFromSession(FunctCode))
                End If

                AddHandler Me.udcClaimVaccineInputRVP.SubsidizeCheckboxClickedEnableRecipientCondition, _
                    AddressOf udcClaimVaccineInputRVP_SubsidizeCheckboxClickedEnableRecipientCondition
            Else
                panRVPRecipientCondition.Visible = False
            End If

        End If

    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputRVP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.lblCategoryText.Width = width
            Me.lblRCHCodeText.Width = width
            Me.lblRCHNameText.Width = width
        Else
            Me.lblCategoryText.Width = 200
            Me.lblRCHCodeText.Width = 200
            Me.lblRCHNameText.Width = 200
        End If
    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        'MyBase.SetDoseErrorImage(blnVisible)
        If Not Me.udcClaimVaccineInputRVP Is Nothing Then
            Me.udcClaimVaccineInputRVP.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetRCHCodeError(ByVal visible As Boolean)
        Me.imgRCHCodeError.Visible = visible
    End Sub

    Public Sub SetCategoryError(ByVal visible As Boolean)
        Me.imgCategoryError.Visible = visible
    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Sub SetRecipientConditionError(ByVal visible As Boolean)
        Me.imgRecipientConditionError.Visible = visible
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Events"

    Private Sub rbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged
        Me.rbCategorySelection.SelectedValue = Me.Request.Form(Me.rbCategorySelection.UniqueID)
        Me.udcClaimVaccineInputRVP.Visible = False
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.panRVPRecipientCondition.Visible = False
        SetRecipientConditionError(False)
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        RaiseEvent CategorySelected(sender, e)
    End Sub

    Private Sub txtRCHCodeText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRCHCodeText.TextChanged
        If Me.txtRCHCodeText.Text.Trim() = String.Empty Then
            Me.lblRCHCode.Text = String.Empty
            Me.lblRCHName.Text = String.Empty
            Me.lblRCHNameChi.Text = String.Empty

            Dim udtSessionHandler As New BLL.SessionHandler()
            'udtSessionHandler.RVPRCHCodeRemoveFromSession(Common.Component.FunctCode.FUNT020201)
            udtSessionHandler.RVPRCHCodeRemoveFromSession(FunctCode)
        Else
            Me.lookUpRCHCode()
        End If
    End Sub

    Private Sub lookUpRCHCode()
        Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
        Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(Me.txtRCHCodeText.Text.Trim())
        Dim udtSessionHandler As New BLL.SessionHandler()

        If dtResult.Rows.Count > 0 Then
            Me.SetUpRCHInfo(dtResult.Rows(0))
            udtSessionHandler.RVPRCHCodeSaveToSession(FunctCode, dtResult.Rows(0)("RCH_Code").ToString().Trim().ToUpper())
            'udtSessionHandler.RVPRCHCodeSaveToSession(Common.Component.FunctCode.FUNT020201, dtResult.Rows(0)("RCH_Code").ToString().Trim().ToUpper())
        Else
            Me.lblRCHCode.Text = String.Empty
            Me.lblRCHName.Text = String.Empty
            Me.lblRCHNameChi.Text = String.Empty
            udtSessionHandler.RVPRCHCodeRemoveFromSession(FunctCode)
            ' udtSessionHandler.RVPRCHCodeRemoveFromSession(Common.Component.FunctCode.FUNT020201)
        End If
    End Sub

    Private Sub SetUpRCHInfo(ByVal drRVPHome As DataRow)
        Me.txtRCHCodeText.Text = drRVPHome("RCH_Code").ToString().Trim().ToUpper()
        Me.lblRCHCode.Text = Me.txtRCHCodeText.Text

        Me.lblRCHName.Text = drRVPHome("Homename_Eng").ToString().Trim()
        If drRVPHome.IsNull("Homename_Chi") Then
            Me.lblRCHNameChi.Text = Me.lblRCHName.Text
            Me.lblRCHNameChi.CssClass = "tableText"
        Else
            Me.lblRCHNameChi.Text = drRVPHome("Homename_Chi").ToString().Trim()
            Me.lblRCHNameChi.CssClass = "tableTextChi"
        End If
    End Sub

    Private Sub btnSearchRCH_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchRCH.Click
        RaiseEvent SearchButtonClick(sender, e)
    End Sub

    Protected Sub udcClaimVaccineInputRVP_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
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

    Protected Sub udcClaimVaccineInputRVP_SubsidizeCheckboxClickedEnableRecipientCondition(ByVal blnEnabled As Boolean)
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

    Protected Sub udcClaimVaccineInputRVP_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub

    Private Sub ImgBtnRecipientConditionHelp_Click(sender As Object, e As ImageClickEventArgs) Handles ImgBtnRecipientConditionHelp.Click
        RaiseEvent RecipientConditionHelpClick(sender, e)
    End Sub
#End Region

#Region "SetValue"

    Public Sub SetRCHCode(ByVal strRCHCode As String)
        Me.txtRCHCodeText.Text = strRCHCode.Trim().ToUpper()
        Me.lookUpRCHCode()
    End Sub

    Public Sub SetHighRisk(ByVal strHighRisk As String)
        If strHighRisk = String.Empty Then Return

        If Me.chkRecipientCondition.Visible Then
            Me.chkRecipientCondition.Checked = IIf(strHighRisk = YesNo.Yes, True, False)
        End If

        If Me.rblRecipientCondition.Visible Then
            'Me.cblRecipientCondition.SelectedValue = IIf(strHighRisk = YesNo.Yes, HighRiskOption.HIGHRISK, HighRiskOption.NOHIGHRISK)
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

    Public Sub ClearClaimDetail()
        Me.rbCategorySelection.SelectedIndex = -1
        Me.rbCategorySelection.SelectedValue = Nothing
        Me.rbCategorySelection.ClearSelection()

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.chkRecipientCondition.Checked = False
        Me.rblRecipientCondition.SelectedIndex = -1
        Me.rblRecipientCondition.SelectedValue = Nothing
        Me.rblRecipientCondition.ClearSelection()
        'CRE16-026 (Add PCV13) [End][Chris YIM]
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property RCHCode() As String
        Get
            'Return Me.lblRCHCode.Text
            Return Me.txtRCHCodeText.Text
        End Get
    End Property

    Public ReadOnly Property RCHName() As String
        Get
            Return Me.lblRCHName.Text
        End Get
    End Property

    Public ReadOnly Property RCHNameChi() As String
        Get
            Return Me.lblRCHNameChi.Text
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

                ' Save the User Input before clear it
                If Me.rblRecipientCondition.SelectedIndex > -1 AndAlso String.IsNullOrEmpty(Me.rblRecipientCondition.SelectedValue) = False Then
                    lstCheckboxListResult = New List(Of Boolean)

                    For i As Integer = 0 To rblRecipientCondition.Items.Count - 1
                        lstCheckboxListResult.Add(rblRecipientCondition.Items(i).Selected)
                    Next
                End If

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

                ' Restore the User Input after clear it
                If Not lstCheckboxListResult Is Nothing AndAlso lstCheckboxListResult.Count > 0 Then
                    For i As Integer = 0 To rblRecipientCondition.Items.Count - 1
                        rblRecipientCondition.Items(i).Selected = lstCheckboxListResult.Item(i)
                    Next
                End If

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
    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox, ByVal strEnableClaimCategory As String) As Boolean
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = _udtSessionHandler.EHSClaimVaccineGetFromSession()
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        If strEnableClaimCategory = YesNo.Yes Then
            'Check Category
            If String.IsNullOrEmpty(Category) Then
                blnResult = False
                SetCategoryError(True)
                objMsg = New ComObject.SystemMessage("990000", "E", "00238")
                objMsgBox.AddMessage(objMsg)
            End If
        End If

        'Check RCHCode
        If RCHCode.Equals(String.Empty) Then
            blnResult = False
            Me._udtSessionHandler.RVPRCHCodeRemoveFromSession(FunctCode)
            SetRCHCodeError(True)
            objMsg = New ComObject.SystemMessage("990000", "E", "00198")
            objMsgBox.AddMessage(objMsg)
        Else
            ' Check RCH Code Valid
            Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
            Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(RCHCode.Trim())
            If dtResult.Rows.Count = 0 Then
                blnResult = False
                Me._udtSessionHandler.RVPRCHCodeRemoveFromSession(FunctCode)
                SetRCHCodeError(True)
                objMsg = New ComObject.SystemMessage("990000", "E", "00219")
                objMsgBox.AddMessage(objMsg)
            End If
        End If

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

        'Check dose
        Dim blnVaccineValid As Boolean = udtEHSClaimVaccine.chkVaccineSelection(udtEHSClaimVaccine, objMsgBox)
        If Not blnVaccineValid Then
            SetDoseErrorImage(True)
        End If

        blnResult = blnResult And blnVaccineValid

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
    'CRE16-026 (Add PCV13) [End][Chris YIM]
#End Region

#Region "Save"
    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal strEnableClaimCategory As String)
        'Category
        Dim udtClaimCategory As ClaimCategoryModel = Me._udtSessionHandler.ClaimCategoryGetFromSession(FunctCode)
        ' -----------------------------------------------
        ' Set up Transaction Model Fields : Category
        '------------------------------------------------
        'Category
        If strEnableClaimCategory = YesNo.Yes Then
            udtEHSTransaction.CategoryCode = Category
        Else
            'Category Code Should be "RESIDENT"
            udtEHSTransaction.CategoryCode = udtClaimCategory.CategoryCode
        End If

        ' ----------------------------------------------------
        ' Set up Transaction Model Fields : High Risk Option
        '-----------------------------------------------------
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

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        ' -----------------------------------------------
        ' Get Latest SchemeSeq Selected
        '------------------------------------------------
        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

        If Not udtSubsidizeLatest Is Nothing Then
            ' ----------------------------------------------------
            ' Set up Transaction Model Additional Fields : RHCCode
            '-----------------------------------------------------
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = "RHCCode"
            udtTransactAdditionfield.AdditionalFieldValueCode = RCHCode
            udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
        End If

    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Other functions"

    Private Sub BindCategory(ByVal strLanguage As String, ByVal updateByTransactionModel As Boolean, ByVal udtClaimCategorys As ClaimCategoryModelCollection)
        Dim strSelectedValue As String
        Dim udtClaimCategory As ClaimCategoryModel = MyBase.SessionHandler.ClaimCategoryGetFromSession(FunctCode)
        Dim dtClaimCategory As DataTable

        If Not udtClaimCategory Is Nothing Then
            strSelectedValue = udtClaimCategory.CategoryCode
        Else
            strSelectedValue = Me.rbCategorySelection.SelectedValue
        End If

        Me.rbCategorySelection.Visible = False
        Me.lblCategory.Visible = False

        'Build Drop Down List
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
                Me.udcClaimVaccineInputRVP.Visible = True
            Else
                Me.udcClaimVaccineInputRVP.Visible = False
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


            Me.udcClaimVaccineInputRVP.Visible = True
        End If


    End Sub
#End Region

End Class