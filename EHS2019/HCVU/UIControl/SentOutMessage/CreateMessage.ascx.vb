' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System.Drawing
Imports System.Text
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.Profession
Imports Common.Component.Scheme
Imports Common.Component.SentOutMessage
Imports Common.Component.SortedGridviewHeader
Imports Common.Format
Imports CustomControls.DynamicControls

Partial Public Class CreateMessage
    'Inherits System.Web.UI.UserControl
    Inherits BaseControlWithGridView

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const PageLoad_ID As String = LogID.LOG00025
        Public Const PageLoad As String = "Create Draft loaded"

        Public Const SelectTemplate_ID As String = LogID.LOG00026
        Public Const SelectTemplate As String = "Create Draft - Select Template"

        Public Const AddRecipientInputParam_ID As String = LogID.LOG00027
        Public Const AddRecipientInputParam As String = "Create Draft - Add Recipient and Input Parameter loaded"

        Public Const PreviewTemplate_ID As String = LogID.LOG00028
        Public Const PreviewTemplate As String = "Create Draft - Preview Template click"

        Public Const HideTemplate_ID As String = LogID.LOG00029
        Public Const HideTemplate As String = "Create Draft - Hide Template click"

        Public Const AddRecipient_ID As String = LogID.LOG00030
        Public Const AddRecipient As String = "Create Draft - Add (Recipient) click"

        Public Const AddRecipientPopUp_ID As String = LogID.LOG00031
        Public Const AddRecipientPopUp As String = "Create Draft - Show Add Recipient Popup"

        Public Const AddRecipientPopUp_Add_ID As String = LogID.LOG00032
        Public Const AddRecipientPopUp_Add As String = "Create Draft - Add (Add Recipient Popup) click"

        Public Const AddRecipientPopUp_Add_Fail_ID As String = LogID.LOG00033
        Public Const AddRecipientPopUp_Add_Fail As String = "Create Draft - Add (Add Recipient Popup) click fail"

        Public Const AddRecipientPopUp_Cancel_ID As String = LogID.LOG00034
        Public Const AddRecipientPopUp_Cancel As String = "Create Draft - Cancel (Add Recipient Popup) click"

        Public Const ResetRecipient_ID As String = LogID.LOG00035
        Public Const ResetRecipient As String = "Create Draft - Reset (Recipient) click"

        Public Const RemoveRecipient_ID As String = LogID.LOG00036
        Public Const RemoveRecipient As String = "Create Draft - Remove (Recipient) click"

        Public Const AddRecipientInputParam_Next_ID As String = LogID.LOG00037
        Public Const AddRecipientInputParam_Next As String = "Create Draft - Next (Add Recipient and Input Parameter) click"

        Public Const AddRecipientInputParam_Next_Fail_ID As String = LogID.LOG00038
        Public Const AddRecipientInputParam_Next_Fail As String = "Create Draft - Next (Add Recipient and Input Parameter) click fail"

        Public Const AddRecipientInputParam_Cancel_ID As String = LogID.LOG00039
        Public Const AddRecipientInputParam_Cancel As String = "Create Draft - Cancel click"

        Public Const Confirmation_ID As String = LogID.LOG00040
        Public Const Confirmation As String = "Create Draft - Confirmation loaded"

        Public Const Confirmation_Back_ID As String = LogID.LOG00041
        Public Const Confirmation_Back As String = "Create Draft - Back (Confirmation) click"

        Public Const Confirmation_Confirm_ID As String = LogID.LOG00042
        Public Const Confirmation_Confirm As String = "Create Draft - Confirm click"

        Public Const Success_ID As String = LogID.LOG00043
        Public Const Success As String = "Create Draft successful"

        Public Const ConfirmAddAllEnrolledSPPopUp_ID As String = LogID.LOG00068
        Public Const ConfirmAddAllEnrolledSPPopUp As String = "Create Draft - Show Confirm Add All Enrolled SP Popup"

        Public Const ConfirmAddAllEnrolledSPPopUp_Confirm_ID As String = LogID.LOG00069
        Public Const ConfirmAddAllEnrolledSPPopUp_Confirm As String = "Create Draft - Confirm (Confirm Add All Enrolled SP Popup) click"

        Public Const ConfirmAddAllEnrolledSPPopUp_Cancel_ID As String = LogID.LOG00070
        Public Const ConfirmAddAllEnrolledSPPopUp_Cancel As String = "Create Draft - Cancel (Confirm Add All Enrolled SP Popup) click"

        Public Const TemplateID As String = "TemplateID"
        Public Const PopUpType As String = "PopupType"
        Public Const AllEnrolledSP As String = "AllEnrolledSP"
        Public Const Profession As String = "Profession"
        Public Const Scheme As String = "Scheme"
        Public Const Recipient As String = "Recipient"
        Public Const InputParam As String = "InputParam"
        Public Const SentOutMessageID As String = "SentOutMessageID"
        Public Const CreateDtm As String = "CreateDtm"
        Public Const NoInputValue As String = "null"
    End Class
#End Region

#Region "Enum"
    Public Enum MultiViewEnum
        SelectTemplateView = 0
        InputParameterView = 1
        SelectRecipientView = 2
        ConfirmCreateMessageView = 3
        CompletedCreateMessageView = 4
    End Enum
#End Region

#Region "Public Constant"
    ' Tag for Template Message Parameter Display
    Public Const PARAM_OPEN_TAG_FOR_DISPLAY As String = "<<"
    Public Const PARAM_CLOSE_TAG_FOR_DISPLAY As String = ">>"
    Public Const PARAM_OPEN_TAG_FOR_FORMAT As String = "<span style=""background-color:Yellow"">"
    Public Const PARAM_CLOSE_TAG_FOR_FORMAT As String = "</span>"
#End Region

#Region "Private Constant"
    ' Used Function Code
    Private Const COMMON_FUNCT_CODE As String = FunctCode.FUNT990000
    Private Const FUNCT_CODE As String = FunctCode.FUNT010003

    ' Name of Session Variable
    Private Const SESS_MESSAGE_TEMPLATE_LIST As String = "MessageTemplateList"
    Private Const SESS_SELECTED_TEMPLATE As String = "SelectedTemplate"
    Private Const SESS_CREATED_SENT_OUT_MSG As String = "CreatedSentOutMessage"
    Private Const SESS_SELECTED_RECIPIENT As String = "SelectedRecipient"

    ' Name of DB Field for Data Binding (Table - [Profession] and [SchemeBackOffice])
    Private Const STR_HEALTH_PROFESSION_DATA_TEXT_FIELD As String = "ServiceCategoryDesc"
    Private Const STR_HEALTH_PROFESSION_DATA_VALUE_FIELD As String = "ServiceCategoryCode"
    Private Const STR_SCHEME_NAME_DATA_TEXT_FIELD As String = "DisplayCode"
    Private Const STR_SCHEME_NAME_DATA_VALUE_FIELD As String = "SchemeCode"

    ' Value of DB Field - [ObjectName] in DB Table - [SystemResource] 
    Private Const STR_RESOURCE_KEY_VALIDATION_FAIL As String = "ValidationFail"
    Private Const STR_SYSTEM_MESSAGE_PARAM_S As String = "%s"
#End Region

#Region "Event"
    Public Event MessageTemplateSelected(ByVal sender As Object, ByVal strTemplateID As String)
    Public Event MessageTemplateClosed(ByVal sender As Object)
    Public Event MessageTemplateCreated(ByVal sender As Object)
#End Region

#Region "Private Data Member"
    Private udtAuditLogEntry As AuditLogEntry
    Private udtFormatter As Formatter = New Formatter

    ' Used for [GridView - gvSelectRecipient] in [View 1 - Preview Template and Input Parameter]
    Private intSelectedRecipientIndex As Integer = 0
#End Region

#Region "Page Event"
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        udtAuditLogEntry = New AuditLogEntry(FUNCT_CODE)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadJavaScriptBlock()
        SetJavaScriptCaller_v2()

        If IsPostBack Then

            ' To retain the ViewState of dynamic server created WebControl, keep creating it when the template was selected
            If mvCreateMessage.ActiveViewIndex > MultiViewEnum.SelectTemplateView Then
                LoadParamsInput()
            End If

        End If
    End Sub
#End Region

#Region "Public Method"
    ' Reset all status of this User Control
    Public Sub ResetAll()
        ' Clear All Session Variable
        Session(SESS_MESSAGE_TEMPLATE_LIST) = Nothing
        Session(SESS_SELECTED_TEMPLATE) = Nothing
        Session(SESS_SELECTED_RECIPIENT) = Nothing
        Session(SESS_CREATED_SENT_OUT_MSG) = Nothing

        ' Reset [View 1 - Preview Template and Input Parameter]
        lblTemplateID_v1.Text = ""
        ibtnPreviewTemplate_v1.Visible = True
        ibtnHideTemplate_v1.Visible = False
        lblCategory_v1.Text = ""

        ibtnAddRecipient_v1.Visible = True
        ibtnResetRecipient_v1.Visible = False
        imgRecipientAlert_v1.Visible = False
        tblRecipient_v1.Visible = False
        lblRecipient_v1.Text = ""
        lblRecipient_v1.Visible = False
        gvSelectRecipient.DataSource = Nothing
        gvSelectRecipient.DataBind()

        lblSubject0_v1.Text = ""
        panPreviewTemplate.Visible = False
        lblSubject_v1.Text = ""
        lblContent_v1.Text = ""
        panInputParameter.Visible = True
        phParam_v1.Controls.Clear()

        ' Reset [Pop-up Window for "Add Recipient"]
        panSelectRecipient.Visible = True
        rdoAllEnrolledSP_v2.Checked = False
        rdoSelectRecipientBy_P_S_v2.Checked = False
        'chkHealthProfession_v2.Enabled = False
        'chkHealthProfession_v2.Checked = False
        ddlHealthProfession_v2.Items.Clear()
        ddlHealthProfession_v2.Enabled = False
        chkScheme_v2.Enabled = False
        chkScheme_v2.Checked = False
        ddlScheme_v2.Items.Clear()
        ddlScheme_v2.Enabled = False
        imgSelectRecipientAlert1_v2.Visible = False
        imgSelectRecipientAlert2_v2.Visible = False
        imgHealthProfessionAlert_v2.Visible = False
        imgSchemeAlert_v2.Visible = False

        ' Reset [View 3 - Confirm to Create Message]
        ucReadOnlyMessageDetails.ResetAll()

        ' Reset [View 4 - Completed to Create Message]
        lblMessageID_v4.Text = ""
        lblCreatedDateTime_v4.Text = ""

        LoadMessageTemplate()
        mvCreateMessage.ActiveViewIndex = MultiViewEnum.SelectTemplateView
    End Sub

    ' Get the active view index of the MultiView - [mvCreateMessage]
    Public Function GetMultiViewIndex() As Integer
        Return mvCreateMessage.ActiveViewIndex
    End Function

    ' Get the formatted String of Selected Recipient for Audit Log Writing
    Public Function GetSelectedRecipientForAuditLog() As String
        Dim udtSentOutMessageModel As SentOutMessageModel
        Dim udtSentOutMsgRecipientModelCollection As SentOutMsgRecipientModelCollection
        Dim udtSentOutMsgRecipientModel As SentOutMsgRecipientModel
        Dim strSelectedRecipient As StringBuilder = New StringBuilder()

        If Not Session(SESS_CREATED_SENT_OUT_MSG) Is Nothing Then

            udtSentOutMessageModel = Session(SESS_CREATED_SENT_OUT_MSG)

            udtSentOutMsgRecipientModelCollection = udtSentOutMessageModel.SentOutMsgRecipients
            For Each udtSentOutMsgRecipientModel In udtSentOutMsgRecipientModelCollection

                If udtSentOutMsgRecipientModel.Profession = SentOutMsgRecipientModel.PROFESSION_NA AndAlso udtSentOutMsgRecipientModel.Scheme = SentOutMsgRecipientModel.SCHEME_NA Then

                    strSelectedRecipient.Append(AuditLogDesc.AllEnrolledSP)
                    Return strSelectedRecipient.ToString()

                Else

                    strSelectedRecipient.Append("[")
                    strSelectedRecipient.Append(udtSentOutMsgRecipientModel.Profession)
                    strSelectedRecipient.Append(",")
                    strSelectedRecipient.Append(udtSentOutMsgRecipientModel.Scheme)
                    strSelectedRecipient.Append("],")

                End If

            Next

            strSelectedRecipient.Remove(strSelectedRecipient.Length - 1, 1)
            Return strSelectedRecipient.ToString()

        Else

            Return Nothing

        End If
    End Function

    ' Get the formatted String of Selected Input Parameter for Audit Log Writing
    Public Function GetSelectedInputParamForAuditLog() As String
        Dim udtMessageTemplateModel As MessageTemplateModel
        Dim udtMsgParam As MessageParameterModel
        Dim udtDynamicControl As DynamicControlBase
        Dim strSelectedInputParam As StringBuilder = New StringBuilder()

        If Not Session(SESS_SELECTED_TEMPLATE) Is Nothing Then

            udtMessageTemplateModel = Session(SESS_SELECTED_TEMPLATE)

            If Not udtMessageTemplateModel.MsgParams Is Nothing Then

                For Each udtMsgParam In udtMessageTemplateModel.MsgParams
                    udtDynamicControl = CType(phParam_v1.FindControl(udtMsgParam.MsgParamID), DynamicControlBase)

                    strSelectedInputParam.Append("[")
                    strSelectedInputParam.Append(udtDynamicControl.ID)
                    strSelectedInputParam.Append(":")
                    strSelectedInputParam.Append(udtDynamicControl.GetText())
                    strSelectedInputParam.Append("],")

                Next

                strSelectedInputParam.Remove(strSelectedInputParam.Length - 1, 1)

            Else

                Return Nothing

            End If

        Else

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [GetSelectedInputParamForAuditLog], Message = The selected message template is lost in this session")

        End If

        Return strSelectedInputParam.ToString()
    End Function
#End Region

#Region "Private Method"
    ' Find the Parent Page of this User Control
    Private Function FindParentPage(ByRef parentControl As Control) As Page
        If TryCast(parentControl, Page) Is Nothing Then

            Return FindParentPage(parentControl.Parent)

        Else

            Return CType(parentControl, Page)

        End If
    End Function

    ' Get ID of the Selected Template for Audit Log Writing
    Private Function GetSelectedTemplateIDforAuditLog() As String
        Dim udtMessageTemplateModel As MessageTemplateModel

        If Not Session(SESS_SELECTED_TEMPLATE) Is Nothing Then

            udtMessageTemplateModel = Session(SESS_SELECTED_TEMPLATE)
            Return udtMessageTemplateModel.MsgTemplateID

        Else

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [GetSelectedTemplateIDforAuditLog], Message = The selected message template is lost in this session")

        End If
    End Function

    ' Load the JavaScript Block to the Parent Page for this User Control
    Private Sub LoadJavaScriptBlock()
        Dim strJavaScriptBlock As StringBuilder = New StringBuilder()

        ' The JavaScript for [Pop-up Window for "Add Recipient"]
        strJavaScriptBlock.Append("function updateRecipient_P_S_Selection(senderID, targetID) {")
        strJavaScriptBlock.Append("var ddlRecipient = document.getElementById(senderID);")
        strJavaScriptBlock.Append("var chkRecipient = document.getElementById(targetID);")
        strJavaScriptBlock.Append("if (ddlRecipient.selectedIndex <= 0) {")
        strJavaScriptBlock.Append("chkRecipient.checked = false;")
        strJavaScriptBlock.Append("} else {")
        strJavaScriptBlock.Append("chkRecipient.checked = true;")
        strJavaScriptBlock.Append("}}")

        strJavaScriptBlock.Append("function resetRecipient_P_S_Selection(senderID, targetID) {")
        strJavaScriptBlock.Append("var chkRecipient = document.getElementById(senderID);")
        strJavaScriptBlock.Append("var ddlRecipient = document.getElementById(targetID);")
        strJavaScriptBlock.Append("if (chkRecipient.checked == false) {")
        strJavaScriptBlock.Append("ddlRecipient.selectedIndex = 0;")
        strJavaScriptBlock.Append("}}")

        ' The JavaScript for [View 2 - Select Recipient]
        'strJavaScriptBlock.Append("function updateRecipient_P_S_Selection(senderID, targetID) {")
        'strJavaScriptBlock.Append("var chkRecipient = document.getElementById(senderID);")
        'strJavaScriptBlock.Append("var ddlRecipient = document.getElementById(targetID);")
        'strJavaScriptBlock.Append("var blnChecked = chkRecipient.checked;")
        'strJavaScriptBlock.Append("if (blnChecked == true) {")
        'strJavaScriptBlock.Append("ddlRecipient.disabled = false;")
        'strJavaScriptBlock.Append("}")
        'strJavaScriptBlock.Append("else {")
        'strJavaScriptBlock.Append("ddlRecipient.selectedIndex = 0;")
        'strJavaScriptBlock.Append("ddlRecipient.disabled = true;")
        'strJavaScriptBlock.Append("}}")

        'strJavaScriptBlock.Append("function checkAllScheme(sender, targetID) {")
        'strJavaScriptBlock.Append("var blnChecked = sender.checked;")
        'strJavaScriptBlock.Append("var panAllScheme = document.getElementById(targetID);")
        'strJavaScriptBlock.Append("var chkInputs = panAllScheme.getElementsByTagName('input');")
        'strJavaScriptBlock.Append("for (var i = 0; i < chkInputs.length; i++) {")
        'strJavaScriptBlock.Append("if (chkInputs[i].disabled == false) {")
        'strJavaScriptBlock.Append("chkInputs[i].checked = blnChecked;")
        'strJavaScriptBlock.Append("}}}")

        'strJavaScriptBlock.Append("function updateCheckAllScheme(senderPanelID, targetID) {")
        'strJavaScriptBlock.Append("var panAllScheme = document.getElementById(senderPanelID);")
        'strJavaScriptBlock.Append("var chkInputs = panAllScheme.getElementsByTagName('input');")
        'strJavaScriptBlock.Append("var chkSchemeAll = document.getElementById(targetID);")
        'strJavaScriptBlock.Append("chkSchemeAll.checked = true;")
        'strJavaScriptBlock.Append("for (var i = 0; i < chkInputs.length; i++) {")
        'strJavaScriptBlock.Append("if (chkInputs[i].disabled == false && chkInputs[i].checked == false) {")
        'strJavaScriptBlock.Append("chkSchemeAll.checked = false;")
        'strJavaScriptBlock.Append("break;")
        'strJavaScriptBlock.Append("}}}")

        FindParentPage(Me.Parent).ClientScript.RegisterClientScriptBlock(Page.GetType(), "JavaScript_v2", strJavaScriptBlock.ToString(), True)
    End Sub

    ' Set Caller of the JavaScript Function
    Private Sub SetJavaScriptCaller_v2()
        'ddlHealthProfession_v2.Attributes.Add("onchange", "javascript:updateRecipient_P_S_Selection('" & ddlHealthProfession_v2.ClientID & "', '" & chkHealthProfession_v2.ClientID & "');")
        ddlScheme_v2.Attributes.Add("onchange", "javascript:updateRecipient_P_S_Selection('" & ddlScheme_v2.ClientID & "', '" & chkScheme_v2.ClientID & "');")

        'chkHealthProfession_v2.Attributes.Add("onclick", "javascript:resetRecipient_P_S_Selection('" & chkHealthProfession_v2.ClientID & "', '" & ddlHealthProfession_v2.ClientID & "');")
        chkScheme_v2.Attributes.Add("onclick", "javascript:resetRecipient_P_S_Selection('" & chkScheme_v2.ClientID & "', '" & ddlScheme_v2.ClientID & "');")

        'Dim i As Integer

        'chkSchemeNameAll.Attributes.Add("onclick", "javascript:checkAllScheme(this, '" & panSchemeName_v2.ClientID & "');")

        'For i = 0 To chklSchemeName_v2.Items.Count - 1
        'chklSchemeName_v2.Items(i).Attributes.Add("onclick", "javascript:updateCheckAllScheme('" & panSchemeName_v2.ClientID & "', '" & chkSchemeNameAll.ClientID & "');")
        'Next
    End Sub

    ' Load All Active Message Template in [View 0 - Select Template]
    Private Sub LoadMessageTemplate()
        Dim udtSentOutMessageBLL As New SentOutMessageBLL()
        Dim udtMsgParams As MessageParameterModelCollection
        Dim dt As DataTable

        dt = udtSentOutMessageBLL.GetAllActiveMsgTemplate()

        ' Replace the Parameter ID with the Display Name for the Subject of Message Template
        For Each dr As DataRow In dt.Rows
            udtMsgParams = udtSentOutMessageBLL.GetMsgParameters(CStr(dr.Item(SentOutMessageBLL.Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplate_ID)).Trim())

            If Not udtMsgParams Is Nothing Then
                dr.Item(SentOutMessageBLL.Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplateSubject) = ReplaceParamsWithDisplayName(CStr(dr.Item(SentOutMessageBLL.Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplateSubject)).Trim(), udtMsgParams)
            End If
        Next

        Session(SESS_MESSAGE_TEMPLATE_LIST) = dt
        GridViewDataBind(gvSelectTemplate, dt, SentOutMessageBLL.Table_InboxMsgTemplate_IBMT.IBMT_MsgTemplate_ID, "ASC", False)

        udtAuditLogEntry.WriteLog(AuditLogDesc.PageLoad_ID, AuditLogDesc.PageLoad)
    End Sub

    ' Load the Message Template with Input Parameter in [View 1 - Preview Template and Input Parameter]
    Private Sub LoadTemplateInput(ByVal strTemplateID As String)
        Dim udtSentOutMessageBLL As New SentOutMessageBLL()
        Dim udtMessageTemplateModel As MessageTemplateModel

        udtMessageTemplateModel = udtSentOutMessageBLL.GetMsgTemplate(strTemplateID)

        If Not udtMessageTemplateModel Is Nothing Then
            Session(SESS_SELECTED_TEMPLATE) = udtMessageTemplateModel

            lblTemplateID_v1.Text = udtMessageTemplateModel.MsgTemplateID
            lblCategory_v1.Text = udtMessageTemplateModel.GetMsgTemplateCategoryDisplayText()

            ' If there is no parameter
            If udtMessageTemplateModel.MsgParams Is Nothing Then

                lblSubject0_v1.Text = udtMessageTemplateModel.MsgTemplateSubject
                lblSubject_v1.Text = lblSubject0_v1.Text
                lblContent_v1.Text = udtMessageTemplateModel.MsgTemplateContent

                ' If there is any parameter
            Else

                lblSubject0_v1.Text = ReplaceParamsWithDisplayName(udtMessageTemplateModel.MsgTemplateSubject, udtMessageTemplateModel.MsgParams)
                lblSubject_v1.Text = lblSubject0_v1.Text
                lblContent_v1.Text = ReplaceParamsWithDisplayName(udtMessageTemplateModel.MsgTemplateContent, udtMessageTemplateModel.MsgParams)

            End If

            LoadParamsInput()

        Else

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [LoadTemplateInput], Message = No message template has been found by the ID that was passed into this method")

        End If
    End Sub

    ' Replace the Parameter ID with the Display Name for the Message Template
    Private Function ReplaceParamsWithDisplayName(ByVal strMsg As String, ByVal udtMsgParams As MessageParameterModelCollection) As String
        Dim strMsgForDisplay As StringBuilder
        Dim udtMsgParam As MessageParameterModel

        If udtMsgParams Is Nothing Then
            Return strMsg
        End If

        strMsgForDisplay = New StringBuilder(strMsg)

        For Each udtMsgParam In udtMsgParams
            strMsgForDisplay = strMsgForDisplay.Replace(MessageTemplateModel.PARAM_OPEN_TAG & udtMsgParam.MsgParamID & MessageTemplateModel.PARAM_CLOSE_TAG, _
                                                        PARAM_OPEN_TAG_FOR_FORMAT & Server.HtmlEncode(PARAM_OPEN_TAG_FOR_DISPLAY & udtMsgParam.MsgParamName & PARAM_CLOSE_TAG_FOR_DISPLAY) & PARAM_CLOSE_TAG_FOR_FORMAT)
        Next

        Return strMsgForDisplay.ToString()
    End Function

    ' Replace the Parameter ID with the Actual Value for the Message Template
    Private Function ReplaceParamsWithValue(ByVal strMsg As String, ByVal udtMsgParams As MessageParameterModelCollection) As String
        Dim strMsgWithValue As StringBuilder
        Dim udtMsgParam As MessageParameterModel

        If udtMsgParams Is Nothing Then
            Return strMsg
        End If

        strMsgWithValue = New StringBuilder(strMsg)

        Try

            For Each udtMsgParam In udtMsgParams
                strMsgWithValue = strMsgWithValue.Replace(MessageTemplateModel.PARAM_OPEN_TAG & udtMsgParam.MsgParamID & MessageTemplateModel.PARAM_CLOSE_TAG, _
                                                          CType(phParam_v1.FindControl(udtMsgParam.MsgParamID), DynamicControlBase).GetText())
            Next

        Catch ex As Exception

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [ReplaceParamsWithValue], Message = Dynamic Control for parameter input must inherit the Abstract Class - [CustomControls.DynamicControls.DynamicControlBase]")

        End Try

        Return strMsgWithValue.ToString()
    End Function

    ' Load the dynamic server created WebControl for Input Parameter
    Private Sub LoadParamsInput()
        Dim udtMessageTemplateModel As MessageTemplateModel

        If Not Session(SESS_SELECTED_TEMPLATE) Is Nothing Then

            udtMessageTemplateModel = Session(SESS_SELECTED_TEMPLATE)

            If udtMessageTemplateModel.MsgParams Is Nothing Then

                panInputParameter.Visible = False

            Else

                Dim udtMessageParameterModel As MessageParameterModel
                Dim lblParamTextTemp As Label
                Dim imgParamAlertTemp As WebControls.Image

                panInputParameter.Visible = True

                phParam_v1.Controls.Add(New LiteralControl("<table border=""0"" cellpadding=""2"" cellspacing=""0"">"))

                For Each udtMessageParameterModel In udtMessageTemplateModel.MsgParams

                    phParam_v1.Controls.Add(New LiteralControl("<tr sytle=""vertical-align:bottom"">"))
                    phParam_v1.Controls.Add(New LiteralControl("<td>"))

                    lblParamTextTemp = New Label()
                    lblParamTextTemp.ID = "lblParamText" & udtMessageParameterModel.MsgParamID
                    lblParamTextTemp.CssClass = "tableText"
                    lblParamTextTemp.Text = Server.HtmlEncode(PARAM_OPEN_TAG_FOR_DISPLAY & udtMessageParameterModel.MsgParamName & PARAM_CLOSE_TAG_FOR_DISPLAY)
                    phParam_v1.Controls.Add(lblParamTextTemp)

                    phParam_v1.Controls.Add(New LiteralControl("</td>"))

                    phParam_v1.Controls.Add(New LiteralControl("<td style=""padding-left:20px"">"))

                    ' Create [DynamicControl] by [DynamicControlCreator] and Add it into this WebControl
                    phParam_v1.Controls.Add(DynamicControlCreator.CreateControl(udtMessageParameterModel.MsgParamID, udtMessageParameterModel.MsgParamType, udtMessageParameterModel.MsgParamArg))

                    imgParamAlertTemp = New WebControls.Image()
                    imgParamAlertTemp.ID = "imgParamAlert" & udtMessageParameterModel.MsgParamID
                    imgParamAlertTemp.AlternateText = GetGlobalResourceObject("AlternateText", "ErrorBtn")
                    imgParamAlertTemp.ImageUrl = GetGlobalResourceObject("ImageUrl", "ErrorBtn")
                    imgParamAlertTemp.ImageAlign = ImageAlign.AbsBottom
                    imgParamAlertTemp.Visible = False
                    phParam_v1.Controls.Add(New LiteralControl("&nbsp;"))
                    phParam_v1.Controls.Add(imgParamAlertTemp)

                    phParam_v1.Controls.Add(New LiteralControl("</td>"))
                    phParam_v1.Controls.Add(New LiteralControl("</tr>"))

                Next

                phParam_v1.Controls.Add(New LiteralControl("</table>"))

            End If

        Else

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [LoadParamsInput], Message = The selected message template is lost in this session")

        End If
    End Sub

    ' Load all component in [Pop-up Window for "Add Recipient"]
    Private Sub LoadSelectRecipient()
        imgSelectRecipientAlert1_v2.Visible = False
        imgSelectRecipientAlert2_v2.Visible = False
        imgRecipientAlert_v1.Visible = False

        LoadRecipient_HP()
        LoadRecipient_SN()
    End Sub

    ' Load all items for Health Profession Selection in [Pop-up Window for "Add Recipient"]
    Private Sub LoadRecipient_HP()
        Dim udtProfessionModelCollection As ProfessionModelCollection

        imgHealthProfessionAlert_v2.Visible = False

        udtProfessionModelCollection = ProfessionBLL.GetProfessionList()
        ddlHealthProfession_v2.DataSource = udtProfessionModelCollection
        ddlHealthProfession_v2.DataTextField = STR_HEALTH_PROFESSION_DATA_TEXT_FIELD
        ddlHealthProfession_v2.DataValueField = STR_HEALTH_PROFESSION_DATA_VALUE_FIELD
        ddlHealthProfession_v2.DataBind()

        ddlHealthProfession_v2.Items.Insert(0, New ListItem(GetGlobalResourceObject("Text", "Any"), "0"))
        ddlHealthProfession_v2.Items.Insert(0, New ListItem(GetGlobalResourceObject("Text", "PleaseSelect"), "-1"))
    End Sub

    ' Load all items for Scheme Selection in [Pop-up Window for "Add Recipient"]
    Private Sub LoadRecipient_SN()
        Dim udtSchemeBackOfficeBLL As SchemeBackOfficeBLL = New SchemeBackOfficeBLL()
        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection

        imgSchemeAlert_v2.Visible = False

        udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
        ddlScheme_v2.DataSource = udtSchemeBackOfficeList
        ddlScheme_v2.DataTextField = STR_SCHEME_NAME_DATA_TEXT_FIELD
        ddlScheme_v2.DataValueField = STR_SCHEME_NAME_DATA_VALUE_FIELD
        ddlScheme_v2.DataBind()

        ddlScheme_v2.Items.Insert(0, New ListItem(GetGlobalResourceObject("Text", "PleaseSelect"), "-1"))
    End Sub

    ' Configure WebControl in [Pop-up Window for "Add Recipient"] appropriately
    Private Sub ConfigPopUpAddRecipient()
        rdoAllEnrolledSP_v2.Checked = False
        rdoSelectRecipientBy_P_S_v2.Checked = False

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())

        If Session(SESS_SELECTED_RECIPIENT) Is Nothing Then
            panSelectRecipient.Visible = True

            udtAuditLogEntry.AddDescripton(AuditLogDesc.PopUpType, "Full")
        Else
            panSelectRecipient.Visible = False
            rdoSelectRecipientBy_P_S_v2.Checked = True

            udtAuditLogEntry.AddDescripton(AuditLogDesc.PopUpType, "SelectedSP")
        End If

        udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_ID, AuditLogDesc.AddRecipientPopUp)

        UpdateRecipientChoiceSelection()
    End Sub

    ' Update the status of WebControl for the Recipient Selection in [Pop-up Window for "Add Recipient"]
    Private Sub UpdateRecipientChoiceSelection()
        Dim udcMessageBox As CustomControls.MessageBox

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        udcMessageBoxPopUp.Clear()

        imgSelectRecipientAlert1_v2.Visible = False
        imgSelectRecipientAlert2_v2.Visible = False
        imgHealthProfessionAlert_v2.Visible = False
        imgSchemeAlert_v2.Visible = False

        'chkHealthProfession_v2.Checked = False
        chkScheme_v2.Checked = False
        ddlHealthProfession_v2.SelectedIndex = 0
        ddlScheme_v2.SelectedIndex = 0

        If rdoSelectRecipientBy_P_S_v2.Checked = True AndAlso rdoAllEnrolledSP_v2.Checked = False Then
            'chkHealthProfession_v2.Enabled = True
            ddlHealthProfession_v2.Enabled = True
            chkScheme_v2.Enabled = True
            ddlScheme_v2.Enabled = True
        Else
            'chkHealthProfession_v2.Enabled = False
            ddlHealthProfession_v2.Enabled = False
            chkScheme_v2.Enabled = False
            ddlScheme_v2.Enabled = False
        End If

        ModalPopupExtenderAddRecipient.Show()
    End Sub

    ' Update the status of the WebControl in [View 2 - Select Recipient] because of the conflict produced by JavaScript
    Private Sub UpdateRecipient_P_S_Selection()
        ' Related JavaScript had been removed
    End Sub

    ' Create the dummy record for the status of No Selected Recipient in [View 2 - Select Recipient]
    Private Sub CreateEmptyData_gvSelectRecipient()
        'Dim dtSelectedRecipient As DataTable
        'Dim drSelectedRecipient As DataRow

        'Session(SESS_SELECTED_RECIPIENT) = Nothing

        'dtSelectedRecipient = New DataTable()
        'CreateDataTable_gvSelectRecipient(dtSelectedRecipient)

        'drSelectedRecipient = dtSelectedRecipient.NewRow()
        'drSelectedRecipient(0) = "-1"
        'drSelectedRecipient(1) = GetGlobalResourceObject("Text", "NoSelectedRecipient")
        'drSelectedRecipient(2) = ""
        'drSelectedRecipient(3) = ""
        'dtSelectedRecipient.Rows.Add(drSelectedRecipient)

        'gvSelectRecipient.DataSource = dtSelectedRecipient.DefaultView
        'gvSelectRecipient.DataBind()
        'gvSelectRecipient.Columns(0).ItemStyle.HorizontalAlign = HorizontalAlign.Left
    End Sub

    ' Create the column for the Data Binding of GridView - [gvSelectRecipient] in [View 1 - Preview Template and Input Parameter]
    Private Sub CreateDataTable_gvSelectRecipient(ByRef dtSelectedRecipient As DataTable)
        Dim dcSelectedRecipient As DataColumn

        dcSelectedRecipient = New DataColumn(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession)
        dcSelectedRecipient.DataType = Type.GetType("System.String")
        dtSelectedRecipient.Columns.Add(dcSelectedRecipient)

        dcSelectedRecipient = New DataColumn("SOMR_Profession_DisplayText")
        dcSelectedRecipient.DataType = Type.GetType("System.String")
        dtSelectedRecipient.Columns.Add(dcSelectedRecipient)

        dcSelectedRecipient = New DataColumn(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme)
        dcSelectedRecipient.DataType = Type.GetType("System.String")
        dtSelectedRecipient.Columns.Add(dcSelectedRecipient)

        dcSelectedRecipient = New DataColumn("SOMR_Scheme_DisplayText")
        dcSelectedRecipient.DataType = Type.GetType("System.String")
        dtSelectedRecipient.Columns.Add(dcSelectedRecipient)
    End Sub

    ' Add the selected recipient from [Pop-up Window for "Add Recipient"] into the GridView - [gvSelectRecipient] in [View 1 - Preview Template and Input Parameter]
    Private Sub AddRecipient()
        Dim dtSelectedRecipient As DataTable
        Dim drSelectedRecipient As DataRow

        ibtnResetRecipient_v1.Visible = True
        tblRecipient_v1.Visible = True

        ' Check if [All Enrolled Service Provider] is selected or should be selected
        If (rdoAllEnrolledSP_v2.Checked = True AndAlso rdoSelectRecipientBy_P_S_v2.Checked = False) OrElse _
           (ddlHealthProfession_v2.SelectedValue = "0" AndAlso ddlScheme_v2.SelectedValue = "-1") Then

            ibtnAddRecipient_v1.Visible = False
            lblRecipient_v1.Text = GetGlobalResourceObject("Text", "AllEnrolledSP")
            lblRecipient_v1.Visible = True

            ' Check if [Select Recipient by Health Profession and/or Scheme] is selected
        ElseIf (rdoAllEnrolledSP_v2.Checked = False AndAlso rdoSelectRecipientBy_P_S_v2.Checked = True) OrElse (Not Session(SESS_SELECTED_RECIPIENT) Is Nothing) Then

            lblRecipient_v1.Text = ""
            lblRecipient_v1.Visible = False

            If Session(SESS_SELECTED_RECIPIENT) Is Nothing Then

                dtSelectedRecipient = New DataTable()
                CreateDataTable_gvSelectRecipient(dtSelectedRecipient)

            Else

                dtSelectedRecipient = Session(SESS_SELECTED_RECIPIENT)

            End If

            drSelectedRecipient = dtSelectedRecipient.NewRow()
            If ddlHealthProfession_v2.SelectedValue <> "0" AndAlso ddlScheme_v2.SelectedValue <> "-1" Then

                drSelectedRecipient(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession) = ddlHealthProfession_v2.SelectedValue.Trim()
                drSelectedRecipient("SOMR_Profession_DisplayText") = ddlHealthProfession_v2.SelectedItem.Text.Trim()
                drSelectedRecipient(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme) = ddlScheme_v2.SelectedValue.Trim()
                drSelectedRecipient("SOMR_Scheme_DisplayText") = ddlScheme_v2.SelectedItem.Text.Trim()

            ElseIf ddlHealthProfession_v2.SelectedValue <> "0" AndAlso ddlScheme_v2.SelectedValue = "-1" Then

                drSelectedRecipient(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession) = ddlHealthProfession_v2.SelectedValue.Trim()
                drSelectedRecipient("SOMR_Profession_DisplayText") = ddlHealthProfession_v2.SelectedItem.Text.Trim()
                drSelectedRecipient(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme) = SentOutMsgRecipientModel.SCHEME_NA
                drSelectedRecipient("SOMR_Scheme_DisplayText") = GetGlobalResourceObject("Text", SentOutMsgRecipientModel.SCHEME_NA)

            ElseIf ddlHealthProfession_v2.SelectedValue = "0" AndAlso ddlScheme_v2.SelectedValue <> "-1" Then

                drSelectedRecipient(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession) = SentOutMsgRecipientModel.PROFESSION_NA
                drSelectedRecipient("SOMR_Profession_DisplayText") = GetGlobalResourceObject("Text", "Any")
                drSelectedRecipient(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme) = ddlScheme_v2.SelectedValue.Trim()
                drSelectedRecipient("SOMR_Scheme_DisplayText") = ddlScheme_v2.SelectedItem.Text.Trim()

            End If
            dtSelectedRecipient.Rows.Add(drSelectedRecipient)

            Session(SESS_SELECTED_RECIPIENT) = dtSelectedRecipient

            gvSelectRecipient.DataSource = dtSelectedRecipient.DefaultView
            gvSelectRecipient.DataBind()

        End If
    End Sub

    ' Reset the selected recipient in [View 1 - Preview Template and Input Parameter]
    Private Sub ResetRecipient()
        Session(SESS_SELECTED_RECIPIENT) = Nothing

        ibtnAddRecipient_v1.Visible = True
        ibtnResetRecipient_v1.Visible = False
        tblRecipient_v1.Visible = False
        lblRecipient_v1.Text = ""
        lblRecipient_v1.Visible = False
        gvSelectRecipient.DataSource = Nothing
        gvSelectRecipient.DataBind()
    End Sub

    ' Load all component in [View 3 - Confirm to Create Message]
    Private Sub LoadConfirmCreateMessage()
        Dim udcInfoMessageBox As CustomControls.InfoMessageBox
        Dim strSelectedInputParam As String

        Session(SESS_CREATED_SENT_OUT_MSG) = CreateSentOutMessage()
        ucReadOnlyMessageDetails.LoadMessage(Session(SESS_CREATED_SENT_OUT_MSG), Session(SESS_SELECTED_TEMPLATE))

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
        udtAuditLogEntry.AddDescripton(AuditLogDesc.Recipient, GetSelectedRecipientForAuditLog())
        strSelectedInputParam = GetSelectedInputParamForAuditLog()
        If Not strSelectedInputParam Is Nothing Then
            udtAuditLogEntry.AddDescripton(AuditLogDesc.InputParam, strSelectedInputParam)
        End If
        udtAuditLogEntry.WriteLog(AuditLogDesc.Confirmation_ID, AuditLogDesc.Confirmation)

        udcInfoMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetInfoMessageBox()
        udcInfoMessageBox.Clear()
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        udcInfoMessageBox.AddMessage(COMMON_FUNCT_CODE, SeverityCode.SEVI, MsgCode.MSG00021)
        udcInfoMessageBox.BuildMessageBox()
    End Sub

    ' Create the [SentOutMessageModel] in [View 3 - Confirm to Create Message]
    Private Function CreateSentOutMessage() As SentOutMessageModel
        Dim udtSentOutMessageModel As SentOutMessageModel
        Dim udtMessageTemplateModel As MessageTemplateModel
        Dim strSubject As String
        Dim strContent As String
        Dim udtHCVUUserBLL As HCVUUserBLL = New HCVUUserBLL()
        Dim strTempID As String = "TEMP_ID"

        If Not Session(SESS_SELECTED_TEMPLATE) Is Nothing Then

            udtMessageTemplateModel = Session(SESS_SELECTED_TEMPLATE)

            If udtMessageTemplateModel.MsgParams Is Nothing Then

                strSubject = udtMessageTemplateModel.MsgTemplateSubject
                strContent = udtMessageTemplateModel.MsgTemplateContent

            Else

                strSubject = ReplaceParamsWithValue(udtMessageTemplateModel.MsgTemplateSubject, udtMessageTemplateModel.MsgParams)
                strContent = ReplaceParamsWithValue(udtMessageTemplateModel.MsgTemplateContent, udtMessageTemplateModel.MsgParams)

            End If

            udtSentOutMessageModel = New SentOutMessageModel(strTempID, _
                                                             SentOutMessageModel.SO_MSG_RECORD_STATUS_P, _
                                                             strSubject, _
                                                             strContent, _
                                                             udtMessageTemplateModel.MsgTemplateCategory, _
                                                             udtHCVUUserBLL.GetHCVUUser().UserID, _
                                                             CreateSentOutMsgRecipients(strTempID))

        Else

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [CreateSentOutMessage], Message = The selected message template is lost in this session")

        End If

        Return udtSentOutMessageModel
    End Function

    ' Create the [SentOutMsgRecipientModelCollection] of [SentOutMessageModel]
    Private Function CreateSentOutMsgRecipients(ByVal strSentOutMsgID As String) As SentOutMsgRecipientModelCollection
        Dim udtSentOutMsgRecipientModelCollection As SentOutMsgRecipientModelCollection = New SentOutMsgRecipientModelCollection()
        Dim udtSentOutMsgRecipientModel As SentOutMsgRecipientModel
        Dim dtSelectedRecipient As DataTable
        Dim i As Integer

        If lblRecipient_v1.Text <> "" Then

            udtSentOutMsgRecipientModel = New SentOutMsgRecipientModel(strSentOutMsgID, SentOutMsgRecipientModel.PROFESSION_NA, SentOutMsgRecipientModel.SCHEME_NA)
            udtSentOutMsgRecipientModelCollection.Add(udtSentOutMsgRecipientModel)

        Else

            If Not Session(SESS_SELECTED_RECIPIENT) Is Nothing Then

                dtSelectedRecipient = Session(SESS_SELECTED_RECIPIENT)
                For i = 0 To dtSelectedRecipient.Rows.Count - 1

                    udtSentOutMsgRecipientModel = New SentOutMsgRecipientModel(strSentOutMsgID, _
                                                                               dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession).ToString(), _
                                                                               dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme).ToString())
                    udtSentOutMsgRecipientModelCollection.Add(udtSentOutMsgRecipientModel)

                Next

            Else

                Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [CreateSentOutMsgRecipients], Message = The selected recipient is lost in this session")

            End If

        End If

        Return udtSentOutMsgRecipientModelCollection
    End Function

    ' Write the created [SentOutMessageModel] to DB and Load all component in [View 4 - Completed to Create Message]
    Private Sub ConfirmCreateMessage()
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtGeneralFunction As GeneralFunction = New GeneralFunction()
        Dim udtSentOutMessageBLL As SentOutMessageBLL = New SentOutMessageBLL()
        Dim udtSentOutMessageModel As SentOutMessageModel
        Dim udtSentOutMsgRecipientModel As SentOutMsgRecipientModel
        Dim strSentOutMsgID As String
        Dim udcInfoMessageBox As CustomControls.InfoMessageBox
        Dim strSelectedTemplateID As String
        Dim strSelectedInputParam As String
        Dim strCreateDtm As String

        If Not Session(SESS_CREATED_SENT_OUT_MSG) Is Nothing Then

            strSelectedTemplateID = GetSelectedTemplateIDforAuditLog()

            udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, strSelectedTemplateID)
            udtAuditLogEntry.AddDescripton(AuditLogDesc.Recipient, GetSelectedRecipientForAuditLog())
            strSelectedInputParam = GetSelectedInputParamForAuditLog()
            If Not strSelectedInputParam Is Nothing Then
                udtAuditLogEntry.AddDescripton(AuditLogDesc.InputParam, strSelectedInputParam)
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.Confirmation_Confirm_ID, AuditLogDesc.Confirmation_Confirm)

            udtSentOutMessageModel = Session(SESS_CREATED_SENT_OUT_MSG)

            strSentOutMsgID = udtGeneralFunction.GenerateSentOutMsgID()
            udtSentOutMessageModel.SentOutMsgID = strSentOutMsgID
            For Each udtSentOutMsgRecipientModel In udtSentOutMessageModel.SentOutMsgRecipients
                udtSentOutMsgRecipientModel.SentOutMsgID = strSentOutMsgID
            Next

            udtSentOutMessageBLL.WriteSentOutMsgToDB(udtSentOutMessageModel)
            strCreateDtm = udtFormatter.convertDateTime(udtGeneralFunction.GetSystemDateTime())

            lblMessageID_v4.Text = udtSentOutMessageModel.SentOutMsgID
            lblCreatedDateTime_v4.Text = strCreateDtm

            udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, strSelectedTemplateID)
            udtAuditLogEntry.AddDescripton(AuditLogDesc.SentOutMessageID, udtSentOutMessageModel.SentOutMsgID)
            udtAuditLogEntry.AddDescripton(AuditLogDesc.CreateDtm, strCreateDtm)
            udtAuditLogEntry.WriteLog(AuditLogDesc.Success_ID, AuditLogDesc.Success)

            udcInfoMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetInfoMessageBox()
            udcInfoMessageBox.Clear()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.AddMessage(FUNCT_CODE, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()

        Else

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [ConfirmCreateMessage], Message = The created message is lost in this session")

        End If
    End Sub

    ' Validate the Message Creation in [View 1 - Preview Template and Input Parameter]
    Private Function ValidateCreateMessage() As Boolean
        Dim udcMessageBox As CustomControls.MessageBox

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Clear()

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())

        ' Check if there is no error
        If ValidateSelectRecipient(udcMessageBox) And ValidateInputParameter(udcMessageBox) Then

            udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientInputParam_Next_ID, AuditLogDesc.AddRecipientInputParam_Next)
            Return True

        Else

            udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientInputParam_Next_ID, AuditLogDesc.AddRecipientInputParam_Next)
            udcMessageBox.BuildMessageBox(STR_RESOURCE_KEY_VALIDATION_FAIL, udtAuditLogEntry, AuditLogDesc.AddRecipientInputParam_Next_Fail_ID, AuditLogDesc.AddRecipientInputParam_Next_Fail)
            CType(FindParentPage(Me.Parent), Inbox).SetFocusToMessageBox()

            Return False

        End If
    End Function

    ' Validate the Input Parameter in [View 1 - Preview Template and Input Parameter]
    Private Function ValidateInputParameter(ByRef udcMessageBox As CustomControls.MessageBox) As Boolean
        Dim udtMessageTemplateModel As MessageTemplateModel
        Dim udtMsgParam As MessageParameterModel
        Dim udtDynamicControl As DynamicControlBase
        Dim imgParamAlertTemp As WebControls.Image
        Dim blnResult As Boolean = True
        Dim strSelectedInputParam As StringBuilder = New StringBuilder()

        If Not Session(SESS_SELECTED_TEMPLATE) Is Nothing Then

            udtMessageTemplateModel = Session(SESS_SELECTED_TEMPLATE)

            If Not udtMessageTemplateModel.MsgParams Is Nothing Then

                For Each udtMsgParam In udtMessageTemplateModel.MsgParams
                    udtDynamicControl = CType(phParam_v1.FindControl(udtMsgParam.MsgParamID), DynamicControlBase)
                    imgParamAlertTemp = CType(phParam_v1.FindControl("imgParamAlert" & udtMsgParam.MsgParamID), WebControls.Image)
                    imgParamAlertTemp.Visible = False

                    strSelectedInputParam.Append("[")
                    strSelectedInputParam.Append(udtDynamicControl.ID)
                    strSelectedInputParam.Append(":")

                    ' Check the Input Parameter is input
                    If udtDynamicControl.HasInputValue() Then

                        strSelectedInputParam.Append(udtDynamicControl.GetText())

                        ' Check the Input Parameter is not valid
                        If Not udtDynamicControl.IsValid() Then
                            udcMessageBox.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00002, STR_SYSTEM_MESSAGE_PARAM_S, Server.HtmlEncode(PARAM_OPEN_TAG_FOR_DISPLAY & udtMsgParam.MsgParamName & PARAM_CLOSE_TAG_FOR_DISPLAY))
                            imgParamAlertTemp.Visible = True
                            blnResult = False
                        End If

                    Else

                        strSelectedInputParam.Append(AuditLogDesc.NoInputValue)

                        udcMessageBox.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00001, STR_SYSTEM_MESSAGE_PARAM_S, Server.HtmlEncode(PARAM_OPEN_TAG_FOR_DISPLAY & udtMsgParam.MsgParamName & PARAM_CLOSE_TAG_FOR_DISPLAY))
                        imgParamAlertTemp.Visible = True
                        blnResult = False

                    End If

                    strSelectedInputParam.Append("],")

                Next

                strSelectedInputParam.Remove(strSelectedInputParam.Length - 1, 1)
                udtAuditLogEntry.AddDescripton(AuditLogDesc.InputParam, strSelectedInputParam.ToString())

            End If

        Else

            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [ValidateInputParameter], Message = The selected message template is lost in this session")

        End If

        Return blnResult
    End Function

    ' Validate to add recipient from [Pop-up Window for "Add Recipient"] into the GridView - [gvSelectRecipient] in [View 1 - Preview Template and Input Parameter]
    Private Function ValidateAddRecipient() As Boolean
        Dim blnResult As Boolean = True
        Dim strSelectedProfession As String = ""
        Dim strSelectedScheme As String = ""

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())

        udcMessageBoxPopUp.Clear()

        imgSelectRecipientAlert1_v2.Visible = False
        imgSelectRecipientAlert2_v2.Visible = False
        imgHealthProfessionAlert_v2.Visible = False
        imgSchemeAlert_v2.Visible = False

        ' Check if [All Enrolled Service Provider] is selected
        If rdoAllEnrolledSP_v2.Checked = True AndAlso rdoSelectRecipientBy_P_S_v2.Checked = False Then

            udtAuditLogEntry.AddDescripton(AuditLogDesc.AllEnrolledSP, "Y")
            udtAuditLogEntry.AddDescripton(AuditLogDesc.Profession, SentOutMsgRecipientModel.PROFESSION_NA)
            udtAuditLogEntry.AddDescripton(AuditLogDesc.Scheme, SentOutMsgRecipientModel.SCHEME_NA)
            udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_Add_ID, AuditLogDesc.AddRecipientPopUp_Add)
            Return True

            ' Check if there is no selection
        ElseIf rdoAllEnrolledSP_v2.Checked = False AndAlso rdoSelectRecipientBy_P_S_v2.Checked = False AndAlso Session(SESS_SELECTED_RECIPIENT) Is Nothing Then

            udtAuditLogEntry.AddDescripton(AuditLogDesc.AllEnrolledSP, AuditLogDesc.NoInputValue)
            udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_Add_ID, AuditLogDesc.AddRecipientPopUp_Add)

            udcMessageBoxPopUp.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00004)
            udcMessageBoxPopUp.BuildMessageBox(STR_RESOURCE_KEY_VALIDATION_FAIL, udtAuditLogEntry, AuditLogDesc.AddRecipientPopUp_Add_Fail_ID, AuditLogDesc.AddRecipientPopUp_Add_Fail)
            imgSelectRecipientAlert1_v2.Visible = True
            imgSelectRecipientAlert2_v2.Visible = True
            ModalPopupExtenderAddRecipient.Show()

            Return False

            ' Check if [Select Recipient by Health Profession and/or Scheme] is selected
        ElseIf (rdoAllEnrolledSP_v2.Checked = False AndAlso rdoSelectRecipientBy_P_S_v2.Checked = True) OrElse (Not Session(SESS_SELECTED_RECIPIENT) Is Nothing) Then

            ' Log the choice of [Selected Service Provider] of [Select Recipient]
            udtAuditLogEntry.AddDescripton(AuditLogDesc.AllEnrolledSP, "N")

            ' Check if the selection of [Health Profession] is missing
            'If chkHealthProfession_v2.Checked = True AndAlso ddlHealthProfession_v2.SelectedIndex <= 0 Then
            If ddlHealthProfession_v2.SelectedIndex <= 0 Then

                udtAuditLogEntry.AddDescripton(AuditLogDesc.Profession, "null")

                imgHealthProfessionAlert_v2.Visible = True
                udcMessageBoxPopUp.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00008)

                blnResult = False
            End If

            ' Check if the selection of [Scheme] is missing
            If chkScheme_v2.Checked = True AndAlso ddlScheme_v2.SelectedIndex <= 0 Then
                udtAuditLogEntry.AddDescripton(AuditLogDesc.Scheme, SentOutMsgRecipientModel.SCHEME_NA)

                imgSchemeAlert_v2.Visible = True
                udcMessageBoxPopUp.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00009)

                blnResult = False
            End If

            ' Check if the selection of [Health Profession] and/or [Scheme] is/are missing
            If blnResult = False Then
                udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_Add_ID, AuditLogDesc.AddRecipientPopUp_Add)

                udcMessageBoxPopUp.BuildMessageBox(STR_RESOURCE_KEY_VALIDATION_FAIL, udtAuditLogEntry, AuditLogDesc.AddRecipientPopUp_Add_Fail_ID, AuditLogDesc.AddRecipientPopUp_Add_Fail)
                ModalPopupExtenderAddRecipient.Show()

                Return False
            End If

            ' Check both [Health Profession] and [Scheme] are not selected specifically
            If ddlHealthProfession_v2.SelectedValue = "0" AndAlso ddlScheme_v2.SelectedValue = "-1" Then
                udtAuditLogEntry.AddDescripton(AuditLogDesc.Profession, SentOutMsgRecipientModel.PROFESSION_NA)
                udtAuditLogEntry.AddDescripton(AuditLogDesc.Scheme, SentOutMsgRecipientModel.SCHEME_NA)
                udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_Add_ID, AuditLogDesc.AddRecipientPopUp_Add)

                udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmAddAllEnrolledSPPopUp_ID, AuditLogDesc.ConfirmAddAllEnrolledSPPopUp)

                'imgHealthProfessionAlert_v2.Visible = True
                'imgSchemeAlert_v2.Visible = True
                'udcMessageBoxPopUp.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00005)
                'udcMessageBoxPopUp.BuildMessageBox(STR_RESOURCE_KEY_VALIDATION_FAIL, udtAuditLogEntry, AuditLogDesc.AddRecipientPopUp_Add_Fail_ID, AuditLogDesc.AddRecipientPopUp_Add_Fail)
                ModalPopupExtenderAddRecipient.Show()
                ModalPopupExtenderConfirmAddAllEnrolledSP.Show()

                Return False
            End If

            ' Check both [Health Profession] and [Scheme] are selected
            If ddlHealthProfession_v2.SelectedValue <> "0" AndAlso ddlScheme_v2.SelectedValue <> "-1" Then
                Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection
                Dim udtSchemeBackOfficeBLL As SchemeBackOfficeBLL = New SchemeBackOfficeBLL()

                udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()
                If Not udtSchemeBackOfficeList Is Nothing Then

                    If udtSchemeBackOfficeList.Item(ddlScheme_v2.SelectedIndex - 1).SchemeCode() = ddlScheme_v2.SelectedValue.Trim() Then

                        ' Check the selected [Profession] is eligible for the selected [Scheme]
                        If Not udtSchemeBackOfficeList.Item(ddlScheme_v2.SelectedIndex - 1).EligibleProfesional(ddlHealthProfession_v2.SelectedValue.Trim()) Then
                            udtAuditLogEntry.AddDescripton(AuditLogDesc.Profession, ddlHealthProfession_v2.SelectedValue.Trim())
                            udtAuditLogEntry.AddDescripton(AuditLogDesc.Scheme, ddlScheme_v2.SelectedValue.Trim())
                            udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_Add_ID, AuditLogDesc.AddRecipientPopUp_Add)

                            imgHealthProfessionAlert_v2.Visible = True
                            imgSchemeAlert_v2.Visible = True
                            udcMessageBoxPopUp.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00006)
                            udcMessageBoxPopUp.BuildMessageBox(STR_RESOURCE_KEY_VALIDATION_FAIL, udtAuditLogEntry, AuditLogDesc.AddRecipientPopUp_Add_Fail_ID, AuditLogDesc.AddRecipientPopUp_Add_Fail)
                            ModalPopupExtenderAddRecipient.Show()

                            Return False
                        End If

                    Else

                        Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [ValidateAddRecipient], Message = The Scheme Code does not match with the bound value in the DropDownList - [ddlScheme_v2]")

                    End If

                Else

                    Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [ValidateAddRecipient], Message = Unable to retrieve the effective scheme information for validation")

                End If
            End If

            ' Identify Selected Profession and Selected Scheme
            If ddlHealthProfession_v2.SelectedValue <> "0" AndAlso ddlScheme_v2.SelectedValue <> "-1" Then
                strSelectedProfession = ddlHealthProfession_v2.SelectedValue.Trim()
                strSelectedScheme = ddlScheme_v2.SelectedValue.Trim()
            ElseIf ddlHealthProfession_v2.SelectedValue <> "0" AndAlso ddlScheme_v2.SelectedValue = "-1" Then
                strSelectedProfession = ddlHealthProfession_v2.SelectedValue.Trim()
                strSelectedScheme = SentOutMsgRecipientModel.SCHEME_NA
            ElseIf ddlHealthProfession_v2.SelectedValue = "0" AndAlso ddlScheme_v2.SelectedValue <> "-1" Then
                strSelectedProfession = SentOutMsgRecipientModel.PROFESSION_NA
                strSelectedScheme = ddlScheme_v2.SelectedValue.Trim()
            End If

            ' Log Selected Profession and Selected Scheme
            udtAuditLogEntry.AddDescripton(AuditLogDesc.Profession, strSelectedProfession)
            udtAuditLogEntry.AddDescripton(AuditLogDesc.Scheme, strSelectedScheme)
            udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_Add_ID, AuditLogDesc.AddRecipientPopUp_Add)

            ' Check is there any selected recipient
            If Not Session(SESS_SELECTED_RECIPIENT) Is Nothing Then
                Dim dtSelectedRecipient As DataTable
                Dim i As Integer

                dtSelectedRecipient = Session(SESS_SELECTED_RECIPIENT)
                For i = 0 To dtSelectedRecipient.Rows.Count - 1

                    ' Check the selected combination of [Health Profession] and [Scheme] is duplicated or not
                    If strSelectedProfession = dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession).ToString() AndAlso _
                       strSelectedScheme = dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme).ToString() Then

                        imgHealthProfessionAlert_v2.Visible = True
                        imgSchemeAlert_v2.Visible = True
                        udcMessageBoxPopUp.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00007)
                        udcMessageBoxPopUp.BuildMessageBox(STR_RESOURCE_KEY_VALIDATION_FAIL, udtAuditLogEntry, AuditLogDesc.AddRecipientPopUp_Add_Fail_ID, AuditLogDesc.AddRecipientPopUp_Add_Fail)
                        ModalPopupExtenderAddRecipient.Show()

                        Return False

                    End If

                Next
            End If

            ' Invalid Action
            Else

                Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [ValidateAddRecipient], Message = Invalid action in [Select Recipient]")

            End If

            Return blnResult
    End Function

    ' Validate the selected recipient in [View 1 - Preview Template and Input Parameter]
    Private Function ValidateSelectRecipient(ByRef udcMessageBox As CustomControls.MessageBox) As Boolean
        imgRecipientAlert_v1.Visible = False

        ' Check if there is no recipient added
        If lblRecipient_v1.Text = "" AndAlso Session(SESS_SELECTED_RECIPIENT) Is Nothing Then
            udtAuditLogEntry.AddDescripton(AuditLogDesc.Recipient, AuditLogDesc.NoInputValue)

            imgRecipientAlert_v1.Visible = True
            udcMessageBox.AddMessage(FUNCT_CODE, SeverityCode.SEVE, MsgCode.MSG00003)
            Return False
        End If

        ' Log the added [Recipient]
        If lblRecipient_v1.Text <> "" Then

            udtAuditLogEntry.AddDescripton(AuditLogDesc.Recipient, AuditLogDesc.AllEnrolledSP)

        ElseIf Not Session(SESS_SELECTED_RECIPIENT) Is Nothing Then

            Dim dtSelectedRecipient As DataTable
            Dim i As Integer
            Dim strSelectedRecipient As StringBuilder = New StringBuilder()

            dtSelectedRecipient = Session(SESS_SELECTED_RECIPIENT)

            strSelectedRecipient.Append("[")
            strSelectedRecipient.Append(dtSelectedRecipient.Rows(0).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession).ToString())
            strSelectedRecipient.Append(",")
            strSelectedRecipient.Append(dtSelectedRecipient.Rows(0).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme).ToString())
            strSelectedRecipient.Append("]")
            For i = 1 To dtSelectedRecipient.Rows.Count - 1
                strSelectedRecipient.Append(",[")
                strSelectedRecipient.Append(dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession).ToString())
                strSelectedRecipient.Append(",")
                strSelectedRecipient.Append(dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme).ToString())
                strSelectedRecipient.Append("]")
            Next

            udtAuditLogEntry.AddDescripton(AuditLogDesc.Recipient, strSelectedRecipient.ToString())

        End If

        Return True
    End Function
#End Region

#Region "Event Handler of GridView - gvSelectTemplate"
    Protected Sub gvSelectTemplate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_MESSAGE_TEMPLATE_LIST)
        'GridviewFunction.GridViewPageIndexChangingHandler(sender, e, Session(SESS_MESSAGE_TEMPLATE_LIST), ViewState("SortExpression_" & gvSelectTemplate.ID), ViewState("SortDirection_" & gvSelectTemplate.ID))
    End Sub

    Protected Sub gvSelectTemplate_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_MESSAGE_TEMPLATE_LIST)
        'GridviewFunction.GridViewPreRenderHandler(sender, e, Session(SESS_MESSAGE_TEMPLATE_LIST), GetGlobalResourceObject("Text", "GridPageInfo"), ViewState("SortExpression_" & gvSelectTemplate.ID), ViewState("SortDirection_" & gvSelectTemplate.ID), "~/Images/others/arrowblank.png", "~/Images/others/arrowup.png", "~/Images/others/arrowdown.png")
    End Sub

    'To handle the Click Event of LinkButton in the GridView
    Protected Sub gvSelectTemplate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strTemplateID As String

            strTemplateID = e.CommandArgument.ToString().Trim()
            udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, strTemplateID)
            udtAuditLogEntry.WriteLog(AuditLogDesc.SelectTemplate_ID, AuditLogDesc.SelectTemplate)

            LoadTemplateInput(strTemplateID)
            LoadSelectRecipient()

            udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, strTemplateID)
            udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientInputParam_ID, AuditLogDesc.AddRecipientInputParam)

            RaiseEvent MessageTemplateSelected(Me, strTemplateID)
            mvCreateMessage.ActiveViewIndex = MultiViewEnum.InputParameterView
        End If
    End Sub

    'To create the image button in header row after row created
    Protected Sub gvSelectTemplate_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

    End Sub

    'To assign CommandArgument of LinkButton and adjust the data in DataRow during data binding
    Protected Sub gvSelectTemplate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lbtnTemplateID As LinkButton
            Dim lblCategory_v0 As Label
            Dim lblTemplateCreationDate_v0 As Label
            Dim strMsgTemplateCategoryDisplayText As String = ""

            lbtnTemplateID = CType(e.Row.FindControl("lbtnTemplateID"), LinkButton)
            lbtnTemplateID.Text = lbtnTemplateID.Text.Trim()
            lbtnTemplateID.CommandArgument = lbtnTemplateID.Text

            lblCategory_v0 = CType(e.Row.FindControl("lblCategory_v0"), Label)
            Status.GetDescriptionFromDBCode(MessageTemplateModel.STATUS_DATA_CLASS, lblCategory_v0.Text.Trim(), strMsgTemplateCategoryDisplayText, String.Empty)
            lblCategory_v0.Text = strMsgTemplateCategoryDisplayText

            lblTemplateCreationDate_v0 = CType(e.Row.FindControl("lblTemplateCreationDate_v0"), Label)
            lblTemplateCreationDate_v0.Text = udtFormatter.convertDate(CType(lblTemplateCreationDate_v0.Text, DateTime))
        End If
    End Sub

    Protected Sub gvSelectTemplate_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_MESSAGE_TEMPLATE_LIST)
    End Sub
#End Region

#Region "Event Handler of GridView - gvSelectRecipient"
    'To handle the Delete Event of ImageButton in the GridView
    Protected Sub gvSelectRecipient_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        If TypeOf e.CommandSource Is ImageButton Then
            Dim dtSelectedRecipient As DataTable
            Dim i As Integer

            If Not Session(SESS_SELECTED_RECIPIENT) Is Nothing Then

                i = CInt(e.CommandArgument.ToString())
                dtSelectedRecipient = Session(SESS_SELECTED_RECIPIENT)

                udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
                udtAuditLogEntry.AddDescripton(AuditLogDesc.Profession, dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Profession).ToString())
                udtAuditLogEntry.AddDescripton(AuditLogDesc.Scheme, dtSelectedRecipient.Rows(i).Item(SentOutMessageBLL.Table_SentOutMsgRecipient_SOMR.SOMR_Scheme).ToString())
                udtAuditLogEntry.WriteLog(AuditLogDesc.RemoveRecipient_ID, AuditLogDesc.RemoveRecipient)

                dtSelectedRecipient.Rows.RemoveAt(i)

                If dtSelectedRecipient.Rows.Count = 0 Then
                    dtSelectedRecipient = Nothing
                    gvSelectRecipient.DataSource = Nothing

                    ibtnAddRecipient_v1.Visible = True
                    ibtnResetRecipient_v1.Visible = False
                    tblRecipient_v1.Visible = False
                Else
                    gvSelectRecipient.DataSource = dtSelectedRecipient.DefaultView
                End If

                Session(SESS_SELECTED_RECIPIENT) = dtSelectedRecipient

                gvSelectRecipient.DataBind()

            Else

                Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [gvSelectRecipient_RowCommand], Message = The selected recipient is lost in this session")

            End If
        End If
    End Sub

    'To assign CommandArgument of ImageButton - "ibtnRemove_v1" during data binding
    Protected Sub gvSelectRecipient_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim lblSelectRecipient_Empty As Label

            'lblSelectRecipient_Empty = CType(e.Row.FindControl("lblHealthProfession_v1"), Label)
            'If lblSelectRecipient_Empty.Text = "-1" Then

            'e.Row.Cells(0).ColumnSpan = e.Row.Cells.Count
            'e.Row.Cells(1).Visible = False
            'e.Row.Cells(2).Visible = False

            'Else

            Dim ibtnDelete As ImageButton

            ibtnDelete = CType(e.Row.FindControl("ibtnRemove_v1"), ImageButton)
            ibtnDelete.CommandArgument = intSelectedRecipientIndex

            intSelectedRecipientIndex += 1

            'End If
        End If
    End Sub
#End Region

#Region "Event Handler of View 1 - viewInputParameter"
    ' [Preview Template] Button
    Protected Sub ibtnPreviewTemplate_v1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
        udtAuditLogEntry.WriteLog(AuditLogDesc.PreviewTemplate_ID, AuditLogDesc.PreviewTemplate)

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        ibtnPreviewTemplate_v1.Visible = False
        ibtnHideTemplate_v1.Visible = True
        panPreviewTemplate.Visible = True
    End Sub

    ' [Hide Template] Button
    Protected Sub ibtnHideTemplate_v1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
        udtAuditLogEntry.WriteLog(AuditLogDesc.HideTemplate_ID, AuditLogDesc.HideTemplate)

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        ibtnPreviewTemplate_v1.Visible = True
        ibtnHideTemplate_v1.Visible = False
        panPreviewTemplate.Visible = False
    End Sub

    ' [Add] Button
    Protected Sub ibtnAddRecipient_v1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
        udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipient_ID, AuditLogDesc.AddRecipient)

        ConfigPopUpAddRecipient()
    End Sub

    ' [Reset] Button
    Protected Sub ibtnResetRecipient_v1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
        udtAuditLogEntry.WriteLog(AuditLogDesc.ResetRecipient_ID, AuditLogDesc.ResetRecipient)

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        ResetRecipient()
    End Sub

    ' [Cancel] Button
    Protected Sub ibtnCancel_v1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
        udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientInputParam_Cancel_ID, AuditLogDesc.AddRecipientInputParam_Cancel)

        RaiseEvent MessageTemplateClosed(Me)
    End Sub

    ' [Next] Button
    Protected Sub ibtnNext_v1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If ValidateCreateMessage() Then
            LoadConfirmCreateMessage()
            mvCreateMessage.ActiveViewIndex = MultiViewEnum.ConfirmCreateMessageView
        End If
    End Sub
#End Region

#Region "Event Handler of Pop-up Window for Add Recipient"
    ' [All Enrolled Service Provider] Radio Button
    Protected Sub rdoAllEnrolledSP_v2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateRecipientChoiceSelection()
    End Sub

    ' [Select by Health Profession and/or Scheme] Radio Button
    Protected Sub rdoSelectRecipientBy_P_S_v2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdateRecipientChoiceSelection()
    End Sub

    ' [Add] Button to add recipient into the GridView - [gvSelectRecipient] in [View 1 - viewInputParameter]
    Protected Sub ibtnPopUpAddRecipient_Add_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        If ValidateAddRecipient() Then
            AddRecipient()
        End If
    End Sub

    ' [Cancel] Button"]
    Protected Sub ibtnPopUpAddRecipient_Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
        udtAuditLogEntry.WriteLog(AuditLogDesc.AddRecipientPopUp_Cancel_ID, AuditLogDesc.AddRecipientPopUp_Cancel)

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True
    End Sub
#End Region

#Region "Event Handler of Pop-up Window for [Confirmation] of Adding [All Enrolled Service Providers]"
    Protected Sub ibtnPopUpConfirmAddAllEnrolledSP_Confirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmAddAllEnrolledSPPopUp_Confirm_ID, AuditLogDesc.ConfirmAddAllEnrolledSPPopUp_Confirm)

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        ResetRecipient()
        AddRecipient()
    End Sub

    Protected Sub ibtnPopUpConfirmAddAllEnrolledSP_Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcMessageBox As CustomControls.MessageBox

        udtAuditLogEntry.WriteLog(AuditLogDesc.ConfirmAddAllEnrolledSPPopUp_Cancel_ID, AuditLogDesc.ConfirmAddAllEnrolledSPPopUp_Cancel)

        udcMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetMessageBox()
        udcMessageBox.Visible = True

        ModalPopupExtenderAddRecipient.Show()
    End Sub
#End Region

#Region "Event Handler of View 3 - viewConfirmCreateMessage"
    ' [Back] Button
    Protected Sub ibtnBack_v3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udcInfoMessageBox As CustomControls.InfoMessageBox
        Dim strSelectedInputParam As String

        udcInfoMessageBox = CType(FindParentPage(Me.Parent), Inbox).GetInfoMessageBox()
        udcInfoMessageBox.Clear()

        If Not Session(SESS_CREATED_SENT_OUT_MSG) Is Nothing Then
            udtAuditLogEntry.AddDescripton(AuditLogDesc.TemplateID, GetSelectedTemplateIDforAuditLog())
            udtAuditLogEntry.AddDescripton(AuditLogDesc.Recipient, GetSelectedRecipientForAuditLog())
            strSelectedInputParam = GetSelectedInputParamForAuditLog()
            If Not strSelectedInputParam Is Nothing Then
                udtAuditLogEntry.AddDescripton(AuditLogDesc.InputParam, strSelectedInputParam)
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.Confirmation_Back_ID, AuditLogDesc.Confirmation_Back)
        Else
            Throw New Exception("Error: Class = [HCVU.CreateMessage], Method = [ibtnBack_v3_Click], Message = The created message is lost in this session")
        End If

        mvCreateMessage.ActiveViewIndex = MultiViewEnum.InputParameterView
    End Sub

    ' [Confirm] Button
    Protected Sub ibtnConfirm_v3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ConfirmCreateMessage()
        RaiseEvent MessageTemplateCreated(Me)
        mvCreateMessage.ActiveViewIndex = MultiViewEnum.CompletedCreateMessageView
    End Sub
#End Region

End Class
