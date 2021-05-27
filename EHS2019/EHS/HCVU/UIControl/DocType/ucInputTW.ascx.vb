Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Format


Public Class ucInputTW
    Inherits ucInputDocTypeBase

    'For Amending Record Used Only
    Private _strDocumentNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOBInWord As String
    Private _strIsExactDOB As String
    Private _strDOD As String ' CRE14-016
    Private _blnDOBTypeSelected As Boolean
    Private _blnDOBInWordCase As Boolean


    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
        'Get Documnet type full name
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

        Me.lblDocNoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.TW).DocIdentityDesc(Session("Language").ToString())

        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        Me.lblNameOriginalText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblDOBOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblGenderOriginalText.Text = Me.GetGlobalResourceObject("Text", "Gender")

        '-----------------------------------------
        'Table title
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")

        If MyBase.Mode = BuildMode.Modification Then
            Me.lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "AmendingRecord")
        Else
            If Not MyBase.UseDefaultAmendingHeader Then
                lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "PendingImmgValid")
            Else
                lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "AmendingRecord")
            End If
        End If

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'Error Image
        Me.imgENameError.ImageUrl = strErrorImageURL
        Me.imgENameError.AlternateText = strErrorImageALT

        Me.imgGenderError.ImageUrl = strErrorImageURL
        Me.imgGenderError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        If Not MyBase.EHSPersonalInfoAmend Is Nothing Then
            Me.lblDocumentTypeOriginal.Text = udtDocTypeModelList.Filter(MyBase.EHSPersonalInfoAmend.DocCode).DocName(Session("Language").ToString())
            ' -------------------------- Creation ------------------------------
            Me.lblNewDocNoText.Text = udtDocTypeModelList.Filter(MyBase.EHSPersonalInfoAmend.DocCode).DocIdentityDesc(Session("Language").ToString())
        End If

        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgNewDocNoErr.ImageUrl = strErrorImageURL
        Me.imgNewDocNoErr.AlternateText = strErrorImageALT

        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT

        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        Me.imgNewDOBErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBErr.AlternateText = strErrorImageALT
    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Me.pnlNew.Visible = False
        Me.pnlModify.Visible = False

        If Me.Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True
            Me.lblNewDocNo_ModifyOneSide.Visible = True
            Me.txtNewDocNo.Visible = False

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                'Pre-Fill before entering account creation page (eHS maintenance)
                Me._strDocumentNo = MyBase.EHSPersonalInfoAmend.IdentityNum
                Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strCName = MyBase.EHSPersonalInfoAmend.CName
                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

                Me.txtNewDocNo.Enabled = False
                SetValue(BuildMode.Creation)
            End If

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True
            Me.lblNewDocNo_ModifyOneSide.Visible = True
            Me.txtNewDocNo.Visible = False
            Me.txtNewDocNo.Enabled = False

            Me._strDocumentNo = MyBase.EHSPersonalInfoAmend.IdentityNum
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strCName = MyBase.EHSPersonalInfoAmend.CName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfoAmend.ExactDOB
            Me._strDOBInWord = MyBase.EHSPersonalInfoAmend.OtherInfo

            Me.SetValue(Mode)

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            Me.txtNewSurname.Enabled = True
            Me.txtNewGivenName.Enabled = True
            Me.txtNewDOB.Enabled = True
            Me.rboNewGender.Enabled = True


        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True
            Me._strDocumentNo = MyBase.EHSPersonalInfoAmend.IdentityNum
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strCName = MyBase.EHSPersonalInfoAmend.CName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfoAmend.ExactDOB
            Me._strDOBInWord = MyBase.EHSPersonalInfoAmend.OtherInfo

            Me.SetValue(Mode)

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            Me.txtDocNo.Enabled = False
            Me.txtDocNo.Visible = False
            Me.lblDocNo.Visible = True

            If Mode = ucInputDocTypeBase.BuildMode.Modification Then
                '--------------------------------------------------------
                'Modification Mode
                '--------------------------------------------------------
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.rbGender.Enabled = True

            Else
                '--------------------------------------------------------
                'Modify Read Only Mode
                'ReadOnly, 2 sides showing both original and amending record 
                '--------------------------------------------------------
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.rbGender.Enabled = False

            End If

        End If


    End Sub

    ''' <summary>
    '''  Update the contents of "DOB in Word" drop down list
    ''' </summary>
    ''' <param name="enable">Whether the radio button for DOB in word type is checked</param>
    ''' <remarks></remarks>
    Private Sub DOBInWordOption(ByVal enable As Boolean)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
        Dim dataTable As DataTable
        Dim dtDOBinWorType As DataTable = New DataTable
        Dim dataRow As DataRow

        If enable Then
            dataTable = udtStaticDataBLL.GetStaticDataList("DOBInWordType")

            dataRow = dataTable.NewRow
            dataRow(StaticDataModel.Column_Name) = 0
            dataRow(StaticDataModel.Item_No) = String.Empty
            dataRow(StaticDataModel.Data_Value) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
            dataRow(StaticDataModel.Data_Value_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
            dataTable.Rows.InsertAt(dataRow, 0)
        End If

    End Sub

    ''' <summary>
    '''  Once the DOB type is changed, re-set the visibility and functionings of 
    '''  DOB related drop down list and textbox
    ''' </summary>
    ''' <param name="enable">Whether the radio button for DOB type is checked</param>
    ''' <remarks></remarks>
    Private Sub ChangeDOBOption(ByVal enable As Boolean)
        If enable Then
            If Me.Mode = BuildMode.Creation Then
                Me.txtNewDOB.Enabled = True
            ElseIf Me.Mode = BuildMode.Modification_OneSide Then
                Me.txtNewDOB.Enabled = True
            Else
                Me.txtDOB.Enabled = True
            End If

        Else
            If Me.Mode = BuildMode.Creation Then
                Me.txtNewDOB.Enabled = False
            ElseIf Me.Mode = BuildMode.Modification_OneSide Then
                Me.txtNewDOB.Enabled = False
                Me.txtNewDOB.Text = String.Empty
            Else
                Me.txtDOB.Enabled = False
            End If

        End If
    End Sub

#Region "Events"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Me.Mode = BuildMode.Modification_OneSide Then
            Me.ChangeDOBOption(True)
            'ElseIf Me.Mode = BuildMode.Modification Or Me.Mode = BuildMode.ModifyReadOnly Then
        End If

    End Sub
#End Region

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            'For Creation Mode
            Me.SetDocNo()
            Me.SetEName()
            Me.SetDOB()
            If Me._blnDOBTypeSelected Then
                Me.SetDOBType()
            End If

        ElseIf mode = BuildMode.Modification_OneSide Then
            'For Modification One Side Mode
            Me.SetDocNo()
            Me.SetEName()
            Me.SetGender()
            Me.SetDOB()
        Else
            'For Modification Mode
            Me.SetDocNo()
            Me.SetEName()
            Me.SetGender()
            Me.SetDOB()
        End If

    End Sub

    Public Sub SetDocNo()
        If Mode = BuildMode.Creation Then
            Me.txtNewDocNo.Text = Me._strDocumentNo
            Me.lblNewDocNo_ModifyOneSide.Text = Me._strDocumentNo

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDocNo.Text = Me._strDocumentNo
            Me.lblNewDocNo_ModifyOneSide.Text = Me._strDocumentNo

        Else
            'Amend
            Me.txtDocNo.Text = Me._strDocumentNo
            Me.lblDocNo.Text = Me._strDocumentNo
            'Orginal
            Me.lblDocNoOriginal.Text = Formatter.FormatDocIdentityNoForDisplay(MyBase.EHSPersonalInfoAmend.DocCode, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)

        End If

    End Sub

    Public Sub SetEName()
        If Mode = BuildMode.Creation Then
            Me.txtNewGivenName.Text = Me._strENameFirstName
            Me.txtNewSurname.Text = Me._strENameSurName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewSurname.Text = Me._strENameSurName
            Me.txtNewGivenName.Text = Me._strENameFirstName

        Else
            'Amend
            Me.txtENameSurname.Text = Me._strENameSurName
            Me.txtENameFirstname.Text = Me._strENameFirstName
            'Original
            lblNameOriginal.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfoOriginal.ENameSurName, MyBase.EHSPersonalInfoOriginal.ENameFirstName)
        End If
    End Sub

    Public Sub SetDOB()
        If Mode = BuildMode.Creation Then
            'Creation Mode -------------------------------------------------------------------------
            Me.txtNewDOB.Text = Me._strDOB

        ElseIf Mode = BuildMode.Modification_OneSide Then
            'Modification Mode (One Side) -----------------------------------------------------------
            Me.SetDOBType()
        Else
            'Amend
            Me.SetDOBType()
            'Original
            Dim udtStaticDataBLL As New StaticDataBLL
            lblDOBOriginal.Text = String.Empty
            lblDOBOriginal.Text += Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoOriginal.ECAge, MyBase.EHSPersonalInfoOriginal.ECDateOfRegistration)
        End If

    End Sub

    Public Sub SetGender()

        If Mode = BuildMode.Creation Then
            Me.rboNewGender.ClearSelection()

        ElseIf Mode = BuildMode.Modification_OneSide Then
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rboNewGender.SelectedValue = Me._strGender
            End If

        Else
            'Amend
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rbGender.SelectedValue = Me._strGender
            End If

            'Orginal
            Select Case MyBase.EHSPersonalInfoOriginal.Gender.Trim
                Case "M"
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select
        End If

    End Sub

    Public Sub SetDOBType()

        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode  ------------------------------------------------------------------------------------------------
        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            'Modification Mode (One Side) ------------------------------------------------------------------------------------
            Me.txtNewDOB.Enabled = True
            Me.txtNewDOB.Text = Me._strDOB

        Else
            'Modification Mode -----------------------------------------------------------------------------------------------
            If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                Me.txtDOB.Enabled = True
            Else
                Me.txtDOB.Enabled = False
            End If

            Me.txtDOB.Text = Me._strDOB

        End If

    End Sub


#End Region

#Region "Set Up Error Image"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetDocNoError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
    End Sub

    Public Sub SetDocNoError(ByVal visible As Boolean)

        Me.imgNewDocNoErr.Visible = visible

    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewENameErr.Visible = visible
        Else
            Me.imgENameError.Visible = visible
        End If

    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewGenderErr.Visible = visible
        Else
            Me.imgGenderError.Visible = visible
        End If

    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOBErr.Visible = visible
        Else
            Me.imgDOBError.Visible = visible
        End If

    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        'Creation Or Modication(One Side) mode
        If mode = BuildMode.Creation Or mode = BuildMode.Modification_OneSide Then

            If mode = BuildMode.Creation Then
                Me._strDocumentNo = Me.txtNewDocNo.Text
            ElseIf mode = BuildMode.Modification_OneSide Then
                Me._strDocumentNo = Me.txtNewDocNo.Text
            End If

            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim

            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)

            If Not strDOBtype.Trim.Equals(String.Empty) Then
                Me._strIsExactDOB = strDOBtype
            Else
                'in case of empty DOB
                Me._strIsExactDOB = "D"
            End If

            Me._blnDOBInWordCase = False

        Else
            Me._strDocumentNo = Me.txtDocNo.Text.Trim
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strGender = Me.rbGender.SelectedValue
            Me._strDOB = Me.txtDOB.Text.Trim

            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)

            If Not strDOBtype.Trim.Equals(String.Empty) Then
                Me._strIsExactDOB = strDOBtype
            Else
                'in case of empty DOB
                Me._strIsExactDOB = "D"
            End If

            Me._blnDOBInWordCase = False

        End If



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

#End Region

End Class