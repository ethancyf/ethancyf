Imports System.Web.Security.AntiXss
Imports Common
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports HCVU.BLL

Partial Public Class ucInputVSSCOVID19
    Inherits ucInputEHSClaimBase

    ''Events 
    'Public Event SearchPIDClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Dim _udtSessionHandler As New SessionHandlerBLL

#Region "Constants"
    'Public Class PID_DOCUMENTARYPROOF
    '    Public Const PID_INSTITUTION_CERT As String = "PID_I_CERT"
    'End Class

    'Public Class RCH_TYPE
    '    Public Const ALL As String = ""
    '    Public Const PID As String = "I"
    'End Class
#End Region

#Region "Properties"
    'Public ReadOnly Property DocumentaryProof() As String
    '    Get
    '        Return Me.Request.Form(Me.ddlDocumentaryProof.UniqueID)
    '    End Get
    'End Property

    'Public ReadOnly Property PIDInstitutionCode() As String
    '    Get
    '        Return Me.Request.Form(Me.txtPIDInstitutionCode.UniqueID)
    '    End Get
    'End Property
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()

    End Sub

    Protected Overrides Sub Setup()

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

        dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForPrivate()

        If dtVaccineLotNo.Rows.Count > 0 Then
            'CRE20-023 Fix the vaccine lot not filtered by service date and record status [Start][Nichole]
            Dim drVaccineLotNo() As DataRow = dtVaccineLotNo.Select
            'Dim drVaccineLotNo() As DataRow = udtCOVID19BLL.FilterVaccineLotNoByServiceDate(dtVaccineLotNo, MyBase.ServiceDate)
            'CRE20-023 Fix the vaccine lot not filtered by service date and record status [End][Nichole]

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
        If MyBase.EHSAccount IsNot Nothing AndAlso MyBase.EHSAccount.SearchDocCode IsNot Nothing Then
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

    End Sub

    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)
        Me.Setup()
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetDetailError(ByVal blnVisible As Boolean)
        Me.imgCMainCategoryError.Visible = blnVisible
        Me.imgCSubCategoryError.Visible = blnVisible
        Me.imgCVaccineBrandError.Visible = blnVisible
        Me.imgCVaccineLotNoError.Visible = blnVisible
        Me.imgCContactNoError.Visible = blnVisible
    End Sub

#End Region

#Region "Set Value"

    Public Sub InitialClaimDetail()
        Me.ddlCMainCategory.Items.Clear()
        Me.ddlCMainCategory.SelectedIndex = -1
        Me.ddlCMainCategory.SelectedValue = Nothing
        Me.ddlCMainCategory.ClearSelection()

        Me.ddlCSubCategory.Items.Clear()
        Me.ddlCSubCategory.SelectedIndex = -1
        Me.ddlCSubCategory.SelectedValue = Nothing
        Me.ddlCSubCategory.ClearSelection()

        Me.ddlCVaccineBrandCovid19.Items.Clear()
        Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
        Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
        Me.ddlCVaccineBrandCovid19.ClearSelection()

        Me.ddlCVaccineLotNoCovid19.Items.Clear()
        Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
        Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
        Me.ddlCVaccineLotNoCovid19.ClearSelection()

        Me.txtCContactNo.Text = String.Empty

        Me.txtCRemark.Text = String.Empty

        Me.chkCJoinEHRSS.Checked = False

    End Sub

#End Region

#Region "Events"

#End Region

#Region "Control Binding"

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

#End Region

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        'COVID19 Vaccination
        If panCOVID19Detail.Visible Then
            'Check Main Category
            If String.IsNullOrEmpty(Me.ddlCMainCategory.SelectedValue) Then
                blnResult = False

                imgCMainCategoryError.Visible = True

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "Category"))
                End If
            End If

            'Check Sub Category
            Dim drSubCategory() As DataRow = Status.GetDescriptionListFromDBEnumCode("VSSC19SubCategory").Select(String.Format("Column_Name='{0}'", Me.ddlCMainCategory.SelectedValue))

            If String.IsNullOrEmpty(Me.ddlCSubCategory.SelectedValue) AndAlso drSubCategory.Length > 0 Then
                'If String.IsNullOrEmpty(Me.ddlCSubCategory.SelectedValue) AndAlso Me.ddlCMainCategory.SelectedValue.Trim.ToUpper <> "PG6" Then
                blnResult = False

                imgCSubCategoryError.Visible = True

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "SubCategory"))
                End If
            End If

            'Check Vaccine Brand
            If String.IsNullOrEmpty(Me.ddlCVaccineBrandCovid19.SelectedValue) Then
                blnResult = False

                imgCVaccineBrandError.Visible = True

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "Vaccines"))
                End If
            End If

            'Check Vaccine Lot No.
            If String.IsNullOrEmpty(Me.ddlCVaccineLotNoCovid19.SelectedValue) Then
                blnResult = False

                imgCVaccineLotNoError.Visible = True

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00462)
                If objMsgBox IsNot Nothing Then
                    objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "VaccineLotNumber"))
                End If
            End If

            ' Contact No. : Not empty
            If String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
                blnResult = False

                imgCContactNoError.Visible = True

                objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))

            End If

            ' Contact No. : Format 20000000 to 99999999
            If Not String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
                If Not Regex.IsMatch(Me.txtCContactNo.Text, "^[2-9]\d{7}$") Then
                    blnResult = False

                    imgCContactNoError.Visible = True

                    objMsg = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                    If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))

                End If

            End If

        End If

        Return blnResult

    End Function

#End Region

#Region "Save"

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel = Nothing
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        'Get Vaccine Brand & Lot No.
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim dtVaccineLotNo As DataTable = Nothing
        Dim strVaccineLotID As String = String.Empty

        dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForPrivate()

        If dtVaccineLotNo.Rows.Count > 0 Then
            'CRE20-023 Fix the vaccine lot not filtered by service date and record status [Start][Nichole]
            Dim drVaccineLotNo() As DataRow = dtVaccineLotNo.Select
            'Dim drVaccineLotNo() As DataRow = udtCOVID19BLL.FilterVaccineLotNoByServiceDate(dtVaccineLotNo, ServiceDate)
            'CRE20-023 Fix the vaccine lot not filtered by service date and record status [End][Nichole]

            If drVaccineLotNo.Length > 0 Then
                For intCt As Integer = 0 To drVaccineLotNo.Length - 1
                    If drVaccineLotNo(intCt)("Vaccine_Lot_No").ToString.Trim.ToUpper = ddlCVaccineLotNoCovid19.SelectedValue.Trim.ToUpper Then
                        strVaccineLotID = drVaccineLotNo(intCt)("Vaccine_Lot_ID").ToString.Trim
                        Exit For
                    End If
                Next
            End If

        End If

        ' -----------------------------------------------
        ' Get Latest SchemeSeq Selected
        '------------------------------------------------
        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

        If Not udtSubsidizeLatest Is Nothing Then

            'Main Category
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.MainCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = ddlCMainCategory.SelectedValue.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Sub Category
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.SubCategory
            udtTransactAdditionfield.AdditionalFieldValueCode = ddlCSubCategory.SelectedValue.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
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

            'Vaccine Lot ID.
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.VaccineLotID
            udtTransactAdditionfield.AdditionalFieldValueCode = strVaccineLotID
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

            'Contact No.
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo
            udtTransactAdditionfield.AdditionalFieldValueCode = txtCContactNo.Text.Trim
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            ''Mobile
            'udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            'udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Mobile
            'udtTransactAdditionfield.AdditionalFieldValueCode = IIf(Me.chkStep2aMobile.Checked, YesNo.Yes, YesNo.No)
            'udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            'udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            'udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            'udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            'Me._udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Remarks
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.Remarks
            udtTransactAdditionfield.AdditionalFieldValueCode = String.Empty
            udtTransactAdditionfield.AdditionalFieldValueDesc = txtCRemark.Text.Trim
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'JoinEHRSS
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

End Class
