Imports System.Configuration


Namespace Generator

    Public MustInherit Class DeactivatedEHRSSTokenReportGeneratorBase
        Inherits BaseGenerator

        Dim _comFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim _strMessageSeparator As String = String.Empty
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
                Return _blnContainSearchResult
            End Get
        End Property

        Public Overrides ReadOnly Property TerminateReport() As Boolean
            Get
                Return Not _blnContainSearchResult
            End Get
        End Property

        Protected Function GetMessageBySearchResult(ByVal strMessageContent As String) As String
            Dim strResult As String = String.Empty
            Dim strMessageContents() As String = strMessageContent.Split(New String() {_strMessageSeparator}, StringSplitOptions.None)
            ' Normal
            If Not strMessageContents Is Nothing AndAlso strMessageContents.Length = 1 Then
                strResult = strMessageContents(0)
            End If

            Return strResult
        End Function

    End Class

End Namespace
