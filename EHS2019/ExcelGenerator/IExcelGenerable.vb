''' <summary>
''' Interface for Generator
''' </summary>
''' <remarks></remarks>
Public Interface IExcelGenerable

    ReadOnly Property SaveReportToDB() As Boolean
    ReadOnly Property TerminateReport() As Boolean

    Function GetDataSet() As DataSet
    Function GetEncryptPassword() As String
    Function GetFilePath() As String
    Function GetFileName() As String
    Function GetFileFullPath() As String
    Function GetTemplate() As String
    Function GetXLSParameter() As List(Of Integer)
    Function GetWorksheetAction() As DataTable

    Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)
End Interface
