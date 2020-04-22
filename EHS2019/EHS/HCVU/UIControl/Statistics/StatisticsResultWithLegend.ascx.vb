Imports Common.ComObject
Imports Common.Component

Partial Public Class StatisticsResultWithLegend
    Inherits System.Web.UI.UserControl

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const OpenLegend_Clicked_ID As String = LogID.LOG00046
        Public Const OpenLegend_Clicked As String = "Open Legend Clicked"

        Public Const CloseLegend_Clicked_ID As String = LogID.LOG00047
        Public Const CloseLegend_Clicked As String = "Close Legend Clicked"
    End Class
#End Region

    Dim _udtAuditLogEntry As AuditLogEntry = New AuditLogEntry("010703")
    Dim _strLegendType As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Public Sub SetDisplayText(ByVal strTitle As String, ByVal strValue As String)
        lblParameterTitle.Text = strTitle.Trim
        lblParameterValue.Text = strValue.Trim
    End Sub

    Public Sub SetInfoBtnOn(ByVal strLegendType As String)
        Select Case strLegendType
            Case "District"
                ibtnOpenLegend.Visible = True
                _strLegendType = "District"
            Case "Profession"
                ibtnOpenLegend.Visible = True
                _strLegendType = "Profession"
            Case Else
                ' Nothing
        End Select
    End Sub

    Public Sub ibtnOpenLegend_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOpenLegend.Click
        _udtAuditLogEntry.WriteLog(AuditLogDesc.OpenLegend_Clicked_ID, AuditLogDesc.OpenLegend_Clicked)

        Select Case _strLegendType
            Case "District"
                popupDistrictLegend.Show()
                udcDistrictLegend.BindDistrict(Session("language"))
            Case "Profession"
                popupProfessionLegend.Show()
                udcProfessionLegend.BindProfession(Session("language"))
            Case Else
        End Select
    End Sub

    Public Sub ibtnCloseProfessionLegend_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseProfessionLegend.Click
        _udtAuditLogEntry.WriteLog(AuditLogDesc.CloseLegend_Clicked_ID, AuditLogDesc.CloseLegend_Clicked)
        popupProfessionLegend.Hide()
    End Sub

    Public Sub ibtnCloseDistrictLegend_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDistrictLegend.Click
        _udtAuditLogEntry.WriteLog(AuditLogDesc.CloseLegend_Clicked_ID, AuditLogDesc.CloseLegend_Clicked)
        popupDistrictLegend.Hide()
    End Sub

End Class