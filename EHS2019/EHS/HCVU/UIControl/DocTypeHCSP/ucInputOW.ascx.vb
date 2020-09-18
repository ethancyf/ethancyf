Imports System.Web.Security.AntiXss
Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.DocTypeHCSP

    Partial Public Class ucInputOW
        Inherits ucInputDocTypeBase

        Private _strDocumentNo As String
        Private _strENameFirstName As String
        Private _strENameSurName As String
        Private _strCName As String
        Private _strGender As String
        Private _strDOB As String
        Private _strIsExactDOB As String
        Private _strReferenceNo As String = String.Empty
        Private _strTransNo As String = String.Empty

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)


            'Table title
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.OW)

            Me.lblDocumentNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language)

            Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
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

            ''Tips
            'Me.txtENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

        End Sub

        Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

            Me._strDocumentNo = MyBase.EHSPersonalInfo.IdentityNum
            Me._strDOB = Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB

            Me.SetDocumentNo()
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

            If modeType = ucInputDocTypeBase.BuildMode.Creation Then
                ' Creation mode
                Me.lblDocumentNo.Visible = True
                Me.txtDocumentNo.Visible = False
                Me.txtDocumentNo.Enabled = False

                If MyBase.FixEnglishNameGender Then
                    Me.txtDOB.Enabled = False
                    txtENameSurname.Enabled = False
                    txtENameFirstname.Enabled = False
                    rbGender.Enabled = False
                Else
                    Me.txtDOB.Enabled = True
                    txtENameSurname.Enabled = True
                    txtENameFirstname.Enabled = True
                    rbGender.Enabled = True
                End If

            Else
                'Fill Data
                Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
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

                    Me.txtDOB.Enabled = True
                    Me.txtENameFirstname.Enabled = True
                    Me.txtENameSurname.Enabled = True
                    Me.rbGender.Enabled = True

                Else
                    'Modification Read-Only Mode
                    If MyBase.EditDocumentNo Then
                        Me.lblDocumentNo.Visible = False
                        Me.txtDocumentNo.Visible = True
                        Me.txtDocumentNo.Enabled = False
                    Else
                        Me.lblDocumentNo.Visible = True
                        Me.txtDocumentNo.Visible = False
                        Me.txtDocumentNo.Enabled = False
                    End If

                    Me.txtDOB.Enabled = False
                    Me.txtENameFirstname.Enabled = False
                    Me.txtENameSurname.Enabled = False
                    Me.rbGender.Enabled = False

                End If

            End If

            Me.SetReferenceNo()
            Me.SetTransactionNo()

            If MyBase.ActiveViewChanged Then

                Me.SetENameError(False)
                Me.SetGenderError(False)
                Me.SetDOBError(False)
            End If
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
        End Sub

        Public Sub SetDOB()
            Me.txtDOB.Text = Me._strDOB
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
            Me.SetDOBError(visible)
            Me.SetDocumentNoError(visible)
        End Sub

        Public Sub SetENameError(ByVal visible As Boolean)
            Me.imgENameError.Visible = visible
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

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim strDOBtype As String = String.Empty

            Me._strDocumentNo = Me.txtDocumentNo.Text.Trim
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
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

End Namespace