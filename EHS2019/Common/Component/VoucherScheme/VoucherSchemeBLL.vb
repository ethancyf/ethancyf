Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.Scheme

' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
' Remove unused class and functions
' -----------------------------------------------------------------------------------------

'Namespace Component.VoucherScheme
'    Public Class VoucherSchemeBLL

        'Public Const SESS_Scheme As String = "Scheme"

        'Public Function GetScheme() As VoucherSchemeModel
        '    Dim udtScheme As VoucherSchemeModel
        '    udtScheme = Nothing
        '    If Not HttpContext.Current.Session(SESS_Scheme) Is Nothing Then
        '        Try
        '            udtScheme = CType(HttpContext.Current.Session(SESS_Scheme), VoucherSchemeModel)
        '        Catch ex As Exception
        '            Throw New Exception("Invalid Session Claim Tran!")
        '        End Try
        '    Else
        '        Throw New Exception("Session Expired!")
        '    End If
        '    Return udtScheme
        'End Function

        'Public Function Exist() As Boolean
        '    If HttpContext.Current.Session Is Nothing Then Return False
        '    If Not HttpContext.Current.Session(SESS_Scheme) Is Nothing Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        'Public Sub ClearSession()
        '    HttpContext.Current.Session(SESS_Scheme) = Nothing
        'End Sub

        'Public Sub SaveToSession(ByRef udtScheme As VoucherSchemeModel)
        '    HttpContext.Current.Session(SESS_Scheme) = udtScheme
        'End Sub

        'Public Function IsEligible(ByVal strSchemeName As String, ByVal dtDOB As Date) As Boolean
        '    Dim blnRes As Boolean
        '    Dim udtcommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        '    blnRes = False

        '    If chkAge(strSchemeName, dtDOB, udtcommfunct.GetSystemDateTime) Then
        '        blnRes = True
        '    End If

        '    Return blnRes
        'End Function

        'Public Function getTotalVoucher(ByVal strSchemeName As String, ByVal dtDOB As Date, ByVal dtAgainstDate As Date) As Integer
        '    Dim intRes As Integer = 0


        '    Return intRes
        'End Function

        'Public Function chkAge(ByVal strSchemeName As String, ByVal dtDOB As Date, ByVal dtAgainstDate As Date) As Boolean
        '    Dim udtdb As Database = New Database
        '    Dim dt As DataTable = New DataTable
        '    Dim dr As DataRow
        '    Dim strRuleName As String
        '    Dim blnRes As Boolean
        '    Dim strOperator, strValue As String
        '    Dim intValue As Integer
        '    Dim intDOBYear, intCurYear, intAge As Integer
        '    blnRes = False

        '    intCurYear = dtAgainstDate.Year
        '    intDOBYear = dtDOB.Year
        '    intAge = intCurYear - intDOBYear

        '    strRuleName = "AGE"
        '    strOperator = ""
        '    strValue = ""

        '    Try
        '        Dim prams() As SqlParameter = { _
        '            udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeName), _
        '            udtdb.MakeInParam("@Rule_Name", SqlDbType.VarChar, 20, strRuleName) _
        '            }
        '        udtdb.RunProc("proc_SchemeRule_get_bySchemeCodeRuleName", prams, dt)

        '        If dt.Rows.Count > 0 Then
        '            dr = dt.Rows(0)
        '            strOperator = dr.Item("Operator")
        '            strValue = dr.Item("Value")
        '            If strValue.Length() > 0 Then
        '                intValue = CInt(strValue)
        '            Else
        '                intValue = 0
        '            End If
        '            Select Case strOperator.Trim()
        '                Case ">"
        '                    If intAge > intValue Then
        '                        blnRes = True
        '                    End If
        '                Case "<"
        '                    If intAge < intValue Then
        '                        blnRes = True
        '                    End If
        '                Case "="
        '                    If intAge = intValue Then
        '                        blnRes = True
        '                    End If
        '                Case Else
        '                    blnRes = True
        '            End Select
        '        Else
        '            blnRes = True
        '        End If
        '    Catch eSQL As SqlException
        '        blnRes = False
        '        Throw eSQL
        '    Catch ex As Exception
        '        blnRes = False
        '        Throw ex
        '    End Try

        '    Return blnRes
        'End Function

        'Public Function LoadVoucheScheme(ByVal strSchemeCode As String, Optional ByVal udtdb As Database = Nothing) As VoucherSchemeModel

        '    Dim udtVoucherScheme As VoucherSchemeModel = New VoucherSchemeModel
        '    Dim dt As DataTable
        '    Dim udtdb As Database

        '    dt = New DataTable

        '    If IsNothing(udtdb) Then
        '        udtdb = New Database()
        '    End If

        '    Dim prams() As SqlParameter = { _
        '    udtdb.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '    }

        '    udtdb.RunProc("proc_VoucherSchemeActive_get_bySchCode", prams, dt)

        '     2009-06-19 Handle New Scheme Change

        '    If Not dt.Rows.Count = 0 Then
        '        With udtVoucherScheme

        '            Dim strSchemeDescChi As String = String.Empty
        '            Dim strSchemeDisplayName As String = String.Empty
        '            Dim strSchemeDetailDisplayName As String = String.Empty

        '             Handle DB Null
        '            If Not dt.Rows(0).IsNull("Scheme_Description_Chi") Then
        '                strSchemeDescChi = dt.Rows(0).IsNull("Scheme_Description_Chi").ToString().Trim()
        '            End If
        '            If Not dt.Rows(0).IsNull("Scheme_Display_Code") Then
        '                strSchemeDisplayName = dt.Rows(0).IsNull("Scheme_Display_Code").ToString().Trim()
        '            End If
        '            If Not dt.Rows(0).IsNull("Scheme_Detail_Display_Name") Then
        '                strSchemeDetailDisplayName = dt.Rows(0).IsNull("Scheme_Detail_Display_Name").ToString().Trim()
        '            End If

        '            .SchemeCode = dt.Rows(0).Item("Scheme_Code")
        '            .SchemeDesc = dt.Rows(0).Item("Scheme_Description")
        '            .SchemeDescChi = strSchemeDescChi
        '            .SeqNo = dt.Rows(0).Item("sequence")
        '            .Effectdate = dt.Rows(0).Item("Effective_Date")
        '            .ExpiryDate = dt.Rows(0).Item("Expiry_Date")
        '            .VoucherNo = dt.Rows(0).Item("Unit")
        '            .VoucherValue = dt.Rows(0).Item("Unit_Value")
        '            .SchemeDisplayName = strSchemeDisplayName
        '            .SchemeDetailDisplayName = strSchemeDetailDisplayName
        '        End With

        '        Return udtVoucherScheme
        '    Else
        '        Return Nothing
        '    End If

        'End Function

        'Public Function getSchemeTotalVoucher(ByVal strSchemeCode As String, ByVal dtDOB As Date) As Integer
        '    Dim udtVoucherScheme As VoucherSchemeModel = New VoucherSchemeModel
        '    Dim dt As DataTable = New DataTable
        '    Dim udtdb As Database
        '    Dim intTotalSchemeVoucher As Integer = 0
        '    Dim intRes As Integer = 0

        '     2009-06-19 Handle New Scheme Change

        '    udtdb = New Database()
        '    Dim prams() As SqlParameter = { _
        '    udtdb.MakeInParam("@Scheme_Code", SqlDbType.VarChar, 10, strSchemeCode) _
        '    }
        '    udtdb.RunProc("proc_VoucherSchemeAllActive_get_bySchCode", prams, dt)
        '    If Not dt Is Nothing Then
        '        For Each row As DataRow In dt.Rows
        '            If chkAge(strSchemeCode, dtDOB, row.Item("Effective_Date")) Then
        '                intTotalSchemeVoucher = intTotalSchemeVoucher + row.Item("Unit")
        '            End If
        '        Next
        '    End If
        '    intRes = intTotalSchemeVoucher
        '    Return intRes

        'End Function
'    End Class

'End Namespace
' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]