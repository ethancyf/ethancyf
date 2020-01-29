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

Partial Public Class ucInputPPP
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
        Public Const PPP_CHILD As Integer = 1
    End Class

#End Region

#Region "Properties"
    Public ReadOnly Property FunctCode() As String
        Get
            Return MyBase.FunctionCode
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

    Public ReadOnly Property SchoolCode() As String
        Get
            Return Me.Request.Form(Me.txtSchoolCode.UniqueID)
        End Get
    End Property

    Public ReadOnly Property EHSPPPTransaction() As EHSTransactionModel
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

        'Category Fields
        Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")

        'School Code & Name Fields
        Me.lblSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
        Me.lblSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")

        If Me.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblSchoolName.Visible = False
            Me.lblSchoolNameChi.Visible = True
            Me.lblSchoolNameChi.CssClass = "tableTextChi"
        Else
            Me.lblSchoolName.Visible = True
            Me.lblSchoolName.CssClass = "tableText"
            Me.lblSchoolNameChi.Visible = False
        End If

        'Vaccine Fields
        Me.udcClaimVaccineInputPPP.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputPPP.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputPPP.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputPPP.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputPPP.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputPPP.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputPPP.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputPPP.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")
        Me.udcClaimVaccineInputPPP.SubsidizeDisableDetail = Me.GetGlobalResourceObject("Text", "Detail")

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
            Me.panPPPCategory.Visible = False
            Me.ClearClaimDetail()
            Me.rbCategorySelection.Items.Clear()
            noCategory = True
        Else
            Me.panPPPCategory.Visible = True
        End If

        If Not noCategory Then
            Me.panPPPCategory.Visible = True

            Me.BindCategory(strLanguage, updateByTransactionModel, MyBase.ClaimCategorys)

            If Not MyBase.EHSClaimVaccine Is Nothing Then
                Me.udcClaimVaccineInputPPP.ShowLegend = MyBase.ShowLegend
                If Not blnPostbackRebuild Then
                    'Me.udcClaimVaccineInputVSS.Controls.Clear()
                    Me.udcClaimVaccineInputPPP.Build(MyBase.EHSClaimVaccine, MyBase.FunctionCode)
                    blnVaccineCreate = True
                End If
            Else
                Me.udcClaimVaccineInputPPP.Controls.Clear()
            End If

            'Load selected options from model if saved
            If Not MyBase.EHSTransaction Is Nothing AndAlso New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(MyBase.EHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.PPP _
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

                    Dim strSchoolCode As String = Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SchoolCode).AdditionalFieldValueCode
                    Me.SetSchoolCode(strSchoolCode)

                    'If Not blnPostbackRebuild And Not blnVaccineCreate And Not Session(claimTransManagement.SESS_OverrideReasonPopup) And Not Session(claimTransManagement.SESS_VaccinationRecordPopupShown) Then
                    '    'Me.udcClaimVaccineInputVSS.Controls.Clear()
                    '    Me.udcClaimVaccineInputVSS.Build(MyBase.EHSClaimVaccine, MyBase.FunctionCode)
                    'End If
                    udcClaimVaccineInputPPP.Visible = True
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

                'Category is selected and so show claim detail 
                showCategoryDetail(strSelectedCategoryCode)
                Me.udcClaimVaccineInputPPP.Visible = True

                AddHandler Me.udcClaimVaccineInputPPP.VaccineLegendClicked, AddressOf udcClaimVaccineInputPPP_VaccineLegendClicked
                AddHandler Me.udcClaimVaccineInputPPP.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputPPP_SubsidizeDisabledRemarkClicked

            Else
                Me.mvCategory.ActiveViewIndex = 0
                Me.udcClaimVaccineInputPPP.Visible = False

            End If
        End If

    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputPPP.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Overrides Sub Clear()
        txtSchoolCode.Text = String.Empty
        lblSchoolName.Text = String.Empty
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region

#Region "Set Up Error Image"

    Public Sub SetCategoryError(ByVal visible As Boolean)
        Me.imgCategoryError.Visible = visible
    End Sub

    Public Sub SetSchoolCodeError(ByVal visible As Boolean)
        Me.imgSchoolCodeError.Visible = visible
    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        If Not Me.udcClaimVaccineInputPPP Is Nothing Then
            Me.udcClaimVaccineInputPPP.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "SetValue"

    Public Sub SetSchoolCode(ByVal strSchoolCode As String, Optional ByVal strSchemeCode As String = "")
        Me.txtSchoolCode.Text = strSchoolCode.Trim().ToUpper()
        Me.lookUpSchoolCode()

    End Sub

    Public Sub FillClaimDetail()

        'Select Case mvCategory.ActiveViewIndex
        '    Case ViewIndexCategory.VSS_PID
        'If Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode) Is Nothing And _
        '    Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode) = String.Empty Then

        'Dim strDocumentaryProofValue As String = _udtSessionHandler.DocumentaryProofForPIDGetFromSession(FunctCode)

        '    If strDocumentaryProofValue = ucInputVSSPID.PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
        If Not _udtSessionHandler.SchoolCodeGetFromSession(FunctCode) Is Nothing And _
            Not _udtSessionHandler.SchoolCodeGetFromSession(FunctCode) = String.Empty Then

            Dim strSchoolCode As String = _udtSessionHandler.SchoolCodeGetFromSession(FunctCode)
            Me.SetSchoolCode(strSchoolCode)
            'Else
            '    Me.ucInputVSSPID.SetDocumentaryProofOptions(strDocumentaryProofValue)
        End If
        '    Else
        'Me.ucInputVSSPID.SetDocumentaryProofOptions(strDocumentaryProofValue)
        '    End If
        'End If
        'End Select

    End Sub

    Public Sub ClearClaimDetail()
        Me.rbCategorySelection.SelectedIndex = -1
        Me.rbCategorySelection.SelectedValue = Nothing
        Me.rbCategorySelection.ClearSelection()
        Me.mvCategory.ActiveViewIndex = -1

    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputPPP_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub

    Protected Sub udcClaimVaccineInputPPP_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    Private Sub btnSearchSchool_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchSchool.Click
        RaiseEvent SearchButtonClick(sender, e)
    End Sub

    Private Sub rbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged
        Me.rbCategorySelection.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rbCategorySelection.UniqueID), True)
        Me.udcClaimVaccineInputPPP.Controls.Clear()

        ' Claim Detail Input by Category
        showCategoryDetail(Me.rbCategorySelection.SelectedValue)

        If String.IsNullOrEmpty(Me.rbCategorySelection.SelectedValue) Then
            Me.udcClaimVaccineInputPPP.Visible = False
        Else
            Me.udcClaimVaccineInputPPP.Visible = True
        End If

        RaiseEvent CategorySelected(sender, e)

    End Sub

    Private Sub showCategoryDetail(ByVal strSelectedCategory As String)

        Select Case strSelectedCategory
            Case CategoryCode.PPP_CHILD, CategoryCode.PPPKG_CHILD
                mvCategory.ActiveViewIndex = ViewIndexCategory.PPP_CHILD

            Case Else
                mvCategory.ActiveViewIndex = ViewIndexCategory.NoCategory

        End Select

    End Sub

    Private Sub SchoolCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSchoolCode.TextChanged
        If Me.txtSchoolCode.Text.Trim() = String.Empty Then
            Me.lblSchoolCode.Text = String.Empty
            Me.lblSchoolName.Text = String.Empty
            Me.lblSchoolNameChi.Text = String.Empty

            _udtSessionHandler.SchoolCodeRemoveFromSession(MyBase.FunctionCode)
        Else
            Me.lookUpSchoolCode()

        End If
    End Sub

#End Region

#Region "School Code"

    Private Sub lookUpSchoolCode()
        Dim udtSchoolListBLL As New Common.Component.School.SchoolBLL()
        Dim dtResult As DataTable = udtSchoolListBLL.GetSchoolListActiveByCode(Me.txtSchoolCode.Text.Trim(), Me.SchemeClaim.SchemeCode)
        'Dim udtSessionHandler As New BLL.SessionHandler()

        If dtResult.Rows.Count > 0 Then
            Me.SetUpSchoolInfo(dtResult.Rows(0))
            '_udtSessionHandler.SchoolCodeSaveToSession(dtResult.Rows(0)("School_Code").ToString().Trim().ToUpper(), MyBase.FunctionCode)
        Else
            Me.lblSchoolCode.Text = String.Empty
            Me.lblSchoolName.Text = String.Empty
            Me.lblSchoolNameChi.Text = String.Empty
            '_udtSessionHandler.SchoolCodeRemoveFromSession(ucInputPPP.FunctCode)
        End If
    End Sub

    Private Sub SetUpSchoolInfo(ByVal drSchool As DataRow)

        Me.txtSchoolCode.Text = drSchool("School_Code").ToString().Trim().ToUpper()
        Me.lblSchoolCode.Text = AntiXssEncoder.HtmlEncode(Me.txtSchoolCode.Text, True)

        Me.lblSchoolName.Text = drSchool("Name_Eng").ToString().Trim()

        If drSchool.IsNull("Name_Chi") Then
            Me.lblSchoolNameChi.Text = Me.lblSchoolName.Text
            Me.lblSchoolNameChi.CssClass = "tableText"
        Else
            Me.lblSchoolNameChi.Text = drSchool("Name_Chi").ToString().Trim()
            Me.lblSchoolNameChi.CssClass = "tableTextChi"
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

        'Check School Code
        Dim strSymbol As String = String.Empty
        Dim strReplaceMessage As String = String.Empty
        objMsg = ValidateSchoolCode(blnShowErrorImage, strSymbol, strReplaceMessage)
        If Not objMsg Is Nothing Then
            blnResult = False

            If objMsgBox IsNot Nothing Then
                If strSymbol <> String.Empty And strReplaceMessage <> String.Empty Then
                    objMsgBox.AddMessage(objMsg, strSymbol, strReplaceMessage)
                Else
                    objMsgBox.AddMessage(objMsg)
                End If

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

    Public Function ValidateSchoolCode(ByVal blnShowErrorImage As Boolean, ByRef strSybmol As String, ByRef strReplace As String) As ComObject.SystemMessage
        Me.imgSchoolCodeError.Visible = False

        If txtSchoolCode.Text = String.Empty Then
            Me.imgSchoolCodeError.Visible = blnShowErrorImage
            strSybmol = "%s"
            strReplace = lblSchoolCodeText.Text
            Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028) ' Please input "School Code"
        Else
            'Check "School Code" Valid
            Dim udtSchoolListBLL As New Common.Component.School.SchoolBLL()
            Dim dtResult As DataTable = udtSchoolListBLL.GetSchoolListActiveByCode(Me.txtSchoolCode.Text.Trim, Me.SchemeClaim.SchemeCode)

            If dtResult.Rows.Count = 0 Then
                '_udtSessionHandler.SchoolCodeRemoveFromSession(ucInputPPP.FunctCode)
                Me.imgSchoolCodeError.Visible = blnShowErrorImage
                strSybmol = "%s"
                strReplace = lblSchoolCodeText.Text
                Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00365) ' "School Code" is invalid

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

        'HighRisk
        udtEHSTransaction.HighRisk = String.Empty

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        ' -----------------------------------------------
        ' Get Latest SchemeSeq Selected
        '------------------------------------------------
        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

        'School Code
        If Not udtSubsidizeLatest Is Nothing Then
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SchoolCode
            udtTransactAdditionfield.AdditionalFieldValueCode = Me.SchoolCode
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            '_udtSessionHandler.SchoolCodeSaveToSession(Me.SchoolCode, MyBase.FunctionCode)

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
            strSelectedValue = AntiXssEncoder.HtmlEncode(Me.rbCategorySelection.SelectedValue, True)
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
                Me.udcClaimVaccineInputPPP.Visible = True
            Else
                Me.udcClaimVaccineInputPPP.Visible = False
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

            Me.udcClaimVaccineInputPPP.Visible = True
        End If

    End Sub
#End Region

End Class
