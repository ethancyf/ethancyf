Imports Common
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory
Imports System.Web.Security.AntiXss
Imports System.Web.Script.Serialization
Imports System.Linq
Imports Common.Component.EHSClaimVaccine


Partial Public Class ucInputCOVID19RVP
    Inherits ucInputEHSClaimBase

    Private _udtSessionHandler As New BLL.SessionHandlerBLL
    Private _udtGeneralFunction As New GeneralFunction
    Private _udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
    Private _strRCHType As String

#Region "Constants"

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

    'Public ReadOnly Property DropDownListCategory() As String
    '    Get
    '        If Me.ddlCCategoryCovid19.SelectedItem Is Nothing Then
    '            Return String.Empty
    '        Else
    '            Return Me.ddlCCategoryCovid19.SelectedItem.Value
    '        End If
    '    End Get
    'End Property

    'Public ReadOnly Property AvaibleSubsidy() As Boolean
    '    Get
    '        Dim blnAvaibleSubsidy As Boolean = False
    '        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In MyBase.EHSClaimVaccine.SubsidizeList
    '            If udtEHSClaimSubsidize.Available Then
    '                blnAvaibleSubsidy = True
    '                Exit For
    '            End If
    '        Next
    '        Return blnAvaibleSubsidy
    '    End Get
    'End Property

    'Public ReadOnly Property IsClaimCOVID19() As Boolean
    '    Get
    '        Return (New BLL.SessionHandlerBLL).ClaimCOVID19GetFromSession()
    '    End Get
    'End Property

    Public ReadOnly Property CategoryForCOVID19() As String
        Get
            If Me.ddlCCategoryCovid19.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCCategoryCovid19.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property RCHCode() As String
        Get
            Return Me.txtRCHCodeText.Text.Trim
        End Get
    End Property

    Public ReadOnly Property RCHType() As String
        Get
            Return Me._strRCHType
        End Get
    End Property

    Public ReadOnly Property RCHName() As String
        Get
            Return Me.lblRCHName.Text
        End Get
    End Property

    Public ReadOnly Property VaccineBrand() As String
        Get
            'If Me.txtCVaccineBrand.Text = "" Then
            '    Return String.Empty
            'Else
            '    Return Me.txtCVaccineBrand.Text
            'End If
            If Me.ddlCVaccineBrandCovid19.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCVaccineBrandCovid19.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property VaccineLotNo() As String
        Get
            'If Me.txtCVaccineLotNo.Text = "" Then
            '    Return String.Empty
            'Else
            '    Return Me.txtCVaccineLotNo.Text
            'End If

            If Me.ddlCVaccineLotNoCovid19.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCVaccineLotNoCovid19.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property DoseForCOVID19() As String
        Get
            If Me.ddlCDoseCovid19.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCDoseCovid19.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property ContactNo() As String
        Get
            If Me.txtCContactNo Is Nothing Then
                Return String.Empty
            Else
                Return Me.txtCContactNo.Text
            End If
        End Get
    End Property

    Public ReadOnly Property Remarks() As String
        Get
            If Me.txtCRemark Is Nothing Then
                Return String.Empty
            Else
                Return Me.txtCRemark.Text
            End If
        End Get
    End Property
#End Region

#Region "Event handlers"
    Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.udcClaimVaccineInputCOVID19.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputCOVID19.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputCOVID19.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputCOVID19.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputCOVID19.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputCOVID19.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputCOVID19.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputCOVID19.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

        Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
        Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
        Me.lblRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")

        Me.lblRCHName.Visible = True
        Me.lblRCHNameChi.Visible = False

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
            Me.panCOVID19Category.Visible = False
            Me.ClearClaimDetail()
            Me.rbCategorySelection.Items.Clear()
            noCategory = True
        Else
            Me.panCOVID19Category.Visible = True
            Me.tblCOVID19Category.Style.Add("display", "none")
        End If

        If Not noCategory Then
            Me.panCOVID19Category.Visible = True
            Me.tblCOVID19Category.Style.Add("display", "none")

            Me.BindCategory(strLanguage, updateByTransactionModel, MyBase.ClaimCategorys)

            Dim strServiceDate = Me.ServiceDate()

            If Not MyBase.EHSClaimVaccine Is Nothing Then
                Me.udcClaimVaccineInputCOVID19.ShowLegend = MyBase.ShowLegend
                If Not blnPostbackRebuild Then
                    'Me.udcClaimVaccineInputVSS.Controls.Clear()
                    Me.udcClaimVaccineInputCOVID19.Build(MyBase.EHSClaimVaccine, MyBase.FunctionCode)
                    blnVaccineCreate = True
                End If
            Else
                Me.udcClaimVaccineInputCOVID19.Controls.Clear()
            End If

            'Load selected options from model if saved
            If Not MyBase.EHSTransaction Is Nothing AndAlso New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(MyBase.EHSTransaction.SchemeCode) = SchemeClaimModel.EnumControlType.VSS _
                AndAlso Not MyBase.EHSTransaction.TransactionAdditionFields Is Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields.Count > 0 Then
                updateByTransactionModel = True

                If updateByTransactionModel Then
                    'RCH Code
                    Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
                    Dim dtRVPhomeList As DataTable

                    dtRVPhomeList = udtRVPHomeListBLL.getRVPHomeListByCode(Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode)

                    If dtRVPhomeList.Rows.Count > 0 Then
                        Me.SetUpRCHInfo(dtRVPhomeList.Rows(0))
                    End If

                    'Category
                    Me.rbCategorySelection.SelectedValue = Me.EHSTransaction.CategoryCode

                    Dim strSelectedCategoryCode As String = String.Empty
                    If Me.rbCategorySelection.Visible Then
                        strSelectedCategoryCode = Me.rbCategorySelection.SelectedValue
                    End If

                    If Me.lblCategory.Visible Then
                        strSelectedCategoryCode = lblCategory.Attributes("SelectedValue")
                    End If

                    udcClaimVaccineInputCOVID19.Visible = True
                End If
            End If

            'Fill data
            If Not IsPostBack Or _udtSessionHandler.ChangeSchemeInPracticeGetFromSession(FunctCode) Then
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

                Me.panCOVID19Detail.Visible = True
                Me.udcClaimVaccineInputCOVID19.Visible = True

                AddHandler Me.udcClaimVaccineInputCOVID19.VaccineLegendClicked, AddressOf udcClaimVaccineInputCOVID19_VaccineLegendClicked
                AddHandler Me.udcClaimVaccineInputCOVID19.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputCOVID19_SubsidizeDisabledRemarkClicked
            Else
                Me.panCOVID19Detail.Visible = False
                Me.udcClaimVaccineInputCOVID19.Visible = False

            End If

            ''Bind DropDownList Category
            'BindCOVID19Category(MyBase.ClaimCategorys)

            'Recipient Type
            ' Fill value by temp save
            If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                If MyBase.EHSTransaction.TransactionAdditionFields.RecipientType IsNot Nothing Then
                    For Each li As ListItem In rblCRecipientType.Items
                        If MyBase.EHSTransaction.TransactionAdditionFields.RecipientType = li.Value Then
                            Me.rblCRecipientType.SelectedValue = li.Value
                        End If
                    Next
                End If
            End If

            'Bind RadioButtonList RecipientType
            BindRecipientType()

            'Get Vaccine Brand & Lot No.
            Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
            Dim dtVaccineLotNo As DataTable = Nothing

            dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForCentre(CurrentPractice.SPID, CurrentPractice.PracticeID)

            If dtVaccineLotNo.Rows.Count > 0 Then

                Dim drVaccineLotNo() As DataRow = dtVaccineLotNo.Select()

                'Bind DropDownList Vaccine Brand
                BindCOVID19VaccineBrand(drVaccineLotNo)

                ' Fill brand value by temp save
                If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                    Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

                    'Vaccine
                    If udtTAFList.VaccineBrand IsNot Nothing Then
                        Dim strVaccineBrand As String = udtTAFList.VaccineBrand.Trim

                        For Each li As ListItem In ddlCVaccineBrandCovid19.Items
                            If strVaccineBrand.ToUpper.Trim = li.Value Then
                                ddlCVaccineBrandCovid19.SelectedValue = li.Value
                            End If
                        Next
                    End If

                End If

                'Bind DropDownList Vaccine Lot No.
                BindCOVID19VaccineLotNo(drVaccineLotNo)

                ' Fill value by temp save
                If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                    Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

                    'Lot No.
                    If udtTAFList.VaccineLotNo IsNot Nothing Then
                        Dim strVaccineLotNo As String = udtTAFList.VaccineLotNo.Trim

                        For Each li As ListItem In ddlCVaccineLotNoCovid19.Items
                            If strVaccineLotNo.Trim = li.Value Then
                                ddlCVaccineLotNoCovid19.SelectedValue = li.Value
                            End If
                        Next
                    End If
                End If

            Else
                ddlCVaccineBrandCovid19.Enabled = False
                ddlCVaccineLotNoCovid19.Enabled = False

            End If

            'Display or hide "Join eHealth"
            Dim blnDisplayJoinEHRSS As Boolean = False
            If MyBase.EHSAccount.SearchDocCode IsNot Nothing Then
                trJoinEHRSS.Style.Add("display", "none")

                If Me.DisplayJoinEHRSS(MyBase.EHSAccount) Then
                    trJoinEHRSS.Style.Remove("display")
                    blnDisplayJoinEHRSS = True
                End If
            End If

            ' Fill value by temp save
            If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

                'Remark
                If udtTAFList.Remarks IsNot Nothing Then
                    txtCRemark.Text = udtTAFList.Remarks.Trim
                End If

                'Join eHealth
                If udtTAFList.JoinEHRSS IsNot Nothing Then
                    chkCJoinEHRSS.Checked = IIf(udtTAFList.JoinEHRSS = YesNo.Yes, True, False)
                End If

            End If

            Select Case rblCRecipientType.SelectedValue
                Case RECIPIENT_TYPE.RESIDENT
                    trCContactNo.Style.Add("display", "none")
                    trJoinEHRSS.Style.Add("display", "none")
                Case RECIPIENT_TYPE.RCH_STAFF, RECIPIENT_TYPE.CCSU_STAFF
                    trCContactNo.Style.Remove("display")
                    If MyBase.EHSAccount IsNot Nothing AndAlso MyBase.EHSAccount.SearchDocCode IsNot Nothing Then
                        If blnDisplayJoinEHRSS Then
                            trJoinEHRSS.Style.Remove("display")
                        End If
                    End If
                Case Else
                    trCContactNo.Style.Add("display", "none")
                    trJoinEHRSS.Style.Add("display", "none")
            End Select

            ''Assign Subsidize Display Code
            'If EHSClaimVaccine IsNot Nothing Then
            '    lblCVaccine.Text = EHSClaimVaccine.SubsidizeList(0).SubsidizeDisplayCode
            'End If

            ''Bind DropDownList Dose
            ''BindCOVID19Dose()

        End If

    End Sub

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputCOVID19.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        If Not Me.udcClaimVaccineInputCOVID19 Is Nothing Then
            Me.udcClaimVaccineInputCOVID19.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetDetailError(ByVal blnVisible As Boolean)
        Me.imgCRecipientTypeError.Visible = blnVisible
        Me.imgRCHCodeError.Visible = blnVisible
        Me.imgCVaccineBrandError.Visible = blnVisible
        Me.imgCVaccineLotNoError.Visible = blnVisible
        Me.imgCContactNoError.Visible = blnVisible
    End Sub

    Public Sub SetCategoryError(ByVal visible As Boolean)
        Me.imgCategoryError.Visible = visible
    End Sub

    'Public Sub SetCategoryForCOVID19Error(ByVal visible As Boolean)
    '    Me.imgCCategoryError.Visible = visible
    'End Sub

    Public Sub SetRecipientTypeError(ByVal visible As Boolean)
        Me.imgCRecipientTypeError.Visible = visible
    End Sub

    Public Sub SetRCHCodeError(ByVal visible As Boolean)
        Me.imgRCHCodeError.Visible = visible
    End Sub

    Public Sub SetVaccineBrandError(ByVal visible As Boolean)
        Me.imgCVaccineBrandError.Visible = visible
    End Sub

    Public Sub SetVaccineLotNoError(ByVal visible As Boolean)
        Me.imgCVaccineLotNoError.Visible = visible
    End Sub

    'Public Sub SetDoseForCOVID19Error(ByVal visible As Boolean)
    '    Me.imgCDoseError.Visible = visible
    'End Sub

    Public Sub SetContactNoCOVID19Error(ByVal visible As Boolean)
        Me.imgCContactNoError.Visible = visible
    End Sub

#End Region

#Region "SetValue"

    Public Sub ClearClaimDetail()

        Me.ddlCCategoryCovid19.SelectedIndex = -1
        Me.ddlCCategoryCovid19.SelectedValue = Nothing
        Me.ddlCCategoryCovid19.ClearSelection()

        Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
        Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
        Me.ddlCVaccineBrandCovid19.ClearSelection()

        Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
        Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
        Me.ddlCVaccineLotNoCovid19.ClearSelection()

        Me.ddlCDoseCovid19.SelectedIndex = -1
        Me.ddlCDoseCovid19.SelectedValue = Nothing
        Me.ddlCDoseCovid19.ClearSelection()

        Me.txtCContactNo.Text = String.Empty
        Me.txtCRemark.Text = String.Empty

    End Sub

    Public Sub SetRCHCode(ByVal strRCHCode As String)
        Me.txtRCHCodeText.Text = strRCHCode.Trim().ToUpper()
        Me.lookUpRCHCode()
    End Sub

    Private Function DisplayJoinEHRSS(ByVal udtEHSAccount As EHSAccountModel) As Boolean
        Dim blnRes As Boolean = False
        Dim intAge As Integer

        If Not Integer.TryParse(_udtGeneralFunction.GetSystemParameterParmValue1("AgeLimitForJoinEHRSS"), intAge) Then
            Throw New Exception(String.Format("Invalid value({0}) is not a integer in DB table SystemParameter(AgeLimitForJoinEHRSS).", intAge))
        End If

        Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

        If Not CompareEligibleRuleByAge(Me.ServiceDate, udtPersonalInfo, intAge, "<", "Y", "DAY3") Then
            If COVID19.COVID19BLL.DisplayJoinEHRSS(udtEHSAccount) Then
                blnRes = True
            End If
        End If

        Return blnRes

    End Function

#End Region

#Region "Events"

    Private Sub btnSearchRCH_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchRCH.Click
        RaiseEvent SearchButtonClick(sender, e)
    End Sub

    Private Sub rbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged
        Me.rbCategorySelection.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rbCategorySelection.UniqueID), True)

        Me.SetVaccineBrandError(False)
        Me.SetVaccineLotNoError(False)
        'Me.SetContactNoCOVID19Error(False)

        Me.udcClaimVaccineInputCOVID19.Controls.Clear()

        If String.IsNullOrEmpty(Me.rbCategorySelection.SelectedValue) Then
            Me.udcClaimVaccineInputCOVID19.Visible = False
        Else
            Me.udcClaimVaccineInputCOVID19.Visible = True
        End If

        RaiseEvent CategorySelected(sender, e)
    End Sub

    Private Sub txtRCHCodeText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRCHCodeText.TextChanged
        If Me.txtRCHCodeText.Text.Trim() = String.Empty Then
            Me.lblRCHCode.Text = String.Empty
            Me.lblRCHName.Text = String.Empty
            Me.lblRCHNameChi.Text = String.Empty

            MyBase.SessionHandler.RVPRCHCodeRemoveFromSession(MyBase.FunctionCode)
        Else
            Me.lookUpRCHCode()
        End If
    End Sub

    Private Sub lookUpRCHCode()
        Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
        Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListByCode(Me.txtRCHCodeText.Text.Trim())

        Dim drResult() As DataRow = dtResult.Select("Type IN ('E','D')")

        If drResult.Length > 0 Then
            Dim dtRCH As DataTable = drResult.CopyToDataTable
            Me.SetUpRCHInfo(dtRCH.Rows(0))
            MyBase.SessionHandler.RVPRCHCodeSaveToSession(MyBase.FunctionCode, dtRCH.Rows(0)("RCH_Code").ToString().Trim().ToUpper())
        Else
            Me.lblRCHCode.Text = String.Empty
            Me.lblRCHName.Text = String.Empty
            Me.lblRCHNameChi.Text = String.Empty
            MyBase.SessionHandler.RVPRCHCodeRemoveFromSession(MyBase.FunctionCode)
        End If
    End Sub

    Private Sub SetUpRCHInfo(ByVal drRVPHome As DataRow)
        Me.txtRCHCodeText.Text = drRVPHome("RCH_Code").ToString().Trim().ToUpper()
        Me.lblRCHCode.Text = AntiXssEncoder.HtmlEncode(txtRCHCodeText.Text, True)
        Me._strRCHType = drRVPHome("Type").ToString.Trim

        Me.lblRCHName.Text = drRVPHome("Homename_Eng").ToString().Trim()

        If drRVPHome.IsNull("Homename_Chi") Then
            Me.lblRCHNameChi.Text = Me.lblRCHName.Text
        Else
            Me.lblRCHNameChi.Text = drRVPHome("Homename_Chi").ToString().Trim()
        End If

    End Sub

    Protected Sub udcClaimVaccineInputCOVID19_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub

    Protected Sub udcClaimVaccineInputCOVID19_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    'Private Sub ddlCBooth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCBooth.SelectedIndexChanged
    '    Me.ddlCBooth.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCBooth.UniqueID), True)
    '    SessionHandler.ClaimCOVID19BoothSaveToSession(Me.ddlCBooth.SelectedValue, FunctCode)

    'End Sub

    'Private Sub ddlCCategoryCovid19_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCCategoryCovid19.SelectedIndexChanged
    '    Me.ddlCCategoryCovid19.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCCategoryCovid19.UniqueID), True)
    '    SessionHandler.ClaimCOVID19CategorySaveToSession(Me.ddlCCategoryCovid19.SelectedValue, FunctCode)

    'End Sub

    Private Sub ddlCVaccineBrandCovid19_selectedindexchanged(sender As Object, e As EventArgs) Handles ddlCVaccineBrandCovid19.SelectedIndexChanged
        Me.ddlCVaccineBrandCovid19.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCVaccineBrandCovid19.UniqueID), True)
        'sessionhandler.claimcovid19vaccinebrandsavetosession(Me.ddlcvaccinebrandcovid19.selectedvalue, functcode)

        'Get Vaccine Brand & Lot No.
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim dtVaccineLotNo As DataTable = Nothing

        dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForCentre(CurrentPractice.SPID, CurrentPractice.PracticeID)

        If dtVaccineLotNo.Rows.Count > 0 Then

            Dim drVaccineLotNo() As DataRow = dtVaccineLotNo.Select()

            ''Bind DropDownList Vaccine Brand
            'BindCOVID19VaccineBrand(drVaccineLotNo)

            'Bind DropDownList Vaccine Lot No.
            BindCOVID19VaccineLotNo(drVaccineLotNo)

        End If

    End Sub

    'Private Sub ddlCVaccineLotNoCovid19_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCVaccineLotNoCovid19.SelectedIndexChanged
    '    Me.ddlCVaccineLotNoCovid19.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCVaccineLotNoCovid19.UniqueID), True)
    '    'SessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(Me.ddlCVaccineLotNoCovid19.SelectedValue, FunctCode)
    'End Sub

    Private Sub rblCRecipientType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblCRecipientType.SelectedIndexChanged

    End Sub

#End Region

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        'Check Category
        If String.IsNullOrEmpty(Me.Category) Then
            blnResult = False

            Me.SetCategoryError(True)

            objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00238)
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
        End If

        ''Check Category
        'If String.IsNullOrEmpty(Me.CategoryForCOVID19) Then
        '    blnResult = False

        '    Me.SetCategoryForCOVID19Error(True)

        '    objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
        '    If objMsgBox IsNot Nothing Then
        '        objMsgBox.AddMessage(objMsg, _
        '                             New String() {"%en", "%tc", "%sc"}, _
        '                             New String() {HttpContext.GetGlobalResourceObject("Text", "Category", New System.Globalization.CultureInfo(CultureLanguage.English)), _
        '                                           HttpContext.GetGlobalResourceObject("Text", "Category", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
        '                                           HttpContext.GetGlobalResourceObject("Text", "Category", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
        '                                           })
        '    End If
        'End If

        'Check Recipient Type
        If String.IsNullOrEmpty(Me.rblCRecipientType.SelectedValue) Then
            blnResult = False

            Me.SetRecipientTypeError(True)

            objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
            If objMsgBox IsNot Nothing Then
                objMsgBox.AddMessage(objMsg, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {HttpContext.GetGlobalResourceObject("Text", "RecipientType", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "RecipientType", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                   HttpContext.GetGlobalResourceObject("Text", "RecipientType", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                   })
            End If
        End If

        'Check RCHCode
        If RCHCode.Equals(String.Empty) Then
            blnResult = False
            'Me.SessionHandler.RVPRCHCodeRemoveFromSession(FunctCode)
            SetRCHCodeError(True)
            objMsg = New ComObject.SystemMessage("990000", "E", "00198")
            objMsgBox.AddMessage(objMsg)
        Else
            ' Check RCH Code Valid
            Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
            Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListByCode(RCHCode.Trim())

            Dim drResult() As DataRow = dtResult.Select("Type IN ('E','D')")

            If drResult.Length = 0 Then
                blnResult = False
                'Me.SessionHandler.RVPRCHCodeRemoveFromSession(FunctCode)
                SetRCHCodeError(True)
                objMsg = New ComObject.SystemMessage("990000", "E", "00219")
                objMsgBox.AddMessage(objMsg)
            Else
                Me._strRCHType = drResult(0)("Type").ToString.Trim
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

        ''Check Dose
        'If String.IsNullOrEmpty(Me.DoseForCOVID19) Then
        '    blnResult = False

        '    Me.SetDoseForCOVID19Error(True)

        '    objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
        '    If objMsgBox IsNot Nothing Then
        '        objMsgBox.AddMessage(objMsg, _
        '                             New String() {"%en", "%tc", "%sc"}, _
        '                             New String() {HttpContext.GetGlobalResourceObject("Text", "DoseSeq", New System.Globalization.CultureInfo(CultureLanguage.English)), _
        '                                           HttpContext.GetGlobalResourceObject("Text", "DoseSeq", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
        '                                           HttpContext.GetGlobalResourceObject("Text", "DoseSeq", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
        '                                           })
        '    End If
        'End If

        'If String.IsNullOrEmpty(Me.txtStep2aContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
        '    blnResult = False

        '    Me.SetContactNoCOVID19Error(True)

        '    Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
        '    If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, _
        '                                                        New String() {"%en", "%tc", "%sc"}, _
        '                                                        New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
        '                                                                        HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
        '                                                                        HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
        'End If

        'If Not String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
        '    If Not Regex.IsMatch(Me.txtCContactNo.Text, "^\d{8}$") Then
        '        blnResult = False

        '        Me.SetContactNoCOVID19Error(True)

        '        Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
        '        If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, _
        '                                                            New String() {"%en", "%tc", "%sc"}, _
        '                                                            New String() {HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.English)), _
        '                                                                            HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
        '                                                                            HttpContext.GetGlobalResourceObject("Text", "ContactNo2", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))})
        '    End If

        'End If

        If rblCRecipientType.SelectedValue <> RECIPIENT_TYPE.RESIDENT AndAlso rblCRecipientType.SelectedValue <> String.Empty Then
            'Contact No. : Not empty
            'If String.IsNullOrEmpty(Me.txtCContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
            If String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
                blnResult = False

                Me.SetContactNoCOVID19Error(True)

                Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))
            End If

            'Contact No. : format
            If Not String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
                If Not Regex.IsMatch(Me.txtCContactNo.Text, "^[2-9]\d{7}$") Then
                    blnResult = False

                    Me.SetContactNoCOVID19Error(True)

                    Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                    If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))
                End If

            End If
        End If

        Return blnResult
    End Function

#End Region

#Region "Select Vaccine & Dose"
    Public Sub Selection()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = _udtSessionHandler.EHSClaimVaccineGetFromSession(FunctionCode)

        'Dose
        If ddlCDoseCovid19.SelectedValue <> String.Empty Then
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.SubsidizeCode.ToUpper.Trim = ddlCCategoryCovid19.SelectedValue.ToUpper.Trim Then
                    udtEHSClaimSubsidize.Selected = True

                    For Each udtEHSClaimSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                        If udtEHSClaimSubsidizeDetail.AvailableItemCode.ToUpper.Trim = ddlCDoseCovid19.SelectedValue.ToUpper.Trim Then
                            udtEHSClaimSubsidizeDetail.Selected = True
                        End If
                    Next
                End If
            Next
        End If

    End Sub
#End Region

#Region "Save"

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtEHSClaimVaccine As EHSClaimVaccineModel)
        'Get Vaccine Brand & Lot No.
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim dtVaccineLotNo As DataTable = Nothing
        Dim strVaccineLotID As String = String.Empty

        dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForCentre(CurrentPractice.SPID, CurrentPractice.PracticeID)

        If dtVaccineLotNo.Rows.Count > 0 Then
            'Dim drVaccineLotNo() As DataRow = udtCOVID19BLL.FilterVaccineLotNoByServiceDate(dtVaccineLotNo, ServiceDate)

            'If drVaccineLotNo.Length > 0 Then
            For intCt As Integer = 0 To dtVaccineLotNo.Rows.Count - 1
                If dtVaccineLotNo.Rows(intCt)("Vaccine_Lot_No").ToString.Trim.ToUpper = ddlCVaccineLotNoCovid19.SelectedValue.Trim.ToUpper Then
                    strVaccineLotID = dtVaccineLotNo.Rows(intCt)("Vaccine_Lot_ID").ToString.Trim
                    Exit For
                End If
            Next
            'End If

        End If

        'Category
        If rbCategorySelection.Visible And rbCategorySelection.SelectedValue <> String.Empty Then
            udtEHSTransaction.CategoryCode = rbCategorySelection.SelectedValue.Trim
        End If

        If lblCategory.Visible And lblCategory.Attributes("SelectedValue") <> String.Empty Then
            udtEHSTransaction.CategoryCode = lblCategory.Attributes("SelectedValue")
        End If

        ''Category
        'Dim udtClaimCategory As ClaimCategoryModel = Nothing
        'Dim udtClaimCategoryList As ClaimCategoryModelCollection = Nothing

        'If ddlCCategoryCovid19.SelectedValue <> String.Empty Then
        '    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
        '        If udtEHSClaimSubsidize.Selected Then
        '            udtClaimCategoryList = (New ClaimCategoryBLL).getAllCategoryCache().Filter(udtEHSClaimSubsidize.SchemeCode, udtEHSClaimSubsidize.SchemeSeq, udtEHSClaimSubsidize.SubsidizeCode)
        '        End If
        '    Next

        '    If udtClaimCategoryList.Count > 0 Then
        '        udtClaimCategory = udtClaimCategoryList(0)
        '        udtEHSTransaction.CategoryCode = udtClaimCategory.CategoryCode

        '        '_udtSessionHandler.ClaimCOVID19CategorySaveToSession(udtClaimCategory.SubsidizeCode, FunctCode)
        '    End If

        'End If

        ' -----------------------------------------------
        ' Get Latest SchemeSeq Selected
        '------------------------------------------------
        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

        If Not udtSubsidizeLatest Is Nothing Then

            'Recipient Type
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.RecipientType
            udtTransactAdditionfield.AdditionalFieldValueCode = rblCRecipientType.SelectedValue
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'RCH Code
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode
            udtTransactAdditionfield.AdditionalFieldValueCode = RCHCode
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Main Category
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.MainCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = "PG3"
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Sub Category
            Dim strSubCategory As String = String.Empty

            Select Case rblCRecipientType.SelectedValue
                Case RECIPIENT_TYPE.RESIDENT
                    Select Case _strRCHType
                        Case RCH_TYPE.RCHE
                            strSubCategory = "GL13"
                        Case RCH_TYPE.RCHD
                            strSubCategory = "GL14"
                    End Select

                Case RECIPIENT_TYPE.RCH_STAFF
                    Select Case _strRCHType
                        Case RCH_TYPE.RCHE
                            strSubCategory = "GL15"
                        Case RCH_TYPE.RCHD
                            strSubCategory = "GL16"
                    End Select

                Case RECIPIENT_TYPE.CCSU_STAFF
                    Select Case _strRCHType
                        Case RCH_TYPE.RCHE
                            strSubCategory = "GL45"
                        Case RCH_TYPE.RCHD
                            strSubCategory = "GL46"
                    End Select

            End Select

            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SubCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = strSubCategory
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Vaccine Brand
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineBrand
            udtTransactAdditionfield.AdditionalFieldValueCode = ddlCVaccineBrandCovid19.SelectedValue.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Vaccine Lot No.
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineLotNo
            udtTransactAdditionfield.AdditionalFieldValueCode = ddlCVaccineLotNoCovid19.SelectedValue.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Vaccine Lot ID
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineLotID
            udtTransactAdditionfield.AdditionalFieldValueCode = strVaccineLotID
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)


            'Contact No.
            Dim strContactNo As String = String.Empty

            If rblCRecipientType.SelectedValue <> RECIPIENT_TYPE.RESIDENT AndAlso rblCRecipientType.SelectedValue <> String.Empty Then
                strContactNo = ContactNo
            End If

            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
            udtTransactAdditionfield.AdditionalFieldValueCode = strContactNo
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Remarks
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
            udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
            udtTransactAdditionfield.AdditionalFieldValueDesc = txtCRemark.Text.Trim
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Join eHRSS
            Dim strJoinEHRSS As String = String.Empty

            If rblCRecipientType.SelectedValue <> RECIPIENT_TYPE.RESIDENT AndAlso rblCRecipientType.SelectedValue <> String.Empty Then
                If udtEHSTransaction.EHSAcct.SearchDocCode IsNot Nothing Then
                    If Me.DisplayJoinEHRSS(udtEHSTransaction.EHSAcct) Then
                        strJoinEHRSS = IIf(chkCJoinEHRSS.Checked, YesNo.Yes, YesNo.No)
                    Else
                        strJoinEHRSS = String.Empty
                    End If
                End If
            End If

            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS
            udtTransactAdditionfield.AdditionalFieldValueCode = strJoinEHRSS
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
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
                Me.udcClaimVaccineInputCOVID19.Visible = True
            Else
                Me.udcClaimVaccineInputCOVID19.Visible = False
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

            Me.udcClaimVaccineInputCOVID19.Visible = True
        End If

    End Sub

    Private Sub BindRecipientType()
        Dim strSelectedValue As String = Nothing

        'Get the value from request
        strSelectedValue = Me.Request.Form(Me.rblCRecipientType.UniqueID)

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            strSelectedValue = rblCRecipientType.SelectedValue
        End If

        Me.rblCRecipientType.Items.Clear()
        Me.rblCRecipientType.SelectedIndex = -1
        Me.rblCRecipientType.SelectedValue = Nothing
        Me.rblCRecipientType.ClearSelection()

        'Build RadioButtonList
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtStaticDataList As StaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("RecipientType")

        Me.rblCRecipientType.DataSource = udtStaticDataList

        Me.rblCRecipientType.DataValueField = "ItemNo"

        If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.rblCRecipientType.DataTextField = "DataValueChi"
        Else
            Me.rblCRecipientType.DataTextField = "DataValue"
        End If

        Me.rblCRecipientType.DataBind()

        'Restore the selected value if has value
        If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
            For Each li As ListItem In rblCRecipientType.Items
                If strSelectedValue = li.Value Then
                    Me.rblCRecipientType.SelectedValue = li.Value
                End If
            Next
        End If

    End Sub

    Private Sub BindCOVID19VaccineBrand(drVaccineLotNo() As DataRow)
        Dim strSelectedValue As String = Nothing

        strSelectedValue = Me.ddlCVaccineBrandCovid19.SelectedValue

        'If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
        '    If Not SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode) Is Nothing Then
        '        strSelectedValue = SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode)
        '    End If
        'Else
        '    SessionHandler.ClaimCOVID19VaccineBrandSaveToSession(strSelectedValue, FunctCode)
        'End If

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            strSelectedValue = String.Empty
            'SessionHandler.ClaimCOVID19VaccineBrandRemoveFromSession(FunctCode)
        End If

        Me.ddlCVaccineBrandCovid19.Items.Clear()
        Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
        Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
        Me.ddlCVaccineBrandCovid19.ClearSelection()

        'Build Vaccine Brand dropdownlist
        If drVaccineLotNo.Length > 0 Then
            Me.ddlCVaccineBrandCovid19.Enabled = True

            Dim dtVaccineBrand As DataTable = Nothing

            dtVaccineBrand = drVaccineLotNo.CopyToDataTable.DefaultView.ToTable(True, New String() {"Brand_ID", "Brand_Trade_Name", "Brand_Trade_Name_Chi"})

            Me.ddlCVaccineBrandCovid19.DataSource = dtVaccineBrand

            Me.ddlCVaccineBrandCovid19.DataValueField = "Brand_ID"

            If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlCVaccineBrandCovid19.DataTextField = "Brand_Trade_Name_Chi"
            Else
                Me.ddlCVaccineBrandCovid19.DataTextField = "Brand_Trade_Name"
            End If

            Me.ddlCVaccineBrandCovid19.ClearSelection()
            Me.ddlCVaccineBrandCovid19.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCVaccineBrandCovid19.Items.Count > 1 Then
                ddlCVaccineBrandCovid19.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            Me.ddlCVaccineBrandCovid19.SelectedIndex = 0
            Me.txtCVaccineBrand.Text = ddlCVaccineBrandCovid19.Items(0).Value.Trim

            'Restore the selected value if has value
            If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                For Each li As ListItem In ddlCVaccineBrandCovid19.Items
                    If strSelectedValue = li.Value Then
                        Me.ddlCVaccineBrandCovid19.SelectedValue = li.Value
                        Me.txtCVaccineBrand.Text = Me.ddlCVaccineBrandCovid19.SelectedValue
                    End If
                Next
            End If

        Else
            Me.ddlCVaccineBrandCovid19.Items.Clear()
            Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
            Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
            Me.ddlCVaccineBrandCovid19.ClearSelection()
            Me.ddlCVaccineBrandCovid19.Enabled = False

        End If

    End Sub

    Private Sub BindCOVID19VaccineLotNo(drVaccineLotNo() As DataRow)
        Dim strSelectedValue As String = Nothing

        strSelectedValue = Me.ddlCVaccineLotNoCovid19.SelectedValue

        'If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
        '    If Not SessionHandler.ClaimCOVID19VaccineLotNoGetFromSession(FunctCode) Is Nothing Then
        '        strSelectedValue = SessionHandler.ClaimCOVID19VaccineLotNoGetFromSession(FunctCode)
        '    End If
        'Else
        '    SessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(strSelectedValue, FunctCode)
        'End If

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            strSelectedValue = String.Empty
            'SessionHandler.ClaimCOVID19VaccineLotNoRemoveFromSession(FunctCode)
        End If

        Me.ddlCVaccineLotNoCovid19.Items.Clear()
        Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
        Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
        Me.ddlCVaccineLotNoCovid19.ClearSelection()

        'Build Vaccine Lot No. dropdownlist Filter With Band id
        Dim drVaccineLotNoFilterWithBrand() As DataRow = Nothing
        Dim strVaccinBandID As String = Me.ddlCVaccineBrandCovid19.SelectedValue

        'If stVaccinBandID = "", it means no brand is selected. (the Practice has more than one brand = > drVaccineLotNoFilterWithBand.length = 0) 
        If drVaccineLotNo.Length > 0 Then
            drVaccineLotNoFilterWithBrand = drVaccineLotNo.CopyToDataTable().Select(String.Format("Brand_ID = '{0}'", strVaccinBandID))
        End If

        'For render the server side dropdown
        If drVaccineLotNoFilterWithBrand IsNot Nothing AndAlso drVaccineLotNoFilterWithBrand.Length > 0 Then
            Me.ddlCVaccineLotNoCovid19.Enabled = True
            Me.ddlCVaccineLotNoCovid19.DataSource = drVaccineLotNoFilterWithBrand.CopyToDataTable()

            Me.ddlCVaccineLotNoCovid19.DataValueField = "Vaccine_Lot_No"
            Me.ddlCVaccineLotNoCovid19.DataTextField = "Vaccine_Lot_No"
            Me.ddlCVaccineLotNoCovid19.ClearSelection()
            Me.ddlCVaccineLotNoCovid19.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCVaccineLotNoCovid19.Items.Count > 1 Then
                ddlCVaccineLotNoCovid19.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            Me.ddlCVaccineLotNoCovid19.SelectedIndex = 0
            Me.txtCVaccineLotNo.Text = ddlCVaccineLotNoCovid19.Items(0).Value.Trim

            'Restore the selected value if has value
            If ddlCVaccineLotNoCovid19.Items.Count > 1 Then
                If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                    For Each li As ListItem In ddlCVaccineLotNoCovid19.Items
                        If strSelectedValue = li.Value Then
                            Me.ddlCVaccineLotNoCovid19.SelectedValue = li.Value
                            Me.txtCVaccineLotNo.Text = Me.ddlCVaccineLotNoCovid19.SelectedValue
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
            Me.ddlCVaccineLotNoCovid19.Items.Clear()
            Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
            Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
            Me.ddlCVaccineLotNoCovid19.ClearSelection()
            Me.ddlCVaccineLotNoCovid19.Enabled = False

        End If
    End Sub

#End Region

End Class
