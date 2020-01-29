Imports System.ComponentModel
Imports Common.Component
Imports Common.ComObject

Partial Public Class ucPCDWarningPopup
    Inherits System.Web.UI.UserControl

#Region "Constants"
    Public Class WarningType
        Public Const Professional As String = "P"
        Public Const Delisted As String = "D"        
        Public Const NotEnrolled As String = "N"
        ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Const Suspended As String = "S"
        ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]
    End Class
#End Region

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
    Public ReadOnly Property MessageLabel() As Label
        Get
            Return Me.lblMsg
        End Get
    End Property

    Public Property MessageText() As String
        Get
            Return Me.lblMsg.Text
        End Get
        Set(ByVal value As String)
            Me.lblMsg.Text = value
        End Set
    End Property

    Public Property CheckboxMessageText() As String
        Get
            Return Me.chkConfirm.Text
        End Get
        Set(ByVal value As String)
            Me.chkConfirm.Text = value
        End Set
    End Property

    Private _blnShowCheckBox As Boolean = True

    <Bindable(True), Category("Appearance"), DefaultValue(True)> _
    Public Property ShowCheckbox() As Boolean
        Get
            Return Me._blnShowCheckBox
        End Get
        Set(ByVal value As Boolean)
            Me._blnShowCheckBox = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub Build(ByVal strWarningType As String)
        udcMessageBox.Visible = False
        imgConfirm.Visible = False
        chkConfirm.Checked = False

        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)
        udtAuditLog.AddDescripton("WarningType", strWarningType)
        udtAuditLog.WriteLog(LogID.LOG01131, "Show PCD Warning Popup")

        hfPCDWarningType.Value = strWarningType

        Select Case strWarningType
            Case WarningType.Professional
                trConfirmCheckbox.Visible = Me.ShowCheckbox
                If Me.ShowCheckbox Then
                    Me.CheckboxMessageText = HttpContext.GetGlobalResourceObject("Text", "EnrolVaccineSchemeEligible")
                End If

            Case WarningType.Delisted
                trConfirmCheckbox.Visible = Me.ShowCheckbox

                Me.MessageText = HttpContext.GetGlobalResourceObject("Text", "PCDDelistedMessage")
                If Me.ShowCheckbox Then
                    Me.CheckboxMessageText = HttpContext.GetGlobalResourceObject("Text", "JoinPCDEligible")
                End If

            Case WarningType.NotEnrolled
                trConfirmCheckbox.Visible = False
                Me.MessageText = Me.GetGlobalResourceObject("Text", "PCDNotEnrolledMessage")

                ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case WarningType.Suspended
                trConfirmCheckbox.Visible = Me.ShowCheckbox

                Me.MessageText = HttpContext.GetGlobalResourceObject("Text", "PCDSuspendedMessage")
                If Me.ShowCheckbox Then
                    Me.CheckboxMessageText = HttpContext.GetGlobalResourceObject("Text", "PCDEnrollVSSEligible")
                End If
                ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]
        End Select
    End Sub

    Private Sub ibtnConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirm.Click
        udcMessageBox.Visible = False
        imgConfirm.Visible = False

        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)
        udtAuditLog.WriteStartLog(LogID.LOG01132, "Show PCD Warning Popup - Confirm Click")

        If trConfirmCheckbox.Visible = False OrElse Me.chkConfirm.Checked Then
            udtAuditLog.WriteEndLog(LogID.LOG01133, "Show PCD Warning Popup - Confirm Click Success")
            RaiseEvent Success_Click(sender, e)
            Return
        Else
            Select Case hfPCDWarningType.Value
                Case WarningType.Delisted
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00416)

                Case WarningType.Professional
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00417)

                    ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case WarningType.Suspended
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00423)
                    ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]
            End Select
        End If

        udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLog, LogID.LOG01134, "Show PCD Warning Popup - Confirm Click fail")
        imgConfirm.Visible = True

        RaiseEvent Failure_Click(sender, e)
        Return        
    End Sub

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)
        udtAuditLog.WriteLog(LogID.LOG01135, "Show PCD Warning Popup - Cancel Click")

        RaiseEvent Cancel_Click(sender, e)
    End Sub

    Public Event Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event Success_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event Failure_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
End Class