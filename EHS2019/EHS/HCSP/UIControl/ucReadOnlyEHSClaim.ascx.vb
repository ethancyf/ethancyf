Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyEHSClaim
    Inherits System.Web.UI.UserControl

    Public Enum ReadOnlyEHSClaimMode
        Normal
        Complete
    End Enum

    Private _strSchemeCode As String
    Private _mode As ReadOnlyEHSClaimMode
    Private _udtEHSClaimVaccine As EHSClaimVaccineModel
    Private _udtEHSTransaction As EHSTransactionModel
    Private _textOnlyVersion As Boolean
    Private _intTableTitleWidth As Integer = 0

    Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Private Class EHSClaimControlID
        Public Const HCVS As String = "ucReadOnlyEHSClaim_HCVS"
        Public Const EVSS As String = "ucReadOnlyEHSClaim_EVSS"
        Public Const CIVSS As String = "ucReadOnlyEHSClaim_CIVSS"
        Public Const HSIVSS As String = "ucReadOnlyEHSClaim_HSIVSS"
        Public Const RVP As String = "ucReadOnlyEHSClaim_RVP"
        Public Const HCVS_CHINA As String = "ucReadOnlyEHSClaim_HCVSChina"
        Public Const VACCINE As String = "ucReadOnlyEHSClaim_Vaccine"
        Public Const EHAPP As String = "ucReadOnlyEHSClaim_EHAPP"
        Public Const PIDVSS As String = "ucReadOnlyEHSClaim_PIDVSS"
        Public Const VSS As String = "ucReadOnlyEHSClaim_VSS"
        Public Const ENHVSSO As String = "ucReadOnlyEHSClaim_ENHVSSO"
        Public Const PPP As String = "ucReadOnlyEHSClaim_PPP"
        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const SSSCMC As String = "ucReadOnlyEHSClaim_SSSCMC"
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]
    End Class


#Region "Setup Function"
    Public Sub Built()
        Dim udcReadOnlyEHSClaim As ucReadOnlyEHSClaimBase = Nothing

        Dim strFolderPath As String = String.Empty
        If Me._textOnlyVersion Then
            strFolderPath = "~/UIControl/EHSClaimText"
        Else
            strFolderPath = "~/UIControl/EHSClaim"
        End If

        Dim enumControlType As SchemeClaimModel.EnumControlType = New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(Me._strSchemeCode)

        Select Case enumControlType
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyHCVS.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.HCVS

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyHCVSChina.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.HCVS_CHINA

            Case SchemeClaimModel.EnumControlType.EVSS
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyEVSS.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.EVSS
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyEVSS).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyEVSS).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

            Case SchemeClaimModel.EnumControlType.CIVSS
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyCIVSS.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.CIVSS
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyCIVSS).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyCIVSS).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyHSIVSS.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.HSIVSS
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyHSIVSS).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyHSIVSS).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

            Case SchemeClaimModel.EnumControlType.RVP
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyRVP.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.RVP
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyRVP).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyRVP).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

            Case SchemeClaimModel.EnumControlType.EHAPP
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyEHAPP.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.EHAPP

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyPIDVSS.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.PIDVSS
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyPIDVSS).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyPIDVSS).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

            Case SchemeClaimModel.EnumControlType.VSS
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyVSS.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.VSS
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyVSS).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyVSS).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyENHVSSO.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.ENHVSSO
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyENHVSSO).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyENHVSSO).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

            Case SchemeClaimModel.EnumControlType.PPP
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlyPPP.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.PPP
                If Me._textOnlyVersion Then
                    AddHandler CType(udcReadOnlyEHSClaim, UIControl.EHCClaimText.ucReadOnlyPPP).VaccineRemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked
                Else
                    AddHandler CType(udcReadOnlyEHSClaim, ucReadOnlyPPP).VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
                End If

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                udcReadOnlyEHSClaim = Me.LoadControl(String.Format("{0}/ucReadOnlySSSCMC.ascx", strFolderPath))
                udcReadOnlyEHSClaim.ID = EHSClaimControlID.SSSCMC
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        End Select

        udcReadOnlyEHSClaim.EHSClaimVaccine = Me._udtEHSClaimVaccine
        udcReadOnlyEHSClaim.EHSTransaction = Me._udtEHSTransaction
        udcReadOnlyEHSClaim.Mode = Me._mode
        udcReadOnlyEHSClaim.SetupTableTitle(Me._intTableTitleWidth)
        Me.Built(udcReadOnlyEHSClaim)
    End Sub

    Private Sub Built(ByVal udcControl As Control)
        If Me.phReadOnlyEHSClaim.FindControl(udcControl.ID) IsNot Nothing Then
            Me.phReadOnlyEHSClaim.Controls.Remove(Me.phReadOnlyEHSClaim.FindControl(udcControl.ID))
        End If
        Me.phReadOnlyEHSClaim.Controls.Add(udcControl)
    End Sub

    Public Sub Clear()
        Me.phReadOnlyEHSClaim.Controls.Clear()
    End Sub
#End Region

#Region "Events"

    Protected Sub udcClaimVaccineReadOnlyText_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent VaccineRemarkClicked(sender, e)
    End Sub


    Protected Sub udcClaimVaccineReadOnly_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

#Region "Property"

    Public Property SchemeCode() As String
        Get
            Return Me._strSchemeCode
        End Get
        Set(ByVal value As String)
            Me._strSchemeCode = value
        End Set
    End Property

    Public Property Mode() As ReadOnlyEHSClaimMode
        Get
            Return Me._mode
        End Get
        Set(ByVal value As ReadOnlyEHSClaimMode)
            Me._mode = value
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


    Public Property TextOnlyVersion() As Boolean
        Get
            Return Me._textOnlyVersion
        End Get
        Set(ByVal value As Boolean)
            Me._textOnlyVersion = value
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

#End Region

End Class