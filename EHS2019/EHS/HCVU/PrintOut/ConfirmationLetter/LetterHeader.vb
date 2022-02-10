Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Namespace PrintOut.ConfirmationLetter

    Public Class LetterHeader

        Private _strTelNo As String
        Private _strFaxNo As String
        Private _strAddress As String
        Private _strSPName As String
        Private _blnIsChineseSPName As Boolean
        Private _strCultureLanguage As String

        Public Sub New(ByVal strTelNo As String, ByVal strFaxNo As String, ByVal strAddress As String, ByVal strSPName As String, ByVal blnIsChineseSPName As Boolean)
            Me.InitializeComponent()

            Me._strTelNo = strTelNo
            Me._strFaxNo = strFaxNo
            Me._strAddress = strAddress
            Me._strSPName = strSPName
            Me._blnIsChineseSPName = blnIsChineseSPName
        End Sub


        Public Sub SetCultureLanguage(ByVal strCultureLanguage As String)
            Dim objFont As System.Drawing.Font

            ' CRE19-008 (Rename VO) [Start][Koala
            Me._strCultureLanguage = strCultureLanguage
            ' CRE19-008 (Rename VO) [End][Koala]

            Select Case strCultureLanguage
                Case Common.Component.CultureLanguage.English
                    objFont = New System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular)
                    ' CRE19-008 (Rename VO) [Start][Koala]
                    Me.txtTelNoEng.Location = New System.Drawing.PointF(1.82F, 0.5F)
                    Me.txtFaxNoEng.Location = New System.Drawing.PointF(1.82F, 0.75F)
                    Me.txtChiFaxNoEngText.Location = New System.Drawing.PointF(0.0F, 0.75F)
                    Me.txtEngFaxNoEngText.Location = New System.Drawing.PointF(0.9F, 0.75F)
                    ' CRE19-008 (Rename VO) [End][Koala]

                Case Common.Component.CultureLanguage.TradChinese
                    objFont = New System.Drawing.Font("MingLiU_HKSCS-ExtB", 11.25F, System.Drawing.FontStyle.Regular)
                    'Me.txtTelNoEng.Location = New System.Drawing.PointF(1.82F, 0.5F)
                    'Me.txtFaxNoEng.Location = New System.Drawing.PointF(1.82F, 0.72F)

                Case Else
                    objFont = New System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular)

            End Select

            Me.txtTelNoEng.Font = objFont
            Me.txtFaxNoEng.Font = objFont

           
        End Sub


        Private Sub FillData()
            Me.txtTelNoEng.Text = Me._strTelNo
            Me.txtFaxNoEng.Text = Me._strFaxNo

            Me.txtRecipientSPNameEng.Text = Me._strSPName

            If Me._blnIsChineseSPName Then
                Me.txtRecipientSPNameEng.Font = New System.Drawing.Font("MingLiU_HKSCS-ExtB", 11.25F, Drawing.FontStyle.Regular)
            Else
                Me.txtRecipientSPNameEng.Font = New System.Drawing.Font("Arial", 10, Drawing.FontStyle.Regular)
            End If

            ' CRE19-008 (Rename VO) [Start][Koala]
            Select Case Me._strCultureLanguage
                Case Common.Component.CultureLanguage.English

                    Me.txtTelNoEng.Text = Me.txtTelNoEng.Text.Replace("/", Chr(10) + "/")
                    Me.txtFaxNoEng.Text = Me.txtFaxNoEng.Text.Replace("/", Chr(10) + "/")

            End Select
            ' CRE19-008 (Rename VO) [End][Koala]

            Me.txtRecipientAddressEng.Text = Me._strAddress
        End Sub

        Private Sub LetterFooter_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.FillData()
        End Sub
    End Class

End Namespace