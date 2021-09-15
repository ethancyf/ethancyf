Imports System.Web.Security.AntiXss
Imports Common
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports HCVU.BLL

Partial Public Class ucInputRVPCOVID19
    Inherits ucInputEHSClaimBase

    Private _udtSessionHandler As New BLL.SessionHandlerBLL
    Private _udtGeneralFunction As New GeneralFunction
    Private _udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL

#Region "Constants"

#End Region

#Region "Properties"
    Public ReadOnly Property ContactNo() As String
        Get
            If Me.txtCContactNo Is Nothing Then
                Return String.Empty
            Else
                Return Me.txtCContactNo.Text
            End If
        End Get
    End Property

#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()

    End Sub

    Protected Overrides Sub Setup()

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

        dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForRCH()

        If dtVaccineLotNo.Rows.Count > 0 Then
            Dim drVaccineLotNo() As DataRow = dtVaccineLotNo.Select

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
        If MyBase.EHSAccount IsNot Nothing AndAlso MyBase.EHSAccount.SearchDocCode IsNot Nothing Then
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

    End Sub

    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)
        Me.Setup()
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetDetailError(ByVal blnVisible As Boolean)
        Me.imgCRecipientTypeError.Visible = blnVisible
        Me.imgCVaccineBrandError.Visible = blnVisible
        Me.imgCVaccineLotNoError.Visible = blnVisible
        Me.imgCContactNoError.Visible = blnVisible
    End Sub

#End Region

#Region "Set Value"

    Public Sub InitialClaimDetail()
        Me.ddlCVaccineBrandCovid19.Items.Clear()
        Me.ddlCVaccineBrandCovid19.SelectedIndex = -1
        Me.ddlCVaccineBrandCovid19.SelectedValue = Nothing
        Me.ddlCVaccineBrandCovid19.ClearSelection()

        Me.ddlCVaccineLotNoCovid19.Items.Clear()
        Me.ddlCVaccineLotNoCovid19.SelectedIndex = -1
        Me.ddlCVaccineLotNoCovid19.SelectedValue = Nothing
        Me.ddlCVaccineLotNoCovid19.ClearSelection()

        Me.txtCRemark.Text = String.Empty

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
    Private Sub ddlCVaccineBrandCovid19_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCVaccineBrandCovid19.SelectedIndexChanged
        Me.ddlCVaccineBrandCovid19.SelectedValue = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.ddlCVaccineBrandCovid19.UniqueID), True)
        'sessionhandler.claimcovid19vaccinebrandsavetosession(Me.ddlcvaccinebrandcovid19.selectedvalue, functcode)

        'Get Vaccine Brand & Lot No.
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim dtVaccineLotNo As DataTable = Nothing

        dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForRCH()

        If dtVaccineLotNo.Rows.Count > 0 Then

            Dim drVaccineLotNo() As DataRow = dtVaccineLotNo.Select()

            ''Bind DropDownList Vaccine Brand
            'BindCOVID19VaccineBrand(drVaccineLotNo)

            'Bind DropDownList Vaccine Lot No.
            BindCOVID19VaccineLotNo(drVaccineLotNo)

        End If
    End Sub

    Private Sub rblCRecipientType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblCRecipientType.SelectedIndexChanged

    End Sub

#End Region

#Region "Control Binding"

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

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        'Check Recipient Type
        If String.IsNullOrEmpty(Me.rblCRecipientType.SelectedValue) Then
            blnResult = False

            imgCRecipientTypeError.Visible = True

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

        'COVID19 Vaccination
        If panCOVID19Detail.Visible Then
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

        End If

        If rblCRecipientType.SelectedValue <> RECIPIENT_TYPE.RESIDENT AndAlso rblCRecipientType.SelectedValue <> String.Empty Then
            'Contact No. : Not empty
            'If String.IsNullOrEmpty(Me.txtCContactNo.Text) AndAlso Me.chkStep2aMobile.Checked Then
            If String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
                blnResult = False

                imgCContactNoError.Visible = True

                Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00463)
                If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))
            End If

            'Contact No. : format
            If Not String.IsNullOrEmpty(Me.txtCContactNo.Text) Then
                If Not Regex.IsMatch(Me.txtCContactNo.Text, "^[2-9]\d{7}$") Then
                    blnResult = False

                    imgCContactNoError.Visible = True

                    Dim udtMsg As ComObject.SystemMessage = New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00466)
                    If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(udtMsg, "%en", GetGlobalResourceObject("Text", "ContactNo2"))
                End If

            End If
        End If

        Return blnResult

    End Function

#End Region

#Region "Save"

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal strRCHType As String)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel = Nothing

        If udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
        End If

        'Get Vaccine Brand & Lot No.
        Dim udtCOVID19BLL As New Common.Component.COVID19.COVID19BLL
        Dim dtVaccineLotNo As DataTable = Nothing
        Dim strVaccineLotID As String = String.Empty

        dtVaccineLotNo = udtCOVID19BLL.GetALLCOVID19VaccineLotMappingForRCH()

        If dtVaccineLotNo.Rows.Count > 0 Then
            Dim drVaccineLotNo() As DataRow = dtVaccineLotNo.Select

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

            'Outreach Type
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.OutreachType
            udtTransactAdditionfield.AdditionalFieldValueCode = TYPE_OF_OUTREACH.RCH
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

            'Recipient Type
            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.RecipientType
            udtTransactAdditionfield.AdditionalFieldValueCode = rblCRecipientType.SelectedValue
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode.Trim()
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
                    Select Case strRCHType
                        Case RCH_TYPE.RCHE
                            strSubCategory = "GL13"
                        Case RCH_TYPE.RCHD
                            strSubCategory = "GL14"
                    End Select

                Case RECIPIENT_TYPE.RCH_STAFF
                    Select Case strRCHType
                        Case RCH_TYPE.RCHE
                            strSubCategory = "GL15"
                        Case RCH_TYPE.RCHD
                            strSubCategory = "GL16"
                    End Select

                Case RECIPIENT_TYPE.CCSU_STAFF
                    Select Case strRCHType
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

            'Non-Local Recoverd History
            Dim strNonLocalRecoveredHistory As String = String.Empty

            strNonLocalRecoveredHistory = IIf(chkCNonLocalRecoveredHistory.Checked, YesNo.Yes, YesNo.No)

            udtTransactAdditionfield = New TransactionAdditionalFieldModel()
            udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.NonLocalRecoveredHistory
            udtTransactAdditionfield.AdditionalFieldValueCode = strNonLocalRecoveredHistory
            udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
            udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
            udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
            udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
            udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
        End If

    End Sub

#End Region

End Class
