Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Partial Public Class ucInputHKIDSmartIDSignal
    Inherits ucInputDocTypeBase

    'Values
    Public Event SelectedGender(ByVal sender As Object, ByVal e As System.EventArgs)

    Private _strCName As String
    Private _strEName As String
    Private _strCCCode1 As String
    Private _strCCCode2 As String
    Private _strCCCode3 As String
    Private _strCCCode4 As String
    Private _strCCCode5 As String
    Private _strCCCode6 As String
    Private _strDOB As String
    Private _strDOI As String
    Private _strHKIDIssuseDate As String
    Private _strGender As String
    Private _strHKID As String

    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
    Private _udtSmartIDContent As BLL.SmartIDContentModel

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
        'Table title
        Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKIC)

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.lblDocumentType.Text = udtDocTypeModel.DocName(MyBase.SessionHandler.Language())
        Me.lblHKICNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.lblDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        Me.lblDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblENameText.Text = Me.GetGlobalResourceObject("Text", "Name")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblCCCodeText.Text = Me.GetGlobalResourceObject("Text", "CCCODE")

        Me.lblDOIText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'Error Image

        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT

        'div gender
        Me.lblIFemale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderFemale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIFemaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Female", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

        Me.lblIMale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderMale", New System.Globalization.CultureInfo(CultureLanguage.English))
        Me.lblIMaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Male", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

    End Sub

    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        Dim udtEHSAccountSmartID As EHSAccountModel = Me._udtSmartIDContent.EHSAccount

        '-------------------------------------------------------------------------------------------------------------------------
        'Fill SmartID Account Information
        '-------------------------------------------------------------------------------------------------------------------------
        Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccountSmartID.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
        Me._strGender = udtPersonalInfoSmartID.Gender
        Me._strEName = MyBase.Formatter.formatEnglishName(udtPersonalInfoSmartID.ENameSurName, udtPersonalInfoSmartID.ENameFirstName)
        Me._strDOB = MyBase.Formatter.formatDOB(udtPersonalInfoSmartID.DOB, udtPersonalInfoSmartID.ExactDOB, Common.Component.CultureLanguage.English, Nothing, Nothing)
        Me._strHKIDIssuseDate = MyBase.Formatter.formatHKIDIssueDate(udtPersonalInfoSmartID.DateofIssue)
        Me._strCCCode1 = udtPersonalInfoSmartID.CCCode1
        Me._strCCCode2 = udtPersonalInfoSmartID.CCCode2
        Me._strCCCode3 = udtPersonalInfoSmartID.CCCode3
        Me._strCCCode4 = udtPersonalInfoSmartID.CCCode4
        Me._strCCCode5 = udtPersonalInfoSmartID.CCCode5
        Me._strCCCode6 = udtPersonalInfoSmartID.CCCode6
        Me._strEName = MyBase.Formatter.formatEnglishName(udtPersonalInfoSmartID.ENameSurName, udtPersonalInfoSmartID.ENameFirstName)
        Me._strHKID = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum, False)

        Me.SetHKID()
        Me.SetDOI()
        Me.SetDOB()
        Me.SetCName(udtPersonalInfoSmartID)
        Me.SetEName()

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If _udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.TwoGender Or _
           _udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.ComboGender Then
            Me.SetGender(True)
        Else
            'Select Gender
            If MyBase.UpdateValue Then
                Me.SetGender(False)

                If MyBase.FixEnglishNameGender Then
                    SetGenderReadOnlyStyle(True)
                Else
                    SetGenderReadOnlyStyle(False)
                End If
            End If
        End If
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        If MyBase.ActiveViewChanged Then
            Me.SetGenderSmartIDError(False)
        End If

        divFemale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_0').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divFemale.Attributes.Add("onmouseover", "document.getElementById('" & divFemale.ClientID & "').style.left='-1px'; document.getElementById('" & divFemale.ClientID & "').style.top='-1px'; ")
        divFemale.Attributes.Add("onmouseout", "document.getElementById('" & divFemale.ClientID & "').style.left='0px'; document.getElementById('" & divFemale.ClientID & "').style.top='0px'; ")
        divMale.Attributes.Add("onclick", "document.getElementById('" & rbGender.ClientID & "_1').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGender.ClientID & "\',\'\')', 0); ")
        divMale.Attributes.Add("onmouseover", "document.getElementById('" & divMale.ClientID & "').style.left='-1px'; document.getElementById('" & divMale.ClientID & "').style.top='-1px'; ")
        divMale.Attributes.Add("onmouseout", "document.getElementById('" & divMale.ClientID & "').style.left='0px'; document.getElementById('" & divMale.ClientID & "').style.top='0px'; ")


    End Sub

#Region "Set Up Text Box Value (Creation Mode)"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)

        Me.SetValue()

    End Sub

    Public Sub SetDOI()
        'Fill Data - hkid only
        Me.lblDOI.Text = Me._strHKIDIssuseDate
    End Sub

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overloads Sub SetValue()
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'SetGender()
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
    End Sub

    Public Sub SetEName()
        'Fill Data - hkid only
        Me.lblEName.Text = Me._strEName
    End Sub


    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Sub SetHKID()
        'Fill Data - hkid only
        Me.lblHKICNo.Text = Me._strHKID
    End Sub

    Public Sub SetDOB()
        'Fill Data - hkid only
        Me.lblDOB.Text = Me._strDOB
    End Sub



    Public Sub SetCName(ByVal strCName As String)
        Me.lblCName.Text = MyBase.Formatter.formatChineseName(strCName)
    End Sub

    Public Sub SetCName(ByVal udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel)
        'Dim udtVAMaintBLL As BLL.VoucherAccountMaintenanceBLL = New BLL.VoucherAccountMaintenanceBLL()
        Dim strDBCName As String = BLL.VoucherAccountMaintenanceBLL.GetCName(udtPersonalInfoSmartID)

        Me.SetCCCodeLabel(Me.lblCCCode1, Me._strCCCode1)
        Me.SetCCCodeLabel(Me.lblCCCode2, Me._strCCCode2)
        Me.SetCCCodeLabel(Me.lblCCCode3, Me._strCCCode3)
        Me.SetCCCodeLabel(Me.lblCCCode4, Me._strCCCode4)
        Me.SetCCCodeLabel(Me.lblCCCode5, Me._strCCCode5)
        Me.SetCCCodeLabel(Me.lblCCCode6, Me._strCCCode6)

        If strDBCName = String.Empty Then
            'If Me._strCName Is Nothing Then
            '    Me._strCName = String.Empty
            'End If
        Else
            Me._strCName = strDBCName
        End If

        If Me._strCName Is Nothing Then
            Me._strCName = String.Empty
        End If

        Me.SetCName(Me._strCName)

        'If strDBCName = String.Empty Then
        '    Me.SetCName(String.Empty)
        'Else
        '    Me._strCName = strDBCName
        '    Me.SetCName(strDBCName)
        'End If

    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Sub SetGender(ByVal blnReadOnly As Boolean)

        HandleDivGenderStyle(String.Empty)
        If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
            Dim strGender As String
            Me.rbGender.SelectedValue = Me._strGender

            If Me._strGender = "M" Then
                strGender = "GenderMale"
            Else
                strGender = "GenderFemale"
            End If
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            HandleDivGenderStyle(Me._strGender)
        Else
            Me.rbGender.SelectedValue = Nothing
        End If

        If blnReadOnly Then
            SetGenderReadOnlyStyle(True)
        Else
            SetGenderReadOnlyStyle(False)
        End If
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    Private Function GetCCCode(ByVal strCCCode As String) As String
        If Not strCCCode Is Nothing AndAlso strCCCode.Length > 4 Then
            Return strCCCode.Substring(0, 4)
        End If
        Return String.Empty
    End Function

#End Region

#Region "Set Up Error Image (Modification Mode)"

    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetGenderSmartIDError(visible)
    End Sub

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Sub SetGenderSmartIDError(ByVal visible As Boolean)
        Me.imgGender.Visible = visible
    End Sub


#End Region

#Region "Events"

    Protected Sub rbGender_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbGender.SelectedIndexChanged
        Dim rbGender As RadioButtonList = CType(sender, RadioButtonList)
        HandleDivGenderStyle(rbGender.SelectedValue)
        RaiseEvent SelectedGender(sender, e)
    End Sub

    Private Sub HandleDivGenderStyle(ByVal strGender As String)
        Select Case strGender
            Case "M"
                divFemale.Style.Add("outline-color", "black")
                divFemale.Style.Add("outline-width", "2px")

                divMale.Style.Add("outline-color", "#3198FF")
                divMale.Style.Add("outline-width", "8px")

            Case "F"
                divFemale.Style.Add("outline-color", "#3198FF")
                divFemale.Style.Add("outline-width", "8px")

                divMale.Style.Add("outline-color", "black")
                divMale.Style.Add("outline-width", "2px")

            Case Else
                divFemale.Style.Add("outline-color", "black")
                divFemale.Style.Add("outline-width", "2px")

                divMale.Style.Add("outline-color", "black")
                divMale.Style.Add("outline-width", "2px")
        End Select
    End Sub

    Private Sub SetGenderReadOnlyStyle(ByVal ReadOnlyMode As Boolean)
        If ReadOnlyMode = True Then
            Me.divGender.Visible = False
            Me.lblReadonlyGender.Visible = True
            Me.trGenderImageInput.Style.Remove("height")
            Me.rbGender.Enabled = False
        Else
            Me.divGender.Visible = True
            Me.lblReadonlyGender.Visible = False
            Me.rbGender.Enabled = True
        End If
    End Sub

#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Me._strGender = Me.rbGender.SelectedValue
    End Sub

    Public Property SmartIDContentModel() As HCSP.BLL.SmartIDContentModel
        Get
            Return Me._udtSmartIDContent
        End Get
        Set(ByVal value As HCSP.BLL.SmartIDContentModel)
            Me._udtSmartIDContent = value
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


    Public ReadOnly Property Gender() As String
        Get
            Return Me._strGender
        End Get
    End Property
#End Region

End Class
