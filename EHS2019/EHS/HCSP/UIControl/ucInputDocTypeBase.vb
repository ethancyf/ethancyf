Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.ComObject

Public MustInherit Class ucInputDocTypeBase
    Inherits System.Web.UI.UserControl

    Public Enum BuildMode
        Creation
        Modification
        ModifyReadOnly
    End Enum

    Private _mode As ucInputDocTypeBase.BuildMode
    Private _updateValue As Boolean
    Private _udtEHSAccountPersonalInfo As EHSPersonalInformationModel
    Private _udtOrgEHSAccountPersonalInfo As EHSPersonalInformationModel
    Private _udtSchemeClaim As SchemeClaimModel

    Private _udtFormatter As Formatter
    Private _udtSessionHandler As BLL.SessionHandler
    Private _activeViewChanged As Boolean
    Private _udtAuditLogEntry As AuditLogEntry
    Private _blnFixEnglishNameGender As Boolean

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private _blnEditDocumentNo As Boolean = False
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
    Private _enumMode As Common.Component.ClaimMode = Common.Component.ClaimMode.All
    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strErrorImageURL As String = Me.GetGlobalResourceObject("ImageUrl", "ErrorBtn")
        Dim strErrorImageALT As String = Me.GetGlobalResourceObject("AlternateText", "ErrorBtn")

        Me._udtSessionHandler = New BLL.SessionHandler()
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

    Public Property EHSPersonalInfo() As EHSPersonalInformationModel
        Get
            Return Me._udtEHSAccountPersonalInfo
        End Get
        Set(ByVal value As EHSPersonalInformationModel)
            Me._udtEHSAccountPersonalInfo = value
        End Set
    End Property

    ' CRE16-012 Removal of DOB InWord [Start][Winnie]
    Public Property OrgEHSPersonalInfo() As EHSPersonalInformationModel
        Get
            Return Me._udtOrgEHSAccountPersonalInfo
        End Get
        Set(ByVal value As EHSPersonalInformationModel)
            Me._udtOrgEHSAccountPersonalInfo = value
        End Set
    End Property
    ' CRE16-012 Removal of DOB InWord [End][Winnie]

    Public Property SchemeClaim() As SchemeClaimModel
        Get
            Return Me._udtSchemeClaim
        End Get
        Set(ByVal value As SchemeClaimModel)
            Me._udtSchemeClaim = value
        End Set
    End Property

    Public ReadOnly Property SessionHandler() As BLL.SessionHandler
        Get
            Return Me._udtSessionHandler
        End Get
    End Property

    Public ReadOnly Property Formatter() As Formatter
        Get
            Return Me._udtFormatter
        End Get
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

    Public Property FixEnglishNameGender() As Boolean
        Get
            Return _blnFixEnglishNameGender
        End Get
        Set(ByVal value As Boolean)
            _blnFixEnglishNameGender = value
        End Set
    End Property

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property EditDocumentNo() As Boolean
        Get
            Return _blnEditDocumentNo
        End Get
        Set(ByVal value As Boolean)
            _blnEditDocumentNo = value
        End Set
    End Property
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [Start][Winnie SUEN]
    ' -------------------------------------------------------------------------
    Public Property ClaimMode() As Common.Component.ClaimMode
        Get
            Return Me._enumMode
        End Get
        Set(ByVal value As Common.Component.ClaimMode)
            Me._enumMode = value
        End Set
    End Property
    ' CRE20-023-67 (COVID19 - Prefill Personal Info) [End][Winnie SUEN]

#End Region

#Region "Functions"

    Public Sub SetCCCodeTextBox(ByVal textBox As TextBox, ByVal strCCCode As String)
        If Not strCCCode Is Nothing Then
            If strCCCode.Length > 4 Then
                textBox.Text = strCCCode.Substring(0, 4)
            Else
                textBox.Text = strCCCode
            End If
        Else
            textBox.Text = String.Empty
        End If

    End Sub

    Public Sub SetCCCodeLabel(ByVal label As Label, ByVal strCCCode As String)
        If Not strCCCode Is Nothing Then
            If strCCCode.Length > 4 Then
                label.Text = strCCCode.Substring(0, 4)
            Else
                label.Text = strCCCode
            End If
        Else
            label.Text = String.Empty
        End If

    End Sub

#End Region

End Class
