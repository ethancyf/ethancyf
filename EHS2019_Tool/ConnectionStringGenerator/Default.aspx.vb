Imports ConnectionStringGenerator.FE_SymmetricNamespace

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub btnDDecrypt_Click(sender As Object, e As System.EventArgs)
        txtEUID.Text = String.Empty
        txtEPassword.Text = String.Empty
        txtESvrName.Text = String.Empty
        txtEDBName.Text = String.Empty
        txtEConnSize.Text = String.Empty
        txtEKey.Text = String.Empty
        txtEResult.Text = String.Empty
        txtEEncryptKey.Text = String.Empty

        Try
            Dim strResult As String = (New FE_Symmetric).DecryptData(Encoding.ASCII.GetString(Convert.FromBase64String(txtDKey.Text)), txtDEncryptStr.Text, rbDMethod.SelectedValue)

            txtDResult.Text = strResult

            If chkDFillEncryptionPart.Checked Then
                txtEUID.Text = FindValue(strResult, "user id=")
                txtEPassword.Text = FindValue(strResult, "password=")
                txtESvrName.Text = FindValue(strResult, "data source=")
                txtEDBName.Text = FindValue(strResult, "initial catalog=")
                txtEConnSize.Text = FindValue(strResult, "max pool size=")
                txtEKey.Text = Encoding.ASCII.GetString(Convert.FromBase64String(txtDKey.Text))
            End If

        Catch ex As Exception
            txtDResult.Text = ex.Message

        End Try

    End Sub

    Protected Sub btnEEncrypt_Click(sender As Object, e As System.EventArgs)
        Try
            Dim strConnectionString As String = String.Format("data source={0}; initial catalog={1}; persist security info=False; user id={2}; password={3}; packet size=4096; max pool size={4}", _
                                                              txtESvrName.Text, txtEDBName.Text, txtEUID.Text, txtEPassword.Text, txtEConnSize.Text)

            Dim strResult As String = (New FE_Symmetric).EncryptData(txtEKey.Text, strConnectionString, rbEMethod.SelectedValue)
            txtEResult.Text = strResult

            ' Key
            txtEEncryptKey.Text = Convert.ToBase64String(Encoding.ASCII.GetBytes(txtEKey.Text))

        Catch ex As Exception
            txtEResult.Text = ex.Message

        End Try

    End Sub

    Private Function FindValue(ByVal strValue As String, ByVal strTarget As String) As String
        If strValue.Substring(strValue.IndexOf(strTarget)).Contains(";") Then
            Return strValue.Substring(strValue.IndexOf(strTarget) + strTarget.Length, strValue.IndexOf(";", strValue.IndexOf(strTarget)) - strValue.IndexOf(strTarget) - strTarget.Length)
        Else
            Return strValue.Substring(strValue.IndexOf(strTarget) + strTarget.Length)
        End If

    End Function

End Class
