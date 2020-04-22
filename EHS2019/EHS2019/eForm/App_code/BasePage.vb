Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports CustomControls
Imports Common.ComObject

Public MustInherit Class BasePage
    Inherits Common.ComObject.MasterPage
    '<summary>
    'Default constructor
    '</summary>
    Public Sub BasePage()
    End Sub

    'Public Const SelectTradChinese As String = "ctl00$btn_langTradChi"
    'Public Const SelectSimpChinese As String = "ctl00$btn_langSimpChi"
    'Public Const SelectEnglish As String = "ctl00$btn_langEng"

    Public Const SelectTradChinese As String = "ctl00$lnkbtnTradChinese"
    Public Const SelectSimpChinese As String = "ctl00$lnkbtnSimpChinese"
    Public Const SelectEnglish As String = "ctl00$lnkbtnEnglish"

    Public Const _SelectTradChinese As String = "lnkbtnTradChinese"
    Public Const _SelectSimpChinese As String = "lnkbtnSimpChinese"
    Public Const _SelectEnglish As String = "lnkbtnEnglish"

    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"

    Public Const PostBackEventTarget As String = "__EVENTTARGET"

    Protected WithEvents _ScriptManager As ScriptManager

    ' CRP12-001 Removing redundant database call [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private _blnLanguageChanged As Boolean = False
    Public ReadOnly Property LanguageChanged()
        Get
            Return _blnLanguageChanged
        End Get
    End Property
    ' CRP12-001 Removing redundant database call [End][Koala]

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strlink As String = String.Empty

        ' Setting the secure flag in the ASP.NET Session id cookie
        Request.Cookies("ASP.NET_SessionId").Secure = True

        If Not HttpContext.Current.Request.CurrentExecutionFilePath.Equals(HttpContext.Current.Request.Path) Then
            If HttpContext.Current.Session("language") Is Nothing Then
                strlink = "~/en/invalidlink.aspx"
            Else
                If HttpContext.Current.Session("language") = "zh-tw" Then
                    strlink = "~/zh/invalidlink.aspx"
                Else
                    strlink = "~/en/invalidlink.aspx"
                End If
            End If
            Response.Redirect(strlink)
        End If

        _ScriptManager = ScriptManager.GetCurrent(Page)
        If _ScriptManager Is Nothing Then
            Throw New Exception("Page must has a ScriptManager")
        End If

        Dim strEndRequestHandlerScript As New StringBuilder
        strEndRequestHandlerScript.Append("<Script language='JavaScript'>")
        strEndRequestHandlerScript.Append("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);")

        strEndRequestHandlerScript.Append("function EndRequestHandler(sender, args) {")
        strEndRequestHandlerScript.Append("if (args.get_error() != undefined){")
        strEndRequestHandlerScript.Append("args.set_errorHandled(true);")
        strEndRequestHandlerScript.Append("alert('Server busy. Please try again later.\r\n系統繁忙，請稍後再嘗試。');}")
        strEndRequestHandlerScript.Append("}")
        strEndRequestHandlerScript.Append("</script>")

        'Page.ClientScript.RegisterStartupScript(Page.GetType, "ErrorHandler_Script", "<script>Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);</script>")
        Page.ClientScript.RegisterStartupScript(Page.GetType, "ErrorHandler_Script", strEndRequestHandlerScript.ToString)

    End Sub

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty
        Dim strlang As String = String.Empty

        ' CRP12-001 Removing redundant database call [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Dim strPrevLang As String = Session("language")
        ' CRP12-001 Removing redundant database call [End][Koala]

        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectSimpChinese) AndAlso Not controlID.Equals(_SelectEnglish) Then
                If controlID.Equals(SelectTradChinese) Then
                    selectedValue = TradChinese
                    Session("language") = selectedValue
                ElseIf controlID.Equals(SelectSimpChinese) Then
                    selectedValue = SimpChinese
                    Session("language") = selectedValue
                ElseIf controlID.Equals(SelectEnglish) Then
                    selectedValue = English
                    Session("language") = selectedValue
                End If
            End If
        Else
            ' If Session("language") Is Nothing Or Session("language") = String.Empty Then
            strlang = HttpContext.Current.Request.QueryString("lang")
            If Not strlang Is Nothing Then
                If strlang.Equals("en") Then
                    Session("language") = English
                ElseIf strlang.Equals("zh") Then
                    Session("language") = TradChinese
                Else
                    Session("language") = English
                End If
            End If
            'End If
        End If

        selectedValue = Session("language")
        Select Case selectedValue
            Case English
                SetCulture("en-US", "en-US")
            Case TradChinese
                SetCulture("zh-TW", "zh-TW")
            Case SimpChinese
                SetCulture("zh-CN", "zh-CN")
            Case Else
                SetCulture("en-US", "en-US")
                Session("language") = English
        End Select

        ' CRP12-001 Removing redundant database call [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        If strPrevLang <> Session("language") Then
            _blnLanguageChanged = True
        End If
        ' CRP12-001 Removing redundant database call [End][Koala]

        'Session("language") = 0
        MyBase.InitializeCulture()
    End Sub

    Protected Sub SetCulture(ByVal name As String, ByVal locale As String)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(name)
        Thread.CurrentThread.CurrentCulture = New CultureInfo(locale)
    End Sub

    Public Function IsFocusMessageBox()
        'Dim dtCode As DataTable
        'If Not ViewState(MessageBox.VS_MESSAGEBOX_SHOWED) Is Nothing Then
        '    If Not CBool(ViewState(MessageBox.VS_MESSAGEBOX_SHOWED)) Then
        '        dtCode = CType(ViewState(MessageBox.VS_MESSAGEBOX_CODETABLE), DataTable)
        '        'Return dtCode
        '        If dtCode.Rows.Count > 0 Then
        '            Return True
        '        End If
        '    End If
        'End If
        Return False
    End Function

    Public Property FunctionCode() As String
        Get
            'Return CType(Me.Master.FindControl("hfFunctionCode"), HiddenField).Value
            Return ViewState("FunctionCode")
        End Get
        Set(ByVal value As String)
            'CType(Me.Master.FindControl("hfFunctionCode"), HiddenField).Value = value
            ViewState("FunctionCode") = value
        End Set
    End Property

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles _ScriptManager.AsyncPostBackError
        ErrorHandler.HandleScriptManagerAsyncPostBackError(e, FunctionCode)
        'If Not (e.Exception.Data("GUID") Is Nothing) Then
        '    _ScriptManager.AsyncPostBackErrorMessage = e.Exception.Message & _
        '        " When reporting this error use the following ID: " & _
        '        e.Exception.Data("GUID").ToString()
        'Else
        '    _ScriptManager.AsyncPostBackErrorMessage = _
        '        "The server could not process the request."
        'End If

    End Sub

    Protected Sub ErrorProcessClick_Handler(ByVal sender As Object, ByVal e As EventArgs)
        'This handler demonstrates an error condition. In this example
        ' the server error gets intercepted on the client and an alert is shown. 
        Dim exc As New ArgumentException()
        exc.Data("GUID") = Guid.NewGuid().ToString()
        Throw exc

    End Sub

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Get eForm demo data from dataset xml
    ''' </summary>
    ''' <returns>Return data if dataset xml exist; Otherwise return nothing</returns>
    ''' <remarks></remarks>
    Public Function GetDemoData() As DataSet
        Dim strPath As String = System.Configuration.ConfigurationManager.AppSettings("DemoDataFilePath")
        If String.IsNullOrEmpty(strPath) Then Return Nothing

        Dim xmlDoc As New System.Xml.XmlDocument
        Dim sr As IO.StreamReader
        Try
            sr = New IO.StreamReader(strPath)
            xmlDoc.Load(New IO.StreamReader(strPath))
            Dim ds As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlDoc.InnerXml)
            sr.Close()
            Return ds
        Catch ex As Exception
            If sr IsNot Nothing Then
                Try
                    sr.Close()
                Catch ex2 As Exception
                    ' Do Nothing
                End Try
            End If
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Check eForm is running in demo mode by SystemParameter "TurnOnDemo_eForm"
    ''' </summary>
    ''' <returns>Return true if demo mode; otherwise false</returns>
    ''' <remarks></remarks>
    Public Function IsDemo() As Boolean
        Dim udtGen As New Common.ComFunction.GeneralFunction
        Dim strValue As String = String.Empty
        If udtGen.getSytemParameterByParameterName("TurnOnDemo_eForm", strValue, String.Empty) Then
            If strValue.ToUpper.Equals("Y") Then
                Return True
            End If
        End If
        Return False
    End Function
    ' CRE12-001 eHS and PCD integration [End][Koala]
End Class

