Public Interface ITextFileGenerable

    'Function GetDataSet() As DataSet
    Function GetData() As String()
    Function GetEncryptPassword() As String
    Function GetFilePath() As String
    Function GetFileNameNoExtension() As String
    Function GetFileFullName() As String
    Function GetFileFullPath() As String

    Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)

End Interface
