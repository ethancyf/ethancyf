Imports Common.Component.Scheme
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.Practice

Public MustInherit Class ucInputEHSClaimBase
    Inherits System.Web.UI.UserControl

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private _udtSchemeClaim As SchemeClaimModel
    'CRE16-026 (Add PCV13) [End][Chris YIM]
    Private _udtEHSTransaction As EHSTransactionModel
    Private _udtEHSClaimVaccine As EHSClaimVaccineModel
    Private _udtEHSAccount As EHSAccountModel
    'Private _udtCurrentPractice As BLL.PracticeDisplayModel
    Private _udtCurrentPractice As PracticeBLL.PracticeDisplayModel
    Private _udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL
    Private _blnAvaliableForClaim As Boolean
    Private _activeViewChanged As Boolean
    Private _blnIsSupportedDevice As Boolean
    Private _dtmServiceDate As DateTime
    Private _udtClaimCategorys As ClaimCategoryModelCollection

    Private _blnShowLegend As Boolean

    Private _strFunctionCode As String

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private _blnNonClinic As Boolean
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class HighRiskOption
        Public Const HIGHRISK As String = "HIGHRISK"
        Public Const NOHIGHRISK As String = "NOHIGHRISK"
    End Class
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.RenderLanguage()
        Me.Setup()
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Public Sub SetupContent(Optional blnPostbackRebulid As Boolean = False)
        Me.Setup(blnPostbackRebulid)

    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Protected MustOverride Sub Setup()

    Protected MustOverride Sub Setup(ByVal blnPostbackRebulid As Boolean)

    Protected Overridable Sub RenderLanguage()

    End Sub

    Public Overridable Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overridable Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Nothing
    End Function

#Region "Method"

    Protected Overridable Sub OnAvailableForClaimChanged()

    End Sub

    Public Overridable Sub Clear()

    End Sub

    Public Overridable Sub SetDoseErrorImage(ByVal blnVisible As Boolean)

    End Sub

#End Region

#Region "Properties"

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Property SchemeClaim() As SchemeClaimModel
        Get
            Return Me._udtSchemeClaim
        End Get
        Set(ByVal value As SchemeClaimModel)
            Me._udtSchemeClaim = value
        End Set
    End Property
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Public Property EHSTransaction() As EHSTransactionModel
        Get
            Return Me._udtEHSTransaction
        End Get
        Set(ByVal value As EHSTransactionModel)
            Me._udtEHSTransaction = value
        End Set
    End Property

    Public Property EHSClaimVaccine() As EHSClaimVaccineModel
        Get
            Return Me._udtEHSClaimVaccine
        End Get
        Set(ByVal value As EHSClaimVaccineModel)
            Me._udtEHSClaimVaccine = value
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

    Public Property CurrentPractice() As PracticeBLL.PracticeDisplayModel 'BLL.PracticeDisplayModel
        Get
            Return Me._udtCurrentPractice
        End Get
        Set(ByVal value As PracticeBLL.PracticeDisplayModel) 'BLL.PracticeDisplayModel)
            Me._udtCurrentPractice = value
        End Set
    End Property

    Public ReadOnly Property SessionHandler() As BLL.SessionHandlerBLL 'BLL.SessionHandler
        Get
            Return Me._udtSessionHandler
        End Get
    End Property

    Public Property AvaliableForClaim() As Boolean
        Get
            Return Me._blnAvaliableForClaim
        End Get
        Set(ByVal value As Boolean)
            Me._blnAvaliableForClaim = value
            Me.OnAvailableForClaimChanged()
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

    Public Property IsSupportedDevice() As Boolean
        Get
            Return Me._blnIsSupportedDevice
        End Get
        Set(ByVal value As Boolean)
            Me._blnIsSupportedDevice = value
        End Set
    End Property

    Public Property ServiceDate() As DateTime
        Get
            Return Me._dtmServiceDate
        End Get
        Set(ByVal value As DateTime)
            Me._dtmServiceDate = value
        End Set
    End Property

    Public Property ClaimCategorys() As ClaimCategoryModelCollection
        Get
            Return Me._udtClaimCategorys
        End Get
        Set(ByVal value As ClaimCategoryModelCollection)
            Me._udtClaimCategorys = value
        End Set
    End Property

    Public Property FunctionCode() As String
        Get
            Return Me._strFunctionCode
        End Get
        Set(ByVal value As String)
            Me._strFunctionCode = value
        End Set
    End Property

    Public Property ShowLegend() As Boolean
        Get
            Return Me._blnShowLegend
        End Get
        Set(ByVal value As Boolean)
            Me._blnShowLegend = value
        End Set
    End Property

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Property NonClinic() As Boolean
        Get
            Return Me._blnNonClinic
        End Get
        Set(ByVal value As Boolean)
            Me._blnNonClinic = value
        End Set
    End Property
    'CRE16-002 (Revamp VSS) [End][Chris YIM]


#End Region
End Class
