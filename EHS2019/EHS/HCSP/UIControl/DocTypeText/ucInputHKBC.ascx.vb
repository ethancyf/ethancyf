Imports System.Web.Security.AntiXss
Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component

Namespace UIControl.DocTypeText
    Partial Public Class ucInputHKBC
        Inherits ucInputDocTypeBase

        Private _strRegistrationNo As String
        Private _strENameFirstName As String
        Private _strENameSurName As String
        Private _strCName As String
        Private _strGender As String
        Private _strDOB As String
        Private _strIsExactDOB As String
        Private _strReferenceNo As String = String.Empty
        Private _strTransNo As String = String.Empty
        Private _blnDOBTypeSelected As Boolean

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
            'Table title
            'Me.lblRegistrationNoText.Text = Me.GetGlobalResourceObject("Text", "BCRegNo")
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKBC)
            Me.lblRegistrationNoText.Text = udtDocTypeModel.DocIdentityDesc(SessionHandler().Language)

            Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
            Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))
            Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")

            'Me.lblTransactionNoText_M.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

            'Gender Radio button list
            Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            'Tips
            'Me.txtENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

        End Sub

        Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)
            
            Me._strRegistrationNo = Formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum, False)
            Me._strDOB = Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me._blnDOBTypeSelected = MyBase.EHSPersonalInfo.DOBTypeSelected
            Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB

            Me.SetRegistrationNo()
            Me.SetDOB()

            If Not modeType = ucInputDocTypeBase.BuildMode.Creation Then
                'Fill Data

                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.lblRegistrationNo.Text = AntiXssEncoder.HtmlEncode(txtRegistrationNo.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
                Me.lblRegistrationNo.Visible = True
                Me.txtRegistrationNo.Visible = False

                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.rbGender.Enabled = True

                Me.SetErrorImage(BuildMode.Creation, False)
            End If


            If FillValue(MyBase.EHSPersonalInfo) AndAlso MyBase.ActiveViewChanged Then

                Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
                Me._strGender = MyBase.EHSPersonalInfo.Gender

                Me.SetEName()
                Me.SetGender()
            End If

        End Sub

        Private Function FillValue(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean

            If String.IsNullOrEmpty(udtEHSPersonalInfo.ENameSurName) Then
                Return False
            End If

            If String.IsNullOrEmpty(udtEHSPersonalInfo.Gender) Then
                Return False
            End If

            Return True
        End Function

#Region "Events"

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
            'Me.SetTransactionNo()
        End Sub

        Public Sub SetRegistrationNo()
            'Fill Data - Registration No only
            Me.txtRegistrationNo.Text = Me._strRegistrationNo
        End Sub

        Public Sub SetEName()
            'Fill Data - English only
            Me.txtENameSurname.Text = Me._strENameSurName
            Me.txtENameFirstname.Text = Me._strENameFirstName
        End Sub

        Public Sub SetDOB()
            'Fill Data - DOB
            Me.txtDOB.Text = Me._strDOB
        End Sub

        Public Sub SetGender()
            'Fill Data - Gender only
            Me.rbGender.SelectedValue = Me._strGender
        End Sub

#End Region

#Region "Set Up Error Image"

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Error Image
        '--------------------------------------------------------------------------------------------------------------
        Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
            Me.SetENameError(visible)
            Me.SetGenderError(visible)
        End Sub

        Public Sub SetENameError(ByVal visible As Boolean)
            Me.lblGivenNameError.Visible = visible
            Me.lblSurNameError.Visible = visible
        End Sub

        Public Sub SetGenderError(ByVal visible As Boolean)
            Me.lblGenderError.Visible = visible
        End Sub

#End Region

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim dtDOB As DateTime
            Dim strDOBtype As String = String.Empty

            Me._strRegistrationNo = Me.txtRegistrationNo.Text.Trim.ToUpper
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim.ToUpper
            Me._strENameSurName = Me.txtENameSurname.Text.Trim.ToUpper
            Me._strGender = Me.rbGender.SelectedValue
            Me._strDOB = Me.txtDOB.Text.Trim

            ' CRE16-012 Removal of DOB InWord [Start][Winnie]
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
            If Not strDOBtype.Trim.Equals(String.Empty) Then
                Me._strIsExactDOB = strDOBtype
            Else
                'in case of empty DOB
                Me._strIsExactDOB = "D"
            End If
            ' CRE16-012 Removal of DOB InWord [End][Winnie]
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
End Namespace

