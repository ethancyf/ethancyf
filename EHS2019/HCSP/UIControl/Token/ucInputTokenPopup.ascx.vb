Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.Token

Partial Public Class ucInputTokenPopup
    Inherits System.Web.UI.UserControl

    Protected Sub ibtnTCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)
        udtAuditLog.AddDescripton("Passcode", txtTPasscode.Text)
        udtAuditLog.WriteLog(LogID.LOG01104, "Input Token Passcode Cancel click")
        udcTMessageBox.Visible = False
        imgTPasscode.Visible = False
        RaiseEvent Cancel_Click(sender, e)
    End Sub

    Protected Sub ibtnTConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcTMessageBox.Visible = False
        imgTPasscode.Visible = False

        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)
        udtAuditLog.AddDescripton("Passcode", txtTPasscode.Text)
        udtAuditLog.WriteStartLog(LogID.LOG01101, "Input Token Passcode Confirm click")

        udtAuditLog.AddDescripton("Passcode", txtTPasscode.Text)

        ' CRE13-029 - RSA server upgrade [Start][Lawrence]
        If txtTPasscode.Text = String.Empty Then
            udcTMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00044)
            imgTPasscode.Visible = True

        Else
            If (New TokenBLL).AuthenTokenHCSP(CType(UserAC.UserACBLL.GetUserAC(), ServiceProvider.ServiceProviderModel).SPID, txtTPasscode.Text) = False Then
                udcTMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00308)
                imgTPasscode.Visible = True
            End If

        End If
        ' CRE13-029 - RSA server upgrade [End][Lawrence]

        If udcTMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcTMessageBox.BuildMessageBox("ValidationFail", udtAuditLog, LogID.LOG01103, "Input Token Passcode failed")
            RaiseEvent Failure_Click(sender, e)
            Return
        Else
            RaiseEvent Success_Click(sender, e)
        End If

        udtAuditLog.WriteEndLog(LogID.LOG01102, "Input Token Passcode success")

        'RaiseEvent Confirm_Click(sender, e)

    End Sub

    Public Sub Build()
        udcTMessageBox.Visible = False
        'lblTMessage.Text = _strMessage
        txtTPasscode.Text = String.Empty
        imgTPasscode.Visible = False
        ScriptManager.GetCurrent(Me.Page).SetFocus(Me.txtTPasscode)
    End Sub

    Public Property TitleText() As String
        Get
            Return Me.lblTitle.Text
        End Get
        Set(ByVal value As String)
            Me.lblTitle.Text = value
        End Set
    End Property

    Public Property Message() As String
        Get
            Return lblTMessage.Text
        End Get
        Set(ByVal value As String)
            lblTMessage.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Use for handle mouse click and move popup numpad (ModalPopupExtender)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Header() As Control
        Get
            Return Me.panHeader
        End Get
    End Property

    '

    Private _strMessage As String

    '

    Public Event Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event Success_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event Failure_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

End Class