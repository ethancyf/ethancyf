Imports Common.Component.EHSTransaction

Partial Public Class ucReadOnlyPIDVSS
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)

        If String.IsNullOrEmpty(udtEHSTransaction.TransactionAdditionFields.DocumentaryProof) = False Then
            Dim udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL
            Dim udtStaticDataModel As Common.Component.StaticData.StaticDataModel

            udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PIDVSS_DOCUMENTARYPROOF", udtEHSTransaction.TransactionAdditionFields.DocumentaryProof)

            If Not udtStaticDataModel Is Nothing Then
                Me.lblDocumentaryProof.Text = udtStaticDataModel.DataValue
            End If

        End If

        udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Control the width of the first column
        tblPIDVSS.Rows(0).Cells(0).Width = intWidth
    End Sub

End Class