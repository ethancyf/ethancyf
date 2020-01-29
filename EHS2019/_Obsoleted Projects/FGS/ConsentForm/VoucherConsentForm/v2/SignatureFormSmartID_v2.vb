Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class SignatureFormSmartID_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            If udtCFInfo.SPName <> String.Empty Then
                Me.SubReport1.Report = New ClaimConsent1SPName40_v2(udtCFInfo)
                Me.SubReport2.Report = New ClaimConsent2SPName40_v2(udtCFInfo)

                If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                    Me.SubReport3.Report = New ClaimConsent3SPName40_v2(udtCFInfo)
                ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                    Me.SubReport3.Report = New ClaimConsent3SPName40UnknownSmartID_v2(udtCFInfo)
                End If

            Else
                Me.SubReport1.Report = New ClaimConsent1SPNameNA_v2(udtCFInfo)
                Me.SubReport2.Report = New ClaimConsent2SPNameNA_v2()

                If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                    Me.SubReport3.Report = New ClaimConsent3SPName40_v2(udtCFInfo)
                ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                    Me.SubReport3.Report = New ClaimConsent3SPName40UnknownSmartID_v2(udtCFInfo)
                End If

            End If

            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)
                Me.txtRecipientHKIDText.Text = "Serial No. of the Certificate of Exemption:"
            Else
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)
                Me.txtRecipientHKIDText.Text = "Hong Kong Identity Card No.:"
            End If

            'Recipient
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientName)

            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, Me.txtRecipientDate)

        End Sub

        Private Sub SignatureForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace
