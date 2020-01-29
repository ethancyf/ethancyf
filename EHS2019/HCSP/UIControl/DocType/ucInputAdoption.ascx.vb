Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.StaticData

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
    Private _strOrgIsExactDOB As String

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL


    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.ADOPC)

        Me.lblEntryNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.rbDOBInWord.Text = Me.GetGlobalResourceObject("Text", "DOBInWordShort")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

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

        ''Tips
        'Me.lblENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

        'Me.lblEntryNoTip.Text = String.Empty

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
                Me.ddlDOBinWordType.BackColor = Drawing.Color.White
            End If

            dataRow = dataTable.NewRow
            dataRow(StaticDataModel.Column_Name) = 0
            dataRow(StaticDataModel.Item_No) = String.Empty
            dataRow(StaticDataModel.Data_Value) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
            dataRow(StaticDataModel.Data_Value_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
            dataRow(StaticDataModel.Data_Value_CN) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_CN")
            dataTable.Rows.InsertAt(dataRow, 0)

            Me.ddlDOBinWordType.DataSource = dataTable
            If MyBase.SessionHandler().Language() = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value_Chi
            ElseIf MyBase.SessionHandler().Language() = Common.Component.CultureLanguage.SimpChinese Then
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value_CN
            Else
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value
            End If
            Me.ddlDOBinWordType.DataValueField = StaticDataModel.Item_No
            Me.ddlDOBinWordType.DataBind()

            If Me.ActiveViewChanged Then
                Me.ddlDOBinWordType.SelectedValue = Me._strDOBInWord
            End If
        Else
            Me.ddlDOBinWordType.Enabled = False

            Me.ddlDOBinWordType.Items.Clear()
       
            Me.ddlDOBinWordType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "EHSClaimPleaseSelect"), String.Empty))

        End If

    End Sub

    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Me._strIdentityNo = MyBase.EHSPersonalInfo.IdentityNum
        Me._strPrefixNo = MyBase.EHSPersonalInfo.AdoptionPrefixNum
        Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
        Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
        Me._strGender = MyBase.EHSPersonalInfo.Gender
        Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
        Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB
        Me._strDOBInWord = MyBase.EHSPersonalInfo.OtherInfo
        Me._blnDOBTypeSelected = MyBase.EHSPersonalInfo.DOBTypeSelected

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        Me._strOrgIsExactDOB = MyBase.OrgEHSPersonalInfo.ExactDOB
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        Me.SetValue(mode)

        'Me.DOBInWordOption(Me.rbDOBInWord.Checked)

        If MyBase.ActiveViewChanged Then

            Me.SetEntryNoError(False)
            Me.SetENameError(False)
            Me.SetGenderError(False)
            Me.SetDOBInWordError(False)
            Me.SetDOBError(False)

        End If

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.lblEntryNo.Visible = True
            Me.txtPerfixNo.Visible = False
            Me.lblEntryNoSymbol.Visible = False
            Me.txtIdentityNo.Visible = False

            Me.txtPerfixNo.Enabled = False
            Me.txtIdentityNo.Enabled = False

            If MyBase.FixEnglishNameGender Then
                txtENameSurname.Enabled = False
                txtENameFirstname.Enabled = False
                rbGender.Enabled = False

            Else
                txtENameSurname.Enabled = True
                txtENameFirstname.Enabled = True
                rbGender.Enabled = True

            End If

        Else
            If mode = ucInputDocTypeBase.BuildMode.Modification Then
                'Modification Mode

                If MyBase.EditDocumentNo Then
                    Me.lblEntryNo.Visible = False
                    Me.txtPerfixNo.Visible = True
                    Me.lblEntryNoSymbol.Visible = True
                    Me.txtIdentityNo.Visible = True

                    Me.txtPerfixNo.Enabled = True
                    Me.txtIdentityNo.Enabled = True
                Else
                    Me.lblEntryNo.Visible = True
                    Me.txtPerfixNo.Visible = False
                    Me.lblEntryNoSymbol.Visible = False
                    Me.txtIdentityNo.Visible = False

                    Me.txtPerfixNo.Enabled = False
                    Me.txtIdentityNo.Enabled = False
                End If

                Me.rbDOB.Enabled = True
                Me.rbDOBInWord.Enabled = True
                Me.ddlDOBinWordType.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.rbGender.Enabled = True
            Else
                'Modification Read Only Mode
                Me.lblEntryNo.Visible = True
                Me.txtPerfixNo.Visible = False
                Me.lblEntryNoSymbol.Visible = False
                Me.txtIdentityNo.Visible = False

                Me.txtPerfixNo.Enabled = False
                Me.txtIdentityNo.Enabled = False

                Me.rbDOB.Enabled = False
                Me.rbDOBInWord.Enabled = False
                Me.ddlDOBinWordType.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.rbGender.Enabled = False
            End If
        End If
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        'Set DOB In word Drop Down list
        'Me.rbDOBInWord.Checked  is correctly set in setDOB()
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)
    End Sub

    Private Sub ChangeDOBOption(ByVal enable As Boolean)
        If Not enable Then
            Me.txtDOB.Enabled = True
            Me.txtDOBInWord.Enabled = False
            Me.ddlDOBinWordType.Enabled = False
            'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
            If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                Me.txtDOBInWord.Text = String.Empty
            End If
        Else
            Me.txtDOB.Enabled = False
            Me.txtDOBInWord.Enabled = True
            Me.ddlDOBinWordType.Enabled = True
            Me.ddlDOBinWordType.BackColor = Drawing.Color.White
            If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                Me.txtDOB.Text = String.Empty
            End If
        End If
    End Sub

#Region "Events"

    Protected Sub rbDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDOB.CheckedChanged, rbDOBInWord.CheckedChanged
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
            txtDOB.Text = String.Empty
            txtDOBInWord.Text = String.Empty
            ddlDOBinWordType.SelectedIndex = 0
        End If

        'Update DOB Options
        Me.ChangeDOBOption(Me.rbDOBInWord.Checked)
        'Update Drop Down List (DOB in word)
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)

        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me.txtDOB.Enabled = False
            Me.txtDOBInWord.Enabled = False
        End If
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

    ' CRE16-012 Removal of DOB InWord [Start][Winnie]
    Public Sub SetupDOB()
        'Fill Data - DOB
        Dim _blnShowDOBInWord As Boolean = False

        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.txtDOB.Text = Me._strDOB
        Else
            'Modification Mode
            ' Handle DOB InWord Option
            If Me._strOrgIsExactDOB = "T" Or Me._strOrgIsExactDOB = "U" Or Me._strOrgIsExactDOB = "V" Then
                _blnShowDOBInWord = True
            End If

            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then
                If Not Me.rbDOBInWord.Checked Then
                    Me.rbDOB.Checked = True
                    If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                        Me.txtDOB.Enabled = True
                    Else
                        Me.txtDOB.Enabled = False
                    End If
                    Me.txtDOB.Text = Me._strDOB

                    Me.rbDOBInWord.Checked = False
                    Me.txtDOBInWord.Enabled = False
                    Me.ddlDOBinWordType.Enabled = False
                End If

            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                If Not Me.rbDOB.Checked Then
                    Me.rbDOBInWord.Checked = True
                    If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                        Me.txtDOBInWord.Enabled = True
                    Else
                        Me.txtDOBInWord.Enabled = False
                    End If
                    Me.txtDOBInWord.Text = Me._strDOB

                    Me.rbDOB.Checked = False
                    Me.txtDOB.Enabled = False
                    Me.ddlDOBinWordType.Enabled = True
                    Me.ddlDOBinWordType.BackColor = Drawing.Color.White
                End If

            End If
        End If

        Me.ShowDOBInWordOption(_blnShowDOBInWord)
    End Sub

    Public Sub ShowDOBInWordOption(ByVal blnShow As Boolean)
        If blnShow Then
            Me.rbDOB.Visible = True
            Me.trDOBInWord.Visible = True
        Else
            ' Hide DOB In Word Option, choose DOB as default
            Me.rbDOB.Checked = True
            Me.rbDOB.Visible = False

            Me.trDOBInWord.Visible = False
            Me.rbDOBInWord.Checked = False
        End If
    End Sub
    ' CRE16-012 Removal of DOB InWord [End][Winnie]

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
        Me.SetEntryNoError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBInWordError(visible)
        Me.SetDOBError(visible)
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
        Else
            If Me.rbDOBInWord.Checked Then
                ' I-CRP16-002 Fix invalid input on English name [Start][Lawrence]
                Me._strDOB = Me.txtDOBInWord.Text.Trim
                ' I-CRP16-002 Fix invalid input on English name [End][Lawrence]

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
                Me._blnDOBInWordCase = False
            End If
        End If

        'If Me.rbDOB.Checked Then
        '    commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
        '    If Not strDOBtype.Trim.Equals(String.Empty) Then
        '        Me._strIsExactDOB = strDOBtype
        '    Else
        '        'in case of empty DOB
        '        Me._strIsExactDOB = "D"
        '    End If
        '    Me._blnDOBInWordCase = False
        'ElseIf Me.rbDOBInWord.Checked Then
        '    Me._strDOB = Me.txtDOBInWord.Text
        '    commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)
        '    If Not strDOBtype.Trim.Equals(String.Empty) Then
        '        Me._strIsExactDOB = strDOBtype
        '    Else
        '        'in case of empty DOB
        '        Me._strIsExactDOB = "T"
        '    End If
        '    Me._blnDOBInWordCase = True
        'Else
        '    Me._strIsExactDOB = String.Empty
        '    Me._blnDOBTypeSelected = False
        'End If
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