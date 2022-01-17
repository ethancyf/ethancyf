Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Partial Public Class ucInputRFNo8
    Inherits ucInputDocTypeBase

    Private _strDocumentNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)


        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.RFNo8)

        Me.lblDocumentNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language)

        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblCName.Text = Me.GetGlobalResourceObject("Text", "NameInChinese")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")

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

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        'div gender
        Me.lblIFemale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderFemale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIFemaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Female", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

        Me.lblIMale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderMale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIMaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Male", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))


    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Me._strDocumentNo = MyBase.EHSPersonalInfo.IdentityNum
        Me._strDOB = Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)

        Me.SetDocumentNo()
        Me.SetDOB()

        If MyBase.UpdateValue Then
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strCName = MyBase.EHSPersonalInfo.CName
            Me._strGender = MyBase.EHSPersonalInfo.Gender

            Me.SetEName()
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.SetGender()
            End If
        End If

        If modeType = ucInputDocTypeBase.BuildMode.Creation Then
            ' Creation mode
            Me.lblDocumentNo.Visible = True
            Me.txtDocumentNo.Visible = False
            Me.txtDocumentNo.Enabled = False

            If MyBase.FixEnglishNameGender Then
                txtENameSurname.Enabled = False
                txtENameFirstname.Enabled = False
                SetGenderReadOnlyStyle(True)
            Else
                txtENameSurname.Enabled = True
                txtENameFirstname.Enabled = True
                SetGenderReadOnlyStyle(False)
            End If

            Me.txtCName.Enabled = True
            Me.txtDOB.Enabled = False

        Else
            'Fill Data
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strCName = MyBase.EHSPersonalInfo.CName
            Me._strGender = MyBase.EHSPersonalInfo.Gender

            Me.SetEName()
            Me.SetGender()

            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                'Modification Mode
                If MyBase.EditDocumentNo Then
                    Me.lblDocumentNo.Visible = False
                    Me.txtDocumentNo.Visible = True
                    Me.txtDocumentNo.Enabled = True
                Else
                    Me.lblDocumentNo.Visible = True
                    Me.txtDocumentNo.Visible = False
                    Me.txtDocumentNo.Enabled = False
                End If

                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtCName.Enabled = True
                SetGenderReadOnlyStyle(False)
                Me.txtDOB.Enabled = True

            Else
                'Modification Read-Only Mode
                Me.lblDocumentNo.Visible = True
                Me.txtDocumentNo.Visible = False
                Me.txtDocumentNo.Enabled = False

                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.txtCName.Enabled = False
                SetGenderReadOnlyStyle(True)
                Me.txtDOB.Enabled = False

            End If

        End If

        Me.SetReferenceNo()
        Me.SetTransactionNo()

        If MyBase.ActiveViewChanged Then
            Me.SetENameError(False)
            Me.SetCNameError(False)
            Me.SetGenderError(False)
            Me.SetDOBError(False)
        End If

        divFemale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_0').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divFemale.Attributes.Add("onmouseover", "document.getElementById('" & divFemale.ClientID & "').style.left='-1px'; document.getElementById('" & divFemale.ClientID & "').style.top='-1px'; ")
        divFemale.Attributes.Add("onmouseout", "document.getElementById('" & divFemale.ClientID & "').style.left='0px'; document.getElementById('" & divFemale.ClientID & "').style.top='0px'; ")
        divMale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_1').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divMale.Attributes.Add("onmouseover", "document.getElementById('" & divMale.ClientID & "').style.left='-1px'; document.getElementById('" & divMale.ClientID & "').style.top='-1px'; ")
        divMale.Attributes.Add("onmouseout", "document.getElementById('" & divMale.ClientID & "').style.left='0px'; document.getElementById('" & divMale.ClientID & "').style.top='0px'; ")

    End Sub

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        Me.SetDocumentNo()
        Me.SetEName()
        Me.SetDOB()
        Me.SetGender()
        Me.SetReferenceNo()
        Me.SetTransactionNo()
    End Sub

    Public Sub SetDocumentNo()
        'Fill Data - Registration No only
        Me.lblDocumentNo.Text = Me._strDocumentNo
        Me.txtDocumentNo.Text = Me._strDocumentNo
    End Sub

    Public Sub SetEName()
        'Fill Data - English only
        Me.txtENameSurname.Text = Me._strENameSurName
        Me.txtENameFirstname.Text = Me._strENameFirstName
        Me.txtCName.Text = Me._strCName

    End Sub

    Public Sub SetDOB()
        'Fill Data - DOB

        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.txtDOB.Text = Me._strDOB

        Else
            'Modification Mode
            Me.txtDOB.Text = Me._strDOB

        End If

    End Sub

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
        Me.SetCNameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetDocumentNoError(visible)
    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        Me.imgENameError.Visible = visible
    End Sub

    Public Sub SetCNameError(ByVal visible As Boolean)
        Me.imgCNameError.Visible = visible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGenderError.Visible = visible
    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        Me.imgDOBError.Visible = visible
    End Sub

    Public Sub SetDocumentNoError(ByVal visible As Boolean)
        Me.imgDocumentNoError.Visible = visible
    End Sub
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
        Me._strDocumentNo = Me.txtDocumentNo.Text.Trim
        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strCName = Me.txtCName.Text.Trim
        Me._strGender = Me.rbGender.SelectedValue
        Me._strDOB = Me.txtDOB.Text.Trim
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

    Public Property CName() As String
        Get
            Return Me._strCName
        End Get
        Set(ByVal value As String)
            Me._strCName = value
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

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property DocumentNo() As String
        Get
            Return Me._strDocumentNo
        End Get
        Set(ByVal value As String)
            Me._strDocumentNo = value
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