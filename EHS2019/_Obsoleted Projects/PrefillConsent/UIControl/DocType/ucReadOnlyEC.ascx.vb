Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel


Partial Public Class ucReadOnlyEC
    Inherits System.Web.UI.UserControl

    Private _isVertical As Boolean
    Private _showAccountRefNo As Boolean
    Private _showAccountCreationDate As Boolean
    Private _maskIdentityNumber As Boolean
    Private _udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
    Private _showTempAccountNotice As Boolean
    Private _highLightDocType As Boolean
    Private _mode As ucInputDocTypeBase.BuildMode = ucInputDocTypeBase.BuildMode.Creation
    Private _intWidth As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.RenderLanguage()
        Me.Setup(Me._mode)
    End Sub

    Public Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
        Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
        Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
        Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim strGender As String
        Dim strDocumentTypeFullName As String

        If Me._udtEHSAccountPersonalInfo.Gender = "M" Then
            strGender = "GenderMale"
        Else
            strGender = "GenderFemale"
        End If
        'Select docuemnt Type 
        If udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocNameChi
        Else
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocName
        End If


        If IsVertical Then
            Me.panReadonlyVerticalEC.Visible = True
            Me.panReadonlyHorizontalEC.Visible = False

            'Show Refence No 
            If Me._showAccountRefNo Then
                Me.lblReadonlyReferenceNo.Text = formatter.formatSystemNumber(Me._udtEHSAccountPersonalInfo.VoucherAccID)
                Me.pnlReadonlyTempAccountRefNo.Visible = True
            Else
                Me.pnlReadonlyTempAccountRefNo.Visible = False
            End If

            'Show Account Creation date
            If Me._showAccountCreationDate Then
                Me.lblReadonlyCreationDateTime.Text = formatter.convertDateTime(Me._udtEHSAccountPersonalInfo.CreateDtm)
                Me.panReadonlyCreationDatetime.Visible = True
                Me.SetupTableTitle(230)
            Else
                Me.panReadonlyCreationDatetime.Visible = False
                Me.SetupTableTitle(150)
            End If

            'Show tempoary ehealth account notice panel 
            If Me._showTempAccountNotice Then
                Me.panReadonlyTempAccountNotice.Visible = True
            Else
                Me.panReadonlyTempAccountNotice.Visible = False
            End If

            Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName
            Me.lblReadonlyECSerialNo.Text = Me._udtEHSAccountPersonalInfo.ECSerialNo
            Me.lblReadonlyECReferenceNo.Text = formatter.formatReferenceNo(Me._udtEHSAccountPersonalInfo.ECReferenceNo, False)
            Me.lblReadonlyECDate.Text = formatter.formatECDOI(Me._udtEHSAccountPersonalInfo.DateofIssue)
            Me.lblReadonlyEName.Text = formatter.formatEnglishName(Me._udtEHSAccountPersonalInfo.ENameSurName, Me._udtEHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyCName.Text = formatter.formatChineseName(Me._udtEHSAccountPersonalInfo.CName)
            Me.lblReadonlyECHKID.Text = formatter.formatHKID(Me._udtEHSAccountPersonalInfo.IdentityNum, True)
            Me.lblReadonlyECHKIDModification.Text = formatter.formatHKID(Me._udtEHSAccountPersonalInfo.IdentityNum, True)
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDOB.Text = formatter.formatDOB(Me._udtEHSAccountPersonalInfo.DOB, Me._udtEHSAccountPersonalInfo.ExactDOB, Session("language"), Me._udtEHSAccountPersonalInfo.ECAge, Me._udtEHSAccountPersonalInfo.ECDateOfRegistration)
        Else
            Me.panReadonlyVerticalEC.Visible = False
            Me.panReadonlyHorizontalEC.Visible = True

            Me.lblReadonlyHorizontalDocumentType.Text = strDocumentTypeFullName
            Me.lblReadonlyHorizontalECSerialNo.Text = Me._udtEHSAccountPersonalInfo.ECSerialNo
            Me.lblReadonlyHorizontalECReferenceNo.Text = formatter.formatReferenceNo(Me._udtEHSAccountPersonalInfo.ECReferenceNo, False)
            Me.lblReadonlyHorizontalECDate.Text = formatter.formatECDOI(Me._udtEHSAccountPersonalInfo.DateofIssue)
            Me.lblReadonlyHorizontalEName.Text = formatter.formatEnglishName(Me._udtEHSAccountPersonalInfo.ENameSurName, Me._udtEHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyHorizontalCName.Text = formatter.formatChineseName(Me._udtEHSAccountPersonalInfo.CName)
            Me.lblReadonlyHorizontalECHKID.Text = formatter.formatHKID(Me._udtEHSAccountPersonalInfo.IdentityNum, True)
            Me.lblReadonlyHorizontalGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyHorizontalDOB.Text = formatter.formatDOB(Me._udtEHSAccountPersonalInfo.DOB, Me._udtEHSAccountPersonalInfo.ExactDOB, Session("language"), Me._udtEHSAccountPersonalInfo.ECAge, Me._udtEHSAccountPersonalInfo.ECDateOfRegistration)
        End If

        'Mode related setting
        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me.trECHKIDCreation.Visible = True
            Me.trECHKIDModication.Visible = False
        Else
            Me.trECHKIDCreation.Visible = False
            Me.trECHKIDModication.Visible = True
        End If

        ' Control the width of the table
        tblEC.Rows(0).Cells(0).Width = _intWidth

    End Sub

    Private Sub RenderLanguage()
        If Me._isVertical Then
            Me.imgReadonlyECHolder.AlternateText = Me.GetGlobalResourceObject("Text", "ExemptionCert")
            Me.lblReadonlyECHolder.Text = Me.GetGlobalResourceObject("Text", "ExemptionCertHolder")

            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")

            Me.lblReadonlyECSerialNoText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
            Me.lblReadonlyECReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
            Me.lblReadonlyECDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")

            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
            Me.lblReadonlyECHKIDModificationText.Text = Me.GetGlobalResourceObject("Text", "HKID")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Else
            Me.lblReadonlyHorizontalDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyHorizontalNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "DOBLongGender")
            Me.lblReadonlyHorizontalECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
            Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")

            Me.lblReadonlyHorizontalECSerialNoText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
            Me.lblReadonlyHorizontalECReferenceNoText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
            Me.lblReadonlyHorizontalECDateText.Text = Me.GetGlobalResourceObject("Text", "ECDate")
        End If
    End Sub

    Private Sub SetupTableTitle(ByVal width As Integer)
        Me.lblReadonlyRefenceText.Width = width
        Me.lblReadonlyCreationDateTimeText.Width = width
        Me.lblReadonlyDocumentTypeText.Width = width

        Me.lblReadonlyDocumentTypeText.Width = width
        Me.lblReadonlyECSerialNoText.Width = width
        Me.lblReadonlyECReferenceNoText.Width = width

        Me.lblReadonlyNameText.Width = width
        Me.lblReadonlyECHKIDText.Width = width
        Me.lblReadonlyDOBText.Width = width
        Me.lblReadonlyGender.Width = width
    End Sub

#Region "Property"

    Public Property IsVertical() As Boolean
        Get
            Return Me._isVertical
        End Get
        Set(ByVal value As Boolean)
            Me._isVertical = value
        End Set
    End Property

    Public Property ShowAccountRefNo() As Boolean
        Get
            Return Me._showAccountRefNo
        End Get
        Set(ByVal value As Boolean)
            Me._showAccountRefNo = value
        End Set
    End Property

    Public Property ShowAccountCreationDate() As Boolean
        Get
            Return Me._showAccountCreationDate
        End Get
        Set(ByVal value As Boolean)
            Me._showAccountCreationDate = value
        End Set
    End Property

    Public Property ShowTempAccountNotice() As Boolean
        Get
            Return Me._showTempAccountNotice
        End Get
        Set(ByVal value As Boolean)
            Me._showTempAccountNotice = value
        End Set
    End Property

    Public Property MaskIdentityNumber() As Boolean
        Get
            Return Me._maskIdentityNumber
        End Get
        Set(ByVal value As Boolean)
            Me._maskIdentityNumber = value
        End Set
    End Property

    Public Property EHSAccountPersonalInfo() As EHSAccountModel.EHSPersonalInformationModel
        Get
            Return Me._udtEHSAccountPersonalInfo
        End Get
        Set(ByVal value As EHSAccountModel.EHSPersonalInformationModel)
            Me._udtEHSAccountPersonalInfo = value
        End Set
    End Property

    Public Property HightLightDocType() As Boolean
        Get
            Return Me._highLightDocType
        End Get
        Set(ByVal value As Boolean)
            Me._highLightDocType = value
        End Set
    End Property

    Public Property Mode() As ucInputDocTypeBase.BuildMode
        Get
            Return Me._mode
        End Get
        Set(ByVal value As ucInputDocTypeBase.BuildMode)
            _mode = value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return Me._intWidth
        End Get
        Set(ByVal value As Integer)
            _intWidth = value
        End Set
    End Property

#End Region

End Class