Imports System.Data.SqlClient
Imports Common.Component
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.Inbox

Public Class BNCGenerator
    Inherits BaseGenerator

    Implements IExcelGenerable

    Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)

        MyBase.New(udtQueue, udtFileGenerationModel)

        ' Get Out Put Path
        Dim strPath As String = String.Empty
        Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
        udtCommonGenFunction.getSystemParameter("BNCFileStoragePath", strPath, String.Empty)
        If strPath.Trim() = "" Then
            Throw New ArgumentException("BNCFileStoragePath Empty!")
        End If

        If Not strPath.Trim().EndsWith("\") Then
            strPath = strPath & "\"
        End If
        Me.m_strFileOutPath = strPath

    End Sub

    Public Overrides Function GetDataSet() As System.Data.DataSet
        Dim dsData As New DataSet()

        Dim udtDB As New Common.DataAccess.Database()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtSPParamCollection As StoreProcParamCollection = udtParamFunction.GetSPParamCollection(Me.m_udtQueue.InParm)

        Dim params(udtSPParamCollection.Count) As SqlParameter

        For i As Integer = 0 To udtSPParamCollection.Count - 1
            Dim udtSPParamObject As StoreProcParamObject = udtSPParamCollection(i)
            params(i) = udtDB.MakeInParam(udtSPParamObject.ParamName, udtSPParamObject.ParamDBType, udtSPParamObject.ParamDBSize, udtSPParamObject.ParamValue)
        Next

        udtDB.RunProc(Me.m_udtFileGeneration.FileDataSP, params, dsData)

        Return dsData
    End Function

    Public Overrides Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As MessageModelCollection, ByRef udtMessageReaderCollection As MessageReaderModelCollection)
        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL()
        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtFormatter As New Common.Format.Formatter

        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        'Dim udtDB As New Common.DataAccess.Database()
        Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, Me.m_udtQueue.GenerationID)

        ' Construct Message & MessageReader
        ' One Message, To Mutilple User (Other Case May Mutil Message, Each Message to Single User
        Dim udtMessage As New MessageModel()
        udtMessage.MessageID = udtGeneral.generateInboxMsgID()

        Dim paramsSubject As New ParameterCollection()
        paramsSubject.AddParam("FileName", Me.GetFileName())

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'paramsSubject.AddParam("Date", Now.ToString("yyyyMMMdd"))
        'paramsSubject.AddParam("Date", udtFormatter.formatDate(Now, "en"))
        paramsSubject.AddParam("Date", udtFormatter.formatDisplayDate(Now, CultureLanguage.English))
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

    End Sub

End Class
