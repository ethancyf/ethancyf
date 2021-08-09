Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Partial Public Class ucInputHKBC
    Inherits ucInputDocTypeBase

    Private _strRegistrationNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOBInWord As String
    Private _strIsExactDOB As String
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty
    Private _blnDOBTypeSelected As Boolean
    Private _blnDOBInWordCase As Boolean
    Private _strOrgIsExactDOB As String

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)


        'Table title
        'Me.lblRegistrationNoText.Text = Me.GetGlobalResourceObject("Text", "BCRegNo")
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKBC)
      
        Me.lblRegistrationNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language)

        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.rbDOBInWord.Text = Me.GetGlobalResourceObject("Text", "DOBInWordShort")

        Me.lblReferenceNoText_M.Text = Me.GetGlobalResourceObject("Text", "RefNo")
        Me.lblTransactionNoText_M.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

        'Error Image
        Me.imgENameError.ImageUrl = strErrorImageURL
        Me.imgENameError.AlternateText = strErrorImageALT

        Me.imgGenderError.ImageUrl = strErrorImageURL
        Me.imgGenderError.AlternateText = strErrorImageALT

        Me.imgDOBInWordError.ImageUrl = strErrorImageURL
        Me.imgDOBInWordError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        'div gender
        Me.lblIFemale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderFemale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIFemaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Female", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

        Me.lblIMale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderMale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIMaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Male", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))


    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Me._strRegistrationNo = Formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum, False)
        Me._strDOB = Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
        Me._blnDOBTypeSelected = MyBase.EHSPersonalInfo.DOBTypeSelected
        Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB
        Me._strDOBInWord = MyBase.EHSPersonalInfo.OtherInfo

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        Me._strOrgIsExactDOB = MyBase.OrgEHSPersonalInfo.ExactDOB
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        Me.SetRegistrationNo()
        Me.SetDOB()

        If MyBase.UpdateValue Then
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender

            Me.SetEName()
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.SetGender()
            End If
        End If

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If Not modeType = ucInputDocTypeBase.BuildMode.Creation Then
            'Handle Modification Mode and Modification Read-Only Mode

            'Fill Data
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender

            Me.SetEName()
            Me.SetGender()

            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                'Me.lblRegistrationNo.Text = AntiXssEncoder.HtmlEncode(txtRegistrationNo.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
                Me.lblRegistrationNo.Visible = False
                Me.txtRegistrationNo.Visible = False

                If MyBase.EditDocumentNo Then
                    Me.txtRegistrationNo.Visible = True
                Else
                    Me.lblRegistrationNo.Visible = True
                End If

                Me.rbDOB.Enabled = True
                Me.rbDOBInWord.Enabled = True
                Me.ddlDOBinWordType.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                SetGenderReadOnlyStyle(False)

            Else
                Me.lblRegistrationNo.Visible = True
                Me.txtRegistrationNo.Visible = False
                Me.txtRegistrationNo.Enabled = False

                Me.rbDOB.Enabled = False
                Me.rbDOBInWord.Enabled = False
                Me.ddlDOBinWordType.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
                SetGenderReadOnlyStyle(True)

            End If

        Else
            'Handle Creation mode
            Me.lblRegistrationNo.Visible = True
            Me.txtRegistrationNo.Visible = False
            Me.txtRegistrationNo.Enabled = False

            If MyBase.FixEnglishNameGender Then
                txtENameSurname.Enabled = False
                txtENameFirstname.Enabled = False
                SetGenderReadOnlyStyle(True)

            Else
                txtENameSurname.Enabled = True
                txtENameFirstname.Enabled = True
                SetGenderReadOnlyStyle(False)

            End If

        End If
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        Me.SetReferenceNo()
        Me.SetTransactionNo()

        'Set DOB In word Drop Down list
        'Me.rbDOBInWord.Checked  is correctly set in setDOB()
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)

        If MyBase.ActiveViewChanged Then

            Me.SetENameError(False)
            Me.SetGenderError(False)
            Me.SetDOBTypeError(False)
            Me.SetDOBError(False)
        End If

        divFemale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_0').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divFemale.Attributes.Add("onmouseover", "document.getElementById('" & divFemale.ClientID & "').style.left='-1px'; document.getElementById('" & divFemale.ClientID & "').style.top='-1px'; ")
        divFemale.Attributes.Add("onmouseout", "document.getElementById('" & divFemale.ClientID & "').style.left='0px'; document.getElementById('" & divFemale.ClientID & "').style.top='0px'; ")
        divMale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_1').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divMale.Attributes.Add("onmouseover", "document.getElementById('" & divMale.ClientID & "').style.left='-1px'; document.getElementById('" & divMale.ClientID & "').style.top='-1px'; ")
        divMale.Attributes.Add("onmouseout", "document.getElementById('" & divMale.ClientID & "').style.left='0px'; document.getElementById('" & divMale.ClientID & "').style.top='0px'; ")

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
            If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value_Chi
            ElseIf MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.SimpChinese Then
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
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        Me.SetRegistrationNo()
        Me.SetEName()
        Me.SetDOB()
        Me.SetGender()
        Me.SetReferenceNo()
        Me.SetTransactionNo()
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetRegistrationNo()
        'Fill Data - Registration No only
        Me.lblRegistrationNo.Text = Me._strRegistrationNo
        Me.txtRegistrationNo.Text = Me._strRegistrationNo
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]


    Public Sub SetEName()
        'Fill Data - English only
        Me.txtENameSurname.Text = Me._strENameSurName
        Me.txtENameFirstname.Text = Me._strENameFirstName
    End Sub

    ' CRE16-012 Removal of DOB InWord [Start][Winnie]
    Public Sub SetDOB()
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

    Public Sub SetGender()
        'Fill Data - Gender only
        Dim strGender As String
        Me.rbGender.SelectedValue = Me._strGender

        If Me._strGender = "M" Then
            strGender = "GenderMale"
        Else
            strGender = "GenderFemale"
        End If
        Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
        HandleDivGenderStyle(Me._strGender)
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

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBTypeError(visible)
        Me.SetDOBError(visible)
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.SetRegistrationNoError(visible)
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        Me.imgENameError.Visible = visible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGenderError.Visible = visible
    End Sub

    Public Sub SetDOBTypeError(ByVal visible As Boolean)
        Me.imgDOBInWordError.Visible = visible
    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        Me.imgDOBError.Visible = visible
    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetRegistrationNoError(ByVal visible As Boolean)
        Me.imgRegistrationNoError.Visible = visible
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region


#Region "Events"

    Protected Sub rbGender_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbGender.SelectedIndexChanged
        Dim rbGender As RadioButtonList = CType(sender, RadioButtonList)
        HandleDivGenderStyle(rbGender.SelectedValue)
    End Sub

    Private Sub HandleDivGenderStyle(ByVal strGender As String)
        Select Case strGender
            Case "M"
                divFemale.Style.Add("outline-color", "black")
                divFemale.Style.Add("outline-width", "2px")

                divMale.Style.Add("outline-color", "#3198FF")
                divMale.Style.Add("outline-width", "8px")

            Case "F"
                divFemale.Style.Add("outline-color", "#3198FF")
                divFemale.Style.Add("outline-width", "8px")

                divMale.Style.Add("outline-color", "black")
                divMale.Style.Add("outline-width", "2px")
        End Select
    End Sub

    Private Sub SetGenderReadOnlyStyle(ByVal ReadOnlyMode As Boolean)
        If ReadOnlyMode = True Then
            Me.divGender.Visible = False
            Me.lblReadonlyGender.Visible = True
            Me.trGenderImageInput.Style.Remove("height")
            Me.rbGender.Enabled = False
        Else
            Me.divGender.Visible = True
            Me.lblReadonlyGender.Visible = False
            Me.rbGender.Enabled = True
        End If
    End Sub

#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        Me._strRegistrationNo = Me.txtRegistrationNo.Text.Trim
        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strGender = Me.rbGender.SelectedValue
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

    End Sub

    Public Property ENameSurName() As String
        Get
            Return Me._strENameSurName
        End Get
        Set(ByVal value As String)
            Me._strENameSurName = value
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

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property RegistrationNo() As String
        Get
            Return Me._strRegistrationNo
        End Get
        Set(ByVal value As String)
            Me._strRegistrationNo = value
        End Set
    End Property

    ' CRE16-012 Removal of DOB InWord [Start][Winnie]
    Public Property OrgIsExactDOB() As String
        Get
            Return Me._strOrgIsExactDOB
        End Get
        Set(ByVal value As String)
            Me._strOrgIsExactDOB = value
        End Set
    End Property
    ' CRE16-012 Removal of DOB InWord [End][Winnie]

    Public Property DOBInWordCase() As Boolean
        Get
            Return Me._blnDOBInWordCase
        End Get
        Set(ByVal value As Boolean)
            Me._blnDOBInWordCase = value
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
#End Region

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class