Imports System.Security.Cryptography
Imports System.IO

Public Class SymmetricMethod
    Private mobjCryptoService As SymmetricAlgorithm
    Private Key As String

    Public Sub New()
        mobjCryptoService = New RijndaelManaged()
        Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7"
    End Sub

    Private Function GetLegalKey() As Byte()
        Dim sTemp As String = Key
        mobjCryptoService.GenerateKey()
        Dim bytTemp As Byte() = mobjCryptoService.Key
        Dim KeyLength As Integer = bytTemp.Length

        If sTemp.Length > KeyLength Then
            sTemp = sTemp.Substring(0, KeyLength)
        ElseIf sTemp.Length < KeyLength Then
            sTemp = sTemp.PadRight(KeyLength, " "c)
        End If

        Return ASCIIEncoding.ASCII.GetBytes(sTemp)
    End Function

    Private Function GetLegalIV() As Byte()
        Dim sTemp As String = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk"
        mobjCryptoService.GenerateIV()
        Dim bytTemp As Byte() = mobjCryptoService.IV
        Dim IVLength As Integer = bytTemp.Length

        If sTemp.Length > IVLength Then
            sTemp = sTemp.Substring(0, IVLength)
        ElseIf sTemp.Length < IVLength Then
            sTemp = sTemp.PadRight(IVLength, " "c)
        End If

        Return ASCIIEncoding.ASCII.GetBytes(sTemp)
    End Function

    Public Function Encrypto(ByVal Source As String) As String
        Dim bytIn As Byte() = UTF8Encoding.UTF8.GetBytes(Source)
        Dim ms As MemoryStream = New MemoryStream()
        mobjCryptoService.Key = GetLegalKey()
        mobjCryptoService.IV = GetLegalIV()
        Dim encryptoObj As ICryptoTransform = mobjCryptoService.CreateEncryptor()
        Dim cs As CryptoStream = New CryptoStream(ms, encryptoObj, CryptoStreamMode.Write)
        cs.Write(bytIn, 0, bytIn.Length)
        cs.FlushFinalBlock()
        ms.Close()
        Dim bytOut As Byte() = ms.ToArray()
        Return Convert.ToBase64String(bytOut)
    End Function

    Public Function Decrypto(ByVal Source As String) As String
        Dim bytIn As Byte() = Convert.FromBase64String(Source)
        Dim ms As MemoryStream = New MemoryStream(bytIn, 0, bytIn.Length)
        mobjCryptoService.Key = GetLegalKey()
        mobjCryptoService.IV = GetLegalIV()
        Dim encrypto As ICryptoTransform = mobjCryptoService.CreateDecryptor()
        Dim cs As CryptoStream = New CryptoStream(ms, encrypto, CryptoStreamMode.Read)
        Dim sr As StreamReader = New StreamReader(cs)
        Return sr.ReadToEnd()
    End Function
End Class
