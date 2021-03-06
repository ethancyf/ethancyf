Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.DocType

Namespace UIControl.DocTypeHCSP

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
            Me.lblTravelDocNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler().Language)

            'Me.lblTravelDocNo.Text = Me.GetGlobalResourceObject("Text", "TravelDocNo")
            Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
            Me.lblCName.Text = Me.GetGlobalResourceObject("Text", "NameInChinese")
            Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
            Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")
            Me.lblReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblTransactionNoText.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

            'Gender Radio button list
            Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
            Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

            'Error Image
            Me.imgENameError.ImageUrl = strErrorImageURL
            Me.imgENameError.AlternateText = strErrorImageALT

            Me.imgCNameError.ImageUrl = strErrorImageURL
            Me.imgCNameError.AlternateText = strErrorImageALT

            Me.imgGenderError.ImageUrl = strErrorImageURL
            Me.imgGenderError.AlternateText = strErrorImageALT

            Me.imgTravelDocNoError.ImageUrl = strErrorImageURL
            Me.imgTravelDocNoError.AlternateText = strErrorImageALT

            Me.imgDOBError.ImageUrl = strErrorImageURL
            Me.imgDOBError.AlternateText = strErrorImageALT

            Me.imgDOIError.ImageUrl = strErrorImageURL
            Me.imgDOIError.AlternateText = strErrorImageALT

            'Tips
            'Me.lblENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")
            'Me.lblDOITip.Text = Me.GetGlobalResourceObject("Text", "DOIHintREPMT")
            'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintREPMT")
            'Me.lblTDNoTip.Text = String.Empty
        End Sub

        Protected Overrides Sub Setup(ByVal mode As BuildMode)
            Me._strTravelDocNo = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, MyBase.EHSPersonalInfo.IdentityNum, False)

            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strCName = MyBase.EHSPersonalInfo.CName
            Me._strGender = MyBase.EHSPersonalInfo.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            'Me._strIsExactDOB = Me._udtEHSAccountPersonalInfo.ExactDOB

            If MyBase.EHSPersonalInfo.DateofIssue.HasValue Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me._strDOI = MyBase.Formatter.formatEnterDate(MyBase.EHSPersonalInfo.DateofIssue)
                Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfo.DateofIssue))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Else
                Me._strDOI = String.Empty
            End If

            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            'Mode related Settings
            If mode = ucInputDocTypeBase.BuildMode.Creation Then
                'Creation Mode
                Me.SetREPMTNo()
                Me.SetEName()
                Me.SetCName()
                Me.SetDOB(True)
                Me.SetGender()
                Me.SetDOI()
                Me.SetReferenceNo()
                Me.SetTransactionNo()

                Me.lblTravelDocNo.Visible = True
                Me.txtTravelDocNo.Visible = False
                Me.txtTravelDocNo.Enabled = False

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

                Me.txtCName.Enabled = True

            Else
                Me.SetREPMTNo()
                Me.SetEName()
                Me.SetCName()
                Me.SetDOB(False)
                Me.SetGender()
                Me.SetDOI()
                Me.SetReferenceNo()
                Me.SetTransactionNo()

                If mode = ucInputDocTypeBase.BuildMode.Modification Then
                    'Modification Modes

                    If MyBase.EditDocumentNo Then
                        Me.lblTravelDocNo.Visible = False
                        Me.txtTravelDocNo.Visible = True
                        Me.txtTravelDocNo.Enabled = True
                    Else
                        Me.lblTravelDocNo.Visible = True
                        Me.txtTravelDocNo.Visible = False
                        Me.txtTravelDocNo.Enabled = False
                    End If

                    Me.txtDOB.Enabled = True
                    Me.txtDOB.Enabled = True
                    Me.rbGender.Enabled = True
                    Me.txtDOI.Enabled = True
                    Me.txtENameFirstname.Enabled = True
                    Me.txtENameSurname.Enabled = True
                    Me.txtCName.Enabled = True
                Else
                    'Modification Read-Only Mode
                    If MyBase.EditDocumentNo Then
                        Me.lblTravelDocNo.Visible = False
                        Me.txtTravelDocNo.Visible = True
                        Me.txtTravelDocNo.Enabled = False
                    Else
                        Me.lblTravelDocNo.Visible = True
                        Me.txtTravelDocNo.Visible = False
                        Me.txtTravelDocNo.Enabled = False
                    End If

                    Me.txtDOB.Enabled = False
                    Me.rbGender.Enabled = False
                    Me.txtDOI.Enabled = False
                    Me.txtENameFirstname.Enabled = False
                    Me.txtENameSurname.Enabled = False
                    Me.txtCName.Enabled = False
                End If

            End If
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            If MyBase.ActiveViewChanged Then
                Me.SetDOBError(False)
                Me.SetENameError(False)
                Me.SetCNameError(False)
                Me.SetGenderError(False)
                Me.SetDOIError(False)
                Me.SetREPMTNoError(False)
            End If

        End Sub

#Region "Set Up Text Box Value"
        '--------------------------------------------------------------------------------------------------------------
        'Set Up Text Box Value
        '--------------------------------------------------------------------------------------------------------------
        Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
            Me.SetREPMTNo()
            Me.SetEName()
            Me.SetCName()
            Me.SetDOB(True)
            Me.SetGender()
            Me.SetDOI()
            Me.SetReferenceNo()
            Me.SetTransactionNo()
        End Sub

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Sub SetREPMTNo()
            Me.txtTravelDocNo.Text = Me._strTravelDocNo
            Me.lblTravelDocNo.Text = Me._strTravelDocNo
        End Sub
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        Public Sub SetEName()
            'Fill Data - English only
            Me.txtENameSurname.Text = Me._strENameSurName
            Me.txtENameFirstname.Text = Me._strENameFirstName
        End Sub

        Public Sub SetCName()
            'Fill Data - Chinese name only
            Me.txtCName.Text = Me._strCName
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
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rbGender.SelectedValue = Me._strGender
            End If
            ' CRE20-003 (Batch Upload) [End][Chris YIM]
        End Sub

        Public Sub SetReferenceNo()
            If Me._strReferenceNo.Trim.Equals(String.Empty) Then
                Me.trReferenceNo.Visible = False
            Else
                Me.trReferenceNo.Visible = True
                Me.lblReferenceNo.Text = Me._strReferenceNo
            End If
        End Sub

        Public Sub SetTransactionNo()
            If Me._strTransNo.Trim.Equals(String.Empty) Then
                Me.trTransactionNo.Visible = False
            Else
                Me.trTransactionNo.Visible = True
                Me.lblTransactionNo.Text = Me._strTransNo
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
            Me.SetDOIError(visible)
            Me.SetREPMTNoError(visible)
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

        Public Sub SetDOIError(ByVal visible As Boolean)
            Me.imgDOIError.Visible = visible
        End Sub

        Public Sub SetREPMTNoError(ByVal visible As Boolean)
            Me.imgTravelDocNoError.Visible = visible
        End Sub
#End Region

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            Me._strTravelDocNo = Me.txtTravelDocNo.Text.Trim
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strCName = Me.txtCName.Text.Trim
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

        Public Property CName() As String
            Get
                Return Me._strCName
            End Get
            Set(ByVal value As String)
                Me._strCName = value
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