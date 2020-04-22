''' <summary>
''' Base Class for Generator, To Handle Retrieve And Process Data Logic
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseGenerator
    Implements IExcelGenerable

    Protected m_udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel
    Protected m_udtFileGeneration As Common.Component.FileGeneration.FileGenerationModel

    Protected m_strFileOutPath As String = ""
    Protected Const cDirectorySeparator As String = "\"

    Protected Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)
        Me.m_udtQueue = udtQueue
        Me.m_udtFileGeneration = udtFileGenerationModel
    End Sub

    Public Overridable ReadOnly Property SaveReportToDB() As Boolean Implements IExcelGenerable.SaveReportToDB
        Get
            Return True
        End Get
    End Property

    ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
    Public Overridable ReadOnly Property TerminateReport() As Boolean Implements IExcelGenerable.TerminateReport
        Get
            Return False
        End Get
    End Property

    Public MustOverride Function GetDataSet() As System.Data.DataSet Implements IExcelGenerable.GetDataSet
    ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]

    Public Function GetEncryptPassword() As String Implements IExcelGenerable.GetEncryptPassword
        Return Me.m_udtQueue.FilePassword
    End Function

    Public Function GetFileFullPath() As String Implements IExcelGenerable.GetFileFullPath
        Return Me.GetFilePath + Me.GetFileName
    End Function

    Public Function GetFileName() As String Implements IExcelGenerable.GetFileName
        Return Me.m_udtQueue.OutputFile.Substring(Me.m_udtQueue.OutputFile.IndexOf(cDirectorySeparator) + 1)
    End Function

    Public Function GetFilePath() As String Implements IExcelGenerable.GetFilePath
        Return Me.m_strFileOutPath + Me.m_udtQueue.OutputFile.Substring(0, Me.m_udtQueue.OutputFile.IndexOf(cDirectorySeparator) + 1)
    End Function

    Public Overridable Function GetTemplate() As String Implements IExcelGenerable.GetTemplate

        Dim strTemplatePath As String = String.Empty

        If Me.m_udtFileGeneration.ReportTemplate Is Nothing OrElse Me.m_udtFileGeneration.ReportTemplate.Trim() = "" Then
            Return String.Empty
        Else
            Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
            udtCommonGenFunction.getSystemParameter("ExcelWithTemplatePath", strTemplatePath, String.Empty)
            Return System.AppDomain.CurrentDomain.BaseDirectory + strTemplatePath + Me.m_udtFileGeneration.ReportTemplate
        End If
    End Function

    Public Function GetXLSParameter() As List(Of Integer) Implements IExcelGenerable.GetXLSParameter
        Return Me.m_udtFileGeneration.XLS_Parameter
    End Function

    Public MustOverride Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection) Implements IExcelGenerable.ConstructMessageParamaterList


    Protected Function GetDataBase() As Common.DataAccess.Database
        Dim udtDB As Common.DataAccess.Database = Nothing

        If Not Me.m_udtFileGeneration Is Nothing AndAlso Not Me.m_udtFileGeneration.GetDataFromBak Is Nothing Then
            Select Case Me.m_udtFileGeneration.GetDataFromBak.Trim().ToUpper()
                Case Common.Component.ReportConnection.Primary
                    udtDB = New Common.DataAccess.Database()
                Case Common.Component.ReportConnection.Secondary
                    udtDB = New Common.DataAccess.Database("DBFlagS")
                Case Common.Component.ReportConnection.Report
                    udtDB = New Common.DataAccess.Database("DBFlagR")
            End Select
        End If

        Return udtDB

    End Function

    Public Overridable Function GetWorksheetAction() As DataTable Implements IExcelGenerable.GetWorksheetAction
        Return Nothing
    End Function

End Class
