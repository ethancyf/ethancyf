Imports AjaxControlToolkit
Imports Common.ComObject
Imports System.ComponentModel
Imports System.Web.Security.AntiXss
'CRE20-006 call the sessionhandler [Start][Nichole]
Imports HCSP.BLL
'CRE20-006 call the sessionhandler [End][Nichole]

Partial Public Class ucNoticePopUp
    Inherits System.Web.UI.UserControl

#Region "Constants"
    Private Const VS_BUTTON_MODE As String = "ButtonMode"
    Private Const VS_ICON_MODE As String = "IconMode"
    Private Const VS_NOTICE_MODE As String = "NoticeMode"
    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Const VS_DIALOG_IMAGE_PATH As String = "DialogImagePath"
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Const VS_CHECKBOX_CHECKED As String = "CheckBoxChecked"
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    Public Enum enumButtonClick
        OK
        Cancel
    End Enum

    Public Enum enumButtonMode
        YesNo
        ConfirmCancel
        ConfirmCancelWithReason
        OK
        Cancel
        OKCancel
        ProceedNotProceed
        Close
        Custom
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

    Public Enum enumCheckBoxClick
        Unchecked
        Checked
    End Enum

#End Region

#Region "Private Members"
    Private _udtNoticeMsg As NoticeMsg
    Private _strCustomHeaderText As String

#End Region

#Region "Register Events"
    Public Event ButtonClick(ByVal e As enumButtonClick)
    Public Event CheckBoxClick(ByVal e As enumCheckBoxClick)

#End Region

#Region "Properties"
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

    Public Property CustomHeaderText() As String
        Get
            Return Me.lblHeader.Text
        End Get
        Set(ByVal value As String)
            Me.lblHeader.Text = value
        End Set
    End Property

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Property DeclarationText() As String
        Get
            Return Me.divDeclaration.InnerHtml
        End Get
        Set(ByVal value As String)
            Me.divDeclaration.InnerHtml = value
        End Set
    End Property
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Property ShowDeclaration() As Boolean
        Get
            Return Me.panDeclaration.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.panDeclaration.Visible = value
        End Set
    End Property
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property ShowEnquiryDesc() As Boolean
        Get
            Return Me.panEnquiry.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.panEnquiry.Visible = value
        End Set
    End Property
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property EnquiryDescText() As String
        Get
            Return Me.divEnquiryDesc.InnerHtml
        End Get
        Set(ByVal value As String)
            Me.divEnquiryDesc.InnerHtml = value
        End Set
    End Property
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property ShowOverrideReason() As Boolean
        Get
            Return Me.panOverrideReason.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.panOverrideReason.Visible = value
        End Set
    End Property
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property OverrideReasonDescText() As String
        Get
            Return Me.divORReasonDesc.InnerHtml
        End Get
        Set(ByVal value As String)
            Me.divORReasonDesc.InnerHtml = value
        End Set
    End Property
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property OverrideReasonText() As String
        Get
            Return Me.lblORReason.Text
        End Get
        Set(ByVal value As String)
            Me.lblORReason.Text = value
        End Set
    End Property
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property OverrideReason() As String
        Get
            Return Me.txtORReason.Text
        End Get
        Set(ByVal value As String)
            Me.txtORReason.Text = value
        End Set
    End Property
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property OverrideReasonMsgBoxErr() As CustomControls.MessageBox
        Get
            Return udcMsgBoxErr
        End Get
    End Property
    ' CRE20-0023 (Immu record) [End][Chris YIM]

    ' CRE20-0023 (Immu record) [Start][Raiman Chong]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property EnableOverrideReasonTextFilter() As Boolean
        Get
            Return Me.fteORReasonBlockVerticalBarAndBackslash.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me.fteORReasonBlockVerticalBarAndBackslash.Enabled = value
        End Set
    End Property
    ' CRE20-0023 (Immu record) [End][Raiman Chong]

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

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    <Bindable(True), Category("Appearance"), DefaultValue(enumCheckBoxClick.Unchecked)> _
    Public Property CheckBoxChecked() As enumCheckBoxClick
        Get
            Return Me.ViewState(VS_CHECKBOX_CHECKED)
        End Get
        Set(ByVal value As enumCheckBoxClick)
            Me.ViewState(VS_CHECKBOX_CHECKED) = value
            SetDisableButton()
        End Set
    End Property
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SetupNoticeMode()
        SetupButtonMode()
        SetupDialogImagePath()
        SetupCheckBox()
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

    Private Sub SetupButtonMode(Optional ByVal udtNoticeMsg As NoticeMsg = Nothing)
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
            Case enumButtonMode.ConfirmCancelWithReason
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
            Case enumButtonMode.Cancel
                Me.ibtnCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelBtn")
                Me.ibtnCancel.AlternateText = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
                Me.ibtnCancel.ToolTip = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
                Me.ibtnOK.Visible = False
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
            Case enumButtonMode.Close
                Me.ibtnCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CloseBtn")
                Me.ibtnCancel.AlternateText = Me.GetGlobalResourceObject("AlternateText", "CloseBtn")
                Me.ibtnCancel.ToolTip = Me.GetGlobalResourceObject("AlternateText", "CloseBtn")
                Me.ibtnOK.Visible = False
            Case enumButtonMode.Custom
                Me.ibtnCancel.Visible = False
                If Not udtNoticeMsg Is Nothing Then
                    Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", udtNoticeMsg.CustomBtnImageResource)
                    Me.ibtnOK.AlternateText = Me.GetGlobalResourceObject("AlternateText", udtNoticeMsg.CustomBtnImageResource)
                    Me.ibtnOK.ToolTip = Me.GetGlobalResourceObject("AlternateText", udtNoticeMsg.CustomBtnImageResource)
                End If

        End Select

        SetDisableButton(udtNoticeMsg)

    End Sub

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub SetDisableButton(Optional ByVal udtNoticeMsg As NoticeMsg = Nothing)
        If ShowDeclaration AndAlso CheckBoxChecked = enumCheckBoxClick.Unchecked Then
            Me.ibtnOK.Enabled = False

            Select Case ButtonMode
                Case enumButtonMode.YesNo
                    Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "YesDisableBtn")

                Case enumButtonMode.ConfirmCancel, enumButtonMode.ConfirmCancelWithReason
                    Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")

                Case enumButtonMode.OK
                    Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "OKDisableBtn")

                Case enumButtonMode.Cancel
                    'N/A

                Case enumButtonMode.OKCancel
                    Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "OKDisableBtn")

                Case enumButtonMode.Close
                    'N/A

                Case enumButtonMode.ProceedNotProceed
                    Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedDisableBtn")

                Case enumButtonMode.Custom
                    If Not udtNoticeMsg Is Nothing Then
                        Me.ibtnOK.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", udtNoticeMsg.CustomDisableBtnImageResource)
                    End If

            End Select

        Else
            Me.ibtnOK.Enabled = True

        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

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

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub SetupCheckBox()
        If ShowDeclaration Then
            panDeclaration.Visible = True

        Else
            panDeclaration.Visible = False

        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]
#End Region

#Region "Events"
    Private Sub ibtnOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOK.Click
        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        WriteAuditLog_ClickOK()
        ' CRE13-003 - Token Replacement [End][Tommy L]

        'CRE20-006 DHC integration - user cancel to create the new claim account, the browswer will close [Start][Nichole]
        Dim udtDHCClient As Common.Component.DHCClaim.DHCClaimBLL.DHCPersonalInformationModel = (New SessionHandler).DHCInfoGetFromSession()

        If udtDHCClient Is Nothing Then
            RaiseEvent ButtonClick(enumButtonClick.OK)
        Else
            If ibtnOK.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ProceedBtn") Then
                RaiseEvent ButtonClick(enumButtonClick.OK)
            Else
                'Clear session 
                Session.RemoveAll()
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "VoucherCloseBrowswerScript", "javascript:window.close();", True)
            End If
        End If
        'CRE20-006 DHC integration - user cancel to create the new claim account, the browswer will close [End][Nichole]

    End Sub

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        WriteAuditLog_ClickCancel()
        ' CRE13-003 - Token Replacement [End][Tommy L]

        RaiseEvent ButtonClick(enumButtonClick.Cancel)
    End Sub

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub ibtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDeclaration.CheckedChanged
        Me.chkDeclaration.Checked = IIf(AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.chkDeclaration.UniqueID), True) = "on", True, False)

        If Me.chkDeclaration.Checked Then
            CheckBoxChecked = enumCheckBoxClick.Checked
        Else
            CheckBoxChecked = enumCheckBoxClick.Unchecked
        End If

        RaiseEvent CheckBoxClick(CheckBoxChecked)

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

#End Region

#Region "Supported Functions"

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Sub LoadNoticeMsg(ByVal udtNoticeMsg As NoticeMsg)
        _udtNoticeMsg = udtNoticeMsg

        HeaderText = udtNoticeMsg.HeaderText
        MessageText = udtNoticeMsg.MessageText
        'CheckBox for Declaration
        ShowDeclaration = udtNoticeMsg.ShowDeclaration
        DeclarationText = udtNoticeMsg.DeclarationText

        Me.ViewState(VS_BUTTON_MODE) = udtNoticeMsg.ButtonMode
        Me.ViewState(VS_ICON_MODE) = udtNoticeMsg.IconMode
        Me.ViewState(VS_NOTICE_MODE) = udtNoticeMsg.NoticeMode

        SetupButtonMode(udtNoticeMsg)
        SetupNoticeMode()
        SetupCheckBox()

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Sub ShowPopUp(ByRef actModalPopupExtender As ModalPopupExtender, Optional ByVal blnWriteLog As Boolean = True)
        If blnWriteLog Then
            WriteAuditLog_Show()
        End If

        actModalPopupExtender.Show()
    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    Private Sub WriteAuditLog_Show()
        If Not IsNothing(_udtNoticeMsg) AndAlso _udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(_udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(_udtNoticeMsg.AuditLogID_Show, _udtNoticeMsg.AuditLogDesc_Show)
        End If
    End Sub

    Private Sub WriteAuditLog_ClickOK()
        If Not IsNothing(_udtNoticeMsg) AndAlso _udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(_udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(_udtNoticeMsg.AuditLogID_ClickOK, _udtNoticeMsg.AuditLogDesc_ClickOK)
        End If
    End Sub

    Private Sub WriteAuditLog_ClickCancel()
        If Not IsNothing(_udtNoticeMsg) AndAlso _udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(_udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(_udtNoticeMsg.AuditLogID_ClickCancel, _udtNoticeMsg.AuditLogDesc_ClickCancel)
        End If
    End Sub

#End Region

#Region "Sub Class: NoticeMsg"
    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    <Serializable()> Public Class NoticeMsg

#Region "Private Members"
        Private _strHeaderText As String = String.Empty
        Private _strMessageText As String = String.Empty
        Private _strDeclarationText As String = String.Empty
        Private _blnShowDeclaration As Boolean = False
        Private _strPopupName As String = String.Empty
        Private _strCustomBtnImageResource As String = String.Empty
        Private _strCustomDisableBtnImageResource As String = String.Empty

        Private _enumNoticeMode As enumNoticeMode = enumNoticeMode.Confirmation
        Private _enumIconMode As enumIconMode = enumIconMode.Information
        Private _enumButtonMode As enumButtonMode = enumButtonMode.YesNo

        Private _blnIsAuditLogEnabled As Boolean = False
        Private _strAuditLogFunctionCode As String = String.Empty

        Private _strAuditLogID_Show As String = String.Empty
        Private _strAuditLogDesc_Show As String = String.Empty
        Private _strAuditLogID_ClickOK As String = String.Empty
        Private _strAuditLogDesc_ClickOK As String = String.Empty
        Private _strAuditLogID_ClickCancel As String = String.Empty
        Private _strAuditLogDesc_ClickCancel As String = String.Empty

#End Region

#Region "Properties"
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

        Public Property PopupName() As String
            Get
                Return _strPopupName
            End Get
            Set(ByVal value As String)
                _strPopupName = value
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

        Public ReadOnly Property AuditLogID_Show() As String
            Get
                Return _strAuditLogID_Show
            End Get
        End Property

        Public ReadOnly Property AuditLogDesc_Show() As String
            Get
                Return _strAuditLogDesc_Show
            End Get
        End Property

        Public ReadOnly Property AuditLogID_ClickOK() As String
            Get
                Return _strAuditLogID_ClickOK
            End Get
        End Property

        Public ReadOnly Property AuditLogDesc_ClickOK() As String
            Get
                Return _strAuditLogDesc_ClickOK
            End Get
        End Property

        Public ReadOnly Property AuditLogID_ClickCancel() As String
            Get
                Return _strAuditLogID_ClickCancel
            End Get
        End Property

        Public ReadOnly Property AuditLogDesc_ClickCancel() As String
            Get
                Return _strAuditLogDesc_ClickCancel
            End Get
        End Property

        ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Property DeclarationText() As String
            Get
                Return _strDeclarationText
            End Get
            Set(value As String)
                _strDeclarationText = value
            End Set
        End Property

        Public Property ShowDeclaration() As Boolean
            Get
                Return _blnShowDeclaration
            End Get
            Set(value As Boolean)
                _blnShowDeclaration = value
            End Set
        End Property

        Public Property CustomBtnImageResource() As String
            Get
                Return _strCustomBtnImageResource
            End Get
            Set(value As String)
                _strCustomBtnImageResource = value
            End Set
        End Property

        Public Property CustomDisableBtnImageResource() As String
            Get
                Return _strCustomDisableBtnImageResource
            End Get
            Set(value As String)
                _strCustomDisableBtnImageResource = value
            End Set
        End Property
        ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

#End Region

#Region "Constructors"
        Public Sub New(ByVal udtEnumNoticeMode As enumNoticeMode, ByVal udtEnumButtonMode As enumButtonMode, ByVal strPopupName As String, _
                       ByVal strMessageText As String)

            _enumNoticeMode = udtEnumNoticeMode
            _enumButtonMode = udtEnumButtonMode
            _strPopupName = strPopupName
            _strMessageText = strMessageText

        End Sub

        Public Sub New(ByVal udtEnumIconMode As enumIconMode, ByVal udtEnumButtonMode As enumButtonMode, ByVal strPopupName As String, _
                       ByVal strHeaderText As String, ByVal strMessageText As String)

            _enumIconMode = udtEnumIconMode
            _enumButtonMode = udtEnumButtonMode
            _strPopupName = strPopupName
            _strHeaderText = strHeaderText
            _strMessageText = strMessageText

            _enumNoticeMode = enumNoticeMode.Custom
        End Sub

#End Region

#Region "Supported Functions"
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

#End Region

    End Class
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

#End Region

#Region "Sub Class: NoticeMsgQueue"
    <Serializable()> Public Class NoticeMsgQueue
        Inherits Queue(Of NoticeMsg)

    End Class

#End Region

End Class

