Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.ComObject
Imports Common.Format

Public Enum InfoMessageBoxType
    Information = 1
    Complete = 2
End Enum

<DefaultProperty("Text"), ToolboxData("<{0}:InfoMessageBox runat=server></{0}:InfoMessageBox>")> _
Public Class InfoMessageBox
    Inherits WebControl

    Private lblMsgDesc As Label
    Private _type As InfoMessageBoxType

    Private Const VS_INFO_MESSAGEBOX_CODETABLE As String = "VS_INFO_MESSAGEBOX_CODETABLE"
    'Private Const VS_INFO_MESSAGEBOX_CODE As String = "VS_INFO_MESSAGEBOX_CODE"
    Private Const VS_INFO_MESSAGEBOX_TYPE As String = "VS_INFO_MESSAGEBOX_TYPE"

    Private Const VS_INFO_MESSAGEBOX_EXTRA_INFO As String = "VS_INFO_MESSAGEBOX_EXTRA_INFO"

    Private Const VS_INFO_MESSAGEBOX_SHOWED As String = "VS_INFO_MESSAGEBOX_SHOWED"
    Private Const VS_INFO_MESSAGEBOX_OLD_CHAR_LIST As String = "VS_INFO_MESSAGEBOX_OLD_CHAR_LIST"
    Private Const VS_INFO_MESSAGEBOX_NEW_CHAR_LIST As String = "VS_INFO_MESSAGEBOX_NEW_CHAR_LIST"

    Private tblInfoMessageBox As Table
    Private imgInfoMessageBox As Image

    Public Property Type() As InfoMessageBoxType
        Get
            Return _type
        End Get
        Set(ByVal value As InfoMessageBoxType)
            _type = value
            If value = InfoMessageBoxType.Complete Then
                ViewState(VS_INFO_MESSAGEBOX_TYPE) = 2
            Else
                ViewState(VS_INFO_MESSAGEBOX_TYPE) = 1
            End If
        End Set
    End Property

    Public Overrides Property Width() As System.Web.UI.WebControls.Unit
        Get
            Return MyBase.Width
        End Get
        Set(ByVal value As System.Web.UI.WebControls.Unit)
            MyBase.Width = value
        End Set
    End Property

    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        MyBase.RenderContents(writer)
    End Sub

    Private Sub InfoMessageBox_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim tb As Table
        Dim tr As TableRow
        Dim td As TableCell
        Dim img As Image
        Dim lbl As Label

        tb = New Table

        tb.CellPadding = 0
        tb.CellSpacing = 0
        If Type = InfoMessageBoxType.Complete Then
            tb.Attributes.Item("style") = "border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial; background-color: #f4ffff"
        Else
            tb.Attributes.Item("style") = "border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial; background-color: #d7f2ff"
        End If
        tb.Width = Unit.Percentage(100)

        tr = New TableRow
        td = New TableCell
        td.VerticalAlign = VerticalAlign.Top
        tblInfoMessageBox = tb

        Dim tb1 As Table
        Dim tr1 As TableRow
        Dim td1 As TableCell

        tb1 = New Table

        tb1.Style.Item("width") = "100%"
        tr1 = New TableRow
        td1 = New TableCell
        td1.Style.Item("width") = "40px"
        img = New Image
        img.ID = CType(sender, InfoMessageBox).UniqueID.Trim & "img_complete"
        If Type = InfoMessageBoxType.Complete Then
            img.ImageUrl = "~/Images/others/tick.png"
        Else
            img.ImageUrl = "~/Images/others/information.png"
        End If
        imgInfoMessageBox = img
        td1.Controls.Add(img)
        td1.VerticalAlign = VerticalAlign.Top
        'td1.VerticalAlign = VerticalAlign.Middle
        tr1.Cells.Add(td1)

        td1 = New TableCell
        td1.VerticalAlign = VerticalAlign.Middle
        td1.HorizontalAlign = WebControls.HorizontalAlign.Left
        lbl = New Label
        lbl.ID = CType(sender, InfoMessageBox).UniqueID.Trim & "lblMsgDesc"
        lblMsgDesc = lbl
        td1.Controls.Add(lbl)
        tr1.Cells.Add(td1)

        tb1.Rows.Add(tr1)
        td.Controls.Add(tb1)
        tr.Cells.Add(td)
        tb.Rows.Add(tr)

        Dim lc As New LiteralControl
        lc.Text = "<br>"

        Dim pnl As New Panel

        pnl.Controls.Add(tb)

        If Width = Unit.Empty Then
            pnl.Width = Unit.Percentage(100)
        Else
            pnl.Width = Width
        End If
        Controls.Add(pnl)
        Controls.Add(lc)

        Me.Visible = False

    End Sub

    Private Sub InfoMessageBox_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'BuildMsg()
        DisplayMessageBox()
    End Sub

    ''' <summary>
    ''' Init code of message datatable
    ''' </summary>
    ''' <remarks>
    ''' To build the structure of code of message datatable
    ''' </remarks>
    Private Function InitializeCodeTable() As DataTable
        Dim dtCode As New DataTable

        dtCode.Columns.Add(New DataColumn("msgCode", GetType(String)))
        dtCode.Columns.Add(New DataColumn("msgExtraInfo", GetType(String)))
        dtCode.Columns.Add(New DataColumn("oldCharList", GetType(String())))
        dtCode.Columns.Add(New DataColumn("newCharList", GetType(String())))

        Return dtCode

    End Function

    ''' <summary>
    ''' Return code of message datatable from ViewState
    ''' </summary>
    ''' <returns>DataTable of code of message</returns>
    ''' <remarks></remarks>
    Public Function GetCodeTable() As DataTable
        Dim dtCode As DataTable
        If Not ViewState(VS_INFO_MESSAGEBOX_SHOWED) Is Nothing Then
            If Not CBool(ViewState(VS_INFO_MESSAGEBOX_SHOWED)) Then
                dtCode = CType(ViewState(VS_INFO_MESSAGEBOX_CODETABLE), DataTable)
                Return dtCode
            End If
        End If
        dtCode = Me.InitializeCodeTable
        Return dtCode
    End Function

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage)
        AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode)
    End Sub

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage, ByRef strOldCharList() As String, ByRef strNewCharList() As String)
        AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, strOldCharList, strNewCharList)
    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String)
        'AddMessageWithExtraInfo(FunctionCode, SeverityCode, StrMsgCode, String.Empty)
        AddMessage(FunctionCode, SeverityCode, StrMsgCode, String.Empty, String.Empty)
    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByRef strOldChar As String, ByRef strNewChar As String)

        Dim strOldCharList(0) As String
        Dim strNewCharList(0) As String

        strOldCharList(0) = strOldChar
        strNewCharList(0) = strNewChar

        AddMessage(FunctionCode, SeverityCode, StrMsgCode, strOldCharList, strNewCharList)

    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByRef strOldCharList() As String, ByRef strNewCharList() As String)
        Dim dtCode As DataTable = Nothing

        If ViewState(VS_INFO_MESSAGEBOX_CODETABLE) Is Nothing OrElse Me.Visible OrElse (Not ViewState(VS_INFO_MESSAGEBOX_SHOWED) Is Nothing AndAlso CBool(ViewState(VS_INFO_MESSAGEBOX_SHOWED))) Then
            dtCode = InitializeCodeTable()
            Me.Visible = False
        Else
            dtCode = CType(ViewState(VS_INFO_MESSAGEBOX_CODETABLE), DataTable)
        End If

        AddCodeTableRow(dtCode, FunctionCode, SeverityCode, StrMsgCode, strOldCharList, strNewCharList, String.Empty)

        ViewState(VS_INFO_MESSAGEBOX_SHOWED) = False
        ViewState(VS_INFO_MESSAGEBOX_CODETABLE) = dtCode

    End Sub

    ''' <summary>
    ''' Add message with extra information
    ''' </summary>
    ''' <param name="strFunctionCode"></param>
    ''' <param name="strSeverityCode"></param>
    ''' <param name="strMsgCode"></param>
    ''' <param name="strExtraInfo">Extra information will show under the message (Support HTML format)</param>
    ''' <remarks></remarks>
    Public Sub AddMessageWithExtraInfo(ByVal strFunctionCode As String, ByVal strSeverityCode As String, ByVal strMsgCode As String, ByVal strExtraInfo As String)
        Dim strOldCharList() As String = {""}
        Dim strNewCharList() As String = {""}

        Dim dtCode As DataTable = Nothing

        If ViewState(VS_INFO_MESSAGEBOX_CODETABLE) Is Nothing OrElse Me.Visible OrElse (Not ViewState(VS_INFO_MESSAGEBOX_SHOWED) Is Nothing AndAlso CBool(ViewState(VS_INFO_MESSAGEBOX_SHOWED))) Then
            dtCode = InitializeCodeTable()
            Me.Visible = False
        Else
            dtCode = CType(ViewState(VS_INFO_MESSAGEBOX_CODETABLE), DataTable)
        End If

        AddCodeTableRow(dtCode, strFunctionCode, strSeverityCode, strMsgCode, strOldCharList, strNewCharList, strExtraInfo)

        ViewState(VS_INFO_MESSAGEBOX_SHOWED) = False
        ViewState(VS_INFO_MESSAGEBOX_CODETABLE) = dtCode

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
    Private Sub AddCodeTableRow(ByRef dtCode As DataTable, _
                                ByVal FunctionCode As String, _
                                ByVal SeverityCode As String, _
                                ByVal StrMsgCode As String, _
                                ByRef strOldCharList() As String, _
                                ByRef strNewCharList() As String, _
                                ByVal strExtraInfo As String)

        Dim drCode As DataRow = dtCode.NewRow

        drCode("msgCode") = FunctionCode & "-" & SeverityCode & "-" & StrMsgCode
        drCode("msgExtraInfo") = strExtraInfo
        drCode("oldCharList") = strOldCharList
        drCode("newCharList") = strNewCharList

        dtCode.Rows.Add(drCode)

    End Sub

    Private Sub DisplayMessageBox()
        Dim dtCode As DataTable

        ''Create message table
        'Dim dtMessage As New DataTable
        'dtMessage.Columns.Add(New DataColumn("msgDesc", GetType(String)))
        'dtMessage.Columns.Add(New DataColumn("msgCode", GetType(String)))
        'dtMessage.Columns.Add(New DataColumn("msgExtraInfo", GetType(String)))

        'Default to hide the message box
        Me.Visible = False

        If Not ViewState(VS_INFO_MESSAGEBOX_CODETABLE) Is Nothing Then
            'Update flag
            ViewState(VS_INFO_MESSAGEBOX_SHOWED) = True

            'Apply style of message box
            If Not ViewState(VS_INFO_MESSAGEBOX_TYPE) Is Nothing Then
                If CInt(ViewState(VS_INFO_MESSAGEBOX_TYPE)) = 2 Then
                    tblInfoMessageBox.Attributes.Item("style") = "font-family: Arial; border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial; background-color: #f4ffff"
                    imgInfoMessageBox.ImageUrl = "~/Images/others/tick.png"
                Else
                    tblInfoMessageBox.Attributes.Item("style") = "font-family: Arial; border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial; background-color: #d7f2ff"
                    imgInfoMessageBox.ImageUrl = "~/Images/others/information.png"
                End If
            End If

            'Load system message into temp data table
            dtCode = CType(ViewState(VS_INFO_MESSAGEBOX_CODETABLE), DataTable)

            If dtCode.Rows.Count > 0 Then
                'Set to display the message box
                Me.Visible = True

                For intCt As Integer = 0 To dtCode.Rows.Count - 1
                    Dim strCode As String = CStr(dtCode.Rows(intCt).Item("msgCode"))
                    Dim strMsgDesc As String = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", strCode))
                    Dim strOldCharList() As String
                    Dim objNewCharList() As Object

                    'Dim drMessage As DataRow = dtMessage.NewRow

                    'If has variable e.g. "%s", replace it
                    If dtCode.Rows(intCt).Item("oldCharList") IsNot DBNull.Value AndAlso CType(dtCode.Rows(intCt).Item("oldCharList"), String()).Length > 0 Then
                        strOldCharList = CType(dtCode.Rows(intCt).Item("oldCharList"), String())
                        objNewCharList = CType(dtCode.Rows(intCt).Item("newCharList"), Object())

                        For i As Integer = 0 To strOldCharList.Length - 1
                            Dim strOldChar As String = strOldCharList(i)
                            Dim objNewChar As Object = objNewCharList(i)
                            Dim strNewChar As String = String.Empty

                            If strOldChar Is Nothing OrElse strOldChar = String.Empty Then
                                Continue For
                            End If

                            If TypeOf objNewChar Is DateTime Then
                                Dim udtFormatter As New Formatter
                                Dim dtmNewChar As DateTime
                                dtmNewChar = CType(objNewChar, DateTime)
                                strNewChar = udtFormatter.convertDate(dtmNewChar)

                            ElseIf TypeOf objNewChar Is String Then
                                strNewChar = CType(objNewChar, String)

                            ElseIf TypeOf objNewChar Is SystemMessage Then
                                Dim udtSystemMessage As SystemMessage
                                udtSystemMessage = CType(objNewChar, SystemMessage)
                                strNewChar = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", udtSystemMessage.ConvertToResourceCode()))
                            End If

                            strMsgDesc = strMsgDesc.Replace(strOldChar, strNewChar)

                        Next

                    End If

                    'Put system message into message box
                    If intCt = 0 Then
                        Me.lblMsgDesc.Text = strMsgDesc
                    Else
                        Me.lblMsgDesc.Text += "<div style='padding-top:8px'>" + strMsgDesc + "</div>"
                    End If

                    'if has extra info, append it 
                    If Not String.IsNullOrEmpty(dtCode.Rows(intCt).Item("msgExtraInfo").ToString) Then
                        Me.lblMsgDesc.Text += "<div style='paddint-top:0px'>" + dtCode.Rows(intCt).Item("msgExtraInfo").ToString + "</div>"
                    End If

                Next

            End If

            'code = CStr(ViewState(VS_INFO_MESSAGEBOX_CODE))

            'strExtraInfo = CStr(ViewState(VS_INFO_MESSAGEBOX_EXTRA_INFO))

            'If Not ViewState(VS_INFO_MESSAGEBOX_TYPE) Is Nothing Then
            '    If CInt(ViewState(VS_INFO_MESSAGEBOX_TYPE)) = 2 Then
            '        tblInfoMessageBox.Attributes.Item("style") = "font-family: Arial; border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial; background-color: #f4ffff"
            '        imgInfoMessageBox.ImageUrl = "~/Images/others/tick.png"
            '    Else
            '        tblInfoMessageBox.Attributes.Item("style") = "font-family: Arial; border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial; background-color: #d7f2ff"
            '        imgInfoMessageBox.ImageUrl = "~/Images/others/information.png"
            '    End If
            'End If

            'If code <> "" Then
            '    Me.Visible = True

            '    Dim strMsgDesc As String
            '    strMsgDesc = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))

            '    If Not ViewState(VS_INFO_MESSAGEBOX_OLD_CHAR_LIST) Is Nothing Then
            '        Dim strOldCharList() As String
            '        Dim objNewCharList() As Object

            '        strOldCharList = CType(ViewState(VS_INFO_MESSAGEBOX_OLD_CHAR_LIST), String())
            '        objNewCharList = CType(ViewState(VS_INFO_MESSAGEBOX_NEW_CHAR_LIST), Object())

            '        Dim i As Integer
            '        Dim strOldChar As String
            '        Dim objNewChar As Object
            '        For i = 0 To strOldCharList.Length - 1
            '            strOldChar = strOldCharList(i)
            '            If Not strOldChar Is Nothing Then
            '                Dim strNewChar As String = ""
            '                objNewChar = objNewCharList(i)
            '                If TypeOf objNewChar Is DateTime Then
            '                    Dim udtFormatter As New Formatter
            '                    Dim dtmNewChar As DateTime
            '                    dtmNewChar = CType(objNewChar, DateTime)
            '                    strNewChar = udtFormatter.convertDate(dtmNewChar)
            '                ElseIf TypeOf objNewChar Is String Then
            '                    strNewChar = CType(objNewChar, String)
            '                ElseIf TypeOf objNewChar Is SystemMessage Then
            '                    Dim udtSystemMessage As SystemMessage
            '                    udtSystemMessage = CType(objNewChar, SystemMessage)
            '                    strNewChar = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", udtSystemMessage.ConvertToResourceCode()))
            '                End If
            '                strMsgDesc = strMsgDesc.Replace(strOldChar, strNewChar)
            '            End If
            '        Next

            '    End If

            '    Me.lblMsgDesc.Text = strMsgDesc

            '    ' [CRE12-004] Statistic Enquiry [Start][Koala]
            '    ' Show extra information under information message
            '    If Not String.IsNullOrEmpty(strExtraInfo) Then
            '        Me.lblMsgDesc.Text += "<br/>" + strExtraInfo
            '    End If
            '    ' [CRE12-004] Statistic Enquiry [End][Koala]
            'End If

        End If

    End Sub

    ''' <summary>
    ''' Build the Message Box and focus to it
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BuildMessageBox()
        If Not ViewState(VS_INFO_MESSAGEBOX_SHOWED) Is Nothing Then
            If Not CBool(ViewState(VS_INFO_MESSAGEBOX_SHOWED)) Then
                DisplayMessageBox()
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "InfoMessageBoxScript", "setTimeout('window.scrollBy(0, -(document.body.clientHeight));', 1); ", True)
                Exit Sub
            End If
        End If
        Me.Visible = False
    End Sub

    ''' <summary>
    ''' Build the Message Box without focusing to it
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BuildMessageBoxWithoutFocus()
        If Not ViewState(VS_INFO_MESSAGEBOX_SHOWED) Is Nothing Then
            If Not CBool(ViewState(VS_INFO_MESSAGEBOX_SHOWED)) Then
                DisplayMessageBox()
                Exit Sub
            End If
        End If
        Me.Visible = False
    End Sub

    Public Sub Clear()
        ViewState(VS_INFO_MESSAGEBOX_CODETABLE) = Nothing
        'ViewState(VS_INFO_MESSAGEBOX_CODE) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_TYPE) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_EXTRA_INFO) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_SHOWED) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_OLD_CHAR_LIST) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_NEW_CHAR_LIST) = Nothing
        Me.Visible = False
    End Sub

End Class
