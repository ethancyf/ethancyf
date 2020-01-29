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
    Private _mode As ucInputDocTypeBase.BuildMode = ucInputDocTypeBase.BuildMode.Creation
    Private _intWidth As Integer = 0

    Private Class DocumentControlID
        Public Const HKID As String = "ucReadOnlyDocumentType_HKID"
        Public Const EC As String = "ucReadOnlyDocumentType_EC"
        Public Const HKBC As String = "ucReadOnlyDocumentType_HKBC"
        Public Const DI As String = "ucReadOnlyDocumentType_DI"
        Public Const REPMT As String = "ucReadOnlyDocumentType_REPMT"
        Public Const ID235B As String = "ucReadOnlyDocumentType_ID235B"
        Public Const VISA As String = "ucReadOnlyDocumentType_VISA"
        Public Const ADOPC As String = "ucReadOnlyDocumentType_ADOPC"
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Setup Function"

    Public Sub Built()
        Select Case Me._documentType
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcReadOnlyHKIC As ucReadOnlyHKIC = Me.LoadControl("~/UIControl/DocType/ucReadOnlyHKIC.ascx")
                udcReadOnlyHKIC.ID = DocumentControlID.HKID
                udcReadOnlyHKIC.IsVertical = Me._isVertical
                udcReadOnlyHKIC.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
                udcReadOnlyHKIC.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyHKIC.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyHKIC.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyHKIC.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyHKIC.HightLightDocType = Me._highLightDocType
                udcReadOnlyHKIC.Mode = Me._mode
                udcReadOnlyHKIC.Width = Me._intWidth
                Me.Built(udcReadOnlyHKIC)

            Case DocTypeModel.DocTypeCode.EC

                Dim udcReadOnlyEC As ucReadOnlyEC = Me.LoadControl("~/UIControl/DocType/ucReadOnlyEC.ascx")
                udcReadOnlyEC.ID = DocumentControlID.EC
                udcReadOnlyEC.IsVertical = Me._isVertical
                'udcReadOnlyEC.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)
                udcReadOnlyEC.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyEC.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyEC.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyEC.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyEC.HightLightDocType = Me._highLightDocType
                udcReadOnlyEC.Mode = Me._mode
                udcReadOnlyEC.Width = Me._intWidth
                Me.Built(udcReadOnlyEC)

            Case DocTypeModel.DocTypeCode.HKBC

                Dim udcReadOnlyHKBC As ucReadOnlyHKBC = Me.LoadControl("~/UIControl/DocType/ucReadOnlyHKBC.ascx")
                udcReadOnlyHKBC.ID = DocumentControlID.HKBC
                udcReadOnlyHKBC.IsVertical = Me._isVertical
                udcReadOnlyHKBC.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC)
                udcReadOnlyHKBC.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyHKBC.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyHKBC.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyHKBC.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyHKBC.HightLightDocType = Me._highLightDocType
                udcReadOnlyHKBC.Width = Me._intWidth
                Me.Built(udcReadOnlyHKBC)

            Case DocTypeModel.DocTypeCode.DI

                Dim udcReadOnlyDI As ucReadOnlyDI = Me.LoadControl("~/UIControl/DocType/ucReadOnlyDI.ascx")
                udcReadOnlyDI.ID = DocumentControlID.DI
                udcReadOnlyDI.IsVertical = Me._isVertical
                udcReadOnlyDI.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)
                udcReadOnlyDI.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyDI.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyDI.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyDI.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyDI.HightLightDocType = Me._highLightDocType
                udcReadOnlyDI.Width = Me._intWidth
                Me.Built(udcReadOnlyDI)

            Case DocTypeModel.DocTypeCode.ID235B

                Dim udcReadOnlyID235B As ucReadOnlyID235B = Me.LoadControl("~/UIControl/DocType/ucReadOnlyID235B.ascx")
                udcReadOnlyID235B.ID = DocumentControlID.ID235B
                udcReadOnlyID235B.IsVertical = Me._isVertical
                udcReadOnlyID235B.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)
                udcReadOnlyID235B.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyID235B.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyID235B.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyID235B.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyID235B.HightLightDocType = Me._highLightDocType
                udcReadOnlyID235B.Width = Me._intWidth
                Me.Built(udcReadOnlyID235B)

            Case DocTypeModel.DocTypeCode.REPMT

                Dim udcReadOnlyRMPT As ucReadOnlyReentryPermit = Me.LoadControl("~/UIControl/DocType/ucReadOnlyReentryPermit.ascx")
                udcReadOnlyRMPT.ID = DocumentControlID.REPMT
                udcReadOnlyRMPT.IsVertical = Me._isVertical
                udcReadOnlyRMPT.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)
                udcReadOnlyRMPT.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyRMPT.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyRMPT.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyRMPT.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyRMPT.Width = Me._intWidth
                Me.Built(udcReadOnlyRMPT)

            Case DocTypeModel.DocTypeCode.VISA

                Dim udcReadOnlyVISA As ucReadOnlyVISA = Me.LoadControl("~/UIControl/DocType/ucReadOnlyVISA.ascx")
                udcReadOnlyVISA.ID = DocumentControlID.VISA
                udcReadOnlyVISA.IsVertical = Me._isVertical
                udcReadOnlyVISA.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)
                udcReadOnlyVISA.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyVISA.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyVISA.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyVISA.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyVISA.HightLightDocType = Me._highLightDocType
                udcReadOnlyVISA.Width = Me._intWidth
                Me.Built(udcReadOnlyVISA)

            Case DocTypeModel.DocTypeCode.ADOPC

                Dim udcReadOnlyADOPC As ucReadOnlyAdoption = Me.LoadControl("~/UIControl/DocType/ucReadOnlyAdoption.ascx")
                udcReadOnlyADOPC.ID = DocumentControlID.ADOPC
                udcReadOnlyADOPC.IsVertical = Me._isVertical
                udcReadOnlyADOPC.EHSAccountPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)
                udcReadOnlyADOPC.MaskIdentityNumber = Me._maskIdentityNo
                udcReadOnlyADOPC.ShowAccountRefNo = Me._showAccountRefNo
                udcReadOnlyADOPC.ShowTempAccountNotice = Me._showTempAccountNotice
                udcReadOnlyADOPC.ShowAccountCreationDate = Me._showAccountCreationDate
                udcReadOnlyADOPC.HightLightDocType = Me._highLightDocType
                udcReadOnlyADOPC.Width = Me._intWidth
                Me.Built(udcReadOnlyADOPC)
        End Select
    End Sub

    Public Sub Built(ByVal udcControl As Control)
        Me.phReadOnlyDocumentType.Controls.Add(udcControl)
    End Sub

    Public Sub Clear()
        Me.phReadOnlyDocumentType.Controls.Clear()
    End Sub

    Public Sub ClearControl()
        Me.Controls.Clear()
        Me.Controls.Add(phReadOnlyDocumentType)
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