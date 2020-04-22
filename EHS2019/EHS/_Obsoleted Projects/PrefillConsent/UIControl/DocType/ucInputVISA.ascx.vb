Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType

Partial Public Class ucInputVISA
    Inherits ucInputDocTypeBase

    Private _strVISANo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strPassportNo As String
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.VISA)

        If MyBase.SessionHandler().Language() = Common.Component.CultureLanguage.English Then
            Me.lblVISANoText.Text = udtDocTypeModel.DocIdentityDesc
        Else
            Me.lblVISANoText.Text = udtDocTypeModel.DocIdentityDescChi
        End If
        'Me.lblVISANo.Text = Me.GetGlobalResourceObject("Text", "VisaRefNo")
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblPassportNoText.Text = Me.GetGlobalResourceObject("Text", "PassportNo")
        'Me.lblPassportNoText.Text = Me.GetGlobalResourceObject("Text", "TravelDocNo")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgVISANo.ImageUrl = strErrorImageURL
        Me.imgVISANo.AlternateText = strErrorImageALT
        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT
        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT
        Me.imgDOBDate.ImageUrl = strErrorImageURL
        Me.imgDOBDate.AlternateText = strErrorImageALT
        Me.imgPassportNo.ImageUrl = strErrorImageURL
        Me.imgPassportNo.AlternateText = strErrorImageALT

        'Tips
        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))
        'Me.lblENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")
        'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintVISA")
        ''Me.lblVISANoTip.Text = Me.GetGlobalResourceObject("Text", "VisaNoHint")
        'Me.lblVISANoTip.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
    End Sub


    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        If Not MyBase.EHSPersonalInfo Is Nothing Then
            Me._strVISANo = MyBase.EHSPersonalInfo.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty)
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me._strPassportNo = MyBase.EHSPersonalInfo.Foreign_Passport_No

            Me.SetValue(mode)
        End If

        'Mode related Settings
        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            Me.lblENameTips.Visible = True
            Me.lblDOBTip.Visible = True
            'Me.txtDOB.Enabled = False

            'Tips
            'Me.lblDOBTip.Visible = False
        Else
            'Modification and Read-Only Modes
            Me.lblVISANo.Visible = True
            Me.txtVISANo1.Visible = False
            Me.lblVISANoSymbol1.Visible = False
            Me.txtVISANo2.Visible = False
            Me.lblVISANoSymbol2.Visible = False
            Me.txtVISANo3.Visible = False
            Me.lblVISANoSymbol3.Visible = False
            Me.txtVISANo4.Visible = False
            Me.lblVISANoSymbol4.Visible = False

            Me.lblENameTips.Visible = True
            Me.lblDOBTip.Visible = True

            If mode = ucInputDocTypeBase.BuildMode.Modification Then
                Me.txtDOB.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOB.Enabled = True
                Me.txtVISANo1.Enabled = True
                Me.txtVISANo2.Enabled = True
                Me.txtVISANo3.Enabled = True
                Me.txtVISANo4.Enabled = True
                Me.txtPassportNo.Enabled = True
            Else
                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOB.Enabled = False
                Me.txtVISANo1.Enabled = False
                Me.txtVISANo2.Enabled = False
                Me.txtVISANo3.Enabled = False
                Me.txtVISANo4.Enabled = False
                Me.txtPassportNo.Enabled = False
            End If
        End If

        Me.SetReferenceNo()
        Me.SetTransactionNo()

        Me.SetVISANoError(False)
        Me.SetPassportNoError(False)
        Me.SetENameError(False)
        Me.SetGenderError(False)
        Me.SetDOBError(False)

    End Sub

#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        SetupVISANo()
        SetupENameFirstName()
        SetupENameSurName()
        SetupGender()
        SetupPassportNo()
        SetupDOB()
        SetReferenceNo()
        SetTransactionNo()
    End Sub

    Public Sub SetupVISANo()
        If _strVISANo.Trim.Length >= 4 Then
            Me.txtVISANo1.Text = Me._strVISANo.Trim.Substring(0, 4)
            Me.lblVISANo.Text = Me._strVISANo.Trim.Substring(0, 4)
            If _strVISANo.Trim.Length >= 11 Then
                Me.txtVISANo2.Text = Me._strVISANo.Trim.Substring(4, 7)
                Me.lblVISANo.Text = Me.lblVISANo.Text + "-" + Me._strVISANo.Trim.Substring(4, 7)
                If _strVISANo.Trim.Length >= 13 Then
                    Me.txtVISANo3.Text = Me._strVISANo.Trim.Substring(11, 2)
                    Me.lblVISANo.Text = Me.lblVISANo.Text + "-" + Me._strVISANo.Trim.Substring(11, 2)
                    If _strVISANo.Trim.Length >= 14 Then
                        Me.txtVISANo4.Text = Me._strVISANo.Trim.Substring(13, 1)
                        Me.lblVISANo.Text = Me.lblVISANo.Text + "(" + Me._strVISANo.Trim.Substring(13, 1) + ")"
                    Else
                        Me.txtVISANo4.Text = Me._strVISANo.Trim.Substring(13)
                        Me.lblVISANo.Text = Me.lblVISANo.Text + "(" + Me._strVISANo.Trim.Substring(13) + ")"
                    End If
                Else
                    Me.txtVISANo3.Text = Me._strVISANo.Trim.Substring(10)

                End If
            Else
                Me.txtVISANo2.Text = Me._strVISANo.Trim.Substring(4)

            End If
        Else
            Me.txtVISANo1.Text = Me._strVISANo.Trim.Substring(0)
            Me.lblVISANo.Text = Me._strVISANo.Trim.Substring(0)
        End If


    End Sub

    Public Sub SetupENameFirstName()
        Me.txtENameFirstname.Text = Me._strENameFirstName
    End Sub

    Public Sub SetupENameSurName()
        Me.txtENameSurname.Text = Me._strENameSurName
    End Sub

    Public Sub SetupPassportNo()
        Me.txtPassportNo.Text = Me._strPassportNo
    End Sub

    Public Sub SetupGender()
        If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
            Me.rbGender.SelectedValue = Me._strGender
        End If
    End Sub

    Public Sub SetupDOB()
        Me.txtDOB.Text = Me._strDOB
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
        Me.SetVISANoError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetPassportNoError(visible)
    End Sub

    Public Sub SetVISANoError(ByVal blnVisible As Boolean)
        Me.imgVISANo.Visible = blnVisible
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

    Public Sub SetPassportNoError(ByVal visible As Boolean)
        Me.imgPassportNo.Visible = visible
    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)

        Me._strVISANo = Me.txtVISANo1.Text.Trim + Me.txtVISANo2.Text.Trim + Me.txtVISANo3.Text.Trim + Me.txtVISANo4.Text.Trim
        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strGender = Me.rbGender.SelectedValue.Trim
        Me._strDOB = Me.txtDOB.Text.Trim
        Me._strPassportNo = Me.txtPassportNo.Text.Trim
    End Sub

    Public Property VISANo() As String
        Get
            Return Me._strVISANo
        End Get
        Set(ByVal value As String)
            Me._strVISANo = value
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

    Public Property PassportNo() As String
        Get
            Return Me._strPassportNo
        End Get
        Set(ByVal value As String)
            Me._strPassportNo = value
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