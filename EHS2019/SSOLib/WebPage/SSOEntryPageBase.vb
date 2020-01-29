' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : SSO Entry Base Page
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

Public MustInherit Class SSOEntryPageBase
    Inherits System.Web.UI.Page

    Protected strLocalSSOAppId As String = ""


    Private Sub loadConfig()
        strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()

    End Sub


    Protected Overrides Sub OnLoad(ByVal e As EventArgs)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        Dim objSSOAssertion As SSODataType.SSOAssertion = Nothing

        loadConfig()

        If Not Me.IsPostBack Then

            Dim strSSOTxnId As String = Request.QueryString("SSOTxnId")

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check SSOTxnId is null or not at Page_Load() in SSOEntryPage.aspx.cs"))
            If strSSOTxnId Is Nothing Then

                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to clear session variable if SSOTxnId is null at Page_Load() in SSOEntryPage.aspx.cs"))

                SSOUtil.HttpSessionStateHelper.clearSession()
                Return
            End If

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get session varibale SSOReceivedAssertion at Page_Load() in SSOEntryPage.aspx.cs"))

            objSSOAssertion = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOReceivedAssertion" + strSSOTxnId), SSODataType.SSOAssertion)

            If objSSOAssertion Is Nothing Then
                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to clear session variable if SSOReceivedAssertion is null at Page_Load() in SSOEntryPage.aspx.cs"))

                SSOUtil.HttpSessionStateHelper.clearSession()
                Return
            End If


            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get session varibale SSOCustomizedContent at Page_Load() in SSOEntryPage.aspx.cs"))

            Dim objSSOCustomizedContent As SSODataType.SSOCustomizedContent = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOCustomizedContent" + strSSOTxnId), SSODataType.SSOCustomizedContent)
            If objSSOCustomizedContent Is Nothing Then
                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to clear session variable if SSOCustomizedContent is null at Page_Load() in SSOEntryPage.aspx.cs"))

                SSOUtil.HttpSessionStateHelper.clearSession()
                Return
            End If

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to clear session variable at Page_Load() in SSOEntryPage.aspx.cs"))
            SSOUtil.HttpSessionStateHelper.clearSession()

            FirstEnter(objSSOAssertion)

        End If

        'perform application logon logic with the information from objSSOCustomizedContent
        'AppLogicHandler(objSSOAssertion)

        'SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to make a Http redirect to SSOApplicationMainPageUrl at Page_Load() in SSOEntryPage.aspx.cs"))
        'Response.Redirect(SSOUtil.SSOAppConfigMgr.getSSOApplicationMainPageUrl())

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub


    'perform application logon logic with the information from objSSOCustomizedContent
    Protected MustOverride Sub AppLogicHandler(ByVal objSSOAssertion As SSODataType.SSOAssertion)

    Protected MustOverride Sub FirstEnter(ByVal objSSOAssertion As SSODataType.SSOAssertion)

End Class