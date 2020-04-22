Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace ComFunction

    Public Class ReportFunction
        Public Class SessionName
            'Public Const strReferenceNumber = "strReferenceNumber"
            'Public Const strRegistrationCode = "strRegistrationCode"
            'Public Const strSchemeCode = "strSchemeCode"
            Public Const strApplicatName = "strApplicatName"
            Public Const strERNReferenceNo = "strERNReferenceNo"
            Public Const udtServiceProvider = "udtServiceProvider"
            Public Const udtPractice = "udtPractice"
            Public Const udtMedicalOrganization = "udtMedicalOrganization"
            Public Const udtMedicalOrganizations = "udtMedicalOrganizations"

        End Class


        Public Sub formatUnderLineTextBox(ByVal strText As String, ByVal textBox As GrapeCity.ActiveReports.SectionReportModel.TextBox, ByVal sigNoTextWidth As Single)

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
        Public Sub formatUnderLineTextBox(ByVal strText As String, ByVal textBox As GrapeCity.ActiveReports.SectionReportModel.TextBox)

            Me.formatUnderLineTextBox(strText, textBox, textBox.Width)
        End Sub


        Public Sub FillSPName(ByRef strSPName As String, ByRef txtTextBox1 As TextBox, Optional ByRef txtTextBox2 As TextBox = Nothing, Optional ByVal intTextBox1Width As Integer = 0)
            If txtTextBox2 Is Nothing Then
                ' put the SP name into textbox1
                txtTextBox1.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                txtTextBox1.Text = strSPName
            Else
                ' split SP name into 2 textbox
                Dim strSPNames As String() = strSPName.Split(" ")
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
                    txtTextBox2.Text = strSPName
                End If

                If String.IsNullOrEmpty(txtTextBox2.Text) Then
                    txtTextBox2.Text = "¡@" ' put a space to the label. show the rendered textbox will be in the same line as other control
                    txtTextBox1.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                End If
                If String.IsNullOrEmpty(txtTextBox1.Text) Then
                    txtTextBox1.Text = "¡@" ' put a space to the label. show the rendered textbox will be in the same line as other control
                    txtTextBox2.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                End If

            End If

        End Sub

    End Class
End Namespace

