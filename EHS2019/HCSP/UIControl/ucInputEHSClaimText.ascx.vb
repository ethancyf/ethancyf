'Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory

Partial Public Class ucInputEHSClaimText
    Inherits System.Web.UI.UserControl

    Private _strSchemeCode As String
    Private _currentPractice As BLL.PracticeDisplayModel
    Private _udtEHSAccount As EHSAccountModel
    Private _udtEHSClaimVaccine As EHSClaimVaccineModel
    Private _udtEHSTransaction As EHSTransactionModel
    Private _udtEHSTransactionOriginal As EHSTransactionModel
    Private _blnAvaliableForClaim As Boolean
    Private _textOnlyVersion As Boolean
    Private _activeViewChanged As Boolean
    Private _intTableTitleWidth As Integer = 0
    Private _blnIsControlBuilt As Boolean = False
    Private _blnIsSupportedDevice As Boolean = False
    Private _dtmServiceDate As DateTime
    Private _udtClaimCategorys As ClaimCategoryModelCollection
    Private _udcInputEHSClaim As ucInputEHSClaimBase = Nothing
    Private _btnIsModifyMode As Boolean = False

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Private _mode As EHSClaimMode

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    'Events 

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Event SelectReasonForVisitClicked(ByVal sender As Object, ByVal e As System.EventArgs)

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event ClaimControlEventFired(ByVal strSchemeCode As String, ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'Public Event PerConditionSelected(ByVal sender As System.Object, ByVal e As System.EventArgs)

    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Private Class EHSClaimControlID
        Public Const HCVS As String = "ucInputEHSClaim_HCVS"
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        'Public Const VOUCHERSLIM As String = "ucInputEHSClaim_HCVSCina"
        'CRE13-019-02 Extend HCVS to China [End][Karl]

    End Class
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Enum EHSClaimMode
        Normal
        Complete
    End Enum

    Public Property Mode() As EHSClaimMode
        Get
            Return Me._mode
        End Get
        Set(ByVal value As EHSClaimMode)
            Me._mode = value
        End Set
    End Property


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#Region "Setup Function"
    Public Sub Built()
        ' Avoid building twice
        If _blnIsControlBuilt = True Then
            Return
        End If


        If _strSchemeCode Is Nothing Then Exit Sub

        If _udcInputEHSClaim Is Nothing Then
            BuiltSchemeControlOnly(True)
        Else
            If Me.PlaceHolder1.Controls.Count = 0 Then
                Me.PlaceHolder1.Controls.Add(_udcInputEHSClaim)
            End If

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(Me._strSchemeCode)
                Case SchemeClaimModel.EnumControlType.VOUCHER
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.HCVS Then BuiltSchemeControlOnly(True)

                    'CRE13-019-02 Extend HCVS to China [Start][Karl]
                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    'no text version
                    'CRE13-019-02 Extend HCVS to China [End][Karl]
            End Select
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End If

        If Not Me._textOnlyVersion And _udtEHSTransactionOriginal IsNot Nothing Then
            _udcInputEHSClaim.Clear()
        End If

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        _udcInputEHSClaim.SchemeClaim = New SchemeClaimBLL().getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(_strSchemeCode)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        _udcInputEHSClaim.ActiveViewChanged = Me.ActiveViewChanged
        _udcInputEHSClaim.IsSupportedDevice = Me.IsSupportedDevice
        _udcInputEHSClaim.AvaliableForClaim = Me._blnAvaliableForClaim
        _udcInputEHSClaim.EHSAccount = Me._udtEHSAccount
        _udcInputEHSClaim.CurrentPractice = Me._currentPractice
        _udcInputEHSClaim.EHSTransaction = Me._udtEHSTransaction
        _udcInputEHSClaim.EHSTransactionOriginal = Me._udtEHSTransactionOriginal
        _udcInputEHSClaim.EHSClaimVaccine = Me._udtEHSClaimVaccine
        _udcInputEHSClaim.ServiceDate = Me._dtmServiceDate
        _udcInputEHSClaim.ClaimCategorys = Me._udtClaimCategorys
        _udcInputEHSClaim.SetupTableTitle(Me._intTableTitleWidth)
        _udcInputEHSClaim.IsModifyMode = Me.IsModifyMode

        _udcInputEHSClaim.SetupContent()
        _blnIsControlBuilt = True
    End Sub

    Public Sub BuiltSchemeControlOnly(ByVal blnForceRebuild As Boolean)
        BuiltSchemeControlOnly(blnForceRebuild, False)
    End Sub

    Public Sub BuiltSchemeControlOnly(ByVal blnForceRebuild As Boolean, ByVal blnByCreateChildControl As Boolean)
        If Not blnForceRebuild Then
            If _udcInputEHSClaim IsNot Nothing Then
                Exit Sub
            End If
        End If

        Dim udcInputEHSClaim As ucInputEHSClaimBase = Nothing

        Dim strFolderPath As String = String.Empty

        If Me._textOnlyVersion Then
            strFolderPath = "~/UIControl/EHSClaimText"
        Else
            strFolderPath = "~/UIControl/EHSClaim"
        End If

        Me.Clear()
        Me.PlaceHolder1.Controls.Clear()
        Me.ucInputHCVS.Visible = False

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        'Me.ucInputVoucherSlim.Visible = False
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(Me._strSchemeCode)
            Case SchemeClaimModel.EnumControlType.VOUCHER
                If Me._textOnlyVersion Then
                    udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputHCVS.ascx", strFolderPath))
                    udcInputEHSClaim.ID = EHSClaimControlID.HCVS
                Else
                    udcInputEHSClaim = Me.ucInputHCVS
                    udcInputEHSClaim.ID = EHSClaimControlID.HCVS
                    udcInputEHSClaim.Visible = True
                End If

                If Me._textOnlyVersion Then

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                    ' -----------------------------------------------------------------------------------------

                    AddHandler CType(udcInputEHSClaim, UIControl.EHCClaimText.ucInputHCVS).SelectReasonForVisitClick, AddressOf udcInputEHSClaim_SelectReasonForVisitClick

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                End If
                'CRE13-019-02 Extend HCVS to China [Start][Karl]
            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                'no text version
                'CRE13-019-02 Extend HCVS to China [End][Karl]

        End Select

        _udcInputEHSClaim = udcInputEHSClaim
        _udcInputEHSClaim.SchemeClaim = New SchemeClaimBLL().getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(_strSchemeCode)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        _udcInputEHSClaim.ActiveViewChanged = Me.ActiveViewChanged
        _udcInputEHSClaim.IsSupportedDevice = Me.IsSupportedDevice
        _udcInputEHSClaim.AvaliableForClaim = Me._blnAvaliableForClaim
        _udcInputEHSClaim.EHSAccount = Me._udtEHSAccount
        _udcInputEHSClaim.CurrentPractice = Me._currentPractice
        _udcInputEHSClaim.EHSTransaction = Me._udtEHSTransaction
        _udcInputEHSClaim.EHSClaimVaccine = Me._udtEHSClaimVaccine
        _udcInputEHSClaim.ServiceDate = Me._dtmServiceDate
        _udcInputEHSClaim.ClaimCategorys = Me._udtClaimCategorys
        _udcInputEHSClaim.SetupTableTitle(Me._intTableTitleWidth)

        _udcInputEHSClaim.SetupContent()
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        If _udcInputEHSClaim.ID <> Me.ucInputHCVS.ID Then
            Me.PlaceHolder1.Controls.Add(_udcInputEHSClaim)
        End If
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        If Not blnByCreateChildControl Then _udcInputEHSClaim.SetupContent()

    End Sub

    Private Sub Built(ByVal udcControl As Control)
        Me.PlaceHolder1.Controls.Add(udcControl)
        _blnIsControlBuilt = True
    End Sub

    Public Sub Clear()

        ' HCVS
        Dim ucHCVSControl As ucInputEHSClaimBase = GetHCVSControl()
        If Not ucHCVSControl Is Nothing Then
            ucHCVSControl.Clear()
        End If

        Me.PlaceHolder1.Controls.Clear()

    End Sub


    Public Sub ClearErrorMessage()

        ' HCVS
        Dim ucHCVSControl As ucInputEHSClaimBase = GetHCVSControl()
        If Not ucHCVSControl Is Nothing Then
            ucHCVSControl.SetDoseErrorImage(False)
            If _textOnlyVersion Then
                Dim ucInputHCVSText As UIControl.EHCClaimText.ucInputHCVS = CType(ucHCVSControl, UIControl.EHCClaimText.ucInputHCVS)
                ucInputHCVSText.SetReasonForVisitError(False)
                ucInputHCVSText.SetVoucherredeemError(False)
            Else
                Dim ucInputHCVSFull As ucInputHCVS = CType(ucHCVSControl, ucInputHCVS)
                ucInputHCVSFull.SetReasonForVisitError(False)
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                'ucInputHCVSFull.SetVoucherredeemError(False)
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
            End If
        End If

    End Sub

    Public Sub SetRebuildRequired()
        ' Force to rebuild the control
        _blnIsControlBuilt = False
    End Sub

#End Region

#Region "Events"

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Private Sub udcInputEHSClaim_SelectReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SelectReasonForVisitClicked(sender, e)
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Private Sub udcInputEHSClaim_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent VaccineRemarkClicked(sender, e)
    End Sub

    Private Sub udcInputRVP_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.RVP, sender, e)
    End Sub

    'Private Sub udcInputEHSClaim_RoleSelected(ByVal sender As Object, ByVal e As System.EventArgs)
    '    RaiseEvent RoleSelected(sender, e)
    'End Sub

    Private Sub udcInputEHSClaim_VaccineLegendClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    Private Sub udcInputEHSClaim_CategorySelected(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent CategorySelected(sender, e)
    End Sub

    'Private Sub udcInputEHSClaim_SelectPerConditionsClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    '    RaiseEvent PerConditionSelected(sender, e)
    'End Sub

#End Region

#Region "get control functions"

    'Get HCVS Control -> HCVS = Claim Voucher
    Public Function GetHCVSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.HCVS)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                Return CType(control, UIControl.EHCClaimText.ucInputHCVS)
            Else
                Return CType(control, ucInputHCVS)
            End If
        Else
            Return Nothing
        End If
    End Function
    'CRE13-019-02 Extend HCVS to China [Start][Karl]
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]

    'Public Function GetHCVSChinaControl() As ucInputEHSClaimBase
    '    Dim control As Control = Me.FindControl(EHSClaimControlID.VOUCHERSLIM)
    '    If Not control Is Nothing Then
    '        If Me._textOnlyVersion Then
    '            Return CType(control, UIControl.EHCClaimText.ucInputVoucherSlim)
    '        Else
    '            Return CType(control, ucInputVoucherSlim)
    '        End If
    '    Else
    '        Return Nothing
    '    End If
    'End Function
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    'CRE13-019-02 Extend HCVS to China [End][Karl]

#End Region

#Region "Property"

    Public Sub ResetSchemeType()
        Me._strSchemeCode = String.Empty
    End Sub

    Public Property SchemeType() As String
        Get
            Return Me._strSchemeCode
        End Get
        Set(ByVal value As String)
            If (Me._strSchemeCode <> value) Then
                ' rebuild when scheme is updated
                _blnIsControlBuilt = False
            End If

            Me._strSchemeCode = value
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
            Return Me._currentPractice
        End Get
        Set(ByVal value As BLL.PracticeDisplayModel)
            Me._currentPractice = value
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

    Public Property AvaliableForClaim() As Boolean
        Get
            Return Me._blnAvaliableForClaim
        End Get
        Set(ByVal value As Boolean)
            Me._blnAvaliableForClaim = value
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

    Public Property ActiveViewChanged() As Boolean
        Get
            If String.IsNullOrEmpty(Me.Attributes("ActiveViewChanged")) Then
                Return True
            Else
                Return CType(Me.Attributes("ActiveViewChanged"), Boolean)
            End If
        End Get

        Set(ByVal value As Boolean)
            Me.Attributes("ActiveViewChanged") = value
            Me._activeViewChanged = CType(Me.Attributes("ActiveViewChanged"), Boolean)
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

    Public ReadOnly Property IsControlBuilt() As Boolean
        Get
            Return _blnIsControlBuilt
        End Get
    End Property

    Public Property IsModifyMode() As Boolean
        Get
            Return _btnIsModifyMode
        End Get
        Set(ByVal value As Boolean)
            _btnIsModifyMode = value
        End Set
    End Property
#End Region


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Function IsIncomplete(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Return _udcInputEHSClaim.IsIncomplete(udtEHSTransaction)
    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
End Class