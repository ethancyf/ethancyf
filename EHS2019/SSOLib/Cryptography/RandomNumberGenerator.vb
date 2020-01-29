' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Generator
' Detail            : Generate Random Number
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'                           03-Jun-2010         Pak Ho LEE                                Add Function getUniqueKey()
' ---------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Security.Cryptography

Namespace Cryptography
    Public Class RandomNumberGenerator
        Private Shared objRNGCSP As New RNGCryptoServiceProvider()
        Private Shared bRndByte As Byte() = New Byte(3) {}


        Public Shared Function [Next]() As Integer
            objRNGCSP.GetBytes(bRndByte)
            Dim intValue As Integer = BitConverter.ToInt32(bRndByte, 0)
            If intValue < 0 Then
                intValue = -intValue
            End If
            Return intValue
        End Function



        Public Shared Function [Next](ByVal intMax As Integer) As Integer
            objRNGCSP.GetBytes(bRndByte)
            Dim intValue As Integer = BitConverter.ToInt32(bRndByte, 0)
            intValue = intValue Mod (intMax + 1)
            If intValue < 0 Then
                intValue = -intValue
            End If
            Return intValue
        End Function

        Public Shared Function [Next](ByVal intMin As Integer, ByVal intMax As Integer) As Integer
            Dim intValue As Integer = [Next](intMax - intMin) + intMin
            Return intValue
        End Function

        Public Shared Function getRandomString(ByVal intLen As Integer) As String
            Dim sbRndStr As New System.Text.StringBuilder()
            Dim chars As Char() = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()
            'int length = RandomNumberGenerator.Next(intLen, intLen);
            Dim length As Integer = intLen

            For intCounter As Integer = 0 To length - 1
                sbRndStr.Append(chars(RandomNumberGenerator.[Next](chars.Length - 1)))
            Next

            Return sbRndStr.ToString()
        End Function

        Public Shared Function getUniqueKey(ByVal intLength As String)

            Dim intMaxSize As Integer = intLength
            'Dim intMinSize As Integer = 5
            Dim chars As Char() = New Char(61) {}
            Dim strCharMask As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
            chars = strCharMask.ToCharArray()
            Dim intSize As Integer = intMaxSize
            Dim data As Byte() = New Byte(0) {}
            Dim crypto As New RNGCryptoServiceProvider()
            crypto.GetNonZeroBytes(data)
            intSize = intMaxSize
            data = New Byte(intSize - 1) {}
            crypto.GetNonZeroBytes(data)

            Dim strBuilder As New StringBuilder(intSize)

            For Each b As Byte In data
                strBuilder.Append(chars(b Mod (chars.Length - 1)))
            Next

            Return strBuilder.ToString()

        End Function
    End Class
End Namespace