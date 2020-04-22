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

Partial Public Class ucInputENHVSSO
    Inherits ucInputEHSClaimBase

    Dim _udtSessionHandler As New SessionHandlerBLL
    Dim _udtGeneralFunction As New GeneralFunction

#Region "Constants"

    Private Class ClinicType
        Public Const Clinic As String = "C"
        Public Const NonClinic As String = "N"
    End Class

    Private Class ViewIndexCategory
        Public Const NoCategory As Integer = 0
        Public Const EVSSO_CHILD As Integer = 1
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

    Public ReadOnly Property EHSENHVSSOTransaction() As EHSTransactionModel
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

#End Region

#Region "Event handlers"
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.udcClaimVaccineInputENHVSSO.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputENHVSSO.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputENHVSSO.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputENHVSSO.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputENHVSSO.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputENHVSSO.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputENHVSSO.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputENHVSSO.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")
        Me.udcClaimVaccineInputENHVSSO.SubsidizeDisableDetail = Me.GetGlobalResourceObject("Text", "Detail")

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
            Me.panENHVSSOCategory.Visible = False
            Me.ClearClaimDetail()
            Me.rbCategorySelection.Items.Clear()
            noCategory = True
        Else
            Me.panENHVSSOCategory.Visible = True
        End If

        If Not noCategory Then
            Me.panENHVSSOCategory.Visible = True

            Me.BindCategory(strLanguage, updateByTransactionModel, MyBase.ClaimCategorys)

            If Not MyBase.EHSClaimVaccine Is Nothing Then
                Me.udcClaimVaccineInputENHVSSO.ShowLegend = MyBase.ShowLegend
                If Not blnPostbackRebuild Then
                    'Me.udcClaimVaccineInputVSS.Controls.Clear()
                    Me.udcClaimVaccineInputENHVSSO.Build(MyBase.EHSClaimVaccine, MyBase.FunctionCode)
                    blnVaccineCreate = True
                End If
            Else
                Me.udcClaimVaccineInputENHVSSO.Controls.Clear()
            End If

            'Show dropdown of "Place of Vaccination"
            BindPlaceOfVaccinationDropDown(MyBase.EHSClaimVaccine)

            'Load selected options from model if saved
            If Not MyBase.EHSTransaction Is Nothing AndAlso New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(MyBase.EHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.ENHVSSO _
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

                    Dim strPlaceVaccinationValue As String = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination).AdditionalFieldValueCode
                    Me.ddlPlaceOfVaccination.SelectedValue = strPlaceVaccinationValue

                    If strPlaceVaccinationValue = PlaceOfVaccinationOptions.OTHERS Then
                        trPlaceOfVaccinationOther.Style.Add("display", "initial")
                        txtPlaceOfVaccinationOther.Text = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination).AdditionalFieldValueDesc
                    Else
                        trPlaceOfVaccinationOther.Style.Add("display", "none")
                    End If

                    'If Not blnPostbackRebuild And Not blnVaccineCreate And Not Session(claimTransManagement.SESS_OverrideReasonPopup) And Not Session(claimTransManagement.SESS_VaccinationRecordPopupShown) Then
                    '    'Me.udcClaimVaccineInputVSS.Controls.Clear()
                    '    Me.udcClaimVaccineInputVSS.Build(MyBase.EHSClaimVaccine, MyBase.FunctionCode)
                    'End If

                    udcClaimVaccineInputENHVSSO.Visible = True
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

                'Category is selected but no High risk option and so show claim detail 
                showCategoryDetail(strSelectedCategoryCode)
                Me.udcClaimVaccineInputENHVSSO.Visible = True
                Me.panENHVSSOPlaceOfVaccination.Visible = True

                AddHandler Me.udcClaimVaccineInputENHVSSO.VaccineLegendClicked, AddressOf udcClaimVaccineInputENHVSSO_VaccineLegendClicked
                AddHandler Me.udcClaimVaccineInputENHVSSO.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputENHVSSO_SubsidizeDisabledRemarkClicked

            Else
                Me.mvCategory.ActiveViewIndex = 0
                Me.panENHVSSOPlaceOfVaccination.Visible = False
                Me.udcClaimVaccineInputENHVSSO.Visible = False

            End If
        End If

    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputENHVSSO.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        If Not Me.udcClaimVaccineInputENHVSSO Is Nothing Then
            Me.udcClaimVaccineInputENHVSSO.SetDoseErrorImage(blnVisible)
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

#End Region

#Region "SetValue"

    Public Sub FillClaimDetail()

        If ddlPlaceOfVaccination.Enabled Then

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

        Me.ddlPlaceOfVaccination.SelectedIndex = -1
        Me.ddlPlaceOfVaccination.SelectedValue = Nothing
        Me.ddlPlaceOfVaccination.ClearSelection()
        Me.panENHVSSOPlaceOfVaccination.Visible = False
        trPlaceOfVaccinationOther.Style.Add("display", "none")
        txtPlaceOfVaccinationOther.Text = String.Empty

    End Sub

    Public Sub ClearClaimDetailWithoutCategory()
        Me.ddlPlaceOfVaccination.SelectedIndex = -1
        Me.ddlPlaceOfVaccination.SelectedValue = Nothing
        Me.ddlPlaceOfVaccination.ClearSelection()
        trPlaceOfVaccinationOther.Style.Add("display", "none")
        txtPlaceOfVaccinationOther.Text = String.Empty

    End Sub
#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputENHVSSO_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub

    Protected Sub udcClaimVaccineInputENHVSSO_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    Private Sub rbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged
        SetPlaceOfVaccinationError(False)

        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
        Me.rbCategorySelection.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rbCategorySelection.UniqueID), True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
        Me.udcClaimVaccineInputENHVSSO.Controls.Clear()
        Me.ClearClaimDetailWithoutCategory()

        ' Claim Detail Input by Category
        showCategoryDetail(Me.rbCategorySelection.SelectedValue)

        If String.IsNullOrEmpty(Me.rbCategorySelection.SelectedValue) Then
            Me.udcClaimVaccineInputENHVSSO.Visible = False
        Else
            Me.udcClaimVaccineInputENHVSSO.Visible = True
        End If

        RaiseEvent CategorySelected(sender, e)

    End Sub

    Private Sub showCategoryDetail(ByVal strSelectedCategory As String)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case strSelectedCategory
            Case CategoryCode.EVSSO_CHILD
                mvCategory.ActiveViewIndex = ViewIndexCategory.EVSSO_CHILD

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

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("ENHVSSO_PLACEOFVACCINATION")

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
        If _udtSessionHandler.NonClinicSettingGetFromSession(FunctCode) Then
            objMsg = ValidatePlaceOfVaccination(blnShowErrorImage)
            If objMsg IsNot Nothing Then
                blnResult = False

                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
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

        'High Risk
        udtEHSTransaction.HighRisk = String.Empty

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        ' -----------------------------------------------
        ' Get Latest SchemeSeq Selected
        '------------------------------------------------
        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

        If Not udtSubsidizeLatest Is Nothing Then
            'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType
            'udtTransactAdditionfield.AdditionalFieldValueCode = ClinicType.Clinic
            'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            'udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            'udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            'udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            'udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Place Of Vaccination
            If Me.PlaceOfVaccination <> String.Empty Then
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
                Me.udcClaimVaccineInputENHVSSO.Visible = True
            Else
                Me.udcClaimVaccineInputENHVSSO.Visible = False
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

            Me.udcClaimVaccineInputENHVSSO.Visible = True
        End If

    End Sub
#End Region

End Class
