Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme

Public MustInherit Class ucReadOnlyEHSClaimBase
    Inherits System.Web.UI.UserControl

    Private _mode As ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode
    Private _udtEHSClaimVaccine As EHSClaimVaccineModel
    Private _udtEHSTransaction As EHSTransactionModel
    Private _udtSessionHandler As BLL.SessionHandler
    Private _ShowSMSWarning As Boolean

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class HighRiskOption
        Public Const HIGHRISK As String = "HIGHRISK"
        Public Const NOHIGHRISK As String = "NOHIGHRISK"
    End Class
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me._udtSessionHandler = New BLL.SessionHandler
        Me.RenderLanguage()
        Me.Setup()
    End Sub

    Protected MustOverride Sub RenderLanguage()
    Protected MustOverride Sub Setup()
    Public MustOverride Sub SetupTableTitle(ByVal width As Integer)

#Region "Properties"

    Public Property EHSClaimVaccine() As EHSClaimVaccineModel
        Get
            Return Me._udtEHSClaimVaccine
        End Get
        Set(ByVal value As EHSClaimVaccineModel)
            Me._udtEHSClaimVaccine = value
        End Set
    End Property

    Public Property EHSTransaction() As EHSTransactionModel
        Get
            Return Me._udtEHSTransaction
        End Get
        Set(ByVal value As EHSTransactionModel)
            Me._udtEHSTransaction = value
        End Set
    End Property

    Public Property Mode() As ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode
        Get
            Return Me._mode
        End Get
        Set(ByVal value As ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode)
            Me._mode = value
        End Set
    End Property

    Public ReadOnly Property SessionHandler() As BLL.SessionHandler
        Get
            Return Me._udtSessionHandler
        End Get
    End Property

    Public Property ShowSMSWarning() As Boolean
        Get
            Return Me._ShowSMSWarning
        End Get
        Set(ByVal value As Boolean)
            Me._ShowSMSWarning = value
        End Set
    End Property

#End Region

End Class
