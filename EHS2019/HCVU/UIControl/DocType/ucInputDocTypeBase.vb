Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.ComObject
Imports Common.Format

Public MustInherit Class ucInputDocTypeBase
    Inherits System.Web.UI.UserControl

    Public Enum BuildMode
        Creation
        Modification
        Modification_OneSide
        ModifyReadOnly
    End Enum

    Private _mode As ucInputDocTypeBase.BuildMode
    Private _updateValue As Boolean
    Private _udtEHSAccountPersonalInfoOriginal As EHSPersonalInformationModel
    Private _udtEHSAccountPersonalInfoAmend As EHSPersonalInformationModel
    Private _useDefaultAmendingHeader As Boolean = False
    Private _activeViewChanged As Boolean
    Private _udtFormatter As Formatter
    Private _udtAuditLogEntry As AuditLogEntry

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strErrorImageURL As String = Me.GetGlobalResourceObject("ImageUrl", "ErrorBtn")
        Dim strErrorImageALT As String = Me.GetGlobalResourceObject("AlternateText", "ErrorBtn")

        Me._udtFormatter = New Formatter()

        Me.RenderLanguage(strErrorImageURL, strErrorImageALT)
        Me.Setup(Me._mode)
    End Sub

    Protected MustOverride Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
    Protected MustOverride Sub Setup(ByVal mode As BuildMode)
    Public MustOverride Sub SetValue(ByVal mode As BuildMode)
    Public MustOverride Sub SetErrorImage(ByVal mode As BuildMode, ByVal visible As Boolean)
    Public MustOverride Sub SetProperty(ByVal mode As BuildMode)

#Region "Properties"

    Public Property UpdateValue() As Boolean
        Get
            Return Me._updateValue
        End Get
        Set(ByVal value As Boolean)
            Me._updateValue = value
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

    Public Property EHSPersonalInfoOriginal() As EHSPersonalInformationModel
        Get
            Return Me._udtEHSAccountPersonalInfoOriginal
        End Get
        Set(ByVal value As EHSPersonalInformationModel)
            Me._udtEHSAccountPersonalInfoOriginal = value
        End Set
    End Property

    Public Property EHSPersonalInfoAmend() As EHSPersonalInformationModel
        Get
            Return Me._udtEHSAccountPersonalInfoAmend
        End Get
        Set(ByVal value As EHSPersonalInformationModel)
            Me._udtEHSAccountPersonalInfoAmend = value
        End Set
    End Property

    Public ReadOnly Property Formatter() As Formatter
        Get
            Return Me._udtFormatter
        End Get
    End Property

    Public Property UseDefaultAmendingHeader() As Boolean
        Get
            Return Me._useDefaultAmendingHeader
        End Get
        Set(ByVal value As Boolean)
            _useDefaultAmendingHeader = value
        End Set
    End Property

    Public Property ActiveViewChanged() As Boolean
        Get
            Return Me._activeViewChanged
        End Get
        Set(ByVal value As Boolean)
            Me._activeViewChanged = value
        End Set
    End Property

    Public Property AuditLogEntry() As AuditLogEntry
        Get
            Return Me._udtAuditLogEntry
        End Get
        Set(ByVal value As AuditLogEntry)
            Me._udtAuditLogEntry = value
        End Set
    End Property

#End Region

End Class
