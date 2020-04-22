Imports Common.DataAccess
Imports System.Data.SqlClient


Namespace Component.CCCode

    Public Class CCCodeBLL

        'Get CCCode Desc
        Public Function GetCCCodeDesc(ByVal strCCCode As String, ByRef strDisplay As String) As String
            Dim udtdb As Database = New Database()
            Dim intIdx As Integer
            Dim dtRes As New DataTable
            Dim ccc_tail As String = String.Empty
            Dim big5 As String
            Dim unicode As Integer

            Try
                Dim prams() As SqlParameter = { _
                 udtdb.MakeInParam("@ccc_head ", SqlDbType.VarChar, 4, strCCCode)}

                udtdb.RunProc("proc_ccc_big5_get_byCCCHeader", prams, dtRes)

                ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
                dtRes.Columns.Add("big5", GetType(String))

                intIdx = 0
                For Each dataRow As DataRow In dtRes.Rows

                    unicode = CStr(IIf(dataRow("UniCode_Int") Is DBNull.Value, "", dataRow("UniCode_Int")))
                    dataRow("big5") = Me.ConvertUnicode2Big5(unicode)

                    ccc_tail = CStr(IIf(dataRow("ccc_tail") Is DBNull.Value, "", dataRow("ccc_tail")))
                    big5 = CStr(IIf(dataRow("big5") Is DBNull.Value, "", dataRow("big5")))
                    strDisplay += ccc_tail.Trim + "." + big5.Trim
                    strDisplay += " "

                    intIdx = intIdx + 1
                Next
                ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

                If intIdx = 1 Then
                    Return strCCCode.Trim + ccc_tail.Trim
                Else
                    Return strCCCode
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetCCCodeDesc(ByVal strCCCode As String) As DataTable
            Dim udtDB As Database = New Database
            Dim dtRes As New DataTable

            Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@ccc_head ", SqlDbType.VarChar, 4, strCCCode)}

            udtDB.RunProc("proc_ccc_big5_get_byCCCHeader", prams, dtRes)

            ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
            dtRes.Columns.Add("big5", GetType(String))

            For Each dataRow As DataRow In dtRes.Rows
                dataRow("big5") = Me.ConvertUnicode2Big5(dataRow("UniCode_Int"))
            Next
            ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

            Return dtRes
        End Function


        Public Function GetChiChar(ByVal strCCCode As String) As String
            Dim udtdb As Database = New Database()
            Dim strReturnValue As String

            Try
                Dim prams() As SqlParameter = { _
                 udtdb.MakeInParam("@ccc", SqlDbType.Char, 5, strCCCode), _
                 udtdb.MakeOutParam("@chi_character", SqlDbType.NVarChar, 4), _
                 udtdb.MakeOutParam("@return_code", SqlDbType.Int, 4), _
                 udtdb.MakeOutParam("@return_msg", SqlDbType.VarChar, 255)}
                udtdb.RunProc("proc_ccc_big5_get_byCCCode", prams)
                strReturnValue = CStr(IIf(prams(1).Value Is DBNull.Value, "", prams(1).Value))
                Return strReturnValue
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        Public Function ConvertUnicode2Big5(ByVal intUniCode As Integer) As String

            Dim strBig5 As String = String.Empty

            Try
                'Convert unicode to big5
                strBig5 = Char.ConvertFromUtf32(intUniCode)

                If strBig5 Is Nothing OrElse strBig5.Equals(String.Empty) Then
                    Return "  "
                Else
                    Return strBig5
                End If

            Catch ex As Exception
                'Cannot convert to char
                Return "  "
            End Try

        End Function
        ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Get CCCode 
        Public Function GetCCCodeByChar(ByVal strChar As String) As String
            Dim udtdb As Database = New Database()

            Dim dtRes As New DataTable
            Dim ccc_head As String = String.Empty
            Dim ccc_tail As String = String.Empty
            Dim strCCCode As String = String.Empty
            Dim intUnicode As Integer

            Try
                intUnicode = ConvertBig5toUnicode(strChar)

                Dim prams() As SqlParameter = { _
                 udtdb.MakeInParam("@UniCode_Int ", SqlDbType.Int, 4, intUnicode)}

                udtdb.RunProc("proc_ccc_big5_get_byUnicode", prams, dtRes)

                If dtRes.Rows.Count = 1 Then
                    Dim dr As DataRow = dtRes.Rows(0)

                    ccc_head = CStr(IIf(dr("ccc_head") Is DBNull.Value, "", dr("ccc_head")))
                    ccc_tail = CStr(IIf(dr("ccc_tail") Is DBNull.Value, "", dr("ccc_tail")))

                    strCCCode = ccc_head + ccc_tail
                End If

                Return strCCCode

            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function ConvertBig5toUnicode(ByVal strBig5 As String) As Integer

            Dim intUniCode As Integer = Nothing

            Try
                'Convert big5 to unicode
                intUniCode = Char.ConvertToUtf32(strBig5, 0)

                Return intUniCode

            Catch ex As Exception
                'Cannot convert to unicode
                Return Nothing
            End Try

        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]
    End Class

End Namespace