Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.Common
    Public Class PersonalInfo

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Fill in Name
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtNameEng)
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtNameChi)

            ' Fill in DOB
            Formatter.FormatUnderLineTextBox(udtCFInfo.DOB, txtDOB)

            ' Fill Gender
            txtGender.Visible = True
            txtGender1.Visible = False
            txtGender2.Visible = False

            Select Case udtCFInfo.Gender
                Case "M"
                    Formatter.FormatUnderLineTextBox("Male", txtGender)
                Case "F"
                    Formatter.FormatUnderLineTextBox("Female", txtGender)
                Case Else
                    txtGender.Visible = False
                    txtGender1.Visible = True
                    txtGender2.Visible = True

            End Select

        End Sub

    End Class

End Namespace
