Imports Common.ComFunction
Imports Common.Encryption
Imports Common.ComObject
Imports Common.Component

Partial Public Class DataEntryRecoverLogin
    Inherits BasePage

    Dim udcGeneralFun As New GeneralFunction
    Dim udcValidator As New Common.Validation.Validator
    Dim udcForgotPasswordBll As New BLL.ForgotPasswordBLL
    Const FUNCTION_CODE As String = "020002"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = Now().Subtract(New TimeSpan(1, 0, 0, 0))
        Response.Expires = 0
        Response.CacheControl = "no-cache"

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Not IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Data Entry Recover Login Load")
        End If


    End Sub

    Protected Sub btn_backToLogin_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_backToLogin.Click
        Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
    End Sub

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
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
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
End Class