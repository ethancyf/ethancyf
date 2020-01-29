Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component

Namespace UIControl.DocTypeText
    Partial Public Class ucInputHKID
        Inherits ucInputDocTypeBase

        'Values
        Public Event SelectChineseName(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Private _strENameFirstName As String
        Private _strENameSurName As String
        Private _strCName As String
        Private _strCCCode1 As String
        Private _strCCCode2 As String
        Private _strCCCode3 As String
        Private _strCCCode4 As String
        Private _strCCCode5 As String
        Private _strCCCode6 As String
        Private _strDOB As String
        Private _strHKIDIssuseDate As String
        Private _strGender As String
        Private _strHKID As String

        Private _strReferenceNo As String = String.Empty
        Private _strTransNo As String = String.Empty

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

        Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

            '-------------------------------------Account Creation Mode-------------------------------------------
            'Me.btnSearchCCCode.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChineseNameSBtn")
            'Me.btnSearchCCCode.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChineseNameBtn")

            'Gender Radio button list
            Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

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

            ' DOI
            Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")
            'Me.lblDOITip.Text = Me.GetGlobalResourceObject("Text", "DOIHintDI")

            ' HKID
            'Me.lblHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKIC)
            Me.lblHKIDText.Text = IIf(SessionHandler.Language() = CultureLanguage.English, udtDocTypeModel.DocIdentityDesc, udtDocTypeModel.DocIdentityDescChi)

        End Sub

        Protected Overrides Sub Setup(ByVal mode As BuildMode)
            'Add Client Event--------------------------------------------------------------------------------------------

            Me._strHKID = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum, False)
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, Me.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me.SetHKID()
            Me.SetDOB()

            If MyBase.UpdateValue Then

                If Me.FillValue(MyBase.EHSPersonalInfo) AndAlso MyBase.ActiveViewChanged Then
                    Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
                    Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
                    Me._strGender = MyBase.EHSPersonalInfo.Gender
                    Me._strHKIDIssuseDate = MyBase.Formatter.formatHKIDIssueDate(MyBase.EHSPersonalInfo.DateofIssue)
                    Me.SetValue(BuildMode.Creation)

                    Me.SetErrorImage(BuildMode.Creation, False)
                End If

            End If
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

#Region "Set Up Text Box Value  (Creation Mode)"

        Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
            Me.SetValue()
        End Sub

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Text Box Value
        '--------------------------------------------------------------------------------------------------------------
        Public Overloads Sub SetValue()
            Me.SetHKID()
            Me.SetDOB()

            'Me.SetCName()
            Me.SetEName()
            Me.SetGender()
            Me.SetHKIDIssuseDate()

        End Sub


        Public Sub SetHKID()
            'Fill Data - hkid only
            Me.txtHKID.Text = Me._strHKID
        End Sub

        Public Sub SetEName()
            'Fill Data - English only
            Me.txtENameSurname.Text = Me._strENameSurName
            Me.txtENameFirstrname.Text = Me._strENameFirstName
        End Sub

        Public Sub SetDOB()
            'Fill Data - DOB only
            Me.txtDOB.Text = Me._strDOB
        End Sub

        Public Sub SetGender()
            'Fill Data - Gender only
            Me.rbGender.SelectedValue = Me._strGender
            If Not Me._strGender Is Nothing Then
                Me.changeHKID(Me.EHSPersonalInfo.DOB, Me._strGender)
            End If

        End Sub

        Public Sub SetHKIDIssuseDate()
            'Fill Data - HKID DOI only
            Me.txtHKIDIssueDate.Text = Me._strHKIDIssuseDate
            Me.txtHKID.Text = Me._strHKID
        End Sub



#End Region

#Region "Set Up Error Image (Creation Mode)"

        Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
            Me.SetErrorImage(visible)
        End Sub

        Public Overloads Sub SetErrorImage(ByVal visible As Boolean)
            Me.SetENameError(visible)
            Me.SetGenderError(visible)
            Me.SetHKIDIssueDateError(visible)
        End Sub

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Error Image
        '--------------------------------------------------------------------------------------------------------------

        Public Sub SetENameError(ByVal visible As Boolean)
            Me.lblSurNameError.Visible = visible
            Me.lblGivenNameError.Visible = visible
        End Sub

        Public Sub SetGenderError(ByVal visible As Boolean)
            Me.lblGenderError.Visible = visible
        End Sub

        Public Sub SetHKIDIssueDateError(ByVal visible As Boolean)
            Me.lblDOIError.Visible = visible
        End Sub

#End Region


#Region "Events"
        '--------------------------------------------------------------------------------------------------------------
        'Events
        '--------------------------------------------------------------------------------------------------------------
        'Protected Sub btnSearchCCCode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchCCCode.Click, btnSearchCCCodeModification.Click
        '    RaiseEvent SelectChineseName(sender, e)
        'End Sub

        Private Sub rbGender_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbGender.SelectedIndexChanged
            Me.changeHKID(Me.EHSPersonalInfo.DOB, CType(sender, RadioButtonList).SelectedValue)
        End Sub

#End Region

#Region "Property"
        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            'If mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me._strENameFirstName = Me.txtENameFirstrname.Text.Trim.ToUpper
            Me._strENameSurName = Me.txtENameSurname.Text.Trim.ToUpper
            'Me._strCName = Me.lblCName.Text
            Me._strGender = Me.rbGender.SelectedValue
            Me._strHKID = Me.txtHKID.Text.Trim.ToUpper
            Me._strDOB = Me.txtDOB.Text.Trim
            Me._strHKIDIssuseDate = Me.txtHKIDIssueDate.Text.Trim
            'Me._strCCCode1 = Me.txtCCCode1.Text
            'Me._strCCCode2 = Me.txtCCCode2.Text
            'Me._strCCCode3 = Me.txtCCCode3.Text
            'Me._strCCCode4 = Me.txtCCCode4.Text
            'Me._strCCCode5 = Me.txtCCCode5.Text
            'Me._strCCCode6 = Me.txtCCCode6.Text
            'Else
            '    Me._strENameFirstName = Me.txtENameFirstnameModification.Text.Trim.ToUpper
            '    Me._strENameSurName = Me.txtENameSurnameModification.Text.Trim.ToUpper
            '    'Me._strCName = Me.lblCNameModification.Text
            '    Me._strGender = Me.rbGenderModification.SelectedValue
            '    Me._strHKID = Me.lblHKICNoModification.Text.Trim.ToUpper
            '    Me._strDOB = Me.txtDOBModification.Text
            '    Me._strHKIDIssuseDate = Me.txtDOIModification.Text
            '    'Me._strCCCode1 = Me.txtCCCode1Modification.Text
            '    'Me._strCCCode2 = Me.txtCCCode2Modification.Text
            '    'Me._strCCCode3 = Me.txtCCCode3Modification.Text
            '    'Me._strCCCode4 = Me.txtCCCode4Modification.Text
            '    'Me._strCCCode5 = Me.txtCCCode5Modification.Text
            '    'Me._strCCCode6 = Me.txtCCCode6Modification.Text
            'End If
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

        Public Property Gender() As String
            Get
                Return Me._strGender
            End Get
            Set(ByVal value As String)
                Me._strGender = value
            End Set
        End Property

        Public Property HKID() As String
            Get
                Return Me._strHKID
            End Get
            Set(ByVal value As String)
                Me._strHKID = value
            End Set
        End Property

        Public Property HKIDIssuseDate() As String
            Get
                Return Me._strHKIDIssuseDate
            End Get
            Set(ByVal value As String)
                Me._strHKIDIssuseDate = value
            End Set
        End Property

        Public Property CCCode1() As String
            Get
                Return Me._strCCCode1
            End Get
            Set(ByVal value As String)
                Me._strCCCode1 = value.Trim()
            End Set
        End Property

        Public Property CCCode2() As String
            Get
                Return Me._strCCCode2
            End Get
            Set(ByVal value As String)
                Me._strCCCode2 = value.Trim()
            End Set
        End Property

        Public Property CCCode3() As String
            Get
                Return Me._strCCCode3
            End Get
            Set(ByVal value As String)
                Me._strCCCode3 = value.Trim()
            End Set
        End Property

        Public Property CCCode4() As String
            Get
                Return Me._strCCCode4
            End Get
            Set(ByVal value As String)
                Me._strCCCode4 = value.Trim()
            End Set
        End Property

        Public Property CCCode5() As String
            Get
                Return Me._strCCCode5
            End Get
            Set(ByVal value As String)
                Me._strCCCode5 = value.Trim()
            End Set
        End Property

        Public Property CCCode6() As String
            Get
                Return Me._strCCCode6
            End Get
            Set(ByVal value As String)
                Me._strCCCode6 = value.Trim()
            End Set
        End Property

        Public Property CName() As String
            Get
                Return Me._strCName
            End Get
            Set(ByVal value As String)
                Me._strCName = value.Trim()
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

#Region "Other"

        Public Sub changeHKID(ByVal dtmDOB As DateTime, ByVal strRadioButtonSelectedGender As String)
            Dim commonFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim dtmCurrentDate As DateTime = commonFunction.GetSystemDateTime()
            Dim strGender As String


            If strRadioButtonSelectedGender Is Nothing OrElse strRadioButtonSelectedGender.Equals(String.Empty) Then
                'Me.hkid_cell.Style.Item("background-image") = "url(../Images/HKID/HKID_Empty.jpg)"
            Else
                If strRadioButtonSelectedGender = "M" Then
                    strGender = "Male"
                Else
                    strGender = "Female"
                End If

                'If dtmCurrentDate.Year - dtmDOB.Year <= 11 Then
                '    Me.hkid_cell.Style.Item("background-image") = String.Format("url(../Images/HKID/HKID_Children_{0}.jpg)", strGender)
                'ElseIf dtmCurrentDate.Year - dtmDOB.Year < 65 Then
                '    Me.hkid_cell.Style.Item("background-image") = String.Format("url(../Images/HKID/HKID_Teenage_{0}.jpg)", strGender)
                'Else
                '    Me.hkid_cell.Style.Item("background-image") = String.Format("url(../Images/HKID/HKID_Elder_{0}.jpg)", strGender)
                'End If
            End If
        End Sub

#End Region

    End Class

End Namespace



#Region "comment Code"

'Private _udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
'Private _updateValue As Boolean
'Private udtVRAcct As VoucherRecipientAccountModel
'Private _udtFormatter As Common.Format.Formatter = New Common.Format.Formatter

'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'    'Dim scriptManager As ScriptManagerProxy = Me.ScriptManagerProxy1


'    Me.Setup()
'    Me.RenderLanguage()
'    'scriptManager.SetFocus(txtENameSurname)
'End Sub


'Script in Page Load----------------------------------------------------------------
'Me.rbGender.Items(0).Attributes.Add("onclick", String.Format("document.getElementById('{0}').style.backgroundImage = 'url(../Images/HKID/HKID_Female.jpg)';", Me.hkid_cell.ClientID))
'Me.rbGender.Items(1).Attributes.Add("onclick", String.Format("document.getElementById('{0}').style.backgroundImage = 'url(../Images/HKID/HKID_Male.jpg)';", Me.hkid_cell.ClientID))



'Public Property DocumentType() As String
'    Get
'        Return Me._strDocumentType
'    End Get
'    Set(ByVal value As String)
'        Me._strDocumentType = value.Trim()
'    End Set
'End Property

'Public Property EHealthAccount() As EHSAccountModel.EHSPersonalInformationModel
'    Get
'        Return Me._udtEHSAccountPersonalInfo
'    End Get
'    Set(ByVal value As EHSAccountModel.EHSPersonalInformationModel)
'        Me._udtEHSAccountPersonalInfo = value
'    End Set
'End Property

'Public Property UpdateValue() As Boolean
'    Get
'        Return Me._updateValue
'    End Get
'    Set(ByVal value As Boolean)
'        Me._updateValue = value
'    End Set
'End Property
#End Region