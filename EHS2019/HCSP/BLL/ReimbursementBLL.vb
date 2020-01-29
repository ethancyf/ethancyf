Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format

Namespace BLL
    Public Class ReimbursementBLL

        Public Function GetMonthlyReimbursementStatement(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer, ByVal strReimburseID As String, ByVal enumSubPlatform As [Enum]) As DataSet
            Dim ds As New DataSet
            Dim db As New Database

            Dim strSubPlatform As String = String.Empty
            If Not enumSubPlatform Is Nothing Then
                strSubPlatform = enumSubPlatform.ToString
            End If

            Dim prams() As SqlParameter = { _
                        db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                        db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, intPracticeDisplaySeq), _
                        db.MakeInParam("@Reimburse_ID", SqlDbType.Char, 15, strReimburseID), _
                        db.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}

            db.RunProc("proc_ReimbursementAuthTranSum_get", prams, ds)

            Return ds

        End Function

        Public Function GetMonthlyReimbursementStatementDetails(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer, ByVal strReimburseID As String, ByVal strSchemeCode As String) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim prams() As SqlParameter = { _
                        db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                        db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, intPracticeDisplaySeq), _
                        db.MakeInParam("@Reimburse_ID", SqlDbType.Char, 15, strReimburseID), _
                        db.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode)}
            db.RunProc("proc_ReimbursementAuthTranList_get", prams, dt)

            Return dt
        End Function

        Public Function GetBankInDateList(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim prams() As SqlParameter = { _
                        db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                        db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, intPracticeDisplaySeq)}
            db.RunProc("proc_BankIn_get_forStatmentList", prams, dt)

            Return dt
        End Function

        Public Function FormatStatementList(ByRef dtBankInDateList As DataTable, ByVal strLang As String) As DataTable

            Dim udtFormatter As New Formatter
            Dim dtStatementList As DataTable

            Dim dtmSubmissionDtm As DateTime
            Dim strDisplayText As String
            Dim strSubmissionDtm As String
            Dim dtmReimbursementDtm As DateTime
            Dim strReimbursementDtm As String

            dtStatementList = dtBankInDateList.Copy
            dtStatementList.Columns.Add(New DataColumn("Display_Text", GetType(String)))

            Dim i As Integer
            For i = 0 To dtStatementList.Rows.Count - 1
                'dtmSubmissionDtm = CType(dtBankInDateList.Rows(i).Item("Submission_Dtm"), DateTime)
                dtmSubmissionDtm = CType(dtBankInDateList.Rows(i).Item("CutOff_Date"), DateTime)
                'strSubmissionDtm = udtFormatter.formatDate(dtmSubmissionDtm)

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL

                'strSubmissionDtm = udtFormatter.formatDate(dtmSubmissionDtm) 'udtFormatter.formatMonth(dtmSubmissionDtm)
                strSubmissionDtm = udtFormatter.formatDisplayDate(dtmSubmissionDtm, strLang) 'udtFormatter.formatMonth(dtmSubmissionDtm)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                strDisplayText = HttpContext.GetGlobalResourceObject("Text", "MonthlyStatementList")
                'strDisplayText = strDisplayText.Replace("%s", strSubmissionDtm.ToUpper())
                strDisplayText = strDisplayText.Replace("%s", strSubmissionDtm)

                dtmReimbursementDtm = CType(dtBankInDateList.Rows(i).Item("Value_Date"), DateTime)
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'strReimbursementDtm = udtFormatter.formatDate(dtmReimbursementDtm)
                strReimbursementDtm = udtFormatter.formatDisplayDate(dtmReimbursementDtm, strLang)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                'If i = 0 Then
                '    strDisplayText = HttpContext.GetGlobalResourceObject("Text", "TheLatestStatement")
                'Else
                '    strDisplayText = HttpContext.GetGlobalResourceObject("Text", "LatestStatement")
                'End If
                'Select Case i + 1
                '    Case 1
                '        strDisplayText = HttpContext.GetGlobalResourceObject("Text", "TheLatestStatement")
                '    Case 2
                '        strDisplayText = HttpContext.GetGlobalResourceObject("Text", "2ndLatestStatement")
                '        strDisplayText = strDisplayText.Replace("%s", 2)
                '    Case 3
                '        strDisplayText = HttpContext.GetGlobalResourceObject("Text", "3rdLatestStatement")
                '        strDisplayText = strDisplayText.Replace("%s", 3)
                '    Case Else
                '        strDisplayText = HttpContext.GetGlobalResourceObject("Text", "LatestStatement")
                '        strDisplayText = strDisplayText.Replace("%s", (i + 1).ToString("#,##0"))
                'End Select
                'strDisplayText &= " (" & strSubmissionDtm & ")"

                dtStatementList.Rows(i).Item("Display_Text") = strDisplayText '& " (" & strReimbursementDtm & ")"
            Next

            Return dtStatementList
        End Function

    End Class
End Namespace



