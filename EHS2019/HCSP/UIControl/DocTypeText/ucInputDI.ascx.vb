Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType

Namespace UIControl.DocTypeText

    Partial Public Class ucInputDI
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
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.DI)

            ' Travel Document No
            Me.lblTDNoText.Text = IIf(SessionHandler.Language() = CultureLanguage.English, udtDocTypeModel.DocIdentityDesc, udtDocTypeModel.DocIdentityDescChi)
            'Me.lblTDNoTip.Text = String.Empty

            ' English Name
            Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
            Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

            ' Gender
            Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            ' DOB
            Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
            'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI")

            ' DOI
            Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")
            'Me.lblDOITip.Text = Me.GetGlobalResourceObject("Text", "DOIHintDI")

        End Sub

        Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)
            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

            Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.DI, MyBase.EHSPersonalInfo.IdentityNum, False)
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            'Me._strDOI = IIf(EHSPersonalInfo.DateofIssue.HasValue, Formatter.formatEnterDate(MyBase.EHSPersonalInfo.DateofIssue), String.Empty)
            SetupTDNumber()
            SetupDOB()
            SetReferenceNo()

            If Me.FillValue(MyBase.EHSPersonalInfo) AndAlso MyBase.ActiveViewChanged Then
                Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
                Me._strGender = MyBase.EHSPersonalInfo.Gender
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strDOI = Formatter.formatEnterDate(MyBase.EHSPersonalInfo.DateofIssue)
                Me._strDOI = Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfo.DateofIssue))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                SetupENameFirstName()
                SetupENameSurName()
                SetupGender()
                SetupDOI()

                Me.SetErrorImage(BuildMode.Creation, False)
            End If

            'Me.SetValue(modeType)

            Me.txtDOB.Enabled = False

            'If MyBase.ActiveViewChanged Then

            '    Me.SetTDError(False)
            '    Me.SetSurnameError(False)
            '    Me.SetGivenNameError(False)
            '    Me.SetGenderError(False)
            '    Me.SetDOBError(False)
            '    Me.SetDOIError(False)

            'End If
        End Sub

        Private Function FillValue(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            If String.IsNullOrEmpty(udtEHSPersonalInfo.ENameSurName) Then
                Return False
            End If

            If String.IsNullOrEmpty(udtEHSPersonalInfo.Gender) Then
                Return False
            End If

            If Not udtEHSPersonalInfo.DateofIssue.HasValue Then
                Return False
            End If

            Return True
        End Function

#Region "Set Up Text Box Value"

        Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
            SetupTDNumber()
            SetupDOB()
            SetReferenceNo()

            SetupENameFirstName()
            SetupENameSurName()
            SetupGender()
            SetupDOI()

        End Sub

        Public Sub SetupTDNumber()
            Me.txtTDNo.Text = Me._strTDNumber

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

        Public Sub SetupDOI()
            Me.txtDOI.Text = Me._strDOI
        End Sub

        'unqiue for modification and modification read-only modes
        Public Sub SetReferenceNo()
            If Me._strReferenceNo.Trim.Equals(String.Empty) Then
                Me.trReferenceNoText.Visible = False
                Me.trReferenceNo.Visible = False
            Else
                Me.trReferenceNoText.Visible = True
                Me.trReferenceNo.Visible = True
                Me.lblReferenceNo.Text = Me._strReferenceNo
            End If
        End Sub

#End Region

#Region "Set Up Error Image"

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Error Image
        '--------------------------------------------------------------------------------------------------------------
        Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
            Me.SetTDError(visible)
            Me.SetSurnameError(visible)
            Me.SetGivenNameError(visible)
            Me.SetGenderError(visible)
            Me.SetDOBError(visible)
            Me.SetDOIError(visible)
        End Sub

        Public Sub SetTDError(ByVal blnVisible As Boolean)
            Me.lblTDNoError.Visible = blnVisible
        End Sub

        Public Sub SetSurnameError(ByVal blnVisible As Boolean)
            Me.lblSurNameError.Visible = blnVisible
        End Sub

        Public Sub SetGivenNameError(ByVal blnVisible As Boolean)
            Me.lblGivenNameError.Visible = blnVisible
        End Sub

        Public Sub SetGenderError(ByVal visible As Boolean)
            Me.lblGenderError.Visible = visible
        End Sub

        Public Sub SetDOBError(ByVal visible As Boolean)
            Me.lblDOBError.Visible = visible
        End Sub

        Public Sub SetDOIError(ByVal visible As Boolean)
            Me.lblDOIError.Visible = visible
        End Sub

#End Region

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            Me._strTDNumber = Me.txtTDNo.Text.Trim.ToUpper
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim.ToUpper
            Me._strENameSurName = Me.txtENameSurname.Text.Trim.ToUpper
            Me._strGender = Me.rbGender.SelectedValue.Trim
            Me._strDOI = Me.txtDOI.Text.Trim
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

#End Region
    End Class
End Namespace