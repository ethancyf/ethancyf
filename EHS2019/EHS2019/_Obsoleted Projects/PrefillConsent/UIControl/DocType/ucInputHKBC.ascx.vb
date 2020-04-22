Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports System.Web.UI.ScriptManager

Partial Public Class ucInputHKBC
    Inherits ucInputDocTypeBase

    Private _strRegistrationNo As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOBInWord As String
    Private _strIsExactDOB As String
    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty
    Private _blnDOBTypeSelected As Boolean
    Private _blnDOBInWordCase As Boolean

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)


        'Table title
        Me.lblRegistrationNoText.Text = Me.GetGlobalResourceObject("Text", "BCRegNo")
        Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.rbDOBInWord.Text = Me.GetGlobalResourceObject("Text", "DOBInWordShort")

        Me.lblReferenceNoText_M.Text = Me.GetGlobalResourceObject("Text", "RefNo")
        Me.lblTransactionNoText_M.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

        'Gender Radio button list
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'Error Image
        Me.imgRegNoError.ImageUrl = strErrorImageURL
        Me.imgRegNoError.AlternateText = strErrorImageALT

        Me.imgENameError.ImageUrl = strErrorImageURL
        Me.imgENameError.AlternateText = strErrorImageALT

        Me.imgGenderError.ImageUrl = strErrorImageURL
        Me.imgGenderError.AlternateText = strErrorImageALT

        Me.imgDOBInWordError.ImageUrl = strErrorImageURL
        Me.imgDOBInWordError.AlternateText = strErrorImageALT

        Me.imgDOBError.ImageUrl = strErrorImageURL
        Me.imgDOBError.AlternateText = strErrorImageALT

        'Tips
        Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
        Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))
        'Me.txtENameTips.Text = Me.GetGlobalResourceObject("Text", "EnameHint")
        'Me.lblDOBTip.Text = Me.GetGlobalResourceObject("Text", "DOBHintHKBC")
        ''Me.lblRegTip.Text = Me.GetGlobalResourceObject("Text", "RegNoHint")
        'Me.lblRegTip.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        If Not MyBase.EHSPersonalInfo Is Nothing Then
            Me._strRegistrationNo = Formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum, False)
            Me._strDOB = Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me._blnDOBTypeSelected = MyBase.EHSPersonalInfo.DOBTypeSelected
            Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB
            Me._strDOBInWord = MyBase.EHSPersonalInfo.OtherInfo

            Me.SetRegistrationNo()
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
        End If

        'Handle Modification Mode and Modification Read-Only Mode
        If Not modeType = ucInputDocTypeBase.BuildMode.Creation Then
            'Fill Data
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender

            Me.SetEName()
            Me.SetGender()

            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                Me.lblRegistrationNo.Text = Me.txtRegistrationNo.Text
                Me.lblRegistrationNo.Visible = True
                Me.txtRegistrationNo.Visible = False

                'Me.txtRegistrationNo.Enabled = False
                Me.rbDOB.Enabled = True
                Me.rbDOBInWord.Enabled = True
                Me.ddlDOBinWordType.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtENameTips.Visible = False
            Else
                Me.txtRegistrationNo.Enabled = False
                Me.rbDOB.Enabled = False
                Me.rbDOBInWord.Enabled = False
                Me.ddlDOBinWordType.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.txtENameSurname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtENameTips.Visible = False
            End If
        Else
            Me.txtRegistrationNo.Enabled = True
            Me.rbDOB.Enabled = True
            Me.rbDOBInWord.Enabled = True
            Me.ddlDOBinWordType.Enabled = True
            Me.txtENameFirstname.Enabled = True
            Me.txtENameSurname.Enabled = True
            Me.rbGender.Enabled = True
        End If
        Me.SetReferenceNo()
        Me.SetTransactionNo()

        'Set DOB In word Drop Down list   
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)

        Me.SetRegNoError(False)
        Me.SetENameError(False)
        Me.SetGenderError(False)
        Me.SetDOBTypeError(False)
        Me.SetDOBError(False)

        'Page.SetFocus(txtENameSurname)

    End Sub

    Private Sub DOBInWordOption(ByVal enable As Boolean)
        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL()
        Dim dataTable As DataTable
        Dim dtDOBinWorType As DataTable = New DataTable
        Dim dataRow As DataRow

        If enable Then
            'Me._strDOBInWord = Me.ddlDOBinWordType.SelectedValue

            dataTable = udtStaticDataBLL.GetStaticDataList("DOBInWordType")

            ' in readonly only.  The drop down list is disabled
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.ddlDOBinWordType.Enabled = True
                'Me.ddlDOBinWordType.BackColor = Drawing.Color.White
            End If

            dataRow = dataTable.NewRow
            dataRow(StaticDataModel.Column_Name) = 0
            dataRow(StaticDataModel.Item_No) = String.Empty
            dataRow(StaticDataModel.Data_Value) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect")
            dataRow(StaticDataModel.Data_Value_Chi) = Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi")
            dataTable.Rows.InsertAt(dataRow, 0)

            Me.ddlDOBinWordType.DataSource = dataTable
            If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value_Chi
            Else
                Me.ddlDOBinWordType.DataTextField = StaticDataModel.Data_Value
            End If
            Me.ddlDOBinWordType.DataValueField = StaticDataModel.Item_No
            Me.ddlDOBinWordType.DataBind()

            Me.ddlDOBinWordType.SelectedValue = Me._strDOBInWord
        Else
            Me.ddlDOBinWordType.Enabled = False
            'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver

            Me.ddlDOBinWordType.Items.Clear()
            If MyBase.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                Me.ddlDOBinWordType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect_Chi"), String.Empty))
            Else
                Me.ddlDOBinWordType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ClaimPleaseSelect"), String.Empty))
            End If
        End If

    End Sub

    Private Sub ChangeDOBOption(ByVal enable As Boolean)
        Dim ScriptManager1 As ScriptManager = ScriptManager.GetCurrent(Page)
        If Not enable Then
            'Me.txtDOB.Enabled = True

            'Me.txtDOBInWord.Enabled = False

            Me.ddlDOBinWordType.Enabled = False
            'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver

            Me.txtDOBInWord.Text = String.Empty
            'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
            '    Me.txtDOBInWord.Text = String.Empty
            'End If
            ScriptManager1.SetFocus(txtDOB)
        Else
            'Me.txtDOB.Enabled = False

            'Me.txtDOBInWord.Enabled = True

            Me.ddlDOBinWordType.Enabled = True
            'Me.ddlDOBinWordType.BackColor = Drawing.Color.White

            Me.txtDOB.Text = String.Empty
            'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
            '    Me.txtDOB.Text = String.Empty
            'End If
            ScriptManager1.SetFocus(txtDOBInWord)
        End If
    End Sub

#Region "Events"

    Protected Sub rbDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDOB.CheckedChanged, rbDOBInWord.CheckedChanged
        'Update DOB Options
        Me.ChangeDOBOption(Me.rbDOBInWord.Checked)

        'Update Drop Down List (DOB in word)
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)

        'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
        '    Me.txtDOB.Enabled = False
        '    Me.txtDOBInWord.Enabled = False
        'End If

        'Page.SetFocus(txtDOB)

    End Sub

#End Region

#Region "Set Up Text Box Value"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        Me.SetRegistrationNo()
        Me.SetEName()
        Me.SetDOB()

        'Me.ddlDOBinWordType.SelectedValue = Me._strDOBInWord
        Me.DOBInWordOption(Me.rbDOBInWord.Checked)

        Me.SetGender()
        Me.SetReferenceNo()
        Me.SetTransactionNo()
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
        'If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
        '    Me.txtDOB.Text = Me._strDOB
        '    Me.txtDOBInWord.Text = Me._strDOB

        '    If Me._blnDOBTypeSelected Then
        '        Me.SetupDOBType()
        '    Else

        '    End If
        'Else
        '    Me.SetupDOBType()
        'End If

        Me.SetupDOBType()
    End Sub

    Public Sub SetGender()
        'Fill Data - Gender only
        Me.rbGender.SelectedValue = Me._strGender
    End Sub

    Public Sub SetupDOBType()
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.Creation Then
            'Creation Mode
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then

                If Not Me.rbDOBInWord.Checked Then
                    Me.rbDOB.Checked = True
                    Me.rbDOBInWord.Checked = False

                    'Me.txtDOB.Enabled = True
                    Me.rbDOBInWord.Checked = False
                    'Me.txtDOBInWord.Enabled = False
                    Me.ddlDOBinWordType.Enabled = False
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
                End If
                Me.txtDOB.Text = Me._strDOB
            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                If Not Me.rbDOB.Checked Then
                    Me.rbDOB.Checked = False
                    Me.rbDOBInWord.Checked = True

                    'Me.txtDOBInWord.Enabled = True
                    Me.ddlDOBinWordType.Enabled = True
                    Me.rbDOB.Checked = False
                    'Me.txtDOB.Enabled = False
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.White
                End If
                Me.txtDOBInWord.Text = Me._strDOB
                Me.ddlDOBinWordType.SelectedValue = Me._strDOBInWord
            Else
                Me.rbDOB.Checked = False
                Me.rbDOBInWord.Checked = False
            End If
        Else
            'Modification Mode
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then
                If Not Me.rbDOBInWord.Checked Then
                    Me.rbDOB.Checked = True
                    If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                        Me.txtDOB.Enabled = True
                    Else
                        Me.txtDOB.Enabled = False
                    End If
                    Me.rbDOBInWord.Checked = False
                    Me.txtDOBInWord.Enabled = False
                    Me.ddlDOBinWordType.Enabled = False
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.Silver
                End If
                Me.txtDOB.Text = Me._strDOB
            ElseIf Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then
                If Not Me.rbDOB.Checked Then
                    Me.rbDOBInWord.Checked = True
                    If MyBase.Mode = ucInputDocTypeBase.BuildMode.Modification Then
                        Me.txtDOBInWord.Enabled = True
                    Else
                        Me.txtDOBInWord.Enabled = False
                    End If
                    Me.ddlDOBinWordType.Enabled = True
                    Me.rbDOB.Checked = False
                    Me.txtDOB.Enabled = False
                    'Me.ddlDOBinWordType.BackColor = Drawing.Color.White
                End If
                Me.txtDOBInWord.Text = Me._strDOB
            Else
                Me.rbDOB.Checked = False
                Me.rbDOBInWord.Checked = False
            End If
        End If
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
        SetRegNoError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBTypeError(visible)
        Me.SetDOBError(visible)
    End Sub

    Public Sub SetRegNoError(ByVal visible As Boolean)
        Me.imgRegNoError.Visible = visible
    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        Me.imgENameError.Visible = visible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGenderError.Visible = visible
    End Sub

    Public Sub SetDOBTypeError(ByVal visible As Boolean)
        Me.imgDOBInWordError.Visible = visible
    End Sub

    Public Sub SetDOBError(ByVal visible As Boolean)
        Me.imgDOBError.Visible = visible
    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtDOB As DateTime
        Dim strDOBtype As String = String.Empty

        Me._strRegistrationNo = Me.txtRegistrationNo.Text
        Me._strENameFirstName = Me.txtENameFirstname.Text
        Me._strENameSurName = Me.txtENameSurname.Text
        Me._strGender = Me.rbGender.SelectedValue
        Me._strDOB = Me.txtDOB.Text

        Me._strDOBInWord = Me.ddlDOBinWordType.SelectedValue

        If Me.rbDOB.Checked Then
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
            If Not strDOBtype.Trim.Equals(String.Empty) Then
                Me._strIsExactDOB = strDOBtype
            Else
                'in case of empty DOB
                Me._strIsExactDOB = "D"
            End If
            Me._blnDOBInWordCase = False
        ElseIf Me.rbDOBInWord.Checked Then
            Me._strDOB = Me.txtDOBInWord.Text
            commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)
            If Not strDOBtype.Trim.Equals(String.Empty) Then
                Me._strIsExactDOB = strDOBtype
            Else
                'in case of empty DOB
                Me._strIsExactDOB = "T"
            End If
            Me._blnDOBInWordCase = True
        Else
            Me._strIsExactDOB = String.Empty
            Me._blnDOBTypeSelected = False
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

    Public Property DOBInWordCase() As Boolean
        Get
            Return Me._blnDOBInWordCase
        End Get
        Set(ByVal value As Boolean)
            Me._blnDOBInWordCase = value
        End Set
    End Property

    Public Property DOBInWord() As String
        Get
            Return Me._strDOBInWord
        End Get
        Set(ByVal value As String)
            Me._strDOBInWord = value
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