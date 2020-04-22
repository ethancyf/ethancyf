Imports Common.ComInterface

Namespace ComObject

    Public MustInherit Class MasterPage
        Inherits System.Web.UI.Page
        Implements ComInterface.IWorkingData

        ''' <summary>
        ''' Clear all the interface(iWorking) value if necessary
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub ClearWorkingData() Implements ComInterface.IWorkingData.ClearWorkingData
            ' Override this function for each driven itself if necessary
        End Sub

        ''' <summary>
        ''' Global working data - Service Provider
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetEHSAccount() As Component.EHSAccount.EHSAccountModel Implements ComInterface.IWorkingData.GetEHSAccount

        ''' <summary>
        ''' Global working data - EHS Account
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetEHSTransaction() As Component.EHSTransaction.EHSTransactionModel Implements ComInterface.IWorkingData.GetEHSTransaction

        ''' <summary>
        ''' Global working data - EHS Transaction
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetServiceProvider() As Component.ServiceProvider.ServiceProviderModel Implements ComInterface.IWorkingData.GetServiceProvider

        ''' <summary>
        ''' Global working data - Document code
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetDocCode() As String Implements ComInterface.IWorkingData.GetDocCode

    End Class

End Namespace
