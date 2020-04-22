Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.DocType

Namespace UIControl.DocTypeText
    Partial Public Class ucInputReentryPermit
        Inherits ucInputDocTypeBase

        Private _strTravelDocNo As String
        Private _strENameFirstName As String
        Private _strENameSurName As String
        Private _strCName As String
        Private _strGender As String
        Private _strDOI As String
        Private _strDOB As String
        'Private _strIsExactDOB As String
        Private _strReferenceNo As String = String.Empty
        Private _strTransNo As String = String.Empty
        Private _showTransNo As Boolean

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Dim commfunct As Common.ComFunction.GeneralFunction


        Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)


            'Table title
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.REPMT)
            If MyBase.SessionHandler().Language() = Common.Component.CultureLanguage.English Then
                Me.lblTravelDocNoText.Text = udtDocTypeModel.DocIdentityDesc
            Else
                Me.lblTravelDocNoText.Text = udtDocTypeModel.DocIdentityDescChi
            End If
            'Me.lblTravelDocNo.Text = Me.GetGlobalResourceObject("Text", "TravelDocNo")
            Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
            Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))
            Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
            Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")
            'Me.lblTransactionNoText.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

            'Gender Radio button list
            Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            'Tips
            'Me.lblENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")
            'Me.lblDOITip.Text = Me.GetGlobalResourceObject("Text", "DOIHintREPMT")
            'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintREPMT")
            'Me.lblTDNoTip.Text = String.Empty
        End Sub

        Protected Overrides Sub Setup(ByVal mode As BuildMode)
            Me._strTravelDocNo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfo.IdentityNum, False)
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            'Me._strIsExactDOB = Me._udtEHSAccountPersonalInfo.ExactDOB

            If Me.FillValue(MyBase.EHSPersonalInfo) AndAlso MyBase.ActiveViewChanged Then
                Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
                Me._strGender = MyBase.EHSPersonalInfo.Gender
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strDOI = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfo.DateofIssue)
                Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfo.DateofIssue))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                Me.SetEName()
                Me.SetGender()
                Me.SetDOI()

                Me.SetErrorImage(BuildMode.Creation, False)
            End If

            'Creation Mode
            Me.SetREPMTNo(False)
            Me.SetDOB(True)
            Me.txtDOB.Enabled = False
            'Me.lblDOBTip.Visible = False

            'If MyBase.ActiveViewChanged Then

            '    Me.SetDOBError(False)
            '    Me.SetENameError(False)
            '    Me.SetGenderError(False)
            '    Me.SetDOIError(False)
            '    Me.SetREPMTNoError(False)
            'End If
        End Sub

        Private Function FillValue(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean

            If String.IsNullOrEmpty(udtEHSPersonalInfo.ENameSurName) Then
                Return False
            End If

            If String.IsNullOrEmpty(udtEHSPersonalInfo.ENameFirstName) OrElse String.IsNullOrEmpty(udtEHSPersonalInfo.ENameSurName) Then
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
        '--------------------------------------------------------------------------------------------------------------
        'Set Up Text Box Value
        '--------------------------------------------------------------------------------------------------------------
        Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
            'no neet implement
        End Sub

        Public Sub SetREPMTNo(ByVal enable As Boolean)
            Me.txtTravelDocNo.Enabled = enable
            Me.txtTravelDocNo.Text = Me._strTravelDocNo
            Me.lblTravelDocNo.Text = Me._strTravelDocNo
        End Sub

        Public Sub SetEName()
            'Fill Data - English only
            Me.txtENameSurname.Text = Me._strENameSurName
            Me.txtENameFirstname.Text = Me._strENameFirstName
        End Sub

        Public Sub SetDOB(ByVal enable As Boolean)
            Me.txtDOB.Enabled = enable
            Me.txtDOB.Text = Me._strDOB
        End Sub

        Public Sub SetDOI()
            Me.txtDOI.Text = Me._strDOI
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
            Me.SetDOBError(visible)
            Me.SetDOIError(visible)
            Me.SetREPMTNoError(visible)
        End Sub

        Public Sub SetENameError(ByVal visible As Boolean)
            Me.lblSurNameError.Visible = visible
            Me.lblGivenNameError.Visible = visible
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

        Public Sub SetREPMTNoError(ByVal visible As Boolean)
            Me.lblTravelDocNoError.Visible = visible
        End Sub
#End Region

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            Me._strTravelDocNo = Me.txtTravelDocNo.Text.Trim.ToUpper
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim.ToUpper
            Me._strENameSurName = Me.txtENameSurname.Text.Trim.ToUpper
            Me._strGender = Me.rbGender.SelectedValue

            Me._strDOB = Me.txtDOB.Text.Trim

            Me._strDOI = Me.txtDOI.Text.Trim
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

        Public Property Gender() As String
            Get
                Return Me._strGender
            End Get
            Set(ByVal value As String)
                Me._strGender = value
            End Set
        End Property

        Public Property REPMTNo() As String
            Get
                Return Me._strTravelDocNo
            End Get
            Set(ByVal value As String)
                Me._strTravelDocNo = value
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

        Public Property DateOfBirth() As String
            Get
                Return Me._strDOB
            End Get
            Set(ByVal value As String)
                Me._strDOB = value
            End Set
        End Property

        Public Property ShowTransNo() As Boolean
            Get
                Return Me._showTransNo
            End Get
            Set(ByVal value As Boolean)
                Me._showTransNo = value
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
End Namespace

