Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType

Public Class ucInputROP140
    Inherits ucInputDocTypeBase

    Private _strTDNumber As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOI As String
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.ROP140)

        Me.lblTDNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language)
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        'Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

        'error message
        Me.imgTDNo.ImageUrl = strErrorImageURL
        Me.imgTDNo.AlternateText = strErrorImageALT

        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT

        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT

        Me.imgDOBDate.ImageUrl = strErrorImageURL
        Me.imgDOBDate.AlternateText = strErrorImageALT

        'Me.imgDOIDate.ImageUrl = strErrorImageURL
        'Me.imgDOIDate.AlternateText = strErrorImageALT

    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, MyBase.EHSPersonalInfo.IdentityNum, False)
        Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
        Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
        Me._strGender = MyBase.EHSPersonalInfo.Gender
        Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)

        If MyBase.EHSPersonalInfo.DateofIssue.HasValue Then
            Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfo.DateofIssue))
        Else
            Me._strDOI = String.Empty
        End If

        Me.SetValue(modeType)


        'Mode related Settings
        If modeType = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.lblTDNo.Visible = True
            Me.txtTDNo.Visible = False
            Me.txtTDNo.Enabled = False

            Me.txtDOB.Enabled = False

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
            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                'Modification Mode

                Me.lblTDNo.Visible = False
                Me.txtTDNo.Visible = False

                If MyBase.EditDocumentNo Then
                    Me.txtTDNo.Visible = True
                Else
                    Me.lblTDNo.Visible = True
                End If

                Me.txtDOB.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.rbGender.Enabled = True
                'Me.txtDOI.Enabled = True
            Else
                'Modification Read-Only Mode
                Me.lblTDNo.Visible = True
                Me.txtTDNo.Visible = False
                Me.txtTDNo.Enabled = False

                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOB.Enabled = False
                'Me.txtDOI.Enabled = False
            End If

        End If

        If MyBase.ActiveViewChanged Then
            Me.SetTDError(False)
            Me.SetENameError(False)
            Me.SetGenderError(False)
            Me.SetDOBError(False)
            'Me.SetDOIError(False)
        End If

    End Sub



#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        SetupTDNumber()
        SetupENameFirstName()
        SetupENameSurName()
        SetupGender()
        SetupDOB()
        'SetupDOI()
        SetReferenceNo()
        SetTransactionNo()
    End Sub

    Public Sub SetupTDNumber()
        Me.txtTDNo.Text = Me._strTDNumber
        Me.lblTDNo.Text = Me._strTDNumber
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
        Me.txtDOB.Text = Me._strDOB
    End Sub

    'Public Sub SetupDOI()
    '    Me.txtDOI.Text = Me._strDOI
    'End Sub

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
        Me.SetTDError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        'Me.SetDOIError(visible)
    End Sub

    Public Sub SetTDError(ByVal blnVisible As Boolean)
        Me.imgTDNo.Visible = blnVisible
    End Sub

    Public Sub SetENameError(ByVal blnVisible As Boolean)
        Me.imgEName.Visible = blnVisible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGender.Visible = visible
    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        Me.imgDOBDate.Visible = visible
    End Sub

    'Public Sub SetDOIError(ByVal visible As Boolean)
    '    Me.imgDOIDate.Visible = visible
    'End Sub

#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Me._strTDNumber = Me.txtTDNo.Text.Trim
        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strGender = Me.rbGender.SelectedValue.Trim
        'Me._strDOI = Me.txtDOI.Text.Trim
        Me._strDOB = Me.txtDOB.Text.Trim
    End Sub

    Public Property TravelDocNo() As String
        Get
            Return Me._strTDNumber
        End Get
        Set(ByVal value As String)
            Me._strTDNumber = value
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

    'Public Property IsExactDOB() As String
    '    Get
    '        Return Me._strIsExactDOB
    '    End Get
    '    Set(ByVal value As String)
    '        Me._strIsExactDOB = value
    '    End Set
    'End Property

    Public Property DateOfIssue() As String
        Get
            Return Me._strDOI
        End Get
        Set(ByVal value As String)
            Me._strDOI = value
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

End Class