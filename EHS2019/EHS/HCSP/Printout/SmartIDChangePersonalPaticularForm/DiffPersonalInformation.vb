Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.EHSAccount

Namespace PrintOut.VoucherAccountChangeForm

    Public Class DiffPersonalInformation

        Private _diffPersonalInformation As EHSAccountModel.EHSPersonalInformationModel
        Private udtFormatter As Formatter
        Private udtReportFunction As ReportFunction
        Private udtGeneralFunction As GeneralFunction
        Private _strDiffValueNumbers As String() = {"(a)", "(b)", "(c)", "(d)", "(e)"}
        Private _strTextStyle As String = "ddo-char-set: 1; text-align: left; font-size: 11.25pt; "
        Private _strTextUnderLineStyle As String = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
        Private _strTextUnderLineCHIStyle As String = "text-decoration: underline; ddo-char-set: 1; text-align: left; font-size: 11.25pt; font-family: HA_MingLiu; "


        Public Sub New(ByVal diffEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.udtReportFunction = New ReportFunction()
            Me.udtFormatter = New Formatter()
            Me.udtGeneralFunction = New GeneralFunction()
            Me._diffPersonalInformation = diffEHSPersonalInfo

        End Sub

        Private Sub DiffPersonalInformation_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Dim startTop As Single = 0.0!
            Dim startLeft As Single = 0.0!
            Dim sngSpaceHeight As Single = 0.031!
            Dim numberOfDiffValue As Integer = 1
            Dim textBoxValue As String
            'Fill English name
            If (Not String.IsNullOrEmpty(Me._diffPersonalInformation.ENameFirstName)) OrElse _
                (Not String.IsNullOrEmpty(Me._diffPersonalInformation.ENameSurName)) Then

                textBoxValue = Me.udtFormatter.formatEnglishName(Me._diffPersonalInformation.ENameSurName, Me._diffPersonalInformation.ENameFirstName)

                'Me.udtReportFunction.formatUnderLineTextBox(strVRAEnglishName, Me.txtDiffVRAEname)
                'Me.CreateDiffValueTextBoxSet(numberOfDiffValue, "Name in English :", strVRAEnglishName, "text-align: left; font-size: 11.25pt; ")
                Me.CreateTextBox(startTop, startLeft, 0.313!, 0.219!, Me._strDiffValueNumbers.GetValue(numberOfDiffValue - 1), Me._strTextStyle)
                startLeft += 0.313!
                Me.CreateTextBox(startTop, startLeft, 1.344!, 0.219!, "Name in English :", Me._strTextStyle)
                startLeft += 1.344!
                Me.CreateTextBox(startTop, startLeft, 4.844!, 0.219!, textBoxValue, Me._strTextUnderLineStyle)
                startLeft = 0.0!
                startTop += 0.219! + sngSpaceHeight
                numberOfDiffValue += 1
            End If

            'Fill Chinese name
            If Not String.IsNullOrEmpty(Me._diffPersonalInformation.CName) Then

                Me.CreateTextBox(startTop, startLeft, 0.313!, 0.219!, Me._strDiffValueNumbers.GetValue(numberOfDiffValue - 1), Me._strTextStyle)
                startLeft += 0.313!
                Me.CreateTextBox(startTop, startLeft, 1.344!, 0.219!, "Name in Chinese :", Me._strTextStyle)
                startLeft += 1.344!
                Me.CreateTextBox(startTop, startLeft, 4.844!, 0.219!, Me._diffPersonalInformation.CName, Me._strTextUnderLineCHIStyle)
                startLeft = 0.0!
                startTop += 0.219! + sngSpaceHeight
                numberOfDiffValue += 1
            End If

            'Fill Gender
            If Not String.IsNullOrEmpty(Me._diffPersonalInformation.Gender) Then
                Me.CreateTextBox(startTop, startLeft, 0.313!, 0.219!, Me._strDiffValueNumbers.GetValue(numberOfDiffValue - 1), Me._strTextStyle)
                startLeft += 0.313!
                Me.CreateTextBox(startTop, startLeft, 1.344!, 0.219!, "Gender :", Me._strTextStyle)
                startLeft += 1.344!


                If Me._diffPersonalInformation.Gender = "M" Then
                    Me.CreateTextBox(startTop, startLeft, 4.844!, 0.219!, "Male", Me._strTextUnderLineStyle)
                ElseIf Me._diffPersonalInformation.Gender = "F" Then
                    Me.CreateTextBox(startTop, startLeft, 4.844!, 0.219!, "Female ", Me._strTextUnderLineStyle)
                End If

                startLeft = 0.0!
                startTop += 0.219! + sngSpaceHeight
                numberOfDiffValue += 1
            End If


            'Fill DOB
            If Not Me._diffPersonalInformation.DOB.Equals(New Date()) Then

                textBoxValue = Me.udtFormatter.formatDOB(Me._diffPersonalInformation.DOB, Me._diffPersonalInformation.ExactDOB, CultureLanguage.English, Nothing, Nothing)
                Me.CreateTextBox(startTop, startLeft, 0.313!, 0.219!, Me._strDiffValueNumbers.GetValue(numberOfDiffValue - 1), Me._strTextStyle)
                startLeft += 0.313!
                Me.CreateTextBox(startTop, startLeft, 1.344!, 0.219!, "Date of Birth :", Me._strTextStyle)
                startLeft += 1.344!
                Me.CreateTextBox(startTop, startLeft, 4.844!, 0.219!, textBoxValue, Me._strTextUnderLineStyle)
                startLeft = 0.0!
                startTop += 0.219! + sngSpaceHeight
                numberOfDiffValue += 1
            End If

            'Fill DOI
            If Not Me._diffPersonalInformation.DateofIssue.Equals(Nothing) AndAlso Not Me._diffPersonalInformation.DateofIssue.Equals(New Date()) Then

                textBoxValue = Me.udtFormatter.formatHKIDIssueDate(Me._diffPersonalInformation.DateofIssue)
                Me.CreateTextBox(startTop, startLeft, 0.313!, 0.219!, Me._strDiffValueNumbers.GetValue(numberOfDiffValue - 1), Me._strTextStyle)
                startLeft += 0.313!
                Me.CreateTextBox(startTop, startLeft, 2.969!, 0.219!, "Date of Issue of Hong Kong Identity Card :", Me._strTextStyle)
                startLeft += 2.969!
                Me.CreateTextBox(startTop, startLeft, 3.281!, 0.219!, textBoxValue, Me._strTextUnderLineStyle)
                startLeft = 0.0!
                startTop += 0.219! + sngSpaceHeight
                numberOfDiffValue += 1
            End If
            Me.DiffValueBatch.Height = startTop
        End Sub

        Public Sub CreateTextBox(ByVal top As Single, ByVal left As Single, ByVal width As Single, ByVal height As Single, ByVal strText As String, ByVal textBoxStyle As String)
            Dim textBox As TextBox = New TextBox

            'A, B, C, D, E
            Me.DiffValueBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})

            textBox.Width = width
            textBox.Height = height

            textBox.Top = top
            textBox.Left = left
            textBox.Text = strText
            textBox.Style = textBoxStyle


        End Sub
    End Class

End Namespace