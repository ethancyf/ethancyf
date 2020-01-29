Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.ComObject

''' <summary>
''' MessageBox Custom Controlto display system message
''' </summary>
''' <remarks></remarks>
<DefaultProperty("Text"), ToolboxData("<{0}:MessageBox runat=server></{0}:MessageBox>")> _
Public Class MessageBox
    Inherits System.Web.UI.WebControls.WebControl

    ''' <summary>
    ''' Label of Header
    ''' </summary>
    ''' <remarks></remarks>
    Private lblMsgMainStatement As Label

    ''' <summary>
    ''' Datalist of message
    ''' </summary>
    ''' <remarks></remarks>
    Private dlMsgDetails As DataList

    ''' <summary>
    ''' Name of ViewState of code of message datatable
    ''' </summary>
    ''' <remarks></remarks>
    Public Const VS_MESSAGEBOX_CODETABLE As String = "VS_MESSAGEBOX_CODETABLE"

    ''' <summary>
    ''' Name of ViewState of resourceKey of Header
    ''' </summary>
    ''' <remarks></remarks>
    Public Const VS_MESSAGEBOX_HEADERKEY As String = "VS_MESSAGEBOX_HEADERKEY"

    ''' <summary>
    ''' Name of ViewState of flag of whether the MessageBox is showed or not
    ''' </summary>
    ''' <remarks></remarks>
    Public Const VS_MESSAGEBOX_SHOWED As String = "VS_MESSAGEBOX_SHOWED"

    Public Const VS_MESSAGEDESCBOX As String = "VS_MESSAGESHOWBOX"

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------

    Public Const VS_SCROLL_TO_HEIGHT As String = "VS_SCROLL_TO_HEIGHT"
    ' CRE12-001 eHS and PCD integration [End][Koala]

    Private strFuncCode As String = ""
    Private _udtAuditLogEntry As AuditLogEntry
    Private _strAuditLogDesc As String = Nothing
    Private _strAuditLogUserID As String = Nothing
    Private _strAuditLogSPID As String = Nothing
    Private _strLogID As String = Nothing
    Private _strAuditLogDataEntryAccount As String = Nothing
    Private _objAuditLogInfo As AuditLogInfo = Nothing

    Public Overrides Property Width() As System.Web.UI.WebControls.Unit
        Get
            Return MyBase.Width
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            MyBase.Width = value
        End Set
    End Property


    ''' <summary>
    ''' Overrides RenderContents
    ''' </summary>
    ''' <param name="writer"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        'Register(Me)
        MyBase.RenderContents(writer)
    End Sub

    Public Property ScrollToHeight() As Boolean
        Get
            Return CType(Me.ViewState(VS_SCROLL_TO_HEIGHT), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState(VS_SCROLL_TO_HEIGHT) = value
        End Set
    End Property
    'Private Sub Register(ByRef ctrl As Control)
    '    Dim c As Control
    '    For Each c In ctrl.Controls
    '        Register(c)
    '    Next
    '    Page.ClientScript.RegisterForEventValidation(ctrl.UniqueID)
    'End Sub

    ''' <summary>
    ''' Init Layout of MessageBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MessageBox_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim pnl As New Panel

        Dim tb As Table
        Dim tr As TableRow
        Dim td As TableCell
        Dim img As Image

        tb = New Table

        tb.CellPadding = 1
        tb.CellSpacing = 0
        tb.Style.Item("width") = "100%"

        tr = New TableRow
        td = New TableCell
        td.VerticalAlign = VerticalAlign.Middle
        td.HorizontalAlign = WebControls.HorizontalAlign.Left
        img = New Image
        img.ImageUrl = "~/Images/others/icon_caution.gif"
        td.Controls.Add(img)
        tr.Cells.Add(td)

        td = New TableCell
        td.VerticalAlign = VerticalAlign.Middle
        td.HorizontalAlign = WebControls.HorizontalAlign.Left
        'td.Style.Item("width") = "567px"
        lblMsgMainStatement = New Label
        lblMsgMainStatement.ID = CType(sender, MessageBox).UniqueID.Trim & "lblMsgMainStatement"
        lblMsgMainStatement.Style.Item("font-size") = "Medium"
        lblMsgMainStatement.Style.Item("font-weight") = "bold"
        lblMsgMainStatement.Style.Item("color") = "#636563"
        td.Controls.Add(lblMsgMainStatement)
        tr.Cells.Add(td)
        tb.Rows.Add(tr)

        tr = New TableRow
        td = New TableCell
        td.Style.Item("width") = "10px"
        td.VerticalAlign = VerticalAlign.Top
        td.HorizontalAlign = WebControls.HorizontalAlign.Left
        tr.Cells.Add(td)

        td = New TableCell
        'td.Style.Item("width") = "567px"
        td.VerticalAlign = VerticalAlign.Top
        td.HorizontalAlign = WebControls.HorizontalAlign.Left
        dlMsgDetails = New DataList
        dlMsgDetails.ID = CType(sender, MessageBox).UniqueID.Trim & "dlMsgDetails"
        dlMsgDetails.ItemTemplate = New DataListTemplate(DataControlRowType.DataRow, "")
        dlMsgDetails.RepeatColumns = 1
        dlMsgDetails.ShowHeader = False
        dlMsgDetails.ShowFooter = False
        dlMsgDetails.Width = Unit.Percentage(98)
        td.Controls.Add(dlMsgDetails)
        tr.Cells.Add(td)

        tb.Rows.Add(tr)

        pnl.Controls.Add(tb)

        pnl.BackColor = Drawing.Color.FromArgb(255, 224, 192)
        pnl.BorderColor = Drawing.Color.Red
        pnl.BorderWidth = Unit.Pixel(1)

        If Width = Unit.Empty Then
            pnl.Width = Unit.Percentage(100)
        Else
            pnl.Width = Width
        End If

        Controls.Add(pnl)

        Dim li As New LiteralControl("<br>")
        Controls.Add(li)

        Me.Visible = False

    End Sub

    ''' <summary>
    ''' Control PreRender
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Reload the data and build the MessageBox again
    ''' </remarks>
    Private Sub MessageBox_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not ViewState(VS_MESSAGEDESCBOX) Is Nothing AndAlso CType(ViewState(VS_MESSAGEDESCBOX), Boolean) Then
            Me.DisplayMessageDescBox()
        Else
            DisplayMessageBox()
        End If

    End Sub

    ''' <summary>
    ''' Init code of message datatable
    ''' </summary>
    ''' <remarks>
    ''' To build the structure of code of message datatable
    ''' </remarks>
    Private Function InitializeCodeTable() As DataTable
        Dim dtCode As DataTable
        dtCode = New DataTable
        dtCode.Columns.Add(New DataColumn("msgCode", GetType(String)))
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        dtCode.Columns.Add(New DataColumn("msgExtraInfo", GetType(String)))
        ' [CRE12-004] Statistic Enquiry [End][Koala]

        'If Not MsgBoxType = MessageBoxType.Normal Then
        dtCode.Columns.Add(New DataColumn("oldCharList", GetType(String())))
        dtCode.Columns.Add(New DataColumn("newCharList", GetType(String())))
        'End If
        Return dtCode
    End Function

    Private Function InitializeCodeTableWithDesc() As DataTable
        Dim dtCode As DataTable
        dtCode = New DataTable
        dtCode.Columns.Add(New DataColumn("msgCode", GetType(String)))
        dtCode.Columns.Add(New DataColumn("msgDesc", GetType(String)))
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        dtCode.Columns.Add(New DataColumn("msgExtraInfo", GetType(String)))
        ' [CRE12-004] Statistic Enquiry [End][Koala]

        'If Not MsgBoxType = MessageBoxType.Normal Then
        dtCode.Columns.Add(New DataColumn("oldCharList", GetType(String())))
        dtCode.Columns.Add(New DataColumn("newCharList", GetType(String())))
        'End If
        Return dtCode
    End Function

    ''' <summary>
    ''' Clear ViewState and set this control to invisible
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()
        ViewState(VS_MESSAGEBOX_CODETABLE) = Nothing
        ViewState(VS_MESSAGEBOX_HEADERKEY) = Nothing
        ViewState(VS_MESSAGEBOX_SHOWED) = Nothing
        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ViewState(VS_SCROLL_TO_HEIGHT) = Nothing
        ' CRE12-001 eHS and PCD integration [End][Koala]
        Me.Visible = False
    End Sub

    ''' <summary>
    ''' Build data into this control
    ''' </summary>
    ''' <remarks>
    ''' bind dtCode to dlMsgDetails and get the resource by strHeaderText to set text of lblMsgMainStatement
    ''' </remarks>
    ''' <param name="dtMessage">DataTable of message</param>
    ''' <param name="strHeaderText">ResourceKey of MessageBox header</param>
    Private Sub BuildData(ByRef dtMessage As DataTable, ByVal strHeaderText As String)
        Dim dt As DataTable
        Dim dr As DataRow
        Dim dv As DataView
        Dim i As Integer

        If Trim(strHeaderText) = "" Then
            Me.lblMsgMainStatement.Text = ""
        Else
            Me.lblMsgMainStatement.Text = strHeaderText
        End If

        dt = New DataTable
        dt.Columns.Add(New DataColumn("msgDesc", GetType(String)))
        dt.Columns.Add(New DataColumn("msgCode", GetType(String)))

        For i = 0 To dtMessage.Rows.Count - 1
            dr = dt.NewRow
            dr("msgDesc") = dtMessage.Rows(i).Item("msgDesc")
            dr("msgCode") = dtMessage.Rows(i).Item("msgCode")
            dt.Rows.Add(dr)

            ' [INT13-0003] Fix Smart ID return error exception [Start][Koala]
            ' [CRE12-004] Statistic Enquiry [Start][Koala]
            If dtMessage.Columns.Contains("msgExtraInfo") Then
                If Not String.IsNullOrEmpty(dtMessage.Rows(i).Item("msgExtraInfo").ToString) Then
                    dr = dt.NewRow
                    dr("msgDesc") = dtMessage.Rows(i).Item("msgExtraInfo")
                    dr("msgCode") = String.Empty
                    dt.Rows.Add(dr)
                End If
            End If

            ' [CRE12-004] Statistic Enquiry [End][Koala]
            ' [INT13-0003] Fix Smart ID return error exception [End][Koala]
        Next

        dv = New DataView(dt)
        dlMsgDetails.DataSource = dv
        dlMsgDetails.DataBind()

    End Sub

    ''' <summary>
    ''' Return code of message datatable from ViewState
    ''' </summary>
    ''' <returns>DataTable of code of message</returns>
    ''' <remarks></remarks>
    Public Function GetCodeTable() As DataTable
        Dim dtCode As DataTable
        If Not ViewState(VS_MESSAGEBOX_SHOWED) Is Nothing Then
            If Not CBool(ViewState(VS_MESSAGEBOX_SHOWED)) Then
                dtCode = CType(ViewState(VS_MESSAGEBOX_CODETABLE), DataTable)
                Return dtCode
            End If
        End If
        dtCode = Me.InitializeCodeTable
        Return dtCode
    End Function

#Region "Add Message"

    ''' <summary>
    ''' Add message to code of message datatable in ViewState
    ''' </summary>
    ''' <param name="FunctionCode">Function Code</param>
    ''' <param name="SeverityCode">Severity</param>
    ''' <param name="StrMsgCode">Message Code</param>
    ''' <remarks>
    ''' Code of messsage is combine of Function Code, Severity and Message Code
    ''' </remarks>
    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String)
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        AddMessageWithExtraInfo(FunctionCode, SeverityCode, StrMsgCode, String.Empty)
        'Dim dtCode As DataTable
        'If ViewState(VS_MESSAGEBOX_CODETABLE) Is Nothing OrElse Me.Visible OrElse (Not ViewState(VS_MESSAGEBOX_SHOWED) Is Nothing AndAlso CBool(ViewState(VS_MESSAGEBOX_SHOWED))) Then
        '    dtCode = InitializeCodeTable()
        '    Me.Visible = False
        'Else
        '    dtCode = CType(ViewState(VS_MESSAGEBOX_CODETABLE), DataTable)
        'End If
        'AddCodeTableRow(dtCode, FunctionCode, SeverityCode, StrMsgCode, Nothing, Nothing, String.Empty)
        'ViewState(VS_MESSAGEBOX_SHOWED) = False
        'ViewState(VS_MESSAGEBOX_CODETABLE) = dtCode

        ' [CRE12-004] Statistic Enquiry [End][Koala]
    End Sub

    ' [CRE12-004] Statistic Enquiry [Start][Koala]
    ''' <summary>
    ''' Add message with extra information
    ''' </summary>
    ''' <param name="FunctionCode"></param>
    ''' <param name="SeverityCode"></param>
    ''' <param name="StrMsgCode"></param>
    ''' <param name="strExtraInfo">Extra information will show under the message (Support HTML format)</param>
    ''' <remarks></remarks>
    Public Sub AddMessageWithExtraInfo(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByVal strExtraInfo As String)
        Dim dtCode As DataTable
        If ViewState(VS_MESSAGEBOX_CODETABLE) Is Nothing OrElse Me.Visible OrElse (Not ViewState(VS_MESSAGEBOX_SHOWED) Is Nothing AndAlso CBool(ViewState(VS_MESSAGEBOX_SHOWED))) Then
            dtCode = InitializeCodeTable()
            Me.Visible = False
        Else
            dtCode = CType(ViewState(VS_MESSAGEBOX_CODETABLE), DataTable)
        End If
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        AddCodeTableRow(dtCode, FunctionCode, SeverityCode, StrMsgCode, Nothing, Nothing, strExtraInfo)
        ' [CRE12-004] Statistic Enquiry [End][Koala]
        ViewState(VS_MESSAGEBOX_SHOWED) = False
        ViewState(VS_MESSAGEBOX_CODETABLE) = dtCode
    End Sub
    ' [CRE12-004] Statistic Enquiry [End][Koala]

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage)
        If Not objSystemMessage Is Nothing Then
            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Handle Replace Message
            Dim lstIdx As New List(Of String)
            Dim lstReplaceMessage As New List(Of String)

            objSystemMessage.GetReplaceMessage(String.Empty, lstIdx, lstReplaceMessage)

            If lstIdx Is Nothing Then
                AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode)
            Else
                AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, lstIdx.ToArray, lstReplaceMessage.ToArray)
            End If
            ' CRE19-003 (Opt voucher capping) [End][Winnie]
        End If
    End Sub


    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByVal strOldChar As String, ByVal strNewChar As String, Optional ByVal blnReplaceByMsg As Boolean = True)

        Dim strOldCharList(0) As String
        Dim strNewCharList(0) As String

        strOldCharList(0) = strOldChar
        strNewCharList(0) = strNewChar

        AddMessage(FunctionCode, SeverityCode, StrMsgCode, strOldCharList, strNewCharList, blnReplaceByMsg)

    End Sub

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage, ByRef strOldCharList() As String, ByRef strNewCharList() As String, Optional ByVal blnReplaceByMsg As Boolean = True)
        If Not objSystemMessage Is Nothing Then
            AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, strOldCharList, strNewCharList, blnReplaceByMsg)
        End If
    End Sub

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage, ByVal strOldChar As String, ByVal strNewChar As String, Optional ByVal blnReplaceByMsg As Boolean = True)
        If Not objSystemMessage Is Nothing Then
            AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, strOldChar, strNewChar, blnReplaceByMsg)
        End If
    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByRef strOldCharList() As String, ByRef strNewCharList() As String, Optional ByVal blnReplaceByMsg As Boolean = True)
        Dim dtCode As DataTable
        If ViewState(VS_MESSAGEBOX_CODETABLE) Is Nothing OrElse Me.Visible OrElse (Not ViewState(VS_MESSAGEBOX_SHOWED) Is Nothing AndAlso CBool(ViewState(VS_MESSAGEBOX_SHOWED))) Then
            dtCode = InitializeCodeTable()
            Me.Visible = False
        Else
            dtCode = CType(ViewState(VS_MESSAGEBOX_CODETABLE), DataTable)
        End If
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        AddCodeTableRow(dtCode, FunctionCode, SeverityCode, StrMsgCode, strOldCharList, strNewCharList, String.Empty)
        ' [CRE12-004] Statistic Enquiry [End][Koala]
        ViewState(VS_MESSAGEBOX_SHOWED) = False
        ViewState(VS_MESSAGEBOX_CODETABLE) = dtCode
    End Sub

    Public Sub AddMessageDesc(ByVal strFunctionCode As String, ByVal strErrorCode As String, ByVal strErrorMsg As String)
        Dim dtCode As DataTable
        If ViewState(VS_MESSAGEBOX_CODETABLE) Is Nothing OrElse Me.Visible OrElse (Not ViewState(VS_MESSAGEBOX_SHOWED) Is Nothing AndAlso CBool(ViewState(VS_MESSAGEBOX_SHOWED))) Then
            dtCode = InitializeCodeTableWithDesc()
            Me.Visible = False
        Else
            dtCode = CType(ViewState(VS_MESSAGEBOX_CODETABLE), DataTable)
        End If
        Me.AddCodeTableRow(dtCode, strFunctionCode, strErrorCode, strErrorMsg)
        ViewState(VS_MESSAGEBOX_SHOWED) = False
        ViewState(VS_MESSAGEBOX_CODETABLE) = dtCode
    End Sub

    ''' <summary>
    ''' Add row into code of message datatable
    ''' </summary>
    ''' <param name="dtCode">DataTable of code</param>
    ''' <param name="FunctionCode">Function Code</param>
    ''' <param name="SeverityCode">Severity</param>
    ''' <param name="StrMsgCode">Message Code</param>
    ''' <remarks>
    ''' Code of messsage is combine of Function Code, Severity and Message Code
    ''' </remarks>
    Private Sub AddCodeTableRow(ByRef dtCode As DataTable, ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByRef strOldCharList() As String, ByRef strNewCharList() As String, ByVal strExtraInfo As String, Optional ByVal blnReplaceByMsg As Boolean = True)
        Dim drCode As DataRow
        drCode = dtCode.NewRow
        drCode("msgCode") = FunctionCode & "-" & SeverityCode & "-" & StrMsgCode
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        drCode("msgExtraInfo") = strExtraInfo
        ' [CRE12-004] Statistic Enquiry [End][Koala]
        drCode("oldCharList") = strOldCharList
        drCode("newCharList") = strNewCharList
        dtCode.Rows.Add(drCode)
    End Sub

    Private Sub AddCodeTableRow(ByRef dtCode As DataTable, ByVal strFunctionCode As String, ByVal strErrorCode As String, ByVal strMsg As String)
        Dim drCode As DataRow
        drCode = dtCode.NewRow
        drCode("msgCode") = "[" & strFunctionCode & "-" & strErrorCode & "]"
        drCode("msgDesc") = strMsg.Trim()
        dtCode.Rows.Add(drCode)
    End Sub

#End Region

#Region "Get Message"

    ''' <summary>
    ''' Get message by code
    ''' </summary>
    ''' <param name="code">Code of message</param>
    ''' <returns>Array of string of message and formatted code</returns>
    ''' <remarks>
    ''' Get message from resource by code
    ''' </remarks>
    Private Function GetMsg(ByVal code As String) As String()
        Dim s(2) As String
        s(0) = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))
        s(1) = "[" & code & "]"
        Return s
    End Function

    Private Function GetMsg(ByVal code As String, ByRef strOldCharList() As String, ByRef strNewCharList() As String) As String()
        Dim s(2) As String
        Dim i As Integer
        s(0) = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))
        For i = 0 To strOldCharList.Length - 1
            If Not strOldCharList(i) Is Nothing AndAlso Not strNewCharList(i) Is Nothing Then
                s(0) = s(0).Replace(strOldCharList(i), strNewCharList(i))
            End If
        Next
        s(1) = "[" & code & "]"
        Return s
    End Function

#End Region

#Region "Display Message"

    Private Sub DisplayMessageBox(Optional ByVal strHeaderKey As String = "")
        Dim dtCode As DataTable
        Dim dtMessage As DataTable
        Dim i As Integer
        Dim dr As DataRow
        Dim code As String
        Dim headertext As String
        Dim strMsg() As String
        Dim strOldCharList() As String
        Dim strNewCharList() As String

        Dim strMsgCode As String = String.Empty

        headertext = ""
        Me.Visible = False
        If Not ViewState(VS_MESSAGEBOX_CODETABLE) Is Nothing Then

            ViewState(VS_MESSAGEDESCBOX) = Nothing
            ViewState(VS_MESSAGEBOX_SHOWED) = True

            dtCode = CType(ViewState(VS_MESSAGEBOX_CODETABLE), DataTable)
            If dtCode.Rows.Count > 0 Then
                dtMessage = New DataTable
                dtMessage.Columns.Add(New DataColumn("msgDesc", GetType(String)))
                dtMessage.Columns.Add(New DataColumn("msgCode", GetType(String)))
                ' [CRE12-004] Statistic Enquiry [Start][Koala]
                dtMessage.Columns.Add(New DataColumn("msgExtraInfo", GetType(String)))
                ' [CRE12-004] Statistic Enquiry [End][Koala]
                For i = 0 To dtCode.Rows.Count - 1
                    code = CStr(dtCode.Rows(i).Item("msgCode"))
                    dr = dtMessage.NewRow
                    If dtCode.Rows(i).Item("oldCharList") Is DBNull.Value Then
                        strMsg = GetMsg(code)
                    Else
                        strOldCharList = CType(dtCode.Rows(i).Item("oldCharList"), String())
                        strNewCharList = CType(dtCode.Rows(i).Item("newCharList"), String())
                        strMsg = GetMsg(code, strOldCharList, strNewCharList)
                    End If
                    dr("msgDesc") = strMsg(0)
                    dr("msgCode") = strMsg(1)
                    ' [CRE12-004] Statistic Enquiry [Start][Koala]
                    dr("msgExtraInfo") = dtCode.Rows(i).Item("msgExtraInfo")
                    ' [CRE12-004] Statistic Enquiry [End][Koala]
                    dtMessage.Rows.Add(dr)
                Next
                If strHeaderKey <> "" Then
                    headertext = CStr(HttpContext.GetGlobalResourceObject("Text", strHeaderKey))
                    ViewState(VS_MESSAGEBOX_HEADERKEY) = strHeaderKey
                    If Not _udtAuditLogEntry Is Nothing Then
                        Dim k As Integer
                        For k = 0 To dtMessage.Rows.Count - 1
                            _udtAuditLogEntry.AddDescripton("message text", CStr(dtMessage.Rows(k).Item("msgDesc")) & " " & CStr(dtMessage.Rows(k).Item("msgCode")))
                            strMsgCode = strMsgCode + CStr(dtMessage.Rows(k).Item("msgCode")).Replace("[", "").Replace("]", "") + ";"
                        Next

                        _udtAuditLogEntry.AddDescripton("**********", strMsgCode)

                        If Not _strAuditLogUserID Is Nothing Then
                            _udtAuditLogEntry.WriteEndLog(_strLogID, _strAuditLogDesc, _strAuditLogUserID)
                        ElseIf Not _strAuditLogSPID Is Nothing Then
                            _udtAuditLogEntry.WriteEndLog(_strLogID, _strAuditLogDesc, _strAuditLogSPID, _strAuditLogDataEntryAccount)
                        Else
                            _udtAuditLogEntry.WriteEndLog(_strLogID, _strAuditLogDesc, _objAuditLogInfo)
                        End If
                    End If
                    'End If
                Else
                    If Not ViewState(VS_MESSAGEBOX_HEADERKEY) Is Nothing Then
                        strHeaderKey = CStr(ViewState(VS_MESSAGEBOX_HEADERKEY))
                        headertext = CStr(HttpContext.GetGlobalResourceObject("Text", strHeaderKey))
                    End If
                End If
                Me.BuildData(dtMessage, headertext)
                Me.Visible = True

            End If
        End If
    End Sub

    Private Sub DisplayMessageDescBox(Optional ByVal strHeaderKey As String = "")
        Dim dtCode As DataTable
        Dim dtMessage As DataTable
        Dim i As Integer
        Dim dr As DataRow
        Dim headertext As String
        Dim strMsg() As String

        Dim strMsgCode As String = String.Empty

        headertext = ""
        Me.Visible = False

        If Not ViewState(VS_MESSAGEBOX_CODETABLE) Is Nothing Then

            'Message Box description is stored in database: VS_MESSAGEDESCBOX = false
            ViewState(VS_MESSAGEDESCBOX) = True

            ViewState(VS_MESSAGEBOX_SHOWED) = True
            dtCode = CType(ViewState(VS_MESSAGEBOX_CODETABLE), DataTable)
            If dtCode.Rows.Count > 0 Then
                dtMessage = New DataTable
                dtMessage.Columns.Add(New DataColumn("msgDesc", GetType(String)))
                dtMessage.Columns.Add(New DataColumn("msgCode", GetType(String)))
                For i = 0 To dtCode.Rows.Count - 1
                    dr = dtMessage.NewRow

                    Dim strMsgAll As String = dtCode.Rows(i).Item("msgDesc").ToString()
                    Dim strCode As String = dtCode.Rows(i).Item("msgCode").ToString()
                    Dim strMsgLang As String = String.Empty

                    ' Seperate the Message by Delimilator = "|||"
                    Dim strResult() As String = strMsgAll.Split(New String() {"|||"}, System.StringSplitOptions.RemoveEmptyEntries)

                    If strResult.Length = 1 Then
                        strMsgLang = strResult(0)
                    End If
                    If strResult.Length > 1 Then
                        strMsgLang = strResult(0)
                        If Not HttpContext.Current Is Nothing Then
                            If Not HttpContext.Current.Session("language") Is Nothing Then
                                If LCase(HttpContext.Current.Session("language").ToString().Trim()) = "zh-tw" Then
                                    strMsgLang = strResult(1)
                                End If
                            End If
                        End If
                    End If

                    Dim strS(2) As String
                    strS(0) = strMsgLang
                    strS(1) = strCode
                    strMsg = strS

                    dr("msgDesc") = strMsg(0)
                    dr("msgCode") = strMsg(1)
                    dtMessage.Rows.Add(dr)
                Next
                If strHeaderKey <> "" Then
                    headertext = CStr(HttpContext.GetGlobalResourceObject("Text", strHeaderKey))
                    'headertext = strHeaderKey

                    ViewState(VS_MESSAGEBOX_HEADERKEY) = strHeaderKey
                    If Not _udtAuditLogEntry Is Nothing Then
                        Dim k As Integer
                        For k = 0 To dtMessage.Rows.Count - 1
                            _udtAuditLogEntry.AddDescripton("message text", CStr(dtMessage.Rows(k).Item("msgDesc")) & " " & CStr(dtMessage.Rows(k).Item("msgCode")))
                            strMsgCode = strMsgCode + CStr(dtMessage.Rows(k).Item("msgCode")).Replace("[", "").Replace("]", "") + ";"
                        Next

                        _udtAuditLogEntry.AddDescripton("**********", strMsgCode)

                        If Not _strAuditLogUserID Is Nothing Then
                            _udtAuditLogEntry.WriteEndLog(_strLogID, _strAuditLogDesc, _strAuditLogUserID)
                        ElseIf Not _strAuditLogSPID Is Nothing Then
                            _udtAuditLogEntry.WriteEndLog(_strLogID, _strAuditLogDesc, _strAuditLogSPID, _strAuditLogDataEntryAccount)
                        Else
                            _udtAuditLogEntry.WriteEndLog(_strLogID, _strAuditLogDesc, _objAuditLogInfo)
                        End If
                    End If
                    'End If
                Else
                    If Not ViewState(VS_MESSAGEBOX_HEADERKEY) Is Nothing Then
                        strHeaderKey = CStr(ViewState(VS_MESSAGEBOX_HEADERKEY))
                        headertext = CStr(HttpContext.GetGlobalResourceObject("Text", strHeaderKey))
                    End If
                End If
                Me.BuildData(dtMessage, headertext)
                Me.Visible = True

            End If
        End If
    End Sub

#End Region

#Region "Build Message Box"

    ''' <summary>
    ''' Build MessagBox with data
    ''' </summary>
    ''' <param name="strHeaderKey">ResourceKey of Header</param>
    ''' <remarks></remarks>
    Public Sub BuildMessageBox(Optional ByVal strHeaderKey As String = "")
        ViewState(VS_MESSAGEDESCBOX) = Nothing
        If Me.GetCodeTable.Rows.Count > 0 Then
            DisplayMessageBox(strHeaderKey)
            If ScrollToHeight Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "MessageBoxScript", "setTimeout('window.scrollBy(0, -(document.body.clientHeight));', 1); ", True)
            End If
        Else
            Me.Visible = False
        End If

    End Sub

    Public Sub BuildMessageBox(ByVal strHeaderKey As String, ByRef udtAuditLogEntry As AuditLogEntry, ByVal strLogID As String, ByVal strAuditLogDesc As String)
        _udtAuditLogEntry = udtAuditLogEntry
        _strAuditLogDesc = strAuditLogDesc
        _strAuditLogUserID = Nothing
        _strAuditLogSPID = Nothing
        _strAuditLogDataEntryAccount = Nothing
        _strLogID = strLogID
        _objAuditLogInfo = Nothing
        BuildMessageBox(strHeaderKey)
    End Sub

    Public Sub BuildMessageBox(ByVal strHeaderKey As String, ByRef udtAuditLogEntry As AuditLogEntry, ByVal strLogID As String, ByVal strAuditLogDesc As String, ByVal objAuditLogInfo As AuditLogInfo)
        _udtAuditLogEntry = udtAuditLogEntry
        _strAuditLogDesc = strAuditLogDesc
        _strAuditLogUserID = Nothing
        _strAuditLogSPID = Nothing
        _strAuditLogDataEntryAccount = Nothing
        _strLogID = strLogID
        _objAuditLogInfo = objAuditLogInfo
        BuildMessageBox(strHeaderKey)
    End Sub

    Public Sub BuildMessageBox(ByVal strHeaderKey As String, ByRef udtAuditLogEntry As AuditLogEntry, ByVal strAuditLogDesc As String, ByVal strLogID As String, ByVal strAuditLogUserID As String)
        _udtAuditLogEntry = udtAuditLogEntry
        _strAuditLogDesc = strAuditLogDesc
        _strAuditLogUserID = strAuditLogUserID
        _strAuditLogSPID = Nothing
        _strAuditLogDataEntryAccount = Nothing
        _strLogID = strLogID
        _objAuditLogInfo = Nothing
        BuildMessageBox(strHeaderKey)
    End Sub

    Public Sub BuildMessageBox(ByVal strHeaderKey As String, ByRef udtAuditLogEntry As AuditLogEntry, ByVal strAuditLogDesc As String, ByVal strLogID As String, ByVal strAuditLogSPID As String, ByVal strAuditLogDataEntryAccount As String)
        _udtAuditLogEntry = udtAuditLogEntry
        _strAuditLogDesc = strAuditLogDesc
        _strAuditLogUserID = Nothing
        _strAuditLogSPID = strAuditLogSPID
        _strAuditLogDataEntryAccount = strAuditLogDataEntryAccount
        _strLogID = strLogID
        _objAuditLogInfo = Nothing
        BuildMessageBox(strHeaderKey)
    End Sub

    Public Sub BuildMessageDescBox(ByVal strHeaderKey As String, ByRef udtAuditLogEntry As AuditLogEntry, ByVal strLogID As String, ByVal strAuditLogDesc As String)
        _udtAuditLogEntry = udtAuditLogEntry
        _strAuditLogDesc = strAuditLogDesc
        _strAuditLogUserID = Nothing
        _strAuditLogSPID = Nothing
        _strAuditLogDataEntryAccount = Nothing
        _strLogID = strLogID
        _objAuditLogInfo = Nothing

        ViewState(VS_MESSAGEDESCBOX) = True
        If Me.GetCodeTable.Rows.Count > 0 Then
            DisplayMessageDescBox(strHeaderKey)
            If ScrollToHeight Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "MessageBoxScript", "setTimeout('window.scrollBy(0, -(document.body.clientHeight));', 1); ", True)
            End If
        Else
            Me.Visible = False
        End If
    End Sub

#End Region
    'Public Sub BuildMsgBoxWithLog(ByVal strFunctionCode As String, ByVal strHeaderKey As String)
    '    strFuncCode = strFunctionCode
    '    BuildMessageBox(strHeaderKey)
    'End Sub

    ''' <summary>
    ''' Template for the DataListTemplate of DataList in MessageBox
    ''' </summary>
    ''' <remarks></remarks>
    Private Class DataListTemplate
        Implements ITemplate

        Private templateType As DataControlRowType
        Private columnName As String

        ''' <summary>
        ''' New
        ''' </summary>
        ''' <param name="type">Data control row type</param>
        ''' <param name="colname">Name of column</param>
        ''' <remarks></remarks>
        Sub New(ByVal type As DataControlRowType, ByVal colname As String)
            templateType = type
            columnName = colname
        End Sub

        ''' <summary>
        ''' InstantiateIn
        ''' </summary>
        ''' <param name="container"></param>
        ''' <remarks></remarks>
        Sub InstantiateIn(ByVal container As System.Web.UI.Control) _
              Implements ITemplate.InstantiateIn

            Select Case templateType

                Case DataControlRowType.Header
                    'Create the controls to put in the header section and set their properties.
                    Dim lc As New Literal
                    lc.Text = "<b>" & columnName & "</b>"

                    'Add the controls to the Controls collection of the container.
                    container.Controls.Add(lc)

                Case DataControlRowType.DataRow
                    'Create the controls to put in a data row section and set their properties.
                    Dim lcRowStart As New Literal
                    Dim lblMsgDesc As New Label
                    Dim lblMsgCode As New Label
                    Dim lcRowEnd As New Literal
                    Dim spacer As Literal = New Literal

                    lcRowStart.Text = "<span style='font-weight:normal;font-style:normal;text-decoration:none;font-size: 12pt; font-family: Arial'>"
                    lblMsgDesc.ForeColor = Drawing.Color.Red
                    lblMsgCode.ForeColor = Drawing.Color.Red
                    lcRowEnd.Text = "</span>"

                    spacer.Text = " "

                    'To support data binding, register the event-handling methods to perform the data binding. 
                    'Each control needs its own event handler.
                    AddHandler lblMsgDesc.DataBinding, AddressOf lblMsgDesc_DataBinding
                    AddHandler lblMsgCode.DataBinding, AddressOf lblMsgCode_DataBinding

                    'Add the controls to the Controls collection of the container.
                    container.Controls.Add(lcRowStart)
                    container.Controls.Add(lblMsgDesc)
                    container.Controls.Add(spacer)
                    container.Controls.Add(lblMsgCode)
                    container.Controls.Add(lcRowEnd)

                    'Insert cases to create the content for the other row types, if desired.

                Case Else
                    'Insert code to handle unexpected values. 

            End Select

        End Sub

        ''' <summary>
        ''' DataBinding function of lblMsgDesc
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>"msgDesc" set as the data bind field</remarks>
        Private Sub lblMsgDesc_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim l As Label = CType(sender, Label)
            Dim row As DataListItem = CType(l.NamingContainer, DataListItem)
            l.Text = DataBinder.Eval(row.DataItem, "msgDesc").ToString()
            'l.Text = l.Text.Replace("<", "&lt;")
            'l.Text = l.Text.Replace(">", "&gt;")

        End Sub

        ''' <summary>
        ''' DataBinding function of lblMsgCode
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>"msgCode" set as the data bind field</remarks>
        Private Sub lblMsgCode_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim l As Label = CType(sender, Label)
            Dim row As DataListItem = CType(l.NamingContainer, DataListItem)
            l.Text = DataBinder.Eval(row.DataItem, "msgCode").ToString()
        End Sub

    End Class

End Class