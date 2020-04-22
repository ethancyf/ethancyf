' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : Access Denied Base Page
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

Public Class SSOAccessDeniedBase
    Inherits System.Web.UI.Page

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        SSOUtil.HttpSessionStateHelper.clearSession()
    End Sub
End Class
