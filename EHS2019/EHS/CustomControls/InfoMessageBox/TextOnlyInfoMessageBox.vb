Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.ComObject
Imports Common.Format


<DefaultProperty("Text"), ToolboxData("<{0}:TextOnlyInfoMessageBox runat=server></{0}:TextOnlyInfoMessageBox>")> _
Public Class TextOnlyInfoMessageBox
    Inherits WebControl

    Private lblMsgDesc As Label
    Private tblInfoMessageBox As Table
    Private _type As InfoMessageBoxType = InfoMessageBoxType.Information

    Private Const VS_TEXTONLYINFOMESSAGEBOX_CODETABLE As String = "VS_TEXTONLYINFOMESSAGEBOX_CODETABLE"
    'Private Const VS_TEXTONLYINFOMESSAGEBOX_CODE As String = "VS_TEXTONLYINFOMESSAGEBOX_CODE"
    Private Const VS_TEXTONLYINFOMESSAGEBOX_TYPE As String = "VS_TEXTONLYINFOMESSAGEBOX_TYPE"

    Private Const VS_TEXTONLYINFOMESSAGEBOX_SHOWED As String = "VS_TEXTONLYINFOMESSAGEBOX_SHOWED"
    Private Const VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST As String = "VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST"
    Private Const VS_TEXTONLYINFOMESSAGEBOX_NEW_CHAR_LIST As String = "VS_TEXTONLYINFOMESSAGEBOX_NEW_CHAR_LIST"

    Public Property Type() As InfoMessageBoxType
        Get
            Return _type
        End Get
        Set(ByVal value As InfoMessageBoxType)
            _type = value
            If value = InfoMessageBoxType.Complete Then
                ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) = 2
            Else
                ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) = 1
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
        Dim lbl As Label

        Dim pnl As New Panel

        tb = New Table

        tb.CellPadding = 0
        tb.CellSpacing = 0
        If _type = InfoMessageBoxType.Complete Then
            tb.Attributes.Item("style") = "border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial;"
        Else
            tb.Attributes.Item("style") = "border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial;"
        End If
        tb.Width = Unit.Percentage(100)
        tblInfoMessageBox = tb

        tr = New TableRow
        td = New TableCell
        td.VerticalAlign = VerticalAlign.Top

        Dim tb1 As Table
        Dim tr1 As TableRow
        Dim td1 As TableCell

        tb1 = New Table

        tb1.Style.Item("width") = "100%"
        tr1 = New TableRow

        td1 = New TableCell
        td1.VerticalAlign = VerticalAlign.Middle
        lbl = New Label
        lbl.ID = "lblMsgDesc"
        lblMsgDesc = lbl
        td1.Controls.Add(lbl)
        tr1.Cells.Add(td1)

        tb1.Rows.Add(tr1)
        td.Controls.Add(tb1)
        tr.Cells.Add(td)
        tb.Rows.Add(tr)

        'Dim lc As New LiteralControl
        'lc.Text = "<br>"

        'Dim lc1 As New LiteralControl
        'lc1.Text = "<br>"

        pnl.Controls.Add(tb)
        'Controls.Add(lc)
        Controls.Add(pnl)
        'Controls.Add(lc1)

        'Me.Width = Unit.Percentage(80)

        If Width = Unit.Empty Then
            pnl.Width = Unit.Percentage(100)
        Else
            pnl.Width = Width
        End If

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
        If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) Is Nothing Then
            If Not CBool(ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED)) Then
                dtCode = CType(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODETABLE), DataTable)
                Return dtCode
            End If
        End If
        dtCode = Me.InitializeCodeTable
        Return dtCode
    End Function

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage)
        ' Handle Replace Message
        Dim lstIdx As New List(Of String)
        Dim lstReplaceMessage As New List(Of String)

        objSystemMessage.GetReplaceMessage(String.Empty, lstIdx, lstReplaceMessage)

        If lstIdx Is Nothing Then
            AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode)
        Else
            AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, lstIdx.ToArray, lstReplaceMessage.ToArray)
        End If

    End Sub

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage, ByRef strOldCharList() As String, ByRef strNewCharList() As String)
        AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, strOldCharList, strNewCharList)
    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String)
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

        If ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODETABLE) Is Nothing OrElse Me.Visible OrElse _
            (Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) Is Nothing AndAlso CBool(ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED))) Then

            dtCode = InitializeCodeTable()
            Me.Visible = False
        Else
            dtCode = CType(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODETABLE), DataTable)
        End If

        AddCodeTableRow(dtCode, FunctionCode, SeverityCode, StrMsgCode, strOldCharList, strNewCharList)

        ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) = False
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODETABLE) = dtCode

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
                                ByRef strNewCharList() As String)

        Dim drCode As DataRow = dtCode.NewRow

        drCode("msgCode") = FunctionCode & "-" & SeverityCode & "-" & StrMsgCode
        drCode("oldCharList") = strOldCharList
        drCode("newCharList") = strNewCharList

        dtCode.Rows.Add(drCode)

    End Sub

    Private Sub DisplayMessageBox()
        Dim dtCode As DataTable

        'Default to hide the message box
        Me.Visible = False

        If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODETABLE) Is Nothing Then
            'Update flag
            ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) = True

            'Apply style of message box
            If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) Is Nothing Then
                If CInt(ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE)) = 2 Then
                    tblInfoMessageBox.Attributes.Item("style") = "border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial;"
                Else
                    tblInfoMessageBox.Attributes.Item("style") = "border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial;"
                End If
            End If

            'Load system message into temp data table
            dtCode = CType(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODETABLE), DataTable)

            If dtCode.Rows.Count > 0 Then
                'Set to display the message box
                Me.Visible = True

                For intCt As Integer = 0 To dtCode.Rows.Count - 1
                    Dim strCode As String = CStr(dtCode.Rows(intCt).Item("msgCode"))
                    Dim strMsgDesc As String = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", strCode))
                    Dim strOldCharList() As String
                    Dim objNewCharList() As Object

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

                Next

            End If

            'code = CStr(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE))

            'If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) Is Nothing Then
            '    If CInt(ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE)) = 2 Then
            '        tblInfoMessageBox.Attributes.Item("style") = "border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial;"
            '    Else
            '        tblInfoMessageBox.Attributes.Item("style") = "border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial;"
            '    End If
            'End If

            'If code <> "" Then
            '    Me.Visible = True

            '    Dim strMsgDesc As String
            '    strMsgDesc = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))
            '    If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST) Is Nothing Then
            '        Dim strOldCharList() As String
            '        Dim objNewCharList() As Object

            '        strOldCharList = CType(ViewState(VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST), String())
            '        objNewCharList = CType(ViewState(VS_TEXTONLYINFOMESSAGEBOX_NEW_CHAR_LIST), Object())

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
            '    'Me.lblMsgDesc.Text = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))
            'End If

        End If

    End Sub

    Public Sub BuildMessageBox()
        If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) Is Nothing Then
            If Not CBool(ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED)) Then
                DisplayMessageBox()
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "InfoMessageBoxScript", "setTimeout('window.scrollBy(0, -(document.body.clientHeight));', 1); ", True)
                Exit Sub
            End If
        End If
        Me.Visible = False
    End Sub


    Public Sub Clear()
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODETABLE) = Nothing
        'ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_NEW_CHAR_LIST) = Nothing
        Me.Visible = False
    End Sub

End Class