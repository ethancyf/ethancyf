Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
' Remove unused class and functions
' -----------------------------------------------------------------------------------------

'Namespace BLL

'Public Class SchemeRuleBLL
'    Public Function IsEligible(ByVal strSchemeName As String, ByVal dtDOB As Date) As Boolean
'        Dim blnRes As Boolean
'        blnRes = False

'        If chkAge(strSchemeName, dtDOB) Then
'            blnRes = True
'        End If

'        Return blnRes
'    End Function

'    Public Function chkAge(ByVal strSchemeName As String, ByVal dtDOB As Date) As Boolean
'        Dim udtdb As Database = New Database
'        Dim dt As DataTable = New DataTable
'        Dim dr As DataRow
'        Dim strRuleName As String
'        Dim blnRes As Boolean
'        Dim strOperator, strValue As String
'        Dim intValue As Integer
'        Dim intDOBYear, intCurYear, intAge As Integer
'        Dim dtToday As DateTime
'        Dim udtcommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
'        dtToday = udtcommfunct.GetSystemDateTime
'        blnRes = False

'        intCurYear = dtToday.Year
'        intDOBYear = dtDOB.Year
'        intAge = intCurYear - intDOBYear

'        strRuleName = "AGE"
'        strOperator = ""
'        strValue = ""

'        Try
'            Dim prams() As SqlParameter = { _
'                udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeName), _
'                udtdb.MakeInParam("@Rule_Name", SqlDbType.VarChar, 20, strRuleName) _
'                }
'            udtdb.RunProc("proc_SchemeRule_get_bySchemeCodeRuleName", prams, dt)

'            If dt.Rows.Count > 0 Then
'                dr = dt.Rows(0)
'                strOperator = dr.Item("Operator")
'                strValue = dr.Item("Value")
'                If strValue.Length() > 0 Then
'                    intValue = CInt(strValue)
'                Else
'                    intValue = 0
'                End If
'                Select Case strOperator.Trim()
'                    Case ">"
'                        If intAge > intValue Then
'                            blnRes = True
'                        End If
'                    Case "<"
'                        If intAge < intValue Then
'                            blnRes = True
'                        End If
'                    Case "="
'                        If intAge = intValue Then
'                            blnRes = True
'                        End If
'                    Case Else
'                        blnRes = True
'                End Select
'            Else
'                blnRes = True
'            End If
'        Catch eSQL As SqlException
'            blnRes = False
'            Throw eSQL
'        Catch ex As Exception
'            blnRes = False
'            Throw ex
'        End Try

'        Return blnRes
'    End Function
'End Class

'End Namespace
' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]