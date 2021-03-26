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


Partial Public Class ucInputVSSCOVID19
    Inherits ucInputEHSClaimBase

    ' Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201
    Public FunctCode As String = (New BLL.SessionHandler).ClaimFunctCodeGetFromSession() 'CRE20-0xx Immue record [Nichole]

    Dim _udtSessionHandler As New SessionHandler
    Dim _udtGeneralFunction As New GeneralFunction

#Region "Constants"

    Private Class ViewIndexCategory
        Public Const NoCategory As Integer = 0
        Public Const VSS_PW As Integer = 1
        Public Const VSS_CHILD As Integer = 2
        Public Const VSS_ELDER As Integer = 3
        Public Const VSS_PID As Integer = 4
        Public Const VSS_DA As Integer = 5
        Public Const VSS_ADULT As Integer = 6
    End Class

#End Region

#Region "Properties"

    'Public ReadOnly Property Category() As String
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

    Public ReadOnly Property IsClaimCOVID19() As Boolean
        Get
            Return (New BLL.SessionHandler).ClaimCOVID19GetFromSession()
        End Get
    End Property

    Public ReadOnly Property Booth() As String
        Get
            If Me.ddlCBooth.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCBooth.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property CategoryForCOVID19() As String
        Get
            If Me.ddlCCategoryCovid19.SelectedItem Is Nothing Then
                Return String.Empty
            Else
                Return Me.ddlCCategoryCovid19.SelectedItem.Value
            End If
        End Get
    End Property

    Public ReadOnly Property MainCategoryForCOVID19() As String
        Get
            If Me.txtCMainCategory.Text = "" Then
                Return String.Empty
            Else
                Return Me.txtCMainCategory.Text
            End If
            'If Me.ddlCMainCategoryCovid19.SelectedItem Is Nothing Then
            '    Return String.Empty
            'Else
            '    Return Me.ddlCMainCategoryCovid19.SelectedItem.Value
            'End If
        End Get
    End Property

    Public ReadOnly Property SubCategoryForCOVID19() As String
        Get
            If Me.txtCSubCategory.Text = "" Then
                Return String.Empty
            Else
                Return Me.txtCSubCategory.Text
            End If
            'If Me.ddlCSubCategoryCovid19.SelectedItem Is Nothing Then
            '    Return String.Empty
            'Else
            '    Return Me.ddlCSubCategoryCovid19.SelectedItem.Value
            'End If
        End Get
    End Property

    Public ReadOnly Property MainCategoryDDL() As DropDownList
        Get
            Return Me.ddlCMainCategoryCovid19
        End Get
    End Property

    Public ReadOnly Property SubCategoryDDL() As DropDownList
        Get
            Return Me.ddlCSubCategoryCovid19
        End Get
    End Property

    Public Property VaccineBrand() As String
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
        Set(value As String)
            Me.txtCVaccineBrand.Text = value
        End Set
    End Property

    Public ReadOnly Property VaccineBrandDDL() As DropDownList
        Get
            Return Me.ddlCVaccineBrandCovid19
        End Get
    End Property

    Public Property VaccineLotNo() As String
        Get
            If Me.txtCVaccineLotNo.Text = "" Then
                Return String.Empty
            Else
                Return Me.txtCVaccineLotNo.Text
            End If

            'If Me.ddlCVaccineLotNoCovid19.SelectedItem Is Nothing Then
            '    Return String.Empty
            'Else
            '    Return Me.ddlCVaccineLotNoCovid19.SelectedItem.Value
            'End If
        End Get
        Set(value As String)
            Me.txtCVaccineLotNo.Text = value
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

#End Region

#Region "Event handlers"
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()

    End Sub

    Protected Overrides Sub Setup()

        'Bind DropDownList Booth
        BindBooth()

        'Bind DropDownList System Category (Hidden)
        BindCOVID19Category(MyBase.ClaimCategorys)

        'Bind DropDownList Main Category
        BindMainCategory()

        ' Fill value by temp save
        If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
            'MainCategory
            If MyBase.EHSTransaction.TransactionAdditionFields.MainCategory IsNot Nothing Then
                For Each li As ListItem In ddlCMainCategoryCovid19.Items
                    If MyBase.EHSTransaction.TransactionAdditionFields.MainCategory.Trim.ToUpper = li.Value.Trim.ToUpper Then
                        ddlCMainCategoryCovid19.SelectedValue = li.Value
                        txtCMainCategory.Text = ddlCMainCategoryCovid19.SelectedValue
                    End If
                Next
            End If
        End If

        'Bind DropDownList Sub Category
        BindSubCategory()

        'Get Vaccine Brand & Lot No.
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim dtVaccineLotNo As DataTable = Nothing

        dtVaccineLotNo = udtCOVID19BLL.GetCOVID19VaccineLotMappingForPrivate()

        If dtVaccineLotNo.Rows.Count > 0 Then

            Dim drVaccineLotNo() As DataRow = udtCOVID19BLL.FilterVaccineLotNoByServiceDate(dtVaccineLotNo, ServiceDate)

            'Bind DropDownList Vaccine Brand
            BindCOVID19VaccineBrand(drVaccineLotNo)

            ' Fill value by temp save
            If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
                'Vaccine
                If MyBase.EHSTransaction.TransactionAdditionFields.VaccineBrand IsNot Nothing Then
                    Dim strSelectedValue As String = MyBase.EHSTransaction.TransactionAdditionFields.VaccineBrand

                    For Each li As ListItem In ddlCVaccineBrandCovid19.Items
                        If strSelectedValue.Trim = li.Value Then
                            ddlCVaccineBrandCovid19.SelectedValue = li.Value
                        End If
                    Next
                End If
            End If

            'Bind DropDownList Vaccine Lot No.
            BindCOVID19VaccineLotNo(drVaccineLotNo)

        Else
            Me.ClearVaccineAndLotNo()
            Me.ddlCVaccineBrandCovid19.Items.Clear()
            Me.ddlCVaccineBrandCovid19.Enabled = False
            Me.ddlCVaccineLotNoCovid19.Enabled = False

        End If

        'Assign Subsidize Display Code
        If EHSClaimVaccine IsNot Nothing Then
            lblCVaccine.Text = EHSClaimVaccine.SubsidizeList(0).SubsidizeDisplayCode
        End If

        'Bind DropDownList Dose
        BindCOVID19Dose()

        ' Fill value by temp save
        If MyBase.EHSTransaction IsNot Nothing AndAlso MyBase.EHSTransaction.TransactionAdditionFields IsNot Nothing Then
            'Category
            If MyBase.EHSTransaction.CategoryCode IsNot Nothing Then
                Dim udtClaimCategory As ClaimCategoryModel = Nothing

                udtClaimCategory = (New ClaimCategoryBLL).getAllCategoryCache().FilterByCategoryCode(MyBase.EHSTransaction.SchemeCode, MyBase.EHSTransaction.CategoryCode)

                For Each li As ListItem In ddlCCategoryCovid19.Items
                    If udtClaimCategory.SubsidizeCode.ToUpper.Trim = li.Value Then
                        ddlCCategoryCovid19.SelectedValue = li.Value
                    End If
                Next
            End If

            'SubCategory
            If MyBase.EHSTransaction.TransactionAdditionFields.SubCategory IsNot Nothing Then
                For Each li As ListItem In ddlCSubCategoryCovid19.Items
                    If MyBase.EHSTransaction.TransactionAdditionFields.SubCategory.Trim.ToUpper = li.Value.Trim.ToUpper And li.Value <> String.Empty Then
                        ddlCSubCategoryCovid19.SelectedValue = li.Value
                        txtCSubCategory.Text = ddlCSubCategoryCovid19.SelectedValue
                    End If
                Next
            End If

            'Lot No.
            If MyBase.EHSTransaction.TransactionAdditionFields.VaccineLotNo IsNot Nothing Then
                Dim strSelectedValue As String = MyBase.EHSTransaction.TransactionAdditionFields.VaccineLotNo

                For Each li As ListItem In ddlCVaccineLotNoCovid19.Items
                    If strSelectedValue.Trim = li.Value Then
                        ddlCVaccineLotNoCovid19.SelectedValue = li.Value
                    End If
                Next
            End If

            'Dose
            If MyBase.EHSTransaction.TransactionDetails IsNot Nothing Then
                If MyBase.EHSTransaction.TransactionDetails(0) IsNot Nothing Then
                    For Each li As ListItem In ddlCDoseCovid19.Items
                        If MyBase.EHSTransaction.TransactionDetails(0).AvailableItemCode.ToUpper.Trim = li.Value Then
                            ddlCDoseCovid19.SelectedValue = li.Value
                        End If
                    Next
                End If
            End If

        End If

    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetBoothError(ByVal visible As Boolean)
        Me.imgCBoothError.Visible = visible
    End Sub

    Public Sub SetCategoryForCOVID19Error(ByVal visible As Boolean)
        Me.imgCCategoryError.Visible = visible
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

    Public Sub SetDoseForCOVID19Error(ByVal visible As Boolean)
        Me.imgCDoseError.Visible = visible
    End Sub

#End Region

#Region "SetValue"

    'Public Sub ClearClaimDetail()

    '    Me.ddlCBooth.SelectedIndex = -1
    '    Me.ddlCBooth.SelectedValue = Nothing
    '    Me.ddlCBooth.ClearSelection()

    '    Me.ddlCCategoryCovid19.SelectedIndex = -1
    '    Me.ddlCCategoryCovid19.SelectedValue = Nothing
    '    Me.ddlCCategoryCovid19.ClearSelection()

    '    Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
    '    Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
    '    Me.ddlCVaccineBrandCovid19.ClearSelection()

    '    Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
    '    Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
    '    Me.ddlCVaccineLotNoCovid19.ClearSelection()

    '    Me.ddlCDoseCovid19.SelectedIndex = -1
    '    Me.ddlCDoseCovid19.SelectedValue = Nothing
    '    Me.ddlCDoseCovid19.ClearSelection()

    'End Sub

    Public Sub ClearBooth()
        Me.ddlCBooth.SelectedIndex = -1
        Me.ddlCBooth.SelectedValue = Nothing
        Me.ddlCBooth.ClearSelection()

    End Sub

    Public Sub ClearCategory()
        Me.ddlCCategoryCovid19.SelectedIndex = -1
        Me.ddlCCategoryCovid19.SelectedValue = Nothing
        Me.ddlCCategoryCovid19.ClearSelection()

    End Sub

    Public Sub ClearVaccineAndLotNo()
        Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
        Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
        Me.ddlCVaccineBrandCovid19.ClearSelection()
        'Me.ddlCVaccineBrandCovid19.Items.Clear()
        Me.txtCVaccineBrand.Text = String.Empty

        Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
        Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
        Me.ddlCVaccineLotNoCovid19.ClearSelection()
        Me.ddlCVaccineLotNoCovid19.Items.Clear()
        Me.ddlCVaccineLotNoCovid19.Enabled = False
        Me.txtCVaccineLotNo.Text = String.Empty

    End Sub

    Public Sub ClearDose()
        Me.ddlCDoseCovid19.SelectedIndex = -1
        Me.ddlCDoseCovid19.SelectedValue = Nothing
        Me.ddlCDoseCovid19.ClearSelection()

    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputCOVID19_SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub

    Protected Sub udcClaimVaccineInputCOVID19_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    Private Sub ddlCBooth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCBooth.SelectedIndexChanged
        Me.ddlCBooth.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCBooth.UniqueID), True)
        SessionHandler.ClaimCOVID19BoothSaveToSession(Me.ddlCBooth.SelectedValue, FunctCode)

    End Sub

    'Private Sub ddlCCategoryCovid19_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCCategoryCovid19.SelectedIndexChanged
    '    Me.ddlCCategoryCovid19.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCCategoryCovid19.UniqueID), True)
    '    SessionHandler.ClaimCOVID19CategorySaveToSession(Me.ddlCCategoryCovid19.SelectedValue, FunctCode)

    'End Sub

    'Private Sub ddlcvaccinebrandcovid19_selectedindexchanged(sender As Object, e As EventArgs) Handles ddlCVaccineBrandCovid19.SelectedIndexChanged
    '    Me.ddlcvaccinebrandcovid19.selectedvalue = antixssencoder.htmlencode(Me.request.form(Me.ddlcvaccinebrandcovid19.uniqueid), True)
    '    sessionhandler.claimcovid19vaccinebrandsavetosession(Me.ddlcvaccinebrandcovid19.selectedvalue, functcode)
    'End Sub

    'Private Sub ddlCVaccineLotNoCovid19_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCVaccineLotNoCovid19.SelectedIndexChanged
    '    Me.ddlCVaccineLotNoCovid19.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCVaccineLotNoCovid19.UniqueID), True)
    '    SessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(Me.ddlCVaccineLotNoCovid19.SelectedValue, FunctCode)

    'End Sub

#End Region

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        ''Check Booth
        'If panBooth.Visible AndAlso String.IsNullOrEmpty(Me.Booth) Then
        '    blnResult = False

        '    Me.SetBoothError(True)

        '    objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
        '    If objMsgBox IsNot Nothing Then
        '        objMsgBox.AddMessage(objMsg, _
        '                             New String() {"%en", "%tc", "%sc"}, _
        '                             New String() {HttpContext.GetGlobalResourceObject("Text", "Booth", New System.Globalization.CultureInfo(CultureLanguage.English)), _
        '                                           HttpContext.GetGlobalResourceObject("Text", "Booth", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
        '                                           HttpContext.GetGlobalResourceObject("Text", "Booth", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
        '                                           })
        '    End If
        'End If

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

        'Check Main Category
        If ddlCMainCategoryCovid19.Enabled Then
            If String.IsNullOrEmpty(Me.MainCategoryForCOVID19) Then
                blnResult = False

                Me.SetMainCategoryForCOVID19Error(True)

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
        End If


        'Check Sub Category
        If ddlCSubCategoryCovid19.Enabled Then
            Dim drSubCategory() As DataRow = Status.GetDescriptionListFromDBEnumCode("VSSC19SubCategory").Select(String.Format("Column_Name='{0}'", Me.MainCategoryForCOVID19))

            If String.IsNullOrEmpty(Me.SubCategoryForCOVID19) AndAlso drSubCategory.Length > 0 Then

                blnResult = False

                Me.SetSubCategoryForCOVID19Error(True)

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {HttpContext.GetGlobalResourceObject("Text", "SubCategory", New System.Globalization.CultureInfo(CultureLanguage.English)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "SubCategory", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)), _
                                                       HttpContext.GetGlobalResourceObject("Text", "SubCategory", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)) _
                                                       })
                End If
            End If
        End If

        'Check Vaccine Brand
        If String.IsNullOrEmpty(Me.VaccineBrand) OrElse Me.VaccineBrandDDL.SelectedItem.Value = String.Empty Then
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
        If String.IsNullOrEmpty(Me.VaccineLotNo) OrElse Me.VaccineLotNoDDL.SelectedItem.Value = String.Empty Then
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
        End If

        Return blnResult
    End Function

#End Region

#Region "Select Vaccine & Dose"
    Public Sub Selection()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = _udtSessionHandler.EHSClaimVaccineGetFromSession()

        'Dose
        If ddlCDoseCovid19.SelectedValue <> String.Empty Then
            Dim strSubsidizeCode As String = String.Empty
            Dim strAvailableItemCode As String = String.Empty
            Dim blnSelected As Boolean = False

            If ddlCDoseCovid19.SelectedValue.Contains("_") Then
                Dim strSelectedValue() As String = Split(ddlCDoseCovid19.SelectedValue, "_")
                strSubsidizeCode = strSelectedValue(0)
                strAvailableItemCode = strSelectedValue(1)
            Else
                strAvailableItemCode = ddlCDoseCovid19.SelectedValue.Trim
            End If

            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

                For Each udtEHSClaimSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                    If udtEHSClaimSubsidizeDetail.AvailableItemCode.ToUpper.Trim = strAvailableItemCode.ToUpper.Trim Then
                        udtEHSClaimSubsidize.Selected = True
                        udtEHSClaimSubsidizeDetail.Selected = True
                        blnSelected = True
                        Exit For
                    End If
                Next

                If blnSelected Then
                    Exit For
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

        dtVaccineLotNo = udtCOVID19BLL.GetCOVID19VaccineLotMappingForPrivate()

        If dtVaccineLotNo.Rows.Count > 0 Then
            Dim drVaccineLotNo() As DataRow = udtCOVID19BLL.FilterVaccineLotNoByServiceDate(dtVaccineLotNo, ServiceDate)

            If drVaccineLotNo.Length > 0 Then
                For intCt As Integer = 0 To drVaccineLotNo.Length - 1
                    If drVaccineLotNo(intCt)("Vaccine_Lot_No").ToString.Trim.ToUpper = txtCVaccineLotNo.Text.Trim.ToUpper Then
                        strVaccineLotID = drVaccineLotNo(intCt)("Vaccine_Lot_ID").ToString.Trim
                        Exit For
                    End If
                Next
            End If

        End If

        'Category
        Dim udtClaimCategory As ClaimCategoryModel = Nothing
        Dim udtClaimCategoryList As ClaimCategoryModelCollection = Nothing

        If Me.CategoryForCOVID19() <> String.Empty Then
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

        ' -----------------------------------------------
        ' Get Latest SchemeSeq Selected
        '------------------------------------------------
        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

        If Not udtSubsidizeLatest Is Nothing Then

            ''Booth
            'If panBooth.Visible Then
            '    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            '    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Booth
            '    udtTransactAdditionfield.AdditionalFieldValueCode = ddlCBooth.SelectedValue.Trim
            '    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            '    udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            '    udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            '    udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            '    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            'End If

            'Main Category
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.MainCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = txtCMainCategory.Text.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            _udtSessionHandler.ClaimCOVID19MainCategorySaveToSession(ddlCMainCategoryCovid19.SelectedValue.Trim, FunctCode)

            'Sub Category
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SubCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = txtCSubCategory.Text.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Vaccine Brand
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineBrand
            udtTransactAdditionfield.AdditionalFieldValueCode = txtCVaccineBrand.Text.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            _udtSessionHandler.ClaimCOVID19VaccineBrandSaveToSession(ddlCVaccineBrandCovid19.SelectedValue.Trim, FunctCode)

            'Vaccine Lot ID.
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineLotID
            udtTransactAdditionfield.AdditionalFieldValueCode = strVaccineLotID
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Vaccine Lot No.
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineLotNo
            'udtTransactAdditionfield.AdditionalFieldValueCode = ddlCVaccineLotNoCovid19.SelectedValue.Trim
            udtTransactAdditionfield.AdditionalFieldValueCode = txtCVaccineLotNo.Text.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtClaimCategory.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            _udtSessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(txtCVaccineLotNo.Text.Trim, FunctCode)
        End If

    End Sub

#End Region

#Region "Other functions"

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
        If udtClaimCategorys IsNot Nothing AndAlso udtClaimCategorys.Count > 0 Then
            Dim strSelectedValue As String = Nothing
            Dim strCategoryCode As String = Nothing

            Dim udtClaimCategory As ClaimCategoryModel = SessionHandler.ClaimCategoryGetFromSession(FunctCode)
            Dim dtClaimCategory As DataTable

            'strSelectedValue = Me.Request.Form(Me.txtCCategory.UniqueID)
            'Hard code to use "Others" Category
            strSelectedValue = "VSSCOVID19"

            If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
                '    If Not SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode) Is Nothing Then
                '        strSelectedValue = SessionHandler.ClaimCOVID19CategoryGetFromSession(FunctCode)
                '    End If
            Else
                SessionHandler.ClaimCOVID19CategorySaveToSession(strSelectedValue, FunctCode)
            End If

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
                strSelectedValue = Me.Request.Form(Me.txtCCategory.UniqueID)
                'strSelectedValue = AntiXssEncoder.HtmlEncode(Me.ddlCCategoryCovid19.SelectedValue, True)
            End If

            'Check selected category whether exists when practice is changed
            Dim udtSelectedCategory As ClaimCategoryModel = udtClaimCategorys.Filter(strCategoryCode)
            If udtSelectedCategory Is Nothing Then
                strSelectedValue = String.Empty
                Me.ddlCCategoryCovid19.Items.Clear()
                Me.ddlCCategoryCovid19.SelectedIndex = -1
                Me.ddlCCategoryCovid19.SelectedValue = Nothing
                Me.ddlCCategoryCovid19.ClearSelection()
                SessionHandler.ClaimCategoryRemoveFromSession(FunctCode)
            End If

            'Build Category dropdownlist
            dtClaimCategory = ClaimCategoryBLL.ConvertCategoryToDatatable(udtClaimCategorys)

            Me.ddlCCategoryCovid19.DataSource = dtClaimCategory

            Me.ddlCCategoryCovid19.DataValueField = ClaimCategoryModel._Subsidize_Code

            If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlCCategoryCovid19.DataTextField = ClaimCategoryModel._Category_Name_Chi
            Else
                Me.ddlCCategoryCovid19.DataTextField = ClaimCategoryModel._Category_Name
            End If

            Me.ddlCCategoryCovid19.Items.Clear()
            Me.ddlCCategoryCovid19.SelectedIndex = -1
            Me.ddlCCategoryCovid19.SelectedValue = Nothing
            Me.ddlCCategoryCovid19.ClearSelection()
            Me.ddlCCategoryCovid19.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCCategoryCovid19.Items.Count > 1 Then
                ddlCCategoryCovid19.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
                'ddlCCategoryCovid19.SelectedIndex = 1
            Else
                ddlCCategoryCovid19.SelectedIndex = 0
            End If

            'Restore the selected value if has value
            If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                For Each li As ListItem In ddlCCategoryCovid19.Items
                    If strSelectedValue = li.Value Then
                        ddlCCategoryCovid19.SelectedValue = li.Value
                    End If
                Next
            End If
        End If

    End Sub

    Private Sub BindMainCategory()
        Dim strSelectedValue As String = Nothing
        Dim strSelectedValueDDL As String = Nothing

        Dim dtMainCategory As DataTable = Nothing

        If MyBase.TranDetailLatestVaccineRecord IsNot Nothing AndAlso _
            MyBase.EHSTransactionLatestVaccineRecord IsNot Nothing AndAlso _
            MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields IsNot Nothing AndAlso _
            MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory IsNot Nothing Then
            'Carry Forward
            dtMainCategory = Status.GetDescriptionAllListFromDBEnumCode("VSSC19MainCategory")
        Else
            'Input
            dtMainCategory = Status.GetDescriptionListFromDBEnumCode("VSSC19MainCategory")
        End If

        'Get the value from request
        If Me.txtCMainCategory.UniqueID.Contains("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$") Then
            strSelectedValue = Me.Request.Form(Me.txtCMainCategory.UniqueID)
        Else
            strSelectedValue = Me.Request.Form("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$" + Me.txtCMainCategory.UniqueID)
        End If

        If Me.MainCategoryDDL.SelectedItem IsNot Nothing Then
            strSelectedValueDDL = Me.MainCategoryDDL.SelectedItem.Value
        End If

        'if the dropdownlist is chosen "Please select", the value of hidden textbox will override to change to empty.
        If strSelectedValueDDL IsNot Nothing AndAlso strSelectedValueDDL = String.Empty Then
            strSelectedValue = String.Empty
        End If

        'If nothing, get value form session
        If strSelectedValue Is Nothing Then
            Dim str As String = SessionHandler.ClaimCOVID19MainCategoryGetFromSession(FunctCode)
            If Not SessionHandler.ClaimCOVID19MainCategoryGetFromSession(FunctCode) Is Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19MainCategoryGetFromSession(FunctCode)
            End If
        Else
            SessionHandler.ClaimCOVID19MainCategorySaveToSession(strSelectedValue, FunctCode)
        End If

        Me.ddlCMainCategoryCovid19.Items.Clear()
        Me.ddlCMainCategoryCovid19.SelectedIndex = -1
        Me.ddlCMainCategoryCovid19.SelectedValue = Nothing
        Me.ddlCMainCategoryCovid19.ClearSelection()

        'Build Vaccine Brand dropdownlist
        If dtMainCategory IsNot Nothing AndAlso dtMainCategory.Rows.Count > 0 Then
            Me.ddlCMainCategoryCovid19.Enabled = True

            Me.ddlCMainCategoryCovid19.DataSource = dtMainCategory

            Me.ddlCMainCategoryCovid19.DataValueField = "Status_Value"

            If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlCMainCategoryCovid19.DataTextField = "Status_Description_Chi"
            Else
                Me.ddlCMainCategoryCovid19.DataTextField = "Status_Description"
            End If

            Me.ddlCMainCategoryCovid19.ClearSelection()
            Me.ddlCMainCategoryCovid19.DataBind()

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCMainCategoryCovid19.Items.Count > 1 Then
                ddlCMainCategoryCovid19.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            Me.ddlCMainCategoryCovid19.SelectedIndex = 0
            Me.txtCMainCategory.Text = ddlCMainCategoryCovid19.Items(0).Value.Trim

            'Restore the selected value if has value
            If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                For Each li As ListItem In ddlCMainCategoryCovid19.Items
                    If strSelectedValue = li.Value Then
                        Me.ddlCMainCategoryCovid19.SelectedValue = li.Value
                        Me.txtCMainCategory.Text = Me.ddlCMainCategoryCovid19.SelectedValue
                    End If
                Next
            End If

            'Carry Forward: Main Category
            If MyBase.TranDetailLatestVaccineRecord IsNot Nothing Then
                Me.ddlCMainCategoryCovid19.Enabled = False
                If MyBase.EHSTransactionLatestVaccineRecord IsNot Nothing Then
                    'EHS Transaction
                    If (MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.VSS OrElse _
                        MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.RVP OrElse _
                        MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19RVP OrElse _
                        MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19OR) AndAlso _
                        MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields IsNot Nothing AndAlso _
                        MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory IsNot Nothing AndAlso _
                        MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory <> String.Empty Then
                        'VSS / RVP / COVID19RVP                 
                        'If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                        Me.ddlCMainCategoryCovid19.Items.Clear()
                        Me.ddlCMainCategoryCovid19.SelectedIndex = -1
                        Me.ddlCMainCategoryCovid19.SelectedValue = Nothing
                        Me.ddlCMainCategoryCovid19.ClearSelection()

                        'For Each li As ListItem In ddlCMainCategoryCovid19.Items
                        '    If MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory.Trim.ToUpper = li.Value.Trim.ToUpper Then
                        '        Me.ddlCMainCategoryCovid19.SelectedValue = li.Value
                        '        Me.txtCMainCategory.Text = Me.ddlCMainCategoryCovid19.SelectedValue
                        '    End If
                        'Next

                        Dim strDescEng As String = String.Empty
                        Dim strDescChi As String = String.Empty

                        Status.GetDescriptionAllFromDBCode("VSSC19MainCategory", MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory.Trim, strDescEng, strDescChi)

                        If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                            ddlCMainCategoryCovid19.Items.Add(New ListItem(strDescChi, MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory.Trim))
                        Else
                            ddlCMainCategoryCovid19.Items.Add(New ListItem(strDescEng, MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory.Trim))
                        End If

                        Me.ddlCMainCategoryCovid19.SelectedValue = MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory.Trim
                        Me.txtCMainCategory.Text = Me.ddlCMainCategoryCovid19.SelectedValue
                        'End If
                    Else
                        'Non-target scheme

                        Me.ddlCMainCategoryCovid19.Enabled = True

                        'Me.ddlCMainCategoryCovid19.Items.Clear()
                        'Me.ddlCMainCategoryCovid19.SelectedIndex = -1
                        'Me.ddlCMainCategoryCovid19.SelectedValue = Nothing
                        'Me.ddlCMainCategoryCovid19.ClearSelection()

                    End If
                Else
                    'CMS / CIMS Record

                    Me.ddlCMainCategoryCovid19.Enabled = True

                    'Me.ddlCMainCategoryCovid19.Items.Clear()
                    'Me.ddlCMainCategoryCovid19.SelectedIndex = -1
                    'Me.ddlCMainCategoryCovid19.SelectedValue = Nothing
                    'Me.ddlCMainCategoryCovid19.ClearSelection()
                End If
            End If

        Else
            Me.ddlCMainCategoryCovid19.Items.Clear()
            Me.ddlCMainCategoryCovid19.SelectedIndex = -1
            Me.ddlCMainCategoryCovid19.SelectedValue = Nothing
            Me.ddlCMainCategoryCovid19.ClearSelection()
            Me.ddlCMainCategoryCovid19.Enabled = False

        End If

    End Sub

    Private Sub BindSubCategory()
        Dim strSelectedValue As String = Nothing
        Dim strSelectedValueDDL As String = Nothing

        Dim dtSubCategory As DataTable = Nothing

        If MyBase.TranDetailLatestVaccineRecord IsNot Nothing AndAlso _
            MyBase.EHSTransactionLatestVaccineRecord IsNot Nothing AndAlso _
            MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields IsNot Nothing AndAlso _
            MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.SubCategory IsNot Nothing Then
            'Carry Forward
            dtSubCategory = Status.GetDescriptionAllListFromDBEnumCode("VSSC19SubCategory")
        Else
            'Input
            dtSubCategory = Status.GetDescriptionListFromDBEnumCode("VSSC19SubCategory")
        End If

        'Get the value from request
        If Me.txtCSubCategory.UniqueID.Contains("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$") Then
            strSelectedValue = Me.Request.Form(Me.txtCSubCategory.UniqueID)
        Else
            strSelectedValue = Me.Request.Form("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$" + Me.txtCSubCategory.UniqueID)
        End If

        If Me.SubCategoryDDL.SelectedItem IsNot Nothing Then
            strSelectedValueDDL = Me.SubCategoryDDL.SelectedItem.Value
        End If

        'if the dropdownlist is chosen "Please select", the value of hidden textbox will override to change to empty.
        If strSelectedValueDDL IsNot Nothing AndAlso strSelectedValueDDL = String.Empty Then
            strSelectedValue = String.Empty
        End If

        'If nothing, get value form session
        If strSelectedValue Is Nothing Then
            If Not SessionHandler.ClaimCOVID19SubCategoryGetFromSession(FunctCode) Is Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19SubCategoryGetFromSession(FunctCode)
            End If
        Else
            SessionHandler.ClaimCOVID19SubCategorySaveToSession(strSelectedValue, FunctCode)
        End If

        Me.ddlCSubCategoryCovid19.Items.Clear()
        Me.ddlCSubCategoryCovid19.SelectedIndex = -1
        Me.ddlCSubCategoryCovid19.SelectedValue = Nothing
        Me.ddlCSubCategoryCovid19.ClearSelection()
        Me.ddlCSubCategoryCovid19.Enabled = False

        If dtSubCategory IsNot Nothing AndAlso dtSubCategory.Rows.Count > 0 Then
            'Build Vaccine Lot No. dropdownlist Filter With Band id
            Dim drSubCategoryFilterByMain() As DataRow = Nothing
            Dim strMainCategory As String = Me.txtCMainCategory.Text.Trim  ' for postback after validation
            If strMainCategory Is Nothing Then
                strMainCategory = Me.ddlCMainCategoryCovid19.SelectedValue ' For the first time render (the Practice only has one brand)
            End If

            'If stVaccinBandID = "", it means no brand is selected. (the Practice has more than one brand = > drVaccineLotNoFilterWithBand.length = 0) 
            drSubCategoryFilterByMain = dtSubCategory.Select(String.Format("Column_Name = '{0}'", strMainCategory))

            'For render the server side dropdown
            If drSubCategoryFilterByMain IsNot Nothing AndAlso drSubCategoryFilterByMain.Length > 0 Then
                Me.ddlCSubCategoryCovid19.Enabled = True
                Me.ddlCSubCategoryCovid19.DataSource = drSubCategoryFilterByMain.CopyToDataTable()

                Me.ddlCSubCategoryCovid19.DataValueField = "Status_Value"

                If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                    Me.ddlCSubCategoryCovid19.DataTextField = "Status_Description_Chi"
                Else
                    Me.ddlCSubCategoryCovid19.DataTextField = "Status_Description"
                End If

                Me.ddlCSubCategoryCovid19.ClearSelection()
                Me.ddlCSubCategoryCovid19.DataBind()

                'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
                If ddlCSubCategoryCovid19.Items.Count > 1 Then
                    ddlCSubCategoryCovid19.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
                End If

                Me.ddlCSubCategoryCovid19.SelectedIndex = 0
                Me.txtCSubCategory.Text = ddlCSubCategoryCovid19.Items(0).Value.Trim

                'Restore the selected value if has value
                If ddlCSubCategoryCovid19.Items.Count > 1 Then
                    If strSelectedValue IsNot Nothing And strSelectedValue <> String.Empty Then
                        For Each li As ListItem In ddlCSubCategoryCovid19.Items
                            If strSelectedValue = li.Value Then
                                Me.ddlCSubCategoryCovid19.SelectedValue = li.Value
                                Me.txtCSubCategory.Text = Me.ddlCSubCategoryCovid19.SelectedValue
                            End If
                        Next
                    End If
                End If

                'Carry Forward: Sub Category
                If MyBase.TranDetailLatestVaccineRecord IsNot Nothing Then
                    Me.ddlCSubCategoryCovid19.Enabled = False
                    If MyBase.EHSTransactionLatestVaccineRecord IsNot Nothing Then
                        'EHS Transaction
                        If (MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.VSS OrElse _
                            MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.RVP OrElse _
                            MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19RVP OrElse _
                            MyBase.EHSTransactionLatestVaccineRecord.SchemeCode.Trim.ToUpper = SchemeClaimModel.COVID19OR) AndAlso _
                            MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields IsNot Nothing AndAlso _
                            MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.SubCategory IsNot Nothing AndAlso _
                            MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.MainCategory <> String.Empty Then
                            'VSS / RVP / COVID19RVP

                            'If _udtSessionHandler.ClaimCOVID19CarryForwordGetFromSession(FunctCode) = False Then
                            Me.ddlCSubCategoryCovid19.Items.Clear()
                            Me.ddlCSubCategoryCovid19.SelectedIndex = -1
                            Me.ddlCSubCategoryCovid19.SelectedValue = Nothing
                            Me.ddlCSubCategoryCovid19.ClearSelection()

                            'For Each li As ListItem In ddlCSubCategoryCovid19.Items
                            '    If MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.SubCategory.Trim.ToUpper = li.Value.Trim.ToUpper Then
                            '        Me.ddlCSubCategoryCovid19.SelectedValue = li.Value
                            '        Me.txtCSubCategory.Text = Me.ddlCSubCategoryCovid19.SelectedValue
                            '    End If
                            'Next

                            Dim strDescEng As String = String.Empty
                            Dim strDescChi As String = String.Empty

                            Status.GetDescriptionAllFromDBCode("VSSC19SubCategory", MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.SubCategory.Trim, strDescEng, strDescChi)

                            If SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                                ddlCSubCategoryCovid19.Items.Add(New ListItem(strDescChi, MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.SubCategory.Trim))
                            Else
                                ddlCSubCategoryCovid19.Items.Add(New ListItem(strDescEng, MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.SubCategory.Trim))
                            End If

                            Me.ddlCSubCategoryCovid19.SelectedValue = MyBase.EHSTransactionLatestVaccineRecord.TransactionAdditionFields.SubCategory.Trim
                            Me.txtCSubCategory.Text = Me.ddlCSubCategoryCovid19.SelectedValue
                            'End If
                        Else
                            'Non-target scheme
                            Me.ddlCSubCategoryCovid19.Enabled = True

                            'Me.ddlCSubCategoryCovid19.Items.Clear()
                            'Me.ddlCSubCategoryCovid19.SelectedIndex = -1
                            'Me.ddlCSubCategoryCovid19.SelectedValue = Nothing
                            'Me.ddlCSubCategoryCovid19.ClearSelection()

                        End If

                    Else
                        'CMS / CIMS Record

                        Me.ddlCSubCategoryCovid19.Enabled = True

                        'Me.ddlCSubCategoryCovid19.Items.Clear()
                        'Me.ddlCSubCategoryCovid19.SelectedIndex = -1
                        'Me.ddlCSubCategoryCovid19.SelectedValue = Nothing
                        'Me.ddlCSubCategoryCovid19.ClearSelection()
                    End If
                End If

            End If

        End If

    End Sub

    Private Sub BindCOVID19VaccineBrand(drVaccineLotNo() As DataRow)
        Dim strSelectedValue As String = Nothing
        Dim strSelectedValueDDL As String = Nothing

        Dim dtVaccineBrand As DataTable = Nothing

        'Get the value from request
        If Me.txtCVaccineBrand.UniqueID.Contains("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$") Then
            strSelectedValue = Me.Request.Form(Me.txtCVaccineBrand.UniqueID)
        Else
            strSelectedValue = Me.Request.Form("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$" + Me.txtCVaccineBrand.UniqueID)
        End If

        If Me.VaccineBrandDDL.SelectedItem IsNot Nothing Then
            strSelectedValueDDL = Me.VaccineBrandDDL.SelectedItem.Value
        End If

        'if the dropdownlist is chosen "Please select", the value of hidden textbox will override to change to empty.
        If strSelectedValueDDL IsNot Nothing AndAlso strSelectedValueDDL = String.Empty Then
            strSelectedValue = String.Empty
        End If

        'If nothing, get value form session
        If strSelectedValue Is Nothing Then
            Dim str As String = SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode)
            If Not SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode) Is Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19VaccineBrandGetFromSession(FunctCode)
            End If
        Else
            SessionHandler.ClaimCOVID19VaccineBrandSaveToSession(strSelectedValue, FunctCode)
        End If

        Me.ddlCVaccineBrandCovid19.Items.Clear()
        Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
        Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
        Me.ddlCVaccineBrandCovid19.ClearSelection()

        'Build Vaccine Brand dropdownlist
        If drVaccineLotNo.Length > 0 Then
            Me.ddlCVaccineBrandCovid19.Enabled = True

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
        Dim strSelectedValueDDL As String = Nothing

        'Get the value from request
        If Me.txtCVaccineLotNo.UniqueID.Contains("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$") Then
            strSelectedValue = Me.Request.Form(Me.txtCVaccineLotNo.UniqueID)
        Else
            strSelectedValue = Me.Request.Form("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$" + Me.txtCVaccineLotNo.UniqueID)
        End If

        If Me.VaccineLotNoDDL.SelectedItem IsNot Nothing Then
            strSelectedValueDDL = Me.VaccineLotNoDDL.SelectedItem.Value
        End If

        'if the dropdownlist is chosen "Please select", the value of hidden textbox will override to change to empty.
        If strSelectedValueDDL IsNot Nothing AndAlso strSelectedValueDDL = String.Empty Then
            strSelectedValue = String.Empty
        End If

        'If nothing, get value form session
        If strSelectedValue Is Nothing Then
            If Not SessionHandler.ClaimCOVID19VaccineLotNoGetFromSession(FunctCode) Is Nothing Then
                strSelectedValue = SessionHandler.ClaimCOVID19VaccineLotNoGetFromSession(FunctCode)
            End If
        Else
            SessionHandler.ClaimCOVID19VaccineLotNoSaveToSession(strSelectedValue, FunctCode)
        End If

        Me.ddlCVaccineLotNoCovid19.Items.Clear()
        Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
        Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
        Me.ddlCVaccineLotNoCovid19.ClearSelection()
        Me.ddlCVaccineLotNoCovid19.Enabled = False

        If drVaccineLotNo.Length > 0 Then
            'Build Vaccine Lot No. dropdownlist Filter With Band id
            Dim drVaccineLotNoFilterWithBand() As DataRow = Nothing
            Dim strVaccinBandID As String = Me.txtCVaccineBrand.Text.Trim  ' for postback after validation
            If strVaccinBandID Is Nothing Then
                strVaccinBandID = Me.ddlCVaccineBrandCovid19.SelectedValue ' For the first time render (the Practice only has one brand)

                If Me.txtCVaccineBrand.UniqueID.Contains("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$") Then
                    strVaccinBandID = Me.Request.Form(Me.txtCVaccineBrand.UniqueID)
                Else
                    strVaccinBandID = Me.Request.Form("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$" + Me.txtCVaccineBrand.UniqueID)
                End If
            End If

            'If stVaccinBandID = "", it means no brand is selected. (the Practice has more than one brand = > drVaccineLotNoFilterWithBand.length = 0) 
            drVaccineLotNoFilterWithBand = drVaccineLotNo.CopyToDataTable().Select(String.Format("Brand_ID = '{0}'", strVaccinBandID))

            'For render the server side dropdown
            If drVaccineLotNoFilterWithBand.Length > 0 Then
                Me.ddlCVaccineLotNoCovid19.Enabled = True
                Me.ddlCVaccineLotNoCovid19.DataSource = drVaccineLotNoFilterWithBand.CopyToDataTable()

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

                'Else
                '    Me.ddlCVaccineLotNoCovid19.Items.Clear()
                '    Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
                '    Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
                '    Me.ddlCVaccineLotNoCovid19.ClearSelection()
                '    Me.ddlCVaccineLotNoCovid19.Enabled = False

            End If

        End If

    End Sub

    Private Sub BindCOVID19Dose()
        Dim strSelectedValue As String = Nothing

        'strSelectedValue = AntiXssEncoder.HtmlEncode(Me.ddlCDoseCovid19.SelectedValue, True)

        'Get the value from request
        If Me.ddlCDoseCovid19.UniqueID.Contains("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$") Then
            strSelectedValue = Me.Request.Form(Me.ddlCDoseCovid19.UniqueID)
        Else
            strSelectedValue = Me.Request.Form("ctl00$ContentPlaceHolder1$udcStep2aInputEHSClaim$" + Me.ddlCDoseCovid19.UniqueID)
        End If

        'Set selected if "1st Dose" exists
        If strSelectedValue = String.Empty AndAlso _udtSessionHandler.ClaimCOVID19DoseGetFromSession(FunctCode) Is Nothing Then
            For Each li As ListItem In ddlCDoseCovid19.Items
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
            If ddlCDoseCovid19.Items.Count > 0 Then
                SessionHandler.ClaimCOVID19DoseSaveToSession(strSelectedValue, FunctCode)
            End If
        End If

        'Bind Dose into dropdownlist
        Me.ddlCDoseCovid19.Items.Clear()
        Me.ddlCDoseCovid19.SelectedIndex = -1
        Me.ddlCDoseCovid19.SelectedValue = Nothing
        Me.ddlCDoseCovid19.ClearSelection()

        If strSelectedValue Is Nothing OrElse strSelectedValue = String.Empty Then
            strSelectedValue = String.Empty
        End If

        'Build Dose dropdownlist
        Dim lstItem As ListItem = Nothing

        If EHSClaimVaccine IsNot Nothing Then
            For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In EHSClaimVaccine.SubsidizeList
                For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                    If udtEHSClaimSubidizeDetail.Available Then
                        lstItem = New ListItem
                        'lstItem.Value = udtSubsidize.SubsidizeCode + "_" + udtEHSClaimSubidizeDetail.AvailableItemCode
                        lstItem.Value = udtEHSClaimSubidizeDetail.AvailableItemCode
                        'lstItem.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc(SessionHandler.Language) + " (" + udtSubsidize.Amount.ToString + ")"
                        lstItem.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc(SessionHandler.Language)

                        ddlCDoseCovid19.Items.Add(lstItem)
                    End If
                Next
            Next

            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            If ddlCDoseCovid19.Items.Count > 1 Then
                ddlCDoseCovid19.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
            End If

            If ddlCDoseCovid19.Items.Count > 0 Then
                ddlCDoseCovid19.SelectedIndex = 0
            Else
                ddlCDoseCovid19.Enabled = False
                ddlCDoseCovid19.Dispose()
            End If

        End If

        'Restore the selected value if has value
        If strSelectedValue IsNot Nothing Then
            For Each li As ListItem In ddlCDoseCovid19.Items
                If strSelectedValue = li.Value Then
                    ddlCDoseCovid19.SelectedValue = li.Value
                End If
            Next
        End If

    End Sub

#End Region

    Private Sub imgCCategoryInfo_Click(sender As Object, e As ImageClickEventArgs) Handles imgCCategoryInfo.Click
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "CategoryList", String.Format("javascript:showCategoryInfo('{0}');", Session("language")), True)

    End Sub

End Class
