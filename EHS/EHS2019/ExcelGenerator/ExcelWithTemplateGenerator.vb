Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component

Imports Common.ComFunction.ParameterFunction
Imports Common.Component.Inbox

Public Class ExcelWithTemplateGenerator
    Inherits BaseGenerator

    Implements IExcelGenerable

    Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)
        MyBase.New(udtQueue, udtFileGenerationModel)

        ' Get Out Put Path
        Dim strPath As String = String.Empty
        Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
        udtCommonGenFunction.getSystemParameter("ExcelWithTemplateDownloadStoragePath", strPath, String.Empty)
        If strPath.Trim() = "" Then
            Throw New ArgumentException("ExcelWithTemplateDownloadStoragePath Empty!")
        End If

        If Not strPath.Trim().EndsWith("\") Then
            strPath = strPath & "\"
        End If
        Me.m_strFileOutPath = strPath
    End Sub

    Public Overrides Function GetDataSet() As System.Data.DataSet
        Dim dsData As New DataSet()
        Dim udtDB As Common.DataAccess.Database = Nothing

        If Not Me.m_udtFileGeneration.GetDataFromBak Is Nothing Then
            Select Case Me.m_udtFileGeneration.GetDataFromBak.Trim().ToUpper()
                Case ReportConnection.Primary
                    udtDB = New Common.DataAccess.Database()
                Case ReportConnection.Secondary
                    udtDB = New Common.DataAccess.Database("DBFlagS")
                Case ReportConnection.Report
                    udtDB = New Common.DataAccess.Database("DBFlagR")
            End Select
        End If

        If IsNothing(udtDB) Then
            udtDB = New Common.DataAccess.Database("DBFlag2")
        End If

        If String.IsNullOrEmpty(Me.m_udtQueue.InParm) Then
            ' No parameter
            udtDB.RunProc(Me.m_udtFileGeneration.FileDataSP, dsData)

        Else
            ' Contains parameters
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udtSPParamCollection As StoreProcParamCollection = udtParamFunction.GetSPParamCollection(Me.m_udtQueue.InParm)

            Dim params(udtSPParamCollection.Count + 1) As SqlParameter
            params(0) = udtDB.MakeInParam("@request_time", SqlDbType.DateTime, 8, Me.m_udtQueue.RequestDtm)

            For i As Integer = 0 To udtSPParamCollection.Count - 1
                Dim udtSPParamObject As StoreProcParamObject = udtSPParamCollection(i)
                params(i + 1) = udtDB.MakeInParam(udtSPParamObject.ParamName, udtSPParamObject.ParamDBType, udtSPParamObject.ParamDBSize, udtSPParamObject.ParamValue)
            Next

            udtDB.RunProc(Me.m_udtFileGeneration.FileDataSP, params, dsData)

        End If

        Return dsData

    End Function

    Public Overrides Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)
        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL()
        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtFormatter As New Common.Format.Formatter

        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

        udtMessageCollection = Nothing
        udtMessageReaderCollection = Nothing


        'Dim udtDB As New Common.DataAccess.Database()
        Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, Me.m_udtQueue.GenerationID)

        ' This report will not send to any users
        If Not IsNothing(dtResult) Then
            If dtResult.Rows.Count > 0 Then

                udtMessageCollection = New MessageModelCollection()
                udtMessageReaderCollection = New MessageReaderModelCollection()

                ' Construct Message & MessageReader
                ' One Message, To Mutilple User (Other Case May Mutil Message, Each Message to Single User
                Dim udtMessage As New MessageModel()
                udtMessage.MessageID = udtGeneral.generateInboxMsgID()

                Dim paramsSubject As New ParameterCollection()
                paramsSubject.AddParam("FileName", Me.GetFileName())

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'paramsSubject.AddParam("Date", DateTime.Now.ToString("yyyyMMMdd", New System.Globalization.CultureInfo("en-US")))
                'paramsSubject.AddParam("Date", udtFormatter.formatDate(Now.AddDays(-1), "en"))
                paramsSubject.AddParam("Date", udtFormatter.formatDisplayDate(Now.AddDays(-1), CultureLanguage.English))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]



                Dim paramsContent As New ParameterCollection()
                paramsContent.AddParam("FileName", Me.GetFileName())
                paramsContent.AddParam("Link", "../ReportAndDownload/Datadownload.aspx")

                udtMessage.Subject = udtParamFunction.GetParsedStringByparameter(Me.m_udtFileGeneration.MessageSubject, paramsSubject)
                udtMessage.Message = udtParamFunction.GetParsedStringByparameter(Me.m_udtFileGeneration.MessageContent, paramsContent)

                udtMessage.CreateBy = "EHCVS"
                udtMessage.CreateDtm = dtmCurrent
                udtMessageCollection.Add(udtMessage)

                For Each drRow As DataRow In dtResult.Rows

                    Dim strUserId As String = drRow("User_ID").ToString().Trim()
                    Dim strUserName As String = drRow("User_Name").ToString().Trim()

                    Dim udtMessageReader As New MessageReaderModel()
                    udtMessageReader.MessageID = udtMessage.MessageID
                    udtMessageReader.MessageReader = strUserId
                    udtMessageReader.UpdateBy = "EHCVS"
                    udtMessageReader.UpdateDtm = dtmCurrent

                    udtMessageReaderCollection.Add(udtMessageReader)
                Next

            End If
        End If

    End Sub

End Class
