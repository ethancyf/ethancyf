Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.Token

Partial Public Class ucInputToken
    Inherits System.Web.UI.UserControl

    Protected Sub ibtnTCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)
        udtAuditLog.AddDescripton("Passcode", txtTPasscode.Text)
        udtAuditLog.WriteLog(LogID.LOG01104, "Input Token Passcode Cancel click")

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
            If (New TokenBLL).AuthenTokenHCVU((New HCVUUserBLL).GetHCVUUser.UserID, txtTPasscode.Text) = False Then
                udcTMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00308)
                imgTPasscode.Visible = True
            End If

        End If
        ' CRE13-029 - RSA server upgrade [End][Lawrence]

        If udcTMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcTMessageBox.BuildMessageBox("ValidationFail", udtAuditLog, LogID.LOG01103, "Input Token Passcode failed")
            Return
        End If

        udtAuditLog.WriteEndLog(LogID.LOG01102, "Input Token Passcode success")

        RaiseEvent Confirm_Click(sender, e)

    End Sub

    ' CRE12-014 - Relax 500 row limit in back office platform [Start][Twinsen]

    Public Sub Build()
        Build(Me.Page)
    End Sub

    Public Sub Build(ByVal page As Page)
        udcTMessageBox.Visible = False
        lblTMessage.Text = _strMessage
        txtTPasscode.Text = String.Empty
        imgTPasscode.Visible = False
        ScriptManager.GetCurrent(page).SetFocus(Me.txtTPasscode)
    End Sub

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

    ''' <summary>
    ''' Display message control
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HeaderText() As String
        Get
            Return Me.lblHeader.Text
        End Get
        Set(ByVal value As String)
            Me.lblHeader.Text = value
        End Set
    End Property

    ' CRE12-014 - Relax 500 row limit in back office platform [End][Twinsen]

    '

    Public WriteOnly Property Message() As String
        Set(ByVal value As String)
            _strMessage = value
        End Set
    End Property

    '

    Private _strMessage As String

    '

    Public Event Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event Confirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

End Class