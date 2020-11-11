'Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component
Imports HCSP.BLL

Partial Public Class ucInputEHSClaim
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
    Private _udtSessionHandler As New SessionHandler

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private _blnNonClinic As Boolean
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Private _mode As EHSClaimMode

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    'Events 

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Event SelectReasonForVisitClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event AddReasonForVisitClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event EditReasonForVisitClicked(ByVal sender As Object, ByVal e As System.EventArgs)

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event ClaimControlEventFired(ByVal strSchemeCode As String, ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Public Event RecipientConditionHelpClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Private Class EHSClaimControlID

        Public Const HCVS As String = "ucInputEHSClaim_HCVS"
        Public Const EVSS As String = "ucInputEHSClaim_EVSS"
        Public Const CIVSS As String = "ucInputEHSClaim_CIVSS"
        Public Const HSIVSS As String = "ucInputEHSClaim_HSIVSS"
        Public Const RVP As String = "ucInputEHSClaim_RVP"
        Public Const EHAPP As String = "ucInputEHSClaim_EHAPP"
        Public Const VACCINE As String = "ucInputEHSClaim_Vaccine"
        Public Const HCVS_CHINA As String = "ucInputEHSClaim_HCVSChina"
        Public Const PIDVSS As String = "ucInputEHSClaim_PIDVSS"
        Public Const VSS As String = "ucInputEHSClaim_VSS"
        Public Const ENHVSSO As String = "ucInputEHSClaim_ENHVSSO"
        Public Const PPP As String = "ucInputEHSClaim_PPP"
        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const SSSCMC As String = "ucInputEHSClaim_SSSCMC"
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    End Class

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

            Dim enumControlType As SchemeClaimModel.EnumControlType = New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(Me._strSchemeCode)

            Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(Me._strSchemeCode)
                Case SchemeClaimModel.EnumControlType.VOUCHER
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.HCVS Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.HCVS_CHINA Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.EVSS
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.EVSS Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.CIVSS
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.CIVSS Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.HSIVSS
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.HSIVSS Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.RVP
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.RVP Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.EHAPP
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.EHAPP Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.PIDVSS
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.PIDVSS Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.VSS
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.VSS Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.ENHVSSO
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.ENHVSSO Then BuiltSchemeControlOnly(True)

                Case SchemeClaimModel.EnumControlType.PPP
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.PPP Then BuiltSchemeControlOnly(True)

                    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                Case SchemeClaimModel.EnumControlType.SSSCMC
                    If _udcInputEHSClaim.ID <> EHSClaimControlID.SSSCMC Then BuiltSchemeControlOnly(True) ' ---------------------------------------------------------------------------------------------------------
                    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                Case Else
                    Throw New Exception(String.Format("No available input control for scheme({0}).", enumControlType.ToString))

            End Select

        End If

        If Not Me._textOnlyVersion And _udtEHSTransactionOriginal IsNot Nothing Then
            _udcInputEHSClaim.Clear()
        End If

        _udcInputEHSClaim.SchemeClaim = New SchemeClaimBLL().getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(_strSchemeCode)
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
        _udcInputEHSClaim.NonClinic = Me._blnNonClinic

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

        'Initial input control
        Me.Clear()
        Me.PlaceHolder1.Controls.Clear()
        Me.ucInputHCVS.Visible = False
        Me.ucInputHCVSChina.Visible = False

        Dim enumControlType As SchemeClaimModel.EnumControlType = New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(Me._strSchemeCode)

        Select Case enumControlType
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
                    AddHandler CType(udcInputEHSClaim, UIControl.EHCClaimText.ucInputHCVS).SelectReasonForVisitClick, AddressOf udcInputEHSClaim_SelectReasonForVisitClick
                    AddHandler CType(udcInputEHSClaim, UIControl.EHCClaimText.ucInputHCVS).AddReasonForVisitClick, AddressOf udcInputEHSClaim_AddReasonForVisitClick
                    AddHandler CType(udcInputEHSClaim, UIControl.EHCClaimText.ucInputHCVS).EditReasonForVisitClick, AddressOf udcInputEHSClaim_EditReasonForVisitClick

                End If

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                If Me._textOnlyVersion Then
                    'no text only version
                Else
                    udcInputEHSClaim = Me.ucInputHCVSChina
                    udcInputEHSClaim.ID = EHSClaimControlID.HCVS_CHINA
                    udcInputEHSClaim.Visible = True

                End If

            Case SchemeClaimModel.EnumControlType.EVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputEVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.EVSS
                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    AddHandler CType(udcInputEHSClaim, ucInputEVSS).VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                End If

            Case SchemeClaimModel.EnumControlType.CIVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputCIVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.CIVSS
                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    AddHandler CType(udcInputEHSClaim, ucInputCIVSS).VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                End If

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputHSIVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.HSIVSS

                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    Dim udcInputHSIVSS As ucInputHSIVSS = CType(udcInputEHSClaim, ucInputHSIVSS)
                    AddHandler udcInputHSIVSS.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                    AddHandler udcInputHSIVSS.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                End If

            Case SchemeClaimModel.EnumControlType.RVP
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputRVP.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.RVP
                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    Dim udcInputRVP As ucInputRVP = CType(udcInputEHSClaim, ucInputRVP)
                    AddHandler udcInputRVP.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                    AddHandler udcInputRVP.SearchButtonClick, AddressOf udcInputRVP_SearchButtonClick
                    AddHandler udcInputRVP.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                    AddHandler udcInputRVP.SubsidizeDisabledRemarkClicked, AddressOf udcInputEHSClaim_SubsidizeDisabledRemarkClick
                    AddHandler udcInputRVP.RecipientConditionHelpClick, AddressOf udcInputEHSClaim_RecipientConditionHelpClick
                End If

            Case SchemeClaimModel.EnumControlType.EHAPP
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputEHAPP.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.EHAPP

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputPIDVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.PIDVSS
                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    Dim udcInputPIDVSS As ucInputPIDVSS = CType(udcInputEHSClaim, ucInputPIDVSS)
                    AddHandler udcInputPIDVSS.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                End If

            Case SchemeClaimModel.EnumControlType.VSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.VSS
                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    Dim udcInputVSS As ucInputVSS = CType(udcInputEHSClaim, ucInputVSS)
                    AddHandler udcInputVSS.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                    AddHandler udcInputVSS.SearchButtonClick, AddressOf udcInputVSS_SearchButtonClick
                    AddHandler udcInputVSS.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                    AddHandler udcInputVSS.SubsidizeDisabledRemarkClicked, AddressOf udcInputEHSClaim_SubsidizeDisabledRemarkClick
                    AddHandler udcInputVSS.RecipientConditionHelpClick, AddressOf udcInputEHSClaim_RecipientConditionHelpClick
                End If

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputENHVSSO.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.ENHVSSO
                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    Dim udcInputENHVSSO As ucInputENHVSSO = CType(udcInputEHSClaim, ucInputENHVSSO)
                    AddHandler udcInputENHVSSO.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                    AddHandler udcInputENHVSSO.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                    AddHandler udcInputENHVSSO.SubsidizeDisabledRemarkClicked, AddressOf udcInputEHSClaim_SubsidizeDisabledRemarkClick
                End If

            Case SchemeClaimModel.EnumControlType.PPP
                ' ''udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputPPP.ascx", strFolderPath))
                ' ''udcInputEHSClaim.ID = EHSClaimControlID.PPP
                ' ''If Me._textOnlyVersion Then
                ' ''    'No Input Control
                ' ''Else
                ' ''    Dim udcInputVSS As ucInputVSS = CType(udcInputEHSClaim, ucInputVSS)
                ' ''    AddHandler udcInputVSS.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                ' ''    AddHandler udcInputVSS.SearchButtonClick, AddressOf udcInputVSS_SearchButtonClick
                ' ''    AddHandler udcInputVSS.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                ' ''    'AddHandler udcInputVSS.SubsidizeDisabledRemarkClicked, AddressOf udcInputEHSClaim_SubsidizeDisabledRemarkClick
                ' ''    'AddHandler udcInputVSS.RecipientConditionHelpClick, AddressOf udcInputEHSClaim_RecipientConditionHelpClick
                ' ''End If

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputSSSCMC.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.SSSCMC
                If Me._textOnlyVersion Then
                    'No Input Control
                Else
                    Dim udcInputSSSCMC As ucInputSSSCMC = CType(udcInputEHSClaim, ucInputSSSCMC)

                End If
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            Case Else
                Throw New Exception(String.Format("No available input control for scheme({0}).", enumControlType.ToString))

        End Select

        _udcInputEHSClaim = udcInputEHSClaim
        _udcInputEHSClaim.SchemeClaim = New SchemeClaimBLL().getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(_strSchemeCode)
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
        _udcInputEHSClaim.NonClinic = Me._blnNonClinic

        _udcInputEHSClaim.SetupContent()

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Select Case _udcInputEHSClaim.ID
            Case Me.ucInputHCVS.ID, ucInputHCVSChina.ID, ucInputSSSCMC.ID
                'Not to add PlaceHolder Control
            Case Else
                Me.PlaceHolder1.Controls.Add(_udcInputEHSClaim)
        End Select
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

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

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim ucHCVSChinaControl As ucInputEHSClaimBase = GetHCVSChinaControl()
        If Not ucHCVSChinaControl Is Nothing Then
            ucHCVSChinaControl.Clear()
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        'CRE13-019-02 Extend HCVS to China [End][Karl]

        ' EVSS - No addition control
        ' CIVSS - No addition control
        ' HSIVSS 
        Dim ucHSIVSSControl As ucInputEHSClaimBase = GetHSIVSSControl()
        If Not ucHSIVSSControl Is Nothing Then
            ucHSIVSSControl.Clear()
        End If

        ' RVP
        Dim ucRVPControl As ucInputEHSClaimBase = GetRVPControl()
        If Not ucRVPControl Is Nothing Then
            ucRVPControl.Clear()
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
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Voucher Slim
        Dim ucHCVSChinaControl As ucInputEHSClaimBase = GetHCVSChinaControl()
        If Not ucHCVSChinaControl Is Nothing Then
            ucHCVSChinaControl.SetDoseErrorImage(False)
            If _textOnlyVersion Then
                'no text vesrion

                'Dim ucInputVoucherSlimText As UIControl.EHCClaimText.ucInputVoucherSlim = CType(ucVoucherSlimControl, UIControl.EHCClaimText.ucInputVoucherSlim)
                'ucInputVoucherSlimText.SetVoucherredeemError(False)
            Else
                Dim ucInputHCVSChinaFull As ucInputHCVSChina = CType(ucHCVSChinaControl, ucInputHCVSChina)
                ucInputHCVSChinaFull.SetVoucherRedeemError(False)
            End If
        End If
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        'CRE13-019-02 Extend HCVS to China [End][Karl]
        ' EVSS 
        Dim ucEVSSControl As ucInputEHSClaimBase = GetEVSSControl()
        If Not ucEVSSControl Is Nothing Then
            ucEVSSControl.SetDoseErrorImage(False)
        End If

        ' CIVSS
        Dim ucCIVSSControl As ucInputEHSClaimBase = GetCIVSSControl()
        If Not ucCIVSSControl Is Nothing Then
            ucCIVSSControl.SetDoseErrorImage(False)
        End If

        ' HSIVSS 
        Dim ucHSIVSSControl As ucInputEHSClaimBase = GetHSIVSSControl()
        If Not ucHSIVSSControl Is Nothing Then
            ucHSIVSSControl.SetDoseErrorImage(False)
            If _textOnlyVersion Then
                'No Input Control
            Else
                Dim ucInputHSIVSSFull As ucInputHSIVSS = CType(ucHSIVSSControl, ucInputHSIVSS)
                ucInputHSIVSSFull.SetPreConditionError(False)
            End If
        End If

        ' RVP
        Dim ucRVPControl As ucInputEHSClaimBase = GetRVPControl()
        If Not ucRVPControl Is Nothing Then
            ucRVPControl.SetDoseErrorImage(False)
            If _textOnlyVersion Then
                'No Input Control
            Else
                Dim ucInputRVPFull As ucInputRVP = CType(ucRVPControl, ucInputRVP)
                ucInputRVPFull.SetRCHCodeError(False)
            End If
        End If

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        ' EHAPP
        Dim ucEHAPPControl As ucInputEHSClaimBase = GetEHAPPControl()
        If Not ucEHAPPControl Is Nothing Then
            If _textOnlyVersion Then
                ' No Text-only Version for EHAPP
            Else
                Dim ucInputEHAPPFull As ucInputEHAPP = CType(ucEHAPPControl, ucInputEHAPP)
                ucInputEHAPPFull.SetAllAlertVisible(False)
            End If
        End If
        ' CRE13-001 - EHAPP [End][Tommy L]

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

    Private Sub udcInputEHSClaim_AddReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent AddReasonForVisitClicked(sender, e)
    End Sub

    Private Sub udcInputEHSClaim_EditReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent EditReasonForVisitClicked(sender, e)
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Private Sub udcInputEHSClaim_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent VaccineRemarkClicked(sender, e)
    End Sub

    Private Sub udcInputRVP_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.RVP, sender, e)
    End Sub

    Private Sub udcInputVSS_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.VSS, sender, e)
    End Sub

    Private Sub udcInputEHSClaim_VaccineLegendClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    Private Sub udcInputEHSClaim_CategorySelected(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent CategorySelected(sender, e)
    End Sub

    Private Sub udcInputEHSClaim_SubsidizeDisabledRemarkClick(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub

    Private Sub udcInputEHSClaim_RecipientConditionHelpClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent RecipientConditionHelpClicked(sender, e)
    End Sub

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
    'Get VoucherSlim Control -> HCVS (Slim) = Claim Voucher
    Public Function GetHCVSChinaControl() As ucInputEHSClaimBase
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Dim control As Control = Me.FindControl(EHSClaimControlID.HCVS_CHINA)

        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'no Text Only Version
            Else
                Return CType(control, ucInputHCVSChina)
            End If
        Else
            Return Nothing
        End If
    End Function
    'CRE13-019-02 Extend HCVS to China [End][Karl]
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    'CRE13-019-02 Extend HCVS to China [End][Karl]

    'Get EVSS Control
    Public Function GetEVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.EVSS)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputEVSS)
            End If
        Else
            Return Nothing
        End If
    End Function

    'Get CIVSS Control
    Public Function GetCIVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.CIVSS)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputCIVSS)
            End If
        Else
            Return Nothing
        End If
    End Function

    'Get HSIVSS Control
    Public Function GetHSIVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.HSIVSS)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputHSIVSS)
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Function GetRVPControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.RVP)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputRVP)
            End If
        Else
            Return Nothing
        End If
    End Function

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    ' Get EHAPP Control
    Public Function GetEHAPPControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.EHAPP)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                ' No Text-only Version for EHAPP
                Return Nothing
            Else
                Return CType(control, ucInputEHAPP)
            End If
        Else
            Return Nothing
        End If
    End Function
    ' CRE13-001 - EHAPP [End][Tommy L]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Get PIDVSS Control
    Public Function GetPIDVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.PIDVSS)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputPIDVSS)
            End If
        Else
            Return Nothing
        End If
    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    Public Function GetVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.VSS)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputVSS)
            End If
        Else
            Return Nothing
        End If
    End Function
    '-----------------------------------------------------------------------------------------
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Function GetENHVSSOControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.ENHVSSO)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputENHVSSO)
            End If
        Else
            Return Nothing
        End If
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Function GetPPPControl() As ucInputEHSClaimBase
        'Dim control As Control = Me.FindControl(EHSClaimControlID.PPP)
        'If Not control Is Nothing Then
        '    If Me._textOnlyVersion Then
        '        'No Input Control
        '    Else
        '        Return CType(control, ucInputPPP)
        '    End If
        'Else
        Return Nothing
        'End If
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Function GetSSSCMCControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.SSSCMC)
        If Not control Is Nothing Then
            If Me._textOnlyVersion Then
                'No Input Control
            Else
                Return CType(control, ucInputSSSCMC)
            End If
        Else
            Return Nothing
        End If
    End Function
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]
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

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Property NonClinic() As Boolean
        Get
            Return _blnNonClinic
        End Get
        Set(ByVal value As Boolean)
            _blnNonClinic = value
        End Set
    End Property
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

#End Region


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Function IsIncomplete(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Return _udcInputEHSClaim.IsIncomplete(udtEHSTransaction)
    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
End Class