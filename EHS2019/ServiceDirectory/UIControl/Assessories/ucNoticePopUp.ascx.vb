Imports AjaxControlToolkit
Imports System.ComponentModel
Imports Common.ComObject

Partial Public Class ucNoticePopUp
    Inherits System.Web.UI.UserControl

    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    <Serializable()> Public Class NoticeMsg
        Private _strHeaderText As String = String.Empty
        Private _strMessageText As String = String.Empty
        Private _enumNoticeMode As enumNoticeMode = enumNoticeMode.Confirmation
        Private _enumIconMode As enumIconMode = enumIconMode.Information
        Private _enumButtonMode As enumButtonMode = enumButtonMode.YesNo

        Private _blnIsAuditLogEnabled As Boolean = False
        Private _strAuditLogFunctionCode As String = String.Empty

        Public _strAuditLogID_Show As String = String.Empty
        Public _strAuditLogDesc_Show As String = String.Empty
        Public _strAuditLogID_ClickOK As String = String.Empty
        Public _strAuditLogDesc_ClickOK As String = String.Empty
        Public _strAuditLogID_ClickCancel As String = String.Empty
        Public _strAuditLogDesc_ClickCancel As String = String.Empty

        Public Property HeaderText() As String
            Get
                Return _strHeaderText
            End Get
            Set(ByVal value As String)
                _strHeaderText = value
            End Set
        End Property

        Public Property MessageText() As String
            Get
                Return _strMessageText
            End Get
            Set(ByVal value As String)
                _strMessageText = value
            End Set
        End Property

        Public Property NoticeMode() As enumNoticeMode
            Get
                Return _enumNoticeMode
            End Get
            Set(ByVal value As enumNoticeMode)
                _enumNoticeMode = value
            End Set
        End Property

        Public Property IconMode() As enumIconMode
            Get
                Return _enumIconMode
            End Get
            Set(ByVal value As enumIconMode)
                _enumIconMode = value
            End Set
        End Property

        Public Property ButtonMode() As enumButtonMode
            Get
                Return _enumButtonMode
            End Get
            Set(ByVal value As enumButtonMode)
                _enumButtonMode = value
            End Set
        End Property

        Public ReadOnly Property IsAuditLogEnabled() As Boolean
            Get
                Return _blnIsAuditLogEnabled
            End Get
        End Property

        Public ReadOnly Property AuditLogFunctionCode() As String
            Get
                Return _strAuditLogFunctionCode
            End Get
        End Property

        Public Sub New(ByVal udtEnumNoticeMode As enumNoticeMode, ByVal udtEnumButtonMode As enumButtonMode, ByVal strMessageText As String)
            _enumNoticeMode = udtEnumNoticeMode
            _enumButtonMode = udtEnumButtonMode
            _strMessageText = strMessageText
        End Sub

        Public Sub New(ByVal udtEnumIconMode As enumIconMode, ByVal udtEnumButtonMode As enumButtonMode, ByVal strHeaderText As String, ByVal strMessageText As String)
            _enumIconMode = udtEnumIconMode
            _enumButtonMode = udtEnumButtonMode
            _strHeaderText = strHeaderText
            _strMessageText = strMessageText

            _enumNoticeMode = enumNoticeMode.Custom
        End Sub

        Public Sub EnableAuditLog(ByVal strAuditLogFunctionCode As String, _
                                  ByVal strAuditLogID_Show As String, ByVal strAuditLogDesc_Show As String, _
                                  ByVal strAuditLogID_ClickOK As String, ByVal strAuditLogDesc_ClickOK As String, _
                                  ByVal strAuditLogID_ClickCancel As String, ByVal strAuditLogDesc_ClickCancel As String)

            _blnIsAuditLogEnabled = True
            _strAuditLogFunctionCode = strAuditLogFunctionCode

            _strAuditLogID_Show = strAuditLogID_Show
            _strAuditLogDesc_Show = strAuditLogDesc_Show
            _strAuditLogID_ClickOK = strAuditLogID_ClickOK
            _strAuditLogDesc_ClickOK = strAuditLogDesc_ClickOK
            _strAuditLogID_ClickCancel = strAuditLogID_ClickCancel
            _strAuditLogDesc_ClickCancel = strAuditLogDesc_ClickCancel
        End Sub
    End Class

    <Serializable()> Public Class NoticeMsgQueue
        Inherits Queue(Of NoticeMsg)

    End Class
    ' CRE13-003 - Token Replacement [End][Tommy L]

    Private Const VS_BUTTON_MODE As String = "ButtonMode"
    Private Const VS_ICON_MODE As String = "IconMode"
    Private Const VS_NOTICE_MODE As String = "NoticeMode"
    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Const VS_DIALOG_IMAGE_PATH As String = "DialogImagePath"
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    Private _udtNoticeMsg As NoticeMsg


    Public Enum enumButtonClick
        OK
        Cancel
    End Enum

    Public Enum enumButtonMode
        YesNo
        ConfirmCancel
        OK
        OKCancel
        ProceedNotProceed
    End Enum

    Public Enum enumIconMode
        ExclamationIcon
        Information
        Question
    End Enum

    Public Enum enumNoticeMode
        Confirmation
        ExclamationConfirmation
        Notification
        Timeout
        Custom
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

    Public ReadOnly Property PanelPopup() As HtmlTable
        Get
            Return Me.tblPopup
        End Get
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(enumButtonMode.YesNo)> _
    Public Property ButtonMode() As enumButtonMode
        Get
            Return Me.ViewState(VS_BUTTON_MODE)
        End Get
        Set(ByVal value As enumButtonMode)
            Me.ViewState(VS_BUTTON_MODE) = value
            SetupButtonMode()
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(enumIconMode.Information)> _
    Public Property IconMode() As enumIconMode
        Get
            Return Me.ViewState(VS_ICON_MODE)
        End Get
        Set(ByVal value As enumIconMode)
            Me.ViewState(VS_ICON_MODE) = value
            SetupNoticeMode()
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(enumNoticeMode.Confirmation)> _
    Public Property NoticeMode() As enumNoticeMode
        Get
            Return Me.ViewState(VS_NOTICE_MODE)
        End Get
        Set(ByVal value As enumNoticeMode)
            Me.ViewState(VS_NOTICE_MODE) = value
            SetupNoticeMode()
        End Set
    End Property

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    <Bindable(True), Category("Appearance"), DefaultValue("../Images/dialog/")> _
    Public Property DialogImagePath() As String
        Get
            Return Me.ViewState(VS_DIALOG_IMAGE_PATH)
        End Get
        Set(ByVal value As String)
            Me.ViewState(VS_DIALOG_IMAGE_PATH) = value
            SetupDialogImagePath()
        End Set
    End Property
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        SetupNoticeMode()
        SetupButtonMode()
        SetupDialogImagePath()
    End Sub

    Private Sub SetupNoticeMode()
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
            Case enumNoticeMode.Custom
                Select Case IconMode
                    Case enumIconMode.ExclamationIcon
                        Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExclamationIcon")
                    Case enumIconMode.Information
                        Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "InformationIcon")
                    Case enumIconMode.Question
                        Me.imgIcon.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "QuestionMarkIcon")
                End Select
        End Select
    End Sub

    Private Sub SetupButtonMode()
        Me.ibtnCancel.Visible = True

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
            Case enumButtonMode.ProceedNotProceed
                Me.ibtnCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "NotProceedBtn")
                Me.ibtnCancel.AlternateText = Me.GetGlobalResourceObject("AlternateText", "NotProceedBtn")
                Me.ibtnCancel.ToolTip = Me.GetGlobalResourceObject("AlternateText", "NotProceedBtn")
                Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedBtn")
                Me.ibtnOK.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ProceedBtn")
                Me.ibtnOK.ToolTip = Me.GetGlobalResourceObject("AlternateText", "ProceedBtn")
        End Select
    End Sub

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub SetupDialogImagePath()
        Dim strDialogImagePath As String = DialogImagePath

        If strDialogImagePath = String.Empty Then
            strDialogImagePath = "../Images/dialog/"
        End If

        Me.tdHeadingLeft.Style.Add("background-image", "url(" & strDialogImagePath & "top-left.png)")
        Me.tdHeadingTop.Style.Add("background-image", "url(" & strDialogImagePath & "top-mid.png)")
        Me.tdHeadingRight.Style.Add("background-image", "url(" & strDialogImagePath & "top-right.png)")
        Me.tdContentLeft.Style.Add("background-image", "url(" & strDialogImagePath & "left.png)")
        Me.tdContentRight.Style.Add("background-image", "url(" & strDialogImagePath & "right.png)")
        Me.tdFooterLeft.Style.Add("background-image", "url(" & strDialogImagePath & "bottom-left.png)")
        Me.tdFooterBottom.Style.Add("background-image", "url(" & strDialogImagePath & "bottom-mid.png)")
        Me.tdFooterRight.Style.Add("background-image", "url(" & strDialogImagePath & "bottom-right.png)")

    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    Private Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        WriteAuditLog_ClickOK()
        ' CRE13-003 - Token Replacement [End][Tommy L]

        RaiseEvent ButtonClick(enumButtonClick.OK)
    End Sub

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        WriteAuditLog_ClickCancel()
        ' CRE13-003 - Token Replacement [End][Tommy L]

        RaiseEvent ButtonClick(enumButtonClick.Cancel)
    End Sub

    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Public Sub LoadNoticeMsg(ByVal udtNoticeMsg As NoticeMsg)
        _udtNoticeMsg = udtNoticeMsg

        HeaderText = udtNoticeMsg.HeaderText
        MessageText = udtNoticeMsg.MessageText

        Me.ViewState(VS_BUTTON_MODE) = udtNoticeMsg.ButtonMode
        Me.ViewState(VS_ICON_MODE) = udtNoticeMsg.IconMode
        Me.ViewState(VS_NOTICE_MODE) = udtNoticeMsg.NoticeMode

        SetupButtonMode()
        SetupNoticeMode()
    End Sub

    Public Sub ShowPopUp(ByRef actModalPopupExtender As ModalPopupExtender)
        WriteAuditLog_Show()
        actModalPopupExtender.Show()
    End Sub

    Private Sub WriteAuditLog_Show()
        If Not IsNothing(_udtNoticeMsg) AndAlso _udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(_udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(_udtNoticeMsg._strAuditLogID_Show, _udtNoticeMsg._strAuditLogDesc_Show)
        End If
    End Sub

    Private Sub WriteAuditLog_ClickOK()
        If Not IsNothing(_udtNoticeMsg) AndAlso _udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(_udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(_udtNoticeMsg._strAuditLogID_ClickOK, _udtNoticeMsg._strAuditLogDesc_ClickOK)
        End If
    End Sub

    Private Sub WriteAuditLog_ClickCancel()
        If Not IsNothing(_udtNoticeMsg) AndAlso _udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(_udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(_udtNoticeMsg._strAuditLogID_ClickCancel, _udtNoticeMsg._strAuditLogDesc_ClickCancel)
        End If
    End Sub
    ' CRE13-003 - Token Replacement [End][Tommy L]
End Class
