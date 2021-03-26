Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType

Partial Public Class ucReadOnlyDocumnetType
    Inherits System.Web.UI.UserControl

    Private _documentType As String
    Private _udtEHSAccount As EHSAccountModel
    Private _showAccountRefNo As Boolean
    Private _isVertical As Boolean
    Private _showAccountCreationDate As Boolean
    Private _showTempAccountNotice As Boolean
    Private _maskIdentityNo As Boolean
    Private _highLightDocType As Boolean
    Private _textOnlyVersion As Boolean
    Private _mode As ucInputDocTypeBase.BuildMode = ucInputDocTypeBase.BuildMode.Creation
    Private _intTableTitleWidth As Integer
    Private _udtSmartIDContent As BLL.SmartIDContentModel
    Private _blnIsInvalidAccount As Boolean = False
    Private _blnSmartID As Boolean = False
    Private _blnEnableToShowHKICSymbol As Boolean = False

    Private Class DocumentControlID
        Public Const HKID As String = "ucReadOnlyDocumentType_HKID"
        Public Const EC As String = "ucReadOnlyDocumentType_EC"
        Public Const HKBC As String = "ucReadOnlyDocumentType_HKBC"
        Public Const DI As String = "ucReadOnlyDocumentType_DI"
        Public Const REPMT As String = "ucReadOnlyDocumentType_REPMT"
        Public Const ID235B As String = "ucReadOnlyDocumentType_ID235B"
        Public Const VISA As String = "ucReadOnlyDocumentType_VISA"
        Public Const ADOPC As String = "ucReadOnlyDocumentType_ADOPC"
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const OW As String = "ucReadOnlyDocumentType_OW"
        Public Const TW As String = "ucReadOnlyDocumentType_TW"
        Public Const RFNo8 As String = "ucReadOnlyDocumentType_RFNo8"
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Public Const OTHER As String = "ucReadOnlyDocumentType_OTHER"
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
        ' CRE20-0022 (Immu record) [Start][Martin]
        Public Const CCIC As String = "ucReadOnlyDocumentType_CCIC"
        Public Const ROP140 As String = "ucReadOnlyDocumentType_ROP140"
        Public Const PASS As String = "ucReadOnlyDocumentType_PASS"

        ' CRE20-0022 (Immu record) [End][Martin]
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Setup Function"

    Public Sub Built()
        Dim strFolderPath As String = String.Empty
        Dim udcReadOnlyDocumentType As ucReadOnlyDocTypeBase = Nothing

        If _blnIsInvalidAccount Then
            lblInvalidAccount.Visible = True

            lblInvalidAccount.Text = GetGlobalResourceObject("Text", "NotApplicable")
        Else
            lblInvalidAccount.Visible = False

            If Me._textOnlyVersion Then
                strFolderPath = "~/UIControl/DocTypeText"
            Else
                strFolderPath = "~/UIControl/DocType"
            End If

            Select Case Me._documentType
                Case DocTypeModel.DocTypeCode.HKIC
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyHKIC.ascx", strFolderPath))
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
                    udcReadOnlyDocumentType.SmartIDContent = Me._udtSmartIDContent
                    udcReadOnlyDocumentType.ID = DocumentControlID.HKID
                    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    udcReadOnlyDocumentType.ShowHKICSymbol = Me._blnEnableToShowHKICSymbol
                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                Case DocTypeModel.DocTypeCode.EC
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyEC.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.EC
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)

                Case DocTypeModel.DocTypeCode.HKBC
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyHKBC.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.HKBC
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC)

                Case DocTypeModel.DocTypeCode.DI
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyDI.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.DI
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)

                Case DocTypeModel.DocTypeCode.ID235B
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyID235B.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.ID235B
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)

                Case DocTypeModel.DocTypeCode.REPMT
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyReentryPermit.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.REPMT
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)

                Case DocTypeModel.DocTypeCode.VISA
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyVISA.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.VISA
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)

                Case DocTypeModel.DocTypeCode.ADOPC

                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyAdoption.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.ADOPC
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)

                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case DocTypeModel.DocTypeCode.OW

                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyOW.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.OW
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.OW)

                Case DocTypeModel.DocTypeCode.TW

                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyRFNo8.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.TW
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.TW)

                Case DocTypeModel.DocTypeCode.RFNo8

                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyRFNo8.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.RFNo8
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.RFNo8)

                Case DocTypeModel.DocTypeCode.OC,
                    DocTypeModel.DocTypeCode.IR,
                    DocTypeModel.DocTypeCode.HKP,
                    DocTypeModel.DocTypeCode.OTHER
                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyOTHER.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.OTHER
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(Me._documentType)

                    ' CRE20-0022 (Immu record) [Start][Martin]
                Case DocTypeModel.DocTypeCode.CCIC
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyCCIC.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.CCIC
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.CCIC)

                Case DocTypeModel.DocTypeCode.ROP140
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyROP140.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.ROP140
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ROP140)

                Case DocTypeModel.DocTypeCode.PASS
                    udcReadOnlyDocumentType = Me.LoadControl(String.Format("{0}/ucReadOnlyPASS.ascx", strFolderPath))
                    udcReadOnlyDocumentType.ID = DocumentControlID.PASS
                    udcReadOnlyDocumentType.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.PASS)
                    ' CRE20-0022 (Immu record) [End][Martin]

            End Select
            udcReadOnlyDocumentType.EHSAccount = Me._udtEHSAccount
            udcReadOnlyDocumentType.IsVertical = Me._isVertical
            udcReadOnlyDocumentType.MaskIdentityNumber = Me._maskIdentityNo
            udcReadOnlyDocumentType.ShowAccountRefNo = Me._showAccountRefNo
            udcReadOnlyDocumentType.ShowTempAccountNotice = Me._showTempAccountNotice
            udcReadOnlyDocumentType.ShowAccountCreationDate = Me._showAccountCreationDate
            udcReadOnlyDocumentType.HightLightDocType = Me._highLightDocType
            udcReadOnlyDocumentType.Mode = Me._mode
            udcReadOnlyDocumentType.IsSmartID = Me._blnSmartID

           

            Me.phReadOnlyDocumentType.Controls.Clear()
            Me.Built(udcReadOnlyDocumentType)

            If Me._intTableTitleWidth > 0 Then
                udcReadOnlyDocumentType.SetupTableTitle(Me._intTableTitleWidth)
            Else
                If ShowAccountCreationDate Then
                    udcReadOnlyDocumentType.SetupTableTitle(230)
                Else
                    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    'udcReadOnlyDocumentType.SetupTableTitle(200)
                    udcReadOnlyDocumentType.SetupTableTitle(210)
                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                End If

            End If
        End If

    End Sub

    Public Sub Built(ByVal udcControl As Control)
        Me.phReadOnlyDocumentType.Controls.Add(udcControl)
    End Sub

    Public Sub Clear()
        Me.phReadOnlyDocumentType.Controls.Clear()
    End Sub
#End Region

#Region "Property"

    Public Property DocumentType() As String
        Get
            Return Me._documentType
        End Get
        Set(ByVal value As String)
            Me._documentType = value
        End Set
    End Property

    Public Property EHSAccount() As EHSAccountModel
        Get
            Return Me._udtEHSAccount

        End Get
        Set(ByVal value As EHSAccountModel)
            Me._udtEHSAccount = value
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

    Public Property Vertical() As Boolean
        Get
            Return Me._isVertical

        End Get
        Set(ByVal value As Boolean)
            Me._isVertical = value
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

    Public Property MaskIdentityNo() As Boolean
        Get
            Return Me._maskIdentityNo
        End Get
        Set(ByVal value As Boolean)
            Me._maskIdentityNo = value
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

    Public Property TableTitleWidth() As Integer
        Get
            Return Me._intTableTitleWidth
        End Get
        Set(ByVal value As Integer)
            _intTableTitleWidth = value
        End Set
    End Property

    Public Property TextOnlyVersion() As Boolean
        Get
            Return Me._textOnlyVersion
        End Get
        Set(ByVal value As Boolean)
            Me._textOnlyVersion = value
        End Set
    End Property

    Public Property SmartIDContent() As BLL.SmartIDContentModel
        Get
            Return Me._udtSmartIDContent
        End Get
        Set(ByVal value As BLL.SmartIDContentModel)
            Me._udtSmartIDContent = value
        End Set
    End Property

    Public Property IsInvalidAccount() As Boolean
        Get
            Return Me._blnIsInvalidAccount
        End Get
        Set(ByVal value As Boolean)
            _blnIsInvalidAccount = value
        End Set
    End Property

    Public Property IsSmartID() As Boolean
        Get
            Return _blnSmartID
        End Get
        Set(ByVal value As Boolean)
            _blnSmartID = value
        End Set
    End Property

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Property SetEnableToShowHKICSymbol() As Boolean
        Get
            Return _blnEnableToShowHKICSymbol
        End Get
        Set(ByVal value As Boolean)
            _blnEnableToShowHKICSymbol = value
        End Set
    End Property
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
#End Region

End Class