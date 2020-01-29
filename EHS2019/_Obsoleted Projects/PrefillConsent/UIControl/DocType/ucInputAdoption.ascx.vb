Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.StaticData
Imports System.Web.UI.ScriptManager

Partial Public Class ucInputAdoption
    Inherits ucInputDocTypeBase

    Private _strIdentityNo As String
    Private _strPrefixNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOBInWord As String
    Private _strIsExactDOB As String
    Private _blnDOBTypeSelected As Boolean
    Private _blnDOBInWordCase As Boolean
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL


    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.ADOPC)

        If MyBase.SessionHandler().Language = Common.Component.CultureLanguage.English Then
            Me.lblEntryNoText.Text = udtDocTypeModel.DocIdentityDesc
        Else
            Me.lblEntryNoText.Text = udtDocTypeModel.DocIdentityDescChi
        End If
        'Me.lblEntryNo.Text = Me.GetGlobalResourceObject("Text", "NoOfEntry")
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.rbDOBInWord.Text = Me.GetGlobalResourceObject("Text", "DOBInWordShort")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgEntryNo.ImageUrl = strErrorImageURL
        Me.imgEntryNo.AlternateText = strErrorImageALT

        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT

        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT

        Me.imgDOBInWordError.ImageUrl = strErrorImageURL
        Me.imgDOBInWordError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        'Tips
        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))
        'Me.lblENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")
        'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintADOPC")
        ''Me.lblEntryNoTip.Text = Me.GetGlobalResourceObject("Text", "NoOfEntryHint")
        'Me.lblEntryNoTip.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

    End Sub

    Private Sub DOBInWordOption(ByVal enable As Boolean)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
        Dim dataTable As DataTable
        Dim dtDOBinWorType As DataTable = New DataTable
        Dim dataRow As DataRow

        If enable Then
            dataTable = udtStaticDataBLL.GetStaticDataList("DOBInWordType")

            ' in readonly only.  The drop down list is disabled
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.ddlDOBinWordType.Enabled = True
                'Me.ddlDOBinWordType.BackColor = Drawing.Color.White
            End If

            dataRow = dataTable.NewRow
            dataRow(StaticDataModel.Column_Name) = 0
            dataRow(StaticDataModel.Item_No) = String.Empty
            dataRow(StaticDataModel.Data_Value) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
            dataRow(StaticDataModel.Data_Value_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
            dataTable.Rows.InsertAt(dataRow, 0)

            Me.ddlDOBinWordType.DataSource = dataTable
            If MyBase.SessionHandler().Language() = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value_Chi
            Else
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value
            End If
            Me.ddlDOBinWordType.DataValueField = StaticDataModel.Item_No
            Me.ddlDOBinWordType.DataBind()

            Me.ddlDOBinWordType.SelectedValue = Me._strDOBInWord
        Else
            Me.ddlDOBinWordType.Enabled = False
            'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver

            Me.ddlDOBinWordType.Items.Clear()
            If MyBase.SessionHandler().Language() = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlDOBinWordType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), String.Empty))
            Else
                Me.ddlDOBinWordType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), String.Empty))
            End If
        End If

    End Sub

    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        If Not MyBase.EHSPersonalInfo Is Nothing Then
            Me._strIdentityNo = MyBase.EHSPersonalInfo.IdentityNum
            Me._strPrefixNo = MyBase.EHSPersonalInfo.AdoptionPrefixNum
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB
            Me._strDOBInWord = MyBase.EHSPersonalInfo.OtherInfo

            Me.SetValue(mode)

        End If

        Me.SetEntryNoError(False)
        Me.SetENameError(False)
        Me.SetGenderError(False)
        Me.SetDOBInWordError(False)
        Me.SetDOBError(False)

        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me.lblEntryNo.Visible = False
            Me.txtPerfixNo.Visible = True
            Me.lblEntryNoSymbol.Visible = True
            Me.txtIdentityNo.Visible = True

            'Me.lblDOBTip.Visible = False
        Else
            Me.lblEntryNo.Visible = True
            Me.txtPerfixNo.Visible = False
            Me.lblEntryNoSymbol.Visible = False
            Me.txtIdentityNo.Visible = False

            Me.lblDOBTip.Visible = True
        End If

        Me.SetReferenceNo()
        Me.SetTransactionNo()

        Me.DOBInWordOption(Me.rbDOBInWord.Checked)

    End Sub

    Private Sub ChangeDOBOption(ByVal enable As Boolean)
        Dim ScriptManager1 As ScriptManager = ScriptManager.GetCurrent(Page)
        If Not enable Then
            'Me.txtDOB.Enabled = True
            'Me.txtDOBInWord.Enabled = False
            Me.ddlDOBinWordType.Enabled = False
            'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
            Me.txtDOBInWord.Text = String.Empty
            'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
            '    Me.txtDOBInWord.Text = String.Empty
            'End If
            ScriptManager1.SetFocus(txtDOB)
        Else
            'Me.txtDOB.Enabled = False
            'Me.txtDOBInWord.Enabled = True
            Me.ddlDOBinWordType.Enabled = True
            'Me.ddlDOBinWordType.BackColor = Drawing.Color.White
            Me.txtDOB.Text = String.Empty
            'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
            '    Me.txtDOB.Text = String.Empty
            'End If
            ScriptManager1.SetFocus(txtDOBInWord)
        End If
    End Sub

#Region "Events"

    Protected Sub rbDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDOB.CheckedChanged, rbDOBInWord.CheckedChanged
        'Update DOB Options
        Me.ChangeDOBOption(Me.rbDOBInWord.Checked)
        'Update Drop Down List (DOB in word)
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)

        'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
        '    Me.txtDOB.Enabled = False
        '    Me.txtDOBInWord.Enabled = False
        'End If
    End Sub

#End Region

#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        Me.SetupEntryNo()
        Me.SetupENameFirstName()
        Me.SetupENameSurName()
        Me.SetupGender()
        Me.SetupDOB()
        Me.SetReferenceNo()
        Me.SetTransactionNo()
    End Sub

    Public Sub SetupEntryNo()
        Me.txtIdentityNo.Text = Me._strIdentityNo
        Me.txtPerfixNo.Text = Me._strPrefixNo

        Me.lblEntryNo.Text = Me._strPrefixNo + "/" + Me._strIdentityNo
    End Sub

    Public Sub SetupENameFirstName()
        Me.txtENameFirstname.Text = Me._strENameFirstName
    End Sub

    Public Sub SetupENameSurName()
        Me.txtENameSurname.Text = Me._strENameSurName
    End Sub

    Public Sub SetupGender()
        If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
            Me.rbGender.SelectedValue = Me._strGender
        End If
    End Sub

    Public Sub SetupDOB()
        'Fill Data - DOB
        'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
        '    Me.txtDOB.Text = Me._strDOB
        '    Me.txtDOBInWord.Text = Me._strDOB

        '    If Me._blnDOBTypeSelected Then
        '        Me.SetupDOBType()
        '    End If
        'Else
        '    Me.SetupDOBType()
        'End If

        Me.SetupDOBType()
    End Sub

    Public Sub SetupDOBType()
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then

                If Not Me.rbDOBInWord.Checked Then
                    Me.rbDOB.Checked = True
                    Me.rbDOBInWord.Checked = False

                    'Me.txtDOB.Enabled = True
                    'Me.txtDOBInWord.Enabled = False
                    Me.ddlDOBinWordType.Enabled = False
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
                End If
                Me.txtDOB.Text = Me._strDOB
            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                If Not Me.rbDOB.Checked Then
                    Me.rbDOB.Checked = False
                    Me.rbDOBInWord.Checked = True

                    'Me.txtDOBInWord.Enabled = True
                    'Me.txtDOB.Enabled = False
                    Me.ddlDOBinWordType.Enabled = True
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.White
                End If
                Me.txtDOBInWord.Text = Me._strDOB
            Else
                Me.rbDOB.Checked = False
                Me.rbDOBInWord.Checked = False
            End If
        Else
            'Modification Mode
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then
                If Not Me.rbDOBInWord.Checked Then
                    Me.rbDOB.Checked = True
                    'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                    '    Me.txtDOB.Enabled = True
                    'Else
                    '    Me.txtDOB.Enabled = False
                    'End If

                    Me.rbDOBInWord.Checked = False
                    'Me.txtDOBInWord.Enabled = False
                    Me.ddlDOBinWordType.Enabled = False
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
                End If
                Me.txtDOB.Text = Me._strDOB
            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                If Not Me.rbDOB.Checked Then
                    Me.rbDOBInWord.Checked = True
                    'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                    '    Me.txtDOBInWord.Enabled = True
                    'Else
                    '    Me.txtDOBInWord.Enabled = False
                    'End If

                    Me.rbDOB.Checked = False
                    'Me.txtDOB.Enabled = False
                    Me.ddlDOBinWordType.Enabled = True
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.White
                End If
                Me.txtDOBInWord.Text = Me._strDOB
            Else
                Me.rbDOB.Checked = False
                Me.rbDOBInWord.Checked = False
                'Me.rbDOBInWord.Font.Bold = False
            End If
        End If
    End Sub

    'unqiue for modification and modification read-only modes
    Public Sub SetReferenceNo()
        If Me._strReferenceNo.Trim.Equals(String.Empty) Then
            Me.trReferenceNo_M.Visible = False
        Else
            Me.trReferenceNo_M.Visible = True
            Me.lblReferenceNo_M.Text = Me._strReferenceNo
        End If
    End Sub

    'unqiue for modification and modification read-only modes
    Public Sub SetTransactionNo()
        If Me._strTransNo.Trim.Equals(String.Empty) Then
            Me.trTransactionNo_M.Visible = False
        Else
            Me.trTransactionNo_M.Visible = True
            Me.lblTransactionNo_M.Text = Me._strTransNo
        End If
    End Sub
#End Region

#Region "Set Up Error Image"

    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetupEntryNo()
        Me.SetupENameFirstName()
        Me.SetupENameSurName()
        Me.SetupGender()
        Me.SetupDOB()
        Me.SetReferenceNo()
        Me.SetTransactionNo()
    End Sub

    Public Sub SetEntryNoError(ByVal blnVisible As Boolean)
        Me.imgEntryNo.Visible = blnVisible
    End Sub

    Public Sub SetENameError(ByVal blnVisible As Boolean)
        Me.imgEName.Visible = blnVisible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGender.Visible = visible
    End Sub

    Public Sub SetDOBInWordError(ByVal visible As Boolean)
        Me.imgDOBInWordError.Visible = visible
    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        Me.imgDOBError.Visible = visible
    End Sub

#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty
        Me._strIdentityNo = Me.txtIdentityNo.Text.Trim()
        Me._strPrefixNo = Me.txtPerfixNo.Text.Trim()

        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strGender = Me.rbGender.SelectedValue.Trim
        Me._strDOB = Me.txtDOB.Text.Trim

        Me._strDOBInWord = Me.ddlDOBinWordType.SelectedValue

        If Me.rbDOB.Checked Then
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
            If Not strDOBtype.Trim.Equals(String.Empty) Then
                Me._strIsExactDOB = strDOBtype
            Else
                'in case of empty DOB
                Me._strIsExactDOB = "D"
            End If
            Me._blnDOBInWordCase = False
        ElseIf Me.rbDOBInWord.Checked Then
            Me._strDOB = Me.txtDOBInWord.Text
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)
            If Not strDOBtype.Trim.Equals(String.Empty) Then
                Me._strIsExactDOB = strDOBtype
            Else
                'in case of empty DOB
                Me._strIsExactDOB = "T"
            End If
            Me._blnDOBInWordCase = True
        Else
            Me._strIsExactDOB = String.Empty
            Me._blnDOBTypeSelected = False
        End If
    End Sub

    Public Property PerfixNo() As String
        Get
            Return Me._strPrefixNo
        End Get
        Set(ByVal value As String)
            Me._strPrefixNo = value
        End Set
    End Property

    Public Property IdentityNo() As String
        Get
            Return Me._strIdentityNo
        End Get
        Set(ByVal value As String)
            Me._strIdentityNo = value
        End Set
    End Property

    Public Property ENameFirstName() As String
        Get
            Return Me._strENameFirstName
        End Get
        Set(ByVal value As String)
            Me._strENameFirstName = value
        End Set
    End Property

    Public Property ENameSurName() As String
        Get
            Return Me._strENameSurName
        End Get
        Set(ByVal value As String)
            Me._strENameSurName = value
        End Set
    End Property

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property DOB() As String
        Get
            Return Me._strDOB
        End Get
        Set(ByVal value As String)
            Me._strDOB = value
        End Set
    End Property

    Public Property IsExactDOB() As String
        Get
            Return Me._strIsExactDOB
        End Get
        Set(ByVal value As String)
            Me._strIsExactDOB = value
        End Set
    End Property

    Public Property DOBInWord() As String
        Get
            Return Me._strDOBInWord
        End Get
        Set(ByVal value As String)
            Me._strDOBInWord = value
        End Set
    End Property

    Public Property DOBInWordCase() As Boolean
        Get
            Return Me._blnDOBInWordCase
        End Get
        Set(ByVal value As Boolean)
            Me._blnDOBInWordCase = value
        End Set
    End Property


    Public Property ReferenceNo() As String
        Get
            Return Me._strReferenceNo
        End Get
        Set(ByVal value As String)
            Me._strReferenceNo = value
        End Set
    End Property

    Public Property TransactionNo() As String
        Get
            Return Me._strTransNo
        End Get
        Set(ByVal value As String)
            Me._strTransNo = value
        End Set
    End Property

    'Public Property EHealthAccount() As EHSAccountModel.EHSPersonalInformationModel
    '    Get
    '        Return Me._udtEHSAccountPersonalInfo
    '    End Get
    '    Set(ByVal value As EHSAccountModel.EHSPersonalInformationModel)
    '        Me._udtEHSAccountPersonalInfo = value
    '    End Set
    'End Property

    'Public Property UpdateValue() As Boolean
    '    Get
    '        Return Me._updateValue
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me._updateValue = value
    '    End Set
    'End Property

    'Public Property ModeToBuild() As ucInputDocTypeBase.BuildMode
    '    Get
    '        Return Me._mode
    '    End Get
    '    Set(ByVal value As ucInputDocTypeBase.BuildMode)
    '        Me._mode = value
    '    End Set
    'End Property
#End Region

End Class