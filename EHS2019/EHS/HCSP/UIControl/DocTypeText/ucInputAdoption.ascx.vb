Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.StaticData

Namespace UIControl.DocTypeText

    Partial Public Class ucInputAdoption
        Inherits ucInputDocTypeBase

        Private _strIdentityNo As String
        Private _strPrefixNo As String
        Private _strENameFirstName As String
        Private _strENameSurName As String
        Private _strGender As String
        Private _strDOB As String
        Private _strIsExactDOB As String
        Private _blnDOBTypeSelected As Boolean
        Private _strReferenceNo As String = String.Empty
        Private _strTransNo As String = String.Empty

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL


        Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

            'Table title
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.ADOPC)

            ' Entry No.
            Me.lblEntryNoText.Text = IIf(SessionHandler().Language = CultureLanguage.English, udtDocTypeModel.DocIdentityDesc, udtDocTypeModel.DocIdentityDescChi)
            Me.lblEntryNoTip.Text = String.Empty

            ' English Name
            Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
            Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

            ' Gender
            Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")

            'Gender Radio button list
            Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            ' DOB
            Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
            'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintADOPC")

        End Sub

        Protected Overrides Sub Setup(ByVal mode As BuildMode)

            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            Me._strIdentityNo = MyBase.EHSPersonalInfo.IdentityNum
            Me._strPrefixNo = MyBase.EHSPersonalInfo.AdoptionPrefixNum
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB
            Me._blnDOBTypeSelected = MyBase.EHSPersonalInfo.DOBTypeSelected

            Me.SetupEntryNo()
            Me.SetupDOB()

            If Me.FillValue(MyBase.EHSPersonalInfo) AndAlso MyBase.ActiveViewChanged Then
                Me._strGender = MyBase.EHSPersonalInfo.Gender
                Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName

                Me.SetupENameFirstName()
                Me.SetupENameSurName()
                Me.SetupGender()
                Me.SetErrorImage(BuildMode.Creation, False)
            End If

        End Sub

        Private Function FillValue(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            If String.IsNullOrEmpty(udtEHSPersonalInfo.ENameSurName) Then
                Return False
            End If

            If String.IsNullOrEmpty(udtEHSPersonalInfo.Gender) Then
                Return False
            End If

            If Not udtEHSPersonalInfo.DOBTypeSelected Then
                Return False
            End If

            Return True
        End Function

#Region "Events"

#End Region

#Region "Set Up Text Box Value"

        Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
            Me.SetupEntryNo()
            Me.SetupENameFirstName()
            Me.SetupENameSurName()
            Me.SetupGender()
            Me.SetupDOB()
        End Sub

        Public Sub SetupEntryNo()
            Me.txtIdentityNo.Text = Me._strIdentityNo
            Me.txtPerfixNo.Text = Me._strPrefixNo
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
            Me.txtDOB.Text = Me._strDOB
        End Sub

#End Region

#Region "Set Up Error Image"

        Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
            Me.SetEntryNoError(visible)
            Me.SetSurnameError(visible)
            Me.SetGivenNameError(visible)
            Me.SetGenderError(visible)
        End Sub

        Public Sub SetEntryNoError(ByVal blnVisible As Boolean)
            Me.lblEntryNoError.Visible = blnVisible
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

#End Region

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim dtDOB As DateTime
            Dim strDOBtype As String = String.Empty
            Me._strIdentityNo = Me.txtIdentityNo.Text.Trim.ToUpper
            Me._strPrefixNo = Me.txtPerfixNo.Text.Trim.ToUpper

            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim.ToUpper
            Me._strENameSurName = Me.txtENameSurname.Text.Trim.ToUpper
            Me._strGender = Me.rbGender.SelectedValue.Trim
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
