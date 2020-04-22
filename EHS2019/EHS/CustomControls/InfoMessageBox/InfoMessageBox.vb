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

    Private Const VS_INFO_MESSAGEBOX_CODE As String = "VS_INFO_MESSAGEBOX_CODE"
    Private Const VS_INFO_MESSAGEBOX_TYPE As String = "VS_INFO_MESSAGEBOX_TYPE"
    ' [CRE12-004] Statistic Enquiry [Start][Koala]
    Private Const VS_INFO_MESSAGEBOX_EXTRA_INFO As String = "VS_INFO_MESSAGEBOX_EXTRA_INFO"
    ' [CRE12-004] Statistic Enquiry [End][Koala]

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

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeveritycodeCode As String, ByVal StrMsgCode As String)
        AddMessageWithExtraInfo(FunctionCode, SeveritycodeCode, StrMsgCode, String.Empty)
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        'Dim code As String
        'If FunctionCode <> "" AndAlso SeveritycodeCode <> "" AndAlso StrMsgCode <> "" Then
        '    code = FunctionCode & "-" & SeveritycodeCode & "-" & StrMsgCode
        'Else
        '    If Not ViewState(VS_INFO_MESSAGEBOX_CODE) Is Nothing Then
        '        code = CStr(ViewState(VS_INFO_MESSAGEBOX_CODE))
        '    Else
        '        code = ""
        '    End If
        'End If
        'ViewState(VS_INFO_MESSAGEBOX_CODE) = code
        'ViewState(VS_INFO_MESSAGEBOX_SHOWED) = False
        ' [CRE12-004] Statistic Enquiry [End][Koala]
    End Sub

    ' [CRE12-004] Statistic Enquiry [Start][Koala]
    ''' <summary>
    ''' Add message with extra information
    ''' </summary>
    ''' <param name="FunctionCode"></param>
    ''' <param name="SeveritycodeCode"></param>
    ''' <param name="StrMsgCode"></param>
    ''' <param name="strExtraInfo">Extra information will show under the message (Support HTML format)</param>
    ''' <remarks></remarks>
    Public Sub AddMessageWithExtraInfo(ByVal FunctionCode As String, ByVal SeveritycodeCode As String, ByVal StrMsgCode As String, ByVal strExtraInfo As String)
        Dim code As String
        If FunctionCode <> "" AndAlso SeveritycodeCode <> "" AndAlso StrMsgCode <> "" Then
            code = FunctionCode & "-" & SeveritycodeCode & "-" & StrMsgCode
        Else
            If Not ViewState(VS_INFO_MESSAGEBOX_CODE) Is Nothing Then
                code = CStr(ViewState(VS_INFO_MESSAGEBOX_CODE))
            Else
                code = ""
            End If
        End If
        ViewState(VS_INFO_MESSAGEBOX_CODE) = code
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        ViewState(VS_INFO_MESSAGEBOX_EXTRA_INFO) = strExtraInfo
        ' [CRE12-004] Statistic Enquiry [End][Koala]
        ViewState(VS_INFO_MESSAGEBOX_SHOWED) = False
    End Sub
    ' [CRE12-004] Statistic Enquiry [End][Koala]


    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage)
        AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode)
    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByRef strOldChar As String, ByRef objNewChar As Object)

        Dim strOldCharList(0) As String
        Dim objNewCharList(0) As Object

        strOldCharList(0) = strOldChar
        objNewCharList(0) = objNewChar

        AddMessage(FunctionCode, SeverityCode, StrMsgCode, strOldCharList, objNewCharList)

    End Sub

    Public Sub AddMessage(ByVal FunctionCode As String, ByVal SeverityCode As String, ByVal StrMsgCode As String, ByRef strOldCharList() As String, ByRef objNewCharList() As Object)
        Dim code As String
        If FunctionCode <> "" AndAlso SeverityCode <> "" AndAlso StrMsgCode <> "" Then
            code = FunctionCode & "-" & SeverityCode & "-" & StrMsgCode
        Else
            If Not ViewState(VS_INFO_MESSAGEBOX_CODE) Is Nothing Then
                code = CStr(ViewState(VS_INFO_MESSAGEBOX_CODE))
            Else
                code = ""
            End If
        End If
        ViewState(VS_INFO_MESSAGEBOX_CODE) = code
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        ViewState(VS_INFO_MESSAGEBOX_EXTRA_INFO) = String.Empty
        ' [CRE12-004] Statistic Enquiry [End][Koala]
        ViewState(VS_INFO_MESSAGEBOX_SHOWED) = False

        ViewState(VS_INFO_MESSAGEBOX_OLD_CHAR_LIST) = strOldCharList
        ViewState(VS_INFO_MESSAGEBOX_NEW_CHAR_LIST) = objNewCharList

    End Sub

    Public Sub AddMessage(ByRef objSystemMessage As SystemMessage, ByRef strOldCharList() As String, ByRef objNewCharList() As Object)
        AddMessage(objSystemMessage.FunctionCode, objSystemMessage.SeverityCode, objSystemMessage.MessageCode, strOldCharList, objNewCharList)
    End Sub

    Private Sub DisplayMessageBox()
        Dim code As String
        Dim strExtraInfo As String
        Me.Visible = False
        If Not ViewState(VS_INFO_MESSAGEBOX_CODE) Is Nothing Then
            ViewState(VS_INFO_MESSAGEBOX_SHOWED) = True
            code = CStr(ViewState(VS_INFO_MESSAGEBOX_CODE))
            ' [CRE12-004] Statistic Enquiry [Start][Koala]
            strExtraInfo = CStr(ViewState(VS_INFO_MESSAGEBOX_EXTRA_INFO))
            ' [CRE12-004] Statistic Enquiry [End][Koala]

            If Not ViewState(VS_INFO_MESSAGEBOX_TYPE) Is Nothing Then
                If CInt(ViewState(VS_INFO_MESSAGEBOX_TYPE)) = 2 Then
                    tblInfoMessageBox.Attributes.Item("style") = "font-family: Arial; border-right: green 1px solid; padding-right: 2px; border-top: green 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: green 1px solid; color: green; padding-top: 2px; border-bottom: green 1px solid; font-family: Arial; background-color: #f4ffff"
                    imgInfoMessageBox.ImageUrl = "~/Images/others/tick.png"
                Else
                    tblInfoMessageBox.Attributes.Item("style") = "font-family: Arial; border-right: #4169e1 1px solid; padding-right: 2px; border-top: #4169e1 1px solid; padding-left: 2px; font-size: 18px; padding-bottom: 2px; border-left: #4169e1 1px solid; color: #0000ff; padding-top: 2px; border-bottom: #4169e1 1px solid; font-family: Arial; background-color: #d7f2ff"
                    imgInfoMessageBox.ImageUrl = "~/Images/others/information.png"
                End If
            End If

            If code <> "" Then
                Me.Visible = True

                Dim strMsgDesc As String
                strMsgDesc = CStr(HttpContext.GetGlobalResourceObject("SystemMessage", code))

                If Not ViewState(VS_INFO_MESSAGEBOX_OLD_CHAR_LIST) Is Nothing Then
                    Dim strOldCharList() As String
                    Dim objNewCharList() As Object

                    strOldCharList = CType(ViewState(VS_INFO_MESSAGEBOX_OLD_CHAR_LIST), String())
                    objNewCharList = CType(ViewState(VS_INFO_MESSAGEBOX_NEW_CHAR_LIST), Object())

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

                ' [CRE12-004] Statistic Enquiry [Start][Koala]
                ' Show extra information under information message
                If Not String.IsNullOrEmpty(strExtraInfo) Then
                    Me.lblMsgDesc.Text += "<br/>" + strExtraInfo
                End If
                ' [CRE12-004] Statistic Enquiry [End][Koala]
            End If
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
        ViewState(VS_INFO_MESSAGEBOX_CODE) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_TYPE) = Nothing
        ' [CRE12-004] Statistic Enquiry [Start][Koala]
        ViewState(VS_INFO_MESSAGEBOX_EXTRA_INFO) = Nothing
        ' [CRE12-004] Statistic Enquiry [End][Koala]
        ViewState(VS_INFO_MESSAGEBOX_SHOWED) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_OLD_CHAR_LIST) = Nothing
        ViewState(VS_INFO_MESSAGEBOX_NEW_CHAR_LIST) = Nothing
        Me.Visible = False
    End Sub

End Class
