Imports Common.Component.EHSTransaction

Partial Public Class ucReadOnlyEVSS
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel)
        udcReadOnlyVaccine.Build(udtEHSTransaction)
    End Sub

End Class