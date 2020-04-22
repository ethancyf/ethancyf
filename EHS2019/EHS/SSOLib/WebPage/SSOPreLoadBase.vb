' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : Pre Load Base Page (To cater prompt cert first if cert not trusted by client)
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

Namespace SSOLib
	Public Class SSOPreLoadBase
		Inherits System.Web.UI.Page
		Protected Overrides Sub OnLoad(e As EventArgs)
			Dim strSSO_URL As String = ""
			strSSO_URL = Request.QueryString("SSO_Artifact_Generator_Url")

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check query string SSO_Artifact_Generator_Url is null or not at Page_Load() in SSOPreLoad.aspx"))
			If strSSO_URL Is Nothing Then

				SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to clear session variable if query string SSO_Artifact_Generator_Url is null at Page_Load() in SSOPreLoad.aspx"))

				SSOUtil.HttpSessionStateHelper.clearSession()
				Return
			End If
			Response.Redirect(strSSO_URL)

            'base.OnLoad(e);
		End Sub
	End Class
End Namespace