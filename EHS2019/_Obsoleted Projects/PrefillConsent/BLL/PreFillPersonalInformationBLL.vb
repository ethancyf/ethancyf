Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.DocType
Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser

Namespace BLL
    Public Class PreFillPersonalInformationBLL

        Private _udtEHSAccountBLL As New EHSAccountBLL()

        Public Sub New()
        End Sub

        Public Sub InsertPreFillConsent(ByVal strPreFillConsentID As String, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel)
            Me._udtEHSAccountBLL.InsertPreFillConsent(strPreFillConsentID, udtEHSPersonalInformation)

        End Sub

    End Class

End Namespace


