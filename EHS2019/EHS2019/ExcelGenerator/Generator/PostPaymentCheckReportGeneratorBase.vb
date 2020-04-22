Imports System.Configuration


Namespace Generator

    Public MustInherit Class PostPaymentCheckReportGeneratorBase
        Inherits BaseGenerator

        Dim _comFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim _strMessageSeparator As String = String.Empty
        Dim _blnSearchResultExceedLimit As Boolean = False
        Dim _blnContainSearchResult As Boolean = True
        Dim _blnReturnParm As Boolean = False

        Protected Property ContainSearchResult() As Boolean
            Get
                Return _blnContainSearchResult
            End Get
            Set(ByVal value As Boolean)
                _blnContainSearchResult = value
            End Set
        End Property

        Protected Property SearchResultExceedLimit() As Boolean
            Get
                Return _blnSearchResultExceedLimit
            End Get
            Set(ByVal value As Boolean)
                _blnSearchResultExceedLimit = value
            End Set
        End Property

        Public Property ReturnParm() As Boolean
            Get
                Return _blnReturnParm
            End Get
            Set(ByVal value As Boolean)
                _blnReturnParm = value
            End Set
        End Property

        Protected Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)
            MyBase.New(udtQueue, udtFileGenerationModel)

            Dim parm2 As String = String.Empty
            _comFunction.getSytemParameterByParameterName("Report_MessageSeparator", _strMessageSeparator, parm2)
        End Sub

        Public Overrides ReadOnly Property SaveReportToDB() As Boolean
            Get
                Return _blnContainSearchResult And Not _blnSearchResultExceedLimit
            End Get
        End Property

        Public Overrides ReadOnly Property TerminateReport() As Boolean
            Get
                Return _blnSearchResultExceedLimit
            End Get
        End Property

        Protected Function GetMessageBySearchResult(ByVal strMessageContent As String) As String
            Dim strResult As String = String.Empty
            Dim strMessageContents() As String = strMessageContent.Split(New String() {_strMessageSeparator}, StringSplitOptions.None)
            ' Normal|||NoResult|||ExceedLimit
            If SearchResultExceedLimit AndAlso strMessageContents.Length > 2 AndAlso Not String.IsNullOrEmpty(strMessageContents(2)) Then
                strResult = strMessageContents(2)
            ElseIf Not ContainSearchResult AndAlso strMessageContents.Length > 1 AndAlso Not String.IsNullOrEmpty(strMessageContents(1)) Then
                strResult = strMessageContents(1)
            Else
                strResult = strMessageContents(0)
            End If

            Return strResult
        End Function

    End Class

End Namespace
