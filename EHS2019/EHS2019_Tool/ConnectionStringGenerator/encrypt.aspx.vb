Imports EncryptDecrypt.FE_SymmetricNamespace
Imports System.Text
Imports System.IO


Partial Class encrypt
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Result As System.Web.UI.WebControls.Label
    Protected WithEvents Encrypt As System.Web.UI.HtmlControls.HtmlInputButton


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim gKey, gstrSource As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim a As FE_SymmetricNamespace.FE_Symmetric
    End Sub


    Private Sub btnEncrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEncrypt.Click
        Try

            gKey = txtKey.Text
            Dim dbSrc As String = txtSvrName.Text
            Dim dbID As String = txtUID.Text
            Dim dbPwd As String = txtPWD.Text
            Dim dbCat As String = txtDBName.Text
            Dim dbConnSize As String = txtConnSize.Text
            Dim dbStr As String = "data source=" & dbSrc & "; initial catalog=" & dbCat & "; persist security info=False; user id=" & dbID & "; password=" & dbPwd & "; packet size=4096; max pool size=" & dbConnSize
            gstrSource = dbStr

            If Not ChkInput(dbID) Then
                AlertMessage("請輸入用戶名稱")
                Return
            End If
            If Not ChkInput(dbPwd) Then
                AlertMessage("請輸入用戶密碼!")
                Return
            End If
            If Not ChkInput(dbSrc) Then
                AlertMessage("請輸入數據庫伺服器!")
                Return
            End If
            If Not ChkInput(dbCat) Then
                AlertMessage("請輸入數據庫名稱!")
                Return
            End If
            If Not ChkInput(dbConnSize) Then
                AlertMessage("數據庫共用連接數!")
                Return
            End If
            If Not ChkInput(gKey) Then
                AlertMessage("請輸入加密鑰匙!")
                Return
            End If

            Dim feService As New FE_SymmetricNamespace.FE_Symmetric
            Dim strTmp As String

            strTmp = feService.EncryptData(gKey, gstrSource)
            txtResult.Text = strTmp
            'OutputFile("c:\key.ctl", ScrambleKey(gKey))
            Me.txtEncryptKey.Text = ScrambleKey(gKey)
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try



    End Sub

    Private Function ScrambleKey(ByVal iKey As String) As String
        Dim keyToEncrypt() As Byte = Encoding.ASCII.GetBytes(iKey)
        ScrambleKey = Convert.ToBase64String(keyToEncrypt)
    End Function
    Private Sub OutputFile(ByVal oFile As String, ByVal iStr As String)
        Dim objStreamWriter As StreamWriter
        'Pass the file path and the file name to the StreamWriter constructor.
        objStreamWriter = New StreamWriter(oFile)

        'Write a line of text.
        objStreamWriter.WriteLine(iStr)

        'Close the file.
        objStreamWriter.Close()

    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Me.txtDBName.Text = ""
        Me.txtKey.Text = ""
        Me.txtPWD.Text = ""
        Me.txtSvrName.Text = ""
        Me.txtUID.Text = ""
        Me.txtResult.Text = ""
    End Sub

    Private Sub AlertMessage(ByVal msgStr As String)
        Page.RegisterStartupScript("AlertMessage", "<script>function AlertMessage() { alert(""" + msgStr.Replace("""", "\""") + """); }  setTimeout('AlertMessage()',1);</script>")
    End Sub

    Private Function ChkInput(ByVal inputStr As String)
        If Trim(inputStr) = "" Then
            Return False
        Else
            Return True
        End If
    End Function


End Class
