''' <summary>
''' Base Class for Generator, To Handle Retrieve And Process Data Logic
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseGenerator
    Implements ITextFileGenerable

    Protected m_udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel
    Protected m_udtFileGeneration As Common.Component.FileGeneration.FileGenerationModel

    Protected m_strFileOutPath As String = ""
    Protected Const cDirectorySeparator As String = "\"
    Protected Const cFileExtensionSeparator As String = "."

    Protected Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)
        Me.m_udtQueue = udtQueue
        Me.m_udtFileGeneration = udtFileGenerationModel
    End Sub

    Public MustOverride Function GetData() As String() Implements ITextFileGenerable.GetData

    Public Function GetEncryptPassword() As String Implements ITextFileGenerable.GetEncryptPassword
        Return Me.m_udtQueue.FilePassword
    End Function

    Public Function GetFileFullPath() As String Implements ITextFileGenerable.GetFileFullPath
        Return Me.GetFilePath + Me.GetFileFullName
    End Function

    Public Function GetFileNameNoExtension() As String Implements ITextFileGenerable.GetFileNameNoExtension
        Dim strFullName As String = Me.GetFileFullName()
        Return strFullName.Substring(0, strFullName.IndexOf(cFileExtensionSeparator))
    End Function

    Public Function GetFileFullName() As String Implements ITextFileGenerable.GetFileFullName
        Return Me.m_udtQueue.OutputFile.Substring(Me.m_udtQueue.OutputFile.IndexOf(cDirectorySeparator) + 1)
    End Function

    Public Function GetFilePath() As String Implements ITextFileGenerable.GetFilePath
        Return Me.m_strFileOutPath + Me.m_udtQueue.OutputFile.Substring(0, Me.m_udtQueue.OutputFile.IndexOf(cDirectorySeparator) + 1)
    End Function

    Public MustOverride Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection) Implements ITextFileGenerable.ConstructMessageParamaterList

End Class
