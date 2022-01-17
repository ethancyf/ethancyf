Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComObject


Namespace Component.CCCode

    Public Class CCCodeBLL

#Region "Cache"
        Public Function GetCCCodeChineseMappingCache() As DataTable
            Dim dt As New DataTable

            If HttpRuntime.Cache("CCCodeChineseMapping") Is Nothing Then
                Try
                    Dim udtDB As New Database

                    udtDB.RunProc("proc_CCCodeChineseMapping_get_all_cache", dt)

                    CacheHandler.InsertCache("CCCodeChineseMapping", dt)

                Catch ex As Exception
                    Throw
                End Try
            Else
                dt = CType(HttpRuntime.Cache("CCCodeChineseMapping"), DataTable)
            End If

            Return dt
        End Function
#End Region

#Region "Retrieve Function"
        Public Function GetCCCodeChineseMappingByUnicode(ByVal strUnicode As String) As DataTable
            Dim dtRes As New DataTable

            Dim dt As DataTable = GetCCCodeChineseMappingCache()

            If dt IsNot Nothing AndAlso dt.Select(String.Format("UniCode_Int = {0}", strUnicode)).Length > 0 Then
                dtRes = dt.Select(String.Format("UniCode_Int = {0}", strUnicode)).CopyToDataTable
            End If

            dtRes.Columns.Add("ConvertedCharacter", GetType(String))

            For Each dataRow As DataRow In dtRes.Rows
                dataRow("ConvertedCharacter") = Me.ConvertUnicodetoChar(dataRow("UniCode_Int"))
            Next

            Return dtRes

        End Function

        Public Function GetCCCodeChineseMappingByCCCode(ByVal strCCCode As String) As DataTable
            Dim dtRes As New DataTable
            Dim dt As DataTable = GetCCCodeChineseMappingCache()

            If dt Is Nothing Then Return dtRes

            dtRes = dt.Clone
            dtRes.Columns.Add("ConvertedCharacter", GetType(String))

            For Each dr As DataRow In dt.Select(String.Format("CCCode = {0}", strCCCode))
                Dim intUnicode As Integer

                If Not IsDBNull(dr("UniCode_Int")) AndAlso Integer.TryParse(dr("UniCode_Int"), intUnicode) Then
                    dtRes.ImportRow(dr)
                End If
            Next

            For Each dataRow As DataRow In dtRes.Rows
                dataRow("ConvertedCharacter") = Me.ConvertUnicodetoChar(dataRow("UniCode_Int"))
            Next

            Return dtRes
        End Function

        Public Function GetCCCodeChineseMappingByCCCHead(ByVal strCCCHead As String) As DataTable
            Dim dtRes As New DataTable
            Dim dt As DataTable = GetCCCodeChineseMappingCache()

            If dt Is Nothing Then Return dtRes

            dtRes = dt.Clone
            dtRes.Columns.Add("ConvertedCharacter", GetType(String))

            For Each dr As DataRow In dt.Select(String.Format("CCC_Head = {0}", strCCCHead))
                Dim intUnicode As Integer

                If Not IsDBNull(dr("UniCode_Int")) AndAlso Integer.TryParse(dr("UniCode_Int"), intUnicode) Then
                    dtRes.ImportRow(dr)
                End If
            Next

            For Each dataRow As DataRow In dtRes.Rows
                dataRow("ConvertedCharacter") = Me.ConvertUnicodetoChar(dataRow("UniCode_Int"))
            Next

            Return dtRes

        End Function

        Public Function getChiCharByCCCode(ByVal strCCCode As String) As String
            Dim strChar As String = String.Empty
            Dim dtRes As DataTable

            If Not strCCCode Is Nothing AndAlso strCCCode.Length > 0 Then
                If strCCCode.Length <> 5 Then
                    Return " "
                End If

                dtRes = GetCCCodeChineseMappingByCCCode(strCCCode)

                If Not dtRes Is Nothing AndAlso dtRes.Rows.Count > 0 Then

                    For Each dataRow As DataRow In dtRes.Rows
                        Return dataRow("ConvertedCharacter").ToString()
                    Next

                    Return " "
                Else
                    Return " "
                End If
            End If

            Return strChar
        End Function

        'Get CCCode 
        Public Function getCCCodeByChar(ByVal strChar As String) As String
            Dim udtdb As Database = New Database()

            Dim dtRes As New DataTable
            Dim strCCCode As String = String.Empty
            Dim intUnicode As Integer

            Try
                ' Convert to Unicode 
                intUnicode = ConvertChartoUnicode(strChar)

                ' Find CCCode from Mapping by Unicode
                dtRes = GetCCCodeChineseMappingByUnicode(intUnicode)

                If dtRes.Rows.Count = 1 Then
                    Dim dr As DataRow = dtRes.Rows(0)
                    strCCCode = CStr(IIf(dr("CCCode") Is DBNull.Value, "", dr("CCCode")))
                End If

                Return strCCCode

            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function getCCCodeForChiName(ByVal strChineseName As String, ByVal intPosition As Integer) As String
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

            If udtGeneralFunction.UTF32_Length(strChineseName) >= intPosition Then
                Dim strCCCode As String = String.Empty

                Dim strChar As String = udtGeneralFunction.UTF32_SubString(strChineseName, intPosition - 1, 1)
                strCCCode = Me.getCCCodeByChar(strChar)

                Return strCCCode
            Else
                Return String.Empty
            End If
        End Function

#End Region

        ' CRE20-023-68 (Remove HA MingLiu) [Start][Winnie SUEN]
        ' -------------------------------------------------------------
        Public Function ConvertUnicodetoChar(ByVal intUniCode As Integer) As String

            Dim strChar As String = String.Empty

            Try
                strChar = Char.ConvertFromUtf32(intUniCode)

                If strChar Is Nothing OrElse strChar.Equals(String.Empty) Then
                    Return "  "
                Else
                    Return strChar
                End If

            Catch ex As Exception
                'Cannot convert to char
                Return "  "
            End Try

        End Function

        Public Function ConvertChartoUnicode(ByVal strChar As String) As Integer

            Dim intUniCode As Integer = Nothing

            Try
                'Convert Char to unicode
                intUniCode = Char.ConvertToUtf32(strChar, 0)

                Return intUniCode

            Catch ex As Exception
                'Cannot convert to unicode
                Return Nothing
            End Try

        End Function
        ' CRE20-023-68 (Remove HA MingLiu) [End][Winnie SUEN]

    End Class

End Namespace