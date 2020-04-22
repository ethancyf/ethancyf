Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount
Imports Common.Component.eHealthAccountDeathRecord
Imports Common.Component.HCVUUser
Imports Common.Component.RedirectParameter
Imports Common.Component.StaticData
Imports Common.Format
Imports CustomControls
Imports HCVU
Imports HCVU.Component.FunctionInformation
Imports HCVU.Component.Menu
Imports System.Data.SqlClient

Partial Public Class eHealthAccountDeathRecordMaintenance
    Inherits BasePage

    ' FunctionCode = FunctCode.FUNT010307

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        FunctionCode = FunctCode.FUNT010307
    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.ucMain.Mode = eHealthAccountDeathRecordMaint.enumMode.Maintenance
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Implement IWorkingData"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region


End Class