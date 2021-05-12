' CRE13-018 - Change Voucher Amount to 1 Dollar [Tommy L]
' -----------------------------------------------------------------------------------------
' Relocated from [FGS]

Imports GrapeCity.ActiveReports.SectionReportModel
Imports Common

Namespace Component.COVID19.PrintOut.Common.Format

    Public Class Formatter

        Public Enum EnumDateFormat
            YYYYMMDD
            DDMMYYYY
        End Enum

        Public Enum EnumLang
            Chi
            Eng
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            SimpChi
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End Enum


        Public Shared Function ConvertHKID(ByVal strHKID As String) As String
            Return strHKID.Trim.Replace("(", String.Empty).Replace(")", String.Empty)
        End Function

        Public Shared Function ConvertECReferenceNo(ByVal strECRef As String) As String
            Return strECRef.Trim.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty)
        End Function

        Public Shared Function ConvertDOI(ByVal strDOI As String, ByVal eDateFormat As EnumDateFormat) As Date
            strDOI = strDOI.Trim

            Dim strDD As String = strDOI.Substring(0, 2)
            Dim strMM As String = strDOI.Substring(3, 2)
            Dim strYY As String = strDOI.Substring(6, 2)

            If CInt(strYY) > 60 Then
                strYY = "19" + strYY
            Else
                strYY = "20" + strYY
            End If

            Return CType(String.Format("{0}-{1}-{2}", strYY, strMM, strDD), Date)

        End Function

        Public Shared Function ConvertDate(ByVal strDate As String, ByVal eDateFormat As EnumDateFormat) As Date
            strDate = strDate.Trim

            Dim strDD As String = String.Empty
            Dim strMM As String = String.Empty
            Dim strYY As String = String.Empty

            If eDateFormat = EnumDateFormat.YYYYMMDD Then
                strDD = strDate.Substring(8, 2)
                strMM = strDate.Substring(5, 2)
                strYY = strDate.Substring(0, 4)

            ElseIf eDateFormat = EnumDateFormat.DDMMYYYY Then
                strDD = strDate.Substring(0, 2)
                strMM = strDate.Substring(3, 2)
                strYY = strDate.Substring(6, 4)

            End If

            Return CType(String.Format("{0}-{1}-{2}", strYY, strMM, strDD), Date)

        End Function

        Public Shared Function ConvertEName(ByVal strSurname As String, ByVal strGivenname As String) As String
            Return String.Format("{0}, {1}", strSurname.Trim, strGivenname.Trim)
        End Function

        '

        Public Shared Function FormatDate(ByVal dtmDate As Date, ByVal eDateFormat As EnumDateFormat) As String
            Select Case eDateFormat
                Case EnumDateFormat.YYYYMMDD
                    Return dtmDate.ToString("yyyy-MM-dd")
                Case EnumDateFormat.DDMMYYYY
                    Return dtmDate.ToString("dd-MM-yyyy")
            End Select

            Return Nothing

        End Function

        Public Shared Function FormatDateChinese(ByVal dtmDate As Date) As String
            Return dtmDate.ToString("yyyy年MM月dd日")
        End Function

        Public Shared Function FormatHKID(ByVal strHKID As String) As String
            strHKID = strHKID.Trim
            Return strHKID.Substring(0, strHKID.Length - 1) + "(" + strHKID.Substring(strHKID.Length - 1, 1) + ")"
        End Function

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function FormatMoney(ByVal strMoney As String, ByVal enumLang As EnumLang) As String
            Select Case enumLang
                Case Formatter.EnumLang.Chi
                    Return "港幣 " + strMoney + " 元"

                Case Formatter.EnumLang.Eng
                    Return "HK$ " + strMoney

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Case Formatter.EnumLang.SimpChi
                    Return "港币 " + strMoney + " 元"
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]
            End Select

            Return Nothing
        End Function
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Shared Function FormatMoneyRMB(ByVal strMoney As String, ByVal enumLang As EnumLang) As String
            Select Case enumLang
                Case Formatter.EnumLang.Chi
                    Return "人民幣 " + strMoney + " 元"

                Case Formatter.EnumLang.Eng
                    Return "RMB¥ " + strMoney

                Case Formatter.EnumLang.SimpChi
                    Return "人民币 " + strMoney + " 元"
            End Select

            Return Nothing
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Shared Sub FormatUnderLineTextBox(ByVal strText As String, ByVal textBox As GrapeCity.ActiveReports.SectionReportModel.TextBox)
            Dim sigNoTextWidth As Single = textBox.Width

            If Not strText Is Nothing Then
                textBox.Text = strText

                If Not strText.Equals(String.Empty) Then
                    textBox.Style += ";text-decoration: underline;"
                    textBox.Height += 0.025!
                    textBox.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
                Else
                    textBox.Width = sigNoTextWidth
                    textBox.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                End If
            Else
                textBox.Width = sigNoTextWidth
                textBox.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            End If
        End Sub

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'Change Function Name
        'Public Shared Sub FillSPName(ByRef strSPName As String, ByRef txtTextBox1 As TextBox, Optional ByRef txtTextBox2 As TextBox = Nothing, Optional ByVal intTextBox1Width As Integer = 0)
        Public Shared Sub FillSeperateText(ByRef strText As String, ByRef txtTextBox1 As TextBox, Optional ByRef txtTextBox2 As TextBox = Nothing, Optional ByVal intTextBox1Width As Integer = 0)
            If txtTextBox2 Is Nothing Then
                ' put the Text into textbox1
                txtTextBox1.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                txtTextBox1.Text = strText
            Else
                ' split SP name into 2 textbox
                Dim strSPNames As String() = strText.Split(" ")
                If strSPNames.Length > 1 Then
                    Dim intSPNameLength As Integer = 0
                    For Each partailName As String In strSPNames
                        '+ 1 is the spacing
                        intSPNameLength += partailName.Length + 1

                        'text will show in txtSPName1, if the # of characters not larger than the width pass-ed in by caller
                        If intSPNameLength <= intTextBox1Width Then
                            txtTextBox1.Text += String.Format("{0} ", partailName)
                            txtTextBox1.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right
                        Else
                            txtTextBox2.Text += String.Format("{0} ", partailName)
                            txtTextBox2.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left
                        End If
                    Next
                Else
                    txtTextBox2.Text = strText
                End If

                If String.IsNullOrEmpty(txtTextBox2.Text) Then
                    txtTextBox2.Text = "　" ' put a space to the label. show the rendered textbox will be in the same line as other control
                    txtTextBox1.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                End If
                If String.IsNullOrEmpty(txtTextBox1.Text) Then
                    txtTextBox1.Text = "　" ' put a space to the label. show the rendered textbox will be in the same line as other control
                    txtTextBox2.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                End If

            End If

        End Sub
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Shared Sub SetSPNameFontSize(ByVal strLang As EnumLang, ByVal SPName As String, ByRef textBox As GrapeCity.ActiveReports.SectionReportModel.TextBox)
        Public Shared Sub SetSPNameFontSize(ByVal strLang As EnumLang, ByVal SPName As String, ByRef textBox As GrapeCity.ActiveReports.SectionReportModel.TextBox, Optional ByVal blnForVoucherNotice As Boolean = False)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            Dim strSPNameStyle_Normal As String = String.Empty
            Dim strSPNameStyle_30 As String = String.Empty

            'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            '' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            '' -----------------------------------------------------------------------------------------
            'If blnForVoucherNotice Then
            '    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            '    'If strLang = EnumLang.Chi Then
            '    If strLang = EnumLang.Chi OrElse strLang = EnumLang.SimpChi Then
            '        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            '        strSPNameStyle_Normal = "font-size: 13pt;"
            '        strSPNameStyle_30 = "font-size: 13pt;"
            '    Else
            '        strSPNameStyle_Normal = "font-size: 12pt;"
            '        strSPNameStyle_30 = "font-size: 11pt;"
            '    End If
            'Else
            '    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            '    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            '    'If strLang = EnumLang.Chi Then
            '    If strLang = EnumLang.Chi OrElse strLang = EnumLang.SimpChi Then
            '        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            '        strSPNameStyle_Normal = "font-size: 12pt;"
            '        strSPNameStyle_30 = "font-size: 10pt;"
            '    Else
            '        strSPNameStyle_Normal = "font-size: 11.25pt;"
            '        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            '        ' -----------------------------------------------------------------------------------------
            '        'strSPNameStyle_30 = "font-size: 8.55pt;"
            '        strSPNameStyle_30 = "font-size: 10.25pt;"
            '        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            '    End If
            '    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            '    ' -----------------------------------------------------------------------------------------
            'End If
            '' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------

            If blnForVoucherNotice Then
                If strLang = EnumLang.Chi OrElse strLang = EnumLang.SimpChi Then
                    strSPNameStyle_Normal = "font-size: 13pt;"
                    strSPNameStyle_30 = "font-size: 13pt;"
                Else
                    strSPNameStyle_Normal = "font-size: 12pt;"
                    strSPNameStyle_30 = "font-size: 11pt;"
                End If
            Else
                If strLang = EnumLang.Chi Then
                    strSPNameStyle_Normal = "font-size: 12pt;"
                    strSPNameStyle_30 = "font-size: 10pt;"
                ElseIf strLang = EnumLang.SimpChi Then
                    strSPNameStyle_Normal = "font-size: 11pt;"
                    strSPNameStyle_30 = "font-size: 8pt;"
                Else
                    strSPNameStyle_Normal = "font-size: 11.25pt;"
                    strSPNameStyle_30 = "font-size: 10.25pt;"
                End If
            End If

            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

            Dim strSPNameStyle As String = textBox.Style.ToString

            Dim startIndex As Integer = strSPNameStyle.IndexOf("font-size:")
            Dim endIndex As Integer = strSPNameStyle.IndexOf("pt;", startIndex) + "pt;".Length - 1

            Dim strSPNameStyleSubString As String = strSPNameStyle.Substring(startIndex, endIndex - startIndex + 1)



            If SPName.Length > 30 Then
                strSPNameStyle = strSPNameStyle.Replace(strSPNameStyleSubString, strSPNameStyle_30)
            Else
                strSPNameStyle = strSPNameStyle.Replace(strSPNameStyleSubString, strSPNameStyle_Normal)
            End If

            textBox.Style = strSPNameStyle
        End Sub

        ''' <summary>
        ''' Format the Doc Identity Number for QR Code displaying to user. Mask is needed if there is any privacy concern.
        ''' </summary>
        ''' <param name="strDocType"></param>
        ''' <param name="strDocIDNo"></param>
        ''' <param name="blnMask"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FormatDocIdentityNoForQrCodeDisplay(ByVal strDocType As String, ByVal strDocIDNo As String, ByVal blnMask As Boolean, Optional ByVal strAdoptionPrefixNum As String = "") As String
            Dim strRes As String = String.Empty
            Dim strMaskStr As String = String.Empty
            Dim strMaskStrPrefix As String = String.Empty

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim udtValidator As New Global.Common.Validation.Validator
            Dim udtSM As Global.Common.ComObject.SystemMessage = Nothing

            ' Check Doc No. format
            udtSM = udtValidator.chkIdentityNumber(strDocType, strDocIDNo, strAdoptionPrefixNum)

            If udtSM IsNot Nothing Then
                ' Invalid Format
                Select Case strDocType
                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.ADOPC

                        'Mask sample 1  (too short, Add X up to 7 chars for Prefix, and add X up to 5 chars for Doc No.)
                        'Before Mark:   123/12
                        'After Mark:    12XXXXX/XXXXX

                        'Mask sample 2
                        'Before Mark:   1234567890/1234567890
                        'After Mark:    1234XXX/12XXX

                        If blnMask Then
                            If strAdoptionPrefixNum.Length <= 4 Then
                                strAdoptionPrefixNum = (Right(strAdoptionPrefixNum.Trim, strAdoptionPrefixNum.Length \ 2)).PadLeft(8, "*")
                            Else
                                strAdoptionPrefixNum = (Right(strAdoptionPrefixNum.Trim, 4)).PadLeft(8, "*")
                            End If

                            If strDocIDNo.Length <= 2 Then
                                strDocIDNo = strDocIDNo.Trim.PadLeft(5, "*")
                            Else
                                strDocIDNo = (Right(strAdoptionPrefixNum.Trim, 2)).PadLeft(5, "*")
                            End If
                        End If

                        If strAdoptionPrefixNum <> String.Empty Then
                            strRes = strAdoptionPrefixNum.Trim + "/" + strDocIDNo.Trim
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
                                strRes = (Right(strDocIDNo.Trim, strAdoptionPrefixNum.Length \ 2)).PadLeft(9, "*")

                            Else
                                ' 1234XXXXX
                                strRes = (Right(strDocIDNo.Trim, 4)).PadLeft(9, "*")
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
                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.ADOPC
                        '***9999/**999

                        strMaskStr = "***"

                        If blnMask Then
                            strRes = (Right(strAdoptionPrefixNum.Trim, 4)).PadLeft(7, "*") + "/" + (Right(strAdoptionPrefixNum.Trim, 2)).PadLeft(5, "*")
                        Else
                            strRes = strAdoptionPrefixNum + "/" + strDocIDNo.Trim
                        End If

                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.DI
                        'XXXXXXXXX

                        strMaskStr = "*****"

                        If blnMask Then
                            strDocIDNo = (Right(strAdoptionPrefixNum.Trim, 4)).PadLeft(9, "*")
                        End If

                        strRes = strDocIDNo.Trim

                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.EC
                        strRes = FormatHKID(strDocIDNo, blnMask)

                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.HKBC, _
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.ROP140, _
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.CCIC
                        'XX999999(X)

                        strRes = FormatHKID(strDocIDNo, blnMask)

                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC
                        'XX999999(X)

                        strRes = FormatHKID(strDocIDNo, blnMask)

                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.ID235B
                        'XX999999

                        strMaskStr = "***"

                        If blnMask Then
                            strDocIDNo = (Right(strAdoptionPrefixNum.Trim, 5)).PadLeft(8, "*")
                        End If

                        strRes = strDocIDNo.Trim

                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.REPMT
                        'XX9999999

                        strMaskStr = "****"

                        If blnMask Then
                            strDocIDNo = (Right(strAdoptionPrefixNum.Trim, 5)).PadLeft(9, "*")
                        End If

                        strRes = strDocIDNo.Trim

                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.VISA
                        'XXXX-9999999-99(X)

                        strMaskStr = "*****"

                        If blnMask Then
                            strDocIDNo = (Right(strAdoptionPrefixNum.Trim, 9)).PadLeft(14, "*")
                        End If

                        If Not strDocType.Trim.Equals(String.Empty) Then
                            strRes = strDocIDNo.Trim.Substring(0, 4) + "-" + strDocIDNo.Trim.Substring(4, 7) + "-" + strDocIDNo.Trim.Substring(11, 2) + "(" + strDocIDNo.Trim.Substring(13, 1) + ")"
                        End If

                        ' CRE19-001 (VSS 2019) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                    Case Global.Common.Component.DocType.DocTypeModel.DocTypeCode.OC,
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.OW,
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.TW,
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.IR,
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.HKP,
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.RFNo8,
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.OTHER,
                        Global.Common.Component.DocType.DocTypeModel.DocTypeCode.PASS
                        ' CRE19-001 (VSS 2019) [End][Winnie]

                        'Mask sample 1
                        'Before Mark:   12345678901234567890
                        'After Mark:    XXXXXXXXXXXXXXXX7890

                        'Mask sample 2
                        'Before Mark:   1234567890
                        'After Mark:    XXXXXX7890

                        'Mask sample 3 (Too short, Add X up to 9 chars)
                        'Before Mark:   1234
                        'After Mark:    XXXXX1234


                        If blnMask Then
                            If strDocIDNo.Length <= 4 Then
                                ' XXXXX1234
                                strRes = (Right(strDocIDNo, 4)).PadLeft(9, "*")
                            Else
                                'Calculate the start position to mask
                                Dim intPos As Integer = 0
                                Dim intChar As Integer = 0
                                Dim intCnt As Integer = 4

                                For idx As Integer = strDocIDNo.Length - 1 To 0 Step -1
                                    Dim strChar As String = strDocIDNo.Chars(idx).ToString

                                    If strChar <> "(" AndAlso strChar <> ")" AndAlso strChar <> "-" AndAlso strChar <> "/" Then
                                        intChar = intChar + 1
                                    End If

                                    intPos = intPos + 1

                                    If intChar = intCnt Then
                                        Exit For
                                    End If
                                Next

                                ' XXXXXXXXXXXXXXXX1234
                                strRes = (Right(strDocIDNo, intPos)).PadLeft(strDocIDNo.Length, "*")
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
        Public Shared Function formatHKID(ByVal strOriHKID As String, ByVal blnMasked As Boolean) As String
            Dim strRes As String
            Dim strMaskStr As String
            strMaskStr = "****"
            strOriHKID = strOriHKID.Trim()
            strRes = strOriHKID

            If blnMasked Then
                Select Case strOriHKID.Length
                    Case 8
                        strOriHKID = (Right(strOriHKID, 4)).PadLeft(8, "*")
                    Case 9
                        strOriHKID = (Right(strOriHKID, 5)).PadLeft(9, "*")
                End Select
            End If

            If Trim(strOriHKID) <> "" Then
                strRes = strOriHKID.Substring(0, strOriHKID.Length() - 1) + "(" + strOriHKID.Substring(strOriHKID.Length() - 1, 1) + ")"
            End If
            strRes = strRes.ToUpper
            Return strRes
        End Function


        Public Shared Function maskEnglishNameByStar(ByVal strName As String) As String

            Dim strFullName() As String = strName.Trim().Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)
            Dim strExtractedName As String = ""

            ' Example: 
            ' Input: AU YEUNG, TAI MAN
            ' Output: AU Y****, T** M**

            For y As Integer = 0 To strFullName.Length - 1
                Dim strNameTemp As String = strFullName(y)
                Dim strExtractedWord As String = ""

                If y = 0 Then 'except the first word
                    strExtractedName &= strNameTemp + " "
                Else
                    For x As Integer = 0 To strNameTemp.Length - 1
                        If x = 0 Or strNameTemp(x) = "," Then 'keep the first character and the ","
                            strExtractedWord += strNameTemp(x)
                        Else
                            strExtractedWord += "*"
                        End If
                    Next
                    strExtractedName &= strExtractedWord + " "
                End If
            Next
            Return strExtractedName.TrimEnd(" ")
        End Function



    End Class

End Namespace
