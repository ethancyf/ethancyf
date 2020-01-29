Namespace ComInterface

    ''' <summary>
    ''' CRE11-004
    ''' Interface for retrieve working data which related to user processing task
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IWorkingData
        ''' <summary>
        ''' Retrieve Service Provider which user working on
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel

        ''' <summary>
        ''' Retrieve EHS Account which user working on
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel

        ''' <summary>
        ''' Retrieve EHS Transaction which user working on
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel

        ''' <summary>
        ''' Retrieve Document Code which user working on
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetDocCode() As String

        ''' <summary>
        ''' Clear all working data
        ''' </summary>
        ''' <remarks></remarks>
        Sub ClearWorkingData()
    End Interface

End Namespace
