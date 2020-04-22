Imports Common.ComFunction
Imports Common.ComObject
Imports Common.OCSSS
Imports Common.Format

Public Class LoadTest
    Inherits System.Web.UI.Page

#Region "Private Variables"

    Private Const CONST_SYS_PARAM_OCSSS_WS_Link As String = "OCSSS_WS_Link"
    Private Const CONST_SYS_PARAM_OCSSS_WS_PassPhrase As String = "OCSSS_WS_PassPhrase"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnCallOCSSS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCallOCSSS.Click
        Dim dtmStart As DateTime = DateTime.MinValue
        Dim dtmEnd As DateTime = DateTime.MinValue

        Me.lblResultCallOCSSS.Text = String.Empty

        If txtHKID.Text = String.Empty Then
            lblResultCallOCSSS.Text = "Please input HKID"
            Return
        End If

        Dim strHKID As String = String.Empty

        If txtHKID.Text <> String.Empty Then
            If (New Regex("^.{8,9}$")).IsMatch(txtHKID.Text.Trim) = False Then
                lblResultCallOCSSS.Text = "Invalid HKID"
                Return
            End If

            strHKID = (New Formatter).formatHKIDInternal(txtHKID.Text)

        End If

        Try
            Dim udtGeneralFunction As New GeneralFunction

            Dim strWSLink As String = udtGeneralFunction.getSystemParameterValue1(CONST_SYS_PARAM_OCSSS_WS_Link)
            Dim strWSPassPhrase As String = udtGeneralFunction.getSystemParameterValue1(CONST_SYS_PARAM_OCSSS_WS_PassPhrase)

            dtmStart = Now

            Dim udtOCSSSServiceBLL As New OCSSSServiceBLL()
            Dim udtOCSSSResult As OCSSSResult = udtOCSSSServiceBLL.IsEligibleForInternalUse(strWSLink, strWSPassPhrase, strHKID)

            dtmEnd = Now
            ShowResult(Me.lblResultCallOCSSS, dtmStart, dtmEnd, udtOCSSSResult.Exception)

        Catch ex As Exception
            dtmEnd = Now
            ShowResult(Me.lblResultCallOCSSS, dtmStart, dtmEnd, ex)
        End Try

    End Sub

    Protected Sub ShowResult(ByVal lbl As Label, ByVal dtmStart As DateTime, ByVal dtmEnd As DateTime, ByVal ex As Exception)
        Dim dtmDiff As DateTime = New DateTime(dtmEnd.Subtract(dtmStart).Ticks)
        lbl.Text = "Time Elapsed: " + dtmDiff.ToString("mm:ss:fff")
        lbl.Text += "<br/>"

        If ex IsNot Nothing Then
            lbl.Text += "Result: Fail<br/>"
            lbl.Text += String.Format("Message: {0}<br/>", ex.Message)
            lbl.Text += String.Format("Stack Trace: {0}<br/>", ex.StackTrace)
        Else
            lbl.Text += "Result: Success"
        End If
    End Sub

End Class