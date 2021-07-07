Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports System.Globalization


Namespace Validation

    Public Class Validator

        Dim sm As ComObject.SystemMessage
        Dim udtcomfunct As ComFunction.GeneralFunction = New ComFunction.GeneralFunction


        Public Function chkEngName(ByVal strOriSurname As String, ByVal strOriFirstname As String, Optional ByVal isMax40 As Boolean = False) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim strErrMsg As String
            Dim intEngNameDataSize As Integer
            Dim strNameLengthMsgCode As String

            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""
            strErrMsg = ""

            If isMax40 = True Then
                intEngNameDataSize = 40
                strNameLengthMsgCode = MsgCode.MSG00068
            Else
                intEngNameDataSize = 82
                strNameLengthMsgCode = MsgCode.MSG00479
            End If

            strOriSurname = strOriSurname.Trim
            strOriFirstname = strOriFirstname.Trim


            If IsEmpty(strOriSurname) Then
                strMsgCode = "00007"
            Else
                If IsEmpty(strOriFirstname) Then
                    If strOriSurname.Length > intEngNameDataSize Then
                        strMsgCode = strNameLengthMsgCode
                    Else
                        If Not IsValidEngName(strOriSurname) Then
                            strMsgCode = "00021"
                        End If
                    End If
                Else
                    'total length 40 for the name, and a comma and a space is reserved, so the value is 38
                    If strOriSurname.Trim.Length + strOriFirstname.Trim.Length > intEngNameDataSize - 2 Then
                        'strMsgCode = "00021"
                        strMsgCode = strNameLengthMsgCode
                    Else
                        'Check whether Surname and/or Firstname has invalid character
                        If IsValidEngName(strOriSurname) Then
                            If Not IsEmpty(strOriFirstname) Then
                                If Not IsValidEngName(strOriFirstname) Then
                                    strMsgCode = "00021"
                                End If
                            End If
                        Else
                            strMsgCode = "00021"
                        End If
                    End If
                End If
            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm

        End Function

        Public Function IsValidEngName(ByVal strEngName As String) As Boolean
            Dim blnRes, blnFoundErr As Boolean
            Dim chrTemp() As Char
            Dim i As Integer
            Dim intasc As Integer = 0

            blnRes = False
            blnFoundErr = False
            i = 0

            chrTemp = strEngName.ToCharArray
            While i < strEngName.Length And Not blnFoundErr
                intasc = Asc(chrTemp(i))
                If (intasc < 65 Or intasc > 90) And Not intasc = 32 And _
                    Not intasc = 39 And Not intasc = 45 And Not intasc = 46 Then
                    blnFoundErr = True
                End If
                i = i + 1
            End While

            If blnFoundErr Then
                blnRes = False
            Else
                blnRes = True
            End If

            Return blnRes
        End Function

        ''' <summary>
        ''' Check the input field where is Empty string
        ''' </summary>
        ''' <param name="StrOriField"></param>
        ''' <returns>
        ''' True: None Empty String
        ''' False: Empty String
        ''' </returns>
        ''' <remarks>It is a common function used to check mandatory field</remarks>
        ''' 
        Public Function IsEmpty(ByVal StrOriField As String) As Boolean
            Dim blnres As Boolean
            blnres = False
            If StrOriField.Length > 0 Then
                blnres = False
            Else
                blnres = True
            End If
            Return blnres
        End Function

        Public Function chkValidatedEHSAccountNumber(ByVal strAccoundId As String) As Boolean
            Dim result As Boolean = False
            Dim strParmValue1 As String = String.Empty
            Dim strParmValue2 As String = String.Empty
            Dim tempAccountIdCheckDigit As String = String.Empty
            Dim tempAccountId As String = strAccoundId.Trim()

            ' Get the Validated EHS Account Prefix
            udtcomfunct.getSytemParameterByParameterName("eHealthAccountPrefix", strParmValue1, strParmValue2)

            If Not String.IsNullOrEmpty(strParmValue1) AndAlso Not tempAccountId.StartsWith(strParmValue1) Then
                tempAccountId = String.Format("{0}{1}", strParmValue1, tempAccountId)
            End If

            ' Validated Account Lenght = 8, Check Digit Lenght = 1
            If tempAccountId.Length = strParmValue1.Length + 8 + 1 Then
                tempAccountIdCheckDigit = tempAccountId(tempAccountId.Length - 1).ToString()

                If tempAccountIdCheckDigit = udtcomfunct.generateChkDgt(tempAccountId.Substring(0, tempAccountId.Length - 1)) Then
                    result = True
                End If
            End If

            Return result
        End Function

        Public Function chkEmailAddress(ByVal strOriEmail As String) As ComObject.SystemMessage
            Dim blnRes As Boolean
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""
            blnRes = True

            If IsEmpty(strOriEmail) Then
                blnRes = False
                strMsgCode = "00005"
            Else

                Try
                    Dim oMailMsg As New System.Net.Mail.MailAddress(strOriEmail)

                    Dim eHead As String = strOriEmail.Split("@")(0)

                    Dim eBodyTail As String = strOriEmail.Split("@")(1)
                    Dim eBodyTailCollection() As String = eBodyTail.Split(".")
                    Dim eLength As Integer = eBodyTailCollection.Length

                    If eLength = 1 Then
                        blnRes = False
                        strMsgCode = "00006"
                    Else

                        Dim eTail As String = eBodyTailCollection(eLength - 1)
                        Dim eBody As String = eBodyTail.Substring(0, eBodyTail.Length - eTail.Length - 1)

                        If eHead.Contains(" ") Or eHead.Contains("(") Or eHead.Contains(")") Or _
                            strOriEmail.Length > 256 Or eHead.Length > 64 Or eBodyTail.Length > 255 Then
                            blnRes = False
                            strMsgCode = "00006"
                        Else

                            Dim i As Integer
                            Dim x As Char
                            For i = 1 To eBody.Length
                                x = GetChar(eBody, i)
                                If x = "_" Or x = "-" Then

                                    If i = 1 Then
                                        blnRes = False
                                        strMsgCode = "00006"
                                        Exit For
                                    ElseIf i = eBody.Length Then
                                        blnRes = False
                                        strMsgCode = "00006"
                                        Exit For
                                    ElseIf GetChar(eBody, i + 1) = "." Or GetChar(eBody, i - 1) = "." Then
                                        blnRes = False
                                        strMsgCode = "00006"
                                        Exit For
                                    End If

                                ElseIf x = "." Then

                                    If i = 1 Then
                                        blnRes = False
                                        strMsgCode = "00006"
                                        Exit For
                                    End If

                                ElseIf Not (Char.GetUnicodeCategory(x) = System.Globalization.UnicodeCategory.LowercaseLetter Or Char.GetUnicodeCategory(x) = System.Globalization.UnicodeCategory.UppercaseLetter Or Char.GetUnicodeCategory(x) = System.Globalization.UnicodeCategory.DecimalDigitNumber) Then
                                    blnRes = False
                                    strMsgCode = "00006"
                                    Exit For
                                End If
                            Next

                            If blnRes Then
                                For i = 1 To eTail.Length
                                    x = GetChar(eTail, i)
                                    If Not (Char.GetUnicodeCategory(x) = System.Globalization.UnicodeCategory.LowercaseLetter Or Char.GetUnicodeCategory(x) = System.Globalization.UnicodeCategory.UppercaseLetter Or Char.GetUnicodeCategory(x) = System.Globalization.UnicodeCategory.DecimalDigitNumber) Then
                                        blnRes = False
                                        strMsgCode = "00006"
                                        Exit For
                                    End If
                                Next
                            End If

                            If blnRes Then
                                If eTail.Length < 2 Or eTail.Length > 6 Then
                                    blnRes = False
                                    strMsgCode = "00006"
                                End If
                            End If
                        End If
                    End If

                Catch exFormat As FormatException
                    blnRes = False
                    strMsgCode = "00006"
                Catch ex As Exception
                    blnRes = False
                    strMsgCode = "00006"
                End Try
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm
        End Function

        Public Function chkDateIsFutureDate(ByVal dtmInputdate As DateTime) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            'Dim nowdtm As New DateTime(Now.Year, Now.Month, Now.Day)
            Dim nowdtm As DateTime = (New Common.ComFunction.GeneralFunction).GetSystemDateTime.Date

            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"

            If DateDiff("d", nowdtm, dtmInputdate) > 0 Then
                strMsgCode = "00022"
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkDateIsTodayFutureDate(ByVal strFunctCode As String, ByVal dtmInputdate As DateTime) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim nowdtm As New DateTime(Now.Year, Now.Month, Now.Day)

            Dim strSeverity, strMsgCode As String
            strSeverity = "E"

            If DateDiff("d", nowdtm, dtmInputdate) >= 0 Then
                strMsgCode = "00022"
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkDateIsTodayFutureDateYesterday(ByVal strFunctCode As String, ByVal dtmInputdate As DateTime) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim nowdtm As New DateTime(Now.Year, Now.Month, Now.Day)

            Dim strSeverity, strMsgCode As String
            strSeverity = "E"

            If DateDiff("d", nowdtm, dtmInputdate) >= -1 Then
                strMsgCode = "00022"
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkDateIsValidBankPaymentDate(ByVal strFunctCode As String, ByVal dtmInputdate As DateTime) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim nowdtm As New DateTime(Now.Year, Now.Month, Now.Day)

            Dim strSeverity, strMsgCode As String
            strSeverity = "E"

            'First Check is a past day
            If DateDiff("d", nowdtm, dtmInputdate) <= 0 Then
                strMsgCode = "00023"
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            'Second check it is Sunday
            Return sm
        End Function

        ''' <summary>
        ''' Check the gridview with any selection
        ''' </summary>
        ''' <param name="gvGrid"></param>
        ''' <param name="strChkboxName"></param>
        ''' <param name="intChkboxColumnIndex"></param>
        ''' <returns>
        ''' With selection: return nothing
        ''' WIthout selection: return 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkGridSelectedNothing(ByVal gvGrid As GridView, ByVal strChkboxName As String, ByVal intChkboxColumnIndex As Integer) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim bIsSelected As Boolean = False
            Dim cb As CheckBox

            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"

            For Each row As GridViewRow In gvGrid.Rows
                cb = CType(row.Cells(intChkboxColumnIndex).FindControl(strChkboxName), CheckBox)
                If cb.Checked = True Then
                    bIsSelected = True
                    Exit For
                End If
            Next

            If Not bIsSelected Then
                strMsgCode = "00023"
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the field of Address
        ''' </summary>
        ''' <param name="strOriBuilding"></param>
        ''' <param name="strOriDistrict"></param>
        ''' <param name="strOriArea"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkAddress(ByVal strOriBuilding As String, ByVal strOriDistrict As String, ByVal strOriArea As String) As ComObject.SystemMessage
            Dim blnRes As Boolean
            Dim blnDistrict As Boolean = False
            Dim blnOtherChar As Boolean = False
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim i As Integer = 0
            Dim c As Char

            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'If IsEmpty(strOriDistrict) OrElse strOriDistrict.Equals(".H") OrElse strOriDistrict.Equals(".K") OrElse strOriDistrict.Equals(".N") Then
            If IsEmpty(strOriDistrict) OrElse strOriDistrict.StartsWith(".") Then
                blnDistrict = True
            End If
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            If IsEmpty(strOriBuilding) And blnDistrict And IsEmpty(strOriArea) Then
                blnRes = False
                strMsgCode = "00008"
            Else
                If IsEmpty(strOriBuilding) Or blnDistrict Or IsEmpty(strOriArea) Then
                    blnRes = False
                    strMsgCode = "00009"
                Else
                    'blnRes = True
                    For i = 1 To strOriBuilding.Length
                        c = GetChar(strOriBuilding, i)
                        If Char.GetUnicodeCategory(c) = System.Globalization.UnicodeCategory.OtherLetter Then
                            blnOtherChar = True
                            strMsgCode = "00009"
                        End If
                    Next

                    If Not blnOtherChar Then
                        blnRes = True
                    End If
                End If

            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check re-enter Email
        ''' </summary>
        ''' <param name="strOriEmail"></param>
        ''' <param name="strOriConfirmEmail"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkConfirmEmail(ByVal strOriEmail As String, ByVal strOriConfirmEmail As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00010"

            If strOriEmail.Equals(strOriConfirmEmail) Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Contact No.
        ''' </summary>
        ''' <param name="strOriContactNo"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkContactNo(ByVal strOriContactNo As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00011"

            If IsEmpty(strOriContactNo) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Name of Practice
        ''' </summary>
        ''' <param name="strOriPracticeName"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkPracticeName(ByVal strOriPracticeName As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00012"

            If IsEmpty(strOriPracticeName) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Address of Practice
        ''' </summary>
        ''' <param name="strOriBuilding"></param>
        ''' <param name="strOriDistrict"></param>
        ''' <param name="strOriArea"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkPracticeAddress(ByVal strOriBuilding As String, ByVal strOriDistrict As String, ByVal strOriArea As String) As ComObject.SystemMessage
            Dim blnRes As Boolean
            Dim blnDistrict As Boolean = False
            Dim blnOtherChar As Boolean = False
            Dim i As Integer = 0
            Dim c As Char

            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'If IsEmpty(strOriDistrict) OrElse strOriDistrict.Equals(".H") OrElse strOriDistrict.Equals(".K") OrElse strOriDistrict.Equals(".N") Then
            If IsEmpty(strOriDistrict) OrElse strOriDistrict.StartsWith(".") Then
                blnDistrict = True
            End If
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            If IsEmpty(strOriBuilding) And blnDistrict And IsEmpty(strOriArea) Then
                blnRes = False
                strMsgCode = "00024"
            Else
                If IsEmpty(strOriBuilding) Or blnDistrict Or IsEmpty(strOriArea) Then
                    blnRes = False
                    strMsgCode = "00025"
                Else
                    'blnRes = True
                    For i = 1 To strOriBuilding.Length
                        c = GetChar(strOriBuilding, i)
                        If Char.GetUnicodeCategory(c) = System.Globalization.UnicodeCategory.OtherLetter Then
                            blnOtherChar = True
                            strMsgCode = "00025"
                        End If
                    Next

                    If Not blnOtherChar Then
                        blnRes = True
                    End If
                End If

            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Type of Practice
        ''' </summary>
        ''' <param name="strOriPracticeType"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkPracticeType(ByVal strOriPracticeType As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00013"

            If IsEmpty(strOriPracticeType) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Health Profession
        ''' </summary>
        ''' <param name="strOriHealthProf"></param>
        ''' <returns>
        ''' True: None Empty String
        ''' False: Empty String
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkHealthProf(ByVal strOriHealthProf As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00014"

            If IsEmpty(strOriHealthProf) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Professional Registration No.
        ''' </summary>
        ''' <param name="strOriRegCode"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkRegCode(ByVal strOriRegCode As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00015"

            If IsEmpty(strOriRegCode) Or HasLowerCharacter(strOriRegCode) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function HasLowerCharacter(ByVal strText As String) As Boolean
            Dim blnFound As Boolean = False

            If Not IsNothing(strText) Then
                Dim chrTemp() As Char = strText.ToCharArray
                Dim intasc As Integer = 0
                Dim i As Integer = 0

                While i < strText.Length And Not blnFound
                    intasc = Asc(chrTemp(i))
                    If (Asc("a") <= intasc And intasc <= Asc("z")) Then
                        blnFound = True
                    End If
                    i = i + 1
                End While
            End If

            Return blnFound
        End Function

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        Public Function chkBRNo(ByVal strBRNo As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00067"

            If IsEmpty(strBRNo) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function


        ''' <summary>
        ''' Check the feild of Bank Name
        ''' </summary>
        ''' <param name="strBankName"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkBankName(ByVal strBankName As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00016"

            If IsEmpty(strBankName) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Branch Name
        ''' </summary>
        ''' <param name="strBranchName"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkBranchName(ByVal strBranchName As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00017"

            If IsEmpty(strBranchName) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the feild of Bank Account No.
        ''' </summary>
        ''' <param name="strBankCode"></param>
        ''' <param name="strBranchCode"></param>
        ''' <param name="strAccNo"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        ''' CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Function chkBankAccount(ByVal strBankCode As String, ByVal strBranchCode As String, ByVal strAccNo As String,
                                       ByVal strAccNoFreeText As String, ByVal blnFreeText As Boolean) As ComObject.SystemMessage
            'Public Function chkBankAccount(ByVal strBankCode As String, ByVal strBranchCode As String, ByVal strAccNo As String) As ComObject.SystemMessage
            Dim blnRes As Boolean = True
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            If blnFreeText Then
                If IsEmpty(strAccNoFreeText) Then
                    blnRes = False
                    strMsgCode = "00018"
                ElseIf Not Regex.IsMatch(strAccNoFreeText, "^(\d[-]?)*\d$") Then
                    blnRes = False
                    strMsgCode = "00019"
                End If
            Else
                If IsEmpty(strBankCode) And IsEmpty(strBranchCode) And IsEmpty(strAccNo) Then
                    blnRes = False
                    strMsgCode = "00018"
                Else
                    'If strBankCode.Length <> 3 Then
                    If Not Regex.IsMatch(strBankCode, "^\d{3}$") Then
                        blnRes = False
                        strMsgCode = "00019"
                    Else
                        'If strBranchCode.Length <> 3 Then
                        If Not Regex.IsMatch(strBranchCode, "^\d{3}$") Then
                            blnRes = False
                            strMsgCode = "00019"
                        Else
                            'If strAccNo.Length = 0 Then
                            If Not Regex.IsMatch(strAccNo, "^\d+$") Then
                                blnRes = False
                                strMsgCode = "00019"
                            End If
                        End If
                    End If
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check the feild of Name of Bank Account Holder
        ''' </summary>
        ''' <param name="strBankOwner"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkBankOwner(ByVal strBankOwner As String, ByVal intMaxLength As Integer, Optional ByVal blnFreeText As Boolean = False) As ComObject.SystemMessage
            'Public Function chkBankOwner(ByVal strBankOwner As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage = Nothing
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            'strMsgCode = "00020"

            If blnFreeText Then
                If IsEmpty(strBankOwner) Then
                    strMsgCode = "00020"
                    sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
                End If
            Else
                If IsEmpty(strBankOwner) Then
                    strMsgCode = "00020"
                    sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)

                End If

                If sm Is Nothing AndAlso chkNonAlphaNumeric(strBankOwner) Then
                    strMsgCode = "00383"
                    sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
                End If

                ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If sm Is Nothing AndAlso strBankOwner.Length > 300 Then
                    strMsgCode = "00408"
                    sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
                End If
                ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]
            End If

            Return sm
        End Function

        Public Function chkNonAlphaNumeric(ByVal str As String) As Boolean

            Return Regex.IsMatch(str, "[^a-zA-Z0-9\ \~\!\@\#\$\%\^\&\*\(\)_\+\`\-\=\[\]\\\{\}\|\;\'\:\<\>\?\,\.\/\""]")

        End Function
        'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]


        ''' <summary>
        ''' Check the field of Are you already join the HA PPI ePR Program
        ''' </summary>
        ''' <param name="strHadJoinedEHRSS"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkHadJoinedEHRSS(ByVal strHadJoinedEHRSS) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00026"

            If IsEmpty(strHadJoinedEHRSS) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        ''' <summary>
        ''' Check the field of Would you like to join PCD
        ''' </summary>
        ''' <param name="strWillJoinPCD"></param>
        ''' <returns>
        ''' Nothing: Valid
        ''' Is not nothing: Invalid 
        ''' </returns>
        ''' <remarks></remarks>
        Public Function chkWillJoinPCD(ByVal strWillJoinPCD As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00315"

            If IsEmpty(strWillJoinPCD) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkServiceDate(ByVal strOriServiceDate As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty
            strMsgCode = "00119"
            'INT16-0028 (Fix the Service Date validation in Claim functions) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim dtmDate As DateTime
            'INT16-0028 (Fix the Service Date validation in Claim functions) [End][Chris YIM]

            If IsEmpty(strOriServiceDate) Then
                blnRes = False
                strMsgCode = "00119"
            Else
                'If strOriServiceDate.Length = 8 Then
                '    strOriServiceDate = strOriServiceDate.Substring(0, 2) + "-" + strOriServiceDate.Substring(2, 2) + "-" + strOriServiceDate.Substring(4, 4)
                'End If

                'INT16-0028 (Fix the Service Date validation in Claim functions) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If Not IsDate(formatter.convertDate(strOriServiceDate, "E")) Then
                If Not DateTime.TryParseExact(strOriServiceDate, formatter.EnterDateFormat, Nothing, DateTimeStyles.None, dtmDate) Then
                    'INT16-0028 (Fix the Service Date validation in Claim functions) [End][Chris YIM]

                    blnRes = False
                    strMsgCode = "00120"
                Else
                    sm = chkDateIsFutureDate(CDate(formatter.convertDate(strOriServiceDate, "E")))
                    If sm Is Nothing Then
                        blnRes = True
                        strMsgCode = String.Empty
                    Else
                        blnRes = False
                        strMsgCode = "00121"
                    End If
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        Public Function chkVoucherRedeem(ByVal intSelectedIndex As Integer, ByVal strEnterValue As String, ByVal intAvailVoucher As Integer) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim intEnterValue As Integer = 0
            Dim blnRes As Boolean = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            If (intSelectedIndex < 0) Or (intSelectedIndex > 4 And strEnterValue.Equals(String.Empty)) Then
                blnRes = False
                strMsgCode = "00122"
            Else
                If intSelectedIndex > 4 Then
                    Try
                        intEnterValue = CInt(strEnterValue)
                        If intEnterValue > intAvailVoucher Then
                            blnRes = False
                            strMsgCode = "00123"
                        End If
                    Catch ex As Exception
                        blnRes = False
                        strMsgCode = "00124"
                    End Try
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function


        Public Function chkClaimBankAccount(ByVal strOriBankAcctKey As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim intEnterValue As Integer = 0
            Dim blnRes As Boolean = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty
            If IsEmpty(strOriBankAcctKey) Then
                blnRes = False
                strMsgCode = "00125"
            Else
                blnRes = True
                strMsgCode = String.Empty
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Function chkReasonForVisit(ByVal strProfCode As String, ByVal strOriReasonForVisit1 As String, ByVal strOriReasonForVisit2 As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim intEnterValue As Integer = 0
            Dim blnRes As Boolean = True
            Dim generalFunction As ComFunction.GeneralFunction = New ComFunction.GeneralFunction
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            Dim udtReasonForVisitBLL As New ReasonForVisit.ReasonForVisitBLL()
            Dim dtReasonForVisit As New DataTable()
            If Not String.IsNullOrEmpty(strOriReasonForVisit1) AndAlso Not String.IsNullOrEmpty(strOriReasonForVisit2) Then
                dtReasonForVisit = udtReasonForVisitBLL.getReasonForVisitL2(strProfCode, strOriReasonForVisit1, strOriReasonForVisit2)
            End If

            Dim strIsNewReasonForVisit As String = String.Empty
            generalFunction.getSystemParameter("TurnOnNewReasonForVisit", strIsNewReasonForVisit, "")

            If strIsNewReasonForVisit = "Y" Then
                strOriReasonForVisit2 = ""
            End If

            If String.IsNullOrEmpty(strOriReasonForVisit1) Or strOriReasonForVisit1 = "0" Or _
               String.IsNullOrEmpty(strOriReasonForVisit2) Or strOriReasonForVisit2 = "0" Or _
               dtReasonForVisit.Rows.Count = 0 Then
                blnRes = False
                strMsgCode = "00273"
            Else
                blnRes = True
                strMsgCode = String.Empty
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Function ChkIsIdenticial(ByVal strOriField1 As String, ByVal strOriField2 As String) As Boolean
            Dim blnRes As Boolean = False
            If strOriField1.Equals(strOriField2) Then
                blnRes = True
            Else
                blnRes = False
            End If
            Return (blnRes)
        End Function

        Public Function chkGender(ByVal strSelectedGender As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty


            If strSelectedGender Is Nothing Or strSelectedGender.Equals(String.Empty) Then
                blnRes = False
                strMsgCode = "00034"
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        Public Function chkChiName(ByVal strChiName As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim strErrMsg As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""
            strErrMsg = ""

            If Not IsEmpty(strChiName) Then
                'Transform to byte (4 byte for each char)
                Dim bytChiName As Byte() = System.Text.Encoding.UTF32.GetBytes(strChiName)
                If bytChiName.Length > 24 Then ' Maximum 6 char
                    strMsgCode = "00363"
                End If
            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function
        ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function chkInputDate(ByVal strFunctCode As String, ByVal strOriInputDate As String, ByVal strEmptyMsgCode As String, ByVal strInvalidFormatMsgCode As String, ByVal strFutureDateMsgCode As String) As ComObject.SystemMessage
        '    Dim sm As ComObject.SystemMessage
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        '    Dim strSeverity, strMsgCode As String
        '    Dim blnRes As Boolean
        '    blnRes = True
        '    'strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = String.Empty
        '    strMsgCode = strEmptyMsgCode

        '    If IsEmpty(strOriInputDate) Then
        '        blnRes = False
        '        strMsgCode = strEmptyMsgCode
        '    Else
        '        If strOriInputDate.Length = 10 Then
        '            If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
        '                blnRes = False
        '                strMsgCode = strInvalidFormatMsgCode
        '            Else
        '                sm = chkDateIsFutureDate(CDate(formatter.convertDate(strOriInputDate, "E")))
        '                If sm Is Nothing Then
        '                    blnRes = True
        '                    strMsgCode = String.Empty
        '                Else
        '                    blnRes = False
        '                    strMsgCode = strFutureDateMsgCode
        '                End If
        '            End If
        '        Else
        '            blnRes = False
        '            strMsgCode = strInvalidFormatMsgCode
        '        End If
        '    End If

        '    If blnRes Then
        '        sm = Nothing
        '    Else
        '        sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

        Public Function chkInputValidFromDateCutoffDate(ByVal strFunctCode As String, ByVal strMsgCode As String, ByVal dtmFromDate As Date, ByVal dtmCutoffDate As Date) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strSeverity As String
            Dim blnRes As Boolean
            blnRes = True
            strSeverity = "E"

            If dtmFromDate > dtmCutoffDate Then
                blnRes = False
                'strMsgCode = "00004"
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ' ''' <summary>
        ' ''' Check the input date is empty, a future day or in invalid date format
        ' ''' </summary>
        ' ''' <param name="strOriInputDate"></param>
        ' ''' <param name="strVariable">The date field name</param>
        ' ''' <returns>checking result</returns>
        ' ''' <remarks></remarks>
        'Public Function chkInputDate(ByVal strOriInputDate As String, ByVal strVariable() As String) As ComObject.SystemMessage
        '    Dim sm As ComObject.SystemMessage
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        '    Dim strFunctCode, strSeverity, strMsgCode As String
        '    Dim blnRes As Boolean
        '    blnRes = True
        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = String.Empty
        '    strMsgCode = "00028"

        '    If IsEmpty(strOriInputDate) Then
        '        blnRes = False
        '        strMsgCode = "00028"
        '    Else
        '        If strOriInputDate.Length = 10 Then
        '            If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
        '                blnRes = False
        '                strMsgCode = "00029"
        '            Else
        '                sm = chkDateIsFutureDate(CDate(formatter.convertDate(strOriInputDate, "E")))
        '                If sm Is Nothing Then
        '                    blnRes = True
        '                    strMsgCode = String.Empty
        '                Else
        '                    blnRes = False
        '                    strMsgCode = "00022"
        '                End If
        '            End If
        '        Else
        '            blnRes = False
        '            strMsgCode = "00029"
        '        End If
        '    End If

        '    If blnRes Then
        '        sm = Nothing
        '    Else
        '        sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function
        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function chkInputDate(ByVal strFunctCode As String, ByVal strOriInputDate As String, ByVal strVariable() As String) As ComObject.SystemMessage
        '    Dim sm As ComObject.SystemMessage
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        '    Dim strSeverity, strMsgCode As String
        '    Dim blnRes As Boolean
        '    blnRes = True
        '    'strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = String.Empty
        '    strMsgCode = "00028"

        '    If IsEmpty(strOriInputDate) Then
        '        blnRes = False
        '        strMsgCode = "00028"
        '    Else
        '        If strOriInputDate.Length = 10 Then
        '            If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
        '                blnRes = False
        '                strMsgCode = "00029"
        '            Else
        '                sm = chkDateIsFutureDate(CDate(formatter.convertDate(strOriInputDate, "E")))
        '                If sm Is Nothing Then
        '                    blnRes = True
        '                    strMsgCode = String.Empty
        '                Else
        '                    blnRes = False
        '                    strMsgCode = "00022"
        '                End If
        '            End If
        '        Else
        '            blnRes = False
        '            strMsgCode = "00029"
        '        End If
        '    End If

        '    If blnRes Then
        '        sm = Nothing
        '    Else
        '        sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function
        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function chkInputDate(ByVal strOriInputDate As String, ByVal bByPassFutureDateChecking As Boolean) As ComObject.SystemMessage
        '    Dim sm As ComObject.SystemMessage
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        '    Dim strFunctCode, strSeverity, strMsgCode As String
        '    Dim blnRes As Boolean
        '    blnRes = True
        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = "00028"

        '    If IsEmpty(strOriInputDate) Then
        '        blnRes = False
        '        strMsgCode = "00028"
        '    Else
        '        If strOriInputDate.Length = 10 Then
        '            If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
        '                blnRes = False
        '                strMsgCode = "00029"
        '            ElseIf Not bByPassFutureDateChecking Then
        '                sm = chkDateIsFutureDate(CDate(formatter.convertDate(strOriInputDate, "E")))
        '                If sm Is Nothing Then
        '                    blnRes = True
        '                    strMsgCode = String.Empty
        '                Else
        '                    blnRes = False
        '                    strMsgCode = "00022"
        '                End If
        '            End If
        '        Else
        '            blnRes = False
        '            strMsgCode = "00029"
        '        End If
        '    End If

        '    If blnRes Then
        '        sm = Nothing
        '    Else
        '        sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        Public Function chkInputDate(ByVal strOriInputDate As String, ByVal blnCheckEmpty As Boolean, ByVal blnCheckFutureDate As Boolean) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean = True
            Dim dtmDate As DateTime

            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            '1. Check empty
            If blnCheckEmpty Then
                If IsEmpty(strOriInputDate) Then
                    blnRes = False
                    strMsgCode = "00028"
                End If
            End If

            '2. Check date format
            If blnRes Then
                If Not DateTime.TryParseExact(strOriInputDate, formatter.EnterDateFormat, Nothing, DateTimeStyles.None, dtmDate) Then
                    blnRes = False
                    strMsgCode = "00029"
                End If
            End If

            '3. Check not to be a future date
            If blnRes Then
                If blnCheckFutureDate Then
                    If Not chkDateIsFutureDate(CDate(formatter.convertDate(strOriInputDate, "E"))) Is Nothing Then
                        blnRes = False
                        strMsgCode = "00022"
                    End If
                End If
            End If

            '4. Check minimum and maximum date to prevent overflow in SQL with data type - datetime
            If blnRes Then
                If CDate(formatter.convertDate(strOriInputDate, "E")).Year < DateValidation.YearMinValue _
                    Or CDate(formatter.convertDate(strOriInputDate, "E")).Year > DateValidation.YearMaxValue Then
                    blnRes = False
                    strMsgCode = "00029"
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

        Public Function chkBankPaymentDate(ByVal strFunctCode As String, ByVal strOriInputDate As String, ByVal strVariable() As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True

            strSeverity = "E"
            strMsgCode = String.Empty
            strMsgCode = "00028"

            If IsEmpty(strOriInputDate) Then
                blnRes = False
                strMsgCode = "00028"
            Else
                If strOriInputDate.Length = 10 Then
                    If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
                        blnRes = False
                        strMsgCode = "00029"
                    Else
                        sm = chkDateIsValidBankPaymentDate(strFunctCode, CDate(formatter.convertDate(strOriInputDate, "E")))
                        If sm Is Nothing Then
                            blnRes = True
                            strMsgCode = String.Empty
                        Else
                            blnRes = False
                            strMsgCode = "00022"
                        End If
                    End If
                Else
                    blnRes = False
                    strMsgCode = "00029"
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        'Public Function chkInputDateIsTodayOrFutureDateClark(ByVal strFunctCode As String, ByVal strOriInputDate As String) As ComObject.SystemMessage
        '    Dim sm As ComObject.SystemMessage
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        '    Dim strSeverity, strMsgCode As String
        '    Dim blnRes As Boolean
        '    blnRes = True
        '    strSeverity = "E"
        '    strMsgCode = String.Empty
        '    strMsgCode = "00028"

        '    If IsEmpty(strOriInputDate) Then
        '        blnRes = False
        '        strMsgCode = "00028"
        '    Else
        '        If strOriInputDate.Length = 10 Then
        '            If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
        '                blnRes = False
        '                strMsgCode = "00029"
        '            Else
        '                sm = chkDateIsTodayFutureDate(strFunctCode, CDate(formatter.convertDate(strOriInputDate, "E")))
        '                If sm Is Nothing Then
        '                    blnRes = True
        '                    strMsgCode = String.Empty
        '                Else
        '                    blnRes = False
        '                    strMsgCode = "00022"
        '                End If
        '            End If
        '        Else
        '            blnRes = False
        '            strMsgCode = "00029"
        '        End If
        '    End If

        '    If blnRes Then
        '        sm = Nothing
        '    Else
        '        sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        Public Function chkInputDateIsValidReimbursementCutoffDate(ByVal strFunctCode As String, ByVal strOriInputDate As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strSeverity = "E"
            strMsgCode = String.Empty
            strMsgCode = "00028"

            If IsEmpty(strOriInputDate) Then
                blnRes = False
                strMsgCode = "00028"
            Else
                If strOriInputDate.Length = 10 Then
                    If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
                        blnRes = False
                        strMsgCode = "00029"
                    Else
                        sm = chkDateIsTodayFutureDateYesterday(strFunctCode, CDate(formatter.convertDate(strOriInputDate, "E")))
                        If sm Is Nothing Then
                            blnRes = True
                            strMsgCode = String.Empty
                        Else
                            blnRes = False
                            strMsgCode = "00022"
                        End If
                    End If
                Else
                    blnRes = False
                    strMsgCode = "00029"
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function


        Public Function chkSearchStructureAddress(ByVal strbuilding As String) As ComObject.SystemMessage
            Dim blnRes As Boolean
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            If IsEmpty(strbuilding) Then
                blnRes = False
                strMsgCode = "00035"
            Else
                Dim intMinWord As Integer
                Dim strvalue As String = String.Empty
                udtcomfunct.getSystemParameter("MinWordForSearchAddress", strvalue, String.Empty)
                intMinWord = CInt(strvalue)

                If strbuilding.Trim.Length < intMinWord Then
                    blnRes = False
                    strMsgCode = "00036"
                Else
                    blnRes = True
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm

        End Function

        Public Function chkPracticeSearchStructureAddress(ByVal strbuilding As String) As ComObject.SystemMessage
            Dim blnRes As Boolean
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            If IsEmpty(strbuilding) Then
                blnRes = False
                strMsgCode = "00037"
            Else
                Dim intMinWord As Integer
                Dim strvalue As String = String.Empty
                udtcomfunct.getSystemParameter("MinWordForSearchAddress", strvalue, String.Empty)
                intMinWord = CInt(strvalue)

                If strbuilding.Trim.Length < intMinWord Then
                    blnRes = False
                    strMsgCode = "00038"
                Else
                    blnRes = True
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm

        End Function

        Public Function chkSelectedBankAccount(ByVal strOriSelectedPractice As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim blnErr As Boolean = False

            blnErr = IsEmpty(strOriSelectedPractice)

            If blnErr Then
                sm = New ComObject.SystemMessage("990000", "E", "00040")
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkSelectedPrintFormOption(ByVal strSelectedPrintFormOption As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim blnErr As Boolean = False

            blnErr = IsEmpty(strSelectedPrintFormOption)

            If blnErr Then
                sm = New ComObject.SystemMessage("990000", "E", "00147")
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkCCCode_UsingDDL(ByVal strcccode1 As String, ByVal strcccode2 As String, ByVal strcccode3 As String, ByVal strcccode4 As String, ByVal strcccode5 As String, ByVal strcccode6 As String) As ComObject.SystemMessage
            strcccode1 = strcccode1.Trim()
            strcccode2 = strcccode2.Trim()
            strcccode3 = strcccode3.Trim()
            strcccode4 = strcccode4.Trim()
            strcccode5 = strcccode5.Trim()
            strcccode6 = strcccode6.Trim()

            If strcccode1.Length = 0 AndAlso strcccode2.Length = 0 AndAlso strcccode3.Length = 0 AndAlso strcccode4.Length = 0 AndAlso strcccode5.Length = 0 AndAlso strcccode6.Length = 0 Then
                'blnErr = True
            Else
                If Not strcccode1.Equals(String.Empty) Then
                    If strcccode1.Length() < 5 Then
                        Return New ComObject.SystemMessage("990000", "E", "00039")
                    End If
                End If


                If Not strcccode2.Equals(String.Empty) Then
                    If strcccode2.Length() < 5 Then
                        Return New ComObject.SystemMessage("990000", "E", "00039")
                    Else
                        If strcccode1.Trim.Equals(String.Empty) Then
                            Return New ComObject.SystemMessage("990000", "E", "00039")
                        End If
                    End If
                End If

                If Not strcccode3.Equals(String.Empty) Then
                    If strcccode3.Length() < 5 Then
                        Return New ComObject.SystemMessage("990000", "E", "00039")
                    Else
                        If strcccode2.Trim.Equals(String.Empty) Then
                            Return New ComObject.SystemMessage("990000", "E", "00039")
                        End If
                    End If
                End If

                If Not strcccode4.Equals(String.Empty) Then
                    If strcccode4.Length() < 5 Then
                        Return New ComObject.SystemMessage("990000", "E", "00039")
                    Else
                        If strcccode3.Trim.Equals(String.Empty) Then
                            Return New ComObject.SystemMessage("990000", "E", "00039")
                        End If
                    End If
                End If

                If Not strcccode5.Equals(String.Empty) Then
                    If strcccode5.Length() < 5 Then
                        Return New ComObject.SystemMessage("990000", "E", "00039")
                    Else
                        If strcccode4.Trim.Equals(String.Empty) Then
                            Return New ComObject.SystemMessage("990000", "E", "00039")
                        End If
                    End If
                End If

                If Not strcccode6.Equals(String.Empty) Then
                    If strcccode6.Length() < 5 Then
                        Return New ComObject.SystemMessage("990000", "E", "00039")
                    Else
                        If strcccode5.Trim.Equals(String.Empty) Then
                            Return New ComObject.SystemMessage("990000", "E", "00039")
                        End If
                    End If
                End If
            End If

            Return Nothing

        End Function

        Public Function chkCCCode(ByVal strcccode1 As String, ByVal strchar1 As String, _
                                ByVal strcccode2 As String, ByVal strchar2 As String, _
                                ByVal strcccode3 As String, ByVal strchar3 As String, _
                                ByVal strcccode4 As String, ByVal strchar4 As String, _
                                ByVal strcccode5 As String, ByVal strchar5 As String, _
                                ByVal strcccode6 As String, ByVal strchar6 As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim blnErr As Boolean = False


            If strcccode1.Length() > 0 And strcccode1.Length() < 5 Then
                blnErr = True
            ElseIf strcccode1.Length() = 5 And strchar1.Equals(String.Empty) Then
                blnErr = True
            End If

            If strcccode2.Length() > 0 And strcccode2.Length() < 5 Then
                blnErr = True
            ElseIf strcccode2.Length() = 5 And strchar2.Equals(String.Empty) Then
                blnErr = True
            End If

            If strcccode3.Length() > 0 And strcccode3.Length() < 5 Then
                blnErr = True
            ElseIf strcccode3.Length() = 5 And strchar3.Equals(String.Empty) Then
                blnErr = True
            End If

            If strcccode4.Length() > 0 And strcccode4.Length() < 5 Then
                blnErr = True
            ElseIf strcccode4.Length() = 5 And strchar4.Equals(String.Empty) Then
                blnErr = True
            End If

            If strcccode5.Length() > 0 And strcccode5.Length() < 5 Then
                blnErr = True
            ElseIf strcccode5.Length() = 5 And strchar5.Equals(String.Empty) Then
                blnErr = True
            End If

            If strcccode6.Length() > 0 And strcccode6.Length() < 5 Then
                blnErr = True
            ElseIf strcccode6.Length() = 5 And strchar6.Equals(String.Empty) Then
                blnErr = True
            End If

            If blnErr Then
                sm = New ComObject.SystemMessage("990000", "E", "00039")
            Else
                sm = Nothing
            End If

            Return sm

        End Function

        Public Function chkIsValidDateChar(ByVal strOriString As String) As Boolean
            Dim blnRes As Boolean = True
            Dim chrChar As Char()
            Dim i As Integer
            chrChar = strOriString.ToCharArray()
            For i = 0 To strOriString.Length - 1
                If (Asc(chrChar(i)) < 48 Or Asc(chrChar(i)) > 57) And Asc(chrChar(i)) <> 45 Then
                    blnRes = False
                End If
            Next

            Return blnRes

        End Function

#Region "Profession Verification"
        Public Function chkSelectedProfession(ByVal strProfession As String) As Boolean
            Dim blnSelected As Boolean = False

            If strProfession Is Nothing OrElse strProfession.Trim() = "" Then
                blnSelected = False
            Else
                blnSelected = True
            End If

            Return blnSelected
        End Function

        Public Function chkValidProfessionReferenceNo(ByVal strReferenceNum As String) As Boolean
            Dim blnValid As Boolean = True
            If strReferenceNum = "" Then blnValid = False
            ' To Do: Validate the Format By Rules

            Return blnValid
        End Function

        Public Function chkValidProfessionStatusResult(ByVal strResult As String) As Boolean
            Dim blnValid As Boolean = True

            If Not (strResult.ToUpper() = Component.HealthProfVerifyStatus.Invalid Or strResult.ToUpper() = Component.HealthProfVerifyStatus.Suspect Or strResult.ToUpper() = Component.HealthProfVerifyStatus.Validated) Then
                blnValid = False
            End If
            Return blnValid
        End Function

        Public Function chkExportListSearchFromToDate(ByVal strFromText As String, ByVal strToText As String) As List(Of ComObject.SystemMessage)

            Dim strFunctionCode As String = "010103"
            Dim lstSysMsg As New List(Of ComObject.SystemMessage)
            Dim udtFormatter As Common.Format.Formatter = New Common.Format.Formatter

            ' Allow Empty Input

            Dim blnIsDateFrom As Boolean = False
            Dim blnIsDateTo As Boolean = False

            If strFromText.Trim() <> "" Then blnIsDateFrom = IsDate(udtFormatter.convertDate(strFromText.Trim(), "E"))

            'dtmOriInputDate = formatter.convertDate(strFormattedInputDate, "E")
            '    End If
            'End If

            'If strMessageCode.Equals(String.Empty) Then
            '    If dtmOriInputDate.Year < 1753 Or dtmOriInputDate > Date.MaxValue Then
            '        'The input Date is out of range. (year < 1753 or date > 31 DEC 9999)
            '        strMessageCode = Common.Component.MsgCode.MSG00102
            '    End If
            'End If

            If blnIsDateFrom Then
                Dim dtmInputFromDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(strFromText.Trim(), "E"))
                'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If dtmInputFromDate.Year < 1753 Or dtmInputFromDate > Date.MaxValue Then
                If dtmInputFromDate.Year < DateValidation.YearMinValue Or dtmInputFromDate.Year > DateValidation.YearMaxValue Then
                    blnIsDateFrom = False
                End If
                'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
            End If

            If strToText.Trim() <> "" Then blnIsDateTo = IsDate(udtFormatter.convertDate(strToText.Trim(), "E"))
            If blnIsDateTo Then
                Dim dtmInputToDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(strToText.Trim(), "E"))
                'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If dtmInputToDate.Year < 1753 Or dtmInputToDate > Date.MaxValue Then
                If dtmInputToDate.Year < DateValidation.YearMinValue Or dtmInputToDate.Year > DateValidation.YearMaxValue Then
                    blnIsDateTo = False
                End If
                'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
            End If

            If strFromText.Trim() <> "" AndAlso blnIsDateFrom = False Then
                ' Invalid From Date Format
                lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00014"))
            End If

            If strToText.Trim() <> "" AndAlso blnIsDateTo = False Then
                ' Invalid To Date Format
                lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00015"))
            End If

            If strFromText.Trim() <> "" AndAlso blnIsDateFrom Then
                ' Future From Date
                If DateTime.Now < CDate(udtFormatter.convertDate(strFromText.Trim(), "E")) Then
                    lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00016"))
                End If
            End If

            If strToText.Trim() <> "" AndAlso blnIsDateTo Then
                ' Future To Date
                If DateTime.Now < CDate(udtFormatter.convertDate(strToText.Trim(), "E")) Then
                    lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00017"))
                End If
            End If

            ' Later From Date than To Date
            If strFromText.Trim() <> "" AndAlso blnIsDateFrom AndAlso strToText.Trim() <> "" AndAlso blnIsDateTo Then
                If CDate(udtFormatter.convertDate(strFromText.Trim(), "E")) > CDate(udtFormatter.convertDate(strToText.Trim(), "E")) Then
                    lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00018"))
                End If
            End If

            Return lstSysMsg

        End Function
#End Region

        Public Function chkSystemNumber(ByVal strSystemNumber As String) As Boolean
            Dim strSystemNumberComponents As String() = strSystemNumber.Trim().Split("-")
            Dim blnResult As Boolean = True

            If strSystemNumberComponents.Length <> 3 Then
                If strSystemNumberComponents.Length <> 4 Then
                    blnResult = False
                End If
            ElseIf Not (strSystemNumberComponents(0).Length = 6 OrElse strSystemNumberComponents(0).Length = 7) OrElse strSystemNumberComponents(2).Length <> 1 Then
                blnResult = False
            End If
            Return blnResult
        End Function

        Public Function chkNumberWithRange(ByVal intNumber As Integer, ByVal intUpperLimit As Integer, ByVal intLowerLimit As Integer) As Boolean
            If intNumber < intLowerLimit Or intNumber > intUpperLimit Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function ValidatePassword(ByVal pwd As String, Optional ByVal IsWithToken As Boolean = True) As Boolean

            Dim intMinLength, intMaxLength, intRuleNum As Integer
            Dim strvalue1, strvalue2 As String
            strvalue1 = String.Empty
            strvalue2 = String.Empty
            udtcomfunct.getSystemParameter("PasswordLengthRange", strvalue1, strvalue2)
            intMinLength = CInt(strvalue1)
            intMaxLength = CInt(strvalue2)

            strvalue1 = String.Empty
            strvalue2 = String.Empty
            udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

            If IsWithToken Then
                intRuleNum = CInt(strvalue1)
            Else
                intRuleNum = CInt(strvalue2)
            End If

            ' Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
            Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
            Dim number As New System.Text.RegularExpressions.Regex("[0-9]")

            ' Special is "none of the above".
            Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

            Dim okCounter As Integer
            okCounter = 0

            ' Check the length.
            If Len(pwd) < intMinLength Then Return False
            If Len(pwd) > intMaxLength Then Return False

            ' Check for minimum number of occurrences.
            If upper.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
            If lower.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
            If number.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
            If special.Matches(pwd).Count > 0 Then okCounter = okCounter + 1

            ' Passed all checks.
            Return (okCounter >= intRuleNum)

        End Function

        Public Function ValidateIVRSPassword(ByVal strPW As String) As Boolean
            Dim intMinLength, intMaxLength As Integer
            Dim strvalue1, strvalue2 As String
            strvalue1 = String.Empty
            strvalue2 = String.Empty
            udtcomfunct.getSystemParameter("IVRSPasswordLengthRange", strvalue1, strvalue2)
            intMinLength = CInt(strvalue1)
            intMaxLength = CInt(strvalue2)

            If IsEmpty(strPW) Then
                ValidateIVRSPassword = False
            ElseIf Not IsNumeric(strPW) Then
                ValidateIVRSPassword = False
            ElseIf Trim(strPW).Length < intMinLength Then
                ValidateIVRSPassword = False
            ElseIf Trim(strPW).Length > intMaxLength Then
                ValidateIVRSPassword = False
            Else
                ValidateIVRSPassword = True
            End If
        End Function

        Public Function ValidateAlias(ByVal strUsername As String) As Boolean
            Dim intMinLength, intMaxLength As Integer
            Dim strvalue1, strvalue2 As String
            strvalue1 = String.Empty
            strvalue2 = String.Empty
            udtcomfunct.getSystemParameter("UsernameLengthRange", strvalue1, strvalue2)
            intMinLength = CInt(strvalue1)
            intMaxLength = CInt(strvalue2)

            'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence]
            Dim strSPAliasRegEx As String = String.Empty

            udtcomfunct.getSystemParameter("LoginIDRegEx", strSPAliasRegEx, String.Empty)

            ''Do not allow space in the username
            'If strUsername.Contains(" ") Then Return False
            'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]

            If IsEmpty(strUsername) Then
                ValidateAlias = False
            ElseIf Trim(strUsername).Length < intMinLength Then
                ValidateAlias = False
            ElseIf Trim(strUsername).Length > intMaxLength Then
                ValidateAlias = False

                'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence]
            ElseIf (New Regex(strSPAliasRegEx)).IsMatch(strUsername) = False Then
                ValidateAlias = False
                'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]

            Else
                ValidateAlias = True
            End If
        End Function

        'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence]
        Public Function ValidateDataEntryUsername(ByVal strUsername As String) As Boolean
            Dim intMinLength, intMaxLength As Integer
            Dim strvalue1, strvalue2 As String
            strvalue1 = String.Empty
            strvalue2 = String.Empty
            udtcomfunct.getSystemParameter("UsernameLengthRange", strvalue1, strvalue2)
            intMinLength = CInt(strvalue1)
            intMaxLength = CInt(strvalue2)

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Revise validation and synchronize function with [Function ValidateAlias]
            'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence]
            Dim strSPAliasRegEx As String = String.Empty

            udtcomfunct.getSystemParameter("LoginIDRegEx", strSPAliasRegEx, String.Empty)

            ''Do not allow space in the username
            'If strUsername.Contains(" ") Then Return False
            'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If IsEmpty(strUsername) Then
                Return False
            ElseIf Trim(strUsername).Length < intMinLength Then
                Return False
            ElseIf Trim(strUsername).Length > intMaxLength Then
                Return False

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Revise validation and synchronize function with [Function ValidateAlias]
                'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence]
            ElseIf (New Regex(strSPAliasRegEx)).IsMatch(strUsername) = False Then
                Return False
                'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

            Return True

        End Function
        'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]

        Public Function ValidateFileDownloadPassword(ByVal pwd As String) As Boolean

            Dim intMinLength, intMaxLength, intRuleNum As Integer
            Dim strvalue1, strvalue2 As String
            strvalue1 = String.Empty
            strvalue2 = String.Empty
            udtcomfunct.getSystemParameter("FilePasswordLengthRange", strvalue1, strvalue2)
            intMinLength = CInt(strvalue1)
            intMaxLength = CInt(strvalue2)

            strvalue1 = String.Empty
            strvalue2 = String.Empty
            udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)
            intRuleNum = CInt(strvalue2)


            ' Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
            Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
            Dim number As New System.Text.RegularExpressions.Regex("[0-9]")

            ' Special is "none of the above".
            Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

            Dim okCounter As Integer
            okCounter = 0

            ' Check the length.
            If Len(pwd) < intMinLength Then Return False
            If Len(pwd) > intMaxLength Then Return False

            ' Check for minimum number of occurrences.
            If upper.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
            If lower.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
            If number.Matches(pwd).Count > 0 Then okCounter = okCounter + 1
            If special.Matches(pwd).Count > 0 Then okCounter = okCounter + 1

            ' Passed all checks.
            Return (okCounter >= intRuleNum)

        End Function

        Public Function chkOptionalInputDate(ByVal strFunctCode As String, ByVal strOriInputDate As String, ByVal strInvalidFormatMsgCode As String, ByVal strFutureDateMsgCode As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strSeverity = "E"
            strMsgCode = String.Empty

            If Not strOriInputDate.Trim.Equals(String.Empty) Then
                If strOriInputDate.Length = 10 Then
                    If Not IsDate(formatter.convertDate(strOriInputDate, "E")) Then
                        blnRes = False
                        strMsgCode = strInvalidFormatMsgCode
                    Else
                        sm = chkDateIsFutureDate(CDate(formatter.convertDate(strOriInputDate, "E")))
                        If sm Is Nothing Then
                            blnRes = True
                            strMsgCode = String.Empty
                        Else
                            blnRes = False
                            strMsgCode = strFutureDateMsgCode
                        End If
                    End If
                Else
                    blnRes = False
                    strMsgCode = strInvalidFormatMsgCode
                End If
            Else
                blnRes = True
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        Public Function chkRangeOfServiceFee(ByVal strServiceFeeFrom As String, ByVal strServiceFeeTo As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            If IsEmpty(strServiceFeeFrom) And IsEmpty(strServiceFeeTo) Then
                strMsgCode = "00069"
                blnRes = False
            Else
                If IsEmpty(strServiceFeeFrom) Or IsEmpty(strServiceFeeTo) Then
                    strMsgCode = "00070"
                    blnRes = False
                Else
                    If CInt(strServiceFeeTo) < CInt(strServiceFeeFrom) Then
                        strMsgCode = "00070"
                        blnRes = False
                    End If
                End If

            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm

        End Function

        Public Function chkMOEnglishName(ByVal strOriEnglishMOName As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00168"

            If IsEmpty(strOriEnglishMOName) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        'Public Function chkMOChinesehName(ByVal strOriChineseMOName As String) As ComObject.SystemMessage
        '    Dim sm As ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String
        '    strFunctCode = "020101"
        '    strSeverity = "E"
        '    strMsgCode = "00004"

        '    If IsEmpty(strOriChineseMOName) Then
        '        sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    Else
        '        sm = Nothing
        '    End If

        '    Return sm
        'End Function

        Public Function chkMOBRCode(ByVal strOriMOBRCode As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00164"

            If IsEmpty(strOriMOBRCode) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkMOEmail(ByVal strMOEmail As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode As String
            strFunctCode = "990000"

            sm = chkEmailAddress(strMOEmail)

            If Not IsNothing(sm) Then
                If sm.MessageCode = "00005" Then
                    sm.MessageCode = "00166"
                ElseIf sm.MessageCode = "00006" Then
                    sm.MessageCode = "00167"
                End If
                sm.FunctionCode = strFunctCode
            End If

            Return sm
        End Function

        Public Function chkMOContactNo(ByVal strOriMOContactNo As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00165"

            If IsEmpty(strOriMOContactNo) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        'Public Function chkMOFax(ByVal strOriMOFax As String) As ComObject.SystemMessage
        '    Dim sm As ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String
        '    strFunctCode = "020101"
        '    strSeverity = "E"
        '    strMsgCode = "00010"

        '    If IsEmpty(strOriMOFax) Then
        '        sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    Else
        '        sm = Nothing
        '    End If

        '    Return sm
        'End Function

        Public Function chkMOAddress(ByVal strOriBuilding As String, ByVal strOriDistrict As String, ByVal strOriArea As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode As String
            strFunctCode = "990000"

            sm = chkAddress(strOriBuilding, strOriDistrict, strOriArea)

            If Not IsNothing(sm) Then
                If sm.MessageCode = "00008" Then
                    sm.MessageCode = "00162"
                ElseIf sm.MessageCode = "00009" Then
                    sm.MessageCode = "00163"
                End If

                sm.FunctionCode = strFunctCode
            End If

            Return sm
        End Function

        Public Function chkMORelation(ByVal strOriMORelation As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00169"

            If IsEmpty(strOriMORelation) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkPracticeTel(ByVal strOriPracticeTel As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00170"

            If IsEmpty(strOriPracticeTel) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkServiceFee(ByVal strOriServiceFee As String) As ComObject.SystemMessage
            Dim blnRes As Boolean = False
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity As String
            Dim strMsgCode As String = String.Empty
            'strFunctCode = "020101"
            strFunctCode = "990000"
            strSeverity = "E"


            If IsEmpty(strOriServiceFee) Then
                blnRes = True
                'strMsgCode = "00015"
                strMsgCode = "00172"
            Else
                Dim intServiceFee As Integer
                If Integer.TryParse(strOriServiceFee, intServiceFee) Then
                    Dim intSF As Integer
                    intSF = CInt(strOriServiceFee)
                    If intSF < 0 Then
                        blnRes = True
                        'strMsgCode = "00016"
                        strMsgCode = "00173"
                    Else
                        sm = Nothing
                    End If
                Else
                    blnRes = True
                    'strMsgCode = "00016"
                    strMsgCode = "00173"
                End If

            End If

            If blnRes Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkProvideSubsidy(ByVal strProvideSubsidyList() As String) As ComObject.SystemMessage
            Dim blnRes As Boolean = False
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity As String
            Dim strMsgCode As String = String.Empty
            'strFunctCode = "020101"
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00358"

            If strProvideSubsidyList Is Nothing Then
                blnRes = False
            Else
                Dim intProvideSubsidy As Integer = 0

                For intIdx As Integer = 0 To strProvideSubsidyList.Length - 1
                    If strProvideSubsidyList(intIdx) = YesNo.Yes Then
                        intProvideSubsidy += 1
                    End If
                Next

                If intProvideSubsidy > 0 Then
                    blnRes = True
                Else
                    blnRes = False
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        Public Function chkPracticeMOName(ByVal strMO As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = "00171"

            If IsEmpty(strMO) Then
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                sm = Nothing
            End If

            Return sm
        End Function

        Public Function chkSPID(ByVal strOriSPID As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage = Nothing
            Dim strFunctionCode, strSeverity, strMsgCode As String
            strFunctionCode = "990000"
            strSeverity = "E"
            strMsgCode = "00271"

            If strOriSPID.Length > 0 AndAlso strOriSPID.Length < 8 Then
                sm = New ComObject.SystemMessage(strFunctionCode, strSeverity, strMsgCode)
            ElseIf strOriSPID.Length = 8 Then
                Dim intSPID As Integer
                If Not Integer.TryParse(strOriSPID, intSPID) Then
                    sm = New ComObject.SystemMessage(strFunctionCode, strSeverity, strMsgCode)
                End If
            End If

            Return sm
        End Function

        Public Function chkRCHCode(ByVal strRCHCode As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage = Nothing
            Dim strFunctionCode, strSeverity, strMsgCode As String
            strFunctionCode = "990000"
            strSeverity = SeverityCode.SEVE
            strMsgCode = "00219"

            If strRCHCode.Length <> 6 Then
                sm = New ComObject.SystemMessage(strFunctionCode, strSeverity, strMsgCode)
            Else
                Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
                Dim number As New System.Text.RegularExpressions.Regex("[0-9]")

                If upper.Matches(strRCHCode.Substring(0, 2)).Count <> 2 _
                Or number.Matches(strRCHCode.Substring(2, 4)).Count <> 4 Then
                    sm = New ComObject.SystemMessage(strFunctionCode, strSeverity, strMsgCode)
                End If
            End If

            Return sm
        End Function

#Region "Exemption Cert"

        'Public Function chkECDOB(ByVal strOriDOB As String) As ComObject.SystemMessage
        '    Dim intAge, intMaxAge As Integer
        '    Dim strParam_1, strParam_2 As String
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        '    Dim strMsgCode As String
        '    Dim strDOBs As String() = strOriDOB.Split("-")
        '    Dim dtToday As DateTime
        '    dtToday = udtcomfunct.GetSystemDateTime

        '    strMsgCode = String.Empty
        '    strParam_1 = String.Empty
        '    strParam_2 = String.Empty

        '    If IsEmpty(strOriDOB) Then
        '        strMsgCode = "00003"
        '    Else
        '        If strDOBs.Length = 1 Then
        '            If (strDOBs.GetValue(0).ToString.Length = 8) Then
        '                strOriDOB = String.Format("{0}-{1}-{2}", strOriDOB.Substring(0, 2), strOriDOB.Substring(2, 2), strOriDOB.Substring(4, 4))
        '            ElseIf (strDOBs.GetValue(0).ToString.Length = 6) Then
        '                'strOriDOB = String.Format("01-{0}-{1}", strOriDOB.Substring(0, 2), strOriDOB.Substring(2, 4))
        '                strMsgCode = "00004"
        '            ElseIf (strDOBs.GetValue(0).ToString.Length = 4) Then
        '                strOriDOB = String.Format("01-01-{0}", strOriDOB)
        '            Else
        '                strMsgCode = "00004"
        '            End If
        '        Else
        '            If (strDOBs.GetValue(0).ToString.Length <> 2) Then
        '                strMsgCode = "00004"
        '            Else
        '                If strDOBs.Length = 2 Then
        '                    If strDOBs.GetValue(1).ToString.Length <> 4 Then
        '                        strMsgCode = "00004"
        '                    Else
        '                        'strOriDOB = String.Format("01-{0}-{1}", strDOBs.GetValue(0), strDOBs.GetValue(1))
        '                        strMsgCode = "00004"
        '                    End If
        '                ElseIf strDOBs.Length = 3 Then
        '                    If (strDOBs.GetValue(1).ToString.Length <> 2 Or strDOBs.GetValue(2).ToString.Length <> 4) Then
        '                        strMsgCode = "00004"
        '                    Else
        '                        strOriDOB = String.Format("{0}-{1}-{2}", strDOBs.GetValue(0), strDOBs.GetValue(1), strDOBs.GetValue(2))
        '                    End If
        '                ElseIf strDOBs.Length > 3 Then
        '                    strMsgCode = "00004"
        '                End If
        '            End If
        '        End If

        '        If strMsgCode = "" Then
        '            If Not IsDate(formatter.convertDate(strOriDOB, "E")) Then
        '                strMsgCode = "00004"
        '            Else
        '                intAge = CDate(formatter.convertDate(strOriDOB, "E")).Year

        '                udtcomfunct.getSystemParameter("MaxDOBAge", strParam_1, strParam_2)
        '                If strParam_1.Equals(String.Empty) Then
        '                    intMaxAge = 150
        '                Else
        '                    intMaxAge = CInt(strParam_1)
        '                End If

        '                If dtToday.Year - intAge > intMaxAge Then
        '                    strMsgCode = "00004"
        '                End If
        '            End If
        '        End If
        '    End If

        '    If Not strMsgCode.Equals(String.Empty) Then
        '        Return New ComObject.SystemMessage("990000", "E", strMsgCode)
        '    Else
        '        Return Nothing
        '    End If
        'End Function

        Public Function chkECAgeAndDOAge(ByVal strOriAge As String, ByVal strOriDORDay As String, ByVal strOriDORMonth As String, ByVal strOriDORYear As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage = Nothing

            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strMsgCode As String = String.Empty
            Dim intAge, intMaxAge As Integer
            Dim strParam_1 As String = String.Empty
            Dim strParam_2 As String = String.Empty

            Dim dtToday As DateTime = udtcomfunct.GetSystemDateTime()

            Dim dtmDOI As Date

            sm = Me.chkECDOAge(strOriDORDay, strOriDORMonth, strOriDORYear)

            If IsNothing(sm) Then
                If Date.TryParseExact(strOriDORDay.Trim + " " + strOriDORMonth.Trim + " " + strOriDORYear.Trim, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDOI) Then
                    If strOriAge.Trim.Equals(String.Empty) Then
                        sm = New ComObject.SystemMessage("990000", "E", "00004")
                    Else
                        intAge = dtmDOI.Year - Convert.ToInt32(strOriAge)

                        udtcomfunct.getSystemParameter("MaxDOBAge", strParam_1, strParam_2)
                        If strParam_1.Equals(String.Empty) Then
                            intMaxAge = 150
                        Else
                            intMaxAge = CInt(strParam_1)
                        End If

                        If dtToday.Year - intAge > intMaxAge Then
                            sm = New ComObject.SystemMessage("990000", "E", "00076")
                        End If
                    End If
                End If
            End If

            Return sm

        End Function

        Public Function chkECAge(ByVal strOriAge As String) As ComObject.SystemMessage
            Dim strMsgCode As String = String.Empty

            If IsEmpty(strOriAge) Then
                strMsgCode = "00074"
            Else
                Dim outNumber As Integer
                Dim blnIsNumber As Boolean = Integer.TryParse(strOriAge, outNumber)
                If Not blnIsNumber Then
                    strMsgCode = "00075"
                Else
                    If outNumber <= 0 Then
                        strMsgCode = "00075"
                    End If
                End If

            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New ComObject.SystemMessage("990000", "E", strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        Public Function chkECDOAge(ByVal strOriDORDay As String, ByVal strOriDORMonth As String, ByVal strOriDORYear As String) As ComObject.SystemMessage
            Dim strMsgCode As String = String.Empty
            Dim blnSuccess As Boolean = False

            Dim dtToday As DateTime = udtcomfunct.GetSystemDateTime()

            If IsEmpty(strOriDORDay) And IsEmpty(strOriDORMonth) And IsEmpty(strOriDORYear) Then
                strMsgCode = "00072"
            Else
                If IsEmpty(strOriDORDay) Or IsEmpty(strOriDORMonth) Or IsEmpty(strOriDORYear) Then
                    strMsgCode = "00073"
                Else
                    'Check day is integer
                    Dim intDay As Integer
                    If Integer.TryParse(strOriDORDay, intDay) Then
                        blnSuccess = True
                    Else
                        blnSuccess = False
                    End If

                    'Check Month is integer
                    Dim intMonth As Integer
                    If blnSuccess Then
                        If Integer.TryParse(strOriDORMonth, intMonth) Then
                            blnSuccess = True
                        Else
                            blnSuccess = False
                        End If
                    End If

                    'Check Year is integer
                    Dim intYear As Integer
                    If blnSuccess Then
                        If Integer.TryParse(strOriDORYear, intYear) Then
                            blnSuccess = True
                        Else
                            blnSuccess = False
                        End If
                    End If

                    'Check is valid date
                    Dim dtmTemp As Date
                    If blnSuccess Then
                        If Date.TryParseExact(intDay.ToString + " " + intMonth.ToString + " " + intYear.ToString, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmTemp) Then
                            blnSuccess = True

                            If dtmTemp > dtToday Then
                                strMsgCode = "00235"
                            End If

                        Else
                            blnSuccess = False
                        End If

                    End If

                End If
            End If

            If Not blnSuccess And strMsgCode.Equals(String.Empty) Then
                strMsgCode = "00073"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New ComObject.SystemMessage("990000", "E", strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        Public Function chkSerialNo(ByVal strSerialNo As String, ByVal blnSerialNoNotProvided As Boolean) As ComObject.SystemMessage
            If blnSerialNoNotProvided Then Return Nothing

            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            If Me.IsAlphanumeric(strSerialNo) Then
                If strSerialNo Is Nothing OrElse strSerialNo.Equals(String.Empty) Then
                    blnRes = False
                    strMsgCode = "00077"
                Else
                    If strSerialNo.Length < 6 OrElse strSerialNo.Length > 8 OrElse strSerialNo.IndexOf(" ") >= 0 Then
                        blnRes = False
                        strMsgCode = "00078"
                    End If
                End If
            Else
                blnRes = False
                strMsgCode = "00078"
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        Public Function chkReferenceNo(ByVal strReferenceNo As String, ByVal blnOtherFormat As Boolean) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            strReferenceNo = strReferenceNo.Replace("-", String.Empty)
            strReferenceNo = strReferenceNo.Replace("(", String.Empty).Replace(")", String.Empty).ToUpper.Trim()

            If strReferenceNo Is Nothing Or strReferenceNo.Equals(String.Empty) Then
                blnRes = False
                strMsgCode = "00079"
            Else
                If Not blnOtherFormat Then
                    If strReferenceNo.Length <> 14 Then
                        blnRes = False
                        strMsgCode = "00080"
                    Else
                        For i As Integer = 0 To 3
                            If strReferenceNo.Substring(i, 1) < "A" Or strReferenceNo.Substring(i, 1) > "Z" Then
                                blnRes = False
                                strMsgCode = "00080"
                            End If
                        Next
                        For i As Integer = 4 To 12
                            If strReferenceNo.Substring(i, 1) < "0" Or strReferenceNo.Substring(i, 1) > "9" Then
                                blnRes = False
                                strMsgCode = "00080"
                            End If
                        Next

                        If (strReferenceNo.Substring(13, 1) < "A" Or strReferenceNo.Substring(13, 1) > "Z") And (strReferenceNo.Substring(13, 1) < "0" Or strReferenceNo.Substring(13, 1) > "9") Then
                            blnRes = False
                            strMsgCode = "00080"
                        End If
                        ' To Do: Check Digit 
                        If Not chkECRefNoChgDgt(strReferenceNo) Then
                            blnRes = False
                            strMsgCode = "00080"
                        End If
                    End If
                End If

            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        Public Function chkECDate(ByVal strOriDOIDay As String, ByVal strOriDOIMonth As String, ByVal strOriDOIYear As String) As ComObject.SystemMessage
            Dim strMsgCode As String = String.Empty
            Dim blnSuccess As Boolean = False

            Dim dtToday As DateTime = udtcomfunct.GetSystemDateTime()

            If IsEmpty(strOriDOIDay) And IsEmpty(strOriDOIMonth) And IsEmpty(strOriDOIYear) Then
                strMsgCode = "00081"
            Else
                If IsEmpty(strOriDOIDay) Or IsEmpty(strOriDOIMonth) Or IsEmpty(strOriDOIYear) Then
                    strMsgCode = "00082"
                Else
                    'Check day is integer
                    Dim intDay As Integer
                    If Integer.TryParse(strOriDOIDay, intDay) Then
                        blnSuccess = True
                    Else
                        blnSuccess = False
                    End If

                    'Check Month is integer
                    Dim intMonth As Integer
                    If blnSuccess Then
                        If Integer.TryParse(strOriDOIMonth, intMonth) Then
                            blnSuccess = True
                        Else
                            blnSuccess = False
                        End If
                    End If

                    'Check Year is integer
                    Dim intYear As Integer
                    If blnSuccess Then
                        If Integer.TryParse(strOriDOIYear, intYear) Then
                            blnSuccess = True
                        Else
                            blnSuccess = False
                        End If
                    End If

                    'Check is valid date
                    Dim dtmTemp As Date
                    If blnSuccess Then
                        If Date.TryParseExact(intDay.ToString + " " + intMonth.ToString + " " + intYear.ToString, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmTemp) Then
                            blnSuccess = True

                            If dtmTemp > dtToday Then
                                strMsgCode = "00231"
                            End If
                        Else
                            blnSuccess = False
                        End If

                    End If

                End If
            End If

            If Not blnSuccess And strMsgCode.Equals(String.Empty) Then
                strMsgCode = "00082"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New ComObject.SystemMessage("990000", "E", strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        Public Function chkECDate(ByVal strOriDOIDay As String, ByVal strOriDOIMonth As String, ByVal strOriDOIYear As String, ByVal dtmDOB As DateTime) As ComObject.SystemMessage
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            Dim dtmIssueDate As Date

            udtSM = chkECDate(strOriDOIDay, strOriDOIMonth, strOriDOIYear)

            If IsNothing(udtSM) Then

                If Date.TryParseExact(CInt(strOriDOIDay).ToString + " " + CInt(strOriDOIMonth).ToString + " " + CInt(strOriDOIYear).ToString, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmIssueDate) Then

                    If dtmIssueDate < dtmDOB Then
                        udtSM = New Common.ComObject.SystemMessage("990000", "E", "00228")
                    End If

                End If
            End If

            Return udtSM

        End Function

        Public Function chkECRefNoChgDgt(ByVal strOriRef As String) As Boolean
            Dim blnRes As Boolean = False
            Dim strTemp As String
            Dim intTotal, intRemainder, intRating, intASC, intChkDgt As Integer
            Dim strOriChkDgt, strChkDgt As String


            If strOriRef.Trim.Equals(String.Empty) Then
                blnRes = False
            Else
                intTotal = 0
                intRemainder = 0
                intChkDgt = 0
                intRating = 14
                strOriRef = strOriRef.ToUpper.Replace("-", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty)
                strTemp = strOriRef.ToUpper.Substring(0, strOriRef.Length - 1)
                strOriChkDgt = strOriRef.Substring(strOriRef.Length - 1, 1)
                strChkDgt = String.Empty

                For Each c As Char In strTemp
                    intASC = Asc(c)
                    If intASC >= 65 And intASC <= 90 Then
                        intTotal = intTotal + intRating * (intASC - 55)
                    Else
                        intTotal = intTotal + intRating * (intASC - 48)
                    End If
                    intRating = intRating - 1
                Next

                intRemainder = intTotal Mod 17
                intChkDgt = 17 - intRemainder
                Select Case intChkDgt
                    Case 10
                        strChkDgt = "A"
                    Case 11
                        strChkDgt = "B"
                    Case 12
                        strChkDgt = "C"
                    Case 13
                        strChkDgt = "D"
                    Case 14
                        strChkDgt = "E"
                    Case 15
                        strChkDgt = "F"
                    Case 16
                        strChkDgt = "G"
                    Case 17
                        strChkDgt = "0"
                    Case Else
                        strChkDgt = intChkDgt.ToString
                End Select

                If strChkDgt.Equals(strOriChkDgt) Then
                    blnRes = True
                Else
                    blnRes = False
                End If

            End If

            Return blnRes
        End Function

        Public Function chkDOBType(ByVal strSelectedDOBType As String) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty


            If strSelectedDOBType Is Nothing Or strSelectedDOBType.Equals(String.Empty) Then
                blnRes = False
                strMsgCode = "00085"
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------   

        'Public Function chkReadyCreateECAccount(ByVal strSchemeCode As String, ByVal strHKID As String, ByVal strECSerial As String, ByVal strECReference As String, ByVal strExcludeVoucherAccID As String, Optional ByVal blnNewCreation As Boolean = False, Optional ByVal strExcludeTempAccID As String = "") As Common.ComObject.SystemMessage

        '    Dim udtVoucherScheme As Common.Component.VoucherScheme.VoucherSchemeModel = Nothing
        '    Dim udtVRAcct As Common.Component.VoucherRecipientAccount.VoucherRecipientAccountModel = Nothing
        '    Dim udtVRAcctCollection As Common.Component.VoucherRecipientAccount.VoucherRecipientAccountModelCollection = Nothing

        '    Dim udtVoucherSchemeBLL As Common.Component.VoucherScheme.VoucherSchemeBLL = New Common.Component.VoucherScheme.VoucherSchemeBLL()
        '    Dim udtVRAcctBLL As Common.Component.VoucherRecipientAccount.VoucherRecipientAccountBLL = New Common.Component.VoucherRecipientAccount.VoucherRecipientAccountBLL()

        '    Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        '    Dim sm As Common.ComObject.SystemMessage = Nothing
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()

        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""

        '    Dim blnValidateFail As Boolean = False
        '    udtVoucherScheme = udtVoucherSchemeBLL.LoadVoucheScheme(strSchemeCode)

        '    If udtVoucherScheme Is Nothing Then
        '        'Throw error if the Scheme setting is invalid
        '        strMsgCode = "00105"
        '    Else
        '        ' To Do: For Voucher Account Creation, No Matter Same Voucher Account, Block

        '        udtVRAcct = udtVRAcctBLL.LoadVRAcct(strHKID, strSchemeCode)
        '        If udtVRAcct Is Nothing OrElse (udtVRAcct.VRAcctID = strExcludeVoucherAccID And udtVRAcct.VRAcctID.Trim() <> "") Then
        '            udtVRAcct = udtVRAcctBLL.LoadVRAcctByECDetail(strECSerial, strECReference, strSchemeCode)
        '            If udtVRAcct Is Nothing OrElse (udtVRAcct.VRAcctID.Trim() = strExcludeVoucherAccID.Trim() And udtVRAcct.VRAcctID.Trim() <> "") Then
        '                ' Check Serial No + Reference No In Use in Temp
        '                udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcctByECDetail(strECSerial, strECReference, strSchemeCode)
        '                For Each udtTempVRAcct As Component.VoucherRecipientAccount.VoucherRecipientAccountModel In udtVRAcctCollection
        '                    If udtTempVRAcct.AcctStatus <> "D" AndAlso udtTempVRAcct.AcctStatus <> "V" AndAlso (strExcludeTempAccID.Trim() = "" OrElse udtTempVRAcct.VRAcctID.Trim() <> strExcludeTempAccID.Trim()) Then
        '                        If blnNewCreation Then
        '                            ' Serial No + Reference No In Use
        '                            strMsgCode = "00118"
        '                            blnValidateFail = True
        '                        Else
        '                            Dim blnDifferent As Boolean = False
        '                            If Not udtTempVRAcct.HKID.Trim().ToUpper() = strHKID.Trim().ToUpper() Then blnDifferent = True
        '                            If blnDifferent Then
        '                                blnValidateFail = True
        '                                ' Serial No + Reference No In Use
        '                                strMsgCode = "00118"
        '                                Exit For
        '                            End If
        '                        End If
        '                    End If
        '                Next

        '                If Not blnValidateFail Then
        '                    udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcct(strHKID, strSchemeCode)
        '                    For Each udtTempVRAcct As Component.VoucherRecipientAccount.VoucherRecipientAccountModel In udtVRAcctCollection
        '                        If udtTempVRAcct.AcctStatus <> "D" AndAlso udtTempVRAcct.AcctStatus <> "V" AndAlso (strExcludeTempAccID.Trim() = "" OrElse udtTempVRAcct.VRAcctID.Trim() <> strExcludeTempAccID.Trim()) Then
        '                            If blnNewCreation Then
        '                                ' Creation: HKID Exist Already
        '                                strMsgCode = "00111"
        '                            Else
        '                                If udtTempVRAcct.HKIDCard = "Y" Then
        '                                    blnValidateFail = True
        '                                    ' Violation of HKIC & EC Case both exist
        '                                    strMsgCode = "00141"
        '                                    Exit For
        '                                End If
        '                            End If
        '                        End If
        '                    Next
        '                End If

        '                If Not blnValidateFail Then
        '                    For Each udtTempVRAcct As Component.VoucherRecipientAccount.VoucherRecipientAccountModel In udtVRAcctCollection
        '                        If udtTempVRAcct.AcctStatus <> "D" AndAlso udtTempVRAcct.AcctStatus <> "V" AndAlso (strExcludeTempAccID.Trim() = "" OrElse udtTempVRAcct.VRAcctID.Trim() <> strExcludeTempAccID.Trim()) Then

        '                            If blnNewCreation Then
        '                                ' Creation: HKID Exist Already
        '                                strMsgCode = "00111"
        '                            Else
        '                                If udtTempVRAcct.HKIDCard = "N" AndAlso udtTempVRAcct.AcctPurpose <> "O" Then
        '                                    Dim blnDifferent As Boolean = False
        '                                    If Not udtTempVRAcct.ECSerialNo.Trim().ToUpper() = strECSerial.Trim().ToUpper() Then blnDifferent = True
        '                                    If Not udtTempVRAcct.ECReferenceNo.Trim().ToUpper() = strECReference.Trim().ToUpper() Then blnDifferent = True
        '                                    If blnDifferent Then
        '                                        blnValidateFail = True
        '                                        ' ?? HKID Exist With Difference Serial No + ReferenceNo
        '                                        strMsgCode = "00111"
        '                                        Exit For
        '                                    End If
        '                                End If
        '                            End If
        '                        End If
        '                    Next
        '                End If
        '            Else
        '                ' Serial No + Reference No In Use In Validated Account
        '                strMsgCode = "00118"
        '            End If
        '        Else
        '            ' Validated Account Exist!
        '            strMsgCode = "00112"
        '        End If
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkReadyRectifyECAccount(ByVal strSchemeCode As String, ByVal strHKID As String, ByVal strECSerial As String, ByVal strECReference As String, ByVal strExcludeVoucherAccID As String, Optional ByVal strExcludeTempAccID As String = "") As Common.ComObject.SystemMessage
        '    Dim udtVoucherScheme As Common.Component.VoucherScheme.VoucherSchemeModel = Nothing
        '    Dim udtVRAcct As Common.Component.VoucherRecipientAccount.VoucherRecipientAccountModel = Nothing
        '    Dim udtVRAcctCollection As Common.Component.VoucherRecipientAccount.VoucherRecipientAccountModelCollection = Nothing

        '    Dim udtVoucherSchemeBLL As Common.Component.VoucherScheme.VoucherSchemeBLL = New Common.Component.VoucherScheme.VoucherSchemeBLL()
        '    Dim udtVRAcctBLL As Common.Component.VoucherRecipientAccount.VoucherRecipientAccountBLL = New Common.Component.VoucherRecipientAccount.VoucherRecipientAccountBLL()

        '    Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        '    Dim sm As Common.ComObject.SystemMessage = Nothing
        '    Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()

        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""

        '    Dim blnValidateFail As Boolean = False
        '    udtVoucherScheme = udtVoucherSchemeBLL.LoadVoucheScheme(strSchemeCode)

        '    If udtVoucherScheme Is Nothing Then
        '        'Throw error if the Scheme setting is invalid
        '        strMsgCode = "00105"
        '    Else
        '        ' To Do: For Voucher Account Creation, No Matter Same Voucher Account, Block

        '        udtVRAcct = udtVRAcctBLL.LoadVRAcct(strHKID, strSchemeCode)
        '        If udtVRAcct Is Nothing OrElse (udtVRAcct.VRAcctID = strExcludeVoucherAccID And udtVRAcct.VRAcctID.Trim() <> "") Then
        '            udtVRAcct = udtVRAcctBLL.LoadVRAcctByECDetail(strECSerial, strECReference, strSchemeCode)
        '            If udtVRAcct Is Nothing OrElse (udtVRAcct.VRAcctID.Trim() = strExcludeVoucherAccID.Trim() And udtVRAcct.VRAcctID.Trim() <> "") Then
        '                ' Check Serial No + Reference No In Use in Temp
        '                udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcctByECDetail(strECSerial, strECReference, strSchemeCode)
        '                For Each udtTempVRAcct As Component.VoucherRecipientAccount.VoucherRecipientAccountModel In udtVRAcctCollection
        '                    If udtTempVRAcct.AcctStatus <> "D" AndAlso udtTempVRAcct.AcctStatus <> "V" AndAlso (strExcludeTempAccID.Trim() = "" OrElse udtTempVRAcct.VRAcctID.Trim() <> strExcludeTempAccID.Trim()) Then

        '                        Dim blnDifferent As Boolean = False
        '                        If Not udtTempVRAcct.HKID.Trim().ToUpper() = strHKID.Trim().ToUpper() Then blnDifferent = True
        '                        If blnDifferent Then
        '                            blnValidateFail = True
        '                            ' Serial No + Reference No In Use
        '                            strMsgCode = "00118"
        '                            Exit For
        '                        End If

        '                    End If
        '                Next

        '                If Not blnValidateFail Then
        '                    udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcct(strHKID, strSchemeCode)
        '                    For Each udtTempVRAcct As Component.VoucherRecipientAccount.VoucherRecipientAccountModel In udtVRAcctCollection
        '                        If udtTempVRAcct.AcctStatus <> "D" AndAlso udtTempVRAcct.AcctStatus <> "V" AndAlso (strExcludeTempAccID.Trim() = "" OrElse udtTempVRAcct.VRAcctID.Trim() <> strExcludeTempAccID.Trim()) Then

        '                            If udtTempVRAcct.HKIDCard = "Y" Then
        '                                blnValidateFail = True
        '                                ' Violation of HKIC & EC Case both exist
        '                                strMsgCode = "00141"
        '                                Exit For

        '                            End If
        '                        End If
        '                    Next
        '                End If

        '                If Not blnValidateFail Then
        '                    For Each udtTempVRAcct As Component.VoucherRecipientAccount.VoucherRecipientAccountModel In udtVRAcctCollection
        '                        If udtTempVRAcct.AcctStatus <> "D" AndAlso udtTempVRAcct.AcctStatus <> "V" AndAlso (strExcludeTempAccID.Trim() = "" OrElse udtTempVRAcct.VRAcctID.Trim() <> strExcludeTempAccID.Trim()) Then


        '                            If udtTempVRAcct.HKIDCard = "N" AndAlso udtTempVRAcct.AcctPurpose <> "O" Then
        '                                Dim blnDifferent As Boolean = False
        '                                If Not udtTempVRAcct.ECSerialNo.Trim().ToUpper() = strECSerial.Trim().ToUpper() Then blnDifferent = True
        '                                If Not udtTempVRAcct.ECReferenceNo.Trim().ToUpper() = strECReference.Trim().ToUpper() Then blnDifferent = True
        '                                If blnDifferent Then
        '                                    blnValidateFail = True
        '                                    ' ?? HKID Exist With Difference Serial No + ReferenceNo
        '                                    strMsgCode = "00111"
        '                                    Exit For
        '                                End If
        '                            End If
        '                        End If
        '                    Next
        '                End If
        '            Else
        '                ' Serial No + Reference No In Use In Validated Account
        '                strMsgCode = "00118"
        '            End If
        '        Else
        '            ' Validated Account Exist!
        '            ' If the Validated Account have same detail with the Tempoary Voucher Account, Allow Rectify
        '            If udtVRAcct.HKIDCard = "Y" Then
        '                ' Validated Account Exist With Different Card Type
        '                strMsgCode = "00112"
        '            Else
        '                ' Validated Account Exist With Different Detail
        '                Dim blnDifferent As Boolean = False
        '                If Not udtVRAcct.ECSerialNo.Trim().ToUpper() = strECSerial.Trim().ToUpper() Then blnDifferent = True
        '                If Not udtVRAcct.ECReferenceNo.Trim().ToUpper() = strECReference.Trim().ToUpper() Then blnDifferent = True
        '                If blnDifferent Then
        '                    strMsgCode = "00112"
        '                End If
        '            End If
        '        End If
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Public Function chkSerialNoNotProvidedAllow(ByVal dtmECDate As Date, ByVal blnSerialNoNotProvided As Boolean) As Common.ComObject.SystemMessage
            ' Get Smartics EC DOI
            Dim strParmValue As String = String.Empty

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            udtGeneralFunction.getSystemParameter("EC_DOI", strParmValue, String.Empty)

            Dim dtmParmValue As Date = CDate(strParmValue)

            ' Compare
            If dtmECDate >= dtmParmValue Then
                ' The Date of Issue inputted is not before Smartic, therefore Serial No. Not Provided and Reference free format are NOT allowed
                If blnSerialNoNotProvided Then
                    Return New Common.ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, Common.Component.SeverityCode.SEVE, "00251")
                End If
            End If

            Return Nothing

        End Function

        Public Function chkReferenceOtherFormatAllow(ByVal dtmECDate As Date, ByVal blnReferenceOtherFormat As Boolean) As Common.ComObject.SystemMessage
            ' Get Smartic EC DOI
            Dim strParmValue As String = String.Empty

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            udtGeneralFunction.getSystemParameter("EC_DOI", strParmValue, String.Empty)

            Dim dtmParmValue As Date = CDate(strParmValue)

            ' Compare
            If dtmECDate >= dtmParmValue Then
                ' The Date of Issue inputted is not before Smartic, therefore Serial No. Not Provided and Reference free format are NOT allowed
                If blnReferenceOtherFormat Then
                    Return New Common.ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, Common.Component.SeverityCode.SEVE, "00252")
                End If
            End If

            Return Nothing

        End Function

        Public Function TryParseECReference(ByRef strReference As String, ByRef blnReferenceOtherFormat As Boolean, ByVal dtmECDOI As Date) As Boolean
            ' Try parse only if the Reference has the following 2 formats:
            ' XXXX9999999999
            ' XXXX-9999999-99(9)

            strReference = strReference.Trim.ToUpper

            Select Case strReference.Length
                Case 14
                    Dim reg As New Regex("^[A-Z]{4}\d{10}$")
                    If Not reg.IsMatch(strReference) Then Return False

                Case 18
                    Dim reg As New Regex("^[A-Z]{4}-\d{7}-\d{2}\(\d\)$")
                    If Not reg.IsMatch(strReference) Then Return False

                Case Else
                    Return False

            End Select

            ' Get Smartics EC DOI
            Dim strParmValue As String = String.Empty

            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            udtGeneralFunction.getSystemParameter("EC_DOI", strParmValue, String.Empty)

            Dim dtmSmarticsDOI As Date = CDate(strParmValue)

            If dtmECDOI >= dtmSmarticsDOI Then
                Dim udtValidator As New Common.Validation.Validator

                If IsNothing(udtValidator.chkReferenceNo(strReference, False)) Then
                    ' EC Reference is valid, set Other Format as false
                    blnReferenceOtherFormat = False

                    Return True

                End If

            End If

            Return False

        End Function

#End Region

#Region "Report Submission"
        Public Function chkReportSubmissionPeriodFromToDate(ByVal strReportID As String, ByVal strFromText As String, ByVal strToText As String) As List(Of ComObject.SystemMessage)

            Dim strFunctionCode As String = "010701"
            Dim lstSysMsg As New List(Of ComObject.SystemMessage)
            Dim udtFormatter As Common.Format.Formatter = New Common.Format.Formatter

            ' Do not Allow Empty Input
            Dim blnIsInputFromText As Boolean = True
            Dim blnIsInputToText As Boolean = True

            If strFromText.Trim() = "" Then
                lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00001"))
                blnIsInputFromText = False
            End If
            If strToText.Trim() = "" Then
                lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00002"))
                blnIsInputToText = False
            End If

            'If Not blnIsInputToText Or Not blnIsInputFromText Then
            '    Return lstSysMsg
            'End If

            Dim blnIsValidFrom As Boolean = False
            Dim blnIsValidTo As Boolean = False

            If strFromText.Trim() <> "" Then

                blnIsValidFrom = IsDate(udtFormatter.convertDate(strFromText.Trim(), "E"))

                If blnIsValidFrom Then
                    Dim dtmInputFromDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(strFromText.Trim(), "E"))
                    'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'If dtmInputFromDate.Year < 1753 Or dtmInputFromDate > Date.MaxValue Then
                    If dtmInputFromDate.Year < DateValidation.YearMinValue Or dtmInputFromDate.Year > DateValidation.YearMaxValue Then
                        blnIsValidFrom = False
                    End If
                    'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
                End If

                If Not blnIsValidFrom Then
                    lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00003"))
                End If

                If blnIsValidFrom Then
                    ' Future From Date
                    If DateTime.Now < CDate(udtFormatter.convertDate(strFromText.Trim(), "E")) Then
                        lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00005"))
                    End If
                End If

            End If

            If strToText.Trim() <> "" Then

                blnIsValidTo = IsDate(udtFormatter.convertDate(strToText.Trim(), "E"))

                If blnIsValidTo Then
                    Dim dtmInputToDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(strToText.Trim(), "E"))
                    'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'If dtmInputToDate.Year < 1753 Or dtmInputToDate > Date.MaxValue Then
                    If dtmInputToDate.Year < DateValidation.YearMinValue Or dtmInputToDate.Year > DateValidation.YearMaxValue Then
                        blnIsValidTo = False
                    End If
                    'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
                End If

                If Not blnIsValidTo Then
                    lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00004"))
                End If

                If blnIsValidTo Then
                    ' Future To Date
                    If DateTime.Now < CDate(udtFormatter.convertDate(strToText.Trim(), "E")) Then
                        lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00006"))
                    End If
                End If
            End If

            If blnIsValidFrom And blnIsValidTo Then
                If CDate(udtFormatter.convertDate(strFromText.Trim(), "E")) > CDate(udtFormatter.convertDate(strToText.Trim(), "E")) Then
                    lstSysMsg.Add(New ComObject.SystemMessage(strFunctionCode, "E", "00007"))
                End If
            End If

            Return lstSysMsg
        End Function
#End Region

#Region "ServiceDate Validation"

        Public Function chkDateBackClaimServiceDate(ByVal strOriServiceDate As String, ByVal intDayLimit As Integer, ByVal dtmMindate As DateTime) As ComObject.SystemMessage

            Dim sm As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean
            blnRes = True
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty
            strMsgCode = "00119"

            If IsEmpty(strOriServiceDate) Then
                blnRes = False
                strMsgCode = "00119"
            Else
                If strOriServiceDate.Length = 8 Then
                    strOriServiceDate = strOriServiceDate.Substring(0, 2) + "-" + strOriServiceDate.Substring(2, 2) + "-" + strOriServiceDate.Substring(4, 4)
                End If

                If Not IsDate(formatter.convertDate(strOriServiceDate, "E")) Then
                    blnRes = False
                    strMsgCode = "00120"
                Else
                    sm = chkDateIsFutureDate(CDate(formatter.convertDate(strOriServiceDate, "E")))
                    If sm Is Nothing Then
                        blnRes = True
                        strMsgCode = String.Empty

                        ' Check Date Back Claim
                        If CDate(formatter.convertDate(strOriServiceDate, "E")).AddDays(intDayLimit) <= DateTime.Now.Date Then
                            ' Exceed Date Back Day Limit
                            blnRes = False
                            strMsgCode = "00149"
                        Else
                            If CDate(formatter.convertDate(strOriServiceDate, "E")) < dtmMindate Then
                                ' Exceed The Min Date
                                blnRes = False
                                strMsgCode = "00150"
                            End If
                        End If
                    Else
                        blnRes = False
                        strMsgCode = "00121"
                    End If
                End If
            End If

            If blnRes Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If

            Return sm
        End Function

        Public Function chkServiceDateSubsidizeDoseLastServiceDate(ByVal dtmServiceDate As DateTime, ByVal udtSchemeDosePeriod As Component.SchemeDetails.SchemeDosePeriodModel) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage = Nothing
            If Not udtSchemeDosePeriod Is Nothing Then
                If dtmServiceDate > udtSchemeDosePeriod.ToDtm Then
                    sm = New ComObject.SystemMessage("990000", "E", "00243")
                End If
            End If
            Return sm
        End Function

        Public Function chkServiceDataSubsidizeGroupLastServiceData(ByVal dtmServiceDate As DateTime, ByVal udtSubsidizeGroupClaim As Component.Scheme.SubsidizeGroupClaimModel) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage = Nothing
            If dtmServiceDate > udtSubsidizeGroupClaim.LastServiceDtm Then
                sm = New ComObject.SystemMessage("990000", "E", "00237")
            End If
            Return sm
        End Function

#End Region

        Public Function chkValidSearchDate(ByVal strOriInputDate As String) As String
            Dim strOriInputDates As String()
            Dim strFormattedInputDate As String
            Dim blnIsValid As Boolean = True
            Dim dtmOriInputDate As DateTime
            Dim strMessageCode As String = String.Empty
            Dim formatter As Format.Formatter = New Format.Formatter

            strOriInputDates = strOriInputDate.Split("-")
            If strOriInputDate.Equals(String.Empty) Then
                'input value is enmpty.
                strMessageCode = Common.Component.MsgCode.MSG00103
            ElseIf strOriInputDates.Length <> 3 Then
                'Input date format is invalid.
                strMessageCode = Common.Component.MsgCode.MSG00101
            Else
                strFormattedInputDate = formatter.formatSearchDate(strOriInputDate)

                If Not IsDate(formatter.convertDate(strFormattedInputDate, "E")) Then
                    'input date is invalid.
                    strMessageCode = Common.Component.MsgCode.MSG00104
                Else
                    dtmOriInputDate = formatter.convertDate(strFormattedInputDate, "E")
                End If
            End If

            If strMessageCode.Equals(String.Empty) Then
                'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If dtmOriInputDate.Year < 1753 Or dtmOriInputDate > Date.MaxValue Then
                If dtmOriInputDate.Year < DateValidation.YearMinValue Or dtmOriInputDate.Year > DateValidation.YearMaxValue Then
                    'The input Date is out of range. (year < 1753 or date > 31 DEC 9999)
                    strMessageCode = Common.Component.MsgCode.MSG00102
                End If
                'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

            End If

            Return strMessageCode
        End Function

        Public Function IsAlphanumeric(ByVal strValue As String)
            Dim isValid As Boolean = True
            If Not strValue Is Nothing AndAlso Not strValue.Equals(String.Empty) Then
                strValue = strValue.ToUpper
                Dim intASC As Integer
                For Each character As Char In strValue
                    intASC = Asc(character)
                    If Not (intASC > 64 And intASC < 91) And Not (intASC > 47 And intASC < 58) Then
                        isValid = False
                        Exit For
                    End If
                Next
            End If
            Return isValid
        End Function

        Public Function IsAlphanumericHyphen(ByVal strValue As String)
            Dim isValid As Boolean = True
            If Not strValue Is Nothing AndAlso Not strValue.Equals(String.Empty) Then
                'strValue = strValue.ToUpper
                Dim intASC As Integer
                For Each character As Char In strValue
                    intASC = Asc(character)
                    If Not (intASC > 64 And intASC < 91) And Not (intASC > 47 And intASC < 58) _
                        And Not (intASC > 96 And intASC < 123) And Not (intASC = 45) Then
                        isValid = False
                        Exit For
                    End If
                Next
            End If
            Return isValid
        End Function

        Public Function IsAlpha(ByVal strValue As String)
            Dim isValid As Boolean = True
            If Not strValue Is Nothing AndAlso Not strValue.Equals(String.Empty) Then
                strValue = strValue.ToUpper
                Dim intASC As Integer
                For Each character As Char In strValue
                    intASC = Asc(character)
                    If Not (intASC > 64 And intASC < 91) Then
                        isValid = False
                        Exit For
                    End If
                Next
            End If
            Return isValid
        End Function

        ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
        Public Function IsInteger(ByVal strInput As String) As Boolean
            Dim intOutput As Integer = 0

            ' For input greater than the max value of Int32 (2,147,483,647), it would return false
            If New Regex("^-?\d+$").IsMatch(strInput.Trim) AndAlso Integer.TryParse(strInput, intOutput) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function IsDecimal(ByVal strInput As String) As Boolean
            Dim decOutput As Decimal = 0

            ' For input greater than the max value of Decimal, it would return false
            If New Regex("^-?\d+(\.\d+)?$").IsMatch(strInput.Trim) AndAlso Decimal.TryParse(strInput, decOutput) Then
                Return True
            Else
                Return False
            End If
        End Function
        ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

        ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check whether string include Full Width chars (Eng, Number, Punctuations)
        ''' </summary>
        ''' <param name="strInput"></param>
        ''' <returns></returns>
        ''' <remarks>Block range: FF01 - FF60 </remarks>
        Public Function ContainsFullWidthChar(ByVal strInput As String) As Boolean
            Dim inputChars() As Char = strInput.ToCharArray

            If Not strInput Is Nothing AndAlso Not strInput.Equals(String.Empty) Then
                For i As Integer = 0 To inputChars.Length - 1
                    Dim intCharCode As Integer = CInt(AscW(inputChars(i)))

                    If intCharCode >= 65281 And intCharCode <= 65376 Then
                        Return True
                    End If
                Next
            End If

            Return False
        End Function
        ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

#Region "Validation for documnet identity number"

        Public Function chkIdentityNumber(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strIdentityNumPrefix As String) As ComObject.SystemMessage
            Dim systemMessage As ComObject.SystemMessage = Nothing

            Select Case strDocCode.Trim()

                Case DocTypeModel.DocTypeCode.ADOPC
                    If strIdentityNumPrefix Is Nothing Then
                        strIdentityNumPrefix = String.Empty
                    End If

                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'systemMessage = Me.chkNoOfEntry(String.Format("{0}{1}", strIdentityNum, strIdentityNumPrefix))
                    systemMessage = Me.chkNoOfEntry(strIdentityNum, strIdentityNumPrefix)
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]
                Case DocTypeModel.DocTypeCode.DI
                    systemMessage = Me.chkDocumentNo(strIdentityNum)

                Case DocTypeModel.DocTypeCode.HKBC
                    systemMessage = Me.chkRegNo(strIdentityNum)

                Case DocTypeModel.DocTypeCode.HKIC, DocTypeModel.DocTypeCode.EC,
                     DocTypeModel.DocTypeCode.CCIC, DocTypeModel.DocTypeCode.ROP140 ' CRE20-0022 (Immu record) [Martin] 
                    systemMessage = Me.chkHKID(strIdentityNum)

                Case DocTypeModel.DocTypeCode.ID235B
                    systemMessage = Me.chkBirthEntryNo(strIdentityNum)

                Case DocTypeModel.DocTypeCode.REPMT
                    systemMessage = Me.chkReEntryNo(strIdentityNum)

                Case DocTypeModel.DocTypeCode.VISA
                    systemMessage = Me.chkVisaNo(strIdentityNum)

                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case DocTypeModel.DocTypeCode.OC,
                    DocTypeModel.DocTypeCode.OW,
                    DocTypeModel.DocTypeCode.TW,
                    DocTypeModel.DocTypeCode.IR,
                    DocTypeModel.DocTypeCode.HKP,
                    DocTypeModel.DocTypeCode.RFNo8,
                    DocTypeModel.DocTypeCode.OTHER,
                    DocTypeModel.DocTypeCode.PASS ' CRE20-0022 (Immu record) [Martin]
                    systemMessage = Me.chkDocumentNoForNonEHSDocType(strIdentityNum)
                    ' CRE19-001 (VSS 2019) [End][Winnie]
            End Select

            Return systemMessage
        End Function

        'Document Type = Hong Kong Identity Card/Certificate of Exemption
        Public Function chkHKID(ByVal strHKID As String) As ComObject.SystemMessage
            Dim AE As New ASCIIEncoding
            Dim intDigitSum As Integer = 0
            Dim intCheckSum As Integer = 0
            Dim ByteASCIIZero As Byte() = AE.GetBytes("0A")
            Dim ByteArray As Byte()
            Dim strCheckSum As String = "123456789A0"
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strHKID = strHKID.Trim.ToUpper
            strHKID = strHKID.Replace("(", String.Empty)
            strHKID = strHKID.Replace(")", String.Empty)

            If IsEmpty(strHKID) Then
                strMsgCode = "00001"
            Else
                ' validate the HKID format
                If strHKID.Length < 8 Or strHKID.Length > 9 Then
                    strMsgCode = "00002"
                ElseIf strHKID.Length = 8 Or strHKID.Length = 9 Then

                    ' Check Leading Letter
                    If strHKID.Substring(0, 1) < "A" Or strHKID.Substring(0, 1) > "Z" Then
                        strMsgCode = "00002"
                    End If

                    If strHKID.Length = 9 Then
                        If strHKID.Substring(1, 1) < "A" Or strHKID.Substring(1, 1) > "Z" Then
                            strMsgCode = "00002"
                        End If
                    End If

                    ' Check Digit Value (Exclude Leading Letter and Tailing Check Digit)
                    Dim intDigitCheck As Integer = IIf(strHKID.Length = 8, 1, 2)
                    For i As Integer = intDigitCheck To strHKID.Length - 2
                        If strHKID(i) < "0" OrElse strHKID(i) > "9" Then
                            strMsgCode = "00002"
                            Exit For
                        End If
                    Next

                End If

                If strMsgCode = "" Then
                    ' to add a space into the HKID input whenever its first 2 characters are not alphabets
                    If strHKID.Length = 8 Or strHKID.Length = 10 Then
                        strHKID = " " + strHKID
                    End If

                    ByteArray = AE.GetBytes(strHKID)

                    ' to calculate the checksumby summing the digits multiplied by their weights
                    If String.Compare(" ", strHKID.Substring(0, 1)) = 0 Then
                        intDigitSum = 36 * 9
                    Else
                        intDigitSum = CType((ByteArray(0) - ByteASCIIZero(1) + 10) * 9, Integer)
                    End If

                    intDigitSum = CType((ByteArray(1) - ByteASCIIZero(1) + 10) * 8, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(2) - ByteASCIIZero(0)) * 7, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(3) - ByteASCIIZero(0)) * 6, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(4) - ByteASCIIZero(0)) * 5, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(5) - ByteASCIIZero(0)) * 4, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(6) - ByteASCIIZero(0)) * 3, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(7) - ByteASCIIZero(0)) * 2, Integer) + intDigitSum
                    intDigitSum = 11 - intDigitSum Mod 11

                    ' to compare the digit sum and the check sum
                    If strHKID.Length = 8 Or strHKID.Length = 9 Then
                        If String.Compare(strHKID.Substring(strHKID.Length - 1, 1), strCheckSum.Substring(intDigitSum - 1, 1)) = 0 Then
                            strMsgCode = ""
                        Else
                            strMsgCode = "00002"
                        End If
                    End If
                End If

            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        'Document Type = Hong Kong Birth Certificate
        Public Function chkRegNo(ByVal strHKID As String) As ComObject.SystemMessage
            Dim AE As New ASCIIEncoding
            Dim intDigitSum As Integer = 0
            Dim intCheckSum As Integer = 0
            Dim ByteASCIIZero As Byte() = AE.GetBytes("0A")
            Dim ByteArray As Byte()
            Dim strCheckSum As String = "123456789A0"
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strHKID = strHKID.Trim.ToUpper
            strHKID = strHKID.Replace("(", String.Empty)
            strHKID = strHKID.Replace(")", String.Empty)

            If IsEmpty(strHKID) Then
                strMsgCode = "00203"
            Else
                ' validate the HKID format
                If strHKID.Length < 8 Or strHKID.Length > 9 Then
                    strMsgCode = "00201"
                ElseIf strHKID.Length = 8 Or strHKID.Length = 9 Then
                    If strHKID.Substring(0, 1) < "A" Or strHKID.Substring(0, 1) > "Z" Then
                        strMsgCode = "00201"
                    End If

                    If strHKID.Length = 9 Then
                        If strHKID.Substring(1, 1) < "A" Or strHKID.Substring(1, 1) > "Z" Then
                            strMsgCode = "00201"
                        End If
                    End If

                    ' Check Digit Value (Exclude Leading Letter and Tailing Check Digit)
                    Dim intDigitCheck As Integer = IIf(strHKID.Length = 8, 1, 2)
                    For i As Integer = intDigitCheck To strHKID.Length - 2
                        If strHKID(i) < "0" OrElse strHKID(i) > "9" Then
                            strMsgCode = "00201"
                            Exit For
                        End If
                    Next

                End If

                If strMsgCode = "" Then
                    ' to add a space into the HKID input whenever its first 2 characters are not alphabets
                    If strHKID.Length = 8 Or strHKID.Length = 10 Then
                        strHKID = " " + strHKID
                    End If

                    ByteArray = AE.GetBytes(strHKID)

                    ' to calculate the checksumby summing the digits multiplied by their weights
                    If String.Compare(" ", strHKID.Substring(0, 1)) = 0 Then
                        intDigitSum = 36 * 9
                    Else
                        intDigitSum = CType((ByteArray(0) - ByteASCIIZero(1) + 10) * 9, Integer)
                    End If

                    intDigitSum = CType((ByteArray(1) - ByteASCIIZero(1) + 10) * 8, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(2) - ByteASCIIZero(0)) * 7, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(3) - ByteASCIIZero(0)) * 6, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(4) - ByteASCIIZero(0)) * 5, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(5) - ByteASCIIZero(0)) * 4, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(6) - ByteASCIIZero(0)) * 3, Integer) + intDigitSum
                    intDigitSum = CType((ByteArray(7) - ByteASCIIZero(0)) * 2, Integer) + intDigitSum
                    intDigitSum = 11 - intDigitSum Mod 11

                    ' to compare the digit sum and the check sum
                    If strHKID.Length = 8 Or strHKID.Length = 9 Then
                        If String.Compare(strHKID.Substring(strHKID.Length - 1, 1), strCheckSum.Substring(intDigitSum - 1, 1)) = 0 Then
                            strMsgCode = ""
                        Else
                            strMsgCode = "00201"
                        End If
                    End If
                End If

            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        'Document Type = Hong Kong Re-entry Permit
        Public Function chkReEntryNo(ByVal strReEntryNo As String) As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strReEntryNo = strReEntryNo.Trim.ToUpper

            If IsEmpty(strReEntryNo) Then
                strMsgCode = "00204"
            Else
                ' validate the ReEntry No format
                If strReEntryNo.Length <> 9 Then
                    strMsgCode = "00202"
                Else
                    Dim number As Integer

                    If Not Me.IsAlphanumeric(strReEntryNo.Substring(0, 2)) Then
                        strMsgCode = "00202"
                    End If


                    If strMsgCode.Equals(String.Empty) Then

                        If Not Integer.TryParse(strReEntryNo.Substring(2, 7), number) Then
                            strMsgCode = "00202"
                        End If

                    End If
                End If
            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        'Document Type = Permit to Remain in HKSAR (ID 235B)
        Public Function chkBirthEntryNo(ByVal strBirthEntryNo As String) As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strBirthEntryNo = strBirthEntryNo.Trim.ToUpper

            If IsEmpty(strBirthEntryNo) Then
                strMsgCode = "00205"
            Else
                ' validate the ReEntry No format
                If strBirthEntryNo.Length <> 8 Then
                    strMsgCode = "00206"
                Else
                    Dim number As Integer

                    If Not Me.IsAlphanumeric(strBirthEntryNo.Substring(0, 2)) Then
                        strMsgCode = "00206"
                    End If


                    If strMsgCode.Equals(String.Empty) Then

                        If Not Integer.TryParse(strBirthEntryNo.Substring(2, 6), number) Then
                            strMsgCode = "00206"
                        End If

                    End If

                End If
            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Document Type = Certificate issued by the Births and Deaths Registry for adopted children
        'Public Function chkNoOfEntry(ByVal strNoOfEntry As String) As ComObject.SystemMessage
        Public Function chkNoOfEntry(ByVal strIdentityNum As String, ByVal strIdentityNumPrefix As String) As ComObject.SystemMessage
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strIdentityNum = strIdentityNum.Trim.ToUpper
            strIdentityNumPrefix = strIdentityNumPrefix.Trim.ToUpper

            Dim strNoOfEntry As String = String.Format("{0}{1}", strIdentityNum, strIdentityNumPrefix)

            strNoOfEntry = strNoOfEntry.Trim.ToUpper
            strNoOfEntry = strNoOfEntry.Replace("/", String.Empty)

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'If IsEmpty(strNoOfEntry) Then
            If IsEmpty(strIdentityNum) OrElse IsEmpty(strIdentityNumPrefix) Then
                strMsgCode = "00209"
            Else
                ' validate the ReEntry No format
                'If strNoOfEntry.Length <> 12 Then
                If strIdentityNum.Length <> 5 OrElse strIdentityNumPrefix.Length <> 7 Then
                    strMsgCode = "00210"
                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]
                Else
                    Dim number As Integer

                    If Not Me.IsAlphanumeric(strNoOfEntry.Substring(5, 1)) Then
                        strMsgCode = "00210"
                    End If

                    If strMsgCode.Equals(String.Empty) Then

                        If Not Integer.TryParse(strNoOfEntry.Substring(0, 5), number) OrElse Not Integer.TryParse(strNoOfEntry.Substring(6, 6), number) Then
                            strMsgCode = "00210"
                        End If

                    End If


                End If
            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        'Document Type = Non-Hong Kong Travel Documents
        Public Function chkVisaNo(ByVal strVisaNo As String) As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strVisaNo = strVisaNo.Trim.ToUpper
            strVisaNo = strVisaNo.Replace("-", String.Empty)
            strVisaNo = strVisaNo.Replace("(", String.Empty)
            strVisaNo = strVisaNo.Replace(")", String.Empty)

            If IsEmpty(strVisaNo) Then
                strMsgCode = "00207"
            Else
                ' validate the ReEntry No format
                If strVisaNo.Length <> 14 Then
                    strMsgCode = "00208"
                Else
                    Dim number As Integer

                    If Not Me.IsAlphanumeric(strVisaNo.Substring(0, 4)) Then
                        strMsgCode = "00208"
                    End If


                    If strMsgCode.Equals(String.Empty) Then

                        If Not Integer.TryParse(strVisaNo.Substring(4, 9), number) Then
                            strMsgCode = "00208"
                        End If

                    End If

                    If strMsgCode.Equals(String.Empty) Then

                        If Not Me.IsAlphanumeric(strVisaNo.Substring(13, 1)) Then
                            strMsgCode = "00208"
                        End If

                    End If

                End If
            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        'Document Type = Document of Identity
        Public Function chkDocumentNo(ByVal strDocumentNo As String) As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strDocumentNo = strDocumentNo.Trim.ToUpper

            If IsEmpty(strDocumentNo) Then
                strMsgCode = "00211"
            Else
                ' validate the ReEntry No format
                If strDocumentNo.Length <> 9 Then
                    strMsgCode = "00212"
                Else
                    If Not Me.IsAlphanumeric(strDocumentNo) Then
                        strMsgCode = "00212"
                    End If

                    'For Each strDocumentNoSubString As String In strDocumentNo.ToCharArray()
                    '    If strDocumentNoSubString < "A" OrElse strDocumentNoSubString > "Z" Then
                    '        strMsgCode = "00212"
                    '        Exit For
                    '    End If
                    'Next
                End If
            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Document Type = Non EHS Doc Type
        Public Function chkDocumentNoForNonEHSDocType(ByVal strDocumentNo As String) As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = ""

            strDocumentNo = strDocumentNo.Trim.ToUpper

            If IsEmpty(strDocumentNo) Then
                strMsgCode = "00211"

            End If

            If strMsgCode = "" Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function
        ' CRE19-001 (VSS 2019) [End][Winnie]
#End Region

        '---------------------------------------------------------------------------------------------------------
        'chkVoucherRedeem is new for 19 OCT 20009
        '---------------------------------------------------------------------------------------------------------
        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Add Quota
        'Public Function chkVoucherRedeem(ByVal strVoucherRedeem As String, ByVal intAvailVoucher As Integer, ByVal dtmServiceDate As DateTime, Optional ByVal intMaxVoucher As Integer = -1, Optional ByRef strMsgParam As String = "") As ComObject.SystemMessage
        Public Function chkVoucherRedeem(ByVal strVoucherRedeem As String, ByVal intAvailVoucher As Integer, ByVal udtVoucherQuota As VoucherInfo.VoucherQuotaModel, ByVal dtmServiceDate As DateTime, Optional ByVal intMaxVoucher As Integer = -1, Optional ByRef strMsgParam As String = "") As ComObject.SystemMessage
            ' CRE19-003 (Opt voucher capping) [End][Winnie]

            Dim systemMessage As ComObject.SystemMessage = Nothing
            Dim intVoucherRedeem As Integer = 0

            'if radio button selected index is more then 6 and voucher redeem is not entered
            If strVoucherRedeem.Equals(String.Empty) Then
                systemMessage = New ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00122)
            Else
                If Integer.TryParse(strVoucherRedeem, intVoucherRedeem) Then

                    If intVoucherRedeem < 1 Then
                        'The "Voucher Amount Claimed" should be greater than $0.
                        systemMessage = New ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00336)

                    ElseIf intMaxVoucher >= 0 AndAlso intVoucherRedeem > intMaxVoucher Then
                        'The "Voucher Amount Claimed" cannot be greater than maximum voucher amount "%s".
                        systemMessage = New ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00331)
                        strMsgParam = (New Common.Format.Formatter).formatMoney(intMaxVoucher.ToString(), True)

                    ElseIf intVoucherRedeem > intAvailVoucher Then
                        'The "Voucher Amount Claimed" cannot be greater than "Available Voucher Amount".
                        systemMessage = New ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00123)

                    ElseIf IsVoucherAmountRedeemMultiplier(intVoucherRedeem, dtmServiceDate, Common.Component.Scheme.SchemeClaimModel.HCVS) = False Then
                        'The "Voucher Amount Claimed" should be a multiple of $50 for transactions with service date before 01 Jul 2014.
                        systemMessage = New ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00314)

                        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                    ElseIf Not udtVoucherQuota Is Nothing AndAlso intVoucherRedeem > udtVoucherQuota.AvailableQuota Then
                        'The "Voucher Amount Claimed" cannot be greater than Available Quota on Optometry Services.
                        systemMessage = New ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00425)

                        Dim strMsg_en = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                                                      , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New System.Globalization.CultureInfo(CultureLanguage.English)))

                        Dim strMsg_tc = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) _
                                                      , HttpContext.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode, New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))

                        systemMessage.AddReplaceMessage("%en", strMsg_en)
                        systemMessage.AddReplaceMessage("%tc", strMsg_tc)
                        ' CRE19-003 (Opt voucher capping) [End][Winnie]

                    End If
                Else
                    'The "Voucher Amount Claimed" is invalid.
                    systemMessage = New ComObject.SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00124)
                End If
            End If

            Return systemMessage
        End Function

        'CRE13-019 China Voucher [Start][Karl]
        Public Function chkVoucherRedeemRMB(ByVal strVoucherRedeem As String, ByVal dblAvailVoucher As Decimal, ByVal dtmServiceDate As DateTime, _
                                            Optional ByVal blnCheckingHKD As Boolean = False, Optional ByVal dblMaxVoucher As Decimal = -1, Optional ByRef strMsgParam As String = "") As ComObject.SystemMessage

            Dim systemMessage As ComObject.SystemMessage = Nothing
            Dim ldblVoucherRedeem As Decimal = 0
            Dim lblnValid As Boolean = False
            Dim ldblLowerLimit As Decimal = Nothing

            If strVoucherRedeem.Equals(String.Empty) Then
                systemMessage = New ComObject.SystemMessage("990000", "E", "00122")
            Else
                If blnCheckingHKD = False Then
                    If Decimal.TryParse(strVoucherRedeem, ldblVoucherRedeem) Then
                        lblnValid = True
                        ldblLowerLimit = 0.01
                    End If
                Else
                    If Integer.TryParse(strVoucherRedeem, ldblVoucherRedeem) Then
                        lblnValid = True
                        ldblLowerLimit = 1
                    End If
                End If

                If lblnValid = True Then

                    'check no. of decimal place                    
                    Dim intIndexOfDecimalPoint As Integer = ldblVoucherRedeem.ToString.IndexOf(".")
                    Dim intNumberOfDecimals As Integer = _
                        ldblVoucherRedeem.ToString.Substring(intIndexOfDecimalPoint + 1).Length

                    If blnCheckingHKD = False AndAlso intIndexOfDecimalPoint > -1 AndAlso intNumberOfDecimals > 2 Then
                        systemMessage = New ComObject.SystemMessage("990000", "E", "00352")
                    ElseIf ldblVoucherRedeem < ldblLowerLimit Then
                        If blnCheckingHKD = False Then
                            systemMessage = New ComObject.SystemMessage("990000", "E", "00350")
                        Else
                            systemMessage = New ComObject.SystemMessage("990000", "E", "00351")
                        End If
                    ElseIf dblMaxVoucher >= 0 AndAlso ldblVoucherRedeem > dblMaxVoucher Then
                        systemMessage = New ComObject.SystemMessage("990000", "E", "00331")
                        strMsgParam = (New Common.Format.Formatter).formatMoneyRMB(dblMaxVoucher.ToString(), True)
                    ElseIf ldblVoucherRedeem > dblAvailVoucher Then
                        systemMessage = New ComObject.SystemMessage("990000", "E", "00123")

                        'China Voucher launched after [change voucher amount to $1], hence, no need to consider this case
                        'ElseIf IsVoucherAmountRedeemMultiplier(ldblVoucherRedeem, dtmServiceDate, Common.Component.Scheme.SchemeClaimModel.HCVS) = False Then
                        '    systemMessage = New ComObject.SystemMessage("990000", "E", "00314") 'The "Voucher Amount Claimed" should be a multiplier of $50                        
                    End If
                Else
                    systemMessage = New ComObject.SystemMessage("990000", "E", "00124")
                End If
            End If

            Return systemMessage
        End Function

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        Public Function IsVoucherAmountRedeemMultiplier(ByVal strVoucherRedeem As String, ByVal dtmServiceDate As DateTime, ByVal strSchcmeCode As String) As Boolean
            Dim blnIsMultiplier As Boolean = True
            Dim intVoucherRedeem As Integer = 0
            Dim strRestructMultiplierDate As String = String.Empty
            Dim intMulitipierValue As Integer = 50

            If Not strVoucherRedeem.Equals(String.Empty) Then

                If IsNumeric(strVoucherRedeem) = True Then
                    intVoucherRedeem = CInt(strVoucherRedeem)
                Else
                    blnIsMultiplier = False
                    Throw New Exception("Validator.IsVoucherAmountRedeemMultiplier: strVoucherRedeem is not numeric ")
                End If

                Me.udtcomfunct.getSystemParameter("DateBackClaimHCVSRestrict50MultiplierDate", strRestructMultiplierDate, Nothing, strSchcmeCode)

                If Not strRestructMultiplierDate.Equals(String.Empty) Then
                    Dim dtmCompareDate As DateTime = DateTime.ParseExact(strRestructMultiplierDate.Trim, "yyyyMMMdd", New System.Globalization.CultureInfo("en-US"))

                    If DateDiff(DateInterval.Day, dtmServiceDate, dtmCompareDate) > 0 Then

                        If ((intVoucherRedeem Mod intMulitipierValue) > 0) Or (intVoucherRedeem < intMulitipierValue) Then
                            blnIsMultiplier = False
                        End If

                    End If
                End If
            End If

            Return blnIsMultiplier
        End Function

        Public Function chkEHAPPHCVCopayment(ByVal strHCVAmt As String, ByVal strMaxTotalAmt As String, ByVal dtmServiceDate As DateTime, ByRef strMsgParam As String) As ComObject.SystemMessage
            Dim udtGenFunc As New Common.ComFunction.GeneralFunction
            Dim systemMessage As ComObject.SystemMessage = Nothing 'use array to provide the flexibily of adding validation for CopayAmt
            Dim intHCVAmt As Integer = 0

            If String.IsNullOrEmpty(strMaxTotalAmt) = True OrElse IsNumeric(strMaxTotalAmt) = False Then
                Throw New Exception("Validator.chkEHAPPHCVCopayment: strMaxCopaymentAmt is invalid ")
            End If

            'Check HCV Amount
            If String.IsNullOrEmpty(strHCVAmt) = True Then
                systemMessage = New ComObject.SystemMessage("990000", "E", "00332")
            Else
                If Integer.TryParse(strHCVAmt, intHCVAmt) Then
                    If intHCVAmt < 1 Then
                        systemMessage = New ComObject.SystemMessage("990000", "E", "00335")
                    Else
                        If intHCVAmt > CInt(strMaxTotalAmt) Then
                            systemMessage = New ComObject.SystemMessage("990000", "E", "00334")
                            strMsgParam = (New Common.Format.Formatter).formatMoney(strMaxTotalAmt, False)
                        Else
                            If IsVoucherAmountRedeemMultiplier(intHCVAmt, dtmServiceDate, Common.Component.Scheme.SchemeClaimModel.EHAPP) = False Then
                                systemMessage = New ComObject.SystemMessage("990000", "E", "00342") 'The "HCV Amount" should be a multiplier of $50
                            End If
                        End If
                    End If
                Else
                    systemMessage = New ComObject.SystemMessage("990000", "E", "00333")
                End If
            End If

            Return systemMessage

        End Function

        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        Public Function chkCoPaymentFee(ByVal strCoPaymentFee As String, ByRef strMsgParam As String) As ComObject.SystemMessage
            Dim udtGenFunc As New Common.ComFunction.GeneralFunction
            Dim systemMessage As ComObject.SystemMessage = Nothing
            Dim intCoPaymentFee As Integer = 0
            Dim intLowerLimit As Integer = 0
            Dim intUpperLimit As Integer = 0
            udtGenFunc.GetCoPaymentFee(intLowerLimit, intUpperLimit)

            If strCoPaymentFee.Equals(String.Empty) Then
                'systemMessage = New ComObject.SystemMessage("990000", "E", "00309")
            Else
                If Integer.TryParse(strCoPaymentFee, intCoPaymentFee) Then
                    If intCoPaymentFee < intLowerLimit Then
                        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                        'strMsgParam = intLowerLimit
                        strMsgParam = (New Common.Format.Formatter).formatMoney(intLowerLimit, False)
                        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                        systemMessage = New ComObject.SystemMessage("990000", "E", "00310")
                    ElseIf intCoPaymentFee > intUpperLimit Then
                        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
                        'strMsgParam = intUpperLimit
                        'CRE13-019 China Voucher [Start][Karl]
                        strMsgParam = (New Common.Format.Formatter).formatMoney(intUpperLimit, True)
                        'CRE13-019 China Voucher [End][Karl]
                        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
                        systemMessage = New ComObject.SystemMessage("990000", "E", "00311")
                    End If
                Else
                    systemMessage = New ComObject.SystemMessage("990000", "E", "00313")
                End If
            End If

            Return systemMessage
        End Function


#Region "Supporting Function"

        Private Function chkValidInputDateFormat(ByRef strOriDOB As String, ByRef strInputDOBFormat As String) As Boolean

            Dim udtFormatter As New Format.Formatter()
            Dim strDOBs As String() = strOriDOB.Split("-")

            strInputDOBFormat = String.Empty

            Select Case strDOBs.Length
                Case 1  ' DDMMYYYY / MMYYYY / YYYY
                    If (strDOBs.GetValue(0).ToString.Length = 8) Then
                        strOriDOB = String.Format("{0}-{1}-{2}", strOriDOB.Substring(0, 2), strOriDOB.Substring(2, 2), strOriDOB.Substring(4, 4))
                        strInputDOBFormat = "D"
                    ElseIf (strDOBs.GetValue(0).ToString.Length = 6) Then
                        strOriDOB = String.Format("01-{0}-{1}", strOriDOB.Substring(0, 2), strOriDOB.Substring(2, 4))
                        strInputDOBFormat = "M"
                    ElseIf (strDOBs.GetValue(0).ToString.Length = 4) Then
                        strOriDOB = String.Format("01-01-{0}", strOriDOB)
                        strInputDOBFormat = "Y"
                    Else
                        Return False
                    End If
                Case 2  ' MM-YYYY
                    ' Pad MM:0
                    If strDOBs.GetValue(0).ToString().Length = 1 Then
                        strDOBs(0) = strDOBs.GetValue(0).ToString().PadLeft(2, "0")
                    End If

                    If strDOBs.GetValue(1).ToString.Length <> 4 Then
                        Return False
                    Else
                        strOriDOB = String.Format("01-{0}-{1}", strDOBs.GetValue(0), strDOBs.GetValue(1))
                    End If

                    strInputDOBFormat = "M"

                Case 3  ' DD-MM-YYYY
                    ' Pad DD:0
                    If strDOBs.GetValue(0).ToString().Length = 1 Then
                        strDOBs(0) = strDOBs.GetValue(0).ToString().PadLeft(2, "0")
                    End If

                    ' Pad MM:0
                    If strDOBs.GetValue(1).ToString().Length = 1 Then
                        strDOBs(1) = strDOBs.GetValue(1).ToString().PadLeft(2, "0")
                    End If

                    If (strDOBs.GetValue(1).ToString.Length <> 2 Or strDOBs.GetValue(2).ToString.Length <> 4) Then
                        Return False
                    Else
                        strOriDOB = String.Format("{0}-{1}-{2}", strDOBs.GetValue(0), strDOBs.GetValue(1), strDOBs.GetValue(2))
                    End If

                    'strInputDOBFormat = "Y"
                    strInputDOBFormat = "D"
                Case Else
                    Return False
            End Select

            If strInputDOBFormat.Trim() <> "" Then
                If Not IsDate(udtFormatter.convertDate(strOriDOB, "E")) Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If

        End Function

        Private Function chkIsFutureDate(ByVal dtmToday As Date, ByVal dtmReferenceDate As Date) As Boolean
            If dtmToday < dtmReferenceDate Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function chkValidIssueDateVsDOB(ByVal dtmDOI As Date, ByVal dtmDOB As Date) As Boolean
            If dtmDOI < dtmDOB Then
                Return False
            Else
                Return True
            End If
        End Function

        ''' <summary>
        ''' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox
        ''' Check if the input date with year, month and day is a valid date
        ''' </summary>
        ''' <param name="intYear"></param>
        ''' <param name="intMonth"></param>
        ''' <param name="intDay"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValidDateValue(ByVal intYear As Integer, ByVal intMonth As Integer, ByVal intDay As Integer) As Boolean
            Dim dtmDate As DateTime

            Try
                dtmDate = New DateTime(intYear, intMonth, intDay)
            Catch ex As Exception
                Return False
            End Try

            Return True
        End Function

        ''' <summary>
        ''' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox
        ''' Check if the input date with month and day is a valid date
        ''' </summary>
        ''' <param name="intMonth"></param>
        ''' <param name="intDay"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValidDateValue(ByVal intMonth As Integer, ByVal intDay As Integer) As Boolean
            Dim intDayMax As Integer

            Select Case intMonth
                Case 1, 3, 5, 7, 8, 10, 12
                    intDayMax = 31

                Case 4, 6, 9, 11
                    intDayMax = 30

                Case 2
                    intDayMax = 29

                Case Else
                    Return False

            End Select

            If intDay > intDayMax Then
                Return False
            Else
                Return True
            End If
        End Function
#End Region

#Region "DOB with Document Type"

        Private Function chkDOB_AgeLimit(ByVal dtmToday As Date, ByVal dtmDOB As Date) As ComObject.SystemMessage

            Dim udtSystemMessage As ComObject.SystemMessage = Nothing
            Dim strFunctionCode As String = "990000"
            Dim strServerityCode As String = "E"
            Dim strMsgCode As String = String.Empty


            Dim strParam_1 As String = String.Empty
            Dim strParam_2 As String = String.Empty
            Me.udtcomfunct.getSystemParameter("MaxDOBAge", strParam_1, strParam_2)

            Dim intAge As Integer = dtmDOB.Year
            Dim intMaxAge As Integer = 150

            If Not strParam_1.Equals(String.Empty) Then
                intMaxAge = CInt(strParam_1)
            End If
            ' DOB Invalid
            If dtmToday.Year - intAge > intMaxAge Then
                strMsgCode = "00004"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                udtSystemMessage = New ComObject.SystemMessage(strFunctionCode, strServerityCode, strMsgCode)
            End If

            Return udtSystemMessage

        End Function

        Private Function chkDOB_Future(ByVal dtmToday As Date, ByVal dtmDOB As Date) As ComObject.SystemMessage
            Dim udtSystemMessage As ComObject.SystemMessage = Nothing
            Dim strFunctionCode As String = "990000"
            Dim strServerityCode As String = "E"
            Dim strMsgCode As String = String.Empty

            If Me.chkIsFutureDate(dtmToday, dtmDOB) Then
                strMsgCode = "00221"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                udtSystemMessage = New ComObject.SystemMessage(strFunctionCode, strServerityCode, strMsgCode)
            End If

            Return udtSystemMessage
        End Function

        Private Function chkDOB_Format(ByVal strOriDOB As String, ByRef strInputDOBFormat As String, ByRef dtmDOB As Date) As ComObject.SystemMessage

            Dim udtSystemMessage As ComObject.SystemMessage = Nothing

            Dim strFunctionCode As String = "990000"
            Dim strServerityCode As String = "E"
            Dim strMsgCode As String = String.Empty

            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter

            ' Handle 1-1-2000
            ' Handle 20-3-2000

            If strOriDOB.Trim().Equals(String.Empty) Then
                strMsgCode = "00003"
            End If

            Dim strDOBs As String() = strOriDOB.Split("-")

            ' ------------------------------------
            ' Convert Date Format
            ' ------------------------------------

            If strMsgCode = String.Empty Then
                If Not Me.chkValidInputDateFormat(strOriDOB, strInputDOBFormat) Then
                    strMsgCode = "00004"
                End If
            End If

            ' ------------------------------------
            ' Is Date
            ' ------------------------------------
            If strMsgCode = String.Empty Then
                If Not IsDate(formatter.convertDate(strOriDOB, "E")) Then
                    strMsgCode = "00004"
                End If
            End If

            ' ------------------------------------
            ' Convert to Date
            ' ------------------------------------
            If strMsgCode = String.Empty Then
                dtmDOB = Convert.ToDateTime(formatter.convertDate(strOriDOB, "E"))
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                udtSystemMessage = New ComObject.SystemMessage(strFunctionCode, strServerityCode, strMsgCode)
            End If

            Return udtSystemMessage

        End Function

        ''' <summary>
        ''' Check the DOB format according to different Document
        ''' ByRef return the DOB in date and input format (Y=YYYY) (M=MM-YYYY) (D=DD-MM-YYYY)
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDOB"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strInputDOBFormat"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkDOB(ByVal strDocCode As String, ByVal strDOB As String, ByRef dtmDOB As Date, ByRef strInputDOBFormat As String) As ComObject.SystemMessage
            Dim strFunctionCode As String = "990000"
            Dim strServerityCode As String = "E"
            Dim strMsgCode As String = String.Empty

            Dim udtSystemMessage As ComObject.SystemMessage = Nothing
            Dim dtmToday As DateTime = Me.udtcomfunct.GetSystemDateTime()

            strInputDOBFormat = String.Empty
            ' -----------------------------------'
            ' Common Checking
            ' -----------------------------------'

            ' Format
            udtSystemMessage = Me.chkDOB_Format(strDOB, strInputDOBFormat, dtmDOB)

            ' Future Date
            If udtSystemMessage Is Nothing AndAlso strMsgCode = String.Empty Then
                udtSystemMessage = Me.chkDOB_Future(dtmToday, dtmDOB)
            End If

            ' Age Limit
            If udtSystemMessage Is Nothing AndAlso strMsgCode = String.Empty Then
                udtSystemMessage = Me.chkDOB_AgeLimit(dtmToday, dtmDOB)
            End If

            ' Specific Document Checking
            If udtSystemMessage Is Nothing AndAlso strMsgCode = String.Empty Then

                Select Case strDocCode.Trim()

                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Case DocTypeModel.DocTypeCode.VISA,
                        DocTypeModel.DocTypeCode.ID235B,
                        DocTypeModel.DocTypeCode.OC,
                        DocTypeModel.DocTypeCode.OW,
                        DocTypeModel.DocTypeCode.IR,
                        DocTypeModel.DocTypeCode.HKP,
                        DocTypeModel.DocTypeCode.RFNo8,
                        DocTypeModel.DocTypeCode.OTHER
                        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

                        If Not strInputDOBFormat.Trim().ToUpper().Equals("D") Then
                            strMsgCode = "00004"
                        End If

                End Select
            End If

            If udtSystemMessage Is Nothing And Not strMsgCode.Equals(String.Empty) Then
                udtSystemMessage = New ComObject.SystemMessage(strFunctionCode, strServerityCode, strMsgCode)
            End If

            Return udtSystemMessage
        End Function

        ''' <summary>
        ''' Check the DOB format according to different Document
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDOB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkDOB(ByVal strDocCode As String, ByVal strDOB As String) As ComObject.SystemMessage
            Dim dtmDOB As Date = DateTime.MaxValue
            Dim strInputDOBFormat As String = String.Empty
            Return Me.chkDOB(strDocCode, strDOB, dtmDOB, strInputDOBFormat)
        End Function

#Region "Valdation for DOB"


        Public Function chkDOB_New(ByVal strOriDOB As String) As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim strMsgCode As String
            Dim strDOBs As String() = strOriDOB.Split("-")
            Dim dtToday As DateTime
            dtToday = udtcomfunct.GetSystemDateTime

            ' Handle 1-1-2000
            ' Handle 20-3-2000

            strMsgCode = String.Empty

            If IsEmpty(strOriDOB) Then
                strMsgCode = "00003"
            Else
                If strDOBs.Length = 1 Then
                    If (strDOBs.GetValue(0).ToString.Length = 8) Then
                        strOriDOB = String.Format("{0}-{1}-{2}", strOriDOB.Substring(0, 2), strOriDOB.Substring(2, 2), strOriDOB.Substring(4, 4))
                    ElseIf (strDOBs.GetValue(0).ToString.Length = 6) Then
                        strOriDOB = String.Format("01-{0}-{1}", strOriDOB.Substring(0, 2), strOriDOB.Substring(2, 4))
                    ElseIf (strDOBs.GetValue(0).ToString.Length = 4) Then
                        strOriDOB = String.Format("01-01-{0}", strOriDOB)
                    Else
                        strMsgCode = "00004"
                    End If
                Else

                    If strDOBs.GetValue(0).ToString().Length = 1 Then
                        strDOBs(0) = strDOBs.GetValue(0).ToString().PadLeft(2, "0")
                    End If

                    If (strDOBs.GetValue(0).ToString.Length <> 2) Then
                        strMsgCode = "00004"
                    Else
                        'strDOBs.Length = 2 -> 011984 -> month = 01, year = 1984
                        If strDOBs.Length = 2 Then
                            If strDOBs.GetValue(1).ToString.Length <> 4 Then
                                strMsgCode = "00004"
                            Else
                                strOriDOB = String.Format("01-{0}-{1}", strDOBs.GetValue(0), strDOBs.GetValue(1))
                            End If
                        ElseIf strDOBs.Length = 3 Then
                            'strDOBs.Length = 2 -> 01011984 -> day = 01, month = 01, year = 1984

                            If strDOBs.GetValue(1).ToString().Length = 1 Then
                                strDOBs(1) = strDOBs.GetValue(1).ToString().PadLeft(2, "0")
                            End If

                            If (strDOBs.GetValue(1).ToString.Length <> 2 Or strDOBs.GetValue(2).ToString.Length <> 4) Then
                                strMsgCode = "00004"
                            Else
                                strOriDOB = String.Format("{0}-{1}-{2}", strDOBs.GetValue(0), strDOBs.GetValue(1), strDOBs.GetValue(2))
                            End If
                        ElseIf strDOBs.Length > 3 Then
                            strMsgCode = "00004"
                        End If
                    End If
                End If

                If strMsgCode = "" Then
                    If Not IsDate(formatter.convertDate(strOriDOB, "E")) Then
                        strMsgCode = "00004"
                    Else
                        Dim dtDOB As DateTime = Convert.ToDateTime(formatter.convertDate(strOriDOB, "E"))
                        If dtToday < dtDOB Then
                            strMsgCode = "00004"
                        Else
                            Dim systemMessage As ComObject.SystemMessage
                            systemMessage = Me.chkDOB_AgeLimit(strOriDOB)
                            If Not systemMessage Is Nothing Then
                                strMsgCode = systemMessage.MessageCode
                            End If
                        End If
                    End If
                End If
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New ComObject.SystemMessage("990000", "E", strMsgCode)
            Else
                Return Nothing
            End If
        End Function

        Public Function chkDOB_AgeLimit(ByVal strOriDOB As String) As ComObject.SystemMessage
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
            Dim intAge, intMaxAge As Integer
            Dim strParam_1 As String = String.Empty
            Dim strParam_2 As String = String.Empty
            Dim dtToday As DateTime = udtcomfunct.GetSystemDateTime
            Dim strMsgCode As String = String.Empty

            intAge = CDate(formatter.convertDate(strOriDOB, "E")).Year

            udtcomfunct.getSystemParameter("MaxDOBAge", strParam_1, strParam_2)
            If strParam_1.Equals(String.Empty) Then
                intMaxAge = 150
            Else
                intMaxAge = CInt(strParam_1)
            End If

            If dtToday.Year - intAge > intMaxAge Then
                strMsgCode = "00004"
            End If

            If Not strMsgCode.Equals(String.Empty) Then
                Return New ComObject.SystemMessage("990000", "E", strMsgCode)
            Else
                Return Nothing
            End If
        End Function

#End Region

#End Region

#Region "Claim Related Checking"

        Public Function ChkServiceDatePermitToRemainUntil(ByVal dtmServiceDate As Date, ByVal dtmPermitToRemainUntil As Nullable(Of Date)) As ComObject.SystemMessage
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
            If Not dtmPermitToRemainUntil.HasValue Then Return Nothing
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

            Dim udtSystemMessage As ComObject.SystemMessage = Nothing
            Dim strFunctionCode As String = "990000"
            Dim strServerityCode As String = "E"
            Dim strMsgCode As String = String.Empty

            If dtmPermitToRemainUntil < dtmServiceDate Then
                strMsgCode = "00230"
            End If
            If Not strMsgCode.Equals(String.Empty) Then
                udtSystemMessage = New ComObject.SystemMessage(strFunctionCode, strServerityCode, strMsgCode)
            End If
            Return udtSystemMessage

        End Function

#End Region

#Region "Date Of Issue"

        ' -------------- FOR HKID Date Of Issue ---------------------

        Public Function isValidDateOfIssueFormat(ByVal strDateOfIssue As String) As String

            Dim strMessage As String = String.Empty
            Dim blnRes As Boolean = False
            Dim blnFuture As Boolean = False
            Dim strTemp As String
            Dim strDay, strMonth, strYear As String
            Dim udtformatter As Format.Formatter = New Format.Formatter
            Dim dtDate As Date
            Dim dtToday As DateTime
            dtToday = udtcomfunct.GetSystemDateTime

            strDay = String.Empty
            strMonth = String.Empty
            strYear = String.Empty

            strTemp = strDateOfIssue.Trim
            If strTemp.Length <> 8 Then
                blnRes = False
            Else
                If chkIsValidDateChar(strDateOfIssue) Then
                    If Not strTemp.Substring(2, 1).Equals("-") Or Not strTemp.Substring(5, 1).Equals("-") Then
                        blnRes = False
                    Else
                        strDay = strTemp.Substring(0, 2)
                        strMonth = strTemp.Substring(3, 2)
                        strMonth = udtformatter.convertMonthNumtoEng(strMonth)

                        If Not strMonth.Equals(String.Empty) Then
                            strYear = strTemp.Substring(6, 2)
                            If CInt(strYear) > 60 Then
                                strYear = "19" + strYear
                            Else
                                strYear = "20" + strYear
                            End If

                            strTemp = strDay + " " + strMonth + " " + strYear

                            Try
                                dtDate = CType(strTemp, Date)
                                If dtDate > dtToday Then
                                    blnRes = False
                                    blnFuture = True
                                Else
                                    blnRes = True
                                End If

                            Catch ex As Exception
                                blnRes = False
                            End Try
                        End If

                    End If
                Else
                    blnRes = False
                End If

            End If

            If blnRes = False Then
                If blnFuture Then
                    strMessage = "00231"
                Else
                    strMessage = "00064"
                End If
            Else
                strMessage = String.Empty
            End If
            Return strMessage
        End Function

        Public Function chkHKIDIssueDateEmpty(ByVal strOriString As String, Optional ByVal bShortMessage As Boolean = False) As ComObject.SystemMessage
            Dim sm As ComObject.SystemMessage
            Dim strFunctCode, strSeverity, strMsgCode As String
            Dim blnRes As Boolean = False
            strFunctCode = "990000"
            strSeverity = "E"
            strMsgCode = String.Empty

            If strOriString.Trim.Equals(String.Empty) Then
                If bShortMessage = True Then
                    strMsgCode = "00081"
                Else
                    strMsgCode = "00047"
                End If

            Else
                strMsgCode = Me.isValidDateOfIssueFormat(strOriString)
            End If

            If strMsgCode.Equals(String.Empty) Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        Public Function chkHKIDIssueDate(ByVal strOriString As String, ByVal dtmDOB As DateTime, Optional ByVal bShortMessage As Boolean = False) As ComObject.SystemMessage
            Dim udtFormatter As Format.Formatter = New Format.Formatter()
            Dim systemMessage As Common.ComObject.SystemMessage = Nothing
            Dim dtmIssueDate As DateTime

            systemMessage = chkHKIDIssueDateEmpty(strOriString, bShortMessage)
            If systemMessage Is Nothing Then
                dtmIssueDate = udtFormatter.convertHKIDIssueDateStringToDate(strOriString)
                If dtmIssueDate < dtmDOB Then
                    systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00228")
                End If
            End If

            Return systemMessage
        End Function

        ' -------------- Other Date Of Issue ---------------------

        Public Function chkDataOfIssue(ByVal strDocCode As String, ByVal strDate As String, ByVal dtmDOB As DateTime) As ComObject.SystemMessage
            Dim systemMessage As ComObject.SystemMessage = Nothing

            ' CRE20-0022 (Immu record) [Start][Martin]
            Select Case strDocCode
                Case Component.DocType.DocTypeModel.DocTypeCode.HKIC
                    systemMessage = Me.chkHKIDIssueDate(strDate, dtmDOB)

                Case Component.DocType.DocTypeModel.DocTypeCode.CCIC
                    systemMessage = Me.chkHKIDIssueDate(strDate, dtmDOB, True)


                Case Component.DocType.DocTypeModel.DocTypeCode.VISA, _
                    Component.DocType.DocTypeModel.DocTypeCode.ADOPC, _
                    Component.DocType.DocTypeModel.DocTypeCode.DI, _
                    Component.DocType.DocTypeModel.DocTypeCode.HKBC, _
                    Component.DocType.DocTypeModel.DocTypeCode.REPMT, _
                    Component.DocType.DocTypeModel.DocTypeCode.ROP140

                    systemMessage = Me.chkIssueDate_DDMMYYYY(strDate, dtmDOB)

                Case Component.DocType.DocTypeModel.DocTypeCode.ID235B
                    ' CRE20-0022 (Immu record) [End][Martin]
                    ' do not have DOI
            End Select

            Return systemMessage
        End Function

        Public Function chkIssueDate_DDMMYYYY(ByVal strDOI As String, ByVal dtmDOB As DateTime) As ComObject.SystemMessage
            Dim systemMessage As ComObject.SystemMessage = Nothing
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty
            Dim formatter As Format.Formatter = New Format.Formatter()

            Dim strInputFormat As String = String.Empty
            Dim dtmDOI As Date

            If strDOI.Trim().Equals(String.Empty) Then
                strMsgCode = "00081"
            End If

            If strMsgCode = "" Then
                Dim blnValidFormat As Boolean = Me.chkValidInputDateFormat(strDOI, strInputFormat)
                If Not (blnValidFormat AndAlso strInputFormat = "D") Then
                    strMsgCode = "00082"
                End If
            End If


            If strMsgCode = "" Then
                dtmDOI = formatter.convertDate(strDOI, Common.Component.CultureLanguage.English)
            End If

            If strMsgCode = "" Then
                If Me.chkIsFutureDate(Me.udtcomfunct.GetSystemDateTime(), dtmDOI) Then
                    strMsgCode = "00231"
                End If
            End If

            If strMsgCode = "" Then
                If Not Me.chkValidIssueDateVsDOB(dtmDOI, dtmDOB) Then
                    strMsgCode = "00228"
                End If
            End If

            If strMsgCode.Equals(String.Empty) Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm
        End Function

#End Region

#Region "Premet To Remain Until"

        Public Function chkPremitToRemainUntil(ByVal strPermitDate As String, ByVal dtmDOB As DateTime, ByVal strDocCode As String) As ComObject.SystemMessage
            Return chkPremitToRemainUntil(strPermitDate, dtmDOB, strDocCode, True)
        End Function

        Public Function chkPremitToRemainUntil(ByVal strPermitDate As String, ByVal dtmDOB As DateTime, ByVal strDocCode As String, ByVal bChkAllowDateBack As Boolean) As ComObject.SystemMessage
            Dim formatter As Format.Formatter = New Format.Formatter()
            Dim udtCommfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim strAllowDateBack As String = String.Empty
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty
            Dim intDayLimit As Integer
            Dim dtmMinDate As DateTime
            Dim dtmPermitRemain As DateTime
            Dim dtToday As DateTime = udtcomfunct.GetSystemDateTime()
            Dim strMsgCode As String = String.Empty
            Dim strInputFormat As String = String.Empty

            If strPermitDate.Trim.Equals(String.Empty) Then
                strMsgCode = "00218"
            Else
                Dim blnValidFormat As Boolean = Me.chkValidInputDateFormat(strPermitDate, strInputFormat)
                If Not (blnValidFormat AndAlso strInputFormat = "D") Then
                    strMsgCode = "00188"
                Else
                    dtmPermitRemain = formatter.convertDate(strPermitDate, Common.Component.CultureLanguage.English)
                End If

                ' DOB is Pass day and Permit Remain is future Date
                'If strMsgCode.Equals(String.Empty) Then
                '    If dtmPermitRemain < dtmDOB Then
                '        strMsgCode = "00188"
                '    End If
                'End If

                If strMsgCode.Equals(String.Empty) Then

                    If bChkAllowDateBack Then
                        udtCommfunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, String.Empty, strDocCode)
                    Else
                        strAllowDateBack = ""
                    End If

                    If strAllowDateBack = "Y" Then
                        udtCommfunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, String.Empty, strDocCode)
                        udtCommfunct.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, strDocCode)
                        intDayLimit = CInt(strClaimDayLimit)
                        dtmMinDate = Convert.ToDateTime(strMinDate)

                        If dtmPermitRemain.AddDays(intDayLimit) <= dtToday.Date Then
                            strMsgCode = "00229"
                        End If

                        If strMsgCode.Trim() = "" Then
                            If dtmPermitRemain < dtmMinDate Then
                                strMsgCode = "00229"
                            End If
                        End If
                    Else
                        If dtmPermitRemain < dtToday.Date Then
                            strMsgCode = "00229"
                        End If
                    End If
                End If
            End If

            If strMsgCode.Equals(String.Empty) Then
                sm = Nothing
            Else
                sm = New ComObject.SystemMessage("990000", "E", strMsgCode)
            End If
            Return sm
        End Function

        Public Function chkPremitToRemainUntil(ByVal strDate As String, ByVal dtmDOB As DateTime) As ComObject.SystemMessage
            Return chkPremitToRemainUntil(strDate, dtmDOB, Common.Component.DocType.DocTypeModel.DocTypeCode.ID235B)
        End Function
#End Region

        Public Function chkPassportNo(ByVal strPassportNo As String) As ComObject.SystemMessage
            Dim systemMessage As ComObject.SystemMessage = Nothing

            If strPassportNo Is Nothing OrElse strPassportNo.Trim().Equals(String.Empty) Then
                systemMessage = New ComObject.SystemMessage("990000", "E", "00236")
            Else
                strPassportNo = strPassportNo.Trim()
                If Not Me.IsAlphanumeric(strPassportNo) OrElse strPassportNo.Length > 20 Then
                    systemMessage = New ComObject.SystemMessage("990000", "E", "00199")
                End If
            End If

            Return systemMessage
        End Function

        ''' <summary>
        ''' CRE11-007
        ''' Common function to get Document is invalid message code
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' ' is not used
        Public Function GetMessageForIdentityNoIsInvalid(ByVal strDocCode As String) As ComObject.SystemMessage
            Dim strMsgCode As String = Nothing
            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.ADOPC
                    strMsgCode = "00210"
                Case DocTypeModel.DocTypeCode.DI
                    strMsgCode = "00212"
                Case DocTypeModel.DocTypeCode.HKIC, DocTypeModel.DocTypeCode.EC
                    strMsgCode = "00002"
                Case DocTypeModel.DocTypeCode.HKBC
                    strMsgCode = "00201"
                Case DocTypeModel.DocTypeCode.ID235B
                    strMsgCode = "00206"
                Case DocTypeModel.DocTypeCode.REPMT
                    strMsgCode = "00202"
                Case DocTypeModel.DocTypeCode.VISA
                    strMsgCode = "00208"
                Case Else
                    Throw New Exception(String.Format("Validator.GetMessageForIdentityNoIsInvalid: Unhandled DocCode({0})", strDocCode))
            End Select

            Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, _
                                            Common.Component.SeverityCode.SEVE, _
                                            strMsgCode)
        End Function

        ''' <summary>
        ''' CRE11-007
        ''' Common function to get Document is invalid message code
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMessageForIdentityNoIsNoLongerValid(ByVal strDocCode As String) As ComObject.SystemMessage
            Dim strMsgCode As String = Nothing
            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.ADOPC
                    strMsgCode = "00306"
                Case DocTypeModel.DocTypeCode.DI, _
                     DocTypeModel.DocTypeCode.OW, _
                     DocTypeModel.DocTypeCode.TW, _
                     DocTypeModel.DocTypeCode.PASS ' CRE20-0022 (Immu record) [Martin]
                    strMsgCode = "00307"
                Case DocTypeModel.DocTypeCode.HKIC, _
                     DocTypeModel.DocTypeCode.EC, _
                     DocTypeModel.DocTypeCode.CCIC, _
                     DocTypeModel.DocTypeCode.ROP140 ' CRE20-0022 (Immu record) [Martin]
                    strMsgCode = "00301"
                Case DocTypeModel.DocTypeCode.HKBC
                    strMsgCode = "00302"
                Case DocTypeModel.DocTypeCode.ID235B
                    strMsgCode = "00304"
                Case DocTypeModel.DocTypeCode.REPMT
                    strMsgCode = "00303"
                Case DocTypeModel.DocTypeCode.VISA
                    strMsgCode = "00305"
                Case Else
                    Throw New Exception(String.Format("Validator.GetMessageForIdentityNoIsNoLongerValid: Unhandled DocCode({0})", strDocCode))
            End Select

            Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, _
                                            Common.Component.SeverityCode.SEVE, _
                                            strMsgCode)
        End Function

        Public Function chkManualValidation(ByVal strDocCode As String, ByVal _udteHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Nullable(Of Boolean)

            Dim ManualValidate As Nullable(Of Boolean) = Nothing
            Dim udtDocTypeBLL As New DocTypeBLL
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

            Dim udtDocTypeList As DocTypeModelCollection
            udtDocTypeList = udtDocTypeBLL.getAllDocType
            If _udteHSAccountPersonalInfo.Deceased Then
                If _udteHSAccountPersonalInfo.Validating Then
                    ManualValidate = False
                Else
                    ManualValidate = True
                End If
            Else
                Select Case strDocCode
                    Case DocType.DocTypeModel.DocTypeCode.ADOPC
                        If _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                            _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                            _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactYear) OrElse _
                            udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.ADOPC).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                    Case DocType.DocTypeModel.DocTypeCode.DI
                        'Dim dtmPermit_DOI As Date = New Date(2003, 9, 1)
                        Dim strDI_DOI As String = String.Empty
                        udtGeneralFunction.getSystemParameter("DI_DOI", strDI_DOI, String.Empty)

                        Dim dtmDI_DOI As New Date
                        dtmDI_DOI = CDate(strDI_DOI)

                        If _udteHSAccountPersonalInfo.DateofIssue < strDI_DOI OrElse _
                            udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.DI).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                    Case DocType.DocTypeModel.DocTypeCode.EC
                        'Dim dtmEC_DOI As Date = New Date(2003, 6, 23)

                        Dim strEC_DOI As String = String.Empty
                        udtGeneralFunction.getSystemParameter("EC_DOI", strEC_DOI, String.Empty)

                        Dim dtmEC_DOI As New Date
                        dtmEC_DOI = CDate(strEC_DOI)

                        If _udteHSAccountPersonalInfo.DateofIssue < dtmEC_DOI OrElse _
                           _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                           _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                           _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactYear) OrElse _
                           udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.EC).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                    Case DocType.DocTypeModel.DocTypeCode.HKBC
                        If _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                            _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                            _udteHSAccountPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactYear) OrElse _
                            udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.HKBC).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                    Case DocType.DocTypeModel.DocTypeCode.HKIC
                        If udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                    Case DocType.DocTypeModel.DocTypeCode.ID235B
                        If udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.ID235B).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                    Case DocType.DocTypeModel.DocTypeCode.REPMT
                        'Dim dtmPermit_DOI As Date = New Date(2007, 6, 4)
                        Dim strREPMT_DOI As String = String.Empty
                        udtGeneralFunction.getSystemParameter("REPMT_DOI", strREPMT_DOI, String.Empty)

                        Dim dtmPermit_DOI As New Date
                        dtmPermit_DOI = CDate(strREPMT_DOI)

                        If _udteHSAccountPersonalInfo.DateofIssue < dtmPermit_DOI OrElse _
                            udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.REPMT).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                    Case DocType.DocTypeModel.DocTypeCode.VISA
                        If udtDocTypeList.Filter(DocType.DocTypeModel.DocTypeCode.VISA).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If

                        ' CRE20-0022 (Immu record) [Start][Martin]
                    Case DocTypeModel.DocTypeCode.OC,
                        DocTypeModel.DocTypeCode.OW,
                        DocTypeModel.DocTypeCode.TW,
                        DocTypeModel.DocTypeCode.IR,
                        DocTypeModel.DocTypeCode.HKP,
                        DocTypeModel.DocTypeCode.RFNo8,
                        DocTypeModel.DocTypeCode.OTHER,
                        DocTypeModel.DocTypeCode.CCIC,
                        DocTypeModel.DocTypeCode.ROP140,
                        DocTypeModel.DocTypeCode.PASS
                        ' CRE20-0022 (Immu record) [End][Martin]

                        If udtDocTypeList.Filter(strDocCode).ForceManualValidate Then
                            If _udteHSAccountPersonalInfo.Validating Then
                                ManualValidate = False
                            Else
                                ManualValidate = True
                            End If
                        End If
                        ' CRE19-001 (VSS 2019) [End][Winnie]


                End Select

            End If

            Return ManualValidate
        End Function

    End Class
End Namespace