Imports System.Text
Imports System
Imports System.IO
Imports System.Collections


Partial Class decrypt
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Result As System.Web.UI.WebControls.Label
    Protected WithEvents Encrypt As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtUID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPWD As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSvrName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDBName As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnEncrypt As System.Web.UI.WebControls.Button


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim gKey, gstrSource As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub


    Private Sub btnDecrypt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDecrypt.Click

        Dim gstrSource As String = Me.txtEncryptStr.Text

        Dim feService As New FE_SymmetricNamespace.FE_Symmetric
        Dim strTmp As String

        gKey = Me.txtKey.Text
        gKey = GetOriginalKey()
        If gKey = "" Or gstrSource = "" Then
            txtResult.Text = "加密字串/加密鑰匙 不能空白"
            Exit Sub
        End If
        strTmp = feService.DecryptData(gKey, gstrSource)
        txtResult.Text = strTmp

    End Sub
    Private Function GetFileData() As String
        Dim objReader As New StreamReader("c:\key.ctl")
        Dim sLine As String = ""
        sLine = objReader.ReadToEnd()
        objReader.Close()
        GetFileData = sLine
    End Function
    Private Function GetOriginalKey() As String
        '        Dim sKey As String = GetFileData()
        Dim keyToDecrypt() As Byte = Convert.FromBase64String(gKey)
        GetOriginalKey = Encoding.ASCII.GetString(keyToDecrypt)
    End Function

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Me.txtEncryptStr.Text = ""
        Me.txtResult.Text = ""
    End Sub
End Class

