Imports System.ComponentModel

Partial Public Class ucNoticePopUp
    Inherits System.Web.UI.UserControl

    Public Enum enumButtonClick
        OK
        Cancel
    End Enum

    Public Enum enumButtonMode
        YesNo
        ConfirmCancel
        OK
        OKCancel
    End Enum

    Public Enum enumIconMode
        ExclamationIcon
        Information
        Question
        None
    End Enum

    Public Enum enumNoticeMode
        Confirmation
        ExclamationConfirmation
        Notification
        Remark
        Timeout
        Custom
        Attention
    End Enum

    Public Enum enumMsgAlign
        Left
        Center
        Right
    End Enum


    Public Event ButtonClick(ByVal e As enumButtonClick)

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

    Public ReadOnly Property ButtonOK() As ImageButton
        Get
            Return Me.ibtnOK
        End Get
    End Property

    Public ReadOnly Property ButtonCancel() As ImageButton
        Get
            Return Me.ibtnCancel
        End Get
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(enumButtonMode.YesNo)> _
    Public Property ButtonMode() As enumButtonMode
        Get
            Return Me.ViewState("ButtonMode")
        End Get
        Set(ByVal value As enumButtonMode)
            Me.ViewState("ButtonMode") = value
            SetupButtonMode()
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(enumIconMode.Information)> _
    Public Property IconMode() As enumIconMode
        Get
            Return Me.ViewState("IconMode")
        End Get
        Set(ByVal value As enumIconMode)
            Me.ViewState("IconMode") = value
            SetupNoticeMode()
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(enumNoticeMode.Confirmation)> _
    Public Property NoticeMode() As enumNoticeMode
        Get
            Return Me.ViewState("NoticeMode")
        End Get
        Set(ByVal value As enumNoticeMode)
            Me.ViewState("NoticeMode") = value
            SetupNoticeMode()
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(enumMsgAlign.Left)> _
    Public Property MessageAlignment() As enumMsgAlign
        Get
            Return Me.ViewState("MessageAlignment")
        End Get
        Set(ByVal value As enumMsgAlign)
            Me.ViewState("MessageAlignment") = value
        End Set
    End Property

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    <Bindable(True), Category("Appearance"), DefaultValue("100%")> _
    Public Property MessageWidth() As String
        Get
            Return Me.ViewState("MessageWidth")
        End Get
        Set(ByVal value As String)
            Me.ViewState("MessageWidth") = value
        End Set
    End Property
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SetupNoticeMode()
        SetupButtonMode()
        SetupMessageAlignment()
    End Sub

    Private Sub SetupNoticeMode()
        Me.imgIcon.Visible = True

        Select Case NoticeMode
            Case enumNoticeMode.Confirmation
                Me.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmBoxTitle")
                Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "QuestionMarkIcon")
            Case enumNoticeMode.ExclamationConfirmation
                Me.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmBoxTitle")
                Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExclamationIcon")
            Case enumNoticeMode.Notification
                Me.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
                Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "InformationIcon")
            Case enumNoticeMode.Timeout
                Me.HeaderText = Me.GetGlobalResourceObject("Text", "TimeoutReminderTitle")
                Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExclamationIcon")
            Case enumNoticeMode.Remark
                Me.HeaderText = Me.GetGlobalResourceObject("Text", "Remark")
                Me.imgIcon.Visible = False
            Case enumNoticeMode.Attention
                Me.HeaderText = Me.GetGlobalResourceObject("Text", "Attention")
                Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExclamationIcon")
            Case enumNoticeMode.Custom
                Select Case IconMode
                    Case enumIconMode.ExclamationIcon
                        Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExclamationIcon")
                    Case enumIconMode.Information
                        Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "InformationIcon")
                    Case enumIconMode.Question
                        Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "QuestionMarkIcon")
                    Case enumIconMode.None
                        Me.imgIcon.Visible = False
                End Select
        End Select
    End Sub

    Private Sub SetupButtonMode()
        Select Case ButtonMode
            Case enumButtonMode.YesNo
                Me.ibtnCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NoBtn")
                Me.ibtnCancel.AlternateText = Me.GetGlobalResourceObject("AlternateText", "NoBtn")
                Me.ibtnCancel.ToolTip = Me.GetGlobalResourceObject("AlternateText", "NoBtn")
                Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "YesBtn")
                Me.ibtnOK.AlternateText = Me.GetGlobalResourceObject("AlternateText", "YesBtn")
                Me.ibtnOK.ToolTip = Me.GetGlobalResourceObject("AlternateText", "YesBtn")
            Case enumButtonMode.ConfirmCancel
                Me.ibtnCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelBtn")
                Me.ibtnCancel.AlternateText = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
                Me.ibtnCancel.ToolTip = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
                Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
                Me.ibtnOK.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
                Me.ibtnOK.ToolTip = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")
            Case enumButtonMode.OK
                Me.ibtnCancel.Visible = False
                Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "OKBtn")
                Me.ibtnOK.AlternateText = Me.GetGlobalResourceObject("AlternateText", "OKBtn")
                Me.ibtnOK.ToolTip = Me.GetGlobalResourceObject("AlternateText", "OKBtn")
            Case enumButtonMode.OKCancel
                Me.ibtnCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelBtn")
                Me.ibtnCancel.AlternateText = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
                Me.ibtnCancel.ToolTip = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
                Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "OKBtn")
                Me.ibtnOK.AlternateText = Me.GetGlobalResourceObject("AlternateText", "OKBtn")
                Me.ibtnOK.ToolTip = Me.GetGlobalResourceObject("AlternateText", "OKBtn")
        End Select
    End Sub

    Private Sub SetupMessageAlignment()
        Select Case MessageAlignment
            Case enumMsgAlign.Left
                lblMsg.Style("text-align") = "left"
            Case enumMsgAlign.Center
                lblMsg.Style("text-align") = "center"
            Case enumMsgAlign.Right
                lblMsg.Style("text-align") = "right"
        End Select

        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        lblMsg.Style("width") = MessageWidth
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
    End Sub

    Private Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        RaiseEvent ButtonClick(enumButtonClick.OK)
    End Sub

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        RaiseEvent ButtonClick(enumButtonClick.Cancel)
    End Sub
End Class