Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.DocType


Namespace UIControl.DocTypeText

    Partial Public Class ucInputEC
        Inherits ucInputDocTypeBase

        Private _strSerialNumber As String
        Private _strECReference As String
        Private _strENameFirstName As String
        Private _strENameSurName As String
        Private _strCName As String
        Private _strGender As String
        Private _strHKID As String
        Private _strDOB As String
        Private _strIsExactDOB As String
        Private _strECAge As String

        'For DOB Age
        Private _strECAgeDORDay As String = String.Empty
        Private _strECAgeDORMonth As String = String.Empty
        Private _strECAgeDORYear As String = String.Empty

        Private _strECDateOfRegistration As String
        Private _blnDOBTypeSelected As Boolean
        Private _strReferenceNo As String = String.Empty
        Private _strTransNo As String = String.Empty

        'For DOI 
        Private _strECDateDay As String
        Private _strECDateMonth As String
        Private _strECDateYear As String

        Private _blnSerialNumberNotProvided As Boolean ' Indicate whether the Serial No. is not provided [True|False]
        Private _blnReferenceNoOtherFormat As Boolean ' Indicate whether the Reference is in free format [True|False]

        Private commfunct As GeneralFunction = New GeneralFunction()
        Private udtFormatter As New Formatter
        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

#Region "Session"

        Private Class VS
            Public Const SerialNumberNotProvided As String = "ucInputEC_SerialNumberNotProvided"
            Public Const ReferenceNoOtherFormat As String = "ucInputEC_ReferenceNoOtherFormat"
        End Class

#End Region

        Protected Overrides Sub Setup(ByVal mode As BuildMode)
            Dim selectedMonth As String = Me.ddlECDateMonth.SelectedValue
            Dim gender As String = Me.rbECGender.SelectedValue
            MyBase.Mode = mode

            ' Setup Page variable
            Me._strHKID = udtFormatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty), False)
            Me._blnDOBTypeSelected = MyBase.EHSPersonalInfo.DOBTypeSelected
            Me._strDOB = udtFormatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, MyBase.SessionHandler.Language(), CInt(Me._strECAge), MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfo.ExactDOB

            If MyBase.EHSPersonalInfo.ECAge.HasValue Then
                Me._strECAge = MyBase.EHSPersonalInfo.ECAge.Value.ToString()
                Me._strECDateOfRegistration = udtFormatter.formatECDORegistration(MyBase.SessionHandler.Language(), MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Else
                Me._strECAge = -1
            End If

            'DOB age (Date of Registration)
            If MyBase.EHSPersonalInfo.ECDateOfRegistration.HasValue Then
                Me._strECAgeDORDay = MyBase.EHSPersonalInfo.ECDateOfRegistration.Value.Day
                Me._strECAgeDORMonth = MyBase.EHSPersonalInfo.ECDateOfRegistration.Value.ToString("MM")
                Me._strECAgeDORYear = MyBase.EHSPersonalInfo.ECDateOfRegistration.Value.Year
            End If

            ' EC Date show English Month in all language
            Me.ddlECDateMonth.DataSource = commfunct.GetMonthSelection(Common.Component.CultureLanguage.English)
            Me.ddlECDateMonth.DataValueField = "Value"
            Me.ddlECDateMonth.DataTextField = "Display"
            Me.ddlECDateMonth.DataBind()

            Me.SetupHKID()
            Me.SetupDOB()
            Me.SetupReferenceNo()

            Me.ddlECDateMonth.SelectedValue = selectedMonth
            'Me.rbECGender.SelectedValue = gender

            If FillValue(MyBase.EHSPersonalInfo) AndAlso MyBase.ActiveViewChanged Then
                If MyBase.EHSPersonalInfo.ECSerialNoNotProvided AndAlso IsNothing(ViewState(VS.SerialNumberNotProvided)) Then ViewState(VS.SerialNumberNotProvided) = "Y"
                Me._strSerialNumber = MyBase.EHSPersonalInfo.ECSerialNo

                If MyBase.EHSPersonalInfo.ECReferenceNoOtherFormat AndAlso IsNothing(ViewState(VS.ReferenceNoOtherFormat)) Then ViewState(VS.ReferenceNoOtherFormat) = "Y"
                Me._strECReference = MyBase.EHSPersonalInfo.ECReferenceNo

                Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
                Me._strGender = MyBase.EHSPersonalInfo.Gender

                Me._strECDateDay = MyBase.EHSPersonalInfo.DateofIssue.Value.Day
                Me._strECDateMonth = MyBase.EHSPersonalInfo.DateofIssue.Value.ToString("MM")
                Me._strECDateYear = MyBase.EHSPersonalInfo.DateofIssue.Value.Year

                If Me._blnDOBTypeSelected Then
                    Me.SetupDOBType()
                End If

                Me.SetupSerialNumber()
                Me.SetupECReferenceNo()
                Me.SetupENameFirstName()
                Me.SetupENameSurName()
                Me.SetupECDate()
                Me.SetupGender()

                Me.SetErrorImage(BuildMode.Creation, False)
            End If

            'If MyBase.ActiveViewChanged Then
            '    'Setup Error
            '    Me.SetECSerialNoError(False)
            '    Me.SetECReferenceError(False)
            '    Me.SetECDateError(False)
            '    Me.SetSurnameError(False)
            '    Me.SetGivenNameError(False)
            '    Me.SetGenderError(False)
            '    Me.SetDOBTypeError(False)
            'End If

            'Me.SetupFieldsStatus(mode)

        End Sub

        Private Function FillValue(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            If String.IsNullOrEmpty(udtEHSPersonalInfo.ECSerialNo) Then
                Return False
            End If

            If String.IsNullOrEmpty(udtEHSPersonalInfo.ECReferenceNo) Then
                Return False
            End If

            If Not udtEHSPersonalInfo.DateofIssue.HasValue Then
                Return False
            End If

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

        Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
            Dim selectedDOBType As String = Me.rbDOBType.SelectedValue

            '--------------------------------------------------- Creation ----------------------------------------------
            Dim errorButtonImageURL As String = Me.GetGlobalResourceObject("ImageUrl", "ErrorBtn")
            Dim errorButtonImageALT As String = Me.GetGlobalResourceObject("AlternateText", "ErrorBtn")
            'Table title

            ' Reference No
            Me.lblReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "RefNo")

            ' Serial No.
            Me.lblECSerialNo.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
            Me.cboECSerialNoNotProvided.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            'Me.lblECSerialNoHints.Text = Me.GetGlobalResourceObject("Text", "ECSerialNoHint")

            ' Reference
            Me.lblECReference.Text = Me.GetGlobalResourceObject("Text", "ECReference")
            Me.btnOtherFormat.Text = Me.GetGlobalResourceObject("Text", "OtherFormats")
            Me.btnSpecificFormat.Text = Me.GetGlobalResourceObject("Text", "SpecificFormat")
            'Me.lblECReferenceHints.Text = Me.GetGlobalResourceObject("Text", "ECReferenceHint")

            ' Date of Issue
            Me.lblECDate.Text = Me.GetGlobalResourceObject("Text", "ECDate")
            'Me.lblECIssueDateHints.Text = Me.GetGlobalResourceObject("Text", "ECIssueDateHint")

            ' Name in English
            Me.lblEName.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            Me.lblSurname.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
            Me.lblGivenName.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

            ' Gender
            Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.rbECGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbECGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            ' HKIC No.
            ' Me.lblECHKID.Text = Me.GetGlobalResourceObject("Text", "HKID")
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.EC)
            lblECHKID.Text = udtDocTypeModel.DocIdentityDesc(SessionHandler().Language)

            ' Date of Birth
            Me.lblECDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")

            ' Age
            Me.lblAge.Text = Me.GetGlobalResourceObject("Text", "Age")
            Me.lblRegisterOn.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")

            ' Date of Birth Type
            Me.lblDOBType.Text = Me.GetGlobalResourceObject("Text", "DOBType")

            Me.BindDOBType(MyBase.EHSPersonalInfo.ExactDOB)

            If selectedDOBType <> "" Then
                Me.rbDOBType.SelectedValue = selectedDOBType
            End If

        End Sub

        Private Sub BindDOBType(ByVal strExactDOB As String)
            Me.rbDOBType.Items.Clear()
            Me.rbDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "DOBReport"), "D"))

            If strExactDOB = "Y" Or strExactDOB = "V" Or strExactDOB = "R" Then
                Me.rbDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "YOB"), "Y"))
            End If

            Me.rbDOBType.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "DOBTravel"), "T"))
        End Sub

        Private Sub SetupFieldsStatus(ByVal modeType As ucInputDocTypeBase.BuildMode)
            Me.SetupHKID()
            Me.SetupDOB()
            Me.SetupReferenceNo()

            Me.SetupSerialNumber()
            Me.SetupECReferenceNo()
            Me.SetupENameFirstName()
            Me.SetupENameSurName()
            Me.SetupECDate()
            Me.SetupGender()

            'Setup Error
            Me.SetECSerialNoError(False)
            Me.SetECReferenceError(False)
            Me.SetECDateError(False)
            Me.SetSurnameError(False)
            Me.SetGivenNameError(False)
            Me.SetGenderError(False)
            Me.SetDOBTypeError(False)

        End Sub

#Region "Set Up Text Box Value  (Creation Mode)"

        Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
            Me.SetValue()
        End Sub

        Public Overloads Sub SetValue()
            Me.SetupHKID()
            Me.SetupDOB()

            Me.SetupSerialNumber()
            Me.SetupECReferenceNo()
            Me.SetupENameFirstName()
            Me.SetupECDate()
            Me.SetupGender()

        End Sub

        Public Sub SetupSerialNumber()
            Me.txtECSerialNo.Text = Me._strSerialNumber

            If ViewState(VS.SerialNumberNotProvided) = "Y" Then
                cboECSerialNoNotProvided.Checked = True
                EnableSerialNo(False)
            Else
                EnableSerialNo(True)
            End If
        End Sub

        Public Sub SetupECReferenceNo()
            If ViewState(VS.ReferenceNoOtherFormat) = "Y" Then
                EnableReferenceOtherFormat(True)
                txtECRefFree.Text = _strECReference

            Else
                EnableReferenceOtherFormat(False)
                If Not IsNothing(_strECReference) AndAlso _strECReference.Length = 14 Then
                    txtECRefence1.Text = _strECReference.Substring(0, 4)
                    txtECRefence2.Text = _strECReference.Substring(4, 7)
                    txtECRefence3.Text = _strECReference.Substring(11, 2)
                    txtECRefence4.Text = _strECReference.Substring(13, 1)

                End If

            End If

        End Sub

        Public Sub SetupENameFirstName()
            Me.txtENameFirstname.Text = Me._strENameFirstName
        End Sub

        Public Sub SetupENameSurName()
            Me.txtENameSurname.Text = Me._strENameSurName
        End Sub

        Public Sub SetupECDate()
            Me.txtECDateDay.Text = Me._strECDateDay
            Me.ddlECDateMonth.SelectedValue = Me._strECDateMonth
            Me.txtECDateYear.Text = Me._strECDateYear
        End Sub

        Public Sub SetupGender()
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rbECGender.SelectedValue = Me._strGender
            End If
        End Sub

        Public Sub SetupHKID()
            Me.txtECHKIC.Text = Me._strHKID
        End Sub

        Public Sub SetupDOB()

            If CInt(Me._strECAge) < 0 Then
                Me.txtECDOB.Text = Me._strDOB
                Me.txtECDOB.Visible = True
                Me.panECDOA.Visible = False
                Me.panECDOBType.Visible = True

            Else
                Me.txtECAge.Text = Me._strECAge
                Me.txtECDOAge.Text = Me._strECDateOfRegistration
                Me.txtECDOB.Visible = False
                Me.panECDOA.Visible = True
                Me.panECDOBType.Visible = False
            End If
        End Sub

        Public Sub SetupDOBType()
            If Me._strIsExactDOB = "R" Then Me.rbDOBType.SelectedValue = "Y"
            If Me._strIsExactDOB = "Y" Or Me._strIsExactDOB = "M" Or Me._strIsExactDOB = "D" Then Me.rbDOBType.SelectedValue = "D"
            If Me._strIsExactDOB = "T" Or Me._strIsExactDOB = "U" Or Me._strIsExactDOB = "V" Then Me.rbDOBType.SelectedValue = "T"
        End Sub

        Public Sub SetupReferenceNo()
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

#Region "Set Up Error Image (Creation Mode)"

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Error Image
        '--------------------------------------------------------------------------------------------------------------
        Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
            Me.SetErrorImage(visible)
        End Sub

        Public Overloads Sub SetErrorImage(ByVal visible As Boolean)
            Me.SetECSerialNoError(visible)
            Me.SetECReferenceError(visible)
            Me.SetECDateError(visible)
            Me.SetSurnameError(visible)
            Me.SetGivenNameError(visible)
            Me.SetGenderError(visible)
            Me.SetDOBTypeError(visible)
        End Sub

        Public Sub SetECSerialNoError(ByVal visible As Boolean)
            Me.lblECSerialNoError.Visible = visible
        End Sub

        Public Sub SetECReferenceError(ByVal visible As Boolean)
            Me.lblECReferenceError.Visible = visible
        End Sub

        Public Sub SetECDateError(ByVal visible As Boolean)
            Me.lblECDateError.Visible = visible
        End Sub

        Public Sub SetSurnameError(ByVal visible As Boolean)
            Me.lblSurNameError.Visible = visible
        End Sub

        Public Sub SetGivenNameError(ByVal visible As Boolean)
            Me.lblGivenNameError.Visible = visible
        End Sub

        Public Sub SetGenderError(ByVal visible As Boolean)
            Me.lblGenderError.Visible = visible
        End Sub

        Public Sub SetDOBTypeError(ByVal visible As Boolean)
            Me.lblECDOBTypeError.Visible = visible
        End Sub

#End Region

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            MyBase.Mode = mode
            Me.SetPropertyCreation()
        End Sub

        Public Sub SetPropertyCreation()
            Dim dtDOB As DateTime
            Dim strDOBtype As String = String.Empty

            Me._strECDateDay = Me.txtECDateDay.Text.Trim()
            Me._strECDateMonth = Me.ddlECDateMonth.SelectedValue.Trim()
            Me._strECDateYear = Me.txtECDateYear.Text.Trim()

            ' Serial No.
            _blnSerialNumberNotProvided = cboECSerialNoNotProvided.Checked

            If _blnSerialNumberNotProvided Then
                _strSerialNumber = String.Empty
            Else
                _strSerialNumber = txtECSerialNo.Text.Trim.ToUpper
            End If

            ' Reference Free Format
            _blnReferenceNoOtherFormat = ViewState(VS.ReferenceNoOtherFormat) = "Y"

            ' Reference
            If _blnReferenceNoOtherFormat = True Then
                _strECReference = txtECRefFree.Text.Trim.ToUpper
            Else
                _strECReference = String.Format("{0}{1}{2}{3}", Me.txtECRefence1.Text.Trim(), Me.txtECRefence2.Text.Trim(), Me.txtECRefence3.Text.Trim(), Me.txtECRefence4.Text.Trim()).ToUpper
            End If

            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim.ToUpper
            Me._strENameSurName = Me.txtENameSurname.Text.Trim.ToUpper
            Me._strGender = Me.rbECGender.SelectedValue.Trim()
            Me._strHKID = Me.txtECHKIC.Text.Trim.ToUpper
            Me._strDOB = Me.txtECDOB.Text.Trim()
            Me._strECAge = Me.txtECAge.Text.Trim()
            'Me._strECDateOfRegistration = Me.txtECDOAge.Text.Trim()

            If Me.rbDOBType.SelectedValue.Trim().ToUpper() = "D" Then
                commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
                Me._strIsExactDOB = strDOBtype
            ElseIf Me.rbDOBType.SelectedValue.Trim().ToUpper() = "Y" Then
                commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, False)
                Me._strIsExactDOB = "R"
            ElseIf Me.rbDOBType.SelectedValue.ToString() = "T" Then
                commfunct.chkDOBtype(Me._strDOB, dtDOB, strDOBtype, True)
                Me._strIsExactDOB = strDOBtype
            Else
                Me._strIsExactDOB = String.Empty
            End If
        End Sub

        Public Property SerialNumber() As String
            Get
                Return Me._strSerialNumber
            End Get
            Set(ByVal value As String)
                Me._strSerialNumber = value
            End Set
        End Property

        Public Property SerialNumberNotProvided() As Boolean
            Get
                Return _blnSerialNumberNotProvided
            End Get
            Set(ByVal value As Boolean)
                _blnSerialNumberNotProvided = value
            End Set
        End Property

        Public Property Reference() As String
            Get
                Return Me._strECReference
            End Get
            Set(ByVal value As String)
                Me._strECReference = value
            End Set
        End Property

        Public Property ReferenceOtherFormat() As Boolean
            Get
                Return _blnReferenceNoOtherFormat
            End Get
            Set(ByVal value As Boolean)
                _blnReferenceNoOtherFormat = value
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

        Public Property HKID() As String
            Get
                Return Me._strHKID
            End Get
            Set(ByVal value As String)
                Me._strHKID = value
            End Set
        End Property

        'DOB----------------------------------------------------------
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

        'DOB - Age - Date of Registration 
        Public Property ECDateOfRegDay() As String
            Get
                Return Me._strECAgeDORDay
            End Get
            Set(ByVal value As String)
                Me._strECAgeDORDay = value
            End Set
        End Property

        Public Property ECDateOfRegMonth() As String
            Get
                Return Me._strECAgeDORMonth
            End Get
            Set(ByVal value As String)
                Me._strECAgeDORMonth = value
            End Set
        End Property

        Public Property ECDateOfRegYear() As String
            Get
                Return Me._strECAgeDORYear
            End Get
            Set(ByVal value As String)
                Me._strECAgeDORYear = value
            End Set
        End Property

        Public Property ECDateOfRegistration() As String
            Get
                Return Me._strECDateOfRegistration
            End Get
            Set(ByVal value As String)
                Me._strECDateOfRegistration = value
            End Set
        End Property

        Public Property ECAge() As String
            Get
                Return Me._strECAge
            End Get
            Set(ByVal value As String)
                Me._strECAge = value
            End Set
        End Property

        'DOI----------------------------------------------------------
        Public Property ECDateDay() As String
            Get
                Return Me._strECDateDay
            End Get
            Set(ByVal value As String)
                Me._strECDateDay = value
            End Set
        End Property

        Public Property ECDateMonth() As String
            Get
                Return Me._strECDateMonth
            End Get
            Set(ByVal value As String)
                Me._strECDateMonth = value
            End Set
        End Property

        Public Property ECDateYear() As String
            Get
                Return Me._strECDateYear
            End Get
            Set(ByVal value As String)
                Me._strECDateYear = value
            End Set
        End Property
        '-------------------------------------------------------------

        Public Property ReferenceNo() As String
            Get
                Return Me._strReferenceNo
            End Get
            Set(ByVal value As String)
                Me._strReferenceNo = value
            End Set
        End Property

#End Region

#Region "Event"

        Protected Sub cboECSerialNoNotProvided_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            MyBase.AuditLogEntry.AddDescripton("Previous Serial No.", txtECSerialNo.Text)
            MyBase.AuditLogEntry.AddDescripton("Checked after", IIf(cboECSerialNoNotProvided.Checked, "Y", "N"))
            MyBase.AuditLogEntry.WriteLog(LogID.LOG00068, "Serial No. Not Provided checked")

            If cboECSerialNoNotProvided.Checked Then
                ViewState(VS.SerialNumberNotProvided) = "Y"
            Else
                ViewState(VS.SerialNumberNotProvided) = "N"
            End If

            EnableSerialNo(Not cboECSerialNoNotProvided.Checked)
        End Sub

        Private Sub EnableSerialNo(ByVal blnEnable As Boolean)
            If blnEnable = False Then
                txtECSerialNo.Enabled = False
                txtECSerialNo.Text = String.Empty
                txtECSerialNo.BackColor = Drawing.Color.LightGray
            Else
                txtECSerialNo.Enabled = True
                txtECSerialNo.BackColor = Nothing
            End If

        End Sub

        Protected Sub btnOtherFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            MyBase.AuditLogEntry.AddDescripton("Previous Reference 1", txtECRefence1.Text)
            MyBase.AuditLogEntry.AddDescripton("Previous Reference 2", txtECRefence2.Text)
            MyBase.AuditLogEntry.AddDescripton("Previous Reference 3", txtECRefence3.Text)
            MyBase.AuditLogEntry.AddDescripton("Previous Reference 4", txtECRefence4.Text)
            MyBase.AuditLogEntry.WriteLog(LogID.LOG00069, "Reference Other Formats clicked")

            EnableReferenceOtherFormat(True)
            ViewState(VS.ReferenceNoOtherFormat) = "Y"
        End Sub

        Protected Sub btnSpecificFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            MyBase.AuditLogEntry.AddDescripton("Previous Reference", txtECRefFree.Text)
            MyBase.AuditLogEntry.WriteLog(LogID.LOG00070, "Reference Specific Format clicked")

            EnableReferenceOtherFormat(False)
            ViewState(VS.ReferenceNoOtherFormat) = "N"
        End Sub

        Private Sub EnableReferenceOtherFormat(ByVal blnOtherFormat As Boolean)
            txtECRefence1.Visible = Not blnOtherFormat
            txtECRefence2.Visible = Not blnOtherFormat
            txtECRefence3.Visible = Not blnOtherFormat
            txtECRefence4.Visible = Not blnOtherFormat
            lblReferenceSep1.Visible = Not blnOtherFormat
            lblReferenceSep2.Visible = Not blnOtherFormat
            lblReferenceSep3.Visible = Not blnOtherFormat
            lblReferenceSep4.Visible = Not blnOtherFormat

            txtECRefFree.Visible = blnOtherFormat

            btnOtherFormat.Visible = Not blnOtherFormat
            btnSpecificFormat.Visible = blnOtherFormat

            btnOtherFormat.Text = Me.GetGlobalResourceObject("Text", "OtherFormats")
            btnSpecificFormat.Text = Me.GetGlobalResourceObject("Text", "SpecificFormat")

            ' Clear the text
            txtECRefence1.Text = String.Empty
            txtECRefence2.Text = String.Empty
            txtECRefence3.Text = String.Empty
            txtECRefence4.Text = String.Empty
            txtECRefFree.Text = String.Empty
        End Sub

#End Region

#Region "EC Search Setup"
        Private Sub BindECDate(ByVal ddlECDate As DropDownList, ByVal strLanguage As String)
            Dim strECDateMonth As String

            strECDateMonth = ddlECDate.SelectedValue()
            ddlECDate.DataSource = Me.commfunct.GetMonthSelection(strLanguage)
            ddlECDate.DataValueField = "Value"
            ddlECDate.DataTextField = "Display"
            ddlECDate.DataBind()

            If Not strECDateMonth Is Nothing Then
                ddlECDate.SelectedValue = strECDateMonth
            End If
        End Sub
#End Region

    End Class

End Namespace