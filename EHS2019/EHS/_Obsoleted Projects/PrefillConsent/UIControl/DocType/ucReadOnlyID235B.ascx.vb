Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel

Partial Public Class ucReadOnlyID235B
    Inherits System.Web.UI.UserControl

    Private _isVertical As Boolean
    Private _showAccountRefNo As Boolean
    Private _showAccountCreationDate As Boolean
    Private _maskBirthEntryNumber As Boolean
    Private _udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
    Private _showTempAccountNotice As Boolean
    Private _highLightDocType As Boolean
    Private _intWidth As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.RenderLanguage()
        Me.Setup()
    End Sub

    Public Sub Setup()
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
        Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
        Dim udtDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL()
        Dim udtDocTypeModelList As DocType.DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim strGender As String
        Dim strDocumentTypeFullName As String
        'Select docuemnt Type 
        If udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocNameChi
        Else
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.ID235B).DocName
        End If

        'Static Fields
        If Me._udtEHSAccountPersonalInfo.Gender = "M" Then
            strGender = "GenderMale"
        Else
            strGender = "GenderFemale"
        End If

        If Me._isVertical Then
            Me.panReadonlyVerticalID235B.Visible = True
            Me.panReadonlyHorizontalID235B.Visible = False

            'Show Refence No 
            If Me._showAccountRefNo Then
                Me.lblReadonlyReferenceNo.Text = formatter.formatSystemNumber(Me._udtEHSAccountPersonalInfo.VoucherAccID)
                Me.panReadonlyTempAccountRefNo.Visible = True
            Else
                Me.panReadonlyTempAccountRefNo.Visible = False
            End If

            'Show Account Creation date
            If Me._showAccountCreationDate Then
                Me.lblReadonlyCreationDateTime.Text = formatter.convertDateTime(Me._udtEHSAccountPersonalInfo.CreateDtm)
                Me.panReadonlyCreationDatetime.Visible = True
                Me.SetupTableTitle(230)
            Else
                Me.panReadonlyCreationDatetime.Visible = False
                Me.SetupTableTitle(230)
            End If

            'Show tempoary ehealth account notice panel 
            If Me._showTempAccountNotice Then
                Me.panReadonlyTempAccountNotice.Visible = True
            Else
                Me.panReadonlyTempAccountNotice.Visible = False
            End If

            Me.lblReadonlyEName.Text = formatter.formatEnglishName(Me._udtEHSAccountPersonalInfo.ENameSurName, Me._udtEHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyBENo.Text = formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, Me._udtEHSAccountPersonalInfo.IdentityNum, Me._maskBirthEntryNumber)
            Me.lblReadonlyDOB.Text = formatter.formatDOB(Me._udtEHSAccountPersonalInfo.DOB, Me._udtEHSAccountPersonalInfo.ExactDOB, udtSessionHandler.Language(), Nothing, Nothing)
            Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            Me.lblReadonlyDocumentType.Text = strDocumentTypeFullName
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.lblReadonlyPmtRemain.Text = formatter.formatEnterDate(Me._udtEHSAccountPersonalInfo.PermitToRemainUntil)
            Me.lblReadonlyPmtRemain.Text = formatter.formatID235BPermittedToRemainUntil(Me._udtEHSAccountPersonalInfo.PermitToRemainUntil)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Else
            Me.panReadonlyVerticalID235B.Visible = False
            Me.panReadonlyHorizontalID235B.Visible = True

            Me.lblReadonlyHorizontalEName.Text = formatter.formatEnglishName(Me._udtEHSAccountPersonalInfo.ENameSurName, Me._udtEHSAccountPersonalInfo.ENameFirstName)
            Me.lblReadonlyHorizontalBENo.Text = formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, Me._udtEHSAccountPersonalInfo.IdentityNum, Me._maskBirthEntryNumber)
            Me.lblReadonlyHorizontalDOB.Text = formatter.formatDOB(Me._udtEHSAccountPersonalInfo.DOB, Me._udtEHSAccountPersonalInfo.ExactDOB, Session("language"), Nothing, Nothing)
            Me.lblReadonlyHorizontalDocumentType.Text = strDocumentTypeFullName
            Me.lblReadonlyHorizontalGender.Text = Me.GetGlobalResourceObject("Text", strGender)
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.lblReadonlyHorizontalPmtRemain.Text = formatter.formatEnterDate(Me._udtEHSAccountPersonalInfo.PermitToRemainUntil)
            Me.lblReadonlyHorizontalPmtRemain.Text = formatter.formatID235BPermittedToRemainUntil(Me._udtEHSAccountPersonalInfo.PermitToRemainUntil)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        End If

        ' Control the width of the table
        tblID235B.Rows(0).Cells(0).Width = _intWidth

    End Sub

    Private Sub RenderLanguage()
        If Me._isVertical Then
            Me.lblReadonlyRefenceText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            Me.lblReadonlyConfirmTempAcctText.Text = Me.GetGlobalResourceObject("Text", "TempVRAcctRecord")
            Me.lblReadonlyCreationDateTimeText.Text = Me.GetGlobalResourceObject("Text", "AccountCreateDate")
            Me.lblReadonlyDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
            Me.lblReadonlyGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.lblReadonlyBENoText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
            Me.lblReadonlyPmtRemainText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")
        Else
            Me.lblReadonlyHorizontalDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
            Me.lblReadonlyHorizontalNameText.Text = Me.GetGlobalResourceObject("Text", "Name")
            Me.lblReadonlyHorizontalDOBGenderText.Text = Me.GetGlobalResourceObject("Text", "DOBLongGender")
            Me.lblReadonlyHorizontalBENoText.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
            Me.lblReadonlyHorizontalPmtRemainText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")
        End If
    End Sub

    Private Sub SetupTableTitle(ByVal width As Integer)
        Me.lblReadonlyRefenceText.Width = width
        Me.lblReadonlyCreationDateTimeText.Width = width
        Me.lblReadonlyDocumentTypeText.Width = width
        Me.lblReadonlyNameText.Width = width
        Me.lblReadonlyDOBText.Width = width
        Me.lblReadonlyGender.Width = width
        Me.lblReadonlyBENoText.Width = width
        Me.lblReadonlyPmtRemainText.Width = width
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
            Return Me._maskBirthEntryNumber
        End Get
        Set(ByVal value As Boolean)
            Me._maskBirthEntryNumber = value
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