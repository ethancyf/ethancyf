Imports System
Imports System.IO
Imports System.Text
Imports System.Security
Imports System.Security.Cryptography
Imports System.Web.Services

Public Class EDSymmetric

    Private m_Key() As Byte = {0, 0, 0, 0, 0, 0, 0, 0}
    Private m_IV() As Byte = {0, 0, 0, 0, 0, 0, 0, 0}

    ''' <summary>This function encrypts the connection string</summary>
    Public Function EncryptData(ByVal strKey As String, ByVal strData As String) As String

        Dim strResult As String   'Return Result

        '1. String Length cannot exceed 90Kb. Otherwise, buffer will overflow. See point 3 for reasons
        If strData.Length > 92160 Then
            strResult = "Error. Data String too large. Keep within 90Kb."
            Return strResult
        End If

        '2. Generate the Keys
        If Not InitKey(strKey) Then
            strResult = "Error. Fail to generate key for encryption"
            Return strResult
        End If
        Try
            '3. Prepare the String
            '	The first 5 character of the string is formatted to store the actual length of the data.
            '	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
            '	If anyone figure a good way to 'remember' the original length to facilite the decryption without having to use additional function parameters, pls let me know.
            strData = String.Format("{0,5:00000}" + strData, strData.Length)


            '4. Encrypt the Data
            Dim rbData(strData.Length) As Byte
            Dim aEnc As ASCIIEncoding = New ASCIIEncoding
            aEnc.GetBytes(strData, 0, strData.Length, rbData, 0)

            Dim descsp As DESCryptoServiceProvider = New DESCryptoServiceProvider

            Dim cryptoProvider As DESCryptoServiceProvider = _
                        New DESCryptoServiceProvider
            Dim ms As MemoryStream = New MemoryStream
            Dim cs As CryptoStream = _
                New CryptoStream(ms, cryptoProvider.CreateEncryptor(m_Key, m_IV), _
                    CryptoStreamMode.Write)
            Dim sw As StreamWriter = New StreamWriter(cs)

            sw.Write(strData)
            sw.Flush()
            cs.FlushFinalBlock()
            ms.Flush()

            'convert back to a string
            Return Convert.ToBase64String(ms.GetBuffer(), 0, CInt(ms.Length))
        Catch ex As Exception
            strResult = "Error. Encryption Failed. Possibly due to incorrect Key"
        End Try
        Return strResult
    End Function

    ''' <summary>This function decrypts the connection string</summary>
    Public Function DecryptData(ByVal strKey As String, ByVal strData As String) As String

        Dim strResult As String

        '1. Generate the Key used for decrypting
        If Not InitKey(strKey) Then
            strResult = "Error. Fail to generate key for decryption"
            Return strResult
        End If

        '2. Initialize the service provider
        Try

            '3. Prepare the streams:
            '	mOut is the output stream. 
            '	cs is the transformation stream.
            Dim cryptoProvider As DESCryptoServiceProvider = _
                        New DESCryptoServiceProvider

            'convert from string to byte array
            Dim buffer As Byte() = Convert.FromBase64String(strData)
            Dim ms As MemoryStream = New MemoryStream(buffer)
            Dim cs As CryptoStream = _
                New CryptoStream(ms, cryptoProvider.CreateDecryptor(m_Key, m_IV), _
                    CryptoStreamMode.Read)
            Dim sr As StreamReader = New StreamReader(cs)


            strResult = sr.ReadToEnd()

            '6. Trim the string to return only the meaningful data
            '	Remember that in the encrypt function, the first 5 character holds the length of the actual data
            '	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
            Dim strLen As String = strResult.Substring(0, 5)
            Dim nLen As Integer = Convert.ToInt32(strLen)
            strResult = strResult.Substring(5, nLen)

            Return strResult

        Catch ex As Exception
            strResult = "Error. Decryption Failed. Possibly due to incorrect Key or corrputed data"
            Return strResult
        End Try
    End Function

    ''' <summary>This function generates the keys into member variables</summary>
    Private Function InitKey(ByVal strKey As String) As Boolean

        Try

            ' Convert Key to byte array

            Dim BP(strKey.Length) As Byte
            Dim aEnc As ASCIIEncoding = New ASCIIEncoding
            aEnc.GetBytes(strKey, 0, strKey.Length, BP, 0)

            'Hash the key using SHA1
            Dim sha As SHA1CryptoServiceProvider = New SHA1CryptoServiceProvider
            Dim bpHash() As Byte = sha.ComputeHash(BP)

            Dim i As Integer

            ' use the low 64-bits for the key value
            For i = 0 To 7
                m_Key(i) = bpHash(i)
            Next i
            For i = 8 To 15
                m_IV(i - 8) = bpHash(i)
            Next i
            Return True
        Catch ex As Exception

            'Error Performing Operations
            Return False
        End Try

    End Function

    ''' <summary>This function gets the key string</summary>
    Public Function GetFileData() As String
        Try
            Dim objReader As New StreamReader("c:\evs\key.ctl")
            Dim sLine As String = ""
            sLine = objReader.ReadToEnd()
            objReader.Close()
            GetFileData = sLine
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>This function get the original key</summary>
    Public Function GetOriginalKey() As String
        Try
            Dim sKey As String = GetFileData()
            Dim keyToDecrypt() As Byte = Convert.FromBase64String(sKey)
            GetOriginalKey = Encoding.ASCII.GetString(keyToDecrypt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
