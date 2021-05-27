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


Partial Public Class ucInputCOVID19OR
    Inherits ucInputEHSClaimBase

    Private _udtSessionHandler As New BLL.SessionHandlerBLL
    Private _udtGeneralFunction As New GeneralFunction
    Private _udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
    Private _strOutreachType As String

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

    Public ReadOnly Property OutreachCode() As String
        Get
            Return Me.txtOutreachCode.Text.Trim
        End Get
    End Property

    Public ReadOnly Property OutreachType() As String
        Get
            Return Me._strOutreachType
        End Get
    End Property

    Public ReadOnly Property OutreachName() As String
        Get
            Return Me.lblOutreachName.Text
        End Get
    End Property

    Public ReadOnly Property MainCategory() As String
        Get
            'If Me.txtCMainCategory.Text = "" Then
            '    Return String.Empty
            'Else
            '    Return Me.txtCMainCategory.Text
            'End If
            If Me.ddlCMainCategory.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCMainCategory.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property SubCategory() As String
        Get
            'If Me.txtCSubCategory.Text = "" Then
            '    Return String.Empty
            'Else
            '    Return Me.txtCSubCategory.Text
            'End If
            If Me.ddlCSubCategory.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCSubCategory.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property MainCategoryDDL() As DropDownList
        Get
            Return Me.ddlCMainCategory
        End Get
    End Property

    Public ReadOnly Property SubCategoryDDL() As DropDownList
        Get
            Return Me.ddlCSubCategory
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

    Public ReadOnly Property VaccineBrandDDL() As DropDownList
        Get
            Return Me.ddlCVaccineBrandCovid19
        End Get
    End Property

    Public Property VaccineLotNo() As String
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
        Set(value As String)
            'Me.txtCVaccineLotNo.Text = value
            Me.ddlCVaccineLotNoCovid19.SelectedItem.Value = value
        End Set
    End Property

    Public ReadOnly Property VaccineLotNoDDL() As DropDownList
        Get
            Return Me.ddlCVaccineLotNoCovid19
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
        Me.lblOutreachCodeText.Text = Me.GetGlobalResourceObject("Text", "OutreachCode")
        Me.lblOutreachNameText.Text = Me.GetGlobalResourceObject("Text", "OutreachName")

        Me.lblOutreachName.Visible = True
        Me.lblOutreachNameChi.Visible = False

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
        End If

        If Not noCategory Then
            Me.panCOVID19Category.Visible = True

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
                    'Outreach Code
                    Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
                    Dim dtRVPhomeList As DataTable

                    dtRVPhomeList = udtRVPHomeListBLL.getRVPHomeListByCode(Me.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode)

                    If dtRVPhomeList.Rows.Count > 0 Then
                        Me.SetUpOutreachInfo(dtRVPhomeList.Rows(0))
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

                'AddHandler Me.udcClaimVaccineInputCOVID19.VaccineLegendClicked, AddressOf udcClaimVaccineInputCOVID19_VaccineLegendClicked
                'AddHandler Me.udcClaimVaccineInputCOVID19.SubsidizeDisabledRemarkClicked, AddressOf udcClaimVaccineInputCOVID19_SubsidizeDisabledRemarkClicked
            Else
                Me.panCOVID19Detail.Visible = False
                Me.udcClaimVaccineInputCOVID19.Visible = False

            End If

            ''Bind DropDownList Category
            'BindCOVID19Category(MyBase.ClaimCategorys)

            'Bind DropDownList Main Category
            BindMainCategory()

            ' Fill brand value by temp save
            If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

                'Main Category
                If udtTAFList.MainCategory IsNot Nothing Then
                    Dim strMainCategory As String = udtTAFList.MainCategory.Trim

                    For Each li As ListItem In ddlCMainCategory.Items
                        If strMainCategory.ToUpper.Trim = li.Value Then
                            ddlCMainCategory.SelectedValue = li.Value
                        End If
                    Next
                End If

            End If

            'Bind DropDownList Sub Category
            BindSubCategory()

            ' Fill brand value by temp save
            If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                Dim udtTAFList As TransactionAdditionalFieldModelCollection = MyBase.EHSTransaction.TransactionAdditionFields

                'Sub Category
                If udtTAFList.SubCategory IsNot Nothing Then
                    Dim strSubCategory As String = udtTAFList.SubCategory.Trim

                    For Each li As ListItem In ddlCSubCategory.Items
                        If strSubCategory.ToUpper.Trim = li.Value Then
                            ddlCSubCategory.SelectedValue = li.Value
                        End If
                    Next
                End If

            End If

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
            If MyBase.EHSAccount.SearchDocCode IsNot Nothing Then
                Select Case MyBase.EHSAccount.SearchDocCode
                    Case DocType.DocTypeModel.DocTypeCode.HKIC, DocType.DocTypeModel.DocTypeCode.EC, DocType.DocTypeModel.DocTypeCode.OW, _
                        DocType.DocTypeModel.DocTypeCode.CCIC, DocType.DocTypeModel.DocTypeCode.TW
                        trJoinEHRSS.Style.Remove("display")
                    Case Else
                        trJoinEHRSS.Style.Add("display", "none")
                End Select
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
        Me.imgOutreachCodeError.Visible = blnVisible
        Me.imgCMainCategoryError.Visible = blnVisible
        Me.imgCSubCategoryError.Visible = blnVisible
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

    Public Sub SetOutreachCodeError(ByVal visible As Boolean)
        Me.imgOutreachCodeError.Visible = visible
    End Sub

    Public Sub SetMainCategoryForCOVID19Error(ByVal visible As Boolean)
        Me.imgCMainCategoryError.Visible = visible
    End Sub

    Public Sub SetSubCategoryForCOVID19Error(ByVal visible As Boolean)
        Me.imgCSubCategoryError.Visible = visible
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

    Public Sub SetOutreachCode(ByVal strOutreachCode As String)
        Me.txtOutreachCode.Text = strOutreachCode.Trim().ToUpper()
        Me.lookUpOutreachCode()
    End Sub

#End Region

#Region "Events"

    Private Sub btnSearchOutreach_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchOutreach.Click
        RaiseEvent SearchButtonClick(sender, e)
    End Sub

    Private Sub rbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCategorySelection.SelectedIndexChanged
        Me.rbCategorySelection.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rbCategorySelection.UniqueID), True)

        Me.SetVaccineBrandError(False)
        Me.SetVaccineLotNoError(False)
        Me.SetContactNoCOVID19Error(False)

        Me.udcClaimVaccineInputCOVID19.Controls.Clear()

        If String.IsNullOrEmpty(Me.rbCategorySelection.SelectedValue) Then
            Me.udcClaimVaccineInputCOVID19.Visible = False
        Else
            Me.udcClaimVaccineInputCOVID19.Visible = True
        End If

        RaiseEvent CategorySelected(sender, e)
    End Sub

    Private Sub txtOutreachCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOutreachCode.TextChanged
        If Me.txtOutreachCode.Text.Trim() = String.Empty Then
            Me.lblOutreachCode.Text = String.Empty
            Me.lblOutreachName.Text = String.Empty
            Me.lblOutreachNameChi.Text = String.Empty

        Else
            Me.lookUpOutreachCode()
        End If
    End Sub

    Public Function lookUpOutreachCode() As Boolean
        Dim blnRes As Boolean = False
        Dim udtOutreachListBLL As New COVID19.OutreachListBLL
        Dim dtResult As DataTable = udtOutreachListBLL.GetOutreachListActiveByCode(Me.txtOutreachCode.Text.Trim())

        'Dim drResult() As DataRow = dtResult.Select("Type IN ('E','D')")
        Dim drResult() As DataRow = dtResult.Select()

        If drResult.Length > 0 Then
            Dim dtOutreach As DataTable = drResult.CopyToDataTable
            Me.SetUpOutreachInfo(dtOutreach.Rows(0))

            blnRes = True
        Else
            Me.lblOutreachCode.Text = String.Empty
            Me.lblOutreachName.Text = String.Empty
            Me.lblOutreachNameChi.Text = String.Empty

            blnRes = False
        End If

        Return blnRes

    End Function

    Private Sub SetUpOutreachInfo(ByVal drOutreach As DataRow)
        Me.txtOutreachCode.Text = drOutreach("Outreach_Code").ToString().Trim().ToUpper()
        Me.lblOutreachCode.Text = Me.txtOutreachCode.Text
        Me._strOutreachType = drOutreach("Type").ToString.Trim

        Me.lblOutreachName.Text = drOutreach("Outreach_Name_Eng").ToString().Trim()
        If drOutreach.IsNull("Outreach_Name_Chi") Then
            Me.lblOutreachNameChi.Text = Me.lblOutreachName.Text
            Me.lblOutreachNameChi.CssClass = "tableText"
        Else
            Me.lblOutreachNameChi.Text = drOutreach("Outreach_Name_Chi").ToString().Trim()
            Me.lblOutreachNameChi.CssClass = "tableTextChi"
        End If
    End Sub

    'Protected Sub udcClaimVaccineInputCOVID19_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
    '    RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    'End Sub

    'Protected Sub udcClaimVaccineInputCOVID19_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    RaiseEvent VaccineLegendClicked(sender, e)
    'End Sub

    'Private Sub ddlCCategoryCovid19_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCCategoryCovid19.SelectedIndexChanged
    '    Me.ddlCCategoryCovid19.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCCategoryCovid19.UniqueID), True)
    '    SessionHandler.ClaimCOVID19CategorySaveToSession(Me.ddlCCategoryCovid19.SelectedValue, FunctCode)

    'End Sub

    Private Sub ddlCMainCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCMainCategory.SelectedIndexChanged
        BindSubCategory()
    End Sub

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

        ''Check Category
        'If String.IsNullOrEmpty(Me.CategoryForCOVID19) Then
        '    blnResult = False

        '    Me.SetCategoryForCOVID19Error(True)

        '    objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
        '    If objMsgBox IsNot Nothing Then
        '        objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "Category"))
        '    End If
        'End If

        'Check Outreach Code
        If OutreachCode.Equals(String.Empty) Then
            blnResult = False

            SetOutreachCodeError(True)
            objMsg = New ComObject.SystemMessage("990000", "E", "00463")
            objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "OutreachCode"))
        Else
            ' Check Outreach Code Valid
            Dim udtOutreachListBLL As New COVID19.OutreachListBLL
            Dim dtResult As DataTable = udtOutreachListBLL.GetOutreachListActiveByCode(OutreachCode.Trim())

            'Dim drResult() As DataRow = dtResult.Select("Type IN ('E','D')")
            Dim drResult() As DataRow = dtResult.Select()

            If drResult.Length = 0 Then
                blnResult = False

                SetOutreachCodeError(True)
                objMsg = New ComObject.SystemMessage("990000", "E", "00466")
                objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "OutreachCode"))
            End If
        End If

        ''Check Main Category
        'If ddlCMainCategory.Enabled Then
        '    If String.IsNullOrEmpty(Me.MainCategory) Then
        '        blnResult = False

        '        Me.SetMainCategoryForCOVID19Error(True)

        '        objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
        '        If objMsgBox IsNot Nothing Then
        '            objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "Category"))
        '        End If
        '    End If
        'End If

        'Check Sub Category
        If ddlCSubCategory.Enabled Then
            If Not String.IsNullOrEmpty(Me.MainCategory) Then
                Dim drSubCategory() As DataRow = Status.GetDescriptionListFromDBEnumCode("VSSC19SubCategory").Select(String.Format("Column_Name='{0}'", Me.MainCategory))

                If String.IsNullOrEmpty(Me.SubCategory) AndAlso drSubCategory.Length > 0 Then

                    blnResult = False

                    Me.SetSubCategoryForCOVID19Error(True)

                    objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                    If objMsgBox IsNot Nothing Then
                        objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "SubCategory"))
                    End If
                End If
            End If
        End If

        'Check Vaccine Brand
        If String.IsNullOrEmpty(Me.VaccineBrand) Then
            blnResult = False

            Me.SetVaccineBrandError(True)

            objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
            If objMsgBox IsNot Nothing Then
                objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "Vaccines"))
            End If
        End If

        'Check Vaccine Lot No.
        Dim blnVaccineLotEmpty As Boolean = False
        If String.IsNullOrEmpty(Me.VaccineLotNo) OrElse Me.VaccineLotNoDDL.SelectedItem.Value = String.Empty Then
            blnResult = False

            blnVaccineLotEmpty = True

            Me.SetVaccineLotNoError(True)

        End If

        'Get the value from request
        Dim strOriginalSelectedValue As String = String.Empty

        If Me.Request.Form(Me.VaccineLotNoDDL.UniqueID) IsNot Nothing Then
            strOriginalSelectedValue = Me.Request.Form(Me.VaccineLotNoDDL.UniqueID)
        End If

        'Check Vaccine Lot No. - UI value vs Request value
        Dim blnVaccineLotDiff As Boolean = False
        If Me.VaccineLotNo.Trim <> strOriginalSelectedValue.Trim Then
            blnResult = False

            blnVaccineLotDiff = True

            Me.SetVaccineLotNoError(True)

        End If

        If blnVaccineLotEmpty AndAlso blnVaccineLotDiff Then
            objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00475)
            If objMsgBox IsNot Nothing Then
                objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "VaccineLotNumber"))
            End If
        Else
            If blnVaccineLotEmpty Then
                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "VaccineLotNumber"))
                End If
            End If

            If blnVaccineLotDiff Then
                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00475)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "VaccineLotNumber"))
                End If
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

        'Contact No. : Not empty
        'If String.IsNullOrEmpty(Me.txtCContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
        If String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
            blnResult = False

            Me.SetContactNoCOVID19Error(True)

            Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))
        End If

        'Contact No. : Not empty
        If Not String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
            If Not Regex.IsMatch(Me.txtCContactNo.Text, "^[2-9]\d{7}$") Then
                blnResult = False

                Me.SetContactNoCOVID19Error(True)

                Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))
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

            'Outreach Code
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.OutreachCode
            udtTransactAdditionfield.AdditionalFieldValueCode = OutreachCode
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Main Category
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.MainCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = ddlCMainCategory.SelectedValue.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Sub Category
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SubCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = ddlCSubCategory.SelectedValue.Trim
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

            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
            udtTransactAdditionfield.AdditionalFieldValueCode = ContactNo
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
            udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
            udtTransactAdditionfield.AdditionalFieldValueDesc = txtCRemark.Text.Trim
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            Dim strJoinEHRSS As String = String.Empty

            If udtEHSTransaction.EHSAcct.SearchDocCode IsNot Nothing Then
                Select Case udtEHSTransaction.EHSAcct.SearchDocCode
                    Case DocType.DocTypeModel.DocTypeCode.HKIC, DocType.DocTypeModel.DocTypeCode.EC, DocType.DocTypeModel.DocTypeCode.OW, _
                        DocType.DocTypeModel.DocTypeCode.CCIC, DocType.DocTypeModel.DocTypeCode.TW
                        strJoinEHRSS = IIf(chkCJoinEHRSS.Checked, YesNo.Yes, YesNo.No)
                    Case Else
                        strJoinEHRSS = String.Empty
                End Select
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

    Private Sub BindMainCategory()
        Dim strSelectedValue As String = Nothing

        Dim dtMainCategory As DataTable = Status.GetDescriptionListFromDBEnumCode("VSSC19MainCategory")

        strSelectedValue = Me.ddlCMainCategory.SelectedValue

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

        Me.ddlCMainCategory.Items.Clear()
        Me.ddlCMainCategory.SelectedIndex = -1
        Me.ddlCMainCategory.SelectedValue = Nothing
        Me.ddlCMainCategory.ClearSelection()

        'Build Vaccine Brand dropdownlist
        If dtMainCategory.Rows.Count > 0 Then
            Me.ddlCMainCategory.Enabled = True

            Me.ddlCMainCategory.DataSource = dtMainCategory
            Me.ddlCMainCategory.DataValueField = "Status_Value"
            Me.ddlCMainCategory.DataTextField = "Status_Description"
            Me.ddlCMainCategory.ClearSelection()
            Me.ddlCMainCategory.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCMainCategory.Items.Count > 1 Then
                ddlCMainCategory.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            Me.ddlCMainCategory.SelectedIndex = 0
            Me.txtCVaccineBrand.Text = ddlCMainCategory.Items(0).Value.Trim

            'Restore the selected value if has value
            If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                For Each li As ListItem In ddlCMainCategory.Items
                    If strSelectedValue = li.Value Then
                        Me.ddlCMainCategory.SelectedValue = li.Value
                        Me.txtCVaccineBrand.Text = Me.ddlCMainCategory.SelectedValue
                    End If
                Next
            End If

        Else
            Me.ddlCMainCategory.Items.Clear()
            Me.ddlCMainCategory.SelectedIndex = -1
            Me.ddlCMainCategory.SelectedValue = Nothing
            Me.ddlCMainCategory.ClearSelection()
            Me.ddlCMainCategory.Enabled = False

        End If

    End Sub

    Private Sub BindSubCategory()
        Dim strSelectedValue As String = Nothing

        Dim dtSubCategory As DataTable = Status.GetDescriptionListFromDBEnumCode("VSSC19SubCategory")

        strSelectedValue = Me.ddlCSubCategory.SelectedValue

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

        Me.ddlCSubCategory.Items.Clear()
        Me.ddlCSubCategory.SelectedIndex = -1
        Me.ddlCSubCategory.SelectedValue = Nothing
        Me.ddlCSubCategory.ClearSelection()

        'Build Vaccine Lot No. dropdownlist Filter With Band id
        Dim drSubCategoryFilterMain() As DataRow = Nothing
        Dim strMainCategory As String = Me.ddlCMainCategory.SelectedValue.Trim

        'If stVaccinBandID = "", it means no brand is selected. (the Practice has more than one brand = > drVaccineLotNoFilterWithBand.length = 0) 
        If dtSubCategory.Rows.Count > 0 Then
            drSubCategoryFilterMain = dtSubCategory.Select(String.Format("Column_Name = '{0}'", strMainCategory))
        End If

        'For render the server side dropdown
        If drSubCategoryFilterMain IsNot Nothing AndAlso drSubCategoryFilterMain.Length > 0 Then
            Me.ddlCSubCategory.Enabled = True
            Me.ddlCSubCategory.DataSource = drSubCategoryFilterMain.CopyToDataTable()
            Me.ddlCSubCategory.DataValueField = "Status_Value"
            Me.ddlCSubCategory.DataTextField = "Status_Description"
            Me.ddlCSubCategory.ClearSelection()
            Me.ddlCSubCategory.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCSubCategory.Items.Count > 1 Then
                ddlCSubCategory.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            Me.ddlCSubCategory.SelectedIndex = 0
            Me.txtCVaccineLotNo.Text = ddlCSubCategory.Items(0).Value.Trim

            'Restore the selected value if has value
            If ddlCSubCategory.Items.Count > 1 Then
                If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                    For Each li As ListItem In ddlCSubCategory.Items
                        If strSelectedValue = li.Value Then
                            Me.ddlCSubCategory.SelectedValue = li.Value
                            Me.txtCVaccineLotNo.Text = Me.ddlCSubCategory.SelectedValue
                        End If
                    Next
                End If
            End If

        Else
            Me.ddlCSubCategory.Items.Clear()
            Me.ddlCSubCategory.SelectedIndex = -1
            Me.ddlCSubCategory.SelectedValue = Nothing
            Me.ddlCSubCategory.ClearSelection()
            Me.ddlCSubCategory.Enabled = False

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
    ' CRE20-0022 (Immu record) [END][Martin Tang]

    'Private Sub BindCOVID19Dose()
    '    Dim strSelectedValue As String = Nothing

    '    strSelectedValue = AntiXssEncoder.HtmlEncode(Me.ddlCDoseCovid19.SelectedValue, True)

    '    'Set selected if "1st Dose" exists
    '    If strSelectedValue = String.Empty Then
    '        For Each li As ListItem In ddlCDoseCovid19.Items
    '            If li.Value = "1STDOSE" Then
    '                strSelectedValue = "1STDOSE"
    '            End If
    '        Next
    '    End If

    '    'Bind Dose into dropdownlist
    '    Me.ddlCDoseCovid19.Items.Clear()
    '    Me.ddlCDoseCovid19.SelectedIndex = -1
    '    Me.ddlCDoseCovid19.SelectedValue = Nothing
    '    Me.ddlCDoseCovid19.ClearSelection()

    '    If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
    '        strSelectedValue = String.Empty
    '    End If

    '    'Build Dose dropdownlist
    '    Dim lstItem As ListItem = Nothing

    '    If EHSClaimVaccine IsNot Nothing Then
    '        For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In EHSClaimVaccine.SubsidizeList(0).SubsidizeDetailList
    '            If udtEHSClaimSubidizeDetail.Available Then
    '                lstItem = New ListItem
    '                lstItem.Value = udtEHSClaimSubidizeDetail.AvailableItemCode

    '                If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
    '                    lstItem.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi
    '                Else
    '                    lstItem.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc
    '                End If

    '                ddlCDoseCovid19.Items.Add(lstItem)
    '            End If
    '        Next

    '        'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
    '        If ddlCDoseCovid19.Items.Count > 1 Then
    '            ddlCDoseCovid19.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
    '        End If

    '        If ddlCDoseCovid19.Items.Count > 0 Then
    '            ddlCDoseCovid19.SelectedIndex = 0
    '        Else
    '            ddlCDoseCovid19.Enabled = False
    '            ddlCDoseCovid19.Dispose()
    '        End If

    '    End If

    '    'Restore the selected value if has value
    '    If strSelectedValue IsNot Nothing Then
    '        For Each li As ListItem In ddlCDoseCovid19.Items
    '            If strSelectedValue = li.Value Then
    '                ddlCDoseCovid19.SelectedValue = li.Value
    '            End If
    '        Next
    '    End If

    'End Sub

#End Region




End Class
