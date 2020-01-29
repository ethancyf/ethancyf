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

    Private Const VS_TEXTONLYINFOMESSAGEBOX_CODE As String = "VS_TEXTONLYINFOMESSAGEBOX_CODE"
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

    'Public Sub BuildMsg(Optional ByVal FunctionCode As String = "", Optional ByVal SeveritycodeCode As String = "", Optional ByVal StrMsgCode As String = "")
    '    Dim code As String
    '    If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) Is Nothing Then
    '        code = CStr(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE))
    '    Else
    '        If FunctionCode <> "" AndAlso SeveritycodeCode <> "" AndAlso StrMsgCode <> "" Then
    '            code = FunctionCode & "-" & SeveritycodeCode & "-" & StrMsgCode
    '        Else
    '            code = ""
    '        End If
    '    End If
    '    ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) = code

    '    If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) Is Nothing Then
    '        If CInt(ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE)) = 2 Then
    '            tblInfoMessageBox.Attributes.Item("style") = "border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial;"
    '        Else
    '            tblInfoMessageBox.Attributes.Item("style") = "border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial;"
    '        End If
    '    End If

    '    If code <> "" Then
    '        Me.Visible = True
    '        Me.lblMsgDesc.Text = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))
    '    End If
    '    If FunctionCode <> "" Then
    '        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "TextOnlyInfoMessageBoxScript", "setTimeout('window.scrollBy(0, -(document.body.clientHeight));', 1); ", True)
    '    End If

    'End Sub

    'Public Sub BuildMsg(ByRef objSystemMessage As SystemMessage)
    '    BuildMsg(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode)
    'End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeveritycodeCode As String, ByVal StrMsgCode As String)
        Dim code As String
        If FunctionCode <> "" AndAlso SeveritycodeCode <> "" AndAlso StrMsgCode <> "" Then
            code = FunctionCode & "-" & SeveritycodeCode & "-" & StrMsgCode
        Else
            If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) Is Nothing Then
                code = CStr(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE))
            Else
                code = ""
            End If
        End If
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) = code
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) = False
    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeveritycodeCode As String, ByVal StrMsgCode As String, ByRef strOldCharList() As String, ByRef objNewCharList() As Object)
        Dim code As String
        If FunctionCode <> "" AndAlso SeveritycodeCode <> "" AndAlso StrMsgCode <> "" Then
            code = FunctionCode & "-" & SeveritycodeCode & "-" & StrMsgCode
        Else
            If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) Is Nothing Then
                code = CStr(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE))
            Else
                code = ""
            End If
        End If
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) = code
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) = False

        ViewState(VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST) = strOldCharList
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_NEW_CHAR_LIST) = objNewCharList

    End Sub

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage)
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
    End Sub

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage, ByRef strOldCharList() As String, ByRef objNewCharList() As Object)
        AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, strOldCharList, objNewCharList)
    End Sub

    Private Sub DisplayMessageBox()
        Dim code As String
        Me.Visible = False

        If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) Is Nothing Then
            ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) = True
            code = CStr(ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE))


            If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) Is Nothing Then
                If CInt(ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE)) = 2 Then
                    tblInfoMessageBox.Attributes.Item("style") = "border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial;"
                Else
                    tblInfoMessageBox.Attributes.Item("style") = "border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial;"
                End If
            End If

            If code <> "" Then
                Me.Visible = True

                Dim strMsgDesc As String
                strMsgDesc = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))
                If Not ViewState(VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST) Is Nothing Then
                    Dim strOldCharList() As String
                    Dim objNewCharList() As Object

                    strOldCharList = CType(ViewState(VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST), String())
                    objNewCharList = CType(ViewState(VS_TEXTONLYINFOMESSAGEBOX_NEW_CHAR_LIST), Object())

                    Dim i As Integer
                    Dim strOldChar As String
                    Dim objNewChar As Object
                    For i = 0 To strOldCharList.Length - 1
                        strOldChar = strOldCharList(i)
                        If Not strOldChar Is Nothing Then
                            Dim strNewChar As String = ""
                            objNewChar = objNewCharList(i)
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
                        End If
                    Next

                End If
                Me.lblMsgDesc.Text = strMsgDesc
                'Me.lblMsgDesc.Text = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))
            End If

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
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_CODE) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_TYPE) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_SHOWED) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_OLD_CHAR_LIST) = Nothing
        ViewState(VS_TEXTONLYINFOMESSAGEBOX_NEW_CHAR_LIST) = Nothing
        Me.Visible = False
    End Sub

End Class