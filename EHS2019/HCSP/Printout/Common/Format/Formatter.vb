' CRE13-018 - Change Voucher Amount to 1 Dollar [Tommy L]
' -----------------------------------------------------------------------------------------
' Relocated from [FGS]

Imports GrapeCity.ActiveReports.SectionReportModel

Namespace PrintOut.Common.Format

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
            Dim sigNoTextWidth As Single = TextBox.Width

            If Not strText Is Nothing Then
                textBox.Text = strText

                If Not strText.Equals(String.Empty) Then
                    textBox.Style += ";text-decoration: underline;"
                    textBox.Height += 0.025!
                    textBox.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
                Else
                    TextBox.Width = sigNoTextWidth
                    textBox.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
                End If
            Else
                TextBox.Width = sigNoTextWidth
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

    End Class

End Namespace
