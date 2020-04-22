Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyPIDVSS
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            'Input Vaccine contorl Fields
            Me.udcClaimVaccineReadOnlyText.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
            Me.udcClaimVaccineReadOnlyText.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
            Me.udcClaimVaccineReadOnlyText.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
            Me.udcClaimVaccineReadOnlyText.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
            Me.udcClaimVaccineReadOnlyText.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
            Me.udcClaimVaccineReadOnlyText.NAText = Me.GetGlobalResourceObject("Text", "N/A")

            'Text Field
            Me.lblDocumentaryProofText.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")

            AddHandler udcClaimVaccineReadOnlyText.RemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked

        End Sub

        Protected Overrides Sub Setup()
            If String.IsNullOrEmpty(MyBase.EHSTransaction.TransactionAdditionFields.DocumentaryProof) = False Then
                Dim udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL
                Dim udtStaticDataModel As Common.Component.StaticData.StaticDataModel

                udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PIDVSS_DOCUMENTARYPROOF", MyBase.EHSTransaction.TransactionAdditionFields.DocumentaryProof)

                If Not udtStaticDataModel Is Nothing Then
                    If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblDocumentaryProof.Text = udtStaticDataModel.DataValueChi

                    ElseIf MyBase.SessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                        Me.lblDocumentaryProof.Text = udtStaticDataModel.DataValueCN
                    Else
                        Me.lblDocumentaryProof.Text = udtStaticDataModel.DataValue
                    End If
                End If

            End If

            Me.udcClaimVaccineReadOnlyText.Build(MyBase.EHSClaimVaccine)

        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

        End Sub
#End Region

#Region "Events"

        Protected Sub udcClaimVaccineReadOnlyText_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
            RaiseEvent VaccineRemarkClicked(sender, e)
        End Sub

#End Region

    End Class
End Namespace


