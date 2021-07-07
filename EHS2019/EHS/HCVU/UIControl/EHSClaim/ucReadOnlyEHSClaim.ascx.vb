Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyEHSClaim
    Inherits System.Web.UI.UserControl

#Region "Private Members"

    Private _udtEHSTransaction As EHSTransactionModel
    Private _intWidth As Integer = 200
    Private Const strSessionCollectionKey As String = "__ascx_ReadOnlyEHSClaim_Session"
    Public Const strForceRebuildEHSClaim As String = "__ascx_RebuildReadOnlyEHSClaim"

#End Region

#Region "Private Classes"

    Private Class UserControlPath
        Public Const HCVS As String = "~/UIControl/EHSClaim/ucReadOnlyHCVS.ascx"
        Public Const CIVSS As String = "~/UIControl/EHSClaim/ucReadOnlyCIVSS.ascx"
        Public Const EVSS As String = "~/UIControl/EHSClaim/ucReadOnlyEVSS.ascx"
        Public Const HSIVSS As String = "~/UIControl/EHSClaim/ucReadOnlyHSIVSS.ascx"
        Public Const RVP As String = "~/UIControl/EHSClaim/ucReadOnlyRVP.ascx"
        Public Const RVPCOVID19 As String = "~/UIControl/EHSClaim/ucReadOnlyRVPCOVID19.ascx"
        Public Const HCVS_CHINA As String = "~/UIControl/EHSClaim/ucReadOnlyHCVSCHina.ascx"
        Public Const PIDVSS As String = "~/UIControl/EHSClaim/ucReadOnlyPIDVSS.ascx"
        Public Const VSS As String = "~/UIControl/EHSClaim/ucReadOnlyVSS.ascx"
        Public Const VSSCOVID19 As String = "~/UIControl/EHSClaim/ucReadOnlyVSSCOVID19.ascx" ' CRE20-0023 (Immu record) [Start][Chris YIM]
        Public Const VACCINE As String = "~/UIControl/EHSClaim/ucReadOnlyVaccine.ascx"
        Public Const EHAPP As String = "~/UIControl/EHSClaim/ucReadOnlyEHAPP.ascx"
        Public Const ENHVSSO As String = "~/UIControl/EHSClaim/ucReadOnlyENHVSSO.ascx"
        Public Const PPP As String = "~/UIControl/EHSClaim/ucReadOnlyPPP.ascx"
        Public Const SSSCMC As String = "~/UIControl/EHSClaim/ucReadOnlySSSCMC.ascx"
        Public Const COVID19 As String = "~/UIControl/EHSClaim/ucReadOnlyCOVID19.ascx"  ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
        Public Const COVID19RVP As String = "~/UIControl/EHSClaim/ucReadOnlyCOVID19RVP.ascx"
        Public Const COVID19OR As String = "~/UIControl/EHSClaim/ucReadOnlyCOVID19OR.ascx"

    End Class

#End Region

    Public Sub BuildHCVS()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "HCVS")

        Dim udcReadOnlyHCVS As ucReadOnlyHCVS = Me.LoadControl(UserControlPath.HCVS)

        udcReadOnlyHCVS.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyHCVS)
    End Sub

    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    Public Sub BuildHCVSChina()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "HCVSCHN")
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Dim udcReadOnlyHCVSChina As ucReadOnlyHCVSChina = Me.LoadControl(UserControlPath.HCVS_CHINA)

        udcReadOnlyHCVSChina.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyHCVSChina)
        'CRE13-019-02 Extend HCVS to China [End][Karl]
    End Sub
    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    Public Sub BuildCIVSS()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "CIVSS")

        Dim udcReadOnlyCIVSS As ucReadOnlyCIVSS = Me.LoadControl(UserControlPath.CIVSS)

        udcReadOnlyCIVSS.Build(_udtEHSTransaction)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyCIVSS)

    End Sub

    Public Sub BuildEVSS()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "EVSS")

        Dim udcReadOnlyEVSS As ucReadOnlyEVSS = Me.LoadControl(UserControlPath.EVSS)

        udcReadOnlyEVSS.Build(_udtEHSTransaction)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyEVSS)
    End Sub

    Public Sub BuildHSIVSS()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "HSIVSS")

        Dim udcReadOnlyHSIVSS As ucReadOnlyHSIVSS = Me.LoadControl(UserControlPath.HSIVSS)

        udcReadOnlyHSIVSS.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyHSIVSS)
    End Sub

    Public Sub BuildRVP()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "RVP")

        Dim udcReadOnlyRVP As ucReadOnlyRVP = Me.LoadControl(UserControlPath.RVP)

        udcReadOnlyRVP.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyRVP)
    End Sub

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub BuildRVPCOVID19(ByVal blnShowSubsidizeAmount As Boolean)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "RVPCOVID19")

        Dim udcReadOnlyRVPCOVID19 As ucReadOnlyRVPCOVID19 = Me.LoadControl(UserControlPath.RVPCOVID19)

        udcReadOnlyRVPCOVID19.Build(_udtEHSTransaction, _intWidth, blnShowSubsidizeAmount)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyRVPCOVID19)
    End Sub
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE15-005 PIDVSS [Start][Winnie]
    Public Sub BuildPIDVSS()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "PIDVSS")

        Dim udcReadOnlyPIDVSS As ucReadOnlyPIDVSS = Me.LoadControl(UserControlPath.PIDVSS)

        udcReadOnlyPIDVSS.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyPIDVSS)
    End Sub

    Public Sub BuildVSS()
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "VSS")

        Dim udcReadOnlyVSS As ucReadOnlyVSS = Me.LoadControl(UserControlPath.VSS)

        udcReadOnlyVSS.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyVSS)
    End Sub

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub BuildVSSCOVID19(ByVal blnShowSubsidizeAmount As Boolean)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "VSSCOVID19")

        Dim udcReadOnlyVSSCOVID19 As ucReadOnlyVSSCOVID19 = Me.LoadControl(UserControlPath.VSSCOVID19)

        udcReadOnlyVSSCOVID19.Build(_udtEHSTransaction, _intWidth, blnShowSubsidizeAmount)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyVSSCOVID19)
    End Sub
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE13-001 - EHAPP [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Public Sub BuildEHAPP()
        'Stores all properties in session (only one session, hastable)
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "EHAPP")

        Dim udcReadOnlyEHAPP As ucReadOnlyEHAPP = Me.LoadControl(UserControlPath.EHAPP)

        udcReadOnlyEHAPP.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyEHAPP)
    End Sub
    ' CRE13-001 - EHAPP [End][Tommy L]


    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Sub BuildENHVSSO()
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "ENHVSSO")

        Dim udcReadOnlyENHVSSO As ucReadOnlyENHVSSO = Me.LoadControl(UserControlPath.ENHVSSO)

        udcReadOnlyENHVSSO.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyENHVSSO)
    End Sub
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Sub BuildPPP()
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "PPP")

        Dim udcReadOnlyPPP As ucReadOnlyPPP = Me.LoadControl(UserControlPath.PPP)

        udcReadOnlyPPP.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyPPP)
    End Sub
    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub BuildSSSCMC()
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "SSSCMC")

        Dim udcReadOnlySSSCMC As ucReadOnlySSSCMC = Me.LoadControl(UserControlPath.SSSCMC)

        udcReadOnlySSSCMC.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlySSSCMC)
    End Sub
    ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
    ' --------------------------------------------------------------------------------------
    Public Sub BuildCOVID19()
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "COVID19")

        Dim udcReadOnlyCOVID19 As ucReadOnlyCOVID19 = Me.LoadControl(UserControlPath.COVID19)

        udcReadOnlyCOVID19.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyCOVID19)
    End Sub
    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

    Public Sub BuildCOVID19RVP()
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "COVID19RVP")

        Dim udcReadOnlyCOVID19RVP As ucReadOnlyCOVID19RVP = Me.LoadControl(UserControlPath.COVID19RVP)

        udcReadOnlyCOVID19RVP.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyCOVID19RVP)
    End Sub

    Public Sub BuildCOVID19OR()
        setSessionValue("_udtEHSTransaction", _udtEHSTransaction)
        setSessionValue("_intWidth", _intWidth)
        setSessionValue("EHSClaimBuild", "COVID19OR")

        Dim udcReadOnlyCOVID19OR As ucReadOnlyCOVID19OR = Me.LoadControl(UserControlPath.COVID19OR)

        udcReadOnlyCOVID19OR.Build(_udtEHSTransaction, _intWidth)

        phReadOnlyEHSClaim.Controls.Add(udcReadOnlyCOVID19OR)
    End Sub

    Public Sub Clear()
        phReadOnlyEHSClaim.Controls.Clear()

        'Clear Session
        Me.clearSession()
    End Sub

#Region "Property"

    Public WriteOnly Property EHSTransaction() As EHSTransactionModel
        Set(ByVal value As EHSTransactionModel)
            _udtEHSTransaction = value
        End Set
    End Property

    Public WriteOnly Property Width() As Integer
        Set(ByVal value As Integer)
            _intWidth = value
        End Set
    End Property

#End Region

#Region "Internal Session Handling"

    Private Sub setSessionValue(ByVal strKey As String, ByVal objValue As Object)

        Dim objInternalSession As Hashtable = Nothing
        Dim objSession As HttpSessionState = Nothing

        objSession = HttpContext.Current.Session

        If objSession(strSessionCollectionKey) Is Nothing Then
            objInternalSession = New System.Collections.Hashtable(100)
            objInternalSession.Add(strKey, objValue)
            objSession(strSessionCollectionKey) = objInternalSession
        Else
            objInternalSession = CType(objSession(strSessionCollectionKey), Hashtable)
            If (objInternalSession.ContainsKey(strKey)) Then
                objInternalSession(strKey) = objValue
            Else
                objInternalSession.Add(strKey, objValue)
            End If
        End If

    End Sub

    Private Function getSession(ByVal strKey As String)

        Dim objInternalSession As Hashtable = Nothing
        Dim objSession As HttpSessionState = Nothing

        objSession = HttpContext.Current.Session

        If Not (objSession(strSessionCollectionKey) Is Nothing) Then
            objInternalSession = CType(objSession(strSessionCollectionKey), Hashtable)
        End If

        If (objInternalSession Is Nothing) Then
            Return Nothing
        End If

        Return objInternalSession(strKey)

    End Function

    Private Sub clearSession()
        If Not IsNothing(Session(strSessionCollectionKey)) Then
            Session(strSessionCollectionKey) = Nothing
        End If
    End Sub
#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If phReadOnlyEHSClaim.Controls.Count = 0 AndAlso Not IsNothing(Session(ucReadOnlyEHSClaim.strForceRebuildEHSClaim)) _
        AndAlso Session(ucReadOnlyEHSClaim.strForceRebuildEHSClaim) = "Y" Then
            'Retrieve all properties
            Me._udtEHSTransaction = getSession("_udtEHSTransaction")
            Me._intWidth = getSession("_intWidth")

            Select Case getSession("EHSClaimBuild")
                Case SchemeClaimModel.HCVS
                    Me.BuildHCVS()
                Case SchemeClaimModel.HCVSCHN
                    Me.BuildHCVSChina()
                Case SchemeClaimModel.CIVSS
                    Me.BuildCIVSS()
                Case SchemeClaimModel.EVSS
                    Me.BuildEVSS()
                Case SchemeClaimModel.RVP
                    Me.BuildRVP()
                Case SchemeClaimModel.HSIVSS
                    Me.BuildHSIVSS()
                Case SchemeClaimModel.EHAPP
                    Me.BuildEHAPP()
                Case SchemeClaimModel.PIDVSS
                    Me.BuildPIDVSS()
                Case SchemeClaimModel.VSS
                    Me.BuildVSS()
                Case SchemeClaimModel.SSSCMC
                    Me.BuildSSSCMC()
                    ' CRE20-0023 (Immu record) [Start][Winnie SUEN]
                Case SchemeClaimModel.COVID19CVC, _
                    SchemeClaimModel.COVID19DH, _
                    SchemeClaimModel.COVID19SR,
                    SchemeClaimModel.COVID19SB
                    Me.BuildCOVID19()
                    ' CRE20-0023 (Immu record) [End][Winnie SUEN]
                Case SchemeClaimModel.COVID19RVP
                    Me.BuildCOVID19RVP()
                Case SchemeClaimModel.COVID19OR
                    Me.BuildCOVID19OR()                    
            End Select

            Session(ucReadOnlyEHSClaim.strForceRebuildEHSClaim) = Nothing
        End If

    End Sub

End Class