Namespace ComObject

    Public Class AuditLogStartKey
        Public _objAuditLogEntry As BaseAuditLogEntry
        Public _dtmStartTime As DateTime
        Public _strKey As String = String.Empty

        Public ReadOnly Property AuditLogEntry() As BaseAuditLogEntry
            Get
                Return _objAuditLogEntry
            End Get
        End Property

        Public ReadOnly Property StartTime() As DateTime
            Get
                Return _dtmStartTime
            End Get
        End Property

        Public Sub New(ByVal objAuditLogEntry As BaseAuditLogEntry, ByVal dtmStartTime As DateTime)
            Me._objAuditLogEntry = objAuditLogEntry
            Me._dtmStartTime = dtmStartTime
            Me._strKey = GenUniqueKey()
        End Sub

        Protected Function GenUniqueKey() As String

            Dim maxSize As Integer = 19
            Dim minSize As Integer = 5
            Dim chars() As Char = New Char(62) {}

            Dim s As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
            chars = s.ToCharArray()
            Dim size As Integer = maxSize
            Dim bytes() As Byte = New Byte(size) {}

            Dim rng As New System.Security.Cryptography.RNGCryptoServiceProvider()
            rng.GetNonZeroBytes(bytes)
            Dim Key As New System.Text.StringBuilder
            Dim b As Byte
            For Each b In bytes
                Key.Append(chars(b Mod (chars.Length - 1)))
            Next
            Return Key.ToString()

        End Function
    End Class

End Namespace