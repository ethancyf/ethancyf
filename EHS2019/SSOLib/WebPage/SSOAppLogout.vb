' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : Logout Base Page
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports System.Text

Public MustInherit Class SSOAppLogout
    Inherits System.Web.UI.Page
    Private strSSOArtifactReceiverUrl As String = Nothing

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)

        If SSOUtil.HttpSessionStateHelper.getSession("SSOArtifactReceiverUrl") IsNot Nothing Then
            strSSOArtifactReceiverUrl = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOArtifactReceiverUrl"), String)
        End If


        AppLogoutHandler()

        redirectToSSOArtifactReceiverUrl()
        MyBase.OnLoad(e)
    End Sub

    Protected Sub redirectToSSOArtifactReceiverUrl()
        If strSSOArtifactReceiverUrl IsNot Nothing Then
            Response.Redirect(strSSOArtifactReceiverUrl)
        End If
    End Sub

    Protected MustOverride Sub AppLogoutHandler()

End Class