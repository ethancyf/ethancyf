Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType

Public Class ucInputROP140
    Inherits ucInputDocTypeBase

    'Declare Event
    Public Event SelectChineseName(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    'Values
    Private _strTDNumber As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strCCCode1 As String
    Private _strCCCode2 As String
    Private _strCCCode3 As String
    Private _strCCCode4 As String
    Private _strCCCode5 As String
    Private _strCCCode6 As String
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

        Me.lblCCCodeText.Text = Me.GetGlobalResourceObject("Text", "CCCODE")
        Me.btnSearchCCCode.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChineseNameSBtn")
        Me.btnSearchCCCode.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChineseNameBtn")

        Me.lblCNameText.Text = Me.GetGlobalResourceObject("Text", "ChineseName")

        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblDOI.Text = Me.GetGlobalResourceObject("Text", "DOILong")

        Me.lblDOIROP140Hint.Text = Me.GetGlobalResourceObject("Text", "DOIHintROP140")

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

        Me.imgDOIDate.ImageUrl = strErrorImageURL
        Me.imgDOIDate.AlternateText = strErrorImageALT

    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, MyBase.EHSPersonalInfo.IdentityNum, False)
        Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)

        If MyBase.UpdateValue Then
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender

            'CName may assiged for display only
            'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
            Me._strCName = MyBase.EHSPersonalInfo.CName

            If MyBase.EHSPersonalInfo.DateofIssue.HasValue Then
                Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfo.DateofIssue))
            Else
                Me._strDOI = String.Empty
            End If

            If Not MyBase.EHSPersonalInfo.CCCode1 Is Nothing Then
                Me._strCCCode1 = MyBase.EHSPersonalInfo.CCCode1.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode2 Is Nothing Then
                Me._strCCCode2 = MyBase.EHSPersonalInfo.CCCode2.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode3 Is Nothing Then
                Me._strCCCode3 = MyBase.EHSPersonalInfo.CCCode3.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode4 Is Nothing Then
                Me._strCCCode4 = MyBase.EHSPersonalInfo.CCCode4.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode5 Is Nothing Then
                Me._strCCCode5 = MyBase.EHSPersonalInfo.CCCode5.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode6 Is Nothing Then
                Me._strCCCode6 = MyBase.EHSPersonalInfo.CCCode6.Trim()
            End If

        End If

        Me.SetValue(modeType)

        Me.txtCCCode1.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode2.ClientID + ",4 );")
        Me.txtCCCode2.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode3.ClientID + ",4 );")
        Me.txtCCCode3.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode4.ClientID + ",4 );")
        Me.txtCCCode4.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode5.ClientID + ",4 );")
        Me.txtCCCode5.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode6.ClientID + ",4 );")

        'Mode related Settings
        If modeType = ucInputDocTypeBase.BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(False)
            End If

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
                '--------------------------------------------------------
                'For Modification Mode
                '--------------------------------------------------------
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
                Me.txtDOI.Enabled = True
            Else
                'Modification Read-Only Mode
                Me.lblTDNo.Visible = True
                Me.txtTDNo.Visible = False
                Me.txtTDNo.Enabled = False

                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOB.Enabled = False
                Me.txtDOI.Enabled = False
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
        If MyBase.ActiveViewChanged Then
            Me.SetCName()
        End If
        SetupTDNumber()
        Me.SetEName()
        SetupGender()
        SetupDOB()
        SetupDOI()
        SetReferenceNo()
        SetTransactionNo()
    End Sub

    Public Sub SetCName()
        Dim udtVAMaintBLL As BLL.VoucherAccountMaintenanceBLL = New BLL.VoucherAccountMaintenanceBLL()
        Dim strDBCName As String = String.Empty

        If Not Me._strCCCode1 Is Nothing Then
            If Me._strCCCode1.Length > 4 Then
                Me.txtCCCode1.Text = Me._strCCCode1.Substring(0, 4)
            Else
                Me.txtCCCode1.Text = Me._strCCCode1
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode1)
        Else
            Me.txtCCCode1.Text = String.Empty
        End If

        If Not Me._strCCCode2 Is Nothing AndAlso Me._strCCCode2.Trim().Length > 4 Then
            If Me._strCCCode2.Length > 4 Then
                Me.txtCCCode2.Text = Me._strCCCode2.Substring(0, 4)
            Else
                Me.txtCCCode2.Text = Me._strCCCode2
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode2)
        Else
            Me.txtCCCode2.Text = String.Empty
        End If

        If Not Me._strCCCode3 Is Nothing AndAlso Me._strCCCode3.Trim().Length > 4 Then
            If Me._strCCCode3.Length > 4 Then
                Me.txtCCCode3.Text = Me._strCCCode3.Substring(0, 4)
            Else
                Me.txtCCCode3.Text = Me._strCCCode3
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode3)
        Else
            Me.txtCCCode3.Text = String.Empty
        End If

        If Not Me._strCCCode4 Is Nothing AndAlso Me._strCCCode4.Trim().Length > 4 Then
            If Me._strCCCode4.Length > 4 Then
                Me.txtCCCode4.Text = Me._strCCCode4.Substring(0, 4)
            Else
                Me.txtCCCode4.Text = Me._strCCCode4
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode4)
        Else
            Me.txtCCCode4.Text = String.Empty
        End If

        If Not Me._strCCCode5 Is Nothing AndAlso Me._strCCCode5.Trim().Length > 4 Then
            If Me._strCCCode5.Length > 4 Then
                Me.txtCCCode5.Text = Me._strCCCode5.Substring(0, 4)
            Else
                Me.txtCCCode5.Text = Me._strCCCode5
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode5)
        Else
            Me.txtCCCode5.Text = String.Empty
        End If

        If Not Me._strCCCode6 Is Nothing AndAlso Me._strCCCode6.Trim().Length > 4 Then
            If Me._strCCCode6.Length > 4 Then
                Me.txtCCCode6.Text = Me._strCCCode6.Substring(0, 4)
            Else
                Me.txtCCCode6.Text = Me._strCCCode6
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode6)
        Else
            Me.txtCCCode6.Text = String.Empty
        End If

        If strDBCName = String.Empty Then
            'Nothing to do
        Else
            Me._strCName = strDBCName
        End If

        If Me._strCName Is Nothing Then
            Me._strCName = String.Empty
        End If

        Me.SetCName(Me._strCName)

    End Sub

    Public Sub SetCName(ByVal strCName As String)
        Me.lblCName.Text = strCName
    End Sub

    Public Sub SetupTDNumber()
        Me.txtTDNo.Text = Me._strTDNumber
        Me.lblTDNo.Text = Me._strTDNumber
    End Sub

    Public Sub SetEName()
        Me.txtENameFirstname.Text = Me._strENameFirstName
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

    Public Function CCCodeIsEmpty() As Boolean
        If Me.txtCCCode1.Text = String.Empty AndAlso _
            Me.txtCCCode2.Text = String.Empty AndAlso _
            Me.txtCCCode3.Text = String.Empty AndAlso _
            Me.txtCCCode4.Text = String.Empty AndAlso _
            Me.txtCCCode5.Text = String.Empty AndAlso _
            Me.txtCCCode6.Text = String.Empty Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#Region "Set Up Error Image"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetErrorImage(visible)
    End Sub

    Public Overloads Sub SetErrorImage(ByVal visible As Boolean)
        Me.SetCCCodeError(visible)
        Me.SetTDError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetDOIError(visible)
    End Sub

    Public Sub SetCCCodeError(ByVal visible As Boolean)
        Me.imgCCCodeError.Visible = visible
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

    Public Sub SetDOIError(ByVal visible As Boolean)
        Me.imgDOIDate.Visible = visible
    End Sub

#End Region

#Region "Events"
    '--------------------------------------------------------------------------------------------------------------
    'Events
    '--------------------------------------------------------------------------------------------------------------
    Protected Sub btnSearchCCCode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchCCCode.Click ', btnSearchCCCodeModification.Click
        RaiseEvent SelectChineseName(sender, e)
    End Sub

#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Me._strTDNumber = Me.txtTDNo.Text.Trim
        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
        Me._strENameSurName = Me.txtENameSurname.Text.Trim
        Me._strCName = Me.lblCName.Text
        Me._strGender = Me.rbGender.SelectedValue.Trim
        Me._strDOI = Me.txtDOI.Text.Trim
        Me._strDOB = Me.txtDOB.Text.Trim
        Me._strCCCode1 = Me.txtCCCode1.Text.Trim
        Me._strCCCode2 = Me.txtCCCode2.Text.Trim
        Me._strCCCode3 = Me.txtCCCode3.Text.Trim
        Me._strCCCode4 = Me.txtCCCode4.Text.Trim
        Me._strCCCode5 = Me.txtCCCode5.Text.Trim
        Me._strCCCode6 = Me.txtCCCode6.Text.Trim
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

#Region "Supported Function"
    Public Function GetCCCode(ByVal strInputCCCode As String, ByVal strExistCCCode As String) As String

        If strInputCCCode Is Nothing Then
            Return String.Empty
        End If

        If Not strInputCCCode.Equals(String.Empty) AndAlso strInputCCCode.Length >= 4 Then

            'check session CCCode exist
            If Not strExistCCCode Is Nothing AndAlso Not strExistCCCode.Equals(String.Empty) AndAlso strExistCCCode.Length > 4 Then
                'check if code head match
                If strInputCCCode.Substring(0, 4) = strExistCCCode.Substring(0, 4) Then
                    Return strExistCCCode
                End If
            End If
        End If

        Return strInputCCCode

    End Function

    Public Function IsValidCCCodeInput() As Boolean
        Return (Me.txtCCCode1.Text.Length = 4 OrElse Me.txtCCCode1.Text.Length = 0) AndAlso _
               (Me.txtCCCode2.Text.Length = 4 OrElse Me.txtCCCode2.Text.Length = 0) AndAlso _
               (Me.txtCCCode3.Text.Length = 4 OrElse Me.txtCCCode3.Text.Length = 0) AndAlso _
               (Me.txtCCCode4.Text.Length = 4 OrElse Me.txtCCCode4.Text.Length = 0) AndAlso _
               (Me.txtCCCode5.Text.Length = 4 OrElse Me.txtCCCode5.Text.Length = 0) AndAlso _
               (Me.txtCCCode6.Text.Length = 4 OrElse Me.txtCCCode6.Text.Length = 0)
    End Function

    Public Function IsValidCCCodeModificationInput() As Boolean
        Return (Me.txtCCCode1.Text.Length = 4 OrElse Me.txtCCCode1.Text.Length = 0) AndAlso _
               (Me.txtCCCode2.Text.Length = 4 OrElse Me.txtCCCode2.Text.Length = 0) AndAlso _
               (Me.txtCCCode3.Text.Length = 4 OrElse Me.txtCCCode3.Text.Length = 0) AndAlso _
               (Me.txtCCCode4.Text.Length = 4 OrElse Me.txtCCCode4.Text.Length = 0) AndAlso _
               (Me.txtCCCode5.Text.Length = 4 OrElse Me.txtCCCode5.Text.Length = 0) AndAlso _
               (Me.txtCCCode6.Text.Length = 4 OrElse Me.txtCCCode6.Text.Length = 0)
    End Function

#End Region

End Class