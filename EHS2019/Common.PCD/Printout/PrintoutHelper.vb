Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component
Imports System.Web
Imports Common.ComFunction

Namespace Printout.EnrolmentInformation

    Public Class PrintoutHelper

        Private GeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Public FamilyNameChinese As String = PDFPrintOutFontFamily("PDFPrintOutFontFamilyChinese")
        Public FamilyNameEnglish As String = PDFPrintOutFontFamily("PDFPrintOutFontFamilyEnglish")
        Public FamilyNameMultiLingual As String = PDFPrintOutFontFamily("PDFPrintOutFontFamilyMultiLingual")


#Region "Methods"

        Public Function PDFPrintOutFontFamily(ByVal strParameterName As String) As String
            Dim strPDFPrintOutFontFamily As String = String.Empty
            GeneralFunction.getSystemParameter(strParameterName, strPDFPrintOutFontFamily, String.Empty)
            Return strPDFPrintOutFontFamily
        End Function

        Public Sub RenderResource(ByRef txt As TextBox, ByVal strResourceKey As String, Optional ByVal intFontSize As Integer = 10, Optional ByVal blnAddNotDiscloseNotation As Boolean = False, Optional ByVal blnColon As Boolean = False, Optional ByVal blnBold As Boolean = False, Optional ByVal blnItalic As Boolean = False, Optional ByVal blnUnderline As Boolean = False)
            txt.Text = GetTextResource(strResourceKey)
            If blnAddNotDiscloseNotation Then txt.Text += GetNotDiscloseNotation()
            If blnColon Then txt.Text += GetLabelValueSeparator()
            RenderTextBoxFont(txt, intFontSize, blnBold, blnItalic, blnUnderline)
        End Sub

        Public Sub RenderText(ByRef txt As TextBox, ByVal strText As String, Optional ByVal intFontSize As Integer = 10, Optional ByVal blnMultiLingual As Boolean = False, Optional ByVal blnColon As Boolean = False, Optional ByVal blnBold As Boolean = False, Optional ByVal blnItalic As Boolean = False, Optional ByVal blnUnderline As Boolean = False)
            txt.Text = strText
            If blnColon Then txt.Text += GetLabelValueSeparator()

            If blnMultiLingual Then
                RenderMultiLingualTextBoxFont(txt, intFontSize, blnBold, blnItalic, blnUnderline)
            Else
                RenderTextBoxFont(txt, intFontSize, blnBold, blnItalic, blnUnderline)
            End If
        End Sub

        Public Sub RenderText(ByRef ri As ReportInfo, ByVal strFormatString As String, Optional ByVal intFontSize As Integer = 9, Optional ByVal blnBold As Boolean = False, Optional ByVal blnItalic As Boolean = False, Optional ByVal blnUnderline As Boolean = False)
            ri.FormatString = strFormatString
            RenderReportInfoFont(ri, intFontSize, blnBold, blnItalic, blnUnderline)
        End Sub

        Public Sub RenderValue(ByRef txt As TextBox, ByVal strValue As String, Optional ByVal intFontSize As Integer = 10, Optional ByVal blnMultiLingual As Boolean = False)
            If strValue <> String.Empty Then
                txt.Text = strValue

                If blnMultiLingual Then
                    RenderMultiLingualTextBoxFont(txt, intFontSize, False, False, False)
                Else
                    RenderTextBoxFont(txt, intFontSize, False, False, False)
                End If
            Else
                txt.Text = GetNotProvidedNotation()
                RenderTextBoxFont(txt, intFontSize, False, True, False)
            End If
        End Sub

        Private Sub RenderTextBoxFont(ByRef txt As TextBox, ByVal intFontSize As Integer, ByVal blnBold As Boolean, ByVal blnItalic As Boolean, ByVal blnUnderline As Boolean)
            Dim style As Drawing.FontStyle = GetFontStyle(blnBold, blnItalic, blnUnderline)

            If _strLanguage = CultureLanguage.TradChinese Then
                txt.Font = New System.Drawing.Font(Me.FamilyNameChinese, intFontSize, style)
            Else
                txt.Font = New System.Drawing.Font(Me.FamilyNameEnglish, intFontSize, style)
            End If
        End Sub

        Private Sub RenderMultiLingualTextBoxFont(ByRef txt As TextBox, ByVal intFontSize As Integer, ByVal blnBold As Boolean, ByVal blnItalic As Boolean, ByVal blnUnderline As Boolean)
            Dim style As Drawing.FontStyle = GetFontStyle(blnBold, blnItalic, blnUnderline)
            txt.Font = New System.Drawing.Font(Me.FamilyNameMultiLingual, intFontSize, style)
        End Sub

        Private Sub RenderReportInfoFont(ByRef ri As ReportInfo, ByVal intFontSize As Integer, ByVal blnBold As Boolean, ByVal blnItalic As Boolean, ByVal blnUnderline As Boolean)
            Dim style As Drawing.FontStyle = GetFontStyle(blnBold, blnItalic, blnUnderline)

            If _strLanguage = CultureLanguage.TradChinese Then
                ri.Font = New System.Drawing.Font(Me.FamilyNameChinese, intFontSize, style)
            Else
                ri.Font = New System.Drawing.Font(Me.FamilyNameEnglish, intFontSize, style)
            End If
        End Sub

        '---Library Functions

        Public Sub RenderLabel(ByRef lbl As Label, ByVal strResourceKey As String, Optional ByVal blnAddNotDiscloseNotation As Boolean = False, Optional ByVal blnColon As Boolean = False, Optional ByVal intFontSize As Integer = 10, Optional ByVal blnBold As Boolean = True, Optional ByVal blnItalic As Boolean = False, Optional ByVal blnUnderline As Boolean = False)

            RenderLabelText(lbl, RenderText(strResourceKey, blnAddNotDiscloseNotation, blnColon))
            RenderLabelFont(lbl, RenderFont(intFontSize, blnBold, blnItalic, blnUnderline))

        End Sub

        Public Sub RenderLabelText(ByRef lbl As Label, ByVal strText As String)
            lbl.Text = strText
        End Sub

        Public Sub RenderLabelFont(ByRef lbl As Label, ByVal font As Drawing.Font)
            lbl.Font = font
        End Sub

        Public Function RenderText(ByVal strResourceKey As String, Optional ByVal blnAddNotDiscloseNotation As Boolean = False, Optional ByVal blnColon As Boolean = False) As String

            Dim strText As String = GetTextResource(strResourceKey)

            If blnAddNotDiscloseNotation Then
                strText += GetNotDiscloseNotation()
            End If

            If blnColon Then
                strText += GetLabelValueSeparator()
            End If

            Return strText

        End Function

        Public Function RenderFont(Optional ByVal intFontSize As Integer = 10, Optional ByVal blnBold As Boolean = False, Optional ByVal blnItalic As Boolean = False, Optional ByVal blnUnderline As Boolean = False) As Drawing.Font

            Dim strFamilyName As String = String.Empty

            Select Case _strLanguage
                Case CultureLanguage.TradChinese
                    strFamilyName = Me.FamilyNameChinese
                    intFontSize += 1
                Case CultureLanguage.English
                    strFamilyName = Me.FamilyNameEnglish
                Case Else
                    strFamilyName = Me.FamilyNameEnglish
            End Select

            Return New Drawing.Font(strFamilyName, intFontSize, GetFontStyle(blnBold, blnItalic, blnUnderline))

        End Function

        Private Function GetLabelValueSeparator() As String

            Dim strLabelValueSeparator As String = ":"

            Return strLabelValueSeparator

        End Function

        Private Function GetNotDiscloseNotation() As String

            Dim strNotDiscloseNotation As String = String.Format(" {0}", GetTextResource("NotDiscloseNotationTextOnly"))

            Return strNotDiscloseNotation

        End Function

        Private Function GetNotProvidedNotation() As String

            Dim strNotDiscloseNotation As String = GetTextResource("NotProvided")

            Return strNotDiscloseNotation

        End Function

        Private Function GetTextResource(ByVal strResourceKey As String) As String

            Dim strResource As String = String.Empty
            Dim languageChosen As String = _strLanguage
            Dim cultureInfo As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(languageChosen)

            strResource = HttpContext.GetGlobalResourceObject("Text", strResourceKey, cultureInfo)

            Return strResource

        End Function

        Private Function GetFontStyle(ByVal blnBold As Boolean, ByVal blnItalic As Boolean, ByVal blnUnderline As Boolean) As Drawing.FontStyle

            Dim FontStyle As Drawing.FontStyle = IIf(blnBold, Drawing.FontStyle.Bold, Drawing.FontStyle.Regular) _
                                                    Or IIf(blnItalic, Drawing.FontStyle.Italic, Drawing.FontStyle.Regular) _
                                                    Or IIf(blnUnderline, Drawing.FontStyle.Underline, Drawing.FontStyle.Regular)

            Return FontStyle

        End Function

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal strLanguage As String)
            Me.New()
            _strLanguage = strLanguage
        End Sub

#End Region

#Region "Properties, Fields"

        Private _strLanguage As String

#End Region

    End Class

End Namespace
