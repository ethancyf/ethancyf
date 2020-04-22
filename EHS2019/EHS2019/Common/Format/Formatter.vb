Imports System.Threading
Imports Common.Component
Imports System.ComponentModel
Imports System.Reflection

Namespace Format

    Public Class Formatter
        Const strDisplayDateFormat As String = "dd MMM yyyy"
        Const strDisplayDateFormatChi As String = "yyyy年MM月dd日"
        Const strDisplayDateFormatCN As String = "yyyy年MM月dd日"

        Const strDisplayDateTimeFormat As String = "dd MMM yyyy HH:mm"
        Const strDisplayDateTimeFormatChi As String = "yyyy年MM月dd日 HH:mm"

        Const strCommonDateFormat As String = "dd-MM-yyyy"
        Const strCommonDBDateFormat As String = "dd MMM yyyy"

        Const strEnterDateFormat As String = "dd-MM-yyyy"
        Const strEnterDateFormatCN As String = "yyyy-MM-dd"
        Const strDisplayIssueDateFormat As String = "dd-MM-yy"
        Const strDisplayIssueDateLongFormat As String = "dd-MM-yyyy"

        Const strDisplayDOBDayFormat As String = "dd-MM-yyyy"
        Const strDisplayDOBDayFormatChi As String = "dd-MM-yyyy"
        Const strDisplayDOBMonthFormat As String = "MM-yyyy"
        Const strDisplayDOBMonthFormatChi As String = "MM-yyyy"
        Const strDisplayDOBYearFormat As String = "yyyy"
        Const strDisplayDOBYearFormatChi As String = "yyyy"

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Const strDisplayClockFormat As String = "dd/MM/yyyy HH:mm:ss"
        Const strDisplayClockTimeFormat As String = "HH:mm:ss"
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public ReadOnly Property EnterDateFormat()
        '    Get
        '        Return strEnterDateFormat
        '    End Get
        'End Property
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Public ReadOnly Property DisplayDateFormat() As String
            Get
                Return strDisplayDateFormat
            End Get
        End Property

        Public ReadOnly Property DisplayDateTimeFormat() As String
            Get
                Return strDisplayDateTimeFormat
            End Get
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public ReadOnly Property DisplayClockFormat() As String
            Get
                Return strDisplayClockFormat
            End Get
        End Property

        Public ReadOnly Property DisplayClockTimeFormat() As String
            Get
                Return strDisplayClockTimeFormat
            End Get
        End Property
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]

        Public Function formatBankPaymentDay(ByVal strInputPaymentDate As String) As String
            Return strInputPaymentDate.Substring(0, 2) & strInputPaymentDate.Substring(3, 2) & strInputPaymentDate.Substring(8, 2)
        End Function

        ''' <summary>
        ''' Format the HKIC No. with blanket for displaying to user. Mask the last four digits is needed if there is any privacy concern.
        ''' </summary>
        ''' <param name="strOriHKID">The un-format validate HKIC No.</param>
        ''' <param name="blnMasked">
        ''' A option for masking the HKIC No.
        ''' True: Mask is applied.
        ''' False: Mask is not applied.
        ''' </param>
        ''' <returns>Formatted HKIC No.</returns>
        ''' <remarks></remarks>
        Public Function formatHKID(ByVal strOriHKID As String, ByVal blnMasked As Boolean) As String
            Dim strRes As String
            Dim strMaskStr As String
            strMaskStr = "XXXX"
            strOriHKID = strOriHKID.Trim()
            strRes = strOriHKID

            If blnMasked Then
                Select Case strOriHKID.Length
                    Case 8
                        strOriHKID = strOriHKID.Substring(0, 4) + strMaskStr
                    Case 9
                        strOriHKID = strOriHKID.Substring(0, 5) + strMaskStr
                End Select
            End If

            If Trim(strOriHKID) <> "" Then
                strRes = strOriHKID.Substring(0, strOriHKID.Length() - 1) + "(" + strOriHKID.Substring(strOriHKID.Length() - 1, 1) + ")"
            End If
            strRes = strRes.ToUpper
            Return strRes
        End Function

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function formatDate(ByVal strOriDate As String) As String

        '    ' Handle 1-1-2000
        '    ' Handle 20-3-2000
        '    Dim strRes As String = strOriDate

        '    If strRes.Contains("-") Then
        '        Dim arrStrDate As String() = strRes.Split("-")
        '        If arrStrDate.Length = 2 Then
        '            strRes = arrStrDate(0).PadLeft(2, "0").Trim() + "-" + arrStrDate(1).Trim()
        '        ElseIf arrStrDate.Length = 3 Then
        '            strRes = arrStrDate(0).PadLeft(2, "0").Trim() + "-" + arrStrDate(1).PadLeft(2, "0").Trim() + "-" + arrStrDate(2).Trim()
        '        Else
        '            strRes = ""
        '        End If
        '    Else
        '        Select Case strOriDate.Length
        '            Case 4, 10
        '                strRes = strOriDate
        '            Case 8
        '                strRes = strOriDate.Substring(0, 2) + "-" + strOriDate.Substring(2, 2) + "-" + strOriDate.Substring(4, 4)
        '            Case 6
        '                strRes = strOriDate.Substring(0, 2) + "-" + strOriDate.Substring(2, 4)
        '        End Select
        '    End If
        '    Return strRes
        'End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Public Function formatHKIDIssueDate(ByVal dtHKIDIssueDate As Date) As String
            Dim strRes As String

            strRes = dtHKIDIssueDate.ToString(strDisplayIssueDateFormat)

            Return strRes
        End Function
        'CRE14-006 VSS consent form update [Start][Karl]
        Public Function formatECDOBTitle(ByVal strExactDOB As String, ByVal strLanguage As String) As String
            Dim strRes As String = Nothing

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Select Case strExactDOB
                Case "D", "M", "Y"
                    Select Case UCase(strLanguage)
                        Case Common.Component.CultureLanguage.TradChinese.ToUpper
                            strRes = "報稱的出生日期"
                        Case Common.Component.CultureLanguage.SimpChinese.ToUpper
                            strRes = "报称的出生日期"
                        Case Common.Component.CultureLanguage.English.ToUpper
                            strRes = "Date of Birth reported"
                    End Select

                Case "R"
                    Select Case UCase(strLanguage)
                        Case Common.Component.CultureLanguage.TradChinese.ToUpper
                            strRes = "報稱出生年份"
                        Case Common.Component.CultureLanguage.SimpChinese.ToUpper
                            strRes = "报称出生年份"
                        Case Common.Component.CultureLanguage.English.ToUpper
                            strRes = "Year of Birth reported"
                    End Select

                Case "T", "U", "V"
                    Select Case UCase(strLanguage)
                        Case Common.Component.CultureLanguage.TradChinese.ToUpper
                            strRes = "旅遊證件所記錄的出生日期"
                        Case Common.Component.CultureLanguage.SimpChinese.ToUpper
                            strRes = "旅游证件所记录的出生日期"
                        Case Common.Component.CultureLanguage.English.ToUpper
                            strRes = "Date of Birth reported on travel document"
                    End Select

                Case "A"
                    Select Case UCase(strLanguage)
                        Case Common.Component.CultureLanguage.TradChinese.ToUpper
                            strRes = "出生日期"
                        Case Common.Component.CultureLanguage.SimpChinese.ToUpper
                            strRes = "出生日期"
                        Case Common.Component.CultureLanguage.English.ToUpper
                            strRes = "Date of Birth"
                    End Select

            End Select
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            Return strRes
        End Function
        'CRE14-006 VSS consent form update [End][Karl]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Function formatDOB(ByVal dtDOB As Date, ByVal strExactDOB As String, ByVal strLanguage As String, ByVal intAge As Nullable(Of Integer), ByVal dtDOR As Nullable(Of Date)) As String
            Dim strRes As String
            strRes = ""
            Dim providerUS As New System.Globalization.CultureInfo("en-us")
            Dim providerTW As New System.Globalization.CultureInfo("zh-tw")
            Dim providerCN As New System.Globalization.CultureInfo("zh-cn")

            ' To Do: Format Case "R" For EC: Year of Birth reported

            Select Case strExactDOB
                Case "Y", "V"
                    If UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper() OrElse
                        UCase(strLanguage) = CultureLanguage.SimpChinese.ToUpper() Then
                        strRes = dtDOB.ToString(strDisplayDOBYearFormatChi)
                    Else
                        strRes = dtDOB.ToString(strDisplayDOBYearFormat)
                    End If
                Case "M", "U"
                    If UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper() OrElse
                        UCase(strLanguage) = CultureLanguage.SimpChinese.ToUpper() Then
                        strRes = dtDOB.ToString(strDisplayDOBMonthFormatChi)
                    Else
                        strRes = dtDOB.ToString(strDisplayDOBMonthFormat)
                    End If
                Case "D", "T"
                    If UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper() OrElse
                        UCase(strLanguage) = CultureLanguage.SimpChinese.ToUpper() Then
                        strRes = dtDOB.ToString(strDisplayDOBDayFormatChi)
                    Else
                        strRes = dtDOB.ToString(strDisplayDOBDayFormat)
                    End If
                Case "A"
                    If UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper() Then
                        strRes = HttpContext.GetGlobalResourceObject("Text", "Age", providerTW) + " " + intAge.Value.ToString() + " " + HttpContext.GetGlobalResourceObject("Text", "RegisterOn", providerTW) + " " + dtDOR.Value.ToString("yyyy年MM月dd日", providerTW)
                    ElseIf UCase(strLanguage) = CultureLanguage.SimpChinese.ToUpper() Then
                        strRes = HttpContext.GetGlobalResourceObject("Text", "Age", providerCN) + " " + intAge.Value.ToString() + " " + HttpContext.GetGlobalResourceObject("Text", "RegisterOn", providerCN) + " " + dtDOR.Value.ToString("yyyy年MM月dd日", providerCN)
                    Else
                        strRes = HttpContext.GetGlobalResourceObject("Text", "Age", providerUS) + " " + intAge.Value.ToString() + " " + HttpContext.GetGlobalResourceObject("Text", "RegisterOn", providerUS) + " " + dtDOR.Value.ToString("d MMMM yyyy", providerUS)
                    End If
                Case "R"
                    If UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper() OrElse
                        UCase(strLanguage) = CultureLanguage.SimpChinese.ToUpper() Then
                        strRes = dtDOB.ToString(strDisplayDOBYearFormatChi)
                    Else
                        strRes = dtDOB.ToString(strDisplayDOBYearFormat)
                    End If

            End Select

            Return strRes
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Function formatDOB(ByVal dtmDOB As DateTime, ByVal strExactDOB As String, ByVal intAge As Nullable(Of Integer), ByVal dtDOR As Nullable(Of Date)) As String
            Dim strDOB As String
            strDOB = formatDOB(dtmDOB, strExactDOB, Thread.CurrentThread.CurrentUICulture.Name, intAge, dtDOR)
            Return strDOB
        End Function

        Public Function formatChineseName(ByVal strOriCName As String) As String
            Dim strRes As String
            strRes = ""
            If strOriCName.Trim.Equals(String.Empty) Then
                strRes = String.Empty
            Else
                strRes = "(" + strOriCName + ")"
            End If
            Return strRes
        End Function

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function formatDate(ByVal dtOriDate As Date) As String
        '    'Dim strRes As String
        '    'If Thread.CurrentThread.CurrentUICulture.Name.ToLower = "zh-tw" Then
        '    '    strRes = dtOriDate.ToString(strDisplayDateFormatChi)
        '    'Else
        '    '    strRes = dtOriDate.ToString(strDisplayDateFormat)
        '    'End If
        '    'Return strRes

        '    Dim strDate As String
        '    Dim strmonth As String = String.Empty
        '    strDate = ""

        '    If dtOriDate = DateTime.MinValue Then
        '        strDate = ""
        '    Else
        '        If Thread.CurrentThread.CurrentUICulture.Name.ToLower = CultureLanguage.TradChinese OrElse Thread.CurrentThread.CurrentUICulture.Name.ToLower = CultureLanguage.SimpChinese Then
        '            strDate = dtOriDate.ToString("yyyy年MM月dd日")
        '        Else
        '            strmonth = convertMonthNumtoEng(dtOriDate.Month.ToString.PadLeft(2, "0"))
        '            strDate = dtOriDate.ToString("dd ") + strmonth + dtOriDate.ToString(" yyyy")
        '        End If
        '    End If
        '    Return strDate
        'End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function formatDate(ByVal dtOriDate As Date, ByVal strLanguage As String) As String
        '    'If UCase(strLanguage) = "ZH-TW" Then
        '    '    Return dtOriDate.ToString(strDisplayDateFormatChi)
        '    'Else
        '    '    Return dtOriDate.ToString(strDisplayDateFormat)
        '    'End If

        '    Dim strDate As String
        '    Dim strmonth As String = String.Empty
        '    strDate = ""

        '    If dtOriDate = DateTime.MinValue Then
        '        strDate = ""
        '    Else
        '        If strLanguage.ToLower = CultureLanguage.TradChinese OrElse strLanguage.ToLower = CultureLanguage.SimpChinese Then
        '            strDate = dtOriDate.ToString("yyyy年MM月dd日")
        '        Else
        '            strmonth = convertMonthNumtoEng(dtOriDate.Month.ToString.PadLeft(2, "0"))
        '            strDate = dtOriDate.ToString("dd ") + strmonth + dtOriDate.ToString(" yyyy")
        '        End If
        '    End If
        '    Return strDate
        'End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Public Function convertDate(ByVal strOriDate As String, ByVal strLanguage As String) As String
            Dim strRes As String
            Dim strDay, strMonth, strYear, strConMonth As String
            strRes = ""
            If strOriDate.Length = 10 Then
                strDay = strOriDate.Substring(0, 2)
                strMonth = strOriDate.Substring(3, 2)
                strYear = strOriDate.Substring(6, 4)

                strConMonth = convertMonthNumtoEng(strMonth)

                If strConMonth = "" Then
                    strRes = ""
                Else
                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'strRes = strDay + " " + strConMonth + " " + strYear
                    strRes = strCommonDBDateFormat

                    If Not strYear = String.Empty Then
                        strRes = strRes.Replace("yyyy", strYear)
                    End If

                    If Not strMonth = String.Empty Then
                        strRes = strRes.Replace("MMM", strConMonth)
                    End If

                    If Not strDay = String.Empty Then
                        strRes = strRes.Replace("dd", strDay)
                    End If
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                End If
                'Else
                '    ' Handle Case:  d-m-yyyy | d-mm-yyyy | dd-m-yyyy
                '    Dim strDatePart As String() = strOriDate.Split("-")
                '    If strDatePart.Length = 3 Then
                '        strRes = String.Format("{0} {1} {2}", strDatePart(0).PadLeft(2, "0"), convertMonthNumtoEng(strDatePart(1)), strDatePart(2))
                '    End If
            End If

            Return strRes
        End Function

        Public Function convertMonthNumtoEng(ByVal strMonth As String) As String
            Dim strRes As String = String.Empty

            Select Case strMonth
                Case "01", "1"
                    strRes = "Jan"
                Case "02", "2"
                    strRes = "Feb"
                Case "03", "3"
                    strRes = "Mar"
                Case "04", "4"
                    strRes = "Apr"
                Case "05", "5"
                    strRes = "May"
                Case "06", "6"
                    strRes = "Jun"
                Case "07", "7"
                    strRes = "Jul"
                Case "08", "8"
                    strRes = "Aug"
                Case "09", "9"
                    strRes = "Sep"
                Case "10"
                    strRes = "Oct"
                Case "11"
                    strRes = "Nov"
                Case "12"
                    strRes = "Dec"
            End Select

            Return strRes
        End Function

        Public Function convertDate(ByVal dtmConvert As DateTime) As String
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'If Thread.CurrentThread.CurrentUICulture.Name.ToLower = "zh-tw" Then
            '    Return convertDate(dtmConvert.ToString("dd-MM-yyyy"), "C")
            'End If
            'Return convertDate(dtmConvert.ToString("dd-MM-yyyy"), "")

            Select Case Thread.CurrentThread.CurrentUICulture.Name.ToLower
                Case CultureLanguage.English
                    Return convertDate(dtmConvert.ToString(strCommonDateFormat), CultureLanguage.English)

                Case CultureLanguage.TradChinese
                    Return convertDate(dtmConvert.ToString(strCommonDateFormat), CultureLanguage.TradChinese)

                Case CultureLanguage.SimpChinese
                    Return convertDate(dtmConvert.ToString(strCommonDateFormat), CultureLanguage.SimpChinese)

                Case Else
                    Return convertDate(dtmConvert.ToString(strCommonDateFormat), "")
            End Select
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End Function

        Public Function convertDateTime(ByVal dtmConvert As DateTime, ByVal strLanguage As String) As String
            Dim strDateTime As String
            Dim strmonth As String = String.Empty
            strDateTime = ""

            If dtmConvert = DateTime.MinValue Then
                strDateTime = ""
            Else
                If strLanguage.ToUpper = CultureLanguage.TradChinese.ToUpper OrElse
                    strLanguage.ToUpper = CultureLanguage.SimpChinese.ToUpper Then
                    strDateTime = dtmConvert.ToString("yyyy年MM月dd日 HH:mm")
                Else
                    strmonth = convertMonthNumtoEng(dtmConvert.Month.ToString.PadLeft(2, "0"))
                    strDateTime = dtmConvert.ToString("dd ") + strmonth + dtmConvert.ToString(" yyyy HH:mm")
                End If
            End If
            Return strDateTime
        End Function

        Public Function convertDateTime(ByVal dtmConvert As DateTime) As String
            Dim strDateTime As String
            Dim strmonth As String = String.Empty
            strDateTime = ""

            If dtmConvert = DateTime.MinValue Then
                strDateTime = ""
            Else
                If Thread.CurrentThread.CurrentUICulture.Name.ToLower = CultureLanguage.TradChinese OrElse _
                        Thread.CurrentThread.CurrentUICulture.Name.ToLower = CultureLanguage.SimpChinese Then
                    strDateTime = dtmConvert.ToString("yyyy年MM月dd日 HH:mm")
                Else
                    strmonth = convertMonthNumtoEng(dtmConvert.Month.ToString.PadLeft(2, "0"))
                    strDateTime = dtmConvert.ToString("dd ") + strmonth + dtmConvert.ToString(" yyyy HH:mm")
                    'strDateTime = dtmConvert.ToString("dd MMM yyyy HH:mm")
                End If
            End If
            Return strDateTime
        End Function


        Public Function maskEnglishName(ByVal strName As String) As String

            ' Extract Name delimiter
            Dim intNameDelimiter As Integer = InStr(strName, ",")

            ' Get the Last Name
            If intNameDelimiter <= 0 Then
                Return strName
            Else
                Dim strLastName As String = Left(strName, intNameDelimiter - 1)
                Dim strGivenName() As String = Mid(strName, intNameDelimiter + 2).Trim().Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)
                Dim strExtractedName As String = ""

                For Each strTemp As String In strGivenName
                    strExtractedName &= Left(strTemp, 1) & ". "
                Next

                Return String.Format("{0}, {1}", strLastName, strExtractedName).TrimEnd(" ")
            End If

        End Function

        Public Function maskBankAccount(ByVal strOriBankAcct As String) As String
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'Change mask logic to handle free format

            Dim strMaskBankAcct As String = String.Empty
            Dim strSegment = strOriBankAcct.Split("-")

            For i As Integer = 0 To strSegment.Length - 1
                If i = 0 Then
                    'No mask for 1st segment
                    strMaskBankAcct += strSegment(0)
                ElseIf i = 1 Then
                    'Mask first and last char for 2nd segment

                    strMaskBankAcct += "-X"
                    If strSegment(1).Length > 1 Then
                        strMaskBankAcct += strSegment(1).ToString.Substring(1, strSegment(1).Length - 2)
                        strMaskBankAcct += "X"
                    End If

                Else
                    Dim arrSegment() As Char
                    arrSegment = strSegment(i).ToCharArray()

                    Dim strFormat As String = "X##X##XXXXXXXXXXXXXXXXXXXX"

                    strMaskBankAcct += "-"

                    For j As Integer = 0 To strSegment(i).Length - 1
                        If strFormat(j) = "#" Then Continue For
                        arrSegment(j) = strFormat(j)
                    Next

                    strMaskBankAcct += New String(arrSegment)
                End If
            Next


            Return strMaskBankAcct

            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            ' CRE11-015
            ' ======================================================================================
            'Dim strFormat As String = "###-X#X-X##X##XXXXXXXXXXXXXXXX"
            'Dim arrOriBankAcct() As Char
            'strFormat = strFormat.Replace("-", String.Empty)
            'arrOriBankAcct = strOriBankAcct.Replace("-", String.Empty).ToCharArray()

            'For i As Integer = 0 To arrOriBankAcct.Length - 1
            '    If strFormat(i) = "#" Then Continue For
            '    arrOriBankAcct(i) = strFormat(i)
            'Next

            'strOriBankAcct = New String(arrOriBankAcct)

            'Return strOriBankAcct.Substring(0, 3) + "-" + strOriBankAcct.Substring(3, 3) + "-" + strOriBankAcct.Substring(6, strOriBankAcct.Length - 6)

            ' ======================================================================================
            'Dim strRes, strTemp As String
            'Dim intAcctlen As Integer
            'Dim strPrefix, strPostfix As String
            'strRes = String.Empty
            'strTemp = String.Empty
            'strPrefix = String.Empty
            'strPostfix = String.Empty
            'intAcctlen = 0

            'strOriBankAcct = strOriBankAcct.Replace("-", String.Empty)
            'strPrefix = "XXX-" 'strOriBankAcct.Substring(0, 3)
            'strPrefix = strPrefix + strOriBankAcct.Substring(3, 3) + "-"
            'strTemp = strOriBankAcct.Substring(6)
            'intAcctlen = strTemp.Length
            'If intAcctlen > 3 Then
            '    strPostfix = strTemp.Substring(0, 3).PadRight(intAcctlen, "X")
            'Else
            '    strPostfix = strTemp.Substring(0).PadRight(intAcctlen, "X")
            'End If
            'strRes = strPrefix + strPostfix

            ' ======================================================================================
            'intAcctlen = strOriBankAcct.Length() - 6
            'strRes = ""
            'strPrefix = ""
            'strPostfix = ""

            'For i = 0 To strOriBankAcct.Length() - 1
            '    strPrefix = strOriBankAcct.Substring(0, 3)
            '    strPostfix = strOriBankAcct.Substring(strOriBankAcct.Length() - 3, 3)
            'Next

            'For i = 1 To intAcctlen
            '    strRes = strRes + "X"
            'Next

            'strRes = strPrefix + strRes + strPostfix
            'strRes = strRes.Substring(0, 3) + "-" + strRes.Substring(3, 3) + "-" + strRes.Substring(6)

            'Return strRes
        End Function

        Public Function maskBankAccount_HCVUSuperDownload(ByVal strOriBankAcct As String) As String
            ' CRE11-015
            ' ======================================================================================
            Return maskBankAccount(strOriBankAcct)
            ' ======================================================================================
            'Dim strRes, strTemp As String
            'Dim intAcctlen As Integer
            'Dim strPrefix, strPostfix As String
            'strRes = String.Empty
            'strTemp = String.Empty
            'strPrefix = String.Empty
            'strPostfix = String.Empty
            'intAcctlen = 0

            'strOriBankAcct = strOriBankAcct.Replace("-", String.Empty)
            'strPrefix = strOriBankAcct.Substring(0, 3) + "-X"
            'strPrefix = strPrefix + strOriBankAcct.Substring(4, 1) + "X-X"
            'strTemp = strOriBankAcct.Substring(7)
            'intAcctlen = strTemp.Length
            'If intAcctlen > 2 Then
            '    strPostfix = strTemp.Substring(0, 2).PadRight(intAcctlen, "X")
            'Else
            '    strPostfix = strTemp.Substring(0).PadRight(intAcctlen, "X")
            'End If
            'strRes = strPrefix + strPostfix

            'Return strRes
        End Function

        Public Function formatGender(ByVal strOriGender) As String
            Dim strRes As String
            If strOriGender = "M" Then
                strRes = "GenderMale"
            Else
                strRes = "GenderFemale"
            End If
            Return strRes
        End Function

        Public Function formatEnglishName(ByVal strENameSurname As String, ByVal strENameFirstname As String) As String
            Dim strRes As String
            strRes = ""
            If strENameFirstname.Trim.Length > 0 Then ' I-CRP16-002 Fix invalid input on English name [Lawrence]
                strRes = Trim(strENameSurname) + ", " + Trim(strENameFirstname)
            Else
                strRes = Trim(strENameSurname)
            End If

            Return strRes
        End Function

        Public Sub seperateEName(ByVal strOriEName As String, ByRef strENameSurname As String, ByRef strENameFirstname As String)

            If strOriEName.IndexOf(",") > 0 Then
                strENameSurname = strOriEName.Substring(0, strOriEName.IndexOf(","))
                strENameFirstname = Trim(strOriEName.Substring(strOriEName.IndexOf(",") + 1))
            Else
                strENameSurname = Trim(strOriEName)
                strENameFirstname = String.Empty
            End If

        End Sub

        Public Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrictCode As String, ByVal strAreaCode As String) As String
            Dim validator As Common.Validation.Validator = New Common.Validation.Validator
            Dim udtAreaBLL As Component.Area.AreaBLL = New Component.Area.AreaBLL
            Dim udtDistrcitBLL As Component.District.DistrictBLL = New Component.District.DistrictBLL

            Dim udtAreaModel As Component.Area.AreaModel
            Dim udtDistrcitModel As Component.District.DistrictModel


            udtDistrcitModel = udtDistrcitBLL.GetDistrictNameByDistrictCode(strDistrictCode)
            udtAreaModel = udtAreaBLL.GetAreaNameByAreaCode(udtDistrcitModel.Area_ID)

            Dim strAddress As String
            strAddress = String.Empty

            If Not validator.IsEmpty(strRoom) Then
                strAddress = strAddress + "ROOM " + strRoom + ", "
            End If

            If Not validator.IsEmpty(strFloor) Then
                strAddress = strAddress + "FLOOR " + strFloor + ", "
            End If

            If Not validator.IsEmpty(strBlock) Then
                strAddress = strAddress + "BLOCK " + strBlock + ", "
            End If

            If Not validator.IsEmpty(strBuilding) Then
                strAddress = strAddress + strBuilding
            End If

            'strAddress = strAddress + udtDistrcitModel.District_Name + "," + udtAreaModel.Area_Name

            If Not validator.IsEmpty(strDistrictCode) Then
                If Not String.IsNullOrEmpty(strAddress) Then
                    strAddress = strAddress + ", "
                End If
                strAddress = strAddress + udtDistrcitModel.District_Name + ", " + udtAreaModel.Area_Name
            End If

            'If Not validator.IsEmpty(strAreaCode) Then
            '    strAddress = strAddress + udtAreaModel.Area_Name
            'End If

            Return strAddress
        End Function

        Public Function formatAddress(ByVal udtAddress As Common.Component.Address.AddressModel) As String
            Return formatAddress(udtAddress, True)
        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function FormatAddressWithoutDistrict(ByVal udtAddress As Common.Component.Address.AddressModel) As String
            Return formatAddress(udtAddress, False)
        End Function

        Protected Function FormatAddress(ByVal udtAddress As Common.Component.Address.AddressModel, ByVal blnIncludeDistrict As Boolean) As String
            Dim strAddress As String = String.Empty
            Dim strRoom As String = String.Empty
            Dim strFloor As String = String.Empty
            Dim strBlock As String = String.Empty
            Dim strBuilding As String = String.Empty
            Dim strDistrictCode As String = String.Empty
            Dim strAreaCode As String = String.Empty

            With udtAddress
                'If Not .Address_Code.HasValue Then
                '    strAreaCode = .AreaCode
                '    strDistrictCode = .District
                'Else
                '    strAreaCode = String.Empty
                '    strDistrictCode = String.Empty
                'End If

                strAreaCode = .AreaCode
                strDistrictCode = .District

                strBuilding = .Building
                strBlock = .Block
                strFloor = .Floor
                strRoom = .Room
            End With

            If blnIncludeDistrict Then
                strAddress = FormatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrictCode, strAreaCode)
            Else
                strAddress = FormatAddress(strRoom, strFloor, strBlock, strBuilding, String.Empty, String.Empty)
            End If

            Return strAddress
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function formatDateTime(ByVal dtDate As DateTime, ByVal strLanguage As String) As String
            'If UCase(strLanguage) = "ZH-TW" Then
            '    Return dtDate.ToString(strDisplayDateTimeFormatChi)
            'Else
            '    Return dtDate.ToString(strDisplayDateTimeFormat)
            'End If
            Return convertDateTime(dtDate, strLanguage)
        End Function

        Public Function formatDateTime(ByVal dtmConvert As DateTime) As String
            'Dim strDateTime As String
            'strDateTime = ""

            'If dtmConvert = DateTime.MinValue Then
            '    strDateTime = ""
            'Else
            '    If Thread.CurrentThread.CurrentUICulture.Name.ToLower = "zh-tw" Then
            '        strDateTime = dtmConvert.ToString(strDisplayDateTimeFormatChi)
            '    Else
            '        strDateTime = dtmConvert.ToString(strDisplayDateTimeFormat)
            '    End If
            'End If
            'Return strDateTime
            Return convertDateTime(dtmConvert)
        End Function

        Public Function formatEnterDate(ByVal dtDate As Date) As String
            Return dtDate.ToString(strEnterDateFormat)
        End Function

        Public Function formatValidatedEHSAccountNumber(ByVal strEHSAccountID As String) As String
            Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim strParmValue1 As String = String.Empty
            Dim strParmValue2 As String = String.Empty
            Dim strFormatEHSAccountID As String = strEHSAccountID.Trim()

            If udtGeneralFunction.getSytemParameterByParameterName("eHealthAccountPrefix", strParmValue1, strParmValue2) Then
                strFormatEHSAccountID = String.Format("{0}{1}", strParmValue1, strEHSAccountID.Trim())
            End If

            Return String.Format("{0}{1}", strFormatEHSAccountID.Trim(), udtGeneralFunction.generateChkDgt(strFormatEHSAccountID))

        End Function

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        Public Function formatMoney(ByVal strValue As String, ByVal blnDollarSign As Boolean, Optional ByVal intDollarSignSpace As Integer = 0) As String
            Dim strReturnValue As String = String.Empty

            If String.IsNullOrEmpty(strValue) = True Or IsNumeric(strValue) = False Then
                Return String.Empty
            Else
                strReturnValue = CInt(strValue).ToString("###,##0")
            End If

            If blnDollarSign = True Then
                Return "$" & Space(intDollarSignSpace) & strReturnValue
            Else
                Return strReturnValue
            End If

        End Function
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        Public Function formatMoneyHKDInChina(ByVal strValue As String, ByVal blnDollarSign As Boolean, Optional ByVal intDollarSignSpace As Integer = 0) As String
            Dim strFormatted As String = formatMoney(strValue, blnDollarSign, intDollarSignSpace)

            Return strFormatted.Replace("$", HttpContext.GetGlobalResourceObject("Text", "HKVoucherUnitInChinaWithColon"))

        End Function
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Function formatMoneyRMB(ByVal strValue As String, ByVal blnDollarSign As Boolean, Optional ByVal intDollarSignSpace As Integer = 0) As String
            Dim strReturnValue As String = String.Empty

            If String.IsNullOrEmpty(strValue) = True Or IsNumeric(strValue) = False Then
                Return String.Empty
            ElseIf CDec(strValue) = 0 Then
                strReturnValue = "0"
            Else
                strReturnValue = CDec(strValue).ToString("N2")
            End If

            If blnDollarSign = True Then
                Return HttpContext.GetGlobalResourceObject("Text", "RMBDollarSign") & Space(intDollarSignSpace) & strReturnValue
            Else
                Return strReturnValue
            End If

        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Shared Function ReverseValidatedEHSAccountNumber(ByVal strFormattedEHSAccountID As String) As String
            Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim strParmValue1 As String = String.Empty
            Dim strParmValue2 As String = String.Empty
            Dim result As String = ""

            udtGeneralFunction.getSytemParameterByParameterName("eHealthAccountPrefix", strParmValue1, strParmValue2)

            ' Formatted Validated EHS Account ID = "EHS<8-digit seq number><1-digit check digit>"
            If strFormattedEHSAccountID.Length = "12" AndAlso strFormattedEHSAccountID.StartsWith(strParmValue1) Then
                result = strFormattedEHSAccountID.Trim().Substring(strParmValue1.Length, 8)
            End If

            Return result

        End Function


        Public Function formatSystemNumber(ByVal StrOriNum As String) As String
            Dim strRes As String
            Dim strPrefix, strGenNum, strChkdgt, strTemp As String

            ' Refine the logic for System Number with different format later
            If StrOriNum.Trim().Length = 16 Then
                Try
                    strTemp = ""
                    strRes = ""
                    strPrefix = ""
                    strGenNum = ""
                    strChkdgt = ""
                    StrOriNum = StrOriNum.Trim
                    strTemp = StrOriNum
                    strPrefix = strTemp.Substring(0, 7)
                    strTemp = strTemp.Substring(7)
                    strGenNum = (CType(strTemp.Substring(0, strTemp.Length() - 1), Long)).ToString
                    strChkdgt = strTemp.Substring(strTemp.Length() - 1, 1)
                    strRes = strPrefix + "-" + strGenNum + "-" + strChkdgt
                    Return strRes
                Catch ex As Exception
                    strRes = StrOriNum
                    Return strRes
                End Try
            Else
                Try
                    strTemp = ""
                    strRes = ""
                    strPrefix = ""
                    strGenNum = ""
                    strChkdgt = ""
                    StrOriNum = StrOriNum.Trim
                    strTemp = StrOriNum
                    strPrefix = strTemp.Substring(0, 6)
                    strTemp = strTemp.Substring(6)
                    strGenNum = (CType(strTemp.Substring(0, strTemp.Length() - 1), Long)).ToString
                    strChkdgt = strTemp.Substring(strTemp.Length() - 1, 1)
                    strRes = strPrefix + "-" + strGenNum + "-" + strChkdgt
                    Return strRes
                Catch ex As Exception
                    strRes = StrOriNum
                    Return strRes
                End Try
            End If
        End Function

        Public Function formatSystemNumberReverse(ByVal StrOriNum As String) As String
            'Dim strRes As String
            'Dim strPrefix, strZero, strTemp As String

            'StrOriNum = StrOriNum.Trim

            'If StrOriNum.Length >= 9 Then
            '    strTemp = ""
            '    strRes = ""
            '    strPrefix = ""
            '    strZero = ""
            '    strTemp = StrOriNum
            '    strPrefix = strTemp.Substring(0, 6)
            '    strTemp = strTemp.Substring(7)
            '    strTemp = strTemp.Replace("-", "")
            '    While strTemp.Length < 9
            '        strTemp = "0" & strTemp
            '    End While

            '    strRes = strPrefix + strTemp
            'Else
            '    strRes = StrOriNum
            'End If

            'Return strRes
            Return ReverseSystemNumber(StrOriNum)
        End Function

        Public Function formatBankAcct(ByVal strBankcode As String, ByVal strBranchCode As String, ByVal strBankAcct As String) As String
            Dim strFullBankAcctFormat As String

            If strBankcode.Equals(String.Empty) And strBranchCode.Equals(String.Empty) And strBankAcct.Equals(String.Empty) Then
                strFullBankAcctFormat = String.Empty
            Else
                strFullBankAcctFormat = strBankcode + "-" + strBranchCode + "-" + strBankAcct
            End If
            Return strFullBankAcctFormat
        End Function

        Public Function splitBankAcct(ByVal strBankAcct As String) As String()
            Return strBankAcct.Split("-")
        End Function

        Public Function convertHKIDIssueDateStringToDate(ByVal strHKIDIssueDate As String) As Date
            Dim strRes As String
            Dim dtRes As Date
            Dim strDay, strMonth, strYear, strConMonth As String
            strRes = ""
            If strHKIDIssueDate.Length = 8 Then
                strDay = strHKIDIssueDate.Substring(0, 2)
                strMonth = strHKIDIssueDate.Substring(3, 2)
                strYear = strHKIDIssueDate.Substring(6, 2)
                If CInt(strYear) > 60 Then
                    strYear = "19" + strYear
                Else
                    strYear = "20" + strYear
                End If

                strConMonth = ""
                strConMonth = convertMonthNumtoEng(strMonth)

                strRes = strDay + " " + strConMonth + " " + strYear

            End If

            dtRes = CType(strRes, Date)

            Return dtRes

        End Function


        ''' <summary>
        ''' Split Profession Reference No To Enrolment Reference No.: String + Professional Sequence : Integer
        ''' </summary>
        ''' <param name="strReferenceNo"></param>
        ''' <param name="strEnrolRefNo">[Out]: Enrolment Reference No</param>
        ''' <param name="intProfSeq">[Out]: Professional Sequence</param>
        ''' <remarks>DataBase Contain the Split Logic as Well:
        '''  Eg. proc_ProfessionalVerification_get_ForVerify</remarks>
        Public Sub SplitProfessionalReferenceNo(ByVal strReferenceNo As String, ByRef strEnrolRefNo As String, ByRef intProfSeq As Integer)
            strEnrolRefNo = strReferenceNo.Trim().Substring(0, 15)
            intProfSeq = Convert.ToInt32(strReferenceNo.Substring(15))
        End Sub

        ''' <summary>
        ''' To Concat Profession Reference No By Enrolment Reference No.: String + Professional Sequence : Integer
        ''' Reference # = Enrolment Ref # + 5 digit ProfessionalSeq
        ''' eg. A000000000000001 + 00001 (int With 5 Char)
        ''' </summary>
        ''' <param name="strEnrolmentRefNo"></param>
        ''' <param name="intProfSeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConcatProfessionalReferenceNo(ByVal strEnrolmentRefNo As String, ByVal intProfSeq As Integer) As String
            Return strEnrolmentRefNo.Trim() + intProfSeq.ToString().PadLeft(5, "0")
        End Function

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Public Shared Sub SplitTransactionNo(ByVal strTranID As String, ByRef strPrefix As String, ByRef strSeqNo As String, ByRef strCheckDigit As String)
            Dim strID As String = strTranID.Replace("-", "")

            strCheckDigit = strID.Substring(strID.Length - 1, 1)
            strSeqNo = CInt(strID.Substring(strID.Length - 9, 8))
            strPrefix = strID.Substring(0, strID.Length - 9)
        End Sub
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Public Shared Function ReverseSystemNumber(ByVal strSystemNumber As String) As String

            Try
                Dim strSystemNumberComponents As String() = strSystemNumber.Trim().Split("-")
                Dim intTranNoLength As Integer = 0

                If IsNumeric(strSystemNumberComponents(0).Substring(1, 1).Trim) Then
                    '15-digit Transaction No.
                    intTranNoLength = 15
                Else
                    '16-digit Transaction No.
                    intTranNoLength = 16
                End If

                Dim intNumberOfZero As Integer = intTranNoLength - strSystemNumberComponents(0).Length - strSystemNumberComponents(2).Length
                strSystemNumberComponents(1) = strSystemNumberComponents(1).PadLeft(intNumberOfZero, "0")
                Return String.Format("{0}{1}{2}", strSystemNumberComponents(0), strSystemNumberComponents(1), strSystemNumberComponents(2))
            Catch ex As Exception
                Return strSystemNumber
            End Try
        End Function

        Public Shared Sub ReverseSystemNumberWithSuffix(ByVal strFormattedSystemNumber As String, ByRef strSystemNumber As String, ByRef strSuffix As String)
            Try
                Dim strSystemNumberComponents As String() = strFormattedSystemNumber.Trim().Split("-")

                Dim intNumberOfZero As Integer = 15 - strSystemNumberComponents(0).Length - strSystemNumberComponents(2).Length
                strSystemNumberComponents(1) = strSystemNumberComponents(1).PadLeft(intNumberOfZero, "0")
                strSystemNumber = String.Format("{0}{1}{2}", strSystemNumberComponents(0), strSystemNumberComponents(1), strSystemNumberComponents(2))

                If strSystemNumberComponents.Length = 4 Then
                    strSuffix = strSystemNumberComponents(3)
                Else
                    strSuffix = String.Empty
                End If

            Catch ex As Exception
                strSystemNumber = strFormattedSystemNumber
                strSuffix = String.Empty
            End Try
        End Sub

        Public Function formatHKIDInternal(ByVal strHKID As String) As String
            Dim strRes As String = String.Empty
            strRes = strHKID.Replace("(", String.Empty).Replace(")", String.Empty).Trim
            strRes = strRes.ToUpper
            Return strRes
        End Function

        Public Function formatAddressWithNewline(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrictCode As String, ByVal strAreaCode As String) As String
            Dim validator As Common.Validation.Validator = New Common.Validation.Validator
            Dim udtAreaBLL As Component.Area.AreaBLL = New Component.Area.AreaBLL
            Dim udtDistrcitBLL As Component.District.DistrictBLL = New Component.District.DistrictBLL

            Dim udtAreaModel As Component.Area.AreaModel
            Dim udtDistrcitModel As Component.District.DistrictModel

            udtAreaModel = udtAreaBLL.GetAreaNameByAreaCode(strAreaCode)
            udtDistrcitModel = udtDistrcitBLL.GetDistrictNameByDistrictCode(strDistrictCode)

            Dim strAddress As String
            strAddress = String.Empty

            If Not validator.IsEmpty(strRoom) Then
                strAddress = strAddress + "ROOM " + strRoom + ", "
            End If

            If Not validator.IsEmpty(strFloor) Then
                strAddress = strAddress + "FLOOR " + strFloor + ", "
            End If

            If Not validator.IsEmpty(strBlock) Then
                strAddress = strAddress + "BLOCK " + strBlock + ", "
            End If

            'If Not validator.IsEmpty(strBuilding) Then
            '    strAddress = strAddress + Environment.NewLine + strBuilding + ", "
            'End If

            If Not validator.IsEmpty(strBuilding) Then
                If strAddress.Trim.Equals(String.Empty) Then
                    strAddress = strBuilding + ", "
                Else
                    strAddress = strAddress + Environment.NewLine + strBuilding + ", "
                End If

            End If

            'strAddress = strAddress + udtDistrcitModel.District_Name + "," + udtAreaModel.Area_Name

            If Not validator.IsEmpty(strDistrictCode) Then
                strAddress = strAddress + Environment.NewLine + udtDistrcitModel.District_Name + ", "
            End If

            If Not validator.IsEmpty(strAreaCode) Then
                strAddress = strAddress + udtAreaModel.Area_Name
            End If

            Return strAddress
        End Function

        Public Function formatAddressChi(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrictCode As String, ByVal strAreaCode As String) As String
            Dim validator As Common.Validation.Validator = New Common.Validation.Validator
            Dim udtAreaBLL As Component.Area.AreaBLL = New Component.Area.AreaBLL
            Dim udtDistrictBLL As Component.District.DistrictBLL = New Component.District.DistrictBLL

            Dim udtAreaModel As Component.Area.AreaModel
            Dim udtDistrictModel As Component.District.DistrictModel
            Dim strAddress = String.Empty

            If Not strAreaCode.Equals(String.Empty) Then
                udtAreaModel = udtAreaBLL.GetAreaNameByAreaCode(strAreaCode)
                If Not validator.IsEmpty(strAreaCode) Then
                    strAddress = strAddress + udtAreaModel.Area_ChiName.Trim
                End If
            End If

            If Not strDistrictCode.Equals(String.Empty) Then
                udtDistrictModel = udtDistrictBLL.GetDistrictNameByDistrictCode(strDistrictCode)
                If Not validator.IsEmpty(strDistrictCode) Then
                    strAddress = strAddress + udtDistrictModel.District_ChiName.Trim
                End If
            End If

            If Not validator.IsEmpty(strBuilding) Then
                strAddress = strAddress + strBuilding.Trim
            End If

            If Not validator.IsEmpty(strBlock) Then
                strAddress = strAddress + strBlock.Trim + "座"
            End If

            If Not validator.IsEmpty(strFloor) Then
                strAddress = strAddress + strFloor.Trim + "樓"
            End If

            If Not validator.IsEmpty(strRoom) Then
                strAddress = strAddress + strRoom.Trim + "室"
            End If

            If strBuilding.Trim().Equals(String.Empty) Then
                strAddress = String.Empty
            End If

            Return strAddress
        End Function


        Public Function FormatAddressChi(ByVal udtAddress As Common.Component.Address.AddressModel) As String
            Return FormatAddressChi(udtAddress, True)
        End Function

        Public Function FormatAddressChiWithoutDistrict(ByVal udtAddress As Common.Component.Address.AddressModel) As String
            Return formatAddressChi(udtAddress, False)
        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Protected Function FormatAddressChi(ByVal udtAddress As Common.Component.Address.AddressModel, ByVal blnIncludeDistrict As Boolean) As String
            Dim strAddress As String = String.Empty
            Dim strRoom As String = String.Empty
            Dim strFloor As String = String.Empty
            Dim strBlock As String = String.Empty
            Dim strBuilding As String = String.Empty
            Dim strDistrictCode As String = String.Empty
            Dim strAreaCode As String = String.Empty

            With udtAddress
                If Not .Address_Code.HasValue Then
                    strAreaCode = .AreaCode
                    strDistrictCode = .District
                Else
                    strAreaCode = String.Empty
                    strDistrictCode = String.Empty
                End If

                strBuilding = .ChiBuilding
                strBlock = .Block
                strFloor = .Floor
                strRoom = .Room
            End With

            If blnIncludeDistrict Then
                strAddress = FormatAddressChi(strRoom, strFloor, strBlock, strBuilding, strDistrictCode, strAreaCode)
            Else
                strAddress = FormatAddressChi(strRoom, strFloor, strBlock, strBuilding, String.Empty, String.Empty)
            End If

            Return strAddress
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

#Region "EC"
        Public Function formatReferenceNo(ByVal strOriReferenceNo As String, ByVal blnMasked As Boolean) As String
            ' To Do: Mask Refenence No
            Dim strRes As String
            Dim strMaskStr As String

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If String.IsNullOrEmpty(strOriReferenceNo) Then Return String.Empty
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            strMaskStr = "XXXXX"
            strOriReferenceNo = strOriReferenceNo.Trim.ToUpper()

            strRes = strOriReferenceNo.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty)

            If strRes.Length = 14 Then
                strRes = strRes.Substring(0, 4) + "-" + strRes.Substring(4, 7) + "-" + strRes.Substring(11, 2) + "(" + strRes.Substring(13, 1) + ")"
            End If

            If blnMasked Then
                'strOriReferenceNo = strOriReferenceNo.Substring(0, 9) + strMaskStr
            End If

            'If strOriReferenceNo.Trim() <> "" Then
            '    strRes = strOriReferenceNo.Substring(0, strOriReferenceNo.Length() - 1) + "(" + strOriReferenceNo.Substring(strOriReferenceNo.Length() - 1, 1) + ")"
            'End If

            Return strRes

        End Function

        Public Function formatECDORegistration(ByVal strLanguage As String, ByVal dtDOR As Nullable(Of Date)) As String

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Dim strRes As String = ""
            Dim providerUS As New System.Globalization.CultureInfo("en-us")
            Dim providerTW As New System.Globalization.CultureInfo("zh-tw")
            Dim providerCN As New System.Globalization.CultureInfo("zh-cn")

            Select Case UCase(strLanguage)
                Case CultureLanguage.TradChinese.ToUpper
                    strRes = dtDOR.Value.ToString("yyyy年MM月dd日", providerTW)
                Case CultureLanguage.SimpChinese.ToUpper
                    strRes = dtDOR.Value.ToString("yyyy年MM月dd日", providerCN)
                Case Else
                    strRes = dtDOR.Value.ToString("d MMMM yyyy", providerUS)
            End Select
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            Return strRes
        End Function

        Public Function formatECDOI(ByVal dtOriDOI As Date) As String
            Dim strRes As String = String.Empty
            Dim strDay, strMonth, strYear As String
            strDay = dtOriDOI.Day.ToString
            strMonth = dtOriDOI.Month.ToString
            strYear = dtOriDOI.Year.ToString

            Select Case strMonth
                Case "1"
                    strMonth = "January"
                Case "2"
                    strMonth = "February"
                Case "3"
                    strMonth = "March"
                Case "4"
                    strMonth = "April"
                Case "5"
                    strMonth = "May"
                Case "6"
                    strMonth = "June"
                Case "7"
                    strMonth = "July"
                Case "8"
                    strMonth = "August"
                Case "9"
                    strMonth = "September"
                Case "10"
                    strMonth = "October"
                Case "11"
                    strMonth = "November"
                Case "12"
                    strMonth = "December"
            End Select

            strRes = strDay + " " + strMonth + " " + strYear

            Return strRes
        End Function
#End Region

#Region "ID235B"
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function formatID235BPermittedToRemainUntil(ByVal dtmNullableRemainUntil As Nullable(Of Date)) As String
            ' If no input, then return ""
            If dtmNullableRemainUntil Is Nothing Then Return String.Empty

            'Convert Date? to Date
            Dim dtmRemainUntil As Date = dtmNullableRemainUntil.Value

            ' Same format as DOI
            Return dtmRemainUntil.ToString(strDisplayIssueDateLongFormat)

        End Function
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]


#End Region

#Region "Re-entry Permit"
        Public Function formatREPMTIssueDate(ByVal dtOriDOI As Date) As String
            'Same as formatECDOI
            Dim strRes As String = String.Empty
            Dim strDay, strMonth, strYear As String
            strDay = dtOriDOI.Day.ToString
            strMonth = dtOriDOI.Month.ToString
            strYear = dtOriDOI.Year.ToString

            Select Case strMonth
                Case "1"
                    strMonth = "January"
                Case "2"
                    strMonth = "February"
                Case "3"
                    strMonth = "March"
                Case "4"
                    strMonth = "April"
                Case "5"
                    strMonth = "May"
                Case "6"
                    strMonth = "June"
                Case "7"
                    strMonth = "July"
                Case "8"
                    strMonth = "August"
                Case "9"
                    strMonth = "September"
                Case "10"
                    strMonth = "October"
                Case "11"
                    strMonth = "November"
                Case "12"
                    strMonth = "December"
            End Select

            strRes = strDay + " " + strMonth + " " + strYear

            Return strRes
        End Function
#End Region

        Public Function formatSearchDate(ByVal strOriInputDate As String) As String
            Dim strOriInputDates As String()
            Dim strFormattedInputDate As String
            strOriInputDates = strOriInputDate.Split("-")

            If strOriInputDates.Length <> 3 Then
                strFormattedInputDate = strOriInputDate
            Else
                'Day
                If strOriInputDates(0).Length = 1 Then
                    strOriInputDates(0) = String.Format("0{0}", strOriInputDates(0))
                End If
                'Month
                If strOriInputDates(1).Length = 1 Then
                    strOriInputDates(1) = String.Format("0{0}", strOriInputDates(1))
                End If
                strFormattedInputDate = String.Format("{0}-{1}-{2}", strOriInputDates(0), strOriInputDates(1), strOriInputDates(2))

            End If



            Return strFormattedInputDate
        End Function

        Public Function formatIVRSDateTimeToString(ByVal dtmConvert As DateTime) As String
            Dim strFormatted As String = String.Empty
            strFormatted = CStr(dtmConvert.Year) + CStr(dtmConvert.Month).PadLeft(2, "0") + CStr(dtmConvert.Day).PadLeft(2, "0") + _
                            CStr(dtmConvert.Hour).PadLeft(2, "0") + CStr(dtmConvert.Minute).PadLeft(2, "0") + CStr(dtmConvert.Second).PadLeft(2, "0")

            ' Return string format : YYYYMMDDHHmmss
            Return strFormatted
        End Function

        Public Function formatIVRSStringToDatetime(ByVal strConvert As String, Optional ByVal ReturnDateOnly As Boolean = False) As DateTime
            Dim dtmFormatted As DateTime = New DateTime

            Dim intYear As Integer
            Dim intMonth As Integer
            Dim intDay As Integer
            Dim intHour As Integer
            Dim intMinute As Integer
            Dim intSecond As Integer

            ' Input format : YYYYMMDDHHmmss
            intYear = CInt(strConvert.Substring(0, 4))
            intMonth = CInt(strConvert.Substring(4, 2))
            intDay = CInt(strConvert.Substring(6, 2))

            If strConvert.Length > 8 Then
                intHour = CInt(strConvert.Substring(8, 2))
                intMinute = CInt(strConvert.Substring(10, 2))
                intSecond = CInt(strConvert.Substring(12, 2))
            End If

            If ReturnDateOnly Then
                dtmFormatted = New DateTime(intYear, intMonth, intDay, 0, 0, 0)
            Else
                dtmFormatted = New DateTime(intYear, intMonth, intDay, intHour, intMinute, intSecond)
            End If

            Return dtmFormatted
        End Function

        Public Function formatHKIDIssueDateBeforeValidate(ByVal strHKIDIssueDate As String) As String
            Dim strRes As String

            If strHKIDIssueDate.Length = 6 Then
                strRes = String.Format("{0}-{1}-{2}", strHKIDIssueDate.Substring(0, 2), strHKIDIssueDate.Substring(2, 2), strHKIDIssueDate.Substring(4, 2))
            Else
                strRes = strHKIDIssueDate
            End If

            Return strRes
        End Function

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Public Function formatPermitToReminUntilBeforeValidate(ByVal strPermit As String) As String
        '    Dim strRes As String

        '    If strPermit.Length = 10 Then
        '        strRes = String.Format("{0}-{1}-{2}", strPermit.Substring(0, 2), strPermit.Substring(2, 2), strPermit.Substring(4, 2))
        '    Else
        '        strRes = strPermit
        '    End If

        '    Return strRes
        'End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        '' Claim Service Date
        'Public Function formatClaimServiceDate(ByVal strServiceDate As String) As String

        '    Dim strReturn As String = strServiceDate.Trim()

        '    If strServiceDate.Length > 0 Then
        '        Dim arrService As String() = strServiceDate.Split("-")
        '        If arrService.Length = 3 Then
        '            If arrService(0).Length = 1 Then
        '                arrService(0) = "0" + arrService(0)
        '            End If

        '            If arrService(1).Length = 1 Then
        '                arrService(1) = "0" + arrService(1)
        '            End If
        '            strServiceDate = arrService(0) + "-" + arrService(1) + "-" + arrService(2)
        '            strReturn = strServiceDate.Trim()
        '        Else
        '            strReturn = formatDate(strServiceDate)
        '        End If
        '    End If
        '    Return strReturn
        'End Function

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function EnterDateFormat(Optional ByVal strLang As String = CultureLanguage.English) As String
            Select Case strLang
                Case CultureLanguage.English
                    Return strEnterDateFormat

                Case CultureLanguage.TradChinese
                    Return strEnterDateFormat

                Case CultureLanguage.SimpChinese
                    Return strEnterDateFormatCN

                Case Else
                    Return strCommonDateFormat
            End Select
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function formatInputTextDate(ByVal strInputTextDate As String, Optional ByVal strLang As String = CultureLanguage.English) As String
            Dim arrStrDate As String() = strInputTextDate.Split("-")
            Dim dtmRes As Date = Nothing

            If arrStrDate.Length = 3 Then
                Select Case strLang
                    Case CultureLanguage.English
                        dtmRes = New Date(arrStrDate(2).Trim(), arrStrDate(1).PadLeft(2, "0").Trim(), arrStrDate(0).PadLeft(2, "0").Trim())
                    Case CultureLanguage.TradChinese
                        dtmRes = New Date(arrStrDate(2).Trim(), arrStrDate(1).PadLeft(2, "0").Trim(), arrStrDate(0).PadLeft(2, "0").Trim())
                    Case CultureLanguage.SimpChinese
                        dtmRes = New Date(arrStrDate(2).Trim(), arrStrDate(1).PadLeft(2, "0").Trim(), arrStrDate(0).PadLeft(2, "0").Trim())
                    Case Else
                        dtmRes = New Date(arrStrDate(2).Trim(), arrStrDate(1).PadLeft(2, "0").Trim(), arrStrDate(0).PadLeft(2, "0").Trim())
                End Select

                Return formatInputTextDate(dtmRes, strLang)
            Else
                Return String.Empty
            End If

        End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function formatInputTextDate(ByVal dtmInputTextDate As Date, Optional ByVal strLang As String = CultureLanguage.English) As String
            Return dtmInputTextDate.ToString(EnterDateFormat(strLang))
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function formatDisplayDate(ByVal dtmDisplayDate As Date) As String

            If dtmDisplayDate = DateTime.MinValue Then
                Return String.Empty
            Else
                Select Case Thread.CurrentThread.CurrentUICulture.Name.ToLower
                    Case CultureLanguage.English
                        Return formatDisplayDate(dtmDisplayDate, CultureLanguage.English)
                    Case CultureLanguage.TradChinese
                        Return formatDisplayDate(dtmDisplayDate, CultureLanguage.TradChinese)
                    Case CultureLanguage.SimpChinese
                        Return formatDisplayDate(dtmDisplayDate, CultureLanguage.SimpChinese)
                    Case Else
                        Return formatDisplayDate(dtmDisplayDate, CultureLanguage.English)
                End Select
            End If

        End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function formatDisplayDate(ByVal strDisplayDate As String, Optional ByVal strLang As String = CultureLanguage.English) As String
            ' Common Format of strDisplayDate:
            ' dd-MM-yyyy
            Dim dtmDisplayDate As Date

            If strDisplayDate.Length > 0 Then
                dtmDisplayDate = New Date(strDisplayDate.Substring(6, 4), strDisplayDate.Substring(3, 2), strDisplayDate.Substring(0, 2))
                Return formatDisplayDate(dtmDisplayDate, strLang)
            Else
                Return strDisplayDate
            End If

        End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function formatDisplayDate(ByVal dtmDisplayDate As Date, Optional ByVal strLang As String = CultureLanguage.English) As String
            Dim strDate As String
            Dim strmonth As String = String.Empty
            strDate = ""

            If dtmDisplayDate = DateTime.MinValue Then
                strDate = ""
            Else
                Select Case strLang
                    Case CultureLanguage.English
                        strmonth = convertMonthNumtoEng(dtmDisplayDate.Month.ToString.PadLeft(2, "0"))
                        strDate = dtmDisplayDate.ToString("dd ") + strmonth + dtmDisplayDate.ToString(" yyyy")
                    Case CultureLanguage.TradChinese
                        strDate = dtmDisplayDate.ToString(strDisplayDateFormatChi)
                    Case CultureLanguage.SimpChinese
                        strDate = dtmDisplayDate.ToString(strDisplayDateFormatCN)
                    Case Else
                        strmonth = convertMonthNumtoEng(dtmDisplayDate.Month.ToString.PadLeft(2, "0"))
                        strDate = dtmDisplayDate.ToString("dd ") + strmonth + dtmDisplayDate.ToString(" yyyy")
                End Select
            End If

            Return strDate
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Public Function formatInputDate(ByVal dtmInputDate As Date) As String
            Return dtmInputDate.ToString(strCommonDateFormat)
        End Function

        Public Function formatInputDate(ByVal strInputDate As String, Optional ByVal strLang As String = CultureLanguage.English) As String
            ' Format Input Date: 
            ' 1. Pad 0 for : 1-1-2009
            ' 2. Separate with '-' : 01012009
            Dim strRes As String = String.Empty

            'INT15-0008 (Fix invalid character in date input for HCVU) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim rgx As Regex = Nothing
            'INT15-0008 (Fix invalid character in date input for HCVU) [End][Chris YIM]

            If strInputDate.Length > 0 Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Select Case strLang
                    Case CultureLanguage.English, CultureLanguage.TradChinese
                        If strInputDate.Contains("-") Then
                            Dim arrStrDate As String() = strInputDate.Split("-")
                            If arrStrDate.Length = 2 Then
                                strRes = arrStrDate(0).PadLeft(2, "0").Trim() + "-" + arrStrDate(1).Trim()
                            ElseIf arrStrDate.Length = 3 Then
                                strRes = arrStrDate(0).PadLeft(2, "0").Trim() + "-" + arrStrDate(1).PadLeft(2, "0").Trim() + "-" + arrStrDate(2).Trim()
                            Else
                                strRes = String.Empty
                            End If
                        Else
                            Select Case strInputDate.Length
                                'INT15-0008 (Fix invalid character in date input for HCVU) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                'Case 4, 10
                                '    strRes = strInputDate
                                Case 8
                                    rgx = New Regex("\d{8}", RegexOptions.IgnoreCase)
                                    If rgx.IsMatch(strInputDate) Then
                                        strRes = strInputDate.Substring(0, 2) + "-" + strInputDate.Substring(2, 2) + "-" + strInputDate.Substring(4, 4)
                                    Else
                                        strRes = strInputDate
                                    End If
                                Case 6
                                    rgx = New Regex("\d{6}", RegexOptions.IgnoreCase)
                                    If rgx.IsMatch(strInputDate) Then
                                        strRes = strInputDate.Substring(0, 2) + "-" + strInputDate.Substring(2, 4)
                                    Else
                                        strRes = strInputDate
                                    End If
                                Case Else
                                    strRes = strInputDate
                                    'INT15-0008 (Fix invalid character in date input for HCVU) [End][Chris YIM]
                            End Select
                        End If
                    Case CultureLanguage.SimpChinese
                        If strInputDate.Contains("-") Then
                            Dim arrStrDate As String() = strInputDate.Split("-")
                            If arrStrDate.Length = 2 Then
                                strRes = arrStrDate(1).PadLeft(2, "0").Trim() + "-" + arrStrDate(0).Trim()
                            ElseIf arrStrDate.Length = 3 Then
                                strRes = arrStrDate(2).PadLeft(2, "0").Trim() + "-" + arrStrDate(1).PadLeft(2, "0").Trim() + "-" + arrStrDate(0).Trim()
                            Else
                                strRes = String.Empty
                            End If
                        Else
                            Select Case strInputDate.Length
                                'INT15-0008 (Fix invalid character in date input for HCVU) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                'Case 4, 10
                                '    strRes = strInputDate
                                Case 8
                                    rgx = New Regex("\d{8}", RegexOptions.IgnoreCase)
                                    If rgx.IsMatch(strInputDate) Then
                                        strRes = strInputDate.Substring(6, 2) + "-" + strInputDate.Substring(4, 2) + "-" + strInputDate.Substring(0, 4)
                                    Else
                                        strRes = strInputDate
                                    End If
                                Case 6
                                    rgx = New Regex("\d{6}", RegexOptions.IgnoreCase)
                                    If rgx.IsMatch(strInputDate) Then
                                        strRes = strInputDate.Substring(4, 2) + "-" + strInputDate.Substring(0, 4)
                                    Else
                                        strRes = strInputDate
                                    End If
                                Case Else
                                    strRes = strInputDate
                                    'INT15-0008 (Fix invalid character in date input for HCVU) [End][Chris YIM]
                            End Select
                        End If
                    Case Else
                        If strInputDate.Contains("-") Then
                            Dim arrStrDate As String() = strInputDate.Split("-")
                            If arrStrDate.Length = 2 Then
                                strRes = arrStrDate(0).PadLeft(2, "0").Trim() + "-" + arrStrDate(1).Trim()
                            ElseIf arrStrDate.Length = 3 Then
                                strRes = arrStrDate(0).PadLeft(2, "0").Trim() + "-" + arrStrDate(1).PadLeft(2, "0").Trim() + "-" + arrStrDate(2).Trim()
                            Else
                                strRes = String.Empty
                            End If
                        Else
                            Select Case strInputDate.Length
                                'INT15-0008 (Fix invalid character in date input for HCVU) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                'Case 4, 10
                                '    strRes = strInputDate
                                Case 8
                                    rgx = New Regex("\d{8}", RegexOptions.IgnoreCase)
                                    If rgx.IsMatch(strInputDate) Then
                                        strRes = strInputDate.Substring(0, 2) + "-" + strInputDate.Substring(2, 2) + "-" + strInputDate.Substring(4, 4)
                                    Else
                                        strRes = strInputDate
                                    End If
                                Case 6
                                    rgx = New Regex("\d{8}", RegexOptions.IgnoreCase)
                                    If rgx.IsMatch(strInputDate) Then
                                        strRes = strInputDate.Substring(0, 2) + "-" + strInputDate.Substring(2, 4)
                                    Else
                                        strRes = strInputDate
                                    End If
                                Case Else
                                    strRes = strInputDate
                                    'INT15-0008 (Fix invalid character in date input for HCVU) [End][Chris YIM]
                            End Select
                        End If
                End Select

                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

            Return strRes

        End Function

        Public Sub seperateHKID(ByVal strSavedHKID As String, ByRef strHKIDPrefix As String, ByRef strHKID As String, ByRef strHKIDDigit As String)
            Dim intLen As Integer
            Dim strTemp As String = String.Empty
            intLen = strSavedHKID.Length

            strHKIDDigit = strSavedHKID.Substring(intLen - 1)
            strTemp = strSavedHKID.Remove(intLen - 1, 1)

            strHKID = strTemp.Substring(intLen - 7)
            strTemp = strTemp.Remove(strTemp.Length - 6, 6)

            strHKIDPrefix = strTemp

        End Sub


        ' TO DO!!! 
        'formatDocumentIdentityNumber
#Region "Document Related"

        ''' <summary>
        ''' Format the Document Accordingly
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function formatDocumentIdentityNumber(ByVal strDocCode As String, ByVal strIdentityNum As String) As String
            ' Do not Trim!
            Select Case strDocCode
                Case Component.DocType.DocTypeModel.DocTypeCode.HKIC, Component.DocType.DocTypeModel.DocTypeCode.HKBC, Component.DocType.DocTypeModel.DocTypeCode.EC
                    Select Case strIdentityNum.Trim().Length
                        Case 8
                            Return " " + strIdentityNum.Trim().ToUpper()
                        Case 9
                            Return strIdentityNum.Trim().ToUpper()
                    End Select
                Case Component.DocType.DocTypeModel.DocTypeCode.ADOPC
                    ' 5 Digit Only
                    Return strIdentityNum.Trim().ToUpper()
            End Select
            Return strIdentityNum.ToUpper()
        End Function

        ''' <summary>
        ''' Format the Doc Identity Number for displaying to user. Mask is needed if there is any privacy concern.
        ''' </summary>
        ''' <param name="strDocType"></param>
        ''' <param name="strDocIDNo"></param>
        ''' <param name="blnMask"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FormatDocIdentityNoForDisplay(ByVal strDocType As String, ByVal strDocIDNo As String, ByVal blnMask As Boolean, Optional ByVal strAdoptionPrefixNum As String = "") As String
            Dim strRes As String = String.Empty
            Dim strMaskStr As String = String.Empty
            Dim strMaskStrPrefix As String = String.Empty

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim udtValidator As New Common.Validation.Validator
            Dim udtSM As Common.ComObject.SystemMessage = Nothing

            ' Check Doc No. format
            udtSM = udtValidator.chkIdentityNumber(strDocType, strDocIDNo, strAdoptionPrefixNum)

            If udtSM IsNot Nothing Then
                ' Invalid Format
                Select Case strDocType
                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.ADOPC

                        'Mask sample 1  (too short, Add X up to 7 chars for Prefix, and add X up to 5 chars for Doc No.)
                        'Before Mark:   123/12
                        'After Mark:    12XXXXX/XXXXX

                        'Mask sample 2
                        'Before Mark:   1234567890/1234567890
                        'After Mark:    1234XXX/12XXX

                        If blnMask Then
                            If strAdoptionPrefixNum.Length <= 4 Then
                                strAdoptionPrefixNum = strAdoptionPrefixNum.Trim.PadRight(7, "X")
                            Else
                                strAdoptionPrefixNum = strAdoptionPrefixNum.Trim.Substring(0, 4).PadRight(7, "X")
                            End If

                            If strDocIDNo.Length <= 2 Then
                                strDocIDNo = strDocIDNo.Trim.PadRight(5, "X")
                            Else
                                strDocIDNo = strDocIDNo.Trim.Substring(0, 2).PadRight(5, "X")
                            End If
                        End If

                        If strAdoptionPrefixNum <> String.Empty Then
                            strRes = strAdoptionPrefixNum + "/" + strDocIDNo.Trim
                        Else
                            strRes = strDocIDNo.Trim
                        End If


                    Case Else
                        ' No formatting for doc no. with invalid format

                        'Mask sample 1  (too short, Add X up to 9 chars)
                        'Before Mark:   12
                        'After Mark:    12XXXXXXX

                        'Mask sample 2
                        'Before Mark:   12345678901234567890
                        'After Mark:    1234XXXXX

                        If blnMask Then
                            If strDocIDNo.Length <= 4 Then
                                ' 12XXXXXXX
                                strRes = strDocIDNo.Trim.PadRight(9, "X")
                            Else
                                ' 1234XXXXX
                                strRes = strDocIDNo.Trim.Substring(0, 4).PadRight(9, "X")
                            End If
                        Else
                            ' 12345678901234567890
                            strRes = strDocIDNo
                        End If
                End Select
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]

            Else
                ' Validate Format

                Select Case strDocType
                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.ADOPC
                        'X999999/99999

                        strMaskStr = "XXX"

                        If blnMask Then
                            strRes = strAdoptionPrefixNum.Substring(0, 4) + strMaskStr + "/" + strDocIDNo.Substring(0, 2) + strMaskStr
                        Else
                            strRes = strAdoptionPrefixNum + "/" + strDocIDNo.Trim
                        End If

                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.DI
                        'XXXXXXXXX

                        strMaskStr = "XXXXX"

                        If blnMask Then
                            strDocIDNo = strDocIDNo.Trim.Substring(0, 4) + strMaskStr
                        End If

                        strRes = strDocIDNo.Trim

                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.EC
                        strRes = formatHKID(strDocIDNo, blnMask)

                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.HKBC
                        'XX999999(X)

                        strRes = formatHKID(strDocIDNo, blnMask)

                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC
                        'XX999999(X)

                        strRes = formatHKID(strDocIDNo, blnMask)

                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.ID235B
                        'XX999999

                        strMaskStr = "XXX"

                        If blnMask Then
                            strDocIDNo = strDocIDNo.Trim.Substring(0, 5) + strMaskStr
                        End If

                        strRes = strDocIDNo.Trim

                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.REPMT
                        'XX9999999

                        strMaskStr = "XXXX"

                        If blnMask Then
                            strDocIDNo = strDocIDNo.Trim.Substring(0, 5) + strMaskStr
                        End If

                        strRes = strDocIDNo.Trim

                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.VISA
                        'XXXX-9999999-99(X)

                        strMaskStr = "XXXXX"

                        If blnMask Then
                            strDocIDNo = strDocIDNo.Trim.Substring(0, 9) + strMaskStr
                        End If

                        If Not strDocType.Trim.Equals(String.Empty) Then
                            strRes = strDocIDNo.Trim.Substring(0, 4) + "-" + strDocIDNo.Trim.Substring(4, 7) + "-" + strDocIDNo.Trim.Substring(11, 2) + "(" + strDocIDNo.Trim.Substring(13, 1) + ")"
                        End If

                        ' CRE19-001 (VSS 2019) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                    Case Common.Component.DocType.DocTypeModel.DocTypeCode.OC,
                        Common.Component.DocType.DocTypeModel.DocTypeCode.OW,
                        Common.Component.DocType.DocTypeModel.DocTypeCode.TW,
                        Common.Component.DocType.DocTypeModel.DocTypeCode.IR,
                        Common.Component.DocType.DocTypeModel.DocTypeCode.HKP,
                        Common.Component.DocType.DocTypeModel.DocTypeCode.RFNo8,
                        Common.Component.DocType.DocTypeModel.DocTypeCode.OTHER
                        ' CRE19-001 (VSS 2019) [End][Winnie]

                        'Mask sample 1
                        'Before Mark:   12345678901234567890
                        'After Mark:    1234XXXXXXXXXXXXXXXX

                        'Mask sample 2
                        'Before Mark:   1234567890
                        'After Mark:    1234XXXXXX

                        'Mask sample 3 (Too short, Add X up to 9 chars)
                        'Before Mark:   1234
                        'After Mark:    1234XXXXX

                        If blnMask Then
                            If strDocIDNo.Length <= 4 Then
                                ' 1234XXXXX
                                strRes = strDocIDNo.Trim.PadRight(9, "X")
                            Else
                                ' 1234XXXXXXXXXXXXXXXX
                                strRes = strDocIDNo.Trim.Substring(0, 4).PadRight(strDocIDNo.Length, "X")
                            End If

                        Else
                            ' 12345678901234567890
                            strRes = strDocIDNo
                        End If
                    Case Else
                        strRes = strDocIDNo
                End Select
            End If

            Return strRes.ToUpper
        End Function

        ''' <summary>
        ''' Format the Document Number To Exclude Non-Number Parts
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function formatDocumentIdentityNumberForIVRS(ByVal strDocCode As String, ByVal strIdentityNum As String) As String
            Select Case strDocCode
                Case Component.DocType.DocTypeModel.DocTypeCode.HKIC, Component.DocType.DocTypeModel.DocTypeCode.HKBC, Component.DocType.DocTypeModel.DocTypeCode.EC

                    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If strIdentityNum.Trim().Length >= 7 Then
                        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]
                        Return strIdentityNum.Trim().Substring(strIdentityNum.Trim().Length - 7, 7)
                    Else
                        Return ""
                    End If
                Case Else
                    Return ""
            End Select
            Return strIdentityNum
        End Function

        'Public Function formatDocumentIdentityNumberForImmd(ByVal strDocCode As String, ByVal strIdentityNum As String) As String
        '    Select Case strDocCode
        '        Case Component.DocType.DocTypeModel.AdoptionCert
        '            Return strIdentityNum.Substring(8, 5)
        '        Case Else
        '            Return String.Empty
        '    End Select
        '    Return strIdentityNum
        'End Function

        ''' <summary>
        ''' Format DOB for display for different document
        ''' </summary>
        ''' <param name="strDocType"></param>
        ''' <param name="dtDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="strLanguage"></param>
        ''' <param name="intAge"></param>
        ''' <param name="dtDOR"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function formatDOB(ByVal strDocType As String, ByVal dtDOB As Date, ByVal strExactDOB As String, ByVal strLanguage As String, ByVal intAge As Nullable(Of Integer), ByVal dtDOR As Nullable(Of Date), ByVal strOtherInfo As String) As String
            Dim strRes As String = String.Empty

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Select Case strDocType
                Case Common.Component.DocType.DocTypeModel.DocTypeCode.ADOPC
                    'DD-MM-YYYY
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)

                    ' DOB in word
                    If strExactDOB.Trim.Equals(Common.Component.EHSAccount.EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                        strExactDOB.Trim.Equals(Common.Component.EHSAccount.EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                        strExactDOB.Trim.Equals(Common.Component.EHSAccount.EHSAccountModel.ExactDOBClass.ManualExactYear) Then

                        If Not strOtherInfo.Trim.Equals(String.Empty) Then
                            Dim udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL
                            Dim udtStaticData As Common.Component.StaticData.StaticDataModel

                            udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", strOtherInfo)
                            If UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper OrElse
                                UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper Then
                                strRes = udtStaticData.DataValueChi.ToString.Trim + " " + strRes
                            Else
                                strRes = udtStaticData.DataValue.ToString.Trim + " " + strRes
                            End If
                        End If

                    End If

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.DI
                    'DD-MM-YYYY / XX-MM-YYYY / XX-XX-YYYY
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)
                    'Select Case strExactDOB
                    '    Case "Y"
                    '        If UCase(strLanguage) = "ZH-TW" Then
                    '            strRes = dtDOB.ToString(strDisplayDOBYearFormatChi)
                    '        Else
                    '            strRes = dtDOB.ToString(strDisplayDOBYearFormat)
                    '        End If

                    '        'strRes = "XX-XX" + strRes
                    '    Case "M"
                    '        If UCase(strLanguage) = "ZH-TW" Then
                    '            strRes = dtDOB.ToString(strDisplayDOBMonthFormatChi)
                    '        Else
                    '            strRes = dtDOB.ToString(strDisplayDOBMonthFormat)
                    '        End If

                    '        'strRes = "XX-" + strRes
                    '    Case "D"
                    '        If UCase(strLanguage) = "ZH-TW" Then
                    '            strRes = dtDOB.ToString(strDisplayDOBDayFormatChi)
                    '        Else
                    '            strRes = dtDOB.ToString(strDisplayDOBDayFormat)
                    '        End If

                    'End Select

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.EC
                    'DD-MM-YYYY / MM-YYYY / YYYY/ Age XX on d MMM yyyy
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.HKBC
                    'DD-MM-YYYY / MM-YYYY / YYYY
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)

                    ' DOB in word
                    If strExactDOB.Trim.Equals(Common.Component.EHSAccount.EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                        strExactDOB.Trim.Equals(Common.Component.EHSAccount.EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                        strExactDOB.Trim.Equals(Common.Component.EHSAccount.EHSAccountModel.ExactDOBClass.ManualExactYear) Then

                        If Not strOtherInfo.Trim.Equals(String.Empty) Then
                            Dim udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL
                            Dim udtStaticData As Common.Component.StaticData.StaticDataModel

                            udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("DOBInWordType", strOtherInfo)
                            If UCase(strLanguage) = CultureLanguage.TradChinese.ToUpper OrElse
                                UCase(strLanguage) = CultureLanguage.SimpChinese.ToUpper Then
                                strRes = udtStaticData.DataValueChi.ToString.Trim + " " + strRes
                            Else
                                strRes = udtStaticData.DataValue.ToString.Trim + " " + strRes
                            End If
                        End If

                    End If


                Case Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC
                    'DD-MM-YYYY / MM-YYYY / YYYY
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.ID235B
                    'DD-MM-YYYY
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.REPMT
                    'DD-MM-YYYY / XX-MM-YYYY / XX-XX-YYYY
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)
                    'Select Case strExactDOB
                    '    Case "Y"
                    '        If UCase(strLanguage) = "ZH-TW" Then
                    '            strRes = dtDOB.ToString(strDisplayDOBYearFormatChi)
                    '        Else
                    '            strRes = dtDOB.ToString(strDisplayDOBYearFormat)
                    '        End If

                    '        'strRes = "XX-XX" + strRes
                    '    Case "M"
                    '        If UCase(strLanguage) = "ZH-TW" Then
                    '            strRes = dtDOB.ToString(strDisplayDOBMonthFormatChi)
                    '        Else
                    '            strRes = dtDOB.ToString(strDisplayDOBMonthFormat)
                    '        End If

                    '        'strRes = "XX-" + strRes
                    '    Case "D"
                    '        If UCase(strLanguage) = "ZH-TW" Then
                    '            strRes = dtDOB.ToString(strDisplayDOBDayFormatChi)
                    '        Else
                    '            strRes = dtDOB.ToString(strDisplayDOBDayFormat)
                    '        End If

                    'End Select

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.VISA
                    'DD-MM-YYYY / MM-YYYY / YYYY
                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)

                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case Common.Component.DocType.DocTypeModel.DocTypeCode.OC,
                    Common.Component.DocType.DocTypeModel.DocTypeCode.OW,
                    Common.Component.DocType.DocTypeModel.DocTypeCode.TW,
                    Common.Component.DocType.DocTypeModel.DocTypeCode.IR,
                    Common.Component.DocType.DocTypeModel.DocTypeCode.HKP,
                    Common.Component.DocType.DocTypeModel.DocTypeCode.RFNo8,
                    Common.Component.DocType.DocTypeModel.DocTypeCode.OTHER
                    ' CRE19-001 (VSS 2019) [End][Winnie]

                    strRes = formatDOB(dtDOB, strExactDOB, strLanguage, intAge, dtDOR)
                    
            End Select
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            Return strRes

        End Function

        Public Function formatDOI_GV(ByVal dtDOI As Nullable(Of DateTime))
            Dim strRes As String = String.Empty

            If dtDOI.HasValue Then
                strRes = dtDOI.Value.ToString(strDisplayDOBDayFormat)
            End If

            Return strRes

        End Function

        Public Function formatDOI(ByVal strDocType As String, ByVal dtDOI As Nullable(Of DateTime))
            Dim strRes As String = String.Empty

            If dtDOI.HasValue = False Then
                Return String.Empty
            End If

            Select Case strDocType
                Case Common.Component.DocType.DocTypeModel.DocTypeCode.ADOPC
                    'No Date of Issue

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.DI
                    'DD-MM-YYYY
                    strRes = dtDOI.Value.ToString(strDisplayDOBDayFormat)


                Case Common.Component.DocType.DocTypeModel.DocTypeCode.EC
                    'DD-MM-YYYY
                    strRes = formatECDOI(dtDOI)

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.HKBC
                    'No Date of Issue

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC
                    'DD-MM-YYYY
                    strRes = formatHKIDIssueDate(dtDOI.Value)

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.ID235B
                    'No Date of Issue

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.REPMT
                    'DD-MM-YYYY
                    strRes = dtDOI.Value.ToString(strDisplayDOBDayFormat)

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.VISA
                    'No Date of Issue
            End Select

            Return strRes
        End Function

        Public Function formatDateBeforValidation_DDMMYYYY(ByVal strPermit As String) As String
            Dim strRes As String

            Dim strTempPermit As String = strPermit.Replace("-", "")

            If strTempPermit.Length = 8 Then
                strRes = String.Format("{0}-{1}-{2}", strTempPermit.Substring(0, 2), strTempPermit.Substring(2, 2), strTempPermit.Substring(4, 4))
            Else
                strRes = strPermit
            End If

            Return strRes
        End Function

        'Public Function formatDate_DDMMYYYY(ByVal dtDate As Date) As String
        '    Dim strRes As String

        '    strRes = dtDate.ToString(strDisplayIssueDateLongFormat)

        '    Return strRes
        'End Function


        'Public Function convertDateOfIssueStringToDate_DDMMYYYY(ByVal strDOI As String) As Date
        '    Dim strRes As String
        '    Dim dtRes As Date
        '    Dim strDay, strMonth, strYear, strConMonth As String
        '    strRes = ""
        '    If strDOI.Length = 10 Then
        '        strDay = strDOI.Substring(0, 2)
        '        strMonth = strDOI.Substring(3, 2)
        '        strYear = strDOI.Substring(6, 4)

        '        strConMonth = ""
        '        strConMonth = convertMonthNumtoEng(strMonth)

        '        strRes = strDay + " " + strConMonth + " " + strYear

        '    End If

        '    dtRes = CType(strRes, Date)

        '    Return dtRes

        'End Function
#End Region

        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Public Function FormatFileExt(ByVal strFileExt As String) As String
            Dim strExt As String = ""

            If String.IsNullOrEmpty(strFileExt) = False Then
                strExt = strFileExt.Trim.ToLower()

                If Left(strExt, 1) <> "." Then
                    strExt = "." & strExt
                End If
            End If

            Return strExt
        End Function
        'CRE13-016 Upgrade to excel 2007 [End][Karl]

#Region "Splitter"

        'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function formatLineBreak(ByVal strInputWithBR As String) As String
            Dim strResult As String
            strResult = strInputWithBR.Replace("<BR>", Environment.NewLine)

            Return strResult
        End Function
        'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]
#End Region

#Region "Enum"

        Public Shared Function EnumToString(ByVal enumValue As [Enum]) As String
            Dim fi As FieldInfo = enumValue.GetType().GetField(enumValue.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            If aattr.Length > 0 Then
                Return aattr(0).Description.Trim
            Else
                Return enumValue.ToString().Trim
            End If
        End Function

        Public Shared Function StringToEnum(ByVal enumType As Type, ByVal strValue As String) As Object
            For Each obj As Object In [Enum].GetValues(enumType)
                If strValue = EnumToString(obj) Then
                    Return obj
                End If
            Next

            Return Nothing

        End Function

#End Region

    End Class

End Namespace