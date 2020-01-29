Imports Common.Component.UserAC
Imports Common.ComObject

Partial Public Class Home_Text
    Inherits TextOnlyBasePage

    Dim udtUserAC As UserACModel = New UserACModel
    Dim strFuncCode As String = Common.Component.FunctCode.FUNT020007
    Dim udtAuditLogEntry As AuditLogEntry

    Public Class AublitLogDescription
        Public Const HomeLoaded As String = "Home loaded"
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtUserAC As UserACModel = New UserACModel
        udtUserAC = UserACBLL.GetUserAC
        'SetLangage()
        Dim masterPage As ClaimVoucherMaster = CType(Me.Master, ClaimVoucherMaster)

        masterPage.BuildMenu(strFuncCode, Session("language"), udtUserAC)

        If Not Me.IsPostBack Then

            'Log Page Load
            Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode)
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00000, AublitLogDescription.HomeLoaded)

        End If

        'Initialize MasterPage
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Visible = False
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblTitle), Label).Text = Me.GetGlobalResourceObject("Text", "EVoucherSystem")
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblSubTitle), Label).Text = Me.GetGlobalResourceObject("Text", "HomePage")
    End Sub

#Region "Implement IWorkingData (CRE11-004)"

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

#End Region
End Class