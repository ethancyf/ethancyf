Imports Common.DataAccess
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.SchemeDetails
Imports Common.Component.ClaimRules
Imports System.Data.SqlClient

Namespace Component.EHSClaim

    Partial Class EHSClaimValidationBLL

        Private _udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL()
        Private _udtEHSTransactionBLL As New EHSTransactionBLL()
        Private _udtClaimRulesBLL As New ClaimRulesBLL()
        Private _udtFormater As New Format.Formatter()


        Public Function chkIsTSWCase(ByVal strSPID As String, ByVal strHKID As String) As Boolean
            Dim blnRes As Boolean = False
            Dim udtdb As Database = New Database()
            Dim dt As DataTable = New DataTable()

            Dim udtFormatter As New Common.Format.Formatter()
            strHKID = udtFormatter.formatDocumentIdentityNumber(Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC, strHKID.Trim())

            Try
                Dim prams() As SqlClient.SqlParameter = { _
                    udtdb.MakeInParam("@GP_SPID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, strHKID)}

                udtdb.RunProc("proc_TSWPatientMapping_bySPIDHKID", prams, dt)

                If dt.Rows(0).Item(0) > 0 Then
                    blnRes = True
                Else
                    blnRes = False
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return blnRes
        End Function

        ' Call By Rule 7
        Public Shared Function ConvertDateOfBirthByCalMethod(ByVal strCalMethod As String, ByVal dtmDOB As Date, ByVal strExactDOB As String) As Date

            Select Case strCalMethod.Trim().ToUpper()
                Case "YEAR1"
                    Return New Date(dtmDOB.Year, 1, 1)
                Case "YEAR2"
                    Return New Date(dtmDOB.Year, 12, 31)
                Case "MONTH1"
                    'If strExactDOB.Trim() = "Y" OrElse strExactDOB = "A" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 1, 1)
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, 1)
                    End If

                Case "MONTH2"
                    'If strExactDOB.Trim() = "Y" OrElse strExactDOB = "A" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 12, Date.DaysInMonth(dtmDOB.Year, 12))
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, Date.DaysInMonth(dtmDOB.Year, dtmDOB.Month))
                    End If

                Case "DAY1"
                    'If strExactDOB.Trim() = "Y" OrElse strExactDOB = "A" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 1, 1)
                        'ElseIf strExactDOB.Trim() = "M" Then
                    ElseIf strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactMonth OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactMonth Then
                        Return New Date(dtmDOB.Year, dtmDOB.Month, 1)
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, dtmDOB.Day)
                    End If

                Case "DAY2"
                    'If strExactDOB.Trim() = "Y" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 12, Date.DaysInMonth(dtmDOB.Year, 12))
                        'ElseIf strExactDOB.Trim() = "M" Then
                    ElseIf strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactMonth OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactMonth Then
                        Return New Date(dtmDOB.Year, dtmDOB.Month, Date.DaysInMonth(dtmDOB.Year, dtmDOB.Month))
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, dtmDOB.Day)
                    End If
                Case "DAY3"
                    'If strExactDOB.Trim() = "Y" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 1, 1)
                        'ElseIf strExactDOB.Trim() = "M" Then
                    ElseIf strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactMonth OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactMonth Then
                        Return New Date(dtmDOB.Year, dtmDOB.Month, Date.DaysInMonth(dtmDOB.Year, dtmDOB.Month))
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, dtmDOB.Day)
                    End If
            End Select
            Return dtmDOB
        End Function

        ' Call By Rule 7
        Public Shared Function ConvertPassValueByCalUnit(ByVal strUnit As String, ByVal dtmPassDOB As DateTime, ByVal dtmCompareDate As Date) As Integer

            '   Y   = Year (exact Year)
            '   YC  = Year (Calendar Year)
            '   M   = Month (exact Month)
            '   MC  = Month (Calendar Month)
            '   D   = Day (exact Day)
            '   W   = Week (exact Week)
            Dim intReferenceValue As Integer = -1

            Select Case strUnit.Trim().ToUpper()
                Case "Y"
                    Dim intCompareYear As Integer = dtmCompareDate.Year
                    Dim intPassYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCompareYear - intPassYear

                    If (dtmPassDOB.Month > dtmCompareDate.Month) OrElse (dtmPassDOB.Month = dtmCompareDate.Month AndAlso dtmPassDOB.Day > dtmCompareDate.Day) Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "YC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCurYear - intDOBYear

                Case "M"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month

                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)
                    If dtmPassDOB.Day > dtmCompareDate.Day Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "MC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month
                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)

                Case "D"
                    intReferenceValue = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)

                Case "W"
                    Dim intDifferentDay As Integer = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)
                    intReferenceValue = CInt(Math.Floor(intDifferentDay / 7))
            End Select

            Return intReferenceValue
        End Function

        Public Shared Function GetValidationRuleCache() As EHSClaimValidationRuleModelCollection

            Dim udtEHSClaimValidationRuleModelCollection = New EHSClaimValidationRuleModelCollection()
            Dim udtEHSClaimValidationRuleModel As EHSClaimValidationRuleModel
            Dim udtDB = New Database()
            Dim dt As New DataTable()

            Try

                udtDB.RunProc("proc_ValidationRule_Get", dt)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows


                        Dim strRuleID As String = Nothing
                        Dim strClaimType As String = Nothing
                        Dim strHandlingMethod As String = Nothing
                        Dim strScheme_Code As String = Nothing
                        Dim intScheme_Seq As Integer = Nothing
                        Dim strSubsidize_Code As String = Nothing
                        Dim strRule_Group_ID1 As String = Nothing
                        Dim strRule_Group_ID2 As String = Nothing
                        Dim strFunction_Code As String = Nothing
                        Dim strSeverity_Code As String = Nothing
                        Dim strMessage_Code As String = Nothing
                        Dim strWarnIndicator_Code As String = Nothing

                        If Not dr.IsNull("RuleID") Then strRuleID = dr("RuleID").ToString().Trim()
                        If Not dr.IsNull("ClaimType") Then strClaimType = dr("ClaimType").ToString().Trim()
                        If Not dr.IsNull("HandlingMethod") Then strHandlingMethod = dr("HandlingMethod").ToString().Trim()
                        If Not dr.IsNull("Scheme_Code") Then strScheme_Code = dr("Scheme_Code").ToString().Trim()
                        If Not dr.IsNull("Scheme_Seq") Then intScheme_Seq = dr("Scheme_Seq").ToString().Trim()
                        If Not dr.IsNull("Subsidize_Code") Then strSubsidize_Code = dr("Subsidize_Code").ToString().Trim()
                        If Not dr.IsNull("Rule_Group_ID1") Then strRule_Group_ID1 = dr("Rule_Group_ID1").ToString().Trim()
                        If Not dr.IsNull("Rule_Group_ID2") Then strRule_Group_ID2 = dr("Rule_Group_ID2").ToString().Trim()
                        If Not dr.IsNull("Function_Code") Then strFunction_Code = dr("Function_Code").ToString().Trim()
                        If Not dr.IsNull("Severity_Code") Then strSeverity_Code = dr("Severity_Code").ToString().Trim()
                        If Not dr.IsNull("WarnIndicatorCode") Then strWarnIndicator_Code = dr("WarnIndicatorCode").ToString().Trim()

                        If Not dr.IsNull("Message_Code") Then strMessage_Code = dr("Message_Code").ToString().Trim()

                        udtEHSClaimValidationRuleModel = New EHSClaimValidationRuleModel( _
                        CInt(dr.Item("RuleSeq")), _
                        strRuleID, _
                        strClaimType, _
                        strHandlingMethod, _
                        strScheme_Code, _
                        intScheme_Seq, _
                        strSubsidize_Code, _
                        strRule_Group_ID1, _
                        strRule_Group_ID2, _
                        strFunction_Code, _
                        strSeverity_Code, _
                        strMessage_Code, _
                        strWarnIndicator_Code)

                        udtEHSClaimValidationRuleModelCollection.Add(udtEHSClaimValidationRuleModel)
                    Next

                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return udtEHSClaimValidationRuleModelCollection

        End Function
    End Class

End Namespace