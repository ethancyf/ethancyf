Imports System.Configuration


Namespace Generator

    Public MustInherit Class ARReportGeneratorBase
        Inherits BaseGenerator

        Dim _comFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim _strMessageSeparator As String = String.Empty
        Dim _blnContainSearchResult As Boolean = False
        Protected Property ContainSearchResult() As Boolean
            Get
                Return _blnContainSearchResult
            End Get
            Set(ByVal value As Boolean)
                _blnContainSearchResult = value
            End Set
        End Property


        Protected Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)
            MyBase.New(udtQueue, udtFileGenerationModel)

            Dim parm2 As String = String.Empty
            _comFunction.getSytemParameterByParameterName("Report_MessageSeparator", _strMessageSeparator, parm2)
        End Sub

        Public Overrides ReadOnly Property SaveReportToDB() As Boolean
            Get
                Return _blnContainSearchResult
            End Get
        End Property

        Protected Function GetMessageBySearchResult(ByVal strMessageContent As String) As String
            Dim strResult As String = String.Empty
            Dim strMessageContents() As String = strMessageContent.Split(_strMessageSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            If strMessageContents.Length > 1 And Not ContainSearchResult Then
                strResult = strMessageContents(1)
            Else
                strResult = strMessageContents(0)
            End If

            Return strResult
        End Function

    End Class

End Namespace
