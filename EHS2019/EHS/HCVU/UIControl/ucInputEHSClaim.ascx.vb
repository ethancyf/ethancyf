Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.Practice
Imports HCVU.BLL

Partial Public Class ucInputEHSClaim
    Inherits System.Web.UI.UserControl

    Private _strSchemeCode As String
    'Private _currentPractice As BLL.PracticeDisplayModel
    Private _currentPractice As PracticeBLL.PracticeDisplayModel
    Private _udtEHSAccount As EHSAccountModel
    Private _udtEHSClaimVaccine As EHSClaimVaccineModel
    Private _udtEHSTransaction As EHSTransactionModel
    Private _blnAvaliableForClaim As Boolean
    Private _textOnlyVersion As Boolean
    Private _activeViewChanged As Boolean
    Private _intTableTitleWidth As Integer = 0
    Private _blnIsControlBuilt As Boolean = False
    Private _blnIsSupportedDevice As Boolean = False
    Private _dtmServiceDate As DateTime
    Private _udtClaimCategorys As ClaimCategoryModelCollection
    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private _blnNonClinic As Boolean
    Private _blnPostbackRebuild As Boolean
    'CRE16-002 (Revamp VSS) [End][Chris YIM]


    Private _strFunctionCode As String
    Private _blnShowLegend As Boolean
    Private _udcInputEHSClaim As ucInputEHSClaimBase = Nothing
    Private udtSessionHandlerBLL As New SessionHandlerBLL

    'Events 
    Public Event ChangeReasonForVisitClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event ClaimControlEventFired(ByVal strSchemeCode As String, ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event CategorySelected(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'Public Event PerConditionSelected(ByVal sender As System.Object, ByVal e As System.EventArgs)

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT010404

    Private Class EHSClaimControlID
        Public Const HCVS As String = "ucInputEHSClaim_HCVS"
        Public Const EVSS As String = "ucInputEHSClaim_EVSS"
        Public Const CIVSS As String = "ucInputEHSClaim_CIVSS"
        Public Const HSIVSS As String = "ucInputEHSClaim_HSIVSS"
        Public Const RVP As String = "ucInputEHSClaim_RVP"
        Public Const HCVS_CHINA As String = "ucInputEHSClaim_HCVSChina"
        Public Const VACCINE As String = "ucInputEHSClaim_Vaccine"
        Public Const EHAPP As String = "ucInputEHSClaim_EHAPP"
        Public Const PIDVSS As String = "ucInputEHSClaim_PIDVSS"
        Public Const VSS As String = "ucInputEHSClaim_VSS"
        Public Const ENHVSSO As String = "ucInputEHSClaim_ENHVSSO"
        Public Const PPP As String = "ucInputEHSClaim_PPP"
        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const SSSCMC As String = "ucInputEHSClaim_SSSCMC"
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]
        Public Const COVID19 As String = "ucInputEHSClaim_COVID19"
        Public Const COVID19RVP As String = "ucInputEHSClaim_COVID19RVP"
        Public Const COVID19OR As String = "ucInputEHSClaim_COVID19OR"

    End Class

#Region "Setup Function"
    Public Sub Built(ByVal blnPostbackRebulid As Boolean)
        ' Avoid building twice
        If _blnIsControlBuilt = True Then
            Return
        End If

        _blnPostbackRebuild = blnPostbackRebulid

        Dim udcInputEHSClaim As ucInputEHSClaimBase = Nothing

        'Must clear -> Page will add diffierent Scheme control in same page, if user seleted other Scheme
        Me.PlaceHolder1.Controls.Clear()

        Dim strFolderPath As String = String.Empty

        strFolderPath = "~/UIControl/EHSClaim"

        ' Reset static control visible before build
        ucInputEHSClaim_HCVS.Visible = False
        ucInputEHSClaim_HCVSChina.Visible = False
        ucInputEHSClaim_VSS.Visible = False
        ucInputEHSClaim_ENHVSSO.Visible = False
        ucInputEHSClaim_PPP.Visible = False
        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ucInputEHSClaim_SSSCMC.Visible = False
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]
        ucInputEHSClaim_COVID19.Visible = False

        Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(Me._strSchemeCode)
            Case SchemeClaimModel.EnumControlType.VOUCHER
                ' Use static control to reduce load view state problem
                udcInputEHSClaim = Me.ucInputEHSClaim_HCVS

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                ' Use static control to reduce load view state problem
                udcInputEHSClaim = Me.ucInputEHSClaim_HCVSChina

            Case SchemeClaimModel.EnumControlType.EVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputEVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.EVSS

                AddHandler CType(udcInputEHSClaim, ucInputEVSS).VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

            Case SchemeClaimModel.EnumControlType.CIVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputCIVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.CIVSS

                AddHandler CType(udcInputEHSClaim, ucInputCIVSS).VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputHSIVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.HSIVSS

                Dim udcInputHSIVSS As ucInputHSIVSS = CType(udcInputEHSClaim, ucInputHSIVSS)
                AddHandler udcInputHSIVSS.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                AddHandler udcInputHSIVSS.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected

            Case SchemeClaimModel.EnumControlType.RVP
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputRVP.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.RVP

                Dim udcInputRVP As ucInputRVP = CType(udcInputEHSClaim, ucInputRVP)
                AddHandler udcInputRVP.SearchButtonClick, AddressOf udcInputRVP_SearchButtonClick
                AddHandler udcInputRVP.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick
                AddHandler udcInputRVP.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected

            Case SchemeClaimModel.EnumControlType.EHAPP
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputEHAPP.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.EHAPP

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputPIDVSS.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.PIDVSS

                AddHandler CType(udcInputEHSClaim, ucInputPIDVSS).VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

            Case SchemeClaimModel.EnumControlType.VSS
                udcInputEHSClaim = Me.ucInputEHSClaim_VSS

                Dim udcInputVSS As ucInputVSS = CType(udcInputEHSClaim, ucInputVSS)
                AddHandler udcInputVSS.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                AddHandler udcInputVSS.SearchButtonClick, AddressOf udcInputVSS_SearchButtonClick
                AddHandler udcInputVSS.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udcInputEHSClaim = Me.ucInputEHSClaim_ENHVSSO

                Dim udcInputENHVSSO As ucInputENHVSSO = CType(udcInputEHSClaim, ucInputENHVSSO)
                AddHandler udcInputENHVSSO.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                AddHandler udcInputENHVSSO.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

            Case SchemeClaimModel.EnumControlType.PPP
                udcInputEHSClaim = Me.ucInputEHSClaim_PPP

                Dim udcInputPPP As ucInputPPP = CType(udcInputEHSClaim, ucInputPPP)
                AddHandler udcInputPPP.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                AddHandler udcInputPPP.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

                If Me._strSchemeCode = SchemeClaimModel.PPP Then
                    AddHandler udcInputPPP.SearchButtonClick, AddressOf udcInputPPP_SearchButtonClick
                End If

                If Me._strSchemeCode = SchemeClaimModel.PPPKG Then
                    AddHandler udcInputPPP.SearchButtonClick, AddressOf udcInputPPPKG_SearchButtonClick
                End If

                'CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                '---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                udcInputEHSClaim = Me.ucInputEHSClaim_SSSCMC
                udcInputEHSClaim.ID = EHSClaimControlID.SSSCMC
                'CRE20-015 (Special Support Scheme) [End][Chris YIM]

            Case SchemeClaimModel.EnumControlType.COVID19
                udcInputEHSClaim = Me.ucInputEHSClaim_COVID19

                Dim udcInputCOVID19 As ucInputCOVID19 = CType(udcInputEHSClaim, ucInputCOVID19)
                AddHandler udcInputCOVID19.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected

                'Case SchemeClaimModel.EnumControlType.COVID19CBD
                '    udcInputEHSClaim = Me.ucInputEHSClaim_COVID19CBD

                '    Dim udcInputCOVID19CBD As ucInputCOVID19CBD = CType(udcInputEHSClaim, ucInputCOVID19CBD)
                '    AddHandler udcInputCOVID19CBD.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected
                '    AddHandler udcInputCOVID19CBD.VaccineLegendClicked, AddressOf udcInputEHSClaim_VaccineLegendClick

            Case SchemeClaimModel.EnumControlType.COVID19RVP
                udcInputEHSClaim = Me.LoadControl(String.Format("{0}/ucInputCOVID19RVP.ascx", strFolderPath))
                udcInputEHSClaim.ID = EHSClaimControlID.COVID19RVP

                Dim udcInputCOVID19RVP As ucInputCOVID19RVP = CType(udcInputEHSClaim, ucInputCOVID19RVP)
                AddHandler udcInputCOVID19RVP.SearchButtonClick, AddressOf udcInputCOVID19RVP_SearchButtonClick
                AddHandler udcInputCOVID19RVP.CategorySelected, AddressOf udcInputEHSClaim_CategorySelected

        End Select

        If Not IsNothing(udcInputEHSClaim) Then
            udcInputEHSClaim.FunctionCode = Me.FunctionCode
            udcInputEHSClaim.SchemeClaim = New SchemeClaimBLL().getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(_strSchemeCode)
            udcInputEHSClaim.ActiveViewChanged = Me.ActiveViewChanged
            udcInputEHSClaim.IsSupportedDevice = Me.IsSupportedDevice
            udcInputEHSClaim.AvaliableForClaim = Me._blnAvaliableForClaim
            udcInputEHSClaim.EHSAccount = Me._udtEHSAccount
            udcInputEHSClaim.CurrentPractice = Me._currentPractice
            udcInputEHSClaim.EHSTransaction = Me._udtEHSTransaction
            udcInputEHSClaim.EHSClaimVaccine = Me._udtEHSClaimVaccine
            udcInputEHSClaim.ServiceDate = Me._dtmServiceDate
            udcInputEHSClaim.ClaimCategorys = Me._udtClaimCategorys
            udcInputEHSClaim.SetupTableTitle(Me._intTableTitleWidth)
            udcInputEHSClaim.ShowLegend = Me._blnShowLegend
            udcInputEHSClaim.NonClinic = Me._blnNonClinic

            Me.Built(udcInputEHSClaim)
        End If

    End Sub

    Private Sub Built(ByVal udcControl As ucInputEHSClaimBase)
        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Select Case udcControl.ID
            Case EHSClaimControlID.HCVS, EHSClaimControlID.HCVS_CHINA, EHSClaimControlID.VSS, EHSClaimControlID.ENHVSSO, EHSClaimControlID.PPP, _
                 EHSClaimControlID.SSSCMC, EHSClaimControlID.COVID19
                'Show Input Control
                udcControl.Visible = True

                Select Case udcControl.ID
                    Case EHSClaimControlID.VSS, EHSClaimControlID.ENHVSSO, EHSClaimControlID.PPP, EHSClaimControlID.COVID19
                        'For vaccine input setup
                        udcControl.SetupContent(_blnPostbackRebuild)

                    Case Else
                        'For voucher input setup
                        udcControl.SetupContent()

                End Select

            Case Else
                Me.PlaceHolder1.Controls.Add(udcControl)

        End Select

        _blnIsControlBuilt = True
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    End Sub

    Public Sub Clear()

        ' HCVS
        Dim ucHCVSControl As ucInputEHSClaimBase = GetHCVSControl()
        If Not ucHCVSControl Is Nothing Then
            ucHCVSControl.Clear()

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Invisible static control
            ucHCVSControl.Visible = False
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
        End If

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Dim ucHCVSChinaControl As ucInputEHSClaimBase = GetHCVSChinaControl()

        If Not ucHCVSChinaControl Is Nothing Then
            ucHCVSChinaControl.Clear()
            ' Invisible static control
            ucHCVSChinaControl.Visible = False
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

        ' VSS
        Dim ucVSSControl As ucInputEHSClaimBase = GetVSSControl()
        If Not ucVSSControl Is Nothing Then
            ucVSSControl.Clear()
            ucVSSControl.Visible = False
        End If

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' PPP
        Dim ucPPPControl As ucInputEHSClaimBase = GetPPPControl()
        If Not ucPPPControl Is Nothing Then
            ucPPPControl.Clear()
            ucPPPControl.Visible = False
        End If
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' COVID19
        Dim ucCOVID19Control As ucInputEHSClaimBase = GetCOVID19Control()
        If Not ucCOVID19Control Is Nothing Then
            ucCOVID19Control.Clear()
            ucCOVID19Control.Visible = False
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.PlaceHolder1.Controls.Clear()

    End Sub


    Public Sub ClearErrorMessage()

        ' HCVS
        Dim ucHCVSControl As ucInputEHSClaimBase = GetHCVSControl()
        If Not ucHCVSControl Is Nothing Then
            ucHCVSControl.SetDoseErrorImage(False)

            Dim ucInputHCVSFull As ucInputHCVS = CType(ucHCVSControl, ucInputHCVS)
            ucInputHCVSFull.SetReasonForVisitError(False)
            ucInputHCVSFull.SetVoucherredeemError(False)
            'CRE13-006 HCVS Ceiling [Start][Karl]
            ucInputHCVSFull.SetCoPaymentFeeError(False)
            'CRE13-006 HCVS Ceiling [End][Karl]
        End If
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim ucHCVSChinaControl As ucInputEHSClaimBase = GetHCVSChinaControl()
        If Not ucHCVSChinaControl Is Nothing Then
            ucHCVSChinaControl.SetDoseErrorImage(False)

            Dim ucInputHCVSChinaFull As ucInputHCVSChina = CType(ucHCVSChinaControl, ucInputHCVSChina)
            ucInputHCVSChinaFull.SetVoucherredeemError(False)

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

            Dim ucInputHSIVSSFull As ucInputHSIVSS = CType(ucHSIVSSControl, ucInputHSIVSS)
            ucInputHSIVSSFull.SetPreConditionError(False)
        End If

        ' RVP
        Dim ucRVPControl As ucInputEHSClaimBase = GetRVPControl()
        If Not ucRVPControl Is Nothing Then
            ucRVPControl.SetDoseErrorImage(False)

            Dim ucInputRVPFull As ucInputRVP = CType(ucRVPControl, ucInputRVP)
            ucInputRVPFull.SetRCHCodeError(False)

        End If

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        ' EHAPP
        Dim ucEHAPPControl As ucInputEHSClaimBase = GetEHAPPControl()
        If Not ucEHAPPControl Is Nothing Then
            Dim ucInputEHAPPFull As ucInputEHAPP = CType(ucEHAPPControl, ucInputEHAPP)

            ucInputEHAPPFull.SetAllAlertVisible(False)
        End If
        ' CRE13-001 - EHAPP [End][Tommy L]

        ''CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
        ''-----------------------------------------------------------------------------------------
        'Dim ucPIDVSSControl As ucInputEHSClaimBase = GetPIDVSSControl()
        'If Not ucPIDVSSControl Is Nothing Then
        '    Dim ucInputPIDVSSFull As ucInputPIDVSS = CType(ucEHAPPControl, ucInputPIDVSS)

        '    ucInputPIDVSSFull.SetAllAlertVisible(False)
        'End If
        ''CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

        ''CRE16-002 (Revamp VSS) [Start][Chris YIM]
        ''-----------------------------------------------------------------------------------------
        'Dim ucEHAPPControl As ucInputEHSClaimBase = GetVSSControl()
        'If Not ucEHAPPControl Is Nothing Then
        '    Dim ucInputEHAPPFull As ucInputEHAPP = CType(ucEHAPPControl, ucInputEHAPP)

        '    ucInputEHAPPFull.SetAllAlertVisible(False)
        'End If
        ''CRE16-002 (Revamp VSS) [End][Chris YIM]

    End Sub

    Public Sub SetRebuildRequired()
        ' Force to rebuild the control
        _blnIsControlBuilt = False
    End Sub

#End Region

#Region "Events"

    Private Sub udcInputEHSClaim_ChangeReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent ChangeReasonForVisitClicked(sender, e)
    End Sub

    Private Sub udcInputEHSClaim_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent VaccineRemarkClicked(sender, e)
    End Sub

    Private Sub udcInputRVP_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.RVP, sender, e)
    End Sub

    Private Sub udcInputVSS_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.VSS, sender, e)
    End Sub

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub udcInputPPP_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.PPP, sender, e)
    End Sub
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub udcInputPPPKG_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.PPPKG, sender, e)
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Private Sub udcInputCOVID19RVP_SearchButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent ClaimControlEventFired(SchemeClaimModel.COVID19RVP, sender, e)
    End Sub

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
            Return CType(control, ucInputHCVS)
        Else
            Return Nothing
        End If
    End Function
    'CRE13-019-02 Extend HCVS to China [Start][Karl]

    Public Function GetHCVSChinaControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.HCVS_CHINA)
        If Not control Is Nothing Then
            Return CType(control, ucInputHCVSChina)
        Else
            Return Nothing
        End If
    End Function

    'CRE13-019-02 Extend HCVS to China [End][Karl]
    ''Get EVSS Control
    Public Function GetEVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.EVSS)
        If Not control Is Nothing Then
            Return CType(control, ucInputEVSS)
        Else
            Return Nothing
        End If
    End Function

    'Get CIVSS Control
    Public Function GetCIVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.CIVSS)
        If Not control Is Nothing Then
            Return CType(control, ucInputCIVSS)
        Else
            Return Nothing
        End If
    End Function

    'Get HSIVSS Control
    Public Function GetHSIVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.HSIVSS)
        If Not control Is Nothing Then
            Return CType(control, ucInputHSIVSS)
        Else
            Return Nothing
        End If
    End Function

    'Get RVP Control
    Public Function GetRVPControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.RVP)
        If Not control Is Nothing Then
            Return CType(control, ucInputRVP)
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
            Return CType(control, ucInputEHAPP)
        Else
            Return Nothing
        End If
    End Function
    ' CRE13-001 - EHAPP [End][Tommy L]

    'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function GetPIDVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.PIDVSS)
        If Not control Is Nothing Then
            Return CType(control, ucInputPIDVSS)
        Else
            Return Nothing
        End If
    End Function
    'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function GetVSSControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.VSS)
        If Not control Is Nothing Then
            Return CType(control, ucInputVSS)
        Else
            Return Nothing
        End If
    End Function
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Function GetENHVSSOControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.ENHVSSO)
        If Not control Is Nothing Then
            Return CType(control, ucInputENHVSSO)
        Else
            Return Nothing
        End If
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Function GetPPPControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.PPP)
        If Not control Is Nothing Then
            Return CType(control, ucInputPPP)
        Else
            Return Nothing
        End If
    End Function
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Function GetSSSCMCControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.SSSCMC)
        If Not control Is Nothing Then
            Return CType(control, ucInputSSSCMC)
        Else
            Return Nothing
        End If
    End Function
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Function GetCOVID19Control() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.COVID19)
        If Not control Is Nothing Then
            Return CType(control, ucInputCOVID19)
        Else
            Return Nothing
        End If
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Function GetCOVID19RVPControl() As ucInputEHSClaimBase
        Dim control As Control = Me.FindControl(EHSClaimControlID.COVID19RVP)
        If Not control Is Nothing Then
            Return CType(control, ucInputCOVID19RVP)
        Else
            Return Nothing
        End If
    End Function
    ' CRE20-0023 (Immu record) [End][Chris YIM]
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

    Public Property CurrentPractice() As PracticeBLL.PracticeDisplayModel
        Get
            Return Me._currentPractice
        End Get
        Set(ByVal value As PracticeBLL.PracticeDisplayModel) 'BLL.PracticeDisplayModel)
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
            Return _blnNonClinic
        End Get
        Set(ByVal value As Boolean)
            _blnNonClinic = value
        End Set
    End Property
    'CRE16-002 (Revamp VSS) [End][Chris YIM]
#End Region

End Class