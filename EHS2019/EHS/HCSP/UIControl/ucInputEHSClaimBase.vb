Imports Common.Component.Scheme
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component.ClaimCategory

Public MustInherit Class ucInputEHSClaimBase
    Inherits System.Web.UI.UserControl

    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Private _udtSchemeClaim As SchemeClaimModel
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    Private _udtEHSTransaction As EHSTransactionModel
    Private _udtEHSTransactionOriginal As EHSTransactionModel
    Private _udtEHSTransactionLatestVaccineRecord As EHSTransactionModel
    Private _udtTranDetailLatestVaccineRecord As TransactionDetailVaccineModel
    Private _udtEHSClaimVaccine As EHSClaimVaccineModel
    Private _udtEHSAccount As EHSAccountModel
    Private _udtCurrentPractice As BLL.PracticeDisplayModel
    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Private _blnAvaliableForClaim As Boolean
    Private _activeViewChanged As Boolean
    Private _blnIsSupportedDevice As Boolean
    Private _dtmServiceDate As DateTime
    Private _udtClaimCategorys As ClaimCategoryModelCollection
    Private _btnIsModifyMode As Boolean = False
    Private _blnNonClinic As Boolean
    Private _udtVaccineList As TransactionDetailVaccineModelCollection

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

    Public Sub SetupContent()
        Setup()
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Overridable Sub RefreshDisplay()

    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Protected MustOverride Sub Setup()

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


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Check specified transaction provided all mandatory value,
    ''' e.g. If HCVS is missing co-payment fee in transaction additional field, then return false
    ''' </summary>
    ''' <param name="udtEHSTransaction">The specified transaction will be checked</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function IsIncomplete(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Dim udtEHSCLaimBLL As New BLL.EHSClaimBLL
        Return udtEHSCLaimBLL.chkEHSTransactionIncomplete(udtEHSTransaction)
    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Shared Function FindNextDoseForSelection(ByVal EHSClaimVaccine As EHSClaimVaccineModel, ByRef ddlCDoseCovid19 As DropDownList) As String
        Dim strSelectedValue = String.Empty
        Dim intLatestDoseInjected As Integer = 0
        Dim intMaxDoseforInject As Integer = 0
        Dim udtDoseList As New EHSClaimVaccineModel.EHSClaimSubidizeDetailModelCollection
        Dim dicDoseList As New Dictionary(Of Integer, Boolean)

        If EHSClaimVaccine IsNot Nothing Then
            For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In EHSClaimVaccine.SubsidizeList
                For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                    If Not dicDoseList.ContainsKey(udtEHSClaimSubidizeDetail.DisplaySeq) Then
                        dicDoseList.Add(udtEHSClaimSubidizeDetail.DisplaySeq, udtEHSClaimSubidizeDetail.Available)
                    Else
                        Dim blnRes As Boolean = dicDoseList.Item(udtEHSClaimSubidizeDetail.DisplaySeq)

                        blnRes = blnRes Or udtEHSClaimSubidizeDetail.Available

                        dicDoseList.Remove(udtEHSClaimSubidizeDetail.DisplaySeq)
                        dicDoseList.Add(udtEHSClaimSubidizeDetail.DisplaySeq, blnRes)
                    End If

                    If udtEHSClaimSubidizeDetail.DisplaySeq > intMaxDoseforInject Then
                        intMaxDoseforInject = udtEHSClaimSubidizeDetail.DisplaySeq
                    End If

                    If udtEHSClaimSubidizeDetail.Available Then
                        udtDoseList.Add(udtEHSClaimSubidizeDetail)
                    End If
                Next
            Next

            For Each udtKVP As KeyValuePair(Of Integer, Boolean) In dicDoseList
                If Not udtKVP.Value Then
                    If udtKVP.Key > intLatestDoseInjected Then
                        intLatestDoseInjected = udtKVP.Key
                    End If
                End If
            Next

            If intMaxDoseforInject > intLatestDoseInjected Then
                Dim udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = udtDoseList.FilterByDisplaySeq(intLatestDoseInjected + 1)

                If udtEHSClaimSubidizeDetail IsNot Nothing Then
                    Dim strTargetDose As String = udtEHSClaimSubidizeDetail.AvailableItemCode

                    For Each li As ListItem In ddlCDoseCovid19.Items
                        'If li.Value = SchemeDetails.SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                        '    strSelectedValue = SchemeDetails.SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                        'End If
                        If li.Value = strTargetDose Then
                            strSelectedValue = strTargetDose
                        End If
                    Next
                End If
            End If
        End If

        Return strSelectedValue

    End Function

#End Region

#Region "Properties"

    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Public Property SchemeClaim() As SchemeClaimModel
        Get
            Return Me._udtSchemeClaim
        End Get
        Set(ByVal value As SchemeClaimModel)
            Me._udtSchemeClaim = value
        End Set
    End Property
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    Public Property EHSTransaction() As EHSTransactionModel
        Get
            Return Me._udtEHSTransaction
        End Get
        Set(ByVal value As EHSTransactionModel)
            Me._udtEHSTransaction = value
        End Set
    End Property

    Public Property EHSTransactionOriginal() As EHSTransactionModel
        Get
            Return Me._udtEHSTransactionOriginal
        End Get
        Set(ByVal value As EHSTransactionModel)
            Me._udtEHSTransactionOriginal = value
        End Set
    End Property

    Public Property EHSTransactionLatestVaccineRecord() As EHSTransactionModel
        Get
            Return Me._udtEHSTransactionLatestVaccineRecord
        End Get
        Set(ByVal value As EHSTransactionModel)
            Me._udtEHSTransactionLatestVaccineRecord = value
        End Set
    End Property

    Public Property TranDetailLatestVaccineRecord() As TransactionDetailVaccineModel
        Get
            Return Me._udtTranDetailLatestVaccineRecord
        End Get
        Set(ByVal value As TransactionDetailVaccineModel)
            Me._udtTranDetailLatestVaccineRecord = value
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

    Public Property CurrentPractice() As BLL.PracticeDisplayModel
        Get
            Return Me._udtCurrentPractice
        End Get
        Set(ByVal value As BLL.PracticeDisplayModel)
            Me._udtCurrentPractice = value
        End Set
    End Property

    Public ReadOnly Property SessionHandler() As BLL.SessionHandler
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

    Public Property IsModifyMode() As Boolean
        Get
            Return _btnIsModifyMode
        End Get
        Set(ByVal value As Boolean)
            _btnIsModifyMode = value
        End Set
    End Property

    Public Property NonClinic() As Boolean
        Get
            Return _blnNonClinic
        End Get
        Set(ByVal value As Boolean)
            _blnNonClinic = value
        End Set
    End Property

    Public Property VaccineList() As TransactionDetailVaccineModelCollection
        Get
            Return _udtVaccineList
        End Get
        Set(ByVal value As TransactionDetailVaccineModelCollection)
            _udtVaccineList = value
        End Set
    End Property

#End Region
End Class
